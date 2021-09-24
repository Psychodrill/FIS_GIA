using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using FogSoft.WSRP;
using GVUZ.Helper;
using GVUZ.Web.Portlets;

namespace GVUZ.Web.Controllers
{
	public class ControllerWrapper
	{
		// TODO: move class outside this project
		private readonly StringBuilder _builder;
		private readonly HttpContext _fakeHttpContext;
		private readonly StringWriter _memoryWriter;

		public ControllerWrapper(Controller controller = null, HttpRequest request = null, RouteData routeData = null)
		{
			if (controller == null)
				controller = new FakeController();
			Controller = controller;

			_builder = new StringBuilder();
			_memoryWriter = new StringWriter(_builder);
			var fakeResponse = new HttpResponse(_memoryWriter);
			var fakeRequest = request ??
				(HttpContext.Current != null ? HttpContext.Current.Request : new HttpRequest(null, "http://uri.com", null));
			_fakeHttpContext = new HttpContext(fakeRequest, fakeResponse);

			Controller.ControllerContext = new ControllerContext
			                               	{
			                               		Controller = controller,
			                               		HttpContext = new PortletHttpContextWrapper(_fakeHttpContext)
			                               	};

			if (routeData == null)
			{
				routeData = new RouteData(
					new Route("{controller}/{action}/{id}", new MvcRouteHandler()),
					new MvcRouteHandler());
				routeData.Values.Add("controller", "FakeController");
				routeData.Values.Add("action", "");
				Controller.ControllerContext.RouteData = routeData;
			}
		}

		public Controller Controller { get; private set; }

		public string RenderView(string viewName, object viewData, bool insidePortlet = true)
		{
			var stringWriter = new StringWriter();

			HttpContext oldContext = HttpContext.Current;
			HttpContext.Current = _fakeHttpContext;
			// копируем Context.Items (включая сессию)
			foreach (DictionaryEntry entry in oldContext.Items)
				_fakeHttpContext.Items[entry.Key] = entry.Value;

			try
			{
				var html = new HtmlHelper(new ViewContext(Controller.ControllerContext, new FakeView(), new ViewDataDictionary(),
				                          	 new TempDataDictionary(), stringWriter), new ViewPage());
				html.ViewContext.HttpContext.SetInsidePortlet(insidePortlet);
				html.RenderPartial(viewName, viewData);
			}
			finally
			{
				HttpContext.Current = oldContext;
				foreach (DictionaryEntry entry in _fakeHttpContext.Items)
					oldContext.Items[entry.Key] = entry.Value;
			}

			_memoryWriter.Flush();
			string result = stringWriter.ToString();
			_builder.Clear();
			return result;
		}

		public void CopyContextItems()
		{
			HttpContext oldContext = HttpContext.Current;
			// копируем Context.Items (включая сессию)
			foreach (DictionaryEntry entry in oldContext.Items)
				_fakeHttpContext.Items[entry.Key] = entry.Value;
		}

		public string GetResponseString(ActionResult result)
		{
			result.ExecuteResult(Controller.ControllerContext);
			return Controller.Response.Output.ToString();
		}

		public static string DoFileReceive<TController>(UploadContext[] uploadContexts) where TController : BaseController, new()
		{
			BaseController controller = (BaseController)Activator.CreateInstance(typeof(TController));
			ControllerWrapper controllerWrapper = new ControllerWrapper(controller);
			ActionResult jsonResult = controller.ReceivePortletFile(uploadContexts);
			return controllerWrapper.GetResponseString(jsonResult);
		}

		#region Nested type: FakeView

		/// <summary>Fake IView implementation, only used to instantiate an HtmlHelper.</summary> 
		public class FakeView : IView
		{
			#region IView Members

			public void Render(ViewContext viewContext, TextWriter writer)
			{
				throw new NotImplementedException();
			}

			#endregion
		}

		#endregion
	}
}