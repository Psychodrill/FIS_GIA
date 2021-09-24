using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using FogSoft.Helpers;

namespace GVUZ.Helper.ExternalValidation
{
    [XmlRoot("items", Namespace = "")]
    public class EgePacket
    { 
        private static readonly XmlSerializer Serializer;
        private static readonly XmlReaderSettings ReaderSettings;

        static EgePacket()
        {
            Serializer = new XmlSerializer(typeof (EgePacket));

            var schemas = new XmlSchemaSet();
            schemas.Add("", XmlReader.Create(Resources.GetResourceStream<EgePacket>("EgePacket.xsd")));
            ReaderSettings = new XmlReaderSettings
                {
                    Schemas = schemas,
                    ValidationType = ValidationType.Schema
                };
        }

        public EgePacket()
        {
            Queries = new List<EgeQuery>();
        }

        public EgePacket(EgeQuery query) : this()
        {
            Queries.Add(query);
        }

        public EgePacket(string clientUserName, string login, string password, params EgeQuery[] queries)
            : this()
        {
            if (queries == null || queries.Length == 0)
                throw new ArgumentException("queries");
           
                ClientUserName = clientUserName;
                Login = login;
                Password = password;
             

            foreach (EgeQuery query in queries)
            {
                Queries.Add(query);
            } 
        }

        [XmlIgnore]
        public string Login { get; set; }

        [XmlIgnore]
        public string Password { get; set; }

        [XmlIgnore]
        public string ClientUserName { get; set; }

        [XmlElement("batchId", IsNullable = false)]
        public string BatchId { get; set; }

        [XmlElement("query", IsNullable = true)]
        public List<EgeQuery> Queries { get; set; }

        public override string ToString()
        {
            // ФБС так хочет пустую серию
            return XmlSerializerHelper.SerializeToString(this, Serializer)
                                      .Replace("<passportSeria />", "<passportSeria></passportSeria>");
        }

        public static EgePacket Create(string xml)
        {
            if (string.IsNullOrEmpty(xml)) throw new ArgumentNullException("xml");

            var result =
                (EgePacket) Serializer.Deserialize(XmlReader.Create(new StringReader(xml), ReaderSettings));
            return result;
        }
    }
}