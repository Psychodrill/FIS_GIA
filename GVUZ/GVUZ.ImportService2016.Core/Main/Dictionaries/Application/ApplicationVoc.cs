using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Main.Dictionaries
{
    public class ApplicationVoc : VocabularyBase<ApplicationVocDto>
    {
        public List<ApplicationVocDto> CurrentItems;
        public ApplicationVoc(DataTable dataTable) : base(dataTable)
        {
            CurrentItems = items.Where(t => t.RegistrationDate.Year == DateTime.Now.Year).ToList();
        }
    }

    public class ApplicationVocDto : VocabularyBaseDto
    {
        public int ApplicationID { get; set; }
        public string ApplicationNumber { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int EntrantID { get; set; }
        public int StatusID { get; set; }
        public Guid? ApplicationGUID { get; set; }

        public int OrderOfAdmissionID { get; set; }
        public int OrderCompetitiveGroupItemID { get; set; }
        public int OrderCompetitiveGroupID { get; set; }
        public int OrderCompetitiveGroupTargetID { get; set; }

        public int ViolationID { get; set; }
        public override int ID
        {
            get
            {
                return ApplicationID;
            }
            set
            {
                ApplicationID = value;
            }
        }

        //public bool OriginalDocumentsReceived { get; set; }

    }
}
