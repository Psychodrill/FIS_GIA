namespace Ege.Check.Logic.LoadServices.Preprocessing
{
    using System.Configuration;
    using Ege.Check.Logic.Models.Services;

    internal class ConfigBatchSizeSettingsReader : IBatchSizeSettingsReader
    {
        private const int Default = 5000;

        public int Read(ServiceDto dto)
        {
            var settingsName = string.Format("LoadServices.Settings.MaxLoad.{0}", dto);
            var setting = ConfigurationManager.AppSettings[settingsName];
            int result;
            return (string.IsNullOrWhiteSpace(setting) || !int.TryParse(setting, out result))
                       ? Default
                       : result;
        }
    }
}