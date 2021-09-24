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
using GVUZ.ServiceModel.Import.Bulk.Extensions;

namespace GVUZ.Web.ContextExtensionsSQL
{
    public static partial class ApplicationSQL
    {

        #region Wz3

        public static ApplicationWz3ViewModel GetApplicationWz3(int ApplicationId, bool isView)
        {
            #region SQL
            string sql = @" 
SELECT	
    app.ApplicationID, 
    app.WizardStepID, 
    e.EntrantID, 
    app.StatusID, 
    app.IsDisabled, 
    app.IsDistant, 
    app.IsDisabledDocumentID, 
    app.DistantPlace

FROM  
    [dbo].[Application] app   (NOLOCK)
	INNER JOIN Entrant e (NOLOCK) on app.EntrantID=e.EntrantID  
WHERE 
    app.ApplicationID = @ApplicationID

SELECT 
    ApplicationID, 
    CompetitiveGroupID, 
    CompetitiveGroupName, 
    Course, 
    EducationLevelID, 
        (SELECT 
            COUNT(ApplicationCompetitiveGroupItem.Id) 
        FROM 
            ApplicationCompetitiveGroupItem 
        WHERE CompetitiveGroupID = AC.CompetitiveGroupID 
            AND ApplicationId = @ApplicationID 
            AND EducationSourceId = 20) as IsQuotaBenefitEnabled,
        (SELECT 
            COUNT(BenefitID) 
        FROM 
            [BenefitItemC] 
        WHERE 
            [CompetitiveGroupID] = AC.CompetitiveGroupID) as HasBenefits
FROM(
    SELECT DISTINCT 
        ACGI.ApplicationID, 
        CG.CompetitiveGroupID, 
        CG.Name as CompetitiveGroupName, 
        CG.Course, 
        cg.EducationLevelID 
    FROM 
        ApplicationCompetitiveGroupItem ACGI  (NOLOCK)
        INNER JOIN CompetitiveGroup CG (NOLOCK) ON CG.CompetitiveGroupID = ACGI.CompetitiveGroupID
        INNER JOIN CompetitiveGroupItem AS cgi (NOLOCK) ON cgi.CompetitiveGroupID = CG.CompetitiveGroupID
    WHERE 
        ACGI.ApplicationId = @ApplicationID
) AC

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
FROM ApplicationCompetitiveGroupItem ACGI (NOLOCK) 
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

---------- DOCUMENTS ------------
SELECT 
    AETD.ID, 
    AETD.ApplicationID,
	AETD.CompetitiveGroupID,
	AETD.SourceID, 
	AETD.SubjectID, 
	AETD.BenefitID, 
    b.Name as BenefitName,
	AETD.EntranceTestItemID,
	AETD.EntranceTestTypeID,
	AETD.EntrantDocumentID,
		(CASE ed.DocumentTypeID 
			WHEN 9 THEN
				dt.Name+' «'+
				(SELECT TOP 1 _ot.Name FROM EntrantDocumentOlympic _edo (NOLOCK) INNER JOIN OlympicType _ot (NOLOCK) on _edo.OlympicID=_ot.OlympicID WHERE _edo.EntrantDocumentID=ed.EntrantDocumentID)
				+'»'
			ELSE
				dt.Name
		END) as AETD_DocumentTypeName,
	ed.DocumentTypeID as AETD_DocumentTypeID,
	ed.DocumentSeries as AETD_DocumentSeries,
	ed.DocumentNumber as AETD_DocumentNumber,
	ed.DocumentDate	as AETD_DocumentDate,
	AETD.InstitutionDocumentTypeID as AETD_InstitutionDocumentTypeID,
	AETD_idt.Name as AETD_InstitutionDocumentTypeName,
	AETD.InstitutionDocumentDate  as AETD_InstitutionDocumentDate,
	AETD.InstitutionDocumentNumber as AETD_InstitutionDocumentNumber,
	AETD.AppealStatusID,
	apps.StatusName AS AppealStatusName,

	AETD.ResultValue as AETD_ResultValue,
	AETD.HasEge as AETD_HasEge,
	AETD.EgeResultValue as AETD_EgeResultValue,
    ed.OlympApproved,
    (CASE 
        WHEN ed.DocumentTypeID=2 OR ed.DocumentTypeID=17 THEN
	        (SELECT TOP 1
                SubjectID 
            FROM   
                EntrantDocumentEgeAndOlympicSubject EDEAOS  
            WHERE 
                EDEAOS.EntrantDocumentID=ed.EntrantDocumentID 
                AND ETIC.SubjectID=SubjectID) 
	    ELSE 
            NULL 
    END
    ) as EGE_SubjectID
FROM 
    ApplicationEntranceTestDocument AETD  (NOLOCK)
    LEFT OUTER JOIN Benefit b (NOLOCK) ON b.BenefitID=AETD.BenefitID
    LEFT OUTER JOIN EntrantDocument ed (NOLOCK) ON ed.EntrantDocumentID=AETD.EntrantDocumentID
    LEFT OUTER JOIN DocumentType dt (NOLOCK) ON dt.DocumentID=ed.DocumentTypeID
    LEFT OUTER JOIN InstitutionDocumentType AETD_idt (NOLOCK) ON AETD_idt.InstitutionDocumentTypeID=AETD.InstitutionDocumentTypeID
    LEFT OUTER JOIN AppealStatus apps (NOLOCK) ON apps.AppealStatusID = AETD.AppealStatusID
    LEFT OUTER JOIN EntranceTestItemC as ETIC (NOLOCK) ON ETIC.EntranceTestItemID=AETD.EntranceTestItemID
WHERE 
    AETD.ApplicationID=@ApplicationID
";
            #endregion

            ApplicationWz3ViewModel appw2 = null;
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("ApplicationID", ApplicationId));
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
            #region Разбираем результат.
            appw2 = new ApplicationWz3ViewModel();
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    appw2.ApplicationID = (int)ds.Tables[0].Rows[0][0];
                    appw2.WizardStepID = (int)ds.Tables[0].Rows[0][1];
                    appw2.EntrantID = (int)ds.Tables[0].Rows[0][2];
                    appw2.StatusID = (int)ds.Tables[0].Rows[0][3];
                    appw2.IsDisabled = ds.Tables[0].Rows[0][4] == null ? false : (bool)ds.Tables[0].Rows[0][4];
                    appw2.IsDistant = ds.Tables[0].Rows[0][5] == null ? false : (bool)ds.Tables[0].Rows[0][5];
                    appw2.IsDisabledDocumentID = ds.Tables[0].Rows[0][6] as int?;
                    appw2.DistantPlace = ds.Tables[0].Rows[0][7] as string;
                }
            }
            #endregion
            #region ДОКУМЕНТЫ
            if (ds.Tables.Count > 3)
            {   // ДОКУМЕНТЫ
                for (int r = 0; r < ds.Tables[3].Rows.Count; r++)
                {
                    EntrantTestItemDocument testDocument = new EntrantTestItemDocument();
                    var row = ds.Tables[3].Rows[r];
                    testDocument.ID = row["ID"] as int?;
                    testDocument.ApplicationID = row["ApplicationID"] as int?;
                    testDocument.CompetitiveGroupID = row["CompetitiveGroupID"] as int?;
                    testDocument.SourceID = row["SourceID"] as int?;
                    testDocument.SubjectID = row["SubjectID"] as int?;
                    testDocument.BenefitID = row["BenefitID"] as short?;
                    testDocument.BenefitName = row["BenefitName"] as string;
                    testDocument.EntranceTestItemID = row["EntranceTestItemID"] as int?;
                    testDocument.EntrantDocumentID = row["EntrantDocumentID"] as int?;
                    testDocument.DocumentTypeID = row["AETD_DocumentTypeID"] as int?;
                    testDocument.DocumentTypeName = row["AETD_DocumentTypeName"] as string;
                    testDocument.DocumentSeries = row["AETD_DocumentSeries"] as string;
                    testDocument.DocumentNumber = row["AETD_DocumentNumber"] as string;
                    testDocument.DocumentDate = row["AETD_DocumentDate"] as DateTime?;
                    testDocument.InstitutionDocumentTypeID = row["AETD_InstitutionDocumentTypeID"] as int?;
                    testDocument.InstitutionDocumentTypeName = row["AETD_InstitutionDocumentTypeName"] as string;
                    testDocument.InstitutionDocumentDate = row["AETD_InstitutionDocumentDate"] as DateTime?;
                    testDocument.InstitutionDocumentNumber = row["AETD_InstitutionDocumentNumber"] as string;
                    testDocument.ResultValue = row["AETD_ResultValue"] as decimal?;
                    testDocument.HasEge = row["AETD_HasEge"] as bool?;
                    testDocument.EgeResultValue = row["AETD_EgeResultValue"] as decimal?;
                    testDocument.OlympApproved = Convert.ToInt32(row["OlympApproved"] as bool?);
                    testDocument.AppealStatusID = Convert.ToInt32(row["AppealStatusID"] as int?);
                    testDocument.AppealStatusName = (row["AppealStatusName"] as string);
                    testDocument.EGE_SubjectID = row["EGE_SubjectID"] as int?;
                    appw2.Documents.Add(testDocument);
                }
            }
            #endregion


            #region  AppCompetitiveGroup

            if (ds.Tables.Count > 1)
            {
                for (int r = 0; r < ds.Tables[1].Rows.Count; r++)
                {
                    var row = ds.Tables[1].Rows[r];
                    AppCompetitiveGroup acg = new AppCompetitiveGroup() { GroupID = (int)row[1], GroupName = (string)row[2], Course = (short)row[3] };
                    acg.IsQuotaBenefitEnabled = ((int)row["IsQuotaBenefitEnabled"] > 0);
                    acg.HasBenefits = ((int)row["HasBenefits"] > 0);

                    acg.BenefitDocuments = appw2.Documents.Where(x => x.CompetitiveGroupID == acg.GroupID && x.isCommon).ToList();
                    appw2.AppComGroups.Add(acg);
                }
            }

            if (ds.Tables.Count > 2)
            {
                Dictionary<int, List<EntranceTestItem>> allTestsByCompetitiveGroups = new Dictionary<int, List<EntranceTestItem>>();
                bool? campaignNeedsTestReplacements = null;


                for (int r = 0; r < ds.Tables[2].Rows.Count; r++)
                {
                    var row = ds.Tables[2].Rows[r];

                    if (!campaignNeedsTestReplacements.HasValue)
                    {
                        campaignNeedsTestReplacements = ((row["CampaignTypeID"] as short?).GetValueOrDefault() == 1);
                    }

                    int competitiveGroupID = (row["CompetitiveGroupID"] as Int32?).GetValueOrDefault();

                    EntranceTestItem testItem = new EntranceTestItem();
                    testItem.GroupID = (int)row["CompetitiveGroupID"];
                    testItem.GroupName = row["CompetitiveName"] as string;
                    testItem.EntranceTestTypeID = (short)row["EntranceTestTypeID"];
                    testItem.EntranceTestItemID = row["EntranceTestItemID"] as int?;
                    testItem.EducationLevelID = row["EducationLevelID"] as int?;
                    testItem.IsProfileSubject = false;
                    testItem.SubjectID = row["SubjectID"] as int?;
                    testItem.SubjectName = row["SubjectName"] as string;
                    testItem.SubjectNameView = row["SubjectNameView"] as string;
                    testItem.SubjectIsEge = row["SubjectIsEge"] as bool?;
                    testItem.Priority = row["TestPriority"] as int?;
                    testItem.BenefitID = row["AETD_BenefitID"] as short?;
                    testItem.ApplicationIsForSPOandVO = row["ACGI_IsForSPOandVO"] as bool?;
                    testItem.EntranceTestIsForSPOandVO = row["ETIC_IsForSPOandVO"] as bool?;
                    testItem.ReplacedEntranceTestItemID = row["ReplacedEntranceTestItemID"] as int?;


                    testItem.AETD_ID = row["AETD_ID"] as int?;
                    if (testItem.AETD_ID != null)
                    {
                        testItem.Doc = appw2.Documents.Where(x => x.ID == testItem.AETD_ID).FirstOrDefault();
                    }

                    if (!allTestsByCompetitiveGroups.ContainsKey(competitiveGroupID))
                    {
                        allTestsByCompetitiveGroups.Add(competitiveGroupID, new List<EntranceTestItem>());
                    }
                    allTestsByCompetitiveGroups[competitiveGroupID].Add(testItem);
                }


                if (!campaignNeedsTestReplacements.GetValueOrDefault())
                {
                    foreach (int competitiveGroupID in allTestsByCompetitiveGroups.Keys)
                    {
                        AppCompetitiveGroup acg = appw2.AppComGroups.FirstOrDefault(x => x.GroupID == competitiveGroupID);
                        if (acg != null)
                        {
                            acg.TestItems.AddRange(allTestsByCompetitiveGroups[competitiveGroupID]);
                        }
                    }
                }
                else
                {
                    foreach (int competitiveGroupID in allTestsByCompetitiveGroups.Keys)
                    {
                        AppCompetitiveGroup acg = appw2.AppComGroups.FirstOrDefault(x => x.GroupID == competitiveGroupID);
                        if (acg != null)
                        {
                            bool applicationNeedsTestReplacements = allTestsByCompetitiveGroups[competitiveGroupID].First().ApplicationIsForSPOandVO.GetValueOrDefault();
                            if (!applicationNeedsTestReplacements)
                            {
                                acg.TestItems.AddRange(allTestsByCompetitiveGroups[competitiveGroupID].Where(x => !x.EntranceTestIsForSPOandVO.GetValueOrDefault()));
                            }
                            else
                            {
                                List<EntranceTestItem> tests = new List<EntranceTestItem>();
                                foreach (EntranceTestItem test in allTestsByCompetitiveGroups[competitiveGroupID])
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
                                acg.TestItems.AddRange(tests);
                            }
                        }
                    }
                }
            }
            if (appw2.AppComGroups.Count > 0)
            {
                appw2.Course = appw2.AppComGroups.First().Course;
            }
            #endregion

            #region Сочинения
            appw2.CompositionResult = GetComposition(ApplicationId);
            #endregion
            appw2.InstitutionDocumentTypes = getInstitutionDocumentTypes();
            appw2.GetChekcEGE = Convert.ToInt32(ApplicationSQL.GetChekcEGE(appw2.ApplicationID));

            return appw2;
        }
        public static List<IDName> getInstitutionDocumentTypes()
        {
            List<IDName> list = new List<IDName>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand("SELECT InstitutionDocumentTypeID as ID, [Name] FROM InstitutionDocumentType ORDER BY DisplayOrder", con);

                try
                {
                    IDName i;
                    con.Open();

                    SqlDataReader r = com.ExecuteReader();

                    #region Read
                    while (r.Read())
                    {
                        i = new IDName();
                        i.ID = (int)r["ID"];
                        i.Name = r["Name"] as string;
                        list.Add(i);
                    }
                    #endregion
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

            return list;

        }
        public static EntrantTestItemDocument AppEntTestDocGet(int? AppEntTestDocID)
        {
            #region SQL
            string sql = @"
SELECT AETD.ApplicationID, AETD.CompetitiveGroupID, CG.Name as CompetitiveGroupName, CG.Course 
, ETIC.SubjectID, ETIC.SubjectName 
,	ISNULL(ETIC_Subject.Name, ETIC.SubjectName) as SubjectNameView
, ETIC_Subject.IsEge as SubjectIsEge
, ETIC.EntranceTestPriority as TestPriority
, ETIC.EntranceTestTypeID 
, ETIC.EntranceTestItemID 
, AETD.EntrantDocumentID
		,(CASE ed.DocumentTypeID 
			WHEN 9 THEN
				dt.Name+' «'+
				(SELECT TOP 1 _ot.Name FROM EntrantDocumentOlympic _edo (NOLOCK) INNER JOIN OlympicType _ot (NOLOCK) on _edo.OlympicID=_ot.OlympicID WHERE _edo.EntrantDocumentID=ed.EntrantDocumentID)
				+'»'
			ELSE
				dt.Name
		END) as AETD_DocumentTypeName
, ed.DocumentTypeID as AETD_DocumentTypeID, ed.DocumentSeries as AETD_DocumentSeries, ed.DocumentNumber as AETD_DocumentNumber, ed.DocumentDate as AETD_DocumentDate

, AETD.ID as AETD_ID -- Для проверки наличия таковой
, AETD.CompetitiveGroupID as AETD_CompetitiveGroupID
, AETD.BenefitID as AETD_BenefitID
, AETD_Benefit.Name as AETD_BenefitName
, AETD.SourceID as AETD_SourceID
, AETD.SubjectID as AETD_SubjectID
, AETD_sb.Name as AETD_SubjectName
, AETD.EntranceTestItemID as AETD_EntranceTestItemID
, AETD.EntrantDocumentID as AETD_EntrantDocumentID
, AETD.ResultValue as AETD_ResultValue
, AETD.InstitutionDocumentNumber as AETD_InstitutionDocumentNumber
, AETD.InstitutionDocumentDate  as AETD_InstitutionDocumentDate
, AETD.InstitutionDocumentTypeID as AETD_InstitutionDocumentTypeID
, AETD_idt.Name as AETD_InstitutionDocumentTypeName
, AETD.HasEge as AETD_HasEge
, AETD.EgeResultValue as AETD_EgeResultValue
FROM ApplicationEntranceTestDocument AETD (NOLOCK)
LEFT OUTER JOIN EntranceTestItemC as ETIC (NOLOCK) on ETIC.EntranceTestItemID=AETD.EntranceTestItemID AND AETD.ApplicationID=@ApplicationID
LEFT OUTER JOIN CompetitiveGroup CG (NOLOCK) ON CG.CompetitiveGroupID = ETIC.CompetitiveGroupID
LEFT OUTER JOIN Subject ETIC_Subject (NOLOCK) ON ETIC_Subject.SubjectID = ETIC.SubjectID
LEFT OUTER JOIN EntrantDocument ed (NOLOCK) on ed.EntrantDocumentID=AETD.EntrantDocumentID
LEFT OUTER JOIN DocumentType dt (NOLOCK) on dt.DocumentID=ed.DocumentTypeID
LEFT OUTER JOIN Subject AETD_sb (NOLOCK) ON AETD_sb.SubjectID = AETD.SubjectID
LEFT OUTER JOIN Benefit AETD_Benefit (NOLOCK) on AETD_Benefit.BenefitID=AETD.BenefitID
LEFT OUTER JOIN InstitutionDocumentType AETD_idt (NOLOCK) on AETD_idt.InstitutionDocumentTypeID=AETD.InstitutionDocumentTypeID
WHERE AETD.ID=@AppEntTestDocID

";
            #endregion

            EntrantTestItemDocument etid = null;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("AppEntTestDocID", SqlDbType.Int) { Value = (AppEntTestDocID == null) ? ((object)DBNull.Value) : AppEntTestDocID });

                try
                {
                    con.Open();
                    SqlDataReader r = com.ExecuteReader();
                    #region Read
                    if (r.Read())
                    {
                        etid = new EntrantTestItemDocument();

                        //eti=new EntranceTestItem();
                        //eti.GroupID=(int)r["CompetitiveGroupID"];
                        //eti.GroupName=r["CompetitiveGroupName"] as string;
                        //eti.EntranceTestTypeID=(short)r["EntranceTestTypeID"];
                        //eti.EntranceTestItemID=r["EntranceTestItemID"] as int?;
                        //eti.IsProfileSubject=false;
                        //eti.SubjectID=r["SubjectID"] as int?;
                        //eti.SubjectName=r["SubjectName"] as string;
                        //eti.SubjectNameView=r["SubjectNameView"] as string;
                        //eti.SubjectIsEge=r["SubjectIsEge"] as bool?;
                        //eti.Priority=r["TestPriority"] as int?;

                        //if(!r.IsDBNull(r.GetOrdinal("AETD_ID"))) {
                        //   EntrantTestItemDocument d=new EntrantTestItemDocument();
                        //   d.ID=r["AETD_ID"] as int?;
                        //   d.ApplicationID=r["ApplicationID"] as int?;
                        //   d.CompetitiveGroupID=eti.GroupID;

                        //   d.SourceID=r["AETD_SourceID"] as int?;
                        //   d.SubjectID=r["AETD_SubjectID"] as int?;
                        //   d.BenefitID=r["AETD_BenefitID"] as short?;
                        //   d.BenefitName=r["AETD_BenefitName"] as string;
                        //   d.EntranceTestItemID=r["AETD_EntranceTestItemID"] as int?;
                        //   d.EntrantDocumentID=r["AETD_EntrantDocumentID"] as int?;
                        //   d.DocumentTypeID=r["AETD_DocumentTypeID"] as int?;
                        //   d.DocumentTypeName=r["AETD_DocumentTypeName"] as string;
                        //   d.DocumentSeries=r["AETD_DocumentSeries"] as string;
                        //   d.DocumentNumber=r["AETD_DocumentNumber"] as string;
                        //   d.DocumentDate=r["AETD_DocumentDate"] as DateTime?;
                        //   d.InstitutionDocumentTypeID=r["AETD_InstitutionDocumentTypeID"] as int?;
                        //   d.InstitutionDocumentTypeName=r["AETD_InstitutionDocumentTypeName"] as string;
                        //   d.InstitutionDocumentDate=r["AETD_InstitutionDocumentDate"] as DateTime?;
                        //   d.InstitutionDocumentNumber=r["AETD_InstitutionDocumentNumber"] as string;
                        //   d.ResultValue=r["AETD_ResultValue"] as decimal?;
                        //   d.HasEge=r["AETD_HasEge"] as bool?;
                        //   d.EgeResultValue=r["AETD_EgeResultValue"] as decimal?;
                        //   eti.Doc=d;
                        //}
                    }
                    #endregion
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
            return etid;
        }

        public static int CheckFromKrym(int applicationId)
        {
            int result = 0;

            string sql = @"
            SELECT cg.IsFromKrym 
            FROM ApplicationCompetitiveGroupItem AS acgi (NOLOCK)
            INNER JOIN CompetitiveGroup AS cg (NOLOCK) ON cg.CompetitiveGroupID = acgi.CompetitiveGroupId
            WHERE acgi.ApplicationId=@ApplicationID;
            ";

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                try
                {
                    SqlCommand com = new SqlCommand(sql, con);
                    com.Parameters.Add(new SqlParameter("ApplicationID", applicationId));

                    con.Open();
                    DataSet ds = new DataSet();
                    SqlDataAdapter sqlAdapter = new SqlDataAdapter(com);
                    sqlAdapter.Fill(ds);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        result = Convert.ToInt32(ds.Tables[0].Rows[0]["IsFromKrym"] as Int32?);
                    }
                    con.Close();
                }
                catch (SqlException e)
                {
                    throw e; // Пробросить дальше
                }
                catch (Exception e)
                {
                    throw e; // Пробросить дальше
                }
                finally
                {
                    if (con.State != ConnectionState.Closed)
                    {
                        con.Close();
                    }
                }

                return result;
            }
        }

        public static
            EntranceTestItem AppEntTestItemDocGet(int ApplicationId, int? EntranceTestItemID, int? AppEntTestDocID)
        {
            #region SQL
            string sql = "";
            if (EntranceTestItemID != null)
            {
                sql = @"
--DECLARE @EntranceTestItemID int
--DECLARE @ApplicationID int
-- @AppEntTestDocID
--SET @EntranceTestItemID=306950
--SET @ApplicationID=20509899

SELECT AETD.ApplicationID, AETD.CompetitiveGroupID, CG.Name as CompetitiveGroupName, CG.Course 
, ETIC.SubjectID, ETIC.SubjectName 
,	ISNULL(ETIC_Subject.Name, ETIC.SubjectName) as SubjectNameView
, ETIC_Subject.IsEge as SubjectIsEge
, ETIC.EntranceTestPriority as TestPriority
, ETIC.EntranceTestTypeID 
, ETIC.EntranceTestItemID 
, AETD.EntrantDocumentID
		,(CASE ed.DocumentTypeID 
			WHEN 9 THEN
				dt.Name+' «'+
				(SELECT TOP 1 _ot.Name FROM EntrantDocumentOlympic _edo (NOLOCK) INNER JOIN OlympicType _ot (NOLOCK) on _edo.OlympicID=_ot.OlympicID WHERE _edo.EntrantDocumentID=ed.EntrantDocumentID)
				+'»'
			ELSE
				dt.Name
		END) as AETD_DocumentTypeName
, ed.DocumentTypeID as AETD_DocumentTypeID, ed.DocumentSeries as AETD_DocumentSeries, ed.DocumentNumber as AETD_DocumentNumber, ed.DocumentDate as AETD_DocumentDate

, AETD.ID as AETD_ID -- Для проверки наличия таковой
, AETD.CompetitiveGroupID as AETD_CompetitiveGroupID
, AETD.BenefitID as AETD_BenefitID
, AETD_Benefit.Name as AETD_BenefitName
, AETD.SourceID as AETD_SourceID
, AETD.SubjectID as AETD_SubjectID
, AETD_sb.Name as AETD_SubjectName
, AETD.EntranceTestItemID as AETD_EntranceTestItemID
, AETD.EntrantDocumentID as AETD_EntrantDocumentID
, AETD.ResultValue as AETD_ResultValue
, AETD.InstitutionDocumentNumber as AETD_InstitutionDocumentNumber
, AETD.InstitutionDocumentDate  as AETD_InstitutionDocumentDate
, AETD.InstitutionDocumentTypeID as AETD_InstitutionDocumentTypeID
, AETD_idt.Name as AETD_InstitutionDocumentTypeName
, AETD.HasEge as AETD_HasEge
, AETD.EgeResultValue as AETD_EgeResultValue
, ed.OlympApproved
FROM EntranceTestItemC as ETIC (NOLOCK)
LEFT OUTER JOIN  ApplicationEntranceTestDocument AETD (NOLOCK) on ETIC.EntranceTestItemID=AETD.EntranceTestItemID AND AETD.ApplicationID=@ApplicationID
INNER JOIN CompetitiveGroup CG (NOLOCK) ON CG.CompetitiveGroupID = ETIC.CompetitiveGroupID
LEFT OUTER JOIN Subject ETIC_Subject (NOLOCK) ON ETIC_Subject.SubjectID = ETIC.SubjectID
LEFT OUTER JOIN EntrantDocument ed (NOLOCK) on ed.EntrantDocumentID=AETD.EntrantDocumentID
LEFT OUTER JOIN DocumentType dt (NOLOCK) on dt.DocumentID=ed.DocumentTypeID
LEFT OUTER JOIN Subject AETD_sb (NOLOCK) ON AETD_sb.SubjectID = AETD.SubjectID
LEFT OUTER JOIN Benefit AETD_Benefit (NOLOCK) on AETD_Benefit.BenefitID=AETD.BenefitID
LEFT OUTER JOIN InstitutionDocumentType AETD_idt (NOLOCK) on AETD_idt.InstitutionDocumentTypeID=AETD.InstitutionDocumentTypeID
WHERE 
AETD.ApplicationID=@ApplicationID AND
(
(@EntranceTestItemID IS NOT NULL AND ETIC.EntranceTestItemID=@EntranceTestItemID)
OR 
(@AppEntTestDocID IS NOT NULL AND AETD.ID=@AppEntTestDocID)
)
";
            }
            if (AppEntTestDocID != null)
            {
                sql = @"
SELECT AETD.ApplicationID, AETD.CompetitiveGroupID, CG.Name as CompetitiveGroupName, CG.Course 
, ETIC.SubjectID, ETIC.SubjectName 
,	ISNULL(ETIC_Subject.Name, ETIC.SubjectName) as SubjectNameView
, ETIC_Subject.IsEge as SubjectIsEge
, ETIC.EntranceTestPriority as TestPriority
, ETIC.EntranceTestTypeID 
, ETIC.EntranceTestItemID 
, AETD.EntrantDocumentID
		,(CASE ed.DocumentTypeID 
			WHEN 9 THEN
				dt.Name+' «'+
				(SELECT TOP 1 _ot.Name FROM EntrantDocumentOlympic _edo (NOLOCK) INNER JOIN OlympicType _ot (NOLOCK) on _edo.OlympicID=_ot.OlympicID WHERE _edo.EntrantDocumentID=ed.EntrantDocumentID)
				+'»'
			ELSE
				dt.Name
		END) as AETD_DocumentTypeName
, ed.DocumentTypeID as AETD_DocumentTypeID, ed.DocumentSeries as AETD_DocumentSeries, ed.DocumentNumber as AETD_DocumentNumber, ed.DocumentDate as AETD_DocumentDate

, AETD.ID as AETD_ID -- Для проверки наличия таковой
, AETD.CompetitiveGroupID as AETD_CompetitiveGroupID
, AETD.BenefitID as AETD_BenefitID
, AETD_Benefit.Name as AETD_BenefitName
, AETD.SourceID as AETD_SourceID
, AETD.SubjectID as AETD_SubjectID
, AETD_sb.Name as AETD_SubjectName
, AETD.EntranceTestItemID as AETD_EntranceTestItemID
, AETD.EntrantDocumentID as AETD_EntrantDocumentID
, AETD.ResultValue as AETD_ResultValue
, AETD.InstitutionDocumentNumber as AETD_InstitutionDocumentNumber
, AETD.InstitutionDocumentDate  as AETD_InstitutionDocumentDate
, AETD.InstitutionDocumentTypeID as AETD_InstitutionDocumentTypeID
, AETD_idt.Name as AETD_InstitutionDocumentTypeName
, AETD.HasEge as AETD_HasEge
, AETD.EgeResultValue as AETD_EgeResultValue
, ed.OlympApproved
,(CASE WHEN ed.DocumentTypeID=2 OR ed.DocumentTypeID=17 THEN
	(SELECT SubjectID FROM EntrantDocumentEgeAndOlympicSubject EDEAOS (NOLOCK)  WHERE EDEAOS.EntrantDocumentID=ed.EntrantDocumentID AND ETIC.SubjectID=SubjectID) 
	ELSE NULL 
END
) as EGE_SubjectID
FROM ApplicationEntranceTestDocument AETD (NOLOCK)
LEFT OUTER JOIN EntranceTestItemC as ETIC (NOLOCK) on ETIC.EntranceTestItemID=AETD.EntranceTestItemID AND AETD.ApplicationID=@ApplicationID
LEFT OUTER JOIN CompetitiveGroup CG (NOLOCK) ON CG.CompetitiveGroupID = ETIC.CompetitiveGroupID
LEFT OUTER JOIN Subject ETIC_Subject (NOLOCK) ON ETIC_Subject.SubjectID = ETIC.SubjectID
LEFT OUTER JOIN EntrantDocument ed (NOLOCK) on ed.EntrantDocumentID=AETD.EntrantDocumentID
LEFT OUTER JOIN DocumentType dt (NOLOCK) on dt.DocumentID=ed.DocumentTypeID
LEFT OUTER JOIN Subject AETD_sb (NOLOCK) ON AETD_sb.SubjectID = AETD.SubjectID
LEFT OUTER JOIN Benefit AETD_Benefit (NOLOCK) on AETD_Benefit.BenefitID=AETD.BenefitID
LEFT OUTER JOIN InstitutionDocumentType AETD_idt (NOLOCK) on AETD_idt.InstitutionDocumentTypeID=AETD.InstitutionDocumentTypeID
WHERE AETD.ID=@AppEntTestDocID
";
            }
            #endregion
            EntranceTestItem eti = null;

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("ApplicationId", ApplicationId));
                com.Parameters.Add(new SqlParameter("EntranceTestItemID", SqlDbType.Int) { Value = (EntranceTestItemID == null) ? ((object)DBNull.Value) : EntranceTestItemID });
                com.Parameters.Add(new SqlParameter("AppEntTestDocID", SqlDbType.Int) { Value = (AppEntTestDocID == null) ? ((object)DBNull.Value) : AppEntTestDocID });

                try
                {
                    con.Open();
                    SqlDataReader r = com.ExecuteReader();
                    #region Read
                    if (r.Read())
                    {
                        eti = new EntranceTestItem();
                        eti.GroupID = r["CompetitiveGroupID"] as int?;
                        eti.GroupName = r["CompetitiveGroupName"] as string;
                        eti.EntranceTestTypeID = r["EntranceTestTypeID"] as short?;
                        eti.EntranceTestItemID = r["EntranceTestItemID"] as int?;
                        eti.IsProfileSubject = false;
                        eti.SubjectID = r["SubjectID"] as int?;
                        eti.SubjectName = r["SubjectName"] as string;
                        eti.SubjectNameView = r["SubjectNameView"] as string;
                        eti.SubjectIsEge = r["SubjectIsEge"] as bool?;
                        eti.Priority = r["TestPriority"] as int?;

                        if (!r.IsDBNull(r.GetOrdinal("AETD_ID")))
                        {
                            EntrantTestItemDocument d = new EntrantTestItemDocument();
                            d.ID = r["AETD_ID"] as int?;
                            d.ApplicationID = r["ApplicationID"] as int?;
                            d.CompetitiveGroupID = r["AETD_CompetitiveGroupID"] as int?;
                            d.SourceID = r["AETD_SourceID"] as int?;
                            d.SubjectID = r["AETD_SubjectID"] as int?;
                            d.BenefitID = r["AETD_BenefitID"] as short?;
                            d.BenefitName = r["AETD_BenefitName"] as string;
                            d.EntranceTestItemID = r["AETD_EntranceTestItemID"] as int?;
                            d.EntrantDocumentID = r["AETD_EntrantDocumentID"] as int?;
                            d.DocumentTypeID = r["AETD_DocumentTypeID"] as int?;
                            d.DocumentTypeName = r["AETD_DocumentTypeName"] as string;
                            d.DocumentSeries = r["AETD_DocumentSeries"] as string;
                            d.DocumentNumber = r["AETD_DocumentNumber"] as string;
                            d.DocumentDate = r["AETD_DocumentDate"] as DateTime?;
                            d.InstitutionDocumentTypeID = r["AETD_InstitutionDocumentTypeID"] as int?;
                            d.InstitutionDocumentTypeName = r["AETD_InstitutionDocumentTypeName"] as string;
                            d.InstitutionDocumentDate = r["AETD_InstitutionDocumentDate"] as DateTime?;
                            d.InstitutionDocumentNumber = r["AETD_InstitutionDocumentNumber"] as string;
                            d.ResultValue = r["AETD_ResultValue"] as decimal?;
                            d.HasEge = r["AETD_HasEge"] as bool?;
                            d.EgeResultValue = r["AETD_EgeResultValue"] as decimal?;
                            d.OlympApproved = Convert.ToInt32(r["OlympApproved"] as bool?);
                            d.EGE_SubjectID = r["EGE_SubjectID"] as int?;
                            eti.Doc = d;
                        }
                    }
                    #endregion
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
            return eti;
        }

        /// <summary>
        /// ApplicationEntranceTestDocument CommmonSave
        /// </summary>
        /// <param name="etid"></param>
        /// <returns></returns>
        public static EntrantTestItemDocument AppEntTestDocCommonSave(EntrantTestItemDocument etid)
        {
            #region SQL
            string sql = @"
--DECLARE @ApplicationID int
--DECLARE @EntrantDocumentID int
--DECLARE @CompetitiveGroupID int
--DECLARE @BenefitID smallint
-DECLARE @SourceID int
DECLARE @AETD_ID	int 


	IF NOT EXISTS(SELECT ID FROM ApplicationEntrantDocument (NOLOCK) WHERE ApplicationID=@ApplicationID AND EntrantDocumentID=@EntrantDocumentID) BEGIN
		INSERT INTO ApplicationEntrantDocument (ApplicationID, EntrantDocumentID)VALUES(@ApplicationID, @EntrantDocumentID)
	END
-- НАДО ЛИ ПРОВЕРЯТЬ НАЛИЧИЕ СВЯЗИ?!
	INSERT INTO [dbo].[ApplicationEntranceTestDocument] (ApplicationID, CompetitiveGroupID, EntrantDocumentID, SourceID, BenefitID)
		 VALUES (@ApplicationID, @CompetitiveGroupID, @EntrantDocumentID, @SourceID, @BenefitID)
	SELECT  @AETD_ID=cast(scope_identity() as int);

SELECT  @AETD_ID as AETD_ID
";
            #endregion
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);
                //com.Parameters.Add(new SqlParameter("aetdID",aetdId));
                try
                {
                    con.Open();
                    SqlDataReader r = com.ExecuteReader();
                    if (r.Read())
                    {
                        etid.ID = (int)r["AETD_ID"];
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
            return etid;
        }

        /// <summary>
        /// Удаление Общего документа из ApplicationEntranceTestDocument AETD и ничего не возвращает
        /// </summary>
        /// <param name="aetdId"></param>
        /// <returns></returns>
        public static void AppEntTestDocCommonDel(int aetdId)
        {
            #region SQL
            string sql = @"
DECLARE @ApplicationID int
DECLARE @EntrantDocumentID int
DECLARE @EntranceTestItemID	int 

SELECT @ApplicationID=ApplicationID, @EntrantDocumentID=EntrantDocumentID, @EntranceTestItemID=EntranceTestItemID FROM ApplicationEntranceTestDocument (NOLOCK) WHERE ID=@aetdID
IF @EntrantDocumentID IS NOT NULL AND @ApplicationID IS NOT NULL BEGIN
	DELETE FROM ApplicationEntrantDocument WHERE ApplicationID=@ApplicationID AND EntrantDocumentID=@EntrantDocumentID
END
DELETE FROM ApplicationEntranceTestDocument WHERE ID=@aetdID

";
            #endregion
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("aetdID", aetdId));
                try
                {
                    con.Open();
                    com.ExecuteNonQuery();
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
        }

        /// <summary>
        /// Удаление из ApplicationEntranceTestDocument AETD и возврат элеента EntranceTestItemС
        /// </summary>
        /// <param name="aetdId"></param>
        /// <returns></returns>
        /// Добавил переменную и условие @aedCount
        public static EntranceTestItem AppEntTestDocDel(int aetdId)
        {
            #region SQL
            string sql = @"
--DECLARE @aetdID int

DECLARE @ApplicationID int
DECLARE @EntrantDocumentID int
DECLARE @EntranceTestItemID	int 
DECLARE @aedCount INT

SELECT @ApplicationID=ApplicationID, @EntrantDocumentID=EntrantDocumentID, @EntranceTestItemID=EntranceTestItemID FROM ApplicationEntranceTestDocument (NOLOCK) WHERE ID=@aetdID
IF @EntrantDocumentID IS NOT NULL AND @ApplicationID IS NOT NULL BEGIN
	SET @aedCount = (SELECT COUNT(*) FROM ApplicationEntranceTestDocument AS aetd (NOLOCK) WHERE  aetd.ApplicationID=@ApplicationID AND aetd.EntrantDocumentID = @EntrantDocumentID	AND aetd.SubjectID IS NOT NULL)
	IF @aedCount=1
	BEGIN
		DELETE FROM ApplicationEntrantDocument WHERE ApplicationID=@ApplicationID AND EntrantDocumentID=@EntrantDocumentID
	END	
END
DELETE FROM ApplicationEntranceTestDocument WHERE ID=@aetdID



SELECT ETIC.CompetitiveGroupID, CG.Name as CompetitiveGroupName, CG.Course 
, ETIC.SubjectID, ETIC.SubjectName 
, ISNULL(ETIC_Subject.Name, ETIC.SubjectName) as SubjectNameView
, ETIC_Subject.IsEge as SubjectIsEge
, ETIC.EntranceTestPriority as TestPriority
, ETIC.EntranceTestTypeID 
, ETIC.EntranceTestItemID
FROM EntranceTestItemC as ETIC (NOLOCK)
INNER JOIN CompetitiveGroup CG (NOLOCK) ON CG.CompetitiveGroupID = ETIC.CompetitiveGroupID
LEFT OUTER JOIN Subject ETIC_Subject (NOLOCK) ON ETIC_Subject.SubjectID = ETIC.SubjectID
WHERE ETIC.EntranceTestItemID=@EntranceTestItemID";
            #endregion

            EntranceTestItem eti = null;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("aetdID", aetdId));
                try
                {
                    con.Open();
                    SqlDataReader r = com.ExecuteReader();
                    if (r.Read())
                    {
                        eti = new EntranceTestItem();
                        eti.GroupID = (int)r["CompetitiveGroupID"];
                        eti.GroupName = r["CompetitiveGroupName"] as string;
                        eti.EntranceTestTypeID = (short)r["EntranceTestTypeID"];
                        eti.EntranceTestItemID = r["EntranceTestItemID"] as int?;
                        eti.IsProfileSubject = false;
                        eti.SubjectID = r["SubjectID"] as int?;
                        eti.SubjectName = r["SubjectName"] as string;
                        eti.SubjectNameView = r["SubjectNameView"] as string;
                        eti.SubjectIsEge = r["SubjectIsEge"] as bool?;
                        eti.Priority = r["TestPriority"] as int?;
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
            return eti;
        }

        /// <summary>
        /// ApplicationEntranceTestDocument CommmonSave
        /// </summary>
        /// <param name="etid"></param>
        /// <returns></returns>
        public static EntrantTestItemDocument AppEntTestDocSave(EntrantTestItemDocument etid)
        {

            string sql = @"";
            SqlCommand com = new SqlCommand();
            #region Types
            //DocumentID	Name
            //1	Документ, удостоверяющий личность
            //2	Свидетельство о результатах ЕГЭ
            //3	Аттестат о среднем (полном) общем образовании
            //4	Диплом о высшем профессиональном образовании
            //5	Диплом о среднем профессиональном образовании   
            //6	Диплом о начальном профессиональном образовании
            //7	Диплом о неполном высшем профессиональном образовании
            //8	Академическая справка
            //9	Диплом победителя/призера олимпиады школьников
            //10	Диплом победителя/призера всероссийской олимпиады школьников
            //11	Справка об установлении инвалидности
            //12	Заключение психолого-медико-педагогической комиссии
            //13	Заключение об отсутствии противопоказаний для обучения
            //14	Военный билет
            //15	Иной документ
            //16	Аттестат об основном общем образовании
            //17	Справка ГИА
            //18	Справка об обучении в другом ВУЗе
            //19	Иной документ об образовании
            //20	Диплом чемпиона/призера Олимпийских игр
            //21	Диплом чемпиона/призера Паралимпийских игр
            //22	Диплом чемпиона/призера Сурдлимпийских игр
            //23	Диплом чемпиона мира
            //24	Диплом чемпиона Европы
            //25	Диплом об окончании аспирантуры (адъюнкатуры)
            //26	Диплом кандидата наук

            //	[EntranceTestResultSource] SourceID	Description
            //	1	Свидетельство ЕГЭ
            //	2	Вступительное испытание ОО
            //	3	Диплом победителя/призера олимпиады
            //	4	Справка ГИА

            // [Benefit] [BenefitID], [Name]
            // 1	Зачисление без вступительных испытаний
            // 2	Приравнивание к лицам, успешно прошедшим дополнительные вступительные испытания
            // 3	Приравнивание к лицам, набравшим максимальное количество баллов по ЕГЭ
            // 4	По квоте приёма лиц, имеющих особое право
            // 5	Преимущественное право на поступление

            #endregion
            #region SQL
            sql = @" 
DECLARE @AETD_ID int 
DECLARE @EntranceTestTypeID int 
DECLARE @DocumentTypeID int
DECLARE @SubjectID int  
DECLARE @ResultValue decimal(7,4)
DECLARE @BenefitID int

SELECT @BenefitID=null, @ResultValue=null, @EgeResultValue=null

IF @HasEge IS NULL BEGIN
	SET @HasEge=0
END

IF @SourceID>100 BEGIN
	SELECT @BenefitID=@SourceID-100
	SET @SourceID=NULL
END
 
IF @EntranceTestItemID IS NOT NULL BEGIN
	SELECT @CompetitiveGroupID=CompetitiveGroupID, @EntranceTestTypeID=EntranceTestTypeID, @SubjectID=SubjectID
		FROM EntranceTestItemC (NOLOCK) WHERE EntranceTestItemID=@EntranceTestItemID
END

IF @EntrantDocumentID IS NOT NULL BEGIN
	SELECT @DocumentTypeID=DocumentTypeID FROM EntrantDocument (NOLOCK) WHERE EntrantDocumentID=@EntrantDocumentID
END

IF @DocumentTypeID =2 OR @DocumentTypeID = 17 BEGIN --2 Свидетельство о результатах ЕГЭ --17	Справка ГИА
	SELECT @SourceID= CASE @DocumentTypeID WHEN 2 THEN 1 WHEN 17 THEN 4 ELSE null END 
	SELECT @ResultValue = Value FROM EntrantDocumentEgeAndOlympicSubject (NOLOCK) WHERE  EntrantDocumentID=@EntrantDocumentID AND SubjectID=@SubjectID
	IF @ResultValue IS NULL BEGIN
		SET @ResultValue=0
	END
END

--Следующие документы - приравниваются к получению 100 баллов
--Документ об участии в международной олимпиаде (DocumentTypeID=28)
--Диплом победителя/призера всероссийской олимпиады школьников (DocumentTypeID=10)
--Диплом победителя/призера IV этапа всеукраинской ученической олимпиады (DocumentTypeID=27)
--Диплом победителя/призера олимпиады школьников (DocumentTypeID=9)
--Диплом чемпиона/призера Олимпийских игр (DocumentTypeID=20)
--Диплом чемпиона/призера Паралимпийских игр (DocumentTypeID=21)
--Диплом чемпиона/призера Сурдлимпийских игр (DocumentTypeID=22)
--Диплом чемпиона мира (DocumentID=23)
--Диплом чемпиона Европы (DocumentID=24)  
--Иной документ для всех кроме бак/спец - FIS-1768 (24.08.2017)    
IF @EntranceTestItemID IS NOT NULL AND @DocumentTypeID IN (9,10,20,21,22,23,24,27,28,15) BEGIN
	SELECT @SourceID= 3, @BenefitID =3, @ResultValue=100
END

IF @EntrantDocumentID IS NOT NULL BEGIN
	IF NOT EXISTS(SELECT ID FROM ApplicationEntrantDocument (NOLOCK) WHERE ApplicationID=@ApplicationID AND EntrantDocumentID=@EntrantDocumentID) BEGIN
		INSERT INTO ApplicationEntrantDocument(ApplicationID, EntrantDocumentID)VALUES(@ApplicationID, @EntrantDocumentID)
	END
END

DELETE FROM ApplicationEntranceTestDocument WHERE ApplicationID=@ApplicationID AND EntranceTestItemID=@EntranceTestItemID

IF @InstitutionDocumentTypeID IS NULL BEGIN
	INSERT INTO [dbo].[ApplicationEntranceTestDocument]
           ( ApplicationID,  CompetitiveGroupID,  EntranceTestItemID,  EntranceTestTypeID,  SourceID,  SubjectID,  BenefitID,  EntrantDocumentID,  ResultValue,  EgeResultValue,  HasEge)
     VALUES(@ApplicationID, @CompetitiveGroupID, @EntranceTestItemID, @EntranceTestTypeID, @SourceID, @SubjectID, @BenefitID, @EntrantDocumentID, @ResultValue, @EgeResultValue, @HasEge)
	SELECT  @AETD_ID=cast(scope_identity() as int);
END
ELSE BEGIN
	SELECT @SourceID=2, @ResultValue=@InstitutionResultValue, @EntrantDocumentID=null
	INSERT INTO [dbo].[ApplicationEntranceTestDocument]
           ( ApplicationID,  CompetitiveGroupID,  EntranceTestItemID,  EntranceTestTypeID,  SourceID,  SubjectID,  BenefitID,  EntrantDocumentID,  ResultValue,
			InstitutionDocumentTypeID, InstitutionDocumentNumber, InstitutionDocumentDate)
     VALUES(@ApplicationID, @CompetitiveGroupID, @EntranceTestItemID, @EntranceTestTypeID, @SourceID, @SubjectID, @BenefitID, @EntrantDocumentID, @InstitutionResultValue
			,@InstitutionDocumentTypeID, @InstitutionDocumentNumber, @InstitutionDocumentDate)
	SELECT  @AETD_ID=cast(scope_identity() as int);	
END 
 
SELECT  @AETD_ID as AETD_ID
";
            #endregion
            com.Parameters.Add(new SqlParameter("ApplicationID", etid.ApplicationID));
            com.Parameters.Add(new SqlParameter("EntranceTestItemID", SqlDbType.Int) { Value = (etid.EntranceTestItemID == null) ? ((object)DBNull.Value) : etid.EntranceTestItemID });
            com.Parameters.Add(new SqlParameter("EntrantDocumentID", SqlDbType.Int) { Value = (etid.EntrantDocumentID == null) ? ((object)DBNull.Value) : etid.EntrantDocumentID });
            com.Parameters.Add(new SqlParameter("CompetitiveGroupID", SqlDbType.Int) { Value = (etid.CompetitiveGroupID == null) ? ((object)DBNull.Value) : etid.CompetitiveGroupID });
            com.Parameters.Add(new SqlParameter("SourceID", SqlDbType.Int) { Value = (etid.SourceID == null) ? ((object)DBNull.Value) : etid.SourceID });

            com.Parameters.Add(new SqlParameter("InstitutionDocumentTypeID", SqlDbType.Int) { Value = (etid.InstitutionDocumentTypeID == null) ? ((object)DBNull.Value) : etid.InstitutionDocumentTypeID });
            com.Parameters.Add(new SqlParameter("InstitutionDocumentNumber", SqlDbType.VarChar) { Value = (etid.InstitutionDocumentNumber == null) ? ((object)DBNull.Value) : etid.InstitutionDocumentNumber });
            com.Parameters.Add(new SqlParameter("InstitutionDocumentDate", SqlDbType.DateTime) { Value = (etid.InstitutionDocumentDate == null) ? ((object)DBNull.Value) : etid.InstitutionDocumentDate });
            com.Parameters.Add(new SqlParameter("InstitutionResultValue", SqlDbType.Decimal) { Value = (etid.ResultValue == null) ? ((object)DBNull.Value) : etid.ResultValue });
            com.Parameters.Add(new SqlParameter("HasEge ", SqlDbType.Bit) { Value = (etid.HasEge == null) ? ((object)DBNull.Value) : etid.HasEge.Value });
            com.Parameters.Add(new SqlParameter("EgeResultValue ", SqlDbType.Decimal) { Value = (etid.EgeResultValue == null) ? ((object)DBNull.Value) : etid.EgeResultValue.Value });

            if (String.IsNullOrEmpty(sql)) { return null; }

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                com.CommandText = sql;
                com.Connection = con;
                try
                {
                    con.Open();
                    SqlDataReader r = com.ExecuteReader();
                    if (r.Read())
                    {
                        etid.ID = (int)r["AETD_ID"];
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
            return etid;
        }

        public static EntrantTestItemDocument AppEntTestDocValueSave(EntrantTestItemDocument etid)
        {
            #region SQL
            string sql = @"
--DECLARE @ApplicationID int
--DECLARE @EntranceTestItemID	int 

DECLARE @CompetitiveGroupID int
DECLARE @AETD_ID int 
DECLARE @EntranceTestTypeID int 
DECLARE @SubjectID int 
DECLARE @SourceID	int

SET @SourceID=1

IF @EntranceTestItemID IS NOT NULL BEGIN
	SELECT @CompetitiveGroupID=CompetitiveGroupID, @EntranceTestTypeID=EntranceTestTypeID, @SubjectID=SubjectID
		FROM [dbo].[EntranceTestItemC] (NOLOCK) WHERE EntranceTestItemID=@EntranceTestItemID
END

DELETE FROM ApplicationEntranceTestDocument WHERE ApplicationID=@ApplicationID AND EntranceTestItemID=@EntranceTestItemID
INSERT INTO [dbo].[ApplicationEntranceTestDocument]
           ( ApplicationID,  CompetitiveGroupID,  EntranceTestItemID,  EntranceTestTypeID,  SourceID,  SubjectID,  ResultValue)
     VALUES(@ApplicationID, @CompetitiveGroupID, @EntranceTestItemID, @EntranceTestTypeID, @SourceID, @SubjectID, @ResultValue)
SELECT  @AETD_ID=cast(scope_identity() as int);

SELECT  @AETD_ID as AETD_ID
";
            #endregion
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("ApplicationID", etid.ApplicationID));
                com.Parameters.Add(new SqlParameter("EntranceTestItemID", etid.EntranceTestItemID));
                com.Parameters.Add(new SqlParameter("ResultValue", etid.ResultValue.Value));
                try
                {
                    con.Open();
                    SqlDataReader r = com.ExecuteReader();
                    if (r.Read())
                    {
                        etid.ID = (int)r["AETD_ID"];
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
            return etid;
        }

        public static int[] AppEntTestDocEgeValueSave(EntrantTestItemDocument etid)
        {
            #region SQL

            string sql = @"
            DECLARE @OlympicTypeID INT,
            @EgeMinValue INT,
            @IsFromKrym INT
				SET @EgeMinValue=0

            SET @OlympicTypeID =
            (
                SELECT edo.OlympicID FROM EntrantDocument AS ed (NOLOCK) 
                INNER JOIN EntrantDocumentOlympic AS edo (NOLOCK) ON edo.EntrantDocumentID = ed.EntrantDocumentID
                WHERE edo.EntrantDocumentID=@EntrantDocumentID
                GROUP BY edo.OlympicID
            )
				IF @SubjectID IS NOT NULL BEGIN
             SET @EgeMinValue =(
			    SELECT DISTINCT (
	                SELECT MIN(bic.EgeMinValue)
	                FROM CompetitiveGroup AS cg (NOLOCK) 
								INNER JOIN BenefitItemC AS bic (NOLOCK) ON bic.CompetitiveGroupID = cg.CompetitiveGroupID
								LEFT JOIN BenefitItemCOlympicType AS bict (NOLOCK) ON bict.BenefitItemID = bic.BenefitItemID
                        INNER JOIN EntranceTestItemC AS etic (NOLOCK) ON etic.EntranceTestItemID = bic.EntranceTestItemID
	                WHERE cg.CompetitiveGroupID=aetd.CompetitiveGroupID AND (bict.OlympicTypeID = @OlympicTypeID OR bict.OlympicTypeID IS NULL) AND etic.SubjectID=@SubjectID
                ) AS EgeMinValue
               FROM ApplicationEntranceTestDocument AS aetd (NOLOCK)
							INNER JOIN EntrantDocument AS ed (NOLOCK) ON ed.EntrantDocumentID = aetd.EntrantDocumentID
							LEFT JOIN EntrantDocumentOlympic AS edo (NOLOCK) ON edo.EntrantDocumentID = ed.EntrantDocumentID
					WHERE aetd.ApplicationID=@ApplicationID  AND aetd.EntranceTestItemID IS NOT NULL AND aetd.ID=@ID AND aetd.SubjectID=@SubjectID
             )
            END
            
            SET @IsFromKrym =
            (
                SELECT cg.IsFromKrym
                FROM ApplicationEntranceTestDocument AS aetd (NOLOCK)
							INNER JOIN CompetitiveGroup AS cg (NOLOCK)  ON cg.CompetitiveGroupID = aetd.CompetitiveGroupID
							INNER JOIN Campaign AS c (NOLOCK) ON c.CampaignID = cg.CampaignID
                WHERE aetd.ApplicationID=@ApplicationID AND aetd.EntrantDocumentID=@EntrantDocumentID
                GROUP BY cg.IsFromKrym
            )
            IF ISNULL(@IsFromKrym,0) <> 1
            BEGIN
                IF ISNULL(@EgeResultValue,0)<ISNULL(@EgeMinValue,0)
                BEGIN
                    SELECT 1 AS IdError, ISNULL(@EgeMinValue, 0) AS EgeMinValue
	                RETURN
                END
                ELSE BEGIN
	                UPDATE ApplicationEntranceTestDocument SET EgeResultValue=@EgeResultValue WHERE ID=@ID
                    SELECT 0 AS IdError, ISNULL(@EgeMinValue, 0) AS EgeMinValue
	            END
            END
            ELSE
            	BEGIN
            		UPDATE ApplicationEntranceTestDocument SET EgeResultValue=@EgeResultValue WHERE ID=@ID
					SELECT 0 AS IdError, ISNULL(@EgeMinValue, 0) AS EgeMinValue
					RETURN
				END
            ";
            #endregion

            int[] result = new int[2];

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("ID", etid.ID));
                com.Parameters.Add(new SqlParameter("EgeResultValue ", SqlDbType.Decimal) { Value = etid.ResultValue ?? (object)DBNull.Value });
                com.Parameters.Add(new SqlParameter("EntrantDocumentID", etid.EntrantDocumentID ?? (object)DBNull.Value));
                com.Parameters.Add(new SqlParameter("ApplicationID", etid.ApplicationID));
                com.Parameters.Add(new SqlParameter("SubjectID", SqlDbType.Int) { Value = etid.SubjectID ?? (object)DBNull.Value });
                try
                {
                    con.Open();
                    DataSet ds = new DataSet();
                    SqlDataAdapter sqlAdapter = new SqlDataAdapter(com);
                    sqlAdapter.Fill(ds);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        result[0] = Convert.ToInt32(ds.Tables[0].Rows[0]["IdError"] as Int32?);
                        result[1] = Convert.ToInt32(ds.Tables[0].Rows[0]["EgeMinValue"] as Int32?);
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
                    if (con.State != ConnectionState.Closed)
                    {
                        con.Close();
                    }
                }
            }
            return result;
        }

        public static SelectEntDocsList SelectEntDocsList(int ApplicationID, int? EntranceTestItemID, int SourceID, int GroupID)
        {
            #region Types
            //DocumentID	Name
            //1	Документ, удостоверяющий личность
            //2	Свидетельство о результатах ЕГЭ
            //3	Аттестат о среднем (полном) общем образовании
            //4	Диплом о высшем профессиональном образовании
            //5	Диплом о среднем профессиональном образовании
            //6	Диплом о начальном профессиональном образовании
            //7	Диплом о неполном высшем профессиональном образовании
            //8	Академическая справка
            //9	Диплом победителя/призера олимпиады школьников
            //10	Диплом победителя/призера всероссийской олимпиады школьников
            //11	Справка об установлении инвалидности
            //12	Заключение психолого-медико-педагогической комиссии
            //13	Заключение об отсутствии противопоказаний для обучения
            //14	Военный билет
            //15	Иной документ
            //16	Аттестат об основном общем образовании
            //17	Справка ГИА
            //18	Справка об обучении в другом ВУЗе
            //19	Иной документ об образовании
            //20	Диплом чемпиона/призера Олимпийских игр
            //21	Диплом чемпиона/призера Паралимпийских игр
            //22	Диплом чемпиона/призера Сурдлимпийских игр
            //23	Диплом чемпиона мира
            //24	Диплом чемпиона Европы
            //25	Диплом об окончании аспирантуры (адъюнкатуры)
            //26	Диплом кандидата наук

            //	[EntranceTestResultSource] SourceID	Description
            //	1	Свидетельство ЕГЭ
            //	2	Вступительное испытание ОО
            //	3	Диплом победителя/призера олимпиады
            //	4	Справка ГИА

            // [Benefit] [BenefitID], [Name]
            // 1	Зачисление без вступительных испытаний
            // 2	Приравнивание к лицам, успешно прошедшим дополнительные вступительные испытания
            // 3	Приравнивание к лицам, набравшим максимальное количество баллов по ЕГЭ
            // 4	По квоте приёма лиц, имеющих особое право
            // 5	Преимущественное право на поступление

            #endregion

            #region sql
            string sql = @"";

            switch (SourceID)
            {
                #region 1 Свидетельство ЕГЭ
                case 1:
                    sql = @"
DECLARE @DocumentID	int
SET @DocumentID=2
SELECT [DocumentID] as ID, [Name]  FROM DocumentType WHERE DocumentID=@DocumentID
SELECT ed.EntrantDocumentID, ed.DocumentTypeID, ed.DocumentSeries, ed.DocumentNumber, ed.DocumentDate
	, ed.DocumentName AS DocumentTypeName
    , ed.OlympApproved
FROM EntrantDocument ed (NOLOCK)
INNER JOIN DocumentType dt (NOLOCK) on dt.DocumentId=ed.DocumentTypeID
INNER JOIN [Application] a (NOLOCK) on a.EntrantID=ed.EntrantID
INNER JOIN EntrantDocumentEgeAndOlympicSubject edeaos (NOLOCK) on  edeaos.EntrantDocumentID=ed.EntrantDocumentID
INNER JOIN EntranceTestItemC eti (NOLOCK) on eti.SubjectID=edeaos.SubjectID
WHERE a.ApplicationID=@ApplicationID AND ed.DocumentTypeID=@DocumentID AND eti.EntranceTestItemID=@EntranceTestItemID
"; break;
                #endregion
                #region SourceID=2
                case 2:
                    break;
                #endregion
                #region  3	Право на 100 баллов
                /*
Документ об участии в международной олимпиаде (DocumentID=28)
Диплом победителя/призера всероссийской олимпиады школьников (DocumentID=10)
Диплом победителя/призера IV этапа всеукраинской ученической олимпиады (DocumentID=27)
Диплом победителя/призера олимпиады школьников (DocumentID=9)
Диплом чемпиона/призера Олимпийских игр (DocumentID=20)
Диплом чемпиона/призера Паралимпийских игр (DocumentID=21)
Диплом чемпиона/призера Сурдлимпийских игр (DocumentID=22)
Диплом чемпиона мира (DocumentID=23)
Диплом чемпиона Европы (DocumentID=24)       
Иной документ для всех кроме бак/спец - FIS-1768 (24.08.2017)                  
                     */
                case 3:
                    sql = @"
SELECT [DocumentID] as ID, [Name]  
FROM DocumentType (NOLOCK)
OUTER APPLY 
(SELECT TOP (1) c.CampaignTypeID FROM Application a (NOLOCK)
JOIN ApplicationCompetitiveGroupItem acgi (NOLOCK) ON acgi.ApplicationId = a.ApplicationID
JOIN CompetitiveGroup cg (NOLOCK) ON cg.CompetitiveGroupID = acgi.CompetitiveGroupId
JOIN Campaign c (NOLOCK) ON c.CampaignID = cg.CampaignID
WHERE a.ApplicationID = @ApplicationID) AS t
WHERE 
(DocumentID in (9,10,20,21,22,23,24,27,28) AND t.CampaignTypeID = 1)
OR
(DocumentID in (9,10,20,21,22,23,24,27,28,15) AND t.CampaignTypeID != 1)
SELECT ed.EntrantDocumentID, ed.DocumentTypeID, ed.DocumentSeries, ed.DocumentNumber, ed.DocumentDate
,ed.DocumentName as DocumentTypeName
,ed.OlympApproved
FROM EntrantDocument ed (NOLOCK)
INNER JOIN DocumentType dt (NOLOCK) on dt.DocumentId=ed.DocumentTypeID
INNER JOIN [Application] a (NOLOCK) on a.EntrantID=ed.EntrantID
OUTER APPLY 
(SELECT TOP (1) c.CampaignTypeID FROM Application a (NOLOCK)
JOIN ApplicationCompetitiveGroupItem acgi (NOLOCK) ON acgi.ApplicationId = a.ApplicationID
JOIN CompetitiveGroup cg (NOLOCK) ON cg.CompetitiveGroupID = acgi.CompetitiveGroupId
JOIN Campaign c (NOLOCK) ON c.CampaignID = cg.CampaignID
WHERE a.ApplicationID = @ApplicationID) AS t
WHERE a.ApplicationID=@ApplicationID 
AND ((DocumentID in (9,10,20,21,22,23,24,27,28) AND t.CampaignTypeID = 1)
OR
(DocumentID in (9,10,20,21,22,23,24,27,28,15) AND t.CampaignTypeID != 1))
";
                    break;
                #endregion
                #region SourceID=4
                case 4:
                    sql = @"
DECLARE @DocumentID	int
SET @DocumentID=17 
SELECT [DocumentID] as ID, [Name]  FROM DocumentType WHERE DocumentID=@DocumentID
SELECT ed.EntrantDocumentID, ed.DocumentTypeID, ed.DocumentSeries, ed.DocumentNumber, ed.DocumentDate
	, ed.DocumentName AS DocumentTypeName
    , ed.OlympApproved
FROM EntrantDocument ed (NOLOCK)
INNER JOIN DocumentType dt (NOLOCK) on dt.DocumentId=ed.DocumentTypeID
INNER JOIN [Application] a (NOLOCK) on a.EntrantID=ed.EntrantID
INNER JOIN EntrantDocumentEgeAndOlympicSubject edeaos (NOLOCK) on  edeaos.EntrantDocumentID=ed.EntrantDocumentID
INNER JOIN EntranceTestItemC eti (NOLOCK) on eti.SubjectID=edeaos.SubjectID
WHERE a.ApplicationID=@ApplicationID AND ed.DocumentTypeID=@DocumentID AND eti.EntranceTestItemID=@EntranceTestItemID
";
                    break;
                #endregion
                #region SourceID=15
                case 15:
                    //15	Иной документ
                    sql = @"SELECT [DocumentID] as ID, [Name]  FROM DocumentType WHERE DocumentID =15
SELECT ed.EntrantDocumentID, ed.DocumentTypeID, ed.DocumentSeries, ed.DocumentNumber, ed.DocumentDate
,ed.DocumentName as DocumentTypeName
,ed.OlympApproved
FROM EntrantDocument ed (NOLOCK)
INNER JOIN DocumentType dt (NOLOCK) on dt.DocumentId=ed.DocumentTypeID
INNER JOIN [Application] a (NOLOCK) on a.EntrantID=ed.EntrantID
WHERE a.ApplicationID=@ApplicationID AND ed.DocumentTypeID=15";
                    break;

                #endregion
                #region SourceID=101 1	Зачисление без вступительных испытаний
                case 101:
                    /*
Документ об участии в международной олимпиаде (DocumentID=28)
Диплом победителя/призера всероссийской олимпиады школьников (DocumentID=10)
Диплом победителя/призера IV этапа всеукраинской ученической олимпиады (DocumentID=27)
Диплом победителя/призера олимпиады школьников (DocumentID=9)
Диплом чемпиона/призера Олимпийских игр (DocumentID=20)
Диплом чемпиона/призера Паралимпийских игр (DocumentID=21)
Диплом чемпиона/призера Сурдлимпийских игр (DocumentID=22)
Диплом чемпиона мира (DocumentID=23)
Диплом чемпиона Европы (DocumentID=24)    
Иной документ для всех кроме бак/спец - FIS-1768 (24.08.2017)    
                     */
                    sql = @"
SELECT [DocumentID] as ID, [Name]  
FROM DocumentType (NOLOCK)
OUTER APPLY 
(SELECT TOP (1) c.CampaignTypeID FROM Application a (NOLOCK)
JOIN ApplicationCompetitiveGroupItem acgi (NOLOCK) ON acgi.ApplicationId = a.ApplicationID
JOIN CompetitiveGroup cg (NOLOCK) ON cg.CompetitiveGroupID = acgi.CompetitiveGroupId
JOIN Campaign c (NOLOCK) ON c.CampaignID = cg.CampaignID
WHERE a.ApplicationID = @ApplicationID) AS t
WHERE 
(DocumentID in (9,10,20,21,22,23,24,27,28) AND t.CampaignTypeID = 1)
OR
(DocumentID in (9,10,20,21,22,23,24,27,28,15) AND t.CampaignTypeID != 1)
SELECT ed.EntrantDocumentID, ed.DocumentTypeID, ed.DocumentSeries, ed.DocumentNumber, ed.DocumentDate
,ed.DocumentName as DocumentTypeName
,ed.OlympApproved
FROM EntrantDocument ed (NOLOCK)
INNER JOIN DocumentType dt (NOLOCK) on dt.DocumentId=ed.DocumentTypeID
INNER JOIN [Application] a (NOLOCK) on a.EntrantID=ed.EntrantID
OUTER APPLY 
(SELECT TOP (1) c.CampaignTypeID FROM Application a (NOLOCK)
JOIN ApplicationCompetitiveGroupItem acgi (NOLOCK) ON acgi.ApplicationId = a.ApplicationID
JOIN CompetitiveGroup cg (NOLOCK) ON cg.CompetitiveGroupID = acgi.CompetitiveGroupId
JOIN Campaign c (NOLOCK) ON c.CampaignID = cg.CampaignID
WHERE a.ApplicationID = @ApplicationID) AS t
WHERE a.ApplicationID=@ApplicationID 
AND ((DocumentID in (9,10,20,21,22,23,24,27,28) AND t.CampaignTypeID = 1)
OR
(DocumentID in (9,10,20,21,22,23,24,27,28,15) AND t.CampaignTypeID != 1))
";
                    break;
                #endregion
                #region SourceID=102
                case 102:
                    break;
                #endregion
                #region SourceID=103
                case 103:
                    break;
                #endregion
                #region SourceID=104 4	По квоте приёма лиц, имеющих особое право
                case 104:
                    /*
    Справка об установлении инвалидности (DocumentID=11)
    Заключение психолого-медико-педагогической комиссии (DocumentID=12)
    Документ, подтверждающий принадлежность к детям-сиротам и детям, оставшимся без попечения родителей (DocumentID=30)
    Документ, подтверждающий принадлежность к ветеранам боевых действий (DocumentID=31) 
                     */
                    sql = @"
SELECT [DocumentID] as ID, [Name]  FROM DocumentType WHERE DocumentID in (11,12,30,31)
SELECT ed.EntrantDocumentID, ed.DocumentTypeID, ed.DocumentSeries, ed.DocumentNumber, ed.DocumentDate
,ed.DocumentName as DocumentTypeName
, ed.OlympApproved
FROM EntrantDocument ed (NOLOCK)
INNER JOIN DocumentType dt (NOLOCK) on dt.DocumentId=ed.DocumentTypeID
INNER JOIN [Application] a (NOLOCK) on a.EntrantID=ed.EntrantID
WHERE a.ApplicationID=@ApplicationID AND ed.DocumentTypeID in (11,12,30,31)
";
                    break;
                #endregion
                #region SourceID=105 5	Преимущественное право на поступление
                //Иной документ для всех кроме бак/спец - FIS-1768 (24.08.2017)   
                case 105:
                    sql = @"
SELECT [DocumentID] as ID, [Name]  
FROM DocumentType (NOLOCK)
OUTER APPLY 
(SELECT TOP (1) c.CampaignTypeID FROM Application a (NOLOCK)
JOIN ApplicationCompetitiveGroupItem acgi (NOLOCK) ON acgi.ApplicationId = a.ApplicationID
JOIN CompetitiveGroup cg (NOLOCK) ON cg.CompetitiveGroupID = acgi.CompetitiveGroupId
JOIN Campaign c (NOLOCK) ON c.CampaignID = cg.CampaignID
WHERE a.ApplicationID = @ApplicationID) AS t
WHERE 
(DocumentID in (11,30,31,32,33,34,35) AND t.CampaignTypeID = 1)
OR
(DocumentID in (11,30,31,32,33,34,35,15) AND t.CampaignTypeID != 1)
SELECT ed.EntrantDocumentID, ed.DocumentTypeID, ed.DocumentSeries, ed.DocumentNumber, ed.DocumentDate
,ed.DocumentName as DocumentTypeName
,ed.OlympApproved
FROM EntrantDocument ed (NOLOCK)
INNER JOIN DocumentType dt (NOLOCK) on dt.DocumentId=ed.DocumentTypeID
INNER JOIN [Application] a (NOLOCK) on a.EntrantID=ed.EntrantID
OUTER APPLY 
(SELECT TOP (1) c.CampaignTypeID FROM Application a (NOLOCK)
JOIN ApplicationCompetitiveGroupItem acgi (NOLOCK) ON acgi.ApplicationId = a.ApplicationID
JOIN CompetitiveGroup cg (NOLOCK) ON cg.CompetitiveGroupID = acgi.CompetitiveGroupId
JOIN Campaign c (NOLOCK) ON c.CampaignID = cg.CampaignID
WHERE a.ApplicationID = @ApplicationID) AS t
WHERE a.ApplicationID=@ApplicationID 
AND ((DocumentID in (11,30,31,32,33,34,35) AND t.CampaignTypeID = 1)
OR
(DocumentID in (11,30,31,32,33,34,35,15) AND t.CampaignTypeID != 1))
";
                    break;
                #endregion
                #region SourceID=106 1	Крым
                case 106:
                    sql = @"
SELECT [DocumentID] as ID, [Name]  FROM DocumentType 
WHERE DocumentID in (1,15) 
SELECT ed.EntrantDocumentID, ed.DocumentTypeID, ed.DocumentSeries, ed.DocumentNumber, ed.DocumentDate
,ed.DocumentName as DocumentTypeName
, ed.OlympApproved
FROM EntrantDocument ed (NOLOCK)
INNER JOIN DocumentType dt (NOLOCK) on dt.DocumentId=ed.DocumentTypeID
INNER JOIN [Application] a (NOLOCK) on a.EntrantID=ed.EntrantID
WHERE a.ApplicationID=@ApplicationID 
AND ed.DocumentTypeID in (1,15)
";
                    break;
                #endregion

                #region SourceID=107 1	Вступительные испытания
                case 107:
                    sql = @"
SELECT DocumentID as ID, Name FROM DocumentType where IsMedical = 1

SELECT ed.EntrantDocumentID, ed.DocumentTypeID, ed.DocumentSeries, ed.DocumentNumber, ed.DocumentDate
,ed.DocumentName as DocumentTypeName
, ed.OlympApproved
FROM EntrantDocument ed (NOLOCK)
INNER JOIN DocumentType dt (NOLOCK) on dt.DocumentId=ed.DocumentTypeID
INNER JOIN [Application] a (NOLOCK) on a.EntrantID=ed.EntrantID
WHERE a.ApplicationID=@ApplicationID 
AND dt.IsMedical =1
";
                    break;
                    #endregion


            }
            #endregion

            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("ApplicationID", ApplicationID));
                com.Parameters.Add(new SqlParameter("EntranceTestItemID", SqlDbType.Int) { Value = (EntranceTestItemID == null) ? ((object)DBNull.Value) : EntranceTestItemID.Value });
                com.Parameters.Add(new SqlParameter("SourceID", SourceID));
                com.Parameters.Add(new SqlParameter("GroupID", GroupID));
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

            SelectEntDocsList D = new SelectEntDocsList();
            if (ds.Tables.Count > 0)
            {
                for (int r = 0; r < ds.Tables[0].Rows.Count; r++)
                {
                    SelectDocumentType dt = new SelectDocumentType();
                    var R = ds.Tables[0].Rows[r];
                    dt.ID = (int)R["ID"];
                    dt.Name = R["Name"] as string;
                    D.DocumentTypes.Add(dt);
                }
                for (int r = 0; r < ds.Tables[1].Rows.Count; r++)
                {
                    SelectEntDoc d = new SelectEntDoc();
                    var R = ds.Tables[1].Rows[r];
                    d.EntrantDocumentID = R["EntrantDocumentID"] as int?;
                    d.EntrantDocumentTypeID = R["DocumentTypeID"] as int?;
                    d.DocumentTypeName = R["DocumentTypeName"] as string;
                    d.DocumentSeries = R["DocumentSeries"] as string;
                    d.DocumentNumber = R["DocumentNumber"] as string;
                    d.DocumentDate = R["DocumentDate"] as DateTime?;
                    d.OlympApproved = R["OlympApproved"] as bool?;
                    
                    D.EntrantDocuments.Add(d);
                }
            }
            return D;
        }

        public static EntrantTestItemDocument AppEntTestDocEGESave(EntrantTestItemDocument etid)
        {
            #region SQL
            string sql = @"
--DECLARE @ApplicationID int
--DECLARE @EntranceTestItemID	int 

DECLARE @CompetitiveGroupID int
DECLARE @AETD_ID int 
DECLARE @EntranceTestTypeID int 
DECLARE @SubjectID int 
DECLARE @SourceID	int
DECLARE @ResultValue decimal(7,4)
DECLARE @BenefitID int
SET @SourceID=1
SET @BenefitID=null


IF @EntranceTestItemID IS NOT NULL BEGIN
	SELECT @CompetitiveGroupID=CompetitiveGroupID, @EntranceTestTypeID=EntranceTestTypeID, @SubjectID=SubjectID
		FROM [dbo].[EntranceTestItemC] (NOLOCK) WHERE EntranceTestItemID=@EntranceTestItemID
END

SELECT @ResultValue = Value FROM EntrantDocumentEgeAndOlympicSubject (NOLOCK) WHERE  EntrantDocumentID=@EntrantDocumentID AND SubjectID=@SubjectID
IF @ResultValue IS NULL BEGIN
	SET @ResultValue=0
END

IF NOT EXISTS(SELECT ID FROM ApplicationEntrantDocument (NOLOCK) WHERE ApplicationID=@ApplicationID  AND EntrantDocumentID=@EntrantDocumentID) BEGIN
	INSERT INTO ApplicationEntrantDocument(ApplicationID, EntrantDocumentID)VALUES(@ApplicationID, @EntrantDocumentID)
END

DELETE FROM ApplicationEntranceTestDocument WHERE ApplicationID=@ApplicationID AND EntranceTestItemID=@EntranceTestItemID
INSERT INTO [dbo].[ApplicationEntranceTestDocument]
           ( ApplicationID,  CompetitiveGroupID,  EntranceTestItemID,  EntranceTestTypeID,  SourceID,  SubjectID,  BenefitID,  EntrantDocumentID,  ResultValue)
     VALUES(@ApplicationID, @CompetitiveGroupID, @EntranceTestItemID, @EntranceTestTypeID, @SourceID, @SubjectID, @BenefitID, @EntrantDocumentID, @ResultValue)
SELECT  @AETD_ID=cast(scope_identity() as int);

SELECT  @AETD_ID as AETD_ID
";
            #endregion
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("ApplicationID", etid.ApplicationID));
                com.Parameters.Add(new SqlParameter("EntranceTestItemID", etid.EntranceTestItemID));
                com.Parameters.Add(new SqlParameter("EntrantDocumentID", etid.EntrantDocumentID));
                try
                {
                    con.Open();
                    SqlDataReader r = com.ExecuteReader();
                    if (r.Read())
                    {
                        etid.ID = (int)r["AETD_ID"];
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
            return etid;
        }
        #endregion

        public static int Wz3Save(AppResultsModel model)
        {
            int c = 0;
            string sql =
                "update Application " +
                "set WizardStepID = @WizardStepID, IsDisabled = @IsDisabled, IsDistant = @IsDistant, " +
                "IsDisabledDocumentID = @IsDisabledDocumentID, DistantPlace = @DistantPlace " +
                "where ApplicationId = @ApplicationID";

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("ApplicationID", SqlDbType.Int) { Value = model.ApplicationID });
                com.Parameters.Add(new SqlParameter("WizardStepID", SqlDbType.Int) { Value = model.Step });
                com.Parameters.Add(new SqlParameter("IsDisabled", SqlDbType.Bit) { Value = model.IsDisabled });
                com.Parameters.Add(new SqlParameter("IsDistant", SqlDbType.Bit) { Value = model.IsDistant });
                com.Parameters.Add(new SqlParameter("IsDisabledDocumentID", SqlDbType.Int) { Value = model.IsDisabledDocumentID == null ? (object)DBNull.Value : model.IsDisabledDocumentID });
                com.Parameters.Add(new SqlParameter("DistantPlace", SqlDbType.VarChar) { Value = model.DistantPlace == null ? (object)DBNull.Value : model.DistantPlace });
                try
                {
                    con.Open();
                    c = com.ExecuteNonQuery();
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
            return c;
        }

        //========================================================================================================

        #region Получить сочинения
        public static List<AppComposition> GetComposition(int ApplicationID)
        {
            string sql = @"
SELECT acr.ApplicationID, acr.Year AS acrYear, 
ISNULL(ct.Name, '') AS acrName, acr.Result AS acrResult, 
acr.ExamDate, acr.HasAppeal, acr.AppealStatus, acr.DownloadDate, acr.CompositionPaths 
FROM ApplicationCompositionResults acr (NOLOCK)	
LEFT JOIN CompositionThemes ct (NOLOCK) ON acr.ThemeID = ct.ThemeID and ct.[Year] = YEAR(acr.[Year])
WHERE acr.ApplicationID = @ApplicationID
";

            DataSet ds = new DataSet();
            List<AppComposition> CompositionResult = new List<AppComposition>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("ApplicationID", ApplicationID));
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
            for (int r = 0; r < ds.Tables[0].Rows.Count; r++)
            {
                var R = ds.Tables[0].Rows[r];
                CompositionResult.Add(new AppComposition()
                {
                    Year = R["acrYear"] as DateTime?,
                    acrName = R["acrName"] as string,
                    acrResult = (bool)R["acrResult"],
                    ExamDate = R["ExamDate"] as DateTime?,
                    HasAppeal = R["HasAppeal"] as bool?,
                    AppealStatus = R["AppealStatus"] as string,
                    CompositionPaths = R["CompositionPaths"] as string,
                    DownloadDate = R["DownloadDate"] as DateTime?,
                });
            }
            return CompositionResult;
        }
        #endregion


        #region Поиск и обновления результатов сочинений
        /// <summary>
        /// FIS-204 Алгоритм поиска и обновления результатов сочинений.
        /// </summary>
        /// <param name="ApplicationID"></param>
        /// <returns></returns>

        public static SPResult GetCompositionResults(int ApplicationID)
        {
            SPResult result = null;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand("FindEntrantCompositionMarks", con);

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@app", SqlDbType.Int) { Value = ApplicationID });
                    command.Parameters.Add(new SqlParameter("@returnValue", SqlDbType.Int) { Direction = ParameterDirection.ReturnValue });
                    //SqlParameter returnValue = new SqlParameter("@returnValue", SqlDbType.Int)
                    //{
                    //    Direction = ParameterDirection.Output
                    //};
                    //command.Parameters.Add(returnValue);

                    con.Open();
                    command.ExecuteNonQuery();

                    result = new SPResult
                    {
                        returnValue = Convert.ToBoolean(command.Parameters["@returnValue"].Value as int?),
                        //errorMessage = command.Parameters["@errorMessage"].Value as string,
                        //violationMessage = command.Parameters["@violationMessage"].Value as string,
                        //violationId = Convert.ToInt32(command.Parameters["@violationId"].Value as int?)
                    };

                    //result = Convert.ToBoolean(returnValue.Value as int?);
                    con.Close();
                }
                catch (SqlException e)
                {
                    throw e;	// Пробросить дальше
                }
                catch (Exception e)
                {
                    throw e;	// Пробросить дальше
                }
                finally
                {
                    if (con.State != ConnectionState.Closed) { con.Close(); }
                }
            }
            return result;
        }
        #endregion




    }
}