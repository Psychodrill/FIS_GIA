using System;
using System.Xml;

namespace Esrp.EIISIntegration.Import.MessageParsers
{
    internal class GetSessionIdParser : ParserBase
    {
        public string SessionId { get; private set; }

        public GetSessionIdParser(string response)
            : base(response) { }

        protected override void ProcessResponseInternal()
        {
            XmlDocument xml = new XmlDocument();
            try
            {
                xml.LoadXml(response_);
                SessionId = xml.FirstChild.Attributes[0].Value;
            }
            catch (Exception ex)
            {
                throw new Exception("Произошла ошибка при разборе сообщения (ожидался XML): " + response_, ex);
            }
        }
    }
}
