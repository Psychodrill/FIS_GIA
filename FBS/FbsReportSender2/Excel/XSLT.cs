using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace FbsReportSender.Excel
{
    public class XSLTParams
    {
        private readonly bool mCreateFromFile;
        private readonly string mXsltSource;
        public bool OmitXmlDeclaration;
        public XsltArgumentList XsltArgumentList = new XsltArgumentList();

        /// <param name="xsltSource">путь к файлу xslt-трансформации или имя ресурса</param>
        /// <param name="createFromFile">True, если указан путь к файлу, False-если ресурс</param>
        public XSLTParams(string xsltSource, bool createFromFile)
        {
            mXsltSource = xsltSource;
            mCreateFromFile = createFromFile;
        }

        public XslCompiledTransform CreateTransformer()
        {
            var transformer = new XslCompiledTransform();
            if (mCreateFromFile)
            {
                transformer.Load(mXsltSource);
            }
            else
            {
                // загрузить из ресурсов xslt шаблон
                using (Stream xslStream = typeof (XSLT).Assembly.GetManifestResourceStream(mXsltSource))
                {
                    if (xslStream != null) transformer.Load(new XmlTextReader(xslStream));
                }
            }
            return transformer;
        }
    }

    public static class XSLT
    {
        public static string GetString(string xml, XSLTParams parameters)
        {
            using (Stream stream = GetStream(xml, parameters))
            {
                using (var readerXml = new StreamReader(stream))
                {
                    stream.Position = 0;
                    return readerXml.ReadToEnd();
                }
            }
        }

        public static Stream GetStream(string xml, XSLTParams parameters)
        {
            XslCompiledTransform transformer = parameters.CreateTransformer();
            // читаем xml
            using (var reader = new StringReader(xml))
            {
                var xpathdocument = new XPathDocument(reader);

                //куда пишем
                var memStream = new MemoryStream();

                var settings = new XmlWriterSettings
                                   {
                                       OmitXmlDeclaration = parameters.OmitXmlDeclaration,
                                       Encoding = Encoding.UTF8
                                   };

                XmlWriter writer = XmlWriter.Create(memStream, settings);
                if (writer != null)
                {
                    writer.WriteStartDocument();

                    //транформация
                    transformer.Transform(xpathdocument, parameters.XsltArgumentList, writer);
                }

                memStream.Position = 0;
                return memStream;
            }
        }
    }
}