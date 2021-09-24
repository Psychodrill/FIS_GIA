namespace Ege.Check.Logic.Services.Staff.Exams
{
    using System.Collections.Generic;
    using Ege.Check.Logic.Models.Staff;

    public interface ISubjectExamService
    {
        IEnumerable<ExamList> GetAllExams();

        IEnumerable<Subject> GetAllSubjects();
    }
}