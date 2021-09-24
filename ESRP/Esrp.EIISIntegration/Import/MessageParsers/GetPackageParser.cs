using System.Collections.Generic;
using System.Xml;

namespace Esrp.EIISIntegration.Import.MessageParsers
{
    internal class GetPackageParser : ParserBase
    {
        public IEnumerable<EIISObject> Objects { get; private set; }

        public GetPackageParser(string response)
            : base(response) { }

        protected override void ProcessResponseInternal()
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(response_);

            List<EIISObject> objects = new List<EIISObject>();

            foreach (XmlNode rowNode in xml.SelectNodes("object/row"))
            {
                EIISObject eIISObject = new EIISObject(rowNode);
                objects.Add(eIISObject);
            }

            Objects = objects;
        }
    }
}
