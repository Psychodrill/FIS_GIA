using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GVUZ.ServiceModel.Import.WebService.Dto.Documents.Base;
using GVUZ.Model.Entrants.Documents;

namespace GVUZ.ServiceModel.Import.WebService.Dto.Documents
{
    public class PhDDiplomaDocumentDto : ApplicationDocumentDto
    {
        public override EntrantDocumentType EntrantDocumentType
        {
            get { return Model.Entrants.Documents.EntrantDocumentType.PhDDiplomaDocument; }
        }
    }
}
