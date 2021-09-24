using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Main.Dictionaries
{
    public class CompetitiveGroupTargetVoc : VocabularyBase<CompetitiveGroupTargetVocDto>
    {
        public CompetitiveGroupTargetVoc(DataTable dataTable) : base(dataTable) { }
    }

    public class CompetitiveGroupTargetVocDto : VocabularyBaseDto
    {
        public int CompetitiveGroupTargetID { get; set; }
        public override int ID
        {
            get { return CompetitiveGroupTargetID; }
            set { CompetitiveGroupTargetID = value; }
        }
    }
}
