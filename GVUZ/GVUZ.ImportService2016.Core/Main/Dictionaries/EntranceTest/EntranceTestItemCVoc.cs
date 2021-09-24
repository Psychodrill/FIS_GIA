using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Main.Dictionaries.EntranceTest
{
    public class EntranceTestItemCVoc : VocabularyBase<EntranceTestItemCVocDto>
    {
        public EntranceTestItemCVoc(DataTable dataTable) : base(dataTable) { }



        //public int? GetIdBySubjectId(int subjectId)
        //{
        //    var item = this.items.FirstOrDefault(t => t.SubjectID == subjectId);
        //    return item == null ? null : (int?)item.EntranceTestItemID;
        //}

        //public int? GetIdBySubjectName(string subjectName)
        //{
        //    var item = this.items.FirstOrDefault(t => t.SubjectName == subjectName);
        //    return item == null ? null : (int?)item.EntranceTestItemID;
        //}
    }

    public class EntranceTestItemCVocDto : VocabularyBaseDto 
    {
        public int EntranceTestItemID { get; set; }
        public int CompetitiveGroupID { get; set; }
        public int EntranceTestTypeID { get; set; }
        public int SubjectID { get; set; }
        public string SubjectName { get; set; }
        public int EntranceTestPriority {get; set;}
        public string CompetitiveGroupUID { get; set; }

        public bool IsForSPOandVO { get; set; }
        public int ReplacedEntranceTestItemID { get; set; }
        public bool IsFirst { get; set; }
        public bool IsSecond { get; set; }


        public override int ID
        {
            get { return EntranceTestItemID; }
            set { EntranceTestItemID = value; }
        }
    }
}
