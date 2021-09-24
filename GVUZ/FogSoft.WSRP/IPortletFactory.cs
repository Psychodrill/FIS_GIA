namespace FogSoft.WSRP
{
	/// <summary>
	/// Portlet factory to retrieve description and concrete portlete instances.</summary>
	public interface IPortletFactory
	{
		/// <summary>
		/// Returns <see cref="IPortlet"/> by the <see cref="PortletDescriptor"/>.</summary>
		IPortlet Get(PortletDescriptor descriptor);

		/// <summary>
		/// Returns <see cref="IPortlet"/> by the handle.</summary>
		IPortlet Get(string portletHandle);

		/// <summary>
		/// Returns <see cref="PortletDescriptor"/> by the handle.</summary>
		PortletDescriptor GetDescriptor(string portletHandle);

		/// <summary>
		/// Gets all<see cref="PortletDescriptor"/>s for this factory.</summary>
		PortletDescriptor[] PortletDescriptors { get; }
	}
}