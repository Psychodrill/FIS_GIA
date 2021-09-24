using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Main.Dictionaries
{
    public class IndividualAchievementsCategoryVoc : VocabularyBase<IndividualAchievementsCategoryVocDto>
    {
        public IndividualAchievementsCategoryVoc(DataTable dataTable) : base(dataTable) { }
    }

    public class IndividualAchievementsCategoryVocDto : VocabularyBaseDto
    {
        public int IdCategory { get; set; }
        public override int ID
        {
            get { return IdCategory; }
            set { IdCategory = value; }
        }

        public string CategoryName { get; set; }
    }
}
