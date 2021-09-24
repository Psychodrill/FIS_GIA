using System;
using System.Reflection;

namespace GVUZ.Web.Helpers
{
	/// <summary>
	/// Хелпер для вывода версии приложения
	/// </summary>
	public static class VersionHelper
	{
		private static string _versionString;
		public static string GetVersion()
		{
			var version = Assembly.GetExecutingAssembly().GetName().Version;
			if (_versionString == null)
			{
				if (version == null) _versionString = "";
				else _versionString = String.Format("v{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Revision, version.Build);
			}

			return _versionString;
		}
	}
}