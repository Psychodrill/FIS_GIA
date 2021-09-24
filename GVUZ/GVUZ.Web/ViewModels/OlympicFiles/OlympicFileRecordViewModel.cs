using System;
using System.ComponentModel.DataAnnotations;

namespace GVUZ.Web.ViewModels.OlympicFiles
{
    public class OlympicFileRecordViewModel
    {
        public string Id { get; set; }
        private const string UploadDateFormat = "dd.MM.yyyy HH:mm:ss";

        [Display(Name = "Имя файла")]
        public string FileName { get; set; }

        [Display(Name = "Комментарий")]
        public string Comments { get; set; }

        [Display(Name = "Дата и время загрузки")]
        public string UploadDateText
        {
            get { return UploadDate.ToString(UploadDateFormat); }
        }
        
        public DateTime UploadDate { get; set; }
    }
}