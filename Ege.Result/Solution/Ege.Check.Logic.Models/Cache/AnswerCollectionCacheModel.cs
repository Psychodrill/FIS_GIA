namespace Ege.Check.Logic.Models.Cache
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Newtonsoft.Json;

    /// <summary>
    ///     Кэш-модель результатов экзамена
    ///     Ключ в кэше - (ParticipantRbdId, ExamGlobalId)
    /// </summary>
    [DataContract]
    public class AnswerCollectionCacheModel
    {
        /// <summary>
        ///     Первичный балл
        /// </summary>
        [DataMember]
        public int PrimaryMark { get; set; }

        /// <summary>
        ///     Тестовый балл
        /// </summary>
        [DataMember]
        public int TestMark { get; set; }

        /// <summary>
        ///     Оценка за сочинение (2 или 5) (зачет/незачет)
        /// </summary>
        [DataMember]
        public int Mark5 { get; set; }

        /// <summary>
        ///     Скрыты ли результаты экзамена
        /// </summary>
        [DataMember]
        public bool IsHidden { get; set; }

        /// <summary>
        ///     Ответы по частям А, Б, Ц, Д
        /// </summary>
        [DataMember]
        public ICollection<AnswerCacheModel> Answers { get; set; }

        /// <summary>
        ///     Бланки ответов
        /// </summary>
        [JsonProperty("CBlanks")]
        [DataMember(Name = "CBlanks")]
        public ICollection<BlankCacheModel> Blanks { get; set; }

        [JsonProperty("Blanks")]
        [DataMember(Name = "Blanks")]
        public ICollection<BlankClientModel> BlanksWithUrls { get; set; } 
    }
}
