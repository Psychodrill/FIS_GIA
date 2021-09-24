namespace Ege.Hsc.Logic.Models.Servers
{
    using System;

    /// <summary>
    /// Статус доступности сервера бланков
    /// </summary>
    public class BlankServerStatus
    {
        /// <summary>
        /// Идентификатор региона
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название региона
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// Адрес сервера бланков сочинения
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// Статус доступности
        /// </summary>
        public bool IsAvailable { get; set; }

        /// <summary>
        /// Количество бланков (из файла)
        /// </summary>
        public int ServerCount { get; set; }

        /// <summary>
        /// Количество бланков (из БД)
        /// </summary>
        public int DbCount { get; set; }

        /// <summary>
        /// Дата последней проверки доступности
        /// </summary>
        public DateTimeOffset? LastAvailabilityCheck { get; set; }

        /// <summary>
        /// Дата последнего обновления проверки из файла
        /// </summary>
        public DateTimeOffset? LastFileCheck { get; set; }

        /// <summary>
        /// Найдены ли ошибки при последней проверке
        /// </summary>
        public bool HasErrors { get; set; }
    }
}
