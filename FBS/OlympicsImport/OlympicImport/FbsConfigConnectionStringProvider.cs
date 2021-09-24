using System.Configuration;
using OlympicImport.Services;

namespace OlympicImport
{
    public class FbsConfigConnectionStringProvider : IConnectionStringProvider
    {
        public string ConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["FbsConnectionString"].ConnectionString; }
        }
    }
}