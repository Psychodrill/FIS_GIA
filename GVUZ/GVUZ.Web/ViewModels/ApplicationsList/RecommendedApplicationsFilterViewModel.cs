using System.ComponentModel;
using GVUZ.Web.Infrastructure;
using GVUZ.Web.ViewModels.Shared;
using GVUZ.DAL.Dapper.ViewModel.Common;

namespace GVUZ.Web.ViewModels.ApplicationsList
{
    public class RecommendedApplicationsFilterViewModel : IFilterState<RecommendedApplicationsFilterViewModel>
    {
        public static readonly RecommendedApplicationsFilterViewModel Default = new RecommendedApplicationsFilterViewModel();

        public RecommendedApplicationsFilterViewModel()
        {
            CompetitiveGroups = new SelectListViewModel<int> { ShowUnselectedText = true, UnselectedText = "[По всем конкурсам]" };
            OriginalDocumentsOptions = new SelectListViewModel<bool?>();
            OriginalDocumentsOptions.Add(null, "[Не важно]");
            OriginalDocumentsOptions.Add(true, "Да");
            OriginalDocumentsOptions.Add(false, "Нет");
            EducationLevels = new SelectListViewModel<short> { ShowUnselectedText = true, UnselectedText = "[По всем уровням образования]" };
            EducationForms = new SelectListViewModel<short> { ShowUnselectedText = true, UnselectedText = "[По всем формам обучения]" };
            Directions = new SelectListViewModel<int> { ShowUnselectedText = true, UnselectedText = "[По всем направлениям подготовки]" };
            Campaigns = new SelectListViewModel<int> { ShowUnselectedText = true, UnselectedText = "[По всем приемным кампаниям]" };
            Stages = new SelectListViewModel<int>();
            Stages.Add(0, "[По всем этапам приема]");
            Stages.Add(1, "1 этап");
            Stages.Add(2, "2 этап");
        }
        
        [DisplayName("Номер заявления")]
        public string ApplicationNumber { get; set; }

        public SelectListViewModel<bool?> OriginalDocumentsOptions { get; private set; }

        [DisplayName("Сдал документы")]
        public bool? OriginalDocumentsReceived { get; set; }

        [DisplayName("Фамилия")]
        public string LastName { get; set; }

        [DisplayName("Имя")]
        public string FirstName { get; set; }

        [DisplayName("Отчество")]
        public string MiddleName { get; set; }

        public SelectListViewModel<short> EducationLevels { get; private set; }
            
        [DisplayName("Уровень")]
        public int? SelectedEducationLevel { get; set; }
        
        public SelectListViewModel<int> CompetitiveGroups { get; private set; }

        [DisplayName("Конкурс")]
        public int? SelectedCompetitiveGroup { get; set; }

        public SelectListViewModel<short> EducationForms { get; private set; }

        [DisplayName("Форма обучения")]
        public int? SelectedEducationForm { get; set; }

        public SelectListViewModel<int> Directions { get; private set; }

        [DisplayName("Направление подготовки")]
        public int? SelectedDirection { get; set; }

        public SelectListViewModel<int> Campaigns { get; private set; }

        [DisplayName("Приемная кампания")]
        public int? SelectedCampaign { get; set; }

        public SelectListViewModel<int> Stages { get; private set; }

        [DisplayName("Этап зачисления")]
        public int? SelectedStage { get; set; }

        public RecommendedApplicationsFilterViewModel CloneInputFields(RecommendedApplicationsFilterViewModel source)
        {
            return new RecommendedApplicationsFilterViewModel
                {
                    ApplicationNumber = source.ApplicationNumber,
                    FirstName = source.FirstName,
                    LastName = source.LastName,
                    MiddleName = source.MiddleName,
                    OriginalDocumentsReceived = source.OriginalDocumentsReceived,
                    SelectedCampaign = source.SelectedCampaign,
                    SelectedCompetitiveGroup = source.SelectedCompetitiveGroup,
                    SelectedDirection = source.SelectedDirection,
                    SelectedEducationForm = source.SelectedEducationForm,
                    SelectedEducationLevel = source.SelectedEducationLevel,
                    SelectedStage = source.SelectedStage,
                    Campaigns = null,
                    CompetitiveGroups = null,
                    Directions = null,
                    EducationForms = null,
                    EducationLevels = null,
                    OriginalDocumentsOptions = null,
                    Stages = null
                };
        }

        public RecommendedApplicationsFilterViewModel CloneInputFields()
        {
            return CloneInputFields(this);
        }
    }
}