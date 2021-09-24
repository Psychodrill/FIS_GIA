namespace FogSoft.Helpers
{
	/// <summary>
	/// Сессия
	/// </summary>
	public interface ISession
	{
		T GetValue<T>(string name, T defaultValue = default(T), bool tryToParse = true);
		void SetValue<T>(string name, T value);
	}
}
