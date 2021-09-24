using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using GVUZ.Web.ViewModels;
using System.Configuration;
using System.Web.Configuration;
using GVUZ.Data.Model;

namespace GVUZ.Web.ContextExtensionsSQL
{

    public static class SQL
    {
        public static int TIMEOUT = 300;

        private static string _connectionString;

        public static string ConnectionString
        {
            get
            {
                if (String.IsNullOrEmpty(_connectionString))
                {
                    ConnectionStringSettings css = ConfigurationManager.ConnectionStrings["Main"];
                    _connectionString = css.ConnectionString;
                }
                return _connectionString;
            }
        }

        public static SqlCommand getCom(string sql)
        {
            SqlConnection con = new SqlConnection(ConnectionString);
            SqlCommand com = new SqlCommand(sql, con);
            com.CommandTimeout = 120;
            return com;
        }

        public static List<ApplicationPriorityViewModel> GetApplicationPriorities(int ApplicationID, int institutionId = 0)
        {
            #region SQL

            string sql = @"
SELECT
		acgi.id
		,acgi.ApplicationId
		,acgi.CompetitiveGroupId
		,CG.Name AS CompetitiveGroupName
		,acgi.CompetitiveGroupItemId
		,COALESCE(D.Name, pd.Name) AS CompetitiveGroupItemName
		,acgi.EducationFormId
		,F.Name AS EducationFormName
		,acgi.EducationSourceId 
		,Sour.Name AS EducationSourceName
        ,acgi.CompetitiveGroupTargetId 
		,COALESCE(cgt.Name,cgt.ContractOrganizationName) AS CompetitiveGroupTargetName
		,acgi.[Priority]
        ,acgi.IsAgreed
        ,acgi.IsDisagreed
        ,acgi.IsAgreedDate
        ,acgi.IsForSPOandVO
        ,acgi.IsDisagreedDate
        ,acgi.CalculatedRating
        ,CAST(CASE WHEN acgi.OrderOfExceptionID IS NOT NULL 
            THEN 1 
            ELSE 0
        END AS BIT) AS HasOrderOfAdmissionRefuse 
        ,Level.ItemTypeID as LevelId 
        ,Level.Name as LevelName
		,substring((
            select t.CompetitiveGroupProgramRow as [text()]
            from (
                select ', ' + ISNULL(ip.code, '') + ' '  + ip.Name   as CompetitiveGroupProgramRow,
                        ROW_NUMBER() over(PARTITION BY pr.InstitutionProgramID ORDER by pr.InstitutionProgramID ) is_distinct
                from
                    CompetitiveGroupToProgram pr (NOLOCK) 
                    inner join InstitutionProgram ip (NOLOCK)  ON pr.InstitutionProgramID = ip.InstitutionProgramID
                where
                    cg.CompetitiveGroupID = pr.CompetitiveGroupID
            ) t 
            where t.is_distinct = 1 order by t.CompetitiveGroupProgramRow
            for xml path('')
        ), 3, 8000)  as CompetitiveGroupProgramRow
		,lev.BudgetName as LevelBudgetRow
FROM
    [dbo].[ApplicationCompetitiveGroupItem] acgi (NOLOCK)
    INNER JOIN [dbo].[CompetitiveGroup] cg (NOLOCK) ON cg.CompetitiveGroupID = acgi.CompetitiveGroupId
    LEFT JOIN [dbo].[Direction] d (NOLOCK) ON d.DirectionID = cg.DirectionID
    LEFT JOIN [dbo].[ParentDirection] pd (NOLOCK) ON pd.ParentDirectionID = cg.ParentDirectionID
    INNER JOIN [dbo].[AdmissionItemType] Sour (NOLOCK) ON Sour.ItemTypeID = acgi.EducationSourceId
    INNER JOIN [dbo].[AdmissionItemType] F (NOLOCK) ON F.ItemTypeID = acgi.EducationFormId
    LEFT JOIN [dbo].[AdmissionItemType] Level (NOLOCK) ON Level.ItemTypeID = cg.EducationLevelID
    LEFT JOIN [dbo].[CompetitiveGroupTarget] cgt (NOLOCK) ON acgi.CompetitiveGroupTargetId = cgt.CompetitiveGroupTargetID
    LEFT JOIN [dbo].[LevelBudget] lev (NOLOCK) ON lev.IdLevelBudget = cg.IdLevelBudget
WHERE 
    acgi.ApplicationID=@ApplicationID
ORDER BY
    CG.Name
";
            #endregion

            List<ApplicationPriorityViewModel> ListApplicationPriorities = new List<ApplicationPriorityViewModel>();
            DataSet ds = null;

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("ApplicationID", ApplicationID));
                com.Parameters.Add(new SqlParameter("orderOfAdmissionRefuseTypeId", OrderOfAdmissionType.OrderOfAdmissionRefuse));

                ds = new DataSet();

                try
                {
                    con.Open();
                    SqlDataAdapter sqlAdapter = new SqlDataAdapter(com);
                    sqlAdapter.Fill(ds);

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        int[] limitedLevelIds= { 2, 5 };
                        int levelId = Convert.ToInt32(ds.Tables[0].Rows[i]["LevelId"]);

                        int[] limitedFormIds= { 11, 12 };
                        int formId =Convert.ToInt32(ds.Tables[0].Rows[i]["EducationFormId"]);

                        int[] limitedFinSourceIds = { 14, 16, 20 };
                        int finSourceId= Convert.ToInt32(ds.Tables[0].Rows[i]["EducationSourceId"]);

                        bool unlimitedAgreements = 
                            !(limitedLevelIds.Contains(levelId) 
                            && limitedFormIds.Contains(formId)
                            && limitedFinSourceIds.Contains(finSourceId));

                        ListApplicationPriorities.Add(new ApplicationPriorityViewModel()
                        {
                            Id = Convert.ToInt32(ds.Tables[0].Rows[i]["Id"]),
                            ApplicationId = Convert.ToInt32(ds.Tables[0].Rows[i]["ApplicationId"]),
                            CompetitiveGroupId = Convert.ToInt32(ds.Tables[0].Rows[i]["CompetitiveGroupId"]),
                            CompetitiveGroupName = ds.Tables[0].Rows[i]["CompetitiveGroupName"] as string,
                            CompetitiveGroupItemId = 0, // Convert.ToInt32(ds.Tables[0].Rows[i]["CompetitiveGroupItemId"]),
                            CompetitiveGroupItemName = ds.Tables[0].Rows[i]["CompetitiveGroupItemName"] as string,
                            EducationFormId = formId,
                            EducationFormName = ds.Tables[0].Rows[i]["EducationFormName"] as string,
                            EducationSourceId = finSourceId,
                            EducationSourceName = ds.Tables[0].Rows[i]["EducationSourceName"] as string,
                            CompetitiveGroupTargetId = Convert.ToInt32(ds.Tables[0].Rows[i]["CompetitiveGroupTargetId"] as int?),
                            TargetOrganizationName = ds.Tables[0].Rows[i]["CompetitiveGroupTargetName"] as string,
                            Priority = ds.Tables[0].Rows[i]["Priority"] as int?,
                            IsAgreed = ds.Tables[0].Rows[i]["IsAgreed"] as bool?,
                            IsDisagreed = ds.Tables[0].Rows[i]["IsDisagreed"] as bool?,
                            IsAgreedDate = ds.Tables[0].Rows[i]["IsAgreedDate"] as DateTime?,
                            IsDisagreedDate = ds.Tables[0].Rows[i]["IsDisagreedDate"] as DateTime?,
                            CalculatedRating = ds.Tables[0].Rows[i]["CalculatedRating"] as decimal?,
                            AllowEdit = !(ds.Tables[0].Rows[i]["HasOrderOfAdmissionRefuse"] as bool?).GetValueOrDefault(false),
                            UnlimitedAgreements = unlimitedAgreements,
                            IsForSPOandVO = ds.Tables[0].Rows[i]["IsForSPOandVO"] as bool?,
                            LevelName = ds.Tables[0].Rows[i]["LevelName"] as string,
                            CompetitiveGroupProgramRow = ds.Tables[0].Rows[i]["CompetitiveGroupProgramRow"] as string,
                            LevelBudgetRow = ds.Tables[0].Rows[i]["LevelBudgetRow"] as string,
                            InstitutionId = institutionId
                        });
                    }
                }
                catch (SqlException e)
                {
                    throw e;
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    if (con.State != ConnectionState.Closed) { con.Close(); }
                }
            }
            return ListApplicationPriorities;
        }

        public static IEnumerable<ForcedAdmissionReasonItemModel> GetForcedAdmissionReasons()
        {
            List<ForcedAdmissionReasonItemModel> result = new List<ForcedAdmissionReasonItemModel>();

            string sql = @"SELECT ApplicationForcedAdmissionReasonsID, Name FROM ApplicationForcedAdmissionReason";

            using (SqlConnection con = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new ForcedAdmissionReasonItemModel()
                        {
                            Id = (reader[0] as int?).GetValueOrDefault(),
                            Name = reader[1] as string
                        });
                    }
                }
            }
            return result;
        }

        public static List<IDName> GetApplicationReturnDocumentsType()
        {
            List<IDName> res = new List<IDName>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand("SELECT ApplicationReturnDocumentsTypeId as ID, Name FROM ApplicationReturnDocumentsType at (NOLOCK) ", con);
                try
                {
                    con.Open();
                    SqlDataReader r = com.ExecuteReader();
                    while (r.Read())
                    {
                        res.Add(new IDName() { ID = (int)r["ID"], Name = r["Name"] as string });
                    }
                    r.Close();
                    con.Close();
                }
                catch (SqlException e)
                {
                    throw e;
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    if (con.State != ConnectionState.Closed) { con.Close(); }
                }
            }
            return res;
        }

        public static IEnumerable<IDName> GetGenders()
        {
            List<IDName> res = new List<IDName>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand("SELECT gt.GenderID as ID, gt.Name FROM [dbo].[GenderType] gt (NOLOCK) ", con);
                try
                {
                    con.Open();
                    SqlDataReader r = com.ExecuteReader();
                    while (r.Read())
                    {
                        res.Add(new IDName() { ID = (int)r["ID"], Name = r["Name"] as string });
                    }
                    r.Close();
                    con.Close();
                }
                catch (SqlException e)
                {
                    throw e;
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    if (con.State != ConnectionState.Closed) { con.Close(); }
                }
            }
            return res;
        }

        public static IEnumerable<IDName> GetCampaigns(int InstitutionID)
        {
            List<IDName> res = new List<IDName>();

            string sql = 
                "SELECT CampaignID as ID, Name " + 
                "FROM Campaign (NOLOCK) WHERE StatusID=1 AND InstitutionID=@InstitutionID ORDER BY Name";

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("InstitutionID", InstitutionID));
                try
                {
                    con.Open();
                    SqlDataReader r = com.ExecuteReader();
                    while (r.Read())
                    {
                        res.Add(new IDName() { ID = (int)r["ID"], Name = r["Name"] as string });
                    }
                    r.Close();
                    con.Close();
                }
                catch (SqlException e)
                {
                    throw e;
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    if (con.State != ConnectionState.Closed) { con.Close(); }
                }
            }
            return res;
        }
        public static IEnumerable<object> GetCompetitiveGroups(int InstitutionID, int CampaignId, int EducationLevelID)
        {
            List<object> res = new List<object>();
            string sql = @"
select cg.CompetitiveGroupID as ID, cg.Name, cg.Course 
from CompetitiveGroup cg (NOLOCK)
left join EntranceTestItemC etic (NOLOCK) on etic.CompetitiveGroupID = cg.CompetitiveGroupID
left join CompetitiveGroupItem cgi (NOLOCK) on cgi.CompetitiveGroupID = cg.CompetitiveGroupID
left join CompetitiveGroupTargetItem cgti (NOLOCK) on cgti.CompetitiveGroupID = cg.CompetitiveGroupID
left join Campaign c (NOLOCK) on c.CampaignID = cg.CampaignID
Where c.CampaignID = @CampaignID and cg.EducationLevelID = @EducationLevelID 
group by cg.CompetitiveGroupID,c.CampaignTypeID, cg.Name, cg.Course
having c.CampaignTypeID not in (1,2,4) OR 
	(count(etic.EntranceTestItemID)> 0
	AND (sum(
		isnull(cgi.NumberBudgetO, 0) + isnull(cgi.NumberBudgetOZ, 0) + isnull(cgi.NumberBudgetZ, 0)
		+ isnull(cgi.NumberPaidO, 0) + isnull(cgi.NumberPaidOZ, 0) + isnull(cgi.NumberPaidZ, 0)
		+ isnull(cgi.NumberQuotaO, 0) + isnull(cgi.NumberQuotaOZ, 0) + isnull(cgi.NumberQuotaZ, 0)
		+ isnull(cgi.NumberTargetO, 0) + isnull(cgi.NumberTargetOZ, 0) + isnull(cgi.NumberTargetZ, 0)
		+ isnull(cgti.NumberTargetO, 0) + isnull(cgti.NumberTargetOZ, 0) + isnull(cgti.NumberTargetZ, 0)
		) > 0
	)
)
ORDER BY Name
";
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("InstitutionID", InstitutionID));
                com.Parameters.Add(new SqlParameter("CampaignId", CampaignId));
                com.Parameters.Add(new SqlParameter("EducationLevelID", EducationLevelID));
                try
                {
                    con.Open();
                    SqlDataReader r = com.ExecuteReader();
                    while (r.Read())
                    {
                        res.Add(new { ID = (int)r["ID"], Name = r["Name"] as string, Course = (Int16)r["Course"] });
                    }
                    r.Close();
                    con.Close();
                }
                catch (SqlException e)
                {
                    throw e;
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    if (con.State != ConnectionState.Closed) { con.Close(); }
                }
            }
            return res;
        }

        public static IEnumerable<IDName> GetIdentityDocuments()
        {
            List<IDName> res = new List<IDName>();
            string sql = @"
SELECT IdentityDocumentTypeID as ID, Name FROM IdentityDocumentType (NOLOCK)  AS idt ORDER BY (CASE WHEN idt.IdentityDocumentTypeID =9 THEN 100 ELSE IdentityDocumentTypeID END )
";
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);
                try
                {
                    con.Open();
                    SqlDataReader r = com.ExecuteReader();
                    while (r.Read())
                    {
                        res.Add(new IDName() { ID = (int)r["ID"], Name = r["Name"] as string });
                    }
                    r.Close();
                    con.Close();
                }
                catch (SqlException e)
                {
                    throw e;
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    if (con.State != ConnectionState.Closed) { con.Close(); }
                }
            }
            return res;
        }

        public static IEnumerable<IDName> GetAdmissionItemTypes()
        {
            List<IDName> res = new List<IDName>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand("SELECT ItemTypeID as ID, Name FROM AdmissionItemType (NOLOCK) ", con);
                try
                {
                    con.Open();
                    SqlDataReader r = com.ExecuteReader();
                    while (r.Read())
                    {
                        res.Add(new IDName() { ID = Convert.ToInt32(r["ID"]), Name = r["Name"] as string });
                    }
                    r.Close();
                    con.Close();
                }
                catch (SqlException e)
                {
                    throw e;
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    if (con.State != ConnectionState.Closed) { con.Close(); }
                }
            }
            return res;
        }

        public static IEnumerable<object> GetDirectionsForCompetitiveGroups(int InstitutionID, int[] competitiveGroupIds)
        {
            List<object> res = new List<object>();
            if (competitiveGroupIds == null) { return res; }
            if (competitiveGroupIds.Count() == 0) { return res; }
            string groupids = string.Join(",", competitiveGroupIds);
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                //SqlCommand com=new SqlCommand("SELECT CompetitiveGroupID as ID, Name, Course FROM CompetitiveGroup WHERE InstitutionID=@InstitutionID AND CampaignID=@CampaignID ORDER BY Name",con);
                string sql = @"
			SELECT 
				CAST(CG.EducationLevelID as varchar(3) )+'@'+ CAST(D.DirectionID as varchar(15))+'@'+CAST(CGI.CompetitiveGroupItemID as varchar(15)) as ID,
				(COALESCE(RTRIM(D.Code),'') +'.'+COALESCE(RTRIM(QualificationCode),'') +'/'+ COALESCE(RTRIM(NewCode),'')) as Code,
				(D.Name+', (' + AIT.Name + ')') as Name
			FROM CompetitiveGroupItem CGI (NOLOCK) 
				INNER JOIN CompetitiveGroup CG (NOLOCK) ON	CGI.CompetitiveGroupID = CG.CompetitiveGroupID 
				INNER JOIN Direction D (NOLOCK) ON D.DirectionID = CG.DirectionID
				INNER JOIN AdmissionItemType AIT (NOLOCK) on AIT.ItemTypeID= CG.EducationLevelID 
			WHERE CG.InstitutionID=@InstitutionID
            AND NOT ((SELECT COUNT(EntranceTestItemID) FROM EntranceTestItemC ETIC (NOLOCK) WHERE ETIC.CompetitiveGroupID=CG.CompetitiveGroupID)=0 AND CG.EducationLevelID <> 17)
";
                sql += " AND CG.CompetitiveGroupID in (" + groupids + ")";
                SqlCommand com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("InstitutionID", InstitutionID));
                try
                {
                    con.Open();
                    SqlDataReader r = com.ExecuteReader();
                    while (r.Read())
                    {
                        res.Add(new { ID = r["ID"] as string, Code = r["Code"] as string, Name = r["Name"] as string });
                    }
                    r.Close();
                    con.Close();
                }
                catch (SqlException e)
                {
                    throw e;
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    if (con.State != ConnectionState.Closed) { con.Close(); }
                }
            }
            return res;
        }

        public static IEnumerable<IDName> ParseTargetOrganizations(string s)
        {
            List<IDName> res = new List<IDName>();
            // 24873@ООО "ФИС ПРИЕМА 1"|24874@ООО "ФИС ПРИЕМА 2"|
            var items = s.Split('|');
            foreach (var i in items)
            {
                if (i.Length == 0) { continue; }
                var x = i.Split('@');
                var a = new IDName();
                int org_value = 0;
                bool ok = Int32.TryParse(x[0], out org_value);
                if (ok)
                {
                    a.ID = org_value;
                    a.Name = x[1];
                    res.Add(a);
                }
            }
            return res;
        }

        public static ApplicationPrioritiesViewModel CreatePrioritiesData(int InstitutionID, int[] competitiveGroupIds, string[] directionKeys)
        {
            var PrioritiesData = new ApplicationPrioritiesViewModel();
            if (competitiveGroupIds == null) { return PrioritiesData; }
            if (competitiveGroupIds.Count() == 0) { return PrioritiesData; }
            if (directionKeys == null) { return PrioritiesData; }
            if (directionKeys.Count() == 0) { return PrioritiesData; }
            IEnumerable<IDName> AdmissionItemType = GetAdmissionItemTypes();

            #region sSelect
            string sSelect = @"
SELECT 
	CGI.CompetitiveGroupItemID,
    CGI.CompetitiveGroupID,
    CG.EducationLevelID,
    CG.DirectionID,
    CGI.NumberBudgetO,
    CGI.NumberBudgetOZ,
    CGI.NumberBudgetZ,
    CGI.NumberPaidO,
    CGI.NumberPaidOZ,
    CGI.NumberPaidZ,
    CGI.NumberQuotaO,
    CGI.NumberQuotaOZ,
    CGI.NumberQuotaZ,
    GroupName = CG.Name,
    DirectionName = D.Name,
	NumberTargetO= COALESCE( (SELECT SUM(COALESCE(NumberTargetO,0)) FROM CompetitiveGroupTargetItem CGTI (NOLOCK) WHERE CGTI.CompetitiveGroupID=CG.CompetitiveGroupID ),0),
	NumberTargetOZ=COALESCE( (SELECT SUM(COALESCE(NumberTargetOZ,0)) FROM CompetitiveGroupTargetItem CGTI (NOLOCK) WHERE CGTI.CompetitiveGroupID=CGI.CompetitiveGroupID ),0),
	NumberTargetZ=COALESCE( (SELECT SUM(COALESCE(NumberTargetZ,0)) FROM CompetitiveGroupTargetItem CGTI (NOLOCK) WHERE CGTI.CompetitiveGroupID=CGI.CompetitiveGroupID ),0)
	
	,TargetOrganizationsO =COALESCE( (
	SELECT CAST(CGT.CompetitiveGroupTargetID as varchar(15))+'@'+CGT.Name+ '|'
	FROM CompetitiveGroupTargetItem CGTI (NOLOCK) INNER JOIN CompetitiveGroupTarget CGT (NOLOCK) on CGTI.CompetitiveGroupTargetID=CGT.CompetitiveGroupTargetID
	WHERE CGTI.CompetitiveGroupID=CGI.CompetitiveGroupID AND CGTI.NumberTargetO > 0 FOR XML PATH('')
	),'')
	,TargetOrganizationsOZ =COALESCE((
	SELECT CAST(CGT.CompetitiveGroupTargetID as varchar(15))+'@'+CGT.Name+ '|'
	FROM CompetitiveGroupTargetItem CGTI (NOLOCK) INNER JOIN CompetitiveGroupTarget CGT (NOLOCK) on CGTI.CompetitiveGroupTargetID=CGT.CompetitiveGroupTargetID
	WHERE CGTI.CompetitiveGroupID=CGI.CompetitiveGroupID AND CGTI.NumberTargetOZ > 0 FOR XML PATH('')
	),'')
	,TargetOrganizationsZ =COALESCE((
	SELECT CAST(CGT.CompetitiveGroupTargetID as varchar(15))+'@'+CGT.Name+ '|'
	FROM CompetitiveGroupTargetItem CGTI (NOLOCK) INNER JOIN CompetitiveGroupTarget CGT (NOLOCK) on CGTI.CompetitiveGroupTargetID=CGT.CompetitiveGroupTargetID
	WHERE CGTI.CompetitiveGroupID=CGI.CompetitiveGroupID AND CGTI.NumberTargetZ > 0 FOR XML PATH('')
	),'')
";
            #endregion
            #region sFrom
            string sFrom = @"
FROM CompetitiveGroupItem CGI (NOLOCK) 
	INNER JOIN CompetitiveGroup CG (NOLOCK) ON	CGI.CompetitiveGroupID = CG.CompetitiveGroupID 
	INNER JOIN Direction D (NOLOCK) ON D.DirectionID = CG.DirectionID
	INNER JOIN AdmissionItemType AIT (NOLOCK) on AIT.ItemTypeID= CG.EducationLevelID
";
            #endregion
            #region sWhere
            StringBuilder sWhere = new StringBuilder();
            sWhere.Append(@" WHERE  CG.InstitutionID=@InstitutionID ");
            sWhere.Append(" AND ( CG.CompetitiveGroupID in (" + string.Join(",", competitiveGroupIds) + ") )");
            sWhere.Append("AND ( 1=2 ");
            foreach (var k in directionKeys)
            {
                sWhere.Append(" OR (CAST(CG.EducationLevelID as varchar(3) )+'@'+ CAST(D.DirectionID as varchar(15))+'@'+CASt(CGI.CompetitiveGroupItemID as varchar(15)))='" + k + "'");
            }
            sWhere.Append(" )  ");
            #endregion
            string sql = sSelect + sFrom + sWhere.ToString();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("InstitutionID", InstitutionID));
                try
                {
                    con.Open();
                    SqlDataReader r = com.ExecuteReader();
                    PrioritiesData.ApplicationId = -1; // Для нового заявления - пока -1
                    while (r.Read())
                    {
                        #region Бюждетные места
                        /* ------------------------------------- Бюждетные места --------------------------- */
                        if (Convert.ToInt32(r["NumberBudgetO"] as int?) > 0)
                        {
                            ApplicationPriorityViewModel priority = new ApplicationPriorityViewModel()
                            {
                                Id = -1
                                ,
                                CompetitiveGroupId = (int)r["CompetitiveGroupID"]
                                ,
                                CompetitiveGroupItemId = (int)r["CompetitiveGroupItemID"]
                                ,
                                EducationSourceId = 14
                                ,
                                EducationFormId = 11
                                ,
                                CompetitiveGroupName = r["GroupName"] as string
                                ,
                                CompetitiveGroupItemName = r["DirectionName"] as string
                                ,
                                EducationFormName = AdmissionItemType.Where(x => x.ID == 11).FirstOrDefault().Name
                                ,
                                EducationSourceName = AdmissionItemType.Where(x => x.ID == 14).FirstOrDefault().Name
                            };
                            PrioritiesData.ApplicationPriorities.Add(priority);
                        }
                        if (Convert.ToInt32(r["NumberBudgetOZ"] as int?) > 0)
                        {
                            ApplicationPriorityViewModel priority = new ApplicationPriorityViewModel()
                            {
                                Id = -2,
                                CompetitiveGroupId = (int)r["CompetitiveGroupID"],
                                CompetitiveGroupItemId = (int)r["CompetitiveGroupItemID"],
                                EducationSourceId = 14,
                                EducationFormId = 12,
                                CompetitiveGroupName = r["GroupName"] as string,
                                CompetitiveGroupItemName = r["DirectionName"] as string,
                                EducationFormName = AdmissionItemType.Where(x => x.ID == 12).FirstOrDefault().Name,
                                EducationSourceName = AdmissionItemType.Where(x => x.ID == 14).FirstOrDefault().Name
                            };
                            PrioritiesData.ApplicationPriorities.Add(priority);
                        }
                        if (Convert.ToInt32(r["NumberBudgetZ"] as int?) > 0)
                        {
                            ApplicationPriorityViewModel priority = new ApplicationPriorityViewModel()
                            {
                                Id = -3,
                                CompetitiveGroupId = (int)r["CompetitiveGroupID"],
                                CompetitiveGroupItemId = (int)r["CompetitiveGroupItemID"],
                                EducationSourceId = 14,
                                EducationFormId = 10,
                                CompetitiveGroupName = r["GroupName"] as string,
                                CompetitiveGroupItemName = r["DirectionName"] as string,
                                EducationFormName = AdmissionItemType.Where(x => x.ID == 10).FirstOrDefault().Name,
                                EducationSourceName = AdmissionItemType.Where(x => x.ID == 14).FirstOrDefault().Name
                            };
                            PrioritiesData.ApplicationPriorities.Add(priority);
                        }
                        /* ------------------------------------------------------------------------------------- */
                        #endregion
                        #region Места по квотам
                        /* ------------------------------------- Места по квотам --------------------------- */
                        if (Convert.ToInt32(r["NumberQuotaO"] as int?) > 0)
                        {
                            ApplicationPriorityViewModel priority = new ApplicationPriorityViewModel()
                            {
                                Id = -11,
                                CompetitiveGroupId = (int)r["CompetitiveGroupID"],
                                CompetitiveGroupItemId = (int)r["CompetitiveGroupItemID"],
                                EducationSourceId = 20,
                                EducationFormId = 11,
                                CompetitiveGroupName = r["GroupName"] as string,
                                CompetitiveGroupItemName = r["DirectionName"] as string,
                                EducationFormName = AdmissionItemType.Where(x => x.ID == 11).FirstOrDefault().Name,
                                EducationSourceName = AdmissionItemType.Where(x => x.ID == 20).FirstOrDefault().Name
                            };
                            PrioritiesData.ApplicationPriorities.Add(priority);
                        }
                        if (Convert.ToInt32(r["NumberQuotaOZ"] as int?) > 0)
                        {
                            ApplicationPriorityViewModel priority = new ApplicationPriorityViewModel()
                            {
                                Id = -12,
                                CompetitiveGroupId = (int)r["CompetitiveGroupID"],
                                CompetitiveGroupItemId = (int)r["CompetitiveGroupItemID"],
                                EducationSourceId = 20,
                                EducationFormId = 12,
                                CompetitiveGroupName = r["GroupName"] as string,
                                CompetitiveGroupItemName = r["DirectionName"] as string,
                                EducationFormName = AdmissionItemType.Where(x => x.ID == 12).FirstOrDefault().Name,
                                EducationSourceName = AdmissionItemType.Where(x => x.ID == 20).FirstOrDefault().Name
                            };
                            PrioritiesData.ApplicationPriorities.Add(priority);
                        }
                        if (Convert.ToInt32(r["NumberQuotaZ"] as int?) > 0)
                        {
                            ApplicationPriorityViewModel priority = new ApplicationPriorityViewModel()
                            {
                                Id = -13,
                                CompetitiveGroupId = (int)r["CompetitiveGroupID"],
                                CompetitiveGroupItemId = (int)r["CompetitiveGroupItemID"],
                                EducationSourceId = 20,
                                EducationFormId = 10,
                                CompetitiveGroupName = r["GroupName"] as string,
                                CompetitiveGroupItemName = r["DirectionName"] as string,
                                EducationFormName = AdmissionItemType.Where(x => x.ID == 10).FirstOrDefault().Name,
                                EducationSourceName = AdmissionItemType.Where(x => x.ID == 20).FirstOrDefault().Name
                            };
                            PrioritiesData.ApplicationPriorities.Add(priority);
                        }
                        /* ------------------------------------------------------------------------------------- */
                        #endregion
                        #region Платные места
                        /* ------------------------------------- Платные места --------------------------- */
                        if (Convert.ToInt32(r["NumberPaidO"] as int?) > 0)
                        {
                            ApplicationPriorityViewModel priority = new ApplicationPriorityViewModel()
                            {
                                Id = -21,
                                CompetitiveGroupId = (int)r["CompetitiveGroupID"],
                                CompetitiveGroupItemId = (int)r["CompetitiveGroupItemID"],
                                EducationSourceId = 15,
                                EducationFormId = 11,
                                CompetitiveGroupName = r["GroupName"] as string,
                                CompetitiveGroupItemName = r["DirectionName"] as string,
                                EducationFormName = AdmissionItemType.Where(x => x.ID == 11).FirstOrDefault().Name,
                                EducationSourceName = AdmissionItemType.Where(x => x.ID == 15).FirstOrDefault().Name
                            };
                            PrioritiesData.ApplicationPriorities.Add(priority);
                        }
                        if (Convert.ToInt32(r["NumberPaidOZ"] as int?) > 0)
                        {
                            ApplicationPriorityViewModel priority = new ApplicationPriorityViewModel()
                            {
                                Id = -22,
                                CompetitiveGroupId = (int)r["CompetitiveGroupID"],
                                CompetitiveGroupItemId = (int)r["CompetitiveGroupItemID"],
                                EducationSourceId = 15,
                                EducationFormId = 12,
                                CompetitiveGroupName = r["GroupName"] as string,
                                CompetitiveGroupItemName = r["DirectionName"] as string,
                                EducationFormName = AdmissionItemType.Where(x => x.ID == 12).FirstOrDefault().Name,
                                EducationSourceName = AdmissionItemType.Where(x => x.ID == 15).FirstOrDefault().Name
                            };
                            PrioritiesData.ApplicationPriorities.Add(priority);
                        }
                        if (Convert.ToInt32(r["NumberPaidZ"] as int?) > 0)
                        {
                            ApplicationPriorityViewModel priority = new ApplicationPriorityViewModel()
                            {
                                Id = -23,
                                CompetitiveGroupId = (int)r["CompetitiveGroupID"],
                                CompetitiveGroupItemId = (int)r["CompetitiveGroupItemID"],
                                EducationSourceId = 15,
                                EducationFormId = 10,
                                CompetitiveGroupName = r["GroupName"] as string,
                                CompetitiveGroupItemName = r["DirectionName"] as string,
                                EducationFormName = AdmissionItemType.Where(x => x.ID == 10).FirstOrDefault().Name,
                                EducationSourceName = AdmissionItemType.Where(x => x.ID == 15).FirstOrDefault().Name
                            };
                            PrioritiesData.ApplicationPriorities.Add(priority);
                        }
                        /* ------------------------------------------------------------------------------------- */
                        if (Convert.ToInt32(r["NumberTargetO"] as int?) > 0)
                        {
                            ApplicationPriorityViewModel priority = new ApplicationPriorityViewModel()
                            {
                                Id = -31,
                                CompetitiveGroupId = (int)r["CompetitiveGroupID"],
                                CompetitiveGroupItemId = (int)r["CompetitiveGroupItemID"],
                                EducationSourceId = 16,
                                EducationFormId = 11,
                                CompetitiveGroupName = r["GroupName"] as string,
                                CompetitiveGroupItemName = r["DirectionName"] as string,
                                EducationFormName = AdmissionItemType.Where(x => x.ID == 11).FirstOrDefault().Name,
                                EducationSourceName = AdmissionItemType.Where(x => x.ID == 16).FirstOrDefault().Name
                            };
                            priority.TargetOrganizations = ParseTargetOrganizations(r["TargetOrganizationsO"] as string).Distinct().OrderBy(y => y.Name).ToArray();
                            PrioritiesData.ApplicationPriorities.Add(priority);
                        }
                        if (Convert.ToInt32(r["NumberTargetOZ"] as int?) > 0)
                        {
                            ApplicationPriorityViewModel priority = new ApplicationPriorityViewModel()
                            {
                                Id = -32,
                                CompetitiveGroupId = (int)r["CompetitiveGroupID"],
                                CompetitiveGroupItemId = (int)r["CompetitiveGroupItemID"],
                                EducationSourceId = 16,
                                EducationFormId = 12,
                                CompetitiveGroupName = r["GroupName"] as string,
                                CompetitiveGroupItemName = r["DirectionName"] as string,
                                EducationFormName = AdmissionItemType.Where(x => x.ID == 12).FirstOrDefault().Name,
                                EducationSourceName = AdmissionItemType.Where(x => x.ID == 16).FirstOrDefault().Name
                            };
                            priority.TargetOrganizations = ParseTargetOrganizations(r["TargetOrganizationsOZ"] as string).Distinct().OrderBy(y => y.Name).ToArray();
                            PrioritiesData.ApplicationPriorities.Add(priority);
                        }
                        if (Convert.ToInt32(r["NumberTargetZ"] as int?) > 0)
                        {
                            ApplicationPriorityViewModel priority = new ApplicationPriorityViewModel()
                            {
                                Id = -33,
                                CompetitiveGroupId = (int)r["CompetitiveGroupID"],
                                CompetitiveGroupItemId = (int)r["CompetitiveGroupItemID"],
                                EducationSourceId = 16,
                                EducationFormId = 10,
                                CompetitiveGroupName = r["GroupName"] as string,
                                CompetitiveGroupItemName = r["DirectionName"] as string,
                                EducationFormName = AdmissionItemType.Where(x => x.ID == 10).FirstOrDefault().Name,
                                EducationSourceName = AdmissionItemType.Where(x => x.ID == 16).FirstOrDefault().Name
                            };
                            priority.TargetOrganizations = ParseTargetOrganizations(r["TargetOrganizationsZ"] as string).Distinct().OrderBy(y => y.Name).ToArray();
                            PrioritiesData.ApplicationPriorities.Add(priority);
                        }

                        #endregion
                    }
                    r.Close();
                    con.Close();
                }
                catch (SqlException e)
                {
                    throw e;
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    if (con.State != ConnectionState.Closed) { con.Close(); }
                }
            }
            return PrioritiesData;
        }

        /// <summary>Проверка на уникальность номера заявления</summary>
        public static bool CheckApplicationNumberIsUnique(int InstitutionID, string applicationNumber)
        {
            int count = 0;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand("SELECT Count(ApplicationID) FROM [Application] (NOLOCK) Where InstitutionID=@InstitutionID AND ApplicationNumber=@ApplicationNumber", con);
                com.Parameters.Add(new SqlParameter("InstitutionID", InstitutionID));
                com.Parameters.Add(new SqlParameter("ApplicationNumber", applicationNumber));
                try
                {
                    con.Open();
                    count = (int)com.ExecuteScalar();
                    con.Close();
                }
                catch (SqlException e)
                {
                    throw e;
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    if (con.State != ConnectionState.Closed) { con.Close(); }
                }
            }
            return (count == 0);
        }


        //==============================================================================================

        public static ApplicationPrioritiesViewModel GetPrioritiesData(int competitiveGroupId)
        {
            var PrioritiesData = new ApplicationPrioritiesViewModel();
            IEnumerable<IDName> AdmissionItemType = GetAdmissionItemTypes();

            short? educationFormId = null;
            short? educationSourceId = null;

            string competitiveGroupInfoSql = @"
SELECT 
    EducationFormId,
    EducationSourceId
FROM
    CompetitiveGroup (NOLOCK)
WHERE
    CompetitiveGroupID = @competitiveGroupId
";

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                SqlCommand com = new SqlCommand(competitiveGroupInfoSql, con);
                com.Parameters.Add(new SqlParameter("competitiveGroupId", competitiveGroupId));

                using (SqlDataReader r = com.ExecuteReader())
                {
                    if (r.Read())
                    {
                        educationFormId = r["EducationFormId"] as short?;
                        educationSourceId = r["EducationSourceId"] as short?;
                    }
                }
            }

            string sourceAndFormPostfix = null;
            if(educationFormId==11 && educationSourceId==16)
            {
                sourceAndFormPostfix = "O";
            }
            if (educationFormId == 12&& educationSourceId == 16)
            {
                sourceAndFormPostfix = "OZ";
            }
            if (educationFormId == 10 && educationSourceId == 16)
            {
                sourceAndFormPostfix = "Z";
            }

            string targetNumbersSqlPart = null;
            if (!String.IsNullOrEmpty(sourceAndFormPostfix))
            {
                targetNumbersSqlPart = String.Format(@"
NumberTarget = 
    ISNULL(
        (SELECT 
            SUM(ISNULL(CGTI.NumberTarget{0},0)+ISNULL(CGI.NumberTarget{0},0)) 
        FROM 
            CompetitiveGroupTargetItem CGTI (NOLOCK) 
            INNER JOIN CompetitiveGroupItem CGI (NOLOCK) on CGI.CompetitiveGroupID=CGTI.CompetitiveGroupID
        WHERE 
            CGTI.CompetitiveGroupID=CG.CompetitiveGroupID)
    ,0),
TargetOrganizations =
    ISNULL( 
        (SELECT 
            CAST(CGT.CompetitiveGroupTargetID as varchar(15))+'@'+ ISNULL(CGT.Name, CGT.ContractOrganizationName)  +ISNULL(' (договор № '+ CGT.ContractNumber +')','')  + '|'
	    FROM 
            CompetitiveGroupTargetItem CGTI (NOLOCK) 
            INNER JOIN CompetitiveGroupTarget CGT (NOLOCK) on CGTI.CompetitiveGroupTargetID=CGT.CompetitiveGroupTargetID
            --INNER JOIN CompetitiveGroupItem CGI (NOLOCK) on CGI.CompetitiveGroupID=CGTI.CompetitiveGroupID
	    WHERE 
            CGTI.CompetitiveGroupID=CGI.CompetitiveGroupID 
            AND (CGTI.NumberTarget{0} > 0 OR CGI.NumberTarget{0} > 0) FOR XML PATH('')
	    )
    ,'')
", sourceAndFormPostfix);
            }
            else
            {
                targetNumbersSqlPart = @"
NumberTarget = NULL,
TargetOrganizations = NULL
";
            }

            #region sSelect
            string sSelect = String.Format(@"
SELECT 
	CGI.CompetitiveGroupItemID,
    CGI.CompetitiveGroupID,
    CG.EducationLevelID,
    CG.DirectionID,
    CGI.NumberBudgetO,
    CGI.NumberBudgetOZ,
    CGI.NumberBudgetZ,
    CGI.NumberPaidO,
    CGI.NumberPaidOZ,
    CGI.NumberPaidZ,
    CGI.NumberQuotaO,
    CGI.NumberQuotaOZ,
    CGI.NumberQuotaZ,
    GroupName = CG.Name,
    DirectionName = ISNULL(D.Name, PD.Name),
    {0}
",targetNumbersSqlPart);
            #endregion
            #region sFrom
            string sFrom = @"
FROM CompetitiveGroupItem CGI (NOLOCK) 
	INNER JOIN CompetitiveGroup CG (NOLOCK) ON	CGI.CompetitiveGroupID = CG.CompetitiveGroupID 
	LEFT JOIN Direction D (NOLOCK) ON D.DirectionID = CG.DirectionID
    LEFT JOIN ParentDirection PD (NOLOCK) ON PD.ParentDirectionID = CG.ParentDirectionID
	INNER JOIN AdmissionItemType AIT (NOLOCK) on AIT.ItemTypeID= CG.EducationLevelID
";
            #endregion
            #region sWhere
            string sWhere = @" WHERE CG.CompetitiveGroupID=@competitiveGroupId";

            #endregion
            string sql = sSelect + sFrom + sWhere.ToString();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("competitiveGroupId", competitiveGroupId));
                try
                {
                    con.Open();
                    SqlDataReader r = com.ExecuteReader();
                    PrioritiesData.ApplicationId = -1; // Для нового заявления - пока -1
                    while (r.Read())
                    {
                        #region Бюждетные места
                        /* ------------------------------------- Бюждетные места --------------------------- */
                        if (Convert.ToInt32(r["NumberBudgetO"] as int?) > 0)
                        {
                            ApplicationPriorityViewModel priority = new ApplicationPriorityViewModel()
                            {
                                Id = -1
                                ,
                                CompetitiveGroupId = (int)r["CompetitiveGroupID"]
                                ,
                                CompetitiveGroupItemId = (int)r["CompetitiveGroupItemID"]
                                ,
                                EducationSourceId = 14
                                ,
                                EducationFormId = 11
                                ,
                                CompetitiveGroupName = r["GroupName"] as string
                                ,
                                CompetitiveGroupItemName = r["DirectionName"] as string
                                ,
                                EducationFormName = AdmissionItemType.Where(x => x.ID == 11).FirstOrDefault().Name
                                ,
                                EducationSourceName = AdmissionItemType.Where(x => x.ID == 14).FirstOrDefault().Name
                            };
                            PrioritiesData.ApplicationPriorities.Add(priority);
                        }
                        if (Convert.ToInt32(r["NumberBudgetOZ"] as int?) > 0)
                        {
                            ApplicationPriorityViewModel priority = new ApplicationPriorityViewModel()
                            {
                                Id = -2,
                                CompetitiveGroupId = (int)r["CompetitiveGroupID"],
                                CompetitiveGroupItemId = (int)r["CompetitiveGroupItemID"],
                                EducationSourceId = 14,
                                EducationFormId = 12,
                                CompetitiveGroupName = r["GroupName"] as string,
                                CompetitiveGroupItemName = r["DirectionName"] as string,
                                EducationFormName = AdmissionItemType.Where(x => x.ID == 12).FirstOrDefault().Name,
                                EducationSourceName = AdmissionItemType.Where(x => x.ID == 14).FirstOrDefault().Name
                            };
                            PrioritiesData.ApplicationPriorities.Add(priority);
                        }
                        if (Convert.ToInt32(r["NumberBudgetZ"] as int?) > 0)
                        {
                            ApplicationPriorityViewModel priority = new ApplicationPriorityViewModel()
                            {
                                Id = -3,
                                CompetitiveGroupId = (int)r["CompetitiveGroupID"],
                                CompetitiveGroupItemId = (int)r["CompetitiveGroupItemID"],
                                EducationSourceId = 14,
                                EducationFormId = 10,
                                CompetitiveGroupName = r["GroupName"] as string,
                                CompetitiveGroupItemName = r["DirectionName"] as string,
                                EducationFormName = AdmissionItemType.Where(x => x.ID == 10).FirstOrDefault().Name,
                                EducationSourceName = AdmissionItemType.Where(x => x.ID == 14).FirstOrDefault().Name
                            };
                            PrioritiesData.ApplicationPriorities.Add(priority);
                        }
                        /* ------------------------------------------------------------------------------------- */
                        #endregion
                        #region Места по квотам
                        /* ------------------------------------- Места по квотам --------------------------- */
                        if (Convert.ToInt32(r["NumberQuotaO"] as int?) > 0)
                        {
                            ApplicationPriorityViewModel priority = new ApplicationPriorityViewModel()
                            {
                                Id = -11,
                                CompetitiveGroupId = (int)r["CompetitiveGroupID"],
                                CompetitiveGroupItemId = (int)r["CompetitiveGroupItemID"],
                                EducationSourceId = 20,
                                EducationFormId = 11,
                                CompetitiveGroupName = r["GroupName"] as string,
                                CompetitiveGroupItemName = r["DirectionName"] as string,
                                EducationFormName = AdmissionItemType.Where(x => x.ID == 11).FirstOrDefault().Name,
                                EducationSourceName = AdmissionItemType.Where(x => x.ID == 20).FirstOrDefault().Name
                            };
                            PrioritiesData.ApplicationPriorities.Add(priority);
                        }
                        if (Convert.ToInt32(r["NumberQuotaOZ"] as int?) > 0)
                        {
                            ApplicationPriorityViewModel priority = new ApplicationPriorityViewModel()
                            {
                                Id = -12,
                                CompetitiveGroupId = (int)r["CompetitiveGroupID"],
                                CompetitiveGroupItemId = (int)r["CompetitiveGroupItemID"],
                                EducationSourceId = 20,
                                EducationFormId = 12,
                                CompetitiveGroupName = r["GroupName"] as string,
                                CompetitiveGroupItemName = r["DirectionName"] as string,
                                EducationFormName = AdmissionItemType.Where(x => x.ID == 12).FirstOrDefault().Name,
                                EducationSourceName = AdmissionItemType.Where(x => x.ID == 20).FirstOrDefault().Name
                            };
                            PrioritiesData.ApplicationPriorities.Add(priority);
                        }
                        if (Convert.ToInt32(r["NumberQuotaZ"] as int?) > 0)
                        {
                            ApplicationPriorityViewModel priority = new ApplicationPriorityViewModel()
                            {
                                Id = -13,
                                CompetitiveGroupId = (int)r["CompetitiveGroupID"],
                                CompetitiveGroupItemId = (int)r["CompetitiveGroupItemID"],
                                EducationSourceId = 20,
                                EducationFormId = 10,
                                CompetitiveGroupName = r["GroupName"] as string,
                                CompetitiveGroupItemName = r["DirectionName"] as string,
                                EducationFormName = AdmissionItemType.Where(x => x.ID == 10).FirstOrDefault().Name,
                                EducationSourceName = AdmissionItemType.Where(x => x.ID == 20).FirstOrDefault().Name
                            };
                            PrioritiesData.ApplicationPriorities.Add(priority);
                        }
                        /* ------------------------------------------------------------------------------------- */
                        #endregion
                        #region Платные места
                        /* ------------------------------------- Платные места --------------------------- */
                        if (Convert.ToInt32(r["NumberPaidO"] as int?) > 0)
                        {
                            ApplicationPriorityViewModel priority = new ApplicationPriorityViewModel()
                            {
                                Id = -21,
                                CompetitiveGroupId = (int)r["CompetitiveGroupID"],
                                CompetitiveGroupItemId = (int)r["CompetitiveGroupItemID"],
                                EducationSourceId = 15,
                                EducationFormId = 11,
                                CompetitiveGroupName = r["GroupName"] as string,
                                CompetitiveGroupItemName = r["DirectionName"] as string,
                                EducationFormName = AdmissionItemType.Where(x => x.ID == 11).FirstOrDefault().Name,
                                EducationSourceName = AdmissionItemType.Where(x => x.ID == 15).FirstOrDefault().Name
                            };
                            PrioritiesData.ApplicationPriorities.Add(priority);
                        }
                        if (Convert.ToInt32(r["NumberPaidOZ"] as int?) > 0)
                        {
                            ApplicationPriorityViewModel priority = new ApplicationPriorityViewModel()
                            {
                                Id = -22,
                                CompetitiveGroupId = (int)r["CompetitiveGroupID"],
                                CompetitiveGroupItemId = (int)r["CompetitiveGroupItemID"],
                                EducationSourceId = 15,
                                EducationFormId = 12,
                                CompetitiveGroupName = r["GroupName"] as string,
                                CompetitiveGroupItemName = r["DirectionName"] as string,
                                EducationFormName = AdmissionItemType.Where(x => x.ID == 12).FirstOrDefault().Name,
                                EducationSourceName = AdmissionItemType.Where(x => x.ID == 15).FirstOrDefault().Name
                            };
                            PrioritiesData.ApplicationPriorities.Add(priority);
                        }
                        if (Convert.ToInt32(r["NumberPaidZ"] as int?) > 0)
                        {
                            ApplicationPriorityViewModel priority = new ApplicationPriorityViewModel()
                            {
                                Id = -23,
                                CompetitiveGroupId = (int)r["CompetitiveGroupID"],
                                CompetitiveGroupItemId = (int)r["CompetitiveGroupItemID"],
                                EducationSourceId = 15,
                                EducationFormId = 10,
                                CompetitiveGroupName = r["GroupName"] as string,
                                CompetitiveGroupItemName = r["DirectionName"] as string,
                                EducationFormName = AdmissionItemType.Where(x => x.ID == 10).FirstOrDefault().Name,
                                EducationSourceName = AdmissionItemType.Where(x => x.ID == 15).FirstOrDefault().Name
                            };
                            PrioritiesData.ApplicationPriorities.Add(priority);
                        }
                        #endregion
                        #region Целевой прием
                        /* ------------------------------------------------------------------------------------- */
                        if (Convert.ToInt32(r["NumberTarget"] as int?) > 0)
                        {
                            ApplicationPriorityViewModel priority = new ApplicationPriorityViewModel()
                            {
                                Id = -31,
                                CompetitiveGroupId = (int)r["CompetitiveGroupID"],
                                CompetitiveGroupItemId = (int)r["CompetitiveGroupItemID"],
                                EducationSourceId = educationSourceId.Value,
                                EducationFormId = educationFormId.Value,
                                CompetitiveGroupName = r["GroupName"] as string,
                                CompetitiveGroupItemName = r["DirectionName"] as string,
                                EducationFormName = AdmissionItemType.Where(x => x.ID == 11).FirstOrDefault().Name,
                                EducationSourceName = AdmissionItemType.Where(x => x.ID == 16).FirstOrDefault().Name
                            };
                            priority.TargetOrganizations = ParseTargetOrganizations(r["TargetOrganizations"] as string).Distinct().OrderBy(y => y.Name).ToArray();
                            PrioritiesData.ApplicationPriorities.Add(priority);
                        } 

                        #endregion
                    }
                    r.Close();
                    con.Close();
                }
                catch (SqlException e)
                {
                    throw e;
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    if (con.State != ConnectionState.Closed) { con.Close(); }
                }
            }
            return PrioritiesData;
        }



    }
}