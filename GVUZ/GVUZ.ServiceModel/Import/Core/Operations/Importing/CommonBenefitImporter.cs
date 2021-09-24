using System;
using FogSoft.Helpers;
using System.Linq;
using GVUZ.Model;
using GVUZ.ServiceModel.Import.Core.Operations.Conflicts;
using GVUZ.ServiceModel.Import.Core.Operations.Deleting;
using GVUZ.ServiceModel.Import.Core.Storages;
using GVUZ.ServiceModel.Import.WebService.Dto;

namespace GVUZ.ServiceModel.Import.Core.Operations.Importing
{
	public class CommonBenefitImporter : ObjectImporter
	{
		private readonly BenefitItemDto _benefitItemDto;
		private readonly BenefitItemC _benefitItemDb;

		public CommonBenefitImporter(StorageManager storageManager, BenefitItemDto benefitItemDto) :
			base(storageManager)
		{
            _benefitItemDto = benefitItemDto;
			_benefitItemDb = DbObjectRepository.GetCommonBenefitItemCObject(_benefitItemDto.UID);
            ProcessedDtoStorage.AddCompetitiveGroupBenefitItem(_benefitItemDto);

		    /* попытка добавления дубликата UID в БД */
		    if (_benefitItemDb != null && _benefitItemDb.CompetitiveGroup.UID != _benefitItemDto.ParentUID)
		    {
                ConflictStorage.AddNotImportedDto(_benefitItemDto,
                    ConflictMessages.BenefitItemCExistsInDbInOtherCompetitiveGroup,
                    _benefitItemDto.UID,
                    _benefitItemDb.CompetitiveGroup.UID);
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
				InsertStorage.AddCompetitiveGroupBenefitItem(_benefitItemDto);
			else
			{
				CompetitiveGroupDto cgDto = ProcessedDtoStorage.FindCompetitiveGroupDto(_benefitItemDto.ParentUID);
				_benefitItemDb.BenefitItemCOlympicType.Load();
                //// если льгота поменялась, то удаляем.
                //if (_benefitItemDb.UID != _benefitItemDto.UID ||
                //    _benefitItemDb.OlympicDiplomTypeID != _benefitItemDto.OlympicDiplomTypesParsed ||
                //    _benefitItemDb.BenefitID != _benefitItemDto.BenefitKindID.To(0) ||
                //    _benefitItemDb.IsForAllOlympic != _benefitItemDto.IsForAllOlympics.To(false) ||
                //    _benefitItemDb.CompetitiveGroup.UID != cgDto.UID ||
                //    // список олимпиад изменился
                //    !ArrayExtensions.ArraysEqual(_benefitItemDb.BenefitItemCOlympicType.Select(x => x.OlympicTypeID.ToString()).ToArray(), _benefitItemDto.Olympics))
				{// Удаляем льготу в любом случае
					// Льг в КГ: любое изменение приравниваем к удалению
					ObjectDeletion deletion = new CompetitiveGroupBenefitItemDeletion(StorageManager, _benefitItemDb,
						_benefitItemDto);
					if (deletion.TryDelete())
						InsertStorage.AddCompetitiveGroupBenefitItem(_benefitItemDto);
				}
/*
				else
					ConflictStorage.AddConflict(_benefitItemDto, deletion.ConflictDeletionList);
*/
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
