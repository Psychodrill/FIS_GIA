namespace GVUZ.DAL.Dapper.ViewModel.Common
{
    public class SelectListItemViewModel<TId>
    {
        public SelectListItemViewModel()
        {
        }
        public SelectListItemViewModel(TId id)
        {
            Id = id;
            DisplayName = id.ToString();
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

        // Чтобы корректно отображать в Html.DropDownListExFor
        public string Name { get { return DisplayName; } }

        public int CampaignStatusID { get; set; }
    }
}
