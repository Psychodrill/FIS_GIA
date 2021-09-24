using System.ComponentModel;
using GVUZ.Web.Infrastructure;
using GVUZ.Web.ViewModels.Shared;
using GVUZ.DAL.Dapper.ViewModel.Common;

namespace GVUZ.Web.ViewModels.OrderOfAdmission
{
    public class ApplicationOrderFilterViewModel : IFilterState<ApplicationOrderFilterViewModel>
    {
        public static readonly ApplicationOrderFilterViewModel Default = new ApplicationOrderFilterViewModel();

        public ApplicationOrderFilterViewModel()
        {
            EducationLevels = new SelectListViewModel<short> {ShowUnselectedText = true, UnselectedText = "Любой"};
            EducationForms = new SelectListViewModel<short> { ShowUnselectedText = true, UnselectedText = "Любая" };
            EducationSources = new SelectListViewModel<short> { ShowUnselectedText = true, UnselectedText = "Любой" };
            CompetitiveGroups = new SelectListViewModel<int> { ShowUnselectedText = true, UnselectedText = "Любая" };
            Benefits = new SelectListViewModel<short> { ShowUnselectedText = true, UnselectedText = "Любая" };
            Benefits.Add(-1, "Без льгот");
        }

        [DisplayName("Номер заявления")]
        public string ApplicationNumber { get; set; }

        [DisplayName("Фамилия")]
        public string LastName { get; set; }

        [DisplayName("Имя")]
        public string FirstName { get; set; }

        [DisplayName("Отчество")]
        public string MiddleName { get; set; }

        [DisplayName("серия")]
        public string DocumentSeries { get; set; }

        [DisplayName("номер")]
        public string DocumentNumber { get; set; }

        [DisplayName("Направление подготовки/ специальность")]
        public string DirectionName { get; set; }

        [DisplayName("Уровень образования")]
        public int? SelectedEducationLevel { get; set; }
        public SelectListViewModel<short> EducationLevels { get; private set; }

        [DisplayName("Форма обучения")]
        public int? SelectedEducationForm { get; set; }
        public SelectListViewModel<short> EducationForms { get; private set; }

        [DisplayName("Источник финансирования")]
        public int? SelectedEducationSource { get; set; }
        public SelectListViewModel<short> EducationSources { get; private set; }

        [DisplayName("Конкурс")]
        public int? SelectedCompetitiveGroup { get; set; }
        public SelectListViewModel<int> CompetitiveGroups { get; private set; }

        [DisplayName("Льгота")]
        public int? SelectedBenefit { get; set; }
        public SelectListViewModel<short> Benefits { get; private set; }

        public ApplicationOrderFilterViewModel CloneInputFields(ApplicationOrderFilterViewModel source)
        {
            return new ApplicationOrderFilterViewModel
                {
                    ApplicationNumber = source.ApplicationNumber,
                    LastName = source.LastName,
                    FirstName = source.FirstName,
                    MiddleName = source.MiddleName,
                    DocumentSeries = source.DocumentSeries,
                    DocumentNumber = source.DocumentNumber,
                    DirectionName = source.DirectionName,
                    SelectedEducationLevel = source.SelectedEducationLevel,
                    SelectedEducationForm = source.SelectedEducationForm,
                    SelectedEducationSource = source.SelectedEducationSource,
                    SelectedCompetitiveGroup = source.SelectedCompetitiveGroup,
                    SelectedBenefit = source.SelectedBenefit,
                    EducationLevels = null,
                    EducationForms = null,
                    EducationSources = null,
                    CompetitiveGroups = null,
                    Benefits = null
                };
        }

        public ApplicationOrderFilterViewModel CloneInputFields()
        {
            return CloneInputFields(this);
        }
    }
}