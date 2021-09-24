namespace Ege.Check.Logic.Models.Cache
{
    using System.Collections.Generic;

    /// <summary>
    ///     Кэш-модель информации о задании с критериями (бывают в частях А (которая только на сочинении), Ц, Д)
    ///     хранится в кэше только в составе коллекции
    /// </summary>
    public class TaskWithCriteriaInfoCacheModel : RealTaskInfoCacheModelBase
    {
        /// <summary>
        ///     Критерии
        /// </summary>
        public ICollection<TaskCriterionCacheModel> Criteria { get; set; }
    }
}