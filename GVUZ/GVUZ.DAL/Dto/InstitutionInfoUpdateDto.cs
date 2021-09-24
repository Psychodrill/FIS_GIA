using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.DAL.Dto
{
    public class InstitutionInfoUpdateDto : InstitutionInfoDto
    {
        /// <summary>
        /// Новый файл общежития
        /// </summary>
        public AttachmentCreateDto UploadedHostelFile { get; set; }

        /// <summary>
        /// Признак удаления существующего файла общежития
        /// </summary>
        public bool IsHostelFileDeleted { get; set; }

        /// <summary>
        /// Новый файл лицензии
        /// </summary>
        public AttachmentCreateDto UploadedLicenseFile { get; set; }

        /// <summary>
        /// Признак удаления существующего файла лицензии
        /// </summary>
        public bool IsLicenseFileDeleted { get; set; }

        /// <summary>
        /// Новый файл аккредитации
        /// </summary>
        public AttachmentCreateDto UploadedAccreditationFile { get; set; }

        /// <summary>
        /// Признак удаления существующего файла аккредитации
        /// </summary>
        public bool IsAccreditationFileDeleted { get; set; }

    }
}
