using System;

namespace FogSoft.WSRP
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class PortletWindowStateAttribute : Attribute
	{
		public PortletWindowStateAttribute(MimeType mimeType, PortletWindowState windowState)
		{
			MimeType = mimeType;
			WindowState = windowState;
		}

		public MimeType MimeType { get; private set; }
		public PortletWindowState WindowState { get; private set; }
	}
}