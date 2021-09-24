using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GVUZ.ServiceModel.Import.Bulk.Model.Base;
using GVUZ.ServiceModel.Import.Bulk.Attributes;

namespace GVUZ.ServiceModel.Import.Bulk.Model
{
    [DestinationTableName("blk_ApplicationIndividualAchievements")]
    public class IndividualAchivementBulkEntity : BulkEntityBase
    {
        public string ApplicationUID { get; set; }
        public string IAUID { get; set; }
        public string IAName { get; set; }
        public decimal? IAMark { get; set; }
        public string EntrantDocumentUID { get; set; }
        public bool? isAdvantageRight { get; set; }
    }
}
