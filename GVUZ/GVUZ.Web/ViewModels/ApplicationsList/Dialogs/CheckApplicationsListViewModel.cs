using System.Collections.Generic;

namespace GVUZ.Web.ViewModels.ApplicationsList
{
    public class CheckApplicationsListViewModel
    {
        public static readonly CheckApplicationsListViewModel MetadataInstance = new CheckApplicationsListViewModel();

        private List<CheckApplicationRecordViewModel> _records;

        public List<CheckApplicationRecordViewModel> ApplicationRecords
        {
            get { return _records ?? (_records = new List<CheckApplicationRecordViewModel>()); }
            set { _records = value; }
        }

        public CheckApplicationRecordViewModel RecordInfo
        {
            get { return CheckApplicationRecordViewModel.MetadataInstance; }
        }

        public bool ApplicationsRemoved { get; set; }
    }
}