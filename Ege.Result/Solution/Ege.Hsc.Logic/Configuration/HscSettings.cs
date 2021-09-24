namespace Ege.Hsc.Logic.Configuration
{
    using System;
    using System.Configuration;
    using Common.Logging;
    using JetBrains.Annotations;

    internal class HscSettings : IHscSettings
    {
        private const string OpenDateConfigString = "Ege.Hsc.OpenDate";
        private const string CsvUploadAllowedForEsrpConfigString = "Ege.Hsc.CsvUploadAllowedForEsrp";
        [NotNull] private readonly ILog _logger;

        public HscSettings()
        {
            _logger = LogManager.GetLogger(GetType());
        }

        public DateTime OpenDate
        {
            get
            {
                var strDate = ConfigurationManager.AppSettings[OpenDateConfigString];
                DateTime date;
                if (!DateTime.TryParse(strDate, out date))
                {
                    _logger.WarnFormat("Отсутствует определение конфигурации даты открытия сервиса загрузки бланков вузами. Имя параметра {0}",
                                       OpenDateConfigString);
                    return DateTime.MinValue;
                }
                return date;
            }
        }

        public bool CsvUploadAllowedForEsrp
        {
            get
            {
                var strDate = ConfigurationManager.AppSettings[CsvUploadAllowedForEsrpConfigString];
                bool result;
                if (!bool.TryParse(strDate, out result))
                {
                    _logger.WarnFormat("Отсутствует определение конфигурации разрешения массовой выгрузки бланков вузами. Имя параметра {0}",
                                       CsvUploadAllowedForEsrpConfigString);
                    return false;
                }
                return result;
            }
        }
    }
}