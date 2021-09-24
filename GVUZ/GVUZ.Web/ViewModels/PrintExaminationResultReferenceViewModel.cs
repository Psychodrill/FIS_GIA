using System.Collections.Generic;

namespace GVUZ.Web.ViewModels
{
    /// <summary>
    /// Модель для печати справки о результатах ЕГЭ
    /// </summary>
    public class PrintExaminationResultReferenceViewModel
    {
        private ICollection<PrintExaminationResultCertificateMark> _marks;

        public string EntrantLastName { get; set; }
        public string EntrantMiddleName { get; set; }
        public string EntrantFistName { get; set; }
        public string DocumentSeries { get; set; }
        public string DocumentNumber { get; set; }
        public string InstitutionFullName { get; set; }
        public ICollection<PrintExaminationResultCertificateMark> Marks
        {
            get { return _marks ?? (_marks = new List<PrintExaminationResultCertificateMark>()); }
            set { _marks = value; }
        }
    }
}