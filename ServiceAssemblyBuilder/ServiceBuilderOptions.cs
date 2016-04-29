using System.Reflection;

namespace ServiceAssemblyBuilder
{
    /// <summary>
    /// Defines basic rules for building the proxy assembly.
    /// </summary>
    public class ServiceBuilderOptions
    {
        #region Properties

        /// <summary>
        /// Gets the name of the generated assembly.
        /// </summary>
        public string AssemblyName { get; set; }
        /// <summary>
        /// Gets the namespace where the service class and interface will be declared.
        /// </summary>
        public string Namespace { get; set; }
        /// <summary>
        /// Gets the name of the generated service class.
        /// </summary>
        public string ClassName { get; set; }
        /// <summary>
        /// Gets the name of the generated service interface.
        /// </summary>
        public string InterfaceName { get; set; }
        /// <summary>
        /// Gets whether unserializable parameter and return types of the wrapped methods
        /// should be exposed as string types.
        /// </summary>
        public bool EmitSymbolInfo { get; set; }
        /// <summary>
        /// Gets whether unserializable parameter and return types of the wrapped methods
        /// should be exposed as string types.
        /// </summary>
        public bool FallbackTypesToString { get; set; }
        /// <summary>
        /// Gets the flags that control which of the requested methods will be wrapped as web methods.
        /// </summary>
        public BindingFlags MethodBindingFlags { get; set; }

        #endregion Properties

        #region ctor bs

        public ServiceBuilderOptions()
            : this("BaseService")
        {
        }

        public ServiceBuilderOptions(
            string namespaceName)
            : this(namespaceName, "Service")
        {
        }

        public ServiceBuilderOptions(
            string namespaceName,
            string className)
            : this(namespaceName, className, string.Concat("I", className), namespaceName)
        {
        }

        public ServiceBuilderOptions(
            string namespaceName,
            string className,
            string interfaceName)
            : this(namespaceName, className, interfaceName, namespaceName)
        {
        }

        public ServiceBuilderOptions(
            string namespaceName,
            string className,
            string interfaceName,
            string assemblyName
            )
        {
            EmitSymbolInfo = true;
            FallbackTypesToString = true;
            MethodBindingFlags = Constants.SupportedMethodBindingFlags;

            Namespace = namespaceName;
            ClassName = className;
            InterfaceName = interfaceName;

            AssemblyName = assemblyName;
        }

        #endregion ctor bs
    }
}
