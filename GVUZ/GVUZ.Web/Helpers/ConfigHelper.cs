using System;
using System.Configuration;
using System.Globalization;
using System.Linq;

namespace GVUZ.Web.Helpers
{
    public static class ConfigHelper
    {
        public const string ShowFilterStatisticsKey = "ShowFilterStatistics";
        public const string HideOrdersMenu = "HideOrdersMenu";
        public const string ShowInstitutionReportsKey = "ShowInstitutionReports";
        public const string InstitutionReportsRootCodeKey = "InstitutionReportsRootCode";
        public const string ShowAutoReportsKey = "ShowAutoReports";
        public const string InstitutionAutoRootCodeKey = "InstitutionAutoRootCode";
        private static readonly char[] AclSplit = new[] { ',' };

        public static bool ShowFilterStatistics()
        {
            string raw = ConfigurationManager.AppSettings[ShowFilterStatisticsKey];

            if (!string.IsNullOrEmpty(raw))
            {
                bool show;
                return bool.TryParse(raw, out show) && show;
            }

            return false;
        }
        
		public static bool ShowOlympicFilesUpload() 
        {
			const string key = "OlympicFileAccessRestriction";

			string val = ConfigurationManager.AppSettings[key];

			int currentId = InstitutionHelper.GetInstitutionID();

			if (val == null) 
            {
				return true;
			}

			string[] acl = val.Split(AclSplit,StringSplitOptions.RemoveEmptyEntries);

			return acl.Any(x => x.Equals(currentId.ToString(CultureInfo.InvariantCulture)));
		}

        public static bool HideOrderOfAdmissionMenu()
        {
            string raw = ConfigurationManager.AppSettings[HideOrdersMenu];

            if (!string.IsNullOrEmpty(raw))
            {
                bool hide;
                return bool.TryParse(raw, out hide) && hide;
            }

            return false;
        }

        public static bool ShowInstitutionReports()
        {
            string raw = ConfigurationManager.AppSettings[ShowInstitutionReportsKey];

            if (!string.IsNullOrEmpty(raw))
            {
                bool hide;
                return bool.TryParse(raw, out hide) && hide;
            }

            return false;
        }

        public static string InstitutionReportsRootCode()
        {
            string raw = ConfigurationManager.AppSettings[InstitutionReportsRootCodeKey];

            if (!string.IsNullOrEmpty(raw))
                return raw;

            return null;
        }

        public static bool ShowAutoReports()
        {
            string raw = ConfigurationManager.AppSettings[ShowAutoReportsKey];

            if (!string.IsNullOrEmpty(raw))
            {
                bool hide;
                return bool.TryParse(raw, out hide) && hide;
            }

            return false;
        }

        public static string InstitutionAutoRootCode()
        {
            string raw = ConfigurationManager.AppSettings[InstitutionAutoRootCodeKey];

            if (!string.IsNullOrEmpty(raw))
                return raw;

            return null;
        }

        public static string FisVersion()
        {
            string raw = ConfigurationManager.AppSettings["FisVersion"];

            if (!string.IsNullOrEmpty(raw))
                return raw;

            return "ФИС ГИА и приема";
        }

    }
}