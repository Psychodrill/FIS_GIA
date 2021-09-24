using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GVUZ.ServiceModel.Import.Bulk.Attributes;
using GVUZ.ServiceModel.Import.Bulk.Model.Base;

namespace GVUZ.ServiceModel.Import.Bulk.Model
{
    [DestinationTableName("blk_ApplicationSelectedCompetitiveGroup")]
    public class AppSelectedCompGroupBulkEntity : BulkEntityBase
    {
        public decimal? CalculatedRating { get; set; }
        public int? CalculatedBenefitId { get; set; }
    }
}
