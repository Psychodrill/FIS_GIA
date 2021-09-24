using GVUZ.Model.Entrants;

namespace GVUZ.Web.ViewModels.Administration
{
	public class AppExtraViewModel
	{
		public int ApplicationExtraID { get; set; }
		public string Name { get; set; }
		public string Value { get; set; }
	}

	public class AppCampaignViewModel
	{
		public ApplicationExtraDefinition[] ExtraFields { get; set; }
		public string ReceiveApplicationsDate { get; set; }
	}
}