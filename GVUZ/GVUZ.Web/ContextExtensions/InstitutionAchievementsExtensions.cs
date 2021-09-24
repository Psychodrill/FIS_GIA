using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using GVUZ.Model.Entrants;
using GVUZ.Web.ViewModels.InstitutionAchievements;
using System.Linq;
namespace GVUZ.Web.ContextExtensions
{
    public static class InstitutionAchievementsExtensions
    {
        public static IEnumerable<InstitutionAchievementCategory> LoadAchievementCategories(this EntrantsEntities db)
        {
            return
                db.IndividualAchievementsCategory.OrderBy(x => x.IdCategory).Select(
                    x => new InstitutionAchievementCategory {CategoryId = x.IdCategory, CategoryName = x.CategoryName});
        }

        public static IEnumerable<InstitutionAchievementCampaign> LoadAchievementCampaigns(this EntrantsEntities db,
                                                                                           int institutionId)
        {
            return
                db.Institution.Where(x => x.InstitutionID == institutionId)
                  .SelectMany(x => x.Campaign)
                  //.Where(x => x.StatusID != CampaignStatusType.Finished).OrderByDescending(x => x.YearStart)
                  .Select(x => new InstitutionAchievementCampaign {CampaignId = x.CampaignID, CampaignName = x.Name, IsFinished = (x.StatusID == CampaignStatusType.Finished) });
        }

        public static IEnumerable<InstitutionAchievementRecordViewModel> LoadInstitutionAchievementsForCampaign(
            this EntrantsEntities db, int institutionId, int campaignId, string sortKey = null, bool sortDesc = false)
        {
            Expression<Func<InstitutionAchievements, object>> sort = null;

            sortKey = sortKey ?? string.Empty;

            switch (sortKey.ToLowerInvariant())
            {
                case "uid":
                    sort = x => x.UID;
                    break;
                case "name":
                    sort = x => x.Name;
                    break;
                case "category":
                    sort = x => x.IndividualAchievementsCategory.CategoryName;
                    break;
                case "maxvalue":
                    sort = x => x.MaxValue;
                    break;
            }

            var query =
                db.InstitutionAchievements.Where(
                    x => x.CampaignID == campaignId && x.Campaign.InstitutionID == institutionId);

            if (sort != null)
            {
                query = sortDesc ? query.OrderByDescending(sort) : query.OrderBy(sort);
            }

            return 
              query.Select(x => new InstitutionAchievementRecordViewModel
                  {
                      Id = x.IdAchievement,
                      Name = x.Name,
                      MaxValue = x.MaxValue,
                      UID = x.UID,
                      CategoryId = x.IndividualAchievementsCategory != null ? x.IndividualAchievementsCategory.IdCategory : 0,
                      CategoryName = x.IndividualAchievementsCategory != null ? x.IndividualAchievementsCategory.CategoryName : null
                  });
        }

        public static InstitutionAchievements GetInstitutionAchievementById(this EntrantsEntities db, int institutionId, int achievementId)
        {
            return
                db.InstitutionAchievements.FirstOrDefault(
                    x => x.IdAchievement == achievementId && x.Campaign.InstitutionID == institutionId);
        }

        public static bool ValidateInstitutionAchievementUpdateRecordViewModel(this EntrantsEntities db, int institutionId, InstitutionAchievementUpdateRecordViewModel model, ModelStateDictionary errors)
        {
            //проверка на уникальность по полям - ОО, ПК, наименование ИД, категория ИД
            var nameDuplicates = db.InstitutionAchievements.Where(x => x.Name.Equals(
                model.Name, StringComparison.OrdinalIgnoreCase) 
                && x.IdAchievement != model.Id 
                && x.Campaign.InstitutionID == institutionId
                && x.Campaign.CampaignID == model.CampaignId
                && x.IdCategory == model.CategoryId
            );
            if (nameDuplicates.Any())
            {
                //errors.AddModelError("Name", "Значение в поле \"Наименование достижения\" должно быть уникальным среди всех индивидуальных достижений ОО!");
                errors.AddModelError("Name", "В приемной кампании уже существует индивидуальное достижение с такими категорией и наименованием!");
            }

            var uidDuplicates = db.InstitutionAchievements.Where(x => x.UID == model.UID && x.IdAchievement != model.Id && x.Campaign.InstitutionID == institutionId);
            if (uidDuplicates.Any())
            {
                errors.AddModelError("UID", "Значение в поле \"UID\" должно быть уникальным среди всех индивидуальных достижений ОО!");
            }
            return errors.IsValid;
        }

        public static bool DeleteInstitutionAchievementRecord(this EntrantsEntities db, int institutionId,
                                                              int institutionAchievementId, List<string> errors)
        {
            var entity = db.InstitutionAchievements.FirstOrDefault(
                    x => x.IdAchievement == institutionAchievementId && x.Campaign.InstitutionID == institutionId);

            if (entity == null)
            {
                errors.Add("Объект не найден, возможно он был удален другим пользователем!");
                return false;
            }

            // проверка связанных заявлений со статусом отличным от отозванного (StatusId != 6)
            if (
                db.IndividualAchivement.Any(
                    x =>
                    x.IdAchievement == institutionAchievementId && x.Application != null && x.Application.StatusID != 6))
            {
                errors.Add("Данное индивидуальное достижение используется в заявлении!");
                return false;
            }

            foreach (var achivement in db.IndividualAchivement.Where(x => x.IdAchievement == institutionAchievementId))
            {
                achivement.InstitutionAchievements = null;
            }

            db.InstitutionAchievements.DeleteObject(entity);
            db.SaveChanges();

            return true;
        }

        public static InstitutionAchievementRecordViewModel UpdateInstitutionAchievementRecord(
            this EntrantsEntities db, int institutionId, InstitutionAchievementUpdateRecordViewModel model)
        {
            InstitutionAchievements entity;

            int categoryId = model.CategoryId.GetValueOrDefault();

            if (model.Id == 0)
            {
                int campaignId = model.CampaignId.GetValueOrDefault();

                entity = new InstitutionAchievements
                {
                    Campaign =
                        db.Campaign.FirstOrDefault(x => x.CampaignID == campaignId),
                    IndividualAchievementsCategory =
                        db.IndividualAchievementsCategory.FirstOrDefault(
                            x => x.IdCategory == categoryId),
                    UID = model.UID,
                    Name = model.Name,
                    MaxValue = Math.Round(model.MaxValue.GetValueOrDefault(), 4)
                };

                db.InstitutionAchievements.AddObject(entity);
            }
            else
            {
                entity =
                    db.InstitutionAchievements.Single(
                        x => x.IdAchievement == model.Id && x.Campaign.InstitutionID == institutionId);

                entity.IndividualAchievementsCategory =
                        db.IndividualAchievementsCategory.FirstOrDefault(
                            x => x.IdCategory == categoryId);
                entity.UID = model.UID;
                entity.Name = model.Name;
                entity.MaxValue = Math.Round(model.MaxValue.GetValueOrDefault(), 4);
            }

            db.SaveChanges();

            return db.InstitutionAchievements.Where(x => x.IdAchievement == entity.IdAchievement)
                     .Select(x => new InstitutionAchievementRecordViewModel
                         {
                             Id = x.IdAchievement,
                             Name = x.Name,
                             MaxValue = x.MaxValue,
                             UID = x.UID,
                             CategoryId = x.IndividualAchievementsCategory.IdCategory,
                             CategoryName = x.IndividualAchievementsCategory.CategoryName
                         }).Single();
        }
    }
}