using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GVUZ.ImportService2015.Core.Import.Result
{
    public class ConflictsResultDto
    {
        [XmlArrayItem(ElementName = "Application")]
        public ApplicationShortRef[] Applications;
        [XmlArrayItem(ElementName = "Application")]
        public ApplicationShortRef[] OrdersOfAdmission;

        [XmlArrayItem(ElementName = "CompetitiveGroupItemID")]
        public string[] CompetitiveGroupItems;
        [XmlArrayItem(ElementName = "EntranceTestsResultID")]
        public string[] EntranceTestResults;
        [XmlArrayItem(ElementName = "ApplicationCommonBenefitID")]
        public string[] ApplicationCommonBenefits;

        [XmlArrayItem(ElementName = "CampaignID")]
        public int[] Campaigns;

        [XmlArrayItem(ElementName = "CompetitiveGroupID")]
        public int[] CompetitiveGroups;

        [XmlArrayItem(ElementName = "Application")]
        public ApplicationShortRef[] ConsideredApplications;
        [XmlArrayItem(ElementName = "Application")]
        public ApplicationShortRef[] RecommendedApplications;

        [XmlArrayItem(ElementName = "RecommendedList")]
        public RecommendedListShort[] RecommendedLists;

        [XmlArrayItem(ElementName = "InstitutionAchievementUID")]
        public string[] InstitutionAchievements;

        public bool Exists
        {
            get
            {
                return
                    (Applications != null && Applications.Length > 0) ||
                    (OrdersOfAdmission != null && OrdersOfAdmission.Length > 0) ||
                    (CompetitiveGroupItems != null && CompetitiveGroupItems.Length > 0) ||
                    (EntranceTestResults != null && EntranceTestResults.Length > 0) ||
                    (ApplicationCommonBenefits != null && ApplicationCommonBenefits.Length > 0) ||
                    (Campaigns != null && Campaigns.Length > 0) ||
                    (CompetitiveGroups != null && CompetitiveGroups.Length > 0) ||
                    (ConsideredApplications != null && ConsideredApplications.Length > 0) ||
                    (RecommendedApplications != null && RecommendedApplications.Length > 0) ||
                    (InstitutionAchievements != null && InstitutionAchievements.Length > 0) ||
                    (RecommendedLists != null && RecommendedLists.Length > 0);
            }
        }
    }
}
