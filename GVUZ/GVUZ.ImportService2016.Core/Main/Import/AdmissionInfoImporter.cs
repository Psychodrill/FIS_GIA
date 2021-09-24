using GVUZ.ImportService2016.Core.Main.Dictionaries;
using GVUZ.ServiceModel.Import.Core.Operations.Conflicts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GVUZ.ImportService2016.Core.Dto.Import;
using GVUZ.ServiceModel.Import.Core.Storages;
using GVUZ.ServiceModel.Import;
using System.Data;
using GVUZ.ImportService2016.Core.Main.Repositories;
using GVUZ.ImportService2016.Core.Dto.DataReaders.AdmissionInfo;
using GVUZ.ImportService2016.Core.Dto.DeleteManager;
using System.Diagnostics;
using GVUZ.ImportService2016.Core.Dto.DataReaders;
using GVUZ.ImportService2016.Core.Main.Extensions;
using GVUZ.ImportService2016.Core.Main.Log;
using GVUZ.Model.Institutions;
using GVUZ.ImportService2016.Core.Main.Dictionaries.Olympic;
using GVUZ.DAL.Dapper.Repository.Model.Dictionary;
using log4net;

namespace GVUZ.ImportService2016.Core.Main.Import
{
    public class AdmissionInfoImporter : BaseImporter
    {
        // Отдельный логгер для отладки импорта объёма приёма
        public static readonly ILog aii_logger = LogManager.GetLogger("AdmissionInfoImporter");

        public AdmissionInfoImporter(PackageData packageData, VocabularyStorage vocabularyStorage, ImportConflictStorage importConflictStorage, bool deleteBulk) : base(packageData, vocabularyStorage, importConflictStorage, deleteBulk)
        {

            aii_logger.DebugFormat("Проверка пакета {0} для ОО {1}...", packageData.ImportPackageId, packageData.InstitutionId);
        }

        //Tuple<List<AdmissionVolumeDeleteManager>, List<CompetitiveGroupDeleteManager>, List<CompetitiveGroupItemDeleteManager>, List<CompetitiveGroupTargetDeleteManager>, List<CompetitiveGroupTargetItemDeleteManager>, List<EntranceTestItemDeleteManager>, List<BenefitItemDeleteManager>> deleteItems;

        CGApplicationDependencyVoc dependencyVoc = null;

        protected override void Validate()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            if (packageData.AdmissionInfo != null)
            {
                dependencyVoc = ADOCompetitiveGroupDependency.GetCGTApplicationDependency(packageData.InstitutionId);
                DebugMessage(sw, "Validate - ADOCompetitiveGroupDependency");
                aii_logger.DebugFormat("Валидация зависимостей от заявлений для ОО {0}...", packageData.InstitutionId);


                var availableDirectionsForInstitute = vocabularyStorage.AllowedDirectionsVoc.Items
                    .Select(x => new { EducationLevelID = x.AdmissionItemTypeID, x.DirectionID, x.ParentDirectionID }).Distinct().ToList();



                DebugMessage(sw, "Validate - AvaibleDirections");
                #region AdmissionVolume
                if (packageData.AdmissionInfo.AdmissionVolume != null)
                {
                    aii_logger.DebugFormat("Валидация допустимых направлений для объёма (всего записей: {0})", packageData.AdmissionInfo.AdmissionVolume.Length);

                    // Проверка уникальности AdmissionVolume.UID в рамках пакета
                    // IsPlan: Проверяем отдельно IsPlan и !IsPlan
                    CheckUniqueUID(packageData.AdmissionInfo.AdmissionVolume.Where(t => t.IsPlan).ToArray(), null);
                    CheckUniqueUID(packageData.AdmissionInfo.AdmissionVolume.Where(t => !t.IsPlan).ToArray(), null);

                    foreach (var admissionVolume in packageData.AdmissionInfo.AdmissionVolume)
                    {
                        // TODO: IsPlan

                        // Проверка ограничений по направлениям в КГ.
                        bool isFail = false;

                        // Проверка, что в БД нет AdmissionVolume с таким же UID, но привязанного к другой CampaignID
                        if (vocabularyStorage.AdmissionVolumeVoc.Items.Any(t => t.UID == admissionVolume.UID 
                        && t.CampaignUID != admissionVolume.CampaignUID))
                        {
                            conflictStorage.SetObjectIsBroken(admissionVolume, ConflictMessages.AdmissionVolumeHasSameUIDAnotherCampaign);
                            isFail = true;
                        }

                        //// Проверка, что в БД нет AdmissionVolume с таким же UID и привязанного к ЭТОЙ Campaign
                        //if (vocabularyStorage.AdmissionVolumeVoc.Items.Any(t => t.UID == admissionVolume.UID 
                        //&& t.CampaignUID == admissionVolume.CampaignUID))
                        //{
                        //    conflictStorage.SetObjectIsBroken(admissionVolume, ConflictMessages.AdmissionVolumeHasSameUID, admissionVolume.CampaignUID);
                        //    isFail = true;
                        //}
                        // не разрешены для института направления  

                        if (admissionVolume.DirectionID != 0)
                        {
                            if (!availableDirectionsForInstitute
                            .Any(x => (x.DirectionID == admissionVolume.DirectionID && x.EducationLevelID == admissionVolume.EducationLevelID)))
                            {
                                // Ошибка №37
                                conflictStorage.SetObjectIsBroken(admissionVolume, ConflictMessages.AdmissionVolumeContainsNotAllowedDirections);
                                isFail = true;
                            }
                        }

                        if (admissionVolume.ParentDirectionID != null)
                        {
                            if (!availableDirectionsForInstitute
                            .Any(x => (x?.ParentDirectionID == admissionVolume.ParentDirectionID && x.EducationLevelID == admissionVolume.EducationLevelID)))
                            {
                                conflictStorage.SetObjectIsBroken(admissionVolume, ConflictMessages.AdmissionVolumeContainsNotAllowedDirections);
                                isFail = true;
                            }
                        }                                      

                        // в admissionVolume одновременно задано направление и УГС
                        if (admissionVolume.ParentDirectionID != null && admissionVolume.DirectionID != 0)
                        {
                            conflictStorage.SetObjectIsBroken(admissionVolume, ConflictMessages.AdmissionVolumeContainsDirectionAndParentDirection);
                            isFail = true;
                        }

                        // не та кампания
                        var campaign = vocabularyStorage.CampaignVoc.Items.Where(t => t.UID == admissionVolume.CampaignUID).FirstOrDefault();
                        if (campaign == null)
                        {
                            conflictStorage.SetObjectIsBroken(admissionVolume, ConflictMessages.AdmissionVolumeContainsNotAllowedCampaign);
                            isFail = true;
                        }
                        // не тот уровень образования у ПК
                        else if (!vocabularyStorage.CampaignEducationLevelVoc.Items.
                            Where(x => x.CampaignID == campaign.CampaignID && x.EducationLevelID == admissionVolume.EducationLevelID).Any())
                        {
                            conflictStorage.SetObjectIsBroken(admissionVolume, ConflictMessages.AdmissionVolumeContainsNotAllowedCampaignEducationLevel);
                            isFail = true;
                        }

                        if (campaign != null)
                        {
                            admissionVolume.CampaignID = campaign.CampaignID;

                            // Если значение [Source]Number[Form] > 0, то проверка, что у кампании с CampaignUID есть такая форма в Campaign.EducationFormFlag
                            if (
                                ((admissionVolume.NumberBudgetO + admissionVolume.NumberPaidO + admissionVolume.NumberQuotaO + admissionVolume.NumberTargetO > 0) && ((campaign.EducationFormFlag & 1) == 0)) ||
                                ((admissionVolume.NumberBudgetOZ + admissionVolume.NumberPaidOZ + admissionVolume.NumberQuotaOZ + admissionVolume.NumberTargetOZ > 0) && ((campaign.EducationFormFlag & 2) == 0)) ||
                                ((admissionVolume.NumberBudgetZ + admissionVolume.NumberPaidZ + admissionVolume.NumberQuotaZ + admissionVolume.NumberTargetZ > 0) && ((campaign.EducationFormFlag & 4) == 0))
                                )
                            {
                                conflictStorage.SetObjectIsBroken(admissionVolume, ConflictMessages.AdmissionVolumeContainsNotAllowedCampaignEducationForm);
                                isFail = true;
                            }
                        }

                        // если нет ошибки, смотрим места, иначе нечего смотреть
                        // IsPlan: проверка актуальна только для 
                        if (!isFail && !admissionVolume.IsPlan)
                        {
                            // если есть DistributedAdmissionVolume в пакете, то надо теперь и для него проверить места (FIS-1790)
                            if (packageData.AdmissionInfo.DistributedAdmissionVolume != null)
                            {
                                aii_logger.DebugFormat("Проверка объёма и распределённого объёма приёма пакета №{0}: (всего: {1})",
                                                       packageData.ImportPackageId, packageData.AdmissionInfo.DistributedAdmissionVolume.Length);
                                // IsPlan: нужно брать только факт. distributedAdmissionVolume
                                foreach (var distributedAdmissionVolume in packageData.AdmissionInfo.DistributedAdmissionVolume.Where(t => t.AdmissionVolumeUID == admissionVolume.UID && !t.IsPlan))
                                {
                                    CheckAllowedPlacesForAdmissionVolumeAndCompetitiveGroups(admissionVolume, distributedAdmissionVolume, null);
                                }
                            }
                            else
                            {
                                aii_logger.DebugFormat("Проверка объёма приёма пакета №{0}. Распределённый объём приёма отсутствует...", packageData.ImportPackageId);
                                // иначе без DistributedAdmissionVolume проверим
                                CheckAllowedPlacesForAdmissionVolumeAndCompetitiveGroups(admissionVolume, null, null);
                            }
                        }

                    } // foreach admissionVolume

                    
                    // проверяем на повторяющиеся значения в объёме приёма
                    var failGroupedVolumes = packageData.AdmissionInfo.AdmissionVolume
                       .GroupBy(x => new { x.DirectionID, x.CampaignUID, x.EducationLevelID, x.IsPlan, x.ParentDirectionID })
                       .Where(x => x.Count() > 1)
                       .ToArray();
                    if (failGroupedVolumes.Any())
                    {
                        foreach (var admissionVolumeGroup in failGroupedVolumes)
                        {
                            foreach (var volumeDto in admissionVolumeGroup)
                            {
                                conflictStorage.SetObjectIsBroken(volumeDto, ConflictMessages.AdmissionVolumeNonUniqueDirections);
                            }
                        }
                    }
                }
                #endregion

                DebugMessage(sw, "Validate - AdmissionVolume");

                #region DistributedAdmissionVolume
                //FIS-1737(1) проверка на то что AV уменьшился, а DAV нет
                #region checkIfDBdav>AV
                if (packageData.AdmissionInfo.AdmissionVolume != null)
                {
                    foreach (var group in packageData.AdmissionInfo.AdmissionVolume.Where(t => !t.IsPlan).GroupBy(t => new { t.UID, t.IsPlan }))
                    {
                        PackageDataAdmissionInfoItem av = null;
                        // взять соотв AV из пакета или БД
                        if (packageData.AdmissionInfo.AdmissionVolume != null)
                        {
                            av = packageData.AdmissionInfo.AdmissionVolume.Where(t => t.UID == group.Key.UID  && t.IsPlan == group.Key.IsPlan).FirstOrDefault();
                        }
                        if (av == null || av.IsBroken )
                        {
                            if (group.Key.IsPlan)
                            {
                                var dbPavs = vocabularyStorage.PlanAdmissionVolumeVoc.Items.Where(t => t.UID == group.Key.UID);
                                if (dbPavs.Any())
                                    av = new PackageDataAdmissionInfoItem()
                                    {
                                        UID = group.Key.UID,
                                        NumberBudgetO = (uint)dbPavs.Where(t => t.EducationFormID == EDFormsConst.O && t.EducationSourceID == EDSourceConst.Budget).Sum(t => t.Number),
                                        NumberBudgetOZ = (uint)dbPavs.Where(t => t.EducationFormID == EDFormsConst.OZ && t.EducationSourceID == EDSourceConst.Budget).Sum(t => t.Number),
                                        NumberBudgetZ = (uint)dbPavs.Where(t => t.EducationFormID == EDFormsConst.Z && t.EducationSourceID == EDSourceConst.Budget).Sum(t => t.Number),
                                        NumberQuotaO = (uint)dbPavs.Where(t => t.EducationFormID == EDFormsConst.O && t.EducationSourceID == EDSourceConst.Quota).Sum(t => t.Number),
                                        NumberQuotaOZ = (uint)dbPavs.Where(t => t.EducationFormID == EDFormsConst.OZ && t.EducationSourceID == EDSourceConst.Quota).Sum(t => t.Number),
                                        NumberQuotaZ = (uint)dbPavs.Where(t => t.EducationFormID == EDFormsConst.Z && t.EducationSourceID == EDSourceConst.Quota).Sum(t => t.Number),
                                        NumberTargetO = (uint)dbPavs.Where(t => t.EducationFormID == EDFormsConst.O && t.EducationSourceID == EDSourceConst.Target).Sum(t => t.Number),
                                        NumberTargetOZ = (uint)dbPavs.Where(t => t.EducationFormID == EDFormsConst.OZ && t.EducationSourceID == EDSourceConst.Target).Sum(t => t.Number),
                                        NumberTargetZ = (uint)dbPavs.Where(t => t.EducationFormID == EDFormsConst.Z && t.EducationSourceID == EDSourceConst.Target).Sum(t => t.Number),
                                    };
                            }
                            else
                            {
                                var dbAv = vocabularyStorage.AdmissionVolumeVoc.Items.FirstOrDefault(t => t.UID == group.Key.UID);
                                if (dbAv != null)
                                    av = new PackageDataAdmissionInfoItem()
                                    {
                                        UID = dbAv.UID,
                                        NumberBudgetO = (uint)dbAv.NumberBudgetO,
                                        NumberBudgetOZ = (uint)dbAv.NumberBudgetOZ,
                                        NumberBudgetZ = (uint)dbAv.NumberBudgetZ,
                                        NumberQuotaO = (uint)dbAv.NumberQuotaO,
                                        NumberQuotaOZ = (uint)dbAv.NumberQuotaOZ,
                                        NumberQuotaZ = (uint)dbAv.NumberQuotaZ,
                                        NumberTargetO = (uint)dbAv.NumberTargetO,
                                        NumberTargetOZ = (uint)dbAv.NumberTargetOZ,
                                        NumberTargetZ = (uint)dbAv.NumberTargetZ,
                                    };
                            }
                        }

                        if (av == null) continue;
                        List<string> errors = new List<string>();

                        if (!group.Key.IsPlan)
                        {
                            // взять DAV из БД
                            var allDav = vocabularyStorage.DistributedAdmissionVolumeVoc.Items.Where(t => t.AdmissionVolumeUID == group.Key.UID && !group.Any(g => !(g.UID == t.AdmissionVolumeUID && !g.IsPlan))).ToList();

                            if (av.NumberBudgetO < allDav.Sum(t => t.NumberBudgetO)) errors.Add("бюджет, очная");
                            if (av.NumberBudgetOZ < allDav.Sum(t => t.NumberBudgetOZ)) errors.Add("бюджет, очно-заочная");
                            if (av.NumberBudgetZ < allDav.Sum(t => t.NumberBudgetZ)) errors.Add("бюджет, заочная");

                            if (av.NumberQuotaO < allDav.Sum(t => t.NumberQuotaO)) errors.Add("квота, очная");
                            if (av.NumberQuotaOZ < allDav.Sum(t => t.NumberQuotaOZ)) errors.Add("квота, очно-заочная");
                            if (av.NumberQuotaZ < allDav.Sum(t => t.NumberQuotaZ)) errors.Add("квота, заочная");

                            if (av.NumberTargetO < allDav.Sum(t => t.NumberTargetO)) errors.Add("целевой прием, очная");
                            if (av.NumberTargetOZ < allDav.Sum(t => t.NumberTargetOZ)) errors.Add("целевой прием, очно-заочная");
                            if (av.NumberTargetZ < allDav.Sum(t => t.NumberTargetZ)) errors.Add("целевой прием, заочная");
                        }
                        else
                        {
                            // взять DPAV из БД
                            var allDpav = vocabularyStorage.DistributedPlanAdmissionVolumeVoc.Items.Where(t => t.PlanAdmissionVolumeUID == group.Key.UID && !group.Any(g => !(g.UID == t.PlanAdmissionVolumeUID && g.IsPlan))).ToList();

                            if (av.NumberBudgetO < allDpav.Where(t => t.EducationFormID == EDFormsConst.O && t.EducationSourceID == EDSourceConst.Budget).Sum(t => t.Number)) errors.Add("бюджет, очная");
                            if (av.NumberBudgetOZ < allDpav.Where(t => t.EducationFormID == EDFormsConst.OZ && t.EducationSourceID == EDSourceConst.Budget).Sum(t => t.Number)) errors.Add("бюджет, очно-заочная");
                            if (av.NumberBudgetZ < allDpav.Where(t => t.EducationFormID == EDFormsConst.Z && t.EducationSourceID == EDSourceConst.Budget).Sum(t => t.Number)) errors.Add("бюджет, заочная");

                            if (av.NumberQuotaO < allDpav.Where(t => t.EducationFormID == EDFormsConst.O && t.EducationSourceID == EDSourceConst.Quota).Sum(t => t.Number)) errors.Add("квота, очная");
                            if (av.NumberQuotaOZ < allDpav.Where(t => t.EducationFormID == EDFormsConst.OZ && t.EducationSourceID == EDSourceConst.Quota).Sum(t => t.Number)) errors.Add("квота, очно-заочная");
                            if (av.NumberQuotaZ < allDpav.Where(t => t.EducationFormID == EDFormsConst.Z && t.EducationSourceID == EDSourceConst.Quota).Sum(t => t.Number)) errors.Add("квота, заочная");

                            if (av.NumberTargetO < allDpav.Where(t => t.EducationFormID == EDFormsConst.O && t.EducationSourceID == EDSourceConst.Target).Sum(t => t.Number)) errors.Add("целевой прием, очная");
                            if (av.NumberTargetOZ < allDpav.Where(t => t.EducationFormID == EDFormsConst.OZ && t.EducationSourceID == EDSourceConst.Target).Sum(t => t.Number)) errors.Add("целевой прием, очно-заочная");
                            if (av.NumberTargetZ < allDpav.Where(t => t.EducationFormID == EDFormsConst.Z && t.EducationSourceID == EDSourceConst.Target).Sum(t => t.Number)) errors.Add("целевой прием, заочная");
                        }

                        if (errors.Count > 0)
                        {
                            conflictStorage.SetObjectIsBroken(av, ConflictMessages.DistributedAdmissionVolumeNumbersSumBiggerAV, av.UID, string.Join("; ", errors), av.IsPlan.ToString());
                            foreach (var item in group)
                            {
                                conflictStorage.SetObjectIsBroken(item, ConflictMessages.DistributedAdmissionVolumeNumbersSumBiggerAV, av.UID, string.Join("; ", errors), av.IsPlan.ToString());
                            }
                        }
                    }
                }
                #endregion

                if (packageData.AdmissionInfo.DistributedAdmissionVolume != null)
                {
                    // проверка, что в admissionInfoDto.DistributedAdmissionVolume нет повторов пар {AdmissionVolumeUID, LevelBudget}
                    foreach (var davGroup in packageData.AdmissionInfo.DistributedAdmissionVolume.GroupBy(t => t.Key()))
                        if (davGroup.Count() > 1)
                        {
                            foreach (var davItem in davGroup)
                            {
                                conflictStorage.SetObjectIsBroken(davItem, ConflictMessages.AdmissionVolumeUIDAndBudgetLevelMustBeUniqueInCollection, davItem.AdmissionVolumeUID, davItem.LevelBudget.ToString(), davItem.IsPlan.ToString());
                            }
                        }

                    foreach (var distributedAdmissionVolume in packageData.AdmissionInfo.DistributedAdmissionVolume)
                    {
                        // 0. Уже сломан?
                        if (distributedAdmissionVolume.IsBroken)
                            continue;

                        AdmissionVolumeVocDto dbAv = null;
                        PlanAdmissionVolumeVocDto dbPav = null;
                        if (distributedAdmissionVolume.IsPlan)
                        {
                            // IsPlan: загрузить из PlanAdmissionVolume, если есть такой
                            dbPav = vocabularyStorage.PlanAdmissionVolumeVoc.Items.Where(t => t.UID == distributedAdmissionVolume.AdmissionVolumeUID).FirstOrDefault();
                        }
                        else
                        {
                            dbAv = vocabularyStorage.AdmissionVolumeVoc.Items.Where(t => t.UID == distributedAdmissionVolume.AdmissionVolumeUID).FirstOrDefault();
                        }
                        var packageAv = (packageData.AdmissionInfo.AdmissionVolume != null) ?
                                packageData.AdmissionInfo.AdmissionVolume.Where(t => t.UID == distributedAdmissionVolume.AdmissionVolumeUID && t.IsPlan == distributedAdmissionVolume.IsPlan).FirstOrDefault() :
                                null;


                        // 1. в базе или пакете есть такой AV.UID и если только в пакете AV - то чтобы не сломан
                        if (((dbAv == null && !distributedAdmissionVolume.IsPlan) ||
                             (dbPav == null && distributedAdmissionVolume.IsPlan)
                            )
                            && (packageAv == null || packageAv.IsBroken))
                        {
                            conflictStorage.SetObjectIsBroken(distributedAdmissionVolume, ConflictMessages.AdmissionVolumeIsNotImportedForDistributedAdmissionVolume, distributedAdmissionVolume.AdmissionVolumeUID, distributedAdmissionVolume.IsPlan.ToString());
                            continue;
                        }
                        else
                        {
                            distributedAdmissionVolume.AdmissionVolumeGUID = packageAv != null ? packageAv.GUID :
                                (distributedAdmissionVolume.IsPlan ? dbPav.GUID : dbAv.GUID);
                            distributedAdmissionVolume.AdmissionVolumeID =
                                distributedAdmissionVolume.IsPlan ?
                                (dbPav != null ? dbPav.PlanAdmissionVolumeID : 0) :
                                (dbAv != null ? dbAv.AdmissionVolumeID : 0);
                        }

                        // 2. LevelBudget есть в справочнике
                        if (!VocabularyStatic.LevelBudgetVoc.Items.Any(t => t.IdLevelBudget == distributedAdmissionVolume.LevelBudget))
                        {
                            // Если бы помечать сломанным только этот объект
                            // ConflictStorage.AddNotImportedDto(dav, ConflictMessages.DictionaryItemAbsent, "LevelBudget=" + dav.LevelBudget);
                            // Но надо помечать сломанными все объекты c данным AdmissionVolumeUID
                            foreach (var item in packageData.AdmissionInfo.DistributedAdmissionVolume.Where(t => t.AdmissionVolumeUID == distributedAdmissionVolume.AdmissionVolumeUID && t.IsPlan == distributedAdmissionVolume.IsPlan))
                            {
                                conflictStorage.SetObjectIsBroken(item, ConflictMessages.DictionaryItemAbsent, "LevelBudget=" + distributedAdmissionVolume.LevelBudget);
                            }
                            continue;
                        }

                        var dbDAV = vocabularyStorage.DistributedAdmissionVolumeVoc.Items.FirstOrDefault(t => t.AdmissionVolumeUID == distributedAdmissionVolume.AdmissionVolumeUID && t.IdLevelBudget == distributedAdmissionVolume.LevelBudget.To(0) && !distributedAdmissionVolume.IsPlan);
                        if (dbDAV != null)
                            distributedAdmissionVolume.ID = dbDAV.ID;

                    }

                    // Проверка того, что сумма распределенных К-Ц-П (DAV.Number[Source][Form]) не превышает исходных К-Ц-П (AV.Number[Source][Form]) по пакету или базе
                    foreach (var group in packageData.AdmissionInfo.DistributedAdmissionVolume.GroupBy(t => new { t.AdmissionVolumeUID, t.IsPlan }))
                    {
                        // Это распределенный объем из пакета
                        // FIS-1737(3) Берем сначала его, а потом уже смотрим в БД
                        List<DistributedAdmissionVolumeVocDto> allDav = new List<DistributedAdmissionVolumeVocDto>();
                        allDav.AddRange(
                        group.Select(t => new DistributedAdmissionVolumeVocDto()
                        {
                            //UID = t.UID,
                            AdmissionVolumeUID = t.Key(),
                            IdLevelBudget = t.LevelBudget.To(0),
                            NumberBudgetO = t.NumberBudgetO.To(0),
                            NumberBudgetOZ = t.NumberBudgetOZ.To(0),
                            NumberBudgetZ = t.NumberBudgetZ.To(0),
                            NumberQuotaO = t.NumberQuotaO.To(0),
                            NumberQuotaOZ = t.NumberQuotaOZ.To(0),
                            NumberQuotaZ = t.NumberQuotaZ.To(0),
                            NumberTargetO = t.NumberTargetO.To(0),
                            NumberTargetOZ = t.NumberTargetOZ.To(0),
                            NumberTargetZ = t.NumberTargetZ.To(0),
                        }
                        ));
                        // Если в пакете нет DAV по какому-либо уровню бюджета, тогда уже берем его из БД
                        for (int i = 1; i <= 3; i++)
                        {
                            if (!allDav.Any(t => t.IdLevelBudget == i))
                            {
                                //FIS-1737(2) Тут почему то было условие на сумму по каждому уровню бюджета
                                if (!group.Key.IsPlan)
                                {
                                    allDav.AddRange(
                                        vocabularyStorage.DistributedAdmissionVolumeVoc.Items.Where(t => t.AdmissionVolumeUID == group.Key.AdmissionVolumeUID
                                        && !group.Any(g => !(g.AdmissionVolumeUID == t.AdmissionVolumeUID))
                                        && t.IdLevelBudget == i
                                    ));
                                }
                                else
                                {
                                    var dpavItems = vocabularyStorage.DistributedPlanAdmissionVolumeVoc.Items.Where(t => t.PlanAdmissionVolumeUID == group.Key.AdmissionVolumeUID
                                        && !group.Any(g => !(g.AdmissionVolumeUID == t.PlanAdmissionVolumeUID))
                                        && t.IdLevelBudget == i
                                    );
                                    allDav.AddRange(
                                        dpavItems.Select(t => new DistributedAdmissionVolumeVocDto()
                                        {
                                            //UID = t.UID,
                                            AdmissionVolumeUID = t.PlanAdmissionVolumeUID,
                                            IdLevelBudget = t.IdLevelBudget,
                                            NumberBudgetO = t.EducationFormID == EDFormsConst.O && t.EducationSourceID == EDSourceConst.Budget ? t.Number : 0,
                                            NumberBudgetOZ = t.EducationFormID == EDFormsConst.OZ && t.EducationSourceID == EDSourceConst.Budget ? t.Number : 0,
                                            NumberBudgetZ = t.EducationFormID == EDFormsConst.Z && t.EducationSourceID == EDSourceConst.Budget ? t.Number : 0,
                                            NumberQuotaO = t.EducationFormID == EDFormsConst.O && t.EducationSourceID == EDSourceConst.Quota ? t.Number : 0,
                                            NumberQuotaOZ = t.EducationFormID == EDFormsConst.OZ && t.EducationSourceID == EDSourceConst.Quota ? t.Number : 0,
                                            NumberQuotaZ = t.EducationFormID == EDFormsConst.Z && t.EducationSourceID == EDSourceConst.Quota ? t.Number : 0,
                                            NumberTargetO = t.EducationFormID == EDFormsConst.O && t.EducationSourceID == EDSourceConst.Target ? t.Number : 0,
                                            NumberTargetOZ = t.EducationFormID == EDFormsConst.OZ && t.EducationSourceID == EDSourceConst.Target ? t.Number : 0,
                                            NumberTargetZ = t.EducationFormID == EDFormsConst.Z && t.EducationSourceID == EDSourceConst.Target ? t.Number : 0,
                                        }
                                    ));

                                }
                            }
                        }
                        PackageDataAdmissionInfoItem av = null;
                        // взять соотв AV из пакета или БД
                        if (packageData.AdmissionInfo.AdmissionVolume != null)
                        {
                            av = packageData.AdmissionInfo.AdmissionVolume.Where(t => t.UID == group.Key.AdmissionVolumeUID).FirstOrDefault();
                        }
                        if (av == null || av.IsBroken)
                        {
                            if (!group.Key.IsPlan)
                            {
                                var dbAv = vocabularyStorage.AdmissionVolumeVoc.Items.FirstOrDefault(t => t.UID == group.Key.AdmissionVolumeUID);
                                if (dbAv != null)
                                    av = new PackageDataAdmissionInfoItem()
                                    {
                                        UID = dbAv.UID,
                                        NumberBudgetO = (uint)dbAv.NumberBudgetO,
                                        NumberBudgetOZ = (uint)dbAv.NumberBudgetOZ,
                                        NumberBudgetZ = (uint)dbAv.NumberBudgetZ,
                                        NumberQuotaO = (uint)dbAv.NumberQuotaO,
                                        NumberQuotaOZ = (uint)dbAv.NumberQuotaOZ,
                                        NumberQuotaZ = (uint)dbAv.NumberQuotaZ,
                                        NumberTargetO = (uint)dbAv.NumberTargetO,
                                        NumberTargetOZ = (uint)dbAv.NumberTargetOZ,
                                        NumberTargetZ = (uint)dbAv.NumberTargetZ,
                                    };
                            }
                            else
                            {
                                //FIS-1815
                                //var dbPav = vocabularyStorage.PlanAdmissionVolumeVoc.Items.FirstOrDefault(t => t.UID == group.Key.AdmissionVolumeUID);

                                //у нас их будет несколько, в отличии от просто объема приема, 
                                //из -за новой структуры базы в части планового приема
                                var dbPavItems = vocabularyStorage.PlanAdmissionVolumeVoc.Items.Where(t => t.UID == group.Key.AdmissionVolumeUID);

                                if (dbPavItems != null && dbPavItems.Any())
                                { 
                                    av = new PackageDataAdmissionInfoItem();
                                    av.UID = dbPavItems.FirstOrDefault().UID;

                                    av.NumberBudgetO = (uint)dbPavItems.Sum(dbPav => dbPav.EducationFormID == EDFormsConst.O && dbPav.EducationSourceID == EDSourceConst.Budget ? dbPav.Number : 0);
                                    av.NumberBudgetOZ = (uint)dbPavItems.Sum(dbPav => dbPav.EducationFormID == EDFormsConst.OZ && dbPav.EducationSourceID == EDSourceConst.Budget ? dbPav.Number : 0);
                                    av.NumberBudgetZ = (uint)dbPavItems.Sum(dbPav => dbPav.EducationFormID == EDFormsConst.Z && dbPav.EducationSourceID == EDSourceConst.Budget ? dbPav.Number : 0);

                                    av.NumberQuotaO = (uint)dbPavItems.Sum(dbPav => dbPav.EducationFormID == EDFormsConst.O && dbPav.EducationSourceID == EDSourceConst.Quota ? dbPav.Number : 0);
                                    av.NumberQuotaOZ = (uint)dbPavItems.Sum(dbPav => dbPav.EducationFormID == EDFormsConst.OZ && dbPav.EducationSourceID == EDSourceConst.Quota ? dbPav.Number : 0);
                                    av.NumberQuotaZ = (uint)dbPavItems.Sum(dbPav => dbPav.EducationFormID == EDFormsConst.Z && dbPav.EducationSourceID == EDSourceConst.Quota ? dbPav.Number : 0);

                                    av.NumberTargetO = (uint)dbPavItems.Sum(dbPav => dbPav.EducationFormID == EDFormsConst.O && dbPav.EducationSourceID == EDSourceConst.Target ? dbPav.Number : 0);
                                    av.NumberTargetOZ = (uint)dbPavItems.Sum(dbPav => dbPav.EducationFormID == EDFormsConst.OZ && dbPav.EducationSourceID == EDSourceConst.Target ? dbPav.Number : 0);
                                    av.NumberTargetZ = (uint)dbPavItems.Sum(dbPav => dbPav.EducationFormID == EDFormsConst.Z && dbPav.EducationSourceID == EDSourceConst.Target ? dbPav.Number : 0);
                                    
                                    //{
                                    //    UID = dbPav.UID,
                                    //    NumberBudgetO = dbPav.EducationFormID == EDFormsConst.O && dbPav.EducationSourceID == EDSourceConst.Budget ? (uint)dbPav.Number : 0,
                                    //    NumberBudgetOZ = dbPav.EducationFormID == EDFormsConst.OZ && dbPav.EducationSourceID == EDSourceConst.Budget ? (uint)dbPav.Number : 0,
                                    //    NumberBudgetZ = dbPav.EducationFormID == EDFormsConst.Z && dbPav.EducationSourceID == EDSourceConst.Budget ? (uint)dbPav.Number : 0,

                                    //    NumberQuotaO = dbPav.EducationFormID == EDFormsConst.O && dbPav.EducationSourceID == EDSourceConst.Quota ? (uint)dbPav.Number : 0,
                                    //    NumberQuotaOZ = dbPav.EducationFormID == EDFormsConst.OZ && dbPav.EducationSourceID == EDSourceConst.Quota ? (uint)dbPav.Number : 0,
                                    //    NumberQuotaZ = dbPav.EducationFormID == EDFormsConst.Z && dbPav.EducationSourceID == EDSourceConst.Quota ? (uint)dbPav.Number : 0,

                                    //    NumberTargetO = dbPav.EducationFormID == EDFormsConst.O && dbPav.EducationSourceID == EDSourceConst.Target ? (uint)dbPav.Number : 0,
                                    //    NumberTargetOZ = dbPav.EducationFormID == EDFormsConst.OZ && dbPav.EducationSourceID == EDSourceConst.Target ? (uint)dbPav.Number : 0,
                                    //    NumberTargetZ = dbPav.EducationFormID == EDFormsConst.Z && dbPav.EducationSourceID == EDSourceConst.Target ? (uint)dbPav.Number : 0
                                    //};
                                    //}
                                }
                            }
                        }

                        if (av == null)
                            continue;

                        List<string> errors = new List<string>();

                        if (av.NumberBudgetO < allDav.Sum(t => t.NumberBudgetO)) errors.Add("бюджет, очная");
                        if (av.NumberBudgetOZ < allDav.Sum(t => t.NumberBudgetOZ)) errors.Add("бюджет, очно-заочная");
                        if (av.NumberBudgetZ < allDav.Sum(t => t.NumberBudgetZ)) errors.Add("бюджет, заочная");

                        if (av.NumberQuotaO < allDav.Sum(t => t.NumberQuotaO)) errors.Add("квота, очная");
                        if (av.NumberQuotaOZ < allDav.Sum(t => t.NumberQuotaOZ)) errors.Add("квота, очно-заочная");
                        if (av.NumberQuotaZ < allDav.Sum(t => t.NumberQuotaZ)) errors.Add("квота, заочная");

                        if (av.NumberTargetO < allDav.Sum(t => t.NumberTargetO)) errors.Add("целевой прием, очная");
                        if (av.NumberTargetOZ < allDav.Sum(t => t.NumberTargetOZ)) errors.Add("целевой прием, очно-заочная");
                        if (av.NumberTargetZ < allDav.Sum(t => t.NumberTargetZ)) errors.Add("целевой прием, заочная");

                        if (errors.Count > 0)
                        {
                            conflictStorage.SetObjectIsBroken(av, ConflictMessages.DistributedAdmissionVolumeNumbersSumBiggerAV, av.UID, string.Join("; ", errors), av.IsPlan.ToString());
                            foreach (var item in group)
                            {
                                conflictStorage.SetObjectIsBroken(item, ConflictMessages.DistributedAdmissionVolumeNumbersSumBiggerAV, av.UID, string.Join("; ", errors), av.IsPlan.ToString());
                            }
                        }
                    }
                }
                #endregion
                DebugMessage(sw, "Validate - DistributedAdmissionVolume");

                if (packageData.AdmissionInfo.CompetitiveGroups != null)
                {

                    // Проверка уникальности CompetitiveGroup.UID в рамках пакета
                    CheckUniqueUID(packageData.AdmissionInfo.CompetitiveGroups, null);

                    // Имя конкурсной группы должно быть уникальной в контексте кампании - для проверки
                    var cgNames = new HashSet<Tuple<string, int>>();

                    var programUIDs = new HashSet<Tuple<string, int>>(); // Для проверки уникальности UID-ов CompetitiveGroupProgram в рамках кампании

                    foreach (var competitiveGroup in packageData.AdmissionInfo.CompetitiveGroups)
                    {
                        //if (vocabularyStorage.CompetitiveGroupVoc.Items.Any(t => t.UID == competitiveGroup.UID))
                        //{
                        //    conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.CompetitiveGroupHasSameUID, competitiveGroup.UID);
                        //}

                        // Для проверки внешних зависимостей и, соответственно, возможности обновления "не К-Ц-П (чисел)"
                        //bool cgHasDependency = false;
                        //List<int> cgiHasDependency = new List<int>();

                        // Проверка уникальности CompetitiveGroupItem.UID в рамках competitiveGroup
                        CheckUniqueUID(competitiveGroup.EntranceTestItems, competitiveGroup);
                        // Проверка уникальности CompetitiveGroupItem.UID в рамках competitiveGroup
                        CheckUniqueUID(competitiveGroup.CommonBenefit, competitiveGroup);
                        //CheckUniqueUID(competitiveGroup.EduPrograms, competitiveGroup);

                        var campaign = vocabularyStorage.CampaignVoc.Items.Where(t => t.UID == competitiveGroup.CampaignUID).FirstOrDefault();
                        if (campaign == null) // CampaignUID д.б. правильным
                        {
                            conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.CompetitiveGroupContainsNotAllowedCampaign);
                            continue;
                        }

                        if (campaign != null)
                            competitiveGroup.CampaignID = campaign.CampaignID; // Для заполнения в БД

                        // Наличие значений в справочниках EducationLevel, Source, Form, Direction
                        if (!VocabularyStatic.AdmissionItemTypeVoc.GetEducationLevel().Any(t => t.ID == competitiveGroup.EducationLevelID.To(0)))
                            conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.DictionaryItemAbsent, "CompetitiveGroup.EducationLevelID");
                        if (!VocabularyStatic.AdmissionItemTypeVoc.GetEducationForm().Any(t => t.ID == competitiveGroup.EducationFormID.To(0)))
                            conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.DictionaryItemAbsent, "CompetitiveGroup.EducationFormID");
                        if (!VocabularyStatic.AdmissionItemTypeVoc.GetFinanceSource().Any(t => t.ID == competitiveGroup.EducationSourceID.To(0)))
                            conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.DictionaryItemAbsent, "CompetitiveGroup.EducationSourceID");  

                        if (!VocabularyStatic.DirectionVoc.Items.Any(t => t.ID == competitiveGroup.DirectionID.To(0)) && competitiveGroup.DirectionID != 0)
                            conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.DictionaryItemAbsent, "CompetitiveGroup.DirectionID");
                        
                        if (!VocabularyStatic.DirectionVoc.Items.Any(t => t.ParentDirectionID == competitiveGroup.ParentDirectionID.To(0)) 
                            && competitiveGroup.ParentDirectionID != 0)
                            conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.DictionaryItemAbsent, "CompetitiveGroup.ParentDirectionID");

                        // Проверяем что для CampaignTypeID из пакета/БД EducationLevelID есть в справочнике EduLevelsToCampaignTypes
                        if (!VocabularyStatic.EduLevelsToCampaignTypesVoc.Items.Any(t => t.CampaignTypeID == campaign.CampaignTypeID && t.AdmissionItemTypeID == competitiveGroup.EducationLevelID.To(0)))
                            conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.CompetitiveGroupContainsNotAvailableEducationLevel);

                        var formFlag = competitiveGroup.EducationFormID.To(0) == EDFormsConst.O ? 1 :
                                        competitiveGroup.EducationFormID.To(0) == EDFormsConst.OZ ? 2 : 4;
                        if ((campaign.EducationFormFlag & formFlag) == 0)
                            conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.CompetitiveGroupContainsNotAvailableEducationForm);

                        // В БД не должно быть CG с таким же Name, но другим UID
                        // возможно, проверка будет не нужна
                        // #FIS-1739 Отключена в рамках задачи 
                        //var conflictedName = vocabularyStorage.CompetitiveGroupVoc.Items.Where(t => t.Name.Equals(competitiveGroup.Name, StringComparison.InvariantCultureIgnoreCase)
                        //                    && t.UID != competitiveGroup.UID).FirstOrDefault();
                        //if (conflictedName != null)
                        //{
                        //    conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.CompetitiveGroupNameEsistsInDbWithOtherUID, conflictedName.Name, conflictedName.UID);
                        //}

                        // Для года > 2016 не может быть заполнено поле IsFromKrym
                        if (competitiveGroup.IsForKrym && campaign.YearStart != 2016)
                        {
                            conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.CompetitiveGroupIsFromKrymWrongYear);
                        }

                        // FIS - 1790 (22.12.2017) - проверки на уровень бюджета в конкурсе
                        // Сначала проверим что значение в справочнике
                        //if (!VocabularyStatic.LevelBudgetVoc.Items.Any(t => t.IdLevelBudget == competitiveGroup.LevelBudget))
                        //{
                        //    conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.DictionaryItemAbsent, "LevelBudget");
                        //}

                        // Что поле передано, если конкурс не платный и кампания не иностранная
                        if (competitiveGroup.EducationSourceID != EDSourceConst.Paid
                            && (campaign != null && campaign.CampaignTypeID != GVUZ.DAL.Dapper.ViewModel.Dictionary.CampaignTypesView.Foreigners)
                            )
                        {
                            if (!competitiveGroup.LevelBudgetSpecified || !VocabularyStatic.LevelBudgetVoc.Items.Any(t => t.IdLevelBudget == competitiveGroup.LevelBudget))
                            {
                                conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.DictionaryItemAbsent, "competitiveGroup.LevelBudget");
                            }
                        }
                        else
                        {
                            competitiveGroup.LevelBudgetSpecified = false;
                            competitiveGroup.LevelBudget = 0;
                        }

                        // 1) Если передано DirectionID и EduProgram, считаем что прием идет по EduProgram
                        // Проверка - что для данных CampaignID, DirectionID, EducationLevelID, EducationSourceID, EducationFormID, IsFromKrym, IsAdditional в БД и пакете нет такой же ProgramName Если есть ошибка №
                        // 2) Если передано только DirectionID -прием целиком по направлению подготовки
                        // Проверка - что для данной ПК в БД и пакете, для такой же DirectionID нет таких сочетаний EducationLevelID, EducationSourceID, EducationFormID, IsFromKrym, IsAdditional. Если есть ошибка №
                        // И все это, когда НЕ конкурс по платной основе


                        //aii_logger.Debug("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
                        //aii_logger.Debug(string.Format("Данные из пакета, GUID {0}, CampaignUID {1}," +
                        //     "DirectionID {2}, EducationFormID {3}, EducationLevelID {4}, EducationSourceID {5}, ParentDirectionID {6}," +
                        //     "IsForKrym {7}, IsAdditional {8}, IdLevelBudget {9} ",
                        //     competitiveGroup.GUID, competitiveGroup.CampaignUID,
                        //     competitiveGroup.DirectionID, competitiveGroup.EducationFormID, competitiveGroup.EducationLevelID, competitiveGroup.EducationSourceID,
                        //     competitiveGroup.ParentDirectionID, competitiveGroup.IsForKrym, competitiveGroup.IsAdditional, competitiveGroup.LevelBudget));
                        //// данные словарей
                        //aii_logger.Debug(string.Format("Данные из vocabularyStorage: UID {0}, CampaignUID {1}," +
                        //     "DirectionID {2}, EducationFormID {3}, EducationLevelID {4}, EducationSourceID {5}," +
                        //     "IsForKrym {6}, IsAdditional {7}, IdLevelBudget {8}",
                        //     packageData.AdmissionInfo.CompetitiveGroups.Where(x => x.GUID == competitiveGroup.GUID).Select(y => y.GUID).FirstOrDefault(),
                        //     vocabularyStorage.CompetitiveGroupVoc.Items.Where(x => x.CampaignUID == competitiveGroup.CampaignUID)
                        //     .Select(y => y.CampaignUID).FirstOrDefault(),
                        //     vocabularyStorage.CompetitiveGroupVoc.Items.Where(x => x.DirectionID == competitiveGroup.DirectionID)
                        //     .Select(y => y.DirectionID).FirstOrDefault(),
                        //     vocabularyStorage.CompetitiveGroupVoc.Items.Where(x => x.EducationFormId == competitiveGroup.EducationFormID)
                        //     .Select(y => y.EducationFormId).FirstOrDefault(),
                        //     vocabularyStorage.CompetitiveGroupVoc.Items.Where(x => x.EducationLevelID == competitiveGroup.EducationLevelID)
                        //     .Select(y => y.EducationLevelID).FirstOrDefault(),
                        //     vocabularyStorage.CompetitiveGroupVoc.Items.Where(x => x.EducationSourceId == competitiveGroup.EducationSourceID)
                        //     .Select(y => y.EducationSourceId).FirstOrDefault(),
                        //     vocabularyStorage.CompetitiveGroupVoc.Items.Where(x => x.IsFromKrym == competitiveGroup.IsForKrym)
                        //     .Select(y => y.IsFromKrym).FirstOrDefault(),
                        //     vocabularyStorage.CompetitiveGroupVoc.Items.Where(x => x.IsAdditional == competitiveGroup.IsAdditional)
                        //     .Select(y => y.IsAdditional).FirstOrDefault(),
                        //     vocabularyStorage.CompetitiveGroupVoc.Items.Where(x => x.IdLevelBudget == competitiveGroup.LevelBudget)
                        //     .Select(y => y.IdLevelBudget).FirstOrDefault()
                        //     ));
                        aii_logger.Debug("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");

                        if (competitiveGroup.EducationSourceID != EDSourceConst.Paid)
                        {
                            if (packageData.AdmissionInfo.CompetitiveGroups.Any
                                    (cg =>
                                        cg.GUID != competitiveGroup.GUID &&
                                        cg.CampaignUID == competitiveGroup.CampaignUID &&
                                        cg.DirectionID == competitiveGroup.DirectionID &&
                                        cg.EducationFormID == competitiveGroup.EducationFormID &&
                                        cg.EducationLevelID == competitiveGroup.EducationLevelID &&
                                        cg.EducationSourceID == competitiveGroup.EducationSourceID &&
                                        cg.IsForKrym == competitiveGroup.IsForKrym &&
                                        cg.IsAdditional == competitiveGroup.IsAdditional &&
                                        cg.LevelBudget == competitiveGroup.LevelBudget &&  //FIS-1790 теперь еще уровень бюджета
                                        (
                                            //FIS - 1733 (13.06.2017)
                                            //условие фиксит баг когда мы передаем одинаковые КГ, но одну с ОП а вторую без ОП
                                            (competitiveGroup.EduPrograms == null && cg.EduPrograms == null)
                                            ||
                                            (
                                                cg.EduPrograms != null && competitiveGroup.EduPrograms != null &&
                                                cg.EduPrograms.All(cgp => competitiveGroup.EduPrograms.Any(p => p.UID == cgp.UID)) &&
                                                competitiveGroup.EduPrograms.All(cgp => cg.EduPrograms.Any(p => p.UID == cgp.UID))
                                            //cg.EduPrograms.Intersect(competitiveGroup.EduPrograms).Count() == cg.EduPrograms.Count()
                                            )
                                        ) &&
                                        (
                                            competitiveGroup.TargetOrganizations == null ||
                                            (
                                                cg.TargetOrganizations != null &&
                                                cg.TargetOrganizations.All(cgt => competitiveGroup.TargetOrganizations.Any(t => t.UID == cgt.UID)) &&
                                                competitiveGroup.TargetOrganizations.All(cgt => cg.TargetOrganizations.Any(t => t.UID == cgt.UID))
                                            )
                                        )
                                    )
                                )
                                conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.CompetitiveGroupWithProgramIsNotUniqueInPackage);
                            // данные пакета
                            aii_logger.Debug("=============================================================================================================");
                            aii_logger.Debug(string.Format("Данные из пакета, UID {0}, CampaignUID {1}," +
                                 "DirectionID {2}, EducationFormID {3}, EducationLevelID {4}, EducationSourceID {5}, ParentDirectionID {6}," +
                                 "IsForKrym {7}, IsAdditional {8}, IdLevelBudget {9}, EduPrograms {10}, TargetOrganizations {11} ",
                                 competitiveGroup.UID, competitiveGroup.CampaignUID,
                                 competitiveGroup.DirectionID, competitiveGroup.EducationFormID, competitiveGroup.EducationLevelID, competitiveGroup.EducationSourceID,
                                 competitiveGroup.ParentDirectionID, competitiveGroup.IsForKrym, competitiveGroup.IsAdditional, competitiveGroup.LevelBudget,
                                 (competitiveGroup.EduPrograms == null) ? "null" : competitiveGroup.EduPrograms[0].GUID.ToString(),
                                 (competitiveGroup.TargetOrganizations == null) ? "null" : competitiveGroup.TargetOrganizations[0].GUID.ToString()));
                            // данные словарей
                            aii_logger.Debug(string.Format("Данные из vocabularyStorage: UID {0}, CampaignUID {1}," +
                                 "DirectionID {2}, EducationFormID {3}, EducationLevelID {4}, EducationSourceID {5}, ParentDirectionID {6}, " +
                                 "IsForKrym {7}, IsAdditional {8}, IdLevelBudget {9}, EduPrograms {10}, TargetOrganizations {11} ",
                                 vocabularyStorage.CompetitiveGroupVoc.Items.Where(x => x.UID == competitiveGroup.UID).Select(y => y.UID).FirstOrDefault(),
                                 vocabularyStorage.CompetitiveGroupVoc.Items.Where(x => x.CampaignUID == competitiveGroup.CampaignUID)
                                 .Select(y => y.CampaignUID).FirstOrDefault(),
                                 vocabularyStorage.CompetitiveGroupVoc.Items.Where(x => x.DirectionID == competitiveGroup.DirectionID)
                                 .Select(y => y.DirectionID).FirstOrDefault(),
                                 vocabularyStorage.CompetitiveGroupVoc.Items.Where(x => x.EducationFormId == competitiveGroup.EducationFormID)
                                 .Select(y => y.EducationFormId).FirstOrDefault(),
                                 vocabularyStorage.CompetitiveGroupVoc.Items.Where(x => x.EducationLevelID == competitiveGroup.EducationLevelID)
                                 .Select(y => y.EducationLevelID).FirstOrDefault(),
                                 vocabularyStorage.CompetitiveGroupVoc.Items.Where(x => x.EducationSourceId == competitiveGroup.EducationSourceID)
                                 .Select(y => y.EducationSourceId).FirstOrDefault(),
                                 vocabularyStorage.CompetitiveGroupVoc.Items.Where(x => x.ParentDirectionID == competitiveGroup.ParentDirectionID)
                                 .Select(y => y.ParentDirectionID).FirstOrDefault(),
                                 vocabularyStorage.CompetitiveGroupVoc.Items.Where(x => x.IsFromKrym == competitiveGroup.IsForKrym)
                                 .Select(y => y.IsFromKrym).FirstOrDefault(),
                                 vocabularyStorage.CompetitiveGroupVoc.Items.Where(x => x.IsAdditional == competitiveGroup.IsAdditional)
                                 .Select(y => y.IsAdditional).FirstOrDefault(),
                                 vocabularyStorage.CompetitiveGroupVoc.Items.Where(x => x.IdLevelBudget == competitiveGroup.LevelBudget)
                                 .Select(y => y.IdLevelBudget).FirstOrDefault(),
                                 vocabularyStorage.CompetitiveGroupProgramVoc.Items
                                                    .Where(cgp0 => cgp0.CompetitiveGroupID == competitiveGroup.ID)
                                                    .Where(cgp => competitiveGroup.EduPrograms.Any(p => p.UID == cgp.UID))
                                                    .Select(x => x.ID).FirstOrDefault(),
                                  vocabularyStorage.CompetitiveGroupTargetItemVoc.Items
                                                    .Where(cgp0 => cgp0.CompetitiveGroupID == competitiveGroup.ID)
                                                    .Where(cgp => competitiveGroup.TargetOrganizations.Any(p => p.UID == cgp.CompetitiveGroupTargetUID))
                                                    .Select(x => x.ID).FirstOrDefault()
                                 ));
                            aii_logger.Debug("=============================================================================================================");
                            if (vocabularyStorage.CompetitiveGroupVoc.Items.Any(cg =>
                                        cg.UID != competitiveGroup.UID &&
                                        cg.CampaignUID == competitiveGroup.CampaignUID &&
                                        cg.DirectionID == competitiveGroup.DirectionID &&
                                        cg.EducationFormId == competitiveGroup.EducationFormID &&
                                        cg.EducationLevelID == competitiveGroup.EducationLevelID &&
                                        cg.EducationSourceId == competitiveGroup.EducationSourceID &&
                                        cg.IsFromKrym == competitiveGroup.IsForKrym &&
                                        cg.IsAdditional == competitiveGroup.IsAdditional &&
                                        cg.IdLevelBudget == competitiveGroup.LevelBudget &&  //FIS-1790 теперь еще уровень бюджета
                                        competitiveGroup.ParentDirectionID == 0 &&
                                        (
                                            (competitiveGroup.EduPrograms == null
                                            ||
                                            //FIS - 1733 (13.06.2017)
                                            //competitiveGroup.EduPrograms == null && cg.EduPrograms == null
                                            //условие фиксит баг когда мы передаем одинаковые КГ, но одну с ОП а вторую без ОП
                                            //vocabularyStorage.CompetitiveGroupProgramVoc.Items.Where(cgp0 => cgp0.CompetitiveGroupID == cg.CompetitiveGroupID) == null)
                                            //||
                                            (
                                                //competitiveGroup.EduPrograms != null ||
                                                //(
                                                vocabularyStorage.CompetitiveGroupProgramVoc.Items.Where(cgp0 => cgp0.CompetitiveGroupID == cg.CompetitiveGroupID)
                                                    .All(cgp => competitiveGroup.EduPrograms.Any(p => p.UID == cgp.UID)) &&
                                                competitiveGroup.EduPrograms.All(p => vocabularyStorage.CompetitiveGroupProgramVoc.Items.Any(cgp => cgp.CompetitiveGroupID == cg.CompetitiveGroupID && p.UID == cgp.UID))
                                                )
                                            //(vocabularyStorage.InstitutionProgramVoc.Items.Where(cgp0 => cgp0.CompetitiveGroupID == cg.CompetitiveGroupID) != null) &&
                                            //vocabularyStorage.InstitutionProgramVoc.Items.Where(cgp0 => cgp0.CompetitiveGroupID == cg.CompetitiveGroupID)
                                            //    .All(cgp => competitiveGroup.EduPrograms.Any(p => p.UID == cgp.Name)) &&
                                            //competitiveGroup.EduPrograms.All(p => vocabularyStorage.InstitutionProgramVoc.Items.Any(cgp => cgp.CompetitiveGroupID == cg.CompetitiveGroupID && p.UID == cgp.UID))

                                            )
                                        ) &&
                                        (
                                            competitiveGroup.TargetOrganizations == null ||
                                            (
                                                vocabularyStorage.CompetitiveGroupTargetItemVoc.Items.Where(cgp0 => cgp0.CompetitiveGroupID == cg.CompetitiveGroupID)
                                                    .All(cgp => competitiveGroup.TargetOrganizations.Any(p => p.UID == cgp.CompetitiveGroupTargetUID)) &&
                                                competitiveGroup.TargetOrganizations.All(p => vocabularyStorage.CompetitiveGroupTargetItemVoc.Items.Any(cgp => cgp.CompetitiveGroupID == cg.CompetitiveGroupID && p.UID == cgp.CompetitiveGroupTargetUID))
                                            )
                                        )
                                    )
                                )
                            {
                                int error_code = ConflictMessages.CompetitiveGroupWithProgramIsNotUniqueInDb;
                                conflictStorage.SetObjectIsBroken(competitiveGroup, error_code);
                                aii_logger.DebugFormat("Пакет: {0} конфликт UID, код {1}", packageData.ImportPackageId, error_code);
                            }

                        }

                        CompetitiveGroupDeleteManager cgDeleteManager = null;
                        var dbCompetitiveGroup = vocabularyStorage.CompetitiveGroupVoc.Items.Where(t => t.UID == competitiveGroup.UID).FirstOrDefault();
                        if (dbCompetitiveGroup != null)
                        {
                            competitiveGroup.ID = dbCompetitiveGroup.ID;

                            // проверка, что можно делать update существующей записи - выполняется если это Update действительно требуется
                            cgDeleteManager = new CompetitiveGroupDeleteManager(dbCompetitiveGroup, vocabularyStorage, dependencyVoc);

                            // Если изменения не только в наименовании и уровне бюджета...
                            bool dont_check = false;
                            dont_check = (dbCompetitiveGroup.Name != competitiveGroup.Name || dbCompetitiveGroup.IdLevelBudget != competitiveGroup.LevelBudget) &&
                                         (dbCompetitiveGroup.DirectionID == competitiveGroup.DirectionID &&
                                          dbCompetitiveGroup.EducationLevelID == competitiveGroup.EducationLevelID &&
                                          dbCompetitiveGroup.EducationFormId == competitiveGroup.EducationFormID &&
                                          dbCompetitiveGroup.EducationSourceId == competitiveGroup.EducationSourceID &&
                                          dbCompetitiveGroup.CampaignID == competitiveGroup.CampaignID &&
                                          dbCompetitiveGroup.IsAdditional == competitiveGroup.IsAdditional);

                            if (dbCompetitiveGroup.Name != competitiveGroup.Name ||
                                dbCompetitiveGroup.DirectionID != competitiveGroup.DirectionID ||
                                dbCompetitiveGroup.EducationLevelID != competitiveGroup.EducationLevelID ||
                                dbCompetitiveGroup.EducationFormId != competitiveGroup.EducationFormID ||
                                dbCompetitiveGroup.EducationSourceId != competitiveGroup.EducationSourceID ||
                                dbCompetitiveGroup.CampaignID != competitiveGroup.CampaignID ||
                                dbCompetitiveGroup.IsAdditional != competitiveGroup.IsAdditional ||
                                dbCompetitiveGroup.IsFromKrym != competitiveGroup.IsForKrym ||
                                dbCompetitiveGroup.IdLevelBudget != competitiveGroup.LevelBudget ||
                                dbCompetitiveGroup.ParentDirectionID != competitiveGroup.ParentDirectionID
                                )
                            {
                                aii_logger.Debug("+-------------------+-----+------+-----+-----+-----+-----+-----+-----+-----+-----+");
                                aii_logger.Debug("| Конкурсная группа | Имя | Напр | Лвл | Фрм | Ист | Кмп | Доп | Крм | Бдж | УГр |");
                                aii_logger.Debug("+-------------------+-----+------+-----+-----+-----+-----+-----+-----+-----+-----+");
                                aii_logger.DebugFormat("| {0,17:G} | {1,3} | {2,4} | {3,3} | {4,3} | {5,3} | {6,3} | {7,3} | {8,3} | {9,3} | {10,3} |",
                                                        competitiveGroup.ID,
                                                        (dbCompetitiveGroup.Name != competitiveGroup.Name) ? "да" : "нет",
                                                        (dbCompetitiveGroup.DirectionID != competitiveGroup.DirectionID) ? "да" : "нет",
                                                        (dbCompetitiveGroup.EducationLevelID != competitiveGroup.EducationLevelID) ? "да" : "нет",
                                                        (dbCompetitiveGroup.EducationFormId != competitiveGroup.EducationFormID) ? "да" : "нет",
                                                        (dbCompetitiveGroup.EducationSourceId != competitiveGroup.EducationSourceID) ? "да" : "нет",
                                                        (dbCompetitiveGroup.CampaignID != competitiveGroup.CampaignID) ? "да" : "нет",
                                                        (dbCompetitiveGroup.IsAdditional != competitiveGroup.IsAdditional) ? "да" : "нет",
                                                        (dbCompetitiveGroup.IsFromKrym != competitiveGroup.IsForKrym) ? "да" : "нет",
                                                        (dbCompetitiveGroup.IdLevelBudget != competitiveGroup.LevelBudget) ? "да" : "нет",
                                                        (dbCompetitiveGroup.ParentDirectionID != competitiveGroup.ParentDirectionID) ? "да" : "нет");
                                aii_logger.Debug("+-------------------+-----+------+-----+-----+-----+-----+-----+-----+-----+-----+");

                                // Если есть заявления, которые привязаны
                                if (!dont_check && !cgDeleteManager.CheckDependency())
                                {
                                    conflictStorage.SetObjectIsBroken(competitiveGroup,
                                         new ConflictStorage.ConflictMessage()
                                         {
                                             Code = ConflictMessages.DependedObjectsExists,
                                             Message = String.Format(
                                                        "Обновление данных у конкурса, отличных от наименования и значений количества мест невозможно из-за наличия заявлений с UID: ({0})",
                                                        string.Join(", ", cgDeleteManager.applicationUIDs))
                                         });

                                    aii_logger.ErrorFormat("Имеются привязанные заявления: {0}",
                                                           string.Join(", ", cgDeleteManager.applicationUIDs));
                                }
                                // Ошибка №14
                                // 1416: Разрешать обновлять только наименование и КЦП, если привязаны ВИ.
                                var depETIC = vocabularyStorage.EntranceTestItemCVoc.Items.Where(t => t.CompetitiveGroupID == dbCompetitiveGroup.CompetitiveGroupID);
                                if (!dont_check && depETIC.Any())
                                {
                                    conflictStorage.SetObjectIsBroken(competitiveGroup,
                                         new ConflictStorage.ConflictMessage()
                                         {
                                             Code = ConflictMessages.DependedObjectsExists,
                                             Message = String.Format(
                                                        "Обновление данных у конкурса, отличных от наименования и значений количества мест невозможно из-за наличия вступительных испытаний: с UID: ({0})",
                                                        string.Join(", ", depETIC.Select(t => string.IsNullOrWhiteSpace(t.UID) ? "[не задан]" : t.UID)))
                                         });
                                    aii_logger.ErrorFormat("Имеются вступительные испытания: {0}", string.Join(", ", depETIC.Select(t => string.IsNullOrWhiteSpace(t.UID) ? "[не задан]" : t.UID)));
                                }

                            }

                        }

                        if (competitiveGroup.CompetitiveGroupItem != null
                                && !((new int[] { (int)ItemChoiceType.NumberTargetO,
                                            (int)ItemChoiceType.NumberTargetOZ,
                                            (int)ItemChoiceType.NumberTargetZ }).Contains((int)competitiveGroup.CompetitiveGroupItem.ItemElementName))
                                && competitiveGroup.TargetOrganizations != null)
                            conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.CompetitiveGroupCannotHaveCGIAndTarget);


                        DebugMessage(sw, "Validate - CompetitiveGroup Base");

                        #region CompetitiveGroupItems - 
                        if (competitiveGroup.CompetitiveGroupItem != null)
                        {
                            // 1) Проверка на соответствие источнику финансирования и форме обучения в CompetitiveGroup
                            // пока решиили не делать: какое число введено, то и идет по соотв. полям CG (EducationForm, EducationSource)

                            // 2) Проверка на допустимость мест по квотам в конкурсной группе (Если NumberQuotaO >= 0 или NumberQuotaOZ >= 0 или NumberQuotaZ >= 0, то EducationLevelID один из { 2, 3, 5, 19})
                            if (competitiveGroup.CompetitiveGroupItem.Item > 0 &&
                                (competitiveGroup.CompetitiveGroupItem.ItemElementName == ItemChoiceType.NumberQuotaO ||
                                competitiveGroup.CompetitiveGroupItem.ItemElementName == ItemChoiceType.NumberQuotaOZ ||
                                competitiveGroup.CompetitiveGroupItem.ItemElementName == ItemChoiceType.NumberQuotaZ)
                                && !(new List<int> { EDLevelConst.Bachelor, EDLevelConst.Speciality }).Contains(competitiveGroup.EducationLevelID.To(0))
                            )
                                conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.CompetitiveGroupItemQuotaIncorrect);

                            // 3) Проверка что сумма мест по всем конкурсам не превышает колличества мест по данному направлению подготовки в объеме приема - 
                            // вынесена в общий блок CompetitiveGroup

                            // Проверить, что не заданы одновременно CompetitiveGroupItem.NumberTargetX и CompetitiveGroupTargetItem.NumberTargetX
                            if ((competitiveGroup.CompetitiveGroupItem != null && competitiveGroup.CompetitiveGroupItem.Item > 0)
                                //(new int[] { (int)ItemChoiceType.NumberTargetO, (int)ItemChoiceType.NumberTargetOZ, (int)ItemChoiceType.NumberTargetZ}.Contains((int)competitiveGroup.CompetitiveGroupItem.ItemElementName))
                                && (competitiveGroup.TargetOrganizations != null && competitiveGroup.TargetOrganizations.Any(t => t.CompetitiveGroupTargetItem != null && t.CompetitiveGroupTargetItem.Item > 0))
                                )
                            {
                                conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.CompetitiveGroupContainsNumberTargetXTwice);
                            }


                            //if (dbCompetitiveGroup!=null)
                            //{
                            //    // TODO: проверить
                            //    //var dbCompetitiveGroupItemEL = vocabularyStorage.CompetitiveGroupItemVoc.Items.Where(t => t.CompetitiveGroupID == dbCompetitiveGroup.CompetitiveGroupID && !(new int[] { 2, 3 }).Contains(t.EducationLevelID)).FirstOrDefault();
                            //    //if (dbCompetitiveGroupItemEL != null)
                            //    //    educationLevelId = dbCompetitiveGroupItemEL.EducationLevelID;
                            //}
                            //foreach (var cgItem in competitiveGroup.CompetitiveGroupItem)
                            //{
                            //    var cgItem = competitiveGroup.CompetitiveGroupItem;
                            //}
                        }
                        #endregion

                        DebugMessage(sw, "Validate - CompetitiveGroupItems");

                        #region CompetitiveGroupPrograms
                        if (competitiveGroup.EduPrograms != null)
                        {
                            foreach (var program in competitiveGroup.EduPrograms)
                            {
                                var dbInstitutionProgram = vocabularyStorage.InstitutionProgramVoc.Items.Where(t => t.UID == program.UID).FirstOrDefault();
                                if (dbInstitutionProgram == null)
                                {
                                    conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.DictionaryItemAbsent, "CompetitiveGroup.program UID=" + program.UID);
                                    continue;
                                }

                                program.InstitutionProgramID = dbInstitutionProgram.ID;

                                // UID - Проверка на уникальность в пакете для данног конкурса
                                //if (vocabularyStorage.InstitutionProgramVoc.Items.Any(t => t.UID == program.UID && t.CompetitiveGroupUID == competitiveGroup.UID))
                                //    conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.UIDMustBeUniqueForAllObjectInstancesOfType, "EduProgram", program.UID);
                                var dupUIDs = competitiveGroup.EduPrograms.GroupBy(c => c.UID).Where(c => c.Count() > 1).Select(c => c.Key).ToList();
                                foreach (var uid in dupUIDs)
                                    conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.UIDMustBeUniqueForAllObjectInstancesOfType, "competitiveGroup.EduPrograms", program.UID);


                                //if (campaign != null)
                                //{
                                //    if (programUIDs.Any(x => program.UID.Equals(x.Item1, StringComparison.InvariantCultureIgnoreCase)
                                //                    && campaign.CampaignID == x.Item2))
                                //        conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.UIDMustBeUniqueForAllObjectInstancesOfType, "EduProgram", program.UID);
                                //    else
                                //        programUIDs.Add(new Tuple<string, int>(program.UID, campaign.CampaignID));
                                //}

                                // Проверка на уникальность UID в БД и пакете для данной CompetitiveGroupToProgram.CompetitiveGroupID
                                //if (vocabularyStorage.InstitutionProgramVoc.Items.Any(t => t.UID != program.UID && t.CompetitiveGroupUID == competitiveGroup.UID))
                                //    conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.UIDMustBeUniqueForChildrenObjects, "competitiveGroup", program.UID);

                                //if (competitiveGroup.EduPrograms.Any(t=> t.UID != program.UID))
                                //    conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.NameMustBeUniqueForAllObjectInstancesOfType, "EduProgram", program.UID);

                                //// проверить, что поля не превышают размеры в БД. 
                                //// TODO: Вынести на уровень XSD
                                //if (program.Name != null && program.Name.Length > 200)
                                //    conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.FieldLengthExceeded, "EduProgram.Name", "200");

                                //if (program.Code != null && program.Code.Length > 10)
                                //    conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.FieldLengthExceeded, "EduProgram.Code", "10");
                            }
                        }
                        #endregion

                        DebugMessage(sw, "Validate - CompetitiveGroupPrograms");

                        CheckAvailablaDirections(competitiveGroup);
                        //CheckRestrictionOnProfileAndCreativeDirection(competitiveGroup);
                        //CheckRestrictionOnEntranceSubjectInDirectionsForCompetitiveGroup(competitiveGroup);

                        // Имя конкурса должно быть уникально в контексте кампании
                        if (campaign != null)
                        {
                            if (vocabularyStorage.CompetitiveGroupVoc.Items.Any(t => t.CampaignID == campaign.CampaignID && t.Name == competitiveGroup.Name && t.UID != competitiveGroup.UID))
                                conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.CompetitiveGroupNameMustBeUnique, competitiveGroup.Name);
                            else if (cgNames.Any(x => competitiveGroup.Name.Equals(x.Item1, StringComparison.InvariantCultureIgnoreCase)
                                                    && campaign.CampaignID == x.Item2))
                                conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.CompetitiveGroupNameMustBeUnique, competitiveGroup.Name);
                            else
                                cgNames.Add(new Tuple<string, int>(competitiveGroup.Name, campaign.CampaignID));
                        }

                        // Проверка на минимальные баллы ЕГЭ для льгот - теперь внутри кода
                        // CheckMinEgeMarksForCommonBenefits(competitiveGroup, campaign);

                        DebugMessage(sw, "Validate - CheckMinEgeMarksForCommonBenefits");

                        #region TargetOrganization
                        if (competitiveGroup.TargetOrganizations != null)
                            foreach (var targetOrganization in competitiveGroup.TargetOrganizations)
                            {
                                List<string> cgtDependencyApplicationIDs = new List<string>();
                                var dbTargetOrganization = vocabularyStorage.CompetitiveGroupTargetVoc.Items.Where(t => t.UID == targetOrganization.UID).FirstOrDefault();
                                if (dbTargetOrganization == null)
                                {
                                    conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.DictionaryItemAbsent, "CompetitiveGroup.TargetOrganization UID=" + targetOrganization.UID);
                                    continue;
                                }

                                targetOrganization.ID = dbTargetOrganization.ID;
                                if (targetOrganization.CompetitiveGroupTargetItem != null && targetOrganization.CompetitiveGroupTargetItem.Item > 0)
                                {
                                    if (competitiveGroup.CompetitiveGroupItem == null)
                                        competitiveGroup.CompetitiveGroupItem = new PackageDataAdmissionInfoCompetitiveGroupCompetitiveGroupItem();
                                    competitiveGroup.CompetitiveGroupItem.Item += targetOrganization.CompetitiveGroupTargetItem.Item;
                                }

                                //var cgtDM = new CompetitiveGroupTargetDeleteManager(dbTargetOrganization, vocabularyStorage, dependencyVoc);
                                //if (!cgtDM.CheckDependency())
                                //{
                                //    cgtDependencyApplicationIDs = cgtDM.applicationUIDs;
                                //    //if (targetOrganization.TargetOrganizationName != dbTargetOrganization.Name)
                                //    //    conflictStorage.SetObjectIsBroken(competitiveGroup,
                                //    //     new ConflictStorage.ConflictMessage()
                                //    //     {
                                //    //         Code = ConflictMessages.DependedObjectsExists,
                                //    //         Message = String.Format(
                                //    //                 "Обновление названия у организации целевого приема невозможно из-за наличия заявлений с UID: ({0})",
                                //    //                 string.Join(", ", cgtDM.applicationUIDs))
                                //    //     });
                                //}



                                //if (targetOrganization.CompetitiveGroupTargetItem!=null)
                                //    //foreach (var targetOrganizationItem in targetOrganization.Items)
                                //    {
                                //        var targetOrganizationItem = targetOrganization.CompetitiveGroupTargetItem;
                                //    }
                            }
                        #endregion

                        DebugMessage(sw, "Validate - TargetOrganizations");

                        #region competitiveGroup.CommonBenefit
                        if (competitiveGroup.CommonBenefit != null)
                            foreach (var commonBenefit in competitiveGroup.CommonBenefit)
                            {
                                #region Проверки

                                if (string.IsNullOrWhiteSpace(commonBenefit.UID))
                                    conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.BenefitMustHaveUID);

                                CheckBenefit(competitiveGroup, commonBenefit);

                                //#22119 В КГ общая льгота может быть только "Без ВИ"(id=1), сейчас если указать другую льготу сервис не выдаёт ошибок
                                if (commonBenefit.BenefitKindID != 1) // раньше было commonBenefit.BenefitKindID != 3
                                {
                                    conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.CompetitiveGroupContainsNotAllowedBenefitType);
                                }

                                if (!commonBenefit.IsVsosh)
                                {
                                    // Должен быть указан ЛИБО предмет с баллом, ЛИБО 1 или 2 чекбокса 
                                    // Если нет предмета и чекбоксов - ошибка
                                    if (commonBenefit.MinEgeMarks == null && !commonBenefit.IsCreative && !commonBenefit.IsAthletic)
                                    {
                                        conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.CommonBenefitMustHaveSubjectOrCheckbox, commonBenefit.UID);
                                    }
                                    // Если и предмет и чекбоксы - ошибка
                                    if (commonBenefit.MinEgeMarks != null && (commonBenefit.IsCreative || commonBenefit.IsAthletic))
                                    {
                                        conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.CommonBenefitMustHaveSubjectOrCheckbox, commonBenefit.UID);
                                    }
                                }

                                if (commonBenefit.MinEgeMarks != null)
                                {
                                    var subject = commonBenefit.MinEgeMarks.MinMarks;
                                    var dbSubject = VocabularyStatic.SubjectVoc.Items.FirstOrDefault(t => t.SubjectID == subject.SubjectID);


                                    if (dbSubject == null)
                                        conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.SubjectIsNotFounded, subject.SubjectID.ToString());
                                    else if (dbSubject != null && dbSubject.IsEge && !competitiveGroup.IsForKrym && commonBenefit.OlympicYear >= 2014 && !commonBenefit.IsVsosh) // Хардкод, что определение мин. балла только у олимпиад начиная с 2014 года
                                    {
                                        var systemMinEge = VocabularyStatic.GlobalMinEgeVoc.Items.FirstOrDefault(x => x.EgeYear == commonBenefit.OlympicYear);

                                        if (systemMinEge == null || systemMinEge.MinEgeScore == 0)
                                        {
                                            // По честному, это ошибка скорее к нам, чем к пользователям. 
                                            // Впрочем, они заметят - мы поправим
                                            conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.NoMinSystemEGE);
                                            continue;
                                        }

                                        //if (!benefit.MinEgeMarkSpecified && campaignYear >= 2014)
                                        //{
                                        //    conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.NoMinEgeForOlympics);
                                        //    continue;
                                        //}

                                        if (systemMinEge.MinEgeScore > subject.MinMark)
                                        {
                                            conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.BenefitEGELessSystemMinEGE, subject.MinMark.ToString(), systemMinEge.MinEgeScore.ToString());
                                            continue;
                                        }
                                    }


                                    //var subjectIDs = commonBenefit.MinEgeMarks.Where(t => t.SubjectID != 0).Select(t => t.SubjectID);
                                    //if (subjectIDs.Distinct().Count() != subjectIDs.Count())
                                    //    conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.DublicateSubjects);
                                }

                                // проверка, что в БД нет CommonBenefit с таким UID, но принадлежащим другой конкурсной группе
                                var dbCommonBenefit = vocabularyStorage.BenefitItemCVoc.Items.Where(t => t.UID == commonBenefit.UID).FirstOrDefault();
                                if (dbCommonBenefit != null)
                                {
                                    commonBenefit.ID = dbCommonBenefit.ID;
                                    if (dbCommonBenefit.CompetitiveGroupUID != competitiveGroup.UID)
                                    {
                                        conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.BenefitItemCExistsInDbInOtherCompetitiveGroup, commonBenefit.UID, dbCommonBenefit.CompetitiveGroupUID);
                                    }
                                }
                                #endregion

                            }
                        #endregion competitiveGroup.CommonBenefit

                        DebugMessage(sw, "Validate - CommonBenefits");

                        #region EntranceTestItems
                        // Направление творческое?
                        var directionIsCreative = VocabularyStatic.EntranceTestCreativeDirectionVoc.Items.Any(t => t.DirectionID == competitiveGroup.DirectionID.To(0));
                        var parentDirectionIsCreative = VocabularyStatic.EntranceTestCreativeDirectionVoc.Items.Any(t => t.ParentID == competitiveGroup.ParentDirectionID.To(0));
                        var directionIsProfile = VocabularyStatic.EntranceTestProfileDirectionVoc.Items.Any(t => t.DirectionID == competitiveGroup.DirectionID.To(0));

                        if (competitiveGroup.EntranceTestItems != null)
                        {
                            var duplicateTests = new HashSet<string>();
                            var isKVK = false;
                            var isSPO = (campaign != null && campaign.CampaignTypeID == GVUZ.DAL.Dapper.ViewModel.Dictionary.CampaignTypesView.SPO);
                            var isMVD = DictionaryContext.Dictionaries.CheckIsMVD(vocabularyStorage.FounderEsrpOrgId);
                            if (campaign != null && campaign.CampaignTypeID == GVUZ.DAL.Dapper.ViewModel.Dictionary.CampaignTypesView.HighQualification)
                            {
                                // FIS-1456 Для вступительных испытаний в конкурсах по кадрам высшей квалификации дать задавать приоритет 0
                                // (но только если хотя бы у одного испытания есть приоритет)
                                isKVK = true;

                                if (!competitiveGroup.EntranceTestItems.Any(t => t.EntranceTestPriority > 0))
                                    conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.CompetitiveGroupKVKMustHaveOneEntranceTestPriority);
                            }

                            foreach (var entranceTestItem in competitiveGroup.EntranceTestItems)
                            {
                                Debug.WriteLine("entranceTestItem:{0}", entranceTestItem);
                                // Проверка уникальности UID
                                CheckUniqueUID(entranceTestItem.EntranceTestBenefits, competitiveGroup);

                                /* 
                                    Добавление разрешено если в БД нет записи у этого же КГ с таким же EntranceTestTypeID и Subject (ID или Name) или если такая запись успешно удалилась. Иначе ошибка.
                                    Обновление разрешено если: не изменился Subject или изменился и старая запись корректно удалилась, иначе ошибка.
                                    При наличии внешних зависимостей - формируется ошибка и конфликты.
                                */
                                var dbEntranceTestItem = vocabularyStorage.EntranceTestItemCVoc.Items.FirstOrDefault(t => t.UID == entranceTestItem.UID);
                                if (dbEntranceTestItem != null)
                                {
                                    entranceTestItem.ID = dbEntranceTestItem.ID;
                                    if (dbEntranceTestItem.CompetitiveGroupUID != competitiveGroup.UID)
                                    {
                                        conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.EntranceTestItemCExistsInDbInOtherCompetitiveGroup,
                                                                            entranceTestItem.UID, dbEntranceTestItem.CompetitiveGroupUID);
                                    }

                                    // Провека внешний зависимостей. Если есть, то ошибка и конфликт!
                                    var eticHasDepedency = vocabularyStorage.ApplicationEntranceTestDocumentVoc.Items.Any(t => t.EntranceTestItemID == dbEntranceTestItem.EntranceTestItemID);
                                    //if (eticHasDepedency &&
                                    //    //entranceTestItem.EntranceTestPriority != dbEntranceTestItem.EntranceTestPriority
                                    //    )
                                    //{
                                    //    conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.EntranceTestItemCExistsInDbInOtherCompetitiveGroup123);
                                    //}

                                    //// Проверка, что если такая запись есть в БД, то не изменился предмет. 
                                    //// Или это нужно только, если есть зависимости?
                                    if (eticHasDepedency &&
                                        dbEntranceTestItem.SubjectID != 0
                                        && entranceTestItem.EntranceTestSubject != null
                                        && entranceTestItem.EntranceTestSubject.Item is uint
                                        && (uint)(entranceTestItem.EntranceTestSubject.Item) != dbEntranceTestItem.SubjectID
                                    )
                                        conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.EntranceTestCannotChangeSubject, entranceTestItem.UID);
                                }

                                if (!VocabularyStatic.EntranceTestTypeVoc.Items.Any(t => t.EntranceTestTypeID == entranceTestItem.EntranceTestTypeID))
                                    conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.EntranceTestTypeIDNotFound, entranceTestItem.EntranceTestTypeID.ToString());


                                // Если entranceTestItem.EntranceTestPriority задан, то он д.б. числом от 1 до 10 кроме КВК 0-10 (выше) и МВД (0-10 и можно вообще не задавать)
                                int priority = entranceTestItem.EntranceTestPriority.To(0);
                                int minPriority = (isKVK || isSPO || (isMVD && entranceTestItem.EntranceTestTypeID == ServiceModel.Import.EntranceTestType.MainType)) ? 0 : 1;
                                if (priority < minPriority || priority > 10)
                                {
                                    conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.EntranceTestPriorityIncorrect, entranceTestItem.UID);
                                }

                                //// ВИ не может состоять одновременно в двух группах
                                //if (entranceTestItem.IsFirst && entranceTestItem.IsSecond)
                                //{
                                //    conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.EntranceTestInTwoGroups, entranceTestItem.SubjectName);
                                //}

                                if (!isKVK && !isSPO && !(isMVD && entranceTestItem.EntranceTestTypeID == ServiceModel.Import.EntranceTestType.MainType))
                                {
                                    if (entranceTestItem.IsForSPOandVO == null)
                                    {
                                        // Если задан, что в пакете у КГ не должно быть еще записи с таким же приоритетом, кроме случаев замены
                                        //var sameInPackage = competitiveGroup.EntranceTestItems.Any(eti => eti.EntranceTestPriority == entranceTestItem.EntranceTestPriority && eti.UID != entranceTestItem.UID && eti.IsForSPOandVO == null);
                                        var sameInPackage = competitiveGroup.EntranceTestItems.Any(eti => eti.EntranceTestPriority == entranceTestItem.EntranceTestPriority && 
                                        eti.UID != entranceTestItem.UID  && eti.IsFirst != entranceTestItem.IsFirst);

                                        // Если задан, то в БД у данного пакета не должно быть записи с другим УИД и таким же приоритетом, кроме случаев замены
                                        //var sameInDB = vocabularyStorage.EntranceTestItemCVoc.Items.Any(eti => eti.EntranceTestPriority == entranceTestItem.EntranceTestPriority && eti.UID != entranceTestItem.UID && eti.CompetitiveGroupUID == competitiveGroup.UID && !eti.IsForSPOandVO);
                                        var sameInDB = vocabularyStorage.EntranceTestItemCVoc.Items.Any(eti => eti.EntranceTestPriority == entranceTestItem.EntranceTestPriority && 
                                        eti.UID != entranceTestItem.UID && eti.CompetitiveGroupUID == competitiveGroup.UID && eti.IsFirst != entranceTestItem.IsFirst && !eti.IsForSPOandVO);
                          

                                        if (sameInDB || sameInPackage)
                                        {
                                            conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.EntranceTestPriorityIncorrect, entranceTestItem.UID);
                                        }
                                    }
                                }

                                string uniqTestID = entranceTestItem.EntranceTestTypeID.ToString() + "@" + entranceTestItem.EntranceTestSubject.Item.ToString();
                                if (duplicateTests.Contains(uniqTestID))
                                    conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.CompetitiveGroupEntranceTestDuplicateInPackage);
                                else
                                    duplicateTests.Add(uniqTestID);


                                decimal maxValue =
                                    campaign.CampaignTypeID == GVUZ.DAL.Dapper.ViewModel.Dictionary.CampaignTypesView.BachelorAndSpeciality ?
                                    100 : 999;
                                if (entranceTestItem.MinScore < 0 || entranceTestItem.MinScore > maxValue)
                                {
                                    //conflictStorage.SetObjectIsBroken(application, ConflictMessages.ResultValueRange, etResult.ResultValue.ToString(), maxValue.ToString());
                                    conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.MinValueRange, entranceTestItem.MinScore.ToString(), maxValue.ToString());
                                }

                                // Проверяем на корректность выставления минимальных баллов в КГ, но только если тип ВИ = основное И и тип ПК не магистратура, КВК, СПО, иностранцы
                                if (entranceTestItem.EntranceTestTypeID == ServiceModel.Import.EntranceTestType.MainType
                                    && campaign.CampaignTypeID == GVUZ.DAL.Dapper.ViewModel.Dictionary.CampaignTypesView.BachelorAndSpeciality
                                    && !competitiveGroup.IsForKrym
                                    ) //обычные ВИ (хардкод!)
                                {
                                    if (entranceTestItem.EntranceTestSubject.Item is uint)
                                    {
                                        int subjectID = ((uint)entranceTestItem.EntranceTestSubject.Item).To(0);
                                        if (subjectID > 0)
                                        {
                                            var egeMinValueItem = VocabularyStatic.SubjectEgeMinValueVoc.Items.Where(t => t.SubjectID == subjectID).FirstOrDefault();
                                            if (egeMinValueItem != null)
                                            {
                                                if (!entranceTestItem.MinScoreSpecified)
                                                    entranceTestItem.MinScore = egeMinValueItem.MinValue;
                                                else if (egeMinValueItem.MinValue > entranceTestItem.MinScore)
                                                {
                                                    conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.EntranceTestContainsScoreLowerThanRequired);
                                                }
                                            }
                                        }
                                    }
                                }

                                if ((entranceTestItem.EntranceTestTypeID == ServiceModel.Import.EntranceTestType.CreativeType) && !(directionIsCreative || parentDirectionIsCreative))
                                    conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.CompetitiveGroupNotAllowedCreativeEntranceTests);

                                if (entranceTestItem.EntranceTestTypeID == ServiceModel.Import.EntranceTestType.ProfileType && !directionIsProfile)
                                    conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.CompetitiveGroupContainsNotAllowedExtraProfileSubjectInEntranceTest);


                                // Если subjectID задан, то проверка по справочнику
                                if (entranceTestItem.EntranceTestSubject.Item is uint)
                                {
                                    entranceTestItem.SubjectID = ((uint)entranceTestItem.EntranceTestSubject.Item).To(0);
                                    entranceTestItem.SubjectName = "";
                                    if (!VocabularyStatic.SubjectVoc.Items.Any(t => t.SubjectID == entranceTestItem.SubjectID))
                                        conflictStorage.SetObjectIsBroken(competitiveGroup, new ConflictStorage.ConflictMessage
                                        {
                                            Code = ConflictMessages.SubjectIsNotFounded,
                                            Message = String.Format(ConflictMessages.GetMessage(ConflictMessages.SubjectIsNotFounded),
                                                                    entranceTestItem.SubjectID)
                                        });
                                }
                                else
                                {
                                    entranceTestItem.SubjectID = 0;
                                    entranceTestItem.SubjectName = entranceTestItem.EntranceTestSubject.Item.ToString();
                                    if (string.IsNullOrWhiteSpace(entranceTestItem.SubjectName))
                                    {
                                        conflictStorage.SetObjectIsBroken(competitiveGroup, new ConflictStorage.ConflictMessage
                                        {
                                            Code = ConflictMessages.SubjectIsNotFounded,
                                            Message = @"Необходимо заполнить EntranceTestItem\EntranceTestSubject\SubjectID или SubjectName!"
                                        });
                                    }
                                }

                                // Проверка, что в БД у данного конкурса нет другого ВИ с таким же типом и предметом
                                if (vocabularyStorage.EntranceTestItemCVoc.Items.Any(t =>
                                    t.CompetitiveGroupUID == competitiveGroup.UID &&
                                    t.UID != entranceTestItem.UID &&
                                    t.EntranceTestTypeID == entranceTestItem.EntranceTestTypeID &&
                                    (t.SubjectID != 0 && entranceTestItem.SubjectID != 0 && t.SubjectID == entranceTestItem.SubjectID) &&
                                    (!string.IsNullOrWhiteSpace(t.SubjectName) && !string.IsNullOrWhiteSpace(entranceTestItem.SubjectName) && t.SubjectName == entranceTestItem.SubjectName)
                                ))
                                    conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.EntranceTestDublicateTypeAndSubject, entranceTestItem.EntranceTestTypeID.ToString(), entranceTestItem.EntranceTestSubject.Item.ToString());

                                if (entranceTestItem.IsForSPOandVO != null)
                                {
                                    // Заменяемый предмет должен быть среди ВИ данного конкурса! (в пакете или БД)
                                    var replaceUID = entranceTestItem.IsForSPOandVO.ReplacedEntranceTestItemUID;
                                    var replacedInPackage = competitiveGroup.EntranceTestItems.Any(eti => eti.UID == replaceUID && eti.IsForSPOandVO == null);
                                    var replacedInDB = vocabularyStorage.EntranceTestItemCVoc.Items.Any(eti => eti.UID == replaceUID && eti.CompetitiveGroupUID == competitiveGroup.UID && !eti.IsForSPOandVO);

                                    if (!replacedInPackage && !replacedInDB)
                                    {
                                        conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.EntranceTestReplacedItemNotFound, replaceUID);
                                    }
                                }

                                // Льготы при ВИ
                                if (entranceTestItem.EntranceTestBenefits != null)
                                {
                                    foreach (var entranceTestBenefit in entranceTestItem.EntranceTestBenefits)
                                    {
                                        #region Проверки
                                        if (string.IsNullOrWhiteSpace(entranceTestBenefit.UID))
                                            conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.BenefitMustHaveUID);

                                        var dbEntranceTestBenefit = vocabularyStorage.BenefitItemCVoc.Items.FirstOrDefault(t => t.UID == entranceTestBenefit.UID);
                                        if (dbEntranceTestBenefit != null)
                                        {
                                            entranceTestBenefit.ID = dbEntranceTestBenefit.ID;
                                            if (dbEntranceTestBenefit.EntranceTestItemUID != entranceTestItem.UID)
                                            {
                                                conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.BenefitItemCExistsInDbInOtherEntranceTestItemC,
                                                                                    entranceTestBenefit.UID, dbEntranceTestBenefit.EntranceTestItemUID);
                                            }
                                        }

                                        CheckBenefit(competitiveGroup, entranceTestBenefit);



                                        //if (!VocabularyStatic.BenefitVoc.Items.Any(t => t.BenefitID == entranceTestBenefit.BenefitKindID))
                                        //    conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.DictionaryItemAbsent, "EntranceTestItems.EntranceTestBenefits.BenefitKindID");

                                        //#22769 При указании вида льготы для ВИ можно указать любой ид. Для основных ВИ должен быть доступен вид льготы только "100 баллов по осн ВИ", для профильных только "100 баллов для доп ВИ"
                                        if (entranceTestItem.EntranceTestTypeID == 1 && entranceTestBenefit.BenefitKindID != 3)
                                        {
                                            conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.CompetitiveGroupContainsNotAllowedBenefitType);
                                        }

                                        if (entranceTestItem.EntranceTestTypeID == 3 && entranceTestBenefit.BenefitKindID != 2)
                                        {
                                            conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.CompetitiveGroupContainsNotAllowedBenefitType);
                                        }

                                        var subject = VocabularyStatic.SubjectVoc.Items.FirstOrDefault(t => t.SubjectID == entranceTestItem.SubjectID);

                                        if (!competitiveGroup.IsForKrym && subject != null && subject.IsEge && entranceTestBenefit.OlympicYear >= 2014 && !entranceTestBenefit.IsVsosh)
                                        {
                                            // проверка на мин. балл
                                            // примечательно, 06.05.2016 совместно с Наргизой решили не смотреть на тип ВИ, а только на subject.IsEge
                                            var systemMinEge = VocabularyStatic.GlobalMinEgeVoc.Items.FirstOrDefault(x => x.EgeYear == entranceTestBenefit.OlympicYear);

                                            if (systemMinEge == null || systemMinEge.MinEgeScore == 0)
                                            {
                                                // По честному, это ошибка скорее к нам, чем к пользователям. Впрочем, они заметят - мы поправим!
                                                conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.NoMinSystemEGE);
                                                continue;
                                            }

                                            if (systemMinEge.MinEgeScore > entranceTestBenefit.MinEgeMark)
                                            {
                                                conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.BenefitEGELessSystemMinEGE, entranceTestBenefit.MinEgeMark.ToString(), systemMinEge.MinEgeScore.ToString());
                                                continue;
                                            }
                                        }
                                        #endregion

                                    }
                                }
                            }
                        }
                        #endregion

                        DebugMessage(sw, "Validate - EntranceTestItems");



                        if (!competitiveGroup.IsBroken)
                        {
                            aii_logger.DebugFormat("Проверка конкурсных групп пакета №{0}...", packageData.ImportPackageId);
                            CheckAllowedPlacesForAdmissionVolumeAndCompetitiveGroups(null, null, competitiveGroup);
                        }
                    }
                }

                DebugMessage(sw, "Validate - CompetitiveGroups (весь цикл)");
            }
        }
        /// <summary>
        /// Проверка олимпиадных данных из льготы (общая для нескольких разных мест)
        /// </summary>
        /// <param name="competitiveGroup"></param>
        /// <param name="commonBenefit"></param>
        private void CheckBenefit(PackageDataAdmissionInfoCompetitiveGroup competitiveGroup, ICommonBenefitItem commonBenefit)
        {
            if (commonBenefit.OlympicDiplomTypes != null)
            {
                // проверка наличия значения в справочнике
                foreach (var odtype in commonBenefit.OlympicDiplomTypes)
                    if (!VocabularyStatic.OlympicDiplomTypeVoc.Items.Any(t => t.OlympicDiplomTypeID == odtype))
                        conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.DictionaryItemAbsent, "Benefit.OlympicDiplomTypes.OlympicDiplomTypeID");

                // не должно быть повторов
                if (commonBenefit.OlympicDiplomTypes.Distinct().Count() != commonBenefit.OlympicDiplomTypes.Count())
                    conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.DictionaryItemNonUnique, "Benefit.OlympicDiplomTypes.OlympicDiplomTypeID");

            }


            // для всех олимпиад, но передали олимпиады - ошибка.
            if (commonBenefit.IsForAllOlympics)
            {
                //commonBenefit.OlympicYear = DateTime.Now.Year;

                if (commonBenefit.Olympics != null && commonBenefit.Olympics.Length > 0)
                    conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.BenefitContainsOlympicsAndAllOlympics);
                if (commonBenefit.BenefitItemOlympicsLevels != null && commonBenefit.BenefitItemOlympicsLevels.Any(c => c.OlympicID != 0))
                    conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.BenefitContainsOlympicsAndAllOlympics);

                // Если true и не передали ProfileForAllOlympics, то ошибка
                // Если true и не передали LevelForAllOlympics, то ошибка
                // Если true и не передали ClassForAllOlympics, то ошибка
                if (commonBenefit.ProfileForAllOlympics == null || commonBenefit.ProfileForAllOlympics.Length == 0)
                    conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.BenefitForAllOlympicMustContainProfile);
                if (!commonBenefit.LevelForAllOlympicsSpecified || commonBenefit.LevelForAllOlympics == 0)
                    conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.BenefitForAllOlympicMustContainLevel);
                if (!commonBenefit.ClassForAllOlympicsSpecified || commonBenefit.ClassForAllOlympics == 0)
                    conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.BenefitForAllOlympicMustContainClass);
            }
            else // не для всех олимпиад и ничего не передали - ошибка
            {
                if ((commonBenefit.Olympics == null || commonBenefit.Olympics.Length == 0) &&
                    (commonBenefit.BenefitItemOlympicsLevels == null || commonBenefit.BenefitItemOlympicsLevels.Length == 0))
                    conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.BenefitContainsNoOlympicsAndNoAllOlympics);
            }

            if (!VocabularyStatic.BenefitVoc.Items.Any(t => t.BenefitID == commonBenefit.BenefitKindID))
                conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.DictionaryItemAbsent, "Benefit.BenefitKindID");


            if (commonBenefit.LevelForAllOlympicsSpecified)
            {
                CheckBenefitLevel(competitiveGroup, commonBenefit.LevelForAllOlympics, "Benefit.LevelForAllOlympics = {0}");
            }
            if (commonBenefit.ClassForAllOlympicsSpecified)
            {
                CheckBenefitClass(competitiveGroup, commonBenefit.ClassForAllOlympics, "Benefit.ClassForAllOlympics = {0}");
            }
            CheckBenefitProfiles(competitiveGroup, commonBenefit.ProfileForAllOlympics, "Benefit.ProfileForAllOlympics.ProfileID = {0}");


            if (commonBenefit.Olympics != null)
            {
                var campaign = vocabularyStorage.CampaignVoc.GetItemByUid(competitiveGroup.CampaignUID);
                var olympicValidityYears = 5;//#FIS-1723 Срок действия олимпиад - текущий + 4 предыдущих года
                var olympicYearStart = campaign.YearStart - olympicValidityYears;

                foreach (var o in commonBenefit.Olympics)
                {
                    var olympic = VocabularyStatic.OlympicTypeVoc.Items.Where(t => t.OlympicID == o).FirstOrDefault();
                    if (olympic == null)
                    {
                        conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.DictionaryItemAbsent, string.Format("Benefit.Olympics.OlympicID = {0}", o));
                        continue;
                    }

                    if (olympic.OlympicYear <= olympicYearStart)//#FIS-1723
                    {
                        conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.OlympicYearExpired, string.Format("Benefit.Olympics.OlympicID = {0}", o));
                        continue;
                    }

                    //if (commonBenefit.OlympicYear == 0)
                    //    commonBenefit.OlympicYear = olympic.OlympicYear;
                    //else if (commonBenefit.OlympicYear != olympic.OlympicYear)
                    //    conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.BenefitOlympicsMustBeTheSameYear);
                }
            }

            if (!commonBenefit.IsForAllOlympics && commonBenefit.BenefitItemOlympicsLevels != null)
            {
                foreach (var ol in commonBenefit.BenefitItemOlympicsLevels)
                {
                    var olympic = VocabularyStatic.OlympicTypeVoc.Items.Where(t => t.OlympicID == ol.OlympicID.To(0)).FirstOrDefault();
                    if (olympic == null)
                    {
                        conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.DictionaryItemAbsent, "Benefit.Olympics.OlympicID");
                        continue;
                    }

                    //if (commonBenefit.OlympicYear == 0)
                    //    commonBenefit.OlympicYear = olympic.OlympicYear;
                    //else if (commonBenefit.OlympicYear != olympic.OlympicYear)
                    //    conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.BenefitOlympicsMustBeTheSameYear);

                    // 23.06.2016 - разрешили не задавать OlympicLevel для олимпиад ВсОШ (OlympicNumber is null)
                    if (olympic.OlympicNumber == 0 && ol.LevelID == 0)
                        ol.LevelID = OlympicTypeVoc.All_Olympic_Level;

                    CheckBenefitLevel(competitiveGroup, ol.LevelID, "Benefit.OlympicsLevel.LevelID = {0}");
                    CheckBenefitClass(competitiveGroup, ol.ClassID, "Benefit.OlympicsLevel.ClassID = {0}");
                    CheckBenefitProfiles(competitiveGroup, ol.Profiles, "Benefit.OlympicsLevel.Profiles.ProfileID = {0}", ol.OlympicID.To(0), ol.LevelIDSpecified ? ol.LevelID.To(0) : OlympicTypeVoc.All_Olympic_Level);
                }
            }
        }

        private void CheckBenefitLevel(PackageDataAdmissionInfoCompetitiveGroup competitiveGroup, uint level, string errorMessage)
        {
            //if (!VocabularyStatic.OlympicLevelVoc.Items.Any(t=> t.ID == level.To(0)))
            //    conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.DictionaryItemAbsent, string.Format(errorMessage, level));

            if (level != 0 && (level < OlympicTypeVoc.Min_Olympic_Level || (level > OlympicTypeVoc.Max_Olympic_Level && level != OlympicTypeVoc.All_Olympic_Level)))
                conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.DictionaryItemAbsent, string.Format(errorMessage, level));
        }

        private void CheckBenefitClass(PackageDataAdmissionInfoCompetitiveGroup competitiveGroup, uint oclass, string errorMessage)
        {
            if (oclass != 0 && (oclass < OlympicTypeVoc.Min_Olympic_Class || (oclass > OlympicTypeVoc.Max_Olympic_Class && oclass != OlympicTypeVoc.All_Olympic_Class)))
                conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.DictionaryItemAbsent, string.Format(errorMessage, oclass));
        }

        private void CheckBenefitProfiles(PackageDataAdmissionInfoCompetitiveGroup competitiveGroup, uint[] profiles, string errorMessage, int olympicID = 0, int levelMask = OlympicTypeVoc.All_Olympic_Level)
        {
            if (profiles == null)
                return;

            foreach (var profile in profiles)
            {
                var olympicProfile = VocabularyStatic.OlympicProfileVoc.Items.FirstOrDefault(t => t.ID == profile.To(0));
                if (olympicProfile == null)
                {
                    conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.DictionaryItemAbsent, string.Format(errorMessage, profile));
                    continue;
                }

                var dbProfiles = VocabularyStatic.OlympicTypeProfileVoc.Items.Where
                    (t =>
                        (t.OlympicProfileID == profile.To(0) || profile.To(0) == OlympicTypeVoc.All_Olympic_Profile)
                        && (t.OlympicTypeID == olympicID || olympicID == 0)
                    );

                if (!dbProfiles.Any())
                    conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.OlympicProfileFromOtherOlympic, profile.ToString(), olympicID.ToString());

                //if (dbProfile == null)
                //{
                //    conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.DictionaryItemAbsent, string.Format(errorMessage, profile));
                //    continue;
                //}

                //if (olympicID != 0 && !dbProfiles.Any(t=> t.OlympicTypeID == olympicID))
                //    conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.OlympicProfileFromOtherOlympic, profile.ToString(), olympicID.ToString());

                var level = 0;  //OlympicTypeVoc.All_Olympic_Level;
                foreach (var dbProfile in dbProfiles)
                {
                    switch (dbProfile.OlympicLevelID)
                    {
                        case 2:
                            level |= 1;
                            break;
                        case 3:
                            level |= 2;
                            break;
                        case 4:
                            level |= 4;
                            break;
                    }
                }

                if (level != 0 && (level & levelMask) == 0)
                    conflictStorage.SetObjectIsBroken(competitiveGroup, ConflictMessages.OlympicProfileHasOtherLevel, profile.ToString(), levelMask.ToString());

            }
        }

        private void SetCampaignObjectsBroken(CompetitiveGroupVocDto competitiveGroup, List<string> applicationUIDs)
        {
            conflictStorage.SetObjectIsBroken(new PackageDataAdmissionInfoCompetitiveGroup() { UID = competitiveGroup.UID, Name = competitiveGroup.Name },
                                        ConflictMessages.CompetitiveGroupCannotBeRemovedWithApplications,
                                        string.Join(",", applicationUIDs));

            SetCampaignObjectsBroken(competitiveGroup.CampaignID, applicationUIDs);
        }

        private void SetCampaignObjectsBroken(AdmissionVolumeVocDto admissionVolume, List<string> applicationUIDs)
        {
            conflictStorage.SetObjectIsBroken(new PackageDataAdmissionInfoItem() { UID = admissionVolume.UID },
                            ConflictMessages.AdmissionVolumeCannotBeRemovedWithApplications,
                            string.Join(",", applicationUIDs));

            SetCampaignObjectsBroken(admissionVolume.CampaignID, applicationUIDs);
        }

        /// <summary>
        /// При удалении AdmissionVolume и CompetitiveGroup, не вошедших в пакет - если ошибка удаления, то по всем записям данной Кампании проставляются ошибки импорта
        /// </summary>
        /// <param name="campaignID">ИД Кампании</param>
        /// <param name="applicationUIDs"></param>
        private void SetCampaignObjectsBroken(int campaignID, List<string> applicationUIDs)
        {
            string campaignUID = vocabularyStorage.CampaignVoc.Items.Where(t => t.CampaignID == campaignID).First().UID;

            foreach (var av in packageData.AdmissionVolumesToImport().Where(t => t.CampaignUID == campaignUID))
            {
                conflictStorage.SetObjectIsBroken(av, ConflictMessages.AdmissionVolumeCannotBeChangedWithCampaign, campaignUID);
            }
            foreach (var cg in packageData.CompetitiveGroupsToImport().Where(t => t.CampaignUID == campaignUID))
            {
                conflictStorage.SetObjectIsBroken(cg, ConflictMessages.CompetitiveGroupCannotBeRemovedWithCampaign, campaignUID);
            }
        }

        private void DebugMessage(Stopwatch sw, string message)
        {
            if (sw != null) sw.Stop();
            if (sw == null || sw.ElapsedMilliseconds > 1000)
            {
                message = string.Format("№ {0}, M: {1}, T: {2}", packageData.ImportPackageId.ToString(), message, sw != null ? sw.Elapsed.TotalSeconds.ToString() : "");
                Debug.WriteLine(message);
                LogHelper.Log.InfoFormat(message);
            }
            if (sw != null) sw.Restart();
        }


        #region Вспомогательные функции для проверки

        #region CheckAvailablaDirections
        /// <summary>
        /// Проверка ограничений по направлениям в КГ.
        /// </summary>
        /// <param name="competitiveGroup"></param>
        private void CheckAvailablaDirections(PackageDataAdmissionInfoCompetitiveGroup competitiveGroup)
        {
            List<string> notAllowedDirectionIDs = new List<string>();

            // КЦП и конкурсы могут проводить по конкретному Direction или по ParentDirection
            Tuple<int, int>[] allowedDirections;

            if (competitiveGroup.DirectionID != 0)
            {
                //разрешённые по вузу
                allowedDirections = vocabularyStorage.AllowedDirectionsVoc.Items
                    .Select(x => new Tuple<int, int>(x.AdmissionItemTypeID, x.DirectionID)).Distinct().ToArray();

                if (!allowedDirections.Any(x => x.Item1 == competitiveGroup.EducationLevelID.To(0) && x.Item2 == competitiveGroup.DirectionID.To(0)))
                    notAllowedDirectionIDs.Add(competitiveGroup.DirectionID.ToString());
            }
            else if (competitiveGroup.ParentDirectionID != 0)
            {
                allowedDirections = vocabularyStorage.AllowedDirectionsVoc.Items
                    .Select(x => new Tuple<int, int>(x.AdmissionItemTypeID, x.ParentDirectionID)).Distinct().ToArray();

                if (!allowedDirections.Any(x => x.Item1 == competitiveGroup.EducationLevelID.To(0) && x.Item2 == competitiveGroup.ParentDirectionID.To(0)))
                    notAllowedDirectionIDs.Add(competitiveGroup.ParentDirectionID.ToString());
            }
        

            if (notAllowedDirectionIDs.Count > 0)
            {
                string conflictMessage = ConflictMessages.GetMessage(ConflictMessages.CompetitiveGroupContainsNotAllowedByInstituteDirections);
                conflictMessage += String.Format(" Недопустимые направления с ID: {0} в КГ {1}",
                    String.Join(",", notAllowedDirectionIDs), competitiveGroup.UID);

                conflictStorage.SetObjectIsBroken(competitiveGroup, new ConflictStorage.ConflictMessage
                {
                    Code = ConflictMessages.CompetitiveGroupContainsNotAllowedByInstituteDirections,
                    Message = conflictMessage
                });
            }
            notAllowedDirectionIDs.Clear();
        }

        #endregion

        private void LogData(bool is_distributed, int dir_id, int? p_id, PackageDataAdmissionInfoItem dto, int[,] kcp)
        {
            aii_logger.Debug((is_distributed? "распределённый " : "") + "объём приёма");
            aii_logger.Debug("+----+-------------+---------+------+------+------+------+------+------+------+------+------+------+------+------+");
            aii_logger.Debug("| Ис | Направление | У.напр. | БОФ  | БОЗФ | БЗФ  | ПОФ  | ПОЗФ | ПЗФ  |  КО  | КОЗ  |  КЗ  |  ЦО  | ЦОЗ  |  ЦЗ  |");
            aii_logger.Debug("+----+-------------+---------+------+------+------+------+------+------+------+------+------+------+------+------+");
            aii_logger.DebugFormat("| П  | {0,11:G} | {1,7} | {2,4} | {3,4} | {4,4} | {5,4} | {6,4} | {7,4} | {8,4} | {9,4} | {10,4} | {11,4} | {12,4} | {13,4} |",
                                    dir_id, (p_id == null ? -1 : p_id), dto.NumberBudgetO, dto.NumberBudgetOZ, dto.NumberBudgetZ,
                                    dto.NumberPaidO, dto.NumberPaidOZ, dto.NumberPaidZ, dto.NumberQuotaO, dto.NumberQuotaOZ, dto.NumberQuotaZ,
                                    dto.NumberTargetO, dto.NumberTargetOZ, dto.NumberTargetZ);
            aii_logger.DebugFormat("| БД |             |         | {0,4} | {1,4} | {2,4} | {3,4} | {4,4} | {5,4} | {6,4} | {7,4} | {8,4} | {9,4} | {10,4} | {11,4} |",
                                    kcp[0, 0], kcp[0, 1], kcp[0, 2], kcp[1, 0], kcp[1, 1], kcp[1, 2], kcp[2, 0], kcp[2, 1], kcp[2, 2], kcp[3, 0], kcp[3, 1], kcp[3, 2]);
            aii_logger.Debug("+----+-------------+---------+------+------+------+------+------+------+------+------+------+------+------+------+");
            return;
        }

        private void LogData1(bool is_distributed, int dir_id, int? p_id, PackageDataAdmissionInfoItem1 dto, int[,] kcp)
        {
            aii_logger.Debug((is_distributed ? "распределённый " : "") + "объём приёма");
            aii_logger.Debug("+----+-------------+---------+------+------+------+------+------+------+------+------+------+------+------+------+");
            aii_logger.Debug("| Ис | Направление | У.напр. | БОФ  | БОЗФ | БЗФ  | ПОФ  | ПОЗФ | ПЗФ  |  КО  | КОЗ  |  КЗ  |  ЦО  | ЦОЗ  |  ЦЗ  |");
            aii_logger.Debug("+----+-------------+---------+------+------+------+------+------+------+------+------+------+------+------+------+");
            aii_logger.DebugFormat("| П  | {0,11:G} | {1,7} | {2,4} | {3,4} | {4,4} | {5,4} | {6,4} | {7,4} | {8,4} | {9,4} | {10,4} | {11,4} | {12,4} | {13,4} |",
                                    dir_id, (p_id == null ? -1 : p_id), dto.NumberBudgetO, dto.NumberBudgetOZ, dto.NumberBudgetZ,
                                    -1, -1, -1, dto.NumberQuotaO, dto.NumberQuotaOZ, dto.NumberQuotaZ,
                                    dto.NumberTargetO, dto.NumberTargetOZ, dto.NumberTargetZ);
            aii_logger.DebugFormat("| БД |             |         | {0,4} | {1,4} | {2,4} | {3,4} | {4,4} | {5,4} | {6,4} | {7,4} | {8,4} | {9,4} | {10,4} | {11,4} |",
                                    kcp[0, 0], kcp[0, 1], kcp[0, 2], kcp[1, 0], kcp[1, 1], kcp[1, 2], kcp[2, 0], kcp[2, 1], kcp[2, 2], kcp[3, 0], kcp[3, 1], kcp[3, 2]);
            aii_logger.Debug("+----+-------------+---------+------+------+------+------+------+------+------+------+------+------+------+------+");
            return;
        }

        /// <summary>
        /// Проверяем допустимый объём приёма: что числа в AdmissionVolume не меньше сумм по Конкурсам
        /// </summary>
        /// <param name="dto"></param>
        private void CheckAllowedPlacesForAdmissionVolumeAndCompetitiveGroups(PackageDataAdmissionInfoItem dtoAV, PackageDataAdmissionInfoItem1 dtoDAV, PackageDataAdmissionInfoCompetitiveGroup dtoCG)
        {
            int[,] kcp = new int[4, 3];
            int? AdmissionVolumeID = null;

            CampaignVocDto dbCampaign = vocabularyStorage.CampaignVoc.Items.Where(t => t.UID == (dtoAV != null ? dtoAV.CampaignUID : dtoCG.CampaignUID)).FirstOrDefault();
            if (dbCampaign == null)
            {
                DebugMessage(null, "LOGICAL ERROR: CheckAllowedPlacesForAdmissionVolumeAndCompetitiveGroups dbCampaign == null!");
                return;
            }

            var educationLevelID = (dtoAV != null) ? dtoAV.EducationLevelID.To(0) : dtoCG.EducationLevelID.To(0);
            var directionID = (dtoAV != null) ? dtoAV.DirectionID : dtoCG.DirectionID.To(0);
            var parentDirectionID = (dtoAV != null) ? dtoAV.ParentDirectionID : dtoCG.ParentDirectionID.To(0);

            var educationSourceID = (dtoCG != null) ? dtoCG.EducationSourceID : 0;
            var educationFormID = (dtoCG != null) ? dtoCG.EducationFormID : 0;

            var levelBudget = (dtoCG != null) ? dtoCG.LevelBudget : 0;

            var competitiveGroups = vocabularyStorage.CompetitiveGroupVoc.Items.Where(t => t.CampaignID == dbCampaign.CampaignID
                                                                            && t.EducationLevelID == educationLevelID
                                                                            && t.DirectionID == directionID
                                                                            && t.ParentDirectionID == parentDirectionID
                                                                            && (levelBudget == 0 || t.IdLevelBudget == levelBudget)
                                                                            && (educationFormID == 0 || t.EducationFormId == educationFormID)
                                                                            && (educationSourceID == 0 || t.EducationSourceId == educationSourceID)
                                                                            );

            var competitiveGroupItems = vocabularyStorage.CompetitiveGroupItemVoc.Items.Where(t => competitiveGroups.Any(x => x.CompetitiveGroupID == t.CompetitiveGroupID)).ToList();
            aii_logger.DebugFormat("Конкурсных групп в словаре: {0}...", competitiveGroupItems.Count);

            var cgTargetItems = new List<CompetitiveGroupTargetItemVocDto>();
            //vocabularyStorage.CompetitiveGroupTargetItemVoc.Items.Where(t=> competitiveGroups.Any(x => x.CompetitiveGroupID == t.CompetitiveGroupID)).ToList();

            // Проверить, что в пакете есть такие же Конкурсы на замену                                                               );
            if (packageData.AdmissionInfo.CompetitiveGroups != null)
            {
                int i = 0;
                foreach (var cg in packageData.AdmissionInfo.CompetitiveGroups.Where(t => !t.IsBroken
                                                                            && t.CampaignUID == dbCampaign.UID
                                                                            && t.EducationLevelID == educationLevelID
                                                                            && t.DirectionID == directionID
                                                                            && t.ParentDirectionID == parentDirectionID
                                                                            && (educationFormID == 0 || t.EducationFormID == educationFormID)
                                                                            && (educationSourceID == 0 || t.EducationSourceID == educationSourceID)
                                                                            ))
                {
                    var cgChange = competitiveGroups.FirstOrDefault(t => t.UID == cg.UID);
                    if (cgChange != null && cg.CompetitiveGroupItem != null)
                    {
                        competitiveGroupItems = competitiveGroupItems.Where(t => t.CompetitiveGroupID != cgChange.CompetitiveGroupID).ToList();
                    }

                    if (cg.CompetitiveGroupItem != null)
                    {
                        competitiveGroupItems.Add(new CompetitiveGroupItemVocDto()
                        {
                            CompetitiveGroupID = 0,
                            CompetitiveGroupUID = cg.UID,
                            IsDeleted = false,
                            NumberBudgetO = (cg.EducationSourceID == EDSourceConst.Budget && cg.EducationFormID == EDFormsConst.O) ? cg.CompetitiveGroupItem.Item.To(0) : 0,
                            NumberBudgetOZ = (cg.EducationSourceID == EDSourceConst.Budget && cg.EducationFormID == EDFormsConst.OZ) ? cg.CompetitiveGroupItem.Item.To(0) : 0,
                            NumberBudgetZ = (cg.EducationSourceID == EDSourceConst.Budget && cg.EducationFormID == EDFormsConst.Z) ? cg.CompetitiveGroupItem.Item.To(0) : 0,
                            NumberPaidO = (cg.EducationSourceID == EDSourceConst.Paid && cg.EducationFormID == EDFormsConst.O) ? cg.CompetitiveGroupItem.Item.To(0) : 0,
                            NumberPaidOZ = (cg.EducationSourceID == EDSourceConst.Paid && cg.EducationFormID == EDFormsConst.OZ) ? cg.CompetitiveGroupItem.Item.To(0) : 0,
                            NumberPaidZ = (cg.EducationSourceID == EDSourceConst.Paid && cg.EducationFormID == EDFormsConst.Z) ? cg.CompetitiveGroupItem.Item.To(0) : 0,
                            NumberQuotaO = (cg.EducationSourceID == EDSourceConst.Quota && cg.EducationFormID == EDFormsConst.O) ? cg.CompetitiveGroupItem.Item.To(0) : 0,
                            NumberQuotaOZ = (cg.EducationSourceID == EDSourceConst.Quota && cg.EducationFormID == EDFormsConst.OZ) ? cg.CompetitiveGroupItem.Item.To(0) : 0,
                            NumberQuotaZ = (cg.EducationSourceID == EDSourceConst.Quota && cg.EducationFormID == EDFormsConst.Z) ? cg.CompetitiveGroupItem.Item.To(0) : 0,
                            NumberTargetO = (cg.EducationSourceID == EDSourceConst.Target && cg.EducationFormID == EDFormsConst.O) ? cg.CompetitiveGroupItem.Item.To(0) : 0,
                            NumberTargetOZ = (cg.EducationSourceID == EDSourceConst.Target && cg.EducationFormID == EDFormsConst.OZ) ? cg.CompetitiveGroupItem.Item.To(0) : 0,
                            NumberTargetZ = (cg.EducationSourceID == EDSourceConst.Target && cg.EducationFormID == EDFormsConst.Z) ? cg.CompetitiveGroupItem.Item.To(0) : 0,
                        });
                        i++;
                    }

                    //if (cgChange != null && cg.TargetOrganizations != null)
                    //{
                    //    cgTargetItems = cgTargetItems.Where(t => t.CompetitiveGroupID != cgChange.CompetitiveGroupID).ToList();
                    //}

                    //if (cg.TargetOrganizations != null)
                    //    foreach (var to in cg.TargetOrganizations)
                    //    {

                    //        cgTargetItems.Add(new CompetitiveGroupTargetItemVocDto()
                    //        {
                    //            CompetitiveGroupID = 0,
                    //            IsDeleted = false,
                    //            NumberTargetO = (cg.EducationSourceID == EDSourceConst.Target && cg.EducationFormID == EDFormsConst.O) ? to.CompetitiveGroupTargetItem.Item.To(0) : 0,
                    //            NumberTargetOZ = (cg.EducationSourceID == EDSourceConst.Target && cg.EducationFormID == EDFormsConst.OZ) ? to.CompetitiveGroupTargetItem.Item.To(0) : 0,
                    //            NumberTargetZ = (cg.EducationSourceID == EDSourceConst.Target && cg.EducationFormID == EDFormsConst.Z) ? to.CompetitiveGroupTargetItem.Item.To(0) : 0,
                    //        });
                    //    }
                }
                aii_logger.DebugFormat("Конкурсных групп добавлено: {0}...", i);
            }

            // А теперь проверить по каждой Source * Form, что суммы не превышают
            foreach (var cgi in competitiveGroupItems)
            {
                kcp[0, 0] += cgi.NumberBudgetO;
                kcp[0, 1] += cgi.NumberBudgetOZ;
                kcp[0, 2] += cgi.NumberBudgetZ;

                kcp[1, 0] += cgi.NumberPaidO;
                kcp[1, 1] += cgi.NumberPaidOZ;
                kcp[1, 2] += cgi.NumberPaidZ;

                kcp[2, 0] += cgi.NumberQuotaO;
                kcp[2, 1] += cgi.NumberQuotaOZ;
                kcp[2, 2] += cgi.NumberQuotaZ;

                kcp[3, 0] += cgi.NumberTargetO;
                kcp[3, 1] += cgi.NumberTargetOZ;
                kcp[3, 2] += cgi.NumberTargetZ;
            }
            /*foreach (var cgti in cgTargetItems)
            {
                kcp[3, 0] += cgti.NumberTargetO;
                kcp[3, 1] += cgti.NumberTargetOZ;
                kcp[3, 2] += cgti.NumberTargetZ;
            }*/
            int dto_src = 0;
            PackageDataAdmissionInfoItem dto = null;
            if (dtoAV != null && !dtoAV.IsBroken)
            {

                dto = dtoAV;
                dto_src = 1;
            }
            // IsPlan: не учитывать IsPlan = true (план. значения)
            else if (packageData.AdmissionInfo.AdmissionVolume != null && packageData.AdmissionInfo.AdmissionVolume.Any(t => t.CampaignID == dbCampaign.CampaignID
                                                                                                                        && t.DirectionID == directionID
                                                                                                                        && t.ParentDirectionID == (parentDirectionID.HasValue ? parentDirectionID.Value : 0)
                                                                                                                        && t.EducationLevelID == educationLevelID
                                                                                                                        && !t.IsPlan))
            {
                dto_src = 2;
                dto = packageData.AdmissionInfo.AdmissionVolume.FirstOrDefault(t => t.CampaignID == dbCampaign.CampaignID
                                                                                && t.DirectionID == directionID
                                                                                && t.ParentDirectionID == parentDirectionID
                                                                                && t.EducationLevelID == educationLevelID
                                                                                && !t.IsPlan);
            }
            else
            {
                dto_src = 3;
                dto = new PackageDataAdmissionInfoItem { };

                AdmissionVolumeVocDto dbAV = vocabularyStorage.AdmissionVolumeVoc.Items.Where(t => t.CampaignID == dbCampaign.CampaignID &&
                                                                                              t.AdmissionItemTypeID == educationLevelID &&
                                                                                              t.DirectionID == directionID &&
                                                                                              t.ParentDirectionID == (parentDirectionID.HasValue ? parentDirectionID.Value : 0)).FirstOrDefault();
                //AdmissionVolumeID = dbAV.AdmissionVolumeID;
                if (dbAV != null)
                {
                    AdmissionVolumeID = dbAV.AdmissionVolumeID;
                    //dto.AdmissionVolumeID = dbAV.AdmissionVolumeID;
                    dto.UID = dbAV.UID;
                    dto.NumberBudgetO = (uint)dbAV.NumberBudgetO;
                    dto.NumberBudgetOZ = (uint)dbAV.NumberBudgetOZ;
                    dto.NumberBudgetZ = (uint)dbAV.NumberBudgetZ;

                    dto.NumberPaidO = (uint)dbAV.NumberPaidO;
                    dto.NumberPaidOZ = (uint)dbAV.NumberPaidOZ;
                    dto.NumberPaidZ = (uint)dbAV.NumberPaidZ;

                    dto.NumberQuotaO = (uint)dbAV.NumberQuotaO;
                    dto.NumberQuotaOZ = (uint)dbAV.NumberQuotaOZ;
                    dto.NumberQuotaZ = (uint)dbAV.NumberQuotaZ;

                    dto.NumberTargetO = (uint)dbAV.NumberTargetO;
                    dto.NumberTargetOZ = (uint)dbAV.NumberTargetOZ;
                    dto.NumberTargetZ = (uint)dbAV.NumberTargetZ;
                }
                else
                {
                    // если ничего не найдено, проверяем направления по УГС
                    var pids = vocabularyStorage.AdmissionVolumeVoc.Items.Where(t => t.CampaignID == dbCampaign.CampaignID &&
                                                                                    t.AdmissionItemTypeID == educationLevelID &&
                                                                                    t.ParentDirectionID > 0).Select(a => a.ParentDirectionID).ToList();
  //                  JOIN AllowedDirections AS ad
  //ON ad.DirectionID = d.DirectionID AND
  //   ad.InstitutionID = av.InstitutionID AND
  //   ad.AdmissionItemTypeID = av.AdmissionItemTypeID

                    var vsl = vocabularyStorage.AllowedDirectionsVoc.Items.Where(a => pids.Contains(a.ParentDirectionID) && a.AdmissionItemTypeID == educationLevelID && a.DirectionID == directionID).Select(a=> new { DirectionID = a.DirectionID, ParentDirectionID = a.ParentDirectionID }).FirstOrDefault();
                    //foreach (var item in vsl)
                    //{
                    //    aii_logger.DebugFormat("Направление {0} ({1})", item.DirectionID, item.ParentDirectionID);
                    //}
                    if (vsl != null)
                    {
                        dbAV = vocabularyStorage.AdmissionVolumeVoc.Items.Where(t => t.CampaignID == dbCampaign.CampaignID &&
                                                                                  t.AdmissionItemTypeID == educationLevelID &&
                                                                                  t.ParentDirectionID == vsl.ParentDirectionID).FirstOrDefault();
                        dto_src = 4;
                    }
                    else
                        dto_src = 5;
                }
                if (dbAV != null)
                {
                    AdmissionVolumeID = dbAV.AdmissionVolumeID;
                    //dto.AdmissionVolumeID = dbAV.AdmissionVolumeID;
                    dto.UID = dbAV.UID;
                    dto.NumberBudgetO = (uint)dbAV.NumberBudgetO;
                    dto.NumberBudgetOZ = (uint)dbAV.NumberBudgetOZ;
                    dto.NumberBudgetZ = (uint)dbAV.NumberBudgetZ;

                    dto.NumberPaidO = (uint)dbAV.NumberPaidO;
                    dto.NumberPaidOZ = (uint)dbAV.NumberPaidOZ;
                    dto.NumberPaidZ = (uint)dbAV.NumberPaidZ;

                    dto.NumberQuotaO = (uint)dbAV.NumberQuotaO;
                    dto.NumberQuotaOZ = (uint)dbAV.NumberQuotaOZ;
                    dto.NumberQuotaZ = (uint)dbAV.NumberQuotaZ;

                    dto.NumberTargetO = (uint)dbAV.NumberTargetO;
                    dto.NumberTargetOZ = (uint)dbAV.NumberTargetOZ;
                    dto.NumberTargetZ = (uint)dbAV.NumberTargetZ;
                }
            }



            //if (dto.NumberBudgetO < kcp[0,0] || dto.NumberBudgetOZ < kcp[0, 1] || dto.NumberBudgetZ < kcp[0, 2] ||
            //    dto.NumberPaidO < kcp[1, 0] || dto.NumberPaidOZ < kcp[1, 1] || dto.NumberPaidZ < kcp[1, 2] ||
            //    dto.NumberQuotaO < kcp[2, 0] || dto.NumberQuotaOZ < kcp[2, 1] || dto.NumberQuotaZ < kcp[2, 2] ||
            //    dto.NumberTargetO < kcp[3, 0] || dto.NumberTargetOZ < kcp[3, 1] || dto.NumberTargetZ < kcp[3, 2]
            //    )
            //{
            //    // ошибка превышения К-Ц-П (которое теперь "количество мест")
            //    if (dtoAV != null)
            //        conflictStorage.SetObjectIsBroken(dtoAV, ConflictMessages.CompetitiveGroupPlacesOnDirectionExceeded);
            //    if (dtoCG != null)
            //        conflictStorage.SetObjectIsBroken(dtoCG, ConflictMessages.CompetitiveGroupPlacesOnDirectionExceeded);
            //}
            aii_logger.DebugFormat("Проверка {0} объёма приёма (кампания: {1}, направление: {2}[{3}])...", dto_src, dbCampaign.CampaignID, directionID, parentDirectionID);

            string errorSource = string.Empty;
            string errorForm = string.Empty;
            int valueAV = 0;
            int valueCG = 0;

            if (dto.NumberBudgetO < kcp[0, 0])
            {
                errorSource = "Бюджетные места, 14";
                errorForm = "Очная, 11";
                valueAV = (int)dto.NumberBudgetO;
                valueCG = kcp[0, 0];
            }
            if (dto.NumberBudgetOZ < kcp[0, 1]) { errorSource = "Бюджетные места, 14"; errorForm = "Очно-заочная, 12"; valueAV = (int)dto.NumberBudgetOZ; valueCG = kcp[0, 1]; }
            if (dto.NumberBudgetZ < kcp[0, 2]) { errorSource = "Бюджетные места, 14"; errorForm = "Заочная, 10"; valueAV = (int)dto.NumberBudgetZ; valueCG = kcp[0, 2]; }

            if (dto.NumberPaidO < kcp[1, 0]) { errorSource = "С оплатой обучения, 15"; errorForm = "Очная, 11"; valueAV = (int)dto.NumberPaidO; valueCG = kcp[1, 0]; }
            if (dto.NumberPaidOZ < kcp[1, 1]) { errorSource = "С оплатой обучения, 15"; errorForm = "Очно-заочная, 12"; valueAV = (int)dto.NumberPaidOZ; valueCG = kcp[1, 1]; }
            if (dto.NumberPaidZ < kcp[1, 2]) { errorSource = "С оплатой обучения, 15"; errorForm = "Заочная, 10"; valueAV = (int)dto.NumberPaidZ; valueCG = kcp[1, 2]; }

            if (dto.NumberQuotaO < kcp[2, 0]) { errorSource = "Квота приема лиц, имеющих особое право, 20"; errorForm = "Очная, 11"; valueAV = (int)dto.NumberQuotaO; valueCG = kcp[2, 0]; }
            if (dto.NumberQuotaOZ < kcp[2, 1]) { errorSource = "Квота приема лиц, имеющих особое право, 20"; errorForm = "Очно-заочная, 12"; valueAV = (int)dto.NumberQuotaOZ; valueCG = kcp[2, 1]; }
            if (dto.NumberQuotaZ < kcp[2, 2]) { errorSource = "Квота приема лиц, имеющих особое право, 20"; errorForm = "Заочная, 10"; valueAV = (int)dto.NumberQuotaZ; valueCG = kcp[2, 2]; }

            if (dto.NumberTargetO < kcp[3, 0]) { errorSource = "Целевой прием, 16"; errorForm = "Очная, 11"; valueAV = (int)dto.NumberTargetO; valueCG = kcp[3, 0]; }
            if (dto.NumberTargetOZ < kcp[3, 1]) { errorSource = "Целевой прием, 16"; errorForm = "Очно-заочная, 12"; valueAV = (int)dto.NumberTargetOZ; valueCG = kcp[3, 1]; }
            if (dto.NumberTargetZ < kcp[3, 2]) { errorSource = "Целевой прием, 16"; errorForm = "Заочная, 10"; valueAV = (int)dto.NumberTargetZ; valueCG = kcp[3, 2]; }

            int message_code = 0;
            List<string> prms = null;

            if (!string.IsNullOrEmpty(errorSource))
            {
                this.LogData(false, directionID, parentDirectionID, dto, kcp);

                //    /*
                //        1) Объем приема - "Количество мест по направлению (DirID) в разрезе источника финансирования ("такой то источник", itemID) 
                //        и формы обучения ("такая то форма", itemID) меньше суммарного количества мест в конкурсах (x < y)"

                //        2) Конкурсы - "Количество мест по направлению (DirID) в разрезе источника финансирования ("такой то источник", itemID) 
                //        и формы обучения ("такая то форма", itemID) больше количества мест в объеме приема (x > y)"
                //    */
                if (dtoAV != null)
                {
                    int d = 0;
                    if (dtoAV.ParentDirectionID == null)
                        d = dtoAV.DirectionID;
                    else
                        d = dtoAV.DirectionID;
                    
                    aii_logger.DebugFormat("КГ н: {0}, рн: {1}", dtoAV.DirectionID, dtoAV.ParentDirectionID);
                    message_code = ConflictMessages.CompetitiveGroupPlacesOnDirectionExceeded;
                    prms = new List<string>() { d.ToString(),
                                                    errorSource,
                                                    errorForm,
                                                    string.Format("меньше суммарного количества мест в конкурсах ({0})", valueCG),
                                                    valueAV.ToString()};
                    conflictStorage.SetObjectIsBroken(dtoAV, message_code, prms.ToArray());
                    aii_logger.ErrorFormat("Ошибка ОП №{0}: {1}", message_code, string.Format(ConflictMessages.GetMessage(message_code), prms.ToArray()));
                }
                if (dtoCG != null)
                {
                    uint d = 0;
                    if (dtoCG.DirectionID == 0)
                        d = dtoCG.ParentDirectionID;
                    else
                        d = dtoCG.DirectionID;
                    //string dir_id = string.Format("{0}", dtoCG.DirectionID == 0 ? dtoCG.ParentDirectionID : dtoCG.DirectionID);
                    aii_logger.ErrorFormat("КГ н: {0}, рн: {1}", dtoCG.DirectionID, dtoCG.ParentDirectionID);
                    message_code = ConflictMessages.CompetitiveGroupPlacesOnDirectionExceeded;
                    prms = new List<string>() { d.ToString(),
                                                errorSource,
                                                errorForm,
                                                string.Format("больше количества мест в объеме приема ({0})", valueAV),
                                                valueCG.ToString()};
                    conflictStorage.SetObjectIsBroken(dtoCG, message_code, prms.ToArray());
                    aii_logger.ErrorFormat("Ошибка КГ №{0}: {1}", message_code, string.Format(ConflictMessages.GetMessage(message_code), prms.ToArray()));
                }
            }

            //FIS - 1790 - такая же проверка по распределенному объему
            #region davCheck
            //var levelBudget = (dtoCG != null) ? dtoCG.LevelBudget : 0;
            if (levelBudget != 0)
            {
                LogHelper.Log.Debug("Проверка распределённого объёма приёма...");
                PackageDataAdmissionInfoItem1 dto1 = null;

                bool dav_found = false;
                if (dtoDAV != null && !dtoDAV.IsBroken)
                {
                    dto1 = dtoDAV;
                    aii_logger.DebugFormat("dto1 = dtoDAV");
                    dav_found = true;
                }
                // IsPlan - учитывать только false (факт. значения)
                else if (packageData.AdmissionInfo.DistributedAdmissionVolume != null &&
                         packageData.AdmissionInfo.DistributedAdmissionVolume.Any(t => t.AdmissionVolumeUID == dto.UID && t.LevelBudget == levelBudget && !t.IsPlan))
                //&& t.DirectionID == directionID
                //&& t.EducationLevelID == educationLevelID))
                {
                    dto1 = packageData.AdmissionInfo.DistributedAdmissionVolume.FirstOrDefault(t => t.AdmissionVolumeUID == dto.UID && t.LevelBudget == levelBudget && !t.IsPlan);
                    aii_logger.DebugFormat("dto1 = распределённому объёму приёма");
                    dav_found = true;
                }
                else
                {
                    aii_logger.DebugFormat("dto1... попытка найти...");
                    //если еще нет AdmissionVolumeID пытаемся его найти!
                    //на случай если пользователь передал AV, a DAV внес вручную без UID
                    var admissionVolume = vocabularyStorage.AdmissionVolumeVoc.Items
                                              .FirstOrDefault(t => t.UID == dto.UID);

                    if (admissionVolume != null && AdmissionVolumeID == null)
                        AdmissionVolumeID = admissionVolume.AdmissionVolumeID;

                    //если есть AV.AdmissionVolumeID (значит AV пришел из базы, а там UID может и не быть) определяем DAV по нему
                    //если нет AdmissionVolumeID (значит AV пришел из пакета) определяем по UID
                    DistributedAdmissionVolumeVocDto dbDAV = null;

                    if (AdmissionVolumeID != null)
                    {
                        dbDAV = vocabularyStorage.DistributedAdmissionVolumeVoc.Items.FirstOrDefault(t =>
                        (t.AdmissionVolumeID == AdmissionVolumeID && t.IdLevelBudget == levelBudget));
                        aii_logger.DebugFormat("Поиск по AdmissionVolumeID = {0}", AdmissionVolumeID);
                    }
                    else if (AdmissionVolumeID == null)
                    {
                        dbDAV = vocabularyStorage.DistributedAdmissionVolumeVoc.Items.FirstOrDefault(t =>
                        (t.AdmissionVolumeUID == dto.UID && t.IdLevelBudget == levelBudget));
                        aii_logger.DebugFormat("Поиск по UID = {0} и IdLevelBudget = {1}", dto.UID, levelBudget);
                    }

                    dto1 = new PackageDataAdmissionInfoItem1 { };
                    if (dbDAV != null)
                    {
                        dto1.NumberBudgetO = (uint)dbDAV.NumberBudgetO;
                        dto1.NumberBudgetOZ = (uint)dbDAV.NumberBudgetOZ;
                        dto1.NumberBudgetZ = (uint)dbDAV.NumberBudgetZ;

                        //dto.NumberPaidO = (uint)dbAV.NumberPaidO;
                        //dto.NumberPaidOZ = (uint)dbAV.NumberPaidOZ;
                        //dto.NumberPaidZ = (uint)dbAV.NumberPaidZ;

                        dto1.NumberQuotaO = (uint)dbDAV.NumberQuotaO;
                        dto1.NumberQuotaOZ = (uint)dbDAV.NumberQuotaOZ;
                        dto1.NumberQuotaZ = (uint)dbDAV.NumberQuotaZ;

                        dto1.NumberTargetO = (uint)dbDAV.NumberTargetO;
                        dto1.NumberTargetOZ = (uint)dbDAV.NumberTargetOZ;
                        dto1.NumberTargetZ = (uint)dbDAV.NumberTargetZ;
                        dav_found = true;
                    }
                    else
                    {
                        aii_logger.WarnFormat("Распределённый объём приёма не найден!");
                        dav_found = false;
                    }
                }

                //string errorSource = string.Empty;
                //string errorForm = string.Empty;
                int valueDAV = 0;
                //int valueCG = 0;
                if (dav_found)
                {
                    if (dto1.NumberBudgetO < kcp[0, 0]) { errorSource = "Бюджетные места, 14"; errorForm = "Очная, 11"; valueDAV = (int)dto1.NumberBudgetO; valueCG = kcp[0, 0]; }
                    if (dto1.NumberBudgetOZ < kcp[0, 1]) { errorSource = "Бюджетные места, 14"; errorForm = "Очно-заочная, 12"; valueDAV = (int)dto1.NumberBudgetOZ; valueCG = kcp[0, 1]; }
                    if (dto1.NumberBudgetZ < kcp[0, 2]) { errorSource = "Бюджетные места, 14"; errorForm = "Заочная, 10"; valueDAV = (int)dto1.NumberBudgetZ; valueCG = kcp[0, 2]; }

                    //if (dto1.NumberPaidO < kcp[1, 0]) { errorSource = "С оплатой обучения, 15"; errorForm = "Очная, 11"; valueDAV = (int)dto1.NumberPaidO; valueCG = kcp[1, 0]; }
                    //if (dto1.NumberPaidOZ < kcp[1, 1]) { errorSource = "С оплатой обучения, 15"; errorForm = "Очно-заочная, 12"; valueDAV = (int)dto1.NumberPaidOZ; valueCG = kcp[1, 1]; }
                    //if (dto1.NumberPaidZ < kcp[1, 2]) { errorSource = "С оплатой обучения, 15"; errorForm = "Заочная, 10"; valueDAV = (int)dto1.NumberPaidZ; valueCG = kcp[1, 2]; }

                    if (dto1.NumberQuotaO < kcp[2, 0]) { errorSource = "Квота приема лиц, имеющих особое право, 20"; errorForm = "Очная, 11"; valueDAV = (int)dto1.NumberQuotaO; valueCG = kcp[2, 0]; }
                    if (dto1.NumberQuotaOZ < kcp[2, 1]) { errorSource = "Квота приема лиц, имеющих особое право, 20"; errorForm = "Очно-заочная, 12"; valueDAV = (int)dto1.NumberQuotaOZ; valueCG = kcp[2, 1]; }
                    if (dto1.NumberQuotaZ < kcp[2, 2]) { errorSource = "Квота приема лиц, имеющих особое право, 20"; errorForm = "Заочная, 10"; valueDAV = (int)dto1.NumberQuotaZ; valueCG = kcp[2, 2]; }

                    if (dto1.NumberTargetO < kcp[3, 0]) { errorSource = "Целевой прием, 16"; errorForm = "Очная, 11"; valueDAV = (int)dto1.NumberTargetO; valueCG = kcp[3, 0]; }
                    if (dto1.NumberTargetOZ < kcp[3, 1]) { errorSource = "Целевой прием, 16"; errorForm = "Очно-заочная, 12"; valueDAV = (int)dto1.NumberTargetOZ; valueCG = kcp[3, 1]; }
                    if (dto1.NumberTargetZ < kcp[3, 2]) { errorSource = "Целевой прием, 16"; errorForm = "Заочная, 10"; valueDAV = (int)dto1.NumberTargetZ; valueCG = kcp[3, 2]; }

                    if (!string.IsNullOrEmpty(errorSource))
                    {
                        // TODO сделать универсальный лог
                        this.LogData1(true, directionID, parentDirectionID, dto1, kcp);

                        //    /*
                        //        1) Объем приема - "Количество мест по направлению (DirID) в разрезе источника финансирования ("такой то источник", itemID) 
                        //        и формы обучения ("такая то форма", itemID) меньше суммарного количества мест в конкурсах (x < y)"

                        //        2) Конкурсы - "Количество мест по направлению (DirID) в разрезе источника финансирования ("такой то источник", itemID) 
                        //        и формы обучения ("такая то форма", itemID) больше количества мест в объеме приема (x > y)"
                        //    */
                        if (dtoDAV != null)
                        {
                            message_code = ConflictMessages.CompetitiveGroupPlacesOnDirectionExceeded;
                            prms = new List<string>() { String.IsNullOrEmpty(dtoAV.DirectionID.ToString()) ? dtoAV.DirectionID.ToString() : dtoAV.ParentDirectionID.ToString(),
                                                    errorSource,
                                                    errorForm,
                                                    string.Format("меньше суммарного количества мест в конкурсах ({0})", valueCG),
                                                    valueDAV.ToString()};
                            conflictStorage.SetObjectIsBroken(dtoDAV, message_code, prms.ToArray());
                            aii_logger.ErrorFormat("Ошибка №{0}: {1}", message_code, string.Format(ConflictMessages.GetMessage(message_code), prms.ToArray()));
                        }
                        if (dtoCG != null)
                        {
                            message_code = ConflictMessages.CompetitiveGroupPlacesOnDirectionExceeded;
                            prms = new List<string>() { String.IsNullOrEmpty(dtoCG.DirectionID.ToString()) ? dtoCG.DirectionID.ToString() : dtoCG.ParentDirectionID.ToString(),
                                                    errorSource,
                                                    errorForm,
                                                    string.Format("больше количества мест в распределенном объеме приема ({0})", valueDAV),
                                                    valueCG.ToString()};
                            conflictStorage.SetObjectIsBroken(dtoCG, message_code, prms.ToArray());
                            aii_logger.ErrorFormat("Ошибка №{0}: {1}", message_code, string.Format(ConflictMessages.GetMessage(message_code), prms.ToArray()));
                        }
                    }
                }
                else
                {

                }
            }
            #endregion

            // Проверка когда могут быть заданы квоты
            if (dtoAV != null && (dto.NumberQuotaO.To(0) > 0 || dto.NumberQuotaOZ.To(0) > 0 || dto.NumberQuotaZ.To(0) > 0))
            {
                LogHelper.Log.Debug("Проверка по квотам...");
                int[] allowedQuotaLevels = new int[] { EDLevelConst.Bachelor, EDLevelConst.Speciality };

                if (!allowedQuotaLevels.Contains(dtoAV.EducationLevelID.To(0)))
                {
                    conflictStorage.SetObjectIsBroken(dtoAV, ConflictMessages.CompetitiveGroupItemQuotaIncorrect);
                }
            }
        }
        #endregion

        protected override List<string> ImportDb()
        {
            List<Tuple<string, IDataReader>> bulks = new List<Tuple<string, IDataReader>>();

            bulks.Add(new Tuple<string, IDataReader>("bulk_AdmissionVolume", new BulkAdmissionVolumeReader(packageData)));
            bulks.Add(new Tuple<string, IDataReader>("bulk_DistributedAdmissionVolume", new BulkDistributedAdmissionVolumeReader(packageData)));
            //bulks.Add(new Tuple<string, IDataReader>("bulk_DistributedPlanAdmissionVolume", new BulkDistributedPlanAdmissionVolumeReader(packageData)));

            bulks.Add(new Tuple<string, IDataReader>("bulk_CompetitiveGroup", new BulkCompetitiveGroupReader(packageData)));
            bulks.Add(new Tuple<string, IDataReader>("bulk_CompetitiveGroupItem", new BulkCompetitiveGroupItemReader(packageData)));
            bulks.Add(new Tuple<string, IDataReader>("bulk_CompetitiveGroupProgram", new BulkCompetitiveGroupProgramReader(packageData)));
            //bulks.Add(new Tuple<string,IDataReader>("bulk_CompetitiveGroupTarget", new BulkCompetitiveGroupTargetReader(packageData)));
            bulks.Add(new Tuple<string, IDataReader>("bulk_CompetitiveGroupTargetItem", new BulkCompetitiveGroupTargetItemReader(packageData)));

            var commonBenefitReader = new BulkBenefitItemReader(packageData);
            bulks.Add(new Tuple<string, IDataReader>("bulk_BenefitItemC", commonBenefitReader));
            bulks.Add(new Tuple<string, IDataReader>("bulk_BenefitItemData", commonBenefitReader.BulkBenefitItemDataReader));

            bulks.Add(new Tuple<string, IDataReader>("bulk_EntranceTestItemC", new BulkEntranceTestItemReader(packageData)));
            var etBenefitReader = new BulkEntranceTestBenefitReader(packageData);
            bulks.Add(new Tuple<string, IDataReader>("bulk_BenefitItemC", etBenefitReader));
            bulks.Add(new Tuple<string, IDataReader>("bulk_BenefitItemData", etBenefitReader.BulkBenefitItemDataReader));

            // Delete items
            //var deleteReader = new BulkDeleteReader(packageData.ImportPackageId);
            //foreach(var dm in deleteItems.Item1)
            //    deleteReader.AddRange(dm.GetDeleteObjects());
            //foreach (var dm in deleteItems.Item2)
            //    deleteReader.AddRange(dm.GetDeleteObjects());
            //foreach (var dm in deleteItems.Item3)
            //    deleteReader.AddRange(dm.GetDeleteObjects());
            //foreach (var dm in deleteItems.Item4)
            //    deleteReader.AddRange(dm.GetDeleteObjects());
            //foreach (var dm in deleteItems.Item5)
            //    deleteReader.AddRange(dm.GetDeleteObjects());
            //foreach (var dm in deleteItems.Item6)
            //    deleteReader.AddRange(dm.GetDeleteObjects());
            //foreach (var dm in deleteItems.Item7)
            //    deleteReader.AddRange(dm.GetDeleteObjects());

            //bulks.Add(new Tuple<string, IDataReader>("bulk_Delete", deleteReader));


            // Должен прийти список импортированных кампаний - пополнить справочники и записать количество в successfulImportStatisticsDto 
            var res = ADOPackageRepository.BulkInsertData(packageData, bulks, "ImportAdmissionInfo", deleteBulk, aii_logger);
            DataSet dsResult = res.Item1;
            if (dsResult != null && dsResult.Tables.Count >= 10)
            {
                conflictStorage.successfulImportStatisticsDto.AdmissionVolumes = (dsResult.Tables[0].Rows.Count + dsResult.Tables[8].Rows.Count).ToString();
                conflictStorage.successfulImportStatisticsDto.DistributedAdmissionVolumes = (dsResult.Tables[1].Rows.Count + dsResult.Tables[9].Rows.Count).ToString();
                conflictStorage.successfulImportStatisticsDto.CompetitiveGroups = dsResult.Tables[2].Rows.Count.ToString();
                conflictStorage.successfulImportStatisticsDto.CompetitiveGroupItems = dsResult.Tables[3].Rows.Count.ToString();

                vocabularyStorage.AdmissionVolumeVoc.AddItems(dsResult.Tables[0]);
                vocabularyStorage.CompetitiveGroupVoc.AddItems(dsResult.Tables[2]);
                vocabularyStorage.CompetitiveGroupItemVoc.AddItems(dsResult.Tables[3]);
                vocabularyStorage.CompetitiveGroupTargetVoc.AddItems(dsResult.Tables[4]);
                vocabularyStorage.CompetitiveGroupTargetItemVoc.AddItems(dsResult.Tables[5]);
                vocabularyStorage.BenefitItemCVoc.AddItems(dsResult.Tables[6]);
                vocabularyStorage.EntranceTestItemCVoc.AddItems(dsResult.Tables[7]);

                return res.Item2;
            }
            else
                throw new Exception("Хранимая процедура ImportAdmissionInfo должна возвращать данные по 10 импортируемым таблицам!");
        }
    }
}
