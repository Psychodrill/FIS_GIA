using System;
using System.Collections.Generic;

namespace GVUZ.DAL.Dto
{
    /// <summary>
    /// Данные ОО для отображения и редактирования на вкладке "Общие сведения"
    /// </summary>
    public class InstitutionInfoDto
    {
        private List<InstitutionInfoYearDocumentDto> _documents;

        public int InstitutionId { get; set; }

        /// <summary>
        /// Полное наименование ОО
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Краткое наименование ОО
        /// </summary>
        public string BriefName { get; set; }

        /// <summary>
        /// Организационно-правовая форма - Id
        /// </summary>
        public int? FormOfLawId { get; set; } 

        /// <summary>
        /// Организационно-правовая форма - наименование
        /// </summary>
        public string FormOfLawName { get; set; }

        /// <summary>
        /// Url веб-сайта ОО
        /// </summary>
        public string Site { get; set; }

        /// <summary>
        /// Регион - Id
        /// </summary>
        public int? RegionId { get; set; }

        /// <summary>
        /// Регион - наименование
        /// </summary>
        public string RegionName { get; set; }

        /// <summary>
        /// Город - наименование
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Телефон
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Фaкс
        /// </summary>
        public string Fax { get; set; }

        /// <summary>
        /// Id лицензии
        /// </summary>
        public int? LicenseId { get; set; }

        /// <summary>
        /// Номер лицензии
        /// </summary>
        public string LicenseNumber { get; set; }

        /// <summary>
        /// Дата лицензии
        /// </summary>
        public DateTime? LicenseDate { get; set; }

        /// <summary>
        /// Id аккредитации
        /// </summary>
        public int? AccreditationId { get; set; }

        /// <summary>
        /// Номер аккредитации
        /// </summary>
        public string AccreditationNumber { get; set; }

        /// <summary>
        /// Количество мест
        /// </summary>
        public int? HostelCapacity { get; set; }

        /// <summary>
        /// Признак наличие военной кафедры
        /// </summary>
        public bool HasMilitaryDepartment { get; set; }

        /// <summary>
        /// Наличие общежития
        /// </summary>
        public bool HasHostel { get; set; }

        /// <summary>
        /// Наличие общежития для абитуриентов
        /// </summary>
        public bool HasHostelForEntrants { get; set; }
                
        /// <summary>
        /// Прикрепленный документ лицензии
        /// </summary>
        public InstitutionInfoDocumentDto LicenseDocument { get; set; }

        /// <summary>
        /// Прикрепленный документ аккредитации
        /// </summary>
        public InstitutionInfoDocumentDto AccreditationDocument { get; set; }

        /// <summary>
        /// Условия предоставления общежития
        /// </summary>
        public InstitutionInfoDocumentDto HostelDocument { get; set; }

        /// <summary>
        /// Признак создания условий для проведения ВИ для лиц с ОВЗ
        /// </summary>
        public bool HasDisabilityEntrance { get; set; }

        /// <summary>
        /// Прикрепленные документы (правила приема и т.п.)
        /// <see cref="InstitutionInfoDocumentDto"/>
        /// </summary>
        public List<InstitutionInfoYearDocumentDto> Documents
        {
            get { return _documents ?? (_documents = new List<InstitutionInfoYearDocumentDto>()); }
            set { _documents = value; }
        }
    }
}
