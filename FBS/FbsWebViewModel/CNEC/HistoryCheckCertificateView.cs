namespace FbsWebViewModel.CNEC
{
    using System;

    /// <summary>
    /// Представление, содержащее информацию о итерационной проверке
    /// </summary>
    public class HistoryCheckCertificateView
    {
        /// <summary>
        /// номер св-ва
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Логин
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Дата
        /// </summary>
        public DateTime DateCreate { get; set; }

        /// <summary>
        /// Результат
        /// </summary>
        public Link LinkResult { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// номер сертификата
        /// </summary>
        public string CNENumber { get; set; }

        /// <summary>
        /// фамилия сертифицируемого
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// имя сертифицируемого
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// отчество сертифицируемого
        /// </summary>
        public string PatronymicName { get; set; }

        /// <summary>
        /// средние оценки по предметам (через запятую, в определенном порядке)
        /// </summary>
        public string Marks { get; set; }

        /// <summary>
        /// типографический номер сертификата
        /// </summary>
        public string TypographicNumber { get; set; }

        /// <summary>
        /// серия документа сертифицируемого (паспорта)
        /// </summary>
        public string PassportSeria { get; set; }

        /// <summary>
        /// номер документа сертифицируемого (паспорта)
        /// </summary>
        public string PassportNumber { get; set; }

        /// <summary>
        /// год выдачи сертификата
        /// </summary>
        public int? YearCertificate { get; set; }

        /// <summary>
        /// глобальный ИД
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// ИД участника
        /// </summary>
        public long? ParticipantsId { get; set; }
    }
}