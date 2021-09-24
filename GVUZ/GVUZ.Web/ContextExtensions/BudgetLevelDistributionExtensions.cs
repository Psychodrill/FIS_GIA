using System;
using System.Collections.Generic;
using System.Linq;
using GVUZ.Model.Entrants;
using GVUZ.Model.Institutions;
using GVUZ.Web.ViewModels.KcpDistribution;

namespace GVUZ.Web.ContextExtensions
{
    public static class BudgetLevelDistributionExtensions
    {
        public static KcpEditorViewModel LoadDistributionOptions(this EntrantsEntities db, int admissionVolumeId,
                                                                 int institutionId)
        {
            var admissionVolume = db.AdmissionVolume.Single(x => x.AdmissionVolumeID == admissionVolumeId && x.InstitutionID == institutionId);

            var model = new KcpEditorViewModel
                {
                    AdmissionVolumeId = admissionVolume.AdmissionVolumeID
                };

            //var availForms = db.CampaignDate.Where(
            //    x => x.Campaign.InstitutionID == institutionId &&
            //    x.CampaignID == admissionVolume.CampaignID && 
            //    x.Course == admissionVolume.Course && 
            //    x.IsActive &&
            //    x.EducationLevelID == admissionVolume.AdmissionItemTypeID)
            //    .Distinct().Select(x => new {x.EducationFormID, x.EducationSourceID}).Distinct().ToArray();

            // Значения EducationLevelId (AdmissionItemTypeId) для которых разрешена квота
            var quotaEnabledForLevels = new[] {2, 3, 5, 19};

            #region Доступность для распределения КЦП и лимиты для валидации
            model.Limits = new KcpLimitsViewModel();

            model.Limits.Budget = new KcpForms<KcpLimitViewModel>
            {
                O = new KcpLimitViewModel
                    {
                        IsSet = true,// availForms.Any(x => x.EducationSourceID == EDSourceConst.Budget && x.EducationFormID == EDFormsConst.O),
                        Limit = admissionVolume.NumberBudgetO
                    },
                OZ = new KcpLimitViewModel
                    {
                        IsSet = true,//availForms.Any(x => x.EducationSourceID == EDSourceConst.Budget && x.EducationFormID == EDFormsConst.OZ),
                        Limit = admissionVolume.NumberBudgetOZ
                    },
                Z = new KcpLimitViewModel
                    {
                        IsSet = true,//availForms.Any(x => x.EducationSourceID == EDSourceConst.Budget && x.EducationFormID == EDFormsConst.Z),
                        Limit = admissionVolume.NumberBudgetZ
                    }
            };

            // Проверка квоты осуществляется почему-то не по источнику финансирования "Квота" а по источнику финансирования "Бюджет"
            // и EducationLevelId (он же AdmissionItemTypeId) in (2, 3, 5, 19)
            model.Limits.Quota = new KcpForms<KcpLimitViewModel>
            {
                O = new KcpLimitViewModel
                {
                    IsSet = quotaEnabledForLevels.Contains(admissionVolume.AdmissionItemTypeID), // && availForms.Any(x => x.EducationSourceID == EDSourceConst.Budget && x.EducationFormID == EDFormsConst.O),
                    Limit = admissionVolume.NumberQuotaO.GetValueOrDefault()
                },
                OZ = new KcpLimitViewModel
                {
                    IsSet = quotaEnabledForLevels.Contains(admissionVolume.AdmissionItemTypeID), // && availForms.Any(x => x.EducationSourceID == EDSourceConst.Budget && x.EducationFormID == EDFormsConst.OZ),
                    Limit = admissionVolume.NumberQuotaOZ.GetValueOrDefault()
                },
                Z = new KcpLimitViewModel
                {
                    IsSet = quotaEnabledForLevels.Contains(admissionVolume.AdmissionItemTypeID), // && availForms.Any(x => x.EducationSourceID == EDSourceConst.Budget && x.EducationFormID == EDFormsConst.Z),
                    Limit = admissionVolume.NumberQuotaZ.GetValueOrDefault()
                }
            };

            model.Limits.Target = new KcpForms<KcpLimitViewModel>
            {
                O = new KcpLimitViewModel
                {
                    IsSet = true, // availForms.Any(x => x.EducationSourceID == EDSourceConst.Target && x.EducationFormID == EDFormsConst.O),
                    Limit = admissionVolume.NumberTargetO
                },
                OZ = new KcpLimitViewModel
                {
                    IsSet = true, //availForms.Any(x => x.EducationSourceID == EDSourceConst.Target && x.EducationFormID == EDFormsConst.OZ),
                    Limit = admissionVolume.NumberTargetOZ
                },
                Z = new KcpLimitViewModel
                {
                    IsSet = true, //availForms.Any(x => x.EducationSourceID == EDSourceConst.Target && x.EducationFormID == EDFormsConst.Z),
                    Limit = admissionVolume.NumberTargetZ
                }
            }; 
            #endregion

            model.BudgetLevels =
                db.LevelBudget.OrderBy(x => x.IdLevelBudget)
                  .Select(x => new KcpBudgetLevelViewModel {BudgetLevelId = x.IdLevelBudget})
                  .ToArray();

            var distributedLevels = db.DistributedAdmissionVolume.Where(x => x.AdmissionVolumeID == admissionVolume.AdmissionVolumeID).ToArray();

            foreach (var budgetLevel in model.BudgetLevels)
            {
                var distributed = distributedLevels.FirstOrDefault(x => x.IdLevelBudget == budgetLevel.BudgetLevelId);

                if (distributed != null)
                {
                    budgetLevel.DistributedAdmissionVolumeId = distributed.DistributedAdmissionVolumeID;
                    budgetLevel.Budget = new KcpForms<KcpFieldViewModel>
                        {
                            O = new KcpFieldViewModel {Value = distributed.NumberBudgetO},
                            OZ = new KcpFieldViewModel {Value = distributed.NumberBudgetOZ},
                            Z = new KcpFieldViewModel {Value = distributed.NumberBudgetZ}
                        };
                    budgetLevel.Quota = new KcpForms<KcpFieldViewModel>
                        {
                            O = new KcpFieldViewModel {Value = distributed.NumberQuotaO},
                            OZ = new KcpFieldViewModel {Value = distributed.NumberQuotaOZ},
                            Z = new KcpFieldViewModel {Value = distributed.NumberQuotaZ}
                        };
                    budgetLevel.Target = new KcpForms<KcpFieldViewModel>
                        {
                            O = new KcpFieldViewModel {Value = distributed.NumberTargetO},
                            OZ = new KcpFieldViewModel {Value = distributed.NumberTargetOZ},
                            Z = new KcpFieldViewModel {Value = distributed.NumberTargetZ}
                        };
                }
            }

            model.AvailableDistributionPoints = admissionVolume.NumberBudgetO +
                                         admissionVolume.NumberBudgetOZ +
                                         admissionVolume.NumberBudgetZ +
                                         admissionVolume.NumberQuotaO.GetValueOrDefault() +
                                         admissionVolume.NumberQuotaOZ.GetValueOrDefault() +
                                         admissionVolume.NumberQuotaZ.GetValueOrDefault() +
                                         admissionVolume.NumberTargetO +
                                         admissionVolume.NumberTargetOZ +
                                         admissionVolume.NumberTargetZ;

            model.DistributedPoints =
                distributedLevels.Sum(
                    x =>
                    x.NumberBudgetO + x.NumberBudgetOZ + x.NumberBudgetZ + x.NumberQuotaO + x.NumberQuotaOZ +
                    x.NumberQuotaZ + x.NumberTargetO + x.NumberTargetOZ + x.NumberTargetZ);

            return model;
        }

        public static bool UpdateDistribution(this EntrantsEntities db, KcpUpdateViewModel model, int institutionId)
        {
            var admissionVolume = db.AdmissionVolume.Single(x => x.AdmissionVolumeID == model.AdmissionVolumeId && x.InstitutionID == institutionId);

            //var availForms = db.CampaignDate.Where(
            //    x => x.Campaign.InstitutionID == institutionId &&
            //    x.CampaignID == admissionVolume.CampaignID &&
            //    x.Course == admissionVolume.Course &&
            //    x.IsActive &&
            //    x.EducationLevelID == admissionVolume.AdmissionItemTypeID)
            //    .Distinct().Select(x => new { x.EducationFormID, x.EducationSourceID }).Distinct().ToArray();

            // Значения EducationLevelId (AdmissionItemTypeId) для которых разрешена квота
            var quotaEnabledForLevels = new[] { 2, 3, 5, 19 };

            var distributedLevels = db.DistributedAdmissionVolume.Where(x => x.AdmissionVolumeID == admissionVolume.AdmissionVolumeID).ToArray();

            var newEntities = new List<DistributedAdmissionVolume>();

            bool hasDistributionErrors = false;

            foreach (var budgetLevel in model.BudgetLevels)
            {
                var distributed = distributedLevels.FirstOrDefault(
                    x => x.DistributedAdmissionVolumeID == budgetLevel.DistributedAdmissionVolumeId && x.IdLevelBudget == budgetLevel.BudgetLevelId);

                if (distributed == null)
                {
                    distributed = new DistributedAdmissionVolume
                    {
                        IdLevelBudget = budgetLevel.BudgetLevelId,
                        AdmissionVolumeID = admissionVolume.AdmissionVolumeID
                    };

                    newEntities.Add(distributed);
                }

                #region Расстановка распределенных значений с проверкой (валидацией)
                // для каждого источника финансирования и каждой соответствующей ему формы обучения
                // проверяем что источник и форма доступны для распределения (есть соответствующие значения в CampaignDate (availForms) и они заполнены

                // Бюджетные места EDSourceConst.Budget
                if (admissionVolume.NumberBudgetO > 0) // && availForms.Any(x => x.EducationSourceID == EDSourceConst.Budget && x.EducationFormID == EDFormsConst.O))
                {
                    distributed.NumberBudgetO = budgetLevel.Budget.O.Value;
                }
                else if (budgetLevel.Budget.O.Value != 0)
                {
                    budgetLevel.Budget.O.ErrorMessage = DistributionValidation.UnavailableForDistribution;
                    hasDistributionErrors = true;
                }
                if (admissionVolume.NumberBudgetOZ > 0) // && availForms.Any(x => x.EducationSourceID == EDSourceConst.Budget && x.EducationFormID == EDFormsConst.OZ))
                {
                    distributed.NumberBudgetOZ = budgetLevel.Budget.OZ.Value;
                }
                else if (budgetLevel.Budget.OZ.Value != 0)
                {
                    budgetLevel.Budget.OZ.ErrorMessage = DistributionValidation.UnavailableForDistribution;
                    hasDistributionErrors = true;
                }
                if (admissionVolume.NumberBudgetZ > 0) // && availForms.Any(x => x.EducationSourceID == EDSourceConst.Budget && x.EducationFormID == EDFormsConst.Z))
                {
                    distributed.NumberBudgetZ = budgetLevel.Budget.Z.Value;
                }
                else if (budgetLevel.Budget.Z.Value != 0)
                {
                    budgetLevel.Budget.Z.ErrorMessage = DistributionValidation.UnavailableForDistribution;
                    hasDistributionErrors = true;
                }

                // Места по квотам EDSourceConst.Quota
                if (admissionVolume.NumberQuotaO.GetValueOrDefault() > 0 && quotaEnabledForLevels.Contains(admissionVolume.AdmissionItemTypeID)) // && availForms.Any(x => x.EducationSourceID == EDSourceConst.Budget && x.EducationFormID == EDFormsConst.O))
                {
                    distributed.NumberQuotaO = budgetLevel.Quota.O.Value;
                }
                else if (budgetLevel.Quota.O.Value != 0) 
                {
                    budgetLevel.Quota.O.ErrorMessage = DistributionValidation.UnavailableForDistribution;
                    hasDistributionErrors = true;
                }
                if (admissionVolume.NumberQuotaOZ.GetValueOrDefault() > 0 && quotaEnabledForLevels.Contains(admissionVolume.AdmissionItemTypeID)) // && availForms.Any(x => x.EducationSourceID == EDSourceConst.Budget && x.EducationFormID == EDFormsConst.OZ))
                {
                    distributed.NumberQuotaOZ = budgetLevel.Quota.OZ.Value;
                }
                else if (budgetLevel.Quota.OZ.Value != 0)
                {
                    budgetLevel.Quota.OZ.ErrorMessage = DistributionValidation.UnavailableForDistribution;
                    hasDistributionErrors = true;
                }
                if (admissionVolume.NumberQuotaZ.GetValueOrDefault() > 0 && quotaEnabledForLevels.Contains(admissionVolume.AdmissionItemTypeID)) // && availForms.Any(x => x.EducationSourceID == EDSourceConst.Budget && x.EducationFormID == EDFormsConst.Z))
                {
                    distributed.NumberQuotaZ = budgetLevel.Quota.Z.Value;
                }
                else if (budgetLevel.Quota.Z.Value != 0)
                {
                    budgetLevel.Quota.Z.ErrorMessage = DistributionValidation.UnavailableForDistribution;
                    hasDistributionErrors = true;
                }

                // Места для целевого приема EDSourceConst.Target
                if (admissionVolume.NumberTargetO > 0) //&& availForms.Any(x => x.EducationSourceID == EDSourceConst.Target && x.EducationFormID == EDFormsConst.O))
                {
                    distributed.NumberTargetO = budgetLevel.Target.O.Value;
                }
                else if (budgetLevel.Target.O.Value != 0)
                {
                    budgetLevel.Target.O.ErrorMessage = DistributionValidation.UnavailableForDistribution;
                    hasDistributionErrors = true;
                }
                if (admissionVolume.NumberTargetOZ > 0 ) //&& availForms.Any(x => x.EducationSourceID == EDSourceConst.Target && x.EducationFormID == EDFormsConst.OZ))
                {
                    distributed.NumberTargetOZ = budgetLevel.Target.OZ.Value;
                }
                else if (budgetLevel.Target.OZ.Value != 0)
                {
                    budgetLevel.Target.OZ.ErrorMessage = DistributionValidation.UnavailableForDistribution;
                    hasDistributionErrors = true;
                }
                if (admissionVolume.NumberTargetZ > 0 ) //&& availForms.Any(x => x.EducationSourceID == EDSourceConst.Target && x.EducationFormID == EDFormsConst.Z))
                {
                    distributed.NumberTargetZ = budgetLevel.Target.Z.Value;
                }
                else if (budgetLevel.Target.Z.Value != 0)
                {
                    budgetLevel.Target.Z.ErrorMessage = DistributionValidation.UnavailableForDistribution;
                    hasDistributionErrors = true;
                }
                #endregion
            }

            // если при расстановке были выявлены нарушения, то возвращаем модель с ошибками
            // иначе - продолжаем
            if (hasDistributionErrors)
            {
                return false;
            }

            //  проверяем
            // что сумма распределенных значений для каждого источника финансирования и каждой соответствующей ему формы обучения
            // по всем уровням бюджета не превышает соответствующего значения в AdmissionVolume
            var checkOverflow = DistributionValidation.CheckOverflow;

            hasDistributionErrors = new[]
                {
                    checkOverflow(model, l => l.Budget.O, admissionVolume.NumberBudgetO),
                    checkOverflow(model, l => l.Budget.OZ, admissionVolume.NumberBudgetOZ),
                    checkOverflow(model, l => l.Budget.Z, admissionVolume.NumberBudgetZ),
                    checkOverflow(model, l => l.Quota.O, admissionVolume.NumberQuotaO.GetValueOrDefault()),
                    checkOverflow(model, l => l.Quota.OZ, admissionVolume.NumberQuotaOZ.GetValueOrDefault()),
                    checkOverflow(model, l => l.Quota.Z, admissionVolume.NumberQuotaZ.GetValueOrDefault()),
                    checkOverflow(model, l => l.Target.O, admissionVolume.NumberTargetO),
                    checkOverflow(model, l => l.Target.OZ, admissionVolume.NumberTargetOZ),
                    checkOverflow(model, l => l.Target.Z, admissionVolume.NumberTargetZ)
                }.Any(y => y); // checkOverflow возвращает true если были ошибки
            
            if (hasDistributionErrors)
            {
                return false;    
            }

            // проверяем, что сумма всех введенных распределенных значений не превышает число доступное для распределения

            model.AvailableForDistribution = admissionVolume.NumberBudgetO +
                                         admissionVolume.NumberBudgetOZ +
                                         admissionVolume.NumberBudgetZ +
                                         admissionVolume.NumberQuotaO.GetValueOrDefault() +
                                         admissionVolume.NumberQuotaOZ.GetValueOrDefault() +
                                         admissionVolume.NumberQuotaZ.GetValueOrDefault() +
                                         admissionVolume.NumberTargetO +
                                         admissionVolume.NumberTargetOZ +
                                         admissionVolume.NumberTargetZ;

            model.TotalDistributed = model.BudgetLevels.Sum(x => x.EnumerateFields().Sum(y => y.Value));

            if (model.TotalDistributed > model.AvailableForDistribution)
            {
                model.BudgetLevels.SelectMany(x => x.EnumerateFields()).Where(f => f.Value != 0).ToList().ForEach(x => x.ErrorMessage = DistributionValidation.TotalDistributionOverflow);
                hasDistributionErrors = true;
            }

            if (hasDistributionErrors)
            {
                return false;
            }
            // добавляем в базу новые DistributedAdmissionVolume которых не было ранее
            newEntities.ForEach(e => db.DistributedAdmissionVolume.AddObject(e));

            // сохраняем изменения
            db.SaveChanges();
            
            return true;
        }
        
        private static class DistributionValidation
        {
            public const string UnavailableForDistribution = "Для данной формы обучения и источника финансирования не предусмотрено распределение количества мест по уровням бюджета";
            private const string DistributionOverflow = "Количество распределенных значений по всем уровням бюджета не должно превышать количество мест по соответствующей форме обучения и источнику финансирования";
            public const string TotalDistributionOverflow = "Суммарное количество всех распределенных значений не должно превышать общее число мест, доступных для распределения";

            public static readonly Func<KcpUpdateViewModel, Func<KcpBudgetLevelViewModel, KcpFieldViewModel>, int, bool> CheckOverflow =
            (m, get, limit) =>
            {
                if (m.BudgetLevels.Sum(x => get(x).Value) > limit)
                {
                    m.BudgetLevels.Select(get).ToList().ForEach(f => f.ErrorMessage = DistributionOverflow);
                    return true;
                }

                return false;
            }; 
        }

        public static IEnumerable<KcpFieldViewModel> EnumerateFields(this KcpBudgetLevelViewModel model)
        {
            yield return model.Budget.O;
            yield return model.Budget.OZ;
            yield return model.Budget.Z;
            yield return model.Quota.O;
            yield return model.Quota.OZ;
            yield return model.Quota.Z;
            yield return model.Target.O;
            yield return model.Target.OZ;
            yield return model.Target.Z;
        }
    }
}