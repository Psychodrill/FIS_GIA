using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace GVUZ.ServiceModel.Import.WebService.Dto
{
    
    public class DistributedAdmissionVolumeDtoCollectionDto
    {
        [XmlArray("DistributedAdmissionVolumeItems")]
        [XmlArrayItem(ElementName = "Item")]
        public DistributedAdmissionVolumeDto[] Collection;
    }

    [Description("Распределенный объем приема")]
    public class DistributedAdmissionVolumeDto : BaseDto
    {
        public string AdmissionVolumeUID;
        public string LevelBudget;

        public string NumberBudgetO;
        public string NumberBudgetOZ;
        public string NumberBudgetZ;
        //public string NumberPaidO;
        //public string NumberPaidOZ;
        //public string NumberPaidZ;
        public string NumberTargetO;
        public string NumberTargetOZ;
        public string NumberTargetZ;
        public string NumberQuotaO;
        public string NumberQuotaOZ;
        public string NumberQuotaZ;

        public string Key() { return AdmissionVolumeUID + "||" + LevelBudget; }
    }
}
