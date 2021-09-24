namespace Ege.Check.Logic.Models.Cache
{
    /// <summary>
    ///     Кэш-модель информации о задании из части Б
    ///     хранится в кэше только в составе коллекции
    /// </summary>
    public class TaskBInfoCacheModel : RealTaskInfoCacheModelBase
    {
        /// <summary>
        ///     Допустимые символы
        /// </summary>
        public string LegalSymbols { get; set; }
    }
}