namespace Ege.Check.Common.Extensions
{
    using System;

    public static class TypeExtensions
    {
        /// <summary>
        ///     Попытаться получить тип T из Nullable{T}
        /// </summary>
        /// <param name="type">Исходный тип</param>
        /// <param name="valueType">Результирующий тип T или null, если исходный тип не Nullable{T}</param>
        /// <returns>Является ли исходный тип Nullable{T}</returns>
        public static bool TryGetNullableParameter(this Type type, out Type valueType)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type", "Type can not be null");
            }
            valueType = default(Type);
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof (Nullable<>))
            {
                valueType = type.GetGenericArguments()[0];
                return true;
            }
            return false;
        }
    }
}