namespace Ege.Check.Dal.Store.Repositories.Exams
{
    using System.Data.Common;
    using System.Threading.Tasks;
    using Ege.Check.Logic.Models.Cache;
    using Ege.Dal.Common.Mappers;
    using Ege.Dal.Common.Repositories;
    using JetBrains.Annotations;

    internal class ParticipantExamRepository : Repository, IParticipantExamRepository
    {
        private const string GetProcedureName = "GetExams";
        private const string CodeParameterName = "@ParticipantCode";
        private const string RegionParameterName = "@RegionId";

        [NotNull] private readonly IDataReaderMapper<ExamCollectionCacheModel> _mapper;

        public ParticipantExamRepository([NotNull] IDataReaderMapper<ExamCollectionCacheModel> mapper)
        {
            _mapper = mapper;
        }

        public async Task<ExamCollectionCacheModel> GetByParticipant(DbConnection connection,
                                                                     ParticipantCacheModel participant)
        {
            var command = StoredProcedureCommand(connection, GetProcedureName);
            AddParameter(command, CodeParameterName, participant.Code);
            AddParameter(command, RegionParameterName, participant.RegionId);

            using (var reader = await command.ExecuteReaderWithTimeElapsedLogAsync())
            {
                var result = await _mapper.Map(reader);
                return result;
            }
        }
    }
}