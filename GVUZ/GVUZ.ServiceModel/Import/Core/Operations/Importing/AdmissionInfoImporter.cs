using System;
using System.Linq;
using GVUZ.ServiceModel.Import.Core.Operations.Deleting;
using GVUZ.ServiceModel.Import.Core.Storages;
using GVUZ.ServiceModel.Import.Package;
using GVUZ.ServiceModel.Import.WebService.Dto;

namespace GVUZ.ServiceModel.Import.Core.Operations.Importing
{
	public class AdmissionInfoImporter : ObjectImporter
	{
		private readonly AdmissionInfoDto _admissionInfoDto;		

		public AdmissionInfoImporter(StorageManager storageManager, AdmissionInfoDto admissionInfoDto) :
			base(storageManager)
		{
			if (admissionInfoDto == null) throw new ArgumentNullException("admissionInfoDto");
			_admissionInfoDto = admissionInfoDto;
		}

		protected override void FindExcludedObjectsInDbForDelete()
		{
			var affectedCampaigns1 = _admissionInfoDto
				.AdmissionVolume
				.Where(x => !String.IsNullOrEmpty(x.CampaignUID)).Select(x => x.CampaignUID)
				.Distinct();
			var affectedCampaigns2 = _admissionInfoDto.CompetitiveGroups
				.Where(x => !String.IsNullOrEmpty(x.CampaignUID)).Select(x => x.CampaignUID)
				.Distinct();
			var affectedCampaigns = affectedCampaigns1.Union(affectedCampaigns2).ToArray();
			//не трогаем данные из других ПК
			foreach (AdmissionVolume admissionVolume in DbObjectRepository.AdmissionVolumes.Where(x => affectedCampaigns.Contains(x.Campaign.UID)))
			{
				// определяем какие объекты из БД не перечислены в списке импорта и подлежат удалению
				if (_admissionInfoDto.AdmissionVolume.Any(x => !String.IsNullOrEmpty(admissionVolume.UID)
					&& x.UID == admissionVolume.UID)) continue;

                new AdmissionVolumeDeletion(StorageManager, admissionVolume).TryDelete();
			}

			foreach (CompetitiveGroup competitiveGroup in DbObjectRepository.CompetitiveGroups.Where(x => affectedCampaigns.Contains(x.Campaign.UID)))
			{
				if (_admissionInfoDto.CompetitiveGroups.Any(x => !String.IsNullOrEmpty(competitiveGroup.UID)
					&& x.UID == competitiveGroup.UID)) continue;

                new CompetitiveGroupDeletion(StorageManager, competitiveGroup).TryDelete();
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
			foreach (AdmissionVolumeDto admissionVolumeDto in _admissionInfoDto.AdmissionVolume)
			{
				if (!ConflictStorage.HasConflictOrNotImported(admissionVolumeDto))
					new AdmissionVolumeImporter(StorageManager, admissionVolumeDto).AnalyzeImportPackage();
			}
				
			if (_admissionInfoDto.CompetitiveGroups != null)
				foreach (CompetitiveGroupDto cgDto in _admissionInfoDto.CompetitiveGroups)
					new CompetitiveGroupImporter(StorageManager, cgDto).AnalyzeImportPackage();

            if (_admissionInfoDto.DistributedAdmissionVolume!=null)
                foreach (DistributedAdmissionVolumeDto davDto in _admissionInfoDto.DistributedAdmissionVolume)
                {
                    new DistributedAdmissionVolumeImporter(StorageManager, davDto).AnalyzeImportPackage();
                }

			//после удаления ещё проверяем на корректность родительских уидов (дубли)
			ObjectIntegrityManager.CheckParentUIDUnique(_admissionInfoDto);
		}

		protected override BaseDto GetDtoObject()
		{
			return null;
		}

		protected override void CheckIntegrity()
		{
			ObjectIntegrityManager.CheckPlacesInDirectionBetweenAdmissionVolumeAndCompetitiveGroupItems(_admissionInfoDto);
            ObjectIntegrityManager.CheckDictionaries(_admissionInfoDto); // только CompetitiveGroup
		}

	}
}
