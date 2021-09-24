namespace Esrp.Services
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
    }
}