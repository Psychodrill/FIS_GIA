using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using GVUZ.Web.ViewModels;
using System.Configuration;
using System.Web.Configuration;
using GVUZ.Model.Entrants.Documents;
using GVUZ.Model.Helpers;
using GVUZ.Model.Entrants;
using GVUZ.Model;
using GVUZ.Helper;
using FogSoft.Helpers;
using GVUZ.Web.Helpers;
using GVUZ.Web.ViewModels.OrderOfAdmission;

namespace GVUZ.Web.ContextExtensionsSQL
{
    public static partial class OrderOfAdmissionSQL
    {

        private static string _connectionString;

        private static string ConnectionString
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


        //TODO: объединить GetIncludeToOrder GetMultiIncludeToOrder
        public static IncludeToOrderViewModel GetIncludeToOrder(int applicationId, int orderId)
        { 
            #region SQL
            
            string sql = @"
SELECT a.ApplicationID, a.InstitutionID, a.ApplicationNumber, a.RegistrationDate,
e.LastName + ' '+ e.FirstName + ' '+ IsNull (e.MiddleName,' ') as FIO,
' серия:'+' '+IsNull (ed.DocumentSeries,' ') + ' '+  ' номер:'+'  '+IsNull (ed.DocumentNumber,'') as Document 
FROM Application a (NOLOCK) 
  INNER JOIN Entrant e (NOLOCK) on a.EntrantID=e.EntrantID
  INNER JOIN EntrantDocument ed (NOLOCK) on ed.EntrantDocumentID = e.IdentityDocumentID
WHERE a.ApplicationID=@ApplicationID

SELECT OrderId, ord.InstitutionId,
	ord.OrderName, ord.OrderNumber, ord.OrderDate, 
	('№'+' '+ ord.OrderNumber + ' от '+ (CASE WHEN ord.OrderDate IS NULL THEN '' ELSE convert(varchar, ord.OrderDate, 104) END)) as OrderDetails,
	ord.IsForBeneficiary, ord.IsForeigner, 
	ord.[EducationLevelID], edu_levels.[Name] EducationLevelName,
	ord.[EducationFormID], 	edu_forms.[Name] EducationFormName,
	ord.[EducationSourceID], edu_sources.[Name] EducationSourceName,
	ord.[Stage]
FROM OrderOfAdmission ord(NOLOCK) 
	left join [dbo].[Campaign] cmp (NOLOCK) on ord.[CampaignID] = cmp.[CampaignID]
	left join [dbo].[AdmissionItemType] edu_levels (NOLOCK) on ord.[EducationLevelID] = edu_levels.[ItemTypeID]
	left join [dbo].[AdmissionItemType] edu_forms (NOLOCK) on ord.[EducationFormID] = edu_forms.[ItemTypeID]
	left join [dbo].[AdmissionItemType] edu_sources (NOLOCK) on ord.[EducationSourceID] = edu_sources.[ItemTypeID]
WHERE  ord.OrderId = @OrderId

SELECT IdLevelBudget, BudgetName FROM LevelBudget  ORDER BY IdLevelBudget

SELECT 
	acgi.id AppCGItemId ,
	cg.Name CompetitiveGroupName,
	dir.Name DirectionName,
	lvl.Name EducationLevelName,
	frm.Name EducationFormName,
	src.Name EducationSourceName,
	acgi.EducationSourceId as EducationSourceID,
	( ISNULL((
	(select SUM(ISNULL(mres,0))  from (
		select etic.EntranceTestPriority, MAX( aetd.ResultValue ) as mres
			FROM ApplicationEntranceTestDocument aetd (NOLOCK)  
            INNER JOIN EntranceTestItemC AS etic (NOLOCK) ON etic.EntranceTestItemID = aetd.EntranceTestItemID
		WHERE aetd.ApplicationID=@ApplicationID AND etic.CompetitiveGroupID=cg.CompetitiveGroupID 
			and not exists (Select * FROM ApplicationEntranceTestDocument aetd2 (NOLOCK)  
							INNER JOIN EntranceTestItemC AS etic2 (NOLOCK) ON etic2.EntranceTestItemID = aetd2.EntranceTestItemID
							Where aetd2.ApplicationID=@ApplicationID AND etic2.CompetitiveGroupID=cg.CompetitiveGroupID 
							and etic2.ReplacedEntranceTestItemID = etic.EntranceTestItemID
						)
						group by etic.EntranceTestPriority 
	 ) as maxprior )
	), 0)
	+  (CASE WHEN ISNULL((SELECT SUM(ISNULL(ia.IAMark,0)) FROM IndividualAchivement ia WHERE ia.ApplicationID=app.ApplicationID),0) > 10 and lvl.ItemTypeID <> 18 
	THEN 10 ELSE  ISNULL((SELECT SUM(ISNULL(ia.IAMark,0)) FROM IndividualAchivement ia WHERE ia.ApplicationID=app.ApplicationID),0) END) ) as Points,
	(SELECT COUNT(DISTINCT a_acgi.ApplicationID) 
	 FROM ApplicationCompetitiveGroupItem a_acgi (NOLOCK) 
        INNER JOIN [Application] a (NOLOCK) on a.ApplicationID = a_acgi.ApplicationId
		INNER JOIN OrderOfAdmission a_ooa (NOLOCK) on a_ooa.OrderID = a_acgi.OrderOfAdmissionID AND a_ooa.IsForeigner=0
	 WHERE 
        a.StatusID = 8 
        AND a_acgi.OrderOfExceptionId IS NULL
		AND a_acgi.CompetitiveGroupID = acgi.CompetitiveGroupID
		AND a_acgi.EducationFormID = acgi.EducationFormID
		AND a_acgi.EducationSourceId = acgi.EducationSourceId
		AND a.InstitutionID = app.InstitutionID		
		AND (a_acgi.CompetitiveGroupTargetId = acgi.CompetitiveGroupTargetID 
            OR (a_acgi.CompetitiveGroupTargetId IS NULL AND acgi.CompetitiveGroupTargetID IS NULL))
	) as CGCount,	
	ISNULL(KCP.CGTotal,0) as CGTotal,
	(SELECT COUNT(DISTINCT a_acgi.ApplicationID) 
	 FROM ApplicationCompetitiveGroupItem a_acgi (NOLOCK) 
        INNER JOIN [Application] a (NOLOCK) on a.ApplicationID = a_acgi.ApplicationId
        INNER JOIN OrderOfAdmission a_ooa (NOLOCK) on a_ooa.OrderID = a_acgi.OrderOfAdmissionID AND a_ooa.IsForeigner=0
		INNER JOIN CompetitiveGroup a_cg (NOLOCK) on a_cg.CompetitiveGroupId = a_acgi.CompetitiveGroupId AND a_cg.CampaignID=cg.CampaignID
	 WHERE 
        a.StatusID = 8 
        AND a_acgi.OrderOfExceptionId IS NULL
		AND a_acgi.EducationFormID = acgi.EducationFormID
		AND a_acgi.EducationSourceId = acgi.EducationSourceId
		AND a_acgi.OrderIdLevelBudget = 1		
		AND a.InstitutionID = app.InstitutionID		
		AND a_cg.DirectionID = cg.DirectionID		
		AND a_cg.EducationLevelID = cg.EducationLevelID		
		AND a_cg.CampaignID = cg.CampaignID
	  ) as CGBFedCount,	
	ISNULL(KCP.CGBFedTotal,0) as CGBFedTotal,
(SELECT COUNT(DISTINCT a_acgi.ApplicationID) 
	 FROM ApplicationCompetitiveGroupItem a_acgi (NOLOCK) 
        INNER JOIN [Application] a (NOLOCK) on a.ApplicationID = a_acgi.ApplicationId
		INNER JOIN OrderOfAdmission a_ooa (NOLOCK) on a_ooa.OrderID = a_acgi.OrderOfAdmissionID AND a_ooa.IsForeigner=0
		INNER JOIN CompetitiveGroup a_cg (NOLOCK) on a_cg.CompetitiveGroupId = a_acgi.CompetitiveGroupId AND a_cg.CampaignID=cg.CampaignID
	 WHERE 
        a.StatusID = 8 
        AND a_acgi.OrderOfExceptionId IS NULL
		AND a_acgi.EducationFormID = acgi.EducationFormID
		AND a_acgi.EducationSourceId = acgi.EducationSourceId
		AND a_acgi.OrderIdLevelBudget = 2		
		AND a.InstitutionID = app.InstitutionID		
		AND a_cg.DirectionID = cg.DirectionID		
		AND a_cg.EducationLevelID = cg.EducationLevelID		
		AND a_cg.CampaignID = cg.CampaignID
	  )	
	as CGBRegCount,
	ISNULL(KCP.CGBRegTotal,0) as CGBRegTotal,
	(SELECT COUNT(DISTINCT a_acgi.ApplicationID) 
	 FROM ApplicationCompetitiveGroupItem a_acgi (NOLOCK) 
        INNER JOIN [Application] a (NOLOCK) on a.ApplicationID = a_acgi.ApplicationId
		INNER JOIN OrderOfAdmission a_ooa (NOLOCK) on a_ooa.OrderID = a_acgi.OrderOfAdmissionID AND a_ooa.IsForeigner=0
		INNER JOIN CompetitiveGroup a_cg (NOLOCK) on a_cg.CompetitiveGroupId = a_acgi.CompetitiveGroupId AND a_cg.CampaignID=cg.CampaignID
	 WHERE 
        a.StatusID = 8 
        AND a_acgi.OrderOfExceptionId IS NULL
		AND a_acgi.EducationFormID = acgi.EducationFormID
		AND a_acgi.EducationSourceId = acgi.EducationSourceId
		AND a_acgi.OrderIdLevelBudget = 3		
		AND a.InstitutionID = app.InstitutionID		
		AND a_cg.DirectionID = cg.DirectionID		
		AND a_cg.EducationLevelID = cg.EducationLevelID		
		AND a_cg.CampaignID = cg.CampaignID
	  ) as CGBMunCount
	   ,ISNULL(KCP.CGBMunTotal,0) as CGBMunTotal
       ,ISNULL(cg.IdLevelBudget, 0) as IdLevelBudget
FROM Application app (NOLOCK) 
	INNER JOIN ApplicationCompetitiveGroupItem acgi (NOLOCK) on acgi.ApplicationID = app.ApplicationID
	INNER JOIN CompetitiveGroup cg (NOLOCK) on acgi.CompetitiveGroupID = cg.CompetitiveGroupId
    INNER JOIN Campaign c (NOLOCK) on cg.CampaignID = c.CampaignID
	LEFT JOIN Direction dir (NOLOCK) on cg.DirectionId = dir.DirectionID
	LEFT JOIN Direction dirP(NOLOCK) ON cg.ParentDirectionID=dir.ParentID
	INNER JOIN AdmissionItemType lvl (NOLOCK) on cg.EducationLevelID = lvl.ItemTypeID
	INNER JOIN AdmissionItemType frm (NOLOCK) on acgi.EducationFormId = frm.ItemTypeID
	INNER JOIN AdmissionItemType src (NOLOCK) on acgi.EducationSourceId = src.ItemTypeID
	CROSS APPLY dbo.FGetKCP(acgi.id) KCP
	INNER JOIN OrderOfAdmission oa (NOLOCK) on 
		 ((oa.EducationLevelID IS NOT NULL AND cg.EducationLevelID=oa.EducationLevelID) OR (oa.EducationLevelID IS NULL))
	    AND ((oa.EducationFormID IS NOT NULL AND acgi.EducationFormID = oa.EducationFormID) OR (oa.EducationFormID IS NULL))
	    AND ((oa.EducationSourceId IS NOT NULL AND acgi.EducationSourceId = oa.EducationSourceId) OR (oa.EducationSourceId IS NULL))
WHERE 
    app.ApplicationID = @ApplicationID 
    AND oa.OrderID = @OrderID
    AND ((acgi.IsAgreed = 1 AND ISNULL(acgi.IsDisagreed,0) = 0) OR c.CampaignTypeID NOT IN (1,2))

SELECT DISTINCT b.BenefitID, b.ShortName as BenefitName
FROM Application app (NOLOCK) 
	INNER JOIN ApplicationCompetitiveGroupItem acgi (NOLOCK) on acgi.ApplicationID = app.ApplicationID
	INNER JOIN CompetitiveGroup cg (NOLOCK) on acgi.CompetitiveGroupID = cg.CompetitiveGroupId
	INNER JOIN ApplicationEntranceTestDocument aetd (NOLOCK) on aetd.ApplicationID=app.ApplicationID AND aetd.CompetitiveGroupID=acgi.CompetitiveGroupID
	INNER JOIN Benefit b (NOLOCK) on b.BenefitID=aetd.BenefitID
	INNER JOIN OrderOfAdmission oa (NOLOCK) on 
		((oa.EducationLevelID IS NOT NULL AND cg.EducationLevelID=oa.EducationLevelID) OR (oa.EducationLevelID IS NULL))	
	AND ((oa.EducationFormID IS NOT NULL AND acgi.EducationFormID = oa.EducationFormID) OR (oa.EducationFormID IS NULL))
	AND ((oa.EducationSourceId IS NOT NULL AND acgi.EducationSourceId = oa.EducationSourceId) OR (oa.EducationSourceId IS NULL))
WHERE app.ApplicationID=@ApplicationID AND oa.OrderID=@OrderID
";
            #endregion
            #region SQL Exec
            //IncludeToOrderViewModel o = null;
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("ApplicationID", applicationId));
                com.Parameters.Add(new SqlParameter("OrderID", orderId));

                //com.Parameters.Add(new SqlParameter("InstitutionID",InstitutionID));
                SqlDataAdapter da = new SqlDataAdapter(com);
                try
                {
                    con.Open();
                    da.Fill(ds);
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
            #endregion
            #region IncludeToOrderViewModel
            IncludeToOrderViewModel m = new IncludeToOrderViewModel();

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    m.App = new IncludeToOrderViewModel.Application();
                    DataRow r = ds.Tables[0].Rows[0];
                    m.App.ApplicationID = (int)r["ApplicationID"];
                    m.App.InstitutionID = (int)r["InstitutionID"];
                    m.App.Number = r["ApplicationNumber"] as string;
                    m.App.FIO = r["FIO"] as string;
                    m.App.Document = r["Document"] as string;
                    m.App.RegistrationDate = r["RegistrationDate"] as DateTime?;
                }
            }
            #region IncludeToOrderViewModel.Order
            if (ds.Tables.Count > 1)
            {
                if (ds.Tables[1].Rows.Count > 0)
                {
                    m.Ord = new IncludeToOrderViewModel.Order();
                    DataRow r = ds.Tables[1].Rows[0];
                    m.Ord.OrderID = (int)r["OrderId"];
                    m.Ord.InstitutionID = (int)r["InstitutionId"];
                    m.Ord.OrderStatusId = 0;
                    m.Ord.IsCampaignFinished = false;
                    m.Ord.OrderName = r["OrderName"] as string;
                    m.Ord.OrderNumber = r["OrderNumber"] as string;
                    m.Ord.OrderDate = r["OrderDate"] as DateTime?;
                    m.Ord.OrderDetails = r["OrderDetails"] as string;
                    m.Ord.IsForBeneficiary = r["IsForBeneficiary"] as bool?;
                    m.Ord.IsForeigner = r["IsForeigner"] as bool?;
                    m.Ord.EducationLevel = r["EducationLevelName"] as string;
                    m.Ord.EducationForm = r["EducationFormName"] as string;
                    m.Ord.EducationSource = r["EducationSourceName"] as string;
                    m.Ord.Stage = Convert.ToString(r["Stage"]);
                    m.Ord.EducationLevel = r["EducationLevelName"] as string;
                }
            }
            #endregion
            #region IncludeToOrderViewModel.BudgetItem
            if (ds.Tables.Count > 2)
            {
                if (ds.Tables[2].Rows.Count > 0)
                {
                    m.BudgetLevels.Add(new IncludeToOrderViewModel.BudgetItem() { ID = null, Name = "Не выбрано" });
                    for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                    {
                        var r = ds.Tables[2].Rows[i];
                        m.BudgetLevels.Add(new IncludeToOrderViewModel.BudgetItem() { ID = (int)r[0], Name = r[1] as string });
                    }
                }
            }
            #endregion
            #region IncludeToOrderViewModel.ConditionInclude
            if (ds.Tables.Count > 3)
            {
                for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                {
                    var r = ds.Tables[3].Rows[i];
                    IncludeToOrderViewModel.ConditionInclude c = new IncludeToOrderViewModel.ConditionInclude();
                    c.AppCGItemId = (int)r["AppCGItemId"];
                    c.CompetitiveGroupName = r["CompetitiveGroupName"] as string;
                    c.DirectionName = r["DirectionName"] as string;
                    c.EduLevelName = r["EducationLevelName"] as string;
                    c.EducationFormName = r["EducationFormName"] as string;
                    c.EducationSourceName = r["EducationSourceName"] as string;
                    c.EducationSourceID = r["EducationSourceID"] as int?;
                    c.Points = Convert.ToDecimal(r["Points"]);
                    //c.Benefit=r["Benefit"] as string;
                    c.CGTotal = Convert.ToInt32(r["CGTotal"]); c.CGCount = Convert.ToInt32(r["CGCount"]);
                    c.CGBFedTotal = Convert.ToInt32(r["CGBFedTotal"]); c.CGBFedCount = Convert.ToInt32(r["CGBFedCount"]);
                    c.CGBRegTotal = Convert.ToInt32(r["CGBRegTotal"]); c.CGBRegCount = Convert.ToInt32(r["CGBRegCount"]);
                    c.CGBMunTotal = Convert.ToInt32(r["CGBMunTotal"]); c.CGBMunCount = Convert.ToInt32(r["CGBMunCount"]);
                    c.IdLevelBudget = Convert.ToInt32(r["IdLevelBudget"]); 
                    m.ConditionIncludes.Add(c);
                }
            }
            #endregion
            #region IncludeToOrderViewModel.Benefit
            if (ds.Tables.Count > 4)
            {
                if (ds.Tables[4].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[4].Rows.Count; i++)
                    {
                        var r = ds.Tables[4].Rows[i];
                        m.Benefits.Add(new IncludeToOrderViewModel.Benefit() { BenefitID = Convert.ToInt32(r[0]), BenefitName = r[1] as string });
                    }
                }
            }
            #endregion
            #endregion
            int InstitutionID = InstitutionHelper.GetInstitutionID();
            if (m.App.InstitutionID != InstitutionID || m.Ord.InstitutionID != InstitutionID) { return null; }

            return m;
        }

        public static MultiIncludeToOrderViewModel GetMultiIncludeToOrder(int[] ApplicationIds, int OrderID)
        {
            int InstitutionID = InstitutionHelper.GetInstitutionID();

            #region SQL
            string sql = @" 
DECLARE @CompetitiveGroupID	INT 
DECLARE @EducationFormID	INT
DECLARE @EducationSourceID	INT
DECLARE @AppCGItemID	INT
DECLARE @AppCount	INT

DECLARE @Apps TABLE (ApplicationID INT)
INSERT INTO @Apps
{0}

----== Количество заявлений ==----
SELECT  @AppCount=COUNT(*) FROM @Apps AS a;

----== Выборка условий ==----
SELECT TOP 1 
    @CompetitiveGroupID=cg.CompetitiveGroupID, 
    @EducationFormId=cg.EducationFormId, 
    @EducationSourceId=cg.EducationSourceId
FROM @Apps a
	INNER JOIN ApplicationCompetitiveGroupItem AS acgi (NOLOCK) ON acgi.ApplicationId = a.ApplicationID
	INNER JOIN CompetitiveGroup AS cg (NOLOCK) ON cg.CompetitiveGroupID = acgi.CompetitiveGroupID
    INNER JOIN Campaign AS c (NOLOCK) ON c.CampaignID = cg.CampaignID
WHERE
    C.CampaignTypeID not in (1, 2) OR (ACGI.IsAgreed = 1 AND ISNULL(ACGI.IsDisagreed, 0) = 0)
GROUP BY 
    cg.CompetitiveGroupID,  
    cg.EducationFormId, 
    cg.EducationSourceId
HAVING  
    COUNT(DISTINCT a.ApplicationID) = @AppCount

SELECT TOP 1 
    @AppCGItemID=acgi.id 
FROM ApplicationCompetitiveGroupItem AS acgi (NOLOCK) 
	INNER JOIN CompetitiveGroup cg (NOLOCK) on acgi.CompetitiveGroupID = cg.CompetitiveGroupId
	INNER JOIN Direction dir (NOLOCK) on cg.DirectionId = dir.DirectionID   
	INNER JOIN @Apps aa ON aa.ApplicationID=acgi.ApplicationID			          
WHERE 
    cg.CompetitiveGroupID=@CompetitiveGroupID 
    AND cg.EducationFormId=@EducationFormID 
    AND cg.EducationSourceID=@EducationSourceID

----==== Приказ ==== ----
SELECT 
    OrderId, ord.InstitutionId,
	ord.OrderName, ord.OrderNumber, ord.OrderDate, 
	('№'+' '+ ord.OrderNumber + ' от '+ (CASE WHEN ord.OrderDate IS NULL THEN '' ELSE convert(varchar, ord.OrderDate, 104) END)) as OrderDetails,
	ord.IsForBeneficiary, ord.IsForeigner, 
	ord.[EducationLevelID], edu_levels.[Name] EducationLevelName, 
	ord.[EducationFormID], 	edu_forms.[Name] EducationFormName,
	ord.[EducationSourceID], edu_sources.[Name] EducationSourceName,
	ord.[Stage]
FROM 
    OrderOfAdmission ord (NOLOCK) 
	inner join [dbo].[Campaign] cmp (NOLOCK) on ord.[CampaignID] = cmp.[CampaignID]
	left join [dbo].[AdmissionItemType] edu_levels (NOLOCK) on ord.[EducationLevelID] = edu_levels.[ItemTypeID]
	left join [dbo].[AdmissionItemType] edu_forms (NOLOCK) on ord.[EducationFormID] = edu_forms.[ItemTypeID]
	left join [dbo].[AdmissionItemType] edu_sources (NOLOCK) on ord.[EducationSourceID] = edu_sources.[ItemTypeID]
WHERE  
    ord.OrderId = @OrderId 

----==== Уровни бюджета ==== ----
SELECT IdLevelBudget, BudgetName FROM LevelBudget (NOLOCK) ORDER BY IdLevelBudget

----==== Заявления ==== ----
SELECT 
    a.ApplicationID, 
    a.InstitutionID, 
    a.ApplicationNumber, 
    a.RegistrationDate,
    FIO = e.LastName + ' '+ e.FirstName + ' '+ IsNull (e.MiddleName,' '),
    Document = ' серия:'+' '+IsNull (ed.DocumentSeries,' ') + ' '+  ' номер:'+'  '+IsNull (ed.DocumentNumber,''),
    Points =
        (ISNULL((SELECT SUM(ISNULL(aetd.ResultValue,0)) 
			FROM ApplicationEntranceTestDocument aetd (NOLOCK)  
            INNER JOIN EntranceTestItemC AS etic (NOLOCK) ON etic.EntranceTestItemID = aetd.EntranceTestItemID
		WHERE aetd.ApplicationID=a.ApplicationID AND etic.CompetitiveGroupID=@CompetitiveGroupID 
			and not exists (Select * FROM ApplicationEntranceTestDocument aetd2 (NOLOCK)  
							INNER JOIN EntranceTestItemC AS etic2 (NOLOCK) ON etic2.EntranceTestItemID = aetd2.EntranceTestItemID
							Where aetd2.ApplicationID=a.ApplicationID AND etic2.CompetitiveGroupID=@CompetitiveGroupID 
							and etic2.ReplacedEntranceTestItemID = etic.EntranceTestItemID
						)
	    ), 0)
	    + ISNULL((SELECT SUM(ISNULL(ia.IAMark,0)) FROM IndividualAchivement ia WHERE ia.ApplicationID=a.ApplicationID), 0))
    ,
    AppCGItemId =
        (SELECT TOP 1 
            acgi.id
        FROM ApplicationCompetitiveGroupItem AS acgi (NOLOCK) 
	        INNER JOIN CompetitiveGroup cg (NOLOCK) on cg.CompetitiveGroupID = acgi.CompetitiveGroupId
	        INNER JOIN Direction dir (NOLOCK) on cg.DirectionId = dir.DirectionID             
        WHERE acgi.ApplicationID = a.ApplicationID 
	        AND cg.CompetitiveGroupID=@CompetitiveGroupID 
	        AND acgi.EducationFormId=@EducationFormID 
            AND acgi.EducationSourceID=@EducationSourceID
        )
FROM 
    [Application] a (NOLOCK)
    INNER JOIN @Apps aa ON aa.ApplicationID=a.ApplicationID	  
    INNER JOIN Entrant e (NOLOCK) on e.EntrantID=a.EntrantID
    INNER JOIN EntrantDocument ed (NOLOCK) on ed.EntrantDocumentID = e.IdentityDocumentID
ORDER BY 
    Points DESC

---=== Условие приема ==========================================--
SELECT 
    acgi.id AppCGItemId,
	cg.Name CompetitiveGroupName,	
    dir.Name DirectionName,	
    lvl.Name EducationLevelName,	
    frm.Name EducationFormName,	
    src.Name EducationSourceName,
	acgi.EducationSourceId as EducationSourceID,
	(SELECT 
        COUNT(DISTINCT a_acgi.ApplicationID) 
	 FROM 
        ApplicationCompetitiveGroupItem a_acgi (NOLOCK) 
        INNER JOIN [Application] a (NOLOCK) on a.ApplicationID = a_acgi.ApplicationId
		INNER JOIN OrderOfAdmission a_ooa (NOLOCK) on a_ooa.OrderID = a_acgi.OrderOfAdmissionID AND a_ooa.IsForeigner=0
	 WHERE 
        a.StatusID = 8 
        AND a_acgi.OrderOfExceptionId IS NULL
		AND a_acgi.CompetitiveGroupID = acgi.CompetitiveGroupID
		AND a_acgi.EducationFormID = acgi.EducationFormID
		AND a_acgi.EducationSourceId = acgi.EducationSourceId
		AND a.InstitutionID = app.InstitutionID		
		AND (a_acgi.CompetitiveGroupTargetID = acgi.CompetitiveGroupTargetID 
            OR (a_acgi.CompetitiveGroupTargetID IS NULL AND acgi.CompetitiveGroupTargetID IS NULL))
	) as CGCount,	
	ISNULL(KCP.CGTotal,0) as CGTotal,
	(SELECT 
        COUNT (DISTINCT a_acgi.ApplicationID) 
	 FROM 
        ApplicationCompetitiveGroupItem a_acgi (NOLOCK) 
        INNER JOIN [Application] a (NOLOCK) on a.ApplicationID = a_acgi.ApplicationId
		INNER JOIN OrderOfAdmission a_ooa (NOLOCK) on a_ooa.OrderID = a_acgi.OrderOfAdmissionID AND a_ooa.IsForeigner=0
		INNER JOIN CompetitiveGroup a_cg (NOLOCK) on a_cg.CompetitiveGroupId = a_acgi.CompetitiveGroupID AND a_cg.CampaignID = cg.CampaignID
	 WHERE 
        a.StatusID = 8 
        AND a_acgi.OrderOfExceptionId IS NULL
		AND a_acgi.EducationFormID = acgi.EducationFormID
		AND a_acgi.EducationSourceId = acgi.EducationSourceId
		AND a_acgi.OrderIdLevelBudget = 1		
		AND a.InstitutionID = app.InstitutionID		
		AND a_cg.DirectionID = cg.DirectionID		
		AND a_cg.EducationLevelID = cg.EducationLevelID		
		AND a_cg.CampaignID = cg.CampaignID
	  ) as CGBFedCount,
	ISNULL(KCP.CGBFedTotal,0) as CGBFedTotal,
    (SELECT 
        COUNT (DISTINCT a_acgi.ApplicationID) 
	 FROM 
        ApplicationCompetitiveGroupItem a_acgi (NOLOCK) 
        INNER JOIN [Application] a (NOLOCK) on a.ApplicationID = a_acgi.ApplicationId
		INNER JOIN OrderOfAdmission a_ooa (NOLOCK) on a_ooa.OrderID = a_acgi.OrderOfAdmissionID AND a_ooa.IsForeigner=0
		INNER JOIN CompetitiveGroup a_cg (NOLOCK) on a_cg.CompetitiveGroupId = a_acgi.CompetitiveGroupID AND a_cg.CampaignID = cg.CampaignID
	 WHERE 
        a.StatusID = 8 
        AND a_acgi.OrderOfExceptionId IS NULL
		AND a_acgi.EducationFormID = acgi.EducationFormID
		AND a_acgi.EducationSourceId = acgi.EducationSourceId
		AND a_acgi.OrderIdLevelBudget = 2		
		AND a.InstitutionID = app.InstitutionID		
		AND a_cg.DirectionID = cg.DirectionID		
		AND a_cg.EducationLevelID = cg.EducationLevelID		
		AND a_cg.CampaignID = cg.CampaignID
	  )	
	as CGBRegCount,
	ISNULL(KCP.CGBRegTotal,0) as CGBRegTotal,
	(SELECT 
        COUNT (DISTINCT a_acgi.ApplicationID) 
	 FROM 
        ApplicationCompetitiveGroupItem a_acgi (NOLOCK) 
        INNER JOIN [Application] a (NOLOCK) on a.ApplicationID = a_acgi.ApplicationId
		INNER JOIN OrderOfAdmission a_ooa (NOLOCK) on a_ooa.OrderID = a_acgi.OrderOfAdmissionID AND a_ooa.IsForeigner=0
		INNER JOIN CompetitiveGroup a_cg (NOLOCK) on a_cg.CompetitiveGroupId = a_acgi.CompetitiveGroupID AND a_cg.CampaignID = cg.CampaignID
	 WHERE 
        a.StatusID = 8 
        AND a_acgi.OrderOfExceptionId IS NULL
		AND a_acgi.EducationFormID = acgi.EducationFormID
		AND a_acgi.EducationSourceId = acgi.EducationSourceId
		AND a_acgi.OrderIdLevelBudget = 3		
		AND a.InstitutionID = app.InstitutionID		
		AND a_cg.DirectionID = cg.DirectionID		
		AND a_cg.EducationLevelID = cg.EducationLevelID		
		AND a_cg.CampaignID = cg.CampaignID
	  ) as CGBMunCount
	 ,ISNULL(KCP.CGBMunTotal, 0) as CGBMunTotal
     ,ISNULL(cg.IdLevelBudget, 0) as IdLevelBudget
FROM 
    Application app (NOLOCK)
	INNER JOIN ApplicationCompetitiveGroupItem acgi (NOLOCK) on acgi.ApplicationID = app.ApplicationID
	INNER JOIN CompetitiveGroup cg (NOLOCK) on acgi.CompetitiveGroupID = cg.CompetitiveGroupId
    INNER JOIN Campaign c (NOLOCK) on cg.CampaignID = c.CampaignID
	INNER JOIN Direction dir (NOLOCK) on cg.DirectionId = dir.DirectionID
	INNER JOIN AdmissionItemType lvl (NOLOCK) on cg.EducationLevelID = lvl.ItemTypeID
	INNER JOIN AdmissionItemType frm (NOLOCK) on acgi.EducationFormId = frm.ItemTypeID
	INNER JOIN AdmissionItemType src (NOLOCK) on acgi.EducationSourceId = src.ItemTypeID
	CROSS APPLY dbo.FGetKCP(acgi.id) KCP
	INNER JOIN OrderOfAdmission oa (NOLOCK) on ((oa.EducationLevelID IS NOT NULL AND cg.EducationLevelID=oa.EducationLevelID) OR (oa.EducationLevelID IS NULL))								
	    AND ((oa.EducationFormID IS NOT NULL AND acgi.EducationFormID = oa.EducationFormID) OR (oa.EducationFormID IS NULL))
	    AND ((oa.EducationSourceId IS NOT NULL AND acgi.EducationSourceId = oa.EducationSourceId) OR (oa.EducationSourceId IS NULL))
WHERE 
    oa.OrderID = @OrderID 
    AND acgi.id = @AppCGItemID
    AND ((acgi.IsAgreed = 1 AND ISNULL(acgi.IsDisagreed,0) = 0) OR c.CampaignTypeID NOT IN (1,2))
    
----== ЛЬГОТЫ ==----
SELECT 
    aetd.BenefitID, 
    b.ShortName AS BenefitName
FROM 
    ApplicationEntranceTestDocument AS aetd(NOLOCK) 
    LEFT JOIN EntranceTestItemC AS etic (NOLOCK) ON etic.EntranceTestItemID = aetd.EntranceTestItemID
    LEFT JOIN Benefit AS b (NOLOCK) ON b.BenefitID = aetd.BenefitID  
    INNER JOIN @Apps a ON a.ApplicationID=aetd.ApplicationID      
WHERE 
    ISNULL(aetd.CompetitiveGroupID, etic.CompetitiveGroupID) = @CompetitiveGroupID  
    AND aetd.BenefitID IS NOT NULL
GROUP BY
    aetd.BenefitID, 
    b.ShortName
HAVING 
    COUNT(DISTINCT aetd.ApplicationID) = @AppCount 
";
            #endregion

            string sApplicationIds = "";

            #region SQL Exec

            int count = ApplicationIds.Count();

            if (count > 0) { sApplicationIds = "SELECT " + ApplicationIds[0].ToString(); }
            for (var i = 1; i < count; i++)
            {
                sApplicationIds += " UNION SELECT " + ApplicationIds[i].ToString() + " ";
            }

            LogHelper.Log.Debug(string.Format("GetMultiIncludeToOrder applicationIds: {0}", sApplicationIds));

            MultiIncludeToOrderViewModel m = new MultiIncludeToOrderViewModel();
            //sApplicationIds = String.Join(",", ApplicationIds);
            sql = String.Format(sql, sApplicationIds);
            LogHelper.Log.Debug(string.Format("GetMultiIncludeToOrder sql: {0}", sql));

            //IncludeToOrderViewModel o = null;
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("OrderID", OrderID));
                com.Parameters.Add(new SqlParameter("InstitutionID", InstitutionID));
                SqlDataAdapter da = new SqlDataAdapter(com);
                try
                {
                    con.Open();
                    da.Fill(ds);
                    con.Close();
                    LogHelper.Log.Debug(string.Format("GetMultiIncludeToOrder query - ok!"));
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
            #endregion

            #region MultiIncludeToOrderViewModel.Order
            if (ds.Tables.Count > 0)
            {
                LogHelper.Log.Debug(string.Format("GetMultiIncludeToOrder ds.Tables.Count > 0"));

                LogHelper.Log.Debug(string.Format("GetMultiIncludeToOrder ds.Tables[0]"));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    m.Ord = new MultiIncludeToOrderViewModel.Order();
                    DataRow r = ds.Tables[0].Rows[0];
                    m.Ord.OrderID = (int)r["OrderId"];
                    m.Ord.InstitutionID = (int)r["InstitutionId"];
                    m.Ord.OrderStatusId = 0;
                    m.Ord.IsCampaignFinished = false;
                    m.Ord.OrderName = r["OrderName"] as string;
                    m.Ord.OrderNumber = r["OrderNumber"] as string;
                    m.Ord.OrderDate = r["OrderDate"] as DateTime?;
                    m.Ord.OrderDetails = r["OrderDetails"] as string;
                    m.Ord.IsForBeneficiary = r["IsForBeneficiary"] as bool?;
                    m.Ord.IsForeigner = r["IsForeigner"] as bool?;
                    m.Ord.EducationLevel = r["EducationLevelName"] as string;
                    m.Ord.EducationForm = r["EducationFormName"] as string;
                    m.Ord.EducationSource = r["EducationSourceName"] as string;
                    m.Ord.Stage = Convert.ToString(r["Stage"]);
                }
            }
            #endregion
            #region MultiIncludeToOrderViewModel.BudgetItem
            if (ds.Tables.Count > 1)
            {
                if (ds.Tables[1].Rows.Count > 0)
                {
                    LogHelper.Log.Debug(string.Format("GetMultiIncludeToOrder ds.Tables[1]"));
                    m.BudgetLevels.Add(new MultiIncludeToOrderViewModel.BudgetItem() { ID = null, Name = "Не выбрано" });
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        var r = ds.Tables[1].Rows[i];
                        m.BudgetLevels.Add(new MultiIncludeToOrderViewModel.BudgetItem() { ID = (int)r[0], Name = r[1] as string });
                    }
                }
            }
            #endregion
            #region MultiIncludeToOrderViewModel.Application
            m.Apps = new List<MultiIncludeToOrderViewModel.Application>();
            if (ds.Tables.Count > 2)
            {
                LogHelper.Log.Debug(string.Format("GetMultiIncludeToOrder ds.Tables[2]"));
                for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                {
                    var r = ds.Tables[2].Rows[i];
                    var a = new MultiIncludeToOrderViewModel.Application(); ;
                    a.ApplicationID = (int)r["ApplicationID"];
                    a.InstitutionID = (int)r["InstitutionID"];
                    a.AppCGItemId = r["AppCGItemID"] == DBNull.Value ? 0 : Convert.ToInt32(r["AppCGItemID"]);
                    a.Number = r["ApplicationNumber"] as string;
                    a.RegistrationDate = r["RegistrationDate"] as DateTime?;
                    a.FIO = r["FIO"] as string;
                    a.Document = r["Document"] as string;
                    a.Points = Convert.ToDecimal(r["Points"]);
                    m.Apps.Add(a);
                }
            }
            #endregion
            #region MultiIncludeToOrderViewModel.ConditionInclude
            m.Condition = new MultiIncludeToOrderViewModel.ConditionInclude();
            if (ds.Tables.Count > 3)
            {
                if (ds.Tables[3].Rows.Count > 0)
                {
                    LogHelper.Log.Debug(string.Format("GetMultiIncludeToOrder ds.Tables[3]"));
                    var r = ds.Tables[3].Rows[0];
                    var c = m.Condition;
                    c.CompetitiveGroupName = r["CompetitiveGroupName"] as string;
                    c.DirectionName = r["DirectionName"] as string;
                    c.EduLevelName = r["EducationLevelName"] as string;
                    c.EducationFormName = r["EducationFormName"] as string;
                    c.EducationSourceName = r["EducationSourceName"] as string;
                    c.EducationSourceID = r["EducationSourceId"] as int?;
                    c.CGTotal = Convert.ToInt32(r["CGTotal"]); c.CGCount = Convert.ToInt32(r["CGCount"]);
                    c.CGBFedTotal = Convert.ToInt32(r["CGBFedTotal"]); c.CGBFedCount = Convert.ToInt32(r["CGBFedCount"]);
                    c.CGBRegTotal = Convert.ToInt32(r["CGBRegTotal"]); c.CGBRegCount = Convert.ToInt32(r["CGBRegCount"]);
                    c.CGBMunTotal = Convert.ToInt32(r["CGBMunTotal"]); c.CGBMunCount = Convert.ToInt32(r["CGBMunCount"]);
                    c.IdLevelBudget = Convert.ToInt32(r["IdLevelBudget"]);
                }
            }

            #endregion
            #region IncludeToOrderViewModel.Benefit
            if (ds.Tables.Count > 4)
            {
                if (ds.Tables[4].Rows.Count > 0)
                {
                    LogHelper.Log.Debug(string.Format("GetMultiIncludeToOrder ds.Tables[4]"));
                    for (int i = 0; i < ds.Tables[4].Rows.Count; i++)
                    {
                        var r = ds.Tables[4].Rows[i];
                        m.Benefits.Add(new MultiIncludeToOrderViewModel.Benefit() { BenefitID = Convert.ToInt32(r[0]), BenefitName = r[1] as string });
                    }
                }
            }
            #endregion
            return m;
        }

        public static List<IncludeToOrderViewModel.CheckError> FuncCheckIncludeAppToOrder(int? ApplicationID, int? OrderID, int? AppCGItemID, int? IdLevelBudget, int? BenefitID)
        {
            string sql = "SELECT ID, Msg FROM [dbo].[FuncCheckIncludeAppToOrder] (@ApplicationID, @OrderID, @InstitutionID, @IdLevelBudget, @AppCGItemId, @BenefitID)";

            List<IncludeToOrderViewModel.CheckError> CheckErrors = new List<IncludeToOrderViewModel.CheckError>();
            int InstitutionID = InstitutionHelper.GetInstitutionID();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("ApplicationID", ApplicationID.Value));
                com.Parameters.Add(new SqlParameter("OrderID", OrderID.Value));
                com.Parameters.Add(new SqlParameter("AppCGItemID", SqlDbType.Int) { Value = AppCGItemID ?? (object)DBNull.Value });
                com.Parameters.Add(new SqlParameter("IdLevelBudget", SqlDbType.Int) { Value = IdLevelBudget ?? (object)DBNull.Value });
                com.Parameters.Add(new SqlParameter("InstitutionID", InstitutionID));
                com.Parameters.Add(new SqlParameter("BenefitID", SqlDbType.Int) { Value = BenefitID ?? (object)DBNull.Value });
                com.CommandTimeout = 120;	// Даем 120 секунд на исполнение.
                try
                {
                    con.Open();
                    var r = com.ExecuteReader();
                    while (r.Read())
                    {
                        CheckErrors.Add(new IncludeToOrderViewModel.CheckError() { ID = (int)r["ID"], Msg = (string)r["Msg"] });
                    }
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
            return CheckErrors;
        }

        public static List<IncludeToOrderViewModel.CheckError> IncludeAppToOrder(int? ApplicationID, int? OrderID, int? AppCGItemID, int? IdLevelBudget, int? BenefitID)
        {
            string sql = "EXEC [dbo].IncludeAppicationToOrder @ApplicationID=@ApplicationID, @OrderID=@OrderID, @InstitutionID=@InstitutionID, @IdLevelBudget=@IdLevelBudget, @AppCGItemId=@AppCGItemId, @BenefitID=@BenefitID";

            List<IncludeToOrderViewModel.CheckError> CheckErrors = new List<IncludeToOrderViewModel.CheckError>();
            int InstitutionID = InstitutionHelper.GetInstitutionID();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("ApplicationID", ApplicationID.Value));
                com.Parameters.Add(new SqlParameter("OrderID", OrderID.Value));
                com.Parameters.Add(new SqlParameter("AppCGItemID", SqlDbType.Int) { Value = AppCGItemID ?? (object)DBNull.Value });
                com.Parameters.Add(new SqlParameter("IdLevelBudget", SqlDbType.Int) { Value = IdLevelBudget ?? (object)DBNull.Value });
                com.Parameters.Add(new SqlParameter("InstitutionID", InstitutionID));
                com.Parameters.Add(new SqlParameter("BenefitID", SqlDbType.Int) { Value = BenefitID ?? (object)DBNull.Value });
                com.CommandTimeout = 300;	// Даем 300 секунд на исполнение.
                try
                {
                    con.Open();
                    var r = com.ExecuteReader();
                    while (r.Read())
                    {
                        CheckErrors.Add(new IncludeToOrderViewModel.CheckError() { ID = (int)r["ID"], Msg = (string)r["Msg"] });
                    }
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
            return CheckErrors;
        }

        public static ConditionForIncudeToOrder GetConditionForIncudeToOrder(int[] applicationIds)
        {
            if(applicationIds==null)
                throw new ArgumentNullException("applicationIds");
            if(!applicationIds.Any())
                throw new ArgumentException("applicationIds");

            #region SQL

            string sql = @"
DECLARE @CompetitiveGroupID	INT
DECLARE @EducationFormID INT
DECLARE @EducationSourceID INT
DECLARE @CampaignID	INT 
DECLARE @EducationLevelID INT
DECLARE @IsForBeneficiary INT
DECLARE @IsForeigner INT

DECLARE @Apps TABLE (ApplicationID int)
INSERT INTO @Apps
{0}
 

DECLARE @Cond TABLE 
    (
    ApplicationID INT,
    ApplicationCompetitiveGroupItemID INT,
    CompetitiveGroupID INT, 
    EducationFormId INT, 
    EducationSourceId INT, 
    CampaignID INT,  
    EducationLevelID INT,
    IsAgreed BIT,
    BenefitIDs NVARCHAR(1000)
    )

INSERT INTO @Cond 
SELECT 
    a.ApplicationID,
    acgi.id,
    cg.CompetitiveGroupID, 
    cg.EducationFormId, 
    cg.EducationSourceId, 
    cg.CampaignID,  
    cg.EducationLevelID,
    CAST(CASE WHEN 
        (c.CampaignTypeID IN (1,2) AND acgi.IsAgreed = 1 AND ISNULL(acgi.IsDisagreed,0) = 0)
        OR (c.CampaignTypeID NOT IN (1,2))
        THEN 1
        ELSE 0
    END AS BIT),
    BenefitIDs
FROM 
    [Application] AS a (NOLOCK)
	INNER JOIN ApplicationCompetitiveGroupItem AS acgi (NOLOCK) ON acgi.ApplicationId = a.ApplicationID
 	INNER JOIN CompetitiveGroup AS cg (NOLOCK) ON cg.CompetitiveGroupID = acgi.CompetitiveGroupID
    INNER JOIN Campaign c ON c.CampaignID = cg.CampaignID
	INNER JOIN @Apps aa ON aa.ApplicationID = a.ApplicationID	    
    OUTER APPLY
		(SELECT 
            CAST(aetd.BenefitID as nvarchar(10))
            + CASE WHEN aetd.EntranceTestItemID IS NULL
                THEN ''
                ELSE ' for '+ISNULL(CAST(aetd.CompetitiveGroupID as nvarchar(10)),'')
                END
            +';' 
		FROM 
            ApplicationEntranceTestDocument aetd (NOLOCK)
		WHERE 
            aetd.ApplicationId = a.ApplicationId 
            AND aetd.BenefitID IS NOT NULL
        ORDER BY
            aetd.CompetitiveGroupID,
            aetd.BenefitID
        FOR XML PATH('')) as BenefitIDs(BenefitIDs)
SELECT 
    ApplicationID,
    ApplicationCompetitiveGroupItemID,
    CompetitiveGroupID,  
    EducationFormId, 
    EducationSourceId, 
    CampaignID,  
    EducationLevelID,
    IsAgreed,
    BenefitIDs
FROM 
    @Cond   
";
            #endregion

            #region SQL Exec

            StringBuilder sApplicationIds = new StringBuilder();
            for (int i = 0; i < applicationIds.Length; i++)
            {
                if (i == applicationIds.Length - 1)
                {
                    sApplicationIds.AppendFormat("SELECT {0} ", applicationIds[i]);
                }
                else
                {
                    sApplicationIds.AppendFormat("SELECT {0} UNION ", applicationIds[i]);
                }
            }
            sql = String.Format(sql, sApplicationIds.ToString());
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);

                if (applicationIds.Length == 1)
                {
                    com.Parameters.Add(new SqlParameter("ApplicationID", applicationIds[0]));
                }
                else
                {
                    com.Parameters.Add(new SqlParameter("ApplicationID", DBNull.Value));
                }
                SqlDataAdapter da = new SqlDataAdapter(com);
                try
                {
                    con.Open();
                    da.Fill(ds);
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
                    if (con.State != ConnectionState.Closed)
                    {
                        con.Close();
                    }
                }
            }

            #endregion

            #region ConditionForIncudeToOrder

            ConditionForIncudeToOrder conditions = new ConditionForIncudeToOrder();
            conditions.Conditions = Conditions(ds.Tables[0]);

            CheckConditions(applicationIds, conditions);
             
            #endregion

            return conditions;
        }

        private static void CheckConditions(IEnumerable<int> applicationIds, ConditionForIncudeToOrder conditions)
        {
            conditions.ErrorId = ConditionForIncudeToOrder.ErrorTypes.None;

            foreach (int applicationId in applicationIds)
            {
                IEnumerable<ConditionForIncudeToOrder.Condition> applicationConditions = conditions.Conditions.Where(x => x.ApplicationId == applicationId);
                if (!applicationConditions.Any())
                {
                    conditions.ErrorId = ConditionForIncudeToOrder.ErrorTypes.NoApplicationItems;
                    break;
                }
                if (!applicationConditions.Any(x => x.IsAgreed))
                {
                    conditions.ErrorId = ConditionForIncudeToOrder.ErrorTypes.NoAgreement;
                    break;
                }
            }
            if (conditions.ErrorId != ConditionForIncudeToOrder.ErrorTypes.None)
                return;

            conditions.Conditions = conditions.Conditions.Where(x => x.IsAgreed).ToList();//

            if (applicationIds.Count() > 1)//Пакетное включение в приказ
            {
                // Условие избыточно, ибо ниже проверяется то же самое на CompetitiveGroupId
                //if (conditions.Conditions.Select(x => x.CampaignID.Value).Distinct().Count() != 1)
                //{
                //    conditions.ErrorId = ConditionForIncudeToOrder.ErrorTypes.PackageError_Other;
                //    return;
                //}
                if (conditions.Conditions.Select(x => x.CompetitiveGroupID.Value).Distinct().Count() != 1)
                {
                    conditions.ErrorId = ConditionForIncudeToOrder.ErrorTypes.PackageError_Other;
                    return;
                } 

                if (conditions.Conditions.Select(x => x.BenefitIDs).Distinct().Count() != 1)
                {
                    conditions.ErrorId = ConditionForIncudeToOrder.ErrorTypes.PackageError_DistinctBenefits;
                    return;
                }

                //Тут же можно проверить разные национальности
            } 
        }

        private static List<ConditionForIncudeToOrder.Condition> Conditions(DataTable data)
        {
            List<ConditionForIncudeToOrder.Condition> ccList = new List<ConditionForIncudeToOrder.Condition>();
            for (int i = 0; i < data.Rows.Count; i++)
            {
                var c = new ConditionForIncudeToOrder.Condition();
                DataRow r = data.Rows[i];
                c.ApplicationId = (int)r["ApplicationId"];
                c.ApplicationCompetitiveGroupItemId = (int)r["ApplicationCompetitiveGroupItemId"] ;
                c.CompetitiveGroupID = r["CompetitiveGroupID"] as int?;
                c.CompetitiveGroupID = r["CompetitiveGroupID"] as int?;
                c.EducationFormID = r["EducationFormID"] as int?;
                c.EducationSourceID = r["EducationSourceID"] as int?;
                c.CampaignID = r["CampaignID"] as int?;
                c.EducationLevelID = r["EducationLevelID"] as int?;
                c.IsAgreed = (bool)r["IsAgreed"];
                c.BenefitIDs = r["BenefitIDs"] as string;
                ccList.Add(c);
            }
            return ccList;
        } 
    }
}
