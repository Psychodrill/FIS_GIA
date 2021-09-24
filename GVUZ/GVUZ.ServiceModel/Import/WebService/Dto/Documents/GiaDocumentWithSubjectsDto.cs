using System.ComponentModel;
using System.Xml.Serialization;
using GVUZ.Model.Entrants.Documents;
using GVUZ.ServiceModel.Import.WebService.Dto.Documents.Base;

namespace GVUZ.ServiceModel.Import.WebService.Dto.Documents
{
    [Description("Справка ГИА")]
    public class GiaDocumentWithSubjectsDto : BaseDocumentDto
    {
        [XmlArrayItem(ElementName = "SubjectData")] public SubjectDataDto[] Subjects;

        public override EntrantDocumentType EntrantDocumentType
        {
            get { return EntrantDocumentType.GiaDocument; }
        }
    }
}