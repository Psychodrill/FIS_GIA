using System.Collections.Generic;
using System.Linq;
using GVUZ.ServiceModel.Import.Bulk.Collectors.Base;
using GVUZ.ServiceModel.Import.Bulk.Extensions;
using GVUZ.ServiceModel.Import.Bulk.Model;
using GVUZ.ServiceModel.Import.Bulk.Model.Base;
using GVUZ.ServiceModel.Import.WebService.Dto;

namespace GVUZ.ServiceModel.Import.Bulk.Collectors
{
    public class AppEntranceTestDocumentBulkEntityCollector : BulkEntityCollectorBase<ApplicationDto, AppEntranceTestDocumentBulkEntity>
    {
        public AppEntranceTestDocumentBulkEntityCollector(IEnumerable<ApplicationDto> collection, int packageId, int institutionId) :
            base(collection, packageId, institutionId) { }

        public override IEnumerable<IBulkEntity> Collect()
        {
            var documents = new List<AppEntranceTestDocumentBulkEntity>();
            documents.AddRange(_collection.Where(c => c.EntranceTestResults != null)
                .Select(c => c.EntranceTestResults.ToBulkEntity<AppEntranceTestDocumentBulkEntity>(_packageId, _institutionId, c.Id))
                .Aggregate(new List<AppEntranceTestDocumentBulkEntity>(), (total, next) =>
                {
                    total.AddRange(next);
                    return total;
                }));

            documents.AddRange(_collection.Where(c => c.ApplicationCommonBenefit != null || c.ApplicationCommonBenefits != null)
                .Select(c => c.GetCommonBenefits().ToBulkEntity<AppEntranceTestDocumentBulkEntity>(_packageId, _institutionId, c.Id))
                .Aggregate(new List<AppEntranceTestDocumentBulkEntity>(), (total, next) =>
                {
                    total.AddRange(next);
                    return total;
                }));
            return documents;
        }
    }
}
