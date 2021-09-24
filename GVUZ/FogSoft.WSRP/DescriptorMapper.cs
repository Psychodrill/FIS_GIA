using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FogSoft.Helpers;
using log4net;
using Microsoft.Practices.ServiceLocation;

namespace FogSoft.WSRP
{
	/// <summary>
	/// Maps portlet-related descriptors to and from other types (attributes etc.).</summary>
	public static class DescriptorMapper
	{
		private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		/// <summary>
		/// Maps <see cref="PortletDescriptor"/>s to WSRP <see cref="PortletDescription"/> type.</summary>
		public static PortletDescription[] GetDescriptions(PortletDescriptor[] descriptors)
		{
			if (descriptors == null) throw new ArgumentNullException("descriptors");
			PortletDescription[] result = new PortletDescription[descriptors.Length];

			string defaultLocale = ServiceLocator.Current.GetInstance<IConfigurationService>().GetLocale();

			for (int index = 0; index < descriptors.Length; index++)
			{
				PortletDescriptor descriptor = descriptors[index];
				MarkupType[] markupTypes = new MarkupType[descriptor.Markups.Length];
				result[index] =
					new PortletDescription
						{
							shortTitle = GetString(descriptor.ShortTitle, descriptor.DefaultLanguage, defaultLocale),
							title = GetString(descriptor.Title, descriptor.DefaultLanguage, defaultLocale),
							displayName = GetString(descriptor.DisplayName, descriptor.DefaultLanguage, defaultLocale),
							portletHandle = descriptor.Handle,
							markupTypes = markupTypes
						};

				
				for (int i = 0; i < descriptor.Markups.Length; i++)
				{
					markupTypes[i] = GetMarkupType(descriptor.Markups[i], descriptor, defaultLocale);
				}
			}

			return result;
		}

		private static MarkupType GetMarkupType(MarkupDescriptor markupDescriptor, PortletDescriptor descriptor, string defaultLocale)

		{
			return 
				new MarkupType
					{
						locales = new [] { descriptor.DefaultLanguage ?? defaultLocale },
						mimeType = GetMimeType(markupDescriptor.MimeType),
						modes = GetModes(markupDescriptor.Modes),
						windowStates = GetStates(markupDescriptor.WindowStates)
					};
		}

		private static string[] GetStates(PortletWindowState[] windowStates)
		{
			string[] result = new string[windowStates.Length];
			for (int index = 0; index < windowStates.Length; index++)
				result[index] = GetState(windowStates[index]);
			
			return result;
		}

		private static string GetState(PortletWindowState windowState)
		{
			switch (windowState)
			{
				case PortletWindowState.Normal:
					return "wsrp:normal";
				case PortletWindowState.Maximized:
					return "wsrp:maximized";
				case PortletWindowState.Minimized:
					return "wsrp:minimized";
				case PortletWindowState.Solo:
					return "wsrp:solo";
				default:
					throw new ArgumentOutOfRangeException("windowState");
			}
		}

		public static Dictionary<string, string> GetNameValues(NamedString[] namedStrings)
		{
			
			Dictionary<string, string> result = new Dictionary<string, string>();
			if (namedStrings == null)
			{
				Log.Debug("Отсутствуют namedStrings в GetNameValues");
				return result;
			}

			foreach (NamedString namedString in namedStrings)
				result.Add(namedString.name, namedString.value);
			
			return result;
		}

		public static PortletWindowState GetState(string windowState)
		{
			switch (windowState)
			{
				case "wsrp:normal":
					return PortletWindowState.Normal;
				case "wsrp:maximized":
					return PortletWindowState.Maximized;
				case "wsrp:minimized":
					return PortletWindowState.Minimized;
				case "wsrp:solo":
					return PortletWindowState.Solo;
				default:
					throw new ArgumentOutOfRangeException("windowState");
			}
		}

		private static string[] GetModes(IList<PortletMode> modes)
		{
			string[] result = new string[modes.Count];
			for (int index = 0; index < modes.Count; index++)
				result[index] = GetMode(modes[index]);
			
			return result;
		}

		private static string GetMode(PortletMode portletMode)
		{
			switch (portletMode)
			{
				case PortletMode.View:
					return "wsrp:view";
				case PortletMode.Edit:
					return "wsrp:edit";
				case PortletMode.Help:
					return "wsrp:help";
				case PortletMode.Preview:
					return "wsrp:preview";
				default:
					throw new ArgumentOutOfRangeException("portletMode");
			}
		}

		public static PortletMode GetMode(string portletMode)
		{
			switch (portletMode)
			{
				case "wsrp:view":
					return PortletMode.View;
				case "wsrp:edit":
					return PortletMode.Edit;
				case "wsrp:help":
					return PortletMode.Help;
				case "wsrp:preview":
					return PortletMode.Preview;
				default:
					throw new ArgumentOutOfRangeException("portletMode");
			}
		}

		private static string GetMimeType(MimeType mimeType)
		{
			switch (mimeType)
			{
				case MimeType.Any:
					return "*";
				case MimeType.TextHtml:
					return "text/html";
				case MimeType.ApplicationXHtml:
					return "application/xhtml+xml";
				case MimeType.TextVndWapWml:
					return "text/vnd.wap.wml";
				default:
					throw new ArgumentOutOfRangeException("mimeType");
			}
		}

		public static MimeType[] GetMimeTypes(string[] mimeTypes)
		{
			if (mimeTypes == null) throw new ArgumentNullException("mimeTypes");
			MimeType[] result = new MimeType[mimeTypes.Length];
			for (int index = 0; index < mimeTypes.Length; index++)
			{
				result[index] = GetMimeType(mimeTypes[index]);
			}
			return result;
		}

		public static MimeType GetMimeType(string mimeType)
		{
			// TODO: refactor pair methods
			switch (mimeType)
			{
				case "*":
					return MimeType.Any;
				case "text/html":
					return MimeType.TextHtml;
				case "application/xhtml+xml":
					return MimeType.ApplicationXHtml;
				case "text/vnd.wap.wml":
					return MimeType.TextVndWapWml;
				default:
					throw new ArgumentOutOfRangeException("mimeType");
			}
		}

		/// <summary>
		/// Maps <see cref="PortletAttribute"/> to the <see cref="PortletDescriptor"/>.
		/// In addiction, calls <see cref="GetMarkups"/> to intialize <see cref="PortletDescriptor.Markups"/>.</summary>
		public static PortletDescriptor GetPortletDescriptor(Type concretePortletType)
		{
			PortletAttribute attribute = concretePortletType.GetAttributes<PortletAttribute>(false)[0];
			return new PortletDescriptor(concretePortletType, attribute.Handle,
			                             GetMarkups(concretePortletType))
			       	{
			       		DefaultLanguage = attribute.DefaultLanguage,
			       		DisplayName = attribute.DisplayName,
			       		ShortTitle = attribute.ShortTitle,
			       		Title = attribute.Title,
			       	};
		}

		/// <summary>
		/// Maps type attributes (<see cref="PortletModeAttribute"/>, <see cref="PortletWindowStateAttribute"/>)
		/// to the array of the <see cref="MarkupDescriptor"/>s.</summary>
		public static MarkupDescriptor[] GetMarkups(Type concretePortletType)
		{
			PortletModeAttribute[] modeAttributes = concretePortletType.GetAttributes<PortletModeAttribute>();
			PortletWindowStateAttribute[] stateAttributes =
				concretePortletType.GetAttributes<PortletWindowStateAttribute>();
			MimeType[] modeMimeTypes =
				(from m in modeAttributes group m by m.MimeType into t select t.Key).ToArray();
			MimeType[] stateMimeTypes =
				(from s in stateAttributes group s by s.MimeType into t select t.Key).ToArray();
			if (modeMimeTypes.Length != stateMimeTypes.Length || modeMimeTypes.Length == 0)
				throw new InvalidOperationException
					(string.Format("Invalid markup attributes for {0}.", concretePortletType.Name));

			MarkupDescriptor[] markups = new MarkupDescriptor[modeMimeTypes.Length];
			for (int index = 0; index < modeMimeTypes.Length; index++)
			{
				MimeType mimeType = modeMimeTypes[index];
				markups[index] = new MarkupDescriptor
					(mimeType,
					 (from x in modeAttributes where x.MimeType == mimeType select x.PortletMode).ToArray(),
					 (from x in stateAttributes where x.MimeType == mimeType select x.WindowState).ToArray());
			}
			return markups;
		}

		private static LocalizedString GetString(string value, string lang, string defaultLocale)
		{
			return new LocalizedString
			       	{
			       		value = value,
			       		lang = string.IsNullOrEmpty(lang) ? defaultLocale : lang
			       	};
		}
	}
}