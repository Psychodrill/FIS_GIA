using System;

namespace GVUZ.DAL.Dto
{
    /// <summary>
    /// Сведения о прикрепленном документе
    /// </summary>
    public class AttachmentDto
    {
        /// <summary>
        /// Id прикрепленного документа
        /// </summary>
        public int AttachmentId { get; set; }

        /// <summary>
        /// Наименование документа
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Название файла
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Mime-тип
        /// </summary>
        public string MimeType { get; set; }

        /// <summary>
        /// Глобальный идентификатор документа
        /// </summary>
        public Guid FileId { get; set; }
    }
}
