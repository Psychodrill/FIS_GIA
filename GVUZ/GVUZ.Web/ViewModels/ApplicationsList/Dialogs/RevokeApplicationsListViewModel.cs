using System.Collections;
using System.Collections.Generic;

namespace GVUZ.Web.ViewModels.ApplicationsList
{
    public class RevokeApplicationsListViewModel
    {
        public static readonly RevokeApplicationsListViewModel MetadataInstance = new RevokeApplicationsListViewModel();

        private List<RevokeApplicationRecordViewModel> _applicationRecords;
        public List<IDName> ReturnDocumentsTypes { get; set; }

        public RevokeApplicationRecordViewModel RecordInfo
        {
            get { return RevokeApplicationRecordViewModel.MetadataInstance; }
        }

        public List<RevokeApplicationRecordViewModel>  ApplicationRecords
        {
            get { return _applicationRecords ?? (_applicationRecords = new List<RevokeApplicationRecordViewModel>()); }
            set { _applicationRecords = value; }
        }
    }

    //public class ReturnDocumentsTypes
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}