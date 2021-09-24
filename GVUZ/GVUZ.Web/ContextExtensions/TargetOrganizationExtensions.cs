using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using GVUZ.Model.Entrants;
using GVUZ.Web.ViewModels.TargetOrganizations;
using System.Linq;

namespace GVUZ.Web.ContextExtensions
{
    public static class TargetOrganizationExtensions
    {
        public static IEnumerable<TargetOrganizationRecordViewModel> LoadTargetOrganization(this EntrantsEntities db, int institutionId, string sortKey = null, bool sortDesc = false)
        {
            Expression<Func<CompetitiveGroupTarget, object>> sort = null;

            sortKey = sortKey ?? string.Empty;

            switch (sortKey.ToLowerInvariant())
            {
                case "uid":
                    sort = x => x.UID;
                    break;
                case "name":
                    sort = x => x.Name;
                    break;
            }

            var query =
                db.CompetitiveGroupTarget.Where(
                    x => x.InstitutionID == institutionId);

            if (sort != null)
            {
                query = sortDesc ? query.OrderByDescending(sort) : query.OrderBy(sort);
            }

            return
             query.Select(x => new TargetOrganizationRecordViewModel
             {
                 Id = x.CompetitiveGroupTargetID,
                 Name = x.Name,
                 UID = x.UID
             });
        }

        public static bool ValidateInstitutionAchievementUpdateRecordViewModel(this EntrantsEntities db, int institutionId, TargetOrganizationUpdateRecordViewModel model, ModelStateDictionary errors)
        {
            if (string.IsNullOrEmpty(model.UID))
            {
                return true;
            }

            if (db.CompetitiveGroupTarget.Any(x => x.UID.Equals(model.UID, StringComparison.OrdinalIgnoreCase) && x.CompetitiveGroupTargetID != model.CompetitiveGroupTargetID && x.InstitutionID == institutionId))
            {
                errors.AddModelError("UID", "Значение в поле UID должно быть уникальным среди всех целевых организаций!");
            }

            return errors.IsValid;
        }

        public static bool DeleteTargetOrganizationRecord(this EntrantsEntities db, int institutionId,
                                                             int competitiveGroupTargetId, List<string> errors)
        {
            var entity = db.CompetitiveGroupTarget.FirstOrDefault(
                    x => x.CompetitiveGroupTargetID == competitiveGroupTargetId && x.InstitutionID == institutionId);

            if (entity == null)
            {
                errors.Add("Объект не найден, возможно он был удален другим пользователем.");
                return false;
            }
            db.CompetitiveGroupTarget.DeleteObject(entity);
            db.SaveChanges();

            return true;
        }

        public static TargetOrganizationRecordViewModel UpdateTargetOrganizationRecord(
            this EntrantsEntities db, int institutionId, TargetOrganizationUpdateRecordViewModel model)
        {
            CompetitiveGroupTarget entity;

            if (model.CompetitiveGroupTargetID == 0)
            {
                entity = new CompetitiveGroupTarget
                {
                    UID = model.UID,
                    Name = model.Name,
                    InstitutionID = institutionId,
                    CreatedDate = DateTime.Now
                };

                db.CompetitiveGroupTarget.AddObject(entity);
            }
            else
            {
                entity =
                    db.CompetitiveGroupTarget.Single(
                        x => x.CompetitiveGroupTargetID == model.CompetitiveGroupTargetID && x.InstitutionID == institutionId);
                entity.UID = model.UID;
                entity.Name = model.Name;
                entity.InstitutionID = institutionId;
                entity.ModifiedDate = DateTime.Now;
            }

            db.SaveChanges();

            return db.CompetitiveGroupTarget.Where(x => x.CompetitiveGroupTargetID == entity.CompetitiveGroupTargetID)
                     .Select(x => new TargetOrganizationRecordViewModel
                     {
                         Id = x.CompetitiveGroupTargetID,
                         Name = x.Name,
                         UID = x.UID,
                         InstitutionID = x.InstitutionID
                     }).Single();
        }
    }
}