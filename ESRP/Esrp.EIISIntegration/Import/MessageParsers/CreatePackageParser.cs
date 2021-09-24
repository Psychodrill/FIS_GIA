using System.Xml;

namespace Esrp.EIISIntegration.Import.MessageParsers
{
    internal class CreatePackageParser : ParserBase
    {
        public string PackageId { get; private set; }

        public CreatePackageParser(string response)
            : base(response) { }

        protected override void ProcessResponseInternal()
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(response_);
            PackageId = xml.FirstChild.Attributes[0].Value;
        }
    }
}
