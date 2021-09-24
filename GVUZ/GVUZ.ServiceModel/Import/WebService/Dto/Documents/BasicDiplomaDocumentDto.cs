using System.ComponentModel;
using GVUZ.Model.Entrants.Documents;
using GVUZ.ServiceModel.Import.WebService.Dto.Documents.Base;

namespace GVUZ.ServiceModel.Import.WebService.Dto.Documents
{
    [Description("Диплом о начальном профессиональном образовании")]
    public class BasicDiplomaDocumentDto : ApplicationDocumentDto
    {
        public override EntrantDocumentType EntrantDocumentType
        {
            get { return EntrantDocumentType.BasicDiplomaDocument; }
        }
    }
}