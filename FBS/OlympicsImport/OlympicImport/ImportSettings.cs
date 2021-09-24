using System;
using System.Configuration;

namespace OlympicImport
{
    public static class ImportSettings
    {
        public const string OlympicYearKey = "OlympicYear";
        public const string RemoveOlympicsKey = "RemoveOlympics";
        public const string RemoveDiplomantsKey = "RemoveDiplomants";
        public const string RemoveSubjectsKey = "RemoveSubjects";

        public static int OlympiadYear
        {
            get
            {
                if (ConfigurationManager.AppSettings[OlympicYearKey] != null)
                {
                    int y;
                    if (int.TryParse(ConfigurationManager.AppSettings[OlympicYearKey], out y))
                    {
                        return y;
                    }
                }
                
                return DateTime.Now.Year;
            }
        }

        public static bool RemoveOlympics
        {
            get
            {
                if (ConfigurationManager.AppSettings[RemoveOlympicsKey] != null)
                {
                    bool b;
                    if (bool.TryParse(ConfigurationManager.AppSettings[RemoveOlympicsKey], out b))
                    {
                        return b;
                    }
                }

                return true;
            }
        }

        public static bool RemoveDiplomants
        {
            get
            {
                if (ConfigurationManager.AppSettings[RemoveDiplomantsKey] != null)
                {
                    bool b;
                    if (bool.TryParse(ConfigurationManager.AppSettings[RemoveDiplomantsKey], out b))
                    {
                        return b;
                    }
                }

                return true;
            }
        }

        public static bool RemoveSubjects
        {
            get
            {
                if (ConfigurationManager.AppSettings[RemoveSubjectsKey] != null)
                {
                    bool b;
                    if (bool.TryParse(ConfigurationManager.AppSettings[RemoveSubjectsKey], out b))
                    {
                        return b;
                    }
                }

                return true;
            }
        }
    }
}