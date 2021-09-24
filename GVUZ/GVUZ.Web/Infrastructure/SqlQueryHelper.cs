using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using GVUZ.Web.ViewModels.Shared;
using GVUZ.DAL.Dapper.ViewModel.Common;


namespace GVUZ.Web.Infrastructure {
    public static class SqlQueryHelper {
        /// <summary>
        /// Постраничный вывод: название парамтера содержащего значение номера первой записи
        /// </summary>
        public const string PageFirstRecordParameterName = "@pFirstPageRecord";
        /// <summary>
        /// Постраничный вывод: название парамтера содержащего значение номера последней записи
        /// </summary>
        public const string PageLastRecordParameterName = "@pLastPageRecord";
        /// <summary>
        /// Постраничный вывод: название временной таблицы (CTE) содержащей основной запрос выборки
        /// </summary>
        public const string PageMainQueryTableName = "main";
        /// <summary>
        /// Постраничный вывод: название временной таблицы (CTE) содержащей запрос разбивающий записи из основного запроса выборки
        /// с присвоением номеров строк
        /// </summary>
        public const string PagePagingQueryTableName = "paged";
        /// <summary>
        /// Формат вывода даты по-умолчанию
        /// </summary>
        public const string DefaultDateFormat = "dd.MM.yyyy";

        #region SqlDataReader helpers
        /// <summary>
        /// Читает строковое значение
        /// </summary>
        /// <param name="reader">инстанс <see cref="SqlDataReader"/></param>
        /// <param name="fieldName">название поля (колонки) в выборке</param>
        /// <returns>Строковое значение или null</returns>
        public static string SafeGetString(this SqlDataReader reader, string fieldName) {
            return SafeGetString(reader, reader.GetOrdinal(fieldName));
        }

        /// <summary>
        /// Читает boolean-значение
        /// </summary>
        /// <param name="reader">инстанс <see cref="SqlDataReader"/></param>
        /// <param name="fieldName">название поля (колонки) в выборке</param>
        /// <returns>bool-значение или null</returns>
        public static bool? SafeGetBool(this SqlDataReader reader, string fieldName) {
            return SafeGetBool(reader, reader.GetOrdinal(fieldName));
        }

        /// <summary>
        /// Читает целочисленное (int) значение
        /// </summary>
        /// <param name="reader">инстанс <see cref="SqlDataReader"/></param>
        /// <param name="fieldName">название поля (колонки) в выборке</param>
        /// <returns>Целочисленное (int) значение или null</returns>
        public static int? SafeGetInt(this SqlDataReader reader, string fieldName) {
            return SafeGetInt(reader, reader.GetOrdinal(fieldName));
        }

        /// <summary>
        /// Читает целочисленное (short) значение
        /// </summary>
        /// <param name="reader">инстанс <see cref="SqlDataReader"/></param>
        /// <param name="fieldName">название поля (колонки) в выборке</param>
        /// <returns>Целочисленное (short) значение или null</returns>
        public static short? SafeGetShort(this SqlDataReader reader, string fieldName) {
            return SafeGetShort(reader, reader.GetOrdinal(fieldName));
        }

        /// <summary>
        /// Читает значение как DateTime
        /// </summary>
        /// <param name="reader">инстанс <see cref="SqlDataReader"/></param>
        /// <param name="fieldName">название поля (колонки) в выборке</param>
        /// <returns>значение DateTime или null</returns>
        public static DateTime? SafeGetDateTime(this SqlDataReader reader, string fieldName) {
            return SafeGetDateTime(reader, reader.GetOrdinal(fieldName));
        }

        /// <summary>
        /// Читает значение как int преобразованное в строку
        /// </summary>
        /// <param name="reader">инстанс <see cref="SqlDataReader"/></param>
        /// <param name="fieldName">название поля (колонки) в выборке</param>
        /// <returns>значение int, преобразованное в строку или null</returns>
        public static string SafeGetIntAsString(this SqlDataReader reader, string fieldName) {
            return SafeGetIntAsString(reader, reader.GetOrdinal(fieldName));
        }

        /// <summary>
        /// Читает значение как short преобразованное в строку
        /// </summary>
        /// <param name="reader">инстанс <see cref="SqlDataReader"/></param>
        /// <param name="fieldName">название поля (колонки) в выборке</param>
        /// <returns>значение short, преобразованное в строку или null</returns>
        public static string SafeGetShortAsString(this SqlDataReader reader, string fieldName) {
            return SafeGetShortAsString(reader, reader.GetOrdinal(fieldName));
        }

        /// <summary>
        /// Читает значение как DateTime преобразованное в строку с заданным форматом
        /// </summary>
        /// <param name="reader">инстанс <see cref="SqlDataReader"/></param>
        /// <param name="fieldName">название поля (колонки) в выборке</param>
        /// <param name="dateFormat">формат для представления DateTime в строке, по-умолчанию соответствует DefaultDateFormat (dd.MM.yyyy)</param>
        /// <returns>значение DateTime, преобразованное в строку с заданным форматом (dateFormat) или null</returns>
        public static string SafeGetDateTimeAsString(this SqlDataReader reader, string fieldName,
                                                     string dateFormat = DefaultDateFormat) {
            return SafeGetDateTimeAsString(reader, reader.GetOrdinal(fieldName), dateFormat);
        }

        /// <summary>
        /// Читает decimal-значение
        /// </summary>
        /// <param name="reader">инстанс <see cref="SqlDataReader"/></param>
        /// <param name="fieldName">название поля (колонки) в выборке</param>
        /// <returns>decimal-значение или null</returns>
        public static decimal? SafeGetDecimal(this SqlDataReader reader, string fieldName) {
            return SafeGetDecimal(reader, reader.GetOrdinal(fieldName));
        }

        /// <summary>
        /// Читает строковое значение
        /// </summary>
        /// <param name="reader">инстанс <see cref="SqlDataReader"/></param>
        /// <param name="ordinal">порядковый номер поля (колонки) в выборке</param>
        /// <returns>Строковое значение или null</returns>
        public static string SafeGetString(this SqlDataReader reader, int ordinal) {
            return reader.IsDBNull(ordinal) ? null : reader.GetString(ordinal);
        }

        /// <summary>
        /// Читает boolean-значение
        /// </summary>
        /// <param name="reader">инстанс <see cref="SqlDataReader"/></param>
        /// <param name="ordinal">порядковый номер поля (колонки) в выборке</param>
        /// <returns>bool-значение или null</returns>
        public static bool? SafeGetBool(this SqlDataReader reader, int ordinal) {
            if (reader.IsDBNull(ordinal))
                return null;

            return reader.GetBoolean(ordinal);
        }

        public static string SafeGetBoolAsString(this SqlDataReader reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
                return null;

            return reader.GetBoolean(ordinal).ToString();
        }

        /// <summary>
        /// Читает целочисленное (int) значение
        /// </summary>
        /// <param name="reader">инстанс <see cref="SqlDataReader"/></param>
        /// <param name="ordinal">порядковый номер поля (колонки) в выборке</param>
        /// <returns>Целочисленное (int) значение или null</returns>
        public static int? SafeGetInt(this SqlDataReader reader, int ordinal) {
            if (reader.IsDBNull(ordinal))
                return null;

            return reader.GetInt32(ordinal);
        }

        /// <summary>
        /// Читает целочисленное (short) значение
        /// </summary>
        /// <param name="reader">инстанс <see cref="SqlDataReader"/></param>
        /// <param name="ordinal">порядковый номер поля (колонки) в выборке</param>
        /// <returns>Целочисленное (short) значение или null</returns>
        public static short? SafeGetShort(this SqlDataReader reader, int ordinal) {
            if (reader.IsDBNull(ordinal))
                return null;

            return reader.GetInt16(ordinal);
        }
        /// <summary>
        /// Читает значение как DateTime
        /// </summary>
        /// <param name="reader">инстанс <see cref="SqlDataReader"/></param>
        /// <param name="ordinal">порядковый номер поля (колонки) в выборке</param>
        /// <returns>значение DateTime или null</returns>
        public static DateTime? SafeGetDateTime(this SqlDataReader reader, int ordinal) {
            if (reader.IsDBNull(ordinal))
                return null;

            return reader.GetDateTime(ordinal);
        }

        /// <summary>
        /// Читает значение как DateTime преобразованное в строку с заданным форматом
        /// </summary>
        /// <param name="reader">инстанс <see cref="SqlDataReader"/></param>
        /// <param name="ordinal">порядковый номер поля (колонки) в выборке</param>
        /// <param name="dateFormat">формат для представления DateTime в строке, по-умолчанию соответствует DefaultDateFormat (dd.MM.yyyy)</param>
        /// <returns>значение DateTime, преобразованное в строку с заданным форматом (dateFormat) или null</returns>
        public static string SafeGetDateTimeAsString(this SqlDataReader reader, int ordinal, string dateFormat = DefaultDateFormat) {
            DateTime? date = SafeGetDateTime(reader, ordinal);
            if (date.HasValue) {
                return date.Value.ToString(dateFormat);
            }

            return null;
        }

        /// <summary>
        /// Читает значение как DateTime
        /// </summary>
        /// <param name="reader">инстанс <see cref="SqlDataReader"/></param>
        /// <param name="ordinal">порядковый номер поля (колонки) в выборке</param>
        /// <returns>значение DateTime или null</returns>
        public static DateTime? SafeGetFullDateTime(this SqlDataReader reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
                return null;

            //DateTime gmtDate = CovertDateTimeToGmt((DateTime)reader.GetValue(ordinal));
            return (DateTime)reader.GetValue(ordinal);
        }

        public static string CovertDateTimeToGmt(this DateTime source)
        {
            var destinationTimezoneId = TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time");
            DateTime utcDateTime = TimeZoneInfo.ConvertTimeToUtc(source, destinationTimezoneId);

            TimeZoneInfo local = TimeZoneInfo.Local;

            DateTime localDateTime = TimeZoneInfo.ConvertTime(utcDateTime, local);

            //return localDateTime;
            string result = string.Concat(localDateTime.ToString(), local.ToString());
            return result;
        }

        /// <summary>
        /// Читает значение как DateTime преобразованное в строку с заданным форматом
        /// </summary>
        /// <param name="reader">инстанс <see cref="SqlDataReader"/></param>
        /// <param name="ordinal">порядковый номер поля (колонки) в выборке</param>
        /// <param name="dateFormat">формат для представления DateTime в строке, по-умолчанию соответствует DefaultDateFormat (dd.MM.yyyy)</param>
        /// <returns>значение DateTime, преобразованное в строку с заданным форматом (dateFormat) или null</returns>
        public static string SafeGetFullDateTimeAsString(this SqlDataReader reader, int ordinal)
        {
            DateTime? date = SafeGetFullDateTime(reader, ordinal);

            if (date.HasValue)
            {
                string gmtDate = CovertDateTimeToGmt(date.Value);
                return gmtDate;
            }

            return null;
        }


        /// <summary>
        /// Читает значение как int преобразованное в строку
        /// </summary>
        /// <param name="reader">инстанс <see cref="SqlDataReader"/></param>
        /// <param name="ordinal">порядковый номер поля (колонки) в выборке</param>
        /// <returns>значение int, преобразованное в строку или null</returns>
        public static string SafeGetIntAsString(this SqlDataReader reader, int ordinal) {
            int? i = SafeGetInt(reader, ordinal);

            if (i.HasValue) {
                return i.Value.ToString(CultureInfo.InvariantCulture);
            }

            return null;
        }

        /// <summary>
        /// Читает значение как short преобразованное в строку
        /// </summary>
        /// <param name="reader">инстанс <see cref="SqlDataReader"/></param>
        /// <param name="ordinal">порядковый номер поля (колонки) в выборке</param>
        /// <returns>значение short, преобразованное в строку или null</returns>
        public static string SafeGetShortAsString(this SqlDataReader reader, int ordinal) {
            short? i = SafeGetShort(reader, ordinal);

            if (i.HasValue) {
                return i.Value.ToString(CultureInfo.InvariantCulture);
            }

            return null;
        }

        /// <summary>
        /// Читает decimal-значение
        /// </summary>
        /// <param name="reader">инстанс <see cref="SqlDataReader"/></param>
        /// <param name="ordinal">порядковый номер поля (колонки) в выборке</param>
        /// <returns>decimal-значение или null</returns>
        public static decimal? SafeGetDecimal(this SqlDataReader reader, int ordinal) {
            if (reader.IsDBNull(ordinal))
                return null;

            return reader.GetDecimal(ordinal);
        }
        #endregion

        /// <summary>
        /// Инстанс <see cref="ITransactionManager"/>, соответствующий текущему контексту http-запроса (HttpContext.Current)
        /// </summary>
        public static ITransactionManager TransactionManager {
            get { return Infrastructure.TransactionManager.Current; }
        }

        /// <summary>
        /// Возвращает список записей соответствующих результату sql-запроса
        /// </summary>
        /// <typeparam name="TRecord">Тип возвращаемой записи (например, тип view-модели)</typeparam>
        /// <param name="queryText">Текст sql-запроса</param>
        /// <param name="parameters">Параметры sql-запроса, соответствующие тексту</param>
        /// <param name="mapFromReader">Процедура для преобразования записи из SqlDataReader к объекту с типом TRecord (делегат)</param>
        /// <returns>Список записей с типом TRecord</returns>
        public static List<TRecord> GetRecords<TRecord>(string queryText, SqlParameter[] parameters, Func<SqlDataReader, TRecord> mapFromReader) {
            List<TRecord> result = new List<TRecord>();

            using (SqlCommand selectCommand = TransactionManager.CreateCommand(CommandType.Text, queryText)) {
                if (parameters != null) {
                    selectCommand.Parameters.AddRange(parameters);
                }
                selectCommand.CommandTimeout = 300;	// Время ожидания ответа 120 секунд

                using (SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.SingleResult)) {
                    while (reader.Read()) {
                        result.Add(mapFromReader(reader));
                    }
                }
            }

            return result;
        }


        public static List<TRecord> GetRecordsProc<TRecord>(string queryText, SqlParameter[] parameters, Func<SqlDataReader, TRecord> mapFromReader)
        {
            List<TRecord> result = new List<TRecord>();

            using (SqlCommand selectCommand = TransactionManager.CreateCommand(CommandType.StoredProcedure, queryText))
            {
                if (parameters != null)
                {
                    selectCommand.Parameters.AddRange(parameters);
                }
                selectCommand.CommandTimeout = 300;	// Время ожидания ответа 120 секунд

                using (SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.SingleResult))
                {
                    while (reader.Read())
                    {
                        result.Add(mapFromReader(reader));
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Возвращает запись соответствующую единственному результату sql-запроса
        /// </summary>
        /// <typeparam name="TRecord">Тип возвращаемой записи (например, тип view-модели)</typeparam>
        /// <param name="queryText">Текст sql-запроса</param>
        /// <param name="parameters">Параметры sql-запроса, соответствующие тексту</param>
        /// <param name="mapFromReader">Процедура для преобразования записи из SqlDataReader к объекту с типом TRecord (делегат)</param>
        /// <param name="defaultValue">Значение, которое будет возвращено если результат запроса не содержит ни одной записи. По-умолчанию соответствует default(TRecord) </param>
        /// <returns>Запись с типом TRecord</returns>
        public static TRecord GetRecord<TRecord>(string queryText, SqlParameter[] parameters, Func<SqlDataReader, TRecord> mapFromReader, TRecord defaultValue = default(TRecord)) {
            using (SqlCommand selectCommand = TransactionManager.CreateCommand(CommandType.Text, queryText)) {
                if (parameters != null) {
                    selectCommand.Parameters.AddRange(parameters);
                }

                using (SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.SingleResult | CommandBehavior.SingleRow)) {
                    if (reader.HasRows && reader.Read()) {
                        return mapFromReader(reader);
                    }
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// Выполняет ExecuteScalar() и возвращает полученное значение как Int32 или null
        /// </summary>
        /// <param name="queryText">Текст запроса</param>
        /// <param name="parameters">Параметры запроса</param>
        /// <returns>Значение в виде Int32 или null (int?)</returns>
        public static int? GetScalarInt(string queryText, SqlParameter[] parameters) {
            using (SqlCommand cmd = TransactionManager.CreateCommand(CommandType.Text, queryText)) {
                if (parameters != null) {
                    cmd.Parameters.AddRange(parameters);
                }

                object res = cmd.ExecuteScalar();

                if (res != null && res != DBNull.Value) {
                    return Convert.ToInt32(res);
                }
            }

            return null;
        }

        /// <summary>
        /// Выполняет ExecuteScalar() и возвращает полученное значение как bool или null
        /// </summary>
        /// <param name="queryText">Текст запроса</param>
        /// <param name="parameters">Параметры запроса</param>
        /// <returns>Значение в виде bool или null (bool?)</returns>
        public static bool? GetScalarBool(string queryText, SqlParameter[] parameters) {
            using (SqlCommand cmd = TransactionManager.CreateCommand(CommandType.Text, queryText)) {
                if (parameters != null) {
                    cmd.Parameters.AddRange(parameters);
                }

                object res = cmd.ExecuteScalar();

                if (res != null && res != DBNull.Value) {
                    return Convert.ToBoolean(res);
                }
            }

            return null;
        }
        /// <summary>
        /// Выполняет ExecuteNonQuery() и возвращает количество строк, измененное запросом
        /// </summary>
        /// <param name="queryText">Текст запроса</param>
        /// <param name="parameters">Параметры запроса</param>
        /// <returns>Количество строк, измененных запросом</returns>
        public static int Execute(string queryText, SqlParameter[] parameters) {
            using (SqlCommand cmd = TransactionManager.CreateCommand(CommandType.Text, queryText)) {
                if (parameters != null) {
                    cmd.Parameters.AddRange(parameters);
                }

                return cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Выполняет запрос, возвращающий единственную запись, и передает результат обработки делегату.
        /// Если запрос возварщает более одной записи, то делегат будет вызван только для самой первой, остальные записи будут проигнорированы.
        /// </summary>
        /// <param name="queryText">Текст запроса</param>
        /// <param name="parameters">Параметры sql-запроса</param>
        /// <param name="processOne">Делегат, принимающий SqlDataReader если запрос вернул хотя бы одну запись</param>
        /// <returns>true если результат выполнения запроса содержит хотя бы одну запись, или false если запрос не содержит ни одной записи</returns>
        public static bool SelectOne(string queryText, SqlParameter[] parameters, Action<SqlDataReader> processOne) {
            using (SqlCommand selectCommand = TransactionManager.CreateCommand(CommandType.Text, queryText)) {
                if (parameters != null) {
                    selectCommand.Parameters.AddRange(parameters);
                }

                using (SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.SingleResult | CommandBehavior.SingleRow)) {
                    if (reader.HasRows && reader.Read()) {
                        processOne(reader);
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Выполняет запрос, возвращающий 0 или более записи, для каждой найденной записи вызывается делегат-обработчик
        /// </summary>
        /// <param name="queryText">Текст запроса</param>
        /// <param name="parameters">Параметры sql-запроса</param>
        /// <param name="processAny">Делегат, принимающий SqlDataReader, вызываемый для одной иили более найденной записи</param>
        public static void SelectAny(string queryText, SqlParameter[] parameters, Action<SqlDataReader> processAny) {
            using (SqlCommand selectCommand = TransactionManager.CreateCommand(CommandType.Text, queryText)) {
                if (parameters != null) {
                    selectCommand.Parameters.AddRange(parameters);
                }

                using (SqlDataReader reader = selectCommand.ExecuteReader(CommandBehavior.SingleResult)) {
                    while (reader.Read()) {
                        processAny(reader);
                    }
                }
            }
        }
        /// <summary>
        /// Возвращает список записей соответствующих результату sql-запроса с постраничной разбивкой
        /// </summary>
        /// <typeparam name="TRecord">Тип возвращаемой записи (например, тип view-модели)</typeparam>
        /// <param name="queryText">Текст sql-запроса</param>
        /// <param name="parameters">Параметры sql-запроса, соответствующие тексту</param>
        /// <param name="mapFromReader">Процедура для преобразования записи из SqlDataReader к объекту с типом TRecord (делегат)</param>
        /// <param name="pager">Данные для постраничной разбивки (диапазон номеров записей и общее количество записей) <see cref="IPagination"/></param>
        /// <param name="sort">Данные для сортировки в подзапросе, осуществляющим разбивку основного запроса на страницы</param>
        /// <param name="additionalFilter">Дополнительный фильтр, используемый в подзапросе разбивки на страницы, для дополнительной фильтрации данных подготовленных основным запросом</param>
        /// <returns>Список записей с типом TRecord</returns>
        public static List<TRecord> GetPagedRecords<TRecord>(string queryText, SqlParameter[] parameters, Func<SqlDataReader, TRecord> mapFromReader, IPagination pager, ISortable sort, string additionalFilter = null) {
            List<TRecord> result = new List<TRecord>();

            using (SqlConnection con = new SqlConnection(GVUZ.Web.ContextExtensionsSQL.SQL.ConnectionString)) {
                SqlCommand countRows = new SqlCommand(queryText.MakeRowCountQueryText(additionalFilter), con);

                if (parameters != null) {
                    countRows.Parameters.AddRange(parameters);
                }
                con.Open();
                pager.TotalRecords = Convert.ToInt32(countRows.ExecuteScalar());
                con.Close();
            }
            if (pager.TotalRecords > 0) {
                using (SqlConnection con = new SqlConnection(GVUZ.Web.ContextExtensionsSQL.SQL.ConnectionString)) {
                    SqlCommand selectPage = new SqlCommand(queryText.MakePagedQueryText(sort, additionalFilter), con);
                    if (parameters != null) {
                        selectPage.Parameters.AddRange(parameters.CopyToArray());
                    }
                    selectPage.Parameters.AddRange(CreatePageRangeParameters(pager.FirstRecordOffset, pager.LastRecordOffset));
                    con.Open();
                    using (SqlDataReader reader = selectPage.ExecuteReader(CommandBehavior.SingleResult)) {
                        while (reader.Read()) {
                            result.Add(mapFromReader(reader));
                        }
                    }
                    con.Close();
                }
            }






            //using (SqlCommand countRows = TransactionManager.CreateCommand(CommandType.Text))
            //{
            //    countRows.CommandText = queryText.MakeRowCountQueryText(additionalFilter);

            //    if (parameters != null)
            //    {
            //        countRows.Parameters.AddRange(parameters);
            //    }

            //    pager.TotalRecords = Convert.ToInt32(countRows.ExecuteScalar());
            //}

            //if (pager.TotalRecords > 0)
            //{
            //    using (SqlCommand selectPage = TransactionManager.CreateCommand(CommandType.Text))
            //    {
            //        selectPage.CommandText = queryText.MakePagedQueryText(sort, additionalFilter);

            //        if (parameters != null)
            //        {
            //            selectPage.Parameters.AddRange(parameters.CopyToArray());    
            //        }

            //        selectPage.Parameters.AddRange(CreatePageRangeParameters(pager.FirstRecordOffset, pager.LastRecordOffset));

            //        using (SqlDataReader reader = selectPage.ExecuteReader(CommandBehavior.SingleResult))
            //        {
            //            while (reader.Read())
            //            {
            //                result.Add(mapFromReader(reader));
            //            }
            //        }
            //    }
            //}

            return result;
        }

        public static List<TRecord> GetPagedRecordsNew<TRecord>(/*string queryText,*/ SqlParameter[] parameters, Func<SqlDataReader, TRecord> mapFromReader, IPagination pager, ISortable sort, string additionalFilter = null)
        {
            List<TRecord> result = new List<TRecord>();

            int totalCount = 0;
            //if (pager.TotalRecords > 0)
            //{
            //using (SqlConnection con = new SqlConnection(GVUZ.Web.ContextExtensionsSQL.SQL.ConnectionString))
            //{
            //    SqlCommand selectPage = new SqlCommand(queryText, con);
            //    if (parameters != null)
            //    {
            //        selectPage.Parameters.AddRange(parameters.CopyToArray());
            //    }
            //    selectPage.Parameters.AddRange(CreatePageRangeParameters(pager.FirstRecordOffset, pager.LastRecordOffset));
            //    con.Open();
            //    using (SqlDataReader reader = selectPage.ExecuteReader(CommandBehavior.SingleResult))
            //    {
            //        while (reader.Read())
            //        {
            //            result.Add(mapFromReader(reader));
            //        }
            //    }
            //    con.Close();
            //}
            Dictionary<int, string> orderBy = new Dictionary<int, string>();
            orderBy.Add(1,"ApplicationNumber");
            orderBy.Add(2, "StatusName");
            orderBy.Add(3, "LastCheckDate");
            orderBy.Add(4, "EntrantName");
            orderBy.Add(5, "IdentityDocument");
            orderBy.Add(6, "RegistrationDate");
            orderBy.Add(7, "Rating");


            int orderKey = orderBy.Where(x => x.Value == sort.SortKey).Select(x=>x.Key).FirstOrDefault();
            using (SqlCommand cmd = TransactionManager.CreateCommand(CommandType.StoredProcedure, "ftc_EntrantApplicationSearch"))
            {


                cmd.Parameters.AddRange(parameters.CopyToArray());
                cmd.Parameters.AddWithValue("count_only", 1);
                cmd.CommandTimeout = 120;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        totalCount = reader.GetInt32(0);


                    }

                }

                cmd.Parameters.RemoveAt(cmd.Parameters.IndexOf("count_only"));
                cmd.Parameters.AddWithValue("count_only", 0);
                cmd.Parameters.AddWithValue("order_by", orderKey);
                cmd.Parameters.AddWithValue("is_desc", sort.SortDescending);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        result.Add(mapFromReader(reader));


                    }
                }


            }
            pager.TotalRecords = totalCount;
            return result;
        }



        /// <summary>
        /// Преобразование текста обычного sql-запроса в запрос на подсчет количества записей
        /// </summary>
        /// <param name="mainQuery">Текст основного запроса выборки данных</param>
        /// <param name="where">Дополнительный фильтр, используемый в подзапросе расчета количества записей для дополнительной фильтрации данных подготовленных основным запросом</param>
        /// <returns>Текст запроса для расчета количества записей, удовлетворяющих условиям основного запроса</returns>
        public static string MakeRowCountQueryText(this string mainQuery, string where = null) {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(string.Format("with {0} as (", PageMainQueryTableName));
            sql.AppendLine(mainQuery);
            sql.AppendLine(")");
            sql.AppendLine(string.Format("select count({0}.ApplicationID) from {0}", PageMainQueryTableName));
            if (where != null) {
                if (where != "") {
                    sql.AppendLine("where");
                    sql.AppendLine(where.Contains("{0}") ? string.Format(where, PageMainQueryTableName) : where);
                }
            }

            return sql.ToString();
        }

        /// <summary>
        /// Преобразование текста обычного sql-запроса в запрос на выборку данных с постраничной разбивкой
        /// </summary>
        /// <param name="mainQuery">Текст основного запроса выборки данных</param>
        /// <param name="sort">Данные о правилах сортировки при разбиении на страницы (используется в ROW_NUMBER() over(ORDER BY ....)</param>
        /// <param name="where">Дополнительный фильтр, используемый в подзапросе разбивки на страницы для дополнительной фильтрации данных подготовленных основным запросом</param>
        /// <returns>Текст запроса для выборки данных, соответствующих условиям диапазона номеров записей</returns>
        public static string MakePagedQueryText(this string mainQuery, ISortable sort, string where = null) {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(string.Format("with {0} as (", PageMainQueryTableName));
            sql.AppendLine(mainQuery);
            sql.AppendLine("),");
            sql.AppendLine(string.Format("{0} as (", PagePagingQueryTableName));
            sql.AppendLine(string.Format("select {0}.*, ROW_NUMBER() over(ORDER BY ", PageMainQueryTableName));
            sql.AppendFormat("{0}.{1} {2}", PageMainQueryTableName, sort.SortKey, sort.SortDescending ? "DESC" : "ASC");
            sql.AppendLine(") as rn");
            sql.AppendLine(string.Format("from {0}", PageMainQueryTableName));
            if (where != null) {
                if (where != "") {
                    sql.AppendLine("where");
                    sql.AppendLine(where.Contains("{0}") ? string.Format(where, PageMainQueryTableName) : where);
                }
            }
            sql.AppendLine(")");
            sql.AppendFormat("select {0}.* from {0} where {0}.rn BETWEEN {1} and {2}", PagePagingQueryTableName, PageFirstRecordParameterName, PageLastRecordParameterName);

            return sql.ToString();
        }

        public static SqlParameter[] CreatePageRangeParameters(int firstRecordOffset, int lastRecordOffset) {
            return new[]
                {
                    new SqlParameter(PageFirstRecordParameterName, SqlDbType.Int)
                        {
                            Value = firstRecordOffset
                        },
                    new SqlParameter(PageLastRecordParameterName, SqlDbType.Int)
                        {
                            Value = lastRecordOffset
                        }
                };
        }

        #region Общие выражения для динамического текста параметризованных SQL-запросов
        public static SqlParameter FieldLikeOrNullParam(this List<string> filter, string field, string parameterName, string parameterValue) {
            SqlParameter p = new SqlParameter(parameterName.AsSqlParamName(), SqlDbType.VarChar);

            if (!string.IsNullOrEmpty(parameterValue)) {
                filter.AddFormat("{0} LIKE({1})", field, p.ParameterName);
                p.Value = string.Format("%{0}%", parameterValue.Replace("%", "[%]"));
            } else {
                filter.AddFormat("{0} is null", p.ParameterName);
                p.Value = DBNull.Value;
            }

            return p;
        }

        public static object LikeParamValueOrDBNull(string parameterValue) {
            if (!string.IsNullOrEmpty(parameterValue)) {
                return string.Format("%{0}%", parameterValue.Replace("%", "[%]"));
            }

            return DBNull.Value;
        }

        public static SqlParameter LikeParamValueOrNull(this List<string> filter, string field, string parameterName, string parameterValue)
        {
            SqlParameter p = new SqlParameter(parameterName.AsSqlParamName(), SqlDbType.VarChar);

            if (!string.IsNullOrEmpty(parameterValue))
            {
                filter.AddFormat("({0} LIKE {1} OR {1} IS NULL)", field, p.ParameterName);
                p.Value = string.Format("%{0}%", parameterValue.Replace("%", "[%]"));
            }
            else
            {
                filter.AddFormat("{0} is null", p.ParameterName);
                p.Value = DBNull.Value;
            }

            return p;
        }

        public static SqlParameter FieldEqualsOrNullParamInt(this List<string> filter, string field,
                                                             string parameterName, int? value) {
            SqlParameter p = new SqlParameter(parameterName.AsSqlParamName(), SqlDbType.Int);

            if (value.HasValue) {
                filter.AddFormat("{0} = {1}", field, p.ParameterName);
                p.Value = value.Value;
            } else {
                filter.AddFormat("{0} is null", p.ParameterName);
                p.Value = DBNull.Value;
            }

            return p;
        }

        public static SqlParameter FieldEqualsOrNullParamShort(this List<string> filter, string field,
                                                             string parameterName, short? value) {
            SqlParameter p = new SqlParameter(parameterName.AsSqlParamName(), SqlDbType.SmallInt);

            if (value.HasValue) {
                filter.AddFormat("{0} = {1}", field, p.ParameterName);
                p.Value = value.Value;
            } else {
                filter.AddFormat("{0} is null", p.ParameterName);
                p.Value = DBNull.Value;
            }

            return p;
        }

        public static SqlParameter FieldEqualsOrNullParamBool(this List<string> filter, string field,
                                                              string parameterName, bool? value) {
            SqlParameter p = new SqlParameter(parameterName.AsSqlParamName(), SqlDbType.Bit);

            if (value.HasValue) {
                filter.AddFormat("{0} = {1}", field, p.ParameterName);
                p.Value = value.Value;
            } else {
                filter.AddFormat("{0} is null", p.ParameterName);
                p.Value = DBNull.Value;
            }

            return p;
        }

        public static IEnumerable<SqlParameter> FieldInDateRangeOrNullParams(this List<string> filter, string field,
                                                                             string fromParameterName, DateTime? fromParameterValue, string toParameterName, DateTime? toParameterValue) {
            SqlParameter pFrom = new SqlParameter(fromParameterName.AsSqlParamName(), SqlDbType.Date);
            SqlParameter pTo = new SqlParameter(toParameterName.AsSqlParamName(), SqlDbType.Date);

            if (fromParameterValue.HasValue && toParameterValue.HasValue) {
                filter.AddFormat("cast({0} as date) between {1} and {2}", field, pFrom.ParameterName, pTo.ParameterName);
                pFrom.Value = fromParameterValue.Value.Date;
                pTo.Value = toParameterValue.Value.Date;
            } else if (fromParameterValue.HasValue) {
                filter.AddFormat("(cast({0} as date) >= {1} and {2} is null)", field, pFrom.ParameterName, pTo.ParameterName);
                pFrom.Value = fromParameterValue.Value.Date;
                pTo.Value = DBNull.Value;
            } else if (toParameterValue.HasValue) {
                filter.AddFormat("(cast({0} as date) <= {1} and {2} is null)", field, pTo.ParameterName,
                                 pFrom.ParameterName);
                pFrom.Value = DBNull.Value;
                pTo.Value = toParameterValue.Value.Date;
            } else {
                filter.AddFormat("({0} is null and {1} is null)", pFrom.ParameterName, pTo.ParameterName);
                pFrom.Value = DBNull.Value;
                pTo.Value = DBNull.Value;
            }

            return new[] { pFrom, pTo };
        }

        public static IEnumerable<SqlParameter> FieldInIntRangeOrNullParams(this List<string> filter, string field,
                                                                            string fromParameterName, int? fromParameterValue, string toParameterName, int? toParameterValue) {
            SqlParameter pFrom = new SqlParameter(fromParameterName.AsSqlParamName(), SqlDbType.Int);
            SqlParameter pTo = new SqlParameter(toParameterName.AsSqlParamName(), SqlDbType.Int);

            if (fromParameterValue.HasValue && toParameterValue.HasValue) {
                filter.AddFormat("{0} between {1} and {2}", field, pFrom.ParameterName, pTo.ParameterName);
                pFrom.Value = fromParameterValue.Value;
                pTo.Value = toParameterValue.Value;
            } else if (fromParameterValue.HasValue) {
                filter.AddFormat("({0} >= {1} and {2} is null)", field, pFrom.ParameterName, pTo.ParameterName);
                pFrom.Value = fromParameterValue.Value;
                pTo.Value = DBNull.Value;
            } else if (toParameterValue.HasValue) {
                filter.AddFormat("({0} <= {1} and {2} is null)", field, pTo.ParameterName, pFrom.ParameterName);
                pFrom.Value = DBNull.Value;
                pTo.Value = toParameterValue.Value;
            } else {
                filter.AddFormat("({0} is null and {1} is null)", pFrom.ParameterName, pTo.ParameterName);
                pFrom.Value = DBNull.Value;
                pTo.Value = DBNull.Value;
            }

            return new[] { pFrom, pTo };
        }
        #endregion

        public static string AsSqlParamName(this string source) {
            return source.StartsWith("@") ? source : "@" + source;
        }

        public static void AddFormat(this ICollection<string> list, string format, params object[] args) {
            list.Add(string.Format(format, args));
        }

        public static string JoinAnd(this ICollection<string> list) {
            if (list == null || list.Count == 0) {
                return null;
            }

            return string.Join("\r\nand ", list);
        }

        public static string JoinOr(this ICollection<string> list) {
            if (list == null || list.Count == 0) {
                return null;
            }

            return string.Join("\r\nor ", list);
        }

        /// <summary>
        /// Создает копию инстанса SqlParameter.
        /// <para>Т.к. один и тот же инстанс SqlParameter нельзя использовать более чем в одном SqlCommand, 
        /// то этот метод помогает создать копию параметра используемого в одном запросе для его применения в другом.
        /// </para>
        /// <para>
        /// Копируются следующие свойства SqlParameter:
        ///  ParameterName,
        ///  SqlDbType,
        ///  Size,
        ///  Value,
        ///  Direction
        /// </para>
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static SqlParameter Copy(this SqlParameter source) {
            return new SqlParameter {
                ParameterName = source.ParameterName,
                SqlDbType = source.SqlDbType,
                Size = source.Size,
                Value = source.Value,
                Direction = source.Direction
            };
        }

        public static SqlParameter[] CopyToArray(this IEnumerable<SqlParameter> source) {
            return (source == null ? Enumerable.Empty<SqlParameter>() : source.Select(x => x.Copy())).ToArray();
        }

        public static SelectListItemViewModel<TId> MapSelectListItem<TId>(this SqlDataReader reader) {
            TId id = reader.IsDBNull(0) ? default(TId) : (TId)reader[0];
            return new SelectListItemViewModel<TId>(id, reader.SafeGetString(1));
        }

        public static SelectListItemViewModel<TId> MapSelectListItems<TId>(this SqlDataReader reader)
        {
            TId id = reader.IsDBNull(0) ? default(TId) : (TId)reader[0];
            return new SelectListItemViewModel<TId>(id, reader.SafeGetString(1), (int)reader[2]);
        }

        public static SelectListItemViewModel<TId> MapSelectListItemsс<TId>(this SqlDataReader reader)
        {
            TId id = reader.IsDBNull(0) ? default(TId) : (TId)reader[0];
            return new SelectListItemViewModel<TId>(id, reader.SafeGetString(1), (int)reader[2]);
        }
        public static SelectListItemViewModel<TId> MapSelectListItemsy<TId>(this SqlDataReader reader)
        {
            TId id = reader.IsDBNull(0) ? default(TId) : (TId)reader[0];
            return new SelectListItemViewModel<TId>(id);
        }

        public static string ToInvariantOrNull(this decimal? value) {
            return value.HasValue ? value.Value.ToString(CultureInfo.InvariantCulture) : null;
        }

        public static string ToInvariantRating(this decimal? value) {
            return value.HasValue && value.Value != 0 ? value.Value.ToString(CultureInfo.InvariantCulture) : null;
        }

        /// <summary>
        /// Копирует список идентификаторов типа int во временную таблицу #tempTableName в контексте текущей транзакции
        /// <para>
        ///  Сначала в памяти создается DataTable со списком идентификаторов в колонке columnName, после чего вызывается
        /// <see cref="WriteToTempTable(DataTable)"/>
        /// </para>
        /// </summary>
        /// <param name="id">Список int-значений идентфикаторов</param>
        /// <param name="tableName">Название временной таблицы (#tableName) </param>
        /// <param name="columnName">Название колонки с идентификатором, по-умолчанию "Id"</param>
        public static void WriteToTempTable(this IEnumerable<int> id, string tableName, string columnName = "Id") {
            using (DataTable table = new DataTable(tableName)) {
                table.Columns.Add(columnName, typeof(int)).AllowDBNull = false;
                table.BeginLoadData();

                foreach (var i in id) {
                    table.Rows.Add(i);
                }

                table.EndLoadData();

                WriteToTempTable(table);
            }
        }

        /// <summary>
        /// Копирует DataTable во временную таблицу c названием table.TableName с использованием SqlBulkCopy (т.е. максимально быстро).
        /// <para>Если имя таблицы не начинается с символа "#", то он автоматически добавляется к table.TableName
        /// т.е. если table.TableName = "tempTable" то в БД будет создана таблица "#tempTable"
        /// </para>
        /// </summary>
        /// <param name="table">DataTable содержание которой нужно скопировать во временную таблицу БД.
        /// <para>Если имя таблицы не начинается с символа "#", то он автоматически добавляется к table.TableName
        /// т.е. если table.TableName = "tempTable" то в БД будет создана таблица "#tempTable"
        /// </para>
        /// </param>
        public static void WriteToTempTable(this DataTable table) {
            if (table == null) {
                throw new ArgumentNullException("table");
            }

            if (table.Columns.Count == 0) {
                throw new ArgumentException("Table contains no columns", "table");
            }

            if (string.IsNullOrEmpty(table.TableName)) {
                throw new ArgumentException("Table name unspecified", "table");
            }

            const string createTempQuery = @"
                IF OBJECT_ID('tempdb..{0}') IS NOT NULL 
                    DROP TABLE {0}
                CREATE TABLE {0} ({1})                    
            ";

            string tableName = table.TableName.StartsWith("#") ? table.TableName : "#" + table.TableName;
            string columns = string.Join(", ", table.Columns.OfType<DataColumn>().
                Select(c => string.Format("[{0}] {1} {2}", c.ColumnName, c.SqlDataType(), c.AllowDBNull ? string.Empty : "not null")));
            string createTempQueryText = string.Format(createTempQuery, tableName, columns);

            using (SqlCommand createTempCommand = TransactionManager.CreateCommand(CommandType.Text, createTempQueryText)) {
                createTempCommand.ExecuteNonQuery();
            }

            if (table.Rows.Count > 0) {
                SqlBulkCopy bulk = TransactionManager.CreateBulkCopy();

                try {
                    bulk.DestinationTableName = table.TableName;
                    bulk.WriteToServer(table);
                } finally {
                    bulk.Close();
                }
            }
        }

        /// <summary>
        /// Определяет строковое представление типа данных T-SQL на основе свойства DataColumn.DataType
        /// </summary>
        /// <param name="column">DataColumn для которой требуется определить тип данных T-SQL</param>
        /// <returns>строковое представление типа данных T-SQL, соответствующее column.DataType</returns>
        public static string SqlDataType(this DataColumn column) {
            if (column.DataType == typeof(string)) {
                //return string.Format("varchar({0})", column.MaxLength > 0 ? column.MaxLength : 4000);
                return string.Format("nvarchar(max)");
            }
            if (column.DataType == typeof(int)) {
                return "int";
            }
            if (column.DataType == typeof(short)) {
                return "smallint";
            }
            if (column.DataType == typeof(bool)) {
                return "bit";
            }
            if (column.DataType == typeof(DateTime)) {
                return "datetime";
            }
            if (column.DataType == typeof(decimal)) {
                return string.Format("decimal(15, 8)");
            }

            throw new NotSupportedException(string.Format("Unsupported type: " + column.DataType.Name));
        }

        public static SqlParameter CreateIntParam(string parameterName, int? paramValue) {
            return new SqlParameter(parameterName.AsSqlParamName(), SqlDbType.Int) {
                Value = paramValue.HasValue ? paramValue.Value : (object)DBNull.Value, Size = 10
            };
        }

        public static SqlParameter CreateShortParam(string parameterName, short? paramValue) {
            return new SqlParameter(parameterName.AsSqlParamName(), SqlDbType.SmallInt) {
                Value = paramValue.HasValue ? paramValue.Value : (object)DBNull.Value,
                Size = 5
            };
        }

        public static SqlParameter CreateStringParam(string parameterName, string paramValue, int size = 0) {
            return new SqlParameter(parameterName.AsSqlParamName(), SqlDbType.VarChar) {
                Value = paramValue ?? (object)DBNull.Value,
                Size = size > 0 ? size : 0
            };
        }

        public static SqlParameter CreateStringLikeParam(string parameterName, string paramValue, int size = 0) {
            return new SqlParameter(parameterName.AsSqlParamName(), SqlDbType.VarChar) {
                Value = LikeParamValueOrDBNull(paramValue),
                Size = size > 0 ? size : 0
            };
        }


        public static SqlParameter CreateDateParam(string parameterName, DateTime? paramValue) {
            return new SqlParameter(parameterName.AsSqlParamName(), SqlDbType.DateTime) {
                Value = paramValue.HasValue ? paramValue.Value : (object)DBNull.Value,
                Size = 10
            };
        }

        public static SqlParameter CreateLongParam(string parameterName, long? paramValue) {
            return new SqlParameter(parameterName.AsSqlParamName(), SqlDbType.BigInt) {
                Value = paramValue.HasValue ? paramValue.Value : (object)DBNull.Value,
                Size = 20
            };
        }

        public static SqlParameter CreateBoolParam(string parameterName, bool? paramValue) {
            return new SqlParameter(parameterName.AsSqlParamName(), SqlDbType.Bit) {
                Value = paramValue.HasValue ? paramValue.Value ? 1 : 0 : (object)DBNull.Value,
                Size = 1
            };
        }

        public static short? AsShortValue(this int? intValue) {
            if (intValue.HasValue) {
                return Convert.ToInt16(intValue.Value);
            }

            return null;
        }

        public static int? AsIntValue(this short? shortValue) {
            if (shortValue.HasValue) {
                return Convert.ToInt32(shortValue.Value);
            }

            return null;
        }


    }

}