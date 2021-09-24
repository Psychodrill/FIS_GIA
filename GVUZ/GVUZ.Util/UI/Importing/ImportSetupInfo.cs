namespace GVUZ.Util.UI.Importing
{
    public class ImportSetupInfo
    {
        public ImportSetupInfo()
        {
            Properties.Settings.Default.Reload();
            ConnectionString = Properties.Settings.Default.parseConnectionString;
        }

        public string ConnectionString { get; set; }

    }
}