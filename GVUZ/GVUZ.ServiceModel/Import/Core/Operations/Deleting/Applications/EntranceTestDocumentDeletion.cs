using GVUZ.ServiceModel.Import.Core.Storages;
using GVUZ.ServiceModel.Import.WebService.Dto;

namespace GVUZ.ServiceModel.Import.Core.Operations.Deleting.Applications
{
	public class EntranceTestDocumentDeletion : ObjectDeletion
	{
		public readonly ApplicationEntranceTestDocument _entranceTestDocument;

		public EntranceTestDocumentDeletion(StorageManager storageManager, 
			ApplicationEntranceTestDocument entranceTestDocument) :
			base(storageManager)
		{
			_entranceTestDocument = entranceTestDocument;
		}

        public override bool TryDelete()
        {
            return true;
        }

		public override bool IsValidExtraConditions()
		{
			//if(ConflictStorage.)
			ApplicationShortRef appShortRef = ObjectLinkManager.ApplicationEntranceTestDocumentLinkWithOrder(_entranceTestDocument);

			if (appShortRef != null)
			{
				ConflictStorage.AddOrdersOfAdmission(_entranceTestDocument, appShortRef);
				return false;
			}

			return true;
		}

		public override int GetDbObjectID()
		{			
			return _entranceTestDocument.ID;
		}

		public override object GetDbObject()
		{
			return _entranceTestDocument;
		}

		public bool EntranceTestIsApplicationCommonBenefit()
		{
			return !_entranceTestDocument.EntranceTestItemID.HasValue;
		}
	}
}
