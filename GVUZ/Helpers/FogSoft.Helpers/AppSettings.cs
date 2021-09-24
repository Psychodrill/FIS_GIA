using System;
using System.Configuration;
using System.Diagnostics;

namespace FogSoft.Helpers
{
	/// <summary>
	/// Realizes simple method to get app settings.</summary>
	[DebuggerStepThrough]
	public static class AppSettings
	{
		public static T Get<T>(string name, T defaultValue, IFormatProvider formatProvider = null)
		{
			string value = ConfigurationManager.AppSettings.Get(name);
			return string.IsNullOrEmpty(value) ? defaultValue : ParseHelper.ConvertTo(value, defaultValue, formatProvider);
		}
	}
}
