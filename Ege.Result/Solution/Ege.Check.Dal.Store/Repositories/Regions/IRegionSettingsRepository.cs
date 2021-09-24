namespace Ege.Check.Dal.Store.Repositories.Regions
{
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Threading.Tasks;
    using Ege.Check.Logic.Models.Cache;
    using Ege.Check.Logic.Models.Staff;
    using Ege.Check.Logic.Services.Dtos.Enums;
    using JetBrains.Annotations;

    public interface IRegionSettingsRepository
    {
        [NotNull]
        Task<IDictionary<int, RegionSettingsCacheModel>> GetSettingsForParticipant([NotNull] DbConnection connection);

        [NotNull]
        Task<ExamSettings> GetSettingsForStaff([NotNull] DbConnection connection, int regionId, ExamWave wave);

        [NotNull]
        Task UpdateSettings([NotNull] DbConnection connection, int regionId, IEnumerable<ExamSetting> settings);

        [NotNull]
        Task<GekDocument> GetGekDocument([NotNull] DbConnection connection, int regionId, int examId);

        [NotNull]
        Task UpdateGekDocument([NotNull] DbConnection connection, int regionId, int examId, GekDocument document);

        [NotNull]
        Task DeleteGekDocument([NotNull] DbConnection connection, int regionId, int examId);
    }
}