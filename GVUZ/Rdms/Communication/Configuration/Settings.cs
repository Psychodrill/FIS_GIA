using System;
using System.Configuration;

namespace Rdms.Communication.SettingsAdapter
{
	public enum ConfigType
	{
		AppConfig,
		UserConfig,
		WebConfig
	} ;

	public class Settings
	{
		#region Singlton

		/// <summary>
		/// 	Вид конфигурации
		/// </summary>
		private ConfigType _configType;

		/// <summary>
		/// 	Экземпляр класса
		/// </summary>
		private static Settings _settings;

		private Settings()
		{
		}

		public static Settings AppSettings
		{
			get
			{
				if (_settings == null)
				{
					_settings = new Settings();
				}

				_settings._configType = ConfigType.AppConfig;

				return _settings;
			}
		}

		public static Settings UserSettings
		{
			get
			{
				if (_settings == null)
				{
					_settings = new Settings();
				}

				_settings._configType = ConfigType.UserConfig;

				return _settings;
			}
		}

		public static Settings WebSettings
		{
			get
			{
				if (_settings == null)
				{
					_settings = new Settings();
				}

				_settings._configType = ConfigType.WebConfig;

				return _settings;
			}
		}

		#endregion

		/// <summary>
		/// 	Добавление новой секции в конфигурационный файл
		/// </summary>
		public void AddSection(string sectionName, ConfigurationSection configurationSection)
		{
			configurationSection.SectionInformation.AllowExeDefinition =
				ConfigurationAllowExeDefinition.MachineToLocalUser;

			System.Configuration.Configuration config = null;

			switch (_configType)
			{
				case ConfigType.AppConfig:
					config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
					break;
				case ConfigType.UserConfig:
					config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
					break;
				case ConfigType.WebConfig:
					//config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("/WebClient");
					break;
			}

			if (config.Sections[sectionName] == null)
			{
				config.Sections.Add(sectionName, configurationSection);
			}

			config.Save(ConfigurationSaveMode.Modified);
		}

		///// <summary>
		///// Добавление новой секции в конфигурационный файл
		///// </summary>
		//public void AddSection(string groupName, string sectionName, ConfigurationSection configurationSection)
		//{
		//    configurationSection.SectionInformation.AllowExeDefinition =
		//        ConfigurationAllowExeDefinition.MachineToLocalUser;

		//    if (_config.SectionGroups[groupName] == null)
		//    {
		//        this.AddGroupSection(groupName).Sections.Add(sectionName, configurationSection);
		//        this.SaveConfig();
		//    }
		//    else
		//    {
		//        ConfigurationSectionGroup configurationSectionGroup = _config.SectionGroups[groupName];
		//        if (configurationSectionGroup.Sections[sectionName] == null)
		//        {
		//            configurationSectionGroup.Sections.Add(sectionName, configurationSection);
		//            this.SaveConfig();
		//        }
		//    }
		//}

		private System.Configuration.Configuration configuration;

		/// <summary>
		/// 	Получаем секцию из конфигурационного файла
		/// </summary>
		public ConfigurationSection GetSection(string sectionName)
		{
			configuration = null;

			switch (_configType)
			{
				case ConfigType.AppConfig:
					configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
					break;
				case ConfigType.UserConfig:
					configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
					break;
				case ConfigType.WebConfig:
					//configuration = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("/WebClient");
					break;
			}

			ConfigurationSection section = null;

			if (configuration.Sections[sectionName] != null)
			{
				section = configuration.Sections[sectionName];
			}

			return section;
		}

		///// <summary>
		///// Получаем секцию из конфигурационного файла
		///// </summary>
		//public ConfigurationSection GetSection(string groupName, string sectionName)
		//{
		//    if (_config.SectionGroups[groupName] == null)
		//    {
		//        return null;
		//    }
		//    else
		//    {
		//        if (_config.SectionGroups[groupName].Sections[sectionName] == null)
		//        {
		//            return null;
		//        }
		//        else return _config.SectionGroups[groupName].Sections[sectionName];
		//    }
		//}

		/// <summary>
		/// 	Получить значени настройки
		/// </summary>
		/// <param name = "value"></param>
		public bool Set(string settingName, string value)
		{
			try
			{
				System.Configuration.Configuration config = null;

				switch (_configType)
				{
					case ConfigType.AppConfig:
						config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
						break;
					case ConfigType.UserConfig:
						return false;
						//break;
					case ConfigType.WebConfig:
						//config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("/WebClient");
						break;
				}

				if (config.AppSettings.Settings[settingName] != null)
				{
					config.AppSettings.Settings[settingName].Value = value;
				}

				config.Save(ConfigurationSaveMode.Modified);

				return true;
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// 	Установить значение настройки
		/// </summary>
		/// <returns></returns>
		public string Get(string settingName)
		{
			try
			{
				System.Configuration.Configuration config = null;

				switch (_configType)
				{
					case ConfigType.AppConfig:
						config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
						break;
					case ConfigType.UserConfig:
						return String.Empty;
						//break;
					case ConfigType.WebConfig:
						//config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("/WebClient");
						break;
				}

				return config.AppSettings.Settings[settingName].Value;
			}
			catch
			{
				return String.Empty;
			}
		}

		public void Save()
		{
			configuration.Save(ConfigurationSaveMode.Modified);
			configuration = null;
		}
	}
}