using System.ComponentModel.DataAnnotations;

namespace GVUZ.Web.ViewModels.ApplicationsList
{
    public class IncludeRecommendedListSubmitViewModel
    {
        [Required]
        public int ApplicationId { get; set; }

        [Required]
        public ApplicationStageSelectionItem[] SelectedItems { get; set; }
    }

    public class ApplicationStageSelectionItem
    {
        public int Id { get; set; }
        public int Stage { get; set; }
    }
}