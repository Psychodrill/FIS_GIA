using System.Collections.Generic;
using System.Linq;
using FogSoft.Helpers;
using GVUZ.ServiceModel.Import.Core.Operations.Conflicts;
using GVUZ.ServiceModel.Import.Core.Storages;
using GVUZ.ServiceModel.Import.WebService.Dto;

namespace GVUZ.ServiceModel.Import.Core.Operations.Importing
{
	public class CampaignImporter : ObjectImporter
	{
		private readonly CampaignDto _campaignDto;
		private readonly Campaign _campaignDb;
		public CampaignImporter(StorageManager storageManager, CampaignDto campaignDtoDto) : base(storageManager)
		{
			_campaignDto = campaignDtoDto;
			_campaignDb = DbObjectRepository.GetObject<Campaign>(_campaignDto.UID);

            /* грабли = если будет удаляться из бд - не надо обновлять, надо пытаться добавлять */
		    if (storageManager.DeleteStorage.ContainsObject<Campaign>(_campaignDto.UID))
		        _campaignDb = null;
		}

		protected override void FindExcludedObjectsInDbForDelete()
		{
			// отсутствуют дочерние объекты
		}

		protected override void FindInsertAndUpdate()
		{
			if (ConflictStorage.HasConflictOrNotImported(_campaignDto)) return;

			if (_campaignDb == null)
			{
				Campaign cgWithSameName = DbObjectRepository.Campaigns.SingleOrDefault(x => x.Name.ToLower() == _campaignDto.Name.ToLower());
				if (cgWithSameName != null)
				{
					ConflictStorage.AddNotImportedDto(_campaignDto, ConflictMessages.CampaignWithSameNameExists);
					return;
				}

				InsertStorage.AddCampaign(_campaignDto);
			}
			else
			{
				if (CanUpdate())
					UpdateStorage.AddCampaign(_campaignDto);
				else
				{
					ConflictStorage.AddCompetitiveGroups(_campaignDto, new HashSet<int>(ObjectLinkManager.CampaignLinkWithCompetitiveGroups(_campaignDb).Select(x => x.CompetitiveGroupID)));
				}
			}
		}

		protected override bool CanUpdate()
		{
			if (ObjectLinkManager.CampaignLinkWithCompetitiveGroups(_campaignDb).Length == 0) //нет КГ, можно обновлять
				return true;
			Campaign cgWithSameName = DbObjectRepository.Campaigns.SingleOrDefault(x => x.Name.ToLower() == _campaignDto.Name.ToLower());
			if (cgWithSameName != null && cgWithSameName.UID != _campaignDto.UID)
			{
				ConflictStorage.AddNotImportedDto(_campaignDto, ConflictMessages.CampaignWithSameNameExists);
				return false;
			}
			//тут не проверяем корректность, главное, что ничего не удалили из новой приёмной кампании
			var dtoDates = _campaignDto.CampaignDates.Select(x => x.EducationFormID + "@" + x.EducationLevelID + "@" + x.EducationSourceID + "@" + x.Course + "@" + x.Stage.To(0)).ToArray();
			var dbDates = _campaignDb.CampaignDate.Select(x => x.EducationFormID + "@" + x.EducationLevelID + "@" + x.EducationSourceID + "@" + x.Course + "@" + x.Stage).ToArray();
			if (dbDates.Except(dtoDates).Any()) //в конфликт и не даём, пока не удалят КГ
			{
				ConflictStorage.AddCompetitiveGroups(_campaignDto, new HashSet<int>(ObjectLinkManager.CampaignLinkWithCompetitiveGroups(_campaignDb).Select(x => x.CompetitiveGroupID)));
				return false;
			}  

			return true;
		}

		protected override void ProcessChildren(bool isParentConflict)
		{
			// нет дочерних объектов
		}

		protected override BaseDto GetDtoObject()
		{
			return _campaignDto;
		}

		protected override void CheckIntegrity()
		{
			ObjectIntegrityManager.CheckIntegrity(_campaignDto);
		}
	}
}
