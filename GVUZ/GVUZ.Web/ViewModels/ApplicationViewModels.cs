using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using FogSoft.Web.Mvc;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.Web.Script.Serialization;
using System.Globalization;
using System.Xml.Serialization;

namespace GVUZ.Web.ViewModels
{
    public class ApplicationViewModels
    {

    }

    //Общие сведения
    public class ApplicationV0Model
    {
        ///*НОВОЕ*/
        //#region Общие сведения
        //public class CommonInfoView
        //{
        //    public int? ApplicationID { get; set; }

        //    public CommonInfoView()
        //    {
        //        CompetitiveGroups = new List<CompetitiveGroupInfo>();
        //    }

        //    public class CompetitiveGroupInfo : InfoView
        //    {
        //        public int CompetitiveGroupID { get; set; }

        //        [DisplayName("Конкурс")]
        //        public string CompetitiveGroupName { get; set; }

        //        [DisplayName("Конкурс")]
        //        public decimal Competition { get; set; }

        //        [DisplayName("Количество мест")]
        //        public int Places { get; set; }

        //        [DisplayName("Количество заявлений")]
        //        public int Requests { get; set; }

        //        [DisplayName("Рейтинг")]
        //        public string Rate { get; set; }

        //        [DisplayName("Приказ о зачислении")]
        //        public string EnrollmentOrder { get; set; }

        //        [DisplayName("Количество баллов")]
        //        public string Points { get; set; }
        //    }

        //    public List<CompetitiveGroupInfo> CompetitiveGroups { get; set; }
        //}

        //public class InfoView
        //{
        //    [DisplayName("Статус")]
        //    public string Status { get; set; }

        //    [DisplayName("Тип нарушения")]
        //    public string Violation { get; set; }

        //    public bool IsVUZ { get; set; }

        //    [DisplayName("Образовательное учреждение")]
        //    public string Institution { get; set; }

        //    [DisplayName("Направления подготовки")]
        //    public string Direction { get; set; }

        //    [DisplayName("Курс")]
        //    public string Course { get; set; }

        //    [DisplayName("Уровни образования")]
        //    public string EducationLevel { get; set; }

        //    [DisplayName("Формы обучения и источники финансирования")]
        //    public string EducationalFormList { get; set; }
        //}
        //#endregion
        ///**/
        public ApplicationV0Model()
        {
            CompetitiveGroup = new List<CompetitiveGroupInfo>();
            SourceForms = new List<listInfo>();
            listESC = new List<listGI>();
        }

        public int? ApplicationID { get; set; }

        public List<listInfo> SourceForms { get; set; }
        public List<CompetitiveGroupInfo> CompetitiveGroup { get; set; }
        public List<listGI> listESC { get; set; }

        private static readonly listGI _SForms = new listGI();
        public listGI SForms
        {
            get { return _SForms; }
        }

        private static readonly listInfo _SFForms = new listInfo();
        public listInfo SFForms
        {
            get { return _SFForms; }
        }

        [DisplayName("Статус")]
        public string StatusName { get; set; }

        [DisplayName("Тип нарушения")]
        public string ViolationName { get; set; }

        [DisplayName("ВУЗ")]
        public string InstitutionName { get; set; }

        public class listInfo
        {
            [DisplayName("Формы обучения и источники финансирования")]
            public string SourceForms { get; set; }
        }

        public class listGI
        {
            [DisplayName("Курс")]
            public int Course { get; set; }

            [DisplayName("Уровни образования")]
            public string EduLevelName { get; set; }

            [DisplayName("Направления подготовки")]
            public string DirectionName { get; set; }
        }

        public class CompetitiveGroupInfo
        {
            public int CompetitiveGroupID { get; set; }

            [DisplayName("Конкурс")]
            public string CompetitiveGroupName { get; set; }

            [DisplayName("Конкурс")]
            public double Competition { get; set; }

            [DisplayName("Количество мест")]
            public double? Places { get; set; }

            [DisplayName("Количество заявлений")]
            public double? Requests { get; set; }

            [DisplayName("Рейтинг")]
            public string Rate { get; set; }

            [DisplayName("Приказ о зачислении")]
            public string EnrollmentOrder { get; set; }

            [DisplayName("Количество баллов")]
            public decimal? Points { get; set; }
        }

        [DisplayName("Формы обучения и источники финансирования")]
        public ApplicationPrioritiesViewModel Priorities { get; set; }
    }

    //Личные данные
    public class ApplicationV1Model
    {
        public int? ApplicationID { get; set; }
        public int EntrantID { get; set; }

        [DisplayName("Фамилия")]
        public string EntrantLastName { get; set; }
        [DisplayName("Имя")]
        public string EntrantFirstName { get; set; }
        [DisplayName("Отчество")]
        public string EntrantMiddleName { get; set; }

        [DisplayName("Дата рождения")]
        [DataType(dataType: DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime? DocumentBirthDate { get; set; }

        [DisplayName("Вид документа удостов. личность")]
        public string DocumentTypeName { get; set; }

        [DisplayName("Серия / № документа")]
        public string DocumentSeriaNumber { get; set; }

        [DisplayName("Кем выдан")]
        public string DocumentOrganization { get; set; }

        [DisplayName("Дата выдачи")]
        [DataType(dataType: DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime? DocumentDate { get; set; }

        [DisplayName("Ссылка на документ удостов. личность")]
        public int? AttachmentID { get; set; }

        public string AttachmentName { get; set; }
        public Guid? AttachmentFileID { get; set; }
        public byte[] AttachmentFileBody { get; set; }

        [DisplayName("Пол")]
        public string GenderName { get; set; }

        [DisplayName("Гражданство")]
        public string CountryName { get; set; }

        [DisplayName("Место рождения")]
        public string BirthPlace { get; set; }

        [DisplayName("О себе могу сообщить следующее")]
        public string CustomInformation { get; set; }

        [DisplayName("Требуется общежитие")]
        public bool NeedHostel { get; set; }
    }

    //Документы
    public class ApplicationV2Model
    {
        public ApplicationV2Model()
        {
            AttachedDocuments = new List<ApplicationV2>();
        }

        public int? ApplicationID { get; set; }

        private static readonly ApplicationV2 _baseDocument = new ApplicationV2();
        public ApplicationV2 BaseDocument
        {
            get { return _baseDocument; }
        }

        [ScriptIgnore]
        public List<ApplicationV2> AttachedDocuments { get; set; }

        public class ApplicationV2
        {
            public int? EntrantDocumentID { get; set; }

            [DisplayName("Тип документа")]
            public string DocumentTypeName { get; set; }

            [DisplayName("Серия и номер документа")]
            public string DocumentSeriesNumber { get; set; }

            [DisplayName("Дата выдачи")]
            public string DocumentDate { get; set; }

            [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
            public DateTime? DocDate { get; set; }
            public string DocNumber { get; set; }
            public string DocSeries { get; set; }
            public int DocTypeID { get; set; }
            public string DocSpecificData { get; set; }

            [DisplayName("Кем выдан")]
            public string DocumentOrganization { get; set; }

            [DisplayName("Ссылка на документ")]
            public Guid? DocumentAttachmentID { get; set; }

            public string DocumentAttachmentName { get; set; }

            public bool CanBeModified { get; set; }
            public bool ShowWarnBeforeModifying { get; set; }
            public bool CanBeDetached { get; set; }

            [DisplayName("Дата предоставления")]
            public string OriginalReceivedDate { get; set; }

            [DisplayName("Оригиналы/заверенные копии предоставлены / Заявление с обязательством предоставления оригинала в течение первого учебного года/ЕПГУ")]
            public bool OriginalReceived { get; set; }

            public bool CanNotSetReceived { get; set; }

            public int? StatusID { get; set; }
        }
    }

    //Испытания
    public class ApplicationV3Model
    {
        public int? ApplicationID { get; set; }

        public ApplicationV3Model()
        {
            ListTest = new List<ApplicationV3>();
            ListCG = new List<CompetitiveGroup>();
            listGeneralBenefit = new List<GeneralBenefits>();
            AttachedDocs = new List<AttachedDocument>();
            
        }

        public List<CompetitiveGroup> ListCG { get; set; }
        public List<GeneralBenefits> listGeneralBenefit { get; set; }
        public List<AttachedDocument> AttachedDocs { get; set; }
        public List<AppComposition> CompositionResult { get; set; }

        public class Composition
        {
            [DisplayName("Год")]
            [DataType(dataType: DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
            public DateTime? Year { get; set; }

            public string acrYear { get { return string.Format("{0:yyyy}", Year); } }

            [DisplayName("Тема")]
            public string acrName { get; set; }

            [DisplayName("Оценка")]
            public bool acrResult { get; set; }
        }

        private static readonly Composition _baseComposition = new Composition();
        public Composition BaseComposition
        {
            get { return _baseComposition; }
        }

        public class GeneralBenefits
        {
            public Int32? CompetitiveGroupID { get; set; }
            public Int32? BenefitId { get; set; }
            public string BenefitName { get; set; }
            public string DocumentName { get; set; }
            public string DocumentNumber { get; set; }
            public string DocumentSeries { get; set; }
            [DataType(dataType: DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
            public DateTime? DocumentDate { get; set; }

            public string BenefitDisplay
            {
                get
                {
                    return string.Format("{0} ({1} № {2}{3} от {4:dd.MM.yyyy})\r\n", BenefitName, DocumentName, DocumentNumber, DocumentSeries, DocumentDate);
                }
            }
        }

        public class AttachedDocument
        {
            public Int32? EntranceTestItemID { get; set; }
            public string DocumentName { get; set; }
            public string DocumentNumber { get; set; }
            public string DocumentSeries { get; set; }
            public int? DocumentTypeID { get; set; }
            public int? EntrantDocumentID { get; set; }
            [DataType(dataType: DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
            public DateTime? DocumentDate { get; set; }
            [DisplayName("Балл")]
            public decimal? ResultValue { get; set; }

            public int? SubjectID { get; set; }
            public short? BenefitID { get; set; }
            public string BenefitName { get; set; }

            [DisplayName("Тип документа")]
            public int? InstitutionDocumentTypeID { get; set; }
            public string InstitutionDocumentTypeName { get; set; }

            [DisplayName("Дата выдачи")]
            [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
            public DateTime? InstitutionDocumentDate { get; set; }
            public string InstitutionDocumentDateString { get { return ((InstitutionDocumentDate == null) ? "" : InstitutionDocumentDate.Value.ToString("dd.MM.yyyy")); } }

            [DisplayName("Номер документа")]
            public string InstitutionDocumentNumber { get; set; }

            public bool? OlympApproved { get; set; }
            public int? AppealStatusID { get; set; }
            public string AppealStatusName { get; set; }

            public string AttachedDisplay
            {
                get
                {
                    //if (ResultValue == null && DocumentTypeID == 2)
                    //{	// Случай ошибочного назначения докмента
                    //    return "Для данного предмета нет результата в указанном сертификате ЕГЭ";
                    //}
                    if (EntrantDocumentID != null)
                    {
                        if (DocumentTypeID == 2)
                        {
                            if (AppealStatusID != null)
                            {
                                if (AppealStatusID > 0)
                                {
                                    return (DocumentName + " " + (DocumentSeries ?? "") + " № " + DocumentNumber + " " +
                                            (DocumentDate != null
                                                ? "от " + DocumentDate.Value.ToString("yyyy") + " года"
                                                : "") + " (Апелляция - " + AppealStatusName + ")");
                                }
                            }
                            return (DocumentName + " " + (DocumentSeries ?? "") + " № " + DocumentNumber + " " + (DocumentDate != null ? "от " + DocumentDate.Value.ToString("yyyy") + " года" : ""));
                        }
                        if ((DocumentTypeID == 9) || (DocumentTypeID == 10))
                        {
                            if (OlympApproved.HasValue && OlympApproved.Value)
                            {
                                return (DocumentName + " " + (DocumentSeries ?? "") + " № " + DocumentNumber + " " +
                                        (DocumentDate != null ? "от " + DocumentDate.Value.ToString("yyyy") + " года" : "") +
                                        " (результаты подтверждены)");
                            }
                            return (DocumentName + " " + (DocumentSeries ?? "") + " № " + DocumentNumber + " " +
                                    (DocumentDate != null ? "от " + DocumentDate.Value.ToString("yyyy") + " года" : "") +
                                    " (результаты не подтверждены)");
                        }
                        return (DocumentName + " " + (DocumentSeries ?? "") + " № " + DocumentNumber + " " + (DocumentDate != null ? "от " + DocumentDate.Value.ToString("dd.MM.yyyy") : ""));
                    }
                    if (InstitutionDocumentTypeID != null)
                    {
                        return InstitutionDocumentTypeName + " " + InstitutionDocumentNumber + " " + (InstitutionDocumentDate != null ? "от " + InstitutionDocumentDate.Value.ToString("dd.MM.yyyy") : "");
                    }
                    if (SubjectID != null && EntrantDocumentID == null) { return "Результат ЕГЭ (балл не проверен)"; }
                    if (BenefitName != null)
                    {
                        return BenefitName + " " + DocumentSeries ?? "" + " № " + DocumentNumber + " " + (DocumentDate != null ? "от " + DocumentDate.Value.ToString("dd.MM.yyyy") : "");
                    }
                    return "Описания документа нет";
                }


                //get
                //{
                //    return string.Format("{0} № {1}{2} от {3:yyyy}\r\n", DocumentName, DocumentNumber, DocumentSeries, DocumentDate);
                //}
            }
        }
        public class CompetitiveGroup
        {
            public Int32? CompetitiveGroupID { get; set; }
            public string CompetitiveGroupName { get; set; }
        }

        public List<ApplicationV3> ListTest { get; private set; }


        private static readonly ApplicationV3 _baseDocument = new ApplicationV3();
        public ApplicationV3 BaseDocument
        {
            get { return _baseDocument; }
        }



        public class ApplicationV3
        {
            public Int32? CompetitiveGroupID { get; set; }
            public Int32? EntranceTestItemID { get; set; }

            [DisplayName("Дисциплина")]
            public string SubjectName { get; set; }

            [DisplayName("Конкурс")]
            public string CompetitiveGroupName { get; set; }

            [DisplayName("Приоритет")]
            public int? Priority { get; set; }

            [DisplayName("Балл")]
            public decimal? ResultValue { get; set; }

            [DisplayName("Балл ЕГЭ")]
            public decimal? EgeResultValue { get; set; }

            [DisplayName("Основание для оценки")]
            public string Source { get; set; }

            public bool IsProfileSubject { get; set; }

            public string DocumentDescription { get; set; }

            public short? EntranceTestTypeID { get; set; }

            public bool? ApplicationIsForSPOandVO { get; set; }

            public bool? EntranceTestIsForSPOandVO { get; set; }

            public int? ReplacedEntranceTestItemID { get; set; }
        } 
    }

    //Индивидуальные достижения
    public class ApplicationV4Model
    {
        public int? ApplicationID { get; set; }
        public int EntrantID { get; set; }
        public decimal IndividualAchivementsMark { get; set; }
        public string IndividualAchivementsMarkStr { get { return IndividualAchivementsMark.ToString("0.##"); } }

        public List<ApplicationV4> Items { get; set; }

        public ApplicationV4Model()
        {
            Items = new List<ApplicationV4>();
        }

        private static readonly ApplicationV4 _FakedAchivement = new ApplicationV4();
        public ApplicationV4 FakedAchivement
        {
            get { return _FakedAchivement; }
        }

        public class ApplicationV4
        {
            public int IAID { get; set; }

            [StringLength(50)]
            [DisplayName("UID")]
            public string UID { get; set; }

            [StringLength(100)]
            [DisplayName("Наименование индивидуального достижения")]
            public string IAName { get; set; }

            [DisplayName("Дополнительный балл")]
            public decimal? IAMark { get; set; }

            public ApplicationV4Document IADocument { get; set; }

            public int? IADocumentID { get; set; } // это свойство нужно для передачи ID подтверждающего документа при создании ИД.

            [DisplayName("Сведения о подтверждающем документе")]
            public string IADocumentDisplay
            {
                get
                {
                    return string.Format("{0} № {1}{2} от {3:dd.MM.yyyy}", IADocument.DocumentTypeNameText, IADocument.DocumentSeries, IADocument.DocumentNumber, IADocument.DocumentDate);
                }
            }

            [DisplayName("Преимущественное право")]
            public bool? isAdvantageRight { get; set; }

            public class ApplicationV4Document
            {
                [XmlIgnore]
                public Int32? EntrantDocumentID { get; set; }

                [LocalRequired]
                [DataType(dataType: DataType.Date)]
                [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
                public DateTime? DocumentDate { get; set; }

                [LocalRequired]
                [StringLength(50)]
                public string DocumentNumber { get; set; }

                [StringLength(20)]
                public string DocumentSeries { get; set; }

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

            }
        }
    }

    //Печатные формы
    public class ApplicationV5Model
    {
        public int? ApplicationID { get; set; }
    }
}