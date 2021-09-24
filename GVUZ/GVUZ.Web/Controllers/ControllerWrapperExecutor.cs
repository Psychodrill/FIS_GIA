using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using GVUZ.Helper;

namespace GVUZ.Web.Controllers
{
	public class ControllerWrapperExecutor
	{
		public ControllerWrapperExecutor()
		{
			IsInsidePortlet = true;
		}

		public bool IsInsidePortlet { get; set; }

		private readonly Dictionary<string, object> _customParameters = new Dictionary<string, object>();

		public Dictionary<string, object> CustomParameters
		{
			get { return _customParameters; }
		}

		private void SetCustomParameters(BaseController controller)
		{
			foreach (KeyValuePair<string, object> customParameter in CustomParameters)
			{
				controller.CustomParameters.Add(customParameter.Key, customParameter.Value);
			}
		}

		public string DoJsonControllerAction<TController>(Expression<Func<TController, ActionResult>> controllerMethod, string jsonData) where TController : BaseController, new()
		{
			MethodCallExpression me = (MethodCallExpression)controllerMethod.Body;
			ParameterInfo[] parameterInfos = me.Method.GetParameters();
			if (parameterInfos.Length > 1)
				throw new MissingMethodException("Required a method with 0 or 1 parameters");
			BaseController controller = Activator.CreateInstance(me.Method.DeclaringType) as BaseController;
			if (controller == null)
				throw new ArgumentException("Required controller derived from BaseController");
			SetCustomParameters(controller);
			ControllerWrapper controllerWrapper = new ControllerWrapper(controller);

			controller.ValueProvider = JsonRequestWrapper.GetValueProvider(jsonData);
			ActionResult result;

			if (parameterInfos.Length == 1)
			{
				object model = Activator.CreateInstance(parameterInfos[0].ParameterType);
				MethodInfo methodInfo = controller.GetType().GetMethod("RefreshModel").MakeGenericMethod(model.GetType());
				model = methodInfo.Invoke(controller, new[] { model });
				result = (ActionResult)me.Method.Invoke(controller, new[] { model });
			}
			else
			{
				result = (ActionResult)me.Method.Invoke(controller, new object[] { });
			}

			return controllerWrapper.GetResponseString(result);
		}

		public string DoViewControllerAction<TController>(Expression<Func<TController, ActionResult>> controllerMethod, params object[] controllerData) where TController : BaseController, new()
		{
			MethodCallExpression me = (MethodCallExpression)controllerMethod.Body;
			ParameterInfo[] parameterInfos = me.Method.GetParameters();
			if (parameterInfos.Length != controllerData.Length)
				throw new MissingMethodException("Incorrect number of parameters provided for controller method");
			BaseController controller = Activator.CreateInstance(me.Method.DeclaringType) as BaseController;
			if (controller == null)
				throw new ArgumentException("Required controller derived from BaseController");
			SetCustomParameters(controller);
			ControllerWrapper controllerWrapper = new ControllerWrapper(controller);
			controllerWrapper.Controller.HttpContext.SetInsidePortlet(IsInsidePortlet);
			controllerWrapper.Controller.HttpContext.User = HttpContext.Current.User;
			controllerWrapper.CopyContextItems();
			ActionResult result = (ActionResult)me.Method.Invoke(controller, controllerData);
			return controllerWrapper.GetResponseString(result);
		}
	}
}