namespace Ege.Check.Logic.Services.Participant.ExamDetails
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ege.Check.Dal.Blanks;
    using Ege.Check.Dal.Cache.AnswerCollection;
    using Ege.Check.Dal.Cache.CacheFactory;
    using Ege.Check.Dal.MemoryCache.CancelledParticipantExams;
    using Ege.Check.Dal.MemoryCache.ExamInfo;
    using Ege.Check.Dal.MemoryCache.Regions;
    using Ege.Check.Dal.MemoryCache.Subjects;
    using Ege.Check.Dal.Store.Repositories.Answers;
    using Ege.Check.Logic.Models.Cache;
    using Ege.Check.Logic.Models.Response;
    using Ege.Dal.Common.Factory;
    using JetBrains.Annotations;

    internal class ExamDetailsService : IExamDetailsService
    {
        [NotNull]private readonly IAnswerCollectionCache _cache;
        [NotNull]private readonly ICacheFactory _cacheFactory;
        [NotNull]private readonly IDbConnectionFactory _connectionFactory;

        [NotNull]private readonly IExamInfoMemoryCache _infoCache;
        [NotNull]private readonly IAnswerRepository _repository;

        [NotNull]private readonly IRegionSettingsMemoryCache _settingsMemoryCache;
        [NotNull]private readonly ISubjectExamMemoryCache _subjectExamMemoryCache;

        [NotNull]private readonly IBlankModelCreator _blankModelCreator;
        [NotNull]private readonly ICancelledParticipantExamMemoryCache _cancelledExamsMemoryCache;

        public ExamDetailsService(
            [NotNull] IAnswerRepository repository,
            [NotNull] IAnswerCollectionCache cache,
            [NotNull] IExamInfoMemoryCache infoCache,
            [NotNull] IDbConnectionFactory connectionFactory,
            [NotNull] IRegionSettingsMemoryCache settingsMemoryCache,
            [NotNull] ICacheFactory cacheFactory,
            [NotNull] ISubjectExamMemoryCache subjectExamMemoryCache,
            [NotNull] IBlankModelCreator blankModelCreator,
            [NotNull] ICancelledParticipantExamMemoryCache cancelledExamsMemoryCache)
        {
            _repository = repository;
            _cache = cache;
            _infoCache = infoCache;
            _connectionFactory = connectionFactory;
            _settingsMemoryCache = settingsMemoryCache;
            _cacheFactory = cacheFactory;
            _subjectExamMemoryCache = subjectExamMemoryCache;
            _blankModelCreator = blankModelCreator;
            _cancelledExamsMemoryCache = cancelledExamsMemoryCache;
        }

        public async Task<ExamResponse> GetExamDetails(KeyValuePair<ParticipantCacheModel, int> participantExam)
        {
            if (participantExam.Key == null)
            {
                throw new ArgumentException("participantExam.Key is null");
            }
            if (participantExam.Key.Code == null)
            {
                throw new ArgumentException("participantExam.Key.Code is null");
            }

            var cacheConnection = _cacheFactory.GetCache();
            var settings = _settingsMemoryCache.Get(participantExam.Key.RegionId);

            RegionExamSettingCacheModel setting;
            if (!settings.Settings.TryGetValue(participantExam.Value, out setting) || setting == null ||
                !setting.ShowResult)
            {
                return null;
            }

            var answers = _cache.Get(cacheConnection, participantExam);
            if (answers == null)
            {
                using (var connection = await _connectionFactory.CreateAsync())
                {
                    if (connection == null)
                    {
                        throw new InvalidOperationException("IDbConnectionFactory::CreateAsync() returned null");
                    }
                    answers = await _repository.GetExamAnswers(connection, participantExam);
                }
                _cache.Put(cacheConnection, participantExam, answers);
            }

            ExamInfoCacheModel info = null;
            if (answers != null)
            {
                answers.IsHidden = _cancelledExamsMemoryCache.IsHidden(participantExam.Key.Code, participantExam.Key.RegionId, participantExam.Value);
                var examInfo = _subjectExamMemoryCache.GetExam(participantExam.Value);
                info = _infoCache.Get(examInfo.SubjectCode);
                if (setting.ShowBlanks && answers.Blanks != null)
                {
                    using (var blankcoder = new NameCoderBlank())
                    {
                        answers.BlanksWithUrls = _blankModelCreator.Create(answers.Blanks, examInfo.Date, examInfo.SubjectCode, blankcoder)
                            .ToList();
                    }
                }
                answers.Blanks = null;  // не отдавать массив на клиент
            }

            return answers != null && !answers.IsHidden
                       ? new ExamResponse
                           {
                               Answers = answers,
                               ExamInfo = info,
                               GekDocument = setting.GekDocument,
                               Servers = settings.Servers,
                           }
                       : null;
        }
    }
}
