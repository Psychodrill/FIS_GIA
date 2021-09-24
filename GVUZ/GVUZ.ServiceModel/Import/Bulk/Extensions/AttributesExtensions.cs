using System;
using System.Reflection;
using GVUZ.ServiceModel.Import.Bulk.Attributes;

namespace GVUZ.ServiceModel.Import.Bulk.Extensions
{
    public static class AttributesExtensions
    {
        public static string GetDestinationTableName(this Type type)
        {
            foreach (DestinationTableNameAttribute attrib in type.GetCustomAttributes(typeof (DestinationTableNameAttribute), true))
                return attrib.TableName;

            throw new CustomAttributeFormatException("Не указана таблица назначения для bulk загрузки");
        }

        public static string GetBulkedProcessQuery<TEnum>(this TEnum item)
        {
            foreach (FieldInfo field in typeof (TEnum).GetFields(BindingFlags.Static | BindingFlags.GetField | BindingFlags.Public))
            {
                foreach (BulkedProcessQueryAttribute attrib in field.GetCustomAttributes(typeof (BulkedProcessQueryAttribute), true))
                    if ((int) field.GetValue(null) == Convert.ToInt32(item)) return attrib.Query;
            }

            throw new CustomAttributeFormatException("Не указана процедура для bulk постобработки");
        }
    }
}