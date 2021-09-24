using System;

namespace FogSoft.WSRP
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class PortletModeAttribute : Attribute
	{
		public PortletModeAttribute(MimeType mimeType, PortletMode portletMode)
		{
			MimeType = mimeType;
			PortletMode = portletMode;
		}

		public MimeType MimeType { get; private set; }
		public PortletMode PortletMode { get; private set; }
	}
}