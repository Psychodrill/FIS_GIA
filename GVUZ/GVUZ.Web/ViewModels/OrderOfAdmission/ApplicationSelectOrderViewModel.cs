using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using GVUZ.Web.Infrastructure;
using GVUZ.Web.ViewModels.Shared;
using GVUZ.DAL.Dapper.ViewModel.Common;

namespace GVUZ.Web.ViewModels.OrderOfAdmission
{
    public class ApplicationSelectOrderViewModel : PagedListViewModelBase<ApplicationSelectOrderRecordViewModel>
    {
        public int ApplicationId { get; set; }
        public int[] ApplicationsId { get; set; }

        public ApplicationSelectOrderFilterViewModel Filter { get; set; }

        public ApplicationSelectOrderRecordViewModel RecordInfo
        {
            get { return null; }
        }
        public int TotalOrdersCount { get; set; }
    }

    public class ApplicationSelectOrderOfAdmissionRefuseViewModel : ApplicationSelectOrderViewModel
    {
        public int? OrderOfAdmissionId { get; set; }
        public int[] ApplicationItemIds { get; set; }
    }

    #region ApplicationSelectOrderFilterViewModel

    public class ApplicationSelectOrderFilterViewModel : IFilterState<ApplicationSelectOrderFilterViewModel>
    {
        public ApplicationSelectOrderFilterViewModel()
        {
            Stages = CreateDefaultSelectListModel<int>("[По всем этапам приема]");
            Stages.Add(0, "0");
            Stages.Add(1, "1");
            //Stages.Add(2, "2");

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

        public static readonly ApplicationSelectOrderFilterViewModel Default =
            new ApplicationSelectOrderFilterViewModel();

        [DisplayName("Наименование")]
        public string OrderName { get; set; }

        [DisplayName("Уровень образования")]
        public int? SelectedEducationLevel { get; set; }

        public SelectListViewModel<short> EducationLevels { get; private set; }
         
        [DisplayName("Форма обучения")]
        public int? SelectedEducationForm { get; set; }

        public SelectListViewModel<short> EducationForms { get; private set; }

        [DisplayName("Источник финансирования")]
        public int? SelectedEducationSource { get; set; }

        public SelectListViewModel<short> EducationSources { get; private set; }

        [DisplayName("Этап")]
        public int? SelectedStage { get; set; }

        public SelectListViewModel<int> Stages { get; private set; }

        [DisplayName("Льготный приказ")]
        public bool? IsForBeneficiary { get; set; }

        public SelectListViewModel<bool> IsForBeneficiaryList { get; private set; }

        [DisplayName("Прием по направлениям Минобрнауки")]
        public bool? IsForeigner { get; set; }

        public SelectListViewModel<bool> IsForeignerList { get; private set; }

        private static SelectListViewModel<T> CreateDefaultSelectListModel<T>(string unselectedText)
        {
            return new SelectListViewModel<T> {ShowUnselectedText = true, UnselectedText = unselectedText};
        }

        public ApplicationSelectOrderFilterViewModel CloneInputFields(ApplicationSelectOrderFilterViewModel source)
        {
            return new ApplicationSelectOrderFilterViewModel
            {
                IsForBeneficiary = source.IsForBeneficiary,
                IsForeigner = source.IsForeigner,
                OrderName = source.OrderName, 
                SelectedEducationForm = source.SelectedEducationForm,
                SelectedEducationLevel = source.SelectedEducationLevel,
                SelectedEducationSource = source.SelectedEducationSource,
                SelectedStage = source.SelectedStage, 
                EducationForms = null,
                EducationLevels = null,
                EducationSources = null,
                IsForBeneficiaryList = null,
                IsForeignerList = null,
                Stages = null
            };
        }

        public ApplicationSelectOrderFilterViewModel CloneInputFields()
        {
            return CloneInputFields(this);
        }
    }

    #endregion

    #region ApplicationSelectOrderRecordViewModel

    public class ApplicationSelectOrderRecordViewModel
    {
        public int OrderId { get; set; }
        public short OrderStatusId { get; set; }

        [DisplayName("Наименование")]
        public string OrderName { get; set; }

        [DisplayName("Номер приказа")]
        public string OrderNumber { get; set; }

        [DisplayName("Уровень образования")]
        public string EducationLevel { get; set; } 

        [DisplayName("Форма обучения")]
        public string EducationForm { get; set; }

        [DisplayName("Источник финансирования")]
        public string EducationSource { get; set; }

        [DisplayName("Этап")]
        public string Stage { get; set; }

        [DisplayName("Льготный приказ")]
        public bool IsForBeneficiary { get; set; }

        [DisplayName("Прием по направлениям Минобрнауки")]
        public bool IsForeigner { get; set; }
    }
    #endregion

    #region ApplicationSelectOrderQueryViewModel

    public class ApplicationSelectOrderQueryViewModel : PagedQueryViewModelBase
    {
        public static readonly string[] ValidSortKeys = new[]
        {
            "OrderId",
            "OrderName",
            "EducationLevel", 
            "EducationForm",
            "EducationSource",
            "Stage",
            "NumberOfApplicants"
        };

        public ApplicationSelectOrderQueryViewModel()
            : base(ValidSortKeys[1])
        {
        }

        public ApplicationSelectOrderFilterViewModel Filter { get; set; }

        protected override bool IsValidSortKey(string value)
        {
            return !string.IsNullOrWhiteSpace(value) &&
                   ValidSortKeys.Any(key => key.Equals(value, StringComparison.OrdinalIgnoreCase));
        }
    }

    #endregion
}