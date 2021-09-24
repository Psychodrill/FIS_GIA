using GVUZ.DAL.Dapper.Repository.Interfaces.CompetitiveGroups;
using System;
using System.Collections.Generic;
using GVUZ.DAL.Dapper.ViewModel.CompetitiveGroups;
using CGM = GVUZ.DAL.Dapper.Model.CompetitiveGroups;
using Dapper;
using System.Linq;
using System.Globalization;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using GVUZ.DAL.Dapper.ViewModel.Campaign;
using GVUZ.DAL.Dapper.Model.TargetOrganization;
using GVUZ.DAL.Dapper.ViewModel.Dictionary;
using SqlClient = System.Data.SqlClient;
using GVUZ.DAL.Dapper.Model.Campaigns;
using GVUZ.DAL.Dapper.ViewModel.Common;
using GVUZ.DAL.Dapper.Model.Benefit;
using GVUZ.DAL.Dapper.ViewModel.Olympic;


namespace GVUZ.DAL.Dapper.Repository.Model.CompetitiveGroups
{
    public class CompetitiveGroupRepository : GvuzRepository, ICompetitiveGroupRepository
    {
        public CompetitiveGroupRepository() : base()
        {

        }

        

        public CompetitiveGroupViewModel GetCompetitiveGroupList(int institutionId)
        {
            CompetitiveGroupViewModel model = new CompetitiveGroupViewModel();
            return DbConnection(db =>
            {
                model.CompetitiveGroupList = db.Query<CompetitiveGroupViewModel.CompetitiveGroupDataModel>(sql: SQLQuery.GetCompetitiveGroupList, param: new { InstitutionID = institutionId });

                IEnumerable<int> campaignStartYears = db.Query<int>(sql: SQLQuery.GetCampaignStartYears, param: new { InstitutionID = institutionId });
                model.CampaignStartYears = campaignStartYears.Select(x => new { ID = x, Name = x.ToString(CultureInfo.InvariantCulture) }).ToArray();
                model.CampaignTypes = Dictionary.DictionaryContext.Dictionaries.GetCampaignTypes();
                model.EducationLevels = Dictionary.DictionaryContext.Dictionaries.GetEducationLevels();
                model.EducationFinanceSources = Dictionary.DictionaryContext.Dictionaries.GetEducationFinanceSources();
                model.EducationForms = Dictionary.DictionaryContext.Dictionaries.GetEducationForms();
                model.LevelBudgets = Dictionary.DictionaryContext.Dictionaries.GetLevelBudget();
                return model;
            });
            
        }

        public IEnumerable<CGM.CompetitiveGroup> GetCompetitiveGroups(int institutionId)
        {
            throw new NotImplementedException();
        }

        public ValidateCompetitiveGroup DeleteCompetitiveGroup(int competitiveGroupID)
        {
            return DbConnection(db =>
            {
                var result = db.Query<ValidateCompetitiveGroup>(sql: SQLQuery.DeleteCompetitiveGroup, param: new { CompetitiveGroupID = competitiveGroupID });

                return result.Any() ? result.First() : null;
            });
        }

        public CompetitiveGroupViewModel FillCompetitiveGroupEditModel(int? competitiveGroupId, int? institutionId, bool IsMultiProfile = false)
        {
            return DbConnection(db =>
            {
                
                CompetitiveGroupViewModel model = new CompetitiveGroupViewModel();
                model.CompetitiveGroupEdit = new CompetitiveGroupViewModel.CompetitiveGroupEditModel();                
                IEnumerable<int> campaignStartYears = db.Query<int>(sql: SQLQuery.GetCampaignStartYears, param: new { InstitutionID = institutionId });
                model.CampaignStartYears = campaignStartYears.Select(x => new { ID = x, Name = x.ToString(CultureInfo.InvariantCulture) }).ToArray();
                model.EducationFinanceSources = Dictionary.DictionaryContext.Dictionaries.GetEducationFinanceSources();
                model.EducationForms = Dictionary.DictionaryContext.Dictionaries.GetEducationForms();

                model.Campaigns = db.Query<CampaignWithTypeViewModel>(sql: SQLQuery.GetCampaignWithTypes, param: new { InstitutionID = institutionId });   //Dictionary.DictionaryContext.Dictionaries.GetCampaignTypes();
                //model.EducationLevels = Dictionary.DictionaryContext.Dictionaries.GetEducationLevels();

                model.Directions = db.Query<DirectionViewModel>(sql: SQLQuery.GetDirectionsByInstitutionAndEducationLevel, param: new { InstitutionID = institutionId });   //Dictionary.DictionaryContext.Dictionaries.GetCampaignTypes();
                model.Subjects = Dictionary.DictionaryContext.Dictionaries.GetSubjects();
                model.LevelBudgets = Dictionary.DictionaryContext.Dictionaries.GetLevelBudget();
                model.IsMultiProfile = IsMultiProfile;
                

                if (competitiveGroupId.HasValue)
                {
                   
                    model.CompetitiveGroupEdit = db.Query<CompetitiveGroupViewModel.CompetitiveGroupEditModel>(sql: SQLQuery.GetCompetitiveGroupById, param:
                        new
                        {
                            CompetitiveGroupID = competitiveGroupId,
                            InstitutionID = institutionId
                        }).First();

                }

                if (model.CompetitiveGroupEdit == null)
                    model.CompetitiveGroupEdit = new CompetitiveGroupViewModel.CompetitiveGroupEditModel();



                model.CompetitiveGroupEdit.Uids = db.Query<UidViewModel>(sql: SQLQuery.GetCompetitiveGroupUIDs, param: new { InstitutionID = institutionId.Value });

                FillCompetitiveGroupPrograms(model, institutionId.Value, competitiveGroupId, db);
                FillCompetitiveGroupTargets(model, institutionId.Value, competitiveGroupId, db);
                FillEntranceTestData(model, institutionId.Value, db);
                FillPropertiesData(model, competitiveGroupId, db);

                model.CompetitiveGroupEdit.Value = GetValue(model);

                // Проверка возможности редактирования Конкурса:
                // Есть ли заявления со статусом 8 (в приказе) - тоогда можно редактировать только КЦП или другими статусами - тогда можно редактировать все, но аккуртано 
                var hasApplicationStatus8 = db.Query<ApplicationStatus8Check>(sql: SQLQuery.CheckCompetitiveGroupCanBeEdited, param:
                    new
                    {
                        CompetitiveGroupID = competitiveGroupId
                    }).First();
                model.CompetitiveGroupEdit.ApplicationState8 = hasApplicationStatus8.ApplicationState8;
                model.CompetitiveGroupEdit.ApplicationStateNot8 = hasApplicationStatus8.ApplicationStateNot8;

                model.CompetitiveGroupEdit.IsMVD = Dictionary.DictionaryContext.Dictionaries.CheckIsMVD(Dictionary.DictionaryContext.Dictionaries.IsMVD(institutionId.Value));

                model.CompetitiveGroupEdit.CanEdit = model.CompetitiveGroupEdit.CampaignStatusID != CampaignStatusType.Finished;

                model.CompetitiveGroupEdit.OlympicValidityYears = 5;//#FIS-1723 Срок действия олимпиад - текущий + 4 предыдущих года



                return model;
            });
        }

        private void FillPropertiesData(CompetitiveGroupViewModel model, int? competitiveGroupId, IDbConnection db)
        {
            if (competitiveGroupId.HasValue)
                model.CompetitiveGroupEdit.Properties = db.Query<CompetitiveGroupProperty>(string.Format(@"SELECT PropertyTypeCode, 
                                                                                                                  PropertyValue
                                                                                                                FROM dbo.CompetitiveGroupProperties (NOLOCK)
                                                                                                                Where CampaignId= {0} 
                                                                                                                AND CompetitiveGroupID = {1}", 

                                                                                                                model.CompetitiveGroupEdit.CampaignID,
                                                                                                                competitiveGroupId));

        }

        private class ApplicationStatus8Check { public int ApplicationState8 { get; set; } public int ApplicationStateNot8 { get; set; } }

        private int GetValue(CompetitiveGroupViewModel model)
        {
            int res = 0;
            switch (model.CompetitiveGroupEdit.EducationSourceID)
            {
                case GVUZ.DAL.Dapper.ViewModel.Dictionary.EDSourceConst.Budget:
                    switch (model.CompetitiveGroupEdit.EducationFormID)
                    {
                        case GVUZ.DAL.Dapper.ViewModel.Dictionary.EDFormsConst.O:
                            return model.CompetitiveGroupEdit.NumberBudgetO;
                        case GVUZ.DAL.Dapper.ViewModel.Dictionary.EDFormsConst.OZ:
                            return model.CompetitiveGroupEdit.NumberBudgetOZ;
                        case GVUZ.DAL.Dapper.ViewModel.Dictionary.EDFormsConst.Z:
                            return model.CompetitiveGroupEdit.NumberBudgetZ;
                    };
                    break;
                case GVUZ.DAL.Dapper.ViewModel.Dictionary.EDSourceConst.Paid:
                    switch (model.CompetitiveGroupEdit.EducationFormID)
                    {
                        case GVUZ.DAL.Dapper.ViewModel.Dictionary.EDFormsConst.O:
                            return model.CompetitiveGroupEdit.NumberPaidO;
                        case GVUZ.DAL.Dapper.ViewModel.Dictionary.EDFormsConst.OZ:
                            return model.CompetitiveGroupEdit.NumberPaidOZ;
                        case GVUZ.DAL.Dapper.ViewModel.Dictionary.EDFormsConst.Z:
                            return model.CompetitiveGroupEdit.NumberPaidZ;
                    };
                    break;
                case GVUZ.DAL.Dapper.ViewModel.Dictionary.EDSourceConst.Quota:
                    switch (model.CompetitiveGroupEdit.EducationFormID)
                    {
                        case GVUZ.DAL.Dapper.ViewModel.Dictionary.EDFormsConst.O:
                            return model.CompetitiveGroupEdit.NumberQuotaO;
                        case GVUZ.DAL.Dapper.ViewModel.Dictionary.EDFormsConst.OZ:
                            return model.CompetitiveGroupEdit.NumberQuotaOZ;
                        case GVUZ.DAL.Dapper.ViewModel.Dictionary.EDFormsConst.Z:
                            return model.CompetitiveGroupEdit.NumberQuotaZ;
                    };
                    break;
                case GVUZ.DAL.Dapper.ViewModel.Dictionary.EDSourceConst.Target:

                    switch (model.CompetitiveGroupEdit.EducationFormID)
                    {
                        case GVUZ.DAL.Dapper.ViewModel.Dictionary.EDFormsConst.O:
                            if (model.CompetitiveGroupEdit.NumberTargetO > 0)
                                return model.CompetitiveGroupEdit.NumberTargetO;

                            foreach (var target in model.CompetitiveGroupTargetsEdit.Targets)
                            {
                                res += target.NumberTargetO;
                            }
                            return res;

                        case GVUZ.DAL.Dapper.ViewModel.Dictionary.EDFormsConst.OZ:
                            if (model.CompetitiveGroupEdit.NumberTargetOZ > 0)
                                return model.CompetitiveGroupEdit.NumberTargetOZ;

                            foreach (var target in model.CompetitiveGroupTargetsEdit.Targets)
                            {
                                res += target.NumberTargetOZ;
                            }
                            return res;

                        case GVUZ.DAL.Dapper.ViewModel.Dictionary.EDFormsConst.Z:
                            if (model.CompetitiveGroupEdit.NumberTargetZ > 0)
                                return model.CompetitiveGroupEdit.NumberTargetZ;

                            foreach (var target in model.CompetitiveGroupTargetsEdit.Targets)
                            {
                                res += target.NumberTargetZ;
                            }
                            return res;
                    };

                    break;
            }
            return 0; 
        }


        private void FillCompetitiveGroupPrograms(CompetitiveGroupViewModel model, int institutitionID, int? competitiveGroupID, IDbConnection db)
        {
            if (model.CompetitiveGroupProgramsEdit == null)
                model.CompetitiveGroupProgramsEdit = new CompetitiveGroupViewModel.CompetitiveGroupProgramsEditModel();

            model.CompetitiveGroupProgramsEdit.InstitutionPrograms = db.Query<CGM.CompetitiveGroupInstitutionProgram>(sql: SQLQuery.GetCompetitiveGroupProgramsByOO, param: new { InstitutionID = institutitionID });
            if (competitiveGroupID.HasValue)
            { 
                model.CompetitiveGroupProgramsEdit.Programs = db.Query<CGM.CompetitiveGroupProgram>(sql: SQLQuery.GetCompetitiveGroupProgramsByCompetitiveGroup, param: new { CompetitiveGroupID = competitiveGroupID.Value });
            }
        }

        private void FillCompetitiveGroupTargets(CompetitiveGroupViewModel model, int institutitionID, int? competitiveGroupID, IDbConnection db)
        {
            if (model.CompetitiveGroupTargetsEdit == null)
                model.CompetitiveGroupTargetsEdit = new CompetitiveGroupViewModel.CompetitiveGroupTargetsEditModel();

            model.CompetitiveGroupTargetsEdit.TargetOrganizations = db.Query<CompetitiveGroupTargetItemViewModel>(sql: SQLQuery.GetCompetitiveGroupTargets, param: new { InstitutionID = institutitionID });
            if (competitiveGroupID.HasValue)
                model.CompetitiveGroupTargetsEdit.Targets = db.Query<CompetitiveGroupTargetItemViewModel>(sql: SQLQuery.GetCompetitiveGroupTargetItemsByCG, param: new { CompetitiveGroupID = competitiveGroupID.Value });
        }
        private void FillEntranceTestData(CompetitiveGroupViewModel model, int institutitionID, IDbConnection db)
        {
            if (model.EntranceTestItemsEdit == null)
                model.EntranceTestItemsEdit = new CompetitiveGroupViewModel.EntranceTestItemsEditModel();

            var competitiveGroup = model.CompetitiveGroupEdit;

            // загружаем ВИ
            var allTestItems = db.Query<EntranceTestItemDataViewModel>(sql: SQLQuery.GetEntranceTestsByCompetitiveGroup, param: new { CompetitiveGroupID = competitiveGroup.CompetitiveGroupID });

            model.EntranceTestItemsEdit.Uids = db.Query<UidViewModel>(sql: SQLQuery.GetEntranceTestItemCUids, param: new { InstitutionID = institutitionID });


#warning вынести в SQLQuery
            var benefitItems = db.Query<BenefitItemViewModel>(
                sql: @"SELECT 
                        b.BenefitItemID
                        ,b.EntranceTestItemID
                        ,b.EgeMinValue
                        ,b.UID
                        ,odt.OlympicDiplomTypeID 
                        ,odt.Name as DiplomType
                        ,b.OlympicYear
                        ,b.IsForAllOlympic

                        ,b.OlympicLevelFlags
                        ,b.ClassFlags
                        ,b.BenefitID
                        ,bf.Name as BenefitName
                        ,b.IsCreative
                        ,b.IsAthletic
                        FROM BenefitItemC b
                        left join OlympicDiplomType  odt on odt.OlympicDiplomTypeID = b.OlympicDiplomTypeID
                        left join Benefit bf on b.BenefitID = bf.BenefitID
                        Where b.CompetitiveGroupID = @CompetitiveGroupID"
                , param: new
                {
                    CompetitiveGroupID = competitiveGroup.CompetitiveGroupID
                });

            foreach(var bi in benefitItems)
            {
                bi.BenefitItemSubjects = db.Query<BenefitItemSubjectViewModel>(
                    sql: @"Select bis.Id, bis.SubjectID, bis.EgeMinValue, s.Name as SubjectName, s.IsEge, s.IsOlympic
                            From BenefitItemSubject bis 
                            left join Subject s on s.SubjectID = bis.SubjectId
                            Where bis.BenefitItemID = @BenefitItemID;"
                    , param: new
                    {
                        BenefitItemID = bi.BenefitItemID
                    });

                bi.BenefitItemOlympics = db.Query<BenefitItemOlympicViewModel>(
                   sql: @"Select 
                            bicot.ID
                            ,ot.OlympicID
                            ,ot.OlympicNumber
                            ,ot.Name
                            ,bicot.OlympicLevelFlags
                            ,bicot.ClassFlags
                            ,ot.OlympicYear
                            ,isnull(Profiles.ProfileName, '') as ProfileNames
                            From BenefitItemCOlympicType bicot 
                            left join OlympicType ot on ot.OlympicID = bicot.OlympicTypeID
                            left join (
                            Select Profiles.[BenefitItemCOlympicTypeID],
                                   Left(Profiles.ProfileName,Len(Profiles.ProfileName)-1) As ProfileName
                            From
                                (
                                    Select distinct bicotp2.[BenefitItemCOlympicTypeID],
                                        (
                                            Select op1.ProfileName + ',' AS [text()]
                                            From OlympicProfile op1
				                            left join BenefitItemCOlympicTypeProfile  bicotp on bicotp.OlympicProfileID = op1.OlympicProfileID
				                            Where bicotp.BenefitItemCOlympicTypeID = bicotp2.BenefitItemCOlympicTypeID
                                            For XML PATH ('')
                                        ) as ProfileName
		                            From BenefitItemCOlympicTypeProfile bicotp2
                                ) Profiles ) Profiles on Profiles.BenefitItemCOlympicTypeID = bicot.ID
                            Where bicot.BenefitItemID = @BenefitItemID"
                   , param: new
                   {
                       BenefitItemID = bi.BenefitItemID
                   });

                foreach (var item in bi.BenefitItemOlympics)
                {
                    item.BenefitItemOlympicProfiles = db.Query<BenefitItemProfileViewModel>(
                       sql: @"--declare @BenefitItemCOlympicTypeID int = 2;
                                SELECT 
	                                [BenefitItemCOlympicTypeProfileID] as ID
                                    ,op.[OlympicProfileID] as OlympicProfileID
	                                ,op.ProfileName
                                FROM [BenefitItemCOlympicTypeProfile] bicotp
                                left join OlympicProfile op ON op.OlympicProfileID = bicotp.OlympicProfileID
                                Where bicotp.BenefitItemCOlympicTypeID = @BenefitItemCOlympicTypeID;"
                        , param: new
                        {
                            BenefitItemCOlympicTypeID = item.ID
                        });
                }

                bi.BenefitItemProfiles = db.Query<BenefitItemProfileViewModel>(
                   sql: @"Select bip.BenefitItemCProfileID as ID, bip.OlympicProfileID, op.ProfileName
                            From [BenefitItemCProfile] bip
                            left join OlympicProfile op on bip.OlympicProfileID = op.OlympicProfileID
                            Where bip.BenefitItemID = @BenefitItemID"
                    , param: new
                    {
                        BenefitItemID = bi.BenefitItemID
                    });

                //if (bi.EntranceTestItemID == 0)
                //{
                //    if (model.EntranceTestItemsEdit.Benefits == null)
                //        model.EntranceTestItemsEdit.Benefits = new List<BenefitItemViewModel>();
                //    model.EntranceTestItemsEdit.Benefits.Add()
                //}
                //else
                //{

                //}
            }

            model.EntranceTestItemsEdit.BenefitItems = benefitItems.Where(t => t.EntranceTestItemID == 0);
            foreach(var test in allTestItems)
            {
                test.BenefitItems = benefitItems.Where(t => t.EntranceTestItemID == test.ItemID);
            }

            model.EntranceTestItemsEdit.TestItems = allTestItems.Where(t => t.TestType == EntranceTestItemDataViewModel.EntranceTestType.MainType);
            model.EntranceTestItemsEdit.CreativeTestItems = allTestItems.Where(t => t.TestType == EntranceTestItemDataViewModel.EntranceTestType.CreativeType);
            model.EntranceTestItemsEdit.ProfileTestItems = allTestItems.Where(t => t.TestType == EntranceTestItemDataViewModel.EntranceTestType.ProfileType);

            // Загрузить справочники
            model.EntranceTestItemsEdit.BenefitList = Dictionary.DictionaryContext.Dictionaries.GetBenefits();

            model.EntranceTestItemsEdit.OlympicLevels = Dictionary.DictionaryContext.Dictionaries.GetOlympicLevels();
            model.EntranceTestItemsEdit.OlympicDiplomTypes = Dictionary.DictionaryContext.Dictionaries.GetOlympicDiplomTypes();
            model.EntranceTestItemsEdit.OlympicProfiles = Dictionary.DictionaryContext.Dictionaries.GetOlympicProfiles();

            model.EntranceTestItemsEdit.GlobalMinEge = Dictionary.DictionaryContext.Dictionaries.GetGlobalMinEge();

            //model.EntranceTestItemsEdit.OlympicTypes = Dictionary.DictionaryContext.Dictionaries.GetOlympicTypes();


            // Загрузить олимпиады с профилями
            // Так нельзя, очень долго выполняется!
            //foreach (var olympic in model.EntranceTestItemsEdit.OlympicTypes) {
            //    olympic.OlympicTypeProfiles = db.Query<ViewModel.Olympic.OlympicTypeProfileViewModel>(
            //        sql: @"select otp.OlympicLevelID, otp.OlympicProfileID, op.ProfileName
            //                  from OlympicTypeProfile otp
            //                  left join OlympicProfile op on otp.OlympicProfileID = op.OlympicProfileID
            //                  Where otp.OlympicTypeID = @OlympicTypeID"
            //        , param: new
            //        {
            //            OlympicTypeID = olympic.OlympicID
            //        });
            //}

            var olympics = db.Query<OlympicTypeViewModel, OlympicTypeProfileViewModel, OlympicTypeViewModel>(
                    sql: @"SELECT
                            ot.[OlympicID]
                            ,ot.[Name]
                            ,ot.[OlympicNumber]
                            ,ot.[OlympicYear]
                            ,otp.OlympicTypeID
                            ,otp.[OlympicTypeProfileID]
                            ,otp.OlympicLevelID
                            ,otp.OlympicProfileID
                            ,op.ProfileName
                              FROM [OlympicType] ot
                              left join OlympicTypeProfile otp on ot.OlympicID = otp.OlympicTypeID
                              left join OlympicProfile op on otp.OlympicProfileID = op.OlympicProfileID
                              Order by OlympicYear, OlympicNumber"
                    //, param: new
                    //{
                    //    CompetitiveGroupID = competitiveGroup.CompetitiveGroupID
                    //}
                    ,
                        map: (olympicType, olympicTypeProfile) =>
                        {
                            if (olympicTypeProfile != null)
                            {
                                olympicType.OlympicTypeProfiles.Add(olympicTypeProfile);
                                //benefitItemCOlympicType.BenefitItemC = benefitItemC;
                            }
                            return olympicType;
                        }
                        , splitOn: "OlympicTypeID"
                );
            model.EntranceTestItemsEdit.OlympicTypes = new List<OlympicTypeViewModel>();
            foreach (var olympic in olympics)
            {
                var o = model.EntranceTestItemsEdit.OlympicTypes.Where(t => t.OlympicID == olympic.OlympicID).FirstOrDefault();
                if (o != null)
                {
                    if (o.OlympicTypeProfiles == null)
                        o.OlympicTypeProfiles = new List<OlympicTypeProfileViewModel>();
                    if (olympic.OlympicTypeProfiles != null)
                        foreach (var otp in olympic.OlympicTypeProfiles)
                            o.OlympicTypeProfiles.Add(otp);
                }
                else
                {
                    model.EntranceTestItemsEdit.OlympicTypes.Add(olympic);
                }
            }

            model.EntranceTestItemsEdit.OlympicTypeYears = model.EntranceTestItemsEdit.OlympicTypes.Select(t => t.OlympicYear).Distinct().OrderByDescending(t => t).ToList();

        }

        public dynamic CompetitiveGroupCopy(int[] competitiveGroupIDs, int copy_year, int copy_сampaignType, int copy_levelBudget, int InstitutionID)
        {
            var competitiveGroupsTable = new DataTable();
            competitiveGroupsTable.Columns.Add("id", typeof(int));

            foreach (var id in competitiveGroupIDs)
            {
                competitiveGroupsTable.Rows.Add(id);
            }
            var competitiveGroups = competitiveGroupsTable.AsTableValuedParameter("[Identifiers]");

            IEnumerable<dynamic> result = null;
            return DbConnection(db =>
            {
                result = db.Query<dynamic>(sql: SQLQuery.CopyCompetitiveGroup, param: new { competitiveGroups = competitiveGroups, copy_year = copy_year, copy_сampaignType = copy_сampaignType, copy_levelBudget = copy_levelBudget, InstitutionID = InstitutionID });

                return result;
            });
        }


        public bool ValidateUpdateCompetitiveGroup(CompetitiveGroupViewModel model, ModelStateDictionary modelState)
        {
            return DbConnection(db =>
            {
                // 1. Проверить, что CampaignStatusID != 2 (мог измениться за время редактирования Конкурса)
                var campaign = db.Query<Campaign>(sql: "Select * from Campaign Where CampaignID = @CampaignID", param: new { CampaignID = model.CompetitiveGroupEdit.CampaignID }).FirstOrDefault();
                if (campaign == null)
                {
                    modelState.AddModelError("Select_Campaigns", "Не существует кампании с указанным годом и типом");
                }
                if (campaign.StatusID == CampaignStatusType.Finished)
                {
                    modelState.AddModelError("Select_Campaigns", "Кампания имеет статус 'Завершена'");
                }

                // 2. Конкурс
                // 2.1. Проверить, что Конкурс не дублируется по скалярным полям, программам обучения и целевым организациям
                if (model.CompetitiveGroupEdit.EducationSourceID != EDSourceConst.Paid)
                {
                    DataTable tbPrograms = new DataTable("Programs");
                    tbPrograms.Columns.Add("id", typeof(int));
                    //tbPrograms.Columns.Add("Name", typeof(string));
                    //tbPrograms.Columns.Add("UID", typeof(string));
                    //tbPrograms.Columns.Add("GUID", typeof(Guid));

                    //int i = 0;
                    if (model.CompetitiveGroupProgramsEdit != null && model.CompetitiveGroupProgramsEdit.Programs != null)
                        model.CompetitiveGroupProgramsEdit.Programs.ToList().ForEach(x => tbPrograms.Rows.Add(//i++,
                            x.ProgramID
                           //x.Name,
                           //x.UID,
                           //null
                           ));
                    DataTable tbTargets = new DataTable("Targets");
                    tbTargets.Columns.Add("id", typeof(int));
                    if (model.CompetitiveGroupTargetsEditResult != null)
                        model.CompetitiveGroupTargetsEditResult.ToList().ForEach(x => tbTargets.Rows.Add(x.CompetitiveGroupTargetID));


                    var ProgramsParam = new SqlClient.SqlParameter("@Programs", SqlDbType.Structured);
                    ProgramsParam.TypeName = "dbo.Identifiers";
                    ProgramsParam.Value = tbPrograms;

                    var TargetsParam = new SqlClient.SqlParameter("@Targets", SqlDbType.Structured);
                    TargetsParam.TypeName = "dbo.Identifiers";
                    TargetsParam.Value = tbTargets;

                    var conn = db as SqlClient.SqlConnection;
                    using (SqlClient.SqlCommand cmd = new SqlClient.SqlCommand(SQLQuery.ValidateCompetitiveGroup, conn))
                    {
                        cmd.Parameters.Add(new SqlClient.SqlParameter("@CompetitiveGroupID", model.CompetitiveGroupEdit.CompetitiveGroupID));

                        cmd.Parameters.Add(new SqlClient.SqlParameter("@CampaignID", model.CompetitiveGroupEdit.CampaignID));
                        cmd.Parameters.Add(new SqlClient.SqlParameter("@IsFromKrym", model.CompetitiveGroupEdit.IsFromKrym));
                        cmd.Parameters.Add(new SqlClient.SqlParameter("@IsAdditional", model.CompetitiveGroupEdit.IsAdditional));
                        cmd.Parameters.Add(new SqlClient.SqlParameter("@EducationLevelID", model.CompetitiveGroupEdit.EducationLevelID));
                        cmd.Parameters.Add(new SqlClient.SqlParameter("@EducationFormID", model.CompetitiveGroupEdit.EducationFormID));
                        cmd.Parameters.Add(new SqlClient.SqlParameter("@EducationSourceID", model.CompetitiveGroupEdit.EducationSourceID));
                        cmd.Parameters.Add(new SqlClient.SqlParameter("@DirectionID", model.CompetitiveGroupEdit.DirectionID));
                        cmd.Parameters.Add(new SqlClient.SqlParameter("@IdLevelBudget", SqlDbType.Int) { Value = (model.CompetitiveGroupEdit.IdLevelBudget == null) ? ((object)DBNull.Value) : model.CompetitiveGroupEdit.IdLevelBudget });

                        cmd.Parameters.Add(ProgramsParam);
                        cmd.Parameters.Add(TargetsParam);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var validateCG = new ValidateCompetitiveGroup { Code = (int)reader["Code"], Message = (string)reader["Message"] };
                                if (validateCG.Code != 0)
                                {
                                    modelState.AddModelError("CompetitiveGroupEdit_CompetitiveGroupName", validateCG.Message);
                                }
                            }
                            reader.Close();
                        }
                    }
                }

                // 2.2. Проверить уникальность CompetitiveGroupUID в рамках InstitutionID
                var uids = db.Query<UidViewModel>(sql: SQLQuery.GetCompetitiveGroupUIDs, param: new { InstitutionID = model.CompetitiveGroupEdit.InstitutionID });
                var sameUids = uids.Where(t => t.ID != model.CompetitiveGroupEdit.CompetitiveGroupID && t.UID == model.CompetitiveGroupEdit.Uid);
                if (sameUids.Any())
                {
                    modelState.AddModelError("CompetitiveGroupEdit_Uid", "В данной ОО уже имеется конкурс с таким же UID");
                }

                // 2.3 проверить уникальность имени в рамках ОО и Кампании
                var sql = @"
                    --declare @CampaignID int = 46;
                    select distinct cg.CompetitiveGroupID AS ID, cg.Name as UID
                    From CompetitiveGroup cg (NOLOCK)
                    Where cg.CampaignID = @CampaignID;
                ";
                if (campaign != null)
                {
                    var names = db.Query<UidViewModel>(sql: sql, param: new { CampaignID = campaign.CampaignID });
                    var sameNames = names.Where(t => t.ID != model.CompetitiveGroupEdit.CompetitiveGroupID && t.UID == model.CompetitiveGroupEdit.CompetitiveGroupName);
                    if (sameNames.Any())
                    {
                        modelState.AddModelError("CompetitiveGroupEdit_CompetitiveGroupName", "В данной ОО у выбранной ПК уже имеется конкурс с таким же названием");
                    }
                }

                //var sqlFindParentDirection = @"
                //                              select av.ParentDirectionID from AdmissionVolume av
                //                              join Direction d on d.ParentID = av.ParentDirectionID
                //                              where av.CampaignID = @CampaignID and d.DirectionID = @DirectionID;";

                //var avSetByParentDirection = db.Query<int?>(sql: sqlFindParentDirection,
                //    param: new { CampaignID = campaign.CampaignID, DirectionID = model.CompetitiveGroupEdit.DirectionID }).FirstOrDefault();

                //var sqlCheckKCP = model.CompetitiveGroupEdit.IsMultiProfile || avSetByParentDirection != null ? SQLQuery.ParentDirectionCheckKCP : SQLQuery.CheckKCP;
                string sqlKCP = @"SELECT [ValueAV],[ValueDAV],[ValueCG], [ValueCGDAV] FROM dbo.GetCampaignKCP (@InstitutionId,
                                       @CampaignID,
                                       @ParentDirectionID,
                                       @DirectionID,
                                       @EducationSourceID,
                                       @EducationLevelID,
                                       @EducationFormID,
                                       @IdLevelBudget,
                                       NULL,
                                       @ExcludedCompetitiveGroupID,
                                       @ExcludeApps)";
                // 3. Проверить непревышение КЦП
                //var kcpValue = db.Query<KCPValue>(sql: sqlCheckKCP, param: new
                var kcpValue = db.Query<KCPValue>(sql: sqlKCP, param: new
                {
                    CampaignID = model.CompetitiveGroupEdit.CampaignID,
                    ExcludedCompetitiveGroupID = model.CompetitiveGroupEdit.CompetitiveGroupID,
                    EducationLevelID = model.CompetitiveGroupEdit.EducationLevelID,
                    EducationFormID = model.CompetitiveGroupEdit.EducationFormID,
                    EducationSourceID = model.CompetitiveGroupEdit.EducationSourceID,
                    DirectionID = !model.CompetitiveGroupEdit.IsMultiProfile ? model.CompetitiveGroupEdit.DirectionID : null,
                    ParentDirectionID = model.CompetitiveGroupEdit.IsMultiProfile ? model.CompetitiveGroupEdit.ParentDirectionID : null,
                    IdLevelBudget = model.CompetitiveGroupEdit.IdLevelBudget,
                    InstitutionID = model.CompetitiveGroupEdit.InstitutionID,
                    //CompetitiveGroupID = null,
                    ExcludeApps = 1
                }).FirstOrDefault();


                //var con = db as SqlClient.SqlConnection;
                //using (SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.GetCompetitiveGroupKCP (@ID)", con ))
                //{

                //    cmd.Parameters.AddWithValue("@ID", model.CompetitiveGroupEdit.CompetitiveGroupID);
                //    cmd.CommandTimeout = 120;
                //    using (SqlDataReader reader = cmd.ExecuteReader())
                //    {
                //        KCPValue kcpValue = new KCPValue();
                //        while (reader.Read())
                //        {
                //            //kcpValue.ValueAV = reader.GetInt32(1);

                //                          //records.Add(MapKCPRecord(reader));


                //        }

                //    }
                //}



                if (model.CompetitiveGroupEdit.Value != 0 && kcpValue.ValueAV < kcpValue.ValueCG + model.CompetitiveGroupEdit.Value)
                {
                    modelState.AddModelError("CompetitiveGroupEdit_Value", string.Format("Для данного направления задан объем приема: {0}, значение текущего конкурса не может превышать {1}!", kcpValue.ValueAV, Math.Max(0, kcpValue.ValueAV - kcpValue.ValueCG)));
                }

                // FIS-1790 - проверка на непревышение количества мест по конкретному уровню бюджета.
                if (model.CompetitiveGroupEdit.IdLevelBudget != null)
                {
                    if (model.CompetitiveGroupEdit.Value != 0 && kcpValue.ValueDAV < kcpValue.ValueCGDAV + model.CompetitiveGroupEdit.Value)
                    {
                        modelState.AddModelError("CompetitiveGroupEdit_Value", string.Format("Для данного направления задан распределенный объем приема: {0}, значение текущего конкурса не может превышать {1}!", kcpValue.ValueDAV, Math.Max(0, kcpValue.ValueDAV - kcpValue.ValueCGDAV)));
                    }
                }

                // 4. Проверить ВИ
                var testsAndBenefits = model.GetTestsAndBenefits();
                List<EntranceTestItemDataViewModel> allTests = testsAndBenefits.Item1;
                List<BenefitItemViewModel> allBenefitItems = testsAndBenefits.Item2;
                

                // 4.1. Что среди других ВИ данного Institution нет таких UID (для непустых)
                uids = db.Query<UidViewModel>(sql: SQLQuery.GetEntranceTestItemCUids, param: new { InstitutionID = model.CompetitiveGroupEdit.InstitutionID });
                foreach (var testItem in allTests)
                {
                    sameUids = uids.Where(t => t.ID != testItem.ItemID && t.UID == testItem.UID);
                    if (sameUids.Any())
                    {
                        //modelState.AddModelError("commonErrors", string.Format("В данной ОО уже имеется ВИ с таким же UID: {0}", testItem.UID));
                        modelState.AddModelError("CompetitiveGroupEdit_CompetitiveGroupName", string.Format("В данной ОО уже имеется ВИ с таким же UID: {0}, как у ВИ: {1} ", testItem.UID, testItem.TestName));
                    }
                }

                // 4.2. Что не меняем ВИ, на которое уже есть заявление (ApplicationEntranceTestDocument)


                // 4.3 Что у всех ВИ заданы приоритеты и они уникальны
                if (!model.CompetitiveGroupEdit.IsMVD &&
                    model.CompetitiveGroupEdit.EducationLevelID != EDLevelConst.SPO &&
                    ((model.CompetitiveGroupEdit.EducationLevelID == EDLevelConst.HighQualification && allTests.Any() && !allTests.Any(t => t.EntranceTestPriority.HasValue))
                    || (model.CompetitiveGroupEdit.EducationLevelID != EDLevelConst.HighQualification &&  allTests.Any(t=> !t.EntranceTestPriority.HasValue))))
                    modelState.AddModelError("CompetitiveGroupEdit_CompetitiveGroupName", string.Format("У всех ВИ должен быть задан приоритет"));

                // 4.4 Одинаковый приоритет у 2 и более ВИ, причем отдельно по основным и заменам
                //foreach (var group in allTests.Where(t => !t.IsForSPOandVO && t.EntranceTestPriority.HasValue && t.EntranceTestPriority.Value != -1).GroupBy(t => t.EntranceTestPriority).Where(t => t.Count() > 1))
                //{
                //    modelState.AddModelError("CompetitiveGroupEdit_CompetitiveGroupName", string.Format("У двух или более ВИ поставлен одинаковый приоритет {0}. Приоритет должен быть уникальным", group.Key.Value));
                //}
                //foreach (var group in allTests.Where(t => t.IsForSPOandVO && t.EntranceTestPriority.HasValue && t.EntranceTestPriority.Value != -1).GroupBy(t => t.EntranceTestPriority).Where(t => t.Count() > 1))
                //{
                //    modelState.AddModelError("CompetitiveGroupEdit_CompetitiveGroupName", string.Format("У двух или более ВИ поставлен одинаковый приоритет {0}. Приоритет должен быть уникальным", group.Key.Value));
                //}


                //// Одинаковый приоритет у 2 и более ВИ
                //foreach (var group in allTests.Where(t => !t.IsFirst && t.EntranceTestPriority.HasValue && t.EntranceTestPriority.Value != -1).GroupBy(t => t.EntranceTestPriority).Where(t => t.Count() > 1))
                //{
                //    modelState.AddModelError("CompetitiveGroupEdit_CompetitiveGroupName", string.Format("У двух или более ВИ поставлен одинаковый приоритет {0}. Приоритет должен быть уникальным", group.Key.Value));
                //}              

                // 5. Проверить льготы
                // 5.1 Что у данной Campaign нет больше льгот с таким UID
                if (campaign != null)
                {
                    uids = db.Query<UidViewModel>(sql: SQLQuery.GetBenefitUIDs, param: new { CampaignID = campaign.CampaignID });
                    foreach (var benefitItem in allBenefitItems)
                    {
                        sameUids = uids.Where(t => t.ID != benefitItem.BenefitItemID && t.UID == benefitItem.UID);
                        if (sameUids.Any())
                        {
                            var viName = benefitItem.EntranceTestItemID != 0 ? 
                                allTests.FirstOrDefault(t=> t.ItemID == benefitItem.EntranceTestItemID) != null ? "ВИ: " + allTests.FirstOrDefault(t => t.ItemID == benefitItem.EntranceTestItemID).TestName : ""
                                : "Общей льготы";
                            //modelState.AddModelError("commonErrors", string.Format("В данной ОО уже имеется ВИ с таким же UID: {0}", testItem.UID));
                            modelState.AddModelError("CompetitiveGroupEdit_CompetitiveGroupName", string.Format("В данной ПК уже имеется льгота с таким же UID: {0}, как у {1} ", benefitItem.UID, viName));
                        }
                    }
                }

                // Редактирование количества мест в конкурсе - проверять, что не меньше, чем число зачисленных в приказ по данному конкурсу
                if (model.CompetitiveGroupEdit.CompetitiveGroupID > 0 && campaign.CampaignTypeID != 5)
                {
                    var applicationsInOrder = db.Query<int>(
                        sql: @"select count(*)
                                From ApplicationCompetitiveGroupItem acgi (NOLOCK)
                                Where acgi.CompetitiveGroupID = @CompetitiveGroupID AND 
                                acgi.[OrderOfAdmissionID] is not null and [OrderOfExceptionID] is null", 
                        param: new { CompetitiveGroupID = model.CompetitiveGroupEdit.CompetitiveGroupID
                        }).FirstOrDefault();

                    if (model.CompetitiveGroupEdit.Value < applicationsInOrder)
                    {
                        modelState.AddModelError("CompetitiveGroupEdit_Value", string.Format("По данному конкурсу имеются заявления, добавленные в приказ о зачислении. Значение конкурса не может быть меньше {0}!", applicationsInOrder));
                    }
                }

                return modelState.IsValid;
            });
        }

        private static KCPValue MapKCPRecord(SqlDataReader reader)
        {
            int index = -1;
            return new KCPValue
            {
                //ApplicationId = reader.GetInt32(++index),
                //ApplicationNumber = reader.SafeGetString(++index),
                //ViolationErrors = reader.SafeGetString(++index),
                //StatusName = reader.SafeGetString(++index),
                //StatusID = reader.GetInt32(++index),
                //LastCheckDate = reader.SafeGetDateTimeAsString(++index),
                //EntrantFullName = reader.SafeGetString(++index),
                //IdentityDocument = reader.SafeGetString(++index),
                //RegistrationDate = reader.SafeGetDateTimeAsString(++index),
                //IsInRecommendedLists = reader.SafeGetBool(++index).GetValueOrDefault(),
                //CompetitiveGroupNames = reader.SafeGetString(++index),
                //IsCampaignFinished = reader.SafeGetBool(++index).GetValueOrDefault()
            };
        }


        public class ValidateCompetitiveGroup { public int Code { get; set; } public string Message { get; set; } }
        private class KCPValue { public int ValueAV { get; set; } public int ValueDAV { get; set; } public int ValueCG { get; set; } public int ValueCGDAV { get; set; }  }


        public int UpdateCompetitiveGroup(CompetitiveGroupViewModel model)
        {
            return DbConnection(db =>
            {
            IDbTransaction tran = null;
            try
            {
                tran = db.BeginTransaction();

                var cgModel = model.CompetitiveGroupEdit;

                    var competitiveGroupID = db.Query<int>(sql: SQLQuery.UpdateCompetitiveGroup,
                    param: new
                    {
                        CompetitiveGroupID = cgModel.CompetitiveGroupID,
                        InstitutionID = cgModel.InstitutionID,
                        Name = cgModel.CompetitiveGroupName,
                        UID = cgModel.Uid,
                        CampaignID = cgModel.CampaignID,
                        IsFromKrym = cgModel.IsFromKrym,
                        IsAdditional = cgModel.IsAdditional,
                        EducationFormID = cgModel.EducationFormID,
                        EducationSourceID = cgModel.EducationSourceID,
                        EducationLevelID = cgModel.EducationLevelID,
                        DirectionID = !model.CompetitiveGroupEdit.IsMultiProfile ? cgModel.DirectionID : null,
                        ParentDirectionID = model.CompetitiveGroupEdit.IsMultiProfile ? cgModel.ParentDirectionID : null,
                        IdLevelBudget = cgModel.IdLevelBudget,

                        NumberBudgetO = cgModel.EducationSourceID == EDSourceConst.Budget && cgModel.EducationFormID == EDFormsConst.O ? cgModel.Value : 0,
                        NumberBudgetOZ = cgModel.EducationSourceID == EDSourceConst.Budget && cgModel.EducationFormID == EDFormsConst.OZ ? cgModel.Value : 0,
                        NumberBudgetZ = cgModel.EducationSourceID == EDSourceConst.Budget && cgModel.EducationFormID == EDFormsConst.Z ? cgModel.Value : 0,

                        NumberPaidO = cgModel.EducationSourceID == EDSourceConst.Paid && cgModel.EducationFormID == EDFormsConst.O ? cgModel.Value : 0,
                        NumberPaidOZ = cgModel.EducationSourceID == EDSourceConst.Paid && cgModel.EducationFormID == EDFormsConst.OZ ? cgModel.Value : 0,
                        NumberPaidZ = cgModel.EducationSourceID == EDSourceConst.Paid && cgModel.EducationFormID == EDFormsConst.Z ? cgModel.Value : 0,

                        NumberQuotaO = cgModel.EducationSourceID == EDSourceConst.Quota && cgModel.EducationFormID == EDFormsConst.O ? cgModel.Value : 0,
                        NumberQuotaOZ = cgModel.EducationSourceID == EDSourceConst.Quota && cgModel.EducationFormID == EDFormsConst.OZ ? cgModel.Value : 0,
                        NumberQuotaZ = cgModel.EducationSourceID == EDSourceConst.Quota && cgModel.EducationFormID == EDFormsConst.Z ? cgModel.Value : 0,

                        NumberTargetO = cgModel.EducationSourceID == EDSourceConst.Target && cgModel.EducationFormID == EDFormsConst.O ? cgModel.Value : 0,
                        NumberTargetOZ = cgModel.EducationSourceID == EDSourceConst.Target && cgModel.EducationFormID == EDFormsConst.OZ ? cgModel.Value : 0,
                        NumberTargetZ = cgModel.EducationSourceID == EDSourceConst.Target && cgModel.EducationFormID == EDFormsConst.Z ? cgModel.Value : 0,


                    },

                    transaction: tran) ;

                    //Дополнительные параметры, такие как начало и окончание и срок обучения
                    if (cgModel.Properties != null)
                    {

                        DataTable propertiesDT = new DataTable();
                        propertiesDT.Columns.Add("CompetitiveGroupID", typeof(int));
                        propertiesDT.Columns.Add("PropertyTypeCode", typeof(int));
                        propertiesDT.Columns.Add("PropertyValue", typeof(string));

                        foreach (CompetitiveGroupProperty property in cgModel.Properties)
                        {

                            propertiesDT.Rows.Add(competitiveGroupID.First(), property.PropertyTypeCode, property.PropertyValue);
                        }


                        var properties = new SqlClient.SqlParameter("@Properties", SqlDbType.Structured);
                        properties.TypeName = "dbo.CompetitiveGroupPropertiesType";
                        properties.Value = propertiesDT;
                        //var properties = propertiesDT.AsTableValuedParameter("dbo.CompetitiveGroupPropertiesType");

                        //try
                        //{
                        //var updateCompetitiveGroupProperties = db.Query<CompetitiveGroupProperty>(sql: SQLQuery.UpdateCompetitiveGroupProperties,
                        //param: new
                        //{
                        //    properties = properties
                        //        //CompetitiveGroupID = competitiveGroupID.First()
                        //    }, transaction: tran);
                        //    //var updateCompetitiveGroupProperties = db.Query<CompetitiveGroupProperty>(sql: @"Exec dbo.[UpdateCompetitiveGroupProperties]",
                        //    //param: new
                        //    //{
                        //    //    properties = properties
                        //    //    //CompetitiveGroupID = competitiveGroupID.First()
                        //    //}, transaction: tran);
                        //}
                        //catch (Exception ex)
                        //{
                        //    throw ex;
                        //}

                        var trans = tran as SqlClient.SqlTransaction;
                        var connection = db as SqlClient.SqlConnection;

                        SqlClient.SqlCommand cmd = new SqlClient.SqlCommand("UpdateCompetitiveGroupProperties") { CommandType = CommandType.StoredProcedure };
                        cmd.Parameters.Add(properties);
                        cmd.Parameters.AddWithValue("@institutionId", model.CompetitiveGroupEdit.InstitutionID);
                        cmd.Parameters.AddWithValue("@campaignId", model.CompetitiveGroupEdit.CampaignID);
                        cmd.Connection = connection;
                        cmd.Transaction = trans;

                        cmd.ExecuteNonQuery();


                    }

                    // Программы обучения                    
                    var cgpModel = model.CompetitiveGroupProgramsEdit;
                    if (cgpModel.Programs != null) { 
                        var programsTable = new DataTable();
                        programsTable.Columns.Add("id", typeof(int));

                        foreach (var program in cgpModel.Programs)
                        {
                            programsTable.Rows.Add(program.ProgramID);
                        }
                        var programs = programsTable.AsTableValuedParameter("[Identifiers]");

                        var updateCompetitiveGroupPrograms = db.Query<CGM.CompetitiveGroupProgram>(sql: SQLQuery.UpdateCompetitiveGroupProgram,
                            param: new
                            {
                                programs = programs,
                                CompetitiveGroupID = competitiveGroupID.First(),
                            },
                            transaction: tran);
                    }
                    // Сохранение целевых организаций
                    if (model.CompetitiveGroupTargetsEditResult != null)
                        foreach (var t in model.CompetitiveGroupTargetsEditResult)
                        {
                            var res = db.Query<CGM.CompetitiveGroupProgram>(sql: SQLQuery.UpdateCompetitiveGroupTargetItem,
                                param: new
                                {
                                    CompetitiveGroupTargetID = t.CompetitiveGroupTargetID,

                                    NumberTargetO = cgModel.EducationSourceID == EDSourceConst.Target && cgModel.EducationFormID == EDFormsConst.O ? t.Value : 0,
                                    NumberTargetOZ = cgModel.EducationSourceID == EDSourceConst.Target && cgModel.EducationFormID == EDFormsConst.OZ ? t.Value : 0,
                                    NumberTargetZ = cgModel.EducationSourceID == EDSourceConst.Target && cgModel.EducationFormID == EDFormsConst.Z ? t.Value : 0,

                                    CompetitiveGroupID = competitiveGroupID.First()
                                },
                                transaction: tran);
                        }

                    // Сохранение ВИ
                    var testsAndBenefits = model.GetTestsAndBenefits();
                    List<EntranceTestItemDataViewModel> allTests = testsAndBenefits.Item1;
                    List<BenefitItemViewModel> allBenefitItems = testsAndBenefits.Item2;



                    #region EntranceTests
                    DataTable tbEntanceTests = new DataTable("EntranceTests");
                    tbEntanceTests.Columns.Add("ItemID", typeof(int));
                    tbEntanceTests.Columns.Add("TestName", typeof(string));
                    tbEntanceTests.Columns.Add("TestType", typeof(int));
                    tbEntanceTests.Columns.Add("UID", typeof(string));
                    tbEntanceTests.Columns.Add("Value", typeof(decimal));
                    tbEntanceTests.Columns.Add("EntranceTestPriority", typeof(int));
                    tbEntanceTests.Columns.Add("IsForSPOandVO", typeof(bool));
                    tbEntanceTests.Columns.Add("ReplacedEntranceTestItemID", typeof(int));
                    tbEntanceTests.Columns.Add("GUID", typeof(Guid));
                    tbEntanceTests.Columns.Add("IsFirst", typeof(bool));
                    tbEntanceTests.Columns.Add("IsSecond", typeof(bool));

                    allTests.ToList().ForEach(x => tbEntanceTests.Rows.Add(x.ItemID,
                                                                            x.TestName,
                                                                            x.TestType,
                                                                            x.UID,
                                                                            Convert.ToDecimal(x.Value),
                                                                            x.EntranceTestPriority, // != -1 ? (object)x.EntranceTestPriority : DBNull.Value,
                                                                            x.IsForSPOandVO,
                                                                            x.ReplacedEntranceTestItemID,
                                                                            Guid.NewGuid(),
                                                                            x.IsFirst,
                                                                            x.IsSecond));

                    var EntranceTestsParam = new SqlClient.SqlParameter("@EntranceTests", SqlDbType.Structured);
                    EntranceTestsParam.TypeName = "dbo.EntranceTests";
                    EntranceTestsParam.Value = tbEntanceTests;
                    #endregion

                    #region BenefitItems
                    DataTable tbBenefitItems = new DataTable("BenefitItems");
                    tbBenefitItems.Columns.Add("ItemID", typeof(int));
                    tbBenefitItems.Columns.Add("EntranceTestItemID", typeof(int));
                    tbBenefitItems.Columns.Add("OlympicDiplomTypeID", typeof(short));
                    tbBenefitItems.Columns.Add("OlympicLevelFlags", typeof(short));
                    tbBenefitItems.Columns.Add("BenefitID", typeof(short));
                    tbBenefitItems.Columns.Add("IsForAllOlympic", typeof(bool));
                    tbBenefitItems.Columns.Add("CompetitiveGroupID", typeof(int));
                    tbBenefitItems.Columns.Add("UID", typeof(string));
                    tbBenefitItems.Columns.Add("OlympicYear", typeof(int));
                    tbBenefitItems.Columns.Add("EgeMinValue", typeof(int));
                    tbBenefitItems.Columns.Add("BenefitItemGUID", typeof(Guid));
                    tbBenefitItems.Columns.Add("ClassFlags", typeof(short));
                    tbBenefitItems.Columns.Add("IsCreative", typeof(bool));
                    tbBenefitItems.Columns.Add("IsAthletic", typeof(bool));

                    allBenefitItems.ToList().ForEach(x => tbBenefitItems.Rows.Add(x.BenefitItemID,
                                                                                    x.EntranceTestItemID,
                                                                                    x.OlympicDiplomTypeID,
                                                                                    x.OlympicLevelFlags,
                                                                                    x.BenefitID,
                                                                                    x.IsForAllOlympic,
                                                                                    competitiveGroupID.First(),
                                                                                    x.UID,
                                                                                    x.OlympicYear,
                                                                                    x.EgeMinValue,
                                                                                    Guid.NewGuid(),
                                                                                    x.ClassFlags,
                                                                                    x.IsCreative,
                                                                                    x.IsAthletic));

                    var BenefitItemsParam = new SqlClient.SqlParameter("@BenefitItems", SqlDbType.Structured);
                    BenefitItemsParam.TypeName = "dbo.BenefitItems";
                    BenefitItemsParam.Value = tbBenefitItems;
                    #endregion

                    #region BenefitItemSubjects
                    DataTable tbBenefitItemSubjects = new DataTable("BenefitItemSubjects");
                    tbBenefitItemSubjects.Columns.Add("BenefitItemTempID", typeof(int));
                    tbBenefitItemSubjects.Columns.Add("SubjectId", typeof(int));
                    tbBenefitItemSubjects.Columns.Add("EgeMinValue", typeof(int));

                    allBenefitItems.Where(t=> t.BenefitItemSubjects != null).ToList().ForEach(t => t.BenefitItemSubjects.ToList().ForEach( s => tbBenefitItemSubjects.Rows.Add( 
                        t.BenefitItemID,
                        s.SubjectID,
                        s.EgeMinValue
                    )));

                    var BenefitItemSubjectsParam = new SqlClient.SqlParameter("@BenefitItemSubjects", SqlDbType.Structured);
                    BenefitItemSubjectsParam.TypeName = "dbo.BenefitItemSubjects";
                    BenefitItemSubjectsParam.Value = tbBenefitItemSubjects;
                    #endregion

                    #region BenefitItemProfiles
                    DataTable tbBenefitItemProfiles = new DataTable("BenefitItemProfiles");
                    tbBenefitItemProfiles.Columns.Add("BenefitItemTempID", typeof(int));
                    tbBenefitItemProfiles.Columns.Add("OlympicProfileID", typeof(int));

                    allBenefitItems.Where(t=> t.BenefitItemProfiles != null).ToList().ForEach(t => t.BenefitItemProfiles.ToList().ForEach(s => tbBenefitItemProfiles.Rows.Add(
                       t.BenefitItemID,
                       s.OlympicProfileID
                    )));

                    var BenefitItemProfilesParam = new SqlClient.SqlParameter("@BenefitItemProfiles", SqlDbType.Structured);
                    BenefitItemProfilesParam.TypeName = "dbo.BenefitItemProfiles";
                    BenefitItemProfilesParam.Value = tbBenefitItemProfiles;
                    #endregion

                    #region BenefitItemOlympics
                    DataTable tbBenefitItemOlympics = new DataTable("BenefitItemOlympics");
                    tbBenefitItemOlympics.Columns.Add("ID", typeof(int));
                    tbBenefitItemOlympics.Columns.Add("BenefitItemTempID", typeof(int));
                    tbBenefitItemOlympics.Columns.Add("OlympicTypeID", typeof(int));
                    tbBenefitItemOlympics.Columns.Add("OlympicLevel", typeof(short));
                    tbBenefitItemOlympics.Columns.Add("ClassFlags", typeof(short));
                    tbBenefitItemOlympics.Columns.Add("OlympicLevelFlags", typeof(short));
                    tbBenefitItemOlympics.Columns.Add("GUID", typeof(Guid));

                    allBenefitItems.Where(t => t.BenefitItemOlympics != null).ToList().ForEach(t => t.BenefitItemOlympics.ToList().ForEach(s => tbBenefitItemOlympics.Rows.Add(
                       s.ID,
                       t.BenefitItemID,
                       s.OlympicID,
                       s.OlympicLevelFlags,
                       s.ClassFlags,
                       s.OlympicLevelFlags,
                       Guid.NewGuid()
                    )));

                    var BenefitItemOlympicsParam = new SqlClient.SqlParameter("@BenefitItemOlympics", SqlDbType.Structured);
                    BenefitItemOlympicsParam.TypeName = "dbo.BenefitItemOlympics";
                    BenefitItemOlympicsParam.Value = tbBenefitItemOlympics;

                    #endregion

                    #region BenefitItemOlympicProfiles
                    DataTable tbBenefitItemOlympicProfiles = new DataTable("BenefitItemOlympicProfiles");
                    tbBenefitItemOlympicProfiles.Columns.Add("BenefitItemTempID", typeof(int));
                    tbBenefitItemOlympicProfiles.Columns.Add("OlympicProfileID", typeof(int));

                    allBenefitItems.Where(t=> t.BenefitItemOlympics != null).ToList().ForEach(t => t.BenefitItemOlympics.Where(n=> n.BenefitItemOlympicProfiles != null).ToList().ForEach(
                        o => o.BenefitItemOlympicProfiles.ToList().ForEach(op => tbBenefitItemOlympicProfiles.Rows.Add(
                            o.ID,
                            op.OlympicProfileID
                    ))));

                    var BenefitItemOlympicProfilesParam = new SqlClient.SqlParameter("@BenefitItemOlympicProfiles", SqlDbType.Structured);
                    BenefitItemOlympicProfilesParam.TypeName = "dbo.BenefitItemProfiles";
                    BenefitItemOlympicProfilesParam.Value = tbBenefitItemOlympicProfiles;

                    #endregion

                    //var args = new DynamicParameters(new { });
                    //args.Add("CompetitiveGroupID", competitiveGroupID.First(), DbType.Int32);
                    //args.Add("EntranceTests", EntranceTestsParam);
                    //args.Add("BenefitItems", BenefitItemsParam);
                    //args.Add("BenefitItemSubjects", BenefitItemSubjectsParam);
                    //args.Add("BenefitItemProfiles", BenefitItemProfilesParam);
                    //args.Add("BenefitItemOlympics", BenefitItemOlympicsParam);
                    //args.Add("BenefitItemOlympicProfiles", BenefitItemOlympicProfilesParam);

                    var tr = tran as SqlClient.SqlTransaction;
                    var conn = db as SqlClient.SqlConnection;
                    using (SqlClient.SqlCommand cmd = new SqlClient.SqlCommand(SQLQuery.UpdateEntranceTestItem, conn, tr))
                    {
                        cmd.CommandTimeout = 60000; // todo: вынести в настройки

                        cmd.Parameters.Add(new SqlClient.SqlParameter("@CompetitiveGroupID", competitiveGroupID.First()));
                        cmd.Parameters.Add(EntranceTestsParam);

                        cmd.Parameters.Add(BenefitItemsParam);
                        cmd.Parameters.Add(BenefitItemSubjectsParam);
                        cmd.Parameters.Add(BenefitItemProfilesParam);
                        cmd.Parameters.Add(BenefitItemOlympicsParam);
                        cmd.Parameters.Add(BenefitItemOlympicProfilesParam);

                        cmd.ExecuteNonQuery();
                    }


                    tran.Commit();
                    return competitiveGroupID.First();
                }
                catch (Exception exc)
                {
                    if (tran != null)
                        tran.Rollback();
                    throw;
                }
            });
        }
    }
}
