using System.Collections.Generic;
using System.Linq;
using GVUZ.ServiceModel.Import.Bulk.Collectors.Base;
using GVUZ.ServiceModel.Import.Bulk.Extensions;
using GVUZ.ServiceModel.Import.Bulk.Model;
using GVUZ.ServiceModel.Import.Bulk.Model.Base;
using GVUZ.ServiceModel.Import.WebService.Dto;

namespace GVUZ.ServiceModel.Import.Bulk.Collectors.EntrantDocument
{
    internal class EntranceTestResultsCollector : BulkEntityCollectorBase<ApplicationDto, EntrantDocumentBulkEntity>
    {
        public EntranceTestResultsCollector(IEnumerable<ApplicationDto> collection, int packageId, int institutionId)
            : base(collection, packageId, institutionId)
        {
        }

        public override IEnumerable<IBulkEntity> Collect()
        {
            return _collection.Where(c => c.EntranceTestResults != null)
                .Select(c => c.EntranceTestResults
                .Aggregate(new List<EntrantDocumentBulkEntity>(), (total, next) =>
                {
                    if (next.ResultDocument != null && next.ResultDocument.OlympicDocument != null)
                    {
                        var document = next.ResultDocument.OlympicDocument.ToBulkEntity<EntrantDocumentBulkEntity>(_packageId, _institutionId, c.Id);
                        document.EntranceTestResultId = next.Id;
                        total.Add(document);
                    }
                    if (next.ResultDocument != null &&
                        next.ResultDocument.OlympicTotalDocument != null)
                    {
                        var document = next.ResultDocument.OlympicTotalDocument.ToBulkEntity<EntrantDocumentBulkEntity>(_packageId, _institutionId, c.Id);
                        document.EntranceTestResultId = next.Id;
                        total.Add(document);
                    }
                    return total;
                }))
                .Aggregate(new List<EntrantDocumentBulkEntity>(), (total, next) =>
                {
                    total.AddRange(next);
                    return total;
                });
        }
    }
}