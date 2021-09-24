namespace Ege.Check.Dal.Cache.Participants
{
    using Ege.Check.Dal.Cache.Interfaces;
    using Ege.Check.Logic.Models.Cache;

    /// <summary>
    ///     Кэш участников
    /// </summary>
    public interface IParticipantCache : ILockableCache<string, ParticipantCollectionCacheModel>
    {
    }
}