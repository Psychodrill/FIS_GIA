namespace Ege.Check.Dal.Cache.LoadServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ege.Check.Dal.Cache.AnswerCollection;
    using Ege.Check.Dal.Cache.Interfaces.CacheFactory;
    using Ege.Check.Dal.Cache.LoadServices.Helpers;
    using Ege.Check.Dal.MemoryCache.Subjects;
    using Ege.Check.Logic.Models.Cache;
    using Ege.Check.Logic.Models.Servers;
    using Ege.Check.Logic.Services.Dtos.Models;
    using JetBrains.Annotations;

    internal class BlankInfoCacheWriter : ICacheWriter<BlankInfoDto>, IBlankInfoCacheUpdater
    {
        [NotNull]private readonly IAnswerCollectionCache _cache;
        [NotNull]private readonly IParticipantLookupCreator _lookupCreator;
        [NotNull]private readonly IBulkProcessor _bulkProcessor;
        [NotNull] private readonly ISubjectExamMemoryCache _examMemoryCache;

        public BlankInfoCacheWriter(
            [NotNull] IAnswerCollectionCache cache,
            [NotNull] IParticipantLookupCreator lookupCreator, 
            [NotNull]IBulkProcessor bulkProcessor, 
            [NotNull]ISubjectExamMemoryCache examMemoryCache)
        {
            _cache = cache;
            _lookupCreator = lookupCreator;
            _bulkProcessor = bulkProcessor;
            _examMemoryCache = examMemoryCache;
        }

        public async Task Write(ICacheWrapper wrapper, IReadOnlyCollection<BlankInfoDto> elements)
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

        private async Task WriteWithBulk([NotNull]ICacheWrapper wrapper, [NotNull]ParticipantExamLookup<BlankInfoDto> lookup)
        {
            using (var locked = await _cache.LockBulkGet(wrapper, lookup.Exams))
            {
                if (locked == null)
                {
                    throw new InvalidOperationException("IAnswerCollectionCache::LockBulkGet returned null");
                }
                _bulkProcessor.Process(locked, lookup.ItemsByExams, ProcessSingleParticipantExamBlank);
            }
        }

        private async Task WriteByOne([NotNull]ICacheWrapper wrapper, [NotNull]ParticipantExamLookup<BlankInfoDto> lookup)
        {
            foreach (var participantBlanks in lookup.Lookup)
            {
                if (participantBlanks.Key == null || participantBlanks.Value == null)
                {
                    continue;
                }
                foreach (var examBlanks in participantBlanks.Value)
                {
                    if (examBlanks == null)
                    {
                        continue;
                    }
                    await WriteParticipantExamBlanks(
                        wrapper,
                        new KeyValuePair<ParticipantCacheModel, int>(participantBlanks.Key, examBlanks.Key),
                        examBlanks);
                }
            }
        }

        private async Task WriteParticipantExamBlanks(
            ICacheWrapper wrapper,
            KeyValuePair<ParticipantCacheModel, int> participantExam,
            [NotNull] IEnumerable<BlankInfoDto> blanks)
        {
            using (var locked = await _cache.LockGet(wrapper, participantExam))
            {
                if (locked == null)
                {
                    throw new InvalidOperationException("IAnswerCollectionCache::LockGet returned null");
                }
                var existingAnswers = locked.Element;
                ProcessSingleParticipantExamBlank(existingAnswers, blanks);
            }
        }

        private void ProcessSingleParticipantExamBlank(
            [NotNull] AnswerCollectionCacheModel existingAnswers,
            [NotNull]IEnumerable<BlankInfoDto> blanks)
        {
            existingAnswers.Blanks = existingAnswers.Blanks ?? new List<BlankCacheModel>();
            foreach (var blank in blanks)
            {
                if (blank == null || blank.Barcode == null)
                {
                    continue;
                }
                var exam = _examMemoryCache.GetExam(blank.ExamGlobalId);
                existingAnswers.Blanks.Add(new BlankCacheModel
                {
                    Barcode = blank.Barcode,
                    BlankType = blank.BlankType,
                    PageCount = !exam.Subject.IsComposition ? blank.PageCount : (int?)null,
                    ProjectBatchId = blank.ProjectBatchId,
                    ProjectName = blank.ProjectName,
                });
            }
        }

        public async Task UpdatePageCount(ICacheWrapper wrapper, IReadOnlyCollection<UpdatedBlankInfo> updates)
        {
            var lookup = _lookupCreator.CreateByExam(updates);
            if (wrapper.SupportsBulkOperations)
            {
                using (var locked = await _cache.LockBulkGet(wrapper, lookup.Exams, false))
                {
                    if (locked == null)
                    {
                        throw new InvalidOperationException("IAnswerCollectionCache::LockBulkGet returned null");
                    }
                    _bulkProcessor.Process(locked, lookup.ItemsByExams, ProcessUpdatePageCount);
                }
            }
            else
            {
                foreach (var participantBlanks in lookup.Lookup)
                {
                    if (participantBlanks.Key == null || participantBlanks.Value == null)
                    {
                        continue;
                    }
                    foreach (var examBlanks in participantBlanks.Value)
                    {
                        if (examBlanks == null)
                        {
                            continue;
                        }
                        await UpdateParticipantExamBlanks(
                            wrapper,
                            new KeyValuePair<ParticipantCacheModel, int>(participantBlanks.Key, examBlanks.Key),
                            examBlanks);
                    }
                }
            }
        }

        [NotNull]
        private async Task UpdateParticipantExamBlanks(
            [NotNull]ICacheWrapper wrapper,
            KeyValuePair<ParticipantCacheModel, int> participantExam, 
            [NotNull]IEnumerable<UpdatedBlankInfo> examBlanks)
        {
            using (var locked = await _cache.LockGet(wrapper, participantExam, false))
            {
                if (locked == null)
                {
                    return;
                }
                var existingAnswers = locked.Element;
                ProcessUpdatePageCount(existingAnswers, examBlanks);
            }
        }

        private void ProcessUpdatePageCount(AnswerCollectionCacheModel cached, [NotNull]IEnumerable<UpdatedBlankInfo> updates)
        {
            if (cached == null || cached.Blanks == null)
            {
                return;
            }
            foreach (var update in updates)
            {
                if (update == null)
                {
                    continue;
                }
                var updated = cached.Blanks.FirstOrDefault(b => b != null && b.BlankType == update.BlankType);
                if (updated != null)
                {
                    updated.PageCount = update.PageCount;
                }
            }
        }
    }
}
