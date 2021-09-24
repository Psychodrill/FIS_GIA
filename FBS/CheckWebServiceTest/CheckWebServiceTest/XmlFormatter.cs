using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;

namespace CheckWebService
{
    public class XmlFormatter
    {
        int depth = 0;
        public string FormatXml(string xmlString)
        {
            if (String.IsNullOrEmpty(xmlString))
                return String.Empty;

            XmlDocument document = new XmlDocument();
            document.LoadXml(xmlString);

            StringBuilder stringBuilder = new StringBuilder();
            FormatXmlNode(document, stringBuilder);
            return stringBuilder.ToString();
        }

        private void FormatXmlNode(XmlNode node, StringBuilder stringBuilder)
        {
            if (node.NodeType == XmlNodeType.Element)
            {
                stringBuilder.Append(GetIndent());
                stringBuilder.Append("<");
                stringBuilder.Append(node.Name);
                stringBuilder.Append(">");
                stringBuilder.Append(Environment.NewLine);
            }
            if (node.NodeType == XmlNodeType.Text)
            {
                stringBuilder.Append(GetIndent());
                stringBuilder.Append(node.InnerText);
                stringBuilder.Append(Environment.NewLine);
            }

            foreach (XmlNode childNode in node.ChildNodes)
            {
                depth++;
                FormatXmlNode(childNode, stringBuilder);
                depth--;
            }

            if (node.NodeType == XmlNodeType.Element)
            {
                stringBuilder.Append(GetIndent());
                stringBuilder.Append("</");
                stringBuilder.Append(node.Name);
                stringBuilder.Append(">");
                stringBuilder.Append(Environment.NewLine);
            }
        }

        private string GetIndent()
        {
            string result = String.Empty;
            for (int i = 1; i < depth; i++)
            {
                result += "   ";
            }
            return result;
        }
    }
}
