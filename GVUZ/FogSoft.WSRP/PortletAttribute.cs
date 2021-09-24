using System;

namespace FogSoft.WSRP
{
	/// <summary>
	/// Attribute for portlet description.
	/// <seealso cref="PortletModeAttribute"/> <seealso cref="PortletWindowStateAttribute"/><seealso cref="PortletDescriptor"/></summary>
	/// <remarks><see cref="Title"/>, <see cref="ShortTitle"/> and <see cref="DisplayName"/> are optional in <see cref="PortletDescriptor"/>
	/// (you can set one of them and others will use this value).</remarks>
	[AttributeUsage(AttributeTargets.Class)]
	public class PortletAttribute : Attribute
	{
		public PortletAttribute(string handle)
		{
			Handle = handle;
		}

		public string Title { get; set; }
		public string DefaultLanguage { get; set; }
		public string ShortTitle { get; set; }
		public string DisplayName { get; set; }
		public string Handle { get; private set; }
	}
}