namespace FBS.Spec.Configuration
{
    using System.Configuration;
    using System.Linq;

    /// <summary>
    /// Набор страниц
    /// </summary>
    [ConfigurationCollection(typeof(PageElement))]
    public class PageCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// Gets or sets a property, attribute, or child element of this configuration element.
        /// </summary>
        /// <param name="prop">Key</param>
        /// <returns>The specified property, attribute, or child element</returns>
        /// <exception cref="T:System.Configuration.ConfigurationErrorsException">
        /// <paramref name="prop"/> is read-only or locked.</exception>
        public new PageElement this[string prop]
        {
            get
            {
                return this.Cast<PageElement>().FirstOrDefault(element => element.Name == prop);
            }
        }

        /// <summary>
        ///   When overridden in a derived class, creates a new <see cref = "T:System.Configuration.ConfigurationElement" />.
        /// </summary>
        /// <returns>
        ///   A new <see cref = "T:System.Configuration.ConfigurationElement" />.
        /// </returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new PageElement();
        }

        /// <summary>
        ///   Gets the element key for a specified configuration element when overridden in a derived class.
        /// </summary>
        /// <returns>
        ///   An <see cref = "T:System.Object" /> that acts as the key for the specified <see cref = "T:System.Configuration.ConfigurationElement" />.
        /// </returns>
        /// <param name = "element">The <see cref = "T:System.Configuration.ConfigurationElement" /> to return the key for. 
        /// </param>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((PageElement)element).Name;
        }
    }
}