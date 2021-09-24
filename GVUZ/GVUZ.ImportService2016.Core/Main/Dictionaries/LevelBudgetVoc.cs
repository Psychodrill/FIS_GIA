using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Main.Dictionaries
{
    public class LevelBudgetVoc : VocabularyBase<LevelBudgetVocDto>
    {
     public LevelBudgetVoc(DataTable dataTable) : base(dataTable) { }
    }

    public class LevelBudgetVocDto : VocabularyBaseDto
    {
        public int IdLevelBudget { get; set; }
        public string BudgetName { get; set; }

        public override int ID
        {
            get { return IdLevelBudget; }
            set { IdLevelBudget = value; }
        }
    }
}
