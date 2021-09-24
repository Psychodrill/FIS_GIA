using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using FogSoft.Helpers;
using GVUZ.Helper.Import;
using GVUZ.Model;
using GVUZ.Model.Helpers;
using GVUZ.ServiceModel.Import.Bulk.Model.Results;
using GVUZ.ServiceModel.Import.Core.Operations.Conflicts;
using GVUZ.ServiceModel.Import.Package;
using GVUZ.ServiceModel.Import.Schemas;
using GVUZ.ServiceModel.Import.WebService.Dto;
using GVUZ.ServiceModel.Import.WebService.Dto.Result;
using GVUZ.ServiceModel.Import.WebService.Dto.Result.Import;

namespace GVUZ.ServiceModel.Import.Core.Packages.Handlers
{
	/// <summary>
	/// Обработчик пакетов удаления
	/// </summary>
	public class DeletePackageHandler : PackageHandler
	{
		private readonly ImportPackage _repositoryPackageData;
		private readonly ConflictsResultDto _dataForDelete;
		private readonly DbObjectRepositoryBase _dbObjectRepository;
		private readonly ImportEntities _dbContext;

		private int InstitutionID
		{
			get { return _repositoryPackageData.InstitutionID; }
		}

		public DeletePackageHandler(ImportPackage repositoryPackageData)
		{
			_repositoryPackageData = repositoryPackageData;
            _dataForDelete = new Serializer().Deserialize<DataForDelete>(_repositoryPackageData.PackageData);
			if (_dataForDelete == null)
				throw new ImportException("Incorrect package structure.");

            int applicationsCount = 0;
            if (_dataForDelete.Applications != null)
            {
                applicationsCount = _dataForDelete.Applications.Length;
            }

            if ((applicationsCount > 0) && (applicationsCount < DbObjectRepositoryBase.DecisionApplicationsCount))
            {
                _dbObjectRepository = DbObjectRepositoryBase.Create(InstitutionID);
            }
            else
            {
                _dbObjectRepository = DbObjectRepositoryBase.CreateWithCache(InstitutionID);
            }

			_dbContext = _dbObjectRepository.ImportEntities;
		}

        public override string ValidatePackage(string packageData, PackageType packageType)
		{
			return ValidatePackage(packageData, XsdManager.XsdName.DoDeleteServiceRequest);
		}

		public override string Process()
		{
			//модель соответствует, десериализуем в неё
			var checkPackage = _dataForDelete;
			ExcludeApplicationsFromOrder(checkPackage.OrdersOfAdmission ?? new ApplicationShortRef[0]);
            RemoveApplications(checkPackage.Applications ?? new ApplicationShortRef[0]);
            RemoveCompetitiveGroups(checkPackage.CompetitiveGroups ?? new string[0]);
			RemoveCompetitiveGroupItems(checkPackage.CompetitiveGroupItems ?? new string[0]);
			RemoveEntranceTestResults(checkPackage.EntranceTestResults ?? new string[0]);
			RemoveCommonBenefitsResults(checkPackage.ApplicationCommonBenefits ?? new string[0]);
            RemoveCampaigns(checkPackage.Campaigns ?? new string[0]);
            RemoveInstitutionAchievements(checkPackage.InstitutionAchievements ?? new string[0]);

			return PackageHelper.GenerateXmlPackageIntoString(PrepareProcessResultObject());
		}

		#region Exclude from order

		private int _removedOrdersCount = 0;
		private readonly List<ApplicationFailDetailsDto> _notExcludedApps = new List<ApplicationFailDetailsDto>();

		/// <summary>
		/// Убираем заявления из приказа
		/// </summary>
		/// <param name="apps"></param>
		private void ExcludeApplicationsFromOrder(IEnumerable<ApplicationShortRef> apps)
		{
			Application[] dbApps = _dbContext.Application.Where(x => 
                x.InstitutionID == InstitutionID && x.StatusID == ApplicationStatusType.InOrder).ToArray();

			foreach (var appDto	in apps)
			{
				ApplicationShortRef dto = appDto;
				DateTime dtoRegDate = dto.RegistrationDateDate;
				Application app = dbApps.Where(x => x.ApplicationNumber == dto.ApplicationNumber && x.RegistrationDate == dtoRegDate).FirstOrDefault();
				if (app != null)
				{
					app.OrderOfAdmission = null;
					app.OrderOfAdmissionID = null;
					app.OrderCompetitiveGroupItemID = null;
					app.OrderCompetitiveGroupID = null;
					app.OrderCalculatedBenefitID = null;
					app.OrderCompetitiveGroupTargetID = null;
					app.OrderCalculatedRating = 0;
					app.StatusID = ApplicationStatusType.Accepted;
					_removedOrdersCount++;
				}
				else
				{
					_notExcludedApps.Add(new ApplicationFailDetailsDto
					                     {
					                     	ApplicationNumber = appDto.ApplicationNumber,
					                     	RegistrationDate = appDto.RegistrationDateString,
					                     	ErrorInfo = new ErrorInfoImportDto(ConflictMessages.ApplicationIsNotFoundOrAbsentInOrder)
					                     });
				}
			}

			_dbContext.SaveChanges();
		}

		#endregion

		#region Remove App

		private int _removedAppCount = 0;
		private readonly List<ApplicationFailDetailsDto> _notRemovedApps = new List<ApplicationFailDetailsDto>();

		/// <summary>
		/// Удаляем заявления
		/// </summary>
		private void RemoveApplications(IEnumerable<ApplicationShortRef> apps)
		{
            var sw = new Stopwatch();
            sw.Start();
            bool logEnabled = PersonalDataAccessLogger.Enabled;
            var results = _dbContext.InsertAsXml<ApplicationShortRef, DeleteApplicationsResult>(apps.ToList().Distinct(),
                "exec DeleteApplications_fromXml @xml, @institutionId, @userLogin, @logEnabled", 5000,
                    new SqlParameter("institutionId", InstitutionID), 
                    new SqlParameter("userLogin", _repositoryPackageData.UserLogin),
                    new SqlParameter("logEnabled", logEnabled));

            /* Обработка результатов */
            _removedAppCount = apps.Count();
            foreach (var result in results)
            {
                /* Заявления со статусами InOrder */
                _removedAppCount = _removedAppCount - result.ApplicationIsInOrder.Count;
                _notRemovedApps.AddRange(result.ApplicationIsInOrder.Select(c => new ApplicationFailDetailsDto
                    {
                        ApplicationNumber = c.ApplicationNumber,
                        RegistrationDate = c.RegistrationDate.GetDateTimeAsString(),
                        ErrorInfo = new ErrorInfoImportDto(ConflictMessages.ApplicationIsInOrder)
                    }));

                /* Не найденные заявления */
                _removedAppCount = _removedAppCount - result.ApplicationIsNotFound.Count;
                _notRemovedApps.AddRange(result.ApplicationIsNotFound.Select(c => new ApplicationFailDetailsDto
                    {
                        ApplicationNumber = c.ApplicationNumber,
                        RegistrationDate = c.RegistrationDate.GetDateTimeAsString(),
                        ErrorInfo = new ErrorInfoImportDto(ConflictMessages.ApplicationIsNotFound)
                    }));
            }
                
            sw.Stop();
            LogHelper.Log.InfoFormat("Время удаления {0} заявлений (Bulk) = {1} сек", apps.Count(), sw.Elapsed.TotalSeconds);
        }
        
		#endregion

		#region Remove Campaigns

		private int _removedCampaignsCount = 0;
		private readonly List<CampaignDetailsFailDto> _notRemovedCampaigns = new List<CampaignDetailsFailDto>();

		/// <summary>
		/// Удаляем кампании
		/// </summary>
        private void RemoveCampaigns(IEnumerable<string> items)
        {
            //StorageManager storageManager = new StorageManager(_dbObjectRepository);
            //DbDataDeleteManager manager = new DbDataDeleteManager(storageManager);
            //var campaigns = _dbContext.Campaign.Where(x => x.InstitutionID == InstitutionID).ToArray(););)
            foreach (var itemUIDStr in items.Distinct())
            {
                Campaign campaign = _dbContext.Campaign.Where(x => x.CampaignID.ToString() == itemUIDStr).FirstOrDefault();
                if (campaign == null)
                {
                    _notRemovedCampaigns.Add(new CampaignDetailsFailDto
                    {
                        ErrorInfo = new ErrorInfoImportDto(ConflictMessages.CampaignIsNotFound)
                    });
                    continue;
                }

                if (_dbContext.CompetitiveGroup.Where(x => x.CampaignID == campaign.CampaignID).Any())
                {
                    _notRemovedCampaigns.Add(new CampaignDetailsFailDto
                    {
                        ErrorInfo = new ErrorInfoImportDto(ConflictMessages.CampaignHasDependentCompetitiveGroups)
                    });
                    continue;
                }

                GVUZ.Model.Entrants.EntrantsEntities _entrantEntities = new Model.Entrants.EntrantsEntities();
                if (_entrantEntities.InstitutionAchievements.Where(t=> t.CampaignID == campaign.CampaignID).Any())
                {
                    _notRemovedCampaigns.Add(new CampaignDetailsFailDto
                    {
                        ErrorInfo = new ErrorInfoImportDto(ConflictMessages.CampaignHasDependentInstitutionArchievements)
                    });
                    continue;
                }


                _dbContext.AdmissionVolume.Where(x => x.CampaignID == campaign.CampaignID).ToList().ForEach(_dbContext.AdmissionVolume.DeleteObject);
                _dbContext.Campaign.DeleteObject(campaign);
                _dbContext.SaveChanges();
                _removedCampaignsCount++;
            }
        }

		#endregion

        #region Remove RemoveInstitutionAchievements
        private int _removedInstitutionAchievementsCount = 0;
        private readonly List<InstitutionAchievementFailDetailsDto> _notRemovedInstitutionAchievements = new List<InstitutionAchievementFailDetailsDto>();
        private void RemoveInstitutionAchievements(IEnumerable<string> items)
        {
            GVUZ.Model.Entrants.EntrantsEntities _entrantEntities = new Model.Entrants.EntrantsEntities();
            foreach (var uid in items.Distinct())
            {
                var  ia = _entrantEntities.InstitutionAchievements.Where(x => x.UID == uid).FirstOrDefault();
                if (ia == null)
                {
                    _notRemovedInstitutionAchievements.Add(new InstitutionAchievementFailDetailsDto
                    {
                        ErrorInfo = new ErrorInfoImportDto(ConflictMessages.InstitutionAchievementIsNotFound),
                        IAUID = uid
                    });
                    continue;
                }
                
                // todo Roman: проверять, что на запись не ссылаются другие таблицы? например, IndividualArchievements
                _entrantEntities.DeleteObject(ia);
                _removedInstitutionAchievementsCount++;
            }
            _entrantEntities.SaveChanges();
        }
        #endregion

        #region Remove CompetitiveGroupItems

        private int _removedCompetitiveGroupsCount;
        private readonly List<CompetitiveGroupFailDetailsDto> _notRemovedCompetitiveGroups = new List<CompetitiveGroupFailDetailsDto>();

		private int _removedCompetitiveGroupItemsCount;
		private readonly List<CompetitiveGroupItemFailDetailsDto> _notRemovedCompetitiveGroupItems = new List<CompetitiveGroupItemFailDetailsDto>();

        void RemoveCompetitiveGroups(IEnumerable<string> items)
        {
            if (!items.Any()) return;

            var groupsIds = new HashSet<int>();
            groupsIds.UnionWith(items.Select(c => int.Parse(c)));

            var competitiveGroups = 
                _dbContext.CompetitiveGroup
                .Where(c => c.InstitutionID == InstitutionID)
                .WhereIn(c => c.CompetitiveGroupID, groupsIds, chunkSize: 500).ToList();

            /* Не найденные группы */
            var findedIds = new HashSet<int>();
            findedIds.UnionWith(competitiveGroups.Select(c => c.CompetitiveGroupID));
            var notFinded = groupsIds.Where(c => !findedIds.Contains(c));
            foreach (var id in notFinded)
            {
                _notRemovedCompetitiveGroups.Add(new CompetitiveGroupFailDetailsDto
                {
                    ErrorInfo = new ErrorInfoImportDto(ConflictMessages.CompetitiveGroupIsNotFound, 
                        new ConflictsResultDto { CompetitiveGroups = new[] { id.ToString() } })
                });                
            }

            /* пробегаемся по тем которые нашлись в БД - пробуем удалить */
            foreach (var group in competitiveGroups)
            {
                /* направления */
                var groupItems = _dbContext.CompetitiveGroupItem.Where(x => x.CompetitiveGroupID == group.CompetitiveGroupID)
                    .Select(c => c.CompetitiveGroupItemID).ToArray();
                if (groupItems.Length > 0)
                {
                    _notRemovedCompetitiveGroups.Add(new CompetitiveGroupFailDetailsDto
                    {
                        CompetitiveGroupName = group.Name,
                        ErrorInfo = 
                            new ErrorInfoImportDto(ConflictMessages.CantDeleteCompetitiveGroupWithGroupItems, 
                            new ConflictsResultDto { CompetitiveGroupItems = groupItems.Select(c => c.ToString()).ToArray() })});
                    continue;
                }

                /* заявления */
                var applications = new HashSet<ApplicationShortRef>();
                applications.UnionWith(_dbContext.Application.Where(x => x.CompetitiveGroupItem.CompetitiveGroupID == group.CompetitiveGroupID)
                   .Select(c => new ApplicationShortRef { ApplicationNumber = c.ApplicationNumber, RegistrationDateDate = c.RegistrationDate}));
                applications.UnionWith(_dbContext.Application.Where(x => 
                    x.ApplicationSelectedCompetitiveGroup.Select(c => c.CompetitiveGroupID).Contains(group.CompetitiveGroupID))
                    .Select(c => new ApplicationShortRef { ApplicationNumber = c.ApplicationNumber, RegistrationDateDate = c.RegistrationDate }));

                if (applications.Count > 0)
                {
                    _notRemovedCompetitiveGroups.Add(new CompetitiveGroupFailDetailsDto
                    {
                        CompetitiveGroupName = group.Name,
                        ErrorInfo = 
                            new ErrorInfoImportDto(ConflictMessages.CantDeleteCompetitiveGroupWithApplications, 
                            new ConflictsResultDto { Applications = applications.ToArray() })});
                    continue;
                }
                /*льготы*/
                _dbContext.BenefitItemC.Where(x => x.CompetitiveGroupID == group.CompetitiveGroupID).ToList()
                    .ForEach(_dbContext.BenefitItemC.DeleteObject);
                _dbContext.CompetitiveGroup.DeleteObject(group);
                _removedCompetitiveGroupsCount++;
            }
        }
        
	    /// <summary>
		/// Удаляем направления КГ
		/// </summary>
		private void RemoveCompetitiveGroupItems(IEnumerable<string> items)
		{
			foreach (string itemIDStr in items)
			{
				int itemID = itemIDStr.To(0);
				CompetitiveGroupItem cgi = _dbContext.CompetitiveGroupItem
				                                     .Include(x => x.Direction)
				                                     .Include(x => x.CompetitiveGroup).FirstOrDefault(x => 
                                                         x.CompetitiveGroup.InstitutionID == InstitutionID && x.CompetitiveGroupItemID == itemID);
				if (cgi != null)
				{
					var appArr = _dbContext.Application.Where(x => x.OrderCompetitiveGroupID == cgi.CompetitiveGroupID
																   && x.InstitutionID == InstitutionID
																   && x.OrderCompetitiveGroupItemID == cgi.CompetitiveGroupItemID)
																   .Select(x => x)
																   .ToArray();
					if (appArr.Length > 0)
					{
						_notRemovedCompetitiveGroupItems.Add(new CompetitiveGroupItemFailDetailsDto
						            {
						            CompetitiveGroupName = cgi.CompetitiveGroup.Name,
						            DirectionCode = cgi.Direction.Code,
						            DirectionName = cgi.Direction.Name,
									ErrorInfo = new ErrorInfoImportDto(ConflictMessages.CantDeleteDirectionLinkedWithAppInOrder,
						                        new ConflictsResultDto
						                                {
						                                Applications = appArr.Select(x => new ApplicationShortRef
						                                {
							                                ApplicationNumber = x.ApplicationNumber,
															RegistrationDateDate = x.RegistrationDate
						                                }).ToArray()
						                                })
						            });
						//throw new ImportException("Невозможно удалить направление, так как по нему есть заявления в приказе");
						continue;
					}

					//есть заявления с выбранными направленяим - в конфликт
					if (_dbContext.ApplicationSelectedCompetitiveGroupItem.Any(x => x.CompetitiveGroupItemID == cgi.CompetitiveGroupItemID))
					{
						_notRemovedCompetitiveGroupItems.Add(new CompetitiveGroupItemFailDetailsDto
						{
							CompetitiveGroupName = cgi.CompetitiveGroup.Name,
							DirectionCode = cgi.Direction.Code,
							DirectionName = cgi.Direction.Name,
							ErrorInfo = new ErrorInfoImportDto(ConflictMessages.CantDeleteDirectionInCompetitiveGroupWithApplications,
								new ConflictsResultDto
								{
									CompetitiveGroupItems = new[] { itemIDStr }
								})
						});
						continue;
					}

					int cgiCount = _dbContext.CompetitiveGroupItem.Count(x => x.CompetitiveGroupID == cgi.CompetitiveGroupID);
					//последнее направление
					if (cgiCount == 1)
					{
						int appCnt = _dbContext.ApplicationSelectedCompetitiveGroup.Count(x => x.CompetitiveGroupID == cgi.CompetitiveGroupID
						                                                                       && x.CompetitiveGroup.InstitutionID == InstitutionID);
						if (appCnt > 0)
						{
							//throw new ImportException(
							//	"Невозможно удалить направление, так как это последнее направление в конкурсной группе и существуют заявления на эту группу");
							_notRemovedCompetitiveGroupItems.Add(new CompetitiveGroupItemFailDetailsDto
							                                     {
							                                     	CompetitiveGroupName = cgi.CompetitiveGroup.Name,
							                                     	DirectionCode = cgi.Direction.Code,
							                                     	DirectionName = cgi.Direction.Name,
																										ErrorInfo = new ErrorInfoImportDto(ConflictMessages.CantDeleteLastDirectionInCompetitiveGroupWithApplications,
																											new ConflictsResultDto
							                                     	  {
							                                     	    CompetitiveGroupItems = new[] { itemIDStr }
							                                     	  })
							                                     });
							continue;
						}

						//последнее направление, поэтому выносим все ВИ, потому что у нас не осталось информации о том, какие правильные
						_dbContext.EntranceTestItemC.Where(x => x.CompetitiveGroupID == cgi.CompetitiveGroupID).ToList()
                            .ForEach(_dbContext.EntranceTestItemC.DeleteObject);
                        
					}

					_dbContext.CompetitiveGroupTargetItem
						.Where(x => x.CompetitiveGroupItemID == cgi.CompetitiveGroupItemID)
						.ToList().ForEach(_dbContext.CompetitiveGroupTargetItem.DeleteObject);
					_dbContext.CompetitiveGroupItem.DeleteObject(cgi);
					_removedCompetitiveGroupItemsCount++;
				}
				else
				{
					_notRemovedCompetitiveGroupItems.Add(new CompetitiveGroupItemFailDetailsDto
					        {
						        ErrorInfo = new ErrorInfoImportDto(ConflictMessages.CompetitiveGroupItemIsNotFound,
																		new ConflictsResultDto
					                                     					{
					                                     						CompetitiveGroupItems = new[] { itemIDStr }
					                                     					})
											});
				}
			}

			//_dbContext.SaveChanges();
			//пустые группы, они теперь нам не нужны, будут только мешаться
			//подумал, решил не удалять, есть связи на Target и Applications 
			//var competitiveGroups = _dbContext.CompetitiveGroup
			//	.Where(x => x.InstitutionID == InstitutionID && x.CompetitiveGroupItem.Count() == 0).ToList();
			//competitiveGroups.ForEach(_dbContext.CompetitiveGroup.DeleteObject);
			_dbContext.SaveChanges();
		}

		#endregion

		private int _removedEntranceTestResultsCount;
		private readonly List<EntranceTestItemFailDetailsDto> _notRemovedEntranceTests = new List<EntranceTestItemFailDetailsDto>();

		/// <summary>
		/// Удаляем РВИ
		/// </summary>
		private void RemoveEntranceTestResults(IEnumerable<string> items)
		{
			var affectedApps = new HashSet<int>();
			foreach (string itemIDStr in items)
			{
				int itemID = itemIDStr.To(0);
				
				var etDoc = _dbContext.ApplicationEntranceTestDocument
				                      .Include(x => x.Application)
				                      .Include(x => x.Application.ApplicationSelectedCompetitiveGroup)
				                      .Include(x => x.Application.ApplicationSelectedCompetitiveGroupItem)
				                      .Include(x => x.EntranceTestItemC)
				                      .Include(x => x.EntranceTestItemC.Subject)
				                      .Include(x => x.EntranceTestItemC.CompetitiveGroup)
				                      .Include(x => x.EntranceTestItemC.EntranceTestType).FirstOrDefault(x => x.ID == itemID
				                                                                                              && x.Application.InstitutionID == InstitutionID
				                                                                                              && x.EntranceTestItemID != null);

				if (etDoc != null && etDoc.Application.StatusID == ApplicationStatusType.InOrder /*&& etDoc.SourceID == 3*/)
				{
					//throw new ImportException("Невозможно удалить результат ВИ, потому что есть заявления в приказе с данной льготой");
					_notRemovedEntranceTests.Add(new EntranceTestItemFailDetailsDto
					{
						CompetitiveGroupName = etDoc.EntranceTestItemC.CompetitiveGroup.Name,
						EntranceTestType = etDoc.EntranceTestItemC.EntranceTestType.Name,
						SubjectName = etDoc.EntranceTestItemC.Subject == null ? etDoc.EntranceTestItemC.SubjectName : etDoc.EntranceTestItemC.Subject.Name,
						ErrorInfo = new ErrorInfoImportDto(ConflictMessages.CantDeleteResultLinkedWithAppInOrder, 
							new ConflictsResultDto
							{
								EntranceTestResults = new[] { itemIDStr },
								Applications = new[] { new ApplicationShortRef { ApplicationNumber = etDoc.Application.ApplicationNumber, RegistrationDateDate = etDoc.Application.RegistrationDate } }
							})
					});
					continue;
				}

				if (etDoc != null)
				{
					//отцепляем от заявления документ, если он больше нигде не используется
					var doc = _dbContext.ApplicationEntrantDocument.FirstOrDefault(x => x.EntrantDocumentID == etDoc.EntrantDocumentID);
					if (doc != null)
					{
						int docCnt = _dbContext.ApplicationEntranceTestDocument.Count(x => x.EntrantDocumentID == etDoc.EntrantDocumentID
						                                                                   && x.ID != etDoc.ID
						                                                                   && x.ApplicationID == etDoc.ApplicationID);
						if (docCnt == 0)
							_dbContext.ApplicationEntrantDocument.DeleteObject(doc);
					}

					if (!affectedApps.Contains(etDoc.ApplicationID))
						affectedApps.Add(etDoc.ApplicationID);
					_dbContext.ApplicationEntranceTestDocument.DeleteObject(etDoc);
					_removedEntranceTestResultsCount++;
				}
				else
				{
					_notRemovedEntranceTests.Add(new EntranceTestItemFailDetailsDto
					{
						ErrorInfo = new ErrorInfoImportDto(ConflictMessages.AppEntranceTestIsNotFound,
							new ConflictsResultDto
							  {
							    EntranceTestResults = new[] { itemIDStr }
							  })
					});
				}
			}

			_dbContext.SaveChanges();
			RecalculateRatingForAffectedApps(affectedApps);
		}
		
		private readonly List<CommonBenefitFailDetailsDto> _notRemovedCommonBenefits = new List<CommonBenefitFailDetailsDto>();

		/// <summary>
		/// Удаляем льготы
		/// </summary>
		private void RemoveCommonBenefitsResults(IEnumerable<string> items)
		{
			HashSet<int> affectedApps = new HashSet<int>();
			foreach (string itemIDStr in items)
			{
				int itemID = itemIDStr.To(0);

				var etDoc = _dbContext.ApplicationEntranceTestDocument
				                      .Include(x => x.Application)
				                      .Include(x => x.CompetitiveGroup)
				                      .Include(x => x.Benefit).FirstOrDefault(x => x.ID == itemID
				                                                                   && x.Application.InstitutionID == InstitutionID
				                                                                   && x.EntranceTestItemID == null);
				if (etDoc != null && etDoc.Application.StatusID == ApplicationStatusType.InOrder && etDoc.Benefit != null)
				{
					//throw new ImportException("Невозможно общую льготу, потому что есть заявления в приказе с данной льготой");
					_notRemovedCommonBenefits.Add(new CommonBenefitFailDetailsDto
					{
						BenefitKindName = etDoc.Benefit.Name,
						CompetitiveGroupName = etDoc.CompetitiveGroup.Name,
						ErrorInfo = new ErrorInfoImportDto(ConflictMessages.CantDeleteCommonBenefitUsedInApp, 
							new ConflictsResultDto
							{
								ApplicationCommonBenefits = new[] { itemIDStr },
								Applications = new[] { new ApplicationShortRef { ApplicationNumber = etDoc.Application.ApplicationNumber, RegistrationDateDate = etDoc.Application.RegistrationDate } }
							})
					});
					continue;
				}

				if (etDoc != null)
				{
					//отцепляем от заявления документ, если он больше нигде не используется
					var doc = _dbContext.ApplicationEntrantDocument.Where(x => x.EntrantDocumentID == etDoc.EntrantDocumentID).FirstOrDefault();
					if (doc != null)
					{
						int docCnt = _dbContext.ApplicationEntranceTestDocument.Count(x => x.EntrantDocumentID == etDoc.EntrantDocumentID
						                                                                   && x.ID != etDoc.ID
						                                                                   && x.ApplicationID == etDoc.ApplicationID);
						if (docCnt == 0)
							_dbContext.ApplicationEntrantDocument.DeleteObject(doc);
					}

					if (!affectedApps.Contains(etDoc.ApplicationID))
						affectedApps.Add(etDoc.ApplicationID);
					_dbContext.ApplicationEntranceTestDocument.DeleteObject(etDoc);
				}
				else
				{
					_notRemovedCommonBenefits.Add(new CommonBenefitFailDetailsDto
					{
						ErrorInfo = new ErrorInfoImportDto(ConflictMessages.ApplicationBenefitIsNotFound)
					});
				}
			}

			_dbContext.SaveChanges();
			RecalculateRatingForAffectedApps(affectedApps);
		}

		/// <summary>
		/// See EntrantApplicationExtensions for rating, here is clone of method due diffent entities
		/// </summary>
		private void RecalculateRatingForAffectedApps(IEnumerable<int> appIDs)
		{
			foreach (int appID in appIDs)
			{
				CalculateApplicationRating(_dbContext, appID);
			}

			_dbContext.SaveChanges();
		}

		/// <summary>
		/// Обновляем рейтинг заявления. метод скопирован, т.к. тут другие контексты
		/// </summary>
		/// <param name="dbContext"></param>
		/// <param name="applicationID"></param>
		//copy from ApplicationRatingCalculator
		public static void CalculateApplicationRating(ImportEntities dbContext, int applicationID)
		{
			ApplicationEntranceTestDocument[] etDocsAll = dbContext.ApplicationEntranceTestDocument.Include(x => x.EntranceTestItemC)
				.Where(x => x.ApplicationID == applicationID && x.EntranceTestItemID != null).ToArray();

			ApplicationEntranceTestDocument[] globalDocAll = dbContext.ApplicationEntranceTestDocument
                .Where(x => x.ApplicationID == applicationID && x.EntranceTestItemID == null).ToArray();
			
            Application app = dbContext.Application
                                       .Include(x => x.ApplicationSelectedCompetitiveGroup).Single(x => x.ApplicationID == applicationID);
			
            foreach (var ascg in app.ApplicationSelectedCompetitiveGroup)
			{
				var etDocs = etDocsAll.Where(x => x.EntranceTestItemC.CompetitiveGroupID == ascg.CompetitiveGroupID).ToArray();
				decimal totalRating = etDocs.Sum(doc => (doc.ResultValue ?? 0));
				ascg.CalculatedRating = totalRating;
				var globalDoc = globalDocAll.FirstOrDefault(x => x.CompetitiveGroupID == ascg.CompetitiveGroupID);
				int benefitType = 0;
				if (globalDoc != null && globalDoc.BenefitID != null)
					benefitType = globalDoc.BenefitID.Value;
				else
				{
					foreach (var doc in etDocs)
					{
						if (doc.BenefitID.HasValue)
							if (benefitType != 2)
								benefitType = doc.BenefitID.Value;
					}
				}
				ascg.CalculatedBenefitID = (benefitType > 0 ? (short?)benefitType : null);
				if (ascg.CompetitiveGroupID == app.OrderCompetitiveGroupID)
				{
					app.OrderCalculatedRating = ascg.CalculatedRating;
					app.OrderCalculatedBenefitID = ascg.CalculatedBenefitID;
				}
			}
    	}

		public override void AddExtraInfoToPackage(ImportPackage importPackage)
		{
		}

		/// <summary>
		/// Готовим ответ клиенту
		/// </summary>
		private DeleteResultPackage PrepareProcessResultObject()
		{
			var x = new DeleteResultPackage
			       {
			       		Conflicts = null,
						Log = new LogDto
						      {
						      	Successful = new SuccessfulImportStatisticsDto
						      	             {
												Applications = _removedAppCount.ToString(),
                                                CompetitiveGroups = _removedCompetitiveGroupsCount.ToString(),
												CompetitiveGroupItems = _removedCompetitiveGroupItemsCount.ToString(),
												ordersImported = _removedOrdersCount,
												campaignsImported = _removedCampaignsCount,
                                                InstitutionAchievements = _removedInstitutionAchievementsCount.ToString()
						      	             },
								Failed = new FailedImportInfoDto
								         {
								         	Applications = _notRemovedApps.ToArray(),
											//OrdersOfAdmissions = _notExcludedApps.ToArray(),
                                            //OrderOfAdmissionsFails = _notRemovedOrders.ToArray(),
                                            CompetitiveGroups = _notRemovedCompetitiveGroups.ToArray(),
											CompetitiveGroupItems = _notRemovedCompetitiveGroupItems.ToArray(),
											CommonBenefit = _notRemovedCommonBenefits.ToArray(),
											EntranceTestItems = _notRemovedEntranceTests.ToArray(),
											Campaigns = _notRemovedCampaigns.ToArray(),
                                            InstitutionAchievements = _notRemovedInstitutionAchievements.ToArray()
								         }
						      }
			       };
			if (x.Log.Failed.Applications.Length == 0)
				x.Log.Failed.Applications = null;
			if (x.Log.Failed.OrdersOfAdmissions.Length == 0)
				x.Log.Failed.OrdersOfAdmissions = null;
            if (x.Log.Failed.ApplicationsInOrders.Length == 0)
                x.Log.Failed.ApplicationsInOrders = null;
            if (x.Log.Failed.CompetitiveGroups.Length == 0)
                x.Log.Failed.CompetitiveGroups = null;
            if (x.Log.Failed.CompetitiveGroupItems.Length == 0)
				x.Log.Failed.CompetitiveGroupItems = null;
			if (x.Log.Failed.CommonBenefit.Length == 0)
				x.Log.Failed.CommonBenefit = null;
			if (x.Log.Failed.EntranceTestItems.Length == 0)
				x.Log.Failed.EntranceTestItems = null;
			if (x.Log.Failed.Campaigns.Length == 0)
				x.Log.Failed.Campaigns = null;
			return x;
		}

		public override void Dispose()
		{
			_dbObjectRepository.Dispose();
		}
	}
}
