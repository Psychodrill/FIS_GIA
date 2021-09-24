namespace FogSoft.WSRP
{
	public interface IConfigurationService
	{
		string GetLocale();
	}

	public class ConfigurationService : IConfigurationService
	{
		public string GetLocale()
		{
			return "ru";
		}
	}
}