using System;
using System.ServiceModel.Channels;
using System.Xml;

/*
 * Want more WCF tips?
 * Visit http://webservices20.blogspot.com/
 */

namespace WebServices20.BindingExtenions
{
	public class ClearUsernameBinding : CustomBinding
	{
		private MessageVersion messageVersion = MessageVersion.None;

		public void SetMessageVersion(MessageVersion value)
		{
			messageVersion = value;
		}

		//public AuthenticationSchemes ProxyAuthSchema { get; set; }
		//public Uri ProxyAddress { get; set; }
		//public bool UseDefaultWebProxy { get; set; }

		//public ClearUsernameBinding()
		//{
		//UseDefaultWebProxy = true;
		//ProxyAuthSchema = AuthenticationSchemes.Anonymous;
		//}

		public override BindingElementCollection CreateBindingElements()
		{
			SendTimeout = new TimeSpan(0, 2, 0);
			ReceiveTimeout = new TimeSpan(0, 2, 0);
			OpenTimeout = new TimeSpan(0, 0, 20);

			var res = new BindingElementCollection();

			var textElement = new TextMessageEncodingBindingElement {MessageVersion = messageVersion};

			textElement.ReaderQuotas.MaxArrayLength = XmlDictionaryReaderQuotas.Max.MaxArrayLength;
			textElement.ReaderQuotas.MaxStringContentLength = XmlDictionaryReaderQuotas.Max.MaxStringContentLength;
			textElement.ReaderQuotas.MaxBytesPerRead = XmlDictionaryReaderQuotas.Max.MaxBytesPerRead;

			res.Add(textElement);

			res.Add(SecurityBindingElement.CreateUserNameOverTransportBindingElement());


			res.Add(new AutoSecuredHttpTransportElement
			        	{
			        		MaxReceivedMessageSize = int.MaxValue,
			        		MaxBufferSize = int.MaxValue,
			        		UseDefaultWebProxy = true
			        		//,
			        		//ProxyAuthenticationScheme = ProxyAuthSchema,
			        		//ProxyAddress = ProxyAddress
			        	});

			return res;
		}

		public override string Scheme
		{
			get { return "http"; }
		}
	}
}