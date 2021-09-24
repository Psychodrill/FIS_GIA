using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GVUZ.ServiceModel.Import.WebService.Dto.Documents.Base;

namespace GVUZ.ServiceModel.Import.WebService.Dto.Documents
{
    public class PostGraduateDiplomaDocumentDto : ApplicationDocumentDto
    {
        public override Model.Entrants.Documents.EntrantDocumentType EntrantDocumentType
        {
            get { return Model.Entrants.Documents.EntrantDocumentType.PostGraduateDiplomaDocument; }
        }
    }
}
