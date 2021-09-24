using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fbs.Core.UICheckLog
{
    public class CheckLogEntry
    {
        /// <summary>
        /// Идентификатор события
        /// </summary>
        public int Id
        {
            get;
            set;
        }

        /// <summary>
        /// Номер сертификата
        /// </summary>
        public string CNENumber
        {
            get;
            set;

        }
        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName
        {
            get;
            set;
        }
        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName
        {
            get;
            set;
        }
        /// <summary>
        /// Отчество
        /// </summary>
        public string PatronymicName
        {
            get;
            set;
        }
        /// <summary>
        /// Баллы по предметам
        /// </summary>
        public string Marks
        {
            get;
            set;
        }

        /// <summary>
        /// Типографиский номер
        /// </summary>
        public string TypographicNumber
        {
            get;
            set;
        }

        /// <summary>
        /// Серия паспорта
        /// </summary>
        public string PassportSeria
        {
            get;
            set;
        }

        /// <summary>
        /// Номер паспорта
        /// </summary>
        public string PassportNumber
        {
            get;
            set;
        }
        /// <summary>
        /// Год
        /// </summary>
        public int Year
        {
            get;
            set;
        }
        /// <summary>
        /// Проверяющий
        /// </summary>
        public string Login
        {
            get;
            set;
        }
        /// <summary>
        /// Дата события
        /// </summary>
        public DateTime EventDate
        {
            get;
            set;
        }



    }
}
