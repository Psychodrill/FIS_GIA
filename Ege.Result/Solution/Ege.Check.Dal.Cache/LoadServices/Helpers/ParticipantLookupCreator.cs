namespace Ege.Check.Dal.Cache.LoadServices.Helpers
{
    using System.Collections.Generic;
    using System.Linq;
    using Ege.Check.Logic.Models.Cache;
    using Ege.Check.Logic.Services.Dtos.Models;

    internal class ParticipantLookupCreator : IParticipantLookupCreator
    {
        public ILookup<ParticipantCacheModel, T> Create<T>(IReadOnlyCollection<T> collection) 
            where T : class, IParticipantDependentThing
        {
            return collection.ToLookup(ex => ex != null
                                                 ? new ParticipantCacheModel {Code = ex.Code, RegionId = ex.RegionId}
                                                 : null,
                                       ParticipantCacheModelEqualityComparer.Instance);
        }


        public ParticipantExamLookup<T> CreateByExam<T>(IReadOnlyCollection<T> collection) 
            where T : class, IParticipantExamDependentThing
        {
            var participantLookup = Create(collection);
            return new ParticipantExamLookup<T>(participantLookup
                .Where(p => p != null)
                .Select(participant => new KeyValuePair<ParticipantCacheModel, ILookup<int, T>>(
                    participant.Key,
                    participant.Where(a => a != null).ToLookup(a => a.ExamGlobalId)))
                .ToList());
        }
    }
}
