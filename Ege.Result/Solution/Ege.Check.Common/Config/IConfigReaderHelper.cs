namespace Ege.Check.Common.Config
{
    using System;
    using System.Configuration;
    using JetBrains.Annotations;

    public interface IConfigReaderHelper
    {
        int GetInt([NotNull] string settingName, string settingDescription, int? defaultValue = null);

        [NotNull]
        byte[] GetByteArrayFromBase64([NotNull] string settingName, string settingDescription, int expectedLength);

        [NotNull]
        string GetString(string settingName, string settingDescription);
    }

    public class ConfigReaderHelper : IConfigReaderHelper
    {
        public int GetInt(string settingName, string settingDescription, int? defaultValue = null)
        {
            var setting = ConfigurationManager.AppSettings[settingName];
            int result;
            if (!int.TryParse(setting, out result))
            {
                if (!defaultValue.HasValue)
                {
                    throw new ConfigurationErrorsException(
                        string.Format("В конфигурационном файле отсутствует или не является целым числом настройка {0} ({1})", settingName,
                                      settingDescription));
                }
                result = defaultValue.Value;
            }
            return result;
        }

        public byte[] GetByteArrayFromBase64(string settingName, string settingDescription, int expectedLength)
        {
            try
            {
                var result = Convert.FromBase64String(ConfigurationManager.AppSettings[settingName]);
                if (result.Length != expectedLength)
                {
                    throw new ConfigurationErrorsException(
                        string.Format("Строка в настройке {0} ({1}) имеет неправильную длину", settingName,
                                      settingDescription));
                }
                return result;
            }
            catch (ArgumentNullException ex)
            {
                throw new ConfigurationErrorsException(
                    string.Format("В конфигурационном файле отсутствует настройка {0} ({1})", settingName,
                                  settingDescription),
                    ex);
            }
            catch (FormatException ex)
            {
                throw new ConfigurationErrorsException(
                    string.Format(
                        "Строка в настройке {0} ({1}) в конфигурационном файле не является правильно сформированной base64-строкой", settingName, settingDescription),
                    ex);
            }
        }

        public string GetString(string settingName, string settingDescription)
        {
            var result = ConfigurationManager.AppSettings[settingName];
            if (string.IsNullOrWhiteSpace(result))
            {
                throw new ConfigurationErrorsException(string.Format(
                    "В конфигурационном файле отсутствует настройка {0} ({1})", settingName, settingDescription));
            }
            return result;
        }
    }
}