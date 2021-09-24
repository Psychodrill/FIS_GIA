using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Main.Dictionaries
{
    public class BenefitVoc : VocabularyBase<BenefitVocDto>
    {
        public BenefitVoc(DataTable dataTable) : base(dataTable) { }
    }

    public class BenefitVocDto : VocabularyBaseDto
    {
        public int BenefitID { get; set; }
        public string ShortName { get; set; }

        public override int ID
        {
            get { return BenefitID; }
            set { BenefitID = value; }
        }
    }
}
