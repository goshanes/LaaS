using System;
using System.Linq;
using System.Reflection;

namespace ServiceAssemblyBuilder.Extensions
{
    public static class EType
    {
        public static Type GetServiceCompatibleFallback(this Type type, bool fallback)
        {
            if (!fallback)
            {
                return type;
            }
            if (type == typeof(string))
            {
                return type;
            }
            if (type.IsPrimitive)
            {
                return type;
            }
            if (type.IsArray)
            {
                if (type.BaseType == typeof(Array))
                {
                    if (type.BaseType.BaseType == typeof(object))
                    {
                        // Type is object[] (array of objects).
                        return typeof(string[]);
                    }
                }
            }
            if (type.IsByRef)
            {
                return type;
            }
            if (type.IsSerializable)
            {
                return type;
            }
            return typeof(string);
        }

        public static MethodInfo[] GetServiceCompatibleMethods(this Type type)
        {
            return type
                .GetMethods(Constants.SupportedMethodBindingFlags)
                .GetServiceCompatibleMethods();
        }

        public static MethodInfo[] GetServiceCompatibleMethods(this MethodInfo[] methods)
        {
            return methods
                .Where(m => m.IsPublic && m.IsStatic)
                // ISSUE: Duplicate names are not allowed.
                .GroupBy(m => m.Name)
                // ISSUE: Interface type as argument is not allowed.
                .Select(g => g.Where(x => x.IsServiceCompatible()).LastOrDefault())
                .Where(x => x != null)
                //.Take(1) // for diagnostics
                .ToArray();
        }
    }
}
