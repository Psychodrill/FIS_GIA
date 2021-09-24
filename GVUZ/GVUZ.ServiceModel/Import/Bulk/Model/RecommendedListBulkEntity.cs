using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GVUZ.ServiceModel.Import.Bulk.Model.Base;
using GVUZ.ServiceModel.Import.Bulk.Attributes;

namespace GVUZ.ServiceModel.Import.Bulk.Model
{
    [DestinationTableName("blk_RecommendedList")]
    public class RecommendedListBulkEntity : BulkEntityBase
    {
        public int Stage { get; set; }
        public string ApplicationNumber { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int EduLevelId { get; set; }
        public int EduFormId { get; set; }
        public int DirectionId { get; set; }
        public string CompetitiveGroupUID { get; set; }
    }
}
