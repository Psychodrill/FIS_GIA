using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.Objects.SqlClient;
using System.Xml;
using System.Xml.Linq;
using GVUZ.ServiceModel.Import.Package;

namespace GVUZ.ServiceModel.SQL {

	public partial class InstitutionExporter {

		private XElement Application_EntranceTestResults(int ApplicationID) {
			#region
			/*
InstitutionExports\InstitutionExport\Applications\Application\EntranceTestResults
InstitutionExports\InstitutionExport\Applications\Application\EntranceTestResults\EntranceTestResult
InstitutionExports\InstitutionExport\Applications\Application\EntranceTestResults\EntranceTestResult\UID
InstitutionExports\InstitutionExport\Applications\Application\EntranceTestResults\EntranceTestResult\EntranceTestsResultID
InstitutionExports\InstitutionExport\Applications\Application\EntranceTestResults\EntranceTestResult\ResultValue
InstitutionExports\InstitutionExport\Applications\Application\EntranceTestResults\EntranceTestResult\ResultSourceTypeID
InstitutionExports\InstitutionExport\Applications\Application\EntranceTestResults\EntranceTestResult\EntranceTestSubject
InstitutionExports\InstitutionExport\Applications\Application\EntranceTestResults\EntranceTestResult\EntranceTestSubject\SubjectID
InstitutionExports\InstitutionExport\Applications\Application\EntranceTestResults\EntranceTestResult\EntranceTestSubject\SubjectName
InstitutionExports\InstitutionExport\Applications\Application\EntranceTestResults\EntranceTestResult\EntranceTestTypeID
InstitutionExports\InstitutionExport\Applications\Application\EntranceTestResults\EntranceTestResult\CompetitiveGroupID
*/
/* 
InstitutionExports\InstitutionExport\Applications\Application\EntranceTestResults\EntranceTestResult\ResultDocument
InstitutionExports\InstitutionExport\Applications\Application\EntranceTestResults\EntranceTestResult\ResultDocument\OlympicDocument
InstitutionExports\InstitutionExport\Applications\Application\EntranceTestResults\EntranceTestResult\ResultDocument\OlympicDocument\UID
InstitutionExports\InstitutionExport\Applications\Application\EntranceTestResults\EntranceTestResult\ResultDocument\OlympicDocument\OriginalReceived
InstitutionExports\InstitutionExport\Applications\Application\EntranceTestResults\EntranceTestResult\ResultDocument\OlympicDocument\OriginalReceivedDate
InstitutionExports\InstitutionExport\Applications\Application\EntranceTestResults\EntranceTestResult\ResultDocument\OlympicDocument\DocumentSeries
InstitutionExports\InstitutionExport\Applications\Application\EntranceTestResults\EntranceTestResult\ResultDocument\OlympicDocument\DocumentNumber
InstitutionExports\InstitutionExport\Applications\Application\EntranceTestResults\EntranceTestResult\ResultDocument\OlympicDocument\DocumenTDate
InstitutionExports\InstitutionExport\Applications\Application\EntranceTestResults\EntranceTestResult\ResultDocument\OlympicDocument\DiplomaTypeID
InstitutionExports\InstitutionExport\Applications\Application\EntranceTestResults\EntranceTestResult\ResultDocument\OlympicDocument\OlympicID
InstitutionExports\InstitutionExport\Applications\Application\EntranceTestResults\EntranceTestResult\ResultDocument\OlympicDocument\LevelID
*/
/* 
InstitutionExports\InstitutionExport\Applications\Application\EntranceTestResults\EntranceTestResult\ResultDocument\OlympicTotalDocument
InstitutionExports\InstitutionExport\Applications\Application\EntranceTestResults\EntranceTestResult\ResultDocument\OlympicTotalDocument\UID
InstitutionExports\InstitutionExport\Applications\Application\EntranceTestResults\EntranceTestResult\ResultDocument\OlympicTotalDocument\OriginalReceived
InstitutionExports\InstitutionExport\Applications\Application\EntranceTestResults\EntranceTestResult\ResultDocument\OlympicTotalDocument\OriginalReceivedDate
InstitutionExports\InstitutionExport\Applications\Application\EntranceTestResults\EntranceTestResult\ResultDocument\OlympicTotalDocument\DocumentSeries
InstitutionExports\InstitutionExport\Applications\Application\EntranceTestResults\EntranceTestResult\ResultDocument\OlympicTotalDocument\DocumentNumber
InstitutionExports\InstitutionExport\Applications\Application\EntranceTestResults\EntranceTestResult\ResultDocument\OlympicTotalDocument\OlympicPlace
InstitutionExports\InstitutionExport\Applications\Application\EntranceTestResults\EntranceTestResult\ResultDocument\OlympicTotalDocument\OlympicDate
InstitutionExports\InstitutionExport\Applications\Application\EntranceTestResults\EntranceTestResult\ResultDocument\OlympicTotalDocument\DiplomaTypeID
InstitutionExports\InstitutionExport\Applications\Application\EntranceTestResults\EntranceTestResult\ResultDocument\OlympicTotalDocument\Subjects
InstitutionExports\InstitutionExport\Applications\Application\EntranceTestResults\EntranceTestResult\ResultDocument\OlympicTotalDocument\Subjects\SubjectBriefData
InstitutionExports\InstitutionExport\Applications\Application\EntranceTestResults\EntranceTestResult\ResultDocument\OlympicTotalDocument\Subjects\SubjectBriefData\SubjectID
*/
/* 
InstitutionExports\InstitutionExport\Applications\Application\EntranceTestResults\EntranceTestResult\ResultDocument\InstitutionDocument
InstitutionExports\InstitutionExport\Applications\Application\EntranceTestResults\EntranceTestResult\ResultDocument\InstitutionDocument\DocumentNumber
InstitutionExports\InstitutionExport\Applications\Application\EntranceTestResults\EntranceTestResult\ResultDocument\InstitutionDocument\DocumenTDate
InstitutionExports\InstitutionExport\Applications\Application\EntranceTestResults\EntranceTestResult\ResultDocument\InstitutionDocument\DocumentTypeID
*/
/*
InstitutionExports\InstitutionExport\Applications\Application\EntranceTestResults\EntranceTestResult\ResultDocument\EgeDocumentID
 -- это ссылка на идентификатор докумена-основания из ApplicationEntranceTestDocument.EntrantDocumentID
 -- при условии, что связный документ из EntrantDocument имеет тип DocumentTypeId =2
*/
			#endregion

			var xEntranceTestResults=new XElement("EntranceTestResults");
			#region sql
			string sql= @"
-- declare @ApplicationID int = 1720139;

SELECT AETD.UID, AETD.ID as EntranceTestsResultID, AETD.ResultValue, AETD.SourceID as ResultSourceTypeID,
AETD.SubjectID as SubjectID,
--ETIC.SubjectID as SubjectID,
ETIC.SubjectName as SubjectName,
AETD.EntranceTestTypeID, AETD.CompetitiveGroupID,
AETD.EntrantDocumentID,
ED.DocumentTypeID,
(CASE WHEN ED.DocumentTypeID=2 THEN  ED.EntrantDocumentID ELSE NULL END) as EgeDocumentID,
CAST( (CASE WHEN AED.OriginalReceivedDate IS NOT NULL THEN 1 ELSE 0 END) as bit) as OriginalReceived,
AED.OriginalReceivedDate as OriginalReceivedDate,
ed.DocumentSeries, ed.DocumentNumber, ed.DocumentDate, 
edo.DiplomaTypeID as edo_DiplomaTypeID, edo.OlympicID as edo_OlympicID, null as edo_LevelID,
--edot.DiplomaTypeID as edot_DiplomaTypeID, edot.OlympicDate as edot_OlympicDate, edot.OlympicPlace as edot_OlympicPlace,
(SELECT CAST(sub.SubjectID as varchar(10)) +',' FROM EntrantDocumentEgeAndOlympicSubject  (NOLOCK)  edeaos
	INNER JOIN [Subject]  (NOLOCK)  sub on edeaos.SubjectID=sub.SubjectID
	WHERE edeaos.EntrantDocumentID=edo.EntrantDocumentID AND edeaos.SubjectID IS NOT NULL for xml path('')) as edot_Subjects,
AETD.InstitutionDocumentNumber, 
AETD.InstitutionDocumentDate, 
AETD.InstitutionDocumentTypeID -- SourceID=2
FROM ApplicationEntranceTestDocument (NOLOCK)  AETD 
--LEFT OUTER JOIN Subject (NOLOCK)  S on S.SubjectID=AETD.SubjectID
LEFT OUTER JOIN ApplicationEntrantDocument (NOLOCK)  AED on AED.ApplicationID=AETD.ApplicationID AND AED.EntrantDocumentID=AETD.EntrantDocumentID
LEFT OUTER JOIN EntrantDocument (NOLOCK)  ED ON ED.EntrantDocumentID=AETD.EntrantDocumentID
LEFT OUTER JOIN EntrantDocumentOlympic (NOLOCK)  edo on AETD.EntrantDocumentID=edo.EntrantDocumentID
--LEFT OUTER JOIN EntrantDocumentOlympicTotal (NOLOCK)  edot on AETD.EntrantDocumentID=edot.EntrantDocumentID
LEFT OUTER JOIN EntranceTestItemC (NOLOCK)  ETIC on ETIC.EntranceTestItemID=AETD.EntranceTestItemID
WHERE AETD.ApplicationID=@ApplicationID
";
			#endregion

			var com=getSqlCommand(sql);
			com.Parameters.Add(new SqlParameter("ApplicationID",SqlDbType.Int) { Value=ApplicationID });
			DataRow r=null;
			DataSet ds=new DataSet();
			try {
				SqlDataAdapter da=new SqlDataAdapter(com);
				da.Fill(ds);
			} catch(Exception) {
			} finally {
			}
			try {
				for(int i=0;i<ds.Tables[0].Rows.Count;i++) {
					r=ds.Tables[0].Rows[i];

					XElement xEntranceTestResult=new XElement("EntranceTestResult");
					xEntranceTestResult.AddX("UID",r["UID"]);
					//xEntranceTestResult.Add(new XElement("UID",O2Str(r["UID"])));
					xEntranceTestResult.AddX("EntranceTestsResultID",r["EntranceTestsResultID"]);
					xEntranceTestResult.AddX("ResultValue",r["ResultValue"]);
					xEntranceTestResult.AddX("ResultSourceTypeID",r["ResultSourceTypeID"]);
					var xEntranceTestSubject=new XElement("EntranceTestSubject");

					xEntranceTestSubject.AddX("SubjectID",r["SubjectID"]);
					//xEntranceTestSubject.Add(new XElement("SubjectID",O2Str(r["SubjectID"])));
					if(r["SubjectID"]==DBNull.Value) {
						xEntranceTestSubject.AddX("SubjectName",r["SubjectName"]);
						//xEntranceTestSubject.Add(new XElement("SubjectName",O2Str(r["SubjectName"])));
					}
					xEntranceTestResult.Add(xEntranceTestSubject);
					xEntranceTestResult.AddX("EntranceTestTypeID",r["EntranceTestTypeID"]);
					xEntranceTestResult.AddX("CompetitiveGroupID",r["CompetitiveGroupID"]);
					xEntranceTestResult.AddX("ResultSourceTypeID",r["ResultSourceTypeID"]);
					int? sourceid=r["ResultSourceTypeID"] as int?;
					XElement xResultDocument=new XElement("ResultDocument");
					if(sourceid==2) {
						XElement xInstitutionDocument=new XElement("InstitutionDocument");
						xInstitutionDocument.AddX("DocumentNumber",r["InstitutionDocumentNumber"]);
						xInstitutionDocument.AddX("DocumentDate",r["InstitutionDocumentDate"]);
						xInstitutionDocument.AddX("DocumentTypeID",r["InstitutionDocumentTypeID"]);
						xResultDocument.Add(xInstitutionDocument);
					} else {
						int? doctypeid=r["DocumentTypeID"] as int?;
						if(doctypeid==9 || doctypeid == 10) { // 9	Диплом победителя/призера олимпиады школьников
							XElement xOlympicDocument=new XElement("OlympicDocument");
							//xOlympicDocument.Add(new XElement("UID",O2Str(r["UID"])));
							xOlympicDocument.AddX("UID",r["UID"]);
							xOlympicDocument.AddX("OriginalReceived",r["OriginalReceived"]);
							xOlympicDocument.AddX("OriginalReceivedDate",r["OriginalReceivedDate"]);
							xOlympicDocument.AddX("DocumentSeries",r["DocumentSeries"]);
							xOlympicDocument.AddX("DocumentNumber",r["DocumentNumber"]);
							xOlympicDocument.AddX("DocumentDate",r["DocumentDate"]);
							xOlympicDocument.AddX("DiplomaTypeID",r["edo_DiplomaTypeID"]);
							xOlympicDocument.AddX("OlympicID",r["edo_OlympicID"]);
							xOlympicDocument.AddX("LevelID",r["edo_LevelID"]);
							xResultDocument.Add(xOlympicDocument);
						}
						//if(doctypeid==10) { // 10	Диплом победителя/призера всероссийской олимпиады школьников
						//	XElement xOlympicTotalDocument=new XElement("OlympicTotalDocument");
						//	//xOlympicTotalDocument.Add(new XElement("UID",O2Str(r["UID"])));
						//	xOlympicTotalDocument.AddX("UID",r["UID"]);
						//	xOlympicTotalDocument.AddX("OriginalReceived",r["OriginalReceived"]);
						//	xOlympicTotalDocument.AddX("OriginalReceivedDate",r["OriginalReceivedDate"]);
						//	xOlympicTotalDocument.AddX("DocumentSeries", r["DocumentSeries"]);
						//	xOlympicTotalDocument.AddX("DocumentNumber", r["DocumentNumber"]);
						//	xOlympicTotalDocument.AddX("DocumentDate", r["DocumentDate"]);
						//	xOlympicTotalDocument.AddX("DiplomaTypeID", r["edot_DiplomaTypeID"]);

						//	//xOlympicTotalDocument.AddX("OlympicDate",r["edot_OlympicDate"]);
						//	//xOlympicTotalDocument.AddX("OlympicPlace",r["edot_OlympicPlace"]);

						//	var Subjects=O2Str(r["edot_Subjects"]).Split(',');
						//	XElement xSubjects=new XElement("Subjects");
						//	foreach(var s in Subjects) {
						//		if(String.IsNullOrEmpty(s)) { continue; }
						//		XElement xSubjectBriefData=new XElement("SubjectBriefData");
						//		xSubjectBriefData.Add(new XElement("SubjectID",s));
						//		xSubjects.Add(xSubjectBriefData);
						//	}
						//	xOlympicTotalDocument.Add(xSubjects);

						//	xResultDocument.Add(xOlympicTotalDocument);
						//}
					}
					xEntranceTestResult.Add(xResultDocument);
					xEntranceTestResults.Add(xEntranceTestResult);
				}			
			} catch(Exception) { 
			}
			return xEntranceTestResults;
		}	

	}
}
