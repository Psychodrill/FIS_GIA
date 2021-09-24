using Dapper;
using System.Linq;
using System.Collections.Generic;
using GVUZ.Data.Model;
using System;
using System.Data;
using GVUZ.Data.Helpers;
using GVUZ.DAL.Helpers;
using GVUZ.DAL.Dapper.Repository.Model;
using GVUZ.DAL.ViewModel;
using GVUZ.CompositionExportModel;

namespace GVUZ.DAL.Dapper.Repository.Model
{
    public class ApplicationRepository : GvuzRepository
    {
        public ApplicationRepository() : base()
        {
        }

        public IEnumerable<AdmissionItemType> GetAdmissionItemTypeAll()
        {
            return DbConnection(db =>
            {
                return db.Query<AdmissionItemType>("select * from AdmissionItemType");
            });
        }

        public IEnumerable<AdmissionItemType> GetAdmissionItemTypeByCampaign(int campaignId)
        {
            return DbConnection(db =>
            {
                return db.Query<AdmissionItemType>("select * from AdmissionItemType where AdmissionItemType.ItemTypeID " +
                    "in (select AdmissionItemTypeID from EduLevelsToCampaignTypes " +
                    "where CampaignTypeID = (select CampaignTypeID from Campaign where CampaignID = " + campaignId + "))");
            });
        }

        public IEnumerable<RegionType> GetRegionTypeAll()
        {
            return DbConnection(db =>
            {
                return db.Query<RegionType>("select * from RegionType order by Name");
            });
        }

        public IEnumerable<TownType> GetTownTypeAll()
        {
            return DbConnection(db =>
            {
                return db.Query<TownType>("select * from TownType order by TownTypeID");
            });
        }

        public CompetitiveGroup GetCompetitiveGroupById(int id)
        {
            //var sql = @"select p.*
            //         ,ISNULL(substring((
            //                select t.CompetitiveGroupProgramRow as [text()]
            //                from (
            //                    select ', ' + ISNULL(ip.code, '') + ' '  + ip.Name   as CompetitiveGroupProgramRow,
            //                            ROW_NUMBER() over(PARTITION BY pr.InstitutionProgramID ORDER by pr.InstitutionProgramID ) is_distinct
            //                    from
            //                        CompetitiveGroupToProgram pr (NOLOCK) 
            //                        inner join InstitutionProgram ip (NOLOCK)  ON pr.InstitutionProgramID = ip.InstitutionProgramID
            //                    where
            //                        pr.CompetitiveGroupID = p.CompetitiveGroupID
            //                ) t 
            //                where t.is_distinct = 1 order by t.CompetitiveGroupProgramRow
            //                for xml path('')
            //            ), 3, 8000), '')  as CompetitiveGroupProgramRow
            //         ,ISNULL(lev.BudgetName, '') as LevelBudgetRow 
            //            ,f.*, s.*, l.*,d.*,c.*                 
            //          from CompetitiveGroup as p (NOLOCK) 
            //          left join AdmissionItemType as f (NOLOCK) on p.EducationFormID = f.ItemTypeID 
            //          left join AdmissionItemType as s (NOLOCK) on p.EducationSourceID = s.ItemTypeID 
            //          left join AdmissionItemType as l (NOLOCK) on p.EducationLevelID = l.ItemTypeID 
            //          left join Direction as d (NOLOCK) on p.DirectionID = d.DirectionID                    
            //          left join Campaign as c (NOLOCK) on p.CampaignID = c.CampaignID
            //          LEFT JOIN [dbo].[LevelBudget] lev (NOLOCK) ON lev.IdLevelBudget = p.IdLevelBudget
            //          where p.CompetitiveGroupID=" + id;

            //return DbConnection(db =>
            //{
            //    return db.Query<CompetitiveGroup, AdmissionItemType, AdmissionItemType, AdmissionItemType, Direction, Campaign, CompetitiveGroup>(
            //        sql, (p, f, s, l, d, c) =>
            //        {
            //            p.Form = f;
            //            p.Source = s;
            //            p.Level = l;
            //            p.Direction = d;                        
            //            p.Campaign = c;
            //            

            //            return p;
            //        }, splitOn: "ItemTypeID,DirectionID,CampaignID").FirstOrDefault();

            //});

            var sql = @"select
                         p.* ,f.*, s.*, l.*,d.*,c.*,pd.ParentDirectionID as parentID, pd.* 
                        ,ISNULL(substring((
                            select t.CompetitiveGroupProgramRow as [text()]
                            from (
                                select ', ' + ISNULL(ip.code, '') + ' '  + ip.Name   as CompetitiveGroupProgramRow,
                                       ROW_NUMBER() over(PARTITION BY pr.InstitutionProgramID ORDER by pr.InstitutionProgramID ) is_distinct
                                from
                                    CompetitiveGroupToProgram pr (NOLOCK) 
                                   inner join InstitutionProgram ip (NOLOCK)  ON pr.InstitutionProgramID = ip.InstitutionProgramID
                                where
                                    pr.CompetitiveGroupID = p.CompetitiveGroupID
                            ) t 
                            where t.is_distinct = 1 order by t.CompetitiveGroupProgramRow
                            for xml path('')
                        ), 3, 8000), '')  as CompetitiveGroupProgramRow
                     ,ISNULL(lev.BudgetName, '') as LevelBudgetRow 

                      from CompetitiveGroup as p (NOLOCK) 
                      left join AdmissionItemType as f (NOLOCK) on p.EducationFormID = f.ItemTypeID 
                      left join AdmissionItemType as s (NOLOCK) on p.EducationSourceID = s.ItemTypeID 
                      left join AdmissionItemType as l (NOLOCK) on p.EducationLevelID = l.ItemTypeID 
                      left join Direction as d (NOLOCK) on p.DirectionID = d.DirectionID
                      left join ParentDirection as pd (NOLOCK) on pd.ParentDirectionID = p.ParentDirectionID
                      left join Campaign as c (NOLOCK) on p.CampaignID = c.CampaignID
                      LEFT JOIN [dbo].[LevelBudget] lev (NOLOCK) ON lev.IdLevelBudget = p.IdLevelBudget
                      where p.CompetitiveGroupID = " + id;
            return DbConnection(db =>
            {
                return db.Query<CompetitiveGroup, AdmissionItemType, AdmissionItemType, AdmissionItemType, Direction, Campaign, ParentDirection, CompetitiveGroup>(
                    sql, (p, f, s, l, d, c, pd) =>
                    {
                        p.Form = f;
                        p.Source = s;
                        p.Level = l;
                        p.Direction = d;
                        p.Campaign = c;
                        p.ParentDirection = pd;

                        return p;
                    }, splitOn: "ItemTypeID,DirectionID,CampaignID,parentID").FirstOrDefault();

            });
        }


        public Campaign GetCampaignById(int campaignId)
        {
            return DbConnection(db =>
            {
                return db.Query<Campaign>("select * from Campaign (NOLOCK) where CampaignID = " + campaignId).FirstOrDefault();
            });
        }

        public bool CheckEntranceTestItemC(int competitiveGroupId)
        {
            return DbConnection(db =>
            {
                int cnt = db.Query<int>("select count(EntranceTestItemID) from EntranceTestItemC (NOLOCK) where CompetitiveGroupID = " + competitiveGroupId + " and IsForSPOandVO=1").Single();
                return (cnt > 0) ;
            });
        }

        public bool DeleteApplications(int[] applicationsId, int institutionId)
        {
            return DbConnection(db =>
            {

                DataTable tbIds = new DataTable("applicationIds");
                tbIds.Columns.Add("id", typeof(int));
                if (applicationsId != null)
                    applicationsId.ToList().ForEach(x => tbIds.Rows.Add(x));

                var IdsParam = new System.Data.SqlClient.SqlParameter("@applicationIds", SqlDbType.Structured);
                IdsParam.TypeName = "dbo.Identifiers";
                IdsParam.Value = tbIds;

                using (var conn = db as System.Data.SqlClient.SqlConnection)
                {
                    using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("DeleteApplications", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@InstitutionId", institutionId));
                        cmd.Parameters.Add(IdsParam);
                        cmd.ExecuteNonQuery();
                    }
                }

                return true;
            }); 

            //WithTransaction(tx =>
            //{
            //    foreach (var applicationId in applicationsId)
            //    {
            //        tx.Execute("delete from ApplicationCompetitiveGroupItem where ApplicationID = " + applicationId);
            //        tx.Execute("delete from ApplicationEntrantDocument where ApplicationID = " + applicationId);
            //        tx.Execute("delete from IndividualAchivement where ApplicationID = " + applicationId);
            //        tx.Execute("delete from ApplicationEntranceTestDocument where ApplicationID = " + applicationId);
            //        tx.Execute("delete from Application where ApplicationID = " + applicationId);
            //    }
            //});

            //return true;
        }

        public int GetStatusApplication(int applicationId)
        {
            return DbConnection(db =>
            {
                return db.Query<int>("select StatusID from Application (NOLOCK) where ApplicationID = " + applicationId).FirstOrDefault();
            });
        }

        public List<CompositionRequestItem> GetCompositionPaths(string aids)
        {
            return DbConnection(db =>
            {
                var sql =
                @"select distinct acr.CompositionPaths, 
                e.EntrantID, e.FirstName, e.LastName, e.MiddleName 
                ,isnull(e_doc.DocumentSeries, '') as DocumentSeries
                ,isnull(e_doc.DocumentNumber, '') as DocumentNumber
                from ApplicationCompositionResults acr (NOLOCK) 
                left join Application a (NOLOCK) on a.ApplicationID = acr.ApplicationID 
                left join Entrant e (NOLOCK) on e.EntrantID = a.EntrantID 
                inner join EntrantDocument e_doc (NOLOCK) on e.IdentityDocumentID = e_doc.EntrantDocumentID
                where acr.ApplicationID in " + aids;

                return db.Query<CompositionRequestItem>(sql).ToList();
            });
        }

        public void UpdateCompositionDates(string aids)
        {
            try
            {
                DbConnection(db =>
                {
                    return db.Query<string>(
                        sql: "update ApplicationCompositionResults set DownloadDate = getdate() where ApplicationID in " + aids
                        , commandTimeout: 120);
                });
            }
            catch (Exception)
            {
            }
        }

        public DocumentInfoViewModel GetDocInfo(int docId)
        {
            return DbConnection(db =>
            {
                return db.Query<DocumentInfoViewModel>(@"select DocumentNumber, DocumentDate 
                                    FROM EntrantDocument (NOLOCK) where EntrantDocumentID = " + docId).FirstOrDefault();
            });
        }

        public bool GetEntrantIsForeigner(int entrantId)
        {
            string sql = @"
select top 1
    case when edi.NationalityTypeID != 1 then 1 else 0 end as IsForeigner
from
    EntrantDocumentIdentity edi  (NOLOCK)
where 
    edi.EntrantDocumentID in
    (select ed.EntrantDocumentID from EntrantDocument (NOLOCK) ed where ed.EntrantId = @entrantId)
";

            return DbConnection(db =>
            {
                int? result = db.ExecuteScalar<int?>(sql, new { entrantId = entrantId });
                return result == 1;
            });
        }

       
    }
}