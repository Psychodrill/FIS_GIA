using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Main.Dictionaries
{
    public class AdmissionVolumeVoc : VocabularyBase<AdmissionVolumeVocDto>
    {
        public AdmissionVolumeVoc(DataTable dataTable) : base(dataTable) { }
    }

    public class AdmissionVolumeVocDto : VocabularyBaseDto
    {
        public int AdmissionVolumeID { get; set; }
        public override int ID
        {
            get { return AdmissionVolumeID; }
            set { AdmissionVolumeID = value; }
        }

        public int DirectionID { get; set; }

        public int ParentDirectionID { get; set; }

        /// <summary>
        /// EducationLevelID
        /// </summary>
        public int AdmissionItemTypeID { get; set; }

        [Obsolete]
        public int Course { get; set; }
        public int CampaignID { get; set; }
        public string CampaignUID { get; set; }

        public int NumberBudgetO { get; set; }
        public int NumberBudgetOZ { get; set; }
        public int NumberBudgetZ { get; set; }

        public int NumberPaidO { get; set; }
        public int NumberPaidOZ { get; set; }
        public int NumberPaidZ { get; set; }

        public int NumberQuotaO { get; set; }
        public int NumberQuotaOZ { get; set; }
        public int NumberQuotaZ { get; set; }

        public int NumberTargetO { get; set; }
        public int NumberTargetOZ { get; set; }
        public int NumberTargetZ { get; set; }
    }
}
