using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;

namespace FogSoft.Web
{
	public static class UrlHelperExtensions
	{
		/// <summary>
		/// Построение url из переданного expression
		/// </summary>
		public static string BuildUrlFromExpression<TController>(RequestContext context, RouteCollection routeCollection, Expression<Action<TController>> action) where TController : Controller
		{
			RouteValueDictionary routeValues = RouteHelper.GetRouteValuesFromExpression(action);
			VirtualPathData vpd = routeCollection.GetVirtualPath(context, routeValues);
			return (vpd == null) ? null : vpd.VirtualPath;
		}

		/// <summary>
		/// Генерация URL по переданному экспрешшену
		/// </summary>
		public static string Generate<TController>(this UrlHelper url, Expression<Action<TController>> action) where TController : Controller
		{
			return url != null ? BuildUrlFromExpression(url.RequestContext, url.RouteCollection, action) : null;
		}
	}
}
