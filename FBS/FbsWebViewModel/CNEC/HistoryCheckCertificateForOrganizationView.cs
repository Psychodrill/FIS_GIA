namespace FbsWebViewModel.CNEC
{
    using System;

    /// <summary>
    /// История проверок сертификатов организацией
    /// </summary>
    public class HistoryCheckCertificateForOrganizationView
    {
        /// <summary>
        /// номер св-ва
        /// </summary>
        public string Number { get; set; }
        
        /// <summary>
        ///  Порядковый номер строки
        /// </summary>
        public int NumberRow { get; set; }

        /// <summary>
        /// ИД записи
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Дата и время проверки
        /// </summary>
        public DateTime DateCheck { get; set; }

        /// <summary>
        /// Тип проверки
        /// </summary>
        public string TypeCheck { get; set; }

        /// <summary>
        /// Свидетельство
        /// </summary>
        public Link Certificate { get; set; }

        /// <summary>
        /// Типографический номер
        /// </summary>
        public string TypographicNumber { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public string PatronymicName { get; set; }

        /// <summary>
        /// Документ
        /// </summary>
        public string Document { get; set; }

        /// <summary>
        /// Год
        /// </summary>
        public string Year { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Количество проверок
        /// </summary>
        public string CountCheck { get; set; }

        /// <summary>
        /// Распечатать справку
        /// </summary>
        public Link PrintNote { get; set; }

        /// <summary>
        /// Былла по предметам чрез запятую
        /// </summary>
        public string Marks
        {
            get;
            set;
        }
    }
}