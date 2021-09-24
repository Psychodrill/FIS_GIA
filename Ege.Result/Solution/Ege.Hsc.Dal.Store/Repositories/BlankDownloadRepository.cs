namespace Ege.Hsc.Dal.Store.Repositories
{
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using Common.Logging;
    using Ege.Check.Dal.Store.Bulk.Load;
    using Ege.Dal.Common.Mappers;
    using Ege.Dal.Common.Repositories;
    using Ege.Hsc.Dal.Entities;
    using Ege.Hsc.Logic.Models.Blanks;
    using JetBrains.Annotations;

    internal class BlankDownloadRepository : Repository, IBlankDownloadRepository
    {
        private const string GetTopProcedure = "GetTopBlanksDownloadByStatusAndSetStatus";
        private const string LoadFromCheckEgeProcedure = "LoadBlanks";
        private const string GetInconsistenciesProcedure = "GetPageCountInconsistencies";
        private const string FixInconsistenciesProcedure = "FixPageCountInconsistencies";
        private const string SetStateByParticipantAndOrderProcedure = "SetStateByParticipantAndOrder";
        private const string TopParam = "Top";
        private const string NeededStateParam = "NeededState";
        private const string UpdatedStateParam = "UpdatedState";
        private const string BlanksTableName = "BlanksDownload";
        private const string CorrectionsParam = "Corrections";
        private const string StateParam = "State";
        private const string BlanksParam = "Blanks";

        [NotNull] private readonly IDataReaderListMapper<BlankToDownload> _dataReaderCollectionMapper;
        [NotNull] private readonly IDataReaderMapper<LoadedBlanks> _loadedBlankMapper;
        [NotNull] private readonly IBulkLoader _bulkLoader;
        [NotNull] private readonly IDataTableMapper<IEnumerable<BlankDownload>> _dataTableMapper;
        [NotNull] private readonly IDataTableMapper<IEnumerable<DownloadedBlank>> _downloadedBlankMapper;

        [NotNull] private static readonly ILog Logger = LogManager.GetLogger<BlankDownloadRepository>();

        public BlankDownloadRepository(
            [NotNull] IDataReaderListMapper<BlankToDownload> dataReaderCollectionMapper, 
            [NotNull] IDataReaderMapper<LoadedBlanks> loadedBlankMapper, 
            [NotNull] IBulkLoader bulkLoader,
            [NotNull] IDataTableMapper<IEnumerable<BlankDownload>> dataTableMapper, 
            [NotNull] IDataTableMapper<IEnumerable<DownloadedBlank>> downloadedBlankMapper)
        {
            _dataReaderCollectionMapper = dataReaderCollectionMapper;
            _loadedBlankMapper = loadedBlankMapper;
            _bulkLoader = bulkLoader;
            _dataTableMapper = dataTableMapper;
            _downloadedBlankMapper = downloadedBlankMapper;
        }

        public async Task ChangeStateAsync(DbConnection connection, int id, BlankDownloadState state)
        {
            var cmd = connection.CreateCommand();
            cmd.CommandText = "Update BlanksDownload set State = @State where Id = @Id";
            AddParameter(cmd, "State", state);
            AddParameter(cmd, "Id", id);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<IList<BlankToDownload>> TopNotDownloadFromServerAsync(DbConnection connection, int maxCount)
        {
            var cmd = StoredProcedureCommand(connection, GetTopProcedure);
            AddParameter(cmd, TopParam, maxCount);
            AddParameter(cmd, NeededStateParam, (int) BlankDownloadState.Queued);
            AddParameter(cmd, UpdatedStateParam, (int) BlankDownloadState.Downloading);
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                return await _dataReaderCollectionMapper.Map(reader);
            }
        }

        public async Task AddAsync(SqlConnection connection, SqlTransaction transaction, IEnumerable<BlankDownload> blanks)
        {
            var table = _dataTableMapper.Map(blanks);
            await _bulkLoader.LoadDataAsync(table, BlanksTableName, connection, transaction);
        }

        public async Task<LoadedBlanks> LoadFromCheckEgeDb(DbConnection connection, DbTransaction transaction)
        {
            var cmd = StoredProcedureCommand(connection, LoadFromCheckEgeProcedure);
            cmd.Transaction = transaction; 
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                return await _loadedBlankMapper.Map(reader);
            }
        }

        public async Task<LoadedBlanks> GetInconsistenciesWithCheckEgeDb(DbConnection connection)
        {
            var cmd = StoredProcedureCommand(connection, GetInconsistenciesProcedure);
            cmd.CommandTimeout = 300;
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                return await _loadedBlankMapper.Map(reader);
            }
        }

        public async Task<int> FixInconsistenciesWithCheckEgeDb(DbConnection connection, IEnumerable<BlankDownload> blanks, DbTransaction transaction = null)
        {
            Logger.Info("Fixing inconsistencies : creating datatable");
            var table = _dataTableMapper.Map(blanks);
            Logger.Info("Fixing inconsistencies : created datatable");
            var cmd = StoredProcedureCommand(connection, FixInconsistenciesProcedure);
            cmd.CommandTimeout = 3000;
            cmd.Transaction = transaction;
            AddParameter(cmd, CorrectionsParam, table);
            Logger.Info("Fixing inconsistencies : created command");
            return await cmd.ExecuteNonQueryAsync();
        }

        public async Task<int> SetErrorStatus(DbConnection connection, IEnumerable<DownloadedBlank> invalidDownloads)
        {
            var cmd = StoredProcedureCommand(connection, SetStateByParticipantAndOrderProcedure);
            AddParameter(cmd, StateParam, (int) BlankDownloadState.Error);
            AddParameter(cmd, BlanksParam, _downloadedBlankMapper.Map(invalidDownloads));
            return await cmd.ExecuteNonQueryAsync();
        }
    }
}
