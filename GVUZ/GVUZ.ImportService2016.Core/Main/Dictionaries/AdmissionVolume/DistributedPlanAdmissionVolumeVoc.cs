using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Main.Dictionaries
{
    public class DistributedPlanAdmissionVolumeVoc : VocabularyBase<DistributedPlanAdmissionVolumeVocDto>
    {
        public DistributedPlanAdmissionVolumeVoc(DataTable dataTable) : base(dataTable) { }
    }

    public class DistributedPlanAdmissionVolumeVocDto : VocabularyBaseDto
    {
        public int DistributedPlanAdmissionVolumeID { get; set; }
        public override int ID
        {
            get { return DistributedPlanAdmissionVolumeID; }
            set { DistributedPlanAdmissionVolumeID = value; }
        }

        public int IdLevelBudget { get; set; }
        public int PlanAdmissionVolumeID { get; set; }
        public string PlanAdmissionVolumeUID { get; set; }

        public int Number { get; set; }

        public int EducationSourceID { get; set; }
        public int EducationFormID { get; set; }
    }
}
