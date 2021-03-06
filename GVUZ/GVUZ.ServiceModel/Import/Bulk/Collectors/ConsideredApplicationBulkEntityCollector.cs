using System.Collections.Generic;
using System.Linq;
using GVUZ.ServiceModel.Import.Bulk.Collectors.Base;
using GVUZ.ServiceModel.Import.Bulk.Extensions;
using GVUZ.ServiceModel.Import.Bulk.Model;
using GVUZ.ServiceModel.Import.Bulk.Model.Base;
using GVUZ.ServiceModel.Import.WebService.Dto;

namespace GVUZ.ServiceModel.Import.Bulk.Collectors
{
    public class ConsideredApplicationBulkEntityCollector : BulkEntityCollectorBase<ConsideredApplicationDto, ConsideredApplicationBulkEntity>
    {
        public ConsideredApplicationBulkEntityCollector(IEnumerable<ConsideredApplicationDto> collection, int packageId, int institutionId) :
            base(collection, packageId, institutionId) { }

        public override IEnumerable<IBulkEntity> Collect()
        {
            return _collection.ToBulkEntity<ConsideredApplicationBulkEntity>(_packageId, _institutionId).Distinct();
        }
    }
}
