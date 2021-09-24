namespace Esrp.Web
{
    using System;
    using System.Collections;
    using System.Web;

    /// <summary>
    /// Расширения для класса SiteMapNode 
    /// </summary>
    public static class SiteMapNodeExtensions
    {
        private const string SessionKey = "stored_site_map_node_properties";

        /// <summary>
        /// Сохранение свойств узла карты сайта
        /// </summary>
        /// <param name="node">Узел карты сайта</param>
        /// <param name="properties">Свойства узла</param>
        public static void StoreProperties(this SiteMapNode node, SiteMapNodeProperties properties)
        {
            var propertyList = GetPropertiesFromSession();
			if (node != null)
			{
			    if (propertyList.ContainsKey(node.Url))
			    {
			        propertyList[node.Url] = properties;
			    }
			    else
			    {
			        propertyList.Add(node.Url, properties);
			    }
			}

        	SavePropertiesToSession(propertyList);
        }

        /// <summary>
        /// Получение свойств узла карты сайта
        /// </summary>
        /// <param name="node">Узел карты сайта</param>
        /// <returns>Объект свойств</returns>
        public static SiteMapNodeProperties GetProperties(this SiteMapNode node)
        {
            // TODO: check is it correct fix
			if (node == null)
			{
			    return null;
			}

            var propertyList = GetPropertiesFromSession();
            if ((propertyList.Count == 0) || (!propertyList.ContainsKey(node.Url)))
            {
                return null;
            }

            return (SiteMapNodeProperties)propertyList[node.Url];
        }

        /// <summary>
        /// Получение заголовка страницы
        /// </summary>
        /// <param name="node">Узел карты сайта</param>
        /// <returns>Заголовок страницы</returns>
        /// <remarks>
        /// Если объект свойств узла не найден в сессии, то возвращается title из карты сайта
        /// </remarks>
        public static string GetActualTitle(this SiteMapNode node)
        {
            var properties = GetProperties(node);
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

        // Получение объекта из сессии
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

        public static bool IsIpCheckDisabled(this SiteMapNode node)
        {
            return  (node!=null && node.Roles!=null) && (node.Roles.Contains("CustomAppRule_IpCheckDisabled") || (node.ParentNode!=null && node.ParentNode.IsIpCheckDisabled()));
        }
    }

    /// <summary>
    /// Свойства страницы
    /// </summary>
    /// <remarks>
    /// Заполнаются в случае изменеия заголовка или uri страницы
    /// </remarks>
    [Serializable()]
    public class SiteMapNodeProperties
    {
        private string mUri;
        private string mTitle;

        public string Url
        {
            get { return this.mUri; }
        }

        public string Title
        {
            get { return this.mTitle; }
        }

        public SiteMapNodeProperties(string uri, string title)
        {
            this.mUri = uri;
            this.mTitle = title;
        }
    }
}
