using System.ComponentModel.DataAnnotations;

namespace GVUZ.Web.ViewModels.Entrants
{
    public class UpdateEntrantUidViewModel
    {
        [Required]
        public int? EntrantId { get; set; }

        public string UID { get; set; }
    }
}