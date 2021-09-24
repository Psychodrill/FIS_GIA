namespace Ege.Check.Logic.Models.Staff
{
    using System.Collections.Generic;

    public class CancelledExamsPage
    {
        /// <summary>
        ///     Количество отменённых экзаменов
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        ///     Запрошенная страница
        /// </summary>
        public ICollection<CancelledExam> Page { get; set; }
    }
}