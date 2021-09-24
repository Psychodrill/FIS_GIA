using Rdms.Communication.BaseConfigurationService;
using Rdms.Communication.Configuration;
using Rdms.Communication.SettingsAdapter;

namespace Rdms.Communication
{
	public class LoginPassConfiguration
	{
		/// <summary>
		/// 	Сохраняем логин и пароль в конфиг
		/// </summary>
		/// <param name = "login"></param>
		/// <param name = "password"></param>
		public static void SaveLoginPass(string login, string password)
		{
			// получаем нужную секцию

			ConfigurationSectionDescriber<UserSettings> userSettingsSection =
				(ConfigurationSectionDescriber<UserSettings>) Settings.UserSettings.GetSection("customSection");


			// если ее нет-создаем

			if (userSettingsSection == null)
			{
				userSettingsSection = new ConfigurationSectionDescriber<UserSettings>();


				UserSettings userSettings = new UserSettings();


				userSettingsSection.Items.Add(userSettings);


				Settings.UserSettings.AddSection("customSection", userSettingsSection);
			}


			//

			userSettingsSection = (ConfigurationSectionDescriber<UserSettings>) Settings.UserSettings.GetSection("customSection");


			// присваиваем значение           

			((UserSettings) userSettingsSection.Items[0]).Login = login;
			((UserSettings) userSettingsSection.Items[0]).Password = password;


			// сохраняем

			Settings.UserSettings.Save();
		}

		/// <summary>
		/// 	Загружаем логин и пароль из конфига
		/// </summary>
		/// <param name = "login"></param>
		/// <param name = "password"></param>
		public static void GetLoginPass(out string login, out string password)
		{
			// берем нужную секцию

			ConfigurationSectionDescriber<UserSettings> userSettingsSection =
				(ConfigurationSectionDescriber<UserSettings>) Settings.UserSettings.GetSection("customSection");


			//если она не создана или ее нет в природе – создаем ее

			if (userSettingsSection == null)
			{
				userSettingsSection = new ConfigurationSectionDescriber<UserSettings>();


				UserSettings userSettings = new UserSettings();


				userSettingsSection.Items.Add(userSettings);


				Settings.UserSettings.AddSection("customSection", userSettingsSection);
			}


			userSettingsSection =
				(ConfigurationSectionDescriber<UserSettings>) Settings.UserSettings.GetSection("customSection");


			// пример получения данных

			login = ((UserSettings) userSettingsSection.Items[0]).Login;
			password = ((UserSettings) userSettingsSection.Items[0]).Password;
		}
	}
}