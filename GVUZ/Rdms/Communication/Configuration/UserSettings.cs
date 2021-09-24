using System.Configuration;
using Rdms.Communication.BaseConfigurationService;

namespace Rdms.Communication.Configuration
{
	public class UserSettings : ConfigurationElementDescriber
	{
		/// <summary>
		/// </summary>
		[ConfigurationProperty("PrintWarning", DefaultValue = "1")]
		public string PrintWarning
		{
			get { return (string) this["PrintWarning"]; }
			set { this["PrintWarning"] = value; }
		}


		/// <summary>
		/// </summary>
		[ConfigurationProperty("Login", DefaultValue = "")]
		public string Login
		{
			get
			{
				try
				{
					return (string) this["Login"];
				}
				catch
				{
					return "";
				}
			}
			set { this["Login"] = value; }
		}

		/// <summary>
		/// </summary>
		[ConfigurationProperty("Password", DefaultValue = "")]
		public string Password
		{
			get
			{
				try
				{
					return (string) this["Password"];
				}
				catch
				{
					return "";
				}
			}
			set { this["Password"] = value; }
		}
	}
}