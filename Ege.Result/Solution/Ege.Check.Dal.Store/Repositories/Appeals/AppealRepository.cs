namespace Ege.Check.Dal.Store.Repositories.Appeals
{
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Threading.Tasks;
    using Ege.Check.Logic.Models.Cache;
    using Ege.Dal.Common.Mappers;
    using Ege.Dal.Common.Repositories;
    using JetBrains.Annotations;

    internal class AppealRepository : Repository, IAppealRepository
    {
        private const string ProcedureName = "GetAppeals";
        private const string CodeParameterName = "@ParticipantCode";
        private const string RegionParameterName = "@RegionId";
        private const string ExamIdParameterName = "@ExamGlobalId";

        [NotNull] private readonly IDataReaderMapper<AppealCollectionCacheModel> _mapper;

        public AppealRepository([NotNull] IDataReaderMapper<AppealCollectionCacheModel> mapper)
        {
            _mapper = mapper;
        }

        public async Task<AppealCollectionCacheModel> GetAppeals(DbConnection connection,
                                                                 KeyValuePair<ParticipantCacheModel, int>
                                                                     participantExam)
        {
            var command = StoredProcedureCommand(connection, ProcedureName);
            AddParameter(command, CodeParameterName, participantExam.Key.Code);
            AddParameter(command, RegionParameterName, participantExam.Key.RegionId);
            AddParameter(command, ExamIdParameterName, participantExam.Value);

            using (var reader = await command.ExecuteReaderAsync())
            {
                var result = await _mapper.Map(reader);
                return result;
            }
        }
    }
}