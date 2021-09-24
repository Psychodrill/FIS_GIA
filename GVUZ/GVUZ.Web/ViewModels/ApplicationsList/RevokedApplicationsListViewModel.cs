using GVUZ.Web.ViewModels.Shared;

namespace GVUZ.Web.ViewModels.ApplicationsList
{
    public class RevokedApplicationsListViewModel : PagedListViewModelBase<RevokedApplicationsRecordViewModel>
    {
        private static readonly RevokedApplicationsRecordViewModel MetadataInstance = new RevokedApplicationsRecordViewModel();

        public RevokedApplicationsRecordViewModel RecordInfo
        {
            get { return MetadataInstance; }
        }

        private RevokedApplicationsFilterViewModel _filter;

        public RevokedApplicationsFilterViewModel Filter
        {
            get { return _filter ?? (_filter = new RevokedApplicationsFilterViewModel()); }
            set { _filter = value; }
        }

        public int? HighlightApplicationId { get; set; }

        public int TotalApplicationsCount { get; set; }
    }
}