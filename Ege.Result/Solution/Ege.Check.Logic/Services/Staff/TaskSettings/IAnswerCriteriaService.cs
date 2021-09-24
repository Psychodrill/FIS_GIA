namespace Ege.Check.Logic.Services.Staff.TaskSettings
{
    using System.Threading.Tasks;
    using Ege.Check.Logic.Models.Cache;
    using JetBrains.Annotations;

    public interface IAnswerCriteriaService
    {
        [NotNull]
        Task<ExamInfoCacheModel> GetTaskSettings(int subjectCode);

        [NotNull]
        Task SetTaskSettings(int subjectCode, ExamInfoCacheModel model);
    }
}