using Microsoft.Practices.ServiceLocation;

namespace FogSoft.WSRP
{
	public class DescriptionService : IWSRP_v2_ServiceDescription_Binding_SOAP
	{
		public ServiceDescription getServiceDescription(getServiceDescription getServiceDescription)
		{
			PortletDescriptor[] descriptors =
				ServiceLocator.Current.GetInstance<IPortletFactory>().PortletDescriptors;

			return new ServiceDescription
			       	{
			       		offeredPortlets = DescriptorMapper.GetDescriptions(descriptors),
			       		requiresInitCookie = CookieProtocol.perUser
			       	};
		}
	}
}