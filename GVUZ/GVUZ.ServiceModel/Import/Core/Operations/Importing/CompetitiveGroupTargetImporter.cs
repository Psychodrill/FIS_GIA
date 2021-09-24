using System;
using System.Linq;
using FogSoft.Helpers;
using GVUZ.ServiceModel.Import.Core.Operations.Conflicts;
using GVUZ.ServiceModel.Import.Core.Operations.Deleting;
using GVUZ.ServiceModel.Import.Core.Operations.Deleting.Applications;
using GVUZ.ServiceModel.Import.Core.Storages;
using GVUZ.ServiceModel.Import.WebService.Dto;

namespace GVUZ.ServiceModel.Import.Core.Operations.Importing
{
	public class CompetitiveGroupTargetImporter : ObjectImporter
	{
		private readonly CompetitiveGroupTargetDto _competitiveGroupTargetDto;
		private readonly CompetitiveGroupTarget _competitiveGroupTargetDb;
		private readonly CompetitiveGroupDto _competitiveGroupDto;

		public CompetitiveGroupTargetImporter(StorageManager storageManager, CompetitiveGroupTargetDto competitiveGroupTargetDto, CompetitiveGroupDto competitiveGroupDto) :
			base(storageManager)
		{
			_competitiveGroupTargetDto = competitiveGroupTargetDto;
            _competitiveGroupDto = competitiveGroupDto;
			_competitiveGroupTargetDb = DbObjectRepository.GetObject<CompetitiveGroupTarget>(_competitiveGroupTargetDto.UID);
            ProcessedDtoStorage.AddCompetitiveGroupTarget(_competitiveGroupTargetDto);

//#warning arzyanin - одна и та же организация может быть в разных КГ
//            if (_competitiveGroupTargetDb != null &&
//                _competitiveGroupTargetDb.CompetitiveGroupTargetItem.Any() &&
//                _competitiveGroupTargetDb.CompetitiveGroupTargetItem.All(c =>
//                    c.CompetitiveGroupItem.CompetitiveGroup.UID != _competitiveGroupTargetDto.ParentUID))
//            {
//                var stub = "";
//                //ConflictStorage.AddNotImportedDto(_competitiveGroupTargetDto,
//                //    ConflictMessages.CompetitiveGroupTargetExistsInDbInOtherCompetitiveGroup,
//                //    _competitiveGroupTargetDto.UID);
//                //_competitiveGroupTargetDb = null;
//                //return;
//            }

            /* грабли = если будет удаляться из бд - не надо обновлять, надо пытаться добавлять */
            if (storageManager.DeleteStorage.ContainsObject<CompetitiveGroupTarget>(_competitiveGroupTargetDto.UID))
                _competitiveGroupTargetDb = null;	
		}

		protected override void FindInsertAndUpdate()
		{
			if (_competitiveGroupTargetDb == null)
				InsertStorage.AddCompetitiveGroupTarget(_competitiveGroupTargetDto);
			else
			{
				if (CanUpdate())
					// обновить 
					UpdateStorage.AddCompetitiveGroupTarget(_competitiveGroupTargetDto);
				else
				{
					// удаление, конфликт
					var objectDeletion = new CompetitiveGroupTargetDeletion(StorageManager, _competitiveGroupTargetDb, _competitiveGroupTargetDto);
					if (objectDeletion.TryDelete())
						InsertStorage.AddCompetitiveGroupTarget(_competitiveGroupTargetDto);
					else
						ConflictStorage.AddNotImportedDto(_competitiveGroupTargetDto, objectDeletion.ConflictDeletionList);
				}
			}			
		}

		protected override bool CanUpdate()
		{
			//КГ пошла на удаление, апдейтить себя нельзя
			if (DeleteStorage.CompetitiveGroups.Any(x => x.UID == _competitiveGroupTargetDto.ParentUID))
				return false;
			var cg = DbObjectRepository.GetObject<CompetitiveGroup>(_competitiveGroupTargetDto.ParentUID);
			if (cg == null) return false;
			return true;
			//return _competitiveGroupTargetDb.CompetitiveGroupID == cg.CompetitiveGroupID;
		}

		protected override void ProcessChildren(bool isParentConflict)
		{
			// импорт направлений целевого приема
			foreach (var competitiveGroupTargetItemDto in _competitiveGroupTargetDto.Items)
			{
				competitiveGroupTargetItemDto.ParentUID = _competitiveGroupTargetDto.UID;
                competitiveGroupTargetItemDto.CompetitiveGroupUID = _competitiveGroupDto.UID;
				if (isParentConflict)
					ConflictStorage.AddNotImportedDto(competitiveGroupTargetItemDto, ConflictMessages.ParentObjectIsNotImported);
				new CompetitiveGroupTargetItemImporter(StorageManager, competitiveGroupTargetItemDto, _competitiveGroupTargetDto).AnalyzeImportPackage();
			}
		}

		protected override BaseDto GetDtoObject()
		{
			return _competitiveGroupTargetDto;
		}

		protected override void FindExcludedObjectsInDbForDelete()
		{
			if (_competitiveGroupTargetDb == null) return;

			// направления целевого приема с привязкой к ЦП
			foreach (var targetItem in DbObjectRepository.CompetitiveGroupTargetItems
				.Where(x => x.CompetitiveGroupTargetID == _competitiveGroupTargetDb.CompetitiveGroupTargetID))
			{
				if (String.IsNullOrEmpty(targetItem.UID) &&
					_competitiveGroupTargetDto.Items.Any(x => x.UID == targetItem.UID)) continue;

    			new CompetitiveGroupTargetItemDeletion(StorageManager, targetItem);
			}
		}
	}
}
