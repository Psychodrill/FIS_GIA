using System.Collections.Generic;
using System.ComponentModel;
using GVUZ.Web.ViewModels.Shared;
using GVUZ.DAL.Dapper.ViewModel.Common;

namespace GVUZ.Web.ViewModels.ApplicationsList
{
    public class IncludeRecommendedListViewModel
    {
        public static readonly IncludeRecommendedListViewModel MetadataInstance = new IncludeRecommendedListViewModel();

        private List<IncludeRecommendedRecordViewModel> _applicationRecords;

        public IncludeRecommendedListViewModel()
        {
            Stages = new SelectListViewModel<int?> {ShowUnselectedText = true, UnselectedText = "Выберите этап"};
            Stages.Add(1, "1 этап");
            Stages.Add(2, "2 этап");
        }

        public IncludeRecommendedRecordViewModel RecordInfo
        {
            get { return IncludeRecommendedRecordViewModel.MetadataInstance; }
        }

        public List<IncludeRecommendedRecordViewModel> ApplicationRecords
        {
            get { return _applicationRecords ?? (_applicationRecords = new List<IncludeRecommendedRecordViewModel>()); }
            set { _applicationRecords = value; }
        }

        public SelectListViewModel<int?> Stages { get; private set; }

        [DisplayName("Этап зачисления")]
        public int? SelectedStage { get; set; }

        [DisplayName("Номер заявления")]
        public string ApplicationNumber { get; set; }

        [DisplayName("ФИО абитуриента")]
        public string EntrantName { get; set; }

        [DisplayName("Документ, удостоверяющий личность")]
        public string IdentityDocument { get; set; }

        public int ApplicationId { get; set; }
    }
}