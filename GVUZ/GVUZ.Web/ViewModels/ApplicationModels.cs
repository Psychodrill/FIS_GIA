using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Script.Serialization;
using FogSoft.Web.Mvc;
using System.Globalization;
using System.Xml.Serialization;
using System.Web.Mvc;
using GVUZ.DAL.Dapper.Repository.Model.Olympics;
using GVUZ.DAL.Dapper.Repository.Model;
using GVUZ.Data.Helpers;
using GVUZ.DAL.Dapper.ViewModel.Dictionary;

namespace GVUZ.Web.ViewModels {

    public class ApplicationModel {
        [DisplayName("Действие")]
        public int? ApplicationID { get; set; }
        public int? WizardStepID { get; set; }
        public int? InstitutionID { get; set; }
        public bool IsCampaignFinished { get; set; }

        public string StatusDecision { get; set; }

        //Общие сведения	
        [DisplayName("№ заявления")]
        public string ApplicationNumber { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime RegistrationDateTime { get; set; }
        [DisplayName("Дата регистрации")]
        public string RegistrationDate { get { return RegistrationDateTime.ToString("dd.MM.yyyy"); } }
        [DisplayName("Фамилия")]
        public string EntrantLastName { get; set; }
        [DisplayName("Имя")]
        public string EntrantFirstName { get; set; }
        [DisplayName("Отчество")]
        public string EntrantMiddleName { get; set; }
        [DisplayName("Вид документа удостов. личность")]
        public string DocumentTypeName { get; set; }
        [DisplayName("Дата рождения")]
        public DateTime? DocumentBirthDate { get; set; }

        public int? Priority { get; set; }
        public string Uid { get; set; }
        public int NeedHostel { get; set; }


    }

    public class TargetOrganization {
        public int CompetitiveGroupTargetID { get; set; }
        public string CompetitiveGroupTargetName { get; set; }

        public int CompetitiveGroupItemID { get; set; }

        public int CompetitiveGroupTargetItemID { get; set; }
    }

    public class ForceAdmissionModel
    {
        public int ApplicationID { get; set; }
        public DateTime ApplicationRegistrationDate { get; set; }
        public int? ApplicationPriority { get; set; }
        public string ApplicationUid { get; set; }
        public bool ApplicationNeedHostel { get; set; }
        public string Comment { get; set; }
        public int ReasonID { get; set; }
        public List<ForceAdmissionAttachmentModel> Attachments { get; set; }
        public ApplicationPrioritiesViewModel ApplicationPriorities { get; set; }
    }

    public class ForceAdmissionAttachmentModel
    {
        public Guid AttachmentFileID { get; set; }
        public int AttachmentType { get; set; }
    }

    // ЕГЭ и Олимпиада
    public class AppResultsModel {

        public AppResultsModel()
        {
            ApplicationPriorities = new ApplicationPrioritiesViewModel();
        }
        public string userLogin { get; set; }
        public bool changePage { get; set; }

        public DateTime RegistrationDate { get; set; }
        public int? Priority { get; set; }
        public string Uid { get; set; }
        public bool NeedHostel { get; set; }

        public int Step { get; set; }

        public string StatusDecision { get; set; }

        //	метод поиска: “по ФИО и серии, номеру паспорта“
        public string method { get; set; }
        ///	идентификатор заявления
        public int ApplicationID { get; set; }
        ///	идентификатор документа типа “Свидетельство ЕГЭ”
        public int? doc { get; set; }
        ///	Регистрационный номер документа
        public string regNum { get; set; }
        ///	признак обновления таблицы ApplicationEntranceTestDocument
        public int refr { get; set; }

        ///	признак проверки на наличие результата ЕГЭ текущего года
        public int currentYear { get; set; }
        //	идентификатор документа (EntrantDocument.DocumentID) (далее – @DocId)
        public int? DocId { get; set; }
        public int? DocTypeID { get; set; }
        //	Признак типа олимпиады (далее – @Typ, 0 – Всероссийская олимпиада школьников / 1 – олимпиада школьников)
        public bool Typ { get; set; }
        //	Номер олимпиады в перечне (OlympicType.OlympicNumber) (далее - @OlympNumber, может быть NULL, если @Typ = 0)
        public int? OlympicID { get; set; }
        public int? OlympNumber { get; set; }
        public int? OlympicTypeProfileID { get; set; }

        public bool res { get; set; }
        public int EtiId { get; set; }


        public object LastEgeDocumentsCheckDate { get; set; }

        public object LastCheckDate { get; set; }
        public bool IsDisabled { get; set; }
        public bool IsDistant { get; set; }
        public int? IsDisabledDocumentID { get; set; }
        public string DistantPlace { get; set; }
        public string ApplicationNumber { get; set; }

        public ApplicationPrioritiesViewModel ApplicationPriorities { get; set; }
    }

    /// <summary>
    /// Модель для результата получения хранимых процедур (проверки)
    /// </summary>
    public class SPResult {
        public bool returnValue { get; set; }
        public string errorMessage { get; set; }
        public string violationMessage { get; set; }
        public int violationId { get; set; }
    }

    public class ApplicationEditModel {
        [DisplayName("Действие")]
        public int? ApplicationID { get; set; }
        public int? WizardStepID { get; set; }
        public int? InstitutionID { get; set; }
        public bool IsCampaignFinished { get; set; }
        public int? StatusID { get; set; }

        public string EntrantFullName { get; set; }

        //Общие сведения	
        [DisplayName("№ заявления")]
        public string ApplicationNumber { get; set; }
        public DateTime RegistrationDateTime { get; set; }
        [DisplayName("Дата регистрации")]
        public string RegistrationDate { get { return RegistrationDateTime.ToString("dd.MM.yyyy"); } }
        [DisplayName("Фамилия")]
        public string EntrantLastName { get; set; }
        [DisplayName("Имя")]
        public string EntrantFirstName { get; set; }
        [DisplayName("Отчество")]
        public string EntrantMiddleName { get; set; }
        [DisplayName("Вид документа удостов. личность")]
        public string DocumentTypeName { get; set; }
        [DisplayName("Дата рождения")]
        public DateTime? DocumentBirthDate { get; set; }
    }

    public class ApplicationWzModel {
        [DisplayName("Действие")]
        public int? ApplicationID { get; set; }
        public int? InstitutionID { get; set; }
        public int? WizardStepID { get; set; }

        public bool IsCampaignFinished { get; set; }

        public string ApplicationNumber { get; set; }
        public string EntrantFullName { get; set; }
    }

    public class ApplicationRespModel
    {
        public int? ApplicationID { get; set; }
        public int? RowUpdated { get; set; }
        public string Error { get; set; }
    }


    #region Wz1

    public class ApplicationWz1ViewModel {
        // Исходник GVUZ.Web.Portlets.Entrants.PersonalRecordsDataViewModel
        public int? ApplicationID { get; set; }
        public int? InstitutionID { get; set; }
        public int? WizardStepID { get { return 1; } }
        public int? EntrantID { get; set; }
        public int? StatusID { get; set; }
        public List<GVUZ.Model.Entrants.UniDocuments.IdentityDocumentType> ListIdentityDocumentType { get; set; }

        [DisplayName("Фамилия")]
        [LocalRequired]
        [StringLength(255)]
        public string LastName { get; set; }
        [DisplayName("Имя")]
        [LocalRequired]
        [StringLength(255)]
        public string FirstName { get; set; }
        [DisplayName("Отчество")]
        [StringLength(255)]
        public string MiddleName { get; set; }

        public string EntrantFullName { get { return (LastName + " " + FirstName + " " + MiddleName); } }

        [DisplayName("Пол")]
        [LocalRequired]
        public int? GenderID { get; set; }

        [DisplayName("Дата рождения")]
        [LocalRequired]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        [Date("<today-7y")]
        [Date(">today-100y")]
        public DateTime? BirthDate { get; set; }

        public int IdentityDocumentID { get; set; }

        [DisplayName("Вид документа удостов. личность")]
        [LocalRequired]
        public int DocumentTypeID { get; set; }

        //[DisplayName("Серия")]
        //[LocalRequired]
        [StringLength(20)]
        public string DocumentSeries { get; set; }

        [DisplayName("Серия / № документа")]
        [LocalRequired]
        [StringLength(50)]
        public string DocumentNumber { get; set; }

        [DisplayName("Кем выдан")]
        [LocalRequired]
        [StringLength(200)]
        public string DocumentOrganization { get; set; }

        [DisplayName("Страна выдачи документа")]
        [LocalRequired]
        [StringLength(200)]
        public int? ReleaseCountryID { get; set; }

        [DisplayName("Место выдачи документа")]
        [LocalRequired]
        [StringLength(200)]
        public string ReleasePlace { get; set; }

        [DisplayName("Дата выдачи")]
        [LocalRequired]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        [Date(">today-100y")]
        [Date("<=today")]
        public DateTime? DocumentDate { get; set; }

        [StringLength(7)]
        [DisplayName("Код подразделения")]
        [LocalRequired]
        public string SubdivisionCode { get; set; }

        [DisplayName("Ссылка на документ удостов. личность")]
        //[LocalRequired]
        public int? DocumentAttachmentID { get; set; }
        [DisplayName("Ссылка на документ удостов. личность")]
        public Guid? AttachmentFileID { get; set; }
        public string DocumentAttachmentName { get; set; }

        [DisplayName("Гражданство")]
        [LocalRequired]
        public int NationalityID { get; set; }

        [DisplayName("Место рождения")]
        //[LocalRequired]
        [StringLength(200)]
        public string BirthPlace { get; set; }

        [DisplayName("СНИЛС")]
        //[LocalRequired]
        [StringLength(11)]
        public string SNILS { get; set; }

        [DisplayName("О себе могу сообщить следующее")]
        public string CustomInformation { get; set; }

        [DisplayName("Срок обучения (мес.)")]
        [Range(1, 90)]
        public int EducationTerm { get; set; }

        [DisplayName("Дата начала обучения")]
        [LocalRequired]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        [Date(">today-100y")]
        [Date("<=today")]
        public DateTime? DateStartEdu { get; set; }

        [DisplayName("Дата окончания")]
        [LocalRequired]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        [Date(">today-100y")]
        [Date("<=today")]
        public DateTime? DateEndEdu { get; set; }

        [DisplayName("Требуется общежитие")]
        public bool NeedHostel { get; set; }

        public IEnumerable GenderList { get; set; }
        public IEnumerable IdentityDocumentList { get; set; }
        public IEnumerable NationalityList { get; set; }
        public IEnumerable ReleaseCountryList { get; set; }
        public IEnumerable SelectedCitizenships { get; set; }
        public int[] RussianDocs { get; set; }

        public int MaxFileSize { get; set; }

        public bool ForceAddData { get; set; }
        public bool IsEdit { get { return EntrantID > 0 && !ForceAddData; } }

        public bool DisableDocumentDataEditing { get; set; }

        [DisplayName("Регион")]
        public int RegionID { get; set; }

        [DisplayName("Тип населенного пункта")]
        public int? TownTypeID { get; set; }

        [DisplayName("Адрес")]
        public string Address { get; set; }

        IEnumerable<SelectorItem> regions;
        public IEnumerable<SelectorItem> Regions
        {
            get
            {
                if (regions == null)
                {
                    regions = new ApplicationRepository().GetRegionTypeAll().Select(s =>
                      new SelectorItem
                      {
                          Id = s.RegionId,
                          Name = s.Name
                      });
                }
                return regions;
            }
        }

        IEnumerable<SelectorItem> townTypes;
        public IEnumerable<SelectorItem> TownTypes
        {
            get
            {
                if (townTypes == null)
                {
                    townTypes = new ApplicationRepository().GetTownTypeAll().Select(s =>
                      new SelectorItem
                      {
                          Id = s.TownTypeID,
                          Name = s.Name
                      });
                    townTypes = new List<SelectorItem>()
                    {
                        new SelectorItem
                        {
                            Id = 0,
                            Name = "Не выбрано"
                        }
                    }.Concat(townTypes);
                }
                return townTypes;
            }
        }

        [DisplayName("Электронный адрес")]
        public string Email { get; set; }

        public bool IsFromKrym { get; set; }
        public int? IsFromKrymEntrantDocumentID { get; set; }
    }

    /// <summary>Модель для создания на 2-м этапе Визарда.</summary>
    public class ApplicationWz1Model {
        // Исходник GVUZ.Web.Portlets.Entrants.PersonalRecordsDataViewModel
        public int? ApplicationID { get; set; }
        public int? InstitutionID { get; set; }
        public int? WizardStepID { get { return 1; } }
        public int? Step { get; set; }

        public int? EntrantID { get; set; }

        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string SNILS { get; set; }
        public int GenderID { get; set; }
        public DateTime BirthDate { get; set; }

        public int DocumentTypeID { get; set; }
        public string DocumentSeries { get; set; }
        public string DocumentNumber { get; set; }
        public string DocumentOrganization { get; set; }
        public DateTime? DocumentDate { get; set; }
        public string SubdivisionCode { get; set; }

        public int NationalityID { get; set; }
        public string BirthPlace { get; set; }
        public string CustomInformation { get; set; }
        public bool NeedHostel { get; set; }
        public int? DocumentAttachmentID { get; set; }

        public Guid AttachmentFileID { get; set; }

        public int RegionID { get; set; }
        public int? TownTypeID { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public bool IsFromKrym { get; set; }
        public int? IsFromKrymEntrantDocumentID { get; set; }
        public IEnumerable<int> SelectedCitizenships { get; set; }
        public string ReleasePlace { get; set; }
        public int ReleaseCountryID { get; set; }
    }

    public class EntrantDocumentModel {
        public EntrantDocumentModel() { }
        public int EntrantDocumentID { get; set; }

        [DisplayName("Вид документа удостов. личность")]
        [LocalRequired]
        public int DocumentTypeID { get; set; }

        [DisplayName("Вид документа")]
        public string DocumentTypeName { get; set; }

        [DisplayName("Серия и номер документа")]
        public string DocumentSeriesNumber { get { return (DocumentSeries + " " + DocumentNumber); } }

        [DisplayName("Серия документа")]
        [LocalRequired]
        [StringLength(20)]
        public string DocumentSeries { get; set; }

        [DisplayName("Номер документа")]
        [LocalRequired]
        [StringLength(50)]
        public string DocumentNumber { get; set; }

        [DisplayName("Дата выдачи")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        [Date(">today-100y")]
        [Date("<=today")]
        public DateTime? DocumentDateTime { get; set; }
        public string DocumentDate {
            get {
                //для свидетельства ЕГЭ только год
                //DocumentDate=DocDate==null?"":DocDate.Value.ToString(DocTypeID!=2?"dd.MM.yyyy":"yyyy",CultureInfo.InvariantCulture);
                return (DocumentDateTime == null) ? "" : DocumentDateTime.Value.ToString("dd.MM.yyyy");
            }
        }

        [DisplayName("Кем выдан")]
        public string DocumentOrganization { get; set; }

        [DisplayName("Ссылка на документ удостов. личность")]
        public int? DocumentAttachmentID { get; set; }
        [DisplayName("Ссылка на документ удостов. личность")]
        public Guid? AttachmentFileID { get; set; }
        public string DocumentAttachmentName { get; set; }
    }

    public class EntrantDocumentsViewModel {
        // C:\Projects\fbdosh\GVUZ\GVUZ.Model\Entrants\Documents\DocumentShortInfoViewModel.cs
        //Documents
    }
    #endregion

    #region Wz2

    public class DocumentListViewModel {
        public int EntrantDocumentID { get; set; }
        public int EntrantID { get; set; }
        public int DocumentTypeID { get; set; }
        public int ApplicationID { get; set; }

        [DisplayName("Тип документа")]
        public string DocumentTypeName { get; set; }

        [DisplayName("Серия и номер документа")]
        public string DocumentSeriesNumber { get; set; }

        [DisplayName("Дата выдачи")]
        public string DocumentDate { get; set; }

        [DisplayName("Кем выдан")]
        public string DocumentOrganization { get; set; }

        [DisplayName("Ссылка на документ")]
        public Guid DocumentAttachmentID { get; set; }

        [DisplayName("Ссылка на документ удостов. личность")]
        public int? AttachmentID { get; set; }
        [DisplayName("Ссылка на документ удостов. личность")]
        public Guid? AttachmentFileID { get; set; }
        public string AttachmentName { get; set; }


        [DisplayName("Дата предоставления")]
        public string OriginalReceivedDate { get; set; }

        [DisplayName("Оригиналы/заверенные копии предоставлены / Заявление с обязательством предоставления оригинала в течение первого учебного года/ЕПГУ")]
        public bool OriginalReceived { get; set; }

        public bool CanBeModified { get; set; }
        public bool ShowWarnBeforeModifying { get; set; }
        public bool CanBeDetached { get; set; }

        public bool CanBeDeleted { get; set; }
        //ToDo
        public bool ShowWarnWhenDeleted { get; set; }
    }

    public class DocumentType
    {
        public int TypeID { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
    }

    public class ApplicationWz2ViewModel {
        public int ApplicationID { get; set; }
        public int? WizardStepID { get; set; }
        public int EntrantID { get; set; }
        public int StatusID { get; set; }

        public int ApplicationStatus { get; set; }
        public bool ApplicationIncludedInOrder { get; set; }

        public bool ShowDenyMessage { get; set; }
        public string StepDirection { get; set; }

        public DocumentListViewModel DocListInfo;

        [ScriptIgnore]
        public List<DocumentListViewModel> ExistingDocuments { get; set; }
        [ScriptIgnore]
        public List<DocumentListViewModel> AttachedDocuments { get; set; }      

        [ScriptIgnore]
        public List<DocumentType> DocumentTypes { get; set; }

        public ApplicationWz2ViewModel() {
            DocumentTypes = new List<DocumentType>();
            ExistingDocuments = new List<DocumentListViewModel>();
            AttachedDocuments = new List<DocumentListViewModel>();
        }
    }
    #endregion

    #region Wz4

    public class ApplicationWz4ViewModel {
        public int? ApplicationID { get; set; }

        public int? EntrantID { get; set; }

        public int? StatusID { get; set; } //movaxcs(1705)

        public int? IAID { get; set; }

        public int? CheckWorks { get; set; }

        public bool CheckAchievementsSum { get; set; }

        public decimal IndividualAchivementsMark { get; set; }

        public string IndividualAchivementsMarkStr { get { return IndividualAchivementsMark.ToString("0.##"); } }

        public ApplicationWz4ViewModel() {
            Items = new List<IndividualAchivementViewModel>();
            IAchievements = new List<InstitutionAchievements>();
        }

        public decimal? MaxIAValues { get; set; }

        public List<IndividualAchivementViewModel> Items { get; set; }
        public List<InstitutionAchievements> IAchievements { get; set; }

        public IndividualAchivementViewModel FakedAchivement { get { return null; } }

        public class InstitutionAchievements {
            public int? IdAchievement { get; set; }
            public string UID { get; set; }
            public string Name { get; set; }
            public int? IdCategory { get; set; }
            public Decimal? MaxValue { get; set; }
            public int? CampaignID { get; set; }
            public int? EducationLevelID { get; set; }
        }

        public class IndividualAchivementViewModel {
            public int? IAID { get; set; }

            public int? ApplicationID { get; set; }

            [StringLength(50)]
            [DisplayName("UID")]
            public string UID { get; set; }

            [StringLength(500)]
            [DisplayName("Наименование индивидуального достижения")]
            public string IAName { get; set; }

            [DisplayName("Дополнительный балл")]
            public decimal? IAMark { get; set; }

            [DisplayName("Преимущественное право")]
            public bool? isAdvantageRight { get; set; }

            public string IAMarkString { get; set; }

            public IADocumentViewModel IADocument { get; set; }


            public int? IdAchievement { get; set; }

            public int IADocumentID { get; set; } // это свойство нужно для передачи ID подтверждающего документа при создании ИД.

            [XmlIgnore]
            public int? EntrantDocumentID { get; set; }

            [DisplayName("Сведения о подтверждающем документе")]
            public string IADocumentDisplay {
                get {
                    if(IADocument != null) {
                        if(IADocumentID > 0) {
                            return string.Format("{0} № {1}{2} от {3:dd.MM.yyyy}", IADocument.DocumentTypeNameText,
                                IADocument.DocumentSeries, IADocument.DocumentNumber, IADocument.DocumentDate);
                        }
                    }
                    return null;
                }
            }

            public class IADocumentViewModel {
                public IADocumentViewModel() { DocumentTypeID = 15; }

                /// Для сохранения документа для заявления
                [XmlIgnore]
                [ScriptIgnore]
                public int? ApplicationID { get; set; }

                [XmlIgnore]
                public int? EntrantDocumentID { get; set; }

                [DisplayName("Тип документа")]
                [XmlIgnore]
                public int DocumentTypeID { get; set; }

                [StringLength(50)]
                public string DocumentNumber { get; set; }

                public string DocumentTypeName { get; set; }
                public string DocumentSeries { get; set; }

                public DateTime? DocumentDate { get; set; }

                [LocalRequired]
                [DisplayName("Кем выдан")]
                [StringLength(500)]
                public string DocumentOrganization { get; set; }

                [DisplayName("Дополнительные сведения")]
                [StringLength(8000)]
                public string AdditionalInfo { get; set; }

                [DisplayName("Тип документа")]
                [LocalRequired]
                [StringLength(500)]
                public string DocumentTypeNameText { get; set; }

                public string Description {
                    get {
                        if(EntrantDocumentID > 0) {
                            return string.Format("{0} № {1}{2} от {3:dd.MM.yyyy}", DocumentTypeNameText, DocumentSeries,
                                DocumentNumber, DocumentDate);
                        }
                        return "";
                    }
                }
            }

            public int InstitutionID { get; set; }
        }

        public int? InstitutionID { get; set; }
        public short EducationLevelID { get; set; }
    }

    #endregion

    #region Wz0
    /// <summary>Модель для создания на 1-м этапе Визарда.</summary>
    public class ApplicationWz0ViewModel
    {
        [DisplayName("Действие")]
        public int? ApplicationID { get; set; }
        public int? WizardStepID { get { return 0; } }
        public int? InstitutionID { get; set; }
        public int? StatusID { get; set; }
        public List<GVUZ.Model.Entrants.UniDocuments.IdentityDocumentType> ListIdentityDocumentType { get; set; }

        [DisplayName("Приемная кампания")]
        [LocalRequired]
        public int CampaignID { get; set; }
        public IEnumerable Campaigns { get; set; }

        [DisplayName("Конкурс")]
        [LocalRequired]
        public int DisplayCompetitiveGroupID { get; set; }
        public string[] CompetitiveGroups { get; set; }

        [DisplayName("Направления подготовки")]
        [LocalRequired]
        public int DisplayDirectionID { get; set; }
        public string[] SelectedDirectionIDs { get; set; }

        [DisplayName("Номер заявления ОО")]
        [LocalRequired]
        public string ApplicationNumber { get; set; }

        [DisplayName("Дата регистрации")]
        [LocalRequired]
        [Date(">today-100y")]
        [Date("<=today")]
        public DateTime RegistrationDate { get; set; }

        //[LocalRequired]
        [DisplayName("Серия документа")]
        [StringLength(20)]
        public string DocumentSeries { get; set; }

        [DisplayName("Серия / № документа, удостоверяющего личность")]
        [LocalRequired]
        [StringLength(50)]
        public string DocumentNumber { get; set; }

        [DisplayName("Вид документа удостов. личность")]
        [LocalRequired]
        public int IdentityDocumentTypeID { get; set; }

        public IEnumerable IdentityDocumentList { get; set; }

        [DisplayName("Приоритет заявления (только в случае нескольких заявлений от одного абитуриента)")]
        public int? Priority { get; set; }
        public int? EntrantId { get; set; }

        public Dictionary<string, int> CompetitiveGroupEducationForms { get; set; }

        
        [DisplayName("Уровень образования")]
        public byte ItemTypeID { get; set; }

        IEnumerable<SelectorItem> admissionItemTypes = new List<SelectorItem>();
        public IEnumerable<SelectorItem> AdmissionItemTypes
        {
            get
            {
                //if (admissionItemTypes == null)
                //{
                //    admissionItemTypes = new ApplicationRepository().GetAdmissionItemTypeAll().Select(s =>
                //      new SelectorItem
                //      {
                //          Id = s.ItemTypeID,
                //          Name = s.Name
                //      });
                //    admissionItemTypes = new List<SelectorItem>()
                //    {
                //        new SelectorItem
                //        {
                //            Id = 0,
                //            Name = "Не выбрано"
                //        }
                //    }.Concat(admissionItemTypes);
                //}
                return admissionItemTypes;
            }
        }

        IEnumerable<SelectListItem> competitions = new List<SelectListItem>();
        public string[] SelectedCompetitions { get; set; }
        public IEnumerable<SelectListItem> Competitions { get { return competitions; } }

    }

    public class ApplicationWz0Model
    {
        public int? ApplicationID { get; set; }
        public int? WizardStepID { get { return 0; } }
        public int? InstitutionID { get; set; }
        public int? EntrantId { get; set; }
        public int? EntrantDocumentID { get; set; }
        public bool? EntrantIsNew { get; set; }

        [StringLength(50)]
        public string ApplicationNumber { get; set; }
        public DateTime RegistrationDate { get; set; }

        public string DocumentSeries { get; set; }
        public string DocumentNumber { get; set; }
        public int IdentityDocumentTypeID { get; set; }

        public int? Priority { get; set; }

        public int CampaignID { get; set; }

        public int[] SelectedCompetitiveGroupIDs { get; set; }
        public string[] SelectedDirectionIDs { get; set; }
        public string[] SelectedParentDirectionIDs { get; set; }

        public ApplicationPrioritiesViewModel Priorities { get; set; }

        [DisplayName("Организация целевого приема")]
        public int SelectedTargetOrganizationIDO { get; set; }
        [DisplayName("Организация целевого приема")]
        public int SelectedTargetOrganizationIDOZ { get; set; }
        [DisplayName("Организация целевого приема")]
        public int SelectedTargetOrganizationIDZ { get; set; }

        public bool CheckForExistingBeforeCreate { get; set; }
        public bool CheckUniqueBeforeCreate { get; set; }
        public bool CheckZerozBeforeCreate { get; set; }
    }

    #endregion

    #region Wz5

    public class ApplicationWz5ViewModel
    {
        public class Wz5SendingViewModel : Wz5InfoViewModel
        {
            public Wz5SendingViewModel()
                : base()
            {
                ForcedAdmissionReasons = new List<ForcedAdmissionReasonItemModel>();
            }

            public int ApplicationID { get; set; }
            public int InstitutionID { get; set; }
            public int StatusID { get; set; }

            [DisplayName("ФИО")]
            public string FIO { get; set; }
            [DisplayName("Дата рождения")]
            public string DOB { get; set; }
            [DisplayName("Документ, удостоверяющий личность")]
            public string IdentityDocument { get; set; }
            [DisplayName("Пол")]
            [LocalRequired]
            public int GenderID { get; set; }
            [DisplayName("Пол")]
            public string Gender { get; set; }
            public IEnumerable Genders { get; set; }
            [DisplayName("Гражданство")]
            public string Citizen { get; set; }
            [DisplayName("Место рождения")]
            public string POB { get; set; }
            [DisplayName("Идентификатор в БД ОО (UID)")]
            [StringLength(200)]
            public string Uid { get; set; }
            [DisplayName("О себе дополнительно сообщаю")]
            public string CustomInformation { get; set; }

            [DisplayName("Подтверждаю подачу заявления не более чем в 5 ВУЗов")]
            public bool ApproveInstitutionCount { get; set; }

            [DisplayName("Нуждаюсь в общежитии")]
            public bool NeedHostel { get; set; }

            public bool FirstHigherEducation { get; set; }
            [DisplayName("Даю согласие на обработку моих персональных данных в порядке, установленном Федеральным законом Российской Федерации от 27.07.2006 № 152-Ф3")]
            public bool ApprovePersonalData { get; set; }

            public bool FamiliarWithLicenseAndRules { get; set; }

            [DisplayName("Дата регистрации")]
            [LocalRequired]
            [Date(">today-100y")]
            [Date("<=today")]
            public DateTime? RegistrationDate { get; set; }

            [DisplayName("С условиями выбора специальности ознакомлен")]
            public bool FamiliarWithAdmissionType { get; set; }
            [DisplayName("Дата предоставления подлинника документа об образовании")]
            public string EducationDocumentDate { get; set; }
            [DisplayName("С датой предоставления подлинника документа об образовании ознакомлен")]
            public bool FamiliarWithOriginalDocumentDeliveryDate { get; set; }
            public bool ShowDenyMessage { get; set; }

            [DisplayName("Приемная кампания")]
            [LocalRequired]
            public int CampaignID { get; set; }
            public int CampaignTypeID { get; set; }
            public IEnumerable Campaigns { get; set; }
            [DisplayName("Уровень образования")]
            public byte LevelId { get; set; }

            IEnumerable<SelectorItem> levels = new List<SelectorItem>();
            public IEnumerable<SelectorItem> Levels
            {
                get
                {
                    return levels;
                }
            }
            IEnumerable<SelectListItem> competitions = new List<SelectListItem>();
            public string[] SelectedCompetitions { get; set; }
            public IEnumerable<SelectListItem> Competitions { get { return competitions; } }

            [DisplayName("Конкурс")]
            [LocalRequired]
            public int DisplayCompetitiveGroupID { get; set; }

            public int[] SelectedCompetitiveGroupIDs { get; set; }

            [DisplayName("Направления подготовки")]
            [LocalRequired]
            public int DisplayDirectionID { get; set; }

            public string[] SelectedDirectionIDs { get; set; }

            public Dictionary<int, IEnumerable> CompetitiveGroupNamesByCampaign { get; set; }

            public class EducationForms
            {
                [DisplayName("Очная форма - Бюджетные места")]
                public bool BudgetO { get; set; }
                [DisplayName("Очно-заочная (вечерняя) - Бюджетные места")]
                public bool BudgetOZ { get; set; }
                [DisplayName("Заочная форма - Бюджетные места")]
                public bool BudgetZ { get; set; }
                [DisplayName("Очная форма - Платные места")]
                public bool PaidO { get; set; }
                [DisplayName("Очно-заочная (вечерняя) - Платные места")]
                public bool PaidOZ { get; set; }
                [DisplayName("Заочная форма - Платные места")]
                public bool PaidZ { get; set; }
                [DisplayName("Очная форма - Целевой прием")]
                public bool TargetO { get; set; }
                [DisplayName("Очно-заочная (вечерняя) - Целевой прием")]
                public bool TargetOZ { get; set; }
                [DisplayName("Заочная форма - Целевой прием")]
                public bool TargetZ { get; set; }

                public IEnumerable TargetOrganizationsO { get; set; }
                public IEnumerable TargetOrganizationsOZ { get; set; }
                public IEnumerable TargetOrganizationsZ { get; set; }

                public bool HasBudget
                {
                    get { return BudgetO || BudgetOZ || BudgetZ; }
                }
                public bool HasPaid
                {
                    get { return PaidO || PaidOZ || PaidZ; }
                }
                public bool HasTarget
                {
                    get { return TargetO || TargetOZ || TargetZ; }
                }
                public bool HasAny
                {
                    get { return HasBudget || HasPaid || HasTarget; }
                }
            }

            [DisplayName("Формы обучения и источники финансирования")]
            public EducationForms EducationFormsAvailable { get; set; }
            public EducationForms EducationFormsSelected { get; set; }

            public ApplicationPrioritiesViewModel Priorities { get; set; }

            public bool OriginalsProvided { get; set; }
            public IEnumerable TargetOrganizations { get; set; }
            [DisplayName("Организация целевого приема")]
            public int SelectedTargetOrganizationIDO { get; set; }
            [DisplayName("Организация целевого приема")]
            public int SelectedTargetOrganizationIDOZ { get; set; }
            [DisplayName("Организация целевого приема")]
            public int SelectedTargetOrganizationIDZ { get; set; }

            public bool IsDraft { get; set; }

            public string ActionCommand { get; set; }

            [DisplayName("Приоритет заявления (только в случае нескольких заявлений от одного абитуриента)")]
            public int? Priority { get; set; }
            public List<ApplicationPriorityViewModel> ListAppPrioritiesG { get; set; }
            public List<ApplicationPriorityViewModel> ListAppPrioritiesGI { get; set; }

            [DisplayName("Электронный адрес")]
            public string Email { get; set; }
            [DisplayName("Адрес")]
            public string Address { get; set; }
            [DisplayName("Регион")]
            public string Region { get; set; }
            [DisplayName("Тип населенного пункта")]
            public string TownType { get; set; }
            [DisplayName("Номер заявления")]
            public string ApplicationNumber { get; set; }

            public List<ForcedAdmissionReasonItemModel> ForcedAdmissionReasons { get; set; } 
        }

        public class Wz5InfoViewModel
        {
            [DisplayName("Статус")]
            public string Status { get; set; }

            [DisplayName("Тип нарушения")]
            public string Violation { get; set; }

            public bool IsVUZ { get; set; }

            [DisplayName("Образовательное учреждение")]
            public string Institution { get; set; }

            [DisplayName("Направления подготовки")]
            public string Direction { get; set; }

            [DisplayName("Курс")]
            public string Course { get; set; }

            [DisplayName("Уровни образования")]
            public string EducationLevel { get; set; }

            [DisplayName("Формы обучения и источники финансирования")]
            public string EducationalFormList { get; set; }

        }

        public class getViolationMoreInfoViewModel
        {
            public int? ApplicationID { get; set; }
            public string StatusDecision { get; set; }
        }
        }

    public class ForcedAdmissionReasonItemModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    #endregion
}