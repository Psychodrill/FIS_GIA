using System.ComponentModel;
using System.Globalization;
using GVUZ.Web.ViewModels.Shared;
using GVUZ.DAL.Dapper.ViewModel.Common;
using GVUZ.Data.Model;

namespace GVUZ.Web.ViewModels.OrderOfAdmission
{
    public class OrderOfAdmissionEditViewModel : OrderOfAdmissionCreateViewModel
    {
        private ApplicationOrderListViewModel _applicationList;

        public OrderOfAdmissionEditViewModel()
        {
            OrderStatusList = new SelectListViewModel<int> { ShowUnselectedText = false };
        }

        public int OrderId { get; set; } 

        [DisplayName("Статус приказа")]
        public int OrderStatus { get; set; }

        [DisplayName("Дата создания")]
        public string DateCreated { get; set; }

        [DisplayName("Дата изменения")]
        public string DateEdited { get; set; }

        [DisplayName("Дата публикации")]
        public string DatePublished { get; set; }

        public string EducationLevelName { get; set; }

        public string EducationFormName { get; set; }

        public string EducationSourceName { get; set; }

        public string CampaignName { get; set; }

        public SelectListViewModel<int> OrderStatusList { get; private set; }

        public string StageName
        {
            get { return Stage.HasValue ? Stage.Value.ToString(CultureInfo.InvariantCulture) : null; }
        }

        public string OrderStatusName { get; set; }

        public bool AllowPublish
        {
            get { return (!IsCampaignFinished) && (OrderStatus == OrderOfAdmissionStatus.NotPublished); }
        }

        public bool IsPublished
        {
            get { return OrderStatus == OrderOfAdmissionStatus.Published; }
        }

        public bool IsNoApplications
        {
            get { return OrderStatus == OrderOfAdmissionStatus.NoApplications; }
        }
         
        public ApplicationOrderListViewModel ApplicationList
        {
            get { return _applicationList ?? (_applicationList = new ApplicationOrderListViewModel()); }
            set { _applicationList = value; }
        }

        public bool IsCampaignFinished { get; set; }
    }
}