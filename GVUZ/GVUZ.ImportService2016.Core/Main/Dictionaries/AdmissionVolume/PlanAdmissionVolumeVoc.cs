using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Main.Dictionaries
{
    public class PlanAdmissionVolumeVoc : VocabularyBase<PlanAdmissionVolumeVocDto>
    {
        public PlanAdmissionVolumeVoc(DataTable dataTable) : base(dataTable) { }
    }

    public class PlanAdmissionVolumeVocDto : VocabularyBaseDto
    {
        public int PlanAdmissionVolumeID { get; set; }
        public override int ID
        {
            get { return PlanAdmissionVolumeID; }
            set { PlanAdmissionVolumeID = value; }
        }

        public int DirectionID { get; set; }

        /// <summary>
        /// EducationLevelID
        /// </summary>
        public int AdmissionItemTypeID { get; set; }

        public int EducationSourceID { get; set; }

        public int EducationFormID { get; set; }

        public int CampaignID { get; set; }
        public string CampaignUID { get; set; }

        
        public int Number { get; set; }
        //public Guid? AdmissionVolumeGUID { get; set; }
    }
}
