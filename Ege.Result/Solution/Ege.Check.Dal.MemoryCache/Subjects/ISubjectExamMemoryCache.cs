namespace Ege.Check.Dal.MemoryCache.Subjects
{
    using System.Collections.Generic;
    using Ege.Check.Logic.Models.Cache;
    using JetBrains.Annotations;

    public interface ISubjectExamMemoryCache
    {
        [NotNull]
        ExamMemoryCacheModel GetExam(int examGlobalId);

        [NotNull]
        IEnumerable<ExamMemoryCacheModel> GetAllExams();

        [NotNull]
        SubjectMemoryCacheModel GetSubject(int subjectCode);

        [NotNull]
        IEnumerable<SubjectMemoryCacheModel> GetAllSubjects();

        void Put([NotNull] IDictionary<int, SubjectMemoryCacheModel> subjects);

        void Put([NotNull] IDictionary<int, ExamMemoryCacheModel> exams,
                 [NotNull] IDictionary<int, SubjectMemoryCacheModel> subjects);
    }
}