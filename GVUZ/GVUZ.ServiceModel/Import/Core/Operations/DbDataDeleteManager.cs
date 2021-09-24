using System.Collections.Generic;
using System.Linq;
using GVUZ.ServiceModel.Import.Core.Storages;

namespace GVUZ.ServiceModel.Import.Core.Operations
{
    /// <summary>
    /// Удаление данных
    /// </summary>
    public class DbDataDeleteManager : StorageConsumer
    {
        private readonly DbObjectStorage _deleteStorage;
        private readonly ImportEntities _importEntities;

        public DbDataDeleteManager(StorageManager storageManager)
            : base(storageManager)
        {
            _deleteStorage = DeleteStorage;
            _importEntities = DbObjectRepository.ImportEntities;
        }

        /// <summary>
        /// Удаление кампаний
        /// </summary>
        public void DeleteCampaigns()
        {
            foreach (Campaign campaignL in _deleteStorage.Campaigns)
            {
                var campaign = campaignL;
                _importEntities.AdmissionVolume.Where(x => x.CampaignID == campaign.CampaignID).ToList().ForEach(_importEntities.AdmissionVolume.DeleteObject);
                //не хотят каскадом
                campaign.CampaignEducationLevel.ToList().ForEach(_importEntities.CampaignEducationLevel.DeleteObject);
                campaign.CampaignDate.ToList().ForEach(_importEntities.CampaignDate.DeleteObject);
                //мы сюда залезли если только прошли проверки и это пустые приказы, их можно удалять
                _importEntities.OrderOfAdmission.Where(x => x.CampaignID == campaign.CampaignID).ToList().ForEach(_importEntities.OrderOfAdmission.DeleteObject);

                _importEntities.Campaign.DeleteObject(campaign);
            }

            _importEntities.SaveChanges();
        }

        /// <summary>
        /// Удаление КГ и объёма приёма
        /// </summary>
        public void DeleteInstitutionStructure()
        {
            var entranceTestItemId = new List<int>();
            entranceTestItemId.AddRange(_deleteStorage.EntranceTestItems.Select(c => c.EntranceTestItemID));
            if (entranceTestItemId.Count > 0)
            {
                _importEntities.ApplicationEntranceTestDocument.DeleteIn("EntranceTestItemID", entranceTestItemId, 500);
                _importEntities.BenefitItemC.DeleteIn("EntranceTestItemID", entranceTestItemId, 500);
                _importEntities.EntranceTestItemC.DeleteIn(c => c.EntranceTestItemID, entranceTestItemId, 500);
            }

            var benefitItemId = new List<int>();
            benefitItemId.AddRange(_deleteStorage.CompetitiveGroupBenefitItems.Select(c => c.BenefitItemID));
            if (benefitItemId.Count > 0)
            {
                _importEntities.BenefitItemCOlympicType.DeleteIn(c => c.BenefitItemID, benefitItemId, 500);
                _importEntities.BenefitItemSubject.DeleteIn(c => c.BenefitItemId, benefitItemId, 500);
                _importEntities.BenefitItemC.DeleteIn(c => c.BenefitItemID, benefitItemId, 500);
            }

            _importEntities.CompetitiveGroupTargetItem.DeleteIn(c => c.CompetitiveGroupTargetItemID, 
                _deleteStorage.CompetitiveGroupTargetItems.Select(c => c.CompetitiveGroupTargetItemID), 500);

            var competitiveGroupTargetId = new List<int>();
            competitiveGroupTargetId.AddRange(_deleteStorage.CompetitiveGroupTargets
                .Where(obj=>obj.Application == null)//Если связанные заявления есть у нас ничего не получится
                .Select(c => c.CompetitiveGroupTargetID));
            if (competitiveGroupTargetId.Count > 0)
            {
                _importEntities.ApplicationSelectedCompetitiveGroupTarget.DeleteIn(c => c.CompetitiveGroupTargetID, competitiveGroupTargetId, 500);
                _importEntities.CompetitiveGroupTarget.DeleteIn(c => c.CompetitiveGroupTargetID, competitiveGroupTargetId, 500);
            }

            var competitiveGroupItemId = new List<int>();
            competitiveGroupItemId.AddRange(_deleteStorage.CompetitiveGroupItems.Select(c => c.CompetitiveGroupItemID));
            if (competitiveGroupItemId.Count > 0)
            {
                _importEntities.ApplicationSelectedCompetitiveGroupItem.DeleteIn(c => c.CompetitiveGroupItemID, competitiveGroupItemId, 500);
                _importEntities.ApplicationCompetitiveGroupItem.DeleteIn(c => c.CompetitiveGroupItemId, competitiveGroupItemId, 500);
                _importEntities.CompetitiveGroupItem.DeleteIn(c => c.CompetitiveGroupItemID, competitiveGroupItemId, 500);
            }

            _importEntities.CompetitiveGroup.DeleteIn(c => c.CompetitiveGroupID, 
                _deleteStorage.CompetitiveGroups.Select(c => c.CompetitiveGroupID), 500);
            _importEntities.AdmissionVolume.DeleteIn(c => c.AdmissionVolumeID, 
                _deleteStorage.AdmissionVolumes.Select(c => c.AdmissionVolumeID), 500);
            // удаление DistributedAdmissionVolume - Cascade!
        }

        /// <summary>
        /// Удаление приказов
        /// </summary>
        public void DeleteOrders()
        {
            foreach (var orders in _deleteStorage.OrdersOfAdmission)
            {
                _importEntities.OrderOfAdmission.DeleteObject(orders);
            }

            _importEntities.SaveChanges();
        }
    }
}
