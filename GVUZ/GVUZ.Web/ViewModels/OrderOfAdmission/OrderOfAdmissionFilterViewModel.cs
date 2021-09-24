using System;
using System.ComponentModel;
using GVUZ.Web.Infrastructure;
using GVUZ.Web.ViewModels.Shared;
using GVUZ.DAL.Dapper.ViewModel.Common;

namespace GVUZ.Web.ViewModels.OrderOfAdmission
{
    public class OrderOfAdmissionFilterViewModel : IFilterState<OrderOfAdmissionFilterViewModel>
    {
        public OrderOfAdmissionFilterViewModel()
        {
            Campaigns = CreateDefaultSelectListModel<int>("[По всем приемным кампаниям]");
            Stages = CreateDefaultSelectListModel<int>("[По всем этапам приема]");
            Stages.Add(0, "0");
            Stages.Add(1, "1");
            //Stages.Add(2, "2");

            OrderStatuses = CreateDefaultSelectListModel<short>("[По всем статусам приказа]");

            EducationLevels = CreateDefaultSelectListModel<short>("[По всем уровням образования]");
            EducationForms = CreateDefaultSelectListModel<short>("[По всем формам обучения]");
            EducationSources = CreateDefaultSelectListModel<short>("[По всем источникам финансирования]");

            IsForBeneficiaryList = CreateDefaultSelectListModel<bool>("[Не важно]");
            IsForBeneficiaryList.Add(true, "Да");
            IsForBeneficiaryList.Add(false, "Нет");

            IsForeignerList = CreateDefaultSelectListModel<bool>("[Не важно]");
            IsForeignerList.Add(true, "Да");
            IsForeignerList.Add(false, "Нет");

        }
        public static readonly OrderOfAdmissionFilterViewModel Default = new OrderOfAdmissionFilterViewModel();

        [DisplayName("Наименование приказа")]
        public string OrderName { get; set; }

        [DisplayName("Приемная кампания")]
        public int? SelectedCampaign { get; set; }
        public SelectListViewModel<int> Campaigns { get; private set; }

        [DisplayName("Этап приема")]
        public int? SelectedStage { get; set; }
        public SelectListViewModel<int> Stages { get; private set; }

        [DisplayName("Номер приказа")]
        public string OrderNumber { get; set; }

        [DisplayName("Дата регистрации с")]
        public DateTime? OrderDateFrom { get; set; }

        [DisplayName("по")]
        public DateTime? OrderDateTo { get; set; }

        [DisplayName("Дата публикации с")]
        public DateTime? OrderPublishDateFrom { get; set; }

        [DisplayName("по")]
        public DateTime? OrderPublishDateTo { get; set; }

        [DisplayName("Статус приказа")]
        public int? SelectedOrderStatus { get; set; }
        public SelectListViewModel<short> OrderStatuses { get; private set; }
         
        [DisplayName("Уровень образования")]
        public int? SelectedEducationLevel { get; set; }
        public SelectListViewModel<short> EducationLevels { get; private set; }

        [DisplayName("Форма обучения")]
        public int? SelectedEducationForm { get; set; }
        public SelectListViewModel<short> EducationForms { get; private set; }

        [DisplayName("Источник финансирования")]
        public int? SelectedEducationSource { get; set; }
        public SelectListViewModel<short> EducationSources { get; private set; }

        [DisplayName("Льготный приказ")]
        public bool? IsForBeneficiary { get; set; }
        public SelectListViewModel<bool> IsForBeneficiaryList { get; private set; }

        [DisplayName("Прием по направлениям Минобрнауки")]
        public bool? IsForeigner { get; set; }
        public SelectListViewModel<bool> IsForeignerList { get; private set; }

        //Тип приказа
        public int OrderTypeId { get; set; }

        private static SelectListViewModel<T> CreateDefaultSelectListModel<T>(string unselectedText)
        {
            return new SelectListViewModel<T> { ShowUnselectedText = true, UnselectedText = unselectedText };
        }

        public OrderOfAdmissionFilterViewModel CloneInputFields(OrderOfAdmissionFilterViewModel source)
        {
            return new OrderOfAdmissionFilterViewModel
                {
                    IsForBeneficiary = source.IsForBeneficiary,
                    IsForeigner = source.IsForeigner,
                    OrderDateFrom = source.OrderDateFrom,
                    OrderDateTo = source.OrderDateTo,
                    OrderName = source.OrderName,
                    OrderNumber = source.OrderNumber,
                    OrderPublishDateFrom = source.OrderPublishDateFrom,
                    OrderPublishDateTo = source.OrderPublishDateTo,
                    SelectedCampaign = source.SelectedCampaign, 
                    SelectedEducationForm = source.SelectedEducationForm,
                    SelectedEducationLevel = source.SelectedEducationLevel,
                    SelectedEducationSource = source.SelectedEducationSource,
                    SelectedOrderStatus = source.SelectedOrderStatus,
                    SelectedStage = source.SelectedStage,
                    Campaigns = null, 
                    EducationForms = null,
                    EducationLevels = null,
                    EducationSources = null,
                    IsForBeneficiaryList = null,
                    IsForeignerList = null,
                    OrderStatuses = null,
                    Stages = null
                };
        }

        public OrderOfAdmissionFilterViewModel CloneInputFields()
        {
            return CloneInputFields(this);
        }
    }
}