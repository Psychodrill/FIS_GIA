using System;
using System.Collections.Generic;
using System.Linq;
using FogSoft.Helpers;
using GVUZ.ServiceModel.Import.Core.Operations.Conflicts;
//using GVUZ.ServiceModel.Import.Core.Operations.Deleting;
//using GVUZ.ServiceModel.Import.Core.Operations.Deleting.Applications;
using GVUZ.ServiceModel.Import.Core.Packages.Handlers;
using GVUZ.ServiceModel.Import.Package;
using GVUZ.ServiceModel.Import.WebService.Dto;
using GVUZ.ServiceModel.Import.WebService.Dto.Result;
using GVUZ.ServiceModel.Import.WebService.Dto.Result.Import;

namespace GVUZ.ServiceModel.Import.Core.Storages
{
	public class ConflictStorage
	{
		public class ConflictMessage
		{
			public string Message;
			public int Code;
			public ConflictsResultDto ConflictItemsInfo;

			public ConflictMessage() {}

			public ConflictMessage(int code, ConflictsResultDto conlictItemsInfo,params string[] args)
			{
				Code = code;
                if (args.Length > 0)
                    Message = string.Format(ConflictMessages.GetMessage(code), args);
                else
                    Message = ConflictMessages.GetMessage(code);
				ConflictItemsInfo = conlictItemsInfo;
			}

			public static implicit operator ConflictMessage(int messageCode)
			{
				return new ConflictMessage() { Code = messageCode };
			}

			public ErrorInfoImportDto GetErrorInfo(BaseDto dto)
			{
				return new ErrorInfoImportDto {
				       		ErrorCode = Code.ToString(),
				       		Message =
                                    string.Format("{2} ({0}, UID: {1})", 
                                    dto.GetDescription(),
                                    (string.IsNullOrEmpty(dto.UID) ? "Не указан" : dto.UID),
                                    String.IsNullOrWhiteSpace(Message) ? 
                                    ConflictMessages.GetMessage(Code) : Message), 
                            ConflictItemsInfo = ConflictItemsInfo
				};
			}
		}

		// Каждый словарь содержит dto объект, который не удалось импортировать со ссылкой на объекты в БД, 
		// с которыми dto объект вступил в конфликт.
		// В данном классе объекты хранятся в качестве ключа. Dto объекты добавляются уже с ParentUID, чтобы выводить информацию после импорта.
		// На основании этих словарей будет создана секция Log / Failed и секция Conflicts.
		public readonly Dictionary<BaseDto, HashSet<string>> _linkedCompetitiveGroupItemsConflict = new Dictionary<BaseDto, HashSet<string>>();
		public readonly Dictionary<BaseDto, HashSet<string>> _linkedCompetitiveGroupsConflict = new Dictionary<BaseDto, HashSet<string>>();
		private readonly Dictionary<BaseDto, List<ApplicationShortRef>> _linkedApplicationsConflict = new Dictionary<BaseDto, List<ApplicationShortRef>>();
		private readonly Dictionary<BaseDto, List<ApplicationShortRef>> _linkedOrdersOfAdmissionConflict = new Dictionary<BaseDto, List<ApplicationShortRef>>();
		private readonly Dictionary<BaseDto, HashSet<string>> _linkedEntranceTestResultConflict = new Dictionary<BaseDto, HashSet<string>>();
		private readonly Dictionary<BaseDto, HashSet<string>> _linkedApplicationCommonBenefitConflict = new Dictionary<BaseDto, HashSet<string>>();

		// это объекты в БД, эти объекты не привязаны к Dto объектам		
		private readonly HashSet<int> _competitiveGroupItemsConflict = new HashSet<int>();
		private readonly HashSet<int> _competitiveGroupsConflict = new HashSet<int>();
		private List<ApplicationShortRef> _applicationsConflict = new List<ApplicationShortRef>();
        private List<ApplicationShortRef> _consideredApplicationsConflict = new List<ApplicationShortRef>();
        private List<ApplicationShortRef> _recommendedApplicationsConflict = new List<ApplicationShortRef>();
		private readonly List<ApplicationShortRef> _ordersOfAdmissionConflictList = new List<ApplicationShortRef>();
		private readonly HashSet<int> _entranceTestResultConflict = new HashSet<int>();
		private readonly HashSet<int> _applicationCommonBenefitConflict = new HashSet<int>();
        private readonly List<RecommendedListShort> _recommendedListConflict = new List<RecommendedListShort>();

		// словарь содержит связку неимпортированного объекта и сообщение для конфликтной ситуации
		// Коды сообщений хранятся в классе ConflictMessages
		private readonly Dictionary<BaseDto, ConflictMessage> _notImportedobjectWithErrorMessages = new Dictionary<BaseDto, ConflictMessage>();

		private readonly PackageData _packageData;
		private readonly DbObjectRepositoryBase _dbObjectRepo;		

		public HashSet<int> GetConflictCompetitiveGroupItemIDs()
		{
			return _competitiveGroupItemsConflict;
		}

		public HashSet<int> GetConflictCompetitiveGroupIDs()
		{
			return _competitiveGroupsConflict;
		}

		public List<ApplicationShortRef> GetConflictApplications()
		{
			return _applicationsConflict;
		}

		public List<ApplicationShortRef> GetConflictOrdersOfAdmission()
		{
			return _ordersOfAdmissionConflictList;
		}

		public HashSet<int> GetConflictApplicationCommonBenefits()
		{
			return _applicationCommonBenefitConflict;
		}

		public HashSet<int> GetConflictApplicationEntranceTestResult()
		{
			return _entranceTestResultConflict;
		}

		public ConflictStorage(DbObjectRepositoryBase dbObjectRepository)
			: this(dbObjectRepository, null)
		{
		}

		public ConflictStorage(DbObjectRepositoryBase dbObjectRepository, PackageData packageData)
		{
			_dbObjectRepo = dbObjectRepository;
			_packageData = packageData;			
		}

		public void AddCompetitiveGroupItem<T>(T notImported, int cgItemID)
		{
			AddCompetitiveGroupItems(notImported, new HashSet<int> { cgItemID });
		}

		public void AddCompetitiveGroups<T>(T notImported, HashSet<int> cgIDs)
		{
			if (notImported is BaseDto)
			{
				BaseDto notImportedDto = notImported as BaseDto;
                notImportedDto.IsBroken = true;

                if (_linkedCompetitiveGroupsConflict.ContainsKey(notImportedDto))
                    _linkedCompetitiveGroupsConflict[notImportedDto].UnionWith(cgIDs.Select(t => t.ToString()));
                else
                {
                    var h = new HashSet<string>();
                    h.UnionWith(cgIDs.Select(t => t.ToString()));
                    _linkedCompetitiveGroupsConflict.Add(notImportedDto, h);
                }
			}

			_competitiveGroupsConflict.UnionWith(cgIDs);
		}

		public void AddCompetitiveGroupItems<T>(T notImported, HashSet<int> cgItemIDs)
		{
			// добавляется объект БД, который нельзя удалить из-за сущестования связанных данных
			if (notImported is BaseDto)
			{
				BaseDto notImportedDto = notImported as BaseDto;
			    notImportedDto.IsBroken = true;

                if (_linkedCompetitiveGroupItemsConflict.ContainsKey(notImportedDto))
                    _linkedCompetitiveGroupItemsConflict[notImportedDto].UnionWith(cgItemIDs.Select(t => t.ToString()));
                else
                {
                    var h = new HashSet<string>();
                    h.UnionWith(cgItemIDs.Select(t => t.ToString()));
                    _linkedCompetitiveGroupItemsConflict.Add(notImportedDto, h);
                }
			}

			_competitiveGroupItemsConflict.UnionWith(cgItemIDs);
		}

		public void AddApplication<T>(T notImported, ApplicationShortRef appShortRef)
		{
			AddApplications(notImported, new HashSet<ApplicationShortRef> { appShortRef });
		}

		public void AddApplication<T>(T notImported, Application app)
		{
			AddApplications(notImported, new HashSet<ApplicationShortRef> {app.GetApplicationShortRef()});
		}

		public void AddApplications<T>(T notImported, HashSet<ApplicationShortRef> applicationIDs)
		{
			// добавляется объект БД, который нельзя удалить из-за сущестования связанных данных 
			if (notImported is BaseDto)
			{
				BaseDto notImportedDto = notImported as BaseDto;
                notImportedDto.IsBroken = true;

				if (!_linkedApplicationsConflict.ContainsKey(notImportedDto))
					_linkedApplicationsConflict.Add(notImported as BaseDto, applicationIDs.ToList());
				else
				{
					List<ApplicationShortRef> appRefs = _linkedApplicationsConflict[notImportedDto];
					_linkedApplicationsConflict[notImportedDto] = appRefs.Union(applicationIDs.ToList()).ToList();
				}				
			}
			// добавляется объект БД, который нельзя удалить из-за сущестования связанных данных 
			foreach (var appShortRef in applicationIDs)
			{
				if (!_applicationsConflict.Contains(appShortRef))
					_applicationsConflict.Add(appShortRef);
			}
		}

		public void AddApplicationCommonBenefits<T>(T notImported, HashSet<int> applicationCommonBenefitIDs, int messageCode = 0)
		{
			// добавляется объект БД, который нельзя удалить из-за сущестования связанных данных 
			if (notImported is BaseDto)
			{
				BaseDto notImportedDto = notImported as BaseDto;
                notImportedDto.IsBroken = true;

				if (_linkedApplicationCommonBenefitConflict.ContainsKey(notImportedDto))
					_linkedApplicationCommonBenefitConflict[notImportedDto].UnionWith(applicationCommonBenefitIDs.Select(t=> t.ToString()));
				else
                {
                    var h = new HashSet<string>(); 
                    h.UnionWith(applicationCommonBenefitIDs.Select(t=>t.ToString()));
					_linkedApplicationCommonBenefitConflict.Add(notImportedDto, h);
                }

				if (messageCode > 0)
					AddNotImportedDto(notImportedDto, messageCode);
			}
			_applicationCommonBenefitConflict.UnionWith(applicationCommonBenefitIDs);
		}

		public void AddApplicationCommonBenefit<T>(T notImported, int appCommonBenefitID)
		{
			AddApplicationCommonBenefits(notImported, new HashSet<int> { appCommonBenefitID });
		}

		public void AddApplicationCommonBenefits<T>(T notImported, ApplicationEntranceTestDocument[] appEntranceTestDoc)
		{
			AddApplicationCommonBenefits(notImported, new HashSet<int>(appEntranceTestDoc.Select(x => x.ID).ToList()));
		}

		public void AddEntranceTestResults<T>(T notImported, int? appEntranceTestDocID, 
			int messageCode = 0)
		{
			AddEntranceTestResults(notImported, 
				appEntranceTestDocID.HasValue ? 
				new HashSet<int> { appEntranceTestDocID.Value } : new HashSet<int>(), 
				messageCode);
		}

		public void AddEntranceTestResults<T>(T notImported, ApplicationEntranceTestDocument[] entranceTestResults)
		{			
			AddEntranceTestResults(notImported, new HashSet<int>(entranceTestResults.Select(x => x.ID).ToList()));
		}

		public void AddEntranceTestResults<T>(T notImported, HashSet<int> entranceTestResults, 
			int messageCode = 0)
		{
			if (entranceTestResults == null || entranceTestResults.Count == 0) return;

			// добавляется объект БД, который нельзя удалить из-за сущестования связанных данных 
			if (notImported is BaseDto)
			{
				BaseDto notImportedDto = notImported as BaseDto;
                notImportedDto.IsBroken = true;

				if (_linkedEntranceTestResultConflict.ContainsKey(notImportedDto))
					_linkedEntranceTestResultConflict[notImportedDto].UnionWith(entranceTestResults.Select(t=> t.ToString()));
				else
                {
                    var h = new HashSet<string>();
                    h.UnionWith(entranceTestResults.Select(t => t.ToString()));
					_linkedEntranceTestResultConflict.Add(notImportedDto, h);
                }

				if (messageCode > 0)
					AddNotImportedDto(notImportedDto, messageCode);
			}
			
			_entranceTestResultConflict.UnionWith(entranceTestResults);
		}

		public void AddOrdersOfAdmission<T>(T notImported, ApplicationShortRef orderOfAdmissionAppRef,
			int messageCode = 0)
		{
			// добавляется объект БД, который нельзя удалить из-за сущестования связанных данных 
			if (notImported is BaseDto)
			{
				var notImportedDto = notImported as BaseDto;
                notImportedDto.IsBroken = true;

				if (!_linkedOrdersOfAdmissionConflict.ContainsKey(notImportedDto))
					_linkedOrdersOfAdmissionConflict.Add(notImported as BaseDto, 
						new List<ApplicationShortRef> { orderOfAdmissionAppRef });
				else
				{
					var appRefs = _linkedOrdersOfAdmissionConflict[notImportedDto];
					if(!appRefs.Contains(orderOfAdmissionAppRef)) appRefs.Add(orderOfAdmissionAppRef);
				}

				if (messageCode > 0)
					AddNotImportedDto(notImportedDto, messageCode);
			}

			if (!_ordersOfAdmissionConflictList.Contains(orderOfAdmissionAppRef))
				_ordersOfAdmissionConflictList.Add(orderOfAdmissionAppRef);
		}

		#region Добавление в коллекцию конфликтов через ObjectDeletion (менеджеры удаления)

		//public void AddNotImportedDto<T>(T notImported, IEnumerable<ObjectDeletion> objectDeletions)
		//{
		//	foreach (var objectDeletion in objectDeletions)
		//	{
		//		if (objectDeletion.CanDelete()) continue;
				
		//		if(objectDeletion.GetType() == typeof(CompetitiveGroupItemDeletion))
		//			AddCompetitiveGroupItem(notImported, objectDeletion.GetDbObjectID());
		//		if (objectDeletion.GetType() == typeof(ApplicationDeletion))
		//			AddApplication(notImported, objectDeletion.GetDbApplicationShortRef());
		//		if (objectDeletion.GetType() == typeof(EntranceTestDocumentDeletion))
		//		{
		//			var etdDeletion = objectDeletion as EntranceTestDocumentDeletion;
		//			if(etdDeletion == null)
		//			{
		//				LogHelper.Log.ErrorFormat("Ошибка при добавлении в конфликт РВИ или общей льготы для заявления с ID: {0}",
		//					objectDeletion.GetDbObjectID());
		//				continue;
		//			}
		//			if(etdDeletion.EntranceTestIsApplicationCommonBenefit())
		//				AddApplicationCommonBenefit(notImported, etdDeletion.GetDbObjectID());
		//			else
		//				AddEntranceTestResults(notImported, etdDeletion.GetDbObjectID());
		//		}
		//	}

		//	// Object Deletion'а для приказов нет
		//}		

		#endregion

	    public void AddConflictWithCustomMessage(BaseDto notImported, ConflictMessage conflictMessage)
		{
		    notImported.IsBroken = true;

			if (!_notImportedobjectWithErrorMessages.ContainsKey(notImported))
				_notImportedobjectWithErrorMessages.Add(notImported, conflictMessage);

            if (notImported is AdmissionVolumeDto)
            {
                var conflictAdmVolumeDto = notImported as AdmissionVolumeDto;
                // не импортируем направления для данного объема приема
                foreach (var cgDto in _packageData.AdmissionInfo.CompetitiveGroups)
                {
                    if (cgDto.CampaignUID == conflictAdmVolumeDto.CampaignUID)
                        foreach (var cgItemDto in cgDto.Items.Where(x => x.DirectionID == conflictAdmVolumeDto.DirectionID && x.EducationLevelID == conflictAdmVolumeDto.EducationLevelID))
                        {
                            cgItemDto.ParentUID = cgDto.UID;
                            AddNotImportedDto(cgItemDto, ConflictMessages.AdmissionVolumeIsNotImportedForDirection);
                        }
                }
                // todo Roman: не импортируем распределенный объем приема на даный объем приема
                if (_packageData.AdmissionInfo.DistributedAdmissionVolume != null)
                    foreach (var dav in _packageData.AdmissionInfo.DistributedAdmissionVolume)
                        if (dav.AdmissionVolumeUID == conflictAdmVolumeDto.UID)
                        {
                            AddNotImportedDto(dav, ConflictMessages.AdmissionVolumeIsNotImportedForDistributedAdmissionVolume, conflictAdmVolumeDto.UID, conflictAdmVolumeDto.IsPlan.ToString());
                        }
            }

			// если объект - Направление в КГ, то добавляем в конфликты Направления Целевого приема для данного Направл. в КГ			
			if (notImported is CompetitiveGroupItemDto)
			{
				var conflictCGItemDto = notImported as CompetitiveGroupItemDto;
				foreach (var cgDto in _packageData.AdmissionInfo.CompetitiveGroups)
				{
					if (cgDto.TargetOrganizations == null) continue;
					foreach (var competitiveGroupTargetDto in cgDto.TargetOrganizations)
					{
						if(competitiveGroupTargetDto.Items == null) continue;
						foreach (var competitiveGroupTargetItemDto in competitiveGroupTargetDto.Items)
						{
							if (competitiveGroupTargetItemDto.DirectionID == conflictCGItemDto.DirectionID && 
                                competitiveGroupTargetItemDto.EducationLevelID == conflictCGItemDto.EducationLevelID)
								AddConflictWithCustomMessage(competitiveGroupTargetItemDto, conflictMessage);
						}
					}
				}
			}

            if (notImported is RecommendedListDto)
            {
                var conflictItem = notImported as RecommendedListDto;

                RecommendedListShort shortRecListInfo = new RecommendedListShort() { Stage = conflictItem.Stage };
                _recommendedListConflict.Add(shortRecListInfo);
            }
		}

		public void AddNotImportedDto(BaseDto notImported, int messageCode, params string[] messageParams)
		{
		    notImported.IsBroken = true;
            notImported.ErrorMessages.Add(String.Format(ConflictMessages.GetMessage(messageCode), messageParams));

            AddConflictWithCustomMessage(notImported,
				new ConflictMessage
				{
					Code = messageCode,
					Message = String.Format(ConflictMessages.GetMessage(messageCode), messageParams)
				});
		}

		public void AddNotImportedDto(IEnumerable<BaseDto> notImportedObjects, int messageCode)
		{
			foreach (var notImportedObject in notImportedObjects)
				AddConflictWithCustomMessage(notImportedObject, messageCode);
		}

		public bool HasConflictOrNotImported(BaseDto checkedObject)
		{
			if (_linkedCompetitiveGroupItemsConflict.ContainsKey(checkedObject))
				return true;

			if (_linkedApplicationCommonBenefitConflict.ContainsKey(checkedObject))
				return true;
			if (_linkedEntranceTestResultConflict.ContainsKey(checkedObject))
				return true;

			if(checkedObject is ApplicationDto)
			{
				var appDto = checkedObject as ApplicationDto;
				if (_linkedApplicationsConflict.ContainsKey(appDto)) return true;			
				if(_applicationsConflict.Contains(appDto.GetApplicationShortRef()))
					return true;
			}

			if(checkedObject is OrderOfAdmissionItemDto)
			{
				var orderItemDto = checkedObject as OrderOfAdmissionItemDto;
				if (_linkedOrdersOfAdmissionConflict.ContainsKey(orderItemDto))
					return true;
				if (_ordersOfAdmissionConflictList.Contains(orderItemDto.Application))
					return true;
			}

			if (_notImportedobjectWithErrorMessages.ContainsKey(checkedObject))
				return true;

			return false;
		}

		public bool IsConflictExists()
		{
            return _linkedCompetitiveGroupItemsConflict.Count > 0 ||
                _linkedApplicationCommonBenefitConflict.Count > 0 ||
                _linkedApplicationsConflict.Count > 0 ||
              _linkedEntranceTestResultConflict.Count > 0 ||
                _linkedOrdersOfAdmissionConflict.Count > 0 ||
                _notImportedobjectWithErrorMessages.Count > 0;
		}

/*
		public bool CheckApplicationInConflicts(ApplicationDto appDto)
		{
			if (appDto == null) return false;

			if (_linkedApplicationsConflict.ContainsKey(appDto)) return true;			
			return _applicationsConflict.Contains(appDto.GetApplicationShortRef());
		}
*/

		public ConflictsResultDto GetConflictsResultDto()
		{
			HashSet<string> cgIDs = new HashSet<string>();
			foreach (HashSet<string> cgConflictIDs in _linkedCompetitiveGroupsConflict.Values)
				cgIDs.UnionWith(cgConflictIDs.Select(t=> t.ToString()));
            cgIDs.UnionWith(_competitiveGroupsConflict.Select(t => t.ToString()));

            HashSet<string> cgItemIDs = new HashSet<string>();
            foreach (HashSet<string> cgItemConflictIDs in _linkedCompetitiveGroupItemsConflict.Values)
				cgItemIDs.UnionWith(cgItemConflictIDs);
            cgItemIDs.UnionWith(_competitiveGroupItemsConflict.Select(t => t.ToString()));

            HashSet<string> appCommonBenefits = new HashSet<string>();
            foreach (HashSet<string> appCommonBenefitIDs in _linkedApplicationCommonBenefitConflict.Values)
				appCommonBenefits.UnionWith(appCommonBenefitIDs);
            appCommonBenefits.UnionWith(_applicationCommonBenefitConflict.Select(t => t.ToString()));

            HashSet<string> entranceTests = new HashSet<string>();
            foreach (HashSet<string> entrTestIDs in _linkedEntranceTestResultConflict.Values)
				entranceTests.UnionWith(entrTestIDs);
            entranceTests.UnionWith(_entranceTestResultConflict.Select(t => t.ToString()));

			var conflictsResultDto = new ConflictsResultDto();
			conflictsResultDto.Applications = (_applicationsConflict.Count == 0) ? null : _applicationsConflict.Distinct().ToArray();
			conflictsResultDto.CompetitiveGroupItems = (cgItemIDs.Count == 0) ? null : cgItemIDs.Select(x => x.ToString()).Distinct().ToArray();
			conflictsResultDto.CompetitiveGroups = (cgIDs.Count == 0) ? null : cgIDs.Select(x => x).Distinct().ToArray();
			conflictsResultDto.ApplicationCommonBenefits = (appCommonBenefits.Count == 0) ? null : appCommonBenefits.Select(x => x.ToString()).Distinct().ToArray();
			conflictsResultDto.EntranceTestResults = (entranceTests.Count == 0) ? null : entranceTests.Select(x => x.ToString()).Distinct().ToArray();
			conflictsResultDto.OrdersOfAdmission = (_ordersOfAdmissionConflictList.Count == 0) ? null : _ordersOfAdmissionConflictList.Distinct().ToArray();
            conflictsResultDto.ConsideredApplications = (_consideredApplicationsConflict.Count == 0) ? null : _consideredApplicationsConflict.Distinct().ToArray();
            conflictsResultDto.RecommendedApplications = (_recommendedApplicationsConflict.Count == 0) ? null : _recommendedApplicationsConflict.Distinct().ToArray();
            conflictsResultDto.RecommendedLists = (_recommendedListConflict.Count == 0) ? null : _recommendedListConflict.Distinct().ToArray();
			return conflictsResultDto;
		}

		public FailedImportInfoDto GetFailedImportInfoDto(DtoObjectStorage processedDtoStorage)
		{
			var notImportedHelper = new NotImportedHelper(_dbObjectRepo, processedDtoStorage, _notImportedobjectWithErrorMessages);			
			foreach (KeyValuePair<BaseDto, ConflictMessage> notImportedObject in _notImportedobjectWithErrorMessages)
				notImportedHelper.AddConflictInfo(notImportedObject.Key, notImportedObject.Value);

			foreach (var notImportedDto in _linkedCompetitiveGroupsConflict.Keys)
			    notImportedHelper.AddConflictInfo(notImportedDto, new ConflictMessage(
			        ConflictMessages.DependedObjectsExists, new ConflictsResultDto()
			        {
			            CompetitiveGroups = _linkedCompetitiveGroupsConflict[notImportedDto].Select(x => x.ToString()).ToArray()
			        }));
			foreach (var notImportedDto in _linkedCompetitiveGroupItemsConflict.Keys)
				notImportedHelper.AddConflictInfo(notImportedDto, new ConflictMessage(
					ConflictMessages.DependedObjectsExists, new ConflictsResultDto()
					{
						CompetitiveGroupItems = _linkedCompetitiveGroupItemsConflict[notImportedDto]
								                            .Select(x => x.ToString()).ToArray().NullOnEmpty()
					}));
			foreach (var notImportedDto in _linkedEntranceTestResultConflict.Keys)
				notImportedHelper.AddConflictInfo(notImportedDto, new ConflictMessage(
					ConflictMessages.DependedObjectsExists, new ConflictsResultDto()
					{
						EntranceTestResults = _linkedEntranceTestResultConflict[notImportedDto]
							.Select(x => x.ToString()).ToArray().NullOnEmpty()
					}));
			
			foreach (var notImportedDto in _linkedApplicationsConflict.Keys)
				notImportedHelper.AddConflictInfo(notImportedDto, new ConflictMessage(
					ConflictMessages.DependedObjectsExists, new ConflictsResultDto()
					{
						Applications = _linkedApplicationsConflict[notImportedDto].ToArray()
					}));

			foreach (var notImportedDto in _linkedApplicationCommonBenefitConflict.Keys)
				notImportedHelper.AddConflictInfo(notImportedDto, new ConflictMessage(
					ConflictMessages.DependedObjectsExists, new ConflictsResultDto()
					{
						ApplicationCommonBenefits = _linkedApplicationCommonBenefitConflict[notImportedDto]
							.Select(x => x.ToString()).ToArray().NullOnEmpty()
					}));
			foreach (var notImportedDto in _linkedOrdersOfAdmissionConflict.Keys)
				notImportedHelper.AddConflictInfo(notImportedDto, new ConflictMessage(
					ConflictMessages.DependedObjectsExists, new ConflictsResultDto()
					{
						OrdersOfAdmission = _linkedOrdersOfAdmissionConflict[notImportedDto].ToArray()
					}));

			FailedImportInfoDto failedImportInfoDto = notImportedHelper.GetFailedImportInfoDto();
			return failedImportInfoDto;
		}

		public bool IsApplicationDataInConflicts(Application application)
		{
			application.ApplicationEntrantDocument.Load();
			application.ApplicationEntranceTestDocument.Load();

			bool hasConflicts = false;
			if (GetConflictApplications().Contains(application.GetApplicationShortRef())) return true;

			foreach (var appEntrTestResult in application.ApplicationEntranceTestDocument)
			{
				bool hasConflictWithAppCommonBenefit = false;
				bool hasConflictWithAppEntranceTestResult = false;
				if (GetConflictApplicationCommonBenefits().Contains(appEntrTestResult.ID))
					hasConflictWithAppCommonBenefit = true;
				if (GetConflictApplicationEntranceTestResult().Contains(appEntrTestResult.ID))
					hasConflictWithAppEntranceTestResult = true;

				hasConflicts = hasConflictWithAppCommonBenefit || hasConflictWithAppEntranceTestResult;
				if (hasConflicts)
				{
					ApplicationDto appDto = _packageData.Applications.SingleOrDefault(x => 
                        x.ApplicationNumber == application.ApplicationNumber && x.RegistrationDateDate == application.RegistrationDate);

					// если заявление не импортировалось из-за конфликта по льготе, то добавляем в конфликты льготу
					if(hasConflictWithAppCommonBenefit)
						AddApplicationCommonBenefit(appDto, appEntrTestResult.ID);
					// иначе заявление не импортировалось из-за конфликта по РВИ. Добавляем в конфликты РВИ
					else
						AddEntranceTestResults(appDto, appEntrTestResult.ID);
				}
			}

			return hasConflicts;
		}

	}
}