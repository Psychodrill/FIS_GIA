using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Main.Dictionaries.Application
{
    public class IndividualAchivementVoc : VocabularyBase<IndividualAchivementVocDto>
    {
    public IndividualAchivementVoc(DataTable dataTable) : base(dataTable) { }
    }

    public class IndividualAchivementVocDto : VocabularyBaseDto
    {
        public int IAID { get; set; }
        public override int ID
        {
            get { return IAID; }
            set { IAID = value; }
        }

        public int ApplicationID { get; set; }
        public string IAUID { get; set; }
        public string IAName { get; set; }
        public decimal IAMark { get; set; }
        public int EntrantDocumentID { get; set; }
        public int IdAchievement { get; set; }
        public bool? isAdvantageRightField { get; set; }
    }
}