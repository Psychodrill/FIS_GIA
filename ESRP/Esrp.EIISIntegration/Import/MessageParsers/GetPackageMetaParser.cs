using System;
using System.Xml;

namespace Esrp.EIISIntegration.Import.MessageParsers
{
    internal class GetPackageMetaParser : ParserBase
    {
        public int PackagePartsCount { get; private set; }

        public GetPackageMetaParser(string response)
            : base(response) { }

        protected override void ProcessResponseInternal()
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(response_);
            PackagePartsCount = Convert.ToInt32(xml.FirstChild.Attributes["capacity"].Value);
        }
    }
}
