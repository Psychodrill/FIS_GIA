using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using GVUZ.DAL.Dapper.Model.AdmissionVolumes;
using GVUZ.DAL.Dapper.Model.Directions;
using GVUZ.DAL.Dapper.Model.DistributedAdmissionVolumes;
using GVUZ.DAL.Dapper.Model.ParentDirections;
using GVUZ.DAL.Dapper.Repository.Interfaces.Admission;
using GVUZ.DAL.Dapper.ViewModel.Dictionary;
using GVUZ.DAL.Dto;
using log4net;


namespace GVUZ.DAL.Dapper.Repository.Model.Admission
{
    public class PlanAdmissionVolumeRepository : GvuzRepository, IPlanAdmissionVolumeRepository
    {

        public static readonly  ILog logger = log4net.LogManager.GetLogger("PlanAdmissionRepository");
        public PlanAdmissionVolumeRepository()
            : base()
        {

        }

        public void CreatePlan(int campaignId)
        {

            DbConnection(db =>
            {
                //try { 

                IEnumerable<AdmissionVolume> avs = db.Query<AdmissionVolume>(sql: SQLQuery.AdmissionVolumeByCampaign, param:
                    new { campaignId = campaignId });

                IEnumerable<PlanAdmissionVolume> pavs = db.Query<PlanAdmissionVolume>(sql: SQLQuery.PlanAdmissionVolumeByCampaign, param:
                    new { campaignId = campaignId });


                IEnumerable<DistributedAdmissionVolume> davs = db.Query<DistributedAdmissionVolume>(sql: SQLQuery.DistributedAdmissionVolumeByCampaign, param:
                    new { campaignId = campaignId });
                Dictionary<int, IEnumerable<DistributedAdmissionVolume>> davsByAvId = davs.GroupBy(x => x.AdmissionVolumeID).ToDictionary(x => x.Key, x => (IEnumerable<DistributedAdmissionVolume>)x.ToArray());
                foreach (AdmissionVolume av in avs)
                {
                    if (!davsByAvId.ContainsKey(av.AdmissionVolumeID))
                    {
                        davsByAvId.Add(av.AdmissionVolumeID, new DistributedAdmissionVolume[0]);
                    }
                }

                IEnumerable<DistributedPlanAdmissionVolume> dpavs = db.Query<DistributedPlanAdmissionVolume>(sql: SQLQuery.DistributedPlanAdmissionVolumeByCampaign, param:
                    new { campaignId = campaignId });
                Dictionary<int, IEnumerable<DistributedPlanAdmissionVolume>> dpavsByPavId = dpavs.GroupBy(x => x.PlanAdmissionVolumeID).ToDictionary(x => x.Key, x => (IEnumerable<DistributedPlanAdmissionVolume>)x.ToArray());
                foreach (PlanAdmissionVolume pav in pavs)
                {
                    if (!dpavsByPavId.ContainsKey(pav.PlanAdmissionVolumeID))
                    {
                        dpavsByPavId.Add(pav.PlanAdmissionVolumeID, new DistributedPlanAdmissionVolume[0]);
                    }
                }

                foreach (PlanAdmissionVolume pav in pavs)
                {
                    AdmissionVolume av = avs
                        .FirstOrDefault(x => (x.DirectionID == pav.DirectionID)
                            && (x.AdmissionItemTypeID == pav.AdmissionItemTypeID));
                    if (av == null)
                    {
                        db.Query(sql: SQLQuery.DeletePlanAdmissionVolume, param: new { PlanAdmissionVolumeID = pav.PlanAdmissionVolumeID });
                        continue;
                    }

                    CopyVolumeNumbers(pav, av);
                    if (pav.Number <= 0)
                    {
                        db.Query(sql: SQLQuery.DeletePlanAdmissionVolume, param: new { PlanAdmissionVolumeID = pav.PlanAdmissionVolumeID });
                        continue;
                    }

                    pav.ModifiedDate = DateTime.Now;
                    db.Query(sql: SQLQuery.UpdatePlanAdmissionVolume, param: pav);

                    foreach (DistributedPlanAdmissionVolume dpav in dpavsByPavId[pav.PlanAdmissionVolumeID])
                    {
                        DistributedAdmissionVolume dav = davsByAvId[av.AdmissionVolumeID]
                            .FirstOrDefault(x => x.IdLevelBudget == dpav.IdLevelBudget);

                        if (dav == null)
                        {
                            db.Query(sql: SQLQuery.DeleteDistributedPlanAdmissionVolume, param: new { DistributedPlanAdmissionVolumeID = dpav.DistributedPlanAdmissionVolumeID });
                            continue;
                        }

                        CopyDistributedVolumeNumbers(pav, dpav, dav);
                        if (dpav.Number <= 0)
                        {
                            db.Query(sql: SQLQuery.DeleteDistributedPlanAdmissionVolume, param: new { DistributedPlanAdmissionVolumeID = dpav.DistributedPlanAdmissionVolumeID });
                            continue;
                        }

                        db.Query(sql: SQLQuery.UpdateDistributedPlanAdmissionVolume, param: dpav);
                    }

                    foreach (DistributedAdmissionVolume dav in davsByAvId[av.AdmissionVolumeID])
                    {
                        DistributedPlanAdmissionVolume dpav = dpavsByPavId[pav.PlanAdmissionVolumeID]
                            .FirstOrDefault(x => x.IdLevelBudget == dav.IdLevelBudget);

                        if (dpav == null)
                        {
                            dpav = new DistributedPlanAdmissionVolume();
                            dpav.PlanAdmissionVolumeID = pav.PlanAdmissionVolumeID;
                            dpav.IdLevelBudget = dav.IdLevelBudget;

                            CopyDistributedVolumeNumbers(pav, dpav, dav);
                            if (dpav.Number <= 0)
                                continue;

                            //logger.ErrorFormat("Input parameters for DistributedPlanAdmissionVolumeID {0}, {1}, {2}", dpav.PlanAdmissionVolumeID, dpav.IdLevelBudget, dpav.Number);
                            dpav.DistributedPlanAdmissionVolumeID = db.Query<int>(sql: SQLQuery.CreateDistributedPlanAdmissionVolume, param: dpav).First();
                        }
                    }


                }

                foreach (AdmissionVolume av in avs)
                {
                    foreach (short educationFormId in new short[] { EDFormsConst.O, EDFormsConst.OZ, EDFormsConst.Z })
                    {
                        foreach (short financeSourceId in new short[] { EDSourceConst.Budget, EDSourceConst.Paid, EDSourceConst.Quota, EDSourceConst.Target })
                        {
                            PlanAdmissionVolume pav = pavs
                                .FirstOrDefault(x => (x.DirectionID == av.DirectionID)
                                    && (x.AdmissionItemTypeID == av.AdmissionItemTypeID)
                                    && (x.EducationFormID == educationFormId)
                                    && (x.EducationSourceID == financeSourceId));

                            if (pav == null)
                            {
                                pav = new PlanAdmissionVolume();
                                pav.CreatedDate = DateTime.Now;

                                pav.CampaignID = campaignId;
                                pav.AdmissionItemTypeID = av.AdmissionItemTypeID;
                                pav.DirectionID = av.DirectionID;
                                pav.EducationFormID = educationFormId;
                                pav.EducationSourceID = financeSourceId;
                                pav.ParentDirectionID = av.ParentDirectionID;

                                CopyVolumeNumbers(pav, av);
                                if (pav.Number <= 0)
                                    continue;

                                pav.PlanAdmissionVolumeID = db.Query<int>(sql: SQLQuery.CreatePlanAdmissionVolume, param: pav).First();

                                if (financeSourceId != EDSourceConst.Paid)//Для платных не бывает PDAV
                                {
                                    foreach (DistributedAdmissionVolume dav in davsByAvId[av.AdmissionVolumeID])
                                    {
                                        DistributedPlanAdmissionVolume dpav = new DistributedPlanAdmissionVolume();
                                        dpav.PlanAdmissionVolumeID = pav.PlanAdmissionVolumeID;
                                        dpav.IdLevelBudget = dav.IdLevelBudget;

                                        CopyDistributedVolumeNumbers(pav, dpav, dav);
                                        if (dpav.Number <= 0)
                                            continue;
                                            //logger.ErrorFormat("Input parameters for DistributedPlanAdmissionVolumeID {0}, {1}, {2}", dpav.PlanAdmissionVolumeID, dpav.IdLevelBudget, dpav.Number);
                                            dpav.DistributedPlanAdmissionVolumeID = db.Query<int>(sql: SQLQuery.CreateDistributedPlanAdmissionVolume, param: dpav).First();
                                    }
                                }
                            }
                        }
                    }
                }
            //}
            //    catch (Exception ex)
            //    {
            //        logger.ErrorFormat(""ex.Message);  
            //    }
                return true;
            });
        }

        private void CopyDistributedVolumeNumbers(PlanAdmissionVolume pav, DistributedPlanAdmissionVolume dpav, DistributedAdmissionVolume dav)
        {
            if ((pav.EducationFormID == EDFormsConst.O) && (pav.EducationSourceID == EDSourceConst.Budget))
            {
                dpav.Number = dav.NumberBudgetO;
            }
            else if ((pav.EducationFormID == EDFormsConst.OZ) && (pav.EducationSourceID == EDSourceConst.Budget))
            {
                dpav.Number = dav.NumberBudgetOZ;
            }
            else if ((pav.EducationFormID == EDFormsConst.Z) && (pav.EducationSourceID == EDSourceConst.Budget))
            {
                dpav.Number = dav.NumberBudgetZ;
            }
            else if ((pav.EducationFormID == EDFormsConst.O) && (pav.EducationSourceID == EDSourceConst.Quota))
            {
                dpav.Number = dav.NumberQuotaO;
            }
            else if ((pav.EducationFormID == EDFormsConst.OZ) && (pav.EducationSourceID == EDSourceConst.Quota))
            {
                dpav.Number = dav.NumberQuotaOZ;
            }
            else if ((pav.EducationFormID == EDFormsConst.Z) && (pav.EducationSourceID == EDSourceConst.Quota))
            {
                dpav.Number = dav.NumberQuotaZ;
            }
            else if ((pav.EducationFormID == EDFormsConst.O) && (pav.EducationSourceID == EDSourceConst.Target))
            {
                dpav.Number = dav.NumberTargetO;
            }
            else if ((pav.EducationFormID == EDFormsConst.OZ) && (pav.EducationSourceID == EDSourceConst.Target))
            {
                dpav.Number = dav.NumberTargetOZ;
            }
            else if ((pav.EducationFormID == EDFormsConst.Z) && (pav.EducationSourceID == EDSourceConst.Target))
            {
                dpav.Number = dav.NumberTargetZ;
            }
        }

        private void CopyVolumeNumbers(PlanAdmissionVolume pav, AdmissionVolume av)
        {
            if ((pav.EducationFormID == EDFormsConst.O) && (pav.EducationSourceID == EDSourceConst.Budget))
            {
                pav.Number = av.NumberBudgetO;
            }
            else if ((pav.EducationFormID == EDFormsConst.OZ) && (pav.EducationSourceID == EDSourceConst.Budget))
            {
                pav.Number = av.NumberBudgetOZ;
            }
            else if ((pav.EducationFormID == EDFormsConst.Z) && (pav.EducationSourceID == EDSourceConst.Budget))
            {
                pav.Number = av.NumberBudgetZ;
            }
            else if ((pav.EducationFormID == EDFormsConst.O) && (pav.EducationSourceID == EDSourceConst.Paid))
            {
                pav.Number = av.NumberPaidO;
            }
            else if ((pav.EducationFormID == EDFormsConst.OZ) && (pav.EducationSourceID == EDSourceConst.Paid))
            {
                pav.Number = av.NumberPaidOZ;
            }
            else if ((pav.EducationFormID == EDFormsConst.Z) && (pav.EducationSourceID == EDSourceConst.Paid))
            {
                pav.Number = av.NumberPaidZ;
            }
            else if ((pav.EducationFormID == EDFormsConst.O) && (pav.EducationSourceID == EDSourceConst.Quota))
            {
                pav.Number = av.NumberQuotaO.GetValueOrDefault();
            }
            else if ((pav.EducationFormID == EDFormsConst.OZ) && (pav.EducationSourceID == EDSourceConst.Quota))
            {
                pav.Number = av.NumberQuotaOZ.GetValueOrDefault();
            }
            else if ((pav.EducationFormID == EDFormsConst.Z) && (pav.EducationSourceID == EDSourceConst.Quota))
            {
                pav.Number = av.NumberQuotaZ.GetValueOrDefault();
            }
            else if ((pav.EducationFormID == EDFormsConst.O) && (pav.EducationSourceID == EDSourceConst.Target))
            {
                pav.Number = av.NumberTargetO;
            }
            else if ((pav.EducationFormID == EDFormsConst.OZ) && (pav.EducationSourceID == EDSourceConst.Target))
            {
                pav.Number = av.NumberTargetOZ;
            }
            else if ((pav.EducationFormID == EDFormsConst.Z) && (pav.EducationSourceID == EDSourceConst.Target))
            {
                pav.Number = av.NumberTargetZ;
            }
        }

        public void SavePlan(int campaignId, IEnumerable<PlanAdmissionVolume> planAdmissionVolumes)
        {
            DbConnection(db =>
            {
                IEnumerable<PlanAdmissionVolume> existingPavs = db.Query<PlanAdmissionVolume>(sql: SQLQuery.PlanAdmissionVolumeByCampaign, param:
                    new { campaignId = campaignId });

                foreach (PlanAdmissionVolume existingPav in existingPavs)
                {
                    PlanAdmissionVolume savingPav = planAdmissionVolumes
                        .FirstOrDefault(x => (x.AdmissionItemTypeID == existingPav.AdmissionItemTypeID)
                        && (x.DirectionID == existingPav.DirectionID && existingPav.DirectionID != null)
                        && (x.EducationSourceID == existingPav.EducationSourceID)
                        && (x.EducationFormID == existingPav.EducationFormID));

                    savingPav = savingPav == null ? planAdmissionVolumes
                        .FirstOrDefault(x => (x.AdmissionItemTypeID == existingPav.AdmissionItemTypeID)
                        && (x.ParentDirectionID == existingPav.ParentDirectionID)
                        && (x.EducationSourceID == existingPav.EducationSourceID)
                        && (x.EducationFormID == existingPav.EducationFormID)) : savingPav;

                    if ((savingPav == null) || (savingPav.Number <= 0))
                    {
                        db.Query(sql: SQLQuery.DeletePlanAdmissionVolume, param: new { PlanAdmissionVolumeID = existingPav.PlanAdmissionVolumeID });
                    }
                    else if (existingPav.Number != savingPav.Number)
                    {
                        bool deleteDistribution = (savingPav.Number < existingPav.Number);
                        existingPav.Number = savingPav.Number;
                        existingPav.ModifiedDate = DateTime.Now;
                        if (deleteDistribution)
                        {
                            db.Query(sql: SQLQuery.DeleteDistributedPlanAdmissionVolumeByPlanAdmissionVolumeId, param: new { PlanAdmissionVolumeID = existingPav.PlanAdmissionVolumeID });
                        }
                        db.Query(sql: SQLQuery.UpdatePlanAdmissionVolume, param: existingPav);
                    }
                }
                foreach (PlanAdmissionVolume savingPav in planAdmissionVolumes)
                {
                    if (savingPav.Number <= 0)
                        continue;

                    PlanAdmissionVolume existingPav = existingPavs
                        .FirstOrDefault(x => (x.AdmissionItemTypeID == savingPav.AdmissionItemTypeID)
                        && (x.DirectionID == savingPav.DirectionID && savingPav.DirectionID != null)
                        && (x.EducationSourceID == savingPav.EducationSourceID)
                        && (x.EducationFormID == savingPav.EducationFormID));

                    existingPav = existingPav == null ? existingPavs
                        .FirstOrDefault(x => (x.AdmissionItemTypeID == savingPav.AdmissionItemTypeID)
                        && (x.ParentDirectionID == savingPav.ParentDirectionID && savingPav.DirectionID == null)
                        && (x.EducationSourceID == savingPav.EducationSourceID)
                        && (x.EducationFormID == savingPav.EducationFormID)) : existingPav;

                    if (existingPav == null)
                    {
                        existingPav = new PlanAdmissionVolume();
                        existingPav.CampaignID = campaignId;
                        existingPav.AdmissionItemTypeID = savingPav.AdmissionItemTypeID;
                        existingPav.DirectionID = savingPav.DirectionID;
                        existingPav.EducationFormID = savingPav.EducationFormID;
                        existingPav.EducationSourceID = savingPav.EducationSourceID;
                        existingPav.Number = savingPav.Number;
                        existingPav.ParentDirectionID = savingPav.ParentDirectionID; ;

                        existingPav.CreatedDate = DateTime.Now;

                        existingPav.PlanAdmissionVolumeID = db.Query<int>(sql: SQLQuery.CreatePlanAdmissionVolume, param: existingPav).First();
                    }
                }
                return true;
            });
        }

        public void SavePlanDistribution(int campaignId, int levelId, int directionId, IEnumerable<DistributedPlanAdmissionVolumeSaveDto> distributedPlanAdmissionVolumeDtos)
        {
            DbConnection(db =>
            {
                IEnumerable<PlanAdmissionVolume> pavs = db.Query<PlanAdmissionVolume>(sql: SQLQuery.PlanAdmissionVolumeByCampaign, param:
                    new { campaignId = campaignId })
                    .Where(x => (x.AdmissionItemTypeID == levelId) && (x.DirectionID == directionId));
                IEnumerable<DistributedPlanAdmissionVolume> dpavs = db.Query<DistributedPlanAdmissionVolume>(sql: SQLQuery.DistributedPlanAdmissionVolumeByCampaign, param:
                    new { campaignId = campaignId });


                foreach (DistributedPlanAdmissionVolumeSaveDto dpavDto in distributedPlanAdmissionVolumeDtos)
                {
                    PlanAdmissionVolume pav = pavs
                        .FirstOrDefault(x => (x.EducationFormID == dpavDto.EducationFormId) && (x.EducationSourceID == dpavDto.FinanceSourceId));
                    if ((pav == null) || (pav.Number <= 0))
                        continue;

                    DistributedPlanAdmissionVolume dpav = dpavs
                        .FirstOrDefault(x => (x.PlanAdmissionVolumeID == pav.PlanAdmissionVolumeID) && (x.IdLevelBudget == dpavDto.BudgetId));

                    if (dpavDto.Number <= 0)
                    {
                        if (dpav != null)
                        {
                            db.Query(sql: SQLQuery.DeleteDistributedPlanAdmissionVolume, param: new { DistributedPlanAdmissionVolumeID = dpav.DistributedPlanAdmissionVolumeID });
                        }
                        continue;
                    }

                    if (dpav == null)
                    {
                        dpav = new DistributedPlanAdmissionVolume();
                        dpav.PlanAdmissionVolumeID = pav.PlanAdmissionVolumeID;
                        dpav.IdLevelBudget = dpavDto.BudgetId;
                        dpav.Number = dpavDto.Number;
                        dpav.DistributedPlanAdmissionVolumeID = db.Query<int>(sql: SQLQuery.CreateDistributedPlanAdmissionVolume, param: dpav).First();
                    }
                    else
                    {
                        dpav.Number = dpavDto.Number;
                        db.Query(sql: SQLQuery.UpdateDistributedPlanAdmissionVolume, param: dpav);
                    }
                }
                return true;
            });
        }

        public IEnumerable<int> GetCampaignEducationForms(int campaignId)
        {
            return DbConnection(db =>
            {
                int flags = db.Query<int>(sql: SQLQuery.GetCampaignFormFlags, param: new { campaignId = campaignId }).First();

                List<int> result = new List<int>();

                if ((flags & 1) > 0)
                {
                    result.Add(EDFormsConst.O);
                }
                if ((flags & 2) > 0)
                {
                    result.Add(EDFormsConst.OZ);
                }
                if ((flags & 4) > 0)
                {
                    result.Add(EDFormsConst.Z);
                }

                return result;
            });
        }

        public IEnumerable<PlanAdmissionVolume> GetPlanAdmissionVolumeByCampaign(int campaignId)
        {
            return DbConnection(db =>
            {
                return db.Query<PlanAdmissionVolume>(sql: SQLQuery.PlanAdmissionVolumeByCampaign, param:
                    new { campaignId = campaignId });
            });
        }

        public IEnumerable<DistributedPlanAdmissionVolume> GetDistributedPlanAdmissionVolumeByCampaign(int campaignId)
        {
            return DbConnection(db =>
            {
                return db.Query<DistributedPlanAdmissionVolume>(sql: SQLQuery.DistributedPlanAdmissionVolumeByCampaign, param:
                    new { campaignId = campaignId });
            });
        }

        public IEnumerable<Direction> GetPlanAdmissionVolumeDirectionsByCampaign(int campaignId)
        {
            return DbConnection(db =>
            {
                return db.Query<Direction>(sql: SQLQuery.PlanAdmissionVolumeDirectionsByCampaign, param:
                    new { campaignId = campaignId });
            });
        }

        public IEnumerable<ParentDirection> GetPlanAdmissionVolumeDirectionGroupsByCampaign(int campaignId)
        {
            return DbConnection(db =>
            {
                return db.Query<ParentDirection>(sql: SQLQuery.PlanAdmissionVolumeDirectionGroupsByCampaign, param:
                    new { campaignId = campaignId });
            });
        }
    }
}
