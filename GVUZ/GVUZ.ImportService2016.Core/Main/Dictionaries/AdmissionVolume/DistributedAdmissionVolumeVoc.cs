using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Main.Dictionaries
{
    public class DistributedAdmissionVolumeVoc : VocabularyBase<DistributedAdmissionVolumeVocDto>
    {
        public DistributedAdmissionVolumeVoc(DataTable dataTable) : base(dataTable) { }
    }

    public class DistributedAdmissionVolumeVocDto : VocabularyBaseDto
    {
        public int DistributedAdmissionVolumeID { get; set; }
        public override int ID
        {
            get { return DistributedAdmissionVolumeID; }
            set { DistributedAdmissionVolumeID = value; }
        }

        public int IdLevelBudget { get; set; }
        public int AdmissionVolumeID { get; set; }
        public string AdmissionVolumeUID { get; set; }

        public int NumberBudgetO { get; set; }
        public int NumberBudgetOZ { get; set; }
        public int NumberBudgetZ { get; set; }

        public int NumberQuotaO { get; set; }
        public int NumberQuotaOZ { get; set; }
        public int NumberQuotaZ { get; set; }

        public int NumberTargetO { get; set; }
        public int NumberTargetOZ { get; set; }
        public int NumberTargetZ { get; set; }

    }
}
