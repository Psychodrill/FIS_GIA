namespace Ege.Check.Dal.Cache.AnswerCollection
{
    using System.Collections.Generic;
    using Ege.Check.Dal.Cache.Interfaces;
    using Ege.Check.Logic.Models.Cache;

    /// <summary>
    ///     Кэш для ответов участника на экзамене
    /// </summary>
    public interface IAnswerCollectionCache :
        ILockableCache<KeyValuePair<ParticipantCacheModel, int>, AnswerCollectionCacheModel>
    {
    }
}