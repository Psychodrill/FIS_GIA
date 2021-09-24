namespace Ege.Check.Dal.Store.Mappers.Staff
{
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Threading.Tasks;
    using Ege.Check.Logic.Models.Cache;
    using Ege.Dal.Common;
    using Ege.Dal.Common.Mappers;

    internal class ExamListMapper :
        DataReaderMapper
            <KeyValuePair<IDictionary<int, SubjectMemoryCacheModel>, IDictionary<int, ExamMemoryCacheModel>>>
    {
        private const string RowType = "RowType";
        private const string ExamId = "ExamGlobalId";
        private const string ExamDate = "ExamDate";
        private const string SubjectCode = "SubjectCode";
        private const string SubjectName = "SubjectName";
        private const string MinValue = "MinValue";
        private const string IsComposition = "IsComposition";
        private const string IsBasicMath = "IsBasicMath";
        private const string IsForeignLanguage = "IsForeignLanguage";

        public override async
            Task<KeyValuePair<IDictionary<int, SubjectMemoryCacheModel>, IDictionary<int, ExamMemoryCacheModel>>> Map(
            DbDataReader @from)
        {
            var rowType = GetOrdinal(from, RowType);
            var examIdOrdinal = GetOrdinal(from, ExamId);
            var examDateOrdinal = GetOrdinal(from, ExamDate);
            var subjectCodeOrdinal = GetOrdinal(from, SubjectCode);
            var subjectNameOrdinal = GetOrdinal(from, SubjectName);
            var minValueOrdinal = GetOrdinal(from, MinValue);
            var isCompositionOrdinal = GetOrdinal(from, IsComposition);
            var isBasicMathOrdinal = GetOrdinal(from, IsBasicMath);
            var isForeignLanguageOrdinal = GetOrdinal(from, IsForeignLanguage);
            var isOralOrdinal = GetOrdinal(from, "IsOral");
            var subjectDisplayNameOrdinal = GetOrdinal(from, "SubjectDisplayName");
            var writtenSubjectCodeOrdinal = GetOrdinal(from, "WrittenSubjectCode");

            var subjects = new Dictionary<int, SubjectMemoryCacheModel>();
            var exams = new Dictionary<int, ExamMemoryCacheModel>();

            while (await from.ReadAsync())
            {
                if (!from.GetBoolean(rowType))
                {
                    var subjectCode = from.GetInt32(subjectCodeOrdinal);
                    subjects.Add(subjectCode, new SubjectMemoryCacheModel
                        {
                            SubjectCode = subjectCode,
                            SubjectName = from.GetString(subjectNameOrdinal),
                            IsBasicMath = from.GetBoolean(isBasicMathOrdinal),
                            IsComposition = from.GetBoolean(isCompositionOrdinal),
                            IsForeignLanguage = from.GetBoolean(isForeignLanguageOrdinal),
                            MinValue = from.GetInt32(minValueOrdinal),
                            IsOral = from.GetBoolean(isOralOrdinal),
                            SubjectDisplayName = from.GetString(subjectDisplayNameOrdinal),
                            WrittenSubjectCode = await from.GetNullableInt32Async(writtenSubjectCodeOrdinal),
                        });
                }
                else
                {
                    var examId = from.GetInt32(examIdOrdinal);
                    var exam = new ExamMemoryCacheModel
                        {
                            Id = examId,
                            Date = from.GetDateTime(examDateOrdinal),
                            SubjectCode = from.GetInt32(subjectCodeOrdinal),
                        };
                    SubjectMemoryCacheModel subject;
                    subjects.TryGetValue(exam.SubjectCode, out subject);
                    exam.Subject = subject ?? new SubjectMemoryCacheModel();
                    exams.Add(examId, exam);
                }
            }

            return new KeyValuePair<IDictionary<int, SubjectMemoryCacheModel>, IDictionary<int, ExamMemoryCacheModel>>(subjects, exams);
        }
    }
}