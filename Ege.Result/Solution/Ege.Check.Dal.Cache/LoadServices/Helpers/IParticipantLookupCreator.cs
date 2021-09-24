namespace Ege.Check.Dal.Cache.LoadServices.Helpers
{
    using System.Collections.Generic;
    using System.Linq;
    using Ege.Check.Logic.Models.Cache;
    using Ege.Check.Logic.Services.Dtos.Models;
    using JetBrains.Annotations;

    public interface IParticipantLookupCreator
    {
        [NotNull]
        ILookup<ParticipantCacheModel, T> Create<T>([NotNull] IReadOnlyCollection<T> collection)
            where T : class, IParticipantDependentThing;

        [NotNull]
        ParticipantExamLookup<T> CreateByExam<T>(
            [NotNull] IReadOnlyCollection<T> collection)
            where T : class, IParticipantExamDependentThing;
    }
}