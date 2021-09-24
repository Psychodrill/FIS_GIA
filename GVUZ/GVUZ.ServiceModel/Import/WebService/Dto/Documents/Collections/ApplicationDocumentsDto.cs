using System.Xml.Serialization;

namespace GVUZ.ServiceModel.Import.WebService.Dto.Documents.Collections
{
    public class ApplicationDocumentsDto : BaseDto
    {
        [XmlArrayItem(ElementName = "CustomDocument")] public CustomDocumentDto[] CustomDocuments;
        [XmlArrayItem(ElementName = "EduDocument")] public EduDocumentsDto[] EduDocuments;
        [XmlArrayItem(ElementName = "EgeDocument")] public EgeDocumentWithSubjectsDto[] EgeDocuments;

        [XmlArrayItem(ElementName = "GiaDocument")] public GiaDocumentWithSubjectsDto[] GiaDocuments;

        public IdentityDocumentDto IdentityDocument;
        public MilitaryCardDocumentDto MilitaryCardDocument;
        public StudentDocumentDto StudentDocument;
    }
}