using System;
using System.Text;

namespace FBS.Replicator
{
    public static class DataHelper
    { 
        public static bool? GetBool(FastDataReader reader, string columnName)
        {
            bool? result;
            if (IsNull(reader, columnName))
            {
                result = null;
            }
            else
            {
                result = Convert.ToBoolean(reader.GetObject(columnName));
            }
            return result;
        }

        public static int? GetInt(FastDataReader reader, string columnName)
        {
            int? result;
            if (IsNull(reader, columnName))
            {
                result = null;
            }
            else
            {
                result = Convert.ToInt32(reader.GetObject(columnName));
            }
            return result;
        }

        public static short? GetShort(FastDataReader reader, string columnName)
        {
            short? result;
            if (IsNull(reader, columnName))
            {
                result = null;
            }
            else
            {
                result = Convert.ToInt16(reader.GetObject(columnName));
            }
            return result;
        }

        public static byte? GetByte(FastDataReader reader, string columnName)
        {
            byte? result;
            if (IsNull(reader, columnName))
            {
                result = null;
            }
            else
            {
                object obj = reader.GetObject(columnName);
                if (obj is byte)
                {
                    result = (byte)obj;
                }
                else if (obj is short)
                {
                    result = (byte)(short)obj;
                }
                else if (obj is int)
                {
                    result = (byte)(int)obj;
                }
                else if (obj is long)
                {
                    result = (byte)(long)obj;
                }
                else
                {
                    result = Convert.ToByte(reader.GetObject(columnName));
                }
            }
            return result;
        }

        public static Guid? GetGuid(FastDataReader reader, string columnName)
        {
            Guid? result;
            if (IsNull(reader, columnName))
            {
                result = null;
            }
            else
            {
                object obj = reader.GetObject(columnName);
                if (obj is Guid)
                {
                    result = (Guid)obj;
                }
                else
                {
                    result = new Guid(obj.ToString());
                }
            }
            return result;
        }

        public static DateTime? GetDateTime(FastDataReader reader, string columnName)
        {
            DateTime? result;
            if (IsNull(reader, columnName))
            {
                result = null;
            }
            else
            {
                object obj = reader.GetObject(columnName);
                if (obj is DateTime)
                {
                    result = (DateTime)obj;
                }
                else
                {
                    string str = obj.ToString();
                    DateTime temp;
                    if (DateTime.TryParse(str, out temp))
                    {
                        result = temp;
                    }
                    else if (DateTime.TryParseExact(str, "yyyy.MM.dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out temp))
                    {
                        result = temp;
                    }
                    else
                    {
                        result = null;
                    }
                }
            }
            return result;
        }

        public static string GetString(FastDataReader reader, string columnName)
        {
            string result;
            if (IsNull(reader, columnName))
            {
                result = null;
            }
            else
            {
                result = reader.GetObject(columnName).ToString();
            }
            return result;
        }

        private static bool IsNull(FastDataReader reader, string columnName)
        {
            bool result = reader.IsNull(columnName);
            return result;
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
