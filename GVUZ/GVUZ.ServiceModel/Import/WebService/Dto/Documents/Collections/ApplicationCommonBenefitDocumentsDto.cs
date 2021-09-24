using System.Collections.Generic;
using GVUZ.ServiceModel.Import.WebService.Dto.Documents.Base;

namespace GVUZ.ServiceModel.Import.WebService.Dto.Documents.Collections
{
    public class ApplicationCommonBenefitDocumentsDto
    {
        public CustomDocumentDto CustomDocument;
        public MedicalDocumentsDto MedicalDocuments;
        public OlympicDocumentDto OlympicDocument;
        public OlympicTotalDocumentDto OlympicTotalDocument;

        public List<ApplicationDocumentDto> GetDocuments()
        {
            var docList = new List<ApplicationDocumentDto>
                {
                    OlympicDocument,
                    OlympicTotalDocument,
                    CustomDocument
                };
            if (MedicalDocuments != null)
                docList.AddRange(MedicalDocuments.GetDocuments());
            return docList;
        }
    }
}