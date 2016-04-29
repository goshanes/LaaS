using System;
using System.Reflection;
using System.Reflection.Emit;
using System.ServiceModel;

namespace ServiceAssemblyBuilder.Extensions
{
    public static class EMethodBuilder
    {
        public static MethodBuilder AddOperationContractAttribute(
            this MethodBuilder methodBuilder)
        {
            CustomAttributeBuilder operationContractAttributeBuilder = new CustomAttributeBuilder(
                typeof(OperationContractAttribute).GetConstructor(Type.EmptyTypes),
                new object[] { });

            methodBuilder.SetCustomAttribute(operationContractAttributeBuilder);

            return methodBuilder;
        }

        public static MethodBuilder CopySignatureOf(
            this MethodBuilder methodBuilder,
            MethodInfo method)
        {
            // Class Method implementation
            int index = 1;
            foreach (var parameter in method.GetParameters())
            {
                //methodBuilder.DefineParameter(index, ParameterAttributes.None, parameter.Name);
                methodBuilder.DefineParameter(index, parameter.Attributes, parameter.Name);
                index++;
            }

            return methodBuilder;
        }

        public static MethodBuilder ImplementInvocationOf(
            this MethodBuilder methodBuilder,
            MethodInfo method)
        {
            // Class Method implementation
            methodBuilder.CopySignatureOf(method);

            ILGenerator methodCodeGenerator = methodBuilder.GetILGenerator();

            int index = 1;
            foreach (var parameter in method.GetParameters())
            {
                methodCodeGenerator.Emit(OpCodes.Ldarg, index);
                index++;
            }

            methodCodeGenerator.Emit(OpCodes.Call, method);
            methodCodeGenerator.Emit(OpCodes.Ret);

            return methodBuilder;
        }

        #region Not Referenced

        internal static MethodBuilder ImplementInvocationDemo(
            this MethodBuilder methodBuilder)
        {
            // Class Method implementation
            ILGenerator methodCodeGenerator = methodBuilder.GetILGenerator();

            LocalBuilder dateVariable = methodCodeGenerator.DeclareLocal(typeof(DateTime));

            MethodInfo migetNow = typeof(DateTime).GetProperty(
                "Now").GetGetMethod();
            MethodInfo miToString = typeof(DateTime).GetMethod(
                "ToString",
                Type.EmptyTypes);

            methodCodeGenerator.Emit(OpCodes.Call, migetNow);
            methodCodeGenerator.Emit(OpCodes.Stloc, dateVariable);
            methodCodeGenerator.Emit(OpCodes.Ldloca, dateVariable);
            methodCodeGenerator.Emit(OpCodes.Call, miToString);
            methodCodeGenerator.Emit(OpCodes.Ret);

            return methodBuilder;
        }

        #endregion Not Referenced
    }
}
