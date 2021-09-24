namespace Ege.Check.Logic.Models.Cache
{
    using Ege.Check.Logic.Services.Dtos.Enums;

    /// <summary>
    ///     Базовый класс для информации о задании/критерии
    /// </summary>
    public class TaskInfoCacheModelBase
    {
        /// <summary>
        ///     Часть А, Б, Ц, Д
        /// </summary>
        public TaskType Type { get; set; }

        /// <summary>
        ///     Номер задания
        /// </summary>
        public int? Number { get; set; }

        /// <summary>
        ///     Максимальный балл
        /// </summary>
        public int? MaxValue { get; set; }
    }

    /// <summary>
    ///     Базовый класс для информации о задании (не критерии)
    /// </summary>
    public class RealTaskInfoCacheModelBase : TaskInfoCacheModelBase
    {
        /// <summary>
        ///     Отображаемый номер
        /// </summary>
        public string DisplayNumber { get; set; }
    }
}