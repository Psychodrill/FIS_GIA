using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Xml;
using Microsoft.Practices.ServiceLocation;
using log4net;

namespace FogSoft.WSRP
{
	public class MarkupService : IWSRP_v2_Markup_Binding_SOAP
	{
		private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		// ReSharper disable InconsistentNaming

		public MarkupResponse getMarkup(getMarkup parameters)
		{
			try
			{
				return ServiceLocator.Current.GetInstance<IPortletFactory>()
					.Get(parameters.portletContext.portletHandle)
					.GetMarkup(DescriptorMapper.GetMimeTypes(parameters.markupParams.mimeTypes),
					           DescriptorMapper.GetMode(parameters.markupParams.mode),
					           DescriptorMapper.GetState(parameters.markupParams.windowState),
					           parameters.markupParams.navigationalContext.opaqueValue,
					           parameters.userContext.userContextKey);
			}
			catch (Exception e)
			{
				Log.Error(e.Message, e);
				throw new PortletException(e.Message);
			}
		}


		public BlockingInteractionResponse performBlockingInteraction(
			performBlockingInteraction parameters)
		{
			try
			{
				return ServiceLocator.Current.GetInstance<IPortletFactory>()
					.Get(parameters.portletContext.portletHandle)
					.PerformBlockingInteraction
					(DescriptorMapper.GetNameValues(parameters.interactionParams.formParameters),
					 parameters.interactionParams.interactionState,
					 DescriptorMapper.GetMimeTypes(parameters.markupParams.mimeTypes),
					 DescriptorMapper.GetMode(parameters.markupParams.mode),
					 DescriptorMapper.GetState(parameters.markupParams.windowState),
					 parameters.interactionParams.uploadContexts
					);
			}
			catch (Exception e)
			{
				Log.Error(e.Message, e);
				throw new PortletException(e.Message);
			}
		}

		public const string REQUEST_METHOD = "POST";
		public const string REQUEST_CONTENT_TYPE = "application/x-www-form-urlencoded";

		internal static string GetSiteMarkUp(string siteAddressWithQuery, string formParametersString)
		{
			string message = string.Empty;
			if (string.IsNullOrEmpty(formParametersString))
			{
				message = "Submit=clicked";
			}
			else
			{
				message = formParametersString;
			}

			HttpWebRequest Request = (HttpWebRequest) WebRequest.Create(siteAddressWithQuery);
			Request.Credentials =
				new NetworkCredential("UserName",
				                      "Password", "DomainName");

			Request.Method = REQUEST_METHOD;
			Request.ContentLength = message.Length;
			Request.ContentType = REQUEST_CONTENT_TYPE;

			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			StreamWriter requestWriter = new StreamWriter(Request.GetRequestStream());
			requestWriter.Write(message);
			requestWriter.Close();

			HttpWebResponse Response = (HttpWebResponse) Request.GetResponse();
			StreamReader responseReader = new StreamReader(Response.GetResponseStream(), Encoding.UTF8);
			string responseString = responseReader.ReadToEnd();
			responseReader.Close();
			stopwatch.Stop();
			//TODO: debug info
			responseString += "<!--Inner request time: " + stopwatch.Elapsed.TotalSeconds + "-->";
			return responseString;
		}

		public ResourceResponse getResource(getResource getResource1)
		{
			// new in v2
			throw new NotImplementedException();
		}

		public HandleEventsResponse handleEvents(handleEvents handleEvents1)
		{
			// new in v2
			throw new NotImplementedException();
		}

		public XmlElement[] releaseSessions(releaseSessions releaseSessions1)
		{
			throw new NotImplementedException();
		}

		public XmlElement[] initCookie(initCookie initCookie1)
		{
			throw new NotImplementedException();
		}
	}
}