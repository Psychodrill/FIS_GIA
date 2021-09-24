using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GVUZ.ServiceModel.Import.Bulk.Attributes;
using GVUZ.ServiceModel.Import.Bulk.Model.Base;

namespace GVUZ.ServiceModel.Import.Bulk.Model
{
    [DestinationTableName("blk_ApplicationSelectedCompetitiveGroupTarget")]
    public class AppSelectedCompGroupTargetBulkEntity : BulkEntityBase
    {
        public string TargetOrganizationUID { get; set; }
        public bool IsForO { get; set; }
        public bool IsForOZ { get; set; }
        public bool IsForZ { get; set; }
    }
}
