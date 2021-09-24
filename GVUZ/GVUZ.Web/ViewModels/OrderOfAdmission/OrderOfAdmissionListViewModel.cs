using GVUZ.Web.ViewModels.Shared;

namespace GVUZ.Web.ViewModels.OrderOfAdmission
{
    public class OrderOfAdmissionListViewModel : PagedListViewModelBase<OrderOfAdmissionRecordViewModel>
    {
        public OrderOfAdmissionFilterViewModel Filter { get; set; }

        public OrderOfAdmissionRecordViewModel RecordInfo { get { return null; }}

        public int TotalOrdersCount { get; set; }
    }
}