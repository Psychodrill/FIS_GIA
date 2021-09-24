using GVUZ.ServiceModel.Import.Core.Operations.Conflicts;
using GVUZ.ServiceModel.Import.Core.Storages;
using GVUZ.ServiceModel.Import.WebService.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GVUZ.ServiceModel.Import.Core.Operations.Importing
{
    public class InstitutionAchievementImporter : ObjectImporter
    {
        private readonly InstitutionAchievementDto _institutionAchievementDto;

        public InstitutionAchievementImporter(StorageManager storageManager, InstitutionAchievementDto institutionAchievementDto) :
			base(storageManager)
		{
            _institutionAchievementDto = institutionAchievementDto;
            ProcessedDtoStorage.AddInstitutionAchievement(_institutionAchievementDto);
		}

		protected override void FindInsertAndUpdate()
		{
            if (ConflictStorage.HasConflictOrNotImported(_institutionAchievementDto)) return;

            InsertStorage.AddInstitutionAchievement(_institutionAchievementDto);

            //var volumeDb = DbObjectRepository.GetObject<DistributedAdmissionVolume>(_distributedAdmissionVolumeDto.UID);
            //if (volumeDb == null)
            //    InsertStorage.AddDistributedAdmissionVolume(_distributedAdmissionVolumeDto);
            //else
            //    UpdateStorage.AddDistributedAdmissionVolume(_distributedAdmissionVolumeDto);
		}

        protected override void CheckIntegrity()
        {
           
        }

		protected override bool CanUpdate()
		{
			return true;
		}

		protected override void ProcessChildren(bool isParentConflict)
		{
			// нет дочерних объектов
		}

		protected override BaseDto GetDtoObject()
		{
            return _institutionAchievementDto;
		}

		protected override void FindExcludedObjectsInDbForDelete()
		{
			// отсутствуют дочерние объекты
		}
    }

    public class InstitutionAchievementCollectionImporter : ObjectImporter
    {
        public InstitutionAchievementDto[] _institutionArchievements { get; private set; }

        public InstitutionAchievementCollectionImporter(StorageManager storageManager, InstitutionAchievementDto[] items)
            : base(storageManager)
        {
            _institutionArchievements = items;
        }

        protected override void FindExcludedObjectsInDbForDelete()
        {
            return;
        }

        protected override void FindInsertAndUpdate()
        {
            return;
        }

        protected override bool CanUpdate()
        {
            return true;
        }

        protected override void ProcessChildren(bool isParentConflict)
        {
            foreach (var ia in _institutionArchievements)
                new InstitutionAchievementImporter(StorageManager, ia).AnalyzeImportPackage();
        }

        protected override BaseDto GetDtoObject()
        {
            return null;
        }

        protected override void CheckIntegrity()
        {
            //  А почему бы валидацию не тут?
            
            // Проверка на дубли IAUID
            foreach (var group in _institutionArchievements.GroupBy(t => t.IAUID))
                if (group.Count() > 1)
                {
                    foreach (var item in group)
                    {
                        ConflictStorage.AddNotImportedDto(item, ConflictMessages.UIDMustBeUniqueInCollection);
                    }
                }

            // Проверка на то, что в пакете все дочерние элементы уникальны по комбинации полей IAUID, CampaignUID
            foreach (var group in _institutionArchievements.GroupBy(t => t.Key()))
                if (group.Count() > 1)
                {
                    foreach (var item in group)
                    {
                        ConflictStorage.AddNotImportedDto(item, ConflictMessages.IAUIDAndCampaignUIDMustBeUniqueInCollection, item.IAUID, item.CampaignUID);
                    }
                }

            //// Проверка наличия в текущем пакете ровно одного элемента "Root\PackageData\CampaignInfo\Campaigns\Campaign\UID" с указанным UID.
            //// Убрали решением от 2015-05-14
            //foreach (var group in _institutionArchievements.GroupBy(t => t.CampaignUID))
            //    if (group.Count() > 1)
            //    {
            //        foreach (var item in group)
            //        {
            //            ConflictStorage.AddNotImportedDto(item, ConflictMessages.CampaignUIDMustBeUniqueInCollection, item.CampaignUID);
            //        }
            //    }

            GVUZ.Model.Entrants.EntrantsEntities _entrantEntities = new Model.Entrants.EntrantsEntities();
            foreach (var ia in _institutionArchievements)
            {
                // 1. UID - не пустой
                if (string.IsNullOrWhiteSpace(ia.UID))
                {
                    ConflictStorage.AddNotImportedDto(ia, ConflictMessages.IAIncorrentFieldValue, "IAUID");
                    continue;
                }
                // 2. Name - не пустой
                if (string.IsNullOrWhiteSpace(ia.Name))
                {
                    ConflictStorage.AddNotImportedDto(ia, ConflictMessages.IAIncorrentFieldValue, "Name");
                    continue;
                }
                // 3. IdCategory - Проверка наличия элемента в справочнике №36 “Категории индивидуальных достижений” (IndividualAchievementsCategory)
                if (!_entrantEntities.IndividualAchievementsCategory.Where(t => t.IdCategory == ia.IdCategory).Any())
                {
                    ConflictStorage.AddNotImportedDto(ia, ConflictMessages.DictionaryItemAbsent, "IdCategory");
                    continue;
                }

                // 4. MaxValue - Проверка на непустоту и числовой формат (до 3 цифр перед запятой, до 4 знаков после запятой)
                string[] parts = ia.MaxValue.ToString().Split(new string[] { ",", "." },  StringSplitOptions.RemoveEmptyEntries);
                if (parts.Count() > 0 && parts[0].Length > 3)
                {
                    ConflictStorage.AddNotImportedDto(ia, ConflictMessages.IAIncorrentFieldValue, "MaxValue");
                    continue;
                }
                if (parts.Count() > 1 && parts[1].Length > 4)
                {
                    ConflictStorage.AddNotImportedDto(ia, ConflictMessages.IAIncorrentFieldValue, "MaxValue");
                    continue;
                }

                // 5. CampaingUID - Если в текущем пакете таких элементов нет, проверяется наличие в БД ровно одной записи в таблице “Приемные кампании” (Campaign), 
                // с указанным UID, и относящейся к тому же самому ОУ (принадлежность к ОУ определяется через поле CampaignID.InstitutionID). 
                // Если найдено более одного элемента или не найдено ни одного - ошибка. 
                // Убрал && t.StatusID == 1, потому что могут быть дозаполнения данных после 
                var dbCampaigns = _entrantEntities.Campaign.Where(t => t.InstitutionID == InstitutionID).ToList();
                var iaCampaign = dbCampaigns.Where(t => t.UID == ia.CampaignUID).FirstOrDefault();
                    //_entrantEntities.Campaign.Where(t => t.UID == ia.CampaignUID && t.InstitutionID == InstitutionID && t.StatusID == 1).FirstOrDefault();

                if (iaCampaign == null)
                {
                    if (dbCampaigns.Count == 1 && string.IsNullOrWhiteSpace(ia.CampaignUID))
                        ia.CampaignUID = dbCampaigns[0].UID;
                    else
                        ConflictStorage.AddNotImportedDto(ia, ConflictMessages.DictionaryItemAbsent, "CampaignUID");
                }


                //if (!ia.IsBroken)
                //    InsertStorage.AddInstitutionAchievement(ia);
            }

        }
    }
}
