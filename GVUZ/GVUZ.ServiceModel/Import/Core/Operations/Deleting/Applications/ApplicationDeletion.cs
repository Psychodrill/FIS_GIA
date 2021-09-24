using GVUZ.ServiceModel.Import.Core.Storages;
using GVUZ.ServiceModel.Import.WebService.Dto;

namespace GVUZ.ServiceModel.Import.Core.Operations.Deleting.Applications
{
	/// <summary>
	/// Удаление заявлений
	/// </summary>
	public class ApplicationDeletion : ObjectDeletion
	{
		private readonly Application _application;

		public ApplicationDeletion(StorageManager storageManager, Application application) :
			base(storageManager)
		{
			_application = application;
		}

		public ApplicationDeletion(StorageManager storageManager, Application application, BaseDto appDto) :
			base(storageManager)
		{
			_application = application;
		}

		protected override void FillDeletionList()
		{
			
		}

		public override bool IsValidExtraConditions()
		{
			bool result = true;

			// проверка на приказ
			// нельзя удалять, если заявление в приказе
			// вначале нужно исключить из него
			if (_application.StatusID == ApplicationStatusType.InOrder)
			{
				ConflictStorage.AddOrdersOfAdmission(_application, _application.GetApplicationShortRef());
				result = false;
			}
			
			// убрал проверку на общ. льготы и РВИ на основании пункта 11 документа. 
			// для удаления заявления нужно проверить только на включение заявления в приказ.
/*
			foreach (var appEntrTest in DbObjectRepository.ApplicationEntranceTestDocuments
				.Where(x => x.ApplicationID == _application.ApplicationID).ToList())
			{
				// общие льготы
				if(appEntrTest.BenefitID.HasValue)
					ConflictStorage.AddApplicationCommonBenefits(_application, new HashSet<int> { appEntrTest.ID });
				else
				// результаты ВИ
					ConflictStorage.AddEntranceTestResults(_application, new HashSet<int> { appEntrTest.ID });
				result = false;
			}
*/

			return result;
		}

		public override bool TryDelete()
		{
			if (CanDelete())
			{
				// Удалятся все зависимые объекты у заявления за один раз
				DeleteStorage.AddApplication(_application);
				return true;
			}

			return false;
		}

		public override int GetDbObjectID()
		{
			return _application.ApplicationID;
		}

		public override ApplicationShortRef GetDbApplicationShortRef()
		{
			return new ApplicationShortRef
			{
				ApplicationNumber = _application.ApplicationNumber,
				RegistrationDateDate = _application.RegistrationDate
			};			
		}

		public override object GetDbObject()
		{
			return _application;
		}
	}
}
