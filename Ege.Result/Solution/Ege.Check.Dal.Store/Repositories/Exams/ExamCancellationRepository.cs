namespace Ege.Check.Dal.Store.Repositories.Exams
{
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Threading.Tasks;
    using Ege.Check.Logic.Models.Staff;
    using Ege.Dal.Common.Mappers;
    using Ege.Dal.Common.Repositories;
    using JetBrains.Annotations;

    internal class ExamCancellationRepository : Repository, IExamCancellationRepository
    {
        private const string GetCancelledProcedureName = "GetCancelledExams";
        private const string GetCancelledParticipantExamsProcedureName = "GetCancelledParticipantExams";
        private const string SetCancellationProcedureName = "SetExamResultCancellation";
        private const string CodeParameterName = "@ParticipantCode";
        private const string RegionParameterName = "@RegionId";
        private const string ExamIdParameterName = "@ExamGlobalId";
        private const string IsCancelledParameterName = "@IsCancelled";
        private const string SkipParameterName = "@Skip";
        private const string TakeParameterName = "@Take";

        [NotNull]
        private readonly IDataReaderMapper<CancelledExamsPage> _mapper;
        [NotNull]
        private readonly IDataReaderCollectionMapper<CancelledParticipantExam> _participantExamMapper;

        public ExamCancellationRepository(
            [NotNull] IDataReaderMapper<CancelledExamsPage> mapper,
            [NotNull] IDataReaderCollectionMapper<CancelledParticipantExam> participantExamMapper)
        {
            _mapper = mapper;
            _participantExamMapper = participantExamMapper;
        }

        public async Task<CancelledExamsPage> GetCancelled([NotNull] DbConnection connection, int? regionId, int take,
                                                           int skip)
        {
            var command = StoredProcedureCommand(connection, GetCancelledProcedureName);
            AddParameter(command, RegionParameterName, regionId);
            AddParameter(command, SkipParameterName, skip);
            AddParameter(command, TakeParameterName, take);
            using (var reader = await command.ExecuteReaderAsync())
            {
                var result = await _mapper.Map(reader);
                return result;
            }
        }

        public async Task<bool> SetCancellation([NotNull] DbConnection connection, string participantCode, int regionId,
                                                int examId, bool isCancelled)
        {
            var command = StoredProcedureCommand(connection, SetCancellationProcedureName);
            AddParameter(command, CodeParameterName, participantCode);
            AddParameter(command, RegionParameterName, regionId);
            AddParameter(command, ExamIdParameterName, examId);
            AddParameter(command, IsCancelledParameterName, isCancelled);
            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

        public async Task<ICollection<CancelledParticipantExam>> GetCancelledParticipanExams(DbConnection connection)
        {
            var command = StoredProcedureCommand(connection, GetCancelledParticipantExamsProcedureName);
            using (var reader = await command.ExecuteReaderAsync())
            {
                var result = await _participantExamMapper.Map(reader);
                return result;
            }
        }
    }
}