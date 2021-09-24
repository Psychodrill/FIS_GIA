using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Xml;

namespace Fbs.Core.Reports.Email
{
    /// <summary>
    /// email сообщение, подготовленное из шаблона
    /// </summary>
    public class EMailMessageFromTemplate : MailMessage
    {
        private readonly Dictionary<string, string> mParameters = new Dictionary<string, string>();

       
        public EMailMessageFromTemplate(  string mailTemplatePath)
        {
            ParseFromXml(mailTemplatePath);
        }

        private void ParseFromXml(string mailTemplatePath)
        {
            if (!File.Exists(mailTemplatePath))
                throw new FileNotFoundException(mailTemplatePath);

            var xml = new XmlDocument();
            xml.Load(mailTemplatePath);

            if (xml.SelectSingleNode("//template/subject") != null)
                Subject = xml.SelectSingleNode("//template/subject").InnerText;
            if (xml.SelectSingleNode("//template/from") != null)
                From = new MailAddress(xml.SelectSingleNode("//template/from").InnerText);
            if (xml.SelectSingleNode("//template/to") != null)
                To.Add(new MailAddress(xml.SelectSingleNode("//template/to").InnerText));

            if (xml.SelectSingleNode("//template/content-type") != null)
                if (xml.SelectSingleNode("//template/content-type").InnerText.ToLower() == "html")
                    IsBodyHtml = true;

            if (xml.SelectSingleNode("//template/body") != null)
                Body = xml.SelectSingleNode("//template/body").InnerText;

            // определю кодировку
            string encodingValue = xml.SelectSingleNode("//template/encoding").InnerText;
            if (!string.IsNullOrEmpty(encodingValue))
            {
                try
                {
                    BodyEncoding = Encoding.GetEncoding(encodingValue);
                }
                catch
                {
                    BodyEncoding = Encoding.UTF8;
                }
            }
        }


        public void AddParam(string key, string value)
        {
            if (mParameters.ContainsKey(key))
                mParameters[key] = value;
            else
                mParameters.Add(key, value);
        }

        public void ApplyParameters()
        {
            foreach (string key in  mParameters.Keys)
            {
                if (Subject.Contains(key))
                    Subject = Subject.Replace(key, mParameters[key]);

                if (Body.Contains(key))
                    Body = Body.Replace(key, mParameters[key]);
            }
        }
    }
}