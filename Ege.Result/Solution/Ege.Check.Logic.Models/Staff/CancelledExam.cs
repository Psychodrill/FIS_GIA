namespace Ege.Check.Logic.Models.Staff
{
    using System;
    using Ege.Check.Logic.Models.Json;
    using Newtonsoft.Json;

    public class CancelledExam : CancelledParticipantExam
    {
        /// <summary>
        ///     Регион
        /// </summary>
        public string RegionName { get; set; }

        /// <summary>
        ///     Дата экзамена
        /// </summary>
        [JsonConverter(typeof(OnlyDateConverter))]
        public DateTime Date { get; set; }

        /// <summary>
        ///     Предмет
        /// </summary>
        public string SubjectName { get; set; }
    }
}