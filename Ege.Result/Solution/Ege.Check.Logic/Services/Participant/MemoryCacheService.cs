namespace Ege.Check.Logic.Services.Participant
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ege.Check.Dal.MemoryCache.CancelledParticipantExams;
    using Ege.Check.Dal.MemoryCache.ExamInfo;
    using Ege.Check.Dal.MemoryCache.Regions;
    using Ege.Check.Dal.MemoryCache.Subjects;
    using Ege.Check.Dal.Store.Repositories.Answers;
    using Ege.Check.Dal.Store.Repositories.Exams;
    using Ege.Check.Dal.Store.Repositories.Regions;
    using Ege.Dal.Common.Factory;
    using JetBrains.Annotations;
    using global::Common.Logging;

    internal class MemoryCacheService : IMemoryCacheService
    {
        [NotNull]private static readonly ILog Logger = LogManager.GetLogger<MemoryCacheService>();
        [NotNull]private readonly IDbConnectionFactory _connectionFactory;
        [NotNull]private readonly IExamInfoMemoryCache _criteriaCache;
        [NotNull]private readonly IAnswerCriteriaRepository _criteriaRepository;
        [NotNull]private readonly ISubjectExamMemoryCache _examCache;
        [NotNull]private readonly IRegionMemoryCache _regionCache;
        [NotNull]private readonly IRegionRepository _regionRepository;
        [NotNull]private readonly IRegionSettingsMemoryCache _settingsCache;
        [NotNull]private readonly IRegionSettingsRepository _settingsRepository;
        [NotNull]private readonly ISubjectExamRepository _subjectExamRepository;
        [NotNull]private readonly ICancelledParticipantExamMemoryCache _participantExamMemoryCache;
        [NotNull]private readonly IExamCancellationRepository _examCancellationRepository;

        private DateTime _lastRefresh;
        private bool _lastRefreshSuccesful;

        public MemoryCacheService(
            [NotNull] IRegionSettingsMemoryCache settingsCache,
            [NotNull] IRegionSettingsRepository settingsRepository,
            [NotNull] IDbConnectionFactory connectionFactory,
            [NotNull] IExamInfoMemoryCache criteriaCache,
            [NotNull] IAnswerCriteriaRepository criteriaRepository,
            [NotNull] IRegionRepository regionRepository,
            [NotNull] IRegionMemoryCache regionCache,
            [NotNull] ISubjectExamMemoryCache examCache,
            [NotNull] ISubjectExamRepository subjectExamRepository,
            [NotNull] ICancelledParticipantExamMemoryCache participantExamMemoryCache,
            [NotNull] IExamCancellationRepository examCancellationRepository)
        {
            _settingsCache = settingsCache;
            _settingsRepository = settingsRepository;
            _connectionFactory = connectionFactory;
            _criteriaCache = criteriaCache;
            _criteriaRepository = criteriaRepository;
            _regionRepository = regionRepository;
            _regionCache = regionCache;
            _examCache = examCache;
            _subjectExamRepository = subjectExamRepository;
            _participantExamMemoryCache = participantExamMemoryCache;
            _examCancellationRepository = examCancellationRepository;
        }

        public async Task RefreshMemoryCache(
            bool refreshRegionSettings = true,
            bool refreshAnswerCriteria = true,
            bool refreshAvailableRegions = true,
            bool refreshCancelledExams = true,
            bool refreshSubjectsAndExams = true)
        {
            try
            {
                _lastRefresh = DateTime.Now;
                using (var connection = await _connectionFactory.CreateAsync())
                {
                    if (connection == null)
                    {
                        throw new InvalidOperationException("IDbConnectionFactory::CreateAsync returned null");
                    }
                    if (refreshRegionSettings)
                    {
                        _settingsCache.Put(await _settingsRepository.GetSettingsForParticipant(connection));
                    }
                    if (refreshAnswerCriteria)
                    {
                        _criteriaCache.Put(await _criteriaRepository.GetAll(connection));
                    }
                    if (refreshAvailableRegions)
                    {
                        _regionCache.Put(await _regionRepository.GetAvailableRegions(connection));
                    }
                    if (refreshCancelledExams)
                    {
                        _participantExamMemoryCache.Put(await _examCancellationRepository.GetCancelledParticipanExams(connection));
                    }
                    if (refreshSubjectsAndExams)
                    {
                        var exams = await _subjectExamRepository.GetAll(connection);
                        _examCache.Put(exams.Value, exams.Key);
                    }
                    _lastRefreshSuccesful = true;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                _lastRefreshSuccesful = false;
            }
        }

        public KeyValuePair<DateTimeOffset, bool> GetLastRefreshTime()
        {
            return new KeyValuePair<DateTimeOffset, bool>(_lastRefresh, _lastRefreshSuccesful);
        }
    }
}
