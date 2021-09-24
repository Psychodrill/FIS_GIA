using GVUZ.ServiceModel.Import.Core.Operations.Deleting.Applications;
using GVUZ.ServiceModel.Import.Core.Storages;
using GVUZ.ServiceModel.Import.WebService.Dto;

namespace GVUZ.ServiceModel.Import.Core.Operations.Deleting
{
	/// <summary>
	/// Удаление организаций ЦП
	/// </summary>
	public class CompetitiveGroupTargetDeletion : ObjectDeletion
	{
		private readonly CompetitiveGroupTarget _competitiveGroupTarget;

		public CompetitiveGroupTargetDeletion(StorageManager storageManager, CompetitiveGroupTarget competitiveGroupTarget) :
			base(storageManager)
		{
			_competitiveGroupTarget = competitiveGroupTarget;
		}

		public CompetitiveGroupTargetDeletion(StorageManager storageManager,
			CompetitiveGroupTarget competitiveGroupTarget, CompetitiveGroupTargetDto competitiveGroupTargetDto) :
			base(storageManager, competitiveGroupTargetDto)
		{
			_competitiveGroupTarget = competitiveGroupTarget;
		}


		protected override void FillDeletionList()
		{
			// связь с заявлениями
			foreach (Application app in ObjectLinkManager.TargetOrganizationItemLinkWithAppsInOrder(_competitiveGroupTarget))
				DependedAndLinkedObjectsDeletionList.Add(new ApplicationDeletion(StorageManager, app));
			// направления ЦП
			foreach (CompetitiveGroupTargetItem targetItem in _competitiveGroupTarget.CompetitiveGroupTargetItem)
				DependedAndLinkedObjectsDeletionList.Add(new CompetitiveGroupTargetItemDeletion(StorageManager, targetItem));			
		}

		public override bool TryDelete()
		{
			if (CanDelete())
			{
				DeleteStorage.AddCompetitiveGroupTarget(_competitiveGroupTarget);
				foreach (ObjectDeletion objectDeletion in DependedAndLinkedObjectsDeletionList)
					objectDeletion.TryDelete();

				return true;
			}

			return false;
		}

		public override int GetDbObjectID()
		{
			return _competitiveGroupTarget.CompetitiveGroupTargetID;
		}

		public override object GetDbObject()
		{
			return _competitiveGroupTarget;
		}
	}
}
