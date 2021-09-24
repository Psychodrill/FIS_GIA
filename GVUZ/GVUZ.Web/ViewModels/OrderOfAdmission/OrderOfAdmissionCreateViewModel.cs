using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GVUZ.Web.ViewModels.Shared;
using GVUZ.DAL.Dapper.ViewModel.Common;

namespace GVUZ.Web.ViewModels.OrderOfAdmission
{
    public class OrderOfAdmissionCreateViewModel
    {
        //private int[] _applicationId;
        private const string DefaultUnselectedText = "Не выбрано";
         
        public OrderOfAdmissionCreateViewModel()
        {
            Campaigns = CreateDefaultSelectListModel<int>();
            EducationLevels = CreateDefaultSelectListModel<short>();
            EducationForms = CreateDefaultSelectListModel<short>();
            EducationSource = CreateDefaultSelectListModel<short>();

            Stages = CreateDefaultSelectListModel<short>();
            Stages.Add(0, "0");
            Stages.Add(1, "1");
            //Stages.Add(2, "2"); 
        }

        protected static SelectListViewModel<T> CreateDefaultSelectListModel<T>(string unselectedText = DefaultUnselectedText)
        {
            return new SelectListViewModel<T> { ShowUnselectedText = true, UnselectedText = unselectedText };
        }

        public int OrderTypeId { get; set; }
         
        public string OrderTypeNamePrepositional
        {
            get
            {
                if (OrderTypeId == GVUZ.Data.Model.OrderOfAdmissionType.OrderOfAdmission)
                    return "о зачислении";
                else if (OrderTypeId == GVUZ.Data.Model.OrderOfAdmissionType.OrderOfAdmissionRefuse)
                    return "об отказе от зачисления";
                return null;
            }
        }

        [DisplayName("Наименование приказа")]
        [StringLength(200)]
        public string OrderName { get; set; }

        [DisplayName("Регистрационный номер приказа")]
        [StringLength(50)]
        public string OrderNumber { get; set; }

        [DisplayName("Дата регистрации приказа")]
        public DateTime? OrderDate { get; set; }

        public string OrderDateText
        {
            get { return OrderDate.HasValue ? OrderDate.Value.ToString("dd.MM.yyyy") : null; }
        }

        [DisplayName("Приемная кампания")]
        [Required(ErrorMessage = "Необходимо указать приемную кампанию")]
        public int? CampaignId { get; set; }
        public SelectListViewModel<int> Campaigns { get; private set; }
        public int? CampaignStatusID { get; set; }
        public bool Additional { get; set; }

        [DisplayName("Уровень образования")]
        public short? EducationLevelId { get; set; }
        public SelectListViewModel<short> EducationLevels { get; private set; }

        [DisplayName("Форма обучения")]
        public short? EducationFormId { get; set; }
        public SelectListViewModel<short> EducationForms { get; private set; }

        [DisplayName("Источник финансирования")]
        public short? EducationSourceId { get; set; }
        public SelectListViewModel<short> EducationSource { get; private set; }

        [DisplayName("Этап приема")]
        public short? Stage { get; set; }
        public SelectListViewModel<short> Stages { get; set; }

        [DisplayName("Льготный приказ")]
        public bool? IsForBeneficiary { get; set; } 

        [DisplayName("Прием по направлениям Минобрнауки")]
        public bool? IsForeigner { get; set; }

        public string IsForBeneficiaryName
        {
            get { return IsForBeneficiary.HasValue ? IsForBeneficiary.Value ? "да" : "нет" : null; }
        } 
        public string IsForeignerName
        {
            get { return IsForeigner.HasValue ? IsForeigner.Value ? "да" : "нет" : null; }
        }

        public bool Reposted { get; set; }

        public int[] ApplicationId { get; set; }

        // Признак того, что приказ формируется на основе данных одного или более заявлений
        public bool FromApplication { get; set; }

        public string SingleCampaignName
        {
            get
            {
                var item = Campaigns.Items.FirstOrDefault();
                return item == null ? string.Empty : item.DisplayName;
            }
        } 

        [DisplayName("Идентификатор в БД ОО (UID)")]
        [StringLength(200)]
        public string UID { get; set; }
    }
}