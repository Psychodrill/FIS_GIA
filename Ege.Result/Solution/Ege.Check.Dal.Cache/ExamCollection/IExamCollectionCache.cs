namespace Ege.Check.Dal.Cache.ExamCollection
{
    using Ege.Check.Dal.Cache.Interfaces;
    using Ege.Check.Logic.Models.Cache;

    /// <summary>
    ///     Кэш для списка экзаменов участника
    /// </summary>
    public interface IExamCollectionCache : ILockableCache<ParticipantCacheModel, ExamCollectionCacheModel>
    {
    }
}