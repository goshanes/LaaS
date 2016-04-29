using System;
using System.Reflection;
using System.Reflection.Emit;
using System.ServiceModel;

namespace ServiceAssemblyBuilder.Extensions
{
    public static class ETypeBuilder
    {
        public static TypeBuilder AddServiceContractAttribute(
            this TypeBuilder typeBuilder)
        {
            CustomAttributeBuilder serviceContractAttributeBuilder = new CustomAttributeBuilder(
                typeof(ServiceContractAttribute).GetConstructor(Type.EmptyTypes),
                new object[] { });

            typeBuilder.SetCustomAttribute(serviceContractAttributeBuilder);

            return typeBuilder;
        }

        public static TypeBuilder DefineDefaultConstructor(
            this TypeBuilder classTypeBuilder)
        {
            ConstructorBuilder classConstructorBuilder = classTypeBuilder
                .DefineDefaultConstructor(Constants.ConstructorAttributes);

            return classTypeBuilder;
        }

        public static TypeBuilder ImplementServiceInterface(
            this TypeBuilder classTypeBuilder,
            Type interfaceType)
        {
            classTypeBuilder.AddInterfaceImplementation(interfaceType);

            return classTypeBuilder;
        }

        public static TypeBuilder ImplementMethodProxies(
            this TypeBuilder classTypeBuilder,
            MethodInfo[] methods,
            bool fallbackToString)
        {
            foreach (var method in methods)
            {
                classTypeBuilder
                    .DefineClassMethod(
                        method.Name,
                        method.ReturnType.GetServiceCompatibleFallback(fallbackToString),
                        method.GetParameterTypes(fallbackToString)
                        )
                    //.CopySignatureOf(method)
                    //.ImplementInvocationDemo();
                    .ImplementInvocationOf(method);
            }

            return classTypeBuilder;
        }

        public static TypeBuilder AddInterfaceMethods(
            this TypeBuilder interfaceTypeBuilder,
            MethodInfo[] methods,
            bool fallbackToString)
        {
            // Interface Method declaration
            foreach (var method in methods)
            {
                interfaceTypeBuilder.AddInterfaceMethod(method, fallbackToString);
            }

            return interfaceTypeBuilder;
        }

        public static TypeBuilder AddInterfaceMethod(
            this TypeBuilder interfaceTypeBuilder,
            MethodInfo method,
            bool fallbackToString)
        {
            // Interface Method declaration
            MethodBuilder interfaceMethodBuilder = interfaceTypeBuilder
                .DefineInterfaceMethod(
                    method.Name,
                    method.ReturnType.GetServiceCompatibleFallback(fallbackToString),
                    method.GetParameterTypes(fallbackToString));

            interfaceMethodBuilder.CopySignatureOf(method);

            return interfaceTypeBuilder;
        }

        public static MethodBuilder DefineInterfaceMethod(
            this TypeBuilder interfaceTypeBuilder,
            string methodName,
            Type returnType = null,
            Type[] parameterTypes = null)
        {
            // Interface Method declaration
            MethodBuilder interfaceMethodBuilder = interfaceTypeBuilder
                .DefineMethod(
                    methodName,
                    Constants.InterfaceMethodAttributes,
                    returnType,
                    parameterTypes)
                .AddOperationContractAttribute();

            return interfaceMethodBuilder;
        }

        public static MethodBuilder DefineClassMethod(
            this TypeBuilder classTypeBuilder,
            string methodName,
            Type returnType = null,
            Type[] parameterTypes = null)
        {
            // Class Method declaration
            MethodBuilder classMethodBuilder = classTypeBuilder
                .DefineMethod(
                    methodName,
                    Constants.ClassMethodAttributes,
                    returnType,
                    parameterTypes);
            
            return classMethodBuilder;
        }

        #region Not Referenced

        public static TypeBuilder AddInterfaceMethod(
            this TypeBuilder interfaceTypeBuilder,
            string methodName,
            Type returnType = null,
            Type[] parameterTypes = null)
        {
            // Interface Method declaration
            MethodBuilder interfaceMethodBuilder = interfaceTypeBuilder
                .DefineInterfaceMethod(methodName, returnType, parameterTypes);

            return interfaceTypeBuilder;
        }

        #endregion Not Referenced
    }
}
