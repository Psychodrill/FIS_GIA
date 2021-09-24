namespace Ege.Check.Dal.Cache.AppealCollection
{
    using System.Collections.Generic;
    using Ege.Check.Dal.Cache.Interfaces;
    using Ege.Check.Logic.Models.Cache;

    /// <summary>
    ///     Кэш для апелляций
    /// </summary>
    public interface IAppealCollectionCache :
        ILockableCache<KeyValuePair<ParticipantCacheModel, int>, AppealCollectionCacheModel>
    {
    }
}