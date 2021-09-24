using System.ServiceModel.Channels;
using System.Xml;

/*
 * Want more WCF tips?
 * Visit http://webservices20.blogspot.com/
 */

namespace WebServices20.BindingExtenions
{
	public class AutoSecuredHttpTransportElement : HttpTransportBindingElement,
	                                               ITransportTokenAssertionProvider
	{
		public override T GetProperty<T>(BindingContext context)
		{
			if (typeof (T) == typeof (ISecurityCapabilities))
				return (T) (ISecurityCapabilities) new AutoSecuredHttpSecurityCapabilities();

			return base.GetProperty<T>(context);
		}

		public XmlElement GetTransportTokenAssertion()
		{
			return null;
		}
	}
}