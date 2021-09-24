using GVUZ.Web.ViewModels.Shared;

namespace GVUZ.Web.ViewModels.ApplicationsList
{
    public class UncheckedApplicationsListViewModel : PagedListViewModelBase<UncheckedApplicationsRecordViewModel>
    {
        public UncheckedApplicationsRecordViewModel RecordInfo
        {
            get { return UncheckedApplicationsRecordViewModel.MetadataInstance; }
        }

        private UncheckedApplicationsFilterViewModel _filter;

        public UncheckedApplicationsFilterViewModel Filter
        {
            get { return _filter ?? (_filter = new UncheckedApplicationsFilterViewModel()); }
            set { _filter = value; }
        }

        public int? HighlightApplicationId { get; set; }

        public int TotalApplicationsCount { get; set; }
    }
}