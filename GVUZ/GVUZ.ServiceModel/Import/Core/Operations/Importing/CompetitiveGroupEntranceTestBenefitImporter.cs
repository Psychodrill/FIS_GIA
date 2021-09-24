using FogSoft.Helpers;
using GVUZ.Model;
using GVUZ.ServiceModel.Import.Core.Operations.Conflicts;
using GVUZ.ServiceModel.Import.Core.Operations.Deleting;
using GVUZ.ServiceModel.Import.Core.Storages;
using GVUZ.ServiceModel.Import.WebService.Dto;

namespace GVUZ.ServiceModel.Import.Core.Operations.Importing
{
	public class CompetitiveGroupEntranceTestBenefitImporter : ObjectImporter
	{
		private readonly BenefitItemDto _benefitItemDto;
		private readonly BenefitItemC _benefitItemDb;

		public CompetitiveGroupEntranceTestBenefitImporter(StorageManager storageManager, BenefitItemDto benefitItemDto) :
			base(storageManager)
		{
			_benefitItemDto = benefitItemDto;
			_benefitItemDb = DbObjectRepository.GetObject<BenefitItemC>(_benefitItemDto.UID);
			ProcessedDtoStorage.AddCompetitiveGroupBenefitItem(_benefitItemDto);

		    /* попытка добавления дубликата UID в БД */
            if (_benefitItemDb != null && _benefitItemDb.EntranceTestItemC.UID != _benefitItemDto.ParentUID)
		    {
                ConflictStorage.AddNotImportedDto(_benefitItemDto,
                    ConflictMessages.BenefitItemCExistsInDbInOtherEntranceTestItemC,
                    _benefitItemDto.UID,
                    _benefitItemDb.EntranceTestItemC.UID);
                _benefitItemDb = null;
		        return;
		    }

            /* грабли = если будет удаляться из бд - не надо обновлять, надо пытаться добавлять */
            if (storageManager.DeleteStorage.ContainsObject<BenefitItemC>(_benefitItemDto.UID))
                _benefitItemDb = null;
		}

		protected override void FindInsertAndUpdate()
		{
			if (_benefitItemDb == null)
				InsertStorage.AddCompetitiveGroupEntranceTestBenefitItem(_benefitItemDto);
			else
			{
				ObjectDeletion objectDeletion = new CompetitiveGroupBenefitItemDeletion(StorageManager, _benefitItemDb, _benefitItemDto);
			    if (objectDeletion.TryDelete())
			        InsertStorage.AddCompetitiveGroupEntranceTestBenefitItem(_benefitItemDto);
			    else
			        ConflictStorage.AddNotImportedDto(_benefitItemDto, objectDeletion.ConflictDeletionList);
			}
		}

		protected override bool CanUpdate()
		{
			return true;
		}

		protected override void ProcessChildren(bool isParentConflict)
		{
			// отсутствуют дочерние объекты
		}

		protected override BaseDto GetDtoObject()
		{
			return _benefitItemDto;
		}

		protected override void FindExcludedObjectsInDbForDelete()
		{
			// отсутствуют дочерние объекты
		}

        protected override void CheckIntegrity()
        {
            //// В случае предоставления льготы призерам олимпиады должна быть предоставлена льгота того же или более высокого порядка также и победителям олимпиады
            //if (_benefitItemDto.OlympicDiplomTypesParsed.IsFlagSet(2) && !_benefitItemDto.OlympicDiplomTypesParsed.IsFlagSet(1))
            //{
            //    ConflictStorage.AddNotImportedDto(_benefitItemDto, ConflictMessages.BenefitNotContainsWinnerBenefitFlag);
            //}
        }
	}
}
