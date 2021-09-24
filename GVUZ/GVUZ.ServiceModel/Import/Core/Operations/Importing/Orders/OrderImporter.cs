using System;
using GVUZ.Model.Import.Core.Operations.Conflicts;
using GVUZ.Model.Import.Core.Storages;
using GVUZ.Model.Import.WebService.Dto;

namespace GVUZ.Model.Import.Core.Operations.Importing.Orders
{
	public class OrderImporter : ObjectImporter
	{
		private readonly OrderOfAdmissionItemDto[] _orderOfAdmissionTypeDto;

		public OrderImporter(StorageManager storageManager, OrderOfAdmissionItemDto[] orderOfAdmissionTypeDto) :
			base(storageManager)
		{
			_orderOfAdmissionTypeDto = orderOfAdmissionTypeDto;
		}

		protected override void FindExcludedObjectsInDbForDelete()
		{			
		}

		protected override void FindInsertAndUpdate()
		{
		}

		protected override bool CanUpdate()
		{
			return true;
		}

		protected override void ProcessChildren(bool isParentConflict)
		{
			foreach (OrderOfAdmissionItemDto orderOfAdmissionItemDto in _orderOfAdmissionTypeDto)
			{
				if (!ConflictStorage.HasConflictOrNotImported(orderOfAdmissionItemDto))
					new ApplicationInOrderImporter(StorageManager, orderOfAdmissionItemDto).AnalyzeImportPackage();
			}
		}

		protected override BaseDto GetDtoObject()
		{
			return null;
		}

		protected override void CheckIntegrity()
		{		
			// Приказы: существует ли заявление указанное в приказе
			foreach (OrderOfAdmissionItemDto orderOfAdmissionItemDto in _orderOfAdmissionTypeDto)
			{
				if (!DbObjectRepository.ApplicationsDictByShortRef.ContainsKey(orderOfAdmissionItemDto.Application))
				{
					// положить информацию в секцию Fails
					ConflictStorage.AddNotImportedDto(orderOfAdmissionItemDto, ConflictMessages.OrderOfAdmissionContainsRefOnNotImportedApplication);			
				}
			}
		}
	}
}