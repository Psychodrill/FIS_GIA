namespace Ege.Check.Dal.Store.Repositories.Answers
{
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Threading.Tasks;
    using Ege.Check.Logic.Models.Cache;
    using Ege.Dal.Common.Mappers;
    using Ege.Dal.Common.Repositories;
    using JetBrains.Annotations;

    internal class AnswerRepository : Repository, IAnswerRepository
    {
        private const string ProcedureName = "GetAnswers";
        private const string CodeParameterName = "@ParticipantCode";
        private const string RegionParameterName = "@RegionId";
        private const string ExamIdParameterName = "@ExamGlobalId";

        [NotNull] private readonly IDataReaderMapper<AnswerCollectionCacheModel> _mapper;

        public AnswerRepository([NotNull] IDataReaderMapper<AnswerCollectionCacheModel> mapper)
        {
            _mapper = mapper;
        }

        public async Task<AnswerCollectionCacheModel> GetExamAnswers(DbConnection connection,
                                                                     KeyValuePair<ParticipantCacheModel, int>
                                                                         participantExam)
        {
            var command = StoredProcedureCommand(connection, ProcedureName);
            AddParameter(command, CodeParameterName, participantExam.Key.Code);
            AddParameter(command, RegionParameterName, participantExam.Key.RegionId);
            AddParameter(command, ExamIdParameterName, participantExam.Value);

            using (var reader = await command.ExecuteReaderWithTimeElapsedLogAsync())
            {
                var result = await _mapper.Map(reader);
                return result;
            }
        }
    }
}