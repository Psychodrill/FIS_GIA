using System.Collections.Generic;
using Dapper;
using System.Data;
using System.IO;
using GVUZ.DAL.Dto;
using System;
using System.Data.SqlClient;
using System.Linq;

namespace GVUZ.DAL.Dapper
{
    public static class DapperHelper
    {
        public static IEnumerable<T> Query<T>(this IDbTransaction tx, string sql, object param = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            return tx.Connection.Query<T>(sql, param, tx, buffered, commandTimeout, commandType);
        }

        public static int Execute(this IDbTransaction tx, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return tx.Connection.Execute(sql, param, tx, commandTimeout, commandType);
        }

        public static byte[] GetBytes(this Stream input)
        {
            using (var ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }

        public static int InsertAttachment(this IDbTransaction tx, AttachmentCreateDto dto)
        {
            var insert = new
            {
                FileId = Guid.NewGuid(),
                FileName = Path.GetFileName(dto.FileName),
                DisplayName = string.IsNullOrWhiteSpace(dto.DisplayName) ? Path.GetFileName(dto.FileName) : dto.DisplayName,
                dto.ContentType,
                dto.ContentLength,
                Content = dto.Content.GetBytes()
            };

            return tx.Connection.ExecuteScalar<int>(SQLQuery.InsertAttachment, insert, tx);
        }

        public static void WriteTempTable(this IDbTransaction tx, DataTable table)
        {
            WriteTempTable(tx as SqlTransaction, table);
        }

        public static void WriteTempTable(this SqlTransaction tx, DataTable table)
        {
            if (tx == null)
            {
                throw new ArgumentNullException("tx");
            }

            if (table == null)
            {
                throw new ArgumentNullException("table");
            }

            if (table.Columns.Count == 0)
            {
                throw new ArgumentException("Table contains no columns", "table");
            }

            if (string.IsNullOrEmpty(table.TableName))
            {
                throw new ArgumentException("Table name unspecified", "table");
            }

            const string createTempQuery = @"
                IF OBJECT_ID('tempdb..{0}') IS NOT NULL 
                    DROP TABLE {0}
                CREATE TABLE {0} ({1})                    
            ";

            string tableName = table.TableName.StartsWith("#") ? table.TableName : "#" + table.TableName;
            string columns = string.Join(", ", table.Columns.OfType<DataColumn>().Select(c => string.Format("[{0}] {1} {2}", c.ColumnName, c.SqlDataType(), c.AllowDBNull ? string.Empty : "not null")));
            string createTempQueryText = string.Format(createTempQuery, tableName, columns);

            using (var createTempCommand = tx.Connection.CreateCommand())
            {
                createTempCommand.CommandType = CommandType.Text;
                createTempCommand.CommandText = createTempQueryText;
                createTempCommand.Transaction = tx;
                createTempCommand.ExecuteNonQuery();
            }

            if (table.Rows.Count > 0)
            {
                SqlBulkCopy bulk = new SqlBulkCopy(tx.Connection, SqlBulkCopyOptions.Default, tx);
                
                try
                {
                    bulk.DestinationTableName = tableName;
                    bulk.WriteToServer(table);
                }
                finally
                {
                    bulk.Close();
                }
            }
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
        public static void WriteToTempTable(this IDbTransaction tx, IEnumerable<int> id, string tableName, string columnName = "Id")
        {
            using (DataTable table = new DataTable(tableName))
            {
                table.Columns.Add(columnName, typeof(int)).AllowDBNull = false;
                table.BeginLoadData();

                foreach (var i in (id ?? Enumerable.Empty<int>()))
                {
                    table.Rows.Add(i);
                }

                table.EndLoadData();

                WriteTempTable(tx, table);
            }
        }

        /// <summary>
        /// Определяет строковое представление типа данных T-SQL на основе свойства DataColumn.DataType
        /// </summary>
        /// <param name="column">DataColumn для которой требуется определить тип данных T-SQL</param>
        /// <returns>строковое представление типа данных T-SQL, соответствующее column.DataType</returns>
        public static string SqlDataType(this DataColumn column)
        {
            if (column.DataType == typeof(string))
            {
                return string.Format("varchar({0})", column.MaxLength > 0 ? column.MaxLength : 4000);
            }
            if (column.DataType == typeof(int))
            {
                return "int";
            }
            if (column.DataType == typeof(short))
            {
                return "smallint";
            }
            if (column.DataType == typeof(bool))
            {
                return "bit";
            }
            if (column.DataType == typeof(DateTime))
            {
                return "datetime";
            }
            if (column.DataType == typeof(decimal))
            {
                return string.Format("decimal(15, 8)");
            }

            throw new NotSupportedException(string.Format("Unsupported type: " + column.DataType.Name));
        }

        private const string sortRuleTemplate = "[{0}] {1}";
        private const string sortAsc = "ASC";
        private const string sortDesc = "DESC";

        public static string Rule(this ISortable sortable)
        {   
            return string.Format(sortRuleTemplate, sortable.SortExpression, sortable.SortDescending ? sortDesc: sortAsc);
        }

        private const string orderByPlaceholder = "@@ORDERBY@@";

        public static string OrderBy(this string query, ISortable sortable)
        {
            return query.Replace(orderByPlaceholder, Rule(sortable));    
        }
    }
}
