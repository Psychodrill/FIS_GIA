namespace Ege.Check.Logic.Models.Staff
{
    using System;
    using System.Collections.Generic;
    using Ege.Check.Logic.Models.Json;
    using Newtonsoft.Json;

    /// <summary>
    ///     Список экзаменов для даты
    /// </summary>
    public class ExamList
    {
        /// <summary>
        ///     Дата экзаменов
        /// </summary>
        [JsonConverter(typeof(OnlyDateConverter))]
        public DateTime Date { get; set; }

        /// <summary>
        ///     Экзамены
        /// </summary>
        public IEnumerable<ExamListElement> Exams { get; set; }
    }
}