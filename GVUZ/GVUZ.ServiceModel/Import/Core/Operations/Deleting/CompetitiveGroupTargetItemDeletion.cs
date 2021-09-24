using GVUZ.ServiceModel.Import.Core.Operations.Deleting.Applications;
using GVUZ.ServiceModel.Import.Core.Storages;
using GVUZ.ServiceModel.Import.WebService.Dto;

namespace GVUZ.ServiceModel.Import.Core.Operations.Deleting
{
	/// <summary>
	/// Удаление целевых направлений в КГ
	/// </summary>
	public class CompetitiveGroupTargetItemDeletion : ObjectDeletion
	{
		public readonly CompetitiveGroupTargetItem CompetitiveGroupTargetItem;

		public CompetitiveGroupTargetItemDeletion(StorageManager storageManager, 
			CompetitiveGroupTargetItem competitiveGroupTargetItem) : base(storageManager)
		{
			CompetitiveGroupTargetItem = competitiveGroupTargetItem;
		}

		public CompetitiveGroupTargetItemDeletion(StorageManager storageManager,
			CompetitiveGroupTargetItem competitiveGroupTargetItem,
			CompetitiveGroupTargetItemDto competitiveGroupTargetItemDto)
			: base(storageManager, competitiveGroupTargetItemDto)
		{
			CompetitiveGroupTargetItem = competitiveGroupTargetItem;
		}
		protected override void FillDeletionList()
		{
			// К НП целевого приема привязаны Заявления в Приказе
			Application[] linkedApps = ObjectLinkManager.CompetitiveGroupTargetItemLinkWithAppsInOrder(CompetitiveGroupTargetItem);
			foreach (Application application in linkedApps)
				DependedAndLinkedObjectsDeletionList.Add(new ApplicationDeletion(StorageManager, application));			
		}

		public override bool TryDelete()
		{
			if (CanDelete())
			{
				DeleteStorage.AddCompetitiveGroupTargetItem(CompetitiveGroupTargetItem);
				foreach (ObjectDeletion objectDeletion in DependedAndLinkedObjectsDeletionList)
					objectDeletion.TryDelete();

				return true;
			}

			return false;
		}

		public override int GetDbObjectID()
		{
			return CompetitiveGroupTargetItem.CompetitiveGroupTargetItemID;
		}

		public override object GetDbObject()
		{
			return CompetitiveGroupTargetItem;
		}
	}
}
