using System;
using System.Linq;
using GVUZ.ServiceModel.Import.Core.Storages;
using GVUZ.ServiceModel.Import.WebService.Dto;

namespace GVUZ.ServiceModel.Import.Core.Operations.Importing
{
	public class CampaignInfoImporter : ObjectImporter
	{
		private readonly CampaignInfoDto _campaignsDto;
		public CampaignInfoImporter(StorageManager storageManager, CampaignInfoDto campaignDto) : base(storageManager)
		{
			if (campaignDto == null) throw new ArgumentNullException("campaignDto");
			_campaignsDto = campaignDto;
		}
		protected override void FindExcludedObjectsInDbForDelete()
		{
			foreach (Campaign campaign in DbObjectRepository.Campaigns)
			{
				// определяем какие объекты из БД не перечислены в списке импорта и подлежат удалению
				if (_campaignsDto.Campaigns.Any(x => !String.IsNullOrEmpty(campaign.UID)
					&& x.UID == campaign.UID)) continue;
				//теперь не удаляем их
				//new CampaignDeletion(StorageManager, campaign).TryDelete();
			}
		}

		protected override void FindInsertAndUpdate()
		{
			
		}

		protected override bool CanUpdate()
		{
			return true;
		}

		protected override void ProcessChildren(bool isParentConflict)
		{
			foreach (CampaignDto campaignDto in _campaignsDto.Campaigns)
			{
				if (!ConflictStorage.HasConflictOrNotImported(campaignDto))
					new CampaignImporter(StorageManager, campaignDto).AnalyzeImportPackage();
			}
		}

		protected override BaseDto GetDtoObject()
		{
			return null;
		}

		protected override void CheckIntegrity()
		{
			if (_campaignsDto == null || _campaignsDto.Campaigns == null) return;
			ObjectIntegrityManager.CheckUIDUnique(_campaignsDto.Campaigns);
		}
	}
}
