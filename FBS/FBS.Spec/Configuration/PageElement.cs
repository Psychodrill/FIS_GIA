namespace FBS.Spec.Configuration
{
    using System.Configuration;

    /// <summary>
    /// Страница
    /// </summary>
    public class PageElement : ConfigurationElement
    {
        /// <summary>
        /// Название
        /// </summary>
        [ConfigurationProperty("name")]
        public string Name
        {
            get { return this["name"].ToString(); }
            set { this["name"] = value; }
        }

        /// <summary>
        /// Адрес
        /// </summary>
        [ConfigurationProperty("url")]
        public string Url
        {
            get { return this["url"].ToString(); }
            set { this["url"] = value; }
        }
    }
}