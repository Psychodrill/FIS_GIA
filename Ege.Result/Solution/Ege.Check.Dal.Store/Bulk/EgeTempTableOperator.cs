namespace Ege.Check.Dal.Store.Bulk
{
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Threading.Tasks;
    using Ege.Check.Common.Async;
    using Ege.Check.Common.Config;
    using JetBrains.Annotations;
    using global::Common.Logging;

    internal class EgeTempTableOperator : IEgeTempTableOperator
    {
        [NotNull] private readonly ILog _log;
        [NotNull] private readonly ITableSqlGenerator _tableSqlGenerator;

        private readonly bool _suppressTableDeletion;
        public const string SuppressTemporaryTableDeletionSetting = "SuppressTemporaryTableDeletion";

        public EgeTempTableOperator([NotNull] ITableSqlGenerator tableSqlGenerator, [NotNull] IConfigReaderHelper reader)
        {
            _tableSqlGenerator = tableSqlGenerator;
            _log = LogManager.GetLogger(GetType());

            _suppressTableDeletion = reader.GetInt(SuppressTemporaryTableDeletionSetting, "", 0) != 0;
        }

        public async Task<IEgeTempTable> CreateAsync(Guid id, DataTable dt, DbConnection connection,
                                                     DbTransaction transaction = null)
        {
            var tmpTable = new EgeTempTable(dt.TableName, id, _suppressTableDeletion, this, connection, transaction);
            _log.TraceFormat("Create descriptor {0}", tmpTable.FullTableName);
            var cmd = connection.CreateCommand();
            cmd.Transaction = transaction;
            var commandText = _tableSqlGenerator.CreateSql(dt, tmpTable.FullTableName);
            _log.TraceFormat("Table create sql:\r\n {0}", commandText);
            cmd.CommandText = commandText;
            await cmd.ExecuteNonQueryAsync();
            _log.TraceFormat("Create table for descriptor {0}", tmpTable.FullTableName);
            return tmpTable;
        }

        private async Task DropAsync([NotNull] IEgeTempTable table, [NotNull] DbConnection connection,
                                     DbTransaction transaction = null)
        {
            var cmd = connection.CreateCommand();
            cmd.Transaction = transaction;
            cmd.CommandText = _tableSqlGenerator.DropSql(table.FullTableName);
            await cmd.ExecuteNonQueryAsync();
            _log.TraceFormat("Dropped table for descriptor {0}", table.FullTableName);
        }

        private sealed class EgeTempTable : IEgeTempTable
        {
            [NotNull] private readonly DbConnection _connection;
            [NotNull] private readonly string _fullTableName;
            [NotNull] private readonly ILog _log;

            [NotNull] private readonly EgeTempTableOperator _tableOperator;
            private readonly DbTransaction _transaction;
            private readonly bool _suppressTableDeletion;

            public EgeTempTable(string tableName, Guid tableId, bool suppressTableDeletion, [NotNull] EgeTempTableOperator tableOperator,
                                [NotNull] DbConnection connection, DbTransaction transaction = null)
            {
                if (string.IsNullOrWhiteSpace(tableName))
                {
                    throw new ArgumentNullException("tableName", "Table name can not be null");
                }
                if (tableOperator == null)
                {
                    throw new ArgumentNullException("tableOperator", "tableOperator can not be null");
                }
                _tableOperator = tableOperator;
                _connection = connection;
                _transaction = transaction;
                _suppressTableDeletion = suppressTableDeletion;
                _fullTableName = string.Format("{0}_{1}", tableName, tableId);
                _log = LogManager.GetLogger(GetType());
            }

            public string FullTableName
            {
                get { return _fullTableName; }
            }

            public void Dispose()
            {
                _log.TraceFormat("Disposing table descriptor {0}", FullTableName);
                if (!_suppressTableDeletion)
                {
                    AsyncHelper.RunSync(() => _tableOperator.DropAsync(this, _connection, _transaction));
                }
                _log.TraceFormat("Disposed table descriptor {0}", FullTableName);
            }
        }
    }
}