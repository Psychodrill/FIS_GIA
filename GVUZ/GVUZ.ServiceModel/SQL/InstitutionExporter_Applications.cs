using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.Objects.SqlClient;
using System.Xml;
using System.Xml.Linq;
using GVUZ.Helper.Import;
using GVUZ.ServiceModel.Import.Package;

namespace GVUZ.ServiceModel.SQL {
	public partial class InstitutionExporter {
		private XElement Applications() {
			var xApplications=new XElement("Applications");
			if(_filter.Applications!=null) {
				foreach(var f in _filter.Applications) { 
					var xApplicationList=ApplicationList(f);
					foreach(var xa in xApplicationList){
						xApplications.Add(xa);					
					}
				}
			}
			return xApplications;
		}

	    private List<XElement> ApplicationList(StatusOrUid f)
	    {
	        List<XElement> xApplicationList = new List<XElement>();
	        if (f.StatusID == null && f.UID == null)
	        {
	            return xApplicationList;
	        }
	        List<int> ApplicationIDs = new List<int>();
	        string sql = @"
SELECT ApplicationID 
FROM [Application] (NOLOCK)  A 
WHERE A.InstitutionID=@InstitutionID AND (A.UID=@ApplicationUID OR @ApplicationUID IS NULL) AND (A.StatusID=@StatusID OR @StatusID IS NULL)
";
	        var com = getSqlCommand(sql);
	        com.Parameters.Add(new SqlParameter("InstitutionID", SqlDbType.Int) {Value = this._institutionID});
	        com.Parameters.Add(new SqlParameter("ApplicationUID", SqlDbType.VarChar)
	        {
	            Value = f.UID ?? (object) DBNull.Value
	        });
	        com.Parameters.Add(new SqlParameter("StatusID", SqlDbType.Int) {Value = f.StatusID ?? (object) DBNull.Value});
	        SqlDataReader r = null;
	        try
	        {
	            r = com.ExecuteReader();
	            while (r.Read())
	            {
	                ApplicationIDs.Add((int) r["ApplicationID"]);
	            }
	        }
	        catch (Exception)
	        {
	        }
	        finally
	        {
	            if (r != null)
	            {
	                r.Close();
	            }
	        }

	        if (ApplicationIDs.Count > 10)
	        {
	            throw new InstitutionExporterException(string.Format(
	                "Невозможно за один вызов выгрузить {0} заявлений по причине большого объема данных, рекомендуем запрашивать заявления по одному с указанием UID",
	                ApplicationIDs.Count));
	        }
	        else
	        {
	            foreach (var id in ApplicationIDs)
	            {
	                var xApplication = Application(id);
	                xApplicationList.Add(xApplication);
	            }
	        }
	        return xApplicationList;
	    }

	    private XElement Application(int ApplicationID) {
			#region SQL
			string sql= @"
--declare @applicationid int = 1720133;

SELECT A.ApplicationID, A.UID, A.ApplicationNumber, A.RegistrationDate, A.LastDenyDate, A.NeedHostel, A.StatusID,
E.UID as E_UID, E.CustomInformation as E_CustomInformation, E.FirstName as E_FirstName, E.LastName as E_LastName, E.MiddleName as E_MiddleName, E.GenderID as E_GenderID, E.Snils as E_Snils
FROM [Application] (NOLOCK)  A 
INNER JOIN Entrant E (NOLOCK)  ON A.EntrantID=E.EntrantID
WHERE ApplicationID=@ApplicationID

SELECT ACGI.EducationSourceId as FinanceSourceID, ACGI.EducationFormID, CGT.UID as TargetOrganizationUID
FROM ApplicationCompetitiveGroupItem (NOLOCK)   ACGI
LEFT OUTER JOIN CompetitiveGroupTarget (NOLOCK)  CGT on ACGI.CompetitiveGroupTargetId=CGT.CompetitiveGroupTargetId
WHERE ACGI.ApplicationID=@ApplicationID

SELECT IA.IAUID, IA.IAName, InsA.UID as InstitutionAchievementUID, IA.IAMark, ED.UID as IADocumentUID, isAdvantageRight
FROM IndividualAchivement (NOLOCK)  IA 
LEFT OUTER JOIN EntrantDocument (NOLOCK)  ED on ED.EntrantDocumentID=IA.EntrantDocumentID
LEFT OUTER JOIN InstitutionAchievements (NOLOCK)  InsA on InsA.IdAchievement=IA.IdAchievement
WHERE ApplicationID=@ApplicationID

SELECT DISTINCT ACGI.CompetitiveGroupID
FROM ApplicationCompetitiveGroupItem (NOLOCK)   ACGI
WHERE ApplicationID=@ApplicationID 

";
			#endregion

			var xApplication=new XElement("Application");
			var com=getSqlCommand(sql);
			com.Parameters.Add(new SqlParameter("ApplicationID",SqlDbType.Int) { Value=ApplicationID });

			DataRow r=null;
			try {
				// com.Connection.Open должен быть открыт перед загрузкой всего пакета
				SqlDataAdapter da=new SqlDataAdapter(com);
				DataSet ds=new DataSet();
				da.Fill(ds);
				#region Application Entrant
				/*
InstitutionExports\InstitutionExport\Applications\Application
InstitutionExports\InstitutionExport\Applications\Application\UID
InstitutionExports\InstitutionExport\Applications\Application\ApplicationNumber
InstitutionExports\InstitutionExport\Applications\Application\RegistrationDate
InstitutionExports\InstitutionExport\Applications\Application\LastDenyDate
InstitutionExports\InstitutionExport\Applications\Application\NeedHostel
InstitutionExports\InstitutionExport\Applications\Application\StatusID

InstitutionExports\InstitutionExport\Applications\Application\Entrant
InstitutionExports\InstitutionExport\Applications\Application\Entrant\UID
InstitutionExports\InstitutionExport\Applications\Application\Entrant\CustomInformation
InstitutionExports\InstitutionExport\Applications\Application\Entrant\FirstName
InstitutionExports\InstitutionExport\Applications\Application\Entrant\LastName
InstitutionExports\InstitutionExport\Applications\Application\Entrant\MiddleName
InstitutionExports\InstitutionExport\Applications\Application\Entrant\GenderID 
InstitutionExports\InstitutionExport\Applications\Application\Entrant\Snils
*/
				for(int i=0;i<ds.Tables[0].Rows.Count;i++) {
					r=ds.Tables[0].Rows[i];
					xApplication.Add(new XElement("UID",O2Str(r["UID"])));
					xApplication.Add(new XElement("ApplicationNumber",Str2Str(r["ApplicationNumber"])));
					xApplication.Add(new XElement("RegistrationDate",Date2Str(r["RegistrationDate"]))); // <AdditionalSet xsi:nil="true" /> ????
					xApplication.Add(new XElement("LastDenyDate",Date2Str(r["LastDenyDate"])));
					xApplication.Add(new XElement("NeedHostel",Bool2Str(r["NeedHostel"])));
					xApplication.Add(new XElement("StatusID",O2Str(r["StatusID"])));

					XElement xEntrant=new XElement("Entrant");
					xEntrant.Add(new XElement("UID",O2Str(r["E_UID"])));
					xEntrant.Add(new XElement("CustomInformation",O2Str(r["E_CustomInformation"])));
					xEntrant.Add(new XElement("FirstName",O2Str(r["E_FirstName"])));
					xEntrant.Add(new XElement("LastName",O2Str(r["E_LastName"])));
					xEntrant.Add(new XElement("MiddleName",O2Str(r["E_MiddleName"])));
					xEntrant.Add(new XElement("GenderID",O2Str(r["E_GenderID"])));
					xEntrant.Add(new XElement("Snils",O2Str(r["E_Snils"])));
					xApplication.Add(xEntrant);
				}
				#endregion

				#region FinSourceAndEduForms
			//InstitutionExports\InstitutionExport\Applications\Application\FinSourceAndEduForms
			//InstitutionExports\InstitutionExport\Applications\Application\FinSourceAndEduForms\FinSourceEduForm
			//InstitutionExports\InstitutionExport\Applications\Application\FinSourceAndEduForms\FinSourceEduForm\FinanceSourceID
			//InstitutionExports\InstitutionExport\Applications\Application\FinSourceAndEduForms\FinSourceEduForm\EducationFormID
			//InstitutionExports\InstitutionExport\Applications\Application\FinSourceAndEduForms\FinSourceEduForm\TargetOrganizationUID
				var xFinSourceAndEduForms=new XElement("FinSourceAndEduForms");
				for(int i=0;i<ds.Tables[1].Rows.Count;i++) {
					r=ds.Tables[1].Rows[i];
					var xFinSourceEduForm=new XElement("FinSourceEduForm");
					xFinSourceEduForm.Add(new XElement("FinanceSourceID",O2Str(r["FinanceSourceID"])));
					xFinSourceEduForm.Add(new XElement("EducationFormID",O2Str(r["EducationFormID"])));
					xFinSourceEduForm.Add(new XElement("TargetOrganizationUID",O2Str(r["TargetOrganizationUID"])));

					xFinSourceAndEduForms.Add(xFinSourceEduForm);
				}
				xApplication.Add(xFinSourceAndEduForms);				
				#endregion

				#region IndividualAchievements
				/*
--InstitutionExports\InstitutionExport\Applications\Application\IndividualAchievements
--InstitutionExports\InstitutionExport\Applications\Application\IndividualAchievements\IndividualAchievement
--InstitutionExports\InstitutionExport\Applications\Application\IndividualAchievements\IndividualAchievement\IAUID
--InstitutionExports\InstitutionExport\Applications\Application\IndividualAchievements\IndividualAchievement\IAName
--InstitutionExports\InstitutionExport\Applications\Application\IndividualAchievements\IndividualAchievement\InstitutionAchievementUID
--InstitutionExports\InstitutionExport\Applications\Application\IndividualAchievements\IndividualAchievement\IAMark
--InstitutionExports\InstitutionExport\Applications\Application\IndividualAchievements\IndividualAchievement\IADocumentUID
*/
				var xIndividualAchievements=new XElement("IndividualAchievements");
				for(int i=0;i<ds.Tables[2].Rows.Count;i++) {
					r=ds.Tables[2].Rows[i];
					var xIndividualAchievement=new XElement("IndividualAchievement");
					xIndividualAchievement.Add(new XElement("IAUID",O2Str(r["IAUID"])));
					xIndividualAchievement.Add(new XElement("IAName",O2Str(r["IAName"])));
					xIndividualAchievement.Add(new XElement("InstitutionAchievementUID",O2Str(r["InstitutionAchievementUID"])));
					xIndividualAchievement.Add(new XElement("IAMark",O2Str(r["IAMark"])));
					xIndividualAchievement.Add(new XElement("IADocumentUID",O2Str(r["IADocumentUID"])));
                    xIndividualAchievement.Add(new XElement("isAdvantageRight", O2Str(r["isAdvantageRight"])));
                    xIndividualAchievements.Add(xIndividualAchievement);
				}
				xApplication.Add(xIndividualAchievements);
				#endregion

//				#region SelectedCompetitiveGroupItems
//				/*
//--InstitutionExports\InstitutionExport\Applications\Application\SelectedCompetitiveGroupItems
//--InstitutionExports\InstitutionExport\Applications\Application\SelectedCompetitiveGroupItems\CompetitiveGroupItemID
//*/
//				var xSelectedCompetitiveGroupItems=new XElement("SelectedCompetitiveGroupItems");
//				for(int i=0;i<ds.Tables[3].Rows.Count;i++) {
//					r=ds.Tables[3].Rows[i];
//					xSelectedCompetitiveGroupItems.Add(new XElement("CompetitiveGroupItemID",O2Str(r["CompetitiveGroupItemID"])));
//				}
//				xApplication.Add(xSelectedCompetitiveGroupItems);
//				#endregion

				#region SelectedCompetitiveGroups
				/*
--InstitutionExports\InstitutionExport\Applications\Application\SelectedCompetitiveGroups
--InstitutionExports\InstitutionExport\Applications\Application\SelectedCompetitiveGroups\CompetitiveGroupID
*/
				var xSelectedCompetitiveGroups=new XElement("SelectedCompetitiveGroups");
				for(int i=0;i<ds.Tables[3].Rows.Count;i++) {
					r=ds.Tables[3].Rows[i];
					xSelectedCompetitiveGroups.Add(new XElement("CompetitiveGroupID",O2Str(r["CompetitiveGroupID"])));
				}
				xApplication.Add(xSelectedCompetitiveGroups);

				#endregion
			} catch(Exception) {
			} finally {
			}

			var xApplicationCommonBenefits=ApplicationCommonBenefits(ApplicationID);
			xApplication.Add(xApplicationCommonBenefits);

			var xApplicationDocuments=ApplicationDocuments(ApplicationID);
			xApplication.Add(xApplicationDocuments);

			var xEntranceTestResults=Application_EntranceTestResults(ApplicationID);
			xApplication.Add(xEntranceTestResults);

			return xApplication;
		}

		private XElement ApplicationCommonBenefits(int ApplicationID) {
			#region SQL
			string sql= @"
SELECT  AETD.ID as ApplicationCommonBenefitID, AETD.BenefitID as BenefitKindID, AETD.CompetitiveGroupID, ED.DocumentTypeID as DocumentTypeID
FROM ApplicationEntranceTestDocument AETD  (NOLOCK)
LEFT OUTER JOIN Benefit B (NOLOCK) on AETD.BenefitID=B.BenefitID
LEFT OUTER JOIN EntrantDocument ED (NOLOCK) on ED.EntrantDocumentID=AETD.EntrantDocumentID
WHERE AETD.ApplicationID=@ApplicationID AND AETD.EntranceTestItemID is null

SELECT AETD.ID as ApplicationCommonBenefitID,
  ed.UID, 
	CAST( (CASE WHEN AED.OriginalReceivedDate IS NOT NULL THEN 1 ELSE 0 END) as bit) as OriginalReceived,
	AED.OriginalReceivedDate as OriginalReceivedDate,
  ed.DocumentSeries, ed.DocumentNumber, ed.DocumentDate, edo.DiplomaTypeID, edo.OlympicID, null as LevelID
FROM ApplicationEntranceTestDocument AETD  (NOLOCK)
INNER JOIN EntrantDocument ed (NOLOCK) on AETD.EntrantDocumentID=ed.EntrantDocumentID
INNER JOIN EntrantDocumentOlympic edo (NOLOCK) on AETD.EntrantDocumentID=edo.EntrantDocumentID
INNER JOIN ApplicationEntrantDocument AED (NOLOCK) on AED.ApplicationID=@ApplicationID AND AED.EntrantDocumentID=AETD.EntrantDocumentID
WHERE AETD.ApplicationID=@ApplicationID AND AETD.EntranceTestItemID is null AND AETD.SubjectID is NULL
ORDER BY AETD.ID

/*
SELECT AETD.ID as ApplicationCommonBenefitID, 
ed.UID,  
CAST( (CASE WHEN AED.OriginalReceivedDate IS NOT NULL THEN 1 ELSE 0 END) as bit) as OriginalReceived,
AED.OriginalReceivedDate as OriginalReceivedDate,
ed.DocumentSeries, ed.DocumentNumber, ed.DocumentDate, ed.DocumentOrganization,
edot.DiplomaTypeID, edot.OlympicDate, edot.OlympicPlace
, (SELECT CAST(sub.SubjectID as varchar(10)) +',' FROM EntrantDocumentEgeAndOlympicSubject edeaos (NOLOCK)
	INNER JOIN [Subject] sub (NOLOCK) on edeaos.SubjectID=sub.SubjectID
	WHERE edeaos.EntrantDocumentID=edot.EntrantDocumentID AND edeaos.SubjectID IS NOT NULL for xml path('')) as Subjects
, ede.GPA
FROM ApplicationEntranceTestDocument AETD (NOLOCK) 
INNER JOIN EntrantDocument ed (NOLOCK) on AETD.EntrantDocumentID=ed.EntrantDocumentID
INNER JOIN EntrantDocumentOlympicTotal edot (NOLOCK) on AETD.EntrantDocumentID=edot.EntrantDocumentID
INNER JOIN ApplicationEntrantDocument AED (NOLOCK) on AED.ApplicationID=@ApplicationID AND AED.EntrantDocumentID=AETD.EntrantDocumentID
LEFT OUTER JOIN EntrantDocumentEdu ede (NOLOCK) on EDE.EntrantDocumentID=ED.EntrantDocumentID
WHERE AETD.ApplicationID=@ApplicationID AND AETD.EntranceTestItemID is null AND AETD.SubjectID is NULL
ORDER BY AETD.ID
*/

SELECT AETD.ID as ApplicationCommonBenefitID, 
ed.UID,  
CAST( (CASE WHEN AED.OriginalReceivedDate IS NOT NULL THEN 1 ELSE 0 END) as bit) as OriginalReceived,
AED.OriginalReceivedDate as OriginalReceivedDate,
ed.DocumentSeries, ed.DocumentNumber, ed.DocumentDate, ed.DocumentOrganization,
EDD.DisabilityTypeID
FROM ApplicationEntranceTestDocument AETD  (NOLOCK)
INNER JOIN EntrantDocument ed (NOLOCK) on AETD.EntrantDocumentID=ed.EntrantDocumentID
INNER JOIN ApplicationEntrantDocument AED (NOLOCK) on AED.ApplicationID=@ApplicationID AND AED.EntrantDocumentID=AETD.EntrantDocumentID
INNER JOIN EntrantDocumentDisability EDD (NOLOCK) on EDD.EntrantDocumentID=ed.EntrantDocumentID
WHERE AETD.ApplicationID=@ApplicationID AND AETD.EntranceTestItemID is null AND AETD.SubjectID is NULL
AND ed.DocumentTypeID=11  -- 11	Справка об установлении инвалидности
ORDER BY AETD.ID

SELECT AETD.ID as ApplicationCommonBenefitID, 
ed.UID,  
CAST( (CASE WHEN AED.OriginalReceivedDate IS NOT NULL THEN 1 ELSE 0 END) as bit) as OriginalReceived,
AED.OriginalReceivedDate as OriginalReceivedDate,
ed.DocumentSeries, ed.DocumentNumber, ed.DocumentDate, ed.DocumentOrganization
FROM ApplicationEntranceTestDocument AETD  (NOLOCK)
INNER JOIN EntrantDocument ed (NOLOCK) on AETD.EntrantDocumentID=ed.EntrantDocumentID
INNER JOIN ApplicationEntrantDocument AED (NOLOCK) on AED.ApplicationID=@ApplicationID AND AED.EntrantDocumentID=AETD.EntrantDocumentID
WHERE AETD.ApplicationID=@ApplicationID AND AETD.EntranceTestItemID is null AND AETD.SubjectID is NULL
AND ed.DocumentTypeID=12 --12	Заключение психолого-медико-педагогической комиссии
ORDER BY AETD.ID

SELECT AETD.ID as ApplicationCommonBenefitID, 
ed.UID,  
CAST( (CASE WHEN AED.OriginalReceivedDate IS NOT NULL THEN 1 ELSE 0 END) as bit) as OriginalReceived,
AED.OriginalReceivedDate as OriginalReceivedDate,
ed.DocumentSeries, ed.DocumentNumber, ed.DocumentDate, ed.DocumentOrganization
FROM ApplicationEntranceTestDocument AETD  (NOLOCK)
INNER JOIN EntrantDocument ed (NOLOCK) on AETD.EntrantDocumentID=ed.EntrantDocumentID
INNER JOIN ApplicationEntrantDocument AED (NOLOCK) on AED.ApplicationID=@ApplicationID AND AED.EntrantDocumentID=AETD.EntrantDocumentID
WHERE AETD.ApplicationID=@ApplicationID AND AETD.EntranceTestItemID is null AND AETD.SubjectID is NULL
AND ed.DocumentTypeID=13 -- 13	Заключение об отсутствии противопоказаний для обучения
ORDER BY AETD.ID 

SELECT AETD.ID as ApplicationCommonBenefitID, 
ed.UID,  
CAST( (CASE WHEN AED.OriginalReceivedDate IS NOT NULL THEN 1 ELSE 0 END) as bit) as OriginalReceived,
AED.OriginalReceivedDate as OriginalReceivedDate,
ed.DocumentSeries, ed.DocumentNumber, ed.DocumentDate, ed.DocumentOrganization,
EDC.AdditionalInfo, EDC.DocumentTypeNameText
FROM ApplicationEntranceTestDocument AETD  (NOLOCK)
INNER JOIN EntrantDocument ed  (NOLOCK) on AETD.EntrantDocumentID=ed.EntrantDocumentID
INNER JOIN ApplicationEntrantDocument AED (NOLOCK) on AED.ApplicationID=@ApplicationID AND AED.EntrantDocumentID=AETD.EntrantDocumentID
INNER JOIN EntrantDocumentCustom EDC (NOLOCK) on EDC.EntrantDocumentID=ed.EntrantDocumentID
WHERE AETD.ApplicationID=@ApplicationID AND AETD.EntranceTestItemID is null AND AETD.SubjectID is NULL
AND ed.DocumentTypeID=15 -- 15	Иной документ
ORDER BY AETD.ID

";
			#endregion 
			var xApplicationCommonBenefits=new XElement("ApplicationCommonBenefits");
			var com=getSqlCommand(sql);
			com.Parameters.Add(new SqlParameter("ApplicationID",SqlDbType.Int) { Value=ApplicationID });

			DataRow r=null;
			try {
			
				// com.Connection.Open должен быть открыт перед загрузкой всего пакета
				SqlDataAdapter da=new SqlDataAdapter(com);
				DataSet ds=new DataSet();
				da.Fill(ds);

				#region ApplicationCommonBenefit
/*
InstitutionExports\InstitutionExport\Applications\Application\ApplicationCommonBenefits
InstitutionExports\InstitutionExport\Applications\Application\ApplicationCommonBenefits\ApplicationCommonBenefit
InstitutionExports\InstitutionExport\Applications\Application\ApplicationCommonBenefits\ApplicationCommonBenefit\ApplicationCommonBenefitID
InstitutionExports\InstitutionExport\Applications\Application\ApplicationCommonBenefits\ApplicationCommonBenefit\BenefitKindID
InstitutionExports\InstitutionExport\Applications\Application\ApplicationCommonBenefits\ApplicationCommonBenefit\CompetitiveGroupID
InstitutionExports\InstitutionExport\Applications\Application\ApplicationCommonBenefits\ApplicationCommonBenefit\DocumentTypeID
*/
				for(int i=0;i<ds.Tables[0].Rows.Count;i++) {
					r=ds.Tables[0].Rows[i];
					var xApplicationCommonBenefit=new XElement("ApplicationCommonBenefit");
					xApplicationCommonBenefit.Add(new XElement("ApplicationCommonBenefitID",O2Str(r["ApplicationCommonBenefitID"])));
					xApplicationCommonBenefit.Add(new XElement("BenefitKindID",O2Str(r["BenefitKindID"])));
					xApplicationCommonBenefit.Add(new XElement("CompetitiveGroupID",O2Str(r["CompetitiveGroupID"])));
					xApplicationCommonBenefit.Add(new XElement("DocumentTypeID",O2Str(r["DocumentTypeID"])));					

					int ApplicationCommonBenefitID=(int)r["ApplicationCommonBenefitID"];
					var xDocumentReason=xApplicationCommonBenefit_DocumentReason(ds.Tables[1],ds.Tables[2],ds.Tables[3],ds.Tables[4],ds.Tables[5], ApplicationCommonBenefitID);

					xApplicationCommonBenefit.Add(xDocumentReason);
					xApplicationCommonBenefits.Add(xApplicationCommonBenefit);
				}
				#endregion

			} catch(Exception) {
			} finally {
			}
			return xApplicationCommonBenefits;
		}

		private XElement xApplicationCommonBenefit_DocumentReason(DataTable odT,DataTable meddisT,DataTable medT,DataTable medallT, DataTable custT, int ApplicationCommonBenefitID) {
			var xDocumentReason=new XElement("DocumentReason");

			#region OlympicDocument
			/*
			InstitutionExports\InstitutionExport\Applications\Application\ApplicationCommonBenefits\ApplicationCommonBenefit\DocumentReason
			InstitutionExports\InstitutionExport\Applications\Application\ApplicationCommonBenefits\ApplicationCommonBenefit\DocumentReason\OlympicDocument
			InstitutionExports\InstitutionExport\Applications\Application\ApplicationCommonBenefits\ApplicationCommonBenefit\DocumentReason\OlympicDocument\UID
			InstitutionExports\InstitutionExport\Applications\Application\ApplicationCommonBenefits\ApplicationCommonBenefit\DocumentReason\OlympicDocument\OriginalReceived
			InstitutionExports\InstitutionExport\Applications\Application\ApplicationCommonBenefits\ApplicationCommonBenefit\DocumentReason\OlympicDocument\OriginalReceivedDate
			InstitutionExports\InstitutionExport\Applications\Application\ApplicationCommonBenefits\ApplicationCommonBenefit\DocumentReason\OlympicDocument\DocumentSeries
			InstitutionExports\InstitutionExport\Applications\Application\ApplicationCommonBenefits\ApplicationCommonBenefit\DocumentReason\OlympicDocument\DocumentNumber
			InstitutionExports\InstitutionExport\Applications\Application\ApplicationCommonBenefits\ApplicationCommonBenefit\DocumentReason\OlympicDocument\DocumenTDate
			InstitutionExports\InstitutionExport\Applications\Application\ApplicationCommonBenefits\ApplicationCommonBenefit\DocumentReason\OlympicDocument\DiplomaTypeID
			InstitutionExports\InstitutionExport\Applications\Application\ApplicationCommonBenefits\ApplicationCommonBenefit\DocumentReason\OlympicDocument\OlympicID
			InstitutionExports\InstitutionExport\Applications\Application\ApplicationCommonBenefits\ApplicationCommonBenefit\DocumentReason\OlympicDocument\LevelID
			*/
			bool found=false;
			int id=0;
			DataRow r;
			found=false;
			for(int i=0;i<odT.Rows.Count;i++) {
				r=odT.Rows[i];
				id=(int)r["ApplicationCommonBenefitID"];
				if(id==ApplicationCommonBenefitID) {
					found=true;
					XElement xOlympicDocument=new XElement("OlympicDocument");
					xOlympicDocument.Add(new XElement("UID",O2Str(r["UID"])));
					xOlympicDocument.Add(new XElement("OriginalReceived",Bool2Str(r["OriginalReceived"])));
					xOlympicDocument.Add(new XElement("OriginalReceivedDate",Date2Str(r["OriginalReceivedDate"])));
					xOlympicDocument.Add(new XElement("DocumentSeries",O2Str(r["DocumentSeries"])));
					xOlympicDocument.Add(new XElement("DocumentNumber",O2Str(r["DocumentNumber"])));
					xOlympicDocument.Add(new XElement("DocumentDate",Date2Str(r["DocumentDate"])));
					xOlympicDocument.Add(new XElement("DiplomaTypeID",O2Str(r["DiplomaTypeID"])));
					xOlympicDocument.Add(new XElement("OlympicID",O2Str(r["OlympicID"])));
					xOlympicDocument.Add(new XElement("LevelID",O2Str(r["LevelID"])));
					xDocumentReason.Add(xOlympicDocument);
				} else {
					if(found) { break; }
				}
			}
			#endregion

			#region OlympicTotalDocument
			/*
			InstitutionExports\InstitutionExport\Applications\Application\ApplicationCommonBenefits\ApplicationCommonBenefit\DocumentReason\OlympicTotalDocument
			InstitutionExports\InstitutionExport\Applications\Application\ApplicationCommonBenefits\ApplicationCommonBenefit\DocumentReason\OlympicTotalDocument\UID
			InstitutionExports\InstitutionExport\Applications\Application\ApplicationCommonBenefits\ApplicationCommonBenefit\DocumentReason\OlympicTotalDocument\DocumentSeries
			InstitutionExports\InstitutionExport\Applications\Application\ApplicationCommonBenefits\ApplicationCommonBenefit\DocumentReason\OlympicTotalDocument\DocumentNumber
			InstitutionExports\InstitutionExport\Applications\Application\ApplicationCommonBenefits\ApplicationCommonBenefit\DocumentReason\OlympicTotalDocument\DocumentDate
			InstitutionExports\InstitutionExport\Applications\Application\ApplicationCommonBenefits\ApplicationCommonBenefit\DocumentReason\OlympicTotalDocument\DocumentOrganization
			InstitutionExports\InstitutionExport\Applications\Application\ApplicationCommonBenefits\ApplicationCommonBenefit\DocumentReason\OlympicTotalDocument\DiplomaTypeID

			InstitutionExports\InstitutionExport\Applications\Application\ApplicationCommonBenefits\ApplicationCommonBenefit\DocumentReason\OlympicTotalDocument\OlympicDate
			InstitutionExports\InstitutionExport\Applications\Application\ApplicationCommonBenefits\ApplicationCommonBenefit\DocumentReason\OlympicTotalDocument\OlympicPlace
			InstitutionExports\InstitutionExport\Applications\Application\ApplicationCommonBenefits\ApplicationCommonBenefit\DocumentReason\OlympicTotalDocument\Subjects
			InstitutionExports\InstitutionExport\Applications\Application\ApplicationCommonBenefits\ApplicationCommonBenefit\DocumentReason\OlympicTotalDocument\Subjects\SubjectBriefData
			InstitutionExports\InstitutionExport\Applications\Application\ApplicationCommonBenefits\ApplicationCommonBenefit\DocumentReason\OlympicTotalDocument\Subjects\SubjectBriefData\SubjectID
			InstitutionExports\InstitutionExport\Applications\Application\ApplicationCommonBenefits\ApplicationCommonBenefit\DocumentReason\OlympicTotalDocument\GPA
			*/
			//found=false;
			//for(int i=0;i<otdT.Rows.Count;i++) {
			//	r=otdT.Rows[i];
			//	id=(int)r["ApplicationCommonBenefitID"];
			//	if(id==ApplicationCommonBenefitID) {
			//		found=true;
			//		XElement xOlympicTotalDocument=new XElement("OlympicTotalDocument");
			//		xOlympicTotalDocument.Add(new XElement("UID",O2Str(r["UID"])));
			//		//					xOlympicTotalDocument.Add(new XElement("OriginalReceived",Bool2Str(r["OriginalReceived"])));
			//		//					xOlympicTotalDocument.Add(new XElement("OriginalReceivedDate",Date2Str(r["OriginalReceivedDate"])));
			//		xOlympicTotalDocument.Add(new XElement("DocumentSeries",O2Str(r["DocumentSeries"])));
			//		xOlympicTotalDocument.Add(new XElement("DocumentNumber",O2Str(r["DocumentNumber"])));
			//		xOlympicTotalDocument.Add(new XElement("DocumentDate",Date2Str(r["DocumentDate"])));
			//		xOlympicTotalDocument.Add(new XElement("DocumentOrganization",O2Str(r["DocumentOrganization"])));
			//		xOlympicTotalDocument.Add(new XElement("DiplomaTypeID",O2Str(r["DiplomaTypeID"])));

			//		xOlympicTotalDocument.Add(new XElement("OlympicDate",Date2Str(r["OlympicDate"])));
			//		xOlympicTotalDocument.Add(new XElement("OlympicPlace",O2Str(r["OlympicPlace"])));

			//		var Subjects=O2Str(r["Subjects"]).Split(',');
			//		XElement xSubjects=new XElement("Subjects");
			//		foreach(var s in Subjects) {
			//			if(String.IsNullOrEmpty(s)) { continue; }
			//			XElement xSubjectBriefData=new XElement("SubjectBriefData");
			//			xSubjectBriefData.Add(new XElement("SubjectID",s));
			//			xSubjects.Add(xSubjectBriefData);
			//		}
			//		xOlympicTotalDocument.Add(xSubjects);
			//		xDocumentReason.Add(xOlympicTotalDocument);
			//	} else {
			//		if(found) { break; }
			//	}
			//}
			#endregion


			#region MedicalDocuments

			XElement xMedicalDocuments=new XElement("MedicalDocuments");
			#region BenefitDocument
			/*
\DocumentReason\MedicalDocuments
\DocumentReason\MedicalDocuments\BenefitDocument
\DocumentReason\MedicalDocuments\BenefitDocument\DisabilityDocument
\DocumentReason\MedicalDocuments\BenefitDocument\DisabilityDocument\UID
\DocumentReason\MedicalDocuments\BenefitDocument\DisabilityDocument\OriginalReceived
\DocumentReason\MedicalDocuments\BenefitDocument\DisabilityDocument\OriginalReceivedDate
\DocumentReason\MedicalDocuments\BenefitDocument\DisabilityDocument\DocumentSeries
\DocumentReason\MedicalDocuments\BenefitDocument\DisabilityDocument\DocumentNumber
\DocumentReason\MedicalDocuments\BenefitDocument\DisabilityDocument\DocumenTDate
\DocumentReason\MedicalDocuments\BenefitDocument\DisabilityDocument\DocumentOrganization
\DocumentReason\MedicalDocuments\BenefitDocument\DisabilityDocument\DisabilityTypeID
			*/

			XElement xBenefitDocument=new XElement("BenefitDocument");
			found=false;
			for(int i=0;i<meddisT.Rows.Count;i++) {
				r=meddisT.Rows[i];
				id=(int)r["ApplicationCommonBenefitID"];
				if(id==ApplicationCommonBenefitID) {
					found=true;
					XElement xDisabilityDocument=new XElement("DisabilityDocument");
					xDisabilityDocument.Add(new XElement("UID",O2Str(r["UID"])));
					xDisabilityDocument.Add(new XElement("OriginalReceived",Bool2Str(r["OriginalReceived"])));
					xDisabilityDocument.Add(new XElement("OriginalReceivedDate",Date2Str(r["OriginalReceivedDate"])));
					xDisabilityDocument.Add(new XElement("DocumentSeries",O2Str(r["DocumentSeries"])));
					xDisabilityDocument.Add(new XElement("DocumentNumber",O2Str(r["DocumentNumber"])));
					xDisabilityDocument.Add(new XElement("DocumentDate",Date2Str(r["DocumentDate"])));
					xDisabilityDocument.Add(new XElement("DocumentOrganization",O2Str(r["DocumentOrganization"])));
					xDisabilityDocument.Add(new XElement("DisabilityTypeID",O2Str(r["DisabilityTypeID"])));
					xBenefitDocument.Add(xDisabilityDocument);
				} else {
					if(found) { break; }
				}
			}
/*
\DocumentReason\MedicalDocuments\BenefitDocument\MedicalDocument
\DocumentReason\MedicalDocuments\BenefitDocument\MedicalDocument\UID
\DocumentReason\MedicalDocuments\BenefitDocument\MedicalDocument\OriginalReceived
\DocumentReason\MedicalDocuments\BenefitDocument\MedicalDocument\OriginalReceivedDate
\DocumentReason\MedicalDocuments\BenefitDocument\MedicalDocument\DocumentNumber
\DocumentReason\MedicalDocuments\BenefitDocument\MedicalDocument\DocumenTDate
\DocumentReason\MedicalDocuments\BenefitDocument\MedicalDocument\DocumentOrganization
*/
			found=false;
			for(int i=0;i<medT.Rows.Count;i++) {
				r=medT.Rows[i];
				id=(int)r["ApplicationCommonBenefitID"];
				if(id==ApplicationCommonBenefitID) {
					found=true;
					XElement xMedicalDocument=new XElement("MedicalDocument");
					xMedicalDocument.Add(new XElement("UID",O2Str(r["UID"])));
					xMedicalDocument.Add(new XElement("OriginalReceived",Bool2Str(r["OriginalReceived"])));
					xMedicalDocument.Add(new XElement("OriginalReceivedDate",Date2Str(r["OriginalReceivedDate"])));
					xMedicalDocument.Add(new XElement("DocumentNumber",O2Str(r["DocumentNumber"])));
					xMedicalDocument.Add(new XElement("DocumentDate",Date2Str(r["DocumentDate"])));
					xMedicalDocument.Add(new XElement("DocumentOrganization",O2Str(r["DocumentOrganization"])));
					xBenefitDocument.Add(xMedicalDocument);
				} else {
					if(found) { break; }
				}
			}


			xMedicalDocuments.Add(xBenefitDocument);
			#endregion

			#region AllowEducationDocument
			/*
InstitutionExports\InstitutionExport\Applications\Application\ApplicationCommonBenefits\ApplicationCommonBenefit\DocumentReason\MedicalDocuments\AllowEducationDocument
InstitutionExports\InstitutionExport\Applications\Application\ApplicationCommonBenefits\ApplicationCommonBenefit\DocumentReason\MedicalDocuments\AllowEducationDocument\UID
InstitutionExports\InstitutionExport\Applications\Application\ApplicationCommonBenefits\ApplicationCommonBenefit\DocumentReason\MedicalDocuments\AllowEducationDocument\OriginalReceived
InstitutionExports\InstitutionExport\Applications\Application\ApplicationCommonBenefits\ApplicationCommonBenefit\DocumentReason\MedicalDocuments\AllowEducationDocument\OriginalReceivedDate
InstitutionExports\InstitutionExport\Applications\Application\ApplicationCommonBenefits\ApplicationCommonBenefit\DocumentReason\MedicalDocuments\AllowEducationDocument\DocumentNumber
InstitutionExports\InstitutionExport\Applications\Application\ApplicationCommonBenefits\ApplicationCommonBenefit\DocumentReason\MedicalDocuments\AllowEducationDocument\DocumenTDate
InstitutionExports\InstitutionExport\Applications\Application\ApplicationCommonBenefits\ApplicationCommonBenefit\DocumentReason\MedicalDocuments\AllowEducationDocument\DocumentOrganization
*/

			XElement xAllowEducationDocument=new XElement("AllowEducationDocument");
			found=false;
			for(int i=0;i<medallT.Rows.Count;i++) {
				r=medallT.Rows[i];
				id=(int)r["ApplicationCommonBenefitID"];
				if(id==ApplicationCommonBenefitID) {
					found=true;
					XElement xDisabilityDocument=new XElement("DisabilityDocument");
					xDisabilityDocument.Add(new XElement("UID",O2Str(r["UID"])));
					xDisabilityDocument.Add(new XElement("OriginalReceived",Bool2Str(r["OriginalReceived"])));
					xDisabilityDocument.Add(new XElement("OriginalReceivedDate",Date2Str(r["OriginalReceivedDate"])));
					xDisabilityDocument.Add(new XElement("DocumentSeries",O2Str(r["DocumentSeries"])));
					xDisabilityDocument.Add(new XElement("DocumentNumber",O2Str(r["DocumentNumber"])));
					xDisabilityDocument.Add(new XElement("DocumentDate",Date2Str(r["DocumentDate"])));
					xDisabilityDocument.Add(new XElement("DocumentOrganization",O2Str(r["DocumentOrganization"])));
					xDisabilityDocument.Add(new XElement("DisabilityTypeID",O2Str(r["DisabilityTypeID"])));
					xBenefitDocument.Add(xDisabilityDocument);
				} else {
					if(found) { break; }
				}
			}
			xMedicalDocuments.Add(xAllowEducationDocument);
			#endregion

			xDocumentReason.Add(xMedicalDocuments);

			#region CustomDocument
/*
\DocumentReason\CustomDocument
\DocumentReason\CustomDocument\UID
\DocumentReason\CustomDocument\OriginalReceived
\DocumentReason\CustomDocument\OriginalReceivedDate
\DocumentReason\CustomDocument\DocumentSeries
\DocumentReason\CustomDocument\DocumentNumber
\DocumentReason\CustomDocument\DocumenTDate
\DocumentReason\CustomDocument\DocumentOrganization
\DocumentReason\CustomDocument\AdditionalInfo
\DocumentReason\CustomDocument\DocumentTypeNameText
*/
			found=false;
			for(int i=0;i<custT.Rows.Count;i++) {
				r=custT.Rows[i];
				id=(int)r["ApplicationCommonBenefitID"];
				if(id==ApplicationCommonBenefitID) {
					found=true;
					XElement xCustomDocument=new XElement("CustomDocument");
					xCustomDocument.Add(new XElement("UID",O2Str(r["UID"])));
					xCustomDocument.Add(new XElement("OriginalReceived",Bool2Str(r["OriginalReceived"])));
					xCustomDocument.Add(new XElement("OriginalReceivedDate",Date2Str(r["OriginalReceivedDate"])));
					xCustomDocument.Add(new XElement("DocumentSeries",O2Str(r["DocumentSeries"])));
					xCustomDocument.Add(new XElement("DocumentNumber",O2Str(r["DocumentNumber"])));
					xCustomDocument.Add(new XElement("DocumentDate",Date2Str(r["DocumentDate"])));
					xCustomDocument.Add(new XElement("DocumentOrganization",O2Str(r["DocumentOrganization"])));
					xCustomDocument.Add(new XElement("AdditionalInfo",O2Str(r["AdditionalInfo"])));
					xCustomDocument.Add(new XElement("DocumentTypeNameText",O2Str(r["DocumentTypeNameText"])));
					xDocumentReason.Add(xCustomDocument);
				} else {
					if(found) { break; }
				}
			}

			#endregion

			#endregion
			return xDocumentReason;
		}
	}
}
