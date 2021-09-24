namespace Ege.Check.Logic.Services.Participant.ExamLists
{
    using System;
    using System.Threading.Tasks;
    using Ege.Check.Dal.Cache.CacheFactory;
    using Ege.Check.Dal.Cache.ExamCollection;
    using Ege.Check.Dal.MemoryCache.Regions;
    using Ege.Check.Dal.MemoryCache.Subjects;
    using Ege.Check.Dal.Store.Repositories.Exams;
    using Ege.Check.Logic.Models.Cache;
    using Ege.Check.Logic.Models.Response;
    using Ege.Dal.Common.Factory;
    using JetBrains.Annotations;

    internal class ExamListService : IExamListService
    {
        [NotNull]private readonly IExamCollectionCache _cache;
        [NotNull]private readonly ICacheFactory _cacheFactory;
        [NotNull]private readonly IDbConnectionFactory _connectionFactory;

        [NotNull]private readonly IRegionSettingsMemoryCache _regionSettingsMemoryCache;
        [NotNull]private readonly IParticipantExamRepository _repository;
        [NotNull]private readonly ISubjectExamMemoryCache _subjectExamMemoryCache;

        [NotNull] private readonly IExamListFilterHelper _filterHelper;

        public ExamListService(
            [NotNull] IParticipantExamRepository repository,
            [NotNull] IExamCollectionCache cache,
            [NotNull] IDbConnectionFactory connectionFactory,
            [NotNull] IRegionSettingsMemoryCache regionSettingsMemoryCache,
            [NotNull] ICacheFactory cacheFactory,
            [NotNull] ISubjectExamMemoryCache subjectExamMemoryCache,
            [NotNull] IExamListFilterHelper filterHelper)
        {
            _repository = repository;
            _cache = cache;
            _connectionFactory = connectionFactory;
            _regionSettingsMemoryCache = regionSettingsMemoryCache;
            _cacheFactory = cacheFactory;
            _subjectExamMemoryCache = subjectExamMemoryCache;
            _filterHelper = filterHelper;
        }

        public async Task<ExamListResponse> GetExamList(ParticipantCacheModel participant)
        {
            var cacheConnection = _cacheFactory.GetCache();

            var regionSettings = _regionSettingsMemoryCache.Get(participant.RegionId);
            var exams = _cache.Get(cacheConnection, participant);
            if (exams == null)
            {
                using (var connection = await _connectionFactory.CreateAsync())
                {
                    if (connection == null)
                    {
                        throw new InvalidOperationException("IDbConnectionFactory::CreateAsync returned null");
                    }
                    exams = await _repository.GetByParticipant(connection, participant);
                    _cache.Put(cacheConnection, participant, exams);
                }
            }

            if (exams == null)
            {
                throw new InvalidOperationException("IParticipantExamRepository::GetByParticipant returned null");
            }
            exams.Exams = exams.Exams ?? new ExamCacheModel[0];
            foreach (var exam in exams.Exams)
            {
                if (exam == null)
                {
                    continue;
                }
                var examInfo = _subjectExamMemoryCache.GetExam(exam.ExamId);
                exam.ExamDate = examInfo.Date;
                exam.Subject = examInfo.Subject.SubjectDisplayName;
                exam.MinMark = examInfo.Subject.MinValue;
                exam.IsComposition = examInfo.Subject.IsComposition;
                exam.IsBasicMath = examInfo.Subject.IsBasicMath;
                exam.IsForeignLanguage = examInfo.Subject.IsForeignLanguage;

                if (exam.OralExamId.HasValue)
                {
                    var oralExamInfo = _subjectExamMemoryCache.GetExam(exam.OralExamId.Value);
                    exam.OralExamDate = oralExamInfo.Date;
                    exam.OralSubject = oralExamInfo.Subject.SubjectDisplayName;
                }
            }
            exams.Exams = _filterHelper.ApplyFilter(participant, exams.Exams, regionSettings.Settings);
            var result = new ExamListResponse
                {
                    Result = exams,
                    Info = regionSettings.Info,
                };
            return result;
        }
    }
}
