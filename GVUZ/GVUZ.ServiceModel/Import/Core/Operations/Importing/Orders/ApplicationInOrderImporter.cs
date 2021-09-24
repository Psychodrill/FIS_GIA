using System;
using GVUZ.Model.Import.Core.Storages;
using GVUZ.Model.Import.WebService.Dto;

namespace GVUZ.Model.Import.Core.Operations.Importing.Orders
{
	/// <summary>
	/// Импорт заявления в приказе. Пустой класс, поскольку сейчас сразу импортируем всем скопом
	/// </summary>
	public class ApplicationInOrderImporter : ObjectImporter
	{
		private readonly OrderOfAdmissionItemDto _orderOfAdmissionItemDto;

		public ApplicationInOrderImporter(StorageManager storageManager, OrderOfAdmissionItemDto orderOfAdmissionItemDto) :
			base(storageManager)
		{
			_orderOfAdmissionItemDto = orderOfAdmissionItemDto;
		}

		protected override void FindExcludedObjectsInDbForDelete()
		{
			throw new NotImplementedException();
		}

		protected override void FindInsertAndUpdate()
		{
			throw new NotImplementedException();
		}

		protected override bool CanUpdate()
		{
			throw new NotImplementedException();
		}

		protected override void ProcessChildren(bool isParentConflict)
		{
			
		}

		protected override BaseDto GetDtoObject()
		{
			return _orderOfAdmissionItemDto;
		}
	}
}