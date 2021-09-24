using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.IO;

namespace Esrp.Web.UrlShortener
{
    public class XmlStorage
    {
        static XDocument _doc;
        static string _fileName;
        static XmlStorage()
        {
            if (HttpContext.Current != null)
            {
                _fileName = Path.Combine(HttpContext.Current.Server.MapPath("~/"), "ShortUrls.xml");
            }
            else
                _fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ShortUrls.xml");
            if (File.Exists(_fileName))
                _doc = XDocument.Load(_fileName);
            else
            {
                _doc = new XDocument(new XElement("Urls"));
               
                _doc.Save(_fileName);
                
            }
        }
        public static void AddShortUrl(Container shortUrl)
        {
            XElement elem = new XElement("Url", shortUrl.RealUrl);

            elem.SetAttributeValue("shortUrl", shortUrl.ShortenedUrl);
            elem.SetAttributeValue("createdDate", shortUrl.CreateDate);

            _doc.Root.Add(elem);
            _doc.Save(_fileName);
        }

        public static Container GetRealUrl(string shortUrl)
        {
            XElement elem = _doc.Root.Elements("Url").Where(x => x.Attribute("shortUrl").Value==shortUrl).FirstOrDefault();
            if (elem == null)
                return null;
            else return new Container { CreateDate =DateTime.Parse(elem.Attribute("createdDate").Value), RealUrl = elem.Value, ShortenedUrl = shortUrl };

        }

        public static Container GetShortUrl(string longUrl)
        {
            XElement elem = _doc.Root.Elements("Url").Where(x => x.Value == longUrl).FirstOrDefault();
            if (elem == null)
                return null;
            else return new Container { CreateDate = DateTime.Parse(elem.Attribute("createdDate").Value), RealUrl = elem.Value, ShortenedUrl = elem.Attribute("shortUrl").Value };

        }
    }
}