namespace Ege.Check.Dal.Cache.LoadServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ege.Check.Dal.Cache.Interfaces.CacheFactory;
    using Ege.Check.Dal.Cache.LoadServices.Helpers;
    using Ege.Check.Dal.Cache.Participants;
    using Ege.Check.Logic.Models.Cache;
    using Ege.Check.Logic.Services.Dtos.Models;
    using JetBrains.Annotations;

    internal class ParticipantCacheWriter : ICacheWriter<ParticipantDto>
    {
        [NotNull] private readonly IParticipantCache _cache;
        [NotNull] private readonly IBulkProcessor _bulkProcessor;

        public ParticipantCacheWriter([NotNull] IParticipantCache cache, [NotNull]IBulkProcessor bulkProcessor)
        {
            _cache = cache;
            _bulkProcessor = bulkProcessor;
        }

        private void ProcessSingleParticipant([NotNull]ParticipantCollectionCacheModel withSameHash, [NotNull]ParticipantDto element)
        {
            withSameHash.Participants = withSameHash.Participants ?? new List<ParticipantCacheModel>();
            var existing = withSameHash.Participants.FirstOrDefault(p =>
                p != null && p.Code != null &&
                p.Code.Equals(element.Code, StringComparison.Ordinal) && p.RegionId == element.RegionId);
            if (existing == null && !element.IsDeleted)
            {
                withSameHash.Participants.Add(new ParticipantCacheModel
                {
                    Code = element.Code,
                    RegionId = element.RegionId,
                    Document = element.Document,
                    Hash = element.Hash,
                });
            }
            else if (existing != null && element.IsDeleted)
            {
                withSameHash.Participants.Remove(existing);
            }
        }

        public async Task Write(ICacheWrapper wrapper, IReadOnlyCollection<ParticipantDto> elements)
        {
            if (wrapper.SupportsBulkOperations)
            {
                using (var locked = await _cache.LockBulkGet(wrapper, elements.Where(e => e != null).Select(e => e.Hash)))
                {
                    if (locked == null)
                    {
                        throw new InvalidOperationException("IParticipantCache::LockBulkGet returned null");
                    }
                    _bulkProcessor.Process(locked, elements.Where(e => e != null), ProcessSingleParticipant);
                }
            }
            else
            {
                foreach (var element in elements)
                {
                    if (element == null)
                    {
                        continue;
                    }
                    using (var locked = await _cache.LockGet(wrapper, element.Hash))
                    {
                        if (locked == null)
                        {
                            throw new InvalidOperationException("IParticipantCache::LockGet returned null");
                        }
                        var withSameHash = locked.Element;
                        ProcessSingleParticipant(withSameHash, element);
                    }
                }
            }
        }
    }
}
