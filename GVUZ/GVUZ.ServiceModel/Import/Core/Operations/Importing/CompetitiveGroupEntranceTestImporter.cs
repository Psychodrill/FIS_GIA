using System;
using System.Linq;
using FogSoft.Helpers;
using GVUZ.ServiceModel.Import.Core.Operations.Conflicts;
using GVUZ.ServiceModel.Import.Core.Operations.Deleting;
using GVUZ.ServiceModel.Import.Core.Storages;
using GVUZ.ServiceModel.Import.WebService.Dto;

namespace GVUZ.ServiceModel.Import.Core.Operations.Importing
{
	public class CompetitiveGroupEntranceTestImporter : ObjectImporter
	{
		private readonly EntranceTestItemDto _entranceTestItemDto;
		private readonly EntranceTestItemC _entranceTestItem;

		public CompetitiveGroupEntranceTestImporter(StorageManager storageManager, EntranceTestItemDto entranceTestItemDto) :
			base(storageManager)
		{
			_entranceTestItemDto = entranceTestItemDto;
			_entranceTestItem = DbObjectRepository.GetObject<EntranceTestItemC>(_entranceTestItemDto.UID);
            ProcessedDtoStorage.AddCompetitiveGroupEntranceTestItem(_entranceTestItemDto);

            //если уже помечено на удаление, то не смотрим на то, что есть в базе
            if(storageManager.DeleteStorage.EntranceTestItems.Any(x => x.UID == _entranceTestItemDto.UID))
				_entranceTestItem = null;

		    if (_entranceTestItem != null && _entranceTestItem.CompetitiveGroup.UID != _entranceTestItemDto.ParentUID)
		    {
                ConflictStorage.AddNotImportedDto(_entranceTestItemDto,
                    ConflictMessages.EntranceTestItemCExistsInDbInOtherCompetitiveGroup,
                    _entranceTestItemDto.GetDescription(),
                    _entranceTestItem.CompetitiveGroup.UID);
                _entranceTestItem = null;
                return;
		    }

            /* грабли = если будет удаляться из бд - не надо обновлять, надо пытаться добавлять */
            if (storageManager.DeleteStorage.ContainsObject<EntranceTestItemC>(_entranceTestItemDto.UID))
                _entranceTestItem = null;		    
		}

		protected override void FindInsertAndUpdate()
		{
			if (_entranceTestItem == null)
			{
				// если уже существует ВИ с такими же данными
				CompetitiveGroup cgDb = DbObjectRepository.CompetitiveGroups.SingleOrDefault(x => 
                    x.UID == _entranceTestItemDto.ParentUID);

				if (cgDb != null)
				{
					int? subjectID = String.IsNullOrWhiteSpace(_entranceTestItemDto.EntranceTestSubject.SubjectID) ?
						(int?) null : _entranceTestItemDto.EntranceTestSubject.SubjectID.To(0);

                    if (subjectID != null && DbObjectRepository.GetSubject(subjectID.Value) == null)
                    {
                        ConflictStorage.AddConflictWithCustomMessage(
                            _entranceTestItemDto, new ConflictStorage.ConflictMessage
                            {
                                Code = ConflictMessages.SubjectIsNotFounded,
                                Message = String.Format(ConflictMessages.GetMessage(ConflictMessages.SubjectIsNotFounded), 
                                subjectID)
                            });
                        return;
                    }

					EntranceTestItemC sameEntrTestItem = DbObjectRepository.CompetitiveGroupEntranceTestItems.SingleOrDefault(x => 
                        x.CompetitiveGroupID == cgDb.CompetitiveGroupID && 
	                    x.SubjectID == subjectID &&
	                    x.SubjectName == _entranceTestItemDto.EntranceTestSubject.SubjectName &&
	                    x.EntranceTestTypeID == _entranceTestItemDto.EntranceTestTypeID.To(0));

					if (sameEntrTestItem != null)
					{
						ObjectDeletion deletion = new CompetitiveGroupEntranceTestItemDeletion(StorageManager, sameEntrTestItem,
							_entranceTestItemDto);
						if (!deletion.TryDelete())
						{
							ConflictStorage.AddConflictWithCustomMessage(_entranceTestItemDto, 
								new ConflictStorage.ConflictMessage()
									{
										Code = ConflictMessages.CompetitiveGroupEntranceTestItemUniqueConstraint,
										Message = String.Format(
											ConflictMessages.GetMessage(ConflictMessages.CompetitiveGroupEntranceTestItemUniqueConstraint),
											sameEntrTestItem.UID)
									});
							return;
						}
					}
				}

				InsertStorage.AddCompetitiveGroupEntranceTestItem(_entranceTestItemDto);
			}
			else
			{
				//#22773 если в базе присутствует элемент льготы для ВИ, но в импортируемом пакете его нет, происходит ошибка.
				//В данном случае элемент льготы для ВИ должен выдаваться в связанных данных.
				if(ContainsDifferentDbData())
				{
					ConflictStorage.AddNotImportedDto(_entranceTestItemDto, ConflictMessages.EntranceTestBenefitItemNotExistsInPackage);
				}
				else
				{
					if(CanUpdate())
						UpdateStorage.AddCompetitiveGroupEntranceTestItem(_entranceTestItemDto);
					else
					{
						ObjectDeletion objectDeletion = new CompetitiveGroupEntranceTestItemDeletion(StorageManager, _entranceTestItem, _entranceTestItemDto);
						if (objectDeletion.TryDelete())
							InsertStorage.AddCompetitiveGroupEntranceTestItem(_entranceTestItemDto);
						else
							ConflictStorage.AddNotImportedDto(_entranceTestItemDto, objectDeletion.ConflictDeletionList);
					}
				}
			}			
		}

		/*
		 * В проверке ID предмета нет необходимости, т.к. ещё на проверке целостности по вхождению предметов в КГ
		 * отсекут не существующие предметы.
		 * protected override void CheckIntegrity()
		{
			// проверка на существование предмета
			if (_entranceTestItemDto.EntranceTestSubject.SubjectID != null)
				ObjectIntegrityManager.CheckIsSubjectExists(_entranceTestItemDto.EntranceTestSubject.SubjectID.To(0), 
					_entranceTestItemDto);
		}*/

		protected override bool CanUpdate()
		{
			// ВИ в КГ: если изменился предмет, то удаление
			int subjectIDDb = _entranceTestItem.SubjectID.HasValue ? _entranceTestItem.SubjectID.Value : 0;
			int subjectIDDto = String.IsNullOrEmpty(_entranceTestItemDto.EntranceTestSubject.SubjectID) ?
				0 : _entranceTestItemDto.EntranceTestSubject.SubjectID.To(-1);
			if (subjectIDDb != subjectIDDto)
				return false;
			return true;
		}

		protected virtual bool ContainsDifferentDbData()
		{
			var benefitsDb = _entranceTestItem.BenefitItemC.Select(x => x.UID).ToArray();
			var benefitsDto = _entranceTestItemDto.EntranceTestBenefits == null ?
				new string[0] : _entranceTestItemDto.EntranceTestBenefits.Select(x => x.UID).ToArray();			
			if (benefitsDb.Except(benefitsDto).Any())
				return true;
			return false;
		}

		protected override void ProcessChildren(bool isParentConflict)
		{
			if (_entranceTestItemDto.EntranceTestBenefits != null)
			foreach (BenefitItemDto benefitItemDto in _entranceTestItemDto.EntranceTestBenefits)
			{
				benefitItemDto.ParentUID = _entranceTestItemDto.UID;
				benefitItemDto.IsCommonBenefit = false;
				if (isParentConflict)
					ConflictStorage.AddNotImportedDto(benefitItemDto, ConflictMessages.ParentObjectIsNotImported);
				new CompetitiveGroupEntranceTestBenefitImporter(StorageManager, benefitItemDto).AnalyzeImportPackage();
			}
		}

		protected override BaseDto GetDtoObject()
		{
			return _entranceTestItemDto;
		}

		protected override void FindExcludedObjectsInDbForDelete()
		{
			if (_entranceTestItem == null) return;

			// льготы для ВИ в КГ
			foreach (BenefitItemC competitiveGroupBenefitItem in DbObjectRepository.CompetitiveGroupBenefitItemsForEntranceTest
				.Where(x => x.EntranceTestItemID == _entranceTestItem.EntranceTestItemID))
			{
				if(!string.IsNullOrEmpty(competitiveGroupBenefitItem.UID) &&
					_entranceTestItemDto.EntranceTestBenefits != null && 
					_entranceTestItemDto.EntranceTestBenefits.Any(x => x.UID == competitiveGroupBenefitItem.UID)) continue;
				new CompetitiveGroupBenefitItemDeletion(StorageManager, competitiveGroupBenefitItem, _entranceTestItemDto);
			}
		}
	}
}