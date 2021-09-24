using System.Xml.Serialization;

namespace GVUZ.ServiceModel.Import.WebService.Dto.Result.Import
{
    public class FailedImportInfoDto
    {
        [XmlArrayItem(ElementName = "AdmissionVolume")] public AdmissionVolumeFailDetailsDto[] AdmissionVolumes;

        [XmlArrayItem(ElementName = "ApplicationCommonBenefit")] public ApplicationCommonBenefitFailDetailsDto[]
            ApplicationCommonBenefits;

        [XmlArrayItem(ElementName = "Application")] public ApplicationFailDetailsDto[] Applications;
        [XmlArrayItem(ElementName = "CampaignDate")] public CampaignDateFailDto[] CampaignDates;
        [XmlArrayItem(ElementName = "Campaign")] public CampaignDetailsFailDto[] Campaigns;

        [XmlArrayItem(ElementName = "CommonBenefitItem")] public CommonBenefitFailDetailsDto[] CommonBenefit;

        [XmlArrayItem(ElementName = "CompetitiveGroupItem")] public CompetitiveGroupItemFailDetailsDto[]
            CompetitiveGroupItems;

        [XmlArrayItem(ElementName = "CompetitiveGroup")] public CompetitiveGroupFailDetailsDto[] CompetitiveGroups;

        [XmlArrayItem(ElementName = "ConsideredApplication")] public ConsideredApplicationFailDetailsDto[]
            ConsideredApplications;

        [XmlArrayItem(ElementName = "DistributedAdmissionVolume")]
        public DistributedAdmissionVolumeFailDetailsDto[] DistributedAdmissionVolumes;

        [XmlArrayItem(ElementName = "EntranceTestBenefitItem")] public EntranceTestBenefitItemFailDetailsDto[]
            EntranceTestBenefits;

        [XmlArrayItem(ElementName = "EntranceTestItem")] public EntranceTestItemFailDetailsDto[] EntranceTestItems;

        [XmlArrayItem(ElementName = "EntranceTestResult")] public EntranceTestResultFailDetailsDto[] EntranceTestResults;

        [XmlArrayItem(ElementName = "OrdersOfAdmission")] public OrderFailDetailsDto[] OrdersOfAdmissions;
        [XmlArrayItem(ElementName = "ApplicationsInOrder")]
        public OrderApplicationFailDetailsDto[] ApplicationsInOrders;

        //[XmlArrayItem(ElementName = "OrdersOfAdmission")]
        //public ApplicationFailDetailsDto[] OrderOfAdmissionsFails;

        [XmlArrayItem(ElementName = "RecommendedApplication")] public RecommendedApplicationFailDetailsDto[]
            RecommendedApplications;

        [XmlArrayItem(ElementName = "TargetOrganizationDirection")] public TargetOrganizationDirectionFailDetailsDto[]
            TargetOrganizationDirections;

        [XmlArrayItem(ElementName = "TargetOrganization")] public TargetOrganizationFailDetailsDto[] TargetOrganizations;

        [XmlArrayItem(ElementName = "RecommendedList")]
        public RecommendedListFailDetailsDto[] RecommendedLists;

        [XmlArrayItem(ElementName = "InstitutionAchievement")]
        public InstitutionAchievementFailDetailsDto[] InstitutionAchievements;

        [XmlArrayItem(ElementName = "InstitutionPrograms")]
        public InstitutionProgramFailDetailsDto[] InstitutionPrograms;


        public bool Exists
        {
            get
            {
                return
                    (AdmissionVolumes != null && AdmissionVolumes.Length > 0) ||
                    (CompetitiveGroups != null && CompetitiveGroups.Length > 0) ||
                    (CompetitiveGroupItems != null && CompetitiveGroupItems.Length > 0) ||
                    (TargetOrganizations != null && TargetOrganizations.Length > 0) ||
                    (TargetOrganizationDirections != null && TargetOrganizationDirections.Length > 0) ||
                    (EntranceTestItems != null && EntranceTestItems.Length > 0) ||
                    (CommonBenefit != null && CommonBenefit.Length > 0) ||
                    (EntranceTestBenefits != null && EntranceTestBenefits.Length > 0) ||
                    (Applications != null && Applications.Length > 0) ||
                    (EntranceTestResults != null && EntranceTestResults.Length > 0) ||
                    (ApplicationCommonBenefits != null && ApplicationCommonBenefits.Length > 0) ||
                    (OrdersOfAdmissions != null && OrdersOfAdmissions.Length > 0) ||
                    //(OrderOfAdmissionsFails != null && OrderOfAdmissionsFails.Length > 0) ||
                    (Campaigns != null && Campaigns.Length > 0) ||
                    (CampaignDates != null && CampaignDates.Length > 0) ||
                    (ConsideredApplications != null && ConsideredApplications.Length > 0) ||
                    (RecommendedApplications != null && RecommendedApplications.Length > 0) ||
                    (RecommendedLists != null && RecommendedLists.Length > 0);
            }
        }
    }

    public class EntranceTestResultFailDetailsDto
    {
        public string ApplicationNumber;
        public ErrorInfoImportDto ErrorInfo;
        public string RegistrationDate;
        public string ResultSourceType;
        public string ResultValue;
        public string SubjectName;
    }

    #region Objects from "ImportResultPackage / Log / Failed" Section

    public class AdmissionVolumeFailDetailsDto
    {
        public string DirectionName;
        public string EducationLevelName;
        public ErrorInfoImportDto ErrorInfo;
    }

    public class DistributedAdmissionVolumeFailDetailsDto
    {
        public string AdmissionVolumeUID;
        public string LevelBudget;
        public ErrorInfoImportDto ErrorInfo;
    }


    public class InstitutionAchievementFailDetailsDto
    {
        public string IAUID;
        public string Name;
        public ErrorInfoImportDto ErrorInfo;
    }

    public class CompetitiveGroupFailDetailsDto
    {
        public string CompetitiveGroupName;
        public string UID;
        public string ID;
        public ErrorInfoImportDto ErrorInfo;
    }

    public class CompetitiveGroupsFailDetailsDto
    {
        public string CompetitiveGroupName;
        public ErrorInfoImportDto ErrorInfo;
    }

    public class CompetitiveGroupItemFailDetailsDto
    {
        public string CompetitiveGroupName;
        public string DirectionCode;
        public string DirectionName;
        public ErrorInfoImportDto ErrorInfo;
    }

    public class TargetOrganizationFailDetailsDto
    {
        public string UID;
        public ErrorInfoImportDto ErrorInfo;
        public string TargetOrganizationName;
    }

    public class InstitutionProgramFailDetailsDto
    {
        public string UID;
        public string Name;
        public string Code;
        public ErrorInfoImportDto ErrorInfo;
    }

    public class TargetOrganizationDirectionFailDetailsDto
    {
        public string CompetitiveGroupName;
        public string DirectionName;
        public string EducationLevelName;
        public ErrorInfoImportDto ErrorInfo;
        public string TargetOrganizationName;
    }

    public class EntranceTestItemFailDetailsDto
    {
        public string CompetitiveGroupName;
        public string EntranceTestType;
        public ErrorInfoImportDto ErrorInfo;
        public string SubjectName;
    }

    public class CommonBenefitFailDetailsDto
    {
        public string BenefitKindName;
        public string CompetitiveGroupName;
        public ErrorInfoImportDto ErrorInfo;
    }

    public class EntranceTestBenefitItemFailDetailsDto
    {
        public string BenefitKindName;
        public string CompetitiveGroupName;
        public string EntranceTestType;
        public ErrorInfoImportDto ErrorInfo;
        public string SubjectName;
    }

    public class ApplicationFailDetailsDto
    {
        public string ApplicationNumber;
        public ErrorInfoImportDto ErrorInfo;
        public string RegistrationDate;
        public string UID;
    }

    public class OrderFailDetailsDto
    {
        public ErrorInfoImportDto ErrorInfo;
        public string OrderUID;
        public string OrderName;
        public string OrderNumber;
        public string OrderDate;
    }
    public class OrderApplicationFailDetailsDto
    {
        public ErrorInfoImportDto ErrorInfo;
        public string ApplicationNumber;
        public string RegistrationDate;
        public string UID;
        public string OrderUID;
    }

    public class RecommendedListFailDetailsDto
    {
        public string Stage;
        public ErrorInfoImportDto ErrorInfo;
    }

    public class ConsideredApplicationFailDetailsDto
    {
        public ConsideredApplicationDto ConsideredApplication;
        public ErrorInfoImportDto ErrorInfo;
    }

    public class RecommendedApplicationFailDetailsDto
    {
        public ErrorInfoImportDto ErrorInfo;
        public RecommendedApplicationDto RecommendedApplication;
    }

    public class ApplicationCommonBenefitFailDetailsDto
    {
        public string ApplicationNumber;
        public string BenefitKindName;
        public ErrorInfoImportDto ErrorInfo;
        public string RegistrationDate;
    }

    public class CampaignDetailsFailDto
    {
        public ErrorInfoImportDto ErrorInfo;
        public string Name;
    }

    public class CampaignDateFailDto
    {
        public ErrorInfoImportDto ErrorInfo;
        public string UID;
    }

    #endregion
}