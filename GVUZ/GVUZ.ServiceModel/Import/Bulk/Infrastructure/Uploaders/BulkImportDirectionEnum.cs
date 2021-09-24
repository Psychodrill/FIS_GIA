using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GVUZ.ServiceModel.Import.Bulk.Attributes;

namespace GVUZ.ServiceModel.Import.Bulk.Infrastructure.Uploaders
{
    public enum BulkImportDirection
    {
        InstitutionCampaign,
        InstitutionStructure,

        /// <summary>
        /// Загрузка заявлений
        /// </summary>
        [BulkedProcessQuery("EXEC blk_ProcessApplicationBulkedPackage @packageId, @userLogin")]
        Applications,

        /// <summary>
        /// Загрука рассматриваемых заявлений
        /// </summary>
        [BulkedProcessQuery("EXEC blk_ProcessConsideredApplicationBulkedPackage @packageId, @userLogin")]
        ConsideredApplications,

        /// <summary>
        /// Загрука рассматриваемых заявлений
        /// </summary>
        [BulkedProcessQuery("EXEC blk_ProcessRecommendedApplicationBulkedPackage @packageId, @userLogin")]
        RecommendedApplications,

        Orders,

        [BulkedProcessQuery("Exec blk_ProcessRecommendedLists @packageId, @userLogin")]
        RecommendedLists
    }
}
