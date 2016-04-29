using System.Reflection;

namespace ServiceAssemblyBuilder
{
    /// <summary>
    /// Declares common sets of flags for service class+interface and their member methods.
    /// </summary>
    public static class Constants
    {
        #region TypeAttributes

        /// <summary>
        /// Set of attributes for declaration of the service interface.
        /// </summary>
        public const TypeAttributes InterfaceAttributes =
            TypeAttributes.Interface
                | TypeAttributes.Abstract;

        /// <summary>
        /// Set of attributes for declaration of the service class.
        /// </summary>
        public const TypeAttributes ClassAttributes =
            TypeAttributes.Class
                | TypeAttributes.Public
                | TypeAttributes.BeforeFieldInit;

        #endregion TypeAttributes

        #region MethodAttributes

        /// <summary>
        /// Set of attributes for declaration of the service interface methods.
        /// </summary>
        public const MethodAttributes InterfaceMethodAttributes =
            MethodAttributes.Public
                | MethodAttributes.HideBySig
                | MethodAttributes.NewSlot
                | MethodAttributes.Abstract
                | MethodAttributes.Virtual;

        /// <summary>
        /// Set of attributes for declaration of the service class methods.
        /// </summary>
        public const MethodAttributes ClassMethodAttributes =
            MethodAttributes.Public
                | MethodAttributes.HideBySig
                | MethodAttributes.NewSlot
                | MethodAttributes.Virtual
                | MethodAttributes.Final;

        /// <summary>
        /// Set of attributes for declaration of the service class constructor.
        /// </summary>
        public const MethodAttributes ConstructorAttributes =
            MethodAttributes.Public
                | MethodAttributes.HideBySig;

        #endregion MethodAttributes

        public const BindingFlags SupportedMethodBindingFlags = BindingFlags.Public | BindingFlags.Static;
    }
}
