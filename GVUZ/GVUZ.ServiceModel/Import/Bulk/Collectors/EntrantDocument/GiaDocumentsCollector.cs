using System.Collections.Generic;
using System.Linq;
using GVUZ.ServiceModel.Import.Bulk.Collectors.Base;
using GVUZ.ServiceModel.Import.Bulk.Extensions;
using GVUZ.ServiceModel.Import.Bulk.Model;
using GVUZ.ServiceModel.Import.Bulk.Model.Base;
using GVUZ.ServiceModel.Import.WebService.Dto;

namespace GVUZ.ServiceModel.Import.Bulk.Collectors.EntrantDocument
{
    internal class GiaDocumentsCollector : BulkEntityCollectorBase<ApplicationDto, EntrantDocumentBulkEntity>
    {
        public GiaDocumentsCollector(IEnumerable<ApplicationDto> collection, int packageId, int institutionId)
            : base(collection, packageId, institutionId) {}

        public override IEnumerable<IBulkEntity> Collect()
        {
            return _collection.Where(c => c.ApplicationDocuments.GiaDocuments != null)
                    .Select(c => c.ApplicationDocuments.GiaDocuments.ToBulkEntity<EntrantDocumentBulkEntity>(_packageId, _institutionId, c.Id))
                    .Aggregate(new List<EntrantDocumentBulkEntity>(), (total, next) =>
                    {
                        total.AddRange(next);
                        return total;
                    });
        }
    }
}