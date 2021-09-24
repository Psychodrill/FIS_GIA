using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Main.Dictionaries.Application
{
    public class ApplicationEntrantDocumentVoc : VocabularyBase<ApplicationEntrantDocumentVocDto>
    {
        public ApplicationEntrantDocumentVoc(DataTable dataTable) : base(dataTable) { }
    }

    public class ApplicationEntrantDocumentVocDto : VocabularyBaseDto
    {
        public int ApplicationID { get; set; }
        public int EntrantDocumentID { get; set; }
    }
}
