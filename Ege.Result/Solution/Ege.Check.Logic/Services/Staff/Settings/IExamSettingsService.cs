namespace Ege.Check.Logic.Services.Settings
{
    using System.Threading.Tasks;
    using Ege.Check.Logic.Models.Staff;
    using JetBrains.Annotations;

    public interface IExamSettingsService
    {
        [NotNull]
        Task<ExamSettings> GetSettingsByRegion(int regionId, ExamWave wave);

        [NotNull]
        Task SetSettings(int regionId, ExamSettings settings);

        [NotNull]
        Task<GekDocument> GetGekDocument(int regionId, int examId);

        [NotNull]
        Task UpdateGekDocument(int regionId, int examId, GekDocument document);
    }
}
