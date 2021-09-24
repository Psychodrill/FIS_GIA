namespace Ege.Check.Dal.Store.Mappers
{
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using global::Ege.Check.Logic.Models.Staff;
    using global::Ege.Dal.Common.Mappers;

    internal class CancelledParticipantExamCollectionMapper : IDataReaderCollectionMapper<CancelledParticipantExam>
    {
        [NotNull] private const string Code = "ParticipantCode";
        [NotNull] private const string RegionId = "RegionId";
        [NotNull] private const string ExamId = "ExamGlobalId";

        public async Task<ICollection<CancelledParticipantExam>> Map(DbDataReader @from)
        {

            if (from == null)
            {
                return new CancelledParticipantExam[0];
            }
            var result = new List<CancelledParticipantExam>();

            var code = from.GetOrdinal(Code);
            var regionId = from.GetOrdinal(RegionId);
            var examId = from.GetOrdinal(ExamId);

            while (await from.ReadAsync())
            {
                result.Add(new CancelledParticipantExam
                    {
                        Code = from.GetString(code),
                        RegionId = from.GetInt32(regionId),
                        ExamGlobalId = from.GetInt32(examId),
                    });
            }
            return result;
        }
    }
}
