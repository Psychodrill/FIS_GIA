namespace FBS.Spec
{
    using System.Configuration;

    using FBS.Spec.Configuration;

    /// <summary>
    /// Список страниц системы
    /// </summary>
    public class PageList
    {
        #region Constants and Fields

        public static readonly string RootUrl = ConfigurationManager.AppSettings["RootUrl"];

        private static readonly PageList Instance = new PageList();

        #endregion

        #region Public Properties

        /// <summary>
        /// Singletone списка страниц
        /// </summary>
        public static PageList Current
        {
            get
            {
                return Instance;
            }
        }

        #endregion

        #region Public Indexers

        /// <summary>
        /// Получение адреса страницы по названию
        /// </summary>
        /// <param name="page">Название страницы</param>
        /// <returns>Адрес страницы</returns>
        public string this[string page]
        {
            get
            {
                if (page.Equals("Главная"))
                {
                    return RootUrl;
                }

                var relUrl = SpecConfigurationSection.Configuration.Pages[page].Url;
                return string.Format("{0}/{1}", RootUrl, relUrl);
            }
        }

        #endregion
    }
}