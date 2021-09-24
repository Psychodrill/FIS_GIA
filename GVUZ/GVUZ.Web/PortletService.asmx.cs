using System.ComponentModel;
using System.Web;
using System.Web.Services;
using System.Web.SessionState;
using System.Xml;
using FogSoft.WSRP;

namespace GVUZ.Web
{
	/// <summary>
	/// Summary description for PortletService
	/// </summary>
	/*[WebService(Namespace = "urn:oasis:names:tc:wsrp:v2:wsdl")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]*/
	[ToolboxItem(false)]
	// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
		// [System.Web.Script.Services.ScriptService]
	public class PortletService : WebService, IWSRP_v2_Markup_Binding_SOAP,
	                              IWSRP_v2_ServiceDescription_Binding_SOAP
	{
		private const string SessionCookieName = "gvuz.cookie";
// ReSharper disable InconsistentNaming
		[WebMethod]
		public ServiceDescription getServiceDescription(getServiceDescription getServiceDescription1)
		{
			return new DescriptionService().getServiceDescription(getServiceDescription1);
		}

		[WebMethod(EnableSession = true)]
		public MarkupResponse getMarkup(getMarkup getMarkup1)
		{
			return new MarkupService().getMarkup(getMarkup1);
		}

		[WebMethod(EnableSession = true)]
		public ResourceResponse getResource(getResource getResource1)
		{
			return new MarkupService().getResource(getResource1);
		}

		[WebMethod(EnableSession = true)]
		public BlockingInteractionResponse performBlockingInteraction(
			performBlockingInteraction performBlockingInteraction1)
		{
			return new MarkupService().performBlockingInteraction(performBlockingInteraction1);
		}

		[WebMethod(EnableSession = true)]
		public HandleEventsResponse handleEvents(handleEvents handleEvents1)
		{
			return new MarkupService().handleEvents(handleEvents1);
		}

		[WebMethod]
		public XmlElement[] releaseSessions(releaseSessions releaseSessions1)
		{
			return new MarkupService().releaseSessions(releaseSessions1);
		}

		[WebMethod]
		public XmlElement[] initCookie(initCookie initCookie1)
		{
			HttpCookie sessionCookie = Context.Request.Cookies.Get(SessionCookieName);
			if (sessionCookie == null)
			{
				HttpCookie cookie = new HttpCookie(SessionCookieName, new SessionIDManager().CreateSessionID(Context));
				Context.Response.Cookies.Add(cookie);
			}
			return new XmlElement[0];
		}
	}
}