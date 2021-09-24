using System;
using System.Collections.Generic;
using System.Linq;
using FogSoft.Helpers;
using GVUZ.ServiceModel.Import.Core.Operations.Conflicts;
using GVUZ.ServiceModel.Import.Core.Operations.Deleting;
using GVUZ.ServiceModel.Import.Core.Storages;
using GVUZ.ServiceModel.Import.WebService.Dto;

namespace GVUZ.ServiceModel.Import.Core.Operations.Importing
{
	public class CompetitiveGroupImporter : ObjectImporter
	{
		private readonly CompetitiveGroupDto _competitiveGroupDto;
		private readonly CompetitiveGroup _cgDb;

		public CompetitiveGroupImporter(StorageManager storageManager, CompetitiveGroupDto competitiveGroupDto) :
			base(storageManager)
		{
			_competitiveGroupDto = competitiveGroupDto;
			_cgDb = DbObjectRepository.GetObject<CompetitiveGroup>(_competitiveGroupDto.UID);
			ProcessedDtoStorage.AddCompetitiveGroup(_competitiveGroupDto);

		    /* грабли = если будет удаляться из бд - не надо обновлять, надо пытаться добавлять */
            if (_cgDb != null && storageManager.DeleteStorage.ContainsObject<CompetitiveGroup>(_competitiveGroupDto.UID))
                _cgDb = null;

            /* Поиск возможных ошибок при перехлестном обновлении индекса UK_CompetitiveGroup_UniqueInstitutionName */
            if (_cgDb != null && !_cgDb.Name.Trim().Equals(_competitiveGroupDto.Name.Trim(), StringComparison.InvariantCultureIgnoreCase))
            {
                /* Если имена не совпадают - ищем в БД с добавляемым именем КГ но с другим UID */
                var conflictedGroup = DbObjectRepository.CompetitiveGroups.FirstOrDefault(c =>
                    c.UID != _competitiveGroupDto.UID &&
                    c.Name.Trim().Equals(_competitiveGroupDto.Name.Trim(), StringComparison.InvariantCultureIgnoreCase));

                if (conflictedGroup != null)
                    ConflictStorage.AddNotImportedDto(_competitiveGroupDto,
                        ConflictMessages.CompetitiveGroupNameEsistsInDbWithOtherUID, conflictedGroup.Name, conflictedGroup.UID);
            }
		}

		protected override void FindInsertAndUpdate()
		{
			// новый объект
            if (ConflictStorage.HasConflictOrNotImported(_competitiveGroupDto))
            {
                return;
            }

			if (_cgDb == null)
			{
				// если уже существует КГ c таким же именем
				CompetitiveGroup cgWithSameName = DbObjectRepository.CompetitiveGroups.SingleOrDefault(x => 
                    x.Name.ToLower() == _competitiveGroupDto.Name.ToLower() && x.Campaign.UID == _competitiveGroupDto.CampaignUID);
				if (cgWithSameName != null)
				{
					ObjectDeletion deletion = new CompetitiveGroupDeletion(StorageManager, cgWithSameName, _competitiveGroupDto);
					if (!deletion.TryDelete())
					{
						ConflictStorage.AddNotImportedDto(_competitiveGroupDto, ConflictMessages.CompetitiveGroupWithSameNameExists);
						return;
					}
				}
				
				InsertStorage.AddCompetitiveGroup(_competitiveGroupDto);
			}
			// обновление, удаление, конфликт
			else
			{
				if (CanUpdate())
					// обновление
					UpdateStorage.AddCompetitiveGroup(_competitiveGroupDto);
				else
				{
					// удаление, конфликт
					var linkedApps = ObjectLinkManager.CompetitiveGroupLinkWithApplications(_cgDb).Select(x => 
                            new ApplicationShortRef { ApplicationNumber = x.ApplicationNumber, RegistrationDateDate = x.RegistrationDate }).ToArray();
					if (linkedApps.Length == 0)
					{
						//DeleteStorage.AddCompetitiveGroup(_cgDb);
						//удаляем из базы существующую и вставляем новую. По контексту этого кода, должно быть именно так
						//иначе логика очень непонятная тут будет. Max
						ObjectDeletion deletion = new CompetitiveGroupDeletion(StorageManager, _cgDb);
						if (!deletion.TryDelete())
						{
							ConflictStorage.AddNotImportedDto(_competitiveGroupDto, ConflictMessages.CompetitiveGroupCannotBeRemoved);
							return;
						}
						InsertStorage.AddCompetitiveGroup(_competitiveGroupDto);
					}
					else
					{
						ConflictStorage.AddApplications(_competitiveGroupDto, new HashSet<ApplicationShortRef>(linkedApps));
					}
				}
			}
		}

		protected override bool CanUpdate()
		{
			return _competitiveGroupDto.Course.To(0) == _cgDb.Course &&
				   _competitiveGroupDto.CampaignUID == _cgDb.Campaign.UID;
		}

		protected override void ProcessChildren(bool isParentConflict)
		{
			// направления КГ
			foreach (CompetitiveGroupItemDto cgItemDto in _competitiveGroupDto.Items)
			{
				cgItemDto.ParentUID = _competitiveGroupDto.UID;
			    if (isParentConflict)
			    {
			        ConflictStorage.AddNotImportedDto(cgItemDto, ConflictMessages.ParentObjectIsNotImported);
			    }
                //Если в КГ уже есть какой-нибудь уровень образования, то добавлять можно только такой же
                //исключение - бакалавриат и бакалавриат сокращенный
                //http://redmine.armd.ru/issues/20835
                if (_competitiveGroupDto.Items.Any(x => x.EducationLevelID != cgItemDto.EducationLevelID))
                    if ((_competitiveGroupDto.Items.Any(x => !(new List<int> {2, 3}).Contains(x.EducationLevelID.To(0)))
                         && !(new List<int> {2, 3}).Contains(cgItemDto.EducationLevelID.To(0))))
                    {
                        ConflictStorage.AddNotImportedDto(_competitiveGroupDto,
                                                          ConflictMessages.CompetitiveGroupCannotContainVariousEdlevels);
                        ConflictStorage.AddNotImportedDto(cgItemDto, ConflictMessages.ParentObjectIsNotImported);
                        isParentConflict = true;
                    }
			    new CompetitiveGroupItemImporter(StorageManager, cgItemDto).AnalyzeImportPackage();
			}

			// общие льготы КГ
			if(_competitiveGroupDto.CommonBenefit != null)
			foreach (BenefitItemDto benefitItemDto in _competitiveGroupDto.CommonBenefit)
			{
				benefitItemDto.ParentUID = _competitiveGroupDto.UID;
				benefitItemDto.IsCommonBenefit = true;
				if (isParentConflict)
					ConflictStorage.AddNotImportedDto(benefitItemDto, ConflictMessages.ParentObjectIsNotImported);
				new CommonBenefitImporter(StorageManager, benefitItemDto).AnalyzeImportPackage();
			}

			// целевой прием
			if (_competitiveGroupDto.TargetOrganizations != null)
			foreach (CompetitiveGroupTargetDto competitiveGroupTarget in _competitiveGroupDto.TargetOrganizations)
			{
				competitiveGroupTarget.ParentUID = _competitiveGroupDto.UID;
				if (isParentConflict)
					ConflictStorage.AddNotImportedDto(competitiveGroupTarget, ConflictMessages.ParentObjectIsNotImported);
				new CompetitiveGroupTargetImporter(StorageManager, competitiveGroupTarget, _competitiveGroupDto).AnalyzeImportPackage();
			}

			// вступительные испытания импорт
			var duplicateTests = new HashSet<string>();
			foreach (EntranceTestItemDto entranceTestItemDto in _competitiveGroupDto.EntranceTestItems)
			{
                entranceTestItemDto.ParentUID = _competitiveGroupDto.UID;
				if (isParentConflict)
					ConflictStorage.AddNotImportedDto(entranceTestItemDto, ConflictMessages.ParentObjectIsNotImported);

                /* Проверка значения EntranceTestTypeID справочника */
                if (DbObjectRepository.GetEntranceTestType(entranceTestItemDto.EntranceTestTypeID.To(0)) == null)
                {
                    ConflictStorage.AddNotImportedDto(entranceTestItemDto,
                        ConflictMessages.EntranceTestTypeIDNotFound, entranceTestItemDto.EntranceTestTypeID);
                    continue;
                }

                //Проверка приоритета вступительных испытаний группы на корректность данных
                int priority;
                bool isNumber = false;
                bool isNullNumber = false;

                isNullNumber = string.IsNullOrEmpty(entranceTestItemDto.EntranceTestPriority);

                isNumber = int.TryParse(entranceTestItemDto.EntranceTestPriority, out priority);
                if (!isNullNumber)
                    if (!isNumber || (isNumber && (priority < 1 || priority > 10)))
                    {
                        ConflictStorage.AddNotImportedDto(entranceTestItemDto,
                            ConflictMessages.EntranceTestPriorityIncorrect, entranceTestItemDto.UID);
                        continue;
                    }

                //Проверка приоритетов вступительных испытаний группы, как уже импортированных, так и существовавших до импорта
                var sameInPackage = _competitiveGroupDto.EntranceTestItems.FirstOrDefault(eti => eti.EntranceTestPriority == entranceTestItemDto.EntranceTestPriority && eti.UID != entranceTestItemDto.UID);
                var sameInDB = DbObjectRepository.CompetitiveGroupEntranceTestItems.FirstOrDefault(eti => eti.EntranceTestPriority.ToString() == entranceTestItemDto.EntranceTestPriority && eti.UID != entranceTestItemDto.UID && eti.CompetitiveGroup.UID == entranceTestItemDto.ParentUID);

                if (sameInDB != null || (sameInPackage != null && !isNullNumber))
                {
                    ConflictStorage.AddNotImportedDto(entranceTestItemDto,
                        ConflictMessages.EntranceTestPriorityIncorrect, entranceTestItemDto.UID);
                    continue;
                }

			    string uniqTestID = entranceTestItemDto.EntranceTestTypeID.ToString() + "@" +
				                  entranceTestItemDto.EntranceTestSubject.SubjectID + "@" +
								  (string.IsNullOrEmpty(entranceTestItemDto.EntranceTestSubject.SubjectID) ? entranceTestItemDto.EntranceTestSubject.SubjectName : "");
				if (duplicateTests.Contains(uniqTestID))
				{
					ConflictStorage.AddNotImportedDto(entranceTestItemDto, ConflictMessages.CompetitiveGroupEntranceTestDuplicateInPackage);
					continue;
				}
				duplicateTests.Add(uniqTestID);
				new CompetitiveGroupEntranceTestImporter(StorageManager, entranceTestItemDto).AnalyzeImportPackage();
			}
		}

		protected override BaseDto GetDtoObject()
		{
			return _competitiveGroupDto;
		}

		protected override void FindExcludedObjectsInDbForDelete()
		{
			// если импортируемый объект с указанным ID отсутствует в БД, то выходим
			if(_cgDb == null) return;			

			// определяем какие объекты из БД не перечислены в списке импорта и подлежат удалению
			// Направления КГ
			foreach (CompetitiveGroupItem cgItem in DbObjectRepository.CompetitiveGroupItems
				.Where(x => x.CompetitiveGroupID == _cgDb.CompetitiveGroupID))
			{
				if (!string.IsNullOrEmpty(cgItem.UID) &&
					_competitiveGroupDto.Items != null &&
					_competitiveGroupDto.Items.Any(x => x.UID == cgItem.UID)) continue;
				new CompetitiveGroupItemDeletion(StorageManager, cgItem).TryDelete();
			}
			
			// целевой прием
			foreach (CompetitiveGroupTarget cgTarget in DbObjectRepository.CompetitiveGroupTargets
				.Where(x => x.CompetitiveGroupTargetItem.Any(y => y.CompetitiveGroupItem.CompetitiveGroupID == _cgDb.CompetitiveGroupID)))
			{
				if (!string.IsNullOrEmpty(cgTarget.UID) &&
					_competitiveGroupDto.TargetOrganizations != null &&
					_competitiveGroupDto.TargetOrganizations.Any(x => x.UID == cgTarget.UID)) continue;

				new CompetitiveGroupTargetDeletion(StorageManager, cgTarget).TryDelete();
			}

			// общие льготы для КГ
			foreach (BenefitItemC benefitItem in DbObjectRepository.CompetitiveGroupCommonBenefitItems
				.Where(x => x.CompetitiveGroupID == _cgDb.CompetitiveGroupID))
			{
				if (!string.IsNullOrEmpty(benefitItem.UID) && 
					_competitiveGroupDto.CommonBenefit != null &&
					_competitiveGroupDto.CommonBenefit.Any(x => x.UID == benefitItem.UID)) continue;

				// удаление общей льготы в КГ
				new CompetitiveGroupBenefitItemDeletion(StorageManager, benefitItem, _competitiveGroupDto).TryDelete();
			}

			// льготы для ВИ
			foreach (BenefitItemC benefitItem in DbObjectRepository.CompetitiveGroupBenefitItemsForEntranceTest
				.Where(x => x.CompetitiveGroupID == _cgDb.CompetitiveGroupID))
			{
				if(!string.IsNullOrEmpty(benefitItem.UID))
				{
					BenefitItemDto benefitItemDto = null;
					foreach (var testItemDto in _competitiveGroupDto.EntranceTestItems)
					{
						if(testItemDto.EntranceTestBenefits != null)
							benefitItemDto = testItemDto.EntranceTestBenefits.Where(x => x.UID == benefitItem.UID).FirstOrDefault();
						if(benefitItemDto != null) break;
					}

					// если найдена льгота для ВИ ищем дальше.
					if (benefitItemDto != null) continue;
				}

				// удаление льготы в ВИ
				new CompetitiveGroupBenefitItemDeletion(StorageManager, benefitItem).TryDelete();
			}

			// вступительные испытания
			foreach (EntranceTestItemC entranceTestItem in DbObjectRepository.CompetitiveGroupEntranceTestItems
				.Where(x => x.CompetitiveGroupID == _cgDb.CompetitiveGroupID))
			{				
				if (!string.IsNullOrEmpty(entranceTestItem.UID) &&
					_competitiveGroupDto.EntranceTestItems.Any(x => x.UID == entranceTestItem.UID)) continue;

				new CompetitiveGroupEntranceTestItemDeletion(StorageManager, entranceTestItem).TryDelete();
			}
		}
	}
}