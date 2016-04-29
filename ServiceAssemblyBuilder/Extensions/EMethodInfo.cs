using System;
using System.Linq;
using System.Reflection;

namespace ServiceAssemblyBuilder.Extensions
{
    public static class EMethodInfo
    {
        public static Type[] GetParameterTypes(this MethodInfo method, bool fallbackToString = false)
        {
            return method
                .GetParameters()
                .Select(p => p.ParameterType.GetServiceCompatibleFallback(fallbackToString))
                .ToArray();
        }

        public static bool IsServiceCompatible(this MethodInfo method)
        {
            return method.HasCompatibleParameterTypes()
                && method.HasCompatibleReturnType();
        }

        public static bool HasCompatibleParameterTypes(this MethodInfo method)
        {
            return !method
                .GetParameterTypes()
                .Any(t => t.IsInterface);
        }

        public static bool HasCompatibleReturnType(this MethodInfo method)
        {
            return !method.ReturnType.IsInterface;
        }
    }
}
