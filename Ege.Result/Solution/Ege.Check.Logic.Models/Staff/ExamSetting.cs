namespace Ege.Check.Logic.Models.Staff
{
    using System;
    using Ege.Check.Logic.Models.Json;
    using Newtonsoft.Json;

    /// <summary>
    ///     Настройка экзамена для региона
    /// </summary>
    public class ExamSetting
    {
        /// <summary>
        ///     Идентификатор экзамена
        /// </summary>
        public int ExamGlobalId { get; set; }

        /// <summary>
        ///     Название экзамена
        /// </summary>
        public string SubjectName { get; set; }

        /// <summary>
        ///     Дата экзамена
        /// </summary>
        [JsonConverter(typeof(OnlyDateConverter))]
        public DateTime ExamDate { get; set; }

        /// <summary>
        ///     Показывать ли результат
        /// </summary>
        public bool ShowResult { get; set; }

        /// <summary>
        ///     Показывать ли бланки
        /// </summary>
        public bool ShowBlank { get; set; }

        /// <summary>
        ///     Имеется ли в системе информация о документе ГЭК
        /// </summary>
        public bool HasGekDocument { get; set; }

        /// <summary>
        ///     Является ли сочинением/изложением
        /// </summary>
        public bool IsComposition { get; set; }
    }
}