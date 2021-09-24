using System;
using System.Web;
using FogSoft.Helpers;

namespace GVUZ.Helper
{
    public static class HttpContextExtensions
    {
        private const string PortletKey = "InsidePortlet";

        public static bool InsidePortlet(this HttpContextBase context)
        {
            return context != null && context.Items[PortletKey].To(false);
        }

        public static void SetInsidePortlet(this HttpContextBase context, bool insidePortlet)
        {
            if (context == null) throw new ArgumentNullException("context");
            context.Items[PortletKey] = insidePortlet;
        }
    }
}