using System;

namespace Engine.Helpers
{
    /// <summary>
    /// Для обработки и помощи с рефлексией
    /// </summary>
    public static class ReflectionHelper
    {
        /// <summary>
        /// Create generic type (baseType<typeT>)
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="typeT"></param>
        /// <returns></returns>
        public static Type GetGenericType(Type baseType, Type typeT) {
            return baseType.MakeGenericType(new Type[] { typeT });
        }

        public static object CreateGenericType(Type baseType, Type typeT) {
            var type = GetGenericType(baseType, typeT);
            return Activator.CreateInstance(type);
        }
    }
}