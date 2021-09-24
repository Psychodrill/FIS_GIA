using System.Collections.Generic;
using GVUZ.ServiceModel.Import.WebService.Dto.Documents.Base;

namespace GVUZ.ServiceModel.Import.WebService.Dto.Documents.Collections
{
    public class MedicalDocumentsDto
    {
        public AllowEducationDocumentDto AllowEducationDocument;
        public BenefitDocumentsDto BenefitDocument;

        public List<ApplicationDocumentDto> GetDocuments()
        {
            var docList = new List<ApplicationDocumentDto>
                {
                    AllowEducationDocument
                };

            if (BenefitDocument != null)
                docList.AddRange(BenefitDocument.GetDocuments());

            return docList;
        }
    }
}