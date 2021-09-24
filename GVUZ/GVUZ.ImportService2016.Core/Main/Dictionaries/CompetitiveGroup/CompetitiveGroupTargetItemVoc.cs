using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Main.Dictionaries
{

    public class CompetitiveGroupTargetItemVoc : VocabularyBase<CompetitiveGroupTargetItemVocDto>
    {
        public CompetitiveGroupTargetItemVoc(DataTable dataTable) : base(dataTable) { }
    }

    public class CompetitiveGroupTargetItemVocDto : VocabularyBaseDto
    {
        public int CompetitiveGroupTargetItemID { get; set; }
        public override int ID
        {
            get { return CompetitiveGroupTargetItemID; }
            set { CompetitiveGroupTargetItemID = value; }
        }

        public int CompetitiveGroupTargetID { get; set; }
        //public int CompetitiveGroupItemID { get; set; }
        public int CompetitiveGroupID { get; set; }

        public int NumberTargetO { get; set; }
        public int NumberTargetOZ { get; set; }
        public int NumberTargetZ { get; set; }

        public string CompetitiveGroupTargetUID { get; set; }
        public string CompetitiveGroupUID { get; set; }
        //public string CompetitiveGroupItemUID { get; set; }


    }
}
