using System.Collections.Generic;
using System.Linq;
using GVUZ.ServiceModel.Import.Bulk.Collectors.Base;
using GVUZ.ServiceModel.Import.Bulk.Extensions;
using GVUZ.ServiceModel.Import.Bulk.Model;
using GVUZ.ServiceModel.Import.Bulk.Model.Base;
using GVUZ.ServiceModel.Import.WebService.Dto;

namespace GVUZ.ServiceModel.Import.Bulk.Collectors.EntrantDocument
{
    internal class MilitaryCardDocumentCollector : BulkEntityCollectorBase<ApplicationDto, EntrantDocumentBulkEntity>
    {
        public MilitaryCardDocumentCollector(IEnumerable<ApplicationDto> collection, int packageId, int institutionId)
            : base(collection, packageId, institutionId) {}

        public override IEnumerable<IBulkEntity> Collect()
        {
            return _collection.Where(c => c.ApplicationDocuments.MilitaryCardDocument != null)
                    .Select(c => c.ApplicationDocuments.MilitaryCardDocument.ToBulkEntity<EntrantDocumentBulkEntity>(_packageId, _institutionId, c.Id));
        }
    }
}