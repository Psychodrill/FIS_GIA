using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace GVUZ.Helper.Import
{
    public class XmlValidator
    {
        public string ErrorMessage;

        public static string ValidateWithXsd(string XsdPath, string XmlPath)
        {
            var streamReader = new StreamReader(XsdPath);
            var xmlTextReader = new XmlTextReader(XmlPath);
            try
            {
                return ValidateWithXsd(streamReader, xmlTextReader);
            }
            finally
            {
                streamReader.Close();
                xmlTextReader.Close();
            }
        }

        public static string ValidateWithXsd(StreamReader xsdSchema, XmlTextReader xmlData)
        {
            try
            {
                var xmlValidator = new XmlValidator();

                // 3- Create a new instance of XmlSchema object
                // 4- Set Schema object by calling XmlSchema.Read() method
                XmlSchema Schema = XmlSchema.Read(xsdSchema,
                                                  xmlValidator.ReaderSettings_ValidationEventHandler);

                // 5- Create a new instance of XmlReaderSettings object
                var ReaderSettings = new XmlReaderSettings();
                // 6- Set ValidationType for XmlReaderSettings object
                ReaderSettings.ValidationType = ValidationType.Schema;
                // 7- Add Schema to XmlReaderSettings Schemas collection
                ReaderSettings.Schemas.Add(Schema);

                // 8- Add your ValidationEventHandler address to
                // XmlReaderSettings ValidationEventHandler
                ReaderSettings.ValidationEventHandler +=
                    xmlValidator.ReaderSettings_ValidationEventHandler;

                // 9- Create a new instance of XmlReader object
                XmlReader objXmlReader = XmlReader.Create(xmlData, ReaderSettings);

                // 10- Read XML content in a loop
                while (objXmlReader.Read())
                {
                    /*Empty loop*/
                }

                return xmlValidator.ErrorMessage;
            }
            catch (UnauthorizedAccessException AccessEx)
            {
                throw AccessEx;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        private void ReaderSettings_ValidationEventHandler(object sender,
                                                           ValidationEventArgs args)
        {
            if (sender == null) return;

            var lineInfo = sender as IXmlLineInfo;
            ErrorMessage = "Line: " + lineInfo.LineNumber + " - Position: "
                           + lineInfo.LinePosition + " - " + args.Message;
        }
    }
}