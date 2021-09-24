namespace Ege.Check.Logic.Models.Cache
{
    using System;

    public class AppealCacheModel
    {
        /// <summary>
        ///     Дата апелляции
        /// </summary>
        public DateTimeOffset Date { get; set; }

        /// <summary>
        ///     Статус апелляции
        /// </summary>
        public int Status { get; set; }
    }
}