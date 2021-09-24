using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.ComponentModel;

namespace Fbs.Web.Extentions
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Преобразует значение перечисления в строку используя аттрибут Description
        /// </summary>
        /// <param name="someEnum">Значение перечесления</param>
        /// <param name="useAttribute">Использовать или нет аттрибут Description</param>
        /// <returns>Стрококое значение перечисления</returns>
        public static string ToString(this Enum someEnum, bool useAttribute)
        {
            if (!useAttribute)
                return someEnum.ToString();
            else
            {
                string name = someEnum.ToString();
                object[] attrs =
                    someEnum.GetType().GetField(someEnum.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
                return (attrs.Length > 0) ? ((DescriptionAttribute)attrs[0]).Description : name;
            }
        }

        /// <summary>
        /// Преобразует значение перечисления в список значений {Id,Value} используя аттрибут Description
        /// </summary>
        /// <param name="someEnum">Значение перечесления</param>
        /// <param name="useAttribute">Использовать или нет аттрибут Description</param>
        /// <returns>Список значений в формате {Id,Value}</returns>
        public static IEnumerable ToEnumerable(this Enum someEnum, bool useAttribute)
        {
            return Enum.GetValues(someEnum.GetType()).Cast<Enum>().Select(x => new { Id = x, Value = x.ToString(useAttribute) }).OrderBy(it => it.Value);
        }

        public static T FromString<T>(string value, bool useAttribute)
        {
            if (typeof(T) == typeof(Enum))
            {
                return (T)(object)Enum.GetValues(typeof(T)).Cast<Enum>().Single(x => x.ToString(useAttribute) == value);
            }
            else
                return default(T);
        }
    }
}
