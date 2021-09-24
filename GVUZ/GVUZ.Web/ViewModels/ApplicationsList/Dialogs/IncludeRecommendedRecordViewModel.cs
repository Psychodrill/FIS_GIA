using System.ComponentModel;

namespace GVUZ.Web.ViewModels.ApplicationsList
{
    public class IncludeRecommendedRecordViewModel
    {
        public static readonly IncludeRecommendedRecordViewModel MetadataInstance = new IncludeRecommendedRecordViewModel();

        [DisplayName("Конкурс")]
        public string CompetitiveGroupName { get; set; }

        [DisplayName("Направление подготовки")]
        public string DirectionName { get; set; }

        [DisplayName("Уровень образования")]
        public string EducationLevelName { get; set; }

        [DisplayName("Форма обучения")]
        public string EducationFormName { get; set; }

        [DisplayName("Источник финансирования")]
        public string EducationSourceName { get; set; }

        [DisplayName("Приоритет")]
        public int? Priority { get; set; }

        public int ApplicationCompetitiveGroupItemId { get; set; }
    }
}