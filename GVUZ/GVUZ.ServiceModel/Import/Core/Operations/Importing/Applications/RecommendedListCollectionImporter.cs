using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GVUZ.ServiceModel.Import.WebService.Dto;
using GVUZ.ServiceModel.Import.Core.Storages;

namespace GVUZ.ServiceModel.Import.Core.Operations.Importing.Applications
{
    public class RecommendedListCollectionImporter : ObjectImporter
    {
        public RecommendedListDto[] _recLists { get; private set; }

        public RecommendedListCollectionImporter(StorageManager storageManager, RecommendedListDto[] lists)
            : base(storageManager)
        {
            _recLists = lists;
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
            foreach (var list in _recLists)
                new RecommendedListsImporter(StorageManager, list).AnalyzeImportPackage();
        }

        protected override BaseDto GetDtoObject()
        {
            return null;
        }
    }
}
