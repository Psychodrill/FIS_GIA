using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Esrp.Core.Reports.Excel
{
    public static class XSLT
    {
        public static Stream GetStream(string xml, string xsltFilePath)
        {
            XslCompiledTransform transformer = new XslCompiledTransform();
            transformer.Load(xsltFilePath);

            // читаем xml
            using (var reader = new StringReader(xml))
            {
                var xpathdocument = new XPathDocument(reader);

                //куда пишем
                var memStream = new MemoryStream();

                var settings = new XmlWriterSettings
                                   {
                                       OmitXmlDeclaration =false,
                                       Encoding = Encoding.UTF8
                                   };

                XmlWriter writer = XmlWriter.Create(memStream, settings);
                if (writer != null)
                {
                    writer.WriteStartDocument();

                    //транформация
                    transformer.Transform(xpathdocument,new XsltArgumentList(), writer);
                }

                memStream.Position = 0;
                return memStream;
            }
        }
    }
}