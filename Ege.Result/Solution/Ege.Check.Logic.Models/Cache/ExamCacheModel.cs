namespace Ege.Check.Logic.Models.Cache
{
    using System;
    using Ege.Check.Logic.Models.Json;
    using Newtonsoft.Json;

    /// <summary>
    ///     Кэш-модель экзамена участника
    ///     хранится в кэше только в составе коллекции
    /// </summary>
    public class ExamCacheModel
    {
        /// <summary>
        ///     Идентификатор экзамена
        /// </summary>
        public int ExamId { get; set; }

        /// <summary>
        /// Идентификатор устного экзамена
        /// </summary>
        public int? OralExamId { get; set; }

        /// <summary>
        ///     Дата экзамена
        /// </summary>
        [JsonConverter(typeof(OnlyDateConverter))]
        public DateTime ExamDate { get; set; }

        /// <summary>
        /// Дата устного экзамена
        /// </summary>
        [JsonConverter(typeof(OnlyDateConverter))]
        public DateTime? OralExamDate { get; set; }

        /// <summary>
        ///     Предмет
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Устный предмет
        /// </summary>
        public string OralSubject { get; set; }

        /// <summary>
        ///     Тестовый балл
        /// </summary>
        public int TestMark { get; set; }

        /// <summary>
        ///     Оценка за сочинение (2 или 5) (зачет/незачет)
        /// </summary>
        public int Mark5 { get; set; }

        /// <summary>
        ///     Минимальный балл
        /// </summary>
        public int MinMark { get; set; }

        /// <summary>
        ///     Статус экзамена
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Статус устного экзамена
        /// </summary>
        public int? OralStatus { get; set; }

        /// <summary>
        ///     Подана ли апелляция
        /// </summary>
        public bool HasAppeal { get; set; }

        /// <summary>
        ///     Скрыт ли результат экзамена для участника
        /// </summary>
        public bool IsHidden { get; set; }

        /// <summary>
        ///     Есть ли ответы
        /// </summary>
        public bool HasResult { get; set; }

        /// <summary>
        /// Есть ли ответы на устную часть
        /// </summary>
        public bool HasOralResult { get; set; }

        /// <summary>
        ///     Снята ли галочка в кабинете РЦОИ
        /// </summary>
        public bool IsHiddenForRegion { get; set; }

        /// <summary>
        ///     Последний статус апеляции
        /// </summary>
        public int? AppealStatus { get; set; }

        /// <summary>
        ///     Является ли сочинением
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
    }
}