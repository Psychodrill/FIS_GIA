using GVUZ.ImportService2016.Core.Main.Dictionaries;
using GVUZ.ImportService2016.Core.Main.Repositories;
using GVUZ.ImportService2016.Core.Dto.DataReaders;
using GVUZ.ImportService2016.Core.Dto.Import;
using GVUZ.ServiceModel.Import.Core.Operations.Conflicts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GVUZ.ImportService2016.Core.Main.Extensions;
using GVUZ.ServiceModel.Import.Core.Storages;
using log4net;

namespace GVUZ.ImportService2016.Core.Main.Import
{
    public class CampaignInfoImporter : BaseImporter
    {
        public static readonly ILog cii_logger = LogManager.GetLogger("CampaignInfoImporter");

        public CampaignInfoImporter(PackageData packageData, VocabularyStorage vocabularyStorage, ImportConflictStorage importConflictStorage, bool deleteBulk) : base(packageData, vocabularyStorage, importConflictStorage, deleteBulk) { }
        public const int VersionMinYear = 2016;

        protected override void Validate()
        {

            if (packageData.CampaignInfo != null && packageData.CampaignInfo.Campaigns != null)
            {
                // Для проверки уникальности UID-а campaign в пакете, а также пар CampaignTypeID и StartYear
                List<Tuple<string, int, int>> campaignUIDs = new List<Tuple<string, int, int>>(); 
                foreach (var campaign in packageData.CampaignInfo.Campaigns)
                {
                    // 1. Проверка уникальности Campaign.UID в рамках пакета
                    if (campaignUIDs.Any(t => t.Item1 == campaign.UID))
                    {
                        conflictStorage.SetObjectIsBroken(campaign, ConflictMessages.UIDMustBeUniqueForAllObjectInstancesOfType, "Приемная кампания", campaign.UID);
                        continue;
                    }
                    else if (campaignUIDs.Any(t => t.Item2 == campaign.CampaignTypeID && t.Item3 == campaign.YearStart))
                    {
                        conflictStorage.SetObjectIsBroken(campaign, ConflictMessages.CampaignWithSameTypeAndYearExistsPackage);
                        continue;
                    }
                    else
                        campaignUIDs.Add(new Tuple<string, int, int>(campaign.UID, campaign.CampaignTypeID.To(0), campaign.YearStart.To(0)));

                    // Проверка того, что в БД у данного ОО нет записи с таким же Name, но другим UID
                    if (vocabularyStorage.CampaignVoc.Items.Where(t => string.Equals(t.Name, campaign.Name, StringComparison.OrdinalIgnoreCase) && t.UID != campaign.UID).Any())
                    {
                        conflictStorage.SetObjectIsBroken(campaign, ConflictMessages.CampaignWithSameNameExists);
                        continue;
                    }

                    if (vocabularyStorage.CampaignVoc.Items.Any(t=> t.CampaignTypeID == campaign.CampaignTypeID && t.YearStart == campaign.YearStart && t.UID != campaign.UID))
                    {
                        conflictStorage.SetObjectIsBroken(campaign, ConflictMessages.CampaignWithSameTypeAndYearExistsDb);
                        continue;
                    }

                    // 3. статус д.б. из справончика статусов
                    if (!VocabularyStatic.CampaignStatusVoc.Items.Any(t => t.StatusID == campaign.StatusID))
                    {
                        conflictStorage.SetObjectIsBroken(campaign, ConflictMessages.DictionaryItemAbsent, "Campaign.StatusID=" + campaign.StatusID.ToString());
                        continue;
                    }

                    // тип д.б. из справончика типов
                    if (!VocabularyStatic.CampaignTypeVoc.Items.Any(t => t.CampaignTypeID == campaign.CampaignTypeID))
                    {
                        conflictStorage.SetObjectIsBroken(campaign, ConflictMessages.DictionaryItemAbsent, "Campaign.CampaignTypeID=" + campaign.CampaignTypeID.ToString());
                        continue;
                    }

                    // Разделение версий 2015 и 2016 года. 
                    if (campaign.YearStart < VersionMinYear || campaign.YearEnd < VersionMinYear)
                    {
                        //conflictStorage.SetObjectIsBroken(campaign, ConflictMessages.CampaignWithInvalidData, string.Format("Campaign.YearStart={0}, Campaign.YearEnd={1}", campaign.YearStart.ToString(), campaign.YearEnd.ToString()));
                        conflictStorage.SetObjectIsBroken(campaign,
                                         new ConflictStorage.ConflictMessage()
                                         {
                                             Code = ConflictMessages.CampaignWithInvalidData,
                                             Message = String.Format(
                                                     "Год приемной кампании не может быть меньше 2016! Campaign.YearStart={0}, Campaign.YearEnd={1}", campaign.YearStart.ToString(), campaign.YearEnd.ToString()
                                                     )
                                         });

                        continue;
                    }

                    // Если создание новой записи - нужно просто пройти все проверки
                    // А вот если update (есть у ОО с таким UID) - то надо проверить, что нет внешних ссыллок!
                    var dbCampaign = vocabularyStorage.CampaignVoc.Items.Where(t => t.UID == campaign.UID).FirstOrDefault();
                    //bool checkChanges = false;
                    if (dbCampaign != null)
                    {
                        var checkCampaignChanges = CheckCampaignChanges(campaign, dbCampaign);
                        var checkCampaignEducationLevelsChanges = CheckCampaignEducationLevelsChanges(campaign, dbCampaign);

                        if (checkCampaignChanges || checkCampaignEducationLevelsChanges)
                        {
                            if (campaign.StatusID != dbCampaign.StatusID)
                            {
                                ADOBaseRepository.ChangeCampaignStatus(dbCampaign.CampaignID, campaign.StatusID.To(0));
                            }
                            conflictStorage.successfulImportStatisticsDto.campaignsImported++;
                            campaign.IsBroken = true;
                            continue;
                        }
                        else if (checkCampaignChanges)
                        {
                            // При обновлении, проверяется наличие внешних ссылок из таблиц: 
                            // AdmissionVolume, InstitutionAchievements, CompetitiveGroup, RecomendedLists, OrderOfAdmission.
                            var admissionVolumes = vocabularyStorage.AdmissionVolumeVoc.Items.Where(t => t.CampaignID == dbCampaign.CampaignID);
                            var institutionAchievements = vocabularyStorage.InstitutionAchievementsVoc.Items.Where(t => t.CampaignID == dbCampaign.CampaignID);
                            var competitiveGroups = vocabularyStorage.CompetitiveGroupVoc.Items.Where(t => t.CampaignID == dbCampaign.CampaignID);
                            var recomendedLists = vocabularyStorage.RecomendedListsVoc.Items.Where(t => t.CampaignID == dbCampaign.CampaignID);
                            var orderOfAdmissions = vocabularyStorage.OrderOfAdmissionVoc.Items.Where(t => t.CampaignID == dbCampaign.CampaignID);

                            if (admissionVolumes.Any())
                                conflictStorage.AddAdmissionVolumes(campaign, new HashSet<int>(admissionVolumes.Select(x => x.AdmissionVolumeID)));
                            if (institutionAchievements.Any())
                                conflictStorage.AddinstitutionAchievements(campaign, new HashSet<int>(institutionAchievements.Select(x => x.IdAchievement)));
                            if (competitiveGroups.Any())
                                conflictStorage.AddCompetitiveGroups(campaign, new HashSet<CompetitiveGroupVocDto>(competitiveGroups));
                            if (recomendedLists.Any())
                                conflictStorage.AddRecomendedLists(campaign, new HashSet<int>(recomendedLists.Select(x => x.RecListID)));
                            if (orderOfAdmissions.Any())
                                conflictStorage.AddOrderOfAdmissions(campaign, new HashSet<OrderOfAdmissionVocDto>(orderOfAdmissions));

                        }
                        else if (checkCampaignEducationLevelsChanges)
                        {
                            // Отдельно вынесено, потому что другая логика: 
                            // CompetitiveGroup - могут быть ссылки на имеющиеся EducationLevel, нельзя удалять тот, на который есть ссылка, а добавлять новый - можно!

                            var competitiveGroups = vocabularyStorage.CompetitiveGroupVoc.Items.Where(t => t.CampaignID == dbCampaign.CampaignID);
                            var conflictCG = competitiveGroups.Where(t => campaign.EducationLevels == null || !campaign.EducationLevels.Any(x => t.EducationLevelID == x));
                            if (conflictCG.Any())
                            {
                                conflictStorage.AddCompetitiveGroups(campaign, new HashSet<CompetitiveGroupVocDto>(competitiveGroups));
                            }

                            // Аналогично AdmissionVolume
                            var admissionVolumes = vocabularyStorage.AdmissionVolumeVoc.Items.Where(t => t.CampaignID == dbCampaign.CampaignID);
                            var conflictAV = admissionVolumes.Where(t => campaign.EducationLevels == null || !campaign.EducationLevels.Any(x => t.AdmissionItemTypeID == x));
                            if (conflictAV.Any())
                            {
                                conflictStorage.AddAdmissionVolumes(campaign, new HashSet<int>(admissionVolumes.Select(x => x.AdmissionVolumeID)));
                            }
                        }
                    }

                    // 2. всё ли в порядке с годами
                    if (campaign.YearEnd < campaign.YearStart
                        || campaign.YearEnd > campaign.YearStart + 1
                        || campaign.YearStart < 1900
                        || string.IsNullOrWhiteSpace(campaign.Name)
                        || campaign.EducationLevels == null)
                    {
                        conflictStorage.SetObjectIsBroken(campaign, ConflictMessages.CampaignWithInvalidData);
                        return;
                    }

                    

                    // 4. проверка EducationForms по справочнику
                    if (campaign.EducationForms != null)
                        foreach (var educationFormID in campaign.EducationForms)
                        {
                            if (!VocabularyStatic.AdmissionItemTypeVoc.GetEducationForm().Any(t => t.ItemTypeID == educationFormID))
                                conflictStorage.SetObjectIsBroken(campaign, ConflictMessages.DictionaryItemAbsent, "Campaign.EducationForms.EducationFormID=" + educationFormID.ToString());
                        }

                    // 5. не лишние ли уровни образования (проверка по AllowedDirections)
                    //var allowedDirectionsByInstition = vocabularyStorage.AllowedDirectionsVoc.Items.Select(x => x.AdmissionItemTypeID).Distinct();
                    if (campaign.EducationLevels != null)
                        foreach (var eduLevel in campaign.EducationLevels)
                        {
                            if (!VocabularyStatic.EduLevelsToCampaignTypesVoc.Items.Any(t=> t.CampaignTypeID == campaign.CampaignTypeID && t.AdmissionItemTypeID == eduLevel) )
                            {
                                var admItemType = VocabularyStatic.AdmissionItemTypeVoc.Items.Where(t => t.ItemTypeID == eduLevel).FirstOrDefault();
                                string eduLevelName = admItemType != null ? admItemType.Name : eduLevel.ToString();

                                conflictStorage.SetObjectIsBroken(campaign, ConflictMessages.CampaignWithInvalidEducationLevelsByInstitution, eduLevelName);
                            }
                        }
                }
            }
        }

        /// <summary>
        /// Проверки, что у ПК не изменились поля или изменился только Статус (тогда можно обновлять независимо от зависимостей)
        /// </summary>
        /// <param name="campaign"></param>
        /// <param name="dbCampaign"></param>
        /// <returns>true если изменился только StatusID или ничего, false - если изменилось что-то еще из полей или дочерних элементов</returns>
        private bool CheckCampaignChanges(PackageDataCampaignInfoCampaign campaign, CampaignVocDto dbCampaign)
        {
            // сравнить скалярные поля
            // можно менять UID и Name, остальные скалярные поля менять нельзя 
            if (//campaign.Name != dbCampaign.Name
                campaign.YearStart != dbCampaign.YearStart
                || campaign.YearEnd != dbCampaign.YearEnd
                || campaign.CampaignTypeID != dbCampaign.CampaignTypeID
                || campaign.EducationFormFlag() != dbCampaign.EducationFormFlag)
            {
                cii_logger.WarnFormat("Кампания №{0} Попытка изменения неразрешённых параметров (не UID ({1}) и Name {2})!", campaign.GUID, campaign.UID, campaign.Name);
                return false;
            }

            return true;
        }

        private bool CheckCampaignEducationLevelsChanges(PackageDataCampaignInfoCampaign campaign, CampaignVocDto dbCampaign)
        {
            // сравнить CampaignEducationLevels
            var dbCampaignLevels = vocabularyStorage.CampaignEducationLevelVoc.Items.Where(t=> t.CampaignID == dbCampaign.CampaignID);
            if (campaign.EducationLevels == null && dbCampaignLevels.Any())
                return false;
            if (campaign.EducationLevels != null)
            {
                if (campaign.EducationLevels.Count() != dbCampaignLevels.Count())
                    return false;
                foreach (var cd in campaign.EducationLevels)
                {
                    var dbCampaignLevel = dbCampaignLevels.Where(t => t.EducationLevelID == cd).FirstOrDefault();
                    if (dbCampaignLevel == null) return false;
                }
            }

            return true;
        }

        //private void CheckCourseAndEducationLevels(ICourseEdLevel[] CourseAndEduLevels, PackageDataCampaignInfoCampaign campaign)
        //{
        //    if (CourseAndEduLevels == null) return;
        //    // Course - от 1 до 6, иначе ошибка 36
        //    if (CourseAndEduLevels.Any(t=> t.Course < 1 || t.Course > 6))
        //        conflictStorage.SetObjectIsBroken(campaign, ConflictMessages.DictionaryItemAbsent, "Course"); 

        //    // Проверка на соответствие уровня образования номеру курса, производится на основании сопоставления EducationLevelID и Course: 
        //    // • Прием на бакалавриат (полн.) (EducationLevelID in (2,19)) может проходить на 4 курса
        //    // • Прием на бакалавриат (сокращ.)  (EducationLevelID in (3))возможен только на 1 курс (теперь на 3)
        //    // • Прием на специалитет  (EducationLevelID in (5)) может проходить на 6 курсов
        //    // • Прием в магистратуру  (EducationLevelID in (4)) возможен только на 1 курс
        //    // • Прием кадров высшей квалификации (EducationLevelID in (18)) возможен только на 1 курс
        //    // Иначе ошибка 67
        //    var edLevels = CourseAndEduLevels.Where(x =>
        //                ((x.EducationLevelID == GVUZ.Model.Institutions.EDLevelConst.Bachelor || x.EducationLevelID == GVUZ.Model.Institutions.EDLevelConst.AppliedBachelor) && x.Course > 4)
        //                || (x.EducationLevelID == GVUZ.Model.Institutions.EDLevelConst.BachelorShort && x.Course > 3)
        //                || (x.EducationLevelID == GVUZ.Model.Institutions.EDLevelConst.Magistracy && x.Course > 1)
        //                || (x.EducationLevelID == GVUZ.Model.Institutions.EDLevelConst.Speciality && x.Course > 6)
        //                || (x.EducationLevelID == GVUZ.Model.Institutions.EDLevelConst.HighQualification && x.Course > 1)).ToArray();
        //    if (edLevels.Length > 0)
        //    {
        //        conflictStorage.SetObjectIsBroken(campaign,
        //            new GVUZ.ServiceModel.Import.Core.Storages.ConflictStorage.ConflictMessage
        //            {
        //                Code = ConflictMessages.CampaignWithInvalidEducationLevelsCombination,
        //                Message = String.Format(ConflictMessages.GetMessage(ConflictMessages.CampaignWithInvalidEducationLevelsCombination),
        //                    String.Join("; ", edLevels.Select(x => x.Course + " курс, "
        //                        + vocabularyStorage.AdmissionItemTypeVoc.Items.Where(t => t.ItemTypeID == x.EducationLevelID).FirstOrDefault() != null ?
        //                                vocabularyStorage.AdmissionItemTypeVoc.Items.Where(t => t.ItemTypeID == x.EducationLevelID).FirstOrDefault().Name : x.EducationLevelID.ToString())))
        //            });
        //    }
        //}

        protected override List<string> ImportDb()
        {
            List<Tuple<string, IDataReader>> bulks = new List<Tuple<string, IDataReader>>();
            
            #region CampaignInfo
            bulks.Add(new Tuple<string,IDataReader>("bulk_Campaign", new BulkCampaignReader(packageData)));
            bulks.Add(new Tuple<string,IDataReader>("bulk_CampaignDate", new BulkCampaignDateReader(packageData)));
            #endregion CampaignInfo

            // Должен прийти список импортированных кампаний - пополнить справочники и записать количество в successfulImportStatisticsDto 
            var res = ADOPackageRepository.BulkInsertData(packageData, bulks, "ImportCampaignInfo", deleteBulk, cii_logger);
            DataSet dsResult = res.Item1;
            if (dsResult != null && dsResult.Tables.Count > 0)
            {
                conflictStorage.successfulImportStatisticsDto.campaignsImported += dsResult.Tables[0].Rows.Count;

                vocabularyStorage.CampaignVoc.AddItems(dsResult.Tables[0]);
#warning данные на выходе!
                //if (dsResult.Tables.Count > 1)
                //    VocabularyStatic.CampaignDateVoc.AddItems(dsResult.Tables[1]);
                if (dsResult.Tables.Count > 2)
                    vocabularyStorage.CampaignEducationLevelVoc.AddItems(dsResult.Tables[2]);
            }
            return res.Item2;
        }
    }
}
