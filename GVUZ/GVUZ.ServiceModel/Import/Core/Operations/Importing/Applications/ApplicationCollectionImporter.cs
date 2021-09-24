using System;
using System.Diagnostics;
using System.Linq;
using FogSoft.Helpers;
using GVUZ.ServiceModel.Import.Core.Storages;
using GVUZ.ServiceModel.Import.WebService.Dto;

namespace GVUZ.ServiceModel.Import.Core.Operations.Importing.Applications
{
	/// <summary>
	/// Импорт коллекции заявлений
	/// </summary>
	public class ApplicationCollectionImporter : ObjectImporter
	{
        public ApplicationDto[] _applicationsDto { get; private set; }

		public ApplicationCollectionImporter(StorageManager storageManager,  ApplicationDto[] applications) : 
			base(storageManager)
		{			
			_applicationsDto = applications;
		}

		protected override void FindExcludedObjectsInDbForDelete()
		{
		}

		protected override void FindInsertAndUpdate()
		{
		}

		protected override bool CanUpdate()
		{
			return true;
		}

		protected override void ProcessChildren(bool isParentConflict)
		{
            var sw = new Stopwatch();
            sw.Start();
			
            if (_applicationsDto == null) return;
			foreach (ApplicationDto application in _applicationsDto)
				new ApplicationImporter(StorageManager, application).AnalyzeImportPackage();

            sw.Stop();
            LogHelper.Log.DebugFormat("ProcessChildren(...) Instition {0}. Elapsed: {1}s", InstitutionID, sw.Elapsed.TotalSeconds);
		}

		protected override BaseDto GetDtoObject()
		{
			return null;
		}

		protected override void CheckIntegrity()
		{
			if (_applicationsDto == null) return;
			
            //проверяем целостность
            //var sw = new Stopwatch();
            //sw.Start();
            ObjectIntegrityManager.CheckUIDUnique(_applicationsDto);
            //sw.Stop();
            //LogHelper.Log.InfoFormat("CheckUIDUnique(...) Instition {0}. Elapsed: {1}s", InstitutionID, sw.Elapsed.TotalSeconds);
            //sw.Restart();

		    ObjectIntegrityManager.CheckAppNumberUnique(_applicationsDto);
            //sw.Stop();
            //LogHelper.Log.InfoFormat("CheckAppNumberUnique(...) Instition {0}. Elapsed: {1}s", InstitutionID, sw.Elapsed.TotalSeconds);
            //sw.Restart();

			ObjectIntegrityManager.CheckAppNumberAndUIDRelated(_applicationsDto);
            //sw.Stop();
            //LogHelper.Log.InfoFormat("CheckAppNumberAndUIDRelated(...) Instition {0}. Elapsed: {1}s", InstitutionID, sw.Elapsed.TotalSeconds);
            //sw.Restart();

			ObjectIntegrityManager.CheckDictionaries(_applicationsDto);
            //sw.Stop();
            //LogHelper.Log.InfoFormat("CheckDictionaries(...) Instition {0}. Elapsed: {1}s", InstitutionID, sw.Elapsed.TotalSeconds);
            //sw.Restart();
		}
	}
}
