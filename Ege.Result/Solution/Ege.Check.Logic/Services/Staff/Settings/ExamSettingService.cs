namespace Ege.Check.Logic.Services.Staff.Settings
{
    using System.Linq;
    using System.Threading.Tasks;
    using Ege.Check.Dal.Store.Repositories.Regions;
    using Ege.Check.Logic.Helpers;
    using Ege.Check.Logic.Models.Staff;
    using Ege.Check.Logic.Services.Dtos.Enums;
    using Ege.Dal.Common.Factory;
    using JetBrains.Annotations;

    internal class ExamSettingService : IExamSettingService
    {
        [NotNull] private readonly IDbConnectionFactory _connectionFactory;
        [NotNull] private readonly IUrlCorrector _corrector;
        [NotNull] private readonly IRegionSettingsRepository _repository;

        public ExamSettingService(
            [NotNull] IRegionSettingsRepository repository,
            [NotNull] IDbConnectionFactory connectionFactory,
            [NotNull] IUrlCorrector corrector)
        {
            _repository = repository;
            _connectionFactory = connectionFactory;
            _corrector = corrector;
        }

        public async Task<ExamSettings> GetSettingsByRegion(int regionId, ExamWave wave)
        {
            ExamSettings result;
            using (var connection = await _connectionFactory.CreateAsync())
            {
                result = await _repository.GetSettingsForStaff(connection, regionId, wave);
            }
            if (wave == ExamWave.Composition)
            {
                var singleCompositionExam = result.Settings.FirstOrDefault();
                if (singleCompositionExam != null)
                {
                    singleCompositionExam.SubjectName = "Сочинение (изложение)";
                    result.Settings = new[] { singleCompositionExam };
                }
            }
            return result;
        }

        public async Task SetSettings(int regionId, ExamSettings settings)
        {
            using (var connection = await _connectionFactory.CreateAsync())
            {
                await _repository.UpdateSettings(connection, regionId, settings.Settings);
            }
        }

        public async Task<GekDocument> GetGekDocument(int regionId, int examId)
        {
            using (var connection = await _connectionFactory.CreateAsync())
            {
                return await _repository.GetGekDocument(connection, regionId, examId);
            }
        }

        public async Task UpdateGekDocument(int regionId, int examId, GekDocument document)
        {
            document.Url = _corrector.Correct(document.Url);
            using (var connection = await _connectionFactory.CreateAsync())
            {
                await _repository.UpdateGekDocument(connection, regionId, examId, document);
            }
        }

        public async Task DeleteGekDocument(int regionId, int examId)
        {
            using (var connection = await _connectionFactory.CreateAsync())
            {
                await _repository.DeleteGekDocument(connection, regionId, examId);
            }
        }
    }
}