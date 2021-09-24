using GVUZ.Web.ViewModels.Shared;

namespace GVUZ.Web.ViewModels
{
    public class InstitutionDirectionRequestListViewModel: PagedListViewModelBase<InstitutionDirectionRequestRecordViewModel>
    {
        public InstitutionDirectionRequestRecordViewModel RecordInfo
        {
            get { return InstitutionDirectionRequestRecordViewModel.MetadataInstance; }
        }
    }
}