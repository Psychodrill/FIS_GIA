namespace Ege.Hsc.Dal.Entities
{
    using System;

    /// <summary>
    ///     Очередь загрузок бланков с серверов регионов
    /// </summary>
    public class BlankDownload
    {
        /// <summary>
        ///     Идентификатор бланка
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Номер бланка
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        ///     Идентификатор участника
        /// </summary>
        public int ParticipantId { get; set; }

        /// <summary>
        ///     Идентификатор региона
        /// </summary>
        public int RegionId { get; set; }

        /// <summary>
        ///     Состояние загрузки
        /// </summary>
        public BlankDownloadState State { get; set; }

        /// <summary>
        ///     Относительный путь на сервере
        /// </summary>
        public string RelativePath { get; set; }

        /// <summary>
        /// Код-хэш бланка
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Дата экзамена
        /// </summary>
        public DateTime ExamDate { get; set; }

        /// <summary>
        /// Идентификатор предмета
        /// </summary>
        public int SubjectCode { get; set; }

        /// <summary>
        ///     Дата и время создания записи
        /// </summary>
        public DateTimeOffset CreateDate { get; set; }
    }
}