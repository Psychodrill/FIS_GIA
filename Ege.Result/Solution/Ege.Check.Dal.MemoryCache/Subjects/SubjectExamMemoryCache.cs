namespace Ege.Check.Dal.MemoryCache.Subjects
{
    using System.Collections.Generic;
    using Ege.Check.Logic.Models.Cache;
    using JetBrains.Annotations;

    internal class SubjectExamMemoryCache : ISubjectExamMemoryCache
    {
        [NotNull] private static volatile IDictionary<int, ExamMemoryCacheModel> _examCache =
            new Dictionary<int, ExamMemoryCacheModel>();

        [NotNull] private static volatile IDictionary<int, SubjectMemoryCacheModel> _subjectCache =
            new Dictionary<int, SubjectMemoryCacheModel>();

        public ExamMemoryCacheModel GetExam(int examGlobalId)
        {
            ExamMemoryCacheModel result;
            if (!_examCache.TryGetValue(examGlobalId, out result) || result == null)
            {
                result = new ExamMemoryCacheModel {Subject = new SubjectMemoryCacheModel()};
            }
            return result;
        }

        public SubjectMemoryCacheModel GetSubject(int subjectCode)
        {
            SubjectMemoryCacheModel result;
            if (!_subjectCache.TryGetValue(subjectCode, out result) || result == null)
            {
                result = new SubjectMemoryCacheModel();
            }
            return result;
        }

        public void Put(IDictionary<int, SubjectMemoryCacheModel> subjects)
        {
            _subjectCache = subjects;
        }

        public void Put(IDictionary<int, ExamMemoryCacheModel> exams, IDictionary<int, SubjectMemoryCacheModel> subjects)
        {
            _examCache = exams;
            _subjectCache = subjects;
        }

        public IEnumerable<ExamMemoryCacheModel> GetAllExams()
        {
            return _examCache.Values;
        }

        public IEnumerable<SubjectMemoryCacheModel> GetAllSubjects()
        {
            return _subjectCache.Values;
        }
    }
}