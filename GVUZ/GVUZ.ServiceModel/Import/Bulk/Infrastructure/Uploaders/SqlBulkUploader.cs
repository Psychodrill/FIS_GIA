using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using FogSoft.Helpers;
using GVUZ.Model;
using GVUZ.ServiceModel.Import.Bulk.Extensions;
using GVUZ.ServiceModel.Import.Bulk.Model.Results;

namespace GVUZ.ServiceModel.Import.Bulk.Infrastructure.Uploaders
{
    /// <summary>
    /// Загрузчик BULK в SQL
    /// </summary>
    public class SqlBulkUploader
    {
        readonly string _connectionString;
        private readonly int _packageId;
        private readonly string _userLogin;

        public SqlBulkUploader(int packageId, string userLogin)
        {
            //_connectionString = ((EntityConnection)context.Connection).StoreConnection.ConnectionString;
            _connectionString = ConfigurationManager.ConnectionStrings["Main"].ConnectionString;

            _packageId = packageId;
            _userLogin = userLogin;
        }

        static object locker = new object();

        /// <summary>
        /// Загрузка BULK в БД
        /// </summary>
        /// <param name="items">Справочник " bulk таблица - сущности"</param>
        /// <param name="direction">Тип загрузки</param>
        /// <returns>Результат загрузки</returns>
        public TResult UploadSafety<TResult>(IDictionary<string, IDataReader> items, BulkImportDirection direction)
            where TResult : class, IEmptyResult, new()
        {
            lock (locker)
            {
                return Upload<TResult>(items, direction);
            }
        }

        public TResult Upload<TResult>(IDictionary<string, IDataReader> items, BulkImportDirection direction)
            where TResult : class, IEmptyResult, new()
        {
            var result = new TResult();
            var sw = new Stopwatch();
            
            try
            {
                sw.Start();
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                    {
                       
                        using (var bulk = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
                        {
                            /* Последовательная отгрузка BULK сериализованных коллекций в БД */
                            foreach (var collection in items)
                            {
                                bulk.DestinationTableName = collection.Key;
                                bulk.WriteToServer(collection.Value);
                            }

                            var command = new SqlCommand(direction.GetBulkedProcessQuery(), connection, transaction);
                            command.Parameters.Add(new SqlParameter("packageId", _packageId));
                            command.Parameters.Add(new SqlParameter("userLogin", _userLogin));
                            command.CommandTimeout = EntitiesHelper.CommandTimeout;

                            result = command.ExecuteXmlToObject<TResult>();
                            transaction.Commit();
                        }
                    }
                }

                sw.Stop();
                LogHelper.Log.InfoFormat("Пакет {1}. Время транзакции импорта заявлений = {0} сек",
                    sw.Elapsed.TotalSeconds, _packageId);

            }
            catch (Exception ex)
            {
                LogHelper.Log.Error(string.Format("Ошибка загрузки заявлений из пакета {0}. Ошибка: {1}", _packageId, ex.Message), ex);
                throw;
            }

            return result;
        }
    }
}
