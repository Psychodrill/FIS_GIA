namespace Ege.Check.Dal.Cache.LoadServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ege.Check.Dal.Cache.AppealCollection;
    using Ege.Check.Dal.Cache.ExamCollection;
    using Ege.Check.Dal.Cache.Interfaces.CacheFactory;
    using Ege.Check.Dal.Cache.LoadServices.Helpers;
    using Ege.Check.Logic.Models.Cache;
    using Ege.Check.Logic.Services.Dtos.Models;
    using JetBrains.Annotations;

    internal class AppealCacheWriter : ICacheWriter<AppealDto>
    {
        [NotNull] private readonly IAppealCollectionCache _appealCollectionCache;
        [NotNull] private readonly IExamCollectionCache _examCollectionCache;
        [NotNull] private readonly IParticipantLookupCreator _lookupCreator;
        [NotNull] private readonly IBulkProcessor _bulkProcessor;

        public AppealCacheWriter(
            [NotNull] IExamCollectionCache examCollectionCache,
            [NotNull] IAppealCollectionCache appealCollectionCache,
            [NotNull] IParticipantLookupCreator lookupCreator, 
            [NotNull] IBulkProcessor bulkProcessor)
        {
            _examCollectionCache = examCollectionCache;
            _appealCollectionCache = appealCollectionCache;
            _lookupCreator = lookupCreator;
            _bulkProcessor = bulkProcessor;
        }

        public async Task Write(ICacheWrapper wrapper, IReadOnlyCollection<AppealDto> elements)
        {
            var lookup = _lookupCreator.CreateByExam(elements);
            if (!wrapper.SupportsBulkOperations)
            {
                await WriteByOne(wrapper, lookup);
            }
            else
            {
                await WriteWithBulk(wrapper, lookup);
            }
        }

        private async Task WriteWithBulk([NotNull]ICacheWrapper wrapper, [NotNull]ParticipantExamLookup<AppealDto> lookup)
        {
            var participantExams = lookup.Lookup;
            using (var locked = await _examCollectionCache.LockBulkGet(wrapper, participantExams.Select(p => p.Key)))
            {
                if (locked == null)
                {
                    throw new InvalidOperationException("IExamCollectionCache::LockBulkGet returned null");
                }
                _bulkProcessor.Process(locked, participantExams.Select(pe => pe.Value), ProcessSingleParticipantAppeals);
            }

            using (var locked = await _appealCollectionCache.LockBulkGet(wrapper, lookup.Exams))
            {
                if (locked == null)
                {
                    throw new InvalidOperationException("IAnswerCollectionCache::LockBulkGet returned null");
                }
                _bulkProcessor.Process(locked, lookup.ItemsByExams, ProcessSingleParticipantExamAppeals);
            }
        }

        private async Task WriteByOne([NotNull]ICacheWrapper wrapper, [NotNull]ParticipantExamLookup<AppealDto> lookup)
        {
            foreach (var participantAppeals in lookup.Lookup)
            {
                if (participantAppeals.Key == null || participantAppeals.Value == null)
                {
                    continue;
                }
                await WriteParticipantAppeals(wrapper, participantAppeals.Key, participantAppeals.Value);
            }
        }

        public async Task WriteParticipantExamAppeals(
            ICacheWrapper wrapper,
            KeyValuePair<ParticipantCacheModel, int> participantExam,
            [NotNull] IEnumerable<AppealDto> appeals)
        {
            using (var locked = await _appealCollectionCache.LockGet(wrapper, participantExam))
            {
                if (locked == null)
                {
                    throw new InvalidOperationException("IAppealCollectionCache::LockGet returned null");
                }
                var existingAppeals = locked.Element;
                ProcessSingleParticipantExamAppeals(existingAppeals, appeals);
            }
        }

        private void ProcessSingleParticipantAppeals([NotNull]ExamCollectionCacheModel existingExams, [NotNull] IEnumerable<IGrouping<int, AppealDto>> examAnswersLookup)
        {
            existingExams.Exams = existingExams.Exams ?? new List<ExamCacheModel>();
            foreach (var examAnswers in examAnswersLookup)
            {
                if (examAnswers == null)
                {
                    continue;
                }
                var examId = examAnswers.Key;
                var existingExam = existingExams.Exams.FirstOrDefault(e => e != null && e.ExamId == examId);
                if (existingExam != null)
                {
                    existingExam.HasAppeal = true;
                }
                else
                {
                    existingExams.Exams.Add(new ExamCacheModel
                    {
                        ExamId = examAnswers.Key,
                        HasAppeal = true,
                    });
                }
            }
        }

        private void ProcessSingleParticipantExamAppeals(
            [NotNull]AppealCollectionCacheModel existingAppeals,
            [NotNull]IEnumerable<AppealDto> appeals)
        {
            existingAppeals.Appeals = existingAppeals.Appeals ?? new List<AppealCacheModel>();
            foreach (var appeal in appeals.OrderBy(a => a != null ? a.CreateDate : new DateTime()))
            {
                if (appeal == null)
                {
                    continue;
                }
                existingAppeals.Appeals.Add(new AppealCacheModel
                {
                    Date = appeal.CreateDate,
                    Status = appeal.Status,
                });
            }
        }

        public async Task WriteParticipantAppeals(
            ICacheWrapper wrapper, 
            [NotNull] ParticipantCacheModel participant,
            [NotNull] ILookup<int, AppealDto> appeals)
        {
            using (var locked = await _examCollectionCache.LockGet(wrapper, participant))
            {
                if (locked == null)
                {
                    throw new InvalidOperationException("IExamCollectionCache::LockGet returned null");
                }
                var exams = locked.Element;
                ProcessSingleParticipantAppeals(exams, appeals);
            }
            foreach (var examAppeals in appeals)
            {
                if (examAppeals == null)
                {
                    continue;
                }
                var examId = examAppeals.Key;
                await WriteParticipantExamAppeals(wrapper,
                    new KeyValuePair<ParticipantCacheModel, int>(participant, examId),
                    examAppeals);
            }
        }
    }
}