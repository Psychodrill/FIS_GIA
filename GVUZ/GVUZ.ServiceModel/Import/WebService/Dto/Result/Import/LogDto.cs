using System.Xml.Serialization;
using FogSoft.Helpers;
namespace GVUZ.ServiceModel.Import.WebService.Dto.Result.Import
{
    public class LogDto
    {
        public FailedImportInfoDto Failed;
        public SuccessfulImportStatisticsDto Successful;
    }

    public class SuccessfulImportStatisticsDto
    {
        public string Campaigns { get { return campaignsImported.ToString(); } set { campaignsImported = value.To(0); } }
        public string AdmissionVolumes { get { return admissionVolumesImported; } set { admissionVolumesImported = value; } }
        
        public string CompetitiveGroups { get { return competitiveGroupsImported; } set { competitiveGroupsImported = value; } }
        public string CompetitiveGroupItems { get { return competitiveGroupItemsImported; } set { competitiveGroupItemsImported = value; } }

        //public string ConsideredApplications { get { return consideredApplicationsImported; } set { consideredApplicationsImported = value; } }
        public string DistributedAdmissionVolumes { get { return distributedAdmissionVolumesImported; } set { distributedAdmissionVolumesImported = value; } }
        public string InstitutionAchievements { get { return institutionAchievementsImported; } set { institutionAchievementsImported = value; } }
        public string TargetOrganizations { get { return targetOrganizationsImported; } set { targetOrganizationsImported = value; } }
        public string InstitutionPrograms { get { return institutionProgramsImported; } set { institutionProgramsImported = value; } }

        public string Applications { get { return applicationsImported; } set { applicationsImported = value; } }

        public string Orders { get { return ordersImported.ToString(); } set { ordersImported = value.To(0); } }
        public string ApplicationsInOrders { get { return applicationsInOrdersImported.ToString(); } set { applicationsInOrdersImported = value.To(0); } }


        //public string RecommendedApplications { get { return recommendedApplicationsImported; } set { recommendedApplicationsImported = value; } }
        //public string RecommendedLists { get { return recommendedListsImported; } set { recommendedListsImported = value; } }

        

        [XmlIgnore()]
        public int campaignsImported = 0;
        [XmlIgnore()]
        public string applicationsImported = "0";
        [XmlIgnore()]
        public string admissionVolumesImported = "0";

        [XmlIgnore()]
        public string competitiveGroupsImported = "0";
        [XmlIgnore()]
        public string competitiveGroupItemsImported = "0";

        [XmlIgnore()]
        public string consideredApplicationsImported = "0";

        [XmlIgnore()]
        public string distributedAdmissionVolumesImported = "0";
        [XmlIgnore()]
        public string institutionAchievementsImported = "0";
        [XmlIgnore()]
        public int ordersImported = 0;
        [XmlIgnore()]
        public int applicationsInOrdersImported = 0;
        [XmlIgnore()]
        public string recommendedApplicationsImported = "0";
        [XmlIgnore()]
        public string recommendedListsImported = "0";

        [XmlIgnore()]
        public string targetOrganizationsImported = "0";

        [XmlIgnore()]
        public string institutionProgramsImported = "0";
    }
}