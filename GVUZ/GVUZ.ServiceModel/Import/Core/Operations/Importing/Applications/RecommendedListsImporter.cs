using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GVUZ.ServiceModel.Import.WebService.Dto;
using GVUZ.ServiceModel.Import.Core.Storages;

namespace GVUZ.ServiceModel.Import.Core.Operations.Importing.Applications
{
    public class RecommendedListsImporter : ObjectImporter
    {
        private readonly RecommendedListDto _recListDto;

        public RecommendedListsImporter(StorageManager storageManager, RecommendedListDto listDto)
            : base(storageManager)
        {
            _recListDto = listDto;
        }

        protected override void FindExcludedObjectsInDbForDelete()
        {
            return;
        }

        protected override void FindInsertAndUpdate()
        {
            if (ConflictStorage.HasConflictOrNotImported(_recListDto)) return;

            InsertStorage.AddRecommendedList(_recListDto);
        }

        protected override bool CanUpdate()
        {
            return true;
        }

        protected override void ProcessChildren(bool isParentConflict)
        {
            // Всё обрабатывается здесь
        }

        protected override BaseDto GetDtoObject()
        {
            return _recListDto;
        }

        protected override void CheckIntegrity()
        {
            ObjectIntegrityManager.CheckRecommendedList(_recListDto);
        }
    }
}
