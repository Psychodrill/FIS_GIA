using GVUZ.Model.Institutions;
using GVUZ.ServiceModel.Import.Core.Operations.Conflicts;
using GVUZ.ServiceModel.Import.Core.Storages;
using GVUZ.ServiceModel.Import.WebService.Dto;

namespace GVUZ.ServiceModel.Import.Core.Operations.Importing.Applications
{
	/// <summary>
	/// Импорт коллекции заявлений
	/// </summary>
	public class RecommendedApplicationCollectionImporter : ObjectImporter
	{
        public RecommendedApplicationDto[] _recommendedApplications { get; private set; }

        public RecommendedApplicationCollectionImporter(StorageManager storageManager,
            RecommendedApplicationDto[] recommendedApplications) : base(storageManager)
		{
            _recommendedApplications = recommendedApplications;
		}

		protected override void CheckIntegrity()
		{
            foreach (var dto in _recommendedApplications)
            {
                /* Проверка справочных значений */
                ObjectIntegrityManager.CheckDictionaryValues(dto.DirectionID.ToString(), "DirectionID", dto, DbObjectRepository.GetDirection);
                ObjectIntegrityManager.CheckDictionaryValues(dto.EducationFormID.ToString(), "EducationFormID", dto, DbObjectRepository.GetEducationForm);
                ObjectIntegrityManager.CheckDictionaryValues(dto.FinanceSourceID.ToString(), "FinanceSourceID", dto, DbObjectRepository.GetFinanceSource);
                ObjectIntegrityManager.CheckDictionaryValues(dto.EducationLevelID.ToString(), "EducationLevelID", dto, DbObjectRepository.GetEducationalLevel);

                /* 1. Заявление не должно быть для Целевого приема. (метка в поле IsRequiresTargetO,  IsRequiresTargetOZ, IsRequiresTargetZ) */
                if (dto.FinanceSourceID == EDSourceConst.Target)
                    ConflictStorage.AddNotImportedDto(dto, ConflictMessages.FinFormSourceNotAllowedForRecommended);

                if (!dto.IsBroken)
                    InsertStorage.AddRecommendedApplication(dto);
            }
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
	        
	    }

	    protected override BaseDto GetDtoObject()
	    {
            return null;
	    }
	}
}
