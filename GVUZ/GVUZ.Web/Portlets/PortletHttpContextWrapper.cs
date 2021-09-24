using System.Collections;
using System.Collections.Generic;
using System.Web;

namespace GVUZ.Web.Portlets
{
	public class PortletHttpContextWrapper : HttpContextWrapper
	{
		private readonly HttpContext _httpContext;
		readonly Dictionary<object,object> _items;

		public PortletHttpContextWrapper(HttpContext httpContext) : base(httpContext)
		{
			_httpContext = httpContext;
			_items = new Dictionary<object, object>();
		}

		public override IDictionary Items
		{
			get { return _httpContext == null ? _items : _httpContext.Items; }
		}
	}
}