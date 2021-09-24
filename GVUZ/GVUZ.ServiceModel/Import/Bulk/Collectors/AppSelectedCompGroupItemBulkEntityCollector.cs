using System;
using System.Collections.Generic;
using System.Linq;
using GVUZ.ServiceModel.Import.Bulk.Collectors.Base;
using GVUZ.ServiceModel.Import.Bulk.Model;
using GVUZ.ServiceModel.Import.Bulk.Model.Base;
using GVUZ.ServiceModel.Import.WebService.Dto;

namespace GVUZ.ServiceModel.Import.Bulk.Collectors
{
    public class AppSelectedCompGroupItemBulkEntityCollector : BulkEntityCollectorBase<ApplicationDto, AppSelectedCompGroupItemBulkEntity>
    {
        public AppSelectedCompGroupItemBulkEntityCollector(IEnumerable<ApplicationDto> collection, int packageId, int institutionId)
            : base(collection, packageId, institutionId) { }

        public override IEnumerable<IBulkEntity> Collect()
        {
            return _collection
                    .Aggregate(new List<AppSelectedCompGroupItemBulkEntity>(), (total, next) =>
                    {
                        total.AddRange(next.SelectedCompetitiveGroupItems.Select(x => new AppSelectedCompGroupItemBulkEntity
                        {
                            ParentId = next.Id,
                            UID = x,
                            ImportPackageId = _packageId,
                            InstitutionId = _institutionId
                        }));
                        return total;
                    });
        }
    }
}
