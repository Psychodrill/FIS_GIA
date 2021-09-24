namespace Ege.Check.Logic.Services.Staff.Settings
{
    using System.Threading.Tasks;
    using Ege.Check.Logic.Models.Staff;
    using Ege.Check.Logic.Services.Dtos.Enums;
    using JetBrains.Annotations;

    public interface IExamSettingService
    {
        [NotNull]
        Task<ExamSettings> GetSettingsByRegion(int regionId, ExamWave wave);

        [NotNull]
        Task SetSettings(int regionId, [NotNull] ExamSettings settings);

        [NotNull]
        Task<GekDocument> GetGekDocument(int regionId, int examId);

        [NotNull]
        Task UpdateGekDocument(int regionId, int examId, [NotNull] GekDocument document);

        [NotNull]
        Task DeleteGekDocument(int regionId, int examId);
    }
}