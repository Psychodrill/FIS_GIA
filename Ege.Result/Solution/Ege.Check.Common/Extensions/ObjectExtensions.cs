namespace Ege.Check.Common.Extensions
{
    using System.Collections.Generic;
    using JetBrains.Annotations;

    public static class ObjectExtensions
    {
        /// <summary>
        ///     Превратить элемент в перечисление таких элементов
        /// </summary>
        /// <typeparam name="T">Тип элемента</typeparam>
        /// <param name="elem">Элемент</param>
        /// <returns>Перечисление из одного элемента</returns>
        [NotNull]
        public static IEnumerable<T> ToEnumerable<T>(this T elem)
        {
            yield return elem;
        }

        /// <summary>
        ///     Превратить элемент в перечисление таких элементов
        /// </summary>
        /// <typeparam name="T">Тип элемента</typeparam>
        /// <param name="elem">Элемент</param>
        /// <returns>Перечисление из одного элемента, если элемент не null. Иначе, пустое перечисление</returns>
        [NotNull]
        public static IEnumerable<T> ToEnumerableNotNull<T>(this T elem)
        {
            if (elem != null)
            {
                yield return elem;
            }
        }

        /// <summary>
        /// Привести объект к типу T, в случае неудачи отдать default(T)
        /// </summary>
        public static T As<T>(this object obj)
        {
            return obj is T ? (T)obj : default(T);
        }
    }
}
