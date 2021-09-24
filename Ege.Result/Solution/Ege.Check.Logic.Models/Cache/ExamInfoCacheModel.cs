namespace Ege.Check.Logic.Models.Cache
{
    using System.Collections.Generic;

    /// <summary>
    ///     Кэш-модель общей информации о предмете (максимальный балл и т.п.)
    ///     Ключ в кэше - SubjectCode
    /// </summary>
    public class ExamInfoCacheModel
    {
        /// <summary>
        ///     Минимальный проходной балл
        /// </summary>
        public int Threshold { get; set; }

        /// <summary>
        ///     Является ли сочинением/изложением
        /// </summary>
        public bool IsComposition { get; set; }

        /// <summary>
        ///     Является ли базовой математикой
        /// </summary>
        public bool IsBasicMath { get; set; }

        /// <summary>
        ///     Является ли иностранным языком
        /// </summary>
        public bool IsForeignLanguage { get; set; }

        /// <summary>
        ///     Задания из части Б (с допустимыми символами)
        /// </summary>
        public ICollection<TaskBInfoCacheModel> PartB { get; set; }

        /// <summary>
        ///     Задания из частей А (встречается только на сочинении), Ц, Д - с критериями
        /// </summary>
        public ICollection<TaskWithCriteriaInfoCacheModel> WithCriteria { get; set; }
    }
}