using System;
using System.Reflection;

namespace GVUZ.ServiceModel.Import.Bulk.Attributes
{
    public class RootTagNameAttribute : Attribute
    {
        public RootTagNameAttribute(string name)
        {
            TagName = name;
        }

        public string TagName { get; private set; }
    }

    public static class AttributesExtensions
    {
        public static string GetRootTagName<TEnum>(this TEnum item)
        {
            return typeof (TEnum).GetRootTagName(Convert.ToInt32(item));
        }

        public static string GetRootTagName(this Type enumType, int value)
        {
            foreach (
                FieldInfo field in enumType.GetFields(BindingFlags.Static | BindingFlags.GetField | BindingFlags.Public)
                )
            {
                foreach (RootTagNameAttribute attrib in field.GetCustomAttributes(typeof (RootTagNameAttribute), true))
                {
                    if ((int) field.GetValue(null) == value) return attrib.TagName;
                }
            }

            throw new ApplicationException("Не найден RootTagNameAttribute аттрибут для XSD валидации");
        }
    }
}