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
using GVUZ.Web.ContextExtensionsSQL;
using GVUZ.Model.Entrants.UniDocuments;
using GVUZ.Model.Entrants.Documents;
using GVUZ.Model.Helpers;
using GVUZ.Model;
using GVUZ.Model.Entrants;
namespace GVUZ.Web.ContextExtensionsSQL
{
    public class EntrantSQL
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

        public static IEnumerable<EntrantDocumentModel> getEntrantIdentityDocuments(int EntrantID, bool includeMainDoc = false)
        {
            List<EntrantDocumentModel> docs = new List<EntrantDocumentModel>();
            #region sql
            string sql = @"SELECT ed.EntrantDocumentID 
	  ,edi.IdentityDocumentTypeID as DocumentTypeID,	dt.Name as DocumentTypeName 
      ,DocumentSeries
      ,DocumentNumber
      ,DocumentDate
      ,DocumentOrganization
	  ,ed.AttachmentID as DocumentAttachmentID,	att.Name as DocumentAttachmentName,	att.FileID as AttachmentFileID
FROM Entrant e  (NOLOCK)
	INNER JOIN EntrantDocument ed (NOLOCK) on ed.EntrantID=e.EntrantID
	LEFT JOIN EntrantDocumentIdentity edi (NOLOCK) on ed.EntrantDocumentID=edi.EntrantDocumentID  
	LEFT JOIN IdentityDocumentType dt (NOLOCK) on dt.IdentityDocumentTypeID=edi.IdentityDocumentTypeID  
	LEFT OUTER JOIN Attachment att (NOLOCK) on att.AttachmentID=ed.AttachmentID
WHERE e.EntrantID=@EntrantID and ed.DocumentTypeID = 1";
            //if (!includeMainDoc) { sql += " and e.IdentityDocumentID!=ed.EntrantDocumentID"; }
            #endregion
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("EntrantID", EntrantID));
                try
                {
                    con.Open();
                    SqlDataReader r = com.ExecuteReader();
                    EntrantDocumentModel d;
                    while (r.Read())
                    {
                        d = new EntrantDocumentModel();
                        d.EntrantDocumentID = (int)r["EntrantDocumentID"];
                        
                        //d.DocumentTypeID = (int)r["DocumentTypeID"];
                        //d.DocumentTypeName = r["DocumentTypeName"] as string;
                        d.DocumentTypeID = r["DocumentTypeID"].ToString() == "" ? 15 :  (int)r["DocumentTypeID"];
                        d.DocumentTypeName = r["DocumentTypeName"] as string;
                            //== null ? "Иной документ" : r["DocumentTypeName"].ToString();
                        d.DocumentSeries = r["DocumentSeries"] as string;
                        d.DocumentNumber = r["DocumentNumber"] as string;
                        d.DocumentDateTime = r["DocumentDate"] as DateTime?;
                        d.DocumentOrganization = r["DocumentOrganization"] as string;
                        d.DocumentAttachmentID = r["DocumentAttachmentID"] as int?;
                        d.DocumentAttachmentName = r["DocumentAttachmentName"] as string;
                        d.AttachmentFileID = r["AttachmentFileID"] as Guid?;
                        if (d.AttachmentFileID == null)
                        {
                            d.AttachmentFileID = Guid.Empty;
                        }
                        docs.Add(d);
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
            return docs;
        }

        public static EntrantDocumentViewModel getEntrantDocument(int? EntrantDocumentID, bool forEdit = false, int doctypeid = 0)
        {
            // Если forEdit=true то надо подтянуть справочники.

            EntrantDocumentID = EntrantDocumentID ?? 0;
            EntrantDocumentViewModel doc = new EntrantDocumentViewModel();
            doc.DocumentTypeID = doctypeid;
            #region Предзагрузка
            if (EntrantDocumentID > 0)
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    SqlCommand com = new SqlCommand("SELECT ed.EntrantDocumentID, ed.DocumentTypeID FROM EntrantDocument ed (NOLOCK) WHERE ed.EntrantDocumentID=@EntrantDocumentID", con);
                    com.Parameters.Add(new SqlParameter("EntrantDocumentID", EntrantDocumentID));
                    try
                    {
                        con.Open();
                        SqlDataReader r = com.ExecuteReader();
                        while (r.Read())
                        {
                            doc.EntrantDocumentID = (int)r["EntrantDocumentID"];
                            doc.DocumentTypeID = (int)r["DocumentTypeID"];
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
            }
            #endregion

            #region Документы
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

            if (doc.EntrantDocumentID == 0)
            {
                var DocumentType = DictionaryCache.GetEntries(DictionaryCache.DictionaryTypeEnum.DocumentType).Where(x => x.Key == doctypeid).Select(x => new EntrantDocumentListViewModel.DocumentType { TypeID = x.Key, Name = x.Value }).FirstOrDefault();
                if (DocumentType != null)
                {
                    doc.DocumentTypeID = DocumentType.TypeID;
                    doc.DocumentTypeName = DocumentType.Name;
                }
            }
            #region switch(doc.DocumentTypeID)
            switch (doc.DocumentTypeID)
            {
                case 1: // 1	Документ, удостоверяющий личность
                    doc.EntDocIdentity = new EntrantDocumentIdentityViewModel();
                    if (forEdit)
                    {
                        doc.EntDocIdentity.GenderList = new[] { new { ID = GenderType.Male, Name = GenderType.GetName(GenderType.Male) }, new { ID = GenderType.Female, Name = GenderType.GetName(GenderType.Female) } };
                        doc.EntDocIdentity.NationalityList = DictionaryCache.GetEntries(DictionaryCache.DictionaryTypeEnum.CountryType).Select(x => new { ID = x.Key, Name = x.Value });
                        //doc.EntDocIdentity.IdentityDocumentList=DictionaryCache.GetEntries(DictionaryCache.DictionaryTypeEnum.IdentityDocumentType).Select(x => new { ID=x.Key,Name=x.Value });
                        doc.EntDocIdentity.IdentityDocumentList = ApplicationSQL.GetIdentityDocumentType().Select(x => new { ID = x.IdentityDocumentTypeId, Name = x.IdentityDocumentTypeName }).ToArray();
                        doc.EntDocIdentity.IdentityDocumentTypeEdit = true;
                    }
                    break;
                case 8: // 8	Академическая справка
                case 26:// 26	Диплом кандидата наук
                case 4: // 4	Диплом о высшем профессиональном образовании
                case 6: // 6	Диплом о начальном профессиональном образовании
                case 7: // 7	Диплом о неполном высшем профессиональном образовании
                case 5: // 5	Диплом о среднем профессиональном образовании
                case 25:// 25	Диплом об окончании аспирантуры (адъюнкатуры)
                case 3: // 3	Аттестат о среднем (полном) общем образовании
                case 16:// 16	Аттестат об основном общем образовании
                case 18:// 18	Справка об обучении в другом ВУЗе
                    doc.EntDocEdu = new EntrantDocumentEduViewModel();
                    doc.EntDocSubBall = new EntrantDocumentSubjectBallViewModel();
                    break;
                case 17:    //17	Справка ГИА
                    doc.EntDocSubBall = new EntrantDocumentSubjectBallViewModel();
                    break;
                case 14://	14	Военный билет
                    break;

                case 9:  // 9	Диплом победителя/призера олимпиады школьников
                case 10: //	10	Диплом победителя/призера всероссийской олимпиады школьников
                    doc.EntDocOlymp = new EntrantDocumentOlympicViewModel(doc.DocumentTypeID);
                    break;

                case 27: //	27	Диплом победителя/призера IV этапа всеукраинской ученической олимпиады
                case 28: //	28	Диплом об участии в международной олимпиаде
                case 29: //	29	Документ, подтверждающий принадлежность к соотечественникам за рубежом
                case 30: //	30	Документ, подтверждающий принадлежность к детям-сиротам и детям, оставшимся без попечения родителей
                case 31: //	31	Документ, подтверждающий принадлежность к ветеранам боевых действий
                case 32: // 32  Документ, подтверждающий наличие только одного родителя - инвалида I группы и принадлежность к числу малоимущих семей
                case 33: // 33  Документ, подтверждающий принадлежность родителей и опекунов к погибшим в связи с исполнением служебных обязанностей
                case 34: // 34  Документ, подтверждающий принадлежность к сотрудникам государственных органов Российской Федерации
                case 35: // 35  Документ, подтверждающий участие в работах на радиационных объектах или воздействие радиации
                    doc.EntDocOther = new EntrantDocumentOtherViewModel(doc.DocumentTypeID);
                    break;

                case 20: // 20	Диплом чемпиона/призера Олимпийских игр
                case 21: // 21	Диплом чемпиона/призера Паралимпийских игр
                case 22: // 22	Диплом чемпиона/призера Сурдлимпийских игр
                case 23: // 23	Диплом чемпиона мира
                case 24: // 24	Диплом чемпиона Европы
                case 15: // 15	Иной документ
                    doc.EntDocCustom = new EntrantDocumentCustomViewModel();
                    break;
                case 19: // 19	Иной документ по образованию
                    doc.EntDocEdu = new EntrantDocumentEduViewModel();
                    doc.EntDocCustom = new EntrantDocumentCustomViewModel();
                    break;
                case 11: // 11	Справка об установлении инвалидности
                    doc.EntDocDis = new EntrantDocumentDisabilityViewModel();
                    if (forEdit)
                    {
                        doc.EntDocDis.DisabilityList = DictionarySQL.GetDisabilityList().ToArray();
                    }
                    break;
                case 2: //2	Свидетельство о результатах ЕГЭ
                    doc.EntDocSubBall = new EntrantDocumentSubjectBallViewModel();
                    doc.EntDocEge = new EntrantDocumentEgeViewModel();
                    break;
                default:
                    break;
            }
            #endregion
            // Доп подгрузка справочников
            if (doc.EntDocSubBall != null)
            {
                if (forEdit)
                {
                    doc.EntDocSubBall.SubjectList = DictionarySQL.GetSubjectList();
                    //doc.EntDocSubBall.SubjectList=DictionaryCache.GetEntries(DictionaryCache.DictionaryTypeEnum.Subject).OrderBy(x => x.Value).Select(x => new SubjectBriefData() { SubjectID=x.Key,SubjectName=x.Value}).ToArray();
                }
            }
            if (doc.EntrantDocumentID == 0)
            {   // Только форма для создания
                return doc;
            }

            StringBuilder qSelect = new StringBuilder();
            StringBuilder qFrom = new StringBuilder();
            StringBuilder qWhere = new StringBuilder();

            #region Общий документ 
            qSelect.Append(@"
SELECT ed.EntrantDocumentID
		,ed.EntrantID
	    ,ed.[UID] as [UID]
	    ,ed.DocumentTypeID
        ,ed.DocumentName
	    ,dt.Name as DocumentTypeName
        ,ed.DocumentSeries
        ,ed.DocumentNumber
        ,ed.DocumentDate
        ,ed.DocumentOrganization
	    ,ed.AttachmentID as AttachmentID
        ,ed.OlympApproved
	    ,att.Name as AttachmentName
	    ,att.FileID as AttachmentFileID
");
            qFrom.Append(@"
FROM EntrantDocument ed (NOLOCK)
	INNER JOIN DocumentType dt (NOLOCK) on ed.DocumentTypeID=dt.DocumentID
	LEFT OUTER JOIN Attachment att (NOLOCK) on att.AttachmentID=ed.AttachmentID
");
            qWhere.Append(@"
WHERE ed.EntrantDocumentID=@EntrantDocumentID
");     // 9975795
            #endregion

            #region ДопЗапросы по Объектам
            #region EntDocIdentity
            if (doc.EntDocIdentity != null)
            {
                qSelect.Append(@"
	,edi.IdentityDocumentTypeID as edi_IdentityDocumentTypeID
	,idt.Name as edi_IdentityDocumentTypeName
	,edi.GenderTypeID as edi_GenderTypeID
	,gt.Name as  edi_GenderTypeName
	,edi.NationalityTypeID as edi_NationalityTypeID
	,nt.Name as edi_NationalityTypeName
	,edi.BirthDate as edi_BirthDate
	,edi.BirthPlace as edi_BirthPlace
	,edi.SubdivisionCode as edi_SubdivisionCode
	,edi.LastName as edi_LastName
	,edi.FirstName as edi_FirstName
	,edi.MiddleName as edi_MiddleName
				");
                qFrom.Append(@"
	INNER JOIN EntrantDocumentIdentity edi (NOLOCK) on ed.EntrantDocumentID=edi.EntrantDocumentID
	INNER JOIN IdentityDocumentType idt  on edi.IdentityDocumentTypeID=idt.IdentityDocumentTypeID
	INNER JOIN GenderType gt on edi.GenderTypeID=gt.GenderID
	INNER JOIN CountryType nt on edi.NationalityTypeID=nt.CountryID
				");
            }
            #endregion
            #region EntDocEdu
            if (doc.EntDocEdu != null)
            {
                qSelect.Append(@"
	,ede.RegistrationNumber as edu_RegistrationNumber
	,ede.QualificationName as edu_QualificationName
	,ede.SpecialityName as edu_SpecialityName
	,ede.GPA	as edu_GPA
    ,ede.IsNostrificated as edu_IsNostrificated
	,ed.DocumentOrganization as edu_DocumentOU 
    ,ede.StateServicePreparation as edu_StateServicePreparation
    ,ede.InstitutionName as edu_InstitutionName
    ,ede.InstitutionAddress as edu_InstitutionAddress
    ,ede.FacultyName as edu_FacultyName
    ,ede.BeginDate as edu_BeginDate
    ,ede.EndDate as edu_EndDate
    ,ede.EducationFormID as edu_EducationFormID
    ,ede.CountryID as edu_CountryID

				");
                qFrom.Append(@"
	LEFT OUTER JOIN EntrantDocumentEdu ede on ed.EntrantDocumentID=ede.EntrantDocumentID
				");
            }
            #endregion
            #region EntDocOlymp
            if (doc.EntDocOlymp != null)

            {
                qSelect.Append(@"	
	,edo.OlympicID as edo_OlympicID
	,edo.OlympicTypeProfileID as edo_OlympicTypeProfileID
	,edo.ClassNumber as edo_ClassNumber
	,ot.Name as edo_OlympicName 
	,ot.OlympicYear as edo_OlympicYear 
    ,edo.DiplomaTypeID as edo_DiplomaTypeID 
    ,op.OlympicLevelID as edo_OlympicLevelID
    ,op.OrganizerName as edo_OrganizerName
    ,profSubj.SubjectID as edo_ProfileSubjectID
    ,profSubj.Name as edo_ProfileSubjectName
    ,egeSubj.SubjectID as edo_EgeSubjectID
    ,egeSubj.Name as edo_EgeSubjectName
    ,(select name from OlympicDiplomType where OlympicDiplomTypeID=edo.DiplomaTypeID) as edo_DiplomaTypeName 
	,(select profilename from OlympicProfile where OlympicProfileID = op.OlympicProfileID) as edo_OlympicProfile
	,(select name from OlympicLevel where OlympicLevelID = op.OlympicLevelID) as edo_OlympicTypeLevel 
				");

                qFrom.Append(@"
	INNER JOIN EntrantDocumentOlympic edo (NOLOCK) on edo.EntrantDocumentID=ed.EntrantDocumentID
	INNER JOIN OlympicTypeProfile op (NOLOCK) on op.OlympicTypeProfileID=edo.OlympicTypeProfileID 
    LEFT JOIN OlympicType ot (NOLOCK) on ot.OlympicID=edo.OlympicID 
    LEFT JOIN [Subject] profSubj on edo.ProfileSubjectID = profSubj.SubjectID
    LEFT JOIN [Subject] egeSubj on edo.EgeSubjectID = egeSubj.SubjectID
				");
            }
            #endregion



            #region Other
            if (doc.EntDocOther != null)
            {
                if(doc.DocumentTypeID == 27)
                { 
                    qSelect.Append(@",edo.Profile as edo_OlympicProfile
                                     ,edo.OlympicPlace as edo_OlympicPlace,edo.OlympicDate as edo_OlympicDate
                                     ,edo.DiplomaTypeID as edo_DiplomaTypeID");
                    qFrom.Append(@"INNER JOIN EntrantDocumentUkraineOlympic edo (NOLOCK) on edo.EntrantDocumentID=ed.EntrantDocumentID");
                }
                if (doc.DocumentTypeID == 28)
                {
                    qSelect.Append(@",edo.Profile as edo_OlympicProfile
                                     ,edo.OlympicPlace as edo_OlympicPlace,edo.OlympicDate as edo_OlympicDate
                                     ,edo.CountryID as edo_CountryID");
                    qFrom.Append(@"INNER JOIN EntrantDocumentInternationalOlympic edo (NOLOCK) on edo.EntrantDocumentID=ed.EntrantDocumentID");
                }
                if (doc.DocumentTypeID == 29)
                {
                    qSelect.Append(@",edo.CompatriotCategoryID as edo_CompatriotCategoryID
                                     ,edo.CompatriotStatus as edo_CompatriotStatus
                                     ");
                    qFrom.Append(@"INNER JOIN EntrantDocumentCompatriot edo (NOLOCK) on edo.EntrantDocumentID=ed.EntrantDocumentID
                                   ");
                }
                if (doc.DocumentTypeID == 30)
                {
                    qSelect.Append(@",edo.OrphanCategoryID as edo_OrphanCategoryID");
                    qFrom.Append(@"INNER JOIN EntrantDocumentOrphan edo on edo.EntrantDocumentID=ed.EntrantDocumentID");
                }
                if (doc.DocumentTypeID == 31)
                {
                    qSelect.Append(@",edo.VeteranCategoryID as edo_VeteranCategoryID");
                    qFrom.Append(@"INNER JOIN EntrantDocumentVeteran edo (NOLOCK) on edo.EntrantDocumentID=ed.EntrantDocumentID");
                }
                if (doc.DocumentTypeID == 33)
                {
                    qSelect.Append(@",edo.ParentsLostCategoryID as edo_ParentsLostCategoryID");
                    qFrom.Append(@"INNER JOIN EntrantDocumentParentsLost edo (NOLOCK) on edo.EntrantDocumentID=ed.EntrantDocumentID");
                }
                if (doc.DocumentTypeID == 34)
                {
                    qSelect.Append(@",edo.StateEmployeeCategoryID as edo_StateEmployeeCategoryID");
                    qFrom.Append(@"INNER JOIN EntrantDocumentStateEmployee edo (NOLOCK) on edo.EntrantDocumentID=ed.EntrantDocumentID");
                }
                if (doc.DocumentTypeID == 35)
                {
                    qSelect.Append(@",edo.RadiationWorkCategoryID as edo_RadiationWorkCategoryID");
                    qFrom.Append(@"INNER JOIN EntrantDocumentRadiationWork edo (NOLOCK) on edo.EntrantDocumentID=ed.EntrantDocumentID");
                }
            }
            #endregion

            #region EntDocCustom
            if (doc.EntDocCustom != null)
            {
                qSelect.Append(@"	
    ,edc.AdditionalInfo as edc_AdditionalInfo
				");
                qFrom.Append(@"
	LEFT OUTER JOIN EntrantDocumentCustom edc (NOLOCK) on edc.EntrantDocumentID=ed.EntrantDocumentID
			");
            }
            #endregion
            #region EntDocDis
            if (doc.EntDocDis != null)
            {
                qSelect.Append(@"	
	,edd.DisabilityTypeID as dis_DisabilityTypeID
    ,edd_dt.Name as dis_DisabilityTypeName
				");
                qFrom.Append(@"
	LEFT OUTER JOIN EntrantDocumentDisability edd (NOLOCK) on edd.EntrantDocumentID=ed.EntrantDocumentID
    INNER JOIN DisabilityType edd_dt on edd_dt.DisabilityID=edd.DisabilityTypeID
			");
            }
            #endregion
            #region EntDocSubBall
            if (doc.EntDocSubBall != null)
            {
                qSelect.Append(@"
	, (SELECT CAST(sub.SubjectID as varchar(10)) +'|'+ sub.Name +'|'+ ISNULL(CAST( [Value] as varchar(50)),'') +'|'
FROM EntrantDocumentEgeAndOlympicSubject edeaos (NOLOCK)	INNER JOIN [Subject] sub on edeaos.SubjectID=sub.SubjectID
WHERE edeaos.EntrantDocumentID=ed.EntrantDocumentID 
FOR XML PATH('')
) as edsb_SubjectBalls
				");
            }
            #endregion
            #region EntDocEge
            if (doc.EntDocEge != null)
            {
                qSelect.Append(@"	
	,ege.TypographicNumber as ege_TypographicNumber	
				");
                qFrom.Append(@"
	LEFT OUTER JOIN EntrantDocumentEge ege (NOLOCK) on ege.EntrantDocumentID=ed.EntrantDocumentID
			   ");
            }
            #endregion

            #endregion

            #region ДопЗапросы по ID
            /*
			switch(doc.DocumentTypeID) {
				#region SQL 1	Документ, удостоверяющий личность
				case 1: // 1	Документ, удостоверяющий личность
				qSelect.Append(@"
	,edi.IdentityDocumentTypeID as edi_IdentityDocumentTypeID
	,idt.Name as edi_IdentityDocumentTypeName
	,edi.GenderTypeID as edi_GenderTypeID
	,gt.Name as  edi_GenderTypeName
	,edi.NationalityTypeID as edi_NationalityTypeID
	,nt.Name as edi_NationalityTypeName
	,edi.BirthDate as edi_BirthDate
	,edi.BirthPlace as edi_BirthPlace
	,edi.SubdivisionCode as edi_SubdivisionCode
				");
				qFrom.Append(@"
	INNER JOIN EntrantDocumentIdentity edi on ed.EntrantDocumentID=edi.EntrantDocumentID
	INNER JOIN IdentityDocumentType idt  on edi.IdentityDocumentTypeID=idt.IdentityDocumentTypeID
	INNER JOIN GenderType gt on edi.GenderTypeID=gt.GenderID
	INNER JOIN NationalityType nt on edi.NationalityTypeID=nt.NationalityID
				");
				break;
				#endregion
				#region EntDocEdu
				case 8:	// 8	Академическая справка
				case 26: // 26	Диплом кандидата наук
				case 4:	//	4	Диплом о высшем профессиональном образовании
				case 6:	// 6	Диплом о начальном профессиональном образовании
				case 7:	// 7	Диплом о неполном высшем профессиональном образовании
				case 5:	// 5	Диплом о среднем профессиональном образовании
				case 25:	// 25	Диплом об окончании аспирантуры (адъюнкатуры)
				qSelect.Append(@"
	,ede.RegistrationNumber
	,ede.QualificationName
	,ede.SpecialityName
	,ede.SpecialityName
	,ede.GPA
	,ed.DocumentOrganization as DocumentOU
				");
				qFrom.Append(@"
	INNER JOIN EntrantDocumentEdu ede on ed.EntrantDocumentID=ede.EntrantDocumentID
				");
				break;
				#endregion
				default:
				break;
			}
 */
            #endregion

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(qSelect.ToString() + qFrom.ToString() + qWhere.ToString(), con);
                com.Parameters.Add(new SqlParameter("EntrantDocumentID", doc.EntrantDocumentID));
                try
                {
                    con.Open();
                    SqlDataReader r = com.ExecuteReader();
                    while (r.Read())
                    {
                        #region doc
                        doc.EntrantID = (int)r["EntrantID"];
                        doc.EntrantDocumentID = (int)r["EntrantDocumentID"];
                        doc.DocumentName = r["DocumentName"] as string;
                        doc.OlympicName = doc.DocumentName;
                        doc.UID = r["UID"] as string;
                        doc.DocumentTypeID = (int)r["DocumentTypeID"];
                        doc.DocumentTypeName = r["DocumentTypeName"] as string;
                        doc.DocumentSeries = r["DocumentSeries"] as string;
                        doc.Series = doc.DocumentSeries;
                        doc.DocumentNumber = r["DocumentNumber"] as string;
                        doc.Number = doc.DocumentNumber;
                        doc.CertificateNumber = doc.DocumentNumber;
                        doc.DocumentDate = r["DocumentDate"] as DateTime?;
                        doc.DocumentOrganization = r["DocumentOrganization"] as string;
                        doc.AttachmentID = r["AttachmentID"] as int?;
                        doc.AttachmentName = r["AttachmentName"] as string;
                        doc.AttachmentFileID = r["AttachmentFileID"] as Guid?;
                        #endregion
                        #region EntDocIdentity
                        if (doc.EntDocIdentity != null)
                        {
                            doc.EntDocIdentity.IdentityDocumentTypeID = (int)r["edi_IdentityDocumentTypeID"];
                            doc.EntDocIdentity.IdentityDocumentTypeName = r["edi_IdentityDocumentTypeName"] as string;
                            doc.EntDocIdentity.GenderTypeID = (int)r["edi_GenderTypeID"];
                            doc.EntDocIdentity.GenderTypeName = r["edi_GenderTypeName"] as string;
                            doc.EntDocIdentity.NationalityTypeID = (int)r["edi_NationalityTypeID"];
                            doc.EntDocIdentity.NationalityTypeName = r["edi_NationalityTypeName"] as string;
                            doc.EntDocIdentity.BirthDate = (DateTime)r["edi_BirthDate"];
                            doc.EntDocIdentity.BirthPlace = r["edi_BirthPlace"] as string;
                            doc.EntDocIdentity.SubdivisionCode = r["edi_SubdivisionCode"] as string;

                            doc.EntDocIdentity.LastName = r["edi_LastName"] as string;
                            doc.EntDocIdentity.FirstName = r["edi_FirstName"] as string;
                            doc.EntDocIdentity.MiddleName = r["edi_MiddleName"] as string;
                        }
                        #endregion
                        #region EntDocEdu
                        if (doc.EntDocEdu != null)
                        {
                            doc.EntDocEdu.EntrantDocumentID = doc.EntrantDocumentID;
                            doc.EntDocEdu.RegistrationNumber = r["edu_RegistrationNumber"] as string;
                            doc.EntDocEdu.QualificationName = r["edu_QualificationName"] as string;
                            doc.EntDocEdu.SpecialityName = r["edu_SpecialityName"] as string;
                            doc.EntDocEdu.GPA = r["edu_GPA"] as float?;
                            doc.EntDocEdu.IsNostrificated = r["edu_IsNostrificated"] as bool?;
                            doc.EntDocEdu.DocumentOU = doc.DocumentOrganization;
                            doc.EntDocEdu.StateServicePreparation = r["edu_StateServicePreparation"] as bool?;
                            doc.EntDocEdu.InstitutionName = (r["edu_InstitutionName"]).ToString();
                            doc.EntDocEdu.InstitutionAddress = (r["edu_InstitutionAddress"]).ToString();
                            doc.EntDocEdu.FacultyName = (r["edu_FacultyName"]).ToString();
                            doc.EntDocEdu.BeginDate = r["edu_BeginDate"] as DateTime?;
                            doc.EntDocEdu.EndDate = r["edu_EndDate"] as DateTime?;
                            doc.EntDocEdu.EducationFormID = r["edu_EducationFormID"] as byte?;
                            doc.EntDocEdu.CountryID = r["edu_CountryID"] as int?;

                        }
                        #endregion
                        #region EntDocOlymp
                        if (doc.EntDocOlymp != null)
                        {
                            doc.EntDocOlymp.OlympicID = (int)r["edo_OlympicID"];
                            doc.EntDocOlymp.DiplomaTypeID = (int)r["edo_DiplomaTypeID"];
                            doc.EntDocOlymp.DiplomaTypeName = r["edo_DiplomaTypeName"] as string;
                            doc.EntDocOlymp.OlympicDetails.OlympicID = (int)doc.EntDocOlymp.OlympicID;
                            doc.EntDocOlymp.OlympicDetails.OlympicName = r["edo_OlympicName"] as string;
                            doc.EntDocOlymp.OlympicTypeProfileID = (int)r["edo_OlympicTypeProfileID"];
                            doc.EntDocOlymp.ProfileSubjectID = r["edo_ProfileSubjectID"] as int?;
                            doc.EntDocOlymp.ProfileSubjectName = r["edo_ProfileSubjectName"] as string;
                            doc.EntDocOlymp.EgeSubjectID = r["edo_EgeSubjectID"] as int?;
                            doc.EntDocOlymp.EgeSubjectName =  r["edo_EgeSubjectName"] as string;
                            doc.EntDocOlymp.FormNumberID = (int)r["edo_ClassNumber"];
                            doc.EntDocOlymp.Approved = r["OlympApproved"] as bool?;
                        }
                        #endregion
                        #region Other
                        if (doc.EntDocOther != null)
                        {
                            if(doc.DocumentTypeID == 27)
                            { 
                                doc.EntDocOther.DiplomaTypeID = Convert.ToByte(r["edo_DiplomaTypeID"]);
                                doc.EntDocOther.OlympicProfile = r["edo_OlympicProfile"] as string;
                                doc.EntDocOther.OlympicPlace = r["edo_OlympicPlace"] as string;
                                doc.EntDocOther.OlympicDate = r["edo_OlympicDate"] as DateTime?;
                            }
                            if (doc.DocumentTypeID == 28)
                            {
                                doc.EntDocOther.CountryID = Convert.ToInt32(r["edo_CountryID"]);
                                doc.EntDocOther.OlympicProfile = r["edo_OlympicProfile"] as string;
                                doc.EntDocOther.OlympicPlace = r["edo_OlympicPlace"] as string;
                                doc.EntDocOther.OlympicDate = r["edo_OlympicDate"] as DateTime?;
                            }
                            if (doc.DocumentTypeID == 29)
                            {
                                doc.EntDocOther.CompatriotCategoryID = Convert.ToInt32(r["edo_CompatriotCategoryID"]);
                                //doc.EntDocOther.DocumentName = r["edo_Name"] as string;
                                doc.EntDocOther.CompatriotStatus = (r["edo_CompatriotStatus"]).ToString();
                            }
                            if (doc.DocumentTypeID == 30)
                            {
                                doc.EntDocOther.OrphanCategoryID = Convert.ToInt32(r["edo_OrphanCategoryID"]);
                                //doc.EntDocOther.DocumentName = r["edo_Name"] as string;
                            }
                            if (doc.DocumentTypeID == 31)
                            {
                                doc.EntDocOther.VeteranCategoryID = Convert.ToInt32(r["edo_VeteranCategoryID"]);
                            }
                            if (doc.DocumentTypeID == 33)
                            {
                                doc.EntDocOther.ParentsLostCategoryID = Convert.ToInt32(r["edo_ParentsLostCategoryID"]);
                            }
                            if (doc.DocumentTypeID == 34)
                            {
                                doc.EntDocOther.StateEmployeeCategoryID = Convert.ToInt32(r["edo_StateEmployeeCategoryID"]);
                            }
                            if (doc.DocumentTypeID == 35)
                            {
                                doc.EntDocOther.RadiationWorkCategoryID = Convert.ToInt32(r["edo_RadiationWorkCategoryID"]);
                            }
                            
                        }
                        #endregion
                        #region EntDocCustom
                        if (doc.EntDocCustom != null)
                        {
                            doc.EntDocCustom.AdditionalInfo = r["edc_AdditionalInfo"] as string;
                            //doc.EntDocCustom.DocumentName = r["DocumentName"] as string;
                        }
                        #endregion
                        #region EntDocDis
                        if (doc.EntDocDis != null)
                        {
                            doc.EntDocDis.DisabilityTypeID = (int)r["dis_DisabilityTypeID"];
                            //doc.EntDocDis.DisabilityType = r["dis_DisabilityTypeName"] as string;
                        }
                        #endregion
                        #region EntDocSubBall
                        if (doc.EntDocSubBall != null)
                        {
                            string edsb_SubjectBalls = r["edsb_SubjectBalls"] as string;
                            // string edot_Subjects="12|SDSD|30|45|Плоыв|40|";
                            List<SubjectBall> list = new List<SubjectBall>();
                            // Парсим В строке через | то SubjectID то SubjectName
                            if (!String.IsNullOrEmpty(edsb_SubjectBalls))
                            {
                                string[] A = edsb_SubjectBalls.Split('|');
                                int subjectID;
                                string subjectName;
                                int value = -1;
                                int count = A.Count();

                                for (int i = 0; i < count; i++)
                                {
                                    if (String.IsNullOrEmpty(A[i])) { break; }
                                    if (count < i + 2) { break; }
                                    if (Int32.TryParse(A[i], out subjectID))
                                    {
                                        subjectName = A[i + 1];
                                        if (!Int32.TryParse(A[i + 2], out value)) { value = -1; }
                                        list.Add(new SubjectBall() { SubjectID = subjectID, SubjectName = subjectName, Value = (value >= 0 ? (int?)value : null) });
                                        i += 2;
                                    }
                                }
                            }
                            doc.EntDocSubBall.SubjectBalls = list;
                        }
                        #endregion
                        #region EntDocEge
                        if (doc.EntDocEge != null)
                        {
                            doc.EntDocEge.TypographicNumber = r["ege_TypographicNumber"] as string;
                        }
                        #endregion
                    }
                    r.Close();
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
            return doc;
        }

        /// <summary>Проверак на идентичный документ</summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static int checkExistEntrantDocument(EntrantDocumentViewModel doc)
        {
            // Перед сохранением карточки документа, Система должна осуществлять проверку на отсутствие данного документа у абитуриента. 
            //	Поиск дубликатов должен осуществляться по EntrantDocument.DocumentNumber, EntrantDocument.DocumentSeries, EntrantDocument.DocumentDate, EntrantDocument.DocumentTypeID. 
            //	В случае наличия документа, процесс сохранения должен прерываться, должно выдаваться окно с текстом «У абитуриента найден документ с теми же данными. Исправьте данные перед сохранением». 
            int id = 0;
            #region SQL
            string sql = @"
IF @IdentityDocumentTypeID IS NULL BEGIN 
	IF @DocumentTypeID = 9 BEGIN
		SELECT TOP 1 ed.EntrantDocumentID 
		FROM EntrantDocument AS ed (NOLOCK) 
        INNER JOIN EntrantDocumentOlympic AS edo (NOLOCK)	ON edo.EntrantDocumentID = ed.EntrantDocumentID
		WHERE  ed.EntrantID=@EntrantID AND ed.DocumentTypeID=@DocumentTypeID 
			AND	ed.DocumentNumber=@DocumentNumber
			AND 	(ed.DocumentSeries=@DocumentSeries OR (@DocumentSeries IS NULL AND ed.DocumentSeries IS NULL  ) ) 
			AND 	(ed.DocumentDate=@DocumentDate OR (@DocumentDate IS NULL AND ed.DocumentDate IS NULL  ) )
			AND	edo.OlympicID=@OlympicID
            AND (ed.EntrantDocumentID <> @EntrantDocumentID OR (@EntrantDocumentID IS NULL AND ed.EntrantDocumentID IS NULL))
	END
	ELSE IF  @DocumentTypeID = 2 BEGIN 	-- ЕГЭ
		SELECT TOP 1 ed.EntrantDocumentID FROM EntrantDocument ed  (NOLOCK)
		WHERE  ed.EntrantID=@EntrantID AND ed.DocumentTypeID=@DocumentTypeID	AND ed.DocumentNumber=@DocumentNumber
        AND (ed.EntrantDocumentID <> @EntrantDocumentID OR (@EntrantDocumentID IS NULL AND ed.EntrantDocumentID IS NULL))
	END
	ELSE BEGIN
		SELECT TOP 1 ed.EntrantDocumentID FROM EntrantDocument ed  (NOLOCK)
		WHERE  
			ed.EntrantID=@EntrantID AND ed.DocumentTypeID=@DocumentTypeID
			AND ed.DocumentNumber=@DocumentNumber 
			AND (ed.DocumentSeries=@DocumentSeries OR (@DocumentSeries IS NULL AND ed.DocumentSeries IS NULL  ) ) 
			AND (ed.DocumentDate=@DocumentDate OR (@DocumentDate IS NULL AND ed.DocumentDate IS NULL  ) )
            AND (ed.EntrantDocumentID <> @EntrantDocumentID OR (@EntrantDocumentID IS NULL AND ed.EntrantDocumentID IS NULL))
		END
END ELSE BEGIN
		SELECT TOP 1 ed.EntrantDocumentID 
		FROM EntrantDocument ed (NOLOCK) INNER JOIN EntrantDocumentIdentity edi (NOLOCK) on ed.EntrantDocumentID=edi.EntrantDocumentID
		WHERE ed.EntrantID=@EntrantID AND 
		ed.DocumentTypeID=@DocumentTypeID 
		AND edi.IdentityDocumentTypeID=@IdentityDocumentTypeID 
		AND ed.DocumentNumber=@DocumentNumber 
		AND (ed.DocumentSeries=@DocumentSeries OR (@DocumentSeries IS NULL AND ed.DocumentSeries IS NULL  ) ) 
		AND (ed.DocumentDate=@DocumentDate OR (@DocumentDate IS NULL AND ed.DocumentDate IS NULL  ) )
        AND (ed.EntrantDocumentID <> @EntrantDocumentID OR (@EntrantDocumentID IS NULL AND ed.EntrantDocumentID IS NULL))
END
";
            #endregion
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("DocumentTypeID", SqlDbType.Int) { Value = doc.DocumentTypeID });
                com.Parameters.Add(new SqlParameter("IdentityDocumentTypeID", SqlDbType.Int) { Value = (doc.EntDocIdentity == null) ? ((object)DBNull.Value) : doc.EntDocIdentity.IdentityDocumentTypeID });
                com.Parameters.Add(new SqlParameter("DocumentName", SqlDbType.VarChar) { Value = (doc.DocumentName == null) ? ((object)DBNull.Value) : doc.DocumentName });
                com.Parameters.Add(new SqlParameter("DocumentSeries", SqlDbType.VarChar) { Value = (doc.DocumentSeries == null) ? ((object)DBNull.Value) : doc.DocumentSeries });
                com.Parameters.Add(new SqlParameter("DocumentNumber", SqlDbType.VarChar) { Value = (doc.DocumentNumber == null) ? ((object)DBNull.Value) : doc.DocumentNumber });
                com.Parameters.Add(new SqlParameter("DocumentDate", SqlDbType.DateTime) { Value = (doc.DocumentDate == null) ? ((object)DBNull.Value) : doc.DocumentDate });
                //com.Parameters.Add(new SqlParameter("DocumentName", SqlDbType.VarChar) { Value = (doc.EntDocCustom == null) ? ((object)DBNull.Value) : doc.EntDocCustom.DocumentName });
                com.Parameters.Add(new SqlParameter("OlympicID", SqlDbType.Int) { Value = (doc.EntDocOlymp == null) ? ((object)DBNull.Value) : doc.EntDocOlymp.OlympicID });
                com.Parameters.Add(new SqlParameter("EntrantID", SqlDbType.Int) { Value = doc.EntrantID });
                com.Parameters.Add(new SqlParameter("EntrantDocumentID", SqlDbType.Int) { Value = doc.EntrantDocumentID });

                // TODO: Доделать проверку на все документы.
                try
                {
                    con.Open();
                    var o = com.ExecuteScalar();
                    if (o != null)
                    {
                        id = (int)o;
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
            return id;
        }

        public static int setEntrantDocument(EntrantDocumentViewModel doc)
        {
            int entrantDocumentId = 0;
            if (doc.EntrantDocumentID == 0)
            {
                entrantDocumentId = insEntrantDocument(doc);
            }
            else
            {
                entrantDocumentId = updEntrantDocument(doc);
            }

            //Проверка достоверности сведений по ОШ\ВОШ
            if ((entrantDocumentId != 0) && (doc.DocumentTypeID == 9 || doc.DocumentTypeID == 10) && (doc.EntDocOlymp != null))
            {
                SPResult checkResult = ApplicationSQL.FindEntrantOlympic(doc.EntrantDocumentID, doc.EntDocOlymp.OlympicID, doc.EntDocOlymp.OlympicTypeProfileID);
            }
            return entrantDocumentId;
        }

        private static int insEntrantDocument(EntrantDocumentViewModel doc)
        {

            // Добавить проверку на дубликаты
            // SELECT TOP 1 ed.EntrantDocumentID FROM EntrantDocument ed WHERE ed.DocumentTypeID=@DocumentTypeID AND ed.DocumentSeries=@DocumentSeries AND ed.DocumentNumber=@DocumentNumber AND ed.DocumentDate=@DocumentDate 

           string sqlBEGIN = @"
DECLARE @EntrantDocumentID	int	
DECLARE @newAttachmentID	int

SET NOCOUNT ON;

IF @AttachmentFileID IS NOT NULL BEGIN
	SELECT @newAttachmentID=AttachmentID FROM Attachment (NOLOCK) WHERE FileID=@AttachmentFileID
END

--BEGIN TRANSACTION Tran1; 
 IF @DocumentTypeID=1 BEGIN	-- Если документ УЛ, то 
	UPDATE Entrant SET PersonId=null WHERE EntrantID=@EntrantID
 END
";
            string sqlEND = @"
--COMMIT TRANSACTION Tran1;
SET NOCOUNT OFF;
SELECT @EntrantDocumentID as EntrantDocumentID ";

            StringBuilder sbsql = new StringBuilder();
            List<SqlParameter> prms = new List<SqlParameter>();
            SqlCommand com = new SqlCommand();

            sbsql.Append(sqlBEGIN);
            #region Вставка в EntrantDocument
            sbsql.Append(
@"
INSERT INTO [dbo].[EntrantDocument] ( EntrantID, DocumentTypeID,DocumentName,DocumentSeries, DocumentNumber, DocumentDate, DocumentOrganization, UID, DocumentSpecificData, AttachmentID) 
	 								  VALUES   (@EntrantID,@DocumentTypeID,@DocumentName,@DocumentSeries,@DocumentNumber,@DocumentDate,@DocumentOrganization,@UID, '', @newAttachmentID)
SELECT @EntrantDocumentID=cast(scope_identity() as int); 

IF @ApplicationID IS NOT NULL BEGIN
	INSERT INTO ApplicationEntrantDocument ( ApplicationID, EntrantDocumentID, AttachedDocumentType) VALUES   (@ApplicationID,@EntrantDocumentID,@DocumentTypeID)
END
");
            com.Parameters.Add(new SqlParameter("ApplicationID", SqlDbType.Int) { Value = doc.ApplicationID });
            com.Parameters.Add(new SqlParameter("EntrantID", SqlDbType.Int) { Value = doc.EntrantID });
            com.Parameters.Add(new SqlParameter("DocumentTypeID", SqlDbType.Int) { Value = doc.DocumentTypeID });
            com.Parameters.Add(new SqlParameter("DocumentName", SqlDbType.VarChar) { Value = (doc.DocumentName == null) ? ((object)DBNull.Value) : doc.DocumentName });          
            com.Parameters.Add(new SqlParameter("DocumentSeries", SqlDbType.VarChar) { Value = (doc.DocumentSeries == null) ? ((object)DBNull.Value) : doc.DocumentSeries });
            com.Parameters.Add(new SqlParameter("DocumentNumber", SqlDbType.VarChar) { Value = (doc.DocumentNumber == null) ? ((object)DBNull.Value) : doc.DocumentNumber });
            com.Parameters.Add(new SqlParameter("DocumentDate", SqlDbType.DateTime) { Value = (doc.DocumentDate == null) ? ((object)DBNull.Value) : doc.DocumentDate });
            com.Parameters.Add(new SqlParameter("DocumentOrganization", SqlDbType.VarChar) { Value = (doc.DocumentOrganization == null) ? ((object)DBNull.Value) : doc.DocumentOrganization });

            com.Parameters.Add(new SqlParameter("UID", SqlDbType.VarChar) { Value = (doc.UID == null) ? ((object)DBNull.Value) : doc.UID });
            if (doc.AttachmentFileID != null && doc.AttachmentFileID != Guid.Empty)
            {
                com.Parameters.Add(new SqlParameter("AttachmentFileID", SqlDbType.UniqueIdentifier) { Value = doc.AttachmentFileID });              //ed.AttachmentID
            }
            else
            {
                com.Parameters.Add(new SqlParameter("AttachmentFileID", DBNull.Value));
            }
            #endregion
            #region Вставка в EntrantDocumentIdentity
            if (doc.EntDocIdentity != null)
            {
                sbsql.Append(
@"
INSERT INTO [EntrantDocumentIdentity]( EntrantDocumentID, IdentityDocumentTypeID, GenderTypeID, NationalityTypeID, BirthDate, BirthPlace, SubdivisionCode, LastName, FirstName, MiddleName)
									  VALUES    (@EntrantDocumentID,@edi_IdentityDocumentTypeID,@edi_GenderTypeID,@edi_NationalityTypeID,@edi_BirthDate,@edi_BirthPlace,@edi_SubdivisionCode, @edi_LastName, @edi_FirstName, @edi_MiddleName)
");
                //com.Parameters.Add(new SqlParameter("EntrantDocumentID",SqlDbType.Int) { Value=doc.EntDocIdentity.EntrantDocumentID });
                com.Parameters.Add(new SqlParameter("edi_IdentityDocumentTypeID", SqlDbType.Int) { Value = doc.EntDocIdentity.IdentityDocumentTypeID });
                com.Parameters.Add(new SqlParameter("edi_GenderTypeID", SqlDbType.Int) { Value = doc.EntDocIdentity.GenderTypeID });
                com.Parameters.Add(new SqlParameter("edi_NationalityTypeID", SqlDbType.Int) { Value = doc.EntDocIdentity.NationalityTypeID });
                com.Parameters.Add(new SqlParameter("edi_BirthDate", SqlDbType.DateTime) { Value = (doc.EntDocIdentity.BirthDate == null) ? ((object)DBNull.Value) : doc.EntDocIdentity.BirthDate });
                com.Parameters.Add(new SqlParameter("edi_BirthPlace", SqlDbType.VarChar) { Value = (doc.EntDocIdentity.BirthPlace == null) ? ((object)DBNull.Value) : doc.EntDocIdentity.BirthPlace });
                com.Parameters.Add(new SqlParameter("edi_SubdivisionCode", SqlDbType.VarChar) { Value = (doc.EntDocIdentity.SubdivisionCode == null) ? ((object)DBNull.Value) : doc.EntDocIdentity.SubdivisionCode });

                com.Parameters.Add(new SqlParameter("edi_LastName", SqlDbType.VarChar) { Value = (doc.EntDocIdentity.LastName == null) ? ((object)DBNull.Value) : doc.EntDocIdentity.LastName });
                com.Parameters.Add(new SqlParameter("edi_FirstName", SqlDbType.VarChar) { Value = (doc.EntDocIdentity.FirstName == null) ? ((object)DBNull.Value) : doc.EntDocIdentity.FirstName });
                com.Parameters.Add(new SqlParameter("edi_MiddleName", SqlDbType.VarChar) { Value = (doc.EntDocIdentity.MiddleName == null) ? ((object)DBNull.Value) : doc.EntDocIdentity.MiddleName });
            }
            #endregion
            #region Вставка в EntrantDocumentEdu
            if (doc.EntDocEdu != null)
            {
                sbsql.Append(@"
    INSERT INTO dbo.EntrantDocumentEdu 
     VALUES(@EntrantDocumentID, @edu_RegistrationNumber, @edu_InstitutionAddress,  @edu_InstitutionName, @edu_FacultyName,
            @edu_SpecialityName, @edu_QualificationName, @edu_BeginDate, @edu_EndDate, @edu_EducationFormID,  @edu_GPA, @edu_IsNostrificated,
            @edu_StateServicePreparation, null, null, null, null, null, null, null, @edu_CountryID, 0,0,0,0, null, null, null, null, 0, null, null, null, null, null, null, null)
                               ");
                //com.Parameters.Add(new SqlParameter("EntrantDocumentID", SqlDbType.Int) { Value = doc.EntDocEdu.EntrantDocumentID });
                com.Parameters.Add(new SqlParameter("edu_RegistrationNumber", SqlDbType.VarChar) { Value = (doc.EntDocEdu.RegistrationNumber == null) ? ((object)DBNull.Value) : doc.EntDocEdu.RegistrationNumber });
                com.Parameters.Add(new SqlParameter("edu_InstitutionAddress", SqlDbType.VarChar) { Value = (doc.EntDocEdu.InstitutionAddress == null) ? ((object)DBNull.Value) : doc.EntDocEdu.InstitutionAddress });
                com.Parameters.Add(new SqlParameter("edu_InstitutionName", SqlDbType.VarChar) { Value = (doc.EntDocEdu.InstitutionName == null) ? ((object)DBNull.Value) : doc.EntDocEdu.InstitutionName });
                com.Parameters.Add(new SqlParameter("edu_FacultyName", SqlDbType.VarChar) { Value = (doc.EntDocEdu.FacultyName == null) ? ((object)DBNull.Value) : doc.EntDocEdu.FacultyName });
                com.Parameters.Add(new SqlParameter("edu_SpecialityName", SqlDbType.VarChar) { Value = (doc.EntDocEdu.SpecialityName == null) ? ((object)DBNull.Value) : doc.EntDocEdu.SpecialityName });
                com.Parameters.Add(new SqlParameter("edu_QualificationName", SqlDbType.VarChar) { Value = (doc.EntDocEdu.QualificationName == null) ? ((object)DBNull.Value) : doc.EntDocEdu.QualificationName });
                com.Parameters.Add(new SqlParameter("edu_BeginDate", SqlDbType.DateTime) { Value = (doc.EntDocEdu.BeginDate == null) ? ((object)DBNull.Value) : doc.EntDocEdu.BeginDate });
                com.Parameters.Add(new SqlParameter("edu_EndDate", SqlDbType.DateTime) { Value = (doc.EntDocEdu.EndDate == null) ? ((object)DBNull.Value) : doc.EntDocEdu.EndDate });
                com.Parameters.Add(new SqlParameter("edu_EducationFormID", SqlDbType.TinyInt) { Value = (doc.EntDocEdu.EducationFormID == null) ? 0 : doc.EntDocEdu.EducationFormID });
                com.Parameters.Add(new SqlParameter("edu_GPA", SqlDbType.Real) { Value = (doc.EntDocEdu.GPA == null) ? ((object)DBNull.Value) : doc.EntDocEdu.GPA });
                com.Parameters.Add(new SqlParameter("edu_IsNostrificated", SqlDbType.Bit) { Value = (doc.EntDocEdu.IsNostrificated == null) ? ((object)DBNull.Value) : doc.EntDocEdu.IsNostrificated });
                com.Parameters.Add(new SqlParameter("edu_StateServicePreparation", SqlDbType.Bit) { Value = (doc.EntDocEdu.StateServicePreparation == null) ? ((object)DBNull.Value) : doc.EntDocEdu.StateServicePreparation });
                com.Parameters.Add(new SqlParameter("edu_CountryID", SqlDbType.Int) { Value = (doc.EntDocEdu.CountryID==null)?((object)DBNull.Value): doc.EntDocEdu.CountryID});
                
                

                if (doc.DocumentOrganization == null && doc.EntDocEdu.DocumentOU != null)
                {
                    com.Parameters["DocumentOrganization"].Value = (doc.EntDocEdu.DocumentOU == null) ? ((object)DBNull.Value) : (doc.EntDocEdu.DocumentOU);
                }
            }
            #endregion
            #region  Вставка в EntrantDocumentOlympic
            if (doc.EntDocOlymp != null)
            {
                sbsql.Append(" \r\n INSERT INTO EntrantDocumentOlympic(EntrantDocumentID, DiplomaTypeID, OlympicID, ClassNumber, OlympicTypeProfileID, ProfileSubjectID, EgeSubjectID) VALUES (@EntrantDocumentID, @edo_DiplomaTypeID, @edo_OlympicID,  @edo_ClassNumber, @edo_OlympicTypeProfileID, @edo_ProfileSubjectID, @edo_EgeSubjectID) \r\n");
                com.Parameters.Add(new SqlParameter("edo_DiplomaTypeID", SqlDbType.Int) { Value = doc.EntDocOlymp.DiplomaTypeID });
                com.Parameters.Add(new SqlParameter("edo_OlympicID", SqlDbType.Int) { Value = doc.EntDocOlymp.OlympicID });
                com.Parameters.Add(new SqlParameter("edo_ClassNumber", SqlDbType.Int) { Value = doc.EntDocOlymp.FormNumberID });
                com.Parameters.Add(new SqlParameter("edo_OlympicTypeProfileID", SqlDbType.Int) { Value = doc.EntDocOlymp.OlympicTypeProfileID });
                com.Parameters.Add(new SqlParameter("edo_ProfileSubjectID", SqlDbType.Int) { Value = doc.EntDocOlymp.ProfileSubjectID.GetValueOrDefault() == 0 ? (object)DBNull.Value : (object)doc.EntDocOlymp.ProfileSubjectID });
                com.Parameters.Add(new SqlParameter("edo_EgeSubjectID", SqlDbType.Int) { Value = doc.EntDocOlymp.EgeSubjectID.GetValueOrDefault() == 0 ? (object)DBNull.Value : (object)doc.EntDocOlymp.EgeSubjectID });
            }
            #endregion

            #region  Вставка Other
            if (doc.EntDocOther != null)
            {
                if(doc.DocumentTypeID == 27)
                { 
                    sbsql.Append(" \r\n INSERT INTO EntrantDocumentUkraineOlympic(" + 
                        "EntrantDocumentID, DiplomaTypeID, Profile, OlympicPlace, OlympicDate) " +
                        "VALUES (@EntrantDocumentID, @edo_DiplomaTypeID, @edo_Profile, @edo_Place, @edo_Date) \r\n");
                    com.Parameters.Add(new SqlParameter("edo_DiplomaTypeID", SqlDbType.Int) { Value = doc.EntDocOther.DiplomaTypeID });                    
                    com.Parameters.Add(new SqlParameter("edo_Profile", SqlDbType.VarChar) { Value = doc.EntDocOther.OlympicProfile });
                    com.Parameters.Add(new SqlParameter("edo_Place", SqlDbType.VarChar) { Value = (doc.EntDocOther.OlympicPlace == null) ? ((object)DBNull.Value) : doc.EntDocOther.OlympicPlace });
                    com.Parameters.Add(new SqlParameter("edo_Date", SqlDbType.DateTime) { Value = (doc.EntDocOther.OlympicDate == null) ? ((object)DBNull.Value) : doc.EntDocOther.OlympicDate });
                }
                if (doc.DocumentTypeID == 28)
                {
                    sbsql.Append(" \r\n INSERT INTO EntrantDocumentInternationalOlympic(" +
                        "EntrantDocumentID, CountryID, Profile, OlympicPlace, OlympicDate) " +
                        "VALUES (@EntrantDocumentID, @edo_CountryID,  @edo_Profile, @edo_Place, @edo_Date) \r\n");
                    com.Parameters.Add(new SqlParameter("edo_CountryID", SqlDbType.Int) { Value = doc.EntDocOther.CountryID });
                    com.Parameters.Add(new SqlParameter("edo_Profile", SqlDbType.VarChar) { Value = (doc.EntDocOther.OlympicProfile == null) ? ((object)DBNull.Value) : doc.EntDocOther.OlympicProfile });
                    com.Parameters.Add(new SqlParameter("edo_Place", SqlDbType.VarChar) { Value = (doc.EntDocOther.OlympicPlace == null) ? ((object)DBNull.Value) : doc.EntDocOther.OlympicPlace });
                    com.Parameters.Add(new SqlParameter("edo_Date", SqlDbType.DateTime) { Value = (doc.EntDocOther.OlympicDate == null) ? ((object)DBNull.Value) : doc.EntDocOther.OlympicDate });
                }
                if (doc.DocumentTypeID == 29)
                {
                    sbsql.Append(" \r\n INSERT INTO EntrantDocumentCompatriot(" +
                        "EntrantDocumentID, CompatriotCategoryID, CompatriotStatus) " +
                        "VALUES (@EntrantDocumentID, @edo_CompatriotCategoryID, @edo_CompatriotStatus) \r\n");
                    com.Parameters.Add(new SqlParameter("edo_CompatriotCategoryID", SqlDbType.Int) { Value = doc.EntDocOther.CompatriotCategoryID });
                    com.Parameters.Add(new SqlParameter("edo_CompatriotStatus", SqlDbType.VarChar) { Value = doc.EntDocOther.CompatriotStatus });
                    //com.Parameters.Add(new SqlParameter("edo_Name", SqlDbType.VarChar) { Value = doc.EntDocOther.DocumentName });
                }
                if (doc.DocumentTypeID == 30)
                {
                    sbsql.Append(" \r\n INSERT INTO EntrantDocumentOrphan(" +
                        "EntrantDocumentID, OrphanCategoryID) " +
                        "VALUES (@EntrantDocumentID, @edo_OrphanCategoryID) \r\n");
                    com.Parameters.Add(new SqlParameter("edo_OrphanCategoryID", SqlDbType.Int) { Value = doc.EntDocOther.OrphanCategoryID });
                    //com.Parameters.Add(new SqlParameter("edo_Name", SqlDbType.VarChar) { Value = doc.EntDocOther.DocumentName });
                }
                if (doc.DocumentTypeID == 31)
                {
                    sbsql.Append(" \r\n INSERT INTO EntrantDocumentVeteran(" +
                        "EntrantDocumentID, VeteranCategoryID) " +
                        "VALUES (@EntrantDocumentID, @edo_VeteranCategoryID) \r\n");
                    com.Parameters.Add(new SqlParameter("edo_VeteranCategoryID", SqlDbType.Int) { Value = doc.EntDocOther.VeteranCategoryID });
                    //com.Parameters.Add(new SqlParameter("edo_Name", SqlDbType.VarChar) { Value = doc.EntDocOther.DocumentName });
                }
                if (doc.DocumentTypeID == 33)
                {
                    sbsql.Append(" \r\n INSERT INTO EntrantDocumentParentsLost(" +
                        "EntrantDocumentID, ParentsLostCategoryID) " +
                        "VALUES (@EntrantDocumentID, @edo_ParentsLostCategoryID) \r\n");
                    com.Parameters.Add(new SqlParameter("edo_ParentsLostCategoryID", SqlDbType.Int) { Value = doc.EntDocOther.ParentsLostCategoryID });
                }                
                if (doc.DocumentTypeID == 34)
                {
                    sbsql.Append(" \r\n INSERT INTO EntrantDocumentStateEmployee(" +
                        "EntrantDocumentID, StateEmployeeCategoryID) " +
                        "VALUES (@EntrantDocumentID, @edo_StateEmployeeCategoryID) \r\n");
                    com.Parameters.Add(new SqlParameter("edo_StateEmployeeCategoryID", SqlDbType.Int) { Value = doc.EntDocOther.StateEmployeeCategoryID });
                }
                if (doc.DocumentTypeID == 35)
                {
                    sbsql.Append(" \r\n INSERT INTO EntrantDocumentRadiationWork(" +
                        "EntrantDocumentID, RadiationWorkCategoryID) " +
                        "VALUES (@EntrantDocumentID, @edo_RadiationWorkCategoryID) \r\n");
                    com.Parameters.Add(new SqlParameter("edo_RadiationWorkCategoryID", SqlDbType.Int) { Value = doc.EntDocOther.RadiationWorkCategoryID });
                }              
            }
            #endregion

            #region  Вставка в EntrantDocumentCustom
            if (doc.EntDocCustom != null)
            {
                sbsql.Append("\r\n INSERT INTO EntrantDocumentCustom(EntrantDocumentID, AdditionalInfo) VALUES (@EntrantDocumentID, @cust_AdditionalInfo) \r\n");
                com.Parameters.Add(new SqlParameter("cust_AdditionalInfo", SqlDbType.VarChar) { Value = (doc.EntDocCustom.AdditionalInfo == null) ? ((object)DBNull.Value) : doc.EntDocCustom.AdditionalInfo });
                //com.Parameters.Add(new SqlParameter("DocumentName", SqlDbType.VarChar) { Value = (doc.EntDocCustom.DocumentName == null) ? ((object)DBNull.Value) : doc.EntDocCustom.DocumentName });
            }
            #endregion
            #region  Вставка в EntrantDocumentDisability
            if (doc.EntDocDis != null)
            {
                sbsql.Append("\r\n	INSERT INTO EntrantDocumentDisability(EntrantDocumentID, DisabilityTypeID) VALUES (@EntrantDocumentID, @dic_DisabilityTypeID) \r\n ");
                com.Parameters.Add(new SqlParameter("dic_DisabilityTypeID", SqlDbType.Int) { Value = doc.EntDocDis.DisabilityTypeID });
            }
            #endregion
            #region Вставка в EntrantDocumentEgeAndOlympicSubject
            if (doc.EntDocSubBall != null)
            {
                if (doc.EntDocSubBall.SubjectBalls != null)
                {
                    foreach (SubjectBall sb in doc.EntDocSubBall.SubjectBalls)
                    {
                        sbsql.AppendFormat(" INSERT INTO EntrantDocumentEgeAndOlympicSubject(EntrantDocumentID, SubjectID, [Value])VALUES(@EntrantDocumentID, {0},{1})\r\n", sb.SubjectID, sb.Value);
                    }
                }
            }
            #endregion
            #region Вставка в EntrantDocumentEge
            if (doc.EntDocEge != null)
            {
                sbsql.Append("\r\n	INSERT INTO EntrantDocumentEge(EntrantDocumentID, TypographicNumber) VALUES (@EntrantDocumentID, @ege_TypographicNumber) \r\n ");
                com.Parameters.Add(new SqlParameter("ege_TypographicNumber", SqlDbType.VarChar) { Value = (doc.EntDocEge.TypographicNumber == null) ? ((object)DBNull.Value) : doc.EntDocEge.TypographicNumber });
            }
            #endregion

            sbsql.Append(sqlEND);
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                com.Connection = con;
                com.CommandText = sbsql.ToString();
                com.CommandTimeout = SQL.TIMEOUT;
                try
                {
                    con.Open();
                    object o = null;// Вставить и вернуть новый ID
                    SqlDataReader r = com.ExecuteReader();
                    if (r.Read())
                    {
                        o = r["EntrantDocumentID"];
                        if (o != null && o != DBNull.Value) { doc.EntrantDocumentID = Convert.ToInt32(o); }
                    }
                    r.Close(); 
                }
                catch (SqlException e)
                {
                    throw e;    // Пробросить дальше
                }
                catch (Exception e)
                {
                    throw e; // Пробросить дальше
                } 
            }

            return doc.EntrantDocumentID;
        }

        private static int updEntrantDocument(EntrantDocumentViewModel doc)
        {
            string sqlBEGIN = @"
DECLARE @newAttachmentID	int
DECLARE @oldAttachmentID	int
DECLARE @oldAttachmentFileID uniqueidentifier

SET NOCOUNT ON;

SELECT @oldAttachmentID=a.AttachmentID, @oldAttachmentFileID=a.FileID 
FROM [EntrantDocument] ed (NOLOCK) 
INNER JOIN Attachment a (NOLOCK) on ed.AttachmentID=a.AttachmentID
WHERE EntrantDocumentID=@EntrantDocumentID

IF @AttachmentFileID IS NOT NULL BEGIN
	SELECT @newAttachmentID=AttachmentID FROM Attachment (NOLOCK) WHERE FileID=@AttachmentFileID
END

--BEGIN TRANSACTION Tran1; 
 IF @DocumentTypeID=1 BEGIN	-- Если документ УЛ, то 
	EXEC [dbo].[SyncEntrant] @entrantId = @EntrantID
    --UPDATE Entrant SET PersonId=null WHERE EntrantID=@EntrantID
 END

";
            string sqlEND = @"
--COMMIT TRANSACTION Tran1;
SET NOCOUNT OFF;
SELECT @EntrantDocumentID as EntrantDocumentID ";

            StringBuilder sbsql = new StringBuilder();
            SqlCommand com = new SqlCommand();

            sbsql.Append(sqlBEGIN);
            #region Обновление EntrantDocument
            sbsql.Append(@"
UPDATE [EntrantDocument] With (ROWLOCK)
SET EntrantID=@EntrantID
	,UID=@UID
	,DocumentTypeID=@DocumentTypeID
    ,DocumentName=@DocumentName
	,DocumentSeries=@DocumentSeries
	,DocumentNumber=@DocumentNumber
	,DocumentDate=@DocumentDate
	,DocumentOrganization=@DocumentOrganization
--	,DocumentSpecificData=''
--	,AttachmentID=@newAttachmentID
WHERE EntrantDocumentID=@EntrantDocumentID

IF @newAttachmentID IS NOT NULL BEGIN
	UPDATE EntrantDocument  SET AttachmentID=@newAttachmentID WHERE EntrantDocumentID=@EntrantDocumentID
END
IF @oldAttachmentID IS NOT NULL AND @AttachmentID IS NULL BEGIN
	UPDATE EntrantDocument  SET AttachmentID=null WHERE EntrantDocumentID=@EntrantDocumentID
END
IF (@oldAttachmentID IS NOT NULL AND (@AttachmentID IS NULL OR @newAttachmentID <> @oldAttachmentID ) )  BEGIN
	DELETE FROM Attachment WHERE AttachmentID=@oldAttachmentID
END

");
            com.Parameters.Add(new SqlParameter("EntrantDocumentID", SqlDbType.Int) { Value = doc.EntrantDocumentID });
            com.Parameters.Add(new SqlParameter("EntrantID", SqlDbType.Int) { Value = doc.EntrantID });
            com.Parameters.Add(new SqlParameter("DocumentTypeID", SqlDbType.Int) { Value = doc.DocumentTypeID });
            com.Parameters.Add(new SqlParameter("DocumentName", SqlDbType.VarChar) { Value = (doc.DocumentName == null) ? ((object)DBNull.Value) : doc.DocumentName });
            com.Parameters.Add(new SqlParameter("DocumentSeries", SqlDbType.VarChar) { Value = (doc.DocumentSeries == null) ? ((object)DBNull.Value) : doc.DocumentSeries });
            com.Parameters.Add(new SqlParameter("DocumentNumber", SqlDbType.VarChar) { Value = (doc.DocumentNumber == null) ? ((object)DBNull.Value) : doc.DocumentNumber });
            com.Parameters.Add(new SqlParameter("DocumentDate", SqlDbType.DateTime) { Value = (doc.DocumentDate == null) ? ((object)DBNull.Value) : doc.DocumentDate });
            com.Parameters.Add(new SqlParameter("DocumentOrganization", SqlDbType.VarChar) { Value = (doc.DocumentOrganization == null) ? ((object)DBNull.Value) : doc.DocumentOrganization });
            com.Parameters.Add(new SqlParameter("UID", SqlDbType.VarChar) { Value = (doc.UID == null) ? ((object)DBNull.Value) : doc.UID });
            com.Parameters.Add(new SqlParameter("AttachmentID", SqlDbType.Int) { Value = (doc.AttachmentID == null) ? ((object)DBNull.Value) : doc.AttachmentID });
            if (doc.AttachmentFileID != null && doc.AttachmentFileID != Guid.Empty)
            {
                com.Parameters.Add(new SqlParameter("AttachmentFileID", SqlDbType.UniqueIdentifier) { Value = doc.AttachmentFileID });              //ed.AttachmentID
            }
            else
            {
                com.Parameters.Add(new SqlParameter("AttachmentFileID", DBNull.Value));
            }
            #endregion
            #region Обновление EntrantDocumentIdentity
            if (doc.EntDocIdentity != null)
            {
                // Вставка в EntrantDocumentIdentity
                sbsql.Append(@"
UPDATE [EntrantDocumentIdentity] With (ROWLOCK)
SET IdentityDocumentTypeID=@edi_IdentityDocumentTypeID
	,GenderTypeID=@edi_GenderTypeID
	,NationalityTypeID=@edi_NationalityTypeID
	,BirthDate=@edi_BirthDate
	,BirthPlace=@edi_BirthPlace
	,SubdivisionCode=@edi_SubdivisionCode
	,LastName=@edi_LastName
	,FirstName=@edi_FirstName
	,MiddleName=@edi_MiddleName
WHERE EntrantDocumentID=@EntrantDocumentID
");
                //com.Parameters.Add(new SqlParameter("EntrantDocumentID",SqlDbType.Int) { Value=doc.EntDocIdentity.EntrantDocumentID });
                com.Parameters.Add(new SqlParameter("edi_IdentityDocumentTypeID", SqlDbType.Int) { Value = doc.EntDocIdentity.IdentityDocumentTypeID });
                com.Parameters.Add(new SqlParameter("edi_GenderTypeID", SqlDbType.Int) { Value = doc.EntDocIdentity.GenderTypeID });
                com.Parameters.Add(new SqlParameter("edi_NationalityTypeID", SqlDbType.Int) { Value = doc.EntDocIdentity.NationalityTypeID });
                com.Parameters.Add(new SqlParameter("edi_BirthDate", SqlDbType.DateTime) { Value = doc.EntDocIdentity.BirthDate });
                com.Parameters.Add(new SqlParameter("edi_BirthPlace", SqlDbType.VarChar) { Value = (doc.EntDocIdentity.BirthPlace == null) ? ((object)DBNull.Value) : doc.EntDocIdentity.BirthPlace });
                com.Parameters.Add(new SqlParameter("edi_SubdivisionCode", SqlDbType.VarChar) { Value = (doc.EntDocIdentity.SubdivisionCode == null) ? ((object)DBNull.Value) : doc.EntDocIdentity.SubdivisionCode });

                com.Parameters.Add(new SqlParameter("edi_LastName", SqlDbType.VarChar) { Value = (doc.EntDocIdentity.LastName == null) ? ((object)DBNull.Value) : doc.EntDocIdentity.LastName });
                com.Parameters.Add(new SqlParameter("edi_FirstName", SqlDbType.VarChar) { Value = (doc.EntDocIdentity.FirstName == null) ? ((object)DBNull.Value) : doc.EntDocIdentity.FirstName });
                com.Parameters.Add(new SqlParameter("edi_MiddleName", SqlDbType.VarChar) { Value = (doc.EntDocIdentity.MiddleName == null) ? ((object)DBNull.Value) : doc.EntDocIdentity.MiddleName });

            }
            #endregion
            #region Обновление EntrantDocumentEdu
            if (doc.EntDocEdu != null)
            { 
                sbsql.Append(@"
IF EXISTS(SELECT EntrantDocumentID FROM EntrantDocumentEdu (NOLOCK) WHERE EntrantDocumentID=@EntrantDocumentID  ) BEGIN
	UPDATE [EntrantDocumentEdu]
	SET	RegistrationNumber=@edu_RegistrationNumber,
        InstitutionName =@edu_InstitutionName,
        InstitutionAddress =@edu_InstitutionAddress,
        FacultyName = @edu_FacultyName,
        SpecialityName = @edu_SpecialityName,
        QualificationName =@edu_QualificationName,
        BeginDate =@edu_BeginDate,
        EndDate =@edu_EndDate,
        EducationFormID = ISNULL(@edu_EducationFormID,0),
		GPA					=@edu_GPA,
        IsNostrificated		=@edu_IsNostrificated,
        StateServicePreparation		=@edu_StateServicePreparation,
        CountryID = @edu_CountryID
	WHERE EntrantDocumentID=@EntrantDocumentID

END ELSE BEGIN
	INSERT INTO EntrantDocumentEdu VALUES (@EntrantDocumentID, @edu_RegistrationNumber, @edu_InstitutionAddress, @edu_InstitutionName, @edu_FacultyName,
            @edu_SpecialityName, @edu_QualificationName, @edu_BeginDate, @edu_EndDate, @edu_EducationFormID,  @edu_GPA, @edu_IsNostrificated,
            @edu_StateServicePreparation, null, null, null, null, null, null, null, @edu_CountryID, 0,0,0,0, null, null, null, null, 0, null, null, null, null, null, null, null)
END
");

                com.Parameters.Add(new SqlParameter("edu_RegistrationNumber", SqlDbType.VarChar) { Value = (doc.EntDocEdu.RegistrationNumber == null) ? ((object)DBNull.Value) : doc.EntDocEdu.RegistrationNumber });
                com.Parameters.Add(new SqlParameter("edu_InstitutionAddress", SqlDbType.VarChar) { Value = (doc.EntDocEdu.InstitutionAddress == null) ? ((object)DBNull.Value) : doc.EntDocEdu.InstitutionAddress });
                com.Parameters.Add(new SqlParameter("edu_InstitutionName", SqlDbType.VarChar) { Value = (doc.EntDocEdu.InstitutionName == null) ? ((object)DBNull.Value) : doc.EntDocEdu.InstitutionName });
                com.Parameters.Add(new SqlParameter("edu_FacultyName", SqlDbType.VarChar) { Value = (doc.EntDocEdu.FacultyName == null) ? ((object)DBNull.Value) : doc.EntDocEdu.FacultyName });
                com.Parameters.Add(new SqlParameter("edu_SpecialityName", SqlDbType.VarChar) { Value = (doc.EntDocEdu.SpecialityName == null) ? ((object)DBNull.Value) : doc.EntDocEdu.SpecialityName });
                com.Parameters.Add(new SqlParameter("edu_QualificationName", SqlDbType.VarChar) { Value = (doc.EntDocEdu.QualificationName == null) ? ((object)DBNull.Value) : doc.EntDocEdu.QualificationName });
                com.Parameters.Add(new SqlParameter("edu_BeginDate", SqlDbType.DateTime) { Value = (doc.EntDocEdu.BeginDate == null) ? ((object)DBNull.Value) : doc.EntDocEdu.BeginDate });
                com.Parameters.Add(new SqlParameter("edu_EndDate", SqlDbType.DateTime) { Value = (doc.EntDocEdu.EndDate == null) ? ((object)DBNull.Value) : doc.EntDocEdu.EndDate });
                com.Parameters.Add(new SqlParameter("edu_EducationFormID", SqlDbType.TinyInt) { Value = (doc.EntDocEdu.EducationFormID == null) ? 0 : doc.EntDocEdu.EducationFormID });
                com.Parameters.Add(new SqlParameter("edu_GPA", SqlDbType.Real) { Value = (doc.EntDocEdu.GPA == null) ? ((object)DBNull.Value) : doc.EntDocEdu.GPA });
                com.Parameters.Add(new SqlParameter("edu_IsNostrificated", SqlDbType.Bit) { Value = (doc.EntDocEdu.IsNostrificated == null) ? ((object)DBNull.Value) : doc.EntDocEdu.IsNostrificated });
                com.Parameters.Add(new SqlParameter("edu_StateServicePreparation", SqlDbType.Bit) { Value = (doc.EntDocEdu.StateServicePreparation == null) ? ((object)DBNull.Value) : doc.EntDocEdu.StateServicePreparation });
                com.Parameters.Add(new SqlParameter("edu_CountryID", SqlDbType.Int) { Value = (doc.EntDocEdu.CountryID == null) ? ((object)DBNull.Value) : doc.EntDocEdu.CountryID });


                if (doc.DocumentOrganization == null && doc.EntDocEdu.DocumentOU != null)
                {
                    com.Parameters["DocumentOrganization"].Value = (doc.EntDocEdu.DocumentOU == null) ? ((object)DBNull.Value) : (doc.EntDocEdu.DocumentOU);
                }
            }
            #endregion
            #region  Обновление EntrantDocumentOlympic
            if (doc.EntDocOlymp != null)
            {
                sbsql.Append("\r\n UPDATE EntrantDocumentOlympic SET DiplomaTypeID = @edo_DiplomaTypeID, OlympicID = @edo_OlympicID, ClassNumber = @edo_ClassNumber, OlympicTypeProfileID = @edo_OlympicTypeProfileID, ProfileSubjectID = @edo_ProfileSubjectID, EgeSubjectID = @edo_EgeSubjectID   WHERE EntrantDocumentID = @EntrantDocumentID \r\n");
                com.Parameters.Add(new SqlParameter("edo_DiplomaTypeID", SqlDbType.Int) { Value = doc.EntDocOlymp.DiplomaTypeID });
                com.Parameters.Add(new SqlParameter("edo_OlympicID", SqlDbType.Int) { Value = doc.EntDocOlymp.OlympicID });
                com.Parameters.Add(new SqlParameter("edo_ClassNumber", SqlDbType.Int) { Value = doc.EntDocOlymp.FormNumberID });
                com.Parameters.Add(new SqlParameter("edo_OlympicTypeProfileID", SqlDbType.Int) { Value = doc.EntDocOlymp.OlympicTypeProfileID });
                com.Parameters.Add(new SqlParameter("edo_ProfileSubjectID", SqlDbType.Int) { Value = doc.EntDocOlymp.ProfileSubjectID.GetValueOrDefault() == 0 ? (object)DBNull.Value : (object)doc.EntDocOlymp.ProfileSubjectID });
                com.Parameters.Add(new SqlParameter("edo_EgeSubjectID", SqlDbType.Int) { Value = doc.EntDocOlymp.EgeSubjectID.GetValueOrDefault() == 0 ? (object)DBNull.Value : (object)doc.EntDocOlymp.EgeSubjectID });
            }
            #endregion

            #region  Обновление Other
            if (doc.EntDocOther != null)
            {
                if (doc.DocumentTypeID == 27)
                {
                    sbsql.Append(" \r\n UPDATE EntrantDocumentUkraineOlympic SET " +
                    "DiplomaTypeID = @edo_DiplomaTypeID, Profile = @edo_Profile, " +
                    "OlympicPlace = @edo_Place, OlympicDate = @edo_Date " +
                    "WHERE EntrantDocumentID = @EntrantDocumentID ");
                    com.Parameters.Add(new SqlParameter("edo_DiplomaTypeID", SqlDbType.Int) { Value = doc.EntDocOther.DiplomaTypeID });                    
                    com.Parameters.Add(new SqlParameter("edo_Profile", SqlDbType.VarChar) { Value = doc.EntDocOther.OlympicProfile });
                    com.Parameters.Add(new SqlParameter("edo_Place", SqlDbType.VarChar) { Value = (doc.EntDocOther.OlympicPlace == null) ? ((object)DBNull.Value) : doc.EntDocOther.OlympicPlace });
                    com.Parameters.Add(new SqlParameter("edo_Date", SqlDbType.DateTime) { Value = (doc.EntDocOther.OlympicDate == null) ? ((object)DBNull.Value) : doc.EntDocOther.OlympicDate });
                }
                if (doc.DocumentTypeID == 28)
                {
                    sbsql.Append(" \r\n UPDATE EntrantDocumentInternationalOlympic SET " +
                    "CountryID = @edo_CountryID, Profile = @edo_Profile, " +
                    "OlympicPlace = @edo_Place, OlympicDate = @edo_Date " +
                    "WHERE EntrantDocumentID = @EntrantDocumentID ");
                    com.Parameters.Add(new SqlParameter("edo_CountryID", SqlDbType.Int) { Value = doc.EntDocOther.CountryID });
                    com.Parameters.Add(new SqlParameter("edo_Profile", SqlDbType.VarChar) { Value = (doc.EntDocOther.OlympicProfile == null) ? ((object)DBNull.Value) : doc.EntDocOther.OlympicProfile });
                    com.Parameters.Add(new SqlParameter("edo_Place", SqlDbType.VarChar) { Value = (doc.EntDocOther.OlympicPlace == null) ? ((object)DBNull.Value) : doc.EntDocOther.OlympicPlace });
                    com.Parameters.Add(new SqlParameter("edo_Date", SqlDbType.DateTime) { Value = (doc.EntDocOther.OlympicDate == null) ? ((object)DBNull.Value) : doc.EntDocOther.OlympicDate });
                }
                if (doc.DocumentTypeID == 29)
                {
                    sbsql.Append(" \r\n UPDATE EntrantDocumentCompatriot SET " +
                    "CompatriotCategoryID = @edo_CompatriotCategoryID," +
                    "CompatriotStatus = @edo_CompatriotStatus" +
                    "WHERE EntrantDocumentID = @EntrantDocumentID ");
                    com.Parameters.Add(new SqlParameter("edo_CompatriotCategoryID", SqlDbType.Int) { Value = doc.EntDocOther.CompatriotCategoryID });
                    com.Parameters.Add(new SqlParameter("edo_CompatriotStatus", SqlDbType.VarChar) { Value = doc.EntDocOther.CompatriotStatus });
                    //com.Parameters.Add(new SqlParameter("edo_Name", SqlDbType.VarChar) { Value = doc.EntDocOther.DocumentName });
                }
                if (doc.DocumentTypeID == 30)
                {
                    sbsql.Append(" \r\n UPDATE EntrantDocumentOrphan SET " +
                    "OrphanCategoryID = @edo_OrphanCategoryID " +
                    "WHERE EntrantDocumentID = @EntrantDocumentID ");
                    com.Parameters.Add(new SqlParameter("edo_OrphanCategoryID", SqlDbType.Int) { Value = doc.EntDocOther.OrphanCategoryID });
                    //com.Parameters.Add(new SqlParameter("edo_Name", SqlDbType.VarChar) { Value = doc.EntDocOther.DocumentName });
                }
                if (doc.DocumentTypeID == 31)
                {
                    sbsql.Append(" \r\n UPDATE EntrantDocumentVeteran SET " +
                    "VeteranCategoryID = @edo_VeteranCategoryID " +
                    "WHERE EntrantDocumentID = @EntrantDocumentID ");
                    com.Parameters.Add(new SqlParameter("edo_VeteranCategoryID", SqlDbType.Int) { Value = doc.EntDocOther.VeteranCategoryID });
                    //com.Parameters.Add(new SqlParameter("edo_Name", SqlDbType.VarChar) { Value = doc.EntDocOther.DocumentName });
                }
                if (doc.DocumentTypeID == 33)
                {
                    sbsql.Append(" \r\n UPDATE EntrantDocumentParentsLost SET " +
                    "ParentsLostCategoryId = @edo_ParentsLostCategoryID " +
                    "WHERE EntrantDocumentID = @EntrantDocumentID ");
                    com.Parameters.Add(new SqlParameter("edo_ParentsLostCategoryID", SqlDbType.Int) { Value = doc.EntDocOther.ParentsLostCategoryID });
                }
                if (doc.DocumentTypeID == 34)
                {
                    sbsql.Append(" \r\n UPDATE EntrantDocumentStateEmployee SET " +
                    "StateEmployeeCategoryId = @edo_StateEmployeeCategoryID " +
                    "WHERE EntrantDocumentID = @EntrantDocumentID ");
                    com.Parameters.Add(new SqlParameter("edo_StateEmployeeCategoryID", SqlDbType.Int) { Value = doc.EntDocOther.StateEmployeeCategoryID });
                }               
                if (doc.DocumentTypeID == 35)
                {
                    sbsql.Append(" \r\n UPDATE EntrantDocumentRadiationWork SET " +
                    "RadiationWorkCategoryId = @edo_RadiationWorkCategoryID " +
                    "WHERE EntrantDocumentID = @EntrantDocumentID ");
                    com.Parameters.Add(new SqlParameter("edo_RadiationWorkCategoryID", SqlDbType.Int) { Value = doc.EntDocOther.RadiationWorkCategoryID });
                }
            }
            #endregion

            #region  Обновление EntrantDocumentCustom
            if (doc.EntDocCustom != null)
            {
                sbsql.Append("\r\n UPDATE EntrantDocumentCustom SET AdditionalInfo = @cust_AdditionalInfo WHERE EntrantDocumentID = @EntrantDocumentID \r\n");

                //            sbsql.Append(@"
                //				IF EXISTS(SELECT EntrantDocumentID FROM EntrantDocumentCustom WHERE EntrantDocumentID=@EntrantDocumentID  ) BEGIN
                //					UPDATE EntrantDocumentCustom SET AdditionalInfo = @cust_AdditionalInfo, DocumentTypeNameText=@cust_DocumentTypeNameText WHERE EntrantDocumentID = @EntrantDocumentID
                //				END ELSE BEGIN
                //					INSERT INTO EntrantDocumentCustom(EntrantDocumentID, AdditionalInfo, DocumentTypeNameText) VALUES (@EntrantDocumentID, @cust_AdditionalInfo, @cust_DocumentTypeNameText)
                //				END
                //				");

                com.Parameters.Add(new SqlParameter("cust_AdditionalInfo", SqlDbType.VarChar) { Value = (doc.EntDocCustom.AdditionalInfo == null) ? ((object)DBNull.Value) : doc.EntDocCustom.AdditionalInfo });
                //com.Parameters.Add(new SqlParameter("DocumentName", SqlDbType.VarChar) { Value = (doc.EntDocCustom.DocumentName == null) ? ((object)DBNull.Value) : doc.EntDocCustom.DocumentName });
            }
            #endregion
            #region  Обновление EntrantDocumentDisability
            if (doc.EntDocDis != null)
            {
                sbsql.Append("\r\n UPDATE EntrantDocumentDisability SET DisabilityTypeID=@dic_DisabilityTypeID WHERE EntrantDocumentID=@EntrantDocumentID \r\n");
                com.Parameters.Add(new SqlParameter("dic_DisabilityTypeID", SqlDbType.Int) { Value = doc.EntDocDis.DisabilityTypeID });
            }
            #endregion
            #region Обновление EntrantDocumentEgeAndOlympicSubject
            if (doc.EntDocSubBall != null)
            {
                if (doc.EntDocSubBall.SubjectBalls != null)
                {
                    sbsql.Append("\r\n DELETE FROM EntrantDocumentEgeAndOlympicSubject WHERE EntrantDocumentID=@EntrantDocumentID \r\n");
                    foreach (SubjectBall sb in doc.EntDocSubBall.SubjectBalls)
                    {
                        sbsql.AppendFormat("\r\n INSERT INTO EntrantDocumentEgeAndOlympicSubject(EntrantDocumentID, SubjectID, [Value])VALUES(@EntrantDocumentID, {0},{1})\r\n", sb.SubjectID, (sb.Value.HasValue ? sb.Value.ToString() : "null"));
                    }
                }
            }
            #endregion
            #region Обновление EntrantDocumentEge
            if (doc.EntDocEge != null)
            {
                sbsql.Append("\r\n	UPDATE EntrantDocumentEge SET TypographicNumber=@ege_TypographicNumber WHERE  EntrantDocumentID=@EntrantDocumentID \r\n ");
                com.Parameters.Add(new SqlParameter("ege_TypographicNumber", SqlDbType.VarChar) { Value = (doc.EntDocEge.TypographicNumber == null) ? ((object)DBNull.Value) : doc.EntDocEge.TypographicNumber });
            }
            #endregion

            sbsql.Append(sqlEND);

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                com.Connection = con;
                com.CommandText = sbsql.ToString();
                try
                {
                    con.Open();
                    object o = null;// Вставить и вернуть новый ID
                    SqlDataReader r = com.ExecuteReader();
                    if (r.Read())
                    {
                        o = r["EntrantDocumentID"];
                        //if(o!=null&&o!=DBNull.Value) { doc.EntrantDocumentID=Convert.ToInt32(o); }
                    }
                    r.Close();
                    con.Close();
                }
                catch (SqlException e)
                {
                    throw e;    // Пробросить дальше
                }
                catch (Exception e)
                {
                    throw e; // Пробросить дальше
                }
                finally
                {
                    con.Close();
                }
            }
            return doc.EntrantDocumentID;
        }

        public static List<IDName> getOlympicDatas()
        {
            List<IDName> l = new List<IDName>();
            string sql = @" SELECT ot.OlympicID, ot.Name as OlympicName, ot.OlympicYear as OlympicYear FROM OlympicType ot ORDER BY ot.Name ";
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);
                try
                {
                    con.Open();
                    SqlDataReader r = com.ExecuteReader();
                    while (r.Read())
                    {
                        l.Add(new IDName()
                        {
                            ID = (int)r["OlympicID"],
                            Name = r["OlympicName"] as string,
                            Year = (int)r["OlympicYear"]
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
            return l;
        }

        public static OlympicData getOlympicData(int OlympicID)
        {
            OlympicData o = new OlympicData();
            string sql = @"
SELECT 
	ot.OlympicID
	,ot.Name as OlympicName
	,SubjectNames=( SELECT s.Name+' ,' FROM OlympicTypeSubjectLink oltsl  INNER JOIN [Subject] s on oltsl.SubjectID = s.SubjectID WHERE oltsl.OlympicID = ot.OlympicID FOR XML PATH(''))
	,ot.OrganizerName
	,ot.OlympicYear
	,ol.Name as OlympicTypeLevel
FROM OlympicType ot
	LEFT OUTER JOIN OlympicLevel ol on  ol.OlympicLevelID=ot.OlympicLevelID
WHERE ot.OlympicID=@OlympicID
";
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("OlympicID", OlympicID));
                try
                {
                    con.Open();
                    SqlDataReader r = com.ExecuteReader();
                    while (r.Read())
                    {
                        o.OlympicID = (int)r["OlympicID"];
                        o.OlympicName = r["OlympicName"] as string;
                        o.SubjectNames = r["SubjectNames"] as string;
                        o.OrganizerName = r["OrganizerName"] as string;
                        o.OlympicYear = (int)r["OlympicYear"];
                        o.LevelName = r["OlympicTypeLevel"] as string;
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
            return o;
        }

        public static int checkDocumentUID(EntrantDocumentViewModel doc)
        {
            int id = 0;

            string sql = @"SELECT TOP 1 ed.EntrantDocumentID 
FROM EntrantDocument ed  (NOLOCK)
INNER JOIN Entrant e (NOLOCK) on e.EntrantID=ed.EntrantID
WHERE e.InstitutionID=(SELECT InstitutionID FROM Entrant (NOLOCK) WHERE EntrantID=@EntrantID)
AND (ed.EntrantDocumentID!=@EntrantDocumentID OR @EntrantDocumentID IS NULL)
AND ed.DocumentTypeID=@DocumentTypeID
AND ed.UID=@UID";

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("EntrantID", SqlDbType.VarChar) { Value = doc.EntrantID });
                com.Parameters.Add(new SqlParameter("EntrantDocumentID", SqlDbType.Int) { Value = ((doc.EntrantDocumentID > 0) ? doc.EntrantDocumentID : (object)DBNull.Value) });
                com.Parameters.Add(new SqlParameter("DocumentTypeID", SqlDbType.Int) { Value = doc.DocumentTypeID });
                com.Parameters.Add(new SqlParameter("UID", SqlDbType.VarChar) { Value = doc.UID });

                // TODO: Доделать проверку на все документы.
                try
                {
                    con.Open();
                    var o = com.ExecuteScalar();
                    if (o != null)
                    {
                        id = (int)o;
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
            return id;



        }

    }
}