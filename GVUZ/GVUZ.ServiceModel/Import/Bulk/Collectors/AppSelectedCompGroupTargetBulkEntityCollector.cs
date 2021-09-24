using System;
using System.Collections.Generic;
using System.Linq;
using GVUZ.Model.Institutions;
using GVUZ.ServiceModel.Import.Bulk.Collectors.Base;
using GVUZ.ServiceModel.Import.Bulk.Extensions;
using GVUZ.ServiceModel.Import.Bulk.Model;
using GVUZ.ServiceModel.Import.Bulk.Model.Base;
using GVUZ.ServiceModel.Import.WebService.Dto;

namespace GVUZ.ServiceModel.Import.Bulk.Collectors
{
    public class AppSelectedCompGroupTargetBulkEntityCollector : BulkEntityCollectorBase<ApplicationDto, AppSelectedCompGroupTargetBulkEntity>
    {
        public AppSelectedCompGroupTargetBulkEntityCollector(IEnumerable<ApplicationDto> collection, int packageId, int institutionId) :
            base(collection, packageId, institutionId) { }

        public override IEnumerable<IBulkEntity> Collect()
        {
            return _collection.Where(c => c.FinSourceAndEduForms != null)
                .Select(c => c.FinSourceAndEduForms.Where(x => x.FinanceSourceIdInt == EDSourceConst.Target)
                    .ToBulkEntity<AppSelectedCompGroupTargetBulkEntity>(_packageId, _institutionId, c.Id))
                .Aggregate(new List<AppSelectedCompGroupTargetBulkEntity>(), (total, next) =>
                {
                    total.AddRange(next);
                    return total;
                });
        }
    }
}
