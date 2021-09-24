using System.Collections.Generic;

namespace GVUZ.Web.ViewModels.Shared
{
    public class PagedListViewModelBase<TRecord>
    {
        private List<TRecord> _records;
        private PagerViewModel _pager;

        public List<TRecord> Records
        {
            get { return _records ?? (_records = new List<TRecord>()); }
            set { _records = value; }
        }

        public PagerViewModel Pager
        {
            get { return _pager ?? (_pager = new PagerViewModel()); }
            set { _pager = value; }
        }

        public string SortKey { get; set; }
        public bool? SortDescending { get; set; }
    }
}