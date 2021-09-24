using System;
using System.Web.UI;

namespace Esrp.Web
{
	public static class PageExtensions
	{
		public static string Required(this Page page)
		{
			return String.Format("<span class=\"required\">(*)</span>");
		}		
	}
}