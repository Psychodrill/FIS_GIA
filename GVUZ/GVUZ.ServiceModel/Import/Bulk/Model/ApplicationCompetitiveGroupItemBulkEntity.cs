using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GVUZ.ServiceModel.Import.Bulk.Model.Base;
using GVUZ.ServiceModel.Import.Bulk.Attributes;

namespace GVUZ.ServiceModel.Import.Bulk.Model
{
    [DestinationTableName("blk_ApplicationCompetitiveGroupItem")]
    public class ApplicationCompetitiveGroupItemBulkEntity : BulkEntityBase
    {
        public string ApplicationUID { get; set; }
        public string CompetitiveGroupUID { get; set; }
        public string CompetitiveGroupItemUID { get; set; }
        public int EducationForm { get; set; }
        public int EducationSource { get; set; }
        public int? Priority { get; set; }
        public string CompetitiveGroupTargetUID { get; set; }
    }
}
