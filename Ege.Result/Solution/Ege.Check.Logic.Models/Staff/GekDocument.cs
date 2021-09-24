namespace Ege.Check.Logic.Models.Staff
{
    using System;
    using Ege.Check.Logic.Models.Json;
    using Newtonsoft.Json;

    /// <summary>
    ///     Документ ГЭК
    /// </summary>
    public class GekDocument
    {
        /// <summary>
        ///     Дата экзамена
        /// </summary>
        [JsonConverter(typeof(OnlyDateConverter))]
        public DateTime ExamDate { get; set; }

        /// <summary>
        ///     Название экзамена
        /// </summary>
        public string SubjectName { get; set; }

        /// <summary>
        ///     Номер документа
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        ///     Дата подписания
        /// </summary>
        [JsonConverter(typeof(OnlyDateConverter))]
        public DateTime CreateDate { get; set; }

        /// <summary>
        ///     Ссылка на документ
        /// </summary>
        public string Url { get; set; }
    }
}