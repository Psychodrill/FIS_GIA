using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Main.Dictionaries
{

    public class IdentityDocumentTypeVoc : VocabularyBase<IdentityDocumentTypeVocDto>
    {
        public IdentityDocumentTypeVoc(DataTable dataTable) : base(dataTable) { }
    }

    public class IdentityDocumentTypeVocDto : VocabularyBaseDto
    {
        public int IdentityDocumentTypeID { get; set; }
        public override int ID
        {
            get { return IdentityDocumentTypeID; }
            set { IdentityDocumentTypeID = value; }
        }
        //public string Name { get; set; }
        public bool IsRussianNationality { get; set; }
    }
}
