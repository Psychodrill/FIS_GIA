using FogSoft.Web.Mvc;
using GVUZ.DAL.Dto;
using GVUZ.Web.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GVUZ.Web.ViewModels.InstitutionInfo
{
    /// <summary>
    /// View-модель для просмотра или редактирования общих сведений об ОО
    /// <see cref="InstutionController"/>, <see cref="InstitutionInfoDto"/>
    /// </summary>
    public class InstitutionInfoViewModel
    {
        private const int BaseDocumentYear = 2016;
        private List<InstitutionInfoYearDocumentViewModel> _documents;

        public InstitutionInfoViewModel()
        {
            DocumentYearsList = Enumerable.Range(BaseDocumentYear, 6)
                .Select(y => new SelectListItem
                             {
                                Value = y.ToString(),
                                Text = y.ToString()
                             })
                .Reverse().ToList();
        }

        public InstitutionInfoViewModel(InstitutionInfoDto dto) : this()
        {
            InstitutionId = dto.InstitutionId;
            FullName = dto.FullName;
            BriefName = dto.BriefName;
            FormOfLawId = dto.FormOfLawId;
            FormOfLawName = dto.FormOfLawName;
            Site = dto.Site;
            RegionId = dto.RegionId;
            RegionName = dto.RegionName;
            City = dto.City;
            Address = dto.Address;
            Phone = dto.Phone;
            Fax = dto.Fax;
            LicenseId = dto.LicenseId;
            LicenseNumber = dto.LicenseNumber;
            LicenseDate = dto.LicenseDate;
            AccreditationId = dto.AccreditationId;
            AccreditationNumber = dto.AccreditationNumber;
	        HasHostel = dto.HasHostel;
            HostelCapacity = dto.HostelCapacity;
            HasMilitaryDepartment = dto.HasMilitaryDepartment;
            HasHostelForEntrants = dto.HasHostelForEntrants;
            HasDisabilityEntrance = dto.HasDisabilityEntrance;

            if (dto.LicenseDocument != null)
            {
                LicenseDocument = new InstitutionInfoDocumentViewModel(dto.LicenseDocument);
            }

            if (dto.AccreditationDocument != null)
            {
                AccreditationDocument = new InstitutionInfoDocumentViewModel(dto.AccreditationDocument);
            }

            if (dto.HostelDocument != null)
            {
                HostelDocument = new InstitutionInfoDocumentViewModel(dto.HostelDocument);
            }

            Documents = dto.Documents.Select(doc => new InstitutionInfoYearDocumentViewModel(doc)).ToList();

        }

        

        public int InstitutionId { get; set; }

        /// <summary>
        /// Полное наименование ОО
        /// </summary>
        [DisplayName("Полное наименование")]
        public string FullName { get; set; }

        /// <summary>
        /// Краткое наименование ОО
        /// </summary>
        [DisplayName("Краткое наименование")]
        public string BriefName { get; set; }

        /// <summary>
        /// Организационно-правовая форма - Id
        /// </summary>
        [DisplayName("Организационно-правовая форма")]
        public int? FormOfLawId { get; set; }

        /// <summary>
        /// Организационно-правовая форма - наименование
        /// </summary>
        [DisplayName("Организационно-правовая форма")]
        public string FormOfLawName { get; set; }

        /// <summary>
        /// Url веб-сайта ОО
        /// </summary>
        [DisplayName("Сайт")]
        [Site]
        [StringLength(255)]
        public string Site { get; set; }

        /// <summary>
        /// Регион - Id
        /// </summary>
        [DisplayName("Регион")]
        public int? RegionId { get; set; }

        /// <summary>
        /// Регион - наименование
        /// </summary>
        [DisplayName("Регион")]
        public string RegionName { get; set; }

        /// <summary>
        /// Город - наименование
        /// </summary>
        [DisplayName("Город")]
        [StringLength(255)]
        public string City { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        [DisplayName("Адрес")]
        [StringLength(500)]
        public string Address { get; set; }

        /// <summary>
        /// Телефон
        /// </summary>
        [DisplayName("Телефон")]
        [StringLength(30)]
        public string Phone { get; set; }

        /// <summary>
        /// Фaкс
        /// </summary>
        [DisplayName("Факс")]
        [StringLength(30)]
        public string Fax { get; set; }

        /// <summary>
        /// Id лицензии
        /// </summary>
        public int? LicenseId { get; set; }

        /// <summary>
        /// Номер лицензии
        /// </summary>
        [DisplayName("Лицензия №")]
        public string LicenseNumber { get; set; }

        /// <summary>
        /// Дата лицензии
        /// </summary>
        [DisplayName("от")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime? LicenseDate { get; set; }

        /// <summary>
        /// Id аккредитации
        /// </summary>
        public int? AccreditationId { get; set; }

        /// <summary>
        /// Номер аккредитации
        /// </summary>
        [DisplayName("Аккредитация")]
        [LocalRequired()]
        [StringLength(500)]
        public string AccreditationNumber { get; set; }

        /// <summary>
        /// Количество мест
        /// </summary>
        [DisplayName("Количество мест")]
        [LocalRange(1, 99999)]
        public int? HostelCapacity { get; set; }

        /// <summary>
        /// Признак наличие военной кафедры
        /// </summary>
        [DisplayName("Наличие воен. кафедры")]
        public bool HasMilitaryDepartment { get; set; }

        /// <summary>
        /// Наличие общежития
        /// </summary>
        [DisplayName("Наличие общежития")]
        public bool HasHostel { get; set; }

        /// <summary>
        /// Наличие общежития для абитуриентов
        /// </summary>
        [DisplayName("Общежитие абитуриентам")]
        public bool HasHostelForEntrants { get; set; }

        /// <summary>
        /// Прикрепленный документ лицензии
        /// </summary>
        public InstitutionInfoDocumentViewModel LicenseDocument { get; set; }

        /// <summary>
        /// Прикрепленный документ аккредитации
        /// </summary>
        public InstitutionInfoDocumentViewModel AccreditationDocument { get; set; }

        /// <summary>
        /// Условия предоставления общежития
        /// </summary>
        public InstitutionInfoDocumentViewModel HostelDocument { get; set; }

        /// <summary>
        /// Загруженный файл лицензии
        /// </summary>
        public HttpPostedFileBase UploadedLicenseFile { get; set; }

        /// <summary>
        /// Признак удаления файла лицензии
        /// </summary>
        public bool IsLicenseFileDeleted { get; set; }
        
        /// <summary>
        /// Загруженный файл аккредитации
        /// </summary>
        public HttpPostedFileBase UploadedAccreditationFile { get; set; }

        /// <summary>
        /// Признак удаления файла аккредитации
        /// </summary>
        public bool IsAccreditationFileDeleted { get; set; }
        /// <summary>
        /// Загруженный файл по общежитию
        /// </summary>
        public HttpPostedFileBase UploadedHostelFile { get; set; }

        /// <summary>
        /// Признак удаления файла по общежитию
        /// </summary>
        public bool IsHostelFileDeleted { get; set; }

        /// <summary>
        /// Признак создания условий для проведения ВИ для лиц с ОВЗ
        /// </summary>
        [DisplayName("Создание условий проведения ВИ \n для лиц с ОВЗ")]
        public bool HasDisabilityEntrance { get; set; }

        /// <summary>
        /// Прикрепленные документы (правила приема и т.п.)
        /// <see cref="InstitutionInfoDocumentViewModel"/>
        /// </summary>
        [DisplayName("Документы")]
        public List<InstitutionInfoYearDocumentViewModel> Documents
        { 
            get { return _documents ?? (_documents = new List<InstitutionInfoYearDocumentViewModel>()); }
            set { _documents = value; }
        }

        /// <summary>
        /// Список годов для выбора при загрузке документа ОО
        /// </summary>
        public List<SelectListItem> DocumentYearsList { get; private set; }

        public InstitutionInfoUpdateDto GetDto()
        {
            return new InstitutionInfoUpdateDto
            {
                InstitutionId = InstitutionId,
                Address = Address,
                City = City,
                Phone = Phone,
                Fax = Fax,
                HasHostel = HasHostel,
                HasMilitaryDepartment = HasMilitaryDepartment,
                HasHostelForEntrants = HasHostelForEntrants,
                HostelCapacity = HostelCapacity.GetValueOrDefault() > 0 ? HostelCapacity.Value : default(int?),
                Site = Site,
                IsLicenseFileDeleted = IsLicenseFileDeleted,
                IsAccreditationFileDeleted = IsAccreditationFileDeleted,
                IsHostelFileDeleted = IsHostelFileDeleted,
                UploadedLicenseFile = UploadedLicenseFile.AsAttachmentCreateDto(),
                UploadedAccreditationFile = UploadedAccreditationFile.AsAttachmentCreateDto(),
                UploadedHostelFile = UploadedHostelFile.AsAttachmentCreateDto(),
                AccreditationNumber = AccreditationNumber,
                HasDisabilityEntrance = HasDisabilityEntrance
            };
        }
    }
}