namespace Ege.Hsc.Logic.Models.Servers
{
    using System;

    /// <summary>
    /// Расхождение в данных БД и сервера бланков
    /// </summary>
    public class BlankServerError
    {
        /// <summary>
        /// GUID участника / null, если информации нет в БД
        /// </summary>
        public Guid? RbdId { get; set; }

        /// <summary>
        /// Хэш бланка
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Дата экзамена
        /// </summary>
        public DateTime ExamDate { get; set; }

        /// <summary>
        /// Причина: «Результат отсутствует в БД», «Бланки отсутствуют на сервере РЦОИ»
        /// </summary>
        public BlankServerErrorType Error { get; set; }
    }
}
