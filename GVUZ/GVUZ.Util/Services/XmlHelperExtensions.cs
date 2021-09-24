using System;
using System.Xml;

namespace GVUZ.Util.Services
{
    public static class XmlHelperExtensions
    {
        public static DateTime? GetDate(this XmlElement el, string xpath)
        {
            el = el.SelectSingleNode(xpath) as XmlElement;

            if (el != null)
            {
                DateTime result;
                if (DateTime.TryParse(el.InnerText, out result))
                {
                    return result;
                }
            }

            return null;
        }

        public static int? GetInt(this XmlElement el, string xpath)
        {
            el = el.SelectSingleNode(xpath) as XmlElement;

            if (el != null)
            {
                int result;
                if (int.TryParse(el.InnerText, out result))
                {
                    return result;
                }
            }

            return null;
        }

        public static bool? GetBool(this XmlElement el, string xpath)
        {
            el = el.SelectSingleNode(xpath) as XmlElement;

            if (el != null)
            {
                bool result;
                if (bool.TryParse(el.InnerText, out result))
                {
                    return result;
                }

                int asInt;

                if (int.TryParse(el.InnerText, out asInt))
                {
                    return asInt == 1;
                }
            }

            return null;
        }

        public static string GetText(this XmlElement el, string xpath)
        {
            el = el.SelectSingleNode(xpath) as XmlElement;

            if (el != null)
            {
                return el.InnerText;
            }

            return null;
        }
    }
}