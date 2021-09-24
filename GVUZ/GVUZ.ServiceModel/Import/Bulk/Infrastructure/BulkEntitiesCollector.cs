using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using GVUZ.ServiceModel.Import.Bulk.Collectors;
using GVUZ.ServiceModel.Import.Bulk.Collectors.Base;
using GVUZ.ServiceModel.Import.WebService.Dto;

namespace GVUZ.ServiceModel.Import.Bulk.Infrastructure
{
    /// <summary>
    /// Формирует коллекции объектов, подготовленные к BULK загрузке
    /// </summary>
    public class BulkEntitiesCollector : IDisposable
    {
        private readonly int _packageId;
        private readonly int _institutionId;

        public BulkEntitiesCollector(int packageId, int institutionId)
        {
            _packageId = packageId;
            _institutionId = institutionId;
        }

        public BulkEntityCollectorList ApplicationCollector(List<ApplicationDto> applications)
        {
            return new BulkEntityCollectorList
            {
                new ApplicationBulkEntityCollector(applications, _packageId, _institutionId),
                new EntrantBulkEntityCollector(applications, _packageId, _institutionId),
                new EntrantDocumentBulkEntityCollector(applications, _packageId, _institutionId),
                new AppSelectedCompGroupBulkEntityCollector(applications, _packageId, _institutionId),
                new AppSelectedCompGroupItemBulkEntityCollector(applications, _packageId, _institutionId),
                new AppSelectedCompGroupTargetBulkEntityCollector(applications, _packageId, _institutionId),
                new AppEntranceTestDocumentBulkEntityCollector(applications, _packageId, _institutionId),
                new EntrantDocumentEgeAndOlympicSubjectBulkEntityCollector(applications, _packageId, _institutionId),
                new AppCompetitiveGroupItemBulkEntityCollector(applications, _packageId, _institutionId),
                new IndividualAchivementBulkEntityCollector(applications, _packageId, _institutionId)
            };
        }

        public BulkEntityCollectorList BuildConsideredApplicationBulkEntityCollector(List<ConsideredApplicationDto> consideredApplications)
        {
            return new BulkEntityCollectorList
            {
                 new ConsideredApplicationBulkEntityCollector(consideredApplications, _packageId, _institutionId)                                          
            };
        }

        public BulkEntityCollectorList BuildRecommendedApplicationBulkEntityCollector(List<RecommendedApplicationDto> recommendedApplications)
        {
            return new BulkEntityCollectorList
            {
                 new RecommendedApplicationBulkEntityCollector(recommendedApplications, _packageId, _institutionId)                                          
            };
        }

        public BulkEntityCollectorList BuildRecommendedListBulkEntityCollector(List<RecommendedListDto> recommendedLists)
        {
            return new BulkEntityCollectorList
            {
                new RecommendedListBulkEntityCollector(recommendedLists, _packageId, _institutionId)
            };
        }

        public void Dispose()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }

    public class BulkEntityCollectorList : List<IBulkEntityCollector>
    {
        /// <summary>
        /// Формирует готовые пары "имя таблицы, ридер" для загрузки через BULK
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, IDataReader> Collect()
        {
            /* Делаем это в N раз быстрее задействуя все ядра процессора */
            var items = this.AsParallel().Select(c => new { TableName = c.DestinationTableName, IReader = c.CollectToReader() });
            return Enumerable.ToDictionary(items.Where(c => c.IReader != null), item => item.TableName, item => item.IReader);
        }
    }
}
