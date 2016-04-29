using System.Reflection.Emit;

namespace ServiceAssemblyBuilder.Extensions
{
    public static class EModuleBuilder
    {
        public static TypeBuilder DefineServiceInterface(
            this ModuleBuilder moduleBuilder,
            string namespaceName,
            string interfaceName)
        {
            string interfaceFullName = string.Join(".", namespaceName, interfaceName);

            TypeBuilder interfaceTypeBuilder = moduleBuilder
                .DefineType(
                    interfaceFullName,
                    Constants.InterfaceAttributes)
                .AddServiceContractAttribute();
            
            return interfaceTypeBuilder;
        }

        public static TypeBuilder DefineServiceClass(
            this ModuleBuilder moduleBuilder,
            string namespaceName,
            string className)
        {
            string classFullName = string.Join(".", namespaceName, className);

            TypeBuilder classTypeBuilder = moduleBuilder
                .DefineType(
                    classFullName,
                    Constants.ClassAttributes,
                    typeof(object))
                .DefineDefaultConstructor();
            
            return classTypeBuilder;
        }
    }
}
