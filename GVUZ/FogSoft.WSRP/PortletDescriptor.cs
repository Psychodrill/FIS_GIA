using System;

namespace FogSoft.WSRP
{
	/// <summary>
	/// Basic portlet attributes.</summary>
	/// <remarks><see cref="Title"/>, <see cref="ShortTitle"/> and <see cref="DisplayName"/> are optional
	/// (you can set one of them and others will use this value).</remarks>
	public class PortletDescriptor
	{
		private string _title;
		private string _shortTitle;
		private string _displayName;

		public PortletDescriptor(Type type, string handle, MarkupDescriptor[] markups)
		{
			if (type == null) throw new ArgumentNullException("type");
			if (string.IsNullOrEmpty(handle)) throw new ArgumentNullException("handle");
			if (markups == null || markups.Length == 0) throw new ArgumentNullException("markups");

			Type = type;
			Handle = handle;
			Markups = markups;
		}

		/// <summary>
		/// Implementation of the portlet.</summary>
		public Type Type { get; private set; }

		public string Handle { get; private set; }

		public MarkupDescriptor[] Markups { get; private set; }

		public string Title
		{
			get { return _title ?? _shortTitle ?? _displayName; }
			set { _title = value; }
		}

		public string ShortTitle
		{
			get { return _shortTitle ?? _title ?? _displayName; }
			set { _shortTitle = value; }
		}

		public string DisplayName
		{
			get { return _displayName ?? _shortTitle ?? _title; }
			set { _displayName = value; }
		}

		public string DefaultLanguage { get; set; }
	}

	public class MarkupDescriptor
	{
		public MarkupDescriptor(MimeType mimeType, PortletMode[] modes, PortletWindowState[] windowStates)
		{
			MimeType = mimeType;
			Modes = modes;
			WindowStates = windowStates;
		}

		/// <summary>
		/// Returns <see cref="MimeType"/>s, supported by specific portlet
		/// for which other <see cref="MarkupDescriptor"/> properties applies.</summary>
		public MimeType MimeType { get; private set; }

		/// <summary>
		/// Returns <see cref="PortletMode"/>s, supported by specific portlet.</summary>
		public PortletMode[] Modes { get; private set; }

		/// <summary>
		/// Returns <see cref="PortletWindowState"/>s, supported by specific portlet.</summary>
		public PortletWindowState[] WindowStates { get; private set; }
	}

	public enum PortletMode
	{
		/// <summary>
		/// The expected functionality for a Portlet in wsrp:view mode is to render markup reflecting
		/// the current state of the Portlet. The wsrp:view mode of a Portlet will include one or more screens
		/// that the End-User can navigate and interact with or it may consist of static content devoid of user interactions. </summary>
		/// <remarks>The behavior and the generated content of a Portlet in the wsrp:view mode
		/// may depend on configuration, personalization and all forms of state.</remarks>
		View,
		/// <summary>
		/// Within the wsrp:edit mode, a Portlet should provide content and logic that let a user customize the behavior of the Portlet,
		/// though such customizations are not limited to markup generated while in this mode.
		/// The wsrp:edit mode can include one or more screens which users can navigate to enter their customization data. </summary>
		Edit,
		/// <summary>
		/// When in wsrp:help mode, a Portlet may provide help screens that explains the Portlet and its expected usage.
		/// Some Portlets will provide context-sensitive help based on the markup
		/// the End-User was viewing when entering this mode. </summary>
		Help,
		/// <summary>
		/// In wsrp:preview mode, a Portlet should provide a rendering of its standard wsrp:view mode content,
		/// as a visual sample of how this Portlet will appear on the End-User's page with the current configuration.
		/// This could be useful for a Consumer that offers an advanced layout capability.</summary>
		Preview,
	}

	public enum PortletWindowState
	{
		/// <summary>
		/// The wsrp:normal window state indicates the Portlet is likely sharing the aggregated page with other Portlets.
		/// The wsrp:normal window state MAY also indicate that the target device has limited display capabilities.
		/// Therefore, a Portlet SHOULD restrict the size of its rendered output in this window state.</summary>
		Normal,
		/// <summary>
		/// When the window state is wsrp:minimized, the Portlet SHOULD NOT render visible markup,
		/// but is free to include non-visible data such as JavaScript [A303] or hidden forms.
		/// The getMarkup operation can be invoked for the wsrp:minimized state just as for all other window states.</summary>
		Minimized,
		/// <summary>
		/// The wsrp:maximized window state is an indication the Portlet is likely the only Portlet being rendered in the aggregated page,
		/// or that the Portlet has more space compared to other Portlets in the aggregated page.
		/// A Portlet SHOULD generate richer content when its window state is wsrp:maximized.</summary>
		Maximized,
		/// <summary>
		/// The wsrp:solo window state is an indication the Portlet is the only Portlet being rendered in the aggregated page.
		/// A Portlet SHOULD generate richer content when its window state is wsrp:solo.</summary>
		Solo
	}

	public enum MimeType
	{
		/// <summary>
		/// Indicates all mime types are supported</summary>
		Any,
		/// <summary>
		/// text/html</summary>
		TextHtml,
		/// <summary>
		/// application/xhtml+xml</summary>
		ApplicationXHtml,
		/// <summary>
		/// text/vnd.wap.wml</summary>
		TextVndWapWml,
	}
}