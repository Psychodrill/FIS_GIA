using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class Institution
    {
        public Institution()
        {
            AdmissionVolume = new HashSet<AdmissionVolume>();
            AllowedDirections = new HashSet<AllowedDirections>();
            Application = new HashSet<Application>();
            Campaign = new HashSet<Campaign>();
            CompetitiveGroup = new HashSet<CompetitiveGroup>();
            CompetitiveGroupTarget = new HashSet<CompetitiveGroupTarget>();
            Entrant1 = new HashSet<Entrant1>();
            ImportPackage = new HashSet<ImportPackage>();
            InstitutionAccreditation = new HashSet<InstitutionAccreditation>();
            InstitutionDirectionRequest = new HashSet<InstitutionDirectionRequest>();
            InstitutionDocuments = new HashSet<InstitutionDocuments>();
            InstitutionFounderToInstitutions = new HashSet<InstitutionFounderToInstitutions>();
            InstitutionItem = new HashSet<InstitutionItem>();
            InstitutionLicense = new HashSet<InstitutionLicense>();
            InstitutionProgram = new HashSet<InstitutionProgram>();
            OlympicTypeProfileCoOrganizer = new HashSet<OlympicTypeProfile>();
            OlympicTypeProfileOrgOlympicEnter = new HashSet<OlympicTypeProfile>();
            OlympicTypeProfileOrganizer = new HashSet<OlympicTypeProfile>();
            OrderOfAdmission = new HashSet<OrderOfAdmission>();
            PreparatoryCourse = new HashSet<PreparatoryCourse>();
            RequestDirection = new HashSet<RequestDirection>();
            UserPolicy = new HashSet<UserPolicy>();
        }

        public int InstitutionId { get; set; }
        public short InstitutionTypeId { get; set; }
        public string FullName { get; set; }
        public string BriefName { get; set; }
        public int? FormOfLawId { get; set; }
        public int? RegionId { get; set; }
        public string Site { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public bool HasMilitaryDepartment { get; set; }
        public bool HasHostel { get; set; }
        public int? HostelCapacity { get; set; }
        public bool HasHostelForEntrants { get; set; }
        public int? HostelAttachmentId { get; set; }
        public string Inn { get; set; }
        public string Ogrn { get; set; }
        public DateTime? AdmissionStructurePublishDate { get; set; }
        public DateTime? ReceivingApplicationDate { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int? EsrpOrgId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string OwnerDepartment { get; set; }
        public int? MainEsrpOrgId { get; set; }
        public int? FounderEsrpOrgId { get; set; }
        public int? StatusId { get; set; }
        public string City { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsFilial { get; set; }
        public string LawAddress { get; set; }
        public string Kpp { get; set; }
        public string EiisId { get; set; }
        public bool? HasDisabilityEntrance { get; set; }
        public bool? ForeignEduDocSelfRecognition { get; set; }
        public bool HasAdmissionDeny { get; set; }

        public virtual FormOfLaw FormOfLaw { get; set; }
        public virtual Attachment HostelAttachment { get; set; }
        public virtual InstitutionType InstitutionType { get; set; }
        public virtual RegionType Region { get; set; }
        public virtual ICollection<AdmissionVolume> AdmissionVolume { get; set; }
        public virtual ICollection<AllowedDirections> AllowedDirections { get; set; }
        public virtual ICollection<Application> Application { get; set; }
        public virtual ICollection<Campaign> Campaign { get; set; }
        public virtual ICollection<CompetitiveGroup> CompetitiveGroup { get; set; }
        public virtual ICollection<CompetitiveGroupTarget> CompetitiveGroupTarget { get; set; }
        public virtual ICollection<Entrant1> Entrant1 { get; set; }
        public virtual ICollection<ImportPackage> ImportPackage { get; set; }
        public virtual ICollection<InstitutionAccreditation> InstitutionAccreditation { get; set; }
        public virtual ICollection<InstitutionDirectionRequest> InstitutionDirectionRequest { get; set; }
        public virtual ICollection<InstitutionDocuments> InstitutionDocuments { get; set; }
        public virtual ICollection<InstitutionFounderToInstitutions> InstitutionFounderToInstitutions { get; set; }
        public virtual ICollection<InstitutionItem> InstitutionItem { get; set; }
        public virtual ICollection<InstitutionLicense> InstitutionLicense { get; set; }
        public virtual ICollection<InstitutionProgram> InstitutionProgram { get; set; }
        public virtual ICollection<OlympicTypeProfile> OlympicTypeProfileCoOrganizer { get; set; }
        public virtual ICollection<OlympicTypeProfile> OlympicTypeProfileOrgOlympicEnter { get; set; }
        public virtual ICollection<OlympicTypeProfile> OlympicTypeProfileOrganizer { get; set; }
        public virtual ICollection<OrderOfAdmission> OrderOfAdmission { get; set; }
        public virtual ICollection<PreparatoryCourse> PreparatoryCourse { get; set; }
        public virtual ICollection<RequestDirection> RequestDirection { get; set; }
        public virtual ICollection<UserPolicy> UserPolicy { get; set; }
    }



    /// <summary>
    /// Статус организации
    /// </summary>
    public enum OrganizationStatus
    {
        /// <summary>
        /// Действующая организация
        /// </summary>
        Operating = 1,

        /// <summary>
        /// Реорганизованная организация
        /// </summary>
        Reorganized = 2,

        /// <summary>
        /// Ликвидированная организация
        /// </summary>>
        Liquidated = 3,

        /// <summary>
        /// Отсутсвует лицензия
        /// </summary>>
        WithoutLicense = 5
    }

    /// <summary>
    /// Организационно-правовая форма
    /// </summary>
    public enum OPF
    {
        /// <summary>
        /// Негосударственный
        /// </summary>
        Private = 1,

        /// <summary>
        /// Государственный
        /// </summary>
        State = 0,

        /// <summary>
        /// Неизвестен (не удалось определить)
        /// </summary>
        Undefinded = -1
    }


}
