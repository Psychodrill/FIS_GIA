using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GVUZ.Web.ViewModels
{
    public class ApplicationExportViewModel
    {
        public IEnumerable<int> YearRange
        {
            get
            {
                for (int startYear = 2014; startYear <= DateTime.Now.Year; startYear++)
                {
                    yield return startYear;
                }
            }
        }

        [Required(ErrorMessage = "Необходимо выбрать год выгрузки")]
        [Display(Name = "Выберите год выгрузки")]
        public int? SelectedYear { get; set; }

        public bool IsExportInProgress { get; set; }

        public bool IsExportComplete { get; set; }

        public DateTime? ExportedFileDate { get; set; }

        public bool IsExportFailed { get; set; }

        [Display(Name = "Формирование выгрузки для ГЗГУ доступно только для пользователей ВУЗов.")]
        public bool IsDenied { get; set; }
    }
}