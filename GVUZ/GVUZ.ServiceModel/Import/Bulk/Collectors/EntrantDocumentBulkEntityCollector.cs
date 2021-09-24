using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FogSoft.Helpers;
using GVUZ.ServiceModel.Import.Bulk.Collectors.Base;
using GVUZ.ServiceModel.Import.Bulk.Collectors.EntrantDocument;
using GVUZ.ServiceModel.Import.Bulk.Infrastructure;
using GVUZ.ServiceModel.Import.Bulk.Model;
using GVUZ.ServiceModel.Import.Bulk.Model.Base;
using GVUZ.ServiceModel.Import.WebService.Dto;

namespace GVUZ.ServiceModel.Import.Bulk.Collectors
{
    public class EntrantDocumentBulkEntityCollector : BulkEntityCollectorBase<ApplicationDto, EntrantDocumentBulkEntity>
    {
        public BulkEntityCollectorList EntrantDocumentCollectors;
        public EntrantDocumentBulkEntityCollector(IEnumerable<ApplicationDto> collection, int packageId, int institutionId)
            : base(collection, packageId, institutionId)
        {
            EntrantDocumentCollectors = new BulkEntityCollectorList
                                            {
                                                new EduDocumentsCollector(collection, packageId, institutionId),
                                                new EntranceTestResultsCollector(collection, packageId, institutionId),
                                                new CustomDocumentsCollector(collection, packageId, institutionId),
                                                new EgeDocumentsCollector(collection, packageId, institutionId),
                                                new GiaDocumentsCollector(collection, packageId, institutionId),
                                                new IdentityDocumentCollector(collection, packageId, institutionId),
                                                new MilitaryCardDocumentCollector(collection, packageId, institutionId),
                                                new StudentDocumentCollector(collection, packageId, institutionId),
                                                new BenefitDocumentCollector(collection, packageId, institutionId)
                                            };
        }

        public override IEnumerable<IBulkEntity> Collect()
        {
            var sw = new Stopwatch();
            sw.Start();

            var documents = new List<EntrantDocumentBulkEntity>();
            var items = EntrantDocumentCollectors.AsParallel().Select(c => c.Collect());
            documents.AddRange(
                items.Aggregate(new List<EntrantDocumentBulkEntity>(), (total, next) =>
                {
                    total.AddRange(next.OfType<EntrantDocumentBulkEntity>());
                    return total;
                }));            

            sw.Stop();
            LogHelper.Log.DebugFormat("Время {0}", sw.Elapsed.TotalSeconds);

            return documents;
        }
    }
}
