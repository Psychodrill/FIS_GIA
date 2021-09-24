namespace Ege.Check.Dal.Store.Repositories.Regions
{
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Threading.Tasks;
    using Ege.Check.Logic.Models.Cache;
    using Ege.Check.Logic.Models.Staff;
    using Ege.Check.Logic.Services.Dtos.Enums;
    using Ege.Dal.Common.Mappers;
    using Ege.Dal.Common.Repositories;
    using JetBrains.Annotations;

    internal class RegionSettingsRepository : Repository, IRegionSettingsRepository
    {
        private const string ParticipantProcedureName = "GetRegionExamSettings";
        private const string StaffGetProcedureName = "GetExamSettings";
        private const string GetGekDocumentsProcedureName = "GetGekDocuments";
        private const string SetExamSettingsProcedureName = "MergeExamSettings";
        private const string SetGekDocumentsProcedureName = "MergeGekDocuments";
        private const string DeleteGekDocumentProcedureName = "DeleteGekDocument";

        private const string RegionParameterName = "@RegionId";
        private const string WaveParameterName = "@WaveCode";
        private const string ExamIdParameterName = "@ExamGlobalId";
        private const string NumberParameterName = "@Number";
        private const string CreateDateParameterName = "@CreateDate";
        private const string UrlParameterName = "@Url";
        private const string SettingsParameterName = "@ExamSettings";

        [NotNull] private readonly IDataReaderMapper<GekDocument> _gekDocumentsMapper;
        [NotNull] private readonly IDataReaderMapper<IDictionary<int, RegionSettingsCacheModel>> _mapper;
        [NotNull] private readonly IDataTableMapper<KeyValuePair<int, IEnumerable<ExamSetting>>> _settingsTableMapper;
        [NotNull] private readonly IDataReaderMapper<ExamSettings> _staffSettingsMapper;

        public RegionSettingsRepository(
            [NotNull] IDataReaderMapper<IDictionary<int, RegionSettingsCacheModel>> mapper,
            [NotNull] IDataReaderMapper<ExamSettings> staffSettingsMapper,
            [NotNull] IDataReaderMapper<GekDocument> gekDocumentsMapper,
            [NotNull] IDataTableMapper<KeyValuePair<int, IEnumerable<ExamSetting>>> settingsTableMapper
            )
        {
            _mapper = mapper;
            _staffSettingsMapper = staffSettingsMapper;
            _gekDocumentsMapper = gekDocumentsMapper;
            _settingsTableMapper = settingsTableMapper;
        }

        public async Task<IDictionary<int, RegionSettingsCacheModel>> GetSettingsForParticipant(DbConnection connection)
        {
            var command = StoredProcedureCommand(connection, ParticipantProcedureName);
            command.CommandTimeout = 300;
            using (var reader = await command.ExecuteReaderWithTimeElapsedLogAsync())
            {
                var result = await _mapper.Map(reader);
                return result;
            }
        }

        public async Task<ExamSettings> GetSettingsForStaff(DbConnection connection, int regionId, ExamWave wave)
        {
            var command = StoredProcedureCommand(connection, StaffGetProcedureName);
            AddParameter(command, RegionParameterName, regionId);
            AddParameter(command, WaveParameterName, (int) wave);
            using (var reader = await command.ExecuteReaderAsync())
            {
                var result = await _staffSettingsMapper.Map(reader);
                return result;
            }
        }

        public async Task UpdateSettings(DbConnection connection, int regionId, IEnumerable<ExamSetting> settings)
        {
            var command = StoredProcedureCommand(connection, SetExamSettingsProcedureName);
            var settingsTable =
                _settingsTableMapper.Map(new KeyValuePair<int, IEnumerable<ExamSetting>>(regionId, settings));
            AddParameter(command, SettingsParameterName, settingsTable);
            await command.ExecuteNonQueryAsync();
        }

        public async Task<GekDocument> GetGekDocument(DbConnection connection, int regionId, int examId)
        {
            var command = StoredProcedureCommand(connection, GetGekDocumentsProcedureName);
            AddParameter(command, RegionParameterName, regionId);
            AddParameter(command, ExamIdParameterName, examId);
            using (var reader = await command.ExecuteReaderAsync())
            {
                var result = await _gekDocumentsMapper.Map(reader);
                return result;
            }
        }

        public async Task UpdateGekDocument(DbConnection connection, int regionId, int examId, GekDocument document)
        {
            var command = StoredProcedureCommand(connection, SetGekDocumentsProcedureName);
            AddParameter(command, RegionParameterName, regionId);
            AddParameter(command, ExamIdParameterName, examId);
            AddParameter(command, NumberParameterName, document.Number);
            AddParameter(command, CreateDateParameterName, document.CreateDate);
            AddParameter(command, UrlParameterName, document.Url);
            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteGekDocument(DbConnection connection, int regionId, int examId)
        {
            var command = StoredProcedureCommand(connection, DeleteGekDocumentProcedureName);
            AddParameter(command, RegionParameterName, regionId);
            AddParameter(command, ExamIdParameterName, examId);
            await command.ExecuteNonQueryAsync();
        }
    }
}