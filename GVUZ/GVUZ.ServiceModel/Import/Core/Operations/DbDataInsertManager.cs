using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AutoMapper;
using FogSoft.Helpers;
using GVUZ.Model.Helpers;
using GVUZ.Model.Institutions;
using GVUZ.ServiceModel.Import.Bulk.Extensions;
using GVUZ.ServiceModel.Import.Bulk.Infrastructure;
using GVUZ.ServiceModel.Import.Bulk.Infrastructure.Uploaders;
using GVUZ.ServiceModel.Import.Bulk.Model.Results;
using GVUZ.ServiceModel.Import.Core.Operations.Conflicts;
using GVUZ.ServiceModel.Import.Core.Packages.Handlers;
using GVUZ.ServiceModel.Import.Core.Storages;
using GVUZ.ServiceModel.Import.WebService.Dto;
using GVUZ.ServiceModel.Import.WebService.Dto.Result.Import;
using GVUZ.ServiceModel.Import.Bulk.Collectors;

namespace GVUZ.ServiceModel.Import.Core.Operations
{
	/// <summary>
	/// Вставка данных в базу
	/// </summary>
	public class DbDataInsertManager : StorageConsumer
	{
		private readonly int _institutionID;
		private readonly DtoObjectStorage _insertStorage;
		private readonly ImportEntities _importEntities;

		private int _admissionVolumesImported;
        private int _distributedAdmissionVolumesImported;
		private int _competitiveGroupItemsImported;
		private int _campaignsImported;
	    private int _competitiveGroupsImported;
		private int _applicationsImported;
		//нам некуда возвращать эти данные, но на всякий случай считаем
		private int _ordersImported;
        private int _applicationsInOrdersImported;
		private readonly SuccessfulImportStatisticsDto _successfulImportStatisticsDto;

		/// <summary>
		/// Идентификаторы импортированных заявлений используются для проверки ЕГЭ.
		/// </summary>
		private readonly List<Tuple<int, int, int>> _importedApplicationIDList = new List<Tuple<int, int, int>>();

		public DbDataInsertManager(StorageManager storageManager, SuccessfulImportStatisticsDto successfulImportStatisticsDto)
			: base(storageManager)
		{
			if (successfulImportStatisticsDto == null) throw new ArgumentNullException("successfulImportStatisticsDto");

			_insertStorage = InsertStorage;
            _institutionID = storageManager.DbObjectRepository.InstitutionId;
			_successfulImportStatisticsDto = successfulImportStatisticsDto;

			_admissionVolumesImported = _successfulImportStatisticsDto.AdmissionVolumes.To(0);
            _competitiveGroupsImported = _successfulImportStatisticsDto.CompetitiveGroups.To(0);
			_competitiveGroupItemsImported = _successfulImportStatisticsDto.CompetitiveGroupItems.To(0);
			_applicationsImported = _successfulImportStatisticsDto.Applications.To(0);
			_ordersImported = _successfulImportStatisticsDto.Orders.To(0);
            _applicationsInOrdersImported = _successfulImportStatisticsDto.ApplicationsInOrders.To(0);
			_campaignsImported = _successfulImportStatisticsDto.Campaigns.To(0);

			_importEntities = DbObjectRepository.ImportEntities;	
		}

		/// <summary>
		/// Вставляем кампании
		/// </summary>
		public void InsertCampaigns()
		{
			foreach (var campaignDto in _insertStorage.Campaign)
			{
				Campaign campaign = _importEntities.Campaign.CreateObject();
				campaign.UID = campaignDto.UID;
				campaign.InstitutionID = InstitutionID;
				campaign.Name = campaignDto.Name;
				campaign.YearStart = campaignDto.YearStart.To(DateTime.Now.Year);
				campaign.YearEnd = campaignDto.YearEnd.To(DateTime.Now.Year);
				campaign.StatusID = campaignDto.StatusID;
			    campaign.Additional = campaignDto.AdditionalSet ?? false;

				if (campaignDto.EducationForms.Contains(EDFormsConst.O.ToString()))
					campaign.EducationFormFlag |= 1;
				if (campaignDto.EducationForms.Contains(EDFormsConst.OZ.ToString()))
					campaign.EducationFormFlag |= 2;
				if (campaignDto.EducationForms.Contains(EDFormsConst.Z.ToString()))
					campaign.EducationFormFlag |= 4;
				foreach (var celDto in campaignDto.EducationLevels)
				{
					CampaignEducationLevel cel = _importEntities.CampaignEducationLevel.CreateObject();
					cel.Campaign = campaign;
					cel.Course = celDto.Course.To(0);
					cel.EducationLevelID = (short)celDto.EducationLevelID.To(0);
				}

				if (campaignDto.CampaignDates != null)
				{
					foreach (var cdDto in campaignDto.CampaignDates)
					{
						CampaignDate cd = _importEntities.CampaignDate.CreateObject();
						cd.Campaign = campaign;
						cd.Course = cdDto.Course.To(0);
						cd.EducationFormID = (short)cdDto.EducationFormID.To(0);
						cd.EducationLevelID = (short)cdDto.EducationLevelID.To(0);
						cd.EducationSourceID = (short)cdDto.EducationSourceID.To(0);
						cd.Stage = !String.IsNullOrEmpty(cdDto.Stage) ? cdDto.Stage.To(0) : 0;
                        
						cd.DateStart = cdDto.DateStart.GetStringOrEmptyAsDate();
						cd.DateEnd = cdDto.DateEnd.GetStringOrEmptyAsDate();
						cd.DateOrder = cdDto.DateOrder.GetStringOrEmptyAsDate();
                        cd.UID = cdDto.UID;
						cd.IsActive = true;
						_importEntities.CampaignDate.AddObject(cd);
					}
				}

                // #28038
			    campaign.IsFromKrym = campaignDto.IsFromKrym.GetValueOrDefault();

				_importEntities.Campaign.AddObject(campaign);
				_campaignsImported++;
			}
			_importEntities.SaveChanges();

			_successfulImportStatisticsDto.campaignsImported = _campaignsImported;
		}

		/// <summary>
		/// Вставляем объём приёма и КГ
		/// </summary>
		public void InsertInstitutionStructure()
		{
			// объем приема
            //List<AdmissionVolume> admissionVolumes = new List<AdmissionVolume>();
		    foreach (AdmissionVolumeDto admissionVolumeDto in _insertStorage.AdmissionVolume)
			{
				AdmissionVolume admissionVolume = Mapper.Map(admissionVolumeDto, _importEntities.AdmissionVolume.CreateObject());
				admissionVolume.Course = admissionVolumeDto.Course.To(0);
				admissionVolume.CampaignID = _importEntities.Campaign.Where(x => 
                    x.InstitutionID == InstitutionID && x.UID == admissionVolumeDto.CampaignUID).Select(x => x.CampaignID).FirstOrDefault();
				
                if (admissionVolume.CampaignID == 0)
					throw new ArgumentNullException("CampaignID", "Admission Volume with incorrect campaign UID: " + admissionVolumeDto.UID + "/" + admissionVolumeDto.CampaignUID);

				admissionVolume.InstitutionID = _institutionID;
				_importEntities.AdmissionVolume.AddObject(admissionVolume);
                _insertStorage.packageAdmissionVolumes.Add(admissionVolume);
                _admissionVolumesImported++;
			}
            _importEntities.SaveChanges();

            

			// КГ
			foreach (CompetitiveGroupDto competitiveGroupDto in _insertStorage.CompetitiveGroup)
			{
                var campaignId = _importEntities.Campaign.Where(x => x.InstitutionID == InstitutionID && x.UID == competitiveGroupDto.CampaignUID)
                    .Select(x => x.CampaignID)
                    .FirstOrDefault();
                if (campaignId == 0)
                    throw new ArgumentNullException("CampaignID", "Competitive Group with incorrect campaign UID: " + competitiveGroupDto.UID + "/" + competitiveGroupDto.CampaignUID);

				var competitiveGroup = Mapper.Map(competitiveGroupDto, _importEntities.CompetitiveGroup.CreateObject());
                competitiveGroup.CampaignID = campaignId;
				competitiveGroup.Course = (short)competitiveGroupDto.Course.To(0);
				competitiveGroup.InstitutionID = _institutionID;
				//505 приказ, или любое
				competitiveGroup.DirectionFilterType = competitiveGroupDto.UseAnyDirectionsFilter ? 2 : 0;
				_importEntities.CompetitiveGroup.AddObject(competitiveGroup);
			    _competitiveGroupsImported++;
			}
			_importEntities.SaveChanges();

			// целевой прием
			foreach (CompetitiveGroupTargetDto competitiveGroupTargetDto in _insertStorage.CompetitiveGroupTarget)
			{
				var competitiveGroupTarget = _importEntities.CompetitiveGroupTarget.FirstOrDefault(x => 
                    x.UID == competitiveGroupTargetDto.UID && x.InstitutionID == InstitutionID);

				if (competitiveGroupTarget == null)
				{
					competitiveGroupTarget = Mapper.Map(competitiveGroupTargetDto, _importEntities.CompetitiveGroupTarget.CreateObject());
					_importEntities.CompetitiveGroupTarget.AddObject(competitiveGroupTarget);
				}

				competitiveGroupTarget.InstitutionID = _institutionID;
				//competitiveGroup.CompetitiveGroupTarget.Add(competitiveGroupTarget);
			}
			_importEntities.SaveChanges();

			// направления в КГ
			foreach (CompetitiveGroupItemDto competitiveGroupItemDto in _insertStorage.CompetitiveGroupItem)
			{
				CompetitiveGroupItem competitiveGroupItem = Mapper.Map(competitiveGroupItemDto,
					_importEntities.CompetitiveGroupItem.CreateObject());
				var competitiveGroup = _importEntities.CompetitiveGroup
					.SingleOrDefault(x => x.UID == competitiveGroupItemDto.ParentUID && x.InstitutionID == _institutionID);
			    if (competitiveGroup == null) continue;

			    _importEntities.CompetitiveGroupItem.AddObject(competitiveGroupItem);
                competitiveGroupItem.CompetitiveGroupID = competitiveGroup.CompetitiveGroupID;
				_competitiveGroupItemsImported++;
			}
			_importEntities.SaveChanges();

			// направления в КГ целевого приема
			foreach (CompetitiveGroupTargetItemDto competitiveGroupTargetItemDto in _insertStorage.CompetitiveGroupTargetItem)
			{
                var competitiveGroupTarget = _importEntities.CompetitiveGroupTarget.SingleOrDefault(
                    x => x.UID == competitiveGroupTargetItemDto.ParentUID && x.InstitutionID == _institutionID
                    );
			    if (competitiveGroupTarget == null) continue;

				int directionID = competitiveGroupTargetItemDto.DirectionID.To(0);
				int edLevelID = competitiveGroupTargetItemDto.EducationLevelID.To(0);

			    var competitiveGroupItem = _importEntities.CompetitiveGroupItem
                    .SingleOrDefault(x => x.CompetitiveGroup.InstitutionID == InstitutionID && 
                        x.CompetitiveGroup.UID == competitiveGroupTargetItemDto.CompetitiveGroupUID && 
                        x.DirectionID == directionID && 
                        x.EducationLevelID == edLevelID);
                if (competitiveGroupItem == null) continue;
                
                /* Если есть такая же группа целевого набора но с другим UID - ругаемся */
			    var existentTargetItem = _importEntities.CompetitiveGroupTargetItem.FirstOrDefault(c =>
                    c.CompetitiveGroupItemID == competitiveGroupItem.CompetitiveGroupItemID 
                    && c.CompetitiveGroupTargetID == competitiveGroupTarget.CompetitiveGroupTargetID);
			    if (existentTargetItem != null)
			    {
                    ConflictStorage.AddNotImportedDto(competitiveGroupTargetItemDto, ConflictMessages.CompetitiveGroupTargeExistsInDb,
                        existentTargetItem.UID, competitiveGroupTargetItemDto.UID, competitiveGroupTargetItemDto.CompetitiveGroupUID);
                    LogHelper.Log.ErrorFormat("В данном конкурсе ({4}) уже существует целевой набор ({2}, {3}), UID которого ({0}) отличается от UID'а, передаваемого в пакете ({1})",
                        existentTargetItem.UID, competitiveGroupTargetItemDto.UID,
                        competitiveGroupItem.CompetitiveGroupItemID, competitiveGroupTarget.CompetitiveGroupTargetID, competitiveGroupTargetItemDto.CompetitiveGroupUID);
			    }
			    else
			    {
                    var competitiveGroupTargetItem = Mapper.Map(competitiveGroupTargetItemDto, _importEntities.CompetitiveGroupTargetItem.CreateObject());
                    competitiveGroupTargetItem.CompetitiveGroupItemID = competitiveGroupItem.CompetitiveGroupItemID;
                    competitiveGroupTargetItem.CompetitiveGroupTargetID = competitiveGroupTarget.CompetitiveGroupTargetID;
                    _importEntities.CompetitiveGroupTargetItem.AddObject(competitiveGroupTargetItem);
			    }
			}
            _importEntities.SaveChanges();

			// общие льготы для КГ
			foreach (BenefitItemDto benefitItemDto in _insertStorage.CompetitiveGroupBenefitItem)
			{
				var benefitItem = Mapper.Map(benefitItemDto, _importEntities.BenefitItemC.CreateObject());
				var competitiveGroup = _importEntities.CompetitiveGroup.SingleOrDefault(x => 
                    x.UID == benefitItemDto.ParentUID && x.InstitutionID == _institutionID);
                if (competitiveGroup == null) continue;

                benefitItem.CompetitiveGroupID = competitiveGroup.CompetitiveGroupID;
				_importEntities.BenefitItemC.AddObject(benefitItem);

			    if (benefitItemDto.IsForAllOlympics.To(false))
			    {
                    benefitItem.OlympicYear = DateTime.Now.Year;
			        benefitItem.OlympicLevelFlags = 7;
			    }

                /* заполняем флаги олимпиад */
                AddBenefitItemCOlympicTypeAndSetOlympicLevelFlags(_importEntities, benefitItem, benefitItemDto);
                _importEntities.SaveChanges();
                AddMinEgeScoresForCommonBenefit(_importEntities, benefitItem, benefitItemDto);
                _importEntities.SaveChanges();
            }
            _importEntities.SaveChanges();

			// ВИ для КГ
			foreach (EntranceTestItemDto entranceTestItemDto in _insertStorage.CompetitiveGroupEntranceTestItem)
			{
				EntranceTestItemC entranceTestItemC = Mapper.Map(entranceTestItemDto, _importEntities.EntranceTestItemC.CreateObject());
                if (entranceTestItemC.SubjectID != null && DbObjectRepository.GetSubject(entranceTestItemC.SubjectID.Value) == null)
                {
                    ConflictStorage.AddConflictWithCustomMessage(
                        entranceTestItemDto, new ConflictStorage.ConflictMessage
                        {
                            Code = ConflictMessages.SubjectIsNotFounded,
                            Message = String.Format(ConflictMessages.GetMessage(ConflictMessages.SubjectIsNotFounded),
                            entranceTestItemC.SubjectID.Value)
                        });
                    return;
                }

				//проставляем нужный балл, если не указан #37138
				if (entranceTestItemDto.EntranceTestTypeID == "1" 
					&& entranceTestItemDto.EntranceTestSubject.SubjectID.To(0) > 0 
					&& string.IsNullOrEmpty(entranceTestItemDto.MinScore))
				{
					entranceTestItemC.MinScore = DbObjectRepository.GetSubjectEgeMinValue(entranceTestItemDto.EntranceTestSubject.SubjectID.To(0));
				}

                var competitiveGroup = _importEntities.CompetitiveGroup.SingleOrDefault(x => 
                    x.UID == entranceTestItemDto.ParentUID && x.InstitutionID == _institutionID);
                if (competitiveGroup == null) continue;

                entranceTestItemC.CompetitiveGroupID = competitiveGroup.CompetitiveGroupID;
				_importEntities.EntranceTestItemC.AddObject(entranceTestItemC);
			}
			_importEntities.SaveChanges();

			// льготы для ВИ в КГ
			foreach (BenefitItemDto benefitItemDto in _insertStorage.CompetitiveGroupEntranceTestBenefitItem)
			{
				BenefitItemC benefitItem = Mapper.Map(benefitItemDto, _importEntities.BenefitItemC.CreateObject());

				var entranceTestItemDb = _importEntities.EntranceTestItemC
					.SingleOrDefault(x => x.UID == benefitItemDto.ParentUID && x.CompetitiveGroup.InstitutionID == _institutionID);
			    if (entranceTestItemDb == null) continue;

			    benefitItem.EntranceTestItemID = entranceTestItemDb.EntranceTestItemID;
                benefitItem.CompetitiveGroupID = entranceTestItemDb.CompetitiveGroupID;

			    if (benefitItemDto.IsForAllOlympics.To(false))
			    {
			        benefitItem.OlympicYear = DateTime.Now.Year;
			        benefitItem.OlympicLevelFlags = 7;
			    }

                /* заполняем флаги олимпиад */
                AddBenefitItemCOlympicTypeAndSetOlympicLevelFlags(_importEntities, benefitItem, benefitItemDto);
                _importEntities.BenefitItemC.AddObject(benefitItem);
			}
			_importEntities.SaveChanges();

			//ставим количество
			_successfulImportStatisticsDto.AdmissionVolumes = _admissionVolumesImported.ToString();

            _successfulImportStatisticsDto.CompetitiveGroups = _competitiveGroupsImported.ToString();
			_successfulImportStatisticsDto.CompetitiveGroupItems = _competitiveGroupItemsImported.ToString();
		}

        public void InsertDistributedAdmissionVolumes()
        {
            // распределенный объем приема
            GVUZ.Model.Entrants.EntrantsEntities _entrantEntities = new Model.Entrants.EntrantsEntities();
            var avIDList = _insertStorage.packageAdmissionVolumes.Select(i => i.AdmissionVolumeID);
            //_entrantEntities.DistributedAdmissionVolume.DeleteIn(t => t.AdmissionVolumeID, avIDList, 500);
            foreach (var itemToDelete in _entrantEntities.DistributedAdmissionVolume.Where(t => avIDList.Contains(t.AdmissionVolumeID)))
                _entrantEntities.DeleteObject(itemToDelete);
            _entrantEntities.SaveChanges();

            foreach (DistributedAdmissionVolumeDto distributedAdmissionVolumeDto in _insertStorage.DistributedAdmissionVolumeDto)
            {
                var avItem = _insertStorage.packageAdmissionVolumes.Where(t => t.UID == distributedAdmissionVolumeDto.AdmissionVolumeUID).FirstOrDefault();

                if (!distributedAdmissionVolumeDto.IsBroken && avItem != null)
                {

                    //GVUZ.Model.Entrants.DistributedAdmissionVolume distributedAdmissionVolume = Mapper.Map(distributedAdmissionVolumeDto, _entrantEntities.DistributedAdmissionVolume.CreateObject());
                    GVUZ.Model.Entrants.DistributedAdmissionVolume distributedAdmissionVolume = _entrantEntities.DistributedAdmissionVolume.CreateObject();
                    distributedAdmissionVolume.AdmissionVolumeID = avItem.AdmissionVolumeID;
                    distributedAdmissionVolume.IdLevelBudget = distributedAdmissionVolumeDto.LevelBudget.To(0);

                    distributedAdmissionVolume.NumberBudgetO = distributedAdmissionVolumeDto.NumberBudgetO.To(0);
                    distributedAdmissionVolume.NumberBudgetOZ = distributedAdmissionVolumeDto.NumberBudgetOZ.To(0);
                    distributedAdmissionVolume.NumberBudgetZ = distributedAdmissionVolumeDto.NumberBudgetZ.To(0);

                    distributedAdmissionVolume.NumberQuotaO = distributedAdmissionVolumeDto.NumberQuotaO.To(0);
                    distributedAdmissionVolume.NumberQuotaOZ = distributedAdmissionVolumeDto.NumberQuotaOZ.To(0);
                    distributedAdmissionVolume.NumberQuotaZ = distributedAdmissionVolumeDto.NumberQuotaZ.To(0);

                    distributedAdmissionVolume.NumberTargetO = distributedAdmissionVolumeDto.NumberTargetO.To(0);
                    distributedAdmissionVolume.NumberTargetOZ = distributedAdmissionVolumeDto.NumberTargetOZ.To(0);
                    distributedAdmissionVolume.NumberTargetZ = distributedAdmissionVolumeDto.NumberTargetZ.To(0);

                    _entrantEntities.DistributedAdmissionVolume.AddObject(distributedAdmissionVolume);
                    _distributedAdmissionVolumesImported++;
                }
            }
            _entrantEntities.SaveChanges();

            _successfulImportStatisticsDto.DistributedAdmissionVolumes = _distributedAdmissionVolumesImported.ToString();
        }

		#region Import Applications

		/// <summary>
		/// Вставка заявлений
		/// </summary>
        public void InsertApplications(int packageId)
		{
            var sw = new Stopwatch();
            /* Если нет заявлений - выходим */
            if (_insertStorage.Application.Count == 0)
                return;

            sw.Start();
            var index = 0;

            var login = StorageManager.UserLogin ?? UserHelper.GetAuthenticatedUserName();
            var packageSize = BulkImportConfig.ImportApplicationsPackageSize ?? _insertStorage.Application.Count;

            while (true)
            {
                var applications = _insertStorage.Application.Skip(index).Take(packageSize).ToList();
                if (applications.Count == 0)
                    break;

                using (var collector = new BulkEntitiesCollector(packageId, _institutionID))
                {
                    var bulks = collector.ApplicationCollector(applications).Collect();
                    if (bulks.Count > 0)
                    {
                        var imported = new SqlBulkUploader(packageId, login).Upload<ImportResult>(bulks, BulkImportDirection.Applications);
                        if (imported != null && imported.Successful.Count > 0)
                            _importedApplicationIDList.AddRange(imported.Successful.Select(c => new Tuple<int, int, int>(c.Id, 0, 4)));
                    }
                }

                index += packageSize;
            }
            _successfulImportStatisticsDto.Applications = _importedApplicationIDList.Count.ToString();

            sw.Stop();
            LogHelper.Log.InfoFormat("Пакет {2}. Время загрузки {0} заявлений = {1} сек", 
                _insertStorage.Application.Count, sw.Elapsed.TotalSeconds, packageId);
            System.Diagnostics.Debug.WriteLine("Пакет {2}. Время загрузки {0} заявлений = {1} сек", _insertStorage.Application.Count, sw.Elapsed.TotalSeconds, packageId);

            /* Сброс кешированных объектов, так как удаление существующих объектов происходит 
                * непосредственно в БД без использования EF */
            _importEntities.Flush();
		}

        public void InsertApplicationConsidered(int packageId)
        {
            var sw = new Stopwatch();
            sw.Start();

            //using (var collector = new BulkEntitiesCollector(_insertStorage, packageId, _institutionID))
            //{
            //    var login = StorageManager.UserLogin ?? UserHelper.GetAuthenticatedUserName();

            //    var consideredApplications = collector.ConsideredApplicationCollectors.Collect();
            //    var recommendedApplications = collector.RecommendedApplicationCollectors.Collect();

            //    var prepareTimeSec = sw.Elapsed.TotalSeconds;
            //    if (consideredApplications.Count > 0)
            //    {
            //        var consideredApplicationsImported = new SqlBulkUploader(_importEntities, packageId, login)
            //            .Upload<ConsideredApplicationsResult>(consideredApplications, BulkImportDirection.ConsideredApplications);
            //        if (consideredApplicationsImported != null && consideredApplicationsImported.Successful.Count > 0)
            //            _successfulImportStatisticsDto.ConsideredApplications = consideredApplicationsImported.Successful.Count.ToString();
            //        ProcessApplicationsConsideredResults<ConsideredApplicationDto>(consideredApplicationsImported);
            //    }

            //    if (recommendedApplications.Count > 0)
            //    {
            //        var recommendedApplicationsImported = new SqlBulkUploader(_importEntities, packageId, login)
            //            .Upload<ConsideredApplicationsResult>(recommendedApplications, BulkImportDirection.RecommendedApplications);
            //        if (recommendedApplicationsImported != null && recommendedApplicationsImported.Successful.Count > 0)
            //            _successfulImportStatisticsDto.RecommendedApplications = recommendedApplicationsImported.Successful.Count.ToString();
            //        ProcessApplicationsRecommendedResults<RecommendedApplicationDto>(recommendedApplicationsImported);
            //    }

            //    /* Сброс кешированных объектов, так как удаление существующих объектов происходит 
            //        * непосредственно в БД без использования EF */
            //    _importEntities.Flush();

            //    sw.Stop();
            //    LogHelper.Log.InfoFormat("Время загрузки рассмотренных и рекомендуемых заявлений = {1} сек. Подготовка к загрузке = {2} сек",
            //        sw.Elapsed.TotalSeconds, prepareTimeSec);
            //}

            return;
        }

        void ProcessApplicationsConsideredResults<TDto>(ConsideredApplicationsResult imported) where TDto : ConsideredApplicationDto, new()
        {
            /* Не найденные заявления */
            if (imported != null && imported.ApplicationNotFound.Count > 0)
            {
                imported.ApplicationNotFound.ForEach(c => ConflictStorage.AddNotImportedDto(c.MappingTo<TDto>(), 
                    ConflictMessages.ApplicationIsNotFound));
            }

            if (imported != null && imported.FinSourceFormNotFound.Count > 0)
            {
                imported.FinSourceFormNotFound.ForEach(c =>
                    ConflictStorage.AddNotImportedDto(c.MappingTo<TDto>(), ConflictMessages.FinFormSourceNotFound,
                    c.EducationFormID.ToString(), c.FinanceSourceID.ToString()));
            }

            if (imported != null && imported.ApplicationNotAccepted.Count > 0)
            {
                imported.ApplicationNotFound.ForEach(c => ConflictStorage.AddNotImportedDto(c.MappingTo<TDto>(),
                    ConflictMessages.ApplicationIsNotAcceptedForConsidered));
            }

            if (imported != null && imported.DirectionLevelNotFound.Count > 0)
            {
                imported.DirectionLevelNotFound.ForEach(c =>
                    ConflictStorage.AddNotImportedDto(c.MappingTo<TDto>(), ConflictMessages.DirectionLevelNotFound, 
                        c.DirectionID.ToString(), c.EducationLevelID.ToString()));
            }
        }

        void ProcessApplicationsRecommendedResults<TDto>(ConsideredApplicationsResult imported) where TDto : ConsideredApplicationDto, new()
        {
            /* Не найденные заявления */
            if (imported != null && imported.ApplicationNotFound.Count > 0)
            {
                imported.ApplicationNotFound.ForEach(c => ConflictStorage.AddNotImportedDto(c.MappingTo<TDto>(),
                    ConflictMessages.ApplicationIsNotFoundInConsidered));
            }

            if (imported != null && imported.AdmissionVolumeOverflow.Count > 0)
            {
                imported.ApplicationNotFound.ForEach(c => ConflictStorage.AddNotImportedDto(c.MappingTo<TDto>(),
                    ConflictMessages.AdmissionVolumeOverflow));
            }
        }

	    //copy from EntrantApplicationExtensions
		private void CalculateApplicationRating(ImportEntities dbContext, int applicationID)
		{
			DeletePackageHandler.CalculateApplicationRating(dbContext, applicationID);
			dbContext.SaveChanges();
		}

		/// <summary>
		/// заявление/предыдущий статус/статус
		/// </summary>
		public List<Tuple<int, int, int>> ImportedApplicationIDList
		{
			get { return _importedApplicationIDList; }
		}

		#endregion

        public void InsertRecommendedLists(int packageId)
        {
            //var login = StorageManager.UserLogin ?? UserHelper.GetAuthenticatedUserName();
            //using (var collector = new BulkEntitiesCollector(packageId, InstitutionID))
            //{
            //    var bulks = collector.BuildRecommendedListBulkEntityCollector(InsertStorage.RecommendedLists).Collect();

            //    if (bulks.Count > 0)
            //    {
            //        var imported = new SqlBulkUploader(packageId, login).Upload<ImportResult>(bulks, BulkImportDirection.RecommendedLists);
            //        _successfulImportStatisticsDto.RecommendedLists = InsertStorage.RecommendedLists.Count.ToString();
            //    }
            //}
        }

        public void InsertInstitutionAchievements(int packageId, int institutionId)
        {
            // распределенный объем приема
            GVUZ.Model.Entrants.EntrantsEntities _entrantEntities = new Model.Entrants.EntrantsEntities();

            // 1. Удалить старые записи по активным кампаниям данного ОО
            // Удаление отменено спецификацией от 12.05.2015
            //foreach (var itemToDelete in _entrantEntities.InstitutionAchievements.Where(t => t.Campaign.InstitutionID==institutionId && t.Campaign.StatusID==1))
            //    _entrantEntities.DeleteObject(itemToDelete);
            //_entrantEntities.SaveChanges();

            // 2. Записать новые или обновить уже имеющиеся в БД
            int imported = 0;
            foreach (InstitutionAchievementDto dto in _insertStorage.InstitutionAchievements)
            {
                if (!dto.IsBroken)
                {
                    var campaign = _entrantEntities.Campaign.Where(t => t.UID == dto.CampaignUID && t.InstitutionID == institutionId).FirstOrDefault(); // не может быть null, потому что это проверяется!
                    //GVUZ.Model.Entrants.InstitutionAchievements entity = _entrantEntities.InstitutionAchievements.Where(t => t.UID == dto.UID && t.CampaignID == campaign.CampaignID).FirstOrDefault();
                    GVUZ.Model.Entrants.InstitutionAchievements entity = _entrantEntities.InstitutionAchievements.Where(t => t.UID == dto.UID && t.Campaign.InstitutionID == institutionId).FirstOrDefault();

                    if (entity == null) // нет такой записи в БД => создание новой
                        entity = _entrantEntities.InstitutionAchievements.CreateObject();

                    entity.UID = dto.IAUID;
                    entity.Name = dto.Name;
                    entity.MaxValue = dto.MaxValue;
                    entity.Campaign = campaign;
                    entity.IdCategory = dto.IdCategory;

                    if (entity.EntityState == System.Data.EntityState.Added)
                        _entrantEntities.InstitutionAchievements.AddObject(entity);
                    imported++;
                }
            }
            _entrantEntities.SaveChanges();

            _successfulImportStatisticsDto.InstitutionAchievements = imported.ToString();
        }
	}
}
