using System;
using System.Diagnostics;
using System.Web;

namespace FogSoft.Helpers
{
	[DebuggerStepThrough]
	public class HttpSession : ISession
	{
		public T GetValue<T>(string name, T defaultValue = default(T), bool tryToParse = true)
		{
			object value = HttpContext.Current.Session[name];
			if (value == null)
				return defaultValue;
			if (value.GetType() == typeof(T))
				return (T) value;
			if (!tryToParse)
				throw new ArgumentException("Invalid session value type.");
			return value.To<T>();
		}

		public void SetValue<T>(string name, T value)
		{
			HttpContext.Current.Session[name] = value;
		}
	}
}
