using System.ComponentModel;

namespace GVUZ.Web.ViewModels.ApplicationsList
{
    public class RecommendedApplicationsRecordViewModel
    {
        public int RecommendedListId { get; set; }
        public int ApplicationId { get; set; }
        public bool IsCampaignFinished { get; set; }
        public int? EducationLevelId { get; set; }
        public int? EducationFormId { get; set; }
        public int? DirectionId { get; set; }

        [DisplayName("Приемная кампания")]
        public string CampaignName { get; set; }

        [DisplayName("Этап")]
        public string StageName { get; set; }

        [DisplayName("№ заявления")]
        public string ApplicationNumber { get; set; }

        [DisplayName("ФИО абитуриента")]
        public string EntrantFullName { get; set; }

        [DisplayName("Уровень")]
        public string EducationLevelName { get; set; }

        [DisplayName("Форма обучения")]
        public string EducationFormName { get; set; }

        [DisplayName("Конкурс")]
        public string CompetitiveGroupName { get; set; }

        [DisplayName("Направление")]
        public string DirectionName { get; set; }

        [DisplayName("Сдал документы")]
        public bool OriginalDocumentsReceived { get; set; }

        [DisplayName("Рейтинг")]
        public string Rating { get; set; }

        public int ApplicationStatusId { get; set; }
        
        public bool DisableIncludeAction
        {
            get { return IsCampaignFinished || ApplicationStatusId == 8; }
        }
    }
}