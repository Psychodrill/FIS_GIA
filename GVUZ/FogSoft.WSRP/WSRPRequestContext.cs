using System;

namespace FogSoft.WSRP
{
	public class WSRPRequestContext
	{
		public WSRPRequestContext(string portletHandle, string userAgent)
		{
			if (portletHandle == null) throw new ArgumentNullException("portletHandle");
			if (userAgent == null) throw new ArgumentNullException("userAgent");

			UserAgent = userAgent;
			PortletHandle = portletHandle;
		}

		public string UserAgent { get; private set; }
		public string PortletHandle { get; private set; }
	}
}