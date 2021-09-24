using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Web;

namespace FbsUtility
{
	/// <summary>
	/// Предастовляет информацию по разного типа объектам.
	/// </summary>
	public static class Informer
	{
		#region Class Methods

		public static string GetExceptionInfo(Exception ex)
		{
			if (ex == null)
			{
				throw new ArgumentNullException("ex");
			}

			StringBuilder result = new StringBuilder();
			List<string> propertiesName = new List<string>();
			Type exType = ex.GetType();
			PropertyInfo[] properties = exType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

			foreach (PropertyInfo info in properties)
				propertiesName.Add(info.Name);

			propertiesName.Remove("InnerException");
			propertiesName.Remove("StackTrace");
			propertiesName.Sort();

			foreach (string name in propertiesName)
			{
				PropertyInfo property = exType.GetProperty(name, BindingFlags.Public | BindingFlags.Instance);
				object value = property.GetValue(ex, null);
				result.AppendFormat("<b>{0}</b>: {1}", property.Name, HttpUtility.HtmlEncode(value == null ? string.Empty : value.ToString()));
				result.Append("</br>");
			}

			if (ex.StackTrace != null)
			{
				result.AppendFormat("<b>{0}</b>: {1}", "StackTrace", ex.StackTrace.Replace(" at ", "</br>"));
			}

			if (ex.InnerException != null)
			{
				result.AppendFormat("</br></br><b><i>Inner Exception</i></b></br>{0}", GetExceptionInfo(ex.InnerException));
			}

			return result.ToString();
		}

		#endregion
	}
}