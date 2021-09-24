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

		private XElement ApplicationDocuments(int ApplicationID) {
			
			#region 
/*
1	Документ, удостоверяющий личность
2	Свидетельство о результатах ЕГЭ
3	Аттестат о среднем (полном) общем образовании
4	Диплом о высшем профессиональном образовании
5	Диплом о среднем профессиональном образовании
6	Диплом о начальном профессиональном образовании
7	Диплом о неполном высшем профессиональном образовании
8	Академическая справка
9	Диплом победителя/призера олимпиады школьников
10	Диплом победителя/призера всероссийской олимпиады школьников
11	Справка об установлении инвалидности
12	Заключение психолого-медико-педагогической комиссии
13	Заключение об отсутствии противопоказаний для обучения
14	Военный билет
15	Иной документ
16	Аттестат об основном общем образовании
17	Справка ГИА
18	Справка об обучении в другом ВУЗе
19	Иной документ об образовании
20	Диплом чемпиона/призера Олимпийских игр
21	Диплом чемпиона/призера Паралимпийских игр
22	Диплом чемпиона/призера Сурдлимпийских игр
23	Диплом чемпиона мира
24	Диплом чемпиона Европы
25	Диплом об окончании аспирантуры (адъюнкатуры)
26	Диплом кандидата наук

*/
			#endregion
			#region SQL
			string sql= @"
--declare @ApplicationID int = 1720139;

SELECT 
CAST( (CASE WHEN E.IdentityDocumentID=ED.EntrantDocumentID THEN 1 ELSE 0 END)  as bit) as isIdentity, 
ED.EntrantDocumentID, ED.UID, ED.DocumentTypeID, ED.DocumentSeries, ED.DocumentNumber, ED.DocumentDate, ED.DocumentOrganization,

CAST( (CASE WHEN AED.OriginalReceivedDate IS NOT NULL THEN 1 ELSE 0 END) as bit) as OriginalReceived,
AED.OriginalReceivedDate as OriginalReceivedDate

, ege.TypographicNumber as ege_TypographicNumber

, (SELECT CAST(sub.SubjectID as varchar(10)) +'|'+ sub.Name +'|'+ ISNULL(CAST( [Value] as varchar(50)),'') +'|'
	FROM EntrantDocumentEgeAndOlympicSubject (NOLOCK) edeaos	
	INNER JOIN [Subject] (NOLOCK)  sub on edeaos.SubjectID=sub.SubjectID
	WHERE edeaos.EntrantDocumentID=ed.EntrantDocumentID 
	FOR XML PATH('')
	) as edsb_SubjectBalls
,edd.DisabilityTypeID as dis_DisabilityTypeID
,edd_dt.Name as dis_DisabilityTypeName
,edc.DocumentTypeNameText as edc_DocumentTypeNameText
,edc.AdditionalInfo as edc_AdditionalInfo
,edo.OlympicID as edo_OlympicID
,edo.DiplomaTypeID as edo_DiplomaTypeID
,odt.Name as edo_DiplomaTypeName
	
,edo_SubjectsName=( 
		SELECT s.Name+', ' 
		FROM Subject (NOLOCK)  s
		WHERE s.SubjectID = os.SubjectID FOR XML PATH(''))

,otp.OrganizerName as edo_OrganizerName
,ot.Name as edo_OlympicName
,ot.OlympicYear as edo_OlympicYear
,ol.Name as edo_OlympicTypeLevel

,edo.DiplomaTypeID as edot_DiplomaTypeID
,edot_odt.Name as edot_DiplomaTypeName

--,otp.OlympicPlace as edot_OlympicPlace
--,edot.OlympicDate as edot_OlympicDate
,'' as edot_OlympicPlace
,'' as edot_OlympicDate

, (
SELECT CAST(sub.SubjectID as varchar(10)) +'|'+ sub.Name +'|' 
FROM EntrantDocumentEgeAndOlympicSubject (NOLOCK)  edeaos
INNER JOIN [Subject] (NOLOCK)  sub on edeaos.SubjectID=sub.SubjectID
WHERE edeaos.EntrantDocumentID = edo.EntrantDocumentID AND edeaos.SubjectID IS NOT NULL
  for xml path('')
) as edot_Subjects

,ede.RegistrationNumber as edu_RegistrationNumber
,ede.QualificationName as edu_QualificationName,	ede_Qualification.DirectionID as edu_QualificationID
,ede.SpecialityName as edu_SpecialityName,			ede_Speciality.DirectionID as edu_SpecialityID
,ede.GPA	as edu_GPA
--,ede.StateServicePreparation	as edu_StateServicePreparation
,ed.DocumentOrganization as edu_DocumentOU 

	,edi.IdentityDocumentTypeID as edi_IdentityDocumentTypeID
--	,idt.Name as edi_IdentityDocumentTypeName
	,edi.GenderTypeID as edi_GenderTypeID
--	,gt.Name as  edi_GenderTypeName
	,edi.NationalityTypeID as edi_NationalityTypeID
--	,nt.Name as edi_NationalityTypeName
	,edi.BirthDate as edi_BirthDate
	,edi.BirthPlace as edi_BirthPlace
	,edi.SubdivisionCode as edi_SubdivisionCode

FROM ApplicationEntrantDocument (NOLOCK)  AED 
INNER JOIN EntrantDocument (NOLOCK)  ED on  ED.EntrantDocumentID=AED.EntrantDocumentID
INNER JOIN Entrant (NOLOCK)  E on ED.EntrantID=E.EntrantID

LEFT OUTER JOIN EntrantDocumentIdentity (NOLOCK)  edi on ed.EntrantDocumentID=edi.EntrantDocumentID
--LEFT OUTER JOIN IdentityDocumentType (NOLOCK)  idt  on edi.IdentityDocumentTypeID=idt.IdentityDocumentTypeID
--LEFT OUTER JOIN GenderType (NOLOCK)  gt on edi.GenderTypeID=gt.GenderID
--LEFT OUTER JOIN NationalityType (NOLOCK)  nt on edi.NationalityTypeID=nt.NationalityID

LEFT OUTER JOIN EntrantDocumentEge (NOLOCK)  ege on ege.EntrantDocumentID=ed.EntrantDocumentID

LEFT OUTER JOIN EntrantDocumentDisability (NOLOCK)  edd on edd.EntrantDocumentID=ed.EntrantDocumentID
LEFT OUTER JOIN DisabilityType (NOLOCK)  edd_dt on edd_dt.DisabilityID=edd.DisabilityTypeID

LEFT OUTER JOIN EntrantDocumentCustom (NOLOCK)  edc on edc.EntrantDocumentID=ed.EntrantDocumentID

LEFT OUTER JOIN EntrantDocumentOlympic (NOLOCK)  edo on edo.EntrantDocumentID=ed.EntrantDocumentID
LEFT OUTER JOIN OlympicDiplomType (NOLOCK)  odt on odt.OlympicDiplomTypeID=edo.DiplomaTypeID	
LEFT OUTER JOIN OlympicType (NOLOCK)  ot on ot.OlympicID=edo.OlympicID 
LEFT OUTER JOIN OlympicTypeProfile otp (NOLOCK) on otp.OlympicTypeID = ot.OlympicID 
and (edo.OlympicTypeProfileID is null OR edo.OlympicTypeProfileID = otp.OlympicTypeProfileID)
LEFT OUTER JOIN OlympicLevel (NOLOCK)  ol on  ol.OlympicLevelID = otp.OlympicLevelID

--LEFT OUTER JOIN EntrantDocumentOlympicTotal (NOLOCK)  edot on edot.EntrantDocumentID=ed.EntrantDocumentID
LEFT OUTER JOIN OlympicDiplomType (NOLOCK)  edot_odt on edot_odt.OlympicDiplomTypeID=edo.DiplomaTypeID

LEFT OUTER JOIN OlympicSubject os (NOLOCK) on os.OlympicTypeProfileID = otp.OlympicTypeProfileID 
and (
	(edo.ProfileSubjectID is not null and edo.ProfileSubjectID = os.SubjectID) OR
	(edo.EgeSubjectID is not null and edo.EgeSubjectID = os.SubjectID)
)

LEFT OUTER JOIN EntrantDocumentEdu (NOLOCK)  ede on ed.EntrantDocumentID=ede.EntrantDocumentID
LEFT OUTER JOIN Direction (NOLOCK)  ede_Speciality on (isnull(ede_Speciality.NewCode,'') +'/'+ isnull(ede_Speciality.Code,'') + ' ' + ede_Speciality.Name)=ede.SpecialityName
LEFT OUTER JOIN Direction (NOLOCK)  ede_Qualification on ede_Qualification.QualificationName=ede.QualificationName
WHERE AED.ApplicationID=@ApplicationID

";
			#endregion

			var xApplicationDocuments=new XElement("ApplicationDocuments");

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

			XElement xIdentityDocument=new XElement("IdentityDocument");
			XElement xOtherIdentityDocuments=new XElement("OtherIdentityDocuments");
			XElement xEgeDocuments=new XElement("EgeDocuments");
			XElement xGiaDocuments=new XElement("GiaDocuments");
			XElement xEduDocuments=new XElement("EduDocuments");
			//XElement xMilitaryCardDocument=new XElement("MilitaryCardDocument");
			List<XElement> lMilitaryCardDocument=new List<XElement>();
			//XElement xStudentDocument=new XElement("StudentDocument");
			List<XElement> lStudentDocument=new List<XElement>();
			XElement xCustomDocuments=new XElement("CustomDocuments");
			#region
			int doctype=0;
			bool isIdentity=false;
			try {
				for(int i=0;i<ds.Tables[0].Rows.Count;i++) {
					r=ds.Tables[0].Rows[i];
					doctype=(int)r["DocumentTypeID"];
					isIdentity=(bool)r["isIdentity"];

					switch(doctype) { 					
						case 1 :	//1	Документ, удостоверяющий личность
							var xxIdentityDocument=ApplicationDocuments_IdentityDocument(r);
							if(isIdentity) {
								xIdentityDocument=xxIdentityDocument;
							} else {
								xOtherIdentityDocuments.Add(xxIdentityDocument);
							}
							break;
						case 2:  // 2	Свидетельство о результатах ЕГЭ
							xEgeDocuments.Add(ApplicationDocuments_EgeDocument(r));
							break;

						case 17:	// 17	Справка ГИА
							xGiaDocuments.Add(ApplicationDocuments_GiaDocument(r));						
							break;
						case 3: // 3	Аттестат о среднем (полном) общем образовании
							xEduDocuments.Add(new XElement("EduDocument", ApplicationDocuments_SchoolCertificateDocument(r)));
							break;
						case 16://	16	Аттестат об основном общем образовании
							xEduDocuments.Add(new XElement("EduDocument", ApplicationDocuments_SchoolCertificateBasicDocument(r)));
							break;
						case 4:// 4	Диплом о высшем профессиональном образовании
							xEduDocuments.Add(new XElement("EduDocument", ApplicationDocuments_HighEduDiplomaDocument(r)));
						break;
						case 25://	25	Диплом об окончании аспирантуры (адъюнкатуры)
							xEduDocuments.Add(new XElement("EduDocument", ApplicationDocuments_PostGraduateDiplomaDocument(r)));
						break;
						case 26://	26	Диплом кандидата наук
							xEduDocuments.Add(new XElement("EduDocument",ApplicationDocuments_PhDDiplomaDocument(r)));
						break;
						case 5://	5	Диплом о среднем профессиональном образовании
							xEduDocuments.Add(new XElement("EduDocument",ApplicationDocuments_MiddleEduDiplomaDocument(r)));
						break;
						case 6://	6	Диплом о начальном профессиональном образовании
							xEduDocuments.Add(new XElement("EduDocument",ApplicationDocuments_BasicDiplomaDocument(r)));
						break;
						case 7:// 7	Диплом о неполном высшем профессиональном образовании
							xEduDocuments.Add(new XElement("EduDocument",ApplicationDocuments_IncomplHighEduDiplomaDocument(r)));
						break;
						case 8:// 8	Академическая справка
							xEduDocuments.Add(new XElement("EduDocument",ApplicationDocuments_AcademicDiplomaDocument(r)));
						break;
						case 19:// 19	Иной документ об образовании
							xEduDocuments.Add(new XElement("EduDocument",ApplicationDocuments_EduCustomDocument(r)));
						break;
						case 14:// 14	Военный билет
							// xMilitaryCardDocument=ApplicationDocuments_MilitaryCardDocument(r);
							lMilitaryCardDocument.Add(ApplicationDocuments_MilitaryCardDocument(r));
						break;
						case 18:// 18	Справка об обучении в другом ВУЗе
							//xStudentDocument=ApplicationDocuments_StudentDocument(r);
							lStudentDocument.Add(ApplicationDocuments_StudentDocument(r));
						break;
						case 15:// 15	Иной документ
							xCustomDocuments.Add(ApplicationDocuments_CustomDocument(r));
						break;
					}
				}
			} catch(Exception) {
			}
			#endregion
			if(xIdentityDocument.Nodes().Count()>0) { xApplicationDocuments.Add(xIdentityDocument); }
			if(xOtherIdentityDocuments.Nodes().Count()>0) { xApplicationDocuments.Add(xOtherIdentityDocuments); }
			if(xEgeDocuments.Nodes().Count()>0) {	xApplicationDocuments.Add(xEgeDocuments);	}
			if(xGiaDocuments.Nodes().Count()>0) { xApplicationDocuments.Add(xGiaDocuments); }
			if(xEduDocuments.Nodes().Count()>0) { xApplicationDocuments.Add(xEduDocuments); }
			
			//if(xMilitaryCardDocument.Nodes().Count()>0) { xApplicationDocuments.Add(xMilitaryCardDocument); }
			foreach(var x in lMilitaryCardDocument) {		xApplicationDocuments.Add(x);	}

			//if(xStudentDocument.Nodes().Count()>0) { xApplicationDocuments.Add(xStudentDocument); }
			foreach(var x in lStudentDocument) { xApplicationDocuments.Add(x); }

			if(xCustomDocuments.Nodes().Count()>0) { xApplicationDocuments.Add(xCustomDocuments); }
			return xApplicationDocuments;
		}

		private XElement ApplicationDocuments_IdentityDocument(DataRow r) {
			XElement xIdentityDocument=new XElement("IdentityDocument");
			/*
			Applications\Application\ApplicationDocuments\IdentityDocument
			Applications\Application\ApplicationDocuments\IdentityDocument\UID
			Applications\Application\ApplicationDocuments\IdentityDocument\OriginalReceived
			Applications\Application\ApplicationDocuments\IdentityDocument\OriginalReceivedDate
			Applications\Application\ApplicationDocuments\IdentityDocument\DocumentSeries
			Applications\Application\ApplicationDocuments\IdentityDocument\DocumentNumber
			Applications\Application\ApplicationDocuments\IdentityDocument\SubdivisionCode
			Applications\Application\ApplicationDocuments\IdentityDocument\DocumentDate
			Applications\Application\ApplicationDocuments\IdentityDocument\DocumentOrganization
			Applications\Application\ApplicationDocuments\IdentityDocument\IdentityDocumentTypeID
			Applications\Application\ApplicationDocuments\IdentityDocument\NationalityTypeID
			Applications\Application\ApplicationDocuments\IdentityDocument\BirthDate
			Applications\Application\ApplicationDocuments\IdentityDocument\BirthPlace
			*/
			xIdentityDocument.AddX("UID",r["UID"]);
			xIdentityDocument.AddX("OriginalReceived",r["OriginalReceived"]);
			xIdentityDocument.AddX("OriginalReceivedDate",r["OriginalReceivedDate"]);
			xIdentityDocument.AddX("DocumentSeries",r["DocumentSeries"]);
			xIdentityDocument.AddX("DocumentNumber",r["DocumentNumber"]);

			xIdentityDocument.AddX("SubdivisionCode",r["edi_SubdivisionCode"]);
			xIdentityDocument.AddX("DocumentDate",r["DocumentDate"]);
			xIdentityDocument.AddX("DocumentOrganization",r["DocumentOrganization"]);
			xIdentityDocument.AddX("IdentityDocumentTypeID",r["edi_IdentityDocumentTypeID"]);
			xIdentityDocument.AddX("NationalityTypeID",r["edi_NationalityTypeID"]);
			xIdentityDocument.AddX("BirthDate",r["edi_BirthDate"]);
			xIdentityDocument.AddX("BirthPlace",r["edi_BirthPlace"]);
			
			return xIdentityDocument;
		}

		private XElement ApplicationDocuments_EgeDocument(DataRow r) {
			/*
			Applications\Application\ApplicationDocuments\EgeDocuments
			Applications\Application\ApplicationDocuments\EgeDocuments\EgeDocument
			Applications\Application\ApplicationDocuments\EgeDocuments\EgeDocument\UID
			Applications\Application\ApplicationDocuments\EgeDocuments\EgeDocument\OriginalReceived
			Applications\Application\ApplicationDocuments\EgeDocuments\EgeDocument\OriginalReceivedDate
			Applications\Application\ApplicationDocuments\EgeDocuments\EgeDocument\DocumentNumber
			Applications\Application\ApplicationDocuments\EgeDocuments\EgeDocument\DocumentDate
			Applications\Application\ApplicationDocuments\EgeDocuments\EgeDocument\DocumentYear
			Applications\Application\ApplicationDocuments\EgeDocuments\EgeDocument\Subjects
			Applications\Application\ApplicationDocuments\EgeDocuments\EgeDocument\Subjects\SubjectData
			Applications\Application\ApplicationDocuments\EgeDocuments\EgeDocument\Subjects\SubjectData\SubjectID
			Applications\Application\ApplicationDocuments\EgeDocuments\EgeDocument\Subjects\SubjectData\Value
			*/

			XElement xEgeDocument=new XElement("EgeDocument");
			xEgeDocument.AddX("UID",r["UID"]);
			xEgeDocument.AddX("OriginalReceived",r["OriginalReceived"]);
			xEgeDocument.AddX("OriginalReceivedDate",r["OriginalReceivedDate"]);
			xEgeDocument.AddX("DocumentNumber",r["DocumentNumber"]);
			xEgeDocument.AddX("DocumentDate",r["DocumentDate"]);
            var docDate = r["DocumentDate"] as DateTime?;
            if (docDate.HasValue)
                xEgeDocument.AddX("DocumentYear", docDate.Value.ToString("yyyy"));	//???

			string edsb_SubjectBalls=r["edsb_SubjectBalls"] as string;
			XElement xSubjects=new XElement("Subjects");
			// Парсим В строке через | то SubjectID то SubjectName
			if(!String.IsNullOrEmpty(edsb_SubjectBalls)) {
				string[] A=edsb_SubjectBalls.Split('|');
				int subjectID;
				string subjectName;
				int value=-1;
				int count=A.Count();
				try {
					for(int i=0;i<count;i++) {
						if(String.IsNullOrEmpty(A[i])) { break; }
						if(count<i+2) { break; }
						if(Int32.TryParse(A[i],out subjectID)) {
							subjectName=A[i+1];
							if(!Int32.TryParse(A[i+2],out value)) { value=-1; }
							XElement xSubjectData=new XElement("SubjectData");
							xSubjectData.Add(new XElement("SubjectID",subjectID.ToString()));
							xSubjectData.Add(new XElement("Value",(value>=0?value.ToString():"")));	// ????
							xSubjects.Add(xSubjectData);
							i+=2;
						}
					}
				} catch(Exception) {
				}
			}
			if(xSubjects.Nodes().Count()>0) { xEgeDocument.Add(xSubjects); }
			return xEgeDocument;
		}

		private XElement ApplicationDocuments_GiaDocument(DataRow r) {
			/*
			Applications\Application\ApplicationDocuments\GiaDocuments
			Applications\Application\ApplicationDocuments\GiaDocuments\GiaDocument
			Applications\Application\ApplicationDocuments\GiaDocuments\GiaDocument\UID
			Applications\Application\ApplicationDocuments\GiaDocuments\GiaDocument\OriginalReceived
			Applications\Application\ApplicationDocuments\GiaDocuments\GiaDocument\OriginalReceivedDate
			Applications\Application\ApplicationDocuments\GiaDocuments\GiaDocument\DocumentNumber
			Applications\Application\ApplicationDocuments\GiaDocuments\GiaDocument\DocumentDate
			Applications\Application\ApplicationDocuments\GiaDocuments\GiaDocument\DocumentOrganization
			Applications\Application\ApplicationDocuments\GiaDocuments\GiaDocument\Subjects
			Applications\Application\ApplicationDocuments\GiaDocuments\GiaDocument\Subjects\SubjectData
			Applications\Application\ApplicationDocuments\GiaDocuments\GiaDocument\Subjects\SubjectData\SubjectID
			Applications\Application\ApplicationDocuments\GiaDocuments\GiaDocument\Subjects\SubjectData\Value
			*/
			XElement xGiaDocument=new XElement("GiaDocument");

			xGiaDocument.AddX("UID",r["UID"]);
			xGiaDocument.AddX("OriginalReceived",r["OriginalReceived"]);
			xGiaDocument.AddX("OriginalReceivedDate",r["OriginalReceivedDate"]);
			xGiaDocument.AddX("DocumentNumber",r["DocumentNumber"]);
			xGiaDocument.AddX("DocumentDate",r["DocumentDate"]);
			xGiaDocument.AddX("DocumentOrganization",r["DocumentOrganization"]);

			string edsb_SubjectBalls=r["edsb_SubjectBalls"] as string;
			XElement xSubjects=new XElement("Subjects");
			// Парсим В строке через | то SubjectID то SubjectName
			if(!String.IsNullOrEmpty(edsb_SubjectBalls)) {
				string[] A=edsb_SubjectBalls.Split('|');
				int subjectID;
				string subjectName;
				int value=-1;
				int count=A.Count();
				try {
					for(int i=0;i<count;i++) {
						if(String.IsNullOrEmpty(A[i])) { break; }
						if(count<i+2) { break; }
						if(Int32.TryParse(A[i],out subjectID)) {
							subjectName=A[i+1];
							if(!Int32.TryParse(A[i+2],out value)) { value=-1; }
							XElement xSubjectData=new XElement("SubjectData");
							xSubjectData.Add(new XElement("SubjectID",subjectID.ToString()));
							xSubjectData.Add(new XElement("Value",(value>=0?value.ToString():"")));	// ????
							xSubjects.Add(xSubjectData);
							i+=2;
						}
					}
				} catch(Exception) {
				}
			}
			if(xSubjects.Nodes().Count()>0) { xGiaDocument.Add(xSubjects); }
			return xGiaDocument;
		}

		private XElement ApplicationDocuments_SchoolCertificateDocument(DataRow r) {
			// 3	Аттестат о среднем (полном) общем образовании
			/*
			Applications\Application\ApplicationDocuments\EduDocuments
			Applications\Application\ApplicationDocuments\EduDocuments\EduDocument
			Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\SchoolCertificateDocument
			Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\SchoolCertificateDocument\UID
			Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\SchoolCertificateDocument\OriginalReceived
			Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\SchoolCertificateDocument\OriginalReceivedDate
			Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\SchoolCertificateDocument\DocumentSeries
			Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\SchoolCertificateDocument\DocumentNumber
			Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\SchoolCertificateDocument\DocumenTDate
			Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\SchoolCertificateDocument\DocumentOrganization
			Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\SchoolCertificateDocument\EndYear
			*/
			XElement xSchoolCertificateDocument=new XElement("SchoolCertificateDocument");

			xSchoolCertificateDocument.AddX("UID",r["UID"]);
			xSchoolCertificateDocument.AddX("OriginalReceived",r["OriginalReceived"]);
			xSchoolCertificateDocument.AddX("OriginalReceivedDate",r["OriginalReceivedDate"]);
			xSchoolCertificateDocument.AddX("DocumentSeries",r["DocumentSeries"]);
			xSchoolCertificateDocument.AddX("DocumentNumber",r["DocumentNumber"]);
			xSchoolCertificateDocument.AddX("DocumentDate",r["DocumentDate"]);
			xSchoolCertificateDocument.AddX("DocumentOrganization",r["DocumentOrganization"]);
			//xSchoolCertificateDocument.AddX("EndYear",r["EndYear"]);	???
			return xSchoolCertificateDocument;
		}

		private XElement ApplicationDocuments_SchoolCertificateBasicDocument(DataRow r) {
			// 16	Аттестат об основном общем образовании
			/*
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\SchoolCertificateBasicDocument
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\SchoolCertificateBasicDocument\UID
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\SchoolCertificateBasicDocument\OriginalReceived
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\SchoolCertificateBasicDocument\OriginalReceivedDate
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\SchoolCertificateBasicDocument\DocumentSeries
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\SchoolCertificateBasicDocument\DocumentNumber
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\SchoolCertificateBasicDocument\DocumenTDate
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\SchoolCertificateBasicDocument\DocumentOrganization
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\SchoolCertificateBasicDocument\EndYear
			*/
			XElement xSchoolCertificateBasicDocument=new XElement("SchoolCertificateBasicDocument");

			xSchoolCertificateBasicDocument.AddX("UID",r["UID"]);
			xSchoolCertificateBasicDocument.AddX("OriginalReceived",r["OriginalReceived"]);	// ??? 0 или false
			xSchoolCertificateBasicDocument.AddX("OriginalReceivedDate",r["OriginalReceivedDate"]);
			xSchoolCertificateBasicDocument.AddX("DocumentSeries",r["DocumentSeries"]);
			xSchoolCertificateBasicDocument.AddX("DocumentNumber",r["DocumentNumber"]);
			xSchoolCertificateBasicDocument.AddX("DocumentDate",r["DocumentDate"]);
			xSchoolCertificateBasicDocument.AddX("DocumentOrganization",r["DocumentOrganization"]);
			//xSchoolCertificateDocument.AddX("EndYear",((DateTime)r["DocumentDate"]).ToString("yyyy")); // ???
			return xSchoolCertificateBasicDocument;
		}

		private XElement ApplicationDocuments_HighEduDiplomaDocument(DataRow r) {
			// 4	Диплом о высшем профессиональном образовании
			/*
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\HighEduDiplomaDocument
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\HighEduDiplomaDocument\UID
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\HighEduDiplomaDocument\OriginalReceived
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\HighEduDiplomaDocument\OriginalReceivedDate
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\HighEduDiplomaDocument\DocumentSeries
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\HighEduDiplomaDocument\DocumentNumber
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\HighEduDiplomaDocument\DocumenTDate
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\HighEduDiplomaDocument\DocumentOrganization
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\HighEduDiplomaDocument\RegistrationNumber
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\HighEduDiplomaDocument\QualificationTypeID
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\HighEduDiplomaDocument\SpecialityID
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\HighEduDiplomaDocument\EndYear
			*/
			XElement xHighEduDiplomaDocument=new XElement("HighEduDiplomaDocument");

			xHighEduDiplomaDocument.AddX("UID",r["UID"]);
			xHighEduDiplomaDocument.AddX("OriginalReceived",r["OriginalReceived"]);	// ??? 0 или false
			xHighEduDiplomaDocument.AddX("OriginalReceivedDate",r["OriginalReceivedDate"]);
			xHighEduDiplomaDocument.AddX("DocumentSeries",r["DocumentSeries"]);
			xHighEduDiplomaDocument.AddX("DocumentNumber",r["DocumentNumber"]);
			xHighEduDiplomaDocument.AddX("DocumentDate",r["DocumentDate"]);
			xHighEduDiplomaDocument.AddX("DocumentOrganization",r["DocumentOrganization"]);
			xHighEduDiplomaDocument.AddX("RegistrationNumber",r["edu_RegistrationNumber"]);
			xHighEduDiplomaDocument.AddX("QualificationTypeID",r["edu_QualificationID"]);	// ???
			xHighEduDiplomaDocument.AddX("SpecialityID",r["edu_SpecialityID"]); 	// ???
			//xHighEduDiplomaDocument.AddX("EndYear",((DateTime)r["DocumentDate"]).ToString("yyyy")); // ???
			

			return xHighEduDiplomaDocument;
		}

		private XElement ApplicationDocuments_PostGraduateDiplomaDocument(DataRow r) {
			// 25	Диплом об окончании аспирантуры (адъюнкатуры)
/*
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\PostGraduateDiplomaDocument
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\PostGraduateDiplomaDocument\UID
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\PostGraduateDiplomaDocument\OriginalReceived
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\PostGraduateDiplomaDocument\OriginalReceivedDate
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\PostGraduateDiplomaDocument\DocumentSeries
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\PostGraduateDiplomaDocument\DocumentNumber
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\PostGraduateDiplomaDocument\DocumentDate
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\PostGraduateDiplomaDocument\DocumentOrganization
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\PostGraduateDiplomaDocument\RegistrationNumber
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\PostGraduateDiplomaDocument\QualificationTypeID
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\PostGraduateDiplomaDocument\SpecialityID
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\PostGraduateDiplomaDocument\SpecializationID
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\PostGraduateDiplomaDocument\EndYear
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\PostGraduateDiplomaDocument\GPA
*/
			XElement xPostGraduateDiplomaDocument=new XElement("PostGraduateDiplomaDocument");

			xPostGraduateDiplomaDocument.AddX("UID",r["UID"]);
			xPostGraduateDiplomaDocument.AddX("OriginalReceived",r["OriginalReceived"]);	// ??? 0 или false
			xPostGraduateDiplomaDocument.AddX("OriginalReceivedDate",r["OriginalReceivedDate"]);
			xPostGraduateDiplomaDocument.AddX("DocumentSeries",r["DocumentSeries"]);
			xPostGraduateDiplomaDocument.AddX("DocumentNumber",r["DocumentNumber"]);
			xPostGraduateDiplomaDocument.AddX("DocumentDate",r["DocumentDate"]);
			xPostGraduateDiplomaDocument.AddX("DocumentOrganization",r["DocumentOrganization"]);
			xPostGraduateDiplomaDocument.AddX("RegistrationNumber",r["edu_RegistrationNumber"]);
			xPostGraduateDiplomaDocument.AddX("QualificationTypeID",r["edu_QualificationID"]);	// ???
			xPostGraduateDiplomaDocument.AddX("SpecialityID",r["edu_SpecialityID"]); 	// ???
			//xPostGraduateDiplomaDocument.AddX("SpecializationID",); 	// ???
			//xPostGraduateDiplomaDocument.AddX("EndYear",((DateTime)r["DocumentDate"]).ToString("yyyy")); // ???
			xPostGraduateDiplomaDocument.AddX("GPA",r["edu_GPA"]);

			return xPostGraduateDiplomaDocument;
		}

		private XElement ApplicationDocuments_PhDDiplomaDocument(DataRow r) {
			// 26	Диплом кандидата наук
			/*
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\PhDDiplomaDocument
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\PhDDiplomaDocument\UID
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\PhDDiplomaDocument\OriginalReceived
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\PhDDiplomaDocument\OriginalReceivedDate
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\PhDDiplomaDocument\DocumentSeries
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\PhDDiplomaDocument\DocumentNumber
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\PhDDiplomaDocument\DocumentDate
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\PhDDiplomaDocument\DocumentOrganization
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\PhDDiplomaDocument\RegistrationNumber
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\PhDDiplomaDocument\QualificationTypeID
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\PhDDiplomaDocument\SpecialityID
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\PhDDiplomaDocument\SpecializationID
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\PhDDiplomaDocument\EndYear
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\PhDDiplomaDocument\GPA
			*/
			XElement xPhDDiplomaDocument=new XElement("PhDDiplomaDocument");

			xPhDDiplomaDocument.AddX("UID",r["UID"]);
			xPhDDiplomaDocument.AddX("OriginalReceived",r["OriginalReceived"]);	// ??? 0 или false
			xPhDDiplomaDocument.AddX("OriginalReceivedDate",r["OriginalReceivedDate"]);
			xPhDDiplomaDocument.AddX("DocumentSeries",r["DocumentSeries"]);
			xPhDDiplomaDocument.AddX("DocumentNumber",r["DocumentNumber"]);
			xPhDDiplomaDocument.AddX("DocumentDate",r["DocumentDate"]);
			xPhDDiplomaDocument.AddX("DocumentOrganization",r["DocumentOrganization"]);
			xPhDDiplomaDocument.AddX("RegistrationNumber",r["edu_RegistrationNumber"]);
			xPhDDiplomaDocument.AddX("QualificationTypeID",r["edu_QualificationID"]);	// ???
			xPhDDiplomaDocument.AddX("SpecialityID",r["edu_SpecialityID"]); 	// ???
			//xPostGraduateDiplomaDocument.AddX("SpecializationID",); 	// ???
			//xPostGraduateDiplomaDocument.AddX("EndYear",((DateTime)r["DocumentDate"]).ToString("yyyy")); // ???
			xPhDDiplomaDocument.AddX("GPA",r["edu_GPA"]);

			return xPhDDiplomaDocument;
		}

		private XElement ApplicationDocuments_MiddleEduDiplomaDocument(DataRow r) {
			// 5	Диплом о среднем профессиональном образовании
			/*
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\MiddleEduDiplomaDocument
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\MiddleEduDiplomaDocument\UID
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\MiddleEduDiplomaDocument\OriginalReceived
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\MiddleEduDiplomaDocument\OriginalReceivedDate
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\MiddleEduDiplomaDocument\DocumentSeries
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\MiddleEduDiplomaDocument\DocumentNumber
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\MiddleEduDiplomaDocument\DocumenTDate
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\MiddleEduDiplomaDocument\DocumentOrganization
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\MiddleEduDiplomaDocument\RegistrationNumber
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\MiddleEduDiplomaDocument\QualificationTypeID
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\MiddleEduDiplomaDocument\SpecialityID
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\MiddleEduDiplomaDocument\EndYear
			*/
			XElement xMiddleEduDiplomaDocument=new XElement("MiddleEduDiplomaDocument");

			xMiddleEduDiplomaDocument.AddX("UID",r["UID"]);
			xMiddleEduDiplomaDocument.AddX("OriginalReceived",r["OriginalReceived"]);	// ??? 0 или false
			xMiddleEduDiplomaDocument.AddX("OriginalReceivedDate",r["OriginalReceivedDate"]);
			xMiddleEduDiplomaDocument.AddX("DocumentSeries",r["DocumentSeries"]);
			xMiddleEduDiplomaDocument.AddX("DocumentNumber",r["DocumentNumber"]);
			xMiddleEduDiplomaDocument.AddX("DocumentDate",r["DocumentDate"]);
			xMiddleEduDiplomaDocument.AddX("DocumentOrganization",r["DocumentOrganization"]);
			xMiddleEduDiplomaDocument.AddX("RegistrationNumber",r["edu_RegistrationNumber"]);
			xMiddleEduDiplomaDocument.AddX("QualificationTypeID",r["edu_QualificationID"]);	// ???
			xMiddleEduDiplomaDocument.AddX("SpecialityID",r["edu_SpecialityID"]); 	// ???
			//xMiddleEduDiplomaDocument.AddX("SpecializationID",); 	// ???
			//xMiddleEduDiplomaDocument.AddX("EndYear",((DateTime)r["DocumentDate"]).ToString("yyyy")); // ???

			return xMiddleEduDiplomaDocument;
		}

		private XElement ApplicationDocuments_BasicDiplomaDocument(DataRow r) {
			// 5	Диплом о среднем профессиональном образовании
			/*
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\BasicDiplomaDocument
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\BasicDiplomaDocument\UID
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\BasicDiplomaDocument\OriginalReceived
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\BasicDiplomaDocument\OriginalReceivedDate
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\BasicDiplomaDocument\DocumentSeries
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\BasicDiplomaDocument\DocumentNumber
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\BasicDiplomaDocument\DocumenTDate
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\BasicDiplomaDocument\DocumentOrganization
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\BasicDiplomaDocument\RegistrationNumber
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\BasicDiplomaDocument\QualificationTypeID
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\BasicDiplomaDocument\ProfessionID
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\BasicDiplomaDocument\EndYear
			*/
			XElement xBasicDiplomaDocument=new XElement("BasicDiplomaDocument");

			xBasicDiplomaDocument.AddX("UID",r["UID"]);
			xBasicDiplomaDocument.AddX("OriginalReceived",r["OriginalReceived"]);	// ??? 0 или false
			xBasicDiplomaDocument.AddX("OriginalReceivedDate",r["OriginalReceivedDate"]);
			xBasicDiplomaDocument.AddX("DocumentSeries",r["DocumentSeries"]);
			xBasicDiplomaDocument.AddX("DocumentNumber",r["DocumentNumber"]);
			xBasicDiplomaDocument.AddX("DocumentDate",r["DocumentDate"]);
			xBasicDiplomaDocument.AddX("DocumentOrganization",r["DocumentOrganization"]);
			xBasicDiplomaDocument.AddX("RegistrationNumber",r["edu_RegistrationNumber"]);
			xBasicDiplomaDocument.AddX("QualificationTypeID",r["edu_QualificationID"]);	// ???
			//xBasicDiplomaDocument.AddX("ProfessionID",r[""]); 	// ???
			//xBasicDiplomaDocument.AddX("EndYear",((DateTime)r["DocumentDate"]).ToString("yyyy")); // ???

			return xBasicDiplomaDocument;
		}

		private XElement ApplicationDocuments_IncomplHighEduDiplomaDocument(DataRow r) {
			// 7	Диплом о неполном высшем профессиональном образовании
			/*
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\IncomplHighEduDiplomaDocument
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\IncomplHighEduDiplomaDocument\UID
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\IncomplHighEduDiplomaDocument\OriginalReceived
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\IncomplHighEduDiplomaDocument\OriginalReceivedDate
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\IncomplHighEduDiplomaDocument\DocumentSeries
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\IncomplHighEduDiplomaDocument\DocumentNumber
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\IncomplHighEduDiplomaDocument\DocumenTDate
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\IncomplHighEduDiplomaDocument\DocumentOrganization
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\IncomplHighEduDiplomaDocument\RegistrationNumber
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\IncomplHighEduDiplomaDocument\QualificationTypeID
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\IncomplHighEduDiplomaDocument\SpecialityID
			*/
			XElement xIncomplHighEduDiplomaDocument=new XElement("IncomplHighEduDiplomaDocument");

			xIncomplHighEduDiplomaDocument.AddX("UID",r["UID"]);
			xIncomplHighEduDiplomaDocument.AddX("OriginalReceived",r["OriginalReceived"]);	// ??? 0 или false
			xIncomplHighEduDiplomaDocument.AddX("OriginalReceivedDate",r["OriginalReceivedDate"]);
			xIncomplHighEduDiplomaDocument.AddX("DocumentSeries",r["DocumentSeries"]);
			xIncomplHighEduDiplomaDocument.AddX("DocumentNumber",r["DocumentNumber"]);
			xIncomplHighEduDiplomaDocument.AddX("DocumentDate",r["DocumentDate"]);
			xIncomplHighEduDiplomaDocument.AddX("DocumentOrganization",r["DocumentOrganization"]);
			xIncomplHighEduDiplomaDocument.AddX("RegistrationNumber",r["edu_RegistrationNumber"]);
			xIncomplHighEduDiplomaDocument.AddX("QualificationTypeID",r["edu_QualificationID"]);	// ???
			xIncomplHighEduDiplomaDocument.AddX("SpecialityID",r["edu_SpecialityID"]); 	// ???
			//xBasicDiplomaDocument.AddX("EndYear",((DateTime)r["DocumentDate"]).ToString("yyyy")); // ???

			return xIncomplHighEduDiplomaDocument;
		}

		private XElement ApplicationDocuments_AcademicDiplomaDocument(DataRow r) {
			// 8	Академическая справка
			/*
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\AcademicDiplomaDocument
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\AcademicDiplomaDocument\UID
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\AcademicDiplomaDocument\OriginalReceived
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\AcademicDiplomaDocument\OriginalReceivedDate
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\AcademicDiplomaDocument\RegistrationNumber
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\AcademicDiplomaDocument\DocumentSeries
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\AcademicDiplomaDocument\DocumentNumber
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\AcademicDiplomaDocument\DocumenTDate
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\AcademicDiplomaDocument\DocumentOrganization
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\AcademicDiplomaDocument\QualificationTypeID
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\AcademicDiplomaDocument\SpecialityID
			*/
			XElement xAcademicDiplomaDocument=new XElement("AcademicDiplomaDocument");

			xAcademicDiplomaDocument.AddX("UID",r["UID"]);
			xAcademicDiplomaDocument.AddX("OriginalReceived",r["OriginalReceived"]);	// ??? 0 или false
			xAcademicDiplomaDocument.AddX("OriginalReceivedDate",r["OriginalReceivedDate"]);
			xAcademicDiplomaDocument.AddX("DocumentSeries",r["DocumentSeries"]);
			xAcademicDiplomaDocument.AddX("DocumentNumber",r["DocumentNumber"]);
			xAcademicDiplomaDocument.AddX("DocumentDate",r["DocumentDate"]);
			xAcademicDiplomaDocument.AddX("DocumentOrganization",r["DocumentOrganization"]);
			xAcademicDiplomaDocument.AddX("RegistrationNumber",r["edu_RegistrationNumber"]);
			xAcademicDiplomaDocument.AddX("QualificationTypeID",r["edu_QualificationID"]);	// ???
			xAcademicDiplomaDocument.AddX("SpecialityID",r["edu_SpecialityID"]); 	// ???
			//xAcademicDiplomaDocument.AddX("EndYear",((DateTime)r["DocumentDate"]).ToString("yyyy")); // ???

			return xAcademicDiplomaDocument;
		}

		private XElement ApplicationDocuments_EduCustomDocument(DataRow r) {
			// 19	Иной документ об образовании
			/*
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\EduCustomDocument
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\EduCustomDocument\UID
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\EduCustomDocument\DocumentSeries
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\EduCustomDocument\DocumentNumber
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\EduCustomDocument\DocumenTDate
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\EduCustomDocument\DocumentOrganization
Applications\Application\ApplicationDocuments\EduDocuments\EduDocument\EduCustomDocument\DocumentTypeNameText
			*/
			XElement xEduCustomDocument=new XElement("EduCustomDocument");

			xEduCustomDocument.AddX("UID",r["UID"]);
			xEduCustomDocument.AddX("DocumentSeries",r["DocumentSeries"]);
			xEduCustomDocument.AddX("DocumentNumber",r["DocumentNumber"]);
			xEduCustomDocument.AddX("DocumentDate",r["DocumentDate"]);
			xEduCustomDocument.AddX("DocumentOrganization",r["DocumentOrganization"]);
			xEduCustomDocument.AddX("DocumentTypeNameText",r["edc_DocumentTypeNameText"]);

			return xEduCustomDocument;
		}

		private XElement ApplicationDocuments_MilitaryCardDocument(DataRow r) {
			// 14	Военный билет
			/*
Applications\Application\ApplicationDocuments\MilitaryCardDocument
Applications\Application\ApplicationDocuments\MilitaryCardDocument\UID
Applications\Application\ApplicationDocuments\MilitaryCardDocument\OriginalReceived
Applications\Application\ApplicationDocuments\MilitaryCardDocument\OriginalReceivedDate
Applications\Application\ApplicationDocuments\MilitaryCardDocument\DocumentSeries
Applications\Application\ApplicationDocuments\MilitaryCardDocument\DocumentNumber
Applications\Application\ApplicationDocuments\MilitaryCardDocument\DocumentDate
Applications\Application\ApplicationDocuments\MilitaryCardDocument\DocumentOrganization
			*/
			XElement xMilitaryCardDocument=new XElement("MilitaryCardDocument");

			xMilitaryCardDocument.AddX("UID",r["UID"]);
			xMilitaryCardDocument.AddX("OriginalReceived",r["OriginalReceived"]);	// ??? 0 или false
			xMilitaryCardDocument.AddX("OriginalReceivedDate",r["OriginalReceivedDate"]);
			xMilitaryCardDocument.AddX("DocumentSeries",r["DocumentSeries"]);
			xMilitaryCardDocument.AddX("DocumentNumber",r["DocumentNumber"]);
			xMilitaryCardDocument.AddX("DocumentDate",r["DocumentDate"]);
			xMilitaryCardDocument.AddX("DocumentOrganization",r["DocumentOrganization"]);

			return xMilitaryCardDocument;
		}

		private XElement ApplicationDocuments_StudentDocument(DataRow r) {
			// 18	Справка об обучении в другом ВУЗе
			/*
Applications\Application\ApplicationDocuments\StudentDocument
Applications\Application\ApplicationDocuments\StudentDocument\UID
Applications\Application\ApplicationDocuments\StudentDocument\DocumentNumber
Applications\Application\ApplicationDocuments\StudentDocument\DocumentDate
Applications\Application\ApplicationDocuments\StudentDocument\DocumentOrganization
			*/
			XElement xStudentDocument=new XElement("StudentDocument");
			xStudentDocument.AddX("UID",r["UID"]);
			xStudentDocument.AddX("DocumentNumber",r["DocumentNumber"]);
			xStudentDocument.AddX("DocumentDate",r["DocumentDate"]);
			xStudentDocument.AddX("DocumentOrganization",r["DocumentOrganization"]);
			return xStudentDocument;
		}

		private XElement ApplicationDocuments_CustomDocument(DataRow r) {
			// 15	Иной документ
			/*
Applications\Application\ApplicationDocuments\CustomDocuments
Applications\Application\ApplicationDocuments\CustomDocuments\CustomDocument
Applications\Application\ApplicationDocuments\CustomDocuments\CustomDocument\UID
Applications\Application\ApplicationDocuments\CustomDocuments\CustomDocument\OriginalReceived
Applications\Application\ApplicationDocuments\CustomDocuments\CustomDocument\OriginalReceivedDate
Applications\Application\ApplicationDocuments\CustomDocuments\CustomDocument\DocumentSeries
Applications\Application\ApplicationDocuments\CustomDocuments\CustomDocument\DocumentNumber
Applications\Application\ApplicationDocuments\CustomDocuments\CustomDocument\DocumenTDate
Applications\Application\ApplicationDocuments\CustomDocuments\CustomDocument\DocumentOrganization
Applications\Application\ApplicationDocuments\CustomDocuments\CustomDocument\AdditionalInfo
Applications\Application\ApplicationDocuments\CustomDocuments\CustomDocument\DocumentTypeNameText
			*/
			XElement xCustomDocuments=new XElement("CustomDocument");

			xCustomDocuments.AddX("UID",r["UID"]);
			xCustomDocuments.AddX("OriginalReceived",r["OriginalReceived"]);	// ??? 0 или false
			xCustomDocuments.AddX("OriginalReceivedDate",r["OriginalReceivedDate"]);
			xCustomDocuments.AddX("DocumentSeries",r["DocumentSeries"]);
			xCustomDocuments.AddX("DocumentNumber",r["DocumentNumber"]);
			xCustomDocuments.AddX("DocumentDate",r["DocumentDate"]);
			xCustomDocuments.AddX("DocumentOrganization",r["DocumentOrganization"]);
			xCustomDocuments.AddX("AdditionalInfo",r["edc_AdditionalInfo"]);
			xCustomDocuments.AddX("DocumentTypeNameText",r["edc_DocumentTypeNameText"]);

			return xCustomDocuments;
		}

	}
}
