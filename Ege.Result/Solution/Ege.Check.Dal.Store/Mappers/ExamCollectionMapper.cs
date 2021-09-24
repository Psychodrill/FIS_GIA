namespace Ege.Check.Dal.Store.Mappers
{
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Threading.Tasks;
    using Ege.Check.Logic.Models.Cache;
    using Ege.Dal.Common;
    using Ege.Dal.Common.Mappers;

    internal class ExamCollectionMapper : DataReaderMapper<ExamCollectionCacheModel>
    {
        private const string ExamId = "ExamGlobalId";
        private const string ExamDate = "ExamDate";
        private const string Subject = "SubjectName";
        private const string TestMark = "TestMark";
        private const string Mark5 = "Mark5";
        private const string MinMark = "MinValue";
        private const string Status = "ProcessCondition";
        private const string HasAppeal = "HasAppeal";
        private const string IsHidden = "IsHidden";
        private const string HasResult = "HasResult";
        private const string AppealStatus = "AppealStatus";
        private const string IsBasicMath = "IsBasicMath";
        private const string IsForeignLanguage = "IsForeignLanguage";
        private const string IsComposition = "IsComposition";

        public override async Task<ExamCollectionCacheModel> Map(DbDataReader @from)
        {
            var examId = GetOrdinal(from, ExamId);
            var examDate = GetOrdinal(from, ExamDate);
            var subject = GetOrdinal(from, Subject);
            var testMark = GetOrdinal(from, TestMark);
            var mark5 = GetOrdinal(from, Mark5);
            var minMark = GetOrdinal(from, MinMark);
            var status = GetOrdinal(from, Status);
            var hasAppeal = GetOrdinal(from, HasAppeal);
            var isHidden = GetOrdinal(from, IsHidden);
            var hasResult = GetOrdinal(from, HasResult);
            var appealStatus = GetOrdinal(from, AppealStatus);
            var isBasicMath = GetOrdinal(from, IsBasicMath);
            var isForeignLanguage = GetOrdinal(from, IsForeignLanguage);
            var isComposition = GetOrdinal(from, IsComposition);
            var oralExamId = GetOrdinal(from, "OralExamGlobalId");
            var hasOralResult = GetOrdinal(from, "HasOralResult");
            var oralStatus = GetOrdinal(from, "OralProcessCondition");

            var exams = new List<ExamCacheModel>();

            while (await from.ReadAsync())
            {
                exams.Add(new ExamCacheModel
                    {
                        ExamId = from.GetInt32(examId),
                        ExamDate = from.GetDateTime(examDate),
                        Subject = from.GetString(subject),
                        TestMark = from.GetInt32(testMark),
                        Mark5 = from.GetInt32(mark5),
                        MinMark = from.GetInt32(minMark),
                        Status = from.GetInt32(status),
                        HasAppeal = from.GetBoolean(hasAppeal),
                        IsHidden = from.GetBoolean(isHidden),
                        HasResult = from.GetBoolean(hasResult),
                        AppealStatus = await from.GetNullableInt32Async(appealStatus),
                        IsBasicMath = from.GetBoolean(isBasicMath),
                        IsComposition = from.GetBoolean(isComposition),
                        IsForeignLanguage = from.GetBoolean(isForeignLanguage),
                        OralExamId = await from.GetNullableInt32Async(oralExamId),
                        HasOralResult = from.GetBoolean(hasOralResult),
                        OralStatus = await from.GetNullableInt32Async(oralStatus),
                    });
            }
            var result = new ExamCollectionCacheModel
                {
                    Exams = exams,
                };
            return result;
        }
    }
}