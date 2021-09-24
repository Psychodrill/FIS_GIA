using System;
using System.Data;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq;
using AutoMapper;
using FogSoft.Helpers;
using GVUZ.Model.Institutions;
using GVUZ.ServiceModel.Import.Core.Storages;
using GVUZ.ServiceModel.Import.WebService.Dto;
using GVUZ.ServiceModel.Import.WebService.Dto.Result.Import;

namespace GVUZ.ServiceModel.Import.Core.Operations
{
	/// <summary>
	/// Обновление данных
	/// </summary>
	public class DbDataUpdateManager : StorageConsumer
	{
		private readonly DtoObjectStorage _updateStorage;
		private readonly ImportEntities _importEntities;

		private int _admissionVolumesImported;
		private int _campaignsImported;
		private int _competitiveGroupItemsImported;
	    private int _competitiveGroupsImported;
		private readonly SuccessfulImportStatisticsDto _successfulImportStatisticsDto;

		public DbDataUpdateManager(StorageManager storageManager, SuccessfulImportStatisticsDto successfulImportStatisticsDto)
			: base(storageManager)
		{
			_updateStorage = UpdateStorage;			
			_importEntities = DbObjectRepository.ImportEntities;
			_successfulImportStatisticsDto = successfulImportStatisticsDto;
		}

		/// <summary>
		/// Обновление кампаний
		/// </summary>
		public void UpdateCampaigns()
		{
			foreach (var campaignDto in _updateStorage.Campaign)
			{
				Campaign campaign = DbObjectRepository.Campaigns.SingleOrDefault(x => x.InstitutionID == InstitutionID && x.UID == campaignDto.UID);
				if (Log.IsNullError(campaign, "Не найдена кампания для обновления с UID {0}", campaignDto.UID))
					continue;
				
				campaign.Name = campaignDto.Name;
				campaign.YearStart = campaignDto.YearStart.To(DateTime.Now.Year);
				campaign.YearEnd = campaignDto.YearEnd.To(DateTime.Now.Year);
				campaign.StatusID = campaignDto.StatusID;
				campaign.EducationFormFlag = 0;
				if (campaignDto.EducationForms.Contains(EDFormsConst.O.ToString()))
					campaign.EducationFormFlag |= 1;
				if (campaignDto.EducationForms.Contains(EDFormsConst.OZ.ToString()))
					campaign.EducationFormFlag |= 2;
				if (campaignDto.EducationForms.Contains(EDFormsConst.Z.ToString()))
					campaign.EducationFormFlag |= 4;
				campaign.CampaignEducationLevel.ToList().ForEach(_importEntities.CampaignEducationLevel.DeleteObject);
				foreach (var celDto in campaignDto.EducationLevels)
				{
					CampaignEducationLevel cel = _importEntities.CampaignEducationLevel.CreateObject();
					cel.Campaign = campaign;
					cel.Course = celDto.Course.To(0);
					cel.EducationLevelID = (short)celDto.EducationLevelID.To(0);
				}

				//удаляем существующие даты и заново их вставляем
				campaign.CampaignDate.ToList().ForEach(_importEntities.CampaignDate.DeleteObject);
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

				_campaignsImported++;
			}

			_importEntities.SaveChanges();
			_successfulImportStatisticsDto.campaignsImported = (_successfulImportStatisticsDto.Campaigns.To(0) + _campaignsImported);
		}



		/// <summary>
		/// Обновление объёма приёма и КГ
		/// </summary>
		public void UpdateInstitutionStructure()
		{
            _importEntities.Flush();

			// объем приема
			foreach (AdmissionVolumeDto admissionVolumeDto in _updateStorage.AdmissionVolume)
			{
				string admissionVolumeUID = admissionVolumeDto.UID;
				AdmissionVolume admissionVolume = DbObjectRepository.AdmissionVolumes.SingleOrDefault(x => 
                    x.UID == admissionVolumeUID && x.InstitutionID == InstitutionID);
                
				if (!Log.IsNullError(admissionVolume, "Не найден объем приема для обновления с UID {0}", admissionVolumeUID))
				{
                    _importEntities.AttachAndMap(admissionVolumeDto, admissionVolume);
					admissionVolume.Course = admissionVolumeDto.Course.To(0);
					admissionVolume.CampaignID = _importEntities.Campaign.Where(x => 
                        x.InstitutionID == InstitutionID && x.UID == admissionVolumeDto.CampaignUID).Select(x => x.CampaignID).FirstOrDefault();

					if (admissionVolume.CampaignID == 0)
					{
						var error = "Not found Campaign With UID " + admissionVolumeDto.CampaignUID + " for admission volume UID " + admissionVolumeDto.UID + ". Institution ID" + InstitutionID;
						Log.ErrorFormat(error);
						throw new ArgumentNullException(error);
					}
					_admissionVolumesImported++;

                    StorageManager.InsertStorage.packageAdmissionVolumes.Add(admissionVolume);
				}
			}
            _importEntities.SaveChanges();

             // Если есть, удалить все DistirubtedAdmissionValues у данного AV
            //DistributedAdmissionVolume


            foreach (CompetitiveGroupDto competitiveGroupDto in _updateStorage.CompetitiveGroup)
            {
                string cgUID = competitiveGroupDto.UID;
                CompetitiveGroup competitiveGroup = DbObjectRepository.CompetitiveGroups.SingleOrDefault(x =>
                    x.UID == cgUID && x.InstitutionID == InstitutionID);
                if (!Log.IsNullError(competitiveGroup, "Не найедна КГ для обновления с UID {0}", cgUID))
                {
                    _importEntities.AttachAndMap(competitiveGroupDto, competitiveGroup);
                    competitiveGroup.Course = (short)competitiveGroupDto.Course.To(0);
                    competitiveGroup.CampaignID =
                        _importEntities.Campaign.Where(x => x.InstitutionID == InstitutionID && x.UID == competitiveGroupDto.CampaignUID)
                        .Select(x => x.CampaignID)
                        .FirstOrDefault();

                    if (competitiveGroup.CampaignID == 0)
                    {
                        var error = "Not found Campaign With UID " + competitiveGroupDto.CampaignUID + " for Competitive Group UID " + competitiveGroupDto.UID + ". Institution ID" + InstitutionID;
                        Log.ErrorFormat(error);
                        throw new ArgumentNullException(error);
                    }
                    _competitiveGroupsImported++;
                }
            }
            _importEntities.SaveChanges();

			// Направления в КГ
			foreach (CompetitiveGroupItemDto competitiveGroupItemDto in _updateStorage.CompetitiveGroupItem)
			{				
				string cgItemUID = competitiveGroupItemDto.UID;
				//_importEntities.CompetitiveGroupItem.Where(x => x.UID == cgItemUID);				

				CompetitiveGroupItem competitiveGroupItem = _importEntities.CompetitiveGroupItem.SingleOrDefault(x => 
                    x.UID == cgItemUID && x.CompetitiveGroup.InstitutionID == InstitutionID);

				if (!Log.IsNullError(competitiveGroupItem, "Не найдено направление КГ для обновления с UID {0}", cgItemUID))
				{
                    _importEntities.AttachAndMap(competitiveGroupItemDto, competitiveGroupItem);
					_competitiveGroupItemsImported++;
				}
			}
            _importEntities.SaveChanges();

			// целевой прием
			foreach (CompetitiveGroupTargetDto competitiveGroupTargetDto in _updateStorage.CompetitiveGroupTarget)
			{
				string cgtUID = competitiveGroupTargetDto.UID;
				CompetitiveGroupTarget competitiveGroupTarget = _importEntities.CompetitiveGroupTarget.SingleOrDefault(x => 
                    x.UID == cgtUID && x.InstitutionID == InstitutionID);
				if (!Log.IsNullError(competitiveGroupTarget, "Не найден целевой прием для обновления с UID {0}", cgtUID))
                    _importEntities.AttachAndMap(competitiveGroupTargetDto, competitiveGroupTarget);
			}
            _importEntities.SaveChanges();

			// направления в КГ целевого приема
			foreach (CompetitiveGroupTargetItemDto competitiveGroupTargetItemDto in _updateStorage.CompetitiveGroupTargetItem)
			{
				string cgtItemUID = competitiveGroupTargetItemDto.UID;
				CompetitiveGroupTargetItem competitiveGroupTargetItem = _importEntities.CompetitiveGroupTargetItem.SingleOrDefault(x => 
                    x.UID == cgtItemUID && x.CompetitiveGroupItem.CompetitiveGroup.InstitutionID == InstitutionID);
				if (!Log.IsNullError(competitiveGroupTargetItem, "Не найдено направление целевого приема для обновления с UID {0}", cgtItemUID))
                    _importEntities.AttachAndMap(competitiveGroupTargetItemDto, competitiveGroupTargetItem);
			}
            _importEntities.SaveChanges();

			// общие льготы для КГ
			foreach (BenefitItemDto benefitItemDto in _updateStorage.CompetitiveGroupBenefitItem)
			{
				string benefitItemUID = benefitItemDto.UID;
				BenefitItemC benefitItemC = _importEntities.BenefitItemC.SingleOrDefault(x => 
                    x.UID == benefitItemUID && x.CompetitiveGroup.InstitutionID == InstitutionID);

				if (!Log.IsNullError(benefitItemC, "Не найдена общая льгота для обновления с UID {0}", benefitItemUID))
                    _importEntities.AttachAndMap(benefitItemDto, benefitItemC);
				else continue;

				benefitItemC.BenefitItemCOlympicType.Load();
				// удаляем олимпиады для льготы
				foreach (var benefitItemCOlympicType in benefitItemC.BenefitItemCOlympicType)
					_importEntities.BenefitItemCOlympicType.DeleteObject(benefitItemCOlympicType);

			    if (benefitItemDto.IsForAllOlympics.To(false))
			    {
			        benefitItemC.OlympicYear = DateTime.Now.Year;
			        benefitItemC.OlympicLevelFlags = 7;
			    }

                /* заполняем флаги олимпиад */
                AddBenefitItemCOlympicTypeAndSetOlympicLevelFlags(_importEntities, benefitItemC, benefitItemDto);
                AddMinEgeScoresForCommonBenefit(_importEntities, benefitItemC, benefitItemDto);
            }
            _importEntities.SaveChanges();

			// ВИ для КГ
			foreach (EntranceTestItemDto entranceTestItemDto in _updateStorage.CompetitiveGroupEntranceTestItem)
			{
				string entranceTestItemUID = entranceTestItemDto.UID;
				EntranceTestItemC entranceTestItem = _importEntities.EntranceTestItemC.FirstOrDefault(x => 
                    x.UID == entranceTestItemUID && x.CompetitiveGroup.InstitutionID == InstitutionID);

			    if (!Log.IsNullError(entranceTestItem, "Не найдено ВИ для обновления с UID {0}", entranceTestItemUID))
			    {
			        _importEntities.AttachAndMap(entranceTestItemDto, entranceTestItem);
                    continue;
			    }
			    //проставляем нужный балл, если не указан #37138
				if (entranceTestItemDto.EntranceTestTypeID == "1"
					&& entranceTestItemDto.EntranceTestSubject.SubjectID.To(0) > 0
					&& string.IsNullOrEmpty(entranceTestItemDto.MinScore))
				{
					entranceTestItem.MinScore = DbObjectRepository.GetSubjectEgeMinValue(entranceTestItemDto.EntranceTestSubject.SubjectID.To(0));
				}
			}
            _importEntities.SaveChanges();

			// льготы для ВИ в КГ
			foreach (BenefitItemDto benefitItemDto in _updateStorage.CompetitiveGroupEntranceTestBenefitItem)
			{
				string benefitItemUID = benefitItemDto.UID;
				BenefitItemC benefitItemC = _importEntities.BenefitItemC.Single(x => x.UID == 
                    benefitItemUID && x.CompetitiveGroup.InstitutionID == InstitutionID);

				if (!Log.IsNullError(benefitItemC, "Не найдена льгота для ВИ для обновления с UID {0}", benefitItemUID))
                    _importEntities.AttachAndMap(benefitItemDto, benefitItemC);
				else continue;

				benefitItemC.BenefitItemCOlympicType.Load();
				// удаляем олимпиады для льготы
				foreach (var benefitItemCOlympicType in benefitItemC.BenefitItemCOlympicType)
					_importEntities.BenefitItemCOlympicType.DeleteObject(benefitItemCOlympicType);

			    if (benefitItemDto.IsForAllOlympics.To(false))
			    {
			        benefitItemC.OlympicYear = DateTime.Now.Year;
			        benefitItemC.OlympicLevelFlags = 7;
			    }

                /* заполняем флаги олимпиад */
                AddBenefitItemCOlympicTypeAndSetOlympicLevelFlags(_importEntities, benefitItemC, benefitItemDto);
			}
            _importEntities.SaveChanges();

			var admVolumes = _successfulImportStatisticsDto.AdmissionVolumes.To(0);
			_successfulImportStatisticsDto.AdmissionVolumes = (admVolumes + _admissionVolumesImported).ToString();

            var cgVolumes = _successfulImportStatisticsDto.CompetitiveGroups.To(0);
            _successfulImportStatisticsDto.CompetitiveGroups = (cgVolumes + _competitiveGroupsImported).ToString();

			var cgItemsVolumes = _successfulImportStatisticsDto.CompetitiveGroupItems.To(0);
			_successfulImportStatisticsDto.CompetitiveGroupItems = (cgItemsVolumes + _competitiveGroupItemsImported).ToString();
		}
	} 
}
