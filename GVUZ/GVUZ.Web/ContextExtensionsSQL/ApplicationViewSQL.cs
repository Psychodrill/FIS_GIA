using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using GVUZ.Web.ViewModels;
using System.Data.SqlClient;
using System.Data;

namespace GVUZ.Web.ContextExtensionsSQL
{
    public class ApplicationViewSQL
    {
        #region ConnectionString Main
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
        #endregion

        #region GetApplicationV0
        public static ApplicationV0Model GetApplicationV0(int ApplicationId)
        {
            #region SQL
            string sql =
@"
--DECLARE @ApplicationID INT =20514997

SELECT
    app.ApplicationID
    ,ast.Name AS StatusName      
    ,app.ViolationErrors AS ViolationName
    ,i.FullName AS InstitutionName
    			
FROM [dbo].[Application] app (NOLOCK)
    INNER JOIN [dbo].[ApplicationStatusType] ast (NOLOCK) ON ast.StatusID = app.StatusID
    INNER JOIN [dbo].[ViolationType] vt (NOLOCK) ON vt.ViolationID = app.ViolationID
    INNER JOIN [dbo].[Institution] i (NOLOCK) ON i.InstitutionID = app.InstitutionID
WHERE app.ApplicationID=@ApplicationID

SELECT CGs2.CompetitiveGroupId, CGs2.CompetitiveGroupName, CGs2.Places
,(
	(select SUM(ISNULL(mres,0))  from (
		select etic.EntranceTestPriority, MAX( aetd.ResultValue ) as mres
			FROM ApplicationEntranceTestDocument aetd (NOLOCK)  
            INNER JOIN EntranceTestItemC AS etic (NOLOCK) ON etic.EntranceTestItemID = aetd.EntranceTestItemID
		WHERE aetd.ApplicationID=@ApplicationID AND etic.CompetitiveGroupID=CGs2.CompetitiveGroupID 
			and not exists (Select * FROM ApplicationEntranceTestDocument aetd2 (NOLOCK)  
							INNER JOIN EntranceTestItemC AS etic2 (NOLOCK) ON etic2.EntranceTestItemID = aetd2.EntranceTestItemID
							Where aetd2.ApplicationID=@ApplicationID AND etic2.CompetitiveGroupID=CGs2.CompetitiveGroupID 
							and etic2.ReplacedEntranceTestItemID = etic.EntranceTestItemID
						)
						group by etic.EntranceTestPriority 
	 ) as maxprior )
	 + (CASE WHEN ISNULL((SELECT SUM(ISNULL(ia.IAMark,0)) FROM IndividualAchivement ia
			WHERE ia.ApplicationID=@ApplicationID),0) > 10  
			and (select distinct (dcg.EducationLevelID) from dbo.ApplicationCompetitiveGroupItem  as acgitem (NOLOCK) 
					inner join dbo.CompetitiveGroup  as dcg (NOLOCK)  on acgitem.CompetitiveGroupId = dcg.CompetitiveGroupID
					where acgitem.ApplicationId = @ApplicationID
			) <> 18
	THEN 10 ELSE  ISNULL((SELECT SUM(ISNULL(ia.IAMark,0)) FROM IndividualAchivement ia WHERE ia.ApplicationID=@ApplicationID),0) END) 
) AS Points,
(SELECT COUNT(DISTINCT acgi.ApplicationID)  FROM [ApplicationCompetitiveGroupItem] acgi (NOLOCK) 
    WHERE acgi.CompetitiveGroupId = CGs2.CompetitiveGroupId) AS Requests
FROM 
(
	SELECT cg.CompetitiveGroupId,
		cg.Name AS CompetitiveGroupName,
		(
			SUM(ISNULL(CGI.NumberBudgetO,0)) + SUM(ISNULL(CGI.NumberBudgetOZ,0)) + SUM(ISNULL(CGI.NumberBudgetZ,0))+ 
			SUM(ISNULL(CGI.NumberPaidO,0)) + SUM(ISNULL(CGI.NumberPaidOZ,0)) + SUM(ISNULL(CGI.NumberPaidZ,0))+ 
			SUM(ISNULL(CGI.NumberQuotaO,0)) + SUM(ISNULL(CGI.NumberQuotaOZ,0)) + SUM(ISNULL(CGI.NumberQuotaZ,0))+
			ISNULL((SELECT SUM(ISNULL(cgti.NumberTargetO,0)) + SUM(ISNULL(cgti.NumberTargetOZ,0)) + SUM(ISNULL(cgti.NumberTargetZ,0)) 
			FROM CompetitiveGroupTarget AS cgt (NOLOCK)
				INNER JOIN CompetitiveGroupTargetItem AS cgti (NOLOCK) ON cgti.CompetitiveGroupTargetID = cgt.CompetitiveGroupTargetID
				inner join CompetitiveGroup cg (NOLOCK) ON cg.CompetitiveGroupID = cgti.CompetitiveGroupID
				left join CompetitiveGroupItem AS cgi (NOLOCK) ON cgi.CompetitiveGroupID = cg.CompetitiveGroupID
			WHERE cg.CompetitiveGroupID IN
			(
				SELECT acgi.CompetitiveGroupId
				FROM ApplicationCompetitiveGroupItem AS acgi (NOLOCK)
				WHERE acgi.ApplicationId=@ApplicationID
			)),0)	        
		)
		AS Places
	FROM CompetitiveGroupItem AS CGI (NOLOCK)
		INNER JOIN CompetitiveGroup AS cg (NOLOCK) ON CGI.CompetitiveGroupID = cg.CompetitiveGroupID
		INNER JOIN
		(
  			SELECT distinct CompetitiveGroupId
  			FROM [dbo].[ApplicationCompetitiveGroupItem] (NOLOCK)
  			WHERE ApplicationID=@ApplicationID
		) AS CGs ON CG.CompetitiveGroupId=CGs.CompetitiveGroupId
GROUP BY cg.CompetitiveGroupId, cg.Name
) AS CGs2


SELECT DISTINCT cg.Course AS Course
    ,COALESCE( d.NewCode + ' ' + d.Name + ' ', pd.Code + ' ' + pd.Name) AS DirectionName
    ,ait.Name AS EduLevelName
FROM [dbo].[ApplicationCompetitiveGroupItem] acgi (NOLOCK)
    INNER JOIN [dbo].[CompetitiveGroup] cg (NOLOCK) ON cg.CompetitiveGroupID = acgi.CompetitiveGroupId
    LEFT JOIN [dbo].[Direction] d (NOLOCK) ON d.DirectionID = cg.DirectionID
    LEFT JOIN [dbo].[ParentDirection] pd (NOLOCK) ON pd.ParentDirectionID = cg.ParentDirectionID
    INNER JOIN [dbo].[AdmissionItemType] ait (NOLOCK) ON ait.ItemTypeID = cg.EducationLevelID
WHERE acgi.ApplicationId = @ApplicationID
";

            #endregion

            ApplicationV0Model app = null;
            DataSet ds = null;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("ApplicationID", ApplicationId));
                ds = new DataSet();

                try
                {
                    con.Open();
                    SqlDataAdapter sqlAdapter = new SqlDataAdapter(com);
                    sqlAdapter.Fill(ds);
                    app = new ApplicationV0Model();

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        app.StatusName = ds.Tables[0].Rows[i]["StatusName"] as string;
                        app.ViolationName = ds.Tables[0].Rows[i]["ViolationName"] as string;
                        app.InstitutionName = ds.Tables[0].Rows[i]["InstitutionName"] as string;
                    }
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        app.CompetitiveGroup.Add(new ApplicationV0Model.CompetitiveGroupInfo()
                        {
                            CompetitiveGroupID = Convert.ToInt32(ds.Tables[1].Rows[i]["CompetitiveGroupID"]),
                            CompetitiveGroupName = ds.Tables[1].Rows[i]["CompetitiveGroupName"] as string,
                            Places = ds.Tables[1].Rows[i]["Places"] as Int32?,
                            Points = ds.Tables[1].Rows[i]["Points"] as decimal?,
                            Requests = ds.Tables[1].Rows[i]["Requests"] as Int32?
                        });
                    }
                    for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                    {
                        app.listESC.Add(new ApplicationV0Model.listGI()
                        {
                            EduLevelName = ds.Tables[2].Rows[i]["EduLevelName"] as string,
                            DirectionName = ds.Tables[2].Rows[i]["DirectionName"] as string,
                            Course = Convert.ToInt32(ds.Tables[2].Rows[i]["Course"])
                        });
                    }
                    app.Priorities = new ApplicationPrioritiesViewModel();
                    app.Priorities.ApplicationPriorities = SQL.GetApplicationPriorities(ApplicationId);

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
            return app;
        }

        #endregion

        #region GetApplicationV1

        public static ApplicationV1Model GetApplicationV1(int ApplicationId)
        {
            #region SQL

            string sql = @"SELECT app.ApplicationID
								,ent.LastName AS EntrantLastName
								,ent.FirstName AS EntrantFirstName
								,ent.MiddleName AS EntrantMiddleName
                                ,ent.EntrantID
								,dt.Name AS DocumentTypeName
								,edi.BirthDate AS DocumentBirthDate
								,ed.DocumentSeries + ' ' + ed.DocumentNumber AS DocumentSeriaNumber
								,ed.DocumentOrganization AS DocumentOrganization
								,ed.DocumentDate AS DocumentDate
								,ed.AttachmentID AS AttachmentID
								,att.Name as AttachmentName
								,att.FileID as AttachmentFileID
                                ,att.Body AS AttachmentFileBody
								,(SELECT gt.Name FROM [dbo].[GenderType] gt WHERE gt.GenderID = edi.GenderTypeID) AS GenderName
								,(SELECT ct.Name FROM [dbo].[CountryType] ct WHERE ct.CountryID = edi.NationalityTypeID) AS CountryName
								
								,edi.BirthPlace AS BirthPlace
								,ent.CustomInformation AS CustomInformation
								,app.NeedHostel AS NeedHostel
								
								
							FROM  [dbo].[Application] app   (NOLOCK)
							INNER JOIN Entrant ent (NOLOCK)
							ON ent.EntrantID =app.EntrantID
							INNER JOIN EntrantDocument ed (NOLOCK)
							ON ed.EntrantDocumentID= ent.IdentityDocumentID 
							INNER JOIN EntrantDocumentIdentity edi (NOLOCK)
							ON edi.EntrantDocumentID=ed.EntrantDocumentID
							LEFT OUTER JOIN Attachment att (NOLOCK)
							ON att.AttachmentID=ed.AttachmentID
							INNER JOIN IdentityDocumentType dt (NOLOCK)
							ON dt.IdentityDocumentTypeID=edi.IdentityDocumentTypeID
            WHERE app.ApplicationID=@ApplicationID";

            #endregion

            ApplicationV1Model app = null;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("ApplicationID", ApplicationId));

                try
                {
                    con.Open();
                    SqlDataReader r = com.ExecuteReader();
                    if (r.Read())
                    {
                        app = new ApplicationV1Model();

                        app.ApplicationID = r["ApplicationID"] as Int32?;
                        app.EntrantID = Convert.ToInt32(r["EntrantID"] as Int32?);
                        app.EntrantLastName = r["EntrantLastName"] as string;
                        app.EntrantFirstName = r["EntrantFirstName"] as string;
                        app.EntrantMiddleName = r["EntrantMiddleName"] as string;
                        app.DocumentTypeName = r["DocumentTypeName"] as string;
                        app.DocumentBirthDate = r["DocumentBirthDate"] as DateTime?;
                        app.DocumentSeriaNumber = r["DocumentSeriaNumber"] as string;
                        app.DocumentOrganization = r["DocumentOrganization"] as string;
                        app.DocumentDate = r["DocumentDate"] as DateTime?;
                        app.AttachmentID = r["AttachmentID"] as int?;
                        app.AttachmentName = r["AttachmentName"] as string;
                        app.AttachmentFileID = r["AttachmentFileID"] as Guid?;
                        app.AttachmentFileBody = r["AttachmentFileBody"] as byte[];
                        app.GenderName = r["GenderName"] as string;
                        app.CountryName = r["CountryName"] as string;
                        app.BirthPlace = r["BirthPlace"] as string;
                        app.CustomInformation = r["CustomInformation"] as string;
                        app.NeedHostel = Convert.ToBoolean(r["NeedHostel"] as bool?);

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
            return app;
        }
        #endregion

        #region GetApplicationV2

        public static ApplicationV2Model GetApplicationV2(int ApplicationId)
        {
            #region SQL

            string sql = @"SELECT
    aed.ApplicationID
	,dt.Name AS DocumentTypeName
    ,ed.EntrantDocumentID AS EntrantDocumentID
	,ISNULL(ed.DocumentSeries, '') + ' ' + ISNULL(ed.DocumentNumber,'') AS DocumentSeriesNumber
	,ed.DocumentDate AS DocumentDate
	,ed.DocumentOrganization AS DocumentOrganization
	,ed.AttachmentID AS DocumentAttachmentID
	,att.Name as DocumentAttachmentName
	,att.FileID as AttachmentFileID
	,aed.OriginalReceivedDate AS OriginalReceivedDate
    ,(SELECT a.StatusID FROM [Application] AS a (NOLOCK) WHERE a.ApplicationID= aed.ApplicationID) AS StatusID
    ,ed.DocumentTypeID
    ,ed.OlympApproved
    FROM ApplicationEntrantDocument aed (NOLOCK)
    INNER JOIN EntrantDocument ed (NOLOCK)
    ON ed.EntrantDocumentID = aed.EntrantDocumentID
    INNER JOIN DocumentType dt (NOLOCK)
    ON dt.DocumentID = ed.DocumentTypeID
    LEFT OUTER JOIN Attachment att (NOLOCK)
    ON att.AttachmentID=ed.AttachmentID
    WHERE aed.ApplicationID = @ApplicationID";

            #endregion

            ApplicationV2Model app = null;
            DataSet ds = null;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("ApplicationID", ApplicationId));
                ds = new DataSet();

                try
                {
                    con.Open();
                    SqlDataAdapter sqlAdapter = new SqlDataAdapter(com);
                    sqlAdapter.Fill(ds);
                    app = new ApplicationV2Model();
                    app.ApplicationID = ApplicationId;

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        ApplicationV2Model.ApplicationV2 item = new ApplicationV2Model.ApplicationV2()
                        {
                            EntrantDocumentID = ds.Tables[0].Rows[i]["EntrantDocumentID"] as int?,
                            DocumentTypeName = ds.Tables[0].Rows[i]["DocumentTypeName"] as string,
                            DocumentSeriesNumber = ds.Tables[0].Rows[i]["DocumentSeriesNumber"] as string,
                            DocumentDate = (ds.Tables[0].Rows[i]["DocumentDate"] as DateTime?).HasValue ? (ds.Tables[0].Rows[i]["DocumentDate"] as DateTime?).Value.ToString("dd.MM.yyyy") : "",
                            DocumentOrganization = ds.Tables[0].Rows[i]["DocumentOrganization"] as string,
                            DocumentAttachmentName = ds.Tables[0].Rows[i]["DocumentAttachmentName"] as string,
                            DocumentAttachmentID = ds.Tables[0].Rows[i]["AttachmentFileID"] as Guid?,
                            OriginalReceivedDate = (ds.Tables[0].Rows[i]["OriginalReceivedDate"] as DateTime?).HasValue ? (ds.Tables[0].Rows[i]["OriginalReceivedDate"] as DateTime?).Value.ToString("dd.MM.yyyy") : "",
                            OriginalReceived = (ds.Tables[0].Rows[i]["OriginalReceivedDate"] as DateTime?).HasValue ? true : false,
                            StatusID = ds.Tables[0].Rows[i]["StatusID"] as int?
                        };

                        int? documentTypeID = ds.Tables[0].Rows[i]["DocumentTypeID"] as int?;
                        bool olympApproved = (ds.Tables[0].Rows[i]["OlympApproved"] as bool?).GetValueOrDefault();
                        if (documentTypeID == 9 || documentTypeID == 10)
                        {
                            item.DocumentTypeName += olympApproved ? " (результаты подтверждены)" : " (результаты не подтверждены)";
                        }

                        app.AttachedDocuments.Add(item);
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
            return app;
        }
        #endregion

        #region GetApplicationV3

        public static ApplicationV3Model GetApplicationV3(int ApplicationId)
        {
            #region SQL

            string sql = @"

SELECT ApplicationID, CompetitiveGroupID, CompetitiveGroupName, Course, EducationLevelID
,(SELECT COUNT(ApplicationCompetitiveGroupItem.Id) FROM ApplicationCompetitiveGroupItem (NOLOCK) WHERE CompetitiveGroupID=AC.CompetitiveGroupID AND ApplicationId = @ApplicationID AND EducationSourceId = 20) AS IsQuotaBenefitEnabled
,(SELECT COUNT(BenefitID) FROM [BenefitItemC] (NOLOCK) WHERE [CompetitiveGroupID]=AC.CompetitiveGroupID) AS HasBenefits
FROM (
SELECT acgi.ApplicationId, cg.CompetitiveGroupID, cg.Name AS CompetitiveGroupName, cg.Course, cg.EducationLevelID
FROM ApplicationCompetitiveGroupItem AS acgi (NOLOCK)
	INNER JOIN CompetitiveGroup AS cg (NOLOCK) ON cg.CompetitiveGroupID = acgi.CompetitiveGroupId
	--INNER JOIN CompetitiveGroupItem AS cgi (NOLOCK) ON cgi.CompetitiveGroupID = cg.CompetitiveGroupID
WHERE acgi.ApplicationId=@ApplicationID
GROUP BY acgi.ApplicationId, cg.CompetitiveGroupID, cg.Name, cg.Course, cg.EducationLevelID) AS AC

--SELECT DISTINCT
--ETIC.CompetitiveGroupID
--,CG.Name AS CompetitiveGroupName
--FROM [dbo].[EntranceTestItemC] ETIC
--INNER JOIN ApplicationCompetitiveGroupItem ACGI ON ETIC.CompetitiveGroupID = ACGI.CompetitiveGroupId
--INNER JOIN CompetitiveGroup CG ON CG.CompetitiveGroupID = ETIC.CompetitiveGroupID
--WHERE (ACGI.ApplicationId = @ApplicationID)

SELECT
	AETD.CompetitiveGroupID AS CompetitiveGroupID
	,aetd.BenefitID AS BenefitID
	,b.Name AS BenefitName
	,dt.Name AS DocumentName
	,ed.DocumentNumber AS DocumentNumber
	,ed.DocumentSeries AS DocumentSeries
	,ed.DocumentDate AS DocumentDate
FROM ApplicationEntranceTestDocument AETD (NOLOCK)
	INNER JOIN EntrantDocument ED (NOLOCK) ON AETD.EntrantDocumentID = ED.EntrantDocumentID
	INNER JOIN DocumentType DT (NOLOCK) ON ED.DocumentTypeID = DT.DocumentID
	INNER JOIN Benefit B (NOLOCK) ON AETD.BenefitID = B.BenefitID
WHERE AETD.ApplicationId = @ApplicationID and AETD.BenefitID is not null and AETD.EntranceTestItemID is null

SELECT CGs.ApplicationID, CGs.IsForSPOandVO AS ACGI_IsForSPOandVO, CGs.CampaignTypeID, CGs.CompetitiveGroupID, CGs.CompetitiveName, CGs.Course, CGs.EducationLevelID
, ETIC.SubjectID
, ETIC.SubjectName 
, ISNULL(ETIC_Subject.Name, ETIC.SubjectName) as SubjectNameView
, ETIC_Subject.IsEge as SubjectIsEge
, ETIC.EntranceTestPriority as TestPriority
, ETIC.EntranceTestTypeID 
, ETIC.EntranceTestItemID 
, ETIC.IsForSPOandVO as ETIC_IsForSPOandVO
, ETIC.ReplacedEntranceTestItemID

, AETD.ID as AETD_ID -- Для проверки наличия таковой
, AETD.CompetitiveGroupID as AETD_CompetitiveGroupID
, AETD.BenefitID as AETD_BenefitID
, AETD_Benefit.Name as AETD_BenefitName
, AETD.SourceID as AETD_SourceID
, AETD.SubjectID as AETD_SubjectID
, AETD_Subject.Name as AETD_SubjectName
, AETD.EntranceTestItemID as AETD_EntranceTestItemID
, AETD.EntrantDocumentID as AETD_EntrantDocumentID
, AETD.ResultValue AS AETD_ResultValue

, AETD.InstitutionDocumentNumber as AETD_InstitutionDocumentNumber
, AETD.InstitutionDocumentDate  as AETD_InstitutionDocumentDate
, AETD.InstitutionDocumentTypeID as AETD_InstitutionDocumentTypeID
, AETD.HasEge as AETD_HasEge
, AETD.EgeResultValue as AETD_EgeResultValue
FROM 
(SELECT DISTINCT ACGI.ApplicationID, ACGI.IsForSPOandVO, C.CampaignTypeID, CG.CompetitiveGroupID, CG.Name as CompetitiveName, CG.Course, cg.EducationLevelID
FROM ApplicationCompetitiveGroupItem ACGI  (NOLOCK)
INNER JOIN CompetitiveGroup CG (NOLOCK) ON CG.CompetitiveGroupID = ACGI.CompetitiveGroupID
INNER JOIN Campaign C (NOLOCK) ON CG.CampaignID = C.CampaignID
--INNER JOIN CompetitiveGroupItem AS cgi ON cgi.CompetitiveGroupID = CG.CompetitiveGroupID
WHERE ACGI.ApplicationId = @ApplicationID
) CGs
INNER JOIN EntranceTestItemC ETIC (NOLOCK) ON ETIC.CompetitiveGroupID = CGs.CompetitiveGroupId
LEFT JOIN Subject ETIC_Subject (NOLOCK) ON ETIC_Subject.SubjectID = ETIC.SubjectID 
LEFT JOIN ApplicationEntranceTestDocument AETD (NOLOCK) ON AETD.ApplicationID=CGs.ApplicationID AND AETD.EntranceTestItemID=ETIC.EntranceTestItemID
LEFT JOIN Subject AETD_Subject (NOLOCK) ON AETD_Subject.SubjectID = AETD.SubjectID

LEFT JOIN Benefit AETD_Benefit (NOLOCK) on AETD_Benefit.BenefitID=AETD.BenefitID
ORDER BY ETIC.CompetitiveGroupID, ETIC.EntranceTestTypeID

SELECT AETD.ID, AETD.ApplicationID
	, AETD.CompetitiveGroupID
	, AETD.SourceID --
	, AETD.SubjectID --
	, AETD.BenefitID, b.Name as BenefitName
	, AETD.EntranceTestItemID
	, AETD.EntranceTestTypeID
	, AETD.EntrantDocumentID

    , ed.DocumentName as AETD_DocumentTypeName
	, ed.DocumentTypeID as AETD_DocumentTypeID
	, ed.DocumentSeries as AETD_DocumentSeries
	, ed.DocumentNumber as AETD_DocumentNumber
	, ed.DocumentDate	as AETD_DocumentDate
	, AETD.InstitutionDocumentTypeID as AETD_InstitutionDocumentTypeID
	, AETD_idt.Name as AETD_InstitutionDocumentTypeName
	, AETD.InstitutionDocumentDate  as AETD_InstitutionDocumentDate
	, AETD.InstitutionDocumentNumber as AETD_InstitutionDocumentNumber

	, AETD.ResultValue as AETD_ResultValue
	, AETD.HasEge as AETD_HasEge
	, AETD.EgeResultValue as AETD_EgeResultValue
    ,ed.OlympApproved
	,AETD.AppealStatusID
	,apps.StatusName AS AppealStatusName
    
FROM ApplicationEntranceTestDocument AETD  (NOLOCK)
LEFT OUTER JOIN Benefit b (NOLOCK) on b.BenefitID=AETD.BenefitID
LEFT OUTER JOIN EntrantDocument ed (NOLOCK) on ed.EntrantDocumentID=AETD.EntrantDocumentID
LEFT OUTER JOIN DocumentType dt (NOLOCK) on dt.DocumentID=ed.DocumentTypeID
LEFT OUTER JOIN InstitutionDocumentType AETD_idt (NOLOCK) on AETD_idt.InstitutionDocumentTypeID=AETD.InstitutionDocumentTypeID
LEFT OUTER JOIN AppealStatus apps (NOLOCK) ON apps.AppealStatusID = AETD.AppealStatusID
WHERE AETD.ApplicationID=@ApplicationID

";
            #endregion

            ApplicationV3Model app = null;
            DataSet ds = null;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("ApplicationID", ApplicationId));
                ds = new DataSet();
                try
                {
                    con.Open();
                    SqlDataAdapter sqlAdapter = new SqlDataAdapter(com);
                    sqlAdapter.Fill(ds);
                    app = new ApplicationV3Model();

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        app.ListCG.Add(new ApplicationV3Model.CompetitiveGroup()
                        {
                            CompetitiveGroupID = ds.Tables[0].Rows[i]["CompetitiveGroupID"] as int?,
                            CompetitiveGroupName = ds.Tables[0].Rows[i]["CompetitiveGroupName"] as string
                        });
                    }
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        app.listGeneralBenefit.Add(new ApplicationV3Model.GeneralBenefits()
                                {
                                    CompetitiveGroupID = ds.Tables[1].Rows[i]["CompetitiveGroupID"] as int?,
                                    BenefitId = ds.Tables[1].Rows[i]["BenefitId"] as int?,
                                    BenefitName = ds.Tables[1].Rows[i]["BenefitName"] as string,
                                    DocumentName = ds.Tables[1].Rows[i]["DocumentName"] as string,
                                    DocumentSeries = ds.Tables[1].Rows[i]["DocumentSeries"] as string,
                                    DocumentNumber = ds.Tables[1].Rows[i]["DocumentNumber"] as string,
                                    DocumentDate = ds.Tables[1].Rows[i]["DocumentDate"] as DateTime?
                                });
                    }

                    Dictionary<int, List<ApplicationV3Model.ApplicationV3>> allTestsByCompetitiveGroups = new Dictionary<int, List<ApplicationV3Model.ApplicationV3>>();
                    bool? campaignNeedsTestReplacements = null;
                    for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                    {
                        if (!campaignNeedsTestReplacements.HasValue)
                        {
                            campaignNeedsTestReplacements = ((ds.Tables[2].Rows[i]["CampaignTypeID"] as short?).GetValueOrDefault() == 1);
                        }

                        int competitiveGroupID = (ds.Tables[2].Rows[i]["CompetitiveGroupID"] as Int32?).GetValueOrDefault();

                        if (!allTestsByCompetitiveGroups.ContainsKey(competitiveGroupID))
                        {
                            allTestsByCompetitiveGroups.Add(competitiveGroupID, new List<ApplicationV3Model.ApplicationV3>());
                        }
                        allTestsByCompetitiveGroups[competitiveGroupID].Add(new ApplicationV3Model.ApplicationV3()
                        {
                            EntranceTestItemID = ds.Tables[2].Rows[i]["EntranceTestItemID"] as Int32?,
                            CompetitiveGroupID = ds.Tables[2].Rows[i]["CompetitiveGroupID"] as Int32?,
                            SubjectName = ds.Tables[2].Rows[i]["SubjectNameView"] as string,
                            Priority = ds.Tables[2].Rows[i]["TestPriority"] as Int32?,
                            ResultValue = ds.Tables[2].Rows[i]["AETD_ResultValue"] as decimal?,
                            EntranceTestTypeID = (short)ds.Tables[2].Rows[i]["EntranceTestTypeID"],
                            EgeResultValue = ds.Tables[2].Rows[i]["AETD_EgeResultValue"] as decimal?,
                            ApplicationIsForSPOandVO = ds.Tables[2].Rows[i]["ACGI_IsForSPOandVO"] as bool?,
                            EntranceTestIsForSPOandVO = ds.Tables[2].Rows[i]["ETIC_IsForSPOandVO"] as bool?,
                            ReplacedEntranceTestItemID = ds.Tables[2].Rows[i]["ReplacedEntranceTestItemID"] as int?,
                        });
                    }
                    if (!campaignNeedsTestReplacements.GetValueOrDefault())
                    {
                        foreach (List<ApplicationV3Model.ApplicationV3> tests in allTestsByCompetitiveGroups.Values)
                        {
                            app.ListTest.AddRange(tests);
                        }
                    }
                    else
                    {
                        foreach (int competitiveGroupID in allTestsByCompetitiveGroups.Keys)
                        {
                            bool applicationNeedsTestReplacements = allTestsByCompetitiveGroups[competitiveGroupID].First().ApplicationIsForSPOandVO.GetValueOrDefault();
                            if (!applicationNeedsTestReplacements)
                            {
                                app.ListTest.AddRange(allTestsByCompetitiveGroups[competitiveGroupID].Where(x => !x.EntranceTestIsForSPOandVO.GetValueOrDefault()));
                            }
                            else
                            {
                                List<ApplicationV3Model.ApplicationV3> tests = new List<ApplicationV3Model.ApplicationV3>();
                                foreach (ApplicationV3Model.ApplicationV3 test in allTestsByCompetitiveGroups[competitiveGroupID])
                                {
                                    if (test.EntranceTestIsForSPOandVO.GetValueOrDefault())
                                    {
                                        tests.Add(test);
                                    }
                                    else 
                                    {
                                        if (!allTestsByCompetitiveGroups[competitiveGroupID].Any(x => x.ReplacedEntranceTestItemID.GetValueOrDefault() == test.EntranceTestItemID))
                                        {
                                            tests.Add(test);
                                        }
                                    }
                                }
                                app.ListTest.AddRange(tests);
                            }
                        }
                    }

                    for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                    {
                        app.AttachedDocs.Add(new ApplicationV3Model.AttachedDocument()
                        {
                            EntranceTestItemID = ds.Tables[3].Rows[i]["EntranceTestItemID"] as Int32?,
                            DocumentName = ds.Tables[3].Rows[i]["AETD_DocumentTypeName"] as string,
                            DocumentSeries = ds.Tables[3].Rows[i]["AETD_DocumentSeries"] as string,
                            DocumentNumber = ds.Tables[3].Rows[i]["AETD_DocumentNumber"] as string,
                            DocumentDate = ds.Tables[3].Rows[i]["AETD_DocumentDate"] as DateTime?,
                            BenefitName = ds.Tables[3].Rows[i]["BenefitName"] as string,
                            SubjectID = ds.Tables[3].Rows[i]["SubjectID"] as Int32?,
                            InstitutionDocumentDate = ds.Tables[3].Rows[i]["AETD_InstitutionDocumentDate"] as DateTime?,
                            InstitutionDocumentNumber = ds.Tables[3].Rows[i]["AETD_InstitutionDocumentNumber"] as string,
                            InstitutionDocumentTypeID = ds.Tables[3].Rows[i]["AETD_InstitutionDocumentTypeID"] as Int32?,
                            InstitutionDocumentTypeName = ds.Tables[3].Rows[i]["AETD_InstitutionDocumentTypeName"] as string,
                            ResultValue = ds.Tables[3].Rows[i]["AETD_ResultValue"] as decimal?,
                            DocumentTypeID = ds.Tables[3].Rows[i]["AETD_DocumentTypeID"] as Int32?,
                            EntrantDocumentID = ds.Tables[3].Rows[i]["EntrantDocumentID"] as Int32?,
                            OlympApproved = ds.Tables[3].Rows[i]["OlympApproved"] as bool?,
                            AppealStatusID = Convert.ToInt32(ds.Tables[3].Rows[i]["AppealStatusID"] as int?),
                            AppealStatusName = (ds.Tables[3].Rows[i]["AppealStatusName"] as string)
                        });
                    }

                    #region Сочинения
                    app.CompositionResult = ApplicationSQL.GetComposition(ApplicationId);
                    #endregion

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
            return app;
        }
        //SqlDataReader r = com.ExecuteReader();

        //if (r.Read())
        //{
        //    app = new ApplicationV3Model();

        //    app.ApplicationID = r["ApplicationID"] as Int32?;

        //    app.ListTest.Add(new ApplicationV3Model.ApplicationV3() {
        //        SubjectName = r["SubjectName"] as string,
        //        CompetitiveGroupName = r["CompetitiveGroupName"] as string,
        //        Priority = r["Priority"] as int?,
        //        ResultValue = r["ResultValue"] as decimal?,
        //        EgeResultValue = r["EgeResultValue"] as decimal?,
        //        Source = r["Source"] as string
        //    });

        //}
        //r.Close();
        #endregion

        #region GetApplicationV4

        public static ApplicationV4Model GetApplicationV4(int ApplicationId)
        {
            #region SQL

            string sql = @"SELECT DISTINCT ia.ApplicationID AS ApplicationID
    ,ia.IAID AS IAID
    ,ia.IAUID AS IAUID
	,ia.IAName AS IAName
	,ia.IAMark AS IAMark
	,ia.EntrantDocumentID AS EntrantDocumentID
    ,ia.isAdvantageRight AS isAdvantageRight
	
    ,app.EntrantID AS EntrantID
	,ed.DocumentDate AS DocumentDate
	,ed.DocumentNumber AS DocumentNumber
	,ed.DocumentSeries AS DocumentSeries
	,ed.DocumentOrganization AS DocumentOrganization
	,ed.DocumentName AS DocumentTypeNameText
    ,app.IndividualAchivementsMark
FROM Application app (NOLOCK)
INNER JOIN IndividualAchivement ia (NOLOCK) ON ia.ApplicationID = app.ApplicationID
LEFT OUTER JOIN EntrantDocument ed (NOLOCK) ON ed.EntrantDocumentID = ia.EntrantDocumentID
WHERE ia.ApplicationID = @ApplicationID";

            #endregion

            ApplicationV4Model app = null;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("ApplicationID", ApplicationId));

                try
                {
                    con.Open();
                    SqlDataReader r = com.ExecuteReader();
                    app = new ApplicationV4Model();

                    while (r.Read())
                    {
                        app.ApplicationID = r["ApplicationID"] as Int32?;
                        app.IndividualAchivementsMark = (r["IndividualAchivementsMark"] as decimal?).GetValueOrDefault();

                        app.Items.Add(new ApplicationV4Model.ApplicationV4()
                        {
                            UID = r["IAUID"] as string,
                            IAName = r["IAName"] as string,
                            IAMark = r["IAMark"] as decimal?,
                            isAdvantageRight = r["isAdvantageRight"] as bool?,
                            IADocument = new ApplicationV4Model.ApplicationV4.ApplicationV4Document()
                            {
                                EntrantDocumentID = r["EntrantDocumentID"] as Int32?,
                                DocumentDate = r["EntrantDocumentID"] as DateTime?,
                                DocumentTypeNameText = r["DocumentTypeNameText"] as string,
                                DocumentSeries = r["DocumentSeries"] as string,
                                DocumentNumber = r["DocumentNumber"] as string,
                                DocumentOrganization = r["DocumentOrganization"] as string
                            }
                        });
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
            return app;
        }

        #endregion

        #region GetApplicationV5

        public static ApplicationV5Model GetApplicationV5(int ApplicationId)
        {
            #region SQL

            string sql = "";

            #endregion

            ApplicationV5Model app = null;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("ApplicationID", ApplicationId));

                try
                {
                    con.Open();
                    SqlDataReader r = com.ExecuteReader();
                    if (r.Read())
                    {
                        app = new ApplicationV5Model();

                        app.ApplicationID = r["ApplicationID"] as Int32?;

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
            return app;
        }
        #endregion
    }
}