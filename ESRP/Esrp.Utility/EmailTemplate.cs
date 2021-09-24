using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Xml;

namespace Esrp.Utility
{
    /// <summary>
    /// Типы шаблонов email сообщений
    /// </summary>
    public enum EmailTemplateTypeEnum
    {
        None,
        Activation,
        ChangePassword,
        Registration,
				RegistrationFbdAdmission,
        RegistrationWithoutOrg,
        Registration_NewOrg,
        RemindPassword,
        SendToRevision,
        Consideration,
        AdminChangePassword,
        RegistrationSetPassword,
        AdminRemindPassword,
        FilialActivationFailed
    }

    /// <summary>
    /// Шаблон email сообщения
    /// </summary>
    public class EmailTemplate
    {
        private EmailTemplateTypeEnum mTemplate;

        public string Subject;
        public string From;
        public string To;
        public Encoding Encoding;
        public string ContentType;
        public string Body;

        public EmailTemplate(EmailTemplateTypeEnum template)
        {
            this.mTemplate = template;
            ParseFromXml();
        }

        private void ParseFromXml()
        {
            string filePath = DefineTemplatePath();
            if (!File.Exists(filePath))
                throw new FileNotFoundException(filePath);

            XmlDocument xml = new XmlDocument();
            xml.Load(filePath);

            if (xml.SelectSingleNode("//template/subject") != null)
                this.Subject = xml.SelectSingleNode("//template/subject").InnerText;
            if (xml.SelectSingleNode("//template/from") != null)
                this.From = xml.SelectSingleNode("//template/from").InnerText;
            if (xml.SelectSingleNode("//template/to") != null)
                this.To = xml.SelectSingleNode("//template/to").InnerText;
            if (xml.SelectSingleNode("//template/content-type") != null)
                this.ContentType = xml.SelectSingleNode("//template/content-type").InnerText;
            if (xml.SelectSingleNode("//template/body") != null)
                this.Body = xml.SelectSingleNode("//template/body").InnerText;

            // определю кодировку
            string encodingValue = xml.SelectSingleNode("//template/encoding").InnerText;
            if (encodingValue != null && encodingValue.Length > 0)
            {
                try
                {
                    Encoding = Encoding.GetEncoding(encodingValue);
                }
                catch
                {
                    throw new ApplicationException(string.Format(
                        "В шаблоне \"{0}\" задана несуществующая кодировка сообщения \"{0}\"", DefineTemplateName(), encodingValue));
                }
            }
        }

        public EmailMessage ToEmailMessage()
        {
            EmailMessage message = new EmailMessage();
            message.Subject = this.Subject;
            message.From = this.From;
            message.To = this.To;
            message.ContentType = this.ContentType;
            message.Encoding = this.Encoding;
            message.Body = this.Body;

            return message;
        }

        private string DefineTemplateName()
        {
            if (this.mTemplate == EmailTemplateTypeEnum.None)
                return "Имя не определено";

            return this.mTemplate.ToString();
        }

        private string DefineTemplatePath()
        {
            if (this.mTemplate == EmailTemplateTypeEnum.None)
                return string.Empty;

            string filePath = String.Format("/Templates/{0}.xml", this.mTemplate.ToString());
            return HttpContext.Current.Server.MapPath(filePath);
        }
    }
}
