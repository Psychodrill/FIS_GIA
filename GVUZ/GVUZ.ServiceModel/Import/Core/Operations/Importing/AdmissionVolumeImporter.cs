using GVUZ.ServiceModel.Import.Core.Storages;
using GVUZ.ServiceModel.Import.WebService.Dto;

namespace GVUZ.ServiceModel.Import.Core.Operations.Importing
{
	public class AdmissionVolumeImporter : ObjectImporter
	{
		private readonly AdmissionVolumeDto _admissionVolumeDto;

		public AdmissionVolumeImporter(StorageManager storageManager, AdmissionVolumeDto admissionVolumeDto) :
			base(storageManager)
		{
			_admissionVolumeDto = admissionVolumeDto;
			ProcessedDtoStorage.AddAdmissionVolume(_admissionVolumeDto);
		}

		protected override void FindInsertAndUpdate()
		{
			if (ConflictStorage.HasConflictOrNotImported(_admissionVolumeDto)) return;

			var volumeDb = DbObjectRepository.GetObject<AdmissionVolume>(_admissionVolumeDto.UID);
			if (volumeDb == null)
				InsertStorage.AddAdmissionVolume(_admissionVolumeDto);
			else
				UpdateStorage.AddAdmissionVolume(_admissionVolumeDto);
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
			return _admissionVolumeDto;
		}

		protected override void FindExcludedObjectsInDbForDelete()
		{
			// отсутствуют дочерние объекты
		}
	}
}
