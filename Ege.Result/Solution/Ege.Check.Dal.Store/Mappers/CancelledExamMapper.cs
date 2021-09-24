namespace Ege.Check.Dal.Store.Mappers
{
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Threading.Tasks;
    using Ege.Check.Logic.Models.Staff;
    using Ege.Dal.Common.Mappers;

    internal class CancelledExamMapper : DataReaderMapper<CancelledExamsPage>
    {
        private const string Count = "RecordCount";
        private const string Code = "ParticipantCode";
        private const string ExamId = "ExamGlobalId";
        private const string ExamDate = "ExamDate";
        private const string SubjectName = "SubjectName";
        private const string RegionName = "RegionName";
        private const string RegionId = "RegionId";

        public override async Task<CancelledExamsPage> Map(DbDataReader @from)
        {
            var result = new CancelledExamsPage
                {
                    Page = new List<CancelledExam>(),
                };

            if (!await from.ReadAsync())
            {
                return result;
            }

            var count = GetOrdinal(from, Count);
            var code = GetOrdinal(from, Code);
            var examId = GetOrdinal(from, ExamId);
            var examDate = GetOrdinal(from, ExamDate);
            var subjectName = GetOrdinal(from, SubjectName);
            var regionName = GetOrdinal(from, RegionName);
            var regionId = GetOrdinal(from, RegionId);

            result.Count = from.GetInt32(count);

            do
            {
                result.Page.Add(new CancelledExam
                    {
                        Code = from.GetString(code),
                        Date = from.GetDateTime(examDate),
                        SubjectName = from.GetString(subjectName),
                        ExamGlobalId = from.GetInt32(examId),
                        RegionName = from.GetString(regionName),
                        RegionId = from.GetInt32(regionId),
                    });
            } while (await from.ReadAsync());
            return result;
        }
    }
}