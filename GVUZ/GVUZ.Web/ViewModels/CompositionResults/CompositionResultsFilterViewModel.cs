using System;
using System.ComponentModel;
using GVUZ.Web.Infrastructure;
using GVUZ.Web.ViewModels.Shared;
using GVUZ.DAL.Dapper.ViewModel.Common;

namespace GVUZ.Web.ViewModels.CompositionResults
{
    public class CompositionResultsFilterViewModel : IFilterState<CompositionResultsFilterViewModel>
    {
        public static readonly CompositionResultsFilterViewModel Default = new CompositionResultsFilterViewModel();

        public CompositionResultsFilterViewModel()
        {
            Campaigns = new SelectListViewModel<int>
            {
                ShowUnselectedText = true,
                UnselectedText = "[По всем приемным кампаниям]"
            };

            CompetitiveGroups = new SelectListViewModel<int>
            {
                ShowUnselectedText = true,
                UnselectedText = "[По всем конкурсам]"
            };
        }

        public SelectListViewModel<int> Campaigns { get; private set; }

        [DisplayName("Приемная кампания, по которой осуществляется выгрузка тем сочинений")]
        public int? SelectedCampaign { get; set; }

        [DisplayName("Дата регистрации заявления с")]
        public DateTime? RegistrationDateFrom { get; set; }

        [DisplayName("по")]
        public DateTime? RegistrationDateTo { get; set; }

        [DisplayName("получены результаты сочинений")]
        public bool? HasResults { get; set; }

        [DisplayName("не выгружавшиеся ранее")]
        public bool? NotDownloaded { get; set; }

        public CompositionResultsFilterViewModel CloneInputFields(CompositionResultsFilterViewModel source)
        {
            return new CompositionResultsFilterViewModel
            {
                RegistrationDateFrom = source.RegistrationDateFrom,
                RegistrationDateTo = source.RegistrationDateTo,
                SelectedCampaign = source.SelectedCampaign,
                SelectedCompetitiveGroup = source.SelectedCompetitiveGroup,
                LastName = source.LastName,
                ApplicationNumber = source.ApplicationNumber
            };
        }

        public CompositionResultsFilterViewModel CloneInputFields()
        {
            return CloneInputFields(this);
        }


        public SelectListViewModel<int> CompetitiveGroups { get; private set; }

        [DisplayName("Конкурс")]
        public int? SelectedCompetitiveGroup { get; set; }

        [DisplayName("Фамилия")]
        public string LastName { get; set; }

        [DisplayName("Номер заявления")]
        public string ApplicationNumber { get; set; }
    }
}