using GVUZ.ServiceModel.Import.WebService.Dto;
using System.Xml.Serialization;

namespace GVUZ.ImportService2016.Core.Main.Conflicts
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
	}
}