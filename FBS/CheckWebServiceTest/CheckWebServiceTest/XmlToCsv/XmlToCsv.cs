using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;

namespace CheckWebService.XmlToCsv
{
    public abstract class XmlToCsv
    {
        protected StringBuilder csv = new StringBuilder();

        protected abstract string HeaderPattern { get; }
        protected abstract string FooterPattern { get; }

        protected abstract void AppendCertificateRow(XmlNode certificateNode);

        public string ExtractCsv(XmlDocument xmlDoc)
        {
            csv.AppendLine(HeaderPattern);

            foreach (XmlNode certificateNode in xmlDoc.SelectNodes("//certificate"))
            {
                AppendCertificateRow(certificateNode);
            }

            csv.AppendLine(FooterPattern);

            return csv.ToString();
        }

        protected virtual string ExtractSubjectMark(XmlNode certificateNode, string markName)
        {
            XmlNode markNameNode = certificateNode.SelectSingleNode(String.Format("descendant::subjectName[text() = '{0}']", markName));
            if (markNameNode == null)
                return "";
            XmlNode markValueNode = markNameNode.ParentNode.SelectSingleNode("subjectMark");
            if (markValueNode == null)
                return "";
            //return String.Format("Ошибка  ({0})", markValueNode.InnerText);
            return markValueNode.InnerText;
        }

        protected virtual string ExtractSubjectAppeal(XmlNode certificateNode, string markName)
        {
            XmlNode markNameNode = certificateNode.SelectSingleNode(String.Format("descendant::subjectName[text() = '{0}']", markName));
            if (markNameNode == null)
                return "";
            XmlNode AppealNode = markNameNode.ParentNode.SelectSingleNode("subjectAppeal");
            if (AppealNode == null)
                return "";
            return AppealNode.InnerText;
        }

        protected string DefaultAppealValue
        {
            get { return "0"; }
        }
    }
}
