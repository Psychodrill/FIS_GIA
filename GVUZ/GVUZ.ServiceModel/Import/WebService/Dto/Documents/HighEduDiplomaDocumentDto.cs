using System.ComponentModel;
using GVUZ.Model.Entrants.Documents;
using GVUZ.ServiceModel.Import.WebService.Dto.Documents.Base;

namespace GVUZ.ServiceModel.Import.WebService.Dto.Documents
{
    [Description("Диплом о высшем профессиональном образовании")]
    public class HighEduDiplomaDocumentDto : ApplicationDocumentDto
    {
        public override EntrantDocumentType EntrantDocumentType
        {
            get { return EntrantDocumentType.HighEduDiplomaDocument; }
        }
    }
}