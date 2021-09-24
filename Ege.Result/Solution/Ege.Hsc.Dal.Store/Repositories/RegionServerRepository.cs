namespace Ege.Hsc.Dal.Store.Repositories
{
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Threading.Tasks;
    using Ege.Dal.Common.Mappers;
    using Ege.Dal.Common.Repositories;
    using Ege.Hsc.Dal.Entities;
    using Ege.Hsc.Logic.Models.Servers;
    using JetBrains.Annotations;

    class RegionServerRepository : Repository, IRegionServerRepository
    {
        private const string LoadFromCheckEgeProcedureName = "LoadRegions";
        private const string GetServersHavingBlanksProc = "GetServersHavingBlanks";
        private const string GetServersWithBlanksProc = "GetServersWithBlanks";
        private const string UpdateAvailabilityProc = "UpdateServerAvailability";
        private const string UpdateErrorsProc = "UpdateServerErrors";
        private const string GetStatusesProc = "GetServerStatuses";
        private const string GetErrorsProc = "GetServerErrorDetails";
        private const string AvailabilityParam = "Availability";
        private const string RegionIdParam = "RegionId";
        private const string ServerBlankCountParam = "ServerBlankCount";
        private const string ErrorsParam = "Errors";
        private const string IsAvailableParam = "IsAvailable";
        private const string NotProcessedState = "NotProcessedState";
        private const string ErrorState = "ErrorState";

        [NotNull] private readonly IDataReaderCollectionMapper<BlankServerAvailabilityModel> _availabilityModelMapper;
        [NotNull] private readonly IDataReaderCollectionMapper<ServerBlanks> _serverBlanksMapper;
        [NotNull] private readonly IDataTableMapper<IDictionary<int, bool>> _availabilityTableMapper;
        [NotNull] private readonly IDataTableMapper<IEnumerable<ServerErrors>> _errorTableMapper;
        [NotNull] private readonly IDataReaderCollectionMapper<BlankServerStatus> _statusMapper;
        [NotNull] private readonly IDataReaderCollectionMapper<BlankServerError> _errorMapper;

        public RegionServerRepository(
            [NotNull]IDataReaderCollectionMapper<BlankServerAvailabilityModel> availabilityModelMapper, 
            [NotNull]IDataTableMapper<IDictionary<int, bool>> availabilityTableMapper, 
            [NotNull]IDataReaderCollectionMapper<ServerBlanks> serverBlanksMapper, 
            [NotNull]IDataTableMapper<IEnumerable<ServerErrors>> errorTableMapper, 
            [NotNull]IDataReaderCollectionMapper<BlankServerStatus> statusMapper, 
            [NotNull]IDataReaderCollectionMapper<BlankServerError> errorMapper)
        {
            _availabilityModelMapper = availabilityModelMapper;
            _availabilityTableMapper = availabilityTableMapper;
            _serverBlanksMapper = serverBlanksMapper;
            _errorTableMapper = errorTableMapper;
            _statusMapper = statusMapper;
            _errorMapper = errorMapper;
        }

        public async Task LoadFromCheckEgeDb(DbConnection connection)
        {
            var command = StoredProcedureCommand(connection, LoadFromCheckEgeProcedureName);
            command.CommandTimeout = 300;
            await command.ExecuteNonQueryAsync();
        }

        public async Task<ICollection<BlankServerAvailabilityModel>> GetServersHavingBlanks(DbConnection connection)
        {
            var command = StoredProcedureCommand(connection, GetServersHavingBlanksProc);
            using (var reader = await command.ExecuteReaderAsync())
            {
                return await _availabilityModelMapper.Map(reader);
            }
        }

        public async Task UpdateServerAvailability(DbConnection connection, IDictionary<int, bool> availabilityByRegion)
        {
            var command = StoredProcedureCommand(connection, UpdateAvailabilityProc);
            AddParameter(command, AvailabilityParam, _availabilityTableMapper.Map(availabilityByRegion));
            await command.ExecuteNonQueryAsync();
        }

        public async Task<ICollection<ServerBlanks>> GetServersWithBlanks(DbConnection connection, int? regionId)
        {
            var command = StoredProcedureCommand(connection, GetServersWithBlanksProc);
            command.CommandTimeout = 3000;
            AddParameter(command, RegionIdParam, regionId);
            using (var reader = await command.ExecuteReaderAsync())
            {
                return await _serverBlanksMapper.Map(reader);
            }
        }

        public async Task UpdateServerData(DbConnection connection, int regionId, int serverBlankCount, IEnumerable<ServerErrors> errors, bool isAvailable)
        {
            var command = StoredProcedureCommand(connection, UpdateErrorsProc);
            AddParameter(command, RegionIdParam, regionId);
            AddParameter(command, ServerBlankCountParam, serverBlankCount);
            AddParameter(command, ErrorsParam, _errorTableMapper.Map(errors));
            AddParameter(command, IsAvailableParam, isAvailable);
            AddParameter(command, NotProcessedState, (int)BlankDownloadState.Queued);
            AddParameter(command, ErrorState, (int)BlankDownloadState.Error);
            command.CommandTimeout = 600;
            await command.ExecuteNonQueryAsync();
        }
    
        public async Task<ICollection<BlankServerStatus>> GetStatuses(DbConnection connection, int? regionId)
        {
            var command = StoredProcedureCommand(connection, GetStatusesProc);
            AddParameter(command, RegionIdParam, regionId);
            using (var reader = await command.ExecuteReaderAsync())
            {
                return await _statusMapper.Map(reader);
            }
        }

        public async Task<ICollection<BlankServerError>> GetErrors(DbConnection connection, int regionId)
        {
            var command = StoredProcedureCommand(connection, GetErrorsProc);
            AddParameter(command, RegionIdParam, regionId);
            using (var reader = await command.ExecuteReaderAsync())
            {
                return await _errorMapper.Map(reader);
            }
        }
    }
}
