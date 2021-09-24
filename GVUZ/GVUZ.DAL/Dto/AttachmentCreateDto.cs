using System.IO;

namespace GVUZ.DAL.Dto
{
    /// <summary>
    /// Данные добавляемого файла-вложения
    /// </summary>
    public class AttachmentCreateDto
    {
        /// <summary>
        /// Наименование файла
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Отображаемое название файла
        /// </summary>
        public string DisplayName { get; set; }
        
        /// <summary>
        /// Данные файла
        /// </summary>
        public Stream Content { get; set; }

        /// <summary>
        /// Mime-тип
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Размер данных файла (Content)
        /// </summary>
        public long ContentLength { get; set; }
    }
}
