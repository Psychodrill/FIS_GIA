using GVUZ.Web.ViewModels.Shared;

namespace GVUZ.Web.ViewModels.CompositionResults
{
    public class CompositionResultsListViewModel : PagedListViewModelBase<CompositionResultRecordViewModel>
    {
        private CompositionResultsFilterViewModel _filter;
        public CompositionResultsFilterViewModel Filter
        {
            get { return _filter ?? (_filter = new CompositionResultsFilterViewModel()); }
            set { _filter = value; }
        }

        public CompositionResultRecordViewModel RecordInfo
        {
            get { return null; }
        }
    }
}