using System.Diagnostics;
using FogSoft.Helpers;
using GVUZ.ServiceModel.Import.Core.Storages;
using GVUZ.ServiceModel.Import.WebService.Dto;

namespace GVUZ.ServiceModel.Import.Core.Operations.Importing
{
	/// <summary>
	/// Обертка для DTO объектов, по которым производится обновление или вставка.
	/// Удаление объектов в БД обрабатывается отдельным менеджером ObjectConflictDeleteManager.
	/// </summary>
	public abstract class ObjectImporter : StorageConsumer
	{
		public ObjectIntegrityManager ObjectIntegrityManager;
		public ObjectLinkManager ObjectLinkManager;		

		protected ObjectImporter(StorageManager storageManager) : base(storageManager)
		{
			ObjectIntegrityManager = new ObjectIntegrityManager(storageManager);						
			ObjectLinkManager = new ObjectLinkManager(storageManager);
		}

		/// <summary>
		/// Проверка целостности объекта проводится Importer'ом объекта, который стоит на уровень выше по иерархии.
		/// ВАЖНО: заполнять у dto объектов перед проверкой целостности свойство ParentUID. 
		/// Это необходимо для того, чтобы выдать информацию о результатах импорта в логе импорта.
		/// Если объект попадет в неимпортированные и не заполнить это свойство, то не будет выдана информация от родителя.
		/// </summary>
		protected virtual void CheckIntegrity() { }

		/// <summary>
		/// Пытаемся добавить на удаление объекты, которые не перечислены в списке импорта (определяются на уровне импортера объекта-родителя)		
		/// </summary>
		protected abstract void FindExcludedObjectsInDbForDelete();

		/// <summary>
		/// Находим объекты для вставки и обновления.
		/// ВАЖНО: Перед вызовом импортеров дочерних dto объектов заполнять свойство ParentUID.
		/// </summary>
		protected abstract void FindInsertAndUpdate();

		/// <summary>
		/// Метод определяющий нужно ли обновлять существующий объект в БД или удалять его. 
		/// </summary>
		protected abstract bool CanUpdate();

		/// <summary>
		/// Выполняем загрузку дочерних объектов.
		/// </summary>
		protected abstract void ProcessChildren(bool isParentConflict);

		public virtual void AnalyzeImportPackage()
		{
		    var sw = new Stopwatch();
            sw.Start();

			// не проводим анализ пакета, который попал в конфликт
			if (IsDtoObjectInConflict())
			{
				ProcessChildren(true);
				return;
			}

			CheckIntegrity();

			// проверяем на конфликты после проверки целостности
			if (IsDtoObjectInConflict())
			{
				ProcessChildren(true);
				return;				
			}

			FindInsertAndUpdate();

			// после обработки текущего объекта - он может попасть в конфликт, так что делаем дополнительную проверку			
			if (IsDtoObjectInConflict())
			{
				ProcessChildren(true);
			}
			else
			{
				// выполняем исключение объектов в БД
				FindExcludedObjectsInDbForDelete();
				ProcessChildren(false);
			}
		}

		private bool IsDtoObjectInConflict()
		{
			/*var dtoObject = GetDtoObject();
			if (dtoObject is ApplicationDto)
				return ConflictStorage.HasConflict(dtoObject);
			if (dtoObject is OrderOfAdmissionItemDto)
				return ConflictStorage.HasConflict(dtoObject);*/

			return GetDtoObject() != null && ConflictStorage.HasConflictOrNotImported(GetDtoObject());
		}

		protected abstract BaseDto GetDtoObject();
	}
}
