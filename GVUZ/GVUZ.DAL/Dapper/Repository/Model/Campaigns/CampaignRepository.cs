using GDDVC = GVUZ.DAL.Dapper.ViewModel.Campaign;
using GDDMC = GVUZ.DAL.Dapper.Model.Campaigns;
using GDDMD = GVUZ.DAL.Dapper.ViewModel.Dictionary;
using GVUZ.DAL.Dapper.Repository.Interfaces.Campaign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using System.Configuration;
using System.Globalization;
using System.Data;
using System.Web.Mvc;

namespace GVUZ.DAL.Dapper.Repository.Model.Campaigns
{
    public class CampaignRepository : GvuzRepository, ICampaignRepository
    {
        public CampaignRepository() : base()
        {
            
        }
        public IEnumerable<GDDMC.Campaign> GetCampaigns(int institutionId)
        {
            return DbConnection(db =>
            {
                return db.Query<GDDMC.Campaign>(sql: SQLQuery.GetCampaign, param: new { InstitutionID = institutionId });
            });
        }
        public IEnumerable<GDDMD.CampaignTypesView> GetEditCampaignTypes(int institutionId, int yearStart)
        {
            return DbConnection(db =>
            {
                return db.Query<GDDMD.CampaignTypesView>(sql: SQLQuery.GetEditCampaignTypes, param: new { InstitutionID = institutionId, YearStart = yearStart });
            });
        }
        public IEnumerable<GDDMC.CampaignEducationLevel> GetCampaignEducationLevel(int campaignId)
        {
            return DbConnection(db =>
            {
                return db.Query<GDDMC.CampaignEducationLevel>(sql: SQLQuery.GetCampaignEducationLevel, param: new { CampaignID = campaignId });
            });
        }
        public IEnumerable<GDDMD.AdmissionItemTypeView> GetAdmissionItemType()
        {
            return DbConnection(db =>
            {
                return db.Query<GDDMD.AdmissionItemTypeView>(sql: SQLQuery.GetAdmissionItemType);
            });
        }
        //public IEnumerable<string> GetLevelsEducation(int campaignId)
        //{
        //    return DbConnection(db =>
        //    {
        //        return db.Query<string>(sql: SQLQuery.GetLevelsEducation, param: new { CampaignID = campaignId });
        //    });
        //}
        public GDDVC.CampaignViewModel GetCampaignList(int institutionId)
        {
            GDDVC.CampaignViewModel model = new GDDVC.CampaignViewModel();
            return DbConnection(db =>
            {
                var campaignList = db.Query<GDDVC.CampaignViewModel.CampaignDataModel>(sql: SQLQuery.GetCampaignList, param: new { InstitutionID = institutionId });
                var CampaignTypes = db.Query<Dapper.ViewModel.Dictionary.CampaignTypesView>(sql: @"SELECT ct.CampaignTypeID, ct.Name FROM CampaignTypes AS ct WHERE ct.CampaignTypeID NOT IN(SELECT DISTINCT c.CampaignTypeID FROM Campaign AS c)");

                model.isCountCampaignType = !CampaignTypes.Any();

                //foreach (var cl in campaignList)
                //{
                //    cl.LevelsEducation = string.Join(",", GetLevelsEducation(cl.CampaignID));
                //    
                //    //cl.isPresentInLicense = 
                //}
                model.CampaignList = campaignList;
                model.CampaignTypes = Dictionary.DictionaryContext.Dictionaries.GetCampaignTypes();
                return model;
            });
        }
        public async Task<IEnumerable<GDDVC.CampaignViewModel.CampaignDataModel>> GetCampaignListAsync(int institutionId)
        {
            return await DbConnectionAsync(async db =>
            {
                return await db.QueryAsync<GDDVC.CampaignViewModel.CampaignDataModel>(sql: SQLQuery.GetCampaignList, commandType: System.Data.CommandType.Text, param: new { InstitutionID = institutionId });
            });
        }
        public GDDVC.CampaignViewModel FillCampaignEditModel(int? campaignId, int? institutionId)
        {

            int yearStart;
            int years;
            if (!Int32.TryParse(ConfigurationManager.AppSettings["CampaignYearRangeStart"], out yearStart) || yearStart < 2000)
            {
                yearStart = 2012;
            }
            if (!Int32.TryParse(ConfigurationManager.AppSettings["CampaignYearRangeLength"], out years) || years < 1)
            {
                years = 10;
            }
            return DbConnection(db =>
            {
                GDDVC.CampaignViewModel model = new GDDVC.CampaignViewModel();
                model.CampaignEdit = new GDDVC.CampaignViewModel.CampaignEditModel();
                if (campaignId.HasValue)
                {
                    model.CampaignEdit = db.Query<GDDVC.CampaignViewModel.CampaignEditModel>(sql: SQLQuery.GetCampaignById, param:
                        new
                        {
                            InstitutionID = institutionId.HasValue ? (object)institutionId.Value : DBNull.Value,
                            CampaignID = campaignId
                        }).FirstOrDefault();

                    if (model != null)
                    {
                        model.CampaignEdit.CanEdit = model.CampaignEdit.StatusID != GDDVC.CampaignStatusType.Finished;
                        model.CampaignEdit.CampaignEducationLevel = db.Query<GDDMC.CampaignEducationLevel>(sql: SQLQuery.GetCampaignEducationLevel, param: new { CampaignID = campaignId });
                        model.CampaignEdit.CanChangeType = model.CampaignEdit.CampaignEducationLevel == null || model.CampaignEdit.CampaignEducationLevel.All(t => t.CanRemove);

                        model.CampaignEdit.UsedEducationFormFlags = db.Query<int>(sql: SQLQuery.GetCampaignUsedEducationForms, param: new { CampaignID = campaignId }).First();
                    }
                    else
                    {
                        model.CampaignEdit.CanEdit = true;
                        model.CampaignEdit.CanChangeType = true;
                        model.CampaignEdit.UsedEducationFormFlags = 0;
                    }
                }
                else
                {
                    model.CampaignEdit.CanEdit = true;
                    model.CampaignEdit.CanChangeType = true;
                    model.CampaignEdit.UsedEducationFormFlags = 0;
                }
                model.CampaignEdit.LevelsEducationNames = Dictionary.DictionaryContext.Dictionaries.GetEduLevelsToCampaignTypes();
                model.CampaignEdit.YearRange = Enumerable.Range(yearStart, years).Select(x => new { ID = x, Name = x.ToString(CultureInfo.InvariantCulture) }).ToArray();
                model.CampaignTypes = Dictionary.DictionaryContext.Dictionaries.GetCampaignTypes();
                return model;
            });
        }

        public bool ValidateUpdateCampaign(GDDVC.CampaignViewModel.CampaignEditModel model, ModelStateDictionary errors)
        {
            return DbConnection(db =>
            {
                var CampaignList = GetCampaignList(model.InstitutionID);
                if (CampaignList.CampaignList.Any(c => c.InstitutionID == model.InstitutionID && c.CampaignName == model.CampaignName && c.CampaignID != model.CampaignID))
                {
                    errors.AddModelError("CampaignName", "Существует кампания с данным именем");
                }
                if (CampaignList.CampaignList.Any(c => c.InstitutionID == model.InstitutionID && c.UID == model.UID && c.CampaignID != model.CampaignID))
                {
                    if (!string.IsNullOrEmpty(model.UID))
                    {
                        errors.AddModelError("UID", "Существует кампания с данным UID'ом");
                    }                    
                }
                if (CampaignList.CampaignList.Any(c => c.InstitutionID == model.InstitutionID 
                        && c.YearStart == model.YearStart 
                        && c.YearEnd == model.YearEnd
                        && c.CampaignTypeID == model.CampaignTypeID
                        && c.CampaignID != model.CampaignID))
                {
                    errors.AddModelError("Select_CampaignType", "Существует кампания с таким же сроком проведения и типом");
                }
                //if (CampaignList.CampaignList.Any(c => c.YearStart == model.YearStart && c.CampaignTypeID == model.CampaignTypeID))
                //{
                //    errors.AddModelError("CampaignType", "В рамках одного года может быть сформирована приемная кампания, только с одним типом ПК");
                //}
                return errors.IsValid;
            });
        }
        public int UpdateCampaign(GDDVC.CampaignViewModel.CampaignEditModel model)
        {
            return DbConnection(db =>
            {
                IDbTransaction tran = null;
                try
                {
                    tran = db.BeginTransaction();
                    var campaignID = db.Query<int>(sql: SQLQuery.UpdateCampaign,
                        param: new
                        {
                            CampaignID = model.CampaignID,
                            InstitutionID = model.InstitutionID,
                            Name = model.CampaignName,
                            YearStart = model.YearStart,
                            YearEnd = model.YearEnd,
                            EducationFormFlag = model.EducationFormFlag,
                            StatusID = model.StatusID,
                            UID = model.UID,
                            CampaignTypeID = model.CampaignTypeID
                        }, 
                        transaction: tran);
                    foreach (var elct in model.LevelsEducationNames)
                    {
                        var updateEducationLevel = db.Query<GDDMC.CampaignEducationLevel>(sql: SQLQuery.UpdateCampaignEducationLevel,
                            param: new
                            {
                                CampaignID = model.CampaignID != 0 ? model.CampaignID : campaignID.First(),
                                Course = 1,
                                EducationLevelID = elct.ItemTypeID,
                                //PresentInLicense = null
                            },
                            transaction: tran);
                    }
                    tran.Commit();
                    return campaignID.First();
                }
                catch
                {
                    if (tran != null)
                        tran.Rollback();
                    throw;
                }
            });
        }
        public GDDVC.CampaignViewModel.CampaignDataModel SwitchCampaignStatus(int institutionId, int campaignId)
        {
            return DbConnection(db =>
            {
                var campaign = db.Query<GDDMC.Campaign>(sql: SQLQuery.GetCampaign, param: new { InstitutionID = institutionId, CampaignID = campaignId }).FirstOrDefault();
                if (campaign.StatusID == GDDVC.CampaignStatusType.NotStart || campaign.StatusID == GDDVC.CampaignStatusType.Finished)
                    campaign.StatusID = GDDVC.CampaignStatusType.Started;
                else {
                    campaign.StatusID = GDDVC.CampaignStatusType.Finished;
                }
                db.Query(sql: @"UPDATE Campaign SET StatusID = @StatusID WHERE CampaignID = @CampaignID AND InstitutionID = @InstitutionID", param: new { StatusID = campaign.StatusID, CampaignID = campaign.CampaignID, InstitutionID = institutionId });

                var result = db.Query<GDDVC.CampaignViewModel.CampaignDataModel>(sql: 
                SQLQuery.GetCampaignList, param: new { InstitutionID = institutionId }).Where(c => c.CampaignID == campaignId).FirstOrDefault();
                return result;

        

            });
        }
    }
}