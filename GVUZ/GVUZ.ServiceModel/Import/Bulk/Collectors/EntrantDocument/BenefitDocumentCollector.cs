using System.Collections.Generic;
using System.Linq;
using GVUZ.ServiceModel.Import.Bulk.Collectors.Base;
using GVUZ.ServiceModel.Import.Bulk.Extensions;
using GVUZ.ServiceModel.Import.Bulk.Model;
using GVUZ.ServiceModel.Import.Bulk.Model.Base;
using GVUZ.ServiceModel.Import.WebService.Dto;
using GVUZ.ServiceModel.Import.WebService.Dto.Documents.Base;

namespace GVUZ.ServiceModel.Import.Bulk.Collectors.EntrantDocument
{
    internal class BenefitDocumentCollector : BulkEntityCollectorBase<ApplicationDto, EntrantDocumentBulkEntity>
    {
        public BenefitDocumentCollector(IEnumerable<ApplicationDto> collection, int packageId, int institutionId)
            : base(collection, packageId, institutionId) {}

        public override IEnumerable<IBulkEntity> Collect()
        {
            return _collection.Where(c => c.ApplicationCommonBenefits != null || c.ApplicationCommonBenefit != null)
                    .Select(c => c.GetCommonBenefits()
                    .Aggregate(new List<ApplicationDocumentDto>(), (t, n) =>
                    {
                        if (n.DocumentReason != null)
                        {
                            if (n.DocumentReason.OlympicDocument != null) t.Add(n.DocumentReason.OlympicDocument);
                            if (n.DocumentReason.OlympicTotalDocument != null) t.Add(n.DocumentReason.OlympicTotalDocument);
                            if (n.DocumentReason.CustomDocument != null) t.Add(n.DocumentReason.CustomDocument);
                            if (n.DocumentReason.MedicalDocuments != null) t.AddRange(n.DocumentReason.MedicalDocuments.GetDocuments());
                        }
                        return t;
                    })
                    .Select(x => x.ToBulkEntity<EntrantDocumentBulkEntity>(_packageId, _institutionId, c.Id)))
                    .Aggregate(new List<EntrantDocumentBulkEntity>(), (total, next) =>
                    {
                        total.AddRange(next);
                        return total;
                    });
        }
    }
}