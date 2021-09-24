using System.ComponentModel.DataAnnotations;
using System.Web;

namespace GVUZ.Web.ViewModels.OlympicFiles
{
    public class OlympicFileUploadViewModel
    {
        [Display(Name = "Комментарий")]
        public string Comments { get; set; }

        [Display(Name = "Файл")]
        [Required]
        public HttpPostedFileBase UploadFile { get; set; }
    }
}