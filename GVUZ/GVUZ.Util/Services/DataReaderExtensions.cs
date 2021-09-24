using System;
using System.Data.SqlClient;

namespace GVUZ.Util.Services
{
    public static class DataReaderExtensions
    {
        public static int GetIntValueOrDefault(this SqlDataReader reader, int ordinal, int defaultValue = 0)
        {
            int? value = GetIntValue(reader, ordinal);

            return value.GetValueOrDefault(defaultValue);
        }

        public static int? GetIntValue(this SqlDataReader reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
            {
                return null;
            }

            return reader.GetInt32(ordinal);
        }

        public static DateTime GetDateTimeValueOrDefault(this SqlDataReader reader, int ordinal, DateTime defaultValue)
        {
            return GetDateTimeValue(reader, ordinal).GetValueOrDefault(defaultValue);
        }

        public static DateTime? GetDateTimeValue(this SqlDataReader reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
            {
                return null;
            }

            return reader.GetDateTime(ordinal);
        }


    }
}