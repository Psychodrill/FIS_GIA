using System;
using System.ComponentModel;
using GVUZ.Web.Infrastructure;
using GVUZ.Web.ViewModels.Shared;
using GVUZ.DAL.Dapper.ViewModel.Common;

namespace GVUZ.Web.ViewModels.Entrants
{
    public class EntrantRecordListFilterViewModel : IFilterState<EntrantRecordListFilterViewModel>
    {
        public static readonly EntrantRecordListFilterViewModel Default = new EntrantRecordListFilterViewModel();

        public EntrantRecordListFilterViewModel()
        {
            CompetitiveGroups = new SelectListViewModel<int> {ShowUnselectedText = true, UnselectedText = " "};
            Campaigns = new SelectListViewModel<int> {ShowUnselectedText = true, UnselectedText = " "};
            Statuses = new SelectListViewModel<int> {ShowUnselectedText = true, UnselectedText = " "};
            CampaignYear = DateTime.Now.Year;
            CampaignYears = new SelectListViewModel<int> { ShowUnselectedText = true, UnselectedText = CampaignYear.ToString() };
        }


        [DisplayName("Год начала ПК")]
        public int CampaignYear { get; set; }
        [DisplayName("Номер заявления")]
        public string ApplicationNumber { get; set; }

        [DisplayName("Дата регистрации с")]
        public DateTime? RegistrationDateFrom { get; set; }

        [DisplayName("по")]
        public DateTime? RegistrationDateTo { get; set; }

        [DisplayName("Конкурс")]
        public int? SelectedCompetitiveGroup { get; set; }
        public SelectListViewModel<int> CompetitiveGroups { get; private set; }

        [DisplayName("Статус заявления")]
        public int? SelectedStatus { get; set; }
        public SelectListViewModel<int> Statuses { get; private set; }

        [DisplayName("Приемная кампания")]
        public int? SelectedCampaign { get; set; }
        public SelectListViewModel<int> Campaigns { get; private set; }

        [DisplayName("Фамилия")]
        public string LastName { get; set; }

        [DisplayName("Имя")]
        public string FirstName { get; set; }

        [DisplayName("Отчество")]
        public string MiddleName { get; set; }

        [DisplayName("Серия паспорта")]
        public string DocumentSeries { get; set; }

        [DisplayName("№ паспорта")]
        public string DocumentNumber { get; set; }

        public SelectListViewModel<int> CampaignYears { get; private set; }

        public EntrantRecordListFilterViewModel CloneInputFields(EntrantRecordListFilterViewModel source)
        {
            return new EntrantRecordListFilterViewModel
                {
                    ApplicationNumber = source.ApplicationNumber,
                    DocumentNumber = source.DocumentNumber,
                    DocumentSeries = source.DocumentSeries,
                    FirstName = source.FirstName,
                    LastName = source.LastName,
                    MiddleName = source.MiddleName,
                    RegistrationDateFrom = source.RegistrationDateFrom,
                    RegistrationDateTo = source.RegistrationDateTo,
                    SelectedCampaign = source.SelectedCampaign,
                    SelectedCompetitiveGroup = source.SelectedCompetitiveGroup,
                    SelectedStatus = source.SelectedStatus,
                    Campaigns = null,
                    CompetitiveGroups = null,
                    Statuses = null,
                    CampaignYear = source.CampaignYear,
                    CampaignYears = source.CampaignYears
            };
        }

        public EntrantRecordListFilterViewModel CloneInputFields()
        {
            return CloneInputFields(this);
        }
    }
}