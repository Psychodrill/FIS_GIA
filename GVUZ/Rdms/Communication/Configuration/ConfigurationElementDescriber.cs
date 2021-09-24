using System.Configuration;

namespace Rdms.Communication.BaseConfigurationService
{
	public abstract class ConfigurationElementDescriber : ConfigurationElement
	{
		[ConfigurationProperty("id", IsRequired = true, IsKey = true)]
		public short ID
		{
			get { return (short) this["id"]; }
			set { this["id"] = value; }
		}
	}
}