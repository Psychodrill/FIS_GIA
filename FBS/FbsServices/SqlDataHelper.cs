using System.Data.SqlClient;

namespace FbsServices
{
    using System;
    using System.Data.SqlTypes;

    /// <summary>
    /// хэлпер для работы с Sql.Data
    /// </summary>
    public static class SqlDataHelper
    {
        /// <summary>
        /// проверить value тип на dbnull и вернуть сконвертированное значение
        /// </summary>
        /// <param name="value">
        /// обьект из дб
        /// </param>
        /// <typeparam name="T">
        /// ожидаемый тип обьекта из бд
        /// </typeparam>
        /// <returns>
        /// сконверченое к нужному типу значение 
        /// </returns>
        public static T? GetDBNullableValue<T>(object value) where T : struct
        {
            if (value is T)
            {
                return (T)value;
            }
            
            if (value is DBNull)
            {
                return null;
            }

            throw new SqlTypeException(string.Format("expected value of type '{0}', got '{1}'", typeof(T).FullName, value.GetType().FullName));
        }

        public static T Get<T>(this SqlDataReader reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
            {
                return default(T);
            }

            object obj = reader.GetValue(ordinal);

            try
            {
                return (T) obj;

            }
            catch (InvalidCastException)
            {
                return (T) Convert.ChangeType(obj, typeof (T));
            }
        }

        public static T Get<T>(this SqlDataReader reader, string column)
        {
            return Get<T>(reader, reader.GetOrdinal(column));
        }
    }
}