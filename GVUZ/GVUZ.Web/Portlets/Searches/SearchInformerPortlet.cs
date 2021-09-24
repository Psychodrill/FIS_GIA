using System;
using System.Collections.Generic;
using FogSoft.WSRP;

namespace GVUZ.Web.Portlets.Searches
{
	[Portlet("gvuz.SearchInformer", DisplayName = "Поиск образовательного учреждения (тест)", Title = "Поиск (тест)")]
	[PortletMode(MimeType.TextHtml, PortletMode.View)]
	[PortletWindowState(MimeType.TextHtml, PortletWindowState.Normal)]
	public class SearchInformerPortlet : MainPortlet
	{

		public SearchInformerPortlet(PortletDescriptor descriptor) : base(descriptor)
		{
		}

	}
}