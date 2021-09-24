namespace Ege.Check.Dal.Store.Repositories.Answers
{
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Threading.Tasks;
    using Ege.Check.Dal.Store.Mappers;
    using Ege.Check.Logic.Models.Cache;
    using Ege.Dal.Common.Mappers;
    using Ege.Dal.Common.Repositories;
    using JetBrains.Annotations;

    internal class AnswerCriteriaRepository : Repository, IAnswerCriteriaRepository
    {
        private const string GetProcedureName = "GetAnswersCriteria";
        private const string UpdateProcedureName = "MergeAnswersCriteria";
        private const string SubjectCodeParameterName = "@SubjectCode";
        private const string SettingsParameterName = "@Settings";

        [NotNull] private readonly IDataReaderMapper<IDictionary<int, ExamInfoCacheModel>> _allInfoMapper;

        [NotNull] private readonly IDataReaderMapper<ExamInfoCacheModel> _mapper;
        [NotNull] private readonly IDataTableMapper<ExamInfoCacheModel> _tableMapper;

        public AnswerCriteriaRepository(
            [NotNull] IDataReaderMapper<ExamInfoCacheModel> mapper,
            [NotNull] IDataTableMapper<ExamInfoCacheModel> tableMapper,
            [NotNull] IDataReaderMapper<IDictionary<int, ExamInfoCacheModel>> allInfoMapper)
        {
            _mapper = mapper;
            _tableMapper = tableMapper;
            _allInfoMapper = allInfoMapper;
        }

        public async Task<ExamInfoCacheModel> GetByCode(DbConnection connection, int subjectCode)
        {
            var command = StoredProcedureCommand(connection, GetProcedureName);
            AddParameter(command, SubjectCodeParameterName, subjectCode);

            using (var reader = await command.ExecuteReaderWithTimeElapsedLogAsync())
            {
                var result = await _mapper.Map(reader);
                return result;
            }
        }

        public async Task SetByCode(DbConnection connection, int subjectCode, ExamInfoCacheModel settings)
        {
            var command = StoredProcedureCommand(connection, UpdateProcedureName);
            AddParameter(command, SubjectCodeParameterName, subjectCode);
            AddParameter(command, SettingsParameterName, _tableMapper.Map(settings));
            await command.ExecuteNonQueryAsync();
        }

        public async Task<IDictionary<int, ExamInfoCacheModel>> GetAll(DbConnection connection)
        {
            var command = StoredProcedureCommand(connection, GetProcedureName);
            command.CommandTimeout = 300;
            using (var reader = await command.ExecuteReaderAsync())
            {
                var result = await _allInfoMapper.Map(reader);
                return result;
            }
        }
    }
}