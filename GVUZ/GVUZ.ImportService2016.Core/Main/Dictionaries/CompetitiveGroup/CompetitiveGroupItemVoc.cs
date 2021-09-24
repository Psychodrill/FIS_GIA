using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Main.Dictionaries
{
    public class CompetitiveGroupItemVoc : VocabularyBase<CompetitiveGroupItemVocDto>
    {
        public CompetitiveGroupItemVoc(DataTable dataTable) : base(dataTable) { }
    }

    public class CompetitiveGroupItemVocDto : VocabularyBaseDto
    {
        public int CompetitiveGroupItemID { get; set; }
        public int CompetitiveGroupID { get; set; }
        public override int ID
        {
            get { return CompetitiveGroupItemID; }
            set { CompetitiveGroupItemID = value; }
        }

        public int NumberBudgetO { get; set; }
        public int NumberBudgetOZ { get; set; }
        public int NumberBudgetZ { get; set; }
        public int NumberPaidO { get; set; }
        public int NumberPaidOZ { get; set; }
        public int NumberPaidZ { get; set; }
        public int NumberQuotaO { get; set; }
        public int NumberQuotaOZ { get; set; }
        public int NumberQuotaZ { get; set; }

        public int NumberTargetO { get; set; }
        public int NumberTargetOZ { get; set; }
        public int NumberTargetZ { get; set; }

        public int Course { get; set; }

        public string CompetitiveGroupUID { get; set; }
    }
}
