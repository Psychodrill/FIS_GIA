using System.Xml.Serialization;

namespace upload.Models
{
    [XmlRoot(ElementName = "Enrollee")]
    public class EnrolleeXML
    {
        [XmlElement("UID", Order = 1)]
        public string UID { get; set; }

        [XmlArray(ElementName = "Files", Order = 2)]
        [XmlArrayItem(ElementName = "File")]
        public FilesXML[] Files;
    }
}