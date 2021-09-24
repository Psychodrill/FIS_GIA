using GVUZ.DAL.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GVUZ.Web.ViewModels.InstitutionInfo
{
    /// <summary>
    /// Модель данных для загрузки документа ОО с привязкой к году
    /// </summary>
    public class InstitutionInfoUploadDocumentViewModel
    {
        /// <summary>
        /// Год
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Название документа
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Заполните наименование документа")]
        public string Name { get; set; }

        /// <summary>
        /// Загружаемый файл
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Необходимо выбрать файл")]
        public HttpPostedFileBase UploadedFile { get; set; }

        public AttachmentCreateDto GetDto()
        {
            return UploadedFile != null ?
                new AttachmentCreateDto
                {
                    DisplayName = Name,
                    FileName = UploadedFile.FileName,
                    Content = UploadedFile.InputStream,
                    ContentLength = UploadedFile.ContentLength,
                    ContentType = UploadedFile.ContentType
                } : null;
        }
    }
}