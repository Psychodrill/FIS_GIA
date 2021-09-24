namespace Fbs.Web
{
    using System;
    using System.Collections;
    using System.Web;

    /// <summary>
    /// Расширения для класса SiteMapNode 
    /// </summary>
    public static class SiteMapNodeExtensions
    {
        #region Constants and Fields

        private const string SessionKey = "stored_site_map_node_properties";

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Получение заголовка страницы
        /// </summary>
        /// <param name="node">
        /// Узел карты сайта
        /// </param>
        /// <returns>
        /// Заголовок страницы
        /// </returns>
        /// <remarks>
        /// Если объект свойств узла не найден в сессии, то возвращается title из карты сайта
        /// </remarks>
        public static string GetActualTitle(this SiteMapNode node)
        {
            SiteMapNodeProperties properties = GetProperties(node);
            if (properties == null)
            {
                if (node != null)
                {
                    return node.Title;
                }

                return string.Empty;
            }

            return properties.Title;
        }

        /// <summary>
        /// The get actual url.
        /// </summary>
        /// <param name="node">
        /// The node.
        /// </param>
        /// <returns>
        /// The get actual url.
        /// </returns>
        public static string GetActualUrl(this SiteMapNode node)
        {
            SiteMapNodeProperties properties = GetProperties(node);
            if (properties == null)
            {
                if (node != null)
                {
                    return node.Url;
                }

                return string.Empty;
            }

            return properties.Url;
        }

        /// <summary>
        /// Получение свойств узла карты сайта
        /// </summary>
        /// <param name="node">
        /// Узел карты сайта
        /// </param>
        /// <returns>
        /// Объект свойств
        /// </returns>
        public static SiteMapNodeProperties GetProperties(this SiteMapNode node)
        {
            Hashtable propertyList = GetPropertiesFromSession();
            if ((propertyList.Count == 0) || (!propertyList.ContainsKey(node.Url)))
            {
                return null;
            }

            return (SiteMapNodeProperties)propertyList[node.Url];
        }

        /// <summary>
        /// Метод, который скрывает узел в левом меню
        /// </summary>
        /// <param name="node">
        /// Узел
        /// </param>
        public static void HideNodeInLeftMenu(this SiteMapNode node)
        {
            if (node == null)
            {
                return;
            }

            // node["showinleftmenu"] = "false";
        }

        /// <summary>
        /// нужно ли показывать узел в левом меню
        /// </summary>
        /// <param name="node">
        /// узел
        /// </param>
        /// <returns>
        /// нужно\не нужно
        /// </returns>
        public static bool ShowInLeftMenu(this SiteMapNode node)
        {
            if (node == null)
            {
                return false;
            }

            string showAttr = node["showinleftmenu"];
            if (string.IsNullOrEmpty(showAttr) || showAttr.ToLower().Trim() == "true")
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Нужно ли показывать узел в закрытой версии приложения
        /// </summary>
        /// <param name="node">Узел</param>
        /// <returns>нужно / не нужно</returns>
        public static bool ShowInClosedFbs(this SiteMapNode node)
        {
            if (node == null)
            {
                return false;
            }

            var showAttr = node["showinclosedfbs"];
            return string.IsNullOrEmpty(showAttr) || showAttr.ToLower().Trim() == "true";
        }

        /// <summary>
        /// Нужно ли показывать узел в открытой версии приложения
        /// </summary>
        /// <param name="node">Узел</param>
        /// <returns>нужно / не нужно</returns>
        public static bool ShowInOpenedFbs(this SiteMapNode node)
        {
            if (node == null)
            {
                return false;
            }

            var showAttr = node["showinopenedfbs"];
            return string.IsNullOrEmpty(showAttr) || showAttr.ToLower().Trim() == "true";
        }

        /// <summary>
        /// Сохранение свойств узла карты сайта
        /// </summary>
        /// <param name="node">
        /// Узел карты сайта
        /// </param>
        /// <param name="properties">
        /// Свойства узла
        /// </param>
        public static void StoreProperties(this SiteMapNode node, SiteMapNodeProperties properties)
        {
            Hashtable propertyList = GetPropertiesFromSession();

            if (propertyList.ContainsKey(node.Url))
            {
                propertyList[node.Url] = properties;
            }
            else
            {
                propertyList.Add(node.Url, properties);
            }

            SavePropertiesToSession(propertyList);
        }

        #endregion

        // Получение объекта из сессии
        #region Methods

        private static Hashtable GetPropertiesFromSession()
        {
            if (HttpContext.Current.Session[SessionKey] == null)
            {
                return new Hashtable();
            }

            return (Hashtable)HttpContext.Current.Session[SessionKey];
        }

        // Сохранение объекта в сесии
        private static void SavePropertiesToSession(Hashtable properties)
        {
            HttpContext.Current.Session[SessionKey] = properties;
        }

        #endregion
    }

    /// <summary>
    /// Свойства страницы
    /// </summary>
    /// <remarks>
    /// Заполнаются в случае изменеия заголовка или uri страницы
    /// </remarks>
    [Serializable]
    public class SiteMapNodeProperties
    {
        #region Constants and Fields

        private readonly string mTitle;

        private readonly string mUri;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SiteMapNodeProperties"/> class.
        /// </summary>
        /// <param name="uri">
        /// The uri.
        /// </param>
        /// <param name="title">
        /// The title.
        /// </param>
        public SiteMapNodeProperties(string uri, string title)
        {
            this.mUri = uri;
            this.mTitle = title;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets Title.
        /// </summary>
        public string Title
        {
            get
            {
                return this.mTitle;
            }
        }

        /// <summary>
        /// Gets Url.
        /// </summary>
        public string Url
        {
            get
            {
                return this.mUri;
            }
        }

        #endregion
    }
}