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

    internal class AnswerCacheWriter : ICacheWriter<AnswerDto>
    {
        [NotNull] private readonly IAnswerCollectionCache _answerCollectionCache;
        [NotNull] private readonly IExamCollectionCache _examCollectionCache;
        [NotNull] private readonly IParticipantLookupCreator _lookupCreator;
        [NotNull] private readonly IBulkProcessor _bulkProcessor;

        public AnswerCacheWriter(
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

        public async Task Write(ICacheWrapper wrapper, IReadOnlyCollection<AnswerDto> elements)
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

        private async Task WriteWithBulk([NotNull]ICacheWrapper wrapper, [NotNull]ParticipantExamLookup<AnswerDto> lookup)
        {
            var participantExams = lookup.Lookup;
            using (var locked = await _examCollectionCache.LockBulkGet(wrapper, participantExams.Select(p => p.Key)))
            {
                if (locked == null)
                {
                    throw new InvalidOperationException("IExamCollectionCache::LockBulkGet returned null");
                }
                _bulkProcessor.Process(locked, participantExams.Select(pe => pe.Value), ProcessSingleParticipantAnswers);
            }

            using (var locked = await _answerCollectionCache.LockBulkGet(wrapper, lookup.Exams))
            {
                if (locked == null)
                {
                    throw new InvalidOperationException("IAnswerCollectionCache::LockBulkGet returned null");
                }
                _bulkProcessor.Process(locked, lookup.ItemsByExams, ProcessSingleExamAnswers);
            }
        }

        private async Task WriteByOne(ICacheWrapper wrapper, [NotNull]ParticipantExamLookup<AnswerDto> lookup)
        {
            foreach (var participantAnswers in lookup.Lookup)
            {
                if (participantAnswers.Key == null)
                {
                    continue;
                }
                var examAnswersLookup = participantAnswers.Value;
                if (examAnswersLookup == null)
                {
                    continue;
                }
                using (var locked = await _examCollectionCache.LockGet(wrapper, participantAnswers.Key))
                {
                    if (locked == null)
                    {
                        throw new InvalidOperationException("IExamCollectionCache::LockGet returned null");
                    }
                    var existingExams = locked.Element;
                    ProcessSingleParticipantAnswers(existingExams, examAnswersLookup);
                }
                foreach (var examAnswers in examAnswersLookup)
                {
                    if (examAnswers == null)
                    {
                        continue;
                    }
                    var examId = examAnswers.Key;
                    var participantExamKey = new KeyValuePair<ParticipantCacheModel, int>(participantAnswers.Key, examId);
                    using (var locked = await _answerCollectionCache.LockGet(wrapper, participantExamKey))
                    {
                        if (locked == null)
                        {
                            throw new InvalidOperationException("IAnswerCollectionCache::LockGet returned null");
                        }
                        var existingAnswers = locked.Element;
                        ProcessSingleExamAnswers(existingAnswers, examAnswers);
                    }
                }
            }
        }

        private void ProcessSingleExamAnswers([NotNull]AnswerCollectionCacheModel existingAnswers, [NotNull] IEnumerable<AnswerDto> answers)
        {
            existingAnswers.Answers = existingAnswers.Answers ?? new List<AnswerCacheModel>();
            foreach (var answer in answers)
            {
                if (answer == null)
                {
                    continue;
                }
                var existingAnswer = existingAnswers.Answers.FirstOrDefault(
                    a => a != null && a.Type == answer.TaskTypeCode && a.Number == answer.TaskNumber);
                if (existingAnswer == null)
                {
                    existingAnswers.Answers.Add(new AnswerCacheModel
                    {
                        Answer = answer.AnswerValue,
                        Mark = answer.Mark,
                        Number = answer.TaskNumber,
                        Type = answer.TaskTypeCode,
                    });
                }
                else
                {
                    existingAnswer.Answer = answer.AnswerValue;
                    existingAnswer.Mark = answer.Mark;
                    existingAnswer.Number = answer.TaskNumber;
                    existingAnswer.Type = answer.TaskTypeCode;
                }
            }
        }

        private void ProcessSingleParticipantAnswers([NotNull]ExamCollectionCacheModel existingExams, [NotNull] IEnumerable<IGrouping<int, AnswerDto>> examAnswersLookup)
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
                    existingExam.HasResult = true;
                }
                else
                {
                    existingExams.Exams.Add(new ExamCacheModel
                    {
                        ExamId = examAnswers.Key,
                        HasResult = true,
                    });
                }
            }
        }
    }
}
