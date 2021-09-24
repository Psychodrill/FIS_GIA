using GVUZ.DAL.Dapper.ViewModel.Common;
using GVUZ.Web.ViewModels.Shared;

namespace GVUZ.Web.ViewModels.OrderOfAdmission
{
    public class OrderOfAdmissionParametersViewModel
    {
        public OrderOfAdmissionParametersViewModel()
        {
            EducationLevels = new SelectListViewModel<short> {ShowUnselectedText = true, UnselectedText = "Не выбрано"};
            EducationForms = new SelectListViewModel<short> { ShowUnselectedText = true, UnselectedText = "Не выбрано" };
            EducationSources = new SelectListViewModel<short> { ShowUnselectedText = true, UnselectedText = "Не выбрано" };
            Campaigns = new SelectListViewModel<int> {ShowUnselectedText = true, UnselectedText = "Не выбрано"};
            Stages = new SelectListViewModel<short> {ShowUnselectedText = true, UnselectedText = "Не выбрано"};
            PublicationStatuses = new SelectListViewModel<int> { ShowUnselectedText = true, UnselectedText = "Не выбрано" };
        }

        public SelectListViewModel<short> EducationLevels { get; private set; }
        public SelectListViewModel<short> EducationForms { get; private set; }
        public SelectListViewModel<short> EducationSources { get; private set; }
        public SelectListViewModel<int> Campaigns { get; set; }
        public SelectListViewModel<short> Stages { get; set; }
        public SelectListViewModel<int> PublicationStatuses { get; set; } 

        public int? SelectedEducationLevel { get; set; }
        public int? SelectedEducationForm { get; set; }
        public int? SelectedEducationSource { get; set; }
        public int? SelectedCourse { get; set; }
        public int? SelectedCampaignId { get; set; }
        public bool SelectedAdditional { get; set; }
        public int? SelectedCampaignStatusID { get; set; }
        public int? SelectedStage { get; set; }
        public int? SelectedStatus { get; set; }
        public bool? IsForBeneficiary { get; set; }
        public bool? IsForeigner { get; set; }

        public string IsForBeneficiaryName
        {
            get { return IsForBeneficiary.HasValue ? IsForBeneficiary.Value ? "да" : "нет" : null; }
        } 
        public string IsForeignerName
        {
            get { return IsForeigner.HasValue ? IsForeigner.Value ? "да" : "нет" : null; }
        }

        public bool FromApplication { get; set; }
        public int[] ApplicationId { get; set; } 
    }
}