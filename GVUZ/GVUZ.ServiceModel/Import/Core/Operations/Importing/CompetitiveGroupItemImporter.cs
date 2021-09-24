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
    public class CompetitiveGroupItemImporter : ObjectImporter
    {
        private readonly CompetitiveGroupItemDto _competitiveGroupItemDto;
        private readonly CompetitiveGroupItem _competitiveGroupItemDb;

        public CompetitiveGroupItemImporter(StorageManager storageManager, CompetitiveGroupItemDto competitiveGroupItemDto) :
            base(storageManager)
        {
            _competitiveGroupItemDto = competitiveGroupItemDto;
            _competitiveGroupItemDb = DbObjectRepository.GetObject<CompetitiveGroupItem>(_competitiveGroupItemDto.UID);
            ProcessedDtoStorage.AddCompetitiveGroupItem(_competitiveGroupItemDto);

            if (_competitiveGroupItemDb != null && _competitiveGroupItemDb.CompetitiveGroup.UID != _competitiveGroupItemDto.ParentUID)
            {
                ConflictStorage.AddNotImportedDto(_competitiveGroupItemDto,
                    ConflictMessages.CompetitiveGroupItemExistsInDbInOtherCompetitiveGroup,
                    _competitiveGroupItemDto.UID,
                    _competitiveGroupItemDb.CompetitiveGroup.UID);
                _competitiveGroupItemDb = null;
                return;
            }

            /* грабли = если будет удаляться из бд - не надо обновлять, надо пытаться добавлять */
            if (storageManager.DeleteStorage.ContainsObject<CompetitiveGroupItem>(_competitiveGroupItemDto.UID))
                _competitiveGroupItemDb = null;
        }

        protected override void FindInsertAndUpdate()
		{
			if (_competitiveGroupItemDb == null)
			{
				// если уже существует Направление КГ на это направлениеКГ
				CompetitiveGroup cgDb = DbObjectRepository.CompetitiveGroups.SingleOrDefault(x => x.UID == _competitiveGroupItemDto.ParentUID);
				if (cgDb != null)
				{

#warning arzyanin - добавил условие по EducationLevelID
                    CompetitiveGroupItem sameCgItem = DbObjectRepository.CompetitiveGroupItems
                        .Where(x => x.CompetitiveGroupID == cgDb.CompetitiveGroupID)
                        .ToArray()//Необходимо, в IQueryable не вписываются условия ниже
                        .SingleOrDefault(x =>
                        x.EducationLevelID == _competitiveGroupItemDto.EducationLevelID.To(0)
                        && x.DirectionID == _competitiveGroupItemDto.DirectionID.To(0));

					if (sameCgItem != null)
					{
						ObjectDeletion deletion = new CompetitiveGroupItemDeletion(StorageManager, sameCgItem, _competitiveGroupItemDto);
						if (!deletion.TryDelete())
						{
							ConflictStorage.AddNotImportedDto(_competitiveGroupItemDto, ConflictMessages.CompetitiveGroupItemOnSameDirectionExists);
							return;
						}
					}
                    ////Если в КГ уже есть какой-нибудь уровень образования, то добавлять можно только такой же
                    //исключение - бакалавриат и бакалавриат сокращенный
                    //http://redmine.armd.ru/issues/20835
                    List<CompetitiveGroupItem> otherCgItems = DbObjectRepository.CompetitiveGroupItems.Where(x =>
                        x.CompetitiveGroupID == cgDb.CompetitiveGroupID).ToList();
                    if(otherCgItems.Count > 0)
                        if (otherCgItems.Any(x => x.EducationLevelID != _competitiveGroupItemDto.EducationLevelID.To(0)))
                            if((otherCgItems.Any(x => !(new List<int>{2, 3}).Contains(x.EducationLevelID)) 
                            && !(new List<int>{2, 3}).Contains(_competitiveGroupItemDto.EducationLevelID.To(0))))
                        {
                            ConflictStorage.AddNotImportedDto(_competitiveGroupItemDto, ConflictMessages.CompetitiveGroupCannotContainVariousEdlevels);
							return;
                        }
				}

				InsertStorage.AddCompetitiveGroupItem(_competitiveGroupItemDto);
			}
			else
			{
				if (CanUpdate())
					UpdateStorage.AddCompetitiveGroupItem(_competitiveGroupItemDto);
				else
				{
					// удаление, конфликт
					ObjectDeletion objectDeletion = new CompetitiveGroupItemDeletion(StorageManager, _competitiveGroupItemDb, _competitiveGroupItemDto);
					if (objectDeletion.TryDelete())
						InsertStorage.AddCompetitiveGroupItem(_competitiveGroupItemDto);
					else
						ConflictStorage.AddNotImportedDto(_competitiveGroupItemDto, objectDeletion.ConflictDeletionList);
				}
			}
		}

        protected override bool CanUpdate()
        {
            //КГ пошла на удаление, апдейтить себя нельзя
            if (DeleteStorage.CompetitiveGroups.Any(x => x.UID == _competitiveGroupItemDto.ParentUID))
                return false;
            var cg = DbObjectRepository.GetObject<CompetitiveGroup>(_competitiveGroupItemDto.ParentUID);
            // если родитель (КГ) только ещё добавляется, то нужно удалить НКГ существующее и вставить новое
            if (cg == null) return false;
            return _competitiveGroupItemDb.DirectionID == _competitiveGroupItemDto.DirectionID.To(0)
                        && _competitiveGroupItemDb.EducationLevelID == _competitiveGroupItemDto.EducationLevelID.To(0)
                        && cg.CompetitiveGroupID == _competitiveGroupItemDb.CompetitiveGroupID;
        }

        protected override void ProcessChildren(bool isParentConflict)
        {
            // нет дочерних объектов
        }

        protected override BaseDto GetDtoObject()
        {
            return _competitiveGroupItemDto;
        }

        protected override void FindExcludedObjectsInDbForDelete()
        {
            if (_competitiveGroupItemDb == null) return;
            // направления целевого приема с привязкой к направлению
            foreach (CompetitiveGroupTargetItem targetItem in DbObjectRepository.CompetitiveGroupTargetItems
                .Where(x => x.CompetitiveGroupItemID == _competitiveGroupItemDb.CompetitiveGroupItemID))
            {
                if (!String.IsNullOrEmpty(targetItem.UID) &&
                    _competitiveGroupItemDb.CompetitiveGroupTargetItem.Any(x => x.UID == targetItem.UID)) continue;

                new CompetitiveGroupTargetItemDeletion(StorageManager, targetItem);
            }
        }
    }
}
