using GVUZ.ImportService2016.Core.Main.Dictionaries;
using GVUZ.ImportService2016.Core.Main.Repositories;
using GVUZ.ImportService2016.Core.Dto.DataReaders;
using GVUZ.ImportService2016.Core.Dto.DataReaders.InstitutionArchievements;
using GVUZ.ServiceModel.Import.Core.Operations.Conflicts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Main.Import
{
    class InstitutionAchievementsImporter : BaseImporter
    {
        public InstitutionAchievementsImporter(GVUZ.ImportService2016.Core.Dto.Import.PackageData packageData, VocabularyStorage vocabularyStorage, ImportConflictStorage importConflictStorage, bool deleteBulk) : base(packageData, vocabularyStorage, importConflictStorage, deleteBulk) { }

        protected override void Validate()
        {
            if (packageData.InstitutionAchievements == null) return;

            // Проверка на дубли IAUID
            foreach (var group in packageData.InstitutionAchievements.GroupBy(t => t.InstitutionAchievementUID))
                if (group.Count() > 1)
                {
                    foreach (var item in group)
                    {
                        conflictStorage.SetObjectIsBroken(item, ConflictMessages.UIDMustBeUniqueInCollection);
                    }
                }

            // Проверка на то, что в пакете все дочерние элементы уникальны по комбинации полей IAUID, CampaignUID
            foreach (var group in packageData.InstitutionAchievements.GroupBy(t => t.Key()))
                if (group.Count() > 1)
                {
                    foreach (var item in group)
                    {
                        conflictStorage.SetObjectIsBroken(item, ConflictMessages.IAUIDAndCampaignUIDMustBeUniqueInCollection, item.InstitutionAchievementUID, item.CampaignUID);
                    }
                }

            //// Проверка наличия в текущем пакете ровно одного элемента "Root\PackageData\CampaignInfo\Campaigns\Campaign\UID" с указанным UID.
            //// Убрали решением от 2015-05-14
            //foreach (var group in _institutionArchievements.GroupBy(t => t.CampaignUID))
            //    if (group.Count() > 1)
            //    {
            //        foreach (var item in group)
            //        {
            //            conflictStorage.SetObjectIsBroken(item, ConflictMessages.CampaignUIDMustBeUniqueInCollection, item.CampaignUID);
            //        }
            //    }

            foreach (var ia in packageData.InstitutionAchievements)
            {
                // 1. UID - не пустой
                if (string.IsNullOrWhiteSpace(ia.InstitutionAchievementUID))
                {
                    conflictStorage.SetObjectIsBroken(ia, ConflictMessages.IAIncorrentFieldValue, "IAUID");
                    continue;
                }
                // 2. Name - не пустой
                if (string.IsNullOrWhiteSpace(ia.Name))
                {
                    conflictStorage.SetObjectIsBroken(ia, ConflictMessages.IAIncorrentFieldValue, "Name");
                    continue;
                }
                // 3. IdCategory - Проверка наличия элемента в справочнике №36 “Категории индивидуальных достижений” (IndividualAchievementsCategory)
                if (!VocabularyStatic.IndividualAchievementsCategoryVoc.Items.Where(t => t.IdCategory == ia.IdCategory).Any())
                {
                    conflictStorage.SetObjectIsBroken(ia, ConflictMessages.DictionaryItemAbsent, "IdCategory");
                    continue;
                }

                // 4. MaxValue - Проверка на непустоту и числовой формат (до 3 цифр перед запятой, до 4 знаков после запятой)
                string[] parts = ia.MaxValue.ToString().Split(new string[] { ",", "." }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Count() > 0 && parts[0].Length > 3)
                {
                    conflictStorage.SetObjectIsBroken(ia, ConflictMessages.IAIncorrentFieldValue, "MaxValue");
                    continue;
                }
                if (parts.Count() > 1 && parts[1].Length > 4)
                {
                    conflictStorage.SetObjectIsBroken(ia, ConflictMessages.IAIncorrentFieldValue, "MaxValue");
                    continue;
                }

                // 5. CampaingUID - Если в текущем пакете таких элементов нет, проверяется наличие в БД ровно одной записи в таблице “Приемные кампании” (Campaign), 
                // с указанным UID, и относящейся к тому же самому ОУ (принадлежность к ОУ определяется через поле CampaignID.InstitutionID). 
                // Если найдено более одного элемента или не найдено ни одного - ошибка. 
                // Убрал && t.StatusID == 1, потому что могут быть дозаполнения данных после 
                var dbCampaigns = vocabularyStorage.CampaignVoc.Items; //.Where(t=> t.YearStart == DateTime.Now.Year);
                var iaCampaign = vocabularyStorage.CampaignVoc.Items.Where(t => t.UID == ia.CampaignUID).FirstOrDefault();
                //_entrantEntities.Campaign.Where(t => t.UID == ia.CampaignUID && t.InstitutionID == InstitutionID && t.StatusID == 1).FirstOrDefault();

                if (iaCampaign != null)
                    ia.CampaignID = iaCampaign.CampaignID;
                else
                {
                    if (dbCampaigns.Count == 1 && string.IsNullOrWhiteSpace(ia.CampaignUID))
                    {
                        ia.CampaignUID = dbCampaigns[0].UID;
                        ia.CampaignID = dbCampaigns[0].CampaignID;
                    }
                    else
                    {
                        conflictStorage.SetObjectIsBroken(ia, ConflictMessages.DictionaryItemAbsent, "CampaignUID");
                    }
                }

                // Если такая запись уже есть в БД, то заполнить ее ID
                var dbIA = vocabularyStorage.InstitutionAchievementsVoc.Items.Where(t => t.UID == ia.InstitutionAchievementUID).FirstOrDefault();
                if (dbIA != null)
                    ia.ID = dbIA.ID;
            }
        }

        protected override List<string> ImportDb()
        {
            List<Tuple<string, IDataReader>> bulks = new List<Tuple<string, IDataReader>>();

            #region InstitutionArchievements
            bulks.Add(new Tuple<string, IDataReader>("bulk_InstitutionAchievements", new BulkInstitutionAchievementsReader(packageData)));
            #endregion InstitutionArchievements

            // Должен прийти список импортированных кампаний - пополнить справочники и записать количество в successfulImportStatisticsDto 
            var res = ADOPackageRepository.BulkInsertData(packageData, bulks, "ImportInstitutionAchievements", deleteBulk);
            DataSet dsResult = res.Item1;
            if (dsResult != null && dsResult.Tables.Count > 0)
            {
                conflictStorage.successfulImportStatisticsDto.InstitutionAchievements = dsResult.Tables[0].Rows.Count.ToString();

                vocabularyStorage.InstitutionAchievementsVoc.AddItems(dsResult.Tables[0]);
            }
            return res.Item2;
        }
    }
}
