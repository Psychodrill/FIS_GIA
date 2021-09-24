using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Main.Dictionaries
{
    public class EntrantVoc : VocabularyBase<EntrantVocDto>
    {
        public EntrantVoc(DataTable dataTable) : base(dataTable) { }
    }

    public class EntrantVocDto : VocabularyBaseDto
    {
        public int EntrantID { get; set; }

        public override int ID
        {
            get
            {
                return EntrantID;
            }
            set
            {
                EntrantID = value;
            }
        }

        public Guid? EntrantGUID { get; set; }
    }
}
