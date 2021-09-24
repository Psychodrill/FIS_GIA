using GVUZ.Web.ViewModels.Shared;

namespace GVUZ.Web.ViewModels.ApplicationsList
{
    public class AcceptedApplicationsListViewModel : PagedListViewModelBase<AcceptedApplicationsRecordViewModel>
    {
        private static readonly AcceptedApplicationsRecordViewModel MetadataInstance = new AcceptedApplicationsRecordViewModel();

        public AcceptedApplicationsRecordViewModel RecordInfo
        {
            get { return MetadataInstance; }
        }

        private AcceptedApplicationsFilterViewModel _filter;

        public AcceptedApplicationsFilterViewModel Filter
        {
            get { return _filter ?? (_filter = new AcceptedApplicationsFilterViewModel()); }
            set { _filter = value; }
        }

        public int? HighlightApplicationId { get; set; }

        public int TotalApplicationsCount { get; set; }
    }
}