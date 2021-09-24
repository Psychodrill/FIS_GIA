using GVUZ.Web.ViewModels.Shared;

namespace GVUZ.Web.ViewModels.ApplicationsList
{
    public class NewApplicationsListViewModel : PagedListViewModelBase<NewApplicationsRecordViewModel>
    {
        public NewApplicationsRecordViewModel RecordInfo
        {
            get { return NewApplicationsRecordViewModel.MetadataInstance; }
        }

        private NewApplicationsFilterViewModel _filter;

        public NewApplicationsFilterViewModel Filter
        {
            get { return _filter ?? (_filter = new NewApplicationsFilterViewModel()); }
            set { _filter = value; }
        }

        public int InstitutionId { get; set; }

        public int? HighlightApplicationId { get; set; }

        public int TotalApplicationsCount { get; set; }
    }
}