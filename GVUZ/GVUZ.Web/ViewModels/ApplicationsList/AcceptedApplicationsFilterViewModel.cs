using System;
using System.ComponentModel;
using GVUZ.Web.Infrastructure;
using GVUZ.Web.ViewModels.Shared;
using GVUZ.DAL.Dapper.ViewModel.Common;

namespace GVUZ.Web.ViewModels.ApplicationsList
{
    public class AcceptedApplicationsFilterViewModel : IFilterState<AcceptedApplicationsFilterViewModel>
    {
        public static readonly AcceptedApplicationsFilterViewModel Default = new AcceptedApplicationsFilterViewModel();

        public AcceptedApplicationsFilterViewModel()
        {
            CompetitiveGroups = new SelectListViewModel<int> { ShowUnselectedText = true, UnselectedText = "[По всем конкурсам]" };
            Benefits = new SelectListViewModel<short>();
            Benefits.Add(0, "[По всем категориям]");
            Benefits.Add(-1, "Без льгот");
            SelectedBenefitId = 0; 
            OriginalDocumentsOptions = new SelectListViewModel<bool?>();
            OriginalDocumentsOptions.Add(null, "[Не важно]");
            OriginalDocumentsOptions.Add(true, "Да");
            OriginalDocumentsOptions.Add(false, "Нет");
            Campaigns = new SelectListViewModel<int> { ShowUnselectedText = true, UnselectedText = "[По всем приемным кампаниям]" };
            EducationFormTypes = new EducationFormsSelectList { ShowUnselectedText = true, UnselectedText = "[По всем формам обучения]" };
            EducationSourceTypes = new EducationSourceSelectList { ShowUnselectedText = true, UnselectedText = "[По всем источникам финансирования]" };
            CampaignYear = DateTime.Now.Year;
            CampaignYears = new SelectListViewModel<int> { ShowUnselectedText = true, UnselectedText = CampaignYear.ToString() };
        }

        [DisplayName("Номер заявления")]
        public string ApplicationNumber { get; set; }

        [DisplayName("Сдал документы")]
        public bool? OriginalDocumentsReceived { get; set; }

        [DisplayName("Фамилия")]
        public string LastName { get; set; }

        [DisplayName("Имя")]
        public string FirstName { get; set; }

        [DisplayName("Отчество")]
        public string MiddleName { get; set; }

        [DisplayName("Дата регистрации с")]
        public DateTime? RegistrationDateFrom { get; set; }

        [DisplayName("по")]
        public DateTime? RegistrationDateTo { get; set; }

        [DisplayName("Серия паспорта")]
        public string DocumentSeries { get; set; }

        [DisplayName("Номер паспорта")]
        public string DocumentNumber { get; set; }

        public SelectListViewModel<int> Campaigns { get; private set; }

        [DisplayName("Приемная кампания")]
        public int? SelectedCampaignId { get; set; }

        public EducationFormsSelectList EducationFormTypes { get; private set; }

        [DisplayName("Год начала ПК")]
        public int CampaignYear { get; set; }
        public EducationSourceSelectList EducationSourceTypes { get; private set; }

        [DisplayName("Форма обучения")]
        public EducationFormId SelectedEducationFormType { get; set; }

        [DisplayName("Источник финансирования")]
        public EducationSourceId SelectedEducationSourceType { get; set; }

        public SelectListViewModel<int> CompetitiveGroups { get; private set; }

        [DisplayName("Конкурс")]
        public int? SelectedCompetitiveGroup { get; set; }

        public SelectListViewModel<short> Benefits { get; private set; }

        [DisplayName("Льгота")]
        public int? SelectedBenefitId { get; set; }
         
        public SelectListViewModel<bool?> OriginalDocumentsOptions { get; private set; }
        public SelectListViewModel<int> CampaignYears { get; private set; }

        public AcceptedApplicationsFilterViewModel CloneInputFields()
        {
            return CloneInputFields(this);
        }

        public AcceptedApplicationsFilterViewModel CloneInputFields(AcceptedApplicationsFilterViewModel source)
        {
            return new AcceptedApplicationsFilterViewModel
            {
                ApplicationNumber = source.ApplicationNumber,
                FirstName = source.FirstName,
                LastName = source.LastName,
                MiddleName = source.MiddleName,
                OriginalDocumentsReceived = source.OriginalDocumentsReceived,
                RegistrationDateFrom = source.RegistrationDateFrom,
                RegistrationDateTo = source.RegistrationDateTo,
                SelectedBenefitId = source.SelectedBenefitId.GetValueOrDefault(),
                SelectedCompetitiveGroup = source.SelectedCompetitiveGroup,
                SelectedEducationFormType = source.SelectedEducationFormType,
                SelectedEducationSourceType = source.SelectedEducationSourceType,
                DocumentNumber = source.DocumentNumber,
                DocumentSeries = source.DocumentSeries,
                Benefits = null,
                CompetitiveGroups = null,
                EducationFormTypes = null,
                EducationSourceTypes = null,
                OriginalDocumentsOptions = null, 
                Campaigns = null,
                SelectedCampaignId = source.SelectedCampaignId,
                CampaignYear = source.CampaignYear,
                CampaignYears = source.CampaignYears
            };
        }
    }
}