namespace FbsWebViewModel.CNEC
{
    /// <summary>
    /// Представление, содержащее информацию о сертификате
    /// </summary>
    public class HistoryCertificateView
    {
        /// <summary>
        /// Ссылка на сертификат
        /// </summary>
        public Link Certificate { get; set; }

        /// <summary>
        /// Год
        /// </summary>
        public string Year { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Оценки
        /// </summary>
        public string Marks { get; set; }
    }
}