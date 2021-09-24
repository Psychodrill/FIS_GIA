using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;
using GVUZ.Model.Entrants;
using GVUZ.Model.Entrants.Documents;
using GVUZ.ServiceModel.Import.Bulk.Extensions;
using GVUZ.ServiceModel.Import.WebService.Dto.Documents.Collections;
using GVUZ.ServiceModel.Import.Core.Packages.Handlers;

namespace GVUZ.ServiceModel.Import.WebService.Dto
{

    #region Информация по абитуриенту

    [Description("Абитуриент")]
    public class EntrantDto : BaseDto
    {
        public string CustomInformation;
        public string FirstName;
        public string GenderID;
        public string LastName;
        public string MiddleName;
        public string Snils;

        public string FIO
        {
            get
            {
                return string.Format("{0} {1} {2}",
                                     LastName ?? "",
                                     MiddleName ?? "",
                                     FirstName ?? "");
            }
        }
    }

    public class AddressDto
    {
        public string Building;
        public string BuildingPart;
        public string CityName;
        public string CountryID;
        public string Phone;
        public string PostalCode;
        public string RegionID;
        public string Room;
        public string Street;
    }

    public class ParentDto
    {
        public string FirstName;
        public string LastName;
        public string MiddleName;
        public string Position;
        public string WorkPhone;
        public string Workplace;
    }

    #endregion

    public class DocumentInfoDto
    {
        public string DocType;
        // каждому элементу навесить если это возможно 
        /*public IdentityDocumentViewModel IdentityDocument;
		public EGEDocumentViewModel EgeDocument;
		public SchoolCertificateDocumentViewModel SchoolCertificateDocument;
		public DiplomaDocumentViewModel DiplomaDocument;
		public BasicDiplomaDocumentViewModel BasicDiplomaDocument;
		public OlympicDocumentViewModel OlympicDocument;
		public OlympicTotalDocumentViewModel OlympicTotalDocument;
		public DisabilityDocumentViewModel DisabilityDocument;
		public PsychoDocumentViewModel PsychoDocument;
		public MilitaryCardDocumentViewModel MilitaryCardDocument;
		public CustomDocumentViewModel CustomDocument;*/
    }

     

    public class ApplicationShortRef : IComparable
    {
        public string ApplicationNumber;

        private DateTime _registrationDateDate;
        public string UID { get; set; }
        public DateTime? OriginalDocumentsReceivedDate;

        [XmlIgnore]
        public int? OrderOfAdmissionId { get; set; }

        [XmlIgnore]
        public DateTime RegistrationDateDate
        {
            get { return _registrationDateDate; }
            set { _registrationDateDate = value.AddMilliseconds(-value.Millisecond); }
        }

        [XmlElement("RegistrationDate")]
        public string RegistrationDateString
        {
            get { return RegistrationDateDate == DateTime.MinValue ? null : RegistrationDateDate.GetDateTimeAsString(); }
            set
            {
                if (!String.IsNullOrEmpty(value))
                    _registrationDateDate = value.GetStringAsDate();
                else
                    _registrationDateDate = DateTime.MinValue;
            }
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return -1;
            if (obj is ApplicationShortRef)
            {
                var appShortRef = obj as ApplicationShortRef;
                if (RegistrationDateDate == appShortRef.RegistrationDateDate &&
                    ApplicationNumber == appShortRef.ApplicationNumber)
                    return 0;
            }

            return -1;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj is ApplicationShortRef)
            {
                var appShortRef = obj as ApplicationShortRef;
                if (RegistrationDateDate == appShortRef.RegistrationDateDate &&
                    ApplicationNumber == appShortRef.ApplicationNumber)
                    return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return (RegistrationDateDate).GetHashCode() & (ApplicationNumber ?? "").GetHashCode();
        }

/*
		public override int GetHashCode()
		{
			DateTime? stringOrEmptyAsDate = RegistrationDate.GetStringOrEmptyAsDate();
			if(stringOrEmptyAsDate == null) return ApplicationNumber.To(0);
			return ApplicationNumber.To(0) + stringOrEmptyAsDate.Value.Year + stringOrEmptyAsDate.Value.Month +
			       stringOrEmptyAsDate.Value.Day + stringOrEmptyAsDate.Value.Minute;
		}
*/
    }

    public class ApplicationRef : ApplicationShortRef
    {
        public string FirstName;
        public string LastName;
        public string MiddleName;
    }

    [Description("Заявление")]
    public class ApplicationDto : BaseDto
    {
        public ApplicationCommonBenefitDto ApplicationCommonBenefit;

        [XmlArrayItem(ElementName = "ApplicationCommonBenefit")] public ApplicationCommonBenefitDto[]
            ApplicationCommonBenefits;

        public ApplicationDocumentsDto ApplicationDocuments;
        [XmlArrayItem(ElementName = "EntranceTestResult")] public EntranceTestAppItemDto[] EntranceTestResults;

        public EntrantDto Entrant;

        [XmlArrayItem(ElementName = "FinSourceEduForm")] public List<FinSourceEduFormDto> FinSourceAndEduForms =
            new List<FinSourceEduFormDto>();

        /// <summary>
        ///     Для хранения привязки заявления к приказу в базе.
        ///     Используется, когда заявление уже существует в БД, и нужно сохранить привязку к приказу, если таковая имеется.
        /// </summary>
        [XmlIgnore] public int? OrderOfAdmissionID;

        /// <summary>
        ///     Хранение предыдущего статуса заявления для обновления. Чтобы не проверять заявление в случае нужной комбинации статусов
        /// </summary>
        [XmlIgnore] public int PreviousStatusID;

        [XmlArrayItem(ElementName = "CompetitiveGroupItemID")] public string[] SelectedCompetitiveGroupItems;
        [XmlArrayItem(ElementName = "CompetitiveGroupID")] public string[] SelectedCompetitiveGroups;

        /// <summary>
        /// Эту коллекцию будем формировать на лету
        /// </summary>
        [XmlIgnore]
        public List<ApplicationCompetitiveGroupItemDto> NewSourcesAndForms = new List<ApplicationCompetitiveGroupItemDto>();

        private DateTime _registrationDateDate;
        public string ApplicationNumber { get; set; }

        [XmlIgnore]
        public DateTime RegistrationDateDate
        {
            get { return _registrationDateDate; }
            set { _registrationDateDate = value.AddMilliseconds(-value.Millisecond); }
        }

        [XmlElement("RegistrationDate")]
        public string RegistrationDateString
        {
            get { return RegistrationDateDate == DateTime.MinValue ? null : RegistrationDateDate.GetDateTimeAsString(); }
            set
            {
                if (!String.IsNullOrEmpty(value))
                    _registrationDateDate = value.GetStringAsDate();
                else
                    _registrationDateDate = DateTime.MinValue;
            }
        }

        public string LastDenyDate { get; set; }
        public string NeedHostel { get; set; }
        public string StatusID { get; set; }
        public string StatusComment { get; set; }
        public string Priority { get; set; }

        public ApplicationCommonBenefitDto[] GetCommonBenefits()
        {
            if (ApplicationCommonBenefits == null && ApplicationCommonBenefit == null)
                return new ApplicationCommonBenefitDto[0];
            if (ApplicationCommonBenefits == null)
                return new[] {ApplicationCommonBenefit};
            if (ApplicationCommonBenefit == null)
                return ApplicationCommonBenefits;
            return ApplicationCommonBenefits.Concat(new[] {ApplicationCommonBenefit}).ToArray();
        }

        /// <summary>
        /// Индивидуальные достижения
        /// </summary>
        [XmlArrayItem(ElementName="IndividualAchievement")]
        public IndividualAchivementDto[] IndividualAchievements { get; set; }

        /// TODO: Вместо этого метода необходимо переопределить GetHashCode + Equals по этим полям!!
        public ApplicationShortRef GetApplicationShortRef()
        {
            return new ApplicationShortRef
                {
                    ApplicationNumber = ApplicationNumber,
                    RegistrationDateDate = RegistrationDateDate
                };
        }
    }

    /// <summary>
    ///     Источник финансирования и форма обучения.
    /// </summary>
    public class FinSourceEduFormDto
    {
        public string TargetOrganizationUID;
        private string _EducationFormID;
        private int _EducationFormIdInt;
        private string _FinanceSourceID;
        private int _FinanceSourceIdInt;

        public string FinanceSourceID
        {
            get { return _FinanceSourceID; }
            set
            {
                _FinanceSourceID = value;
                _FinanceSourceIdInt = value.ToInt(0);
            }
        }

        public string EducationFormID
        {
            get { return _EducationFormID; }
            set
            {
                _EducationFormID = value;
                _EducationFormIdInt = value.ToInt(0);
            }
        }

        public string Priority;
        public string CompetitiveGroupID;
        public string CompetitiveGroupItemID;

        [XmlIgnore]
        public int FinanceSourceIdInt
        {
            get { return _FinanceSourceIdInt; }
            set { _FinanceSourceIdInt = value; }
        }

        [XmlIgnore]
        public int EducationFormIdInt
        {
            get { return _EducationFormIdInt; }
            set { _EducationFormIdInt = value; }
        }
    }

    /// <summary>
    /// Новая структура для приоритетов в заявлении
    /// Старая будет автоматически мапиться на эту структуру
    /// </summary>
    public class ApplicationCompetitiveGroupItemDto
    {
        public string ApplicationUID { get; set; }
        public string CompetitiveGroupUID { get; set; }
        public string CompetitiveGroupItemUID { get; set; }
        public int EducationForm { get; set; }
        public int EducationSource { get; set; }
        public int? Priority { get; set; }
        public string CompetitiveGroupTargetUID { get; set; }
    }

    public class IndividualAchivementDto
    {
        public string ApplicationUID { get; set; }
        public string IAUID { get; set; }
        public string IAName { get; set; }
        public string IAMark { get; set; }
        public string IADocumentUID { get; set; }
        public bool? isAdvantageRight { get; set; }
    }

    //public class LastEducationDto
    //{
    //    public string KindLastEducationID;
    //    public string OriginalDocumentsReceived;
    //    public string IsSameVPO;
    //}

    #region Результаты ВИ для заявления

    public class ApplicationLastEducation
    {
    }

    [Description("Результаты вступительных испытаний")]
    public class EntranceTestAppItemDto : BaseDto
    {
        public string CompetitiveGroupID;
        public EntranceTestSubjectDto EntranceTestSubject;
        public string EntranceTestTypeID;

        //для экспорта
        public string EntranceTestsResultID;
        public EntranceTestResultDocumentsDto ResultDocument;
        public string ResultSourceTypeID;
        public string ResultValue;
    }

    public static class EntranceTestAppItemDtoExtensions
    {
        public static decimal? GetResultValue(this EntranceTestAppItemDto item)
        {
            if (!string.IsNullOrEmpty(item.ResultSourceTypeID) &&
                item.ResultSourceTypeID.ToInt(-1) == (int) EntranceTestResultSourceEnum.OlympicDocument)
                return 100;

            return item.ResultValue.ToDecimalNullable();
        }
    }

    public class EntranceTestSubjectDto
    {
        public string SubjectID;
        public string SubjectName;
    }

    public class EntranceTestInstitutionDocumentDto
    {
        public string DocumentTypeID { get; set; }
        public string DocumentDate { get; set; }
        public string DocumentNumber { get; set; }
    }

    #endregion

    /// <summary>
    ///     Список справочников
    /// </summary>
    //[XmlRoot("Dictionaries")]
    //public class DictionariesDto
    //{
    //    [XmlArrayItem("Dictionary")]
    //    public DictionaryDto[] Dictionaries;
    //}

    #region Справочники

    #endregion
    public static class ApplicationDtoExtensions
    {
        public static decimal? CalcSelectedCompetitiveGroupRating(this ApplicationDto application,
                                                                  string competitiveGroupUID)
        {
            if (application.EntranceTestResults == null) return null;
            decimal rating = application.EntranceTestResults.Where(c => c.ResultValue != null &&
                                                                        c.CompetitiveGroupID == competitiveGroupUID)
                                        .Sum(c => c.GetResultValue() ?? 0);
            return rating;
        }

        public static int? GetCalculatedBenefitId(this ApplicationDto application, string competitiveGroupUID)
        {
            ApplicationCommonBenefitDto[] benefits = application.GetCommonBenefits();
            if (benefits == null) return null;

            /* Если есть бенефит - возвращаем его */
            ApplicationCommonBenefitDto benefit =
                benefits.FirstOrDefault(c => c.BenefitKindID != null && c.CompetitiveGroupID == competitiveGroupUID);
            if (benefit != null && benefit.BenefitKindID != null)
                return benefit.BenefitKindID.ToIntNullable();

            return null;
        }

        public static void CreateApplicationCompetitiveGroupItems(this ApplicationDto application)
        {
            using (ImportEntities context = new ImportEntities())
            {
                var allGroupItems = context.CompetitiveGroupItem.Where(x => application.SelectedCompetitiveGroups.Contains(x.CompetitiveGroup.UID))
                    .Select(x => new { GroupUid = x.CompetitiveGroup.UID, GroupItemUID = x.UID });

                foreach (var finSourceAndEduForm in application.FinSourceAndEduForms)
                {
                    if (string.IsNullOrEmpty(finSourceAndEduForm.Priority) &&
                        string.IsNullOrEmpty(finSourceAndEduForm.CompetitiveGroupID) &&
                        string.IsNullOrEmpty(finSourceAndEduForm.CompetitiveGroupItemID))
                    {
                        // Все 3 новых поля пусты, значит передаётся старая структура

                        foreach (var compGroup in application.SelectedCompetitiveGroups)
                        {
                            var items = allGroupItems.Where(x => x.GroupUid == compGroup).ToList();

                            foreach (var groupItem in application.SelectedCompetitiveGroupItems)
                            {
                                if (items.Any(x => x.GroupItemUID == groupItem))
                                    application.NewSourcesAndForms.Add(
                                        new ApplicationCompetitiveGroupItemDto
                                        {
                                            ApplicationUID = application.UID,
                                            CompetitiveGroupItemUID = groupItem,
                                            CompetitiveGroupUID = compGroup,
                                            EducationForm = finSourceAndEduForm.EducationFormIdInt,
                                            EducationSource = finSourceAndEduForm.FinanceSourceIdInt,
                                            Priority = 0,
                                            CompetitiveGroupTargetUID = finSourceAndEduForm.TargetOrganizationUID
                                        });
                            }
                        }
                    }
                    else
                    {
                        application.NewSourcesAndForms.Add(
                            new ApplicationCompetitiveGroupItemDto()
                            {
                                ApplicationUID = application.UID,
                                CompetitiveGroupUID = finSourceAndEduForm.CompetitiveGroupID,
                                CompetitiveGroupItemUID = finSourceAndEduForm.CompetitiveGroupItemID,
                                CompetitiveGroupTargetUID = finSourceAndEduForm.TargetOrganizationUID,
                                EducationForm = finSourceAndEduForm.EducationFormIdInt,
                                EducationSource = finSourceAndEduForm.FinanceSourceIdInt,
                                Priority = finSourceAndEduForm.Priority.ToIntNullable()
                            });
                    }
                }
            }
        }
    }
}