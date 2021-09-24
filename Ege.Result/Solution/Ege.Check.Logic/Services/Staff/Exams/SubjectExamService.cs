namespace Ege.Check.Logic.Services.Staff.Exams
{
    using System.Collections.Generic;
    using System.Linq;
    using Ege.Check.Dal.MemoryCache.Subjects;
    using Ege.Check.Logic.Models.Staff;
    using JetBrains.Annotations;

    internal class SubjectExamService : ISubjectExamService
    {
        [NotNull] private readonly ISubjectExamMemoryCache _cache;

        public SubjectExamService([NotNull] ISubjectExamMemoryCache cache)
        {
            _cache = cache;
        }

        public IEnumerable<ExamList> GetAllExams()
        {
            return _cache.GetAllExams()
                         .Where(e => e != null)
                         .GroupBy(e => e.Date)
                         .Select(g => new ExamList
                             {
                                 Date = g.Key,
                                 Exams = g.Select(e => new ExamListElement
                                     {
                                         ExamGlobalId = e.Id,
                                         SubjectName = e.Subject.SubjectName,
                                     }),
                             })
                         .OrderBy(l => l.Date);
        }

        public IEnumerable<Subject> GetAllSubjects()
        {
            return _cache.GetAllSubjects()
                         .Where(s => s != null && !s.IsOral)
                         .Select(s => new Subject
                             {
                                 SubjectCode = s.SubjectCode,
                                 SubjectName = s.SubjectName,
                             });
        }
    }
}
