namespace Esrp.Web.ViewModel.Documents
{
    using System;

    /// <summary>
    /// Представление для документа
    /// </summary>
    public class DocumentView
    {
        #region Public Properties

        /// <summary>
        /// Дата создания документа
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Описание документа
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Идентификатор
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Имя документа
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Относительный Url
        /// </summary>
        public string RelativeUrl { get; set; }

        #endregion
    }
}