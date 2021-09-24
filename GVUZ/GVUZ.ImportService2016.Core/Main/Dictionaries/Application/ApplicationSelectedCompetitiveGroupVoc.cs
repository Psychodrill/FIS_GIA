using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Main.Dictionaries.Application
{
    public class ApplicationSelectedCompetitiveGroupVoc : VocabularyBase<ApplicationSelectedCompetitiveGroupVocDto>
    {
        public ApplicationSelectedCompetitiveGroupVoc(DataTable dataTable) : base(dataTable) { }
    }

    public class ApplicationSelectedCompetitiveGroupVocDto : VocabularyBaseDto
    {
        public int ApplicationID { get; set; }
        public int CompetitiveGroupID { get; set; }
        public int ItemID { get; set; }
        public override int ID
        {
            get { return ItemID; }
            set { ItemID = value; }
        }
    }
}
