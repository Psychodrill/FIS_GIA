using System.Collections.Generic;

namespace GVUZ.Web.ViewModels.ApplicationsList
{
    public class AcceptApplicationsListViewModel
    {
        public static readonly AcceptApplicationsListViewModel MetadataInstance = new AcceptApplicationsListViewModel();

        private List<AcceptApplicationRecordViewModel> _applicationRecords;

        public AcceptApplicationRecordViewModel RecordInfo
        {
            get { return AcceptApplicationRecordViewModel.MetadataInstance; }
        }

        public List<AcceptApplicationRecordViewModel> ApplicationRecords
        {
            get { return _applicationRecords ?? (_applicationRecords = new List<AcceptApplicationRecordViewModel>()); }
            set { _applicationRecords = value; }
        }
    }
}