using GVUZ.Web.ViewModels.Shared;

namespace GVUZ.Web.ViewModels.ApplicationsList
{
    public class RecommendedApplicationsListViewModel : PagedListViewModelBase<RecommendedApplicationsRecordViewModel>
    {
        private static readonly RecommendedApplicationsRecordViewModel MetadataInstance = new RecommendedApplicationsRecordViewModel();

        public RecommendedApplicationsRecordViewModel RecordInfo
        {
            get { return MetadataInstance; }
        }

        private RecommendedApplicationsFilterViewModel _filter;

        public RecommendedApplicationsFilterViewModel Filter
        {
            get { return _filter ?? (_filter = new RecommendedApplicationsFilterViewModel()); }
            set { _filter = value; }
        }

        public int TotalApplicationsCount { get; set; }
    }
}