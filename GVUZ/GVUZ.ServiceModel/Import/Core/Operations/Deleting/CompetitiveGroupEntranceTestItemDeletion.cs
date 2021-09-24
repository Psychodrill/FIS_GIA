using FogSoft.Helpers;
using GVUZ.ServiceModel.Import.Core.Storages;
using GVUZ.ServiceModel.Import.WebService.Dto;

namespace GVUZ.ServiceModel.Import.Core.Operations.Deleting
{
	/// <summary>
	/// Управление проверкой на возможность удаления и отправкой на удаление вступительных испытаний в конкурсной группе.
	/// </summary>
	public class CompetitiveGroupEntranceTestItemDeletion : ObjectDeletion
	{
		public readonly EntranceTestItemC EntranceTestItem;
		private readonly ApplicationEntranceTestDocument[] _linkedEntranceTestDocuments;
		private bool _hasLinkedApps;

		public CompetitiveGroupEntranceTestItemDeletion(StorageManager storageManager, 
			EntranceTestItemC entranceTestItem) : base(storageManager)
		{
			EntranceTestItem = entranceTestItem;
			_linkedEntranceTestDocuments = ObjectLinkManager.CompetitiveGroupEntranceTestLinkWithAppEntranceTest(entranceTestItem);
		}

		public CompetitiveGroupEntranceTestItemDeletion(StorageManager storageManager,
			EntranceTestItemC entranceTestItem, BaseDto linkedObjDto)
			: base(storageManager, linkedObjDto)
		{
			EntranceTestItem = entranceTestItem;
			_linkedEntranceTestDocuments = ObjectLinkManager.CompetitiveGroupEntranceTestLinkWithAppEntranceTest(entranceTestItem);
		}

		protected override void FillDeletionList()
		{
			// Сведения о всех результатах ВИ, указанных в заявлениях
		    if (_notImportedDto != null)
		    {
		        ConflictStorage.AddEntranceTestResults(_notImportedDto, _linkedEntranceTestDocuments);
		    }
		    else if (_linkedEntranceTestDocuments.Length > 0)
		    {
		        ConflictStorage.AddEntranceTestResults(_notImportedDto, _linkedEntranceTestDocuments);
		    }

		    // льготы на ВИ
			foreach (BenefitItemC benefitItem in EntranceTestItem.BenefitItemC)
				DependedAndLinkedObjectsDeletionList.Add(new CompetitiveGroupBenefitItemDeletion(StorageManager, benefitItem));
			
			if (_notImportedDto != null)
			{
				// заявления, входящие в КГ
				foreach (Application app in ObjectLinkManager.CompetitiveGroupLinkWithApplications(EntranceTestItem.CompetitiveGroup))
				{
					_hasLinkedApps = true;
					ConflictStorage.AddApplication(_notImportedDto, app);
					ConflictStorage.AddEntranceTestResults(_notImportedDto, ObjectLinkManager.ApplicationLinkWithEntranceTestResults(app));

					// приказы, в которые включены заявления.
					var order = ObjectLinkManager.ApplicationLinkWithOrder(app);
					if(order != null)
						ConflictStorage.AddOrdersOfAdmission(_notImportedDto, order);
				}
			}

			/*// документы на ВИ
			foreach (ApplicationEntranceTestDocument appEntranceTestDocument in EntranceTestItem.ApplicationEntranceTestDocument)
				DependedAndLinkedObjectsDeletionList.Add(new EntranceTestDocumentDeletion(StorageManager, appEntranceTestDocument));*/
		}

		public override bool IsValidExtraConditions()
		{
			return !_hasLinkedApps;
		}

		public override bool TryDelete()
		{
			if (CanDelete())
			{
				DeleteStorage.AddEntranceTestItem(EntranceTestItem);
				foreach (ObjectDeletion objectDeletion in DependedAndLinkedObjectsDeletionList)
					objectDeletion.TryDelete();

				return true;
			}

			return false;
		}

		public override int GetDbObjectID()
		{
			return EntranceTestItem.EntranceTestItemID;
		}

		public override object GetDbObject()
		{
			return EntranceTestItem;
		}
	}
}
