using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;

namespace FogSoft.Web
{
	public static class RouteHelper
	{
		public static RouteValueDictionary GetRouteValuesFromExpression<TController>(Expression<Action<TController>> action)
			where TController : Controller
		{
			if (action == null)
				throw new ArgumentNullException("action");

			var call = action.Body as MethodCallExpression;
			if (call == null)
				throw new ArgumentException(Errors.RouteHelper_Action_must_be_method_call, "action");

			string controllerName = typeof(TController).Name;
			if (!controllerName.EndsWith("Controller", StringComparison.OrdinalIgnoreCase))
				throw new ArgumentException(Errors.RouteHelper_Target_must_end_in_controller, "action");

			controllerName = controllerName.Substring(0, controllerName.Length - "Controller".Length);
			if (controllerName.Length == 0)
				throw new ArgumentException(Errors.RouteHelper_Cannot_route_to_controller, "action");

			var routeValueDictionary = new RouteValueDictionary { { "Controller", controllerName }, { "Action", call.Method.Name } };
			AddParameterValuesFromExpressionToDictionary(routeValueDictionary, call);
			return routeValueDictionary;
		}

		private static void AddParameterValuesFromExpressionToDictionary(RouteValueDictionary routeValueDictionary, MethodCallExpression call)
		{
			ParameterInfo[] parameters = call.Method.GetParameters();

			if (parameters.Length > 0)
			{
				for (int i = 0; i < parameters.Length; i++)
				{
					Expression arg = call.Arguments[i];
					object value;
					var constantExpression = arg as ConstantExpression;
					if (constantExpression != null)
					{
						value = constantExpression.Value;
					}
					else
					{
						Expression<Func<object>> lambdaExpression =
							Expression.Lambda<Func<object>>(Expression.Convert(arg, typeof(object)));
						Func<object> func = lambdaExpression.Compile();
						value = func();
					}

					routeValueDictionary.Add(parameters[i].Name, value);
				}
			}
		}
	}
}
