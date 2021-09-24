namespace FBS.Spec.Configuration
{
    using System.Configuration;

    /// <summary>
    /// Настройки приемочного тестирования
    /// </summary>
    public class SpecConfigurationSection : ConfigurationSection
    {
        #region Constants and Fields

        private const string SectionName = "FBS.Spec";

        #endregion

        /// <summary>
        /// Настройки приемочного тестирования
        /// </summary>
        public static SpecConfigurationSection Configuration
        {
            get
            {
                return ConfigurationManager.GetSection(SectionName) as SpecConfigurationSection;
            }
        }

        /// <summary>
        /// Тестируемые страницы
        /// </summary>
        [ConfigurationProperty("pages")]
        public PageCollection Pages
        {
            get
            {
                return this["pages"] as PageCollection;
            }
        }
    }
}