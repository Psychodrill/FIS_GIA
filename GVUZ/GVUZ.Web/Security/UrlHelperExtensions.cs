using System;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using GVUZ.Helper;

namespace GVUZ.Web.Security
{
	public static class UrlHelperExtensions
	{
		public static MvcHtmlString GenerateNavLink<TController>(this UrlHelper urlHelper,
			Expression<Action<TController>> action, string linkText, Role allowedRoles, string denyRole = null) where TController : Controller
		{
			UserRole userRole = new UserRole(allowedRoles);
			if (userRole.IsUserInRole() && (denyRole == null || !UserRole.CurrentUserInRole(denyRole)))
			{
				string url = urlHelper.Generate(action);
				return new MvcHtmlString(String.Format("<input onclick=\"navigateTo('{0}')\" class=\"button3\" value=\"{1}\" type=\"button\" />",
					url, HttpUtility.HtmlEncode(linkText)));
			}

			return new MvcHtmlString("");
		}
	}
}