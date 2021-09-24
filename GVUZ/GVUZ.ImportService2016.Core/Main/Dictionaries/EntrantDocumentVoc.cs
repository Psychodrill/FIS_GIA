using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Main.Dictionaries
{

    public class EntrantDocumentVoc : VocabularyBase<EntrantDocumentVocDto>
    {
        public EntrantDocumentVoc(DataTable dataTable) : base(dataTable) { }

        public List<EntrantDocumentVocDto>  GetItemsByUid(string uid)
        {
            return items.Where(t => t.UID == uid).ToList();
        }
    }

    public class EntrantDocumentVocDto : VocabularyBaseDto
    {
        public int EntrantDocumentID { get; set; }
        public string EntrantUID { get; set; }
        public int EntrantID { get; set; }
        public int DocumentTypeID { get; set; }
        public Guid? EntrantDocumentGUID { get; set; }

        public override int ID
        {
            get
            {
                return EntrantDocumentID;
            }
            set
            {
                EntrantDocumentID = value;
            }
        }

        
    }
}
