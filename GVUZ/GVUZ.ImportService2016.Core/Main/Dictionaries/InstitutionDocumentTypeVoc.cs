using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Main.Dictionaries
{
    public class InstitutionDocumentTypeVoc : VocabularyBase<InstitutionDocumentTypeVocDto>
    {
        public InstitutionDocumentTypeVoc(DataTable dataTable) : base(dataTable) { }
    }

    public class InstitutionDocumentTypeVocDto : VocabularyBaseDto
    {
        public int InstitutionDocumentTypeID { get; set; }
        public override int ID
        {
            get { return InstitutionDocumentTypeID; }
            set { InstitutionDocumentTypeID = value; }
        }
        //public string Name { get; set; }
    }
}
