using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Xml;

namespace FbsReportSender.Email
{
    /// <summary>
    /// Типы шаблонов email сообщений
    /// </summary>
    public enum EmailTemplateTypeEnum
    {
        ReportViews
    }

    /// <summary>
    /// Типы расписаний email сообщений
    /// </summary>
    public enum EmailScheduleTypeEnum
    {
        Daily,
        Weekly
    }

    /// <summary>
    /// email сообщение, подготовленное из шаблона
    /// </summary>
    public class EMailMessageFromTemplate : MailMessage
    {
        private readonly Dictionary<string, string> mParameters = new Dictionary<string, string>();
        private readonly EmailTemplateTypeEnum mTemplate;

        public EMailMessageFromTemplate(EmailTemplateTypeEnum template)
        {
            mTemplate = template;
            ParseFromXml();
        }

        private string DefineTemplatePath()
        {
            string filePath = String.Format("Resources/EmailTemplate_{0}.xml", mTemplate);
            return Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, filePath);
        }

        private void ParseFromXml()
        {
            string filePath = DefineTemplatePath();
            if (!File.Exists(filePath))
                throw new FileNotFoundException(filePath);

            var xml = new XmlDocument();
            xml.Load(filePath);

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