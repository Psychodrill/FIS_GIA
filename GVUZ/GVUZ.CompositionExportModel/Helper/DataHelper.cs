using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace GVUZ.CompositionExportModel.Helper
{
    public static class DataHelper
    {
        public static bool? GetBool(IDataReader reader, string columnName)
        {
            if (IsNull(reader, columnName))
                return null;

            return Convert.ToBoolean(reader[columnName]);
        }

        public static int? GetInt(IDataReader reader, string columnName)
        {
            if (IsNull(reader, columnName))
                return null;

            return Convert.ToInt32(reader[columnName]);
        }

        public static short? GetShort(IDataReader reader, string columnName)
        {
            if (IsNull(reader, columnName))
                return null;

            return Convert.ToInt16(reader[columnName]);
        }

        public static byte? GetByte(IDataReader reader, string columnName)
        {
            if (IsNull(reader, columnName))
                return null;

            return Convert.ToByte(reader[columnName]);
        }

        public static Guid? GetGuid(IDataReader reader, string columnName)
        {
            if (IsNull(reader, columnName))
                return null;

            return new Guid(reader[columnName].ToString());
        }

        public static DateTime? GetDateTime(IDataReader reader, string columnName)
        {
            if (IsNull(reader, columnName))
                return null;

            string str = reader[columnName].ToString();
            DateTime result;
            if (DateTime.TryParse(str, out result))
                return result;
            if (DateTime.TryParseExact(str, "yyyy.MM.dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out result))
                return result;

            return null;
        }

        public static string GetString(IDataReader reader, string columnName)
        {
            if (IsNull(reader, columnName))
                return null;

            return reader[columnName].ToString();
        }

        public static string ToString(DateTime? dateTime)
        {
            if (dateTime == null)
                return String.Empty;
            return dateTime.Value.ToShortDateString();
        }

        private static bool IsNull(IDataReader reader, string columnName)
        {
            return ((reader[columnName] == DBNull.Value) || (reader[columnName] == null));
        }

        public static byte[] StringToBytes(string stringValue)
        {
            if (stringValue == null)
                return null;
            return Encoding.UTF8.GetBytes(stringValue);
        }

        public static string BytesToString(byte[] bytes)
        {
            if (bytes == null)
                return null;
            return Encoding.UTF8.GetString(bytes);
        }

        public static object ReplaceNullToDBNull(object value)
        {
            if (value == null)
                return DBNull.Value;
            return value;
        }

        public static string NormalizeString(string value, bool removeWhitespaces)
        {
            if (value == null)
            {
                value = String.Empty;
            }
            if (removeWhitespaces)
            {
                value = value.Replace(" ", "");
            }
            return value.ToUpper().Replace("Ё", "Е").Replace("Й", "И");
        }
    }
}
