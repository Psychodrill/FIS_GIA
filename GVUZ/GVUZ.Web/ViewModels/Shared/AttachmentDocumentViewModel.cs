using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GVUZ.DAL.Dto;

namespace GVUZ.Web.ViewModels.Shared
{
    /// <summary>
    /// Модель для отображения сведений о прикрепленном документе <see cref="AttachmentDto"/>
    /// </summary>
    public class AttachmentDocumentViewModel
    {
        public AttachmentDocumentViewModel()
        {
        }

        public AttachmentDocumentViewModel(AttachmentDto dto)
        {
            AttachmentId = dto.AttachmentId;
            DisplayName = dto.DisplayName ?? dto.FileName;
            MimeType = dto.MimeType;
            FileId = dto.FileId;
            FileName = dto.FileName;
        }

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