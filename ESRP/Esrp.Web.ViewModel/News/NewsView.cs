namespace Esrp.Web.ViewModel.News
{
    using System;

    /// <summary>
    /// Представление для одной новости
    /// </summary>
    public class NewsView
    {
        /// <summary>
        /// Дата создания новости
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Имя новости
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание новости
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Идентификатор
        /// </summary>
        public long Id { get; set; }
    }
}