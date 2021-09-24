namespace Ege.Check.Dal.Cache.LoadServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ege.Check.Dal.Cache.AnswerCollection;
    using Ege.Check.Dal.Cache.ExamCollection;
    using Ege.Check.Dal.Cache.Interfaces.CacheFactory;
    using Ege.Check.Dal.Cache.LoadServices.Helpers;
    using Ege.Check.Logic.Models.Cache;
    using Ege.Check.Logic.Services.Dtos.Models;
    using JetBrains.Annotations;

    internal class ParticipantExamCacheWriter : ICacheWriter<ParticipantExamDto>
    {
        [NotNull]private readonly IAnswerCollectionCache _answerCollectionCache;
        [NotNull]private readonly IExamCollectionCache _examCollectionCache;
        [NotNull]private readonly IParticipantLookupCreator _lookupCreator;
        [NotNull]private readonly IBulkProcessor _bulkProcessor;

        public ParticipantExamCacheWriter(
            [NotNull] IExamCollectionCache examCollectionCache,
            [NotNull] IAnswerCollectionCache answerCollectionCache,
            [NotNull] IParticipantLookupCreator lookupCreator, 
            [NotNull] IBulkProcessor bulkProcessor)
        {
            _examCollectionCache = examCollectionCache;
            _answerCollectionCache = answerCollectionCache;
            _lookupCreator = lookupCreator;
            _bulkProcessor = bulkProcessor;
        }

        private void ProcessSingleParticipantExam(
            [NotNull]ExamCollectionCacheModel existingExams, 
            [NotNull]IEnumerable<ParticipantExamDto> participantExams)
        {
            existingExams.Exams = existingExams.Exams ?? new List<ExamCacheModel>();
            foreach (var element in participantExams)
            {
                if (element == null)
                {
                    continue;
                }
                var existingExam = existingExams.Exams.FirstOrDefault(
                    e => e != null && e.ExamId == element.ExamGlobalId);
                if (existingExam != null)
                {
                    existingExam.Mark5 = element.Mark5;
                    existingExam.TestMark = element.TestMark;
                    existingExam.Status = element.ProcessCondition;
                }
                else
                {
                    var currentExam = new ExamCacheModel
                        {
                            ExamId = element.ExamGlobalId,
                            Mark5 = element.Mark5,
                            Status = element.ProcessCondition,
                            TestMark = element.TestMark,
                            AppealStatus = null,
                            HasAppeal = false,
                            HasResult = false,
                            IsHidden = false,
                            IsHiddenForRegion = false,
                        };
                    existingExams.Exams.Add(currentExam);
                }
            }
        }

        private void ProcessSingleParticipantExamAnswers(
            [NotNull] AnswerCollectionCacheModel existingAnswers,
            [NotNull] ParticipantExamDto element)
        {
            existingAnswers.Mark5 = element.Mark5;
            existingAnswers.TestMark = element.TestMark;
            existingAnswers.PrimaryMark = element.PrimaryMark;
        }

        public async Task WriteByOne([NotNull]ICacheWrapper cacheWrapper, [NotNull]ILookup<ParticipantCacheModel, ParticipantExamDto> lookup)
        {
            foreach (var participantExams in lookup)
            {
                if (participantExams == null || participantExams.Key == null)
                {
                    continue;
                }
                var participant = participantExams.Key;
                using (var locked = await _examCollectionCache.LockGet(cacheWrapper, participant))
                {
                    if (locked == null)
                    {
                        throw new InvalidOperationException("IExamCollection::LockGet returned null");
                    }
                    var existingExams = locked.Element;
                    ProcessSingleParticipantExam(existingExams, participantExams);
                }

                foreach (var element in participantExams)
                {
                    if (element == null)
                    {
                        continue;
                    }
                    using (var lockedAnswers = await _answerCollectionCache.LockGet(
                            cacheWrapper,
                            new KeyValuePair<ParticipantCacheModel, int>(participant, element.ExamGlobalId)))
                    {
                        if (lockedAnswers == null)
                        {
                            throw new InvalidOperationException("IAnswerCollection::LockGet returned null");
                        }
                        var existingAnswers = lockedAnswers.Element;
                        ProcessSingleParticipantExamAnswers(existingAnswers, element);
                    }
                }
            }
        }

        public async Task Write(ICacheWrapper cacheWrapper, IReadOnlyCollection<ParticipantExamDto> elements)
        {
            var lookup = _lookupCreator.Create(elements);
            if (!cacheWrapper.SupportsBulkOperations)
            {
                await WriteByOne(cacheWrapper, lookup);
            }
            else
            {
                using (var locked = await _examCollectionCache.LockBulkGet(cacheWrapper, lookup.Where(l => l != null).Select(l => l.Key)))
                {
                    if (locked == null)
                    {
                        throw new InvalidOperationException("IExamCollectionCache::LockBulkGet returned null");
                    }
                    _bulkProcessor.Process(locked, lookup.Where(l => l != null), ProcessSingleParticipantExam);
                }
                using (var locked = await _answerCollectionCache.LockBulkGet(
                    cacheWrapper,
                    lookup.Where(l => l != null).SelectMany(
                        l => l.Where(le => le != null).Select(
                           le => new KeyValuePair<ParticipantCacheModel, int>(l.Key, le.ExamGlobalId)))))
                {
                    if (locked == null)
                    {
                        throw new InvalidOperationException("IAnswerCollectionCache::LockBulkGet returned null");
                    }
                    _bulkProcessor.Process(locked, lookup.Where(l => l != null).SelectMany(l => l).Where(l => l != null), ProcessSingleParticipantExamAnswers);
                }
            }
        }
    }
}
