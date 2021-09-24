using System;
using System.ComponentModel;
using GVUZ.Web.Infrastructure;
using GVUZ.Web.ViewModels.Shared;
using GVUZ.DAL.Dapper.ViewModel.Common;

namespace GVUZ.Web.ViewModels.ApplicationsList
{
    public class SearchApplicationsFilterViewModel : IFilterState<SearchApplicationsFilterViewModel>
    {
        public static readonly SearchApplicationsFilterViewModel Default = new SearchApplicationsFilterViewModel();

        public SearchApplicationsFilterViewModel()
        {
            CompetitiveGroups = new SelectListViewModel<int>{ ShowUnselectedText = true, UnselectedText = " " };
            Statuses = new SelectListViewModel<int> {ShowUnselectedText = true, UnselectedText = "[По всем статусам]"};
        }

        [DisplayName("Номер заявления")]
        public string ApplicationNumber { get; set; }

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

        public SelectListViewModel<int> CompetitiveGroups { get; private set; }

        [DisplayName("Конкурс")]
        public int? SelectedCompetitiveGroup { get; set; }

        public SelectListViewModel<int> Statuses { get; private set; }

        [DisplayName("Статус")]
        public int? SelectedStatus { get; set; }

        public SearchApplicationsFilterViewModel CloneInputFields(SearchApplicationsFilterViewModel source)
        {
            return new SearchApplicationsFilterViewModel
                {
                    ApplicationNumber = source.ApplicationNumber,
                    DocumentNumber = source.DocumentNumber,
                    DocumentSeries = source.DocumentSeries,
                    FirstName = source.FirstName,
                    LastName = source.LastName,
                    MiddleName = source.MiddleName,
                    RegistrationDateFrom = source.RegistrationDateFrom,
                    RegistrationDateTo = source.RegistrationDateTo,
                    SelectedCompetitiveGroup = source.SelectedCompetitiveGroup,
                    SelectedStatus = source.SelectedStatus,
                    CompetitiveGroups = null,
                    Statuses = null
                };
        }

        public SearchApplicationsFilterViewModel CloneInputFields()
        {
            return CloneInputFields(this);
        }
    }
}