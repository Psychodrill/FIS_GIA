using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FogSoft.Helpers;
using FogSoft.WSRP.Portlets;
using log4net;
using System.Diagnostics;

namespace FogSoft.WSRP.Factories
{
    /// <summary>
    ///     <see cref="IPortletFactory" /> implementation for portlets marked with <see cref="PortletAttribute" />.
    /// </summary>
    public class AttributedPortletFactory : IPortletFactory
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly Dictionary<string, PortletDescriptor> _portletDescriptors;

        //[DebuggerNonUserCode]
        public AttributedPortletFactory()
        {
            _portletDescriptors = new Dictionary<string, PortletDescriptor>
                (StringComparer.CurrentCultureIgnoreCase);

            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            foreach (Type type in AppDomain.CurrentDomain.GetTypesWith<PortletAttribute>(ignoreSystem: true))
            // ReSharper restore DoNotCallOverridableMethodsInConstructor
            {
                try
                {
                    PortletDescriptor descriptor = ExtractDescriptor(type);
                    _portletDescriptors.Add(descriptor.Handle, descriptor);
                }
                catch (ArgumentException ex)
                {
                    LogIgnoringType(type, ex);
                }
                catch (InvalidOperationException ex)
                {
                    LogIgnoringType(type, ex);
                }
            }

            PortletDescriptors = _portletDescriptors.Values.ToArray();
        }

        public virtual IPortlet Get(PortletDescriptor descriptor)
        {
            if (descriptor == null) throw new ArgumentNullException("descriptor");
            // TODO: check performance
            return (IPortlet) Activator.CreateInstance(descriptor.Type, descriptor);
        }

        public virtual IPortlet Get(string portletHandle)
        {
            if (string.IsNullOrEmpty(portletHandle)) throw new ArgumentNullException("portletHandle");

            // TODO: check performance
            return Get(GetDescriptor(portletHandle));
        }

        public PortletDescriptor GetDescriptor(string portletHandle)
        {
            if (string.IsNullOrEmpty(portletHandle)) throw new ArgumentNullException("portletHandle");
            return _portletDescriptors[portletHandle];
        }

        public PortletDescriptor[] PortletDescriptors { get; private set; }

        private static void LogIgnoringType(Type type, Exception ex)
        {
            Log.Error("Ignore type {0}: {1}.".FormatWith(type.Name, ex.Message), ex);
        }

        private static PortletDescriptor ExtractDescriptor(Type concretePortletType)
        {
            if (concretePortletType == null) throw new ArgumentNullException("concretePortletType");
            if (!typeof (Portlet).IsAssignableFrom(concretePortletType))
                throw new ArgumentException("Specified portlet type does not inherited from MvcPortlet.",
                                            "concretePortletType");

            return DescriptorMapper.GetPortletDescriptor(concretePortletType);
        }
    }
}