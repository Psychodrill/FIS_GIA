using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Script.Serialization;
using GVUZ.Model.Entrants.Documents;
using GVUZ.Web.Models;
using GVUZ.Web.Controllers.Admission;

namespace GVUZ.Web.Portlets.Applications
{
    public class ApplicationInfoViewModelBase
    {
        [DisplayName("Статус")]
        public string Status { get; set; }

        [DisplayName("Тип нарушения")]
        public string Violation { get; set; }

		public bool IsVUZ { get; set; }

        [DisplayName("Образовательное учреждение")]
		public string Institution { get; set; }

        [DisplayName("Направления подготовки")]
        public string Direction { get; set; }

        [DisplayName("Курс")]
        public string Course { get; set; }

        [DisplayName("Уровни образования")]
        public string EducationLevel { get; set; }

        [DisplayName("Формы обучения и источники финансирования")]
        public string EducationalFormList { get; set; }
    }
}