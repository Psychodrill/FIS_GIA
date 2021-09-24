using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Main.Dictionaries.Application
{
    public class ApplicationCompetitiveGroupItemVoc : VocabularyBase<ApplicationCompetitiveGroupItemVocDto>
    {
        public ApplicationCompetitiveGroupItemVoc(DataTable dataTable) : base(dataTable) { }
    }

    public class ApplicationCompetitiveGroupItemVocDto : VocabularyBaseDto
    {
        public int ApplicationId { get; set; }
        public int CompetitiveGroupId { get; set; }
        //public int CompetitiveGroupItemId { get; set; }
        public int CompetitiveGroupTargetId { get; set; }

        //public int EducationFormId { get; set; }
        //public int EducationSourceId { get; set; }
        //public int Priority { get; set; }

        //public int Course { get; set; }
        //public int CampaignID { get; set; }
        //public int DirectionID { get; set; }
        //public int EducationLevelID { get; set; }

        //public string CompetitiveGroupUID { get; set; }
        //public int CampaignStatusID { get; set; }

        public bool IsAgreed { get; set; }
        public bool IsDisagreed { get; set; }
        public bool IsForSPOandVO { get; set; }
        public DateTime IsAgreedDate { get; set; }
        public DateTime IsDisagreedDate { get; set; }
        public decimal CalculatedRating { get; set; }
        public int OrderOfAdmissionID { get; set; }
        public int OrderOfExceptionID { get; set; }
        public DateTime AdmissionDate { get; set; }
        public DateTime ExceptionDate { get; set; }
    }
}
