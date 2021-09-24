namespace Ege.Hsc.Dal.Entities
{
    using System;

    /// <summary>
    /// Бланк для загрузки
    /// </summary>
    public class BlankToDownload
    {
        /// <summary>
        ///     Идентификатор бланка
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Номер бланка по порядку (для участника)
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        ///     Относительный путь на сервере
        /// </summary>
        public string RelativePath { get; set; }

        /// <summary>
        ///     Url сервера
        /// </summary>
        public string ServerUrl { get; set; }

        /// <summary>
        ///     Хеш фио участника, для которого выгружается бланк
        /// </summary>
        public string ParticipantHash { get; set; }

        /// <summary>
        ///     Номер документа участника
        /// </summary>
        public string ParticipantDocumentNumber { get; set; }

        /// <summary>
        ///     Глобальный идентификатор участника
        /// </summary>
        public Guid ParticipantRbdId { get; set; }
    }
}