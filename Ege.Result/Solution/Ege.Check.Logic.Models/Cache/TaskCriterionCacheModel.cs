namespace Ege.Check.Logic.Models.Cache
{
    /// <summary>
    ///     Кэш-модель критерия в задании с критериями
    ///     хранится в кэше только в составе коллекции
    /// </summary>
    public class TaskCriterionCacheModel : TaskInfoCacheModelBase
    {
        /// <summary>
        ///     Критерий
        /// </summary>
        public string Name { get; set; }
    }
}