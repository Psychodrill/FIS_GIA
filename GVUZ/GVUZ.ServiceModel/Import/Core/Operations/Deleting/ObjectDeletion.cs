using System.Collections.Generic;
using System.Linq;
using FogSoft.Helpers;
using GVUZ.ServiceModel.Import.Core.Storages;
using GVUZ.ServiceModel.Import.WebService.Dto;

namespace GVUZ.ServiceModel.Import.Core.Operations.Deleting
{
	public abstract class ObjectDeletion : StorageConsumer
	{
		private List<ObjectDeletion> _conflictObjectDeletionList = new List<ObjectDeletion>();

		protected ObjectLinkManager ObjectLinkManager;		
		protected List<ObjectDeletion> DependedAndLinkedObjectsDeletionList = new List<ObjectDeletion>();		
		
		protected bool? CanDeleteResult;

		// объект импорта для которого пытаемся удалить объект в БД.
		// Указывается не всегда.
		protected BaseDto _notImportedDto;

		/// <summary>
		/// Для менеджеров удаления, которым передается не импортируемый dto объект, автоматически заносится
		/// связанный конфликт.
		/// </summary>
		protected ObjectDeletion(StorageManager storageManager, BaseDto notImportedDto)
			: base(storageManager)
		{
			ObjectLinkManager = new ObjectLinkManager(storageManager);
			_notImportedDto = notImportedDto;
		}

		protected ObjectDeletion(StorageManager storageManager)
			: base(storageManager)
		{
			ObjectLinkManager = new ObjectLinkManager(storageManager);
			//_baseDto = baseDto;
		}

		protected ObjectDeletion(StorageManager storageManager, bool tryDeleteParent)
			: base(storageManager)
		{
			ObjectLinkManager = new ObjectLinkManager(storageManager);			
		}

		/// <summary>
		/// Проверка на возможность удаления. 
		/// Проверка на возможность удаления связанных объектов. Связанные объекты можно получить из ObjectLinkManager.
		/// Проверка на возможность удаления потомков.
		/// </summary>		
		public bool CanDelete()
		{
			if (CanDeleteResult.HasValue) return CanDeleteResult.Value;

			FillDeletionList();

			if (DependedAndLinkedObjectsDeletionList.Count > 0)
			{
				var cantDeleteList = DependedAndLinkedObjectsDeletionList.Where(x => !x.CanDelete()).ToList();
				CanDeleteResult = cantDeleteList.Count == 0;
				if (!CanDeleteResult.Value)
				{
					if (GetDtoObject() != null)
						ConflictStorage.AddNotImportedDto(GetDtoObject(), cantDeleteList);
					else
						ConflictStorage.AddNotImportedDto(GetDbObjectID(), cantDeleteList);
					// вызываем проверку на доп. условия (на случай, если в методе происходит пополнение коллекции конфликтов), 
					// но в любом случае возвращаем false
					IsValidExtraConditions();
					return CanDeleteResult.Value;
				}
				// есл проверка по удалениям прошла без конфликтов, то проверяем выполнение доп. условий
				// в зависимости от этой проверки возвращаем результат
			}

			CanDeleteResult = IsValidExtraConditions();
			return CanDeleteResult.Value;
		}

		/// <summary>
		/// Создаем менеджеры для удаления на основании зависимых и связанных объектов.
		/// </summary>
		protected virtual void FillDeletionList() { }

		/// <summary>
		/// Проверка на дополнительные условия при удалении, не опираясь на менеджеры удаления зависимых объектов.
		/// Например, при удалении объема приема нужно удалить соответствующие направления в КГ.
		/// </summary>		
		public virtual bool IsValidExtraConditions()
		{
			return true;
		}

		/// <summary>
		/// Попытка удалить объект.
		/// Если объект удалить не получилось, то пополняется коллекция конфликтов.
		/// </summary>
		public virtual bool TryDelete()
		{
			return false;
		}

		/// <summary>
		/// Возвращается идентификатор объекта, который проверяют на возможность удаления.
		/// </summary>
		public abstract int GetDbObjectID();

		/// <summary>
		/// Объект импорта, для которого производится попытка удаления уже существующего объекта БД.
		/// </summary>
		public BaseDto GetDtoObject()
		{
			return _notImportedDto;
		}

		/// <summary>
		/// Возвращается идентификатор заявления или приказа, который проверяют на возможность удаления.
		/// Для других типов объектов используется метод GetDbObjectID.
		/// </summary>
		public virtual ApplicationShortRef GetDbApplicationShortRef()
		{
			return null; 
		}

		/// <summary>
		/// Возвращается объект, который проверяют на возможность удаления.
		/// </summary>
		/// <returns></returns>
		public abstract object GetDbObject();

		public IEnumerable<ObjectDeletion> ConflictDeletionList
		{
			get { return _conflictObjectDeletionList; }
		}
	}
}
