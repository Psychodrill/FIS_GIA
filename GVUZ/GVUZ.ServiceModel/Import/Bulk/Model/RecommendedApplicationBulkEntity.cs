using System;
using GVUZ.ServiceModel.Import.Bulk.Attributes;

namespace GVUZ.ServiceModel.Import.Bulk.Model
{
    [DestinationTableName("blk_RecommendedApplication")]
    public class RecommendedApplicationBulkEntity : ConsideredApplicationBulkEntity
    {
        public int? Stage { get; set; }
    }
}
