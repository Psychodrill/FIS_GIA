namespace Ege.Hsc.Logic.Models.Servers
{
    using System;
    using System.Collections.Generic;
    using JetBrains.Annotations;

    /// <summary>
    /// Ошибки сервера бланков
    /// </summary>
    public class ServerErrors
    {
        /// <summary>
        /// Дата экзамена
        /// </summary>
        public DateTime ExamDate { get; set; }

        /// <summary>
        /// Успешно ли прочитан файл
        /// </summary>
        public bool FileRead { get; set; }

        /// <summary>
        /// Количество бланков, указанных в файле
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Отсутствующие на сервере бланки
        /// </summary>
        [NotNull]
        public ICollection<string> Missing { get; set; } 

        /// <summary>
        /// Лишние бланки на сервере
        /// </summary>
        [NotNull]
        public ICollection<string> Extra { get; set; }
    }
}
