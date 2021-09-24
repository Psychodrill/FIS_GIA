namespace Ege.Hsc.Logic.Blanks
{
    using Ege.Check.Common.Config;
    using JetBrains.Annotations;

    internal class ConfigBlankApplicationSettings : IBlankApplicationSettings
    {
        private const string BlanksBatchsizeConfigName = "Blanks.BatchSize";
        private const string BlanksZipBatchsizeConfigName = "Blanks.ZipBatchSize";
        private const string BlanksStorageRootConfigName = "Blanks.Storage.Root";
        private const string BlanksZipStorageRootConfigName = "Blanks.ZipStorage.Root";

        [NotNull]private readonly IConfigReaderHelper _configReaderHelper;

        public ConfigBlankApplicationSettings([NotNull]IConfigReaderHelper configReaderHelper)
        {
            _configReaderHelper = configReaderHelper;
        }

        public int BatchBlankDownload()
        {
            return _configReaderHelper.GetInt(BlanksBatchsizeConfigName, "", 50);
        }

        public string BlanksRootPath()
        {
            return _configReaderHelper.GetString(BlanksStorageRootConfigName, "");
        }

        public string ZipRootPath()
        {
            return _configReaderHelper.GetString(BlanksZipStorageRootConfigName, "");
        }

        public int BatchBlankRequest()
        {
            return _configReaderHelper.GetInt(BlanksZipBatchsizeConfigName, "", 10);
        }
    }
}