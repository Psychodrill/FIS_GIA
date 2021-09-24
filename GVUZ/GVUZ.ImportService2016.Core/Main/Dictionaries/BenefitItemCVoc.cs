using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Main.Dictionaries
{

    public class BenefitItemCVoc : VocabularyBase<BenefitItemCVocDto>
    {
        public BenefitItemCVoc(DataTable dataTable) : base(dataTable) { }
    }

    public class BenefitItemCVocDto : VocabularyBaseDto
    {
        public int BenefitItemID { get; set; }
        public int EntranceTestItemID { get; set; }
        public int OlympicLevelFlags { get; set; }
        public int BenefitID { get; set; }
        public bool IsForAllOlympic { get; set; }
        public bool IsProfileSubject { get; set; }
        public int CompetitiveGroupID { get; set; }
        public int OlympicYear { get; set; }
        public int? EgeMinValue { get; set; }
        public string CompetitiveGroupUID { get; set; }
        public string EntranceTestItemUID { get; set; }

        public bool IsCreative { get; set; }
        public bool IsAthletic { get; set; }

        public override int ID
        {
            get { return BenefitItemID; }
            set { BenefitItemID = value; }
        }
    }
}
