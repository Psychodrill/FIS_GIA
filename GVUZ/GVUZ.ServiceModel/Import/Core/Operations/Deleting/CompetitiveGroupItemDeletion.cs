using System.Collections.Generic;
using System.Linq;
using GVUZ.ServiceModel.Import.Core.Operations.Deleting.Applications;
using GVUZ.ServiceModel.Import.Core.Storages;
using GVUZ.ServiceModel.Import.WebService.Dto;

namespace GVUZ.ServiceModel.Import.Core.Operations.Deleting
{
	/// <summary>
	/// Удаление направление в КГ
	/// </summary>
	public class CompetitiveGroupItemDeletion : ObjectDeletion
	{
		public readonly CompetitiveGroupItem CompetitiveGroupItem;

		public CompetitiveGroupItemDeletion(StorageManager storageManager, CompetitiveGroupItem competitiveGroupItem) :
			base(storageManager)
		{
			CompetitiveGroupItem = competitiveGroupItem;
		}

		public CompetitiveGroupItemDeletion(StorageManager storageManager, CompetitiveGroupItem competitiveGroupItem,
			CompetitiveGroupItemDto competitiveGroupItemDto) :
			base(storageManager, competitiveGroupItemDto)
		{
			CompetitiveGroupItem = competitiveGroupItem;
		}

		public override bool IsValidExtraConditions()
		{
			// если направление КГ в конфликтах, то удаление не производим
			return !ConflictStorage.GetConflictCompetitiveGroupItemIDs().Contains(CompetitiveGroupItem.CompetitiveGroupItemID);
		}

		protected override void FillDeletionList()
		{			
			// К НП привязаны Заявления в Приказе
			foreach (Application app in ObjectLinkManager.CompetitiveGroupItemLinkWithAppsInOrder(CompetitiveGroupItem))
				DependedAndLinkedObjectsDeletionList.Add(new ApplicationDeletion(StorageManager, app));

			// удаление направлений ЦП
			foreach (CompetitiveGroupTargetItem cgTargetItem in DbObjectRepository.CompetitiveGroupTargetItems
				.Where(x => x.CompetitiveGroupItemID == CompetitiveGroupItem.CompetitiveGroupItemID))
				DependedAndLinkedObjectsDeletionList.Add(new CompetitiveGroupTargetItemDeletion(StorageManager, cgTargetItem));

			// К НП (= 1 в КГ ) все заявления в КГ
			IEnumerable<CompetitiveGroupItem> cgItemsForDelete = StorageManager.DeleteStorage.CompetitiveGroupItems
				.Where(x => x.CompetitiveGroupID == CompetitiveGroupItem.CompetitiveGroupID);
			IEnumerable<CompetitiveGroupItem> cgItemsInCG = DbObjectRepository.CompetitiveGroupItems.Where(x => 
                x.CompetitiveGroupID == CompetitiveGroupItem.CompetitiveGroupID);
			//если удаляем все, смотрим на зависимые заявления
			if(cgItemsForDelete.Count() == cgItemsInCG.Count())
				foreach (Application application in 
						ObjectLinkManager.LastCompetitiveGroupItemLinkWithApplicationsOnCompetitiveGroup(CompetitiveGroupItem))
					DependedAndLinkedObjectsDeletionList.Add(new ApplicationDeletion(StorageManager, application));
		}

		public override bool TryDelete()
		{
		    if (CanDelete())
			{
				DeleteStorage.AddCompetitiveGroupItem(CompetitiveGroupItem);
				foreach (ObjectDeletion objectDeletion in DependedAndLinkedObjectsDeletionList)
					objectDeletion.TryDelete();

				return true;
			}

			return false;
		}

		public override int GetDbObjectID()
		{
			return CompetitiveGroupItem.CompetitiveGroupItemID;
		}

		public override object GetDbObject()
		{
			return CompetitiveGroupItem;
		}
	}
}
