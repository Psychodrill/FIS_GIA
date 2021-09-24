using System;
using System.Text;

namespace Esrp.SelfIntegration
{
    public static class XmlStrings
    {
        public static string Escape(string str)
        {
            if (String.IsNullOrEmpty(str))
                return str;
            return new StringBuilder(str)
                .Replace("&", "&amp;")
                .Replace("<", "&lt;")
                .Replace(">", "&gt;")
                .Replace("\"", "&quot;")
                .Replace("'", "&apos;").ToString();
        } 
    }
}
