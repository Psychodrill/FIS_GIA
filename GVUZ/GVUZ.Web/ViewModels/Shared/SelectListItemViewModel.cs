namespace GVUZ.Web.ViewModels.Shared
{
    public class SelectListItemViewModel<TId>
    {
        public SelectListItemViewModel()
        {   
        }

        public SelectListItemViewModel(TId id, string displayName)
        {
            Id = id;
            DisplayName = displayName;
        }

        public SelectListItemViewModel(TId id, string displayName, int campaignStatusID)
        {
            Id = id;
            DisplayName = displayName;
            CampaignStatusID = campaignStatusID;
        }

        public TId Id { get; set; }
        public string DisplayName { get; set; }
        //public bool Additional { get; set; }
        public int CampaignStatusID { get; set; }
    }
}