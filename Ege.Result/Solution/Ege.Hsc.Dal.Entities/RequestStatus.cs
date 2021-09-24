namespace Ege.Hsc.Dal.Entities
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Статус запроса на выгрузку бланков
    /// </summary>
    public class RequestStatus
    {
        /// <summary>
        /// Идентификатор запроса
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Комментарий к запросу
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// Состояние обработки запроса
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Дата постановки запроса в очередь
        /// </summary>
        public DateTimeOffset CreateDate { get; set; }

        /// <summary>
        /// Общее количество бланков
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// Количество успешно загруженных бланков
        /// </summary>
        public int Downloaded { get; set; }

        /// <summary>
        /// Количество ошибок загрузки бланков
        /// </summary>
        public int Error { get; set; }

        /// <summary>
        /// Общее количество участников
        /// </summary>
        public int TotalParticipants { get; set; }

        /// <summary>
        /// Количество участников, все бланки которых загружены без ошибок
        /// </summary>
        public int SuccessfullyProcessedParticipants { get; set; }

        /// <summary>
        /// Количество участников, не найденных в БД
        /// </summary>
        public int NotFoundParticipants { get; set; }

        /// <summary>
        /// Количество участников, найденных в БД, все бланки которых обработаны и при обработке произошли ошибки
        /// </summary>
        public int ProcessedWithErrorsParticipants { get; set; }

        /// <summary>
        /// Готов ли архив
        /// </summary>
        public bool IsReady { get; set; }
    }

    public class RequestStatusPage
    {
        public int Count { get; set; }

        public ICollection<RequestStatus> Page { get; set; }
    }
}
