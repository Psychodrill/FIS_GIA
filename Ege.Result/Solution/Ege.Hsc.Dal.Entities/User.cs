namespace Ege.Hsc.Dal.Entities
{
    using System;

    /// <summary>
    ///     Пользователи ЕСРП
    /// </summary>
    public class User
    {
        /// <summary>
        ///     Идентификатор системы выгрузки бланков
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Логин в ЕСРП
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        ///     Тикет пользователя
        /// </summary>
        public Guid? Ticket { get; set; }
    }
}