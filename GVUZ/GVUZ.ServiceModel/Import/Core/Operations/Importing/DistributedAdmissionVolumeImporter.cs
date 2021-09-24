using GVUZ.Model.Entrants;
using GVUZ.ServiceModel.Import.Core.Storages;
using GVUZ.ServiceModel.Import.WebService.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GVUZ.ServiceModel.Import.Core.Operations.Importing
{

    public class DistributedAdmissionVolumeImporter : ObjectImporter
	{
        private readonly DistributedAdmissionVolumeDto _distributedAdmissionVolumeDto;

        public DistributedAdmissionVolumeImporter(StorageManager storageManager, DistributedAdmissionVolumeDto distributedAdmissionVolumeDto) :
			base(storageManager)
		{
            _distributedAdmissionVolumeDto = distributedAdmissionVolumeDto;
            ProcessedDtoStorage.AddDistributedAdmissionVolume(_distributedAdmissionVolumeDto);
		}

		protected override void FindInsertAndUpdate()
		{
            if (ConflictStorage.HasConflictOrNotImported(_distributedAdmissionVolumeDto)) return;

            InsertStorage.AddDistributedAdmissionVolume(_distributedAdmissionVolumeDto);

            //var volumeDb = DbObjectRepository.GetObject<DistributedAdmissionVolume>(_distributedAdmissionVolumeDto.UID);
            //if (volumeDb == null)
            //    InsertStorage.AddDistributedAdmissionVolume(_distributedAdmissionVolumeDto);
            //else
            //    UpdateStorage.AddDistributedAdmissionVolume(_distributedAdmissionVolumeDto);
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
            return _distributedAdmissionVolumeDto;
		}

		protected override void FindExcludedObjectsInDbForDelete()
		{
			// отсутствуют дочерние объекты
		}
	}
}
