using System.Net.Security;
using System.ServiceModel.Channels;

/*
 * Want more WCF tips?
 * Visit http://webservices20.blogspot.com/
 */

namespace WebServices20.BindingExtenions
{
	public class AutoSecuredHttpSecurityCapabilities : ISecurityCapabilities
	{
		public ProtectionLevel SupportedRequestProtectionLevel
		{
			get { return ProtectionLevel.EncryptAndSign; }
		}

		public ProtectionLevel SupportedResponseProtectionLevel
		{
			get { return ProtectionLevel.EncryptAndSign; }
		}

		public bool SupportsClientAuthentication
		{
			get { return false; }
		}

		public bool SupportsClientWindowsIdentity
		{
			get { return false; }
		}

		public bool SupportsServerAuthentication
		{
			get { return true; }
		}
	}
}