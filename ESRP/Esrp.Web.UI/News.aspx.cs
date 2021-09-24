namespace Esrp.Web
{
    using System;

    /// <summary>
    /// Страница отображающая новость по ее идентификатору
    /// </summary>
    public partial class News : BasePage
    {
        #region Constants and Fields

        private const string ErrorDocumentNotFound = "Новость не найдена";

        private const string NewsQueryKey = "id";

        private Core.News mCurrentNews;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets CurrentNews.
        /// </summary>
        /// <exception cref="NullReferenceException">
        /// </exception>
        public Core.News CurrentNews
        {
            get
            {
                if (this.mCurrentNews == null)
                {
                    if ((this.mCurrentNews = Core.News.GetNews(this.NewsId)) == null || !this.mCurrentNews.IsActive)
                    {
                        throw new NullReferenceException(ErrorDocumentNotFound);
                    }
                }
                return this.mCurrentNews;
            }
        }

        #endregion

        #region Properties

        private long NewsId
        {
            get
            {
                long result;
                if (!long.TryParse(this.Request.QueryString[NewsQueryKey], out result))
                {
                    throw new NullReferenceException(ErrorDocumentNotFound);
                }

                return result;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Обработчик события "Загрузка страницы"
        /// </summary>
        /// <param name="sender">
        /// Источник события
        /// </param>
        /// <param name="e">
        /// Параметры события
        /// </param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // Установлю заголовок страницы
            this.PageTitle = string.Format("{0} {1}", this.CurrentNews.Date.ToShortDateString(), this.CurrentNews.Name);
            if (!IsPostBack && this.Request.UrlReferrer != null)
            {
                this.BackLink.HRef = this.Request.UrlReferrer.ToString();
            }
        }

        #endregion
    }
}