using ServiceAssemblyBuilder.Extensions;
using System;
using System.Reflection;
using System.Reflection.Emit;

namespace ServiceAssemblyBuilder
{
    /// <summary>
    /// Builds a WCF Service Library that wraps a set of methods and exposes them as web methods.
    /// </summary>
    public class ServiceBuilder
    {
        #region Properties

        /// <summary>
        /// Gets the options that control the building of the proxy assembly.
        /// </summary>
        public ServiceBuilderOptions Options { get; }
        /// <summary>
        /// Gets the built service class type.
        /// </summary>
        public Type ServiceClassType { get; private set; }
        /// <summary>
        /// Gets the built service interface type.
        /// </summary>
        public Type ServiceInterfaceType { get; private set; }
        
        #endregion Properties

        #region Constructors

        public ServiceBuilder()
        {
            Options = new ServiceBuilderOptions();
        }

        public ServiceBuilder(ServiceBuilderOptions options)
        {
            Options = options;
        }

        /// <summary>
        /// Creates a WCF Service Class exposing the methods of the specified type.
        /// </summary>
        /// <param name="type">The type that defines the methods to wrap and expose as web methods.</param>
        /// <returns>A Service Type.</returns>
        public static Type CreateFromType(Type type)
        {
            var serviceBuilder = new ServiceBuilder(new ServiceBuilderOptions());
            var assemblyBuilder = serviceBuilder.DefineServiceType(type);
            string filename = string.Concat(serviceBuilder.Options.AssemblyName, ".dll");
            assemblyBuilder.Save(filename);
            return serviceBuilder.ServiceClassType;
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Builds a proxy assembly that exposes the methods of the specified type as web methods.
        /// </summary>
        /// <param name="type">The type that defines the methods to wrap and expose as web methods.</param>
        /// <returns>An assembly builder instance that contains the generated service type.</returns>
        public AssemblyBuilder DefineServiceType(Type type)
        {
            if (!type.IsClass)
            {
                throw new ArgumentException("The argument must be a class.", "type");
            }

            return DefineServiceType(type.GetMethods());
        }

        /// <summary>
        /// Builds a proxy assembly that exposes each of the provided methods as web methods.
        /// </summary>
        /// <param name="methods">A collection of methods to wrap and expose as web methods.</param>
        /// <returns>An assembly builder instance that contains the generated service type.</returns>
        public AssemblyBuilder DefineServiceType(MethodInfo[] methods)
        {
            // Filter only compatible methods
            methods = methods.GetServiceCompatibleMethods();

            // Assembly declaration
            AssemblyName assemblyName = new AssemblyName(Options.AssemblyName);
            AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(
                assemblyName,
                AssemblyBuilderAccess.RunAndSave);

            // Module declaration
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(
                Options.AssemblyName,
                Options.EmitSymbolInfo);

            // Service Interface declaration
            ServiceInterfaceType = moduleBuilder
                .DefineServiceInterface(Options.Namespace, Options.InterfaceName)
                .AddInterfaceMethods(methods, Options.FallbackTypesToString)
                .CreateType();

            // Service Class declaration            
            ServiceClassType = moduleBuilder
                .DefineServiceClass(Options.Namespace, Options.ClassName)
                .ImplementServiceInterface(ServiceInterfaceType)
                .ImplementMethodProxies(methods, Options.FallbackTypesToString)
                .CreateType();

            // Setup is complete. Return the assembly builder.
            return assemblyBuilder;
        }

        #endregion Public Methods
    }
}
