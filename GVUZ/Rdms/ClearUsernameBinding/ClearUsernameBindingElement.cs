using System;
using System.Configuration;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;

/*
 * Want more WCF tips?
 * Visit http://webservices20.blogspot.com/
 */

namespace WebServices20.BindingExtenions
{
	internal class ClearUsernameBindingElement : StandardBindingElement
	{
		private ConfigurationPropertyCollection _properties;

		protected override void OnApplyConfiguration(Binding binding)
		{
			ClearUsernameBinding b = binding as ClearUsernameBinding;
			b.SetMessageVersion(MessageVersion);
		}

		protected override Type BindingElementType
		{
			get { return typeof (ClearUsernameBinding); }
		}

		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				if (_properties == null)
				{
					ConfigurationPropertyCollection baseProperties = base.Properties;
					baseProperties.Add(new ConfigurationProperty("messageVersion", typeof(MessageVersion), MessageVersion.Soap11,
					                                         new MessageVersionConverter(), null, ConfigurationPropertyOptions.None));
					this._properties = baseProperties;
				}
				return _properties;
			}
		}

		public MessageVersion MessageVersion
		{
			get { return (MessageVersion) base["messageVersion"]; }
			set { base["messageVersion"] = value; }
		}
	}
}