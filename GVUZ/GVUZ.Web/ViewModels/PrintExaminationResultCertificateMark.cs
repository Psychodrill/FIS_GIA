namespace GVUZ.Web.ViewModels
{
    /// <summary>
    /// Сведения об оценках в справке о результатах ЕГЭ
    /// </summary>
    public class PrintExaminationResultCertificateMark
    {
        public string SubjectName { get; set; }
        public int Mark { get; set; }
        public int Year { get; set; }
        public string Status { get; set; }
        public string CertificateNumber { get; set; }
    }
}