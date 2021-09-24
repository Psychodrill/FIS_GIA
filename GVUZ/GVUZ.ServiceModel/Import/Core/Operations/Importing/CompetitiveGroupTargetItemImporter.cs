using System.Linq;
using FogSoft.Helpers;
using GVUZ.ServiceModel.Import.Core.Operations.Conflicts;
using GVUZ.ServiceModel.Import.Core.Operations.Deleting;
using GVUZ.ServiceModel.Import.Core.Storages;
using GVUZ.ServiceModel.Import.WebService.Dto;

namespace GVUZ.ServiceModel.Import.Core.Operations.Importing
{
    /// <summary>
    /// Загрузка мест целевого приема
    /// </summary>
    public class CompetitiveGroupTargetItemImporter : ObjectImporter
	{
		private readonly CompetitiveGroupTargetItemDto _competitiveGroupTargetItemDto;
		private readonly CompetitiveGroupTargetItem _competitiveGroupTargetItemDb;
		private readonly CompetitiveGroupTargetDto _competitiveGroupTargetDto;

		public CompetitiveGroupTargetItemImporter(StorageManager storageManager, 
			CompetitiveGroupTargetItemDto competitiveGroupTargetItemDto, CompetitiveGroupTargetDto parentOrg) : base(storageManager)
		{
			_competitiveGroupTargetItemDto = competitiveGroupTargetItemDto;
			_competitiveGroupTargetItemDb = DbObjectRepository.GetObject<CompetitiveGroupTargetItem>(_competitiveGroupTargetItemDto.UID);
			_competitiveGroupTargetDto = parentOrg;
            ProcessedDtoStorage.AddCompetitiveGroupTargetItem(_competitiveGroupTargetItemDto);

            if (_competitiveGroupTargetItemDb != null &&
                _competitiveGroupTargetItemDb.CompetitiveGroupTarget.UID != parentOrg.UID)
            {
                ConflictStorage.AddNotImportedDto(_competitiveGroupTargetItemDto,
                    ConflictMessages.CompetitiveGroupTargetItemExistsInDbInOtherCompetitiveGroupTarget,
                    _competitiveGroupTargetItemDto.UID,
                    _competitiveGroupTargetItemDb.CompetitiveGroupTarget.UID);
                _competitiveGroupTargetItemDb = null;
                return;
            }

            /* грабли = если будет удаляться из бд - не надо обновлять, надо пытаться добавлять */
            if (storageManager.DeleteStorage.ContainsObject<CompetitiveGroupTargetItem>(_competitiveGroupTargetItemDto.UID))
                _competitiveGroupTargetItemDb = null;	
		}

		protected override void FindInsertAndUpdate()
		{
			if(_competitiveGroupTargetItemDb == null)
				InsertStorage.AddCompetitiveGroupTargetItem(_competitiveGroupTargetItemDto);
			else
			{
				if (CanUpdate())
					UpdateStorage.AddCompetitiveGroupTargetItem(_competitiveGroupTargetItemDto);
				else
				{
					//уже в конфликте
					if (ConflictStorage.HasConflictOrNotImported(_competitiveGroupTargetItemDto))
						return;
					// удаление, конфликт
					ObjectDeletion objectDeletion = new CompetitiveGroupTargetItemDeletion(StorageManager, _competitiveGroupTargetItemDb, _competitiveGroupTargetItemDto);
					if (objectDeletion.TryDelete())
						InsertStorage.AddCompetitiveGroupTargetItem(_competitiveGroupTargetItemDto);
					else
						ConflictStorage.AddNotImportedDto(_competitiveGroupTargetItemDto, objectDeletion.ConflictDeletionList);
				}
			}
		}

		protected override bool CanUpdate()
		{
			//пошло на удаление
			if (DeleteStorage.CompetitiveGroupTargets.Any(x => x.UID == _competitiveGroupTargetItemDto.ParentUID))
				return false;

			var competitiveGroupTarget = DbObjectRepository.GetObject<CompetitiveGroupTarget>(_competitiveGroupTargetItemDto.ParentUID);

			// нам передали направления, с существующим уидом, но у родительской организации другой уид
			// здесь не очень правильная проверка и возврат ошибки про это, но иначе мы пометим старую запись на удаление
			// и добавим новую
			if (competitiveGroupTarget == null || competitiveGroupTarget.UID != _competitiveGroupTargetItemDto.ParentUID)
			{
				ConflictStorage.AddNotImportedDto(_competitiveGroupTargetItemDto,
					ConflictMessages.UIDMustBeUniqueForAllObjectInstancesOfType,
					"Места для целевого приема", _competitiveGroupTargetItemDto.UID);
				ConflictStorage.AddNotImportedDto(_competitiveGroupTargetDto, ConflictMessages.UIDMustBeUniqueForChildrenObjects,
					"Места для целевого приема", _competitiveGroupTargetItemDto.UID);
				return false;
			}

            return
                _competitiveGroupTargetItemDb.CompetitiveGroupItem.DirectionID == _competitiveGroupTargetItemDto.DirectionID.To(0) &&
				_competitiveGroupTargetItemDb.CompetitiveGroupItem.EducationLevelID == _competitiveGroupTargetItemDto.EducationLevelID.To(0) &&
				_competitiveGroupTargetItemDb.CompetitiveGroupTargetID == competitiveGroupTarget.CompetitiveGroupTargetID;
		}

		protected override void ProcessChildren(bool isParentConflict)
		{
			// нет дочерних объектов
		}

		protected override BaseDto GetDtoObject()
		{
			return _competitiveGroupTargetItemDto;
		}

		protected override void FindExcludedObjectsInDbForDelete()
		{
			// нет дочерних объектов
		}
	}
}
