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
namespace GVUZ.Web.ContextExtensionsSQL
{

    public static partial class ApplicationSQL
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

        public static List<GVUZ.Model.Entrants.UniDocuments.IdentityDocumentType> GetIdentityDocumentType()
        {
            string sql = @"SELECT * FROM IdentityDocumentType (NOLOCK)  AS idt ORDER BY (CASE WHEN idt.IdentityDocumentTypeID =9 THEN 100 ELSE IdentityDocumentTypeID END ) ";
            List<GVUZ.Model.Entrants.UniDocuments.IdentityDocumentType> idt = null;
            DataSet ds = null;

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);
                ds = new DataSet();

                try
                {
                    con.Open();
                    SqlDataAdapter sqlAdapter = new SqlDataAdapter(com);
                    idt = new List<GVUZ.Model.Entrants.UniDocuments.IdentityDocumentType>();
                    sqlAdapter.Fill(ds);

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        idt.Add(new GVUZ.Model.Entrants.UniDocuments.IdentityDocumentType
                        {
                            IdentityDocumentTypeId = Convert.ToInt32(ds.Tables[0].Rows[i]["IdentityDocumentTypeID"] as int?),
                            IdentityDocumentTypeName = ds.Tables[0].Rows[i]["Name"] as string,
                            IsRussianNationality = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsRussianNationality"] as bool?)
                        });
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
            return idt;
        }

        public static ApplicationModel GetApplication(int ApplicationId)
        {
            string sql = @"SELECT app.ApplicationID, app.InstitutionID, app.WizardStepID,								
								COALESCE(app.ApplicationNumber,'не задан') as ApplicationNumber, 
								app.RegistrationDate,
								ent.LastName as EntrantLastName, 
								ent.FirstName as EntrantFirstName, 
								ent.MiddleName as EntrantMiddleName,
								dt.Name as DocumentTypeName, 
								ed.BirthDate as DocumentBirthDate
							FROM  [dbo].[Application] app (NOLOCK)  
								INNER JOIN Entrant ent (NOLOCK) on app.EntrantID=ent.EntrantID  
								INNER JOIN EntrantDocumentIdentity ed (NOLOCK) on ent.IdentityDocumentID=ed.EntrantDocumentID  
								INNER JOIN IdentityDocumentType dt (NOLOCK)  on dt.IdentityDocumentTypeID=ed.IdentityDocumentTypeID  
							WHERE app.ApplicationID=@ApplicationID";

            ApplicationModel app = null;
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
                        app = new ApplicationModel();

                        app.ApplicationID = r["ApplicationID"] as Int32?;
                        app.InstitutionID = r["InstitutionID"] as Int32?;
                        app.WizardStepID = r["WizardStepID"] as Int32?;

                        app.ApplicationNumber = r["ApplicationNumber"] as string;
                        app.RegistrationDateTime = (DateTime)r["RegistrationDate"];
                        app.EntrantLastName = r["EntrantLastName"] as string;
                        app.EntrantFirstName = r["EntrantFirstName"] as string;
                        app.EntrantMiddleName = r["EntrantMiddleName"] as string;
                        app.DocumentTypeName = r["DocumentTypeName"] as string;
                        app.DocumentBirthDate = r["DocumentBirthDate"] as DateTime?;

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

        public static ApplicationWzModel GetApplicationWz(int ApplicationId)
        {
            string sql = @"
SELECT DISTINCT app.ApplicationID, app.InstitutionID, CASE app.WizardStepID WHEN 0 THEN 1 ELSE app.WizardStepID END AS WizardStepID,
	COALESCE(e.LastName, '') +' '+  COALESCE(e.FirstName, '')  +' '+ COALESCE(e.MiddleName,'') as EntrantFullName,
app.ApplicationNumber, cmp.StatusID,
cast(case cmp.StatusID when 2 then 1 else 0 end as bit) IsCampaignFinished
FROM  [dbo].[Application] app (NOLOCK)  
	INNER JOIN Entrant e (NOLOCK) on app.EntrantID=e.EntrantID  
	LEFT OUTER JOIN ApplicationCompetitiveGroupItem acgi (NOLOCK) on acgi.ApplicationID=app.ApplicationID
	LEFT OUTER JOIN CompetitiveGroup cg (NOLOCK) on cg.CompetitiveGroupID=acgi.CompetitiveGroupID
	LEFT OUTER JOIN Campaign cmp (NOLOCK) on cmp.CampaignID=cg.CampaignID
WHERE app.ApplicationID=@ApplicationID";

            ApplicationWzModel app = null;
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
                        app = new ApplicationWzModel();

                        app.ApplicationID = r["ApplicationID"] as Int32?;
                        app.InstitutionID = r["InstitutionID"] as Int32?;
                        app.WizardStepID = r["WizardStepID"] as Int32?;
                        app.EntrantFullName = r["EntrantFullName"] as string;
                        app.ApplicationNumber = r["ApplicationNumber"] as string;
                        app.IsCampaignFinished = (bool)r["IsCampaignFinished"];
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
            if (app.InstitutionID != InstitutionHelper.GetInstitutionID())
            {
                return null;
            }
            if (app.IsCampaignFinished) { return null; }
            return app;
        }

        public static int SetWzStep(int ApplicationID, int step)
        {
            int c = 0;
            #region SQL
            string sql = @"UPDATE [dbo].[Application] SET [WizardStepID]=@WizardStepID WHERE ApplicationId=@ApplicationID";
            #endregion
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);

                #region Параметры
                com.Parameters.Add(new SqlParameter("ApplicationID", SqlDbType.Int) { Value = ApplicationID });
                com.Parameters.Add(new SqlParameter("WizardStepID", SqlDbType.Int) { Value = step });

                #endregion
                try
                {
                    con.Open();
                    // Вставить и вернуть новый ID
                    c = com.ExecuteNonQuery();
                    if (c == 0)
                    {
                        // Не обновилась запись
                    }
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
            return c;
        }

        #region Edit
        public static ApplicationEditModel GetApplicationEdit(int ApplicationId)
        {
            #region SQL
            string sql = @"
SELECT DISTINCT app.ApplicationID, app.InstitutionID, app.WizardStepID, app.StatusID, 
	COALESCE(e.LastName, '') +' '+  COALESCE(e.FirstName, '')  +' '+ COALESCE(e.MiddleName,'') as EntrantFullName,
app.ApplicationNumber, cmp.StatusID,
cast(case cmp.StatusID when 2 then 1 else 0 end as bit) IsCampaignFinished
FROM  [dbo].[Application] app (NOLOCK)  
	INNER JOIN Entrant e (NOLOCK) on app.EntrantID=e.EntrantID  
	LEFT OUTER JOIN ApplicationCompetitiveGroupItem acgi (NOLOCK) on acgi.ApplicationID=app.ApplicationID
	LEFT OUTER JOIN CompetitiveGroup cg (NOLOCK) on cg.CompetitiveGroupID=acgi.CompetitiveGroupID
	LEFT OUTER JOIN Campaign cmp (NOLOCK) on cmp.CampaignID=cg.CampaignID
WHERE app.ApplicationID=@ApplicationID ";
            #endregion

            ApplicationEditModel app = null;
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
                        app = new ApplicationEditModel();

                        app.ApplicationID = r["ApplicationID"] as Int32?;
                        app.InstitutionID = r["InstitutionID"] as Int32?;
                        app.WizardStepID = r["WizardStepID"] as Int32?;
                        app.ApplicationNumber = r["ApplicationNumber"] as string;
                        app.EntrantFullName = r["EntrantFullName"] as string;
                        app.IsCampaignFinished = (bool)r["IsCampaignFinished"];
                        app.StatusID = r["StatusID"] as Int32?;
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
            if (app.InstitutionID != InstitutionHelper.GetInstitutionID())
            {
                return null;
            }
            if (app.IsCampaignFinished) { return null; }
            return app;

            //            
            //            string sql=@"SELECT app.ApplicationID, app.InstitutionID, app.WizardStepID,								
            //								COALESCE(app.ApplicationNumber,'не задан') as ApplicationNumber, 
            //								app.RegistrationDate,
            //								ent.LastName as EntrantLastName, 
            //								ent.FirstName as EntrantFirstName, 
            //								ent.MiddleName as EntrantMiddleName,
            //								dt.Name as DocumentTypeName, 
            //								ed.BirthDate as DocumentBirthDate
            //							FROM  [dbo].[Application] app  
            //								INNER JOIN Entrant ent on app.EntrantID=ent.EntrantID  
            //								INNER JOIN EntrantDocumentIdentity ed on ent.IdentityDocumentID=ed.EntrantDocumentID  
            //								INNER JOIN IdentityDocumentType dt  on dt.IdentityDocumentTypeID=ed.IdentityDocumentTypeID  
            //							WHERE app.ApplicationID=@ApplicationID";
            //            #endregion

            //            ApplicationEditModel app=null;
            //            using(SqlConnection con=new SqlConnection(ConnectionString)) {
            //                SqlCommand com=new SqlCommand(sql,con);
            //                com.Parameters.Add(new SqlParameter("ApplicationID",ApplicationId));

            //                try {
            //                    con.Open();
            //                    SqlDataReader r=com.ExecuteReader();
            //                    if(r.Read()) {
            //                        app=new ApplicationEditModel();

            //                        app.ApplicationID=r["ApplicationID"] as int?;
            //                        app.InstitutionID=r["InstitutionID"] as int?;
            //                        app.WizardStepID=r["WizardStepID"] as int?;

            //                        app.ApplicationNumber=r["ApplicationNumber"] as string;
            //                        app.RegistrationDateTime=(DateTime)r["RegistrationDate"];
            //                        app.EntrantLastName=r["EntrantLastName"] as string;
            //                        app.EntrantFirstName=r["EntrantFirstName"] as string;
            //                        app.EntrantMiddleName=r["EntrantMiddleName"] as string;
            //                        app.DocumentTypeName=r["DocumentTypeName"] as string;
            //                        app.DocumentBirthDate=r["DocumentBirthDate"] as DateTime?;

            //                    }
            //                    r.Close();
            //                    con.Close();
            //                } catch(SqlException e) {
            //                    throw e;
            //                } catch(Exception e) {
            //                    throw e;
            //                } finally {
            //                    if(con.State!=ConnectionState.Closed) { con.Close(); }
            //                }
            //            }
            //            return app;
        }
        #endregion


        #region Wz1

        public static ApplicationWz1ViewModel GetApplicationWz1(int ApplicationId)
        {
            string sql = @"SELECT app.ApplicationID, e.EntrantID, e.SNILS,
								e.LastName as LastName, e.FirstName as FirstName, e.MiddleName as MiddleName,
								dt.IdentityDocumentTypeID as DocumentTypeID,	dt.Name as DocumentTypeName, 
								edi.BirthDate as BirthDate,	edi.GenderTypeID,
								ed.DocumentSeries,	ed.DocumentNumber,	ed.DocumentOrganization,	ed.DocumentDate,	edi.SubDivisionCode,
								ed.AttachmentID as DocumentAttachmentID,	att.Name as DocumentAttachmentName,
								att.FileID as AttachmentFileID,
								COALESCE(edi.NationalityTypeID,1) as NationalityID,
								edi.BirthPlace as BirthPlace,
								e.CustomInformation as CustomInformation,
								COALESCE(app.NeedHostel,0) as NeedHostel,
								app.WizardStepID, app.StatusID, 
                                e.IsFromKrymEntrantDocumentID,e.Email,e.RegionID,e.TownTypeID,e.Address,e.IsFromKrym
                                ,ed.ReleaseCountryID, ed.ReleasePlace
							FROM  [dbo].[Application] app (NOLOCK)  
								INNER JOIN Entrant e (NOLOCK) on app.EntrantID=e.EntrantID  
								INNER JOIN EntrantDocument ed (NOLOCK) on ed.EntrantDocumentID=e.IdentityDocumentID
								INNER JOIN EntrantDocumentIdentity edi (NOLOCK) on e.IdentityDocumentID=edi.EntrantDocumentID  
								INNER JOIN IdentityDocumentType dt (NOLOCK) on dt.IdentityDocumentTypeID=edi.IdentityDocumentTypeID  
								LEFT OUTER JOIN Attachment att (NOLOCK) on att.AttachmentID=ed.AttachmentID
                                
							WHERE app.ApplicationID=@ApplicationID ";

            ApplicationWz1ViewModel appw1 = null;
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
                        appw1 = new ApplicationWz1ViewModel();
                        appw1.ApplicationID = r["ApplicationID"] as Int32?;
                        appw1.EntrantID = (Int32)r["EntrantID"];
                        appw1.LastName = r["LastName"] as string;
                        appw1.FirstName = r["FirstName"] as string;
                        appw1.MiddleName = r["MiddleName"] as string;
                        appw1.SNILS = r["SNILS"] as string;
                        appw1.BirthDate = r["BirthDate"] as DateTime?;
                        appw1.GenderID = r["GenderTypeID"] as int?;
                        appw1.DocumentTypeID = (Int32)r["DocumentTypeID"];
                        appw1.DocumentNumber = r["DocumentNumber"] as string;
                        appw1.DocumentSeries = r["DocumentSeries"] as string;
                        appw1.DocumentOrganization = r["DocumentOrganization"] as string;
                        appw1.DocumentDate = r["DocumentDate"] as DateTime?;
                        appw1.SubdivisionCode = r["SubDivisionCode"] as string;
                        appw1.ReleaseCountryID = r["ReleaseCountryID"] as int?;
                        appw1.ReleasePlace = r["ReleasePlace"] as string;

                        appw1.NationalityID = (Int32)r["NationalityID"];
                        appw1.BirthPlace = r["BirthPlace"] as string;
                        appw1.CustomInformation = r["CustomInformation"] as string;
                        appw1.NeedHostel = (1 == (int)r["NeedHostel"]);

                        appw1.DocumentAttachmentID = r["DocumentAttachmentID"] as int?;
                        appw1.AttachmentFileID = r["AttachmentFileID"] as Guid?;
                        if (appw1.AttachmentFileID == null)
                        {
                            appw1.AttachmentFileID = Guid.Empty;
                        }
                        appw1.DocumentAttachmentName = r["DocumentAttachmentName"] as string;

                        appw1.Email = r["Email"] as string;
                        appw1.Address = r["Address"] as string;
                        appw1.RegionID = (int)r["RegionID"];
                        appw1.TownTypeID = r["TownTypeID"] as int?;
                        appw1.IsFromKrym = (bool)r["IsFromKrym"];
                        appw1.IsFromKrymEntrantDocumentID = r["IsFromKrymEntrantDocumentID"] as int?;

                        appw1.ListIdentityDocumentType = ApplicationSQL.GetIdentityDocumentType();

                        appw1.StatusID = r["StatusID"] as Int32?;
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
            return appw1;
        }

        public static IEnumerable<int?> GetSelectedCitizenships(int? appID)
        {
            string sql = @" 
                            SELECT  ec.CountryID AS ID
                            FROM    EntrantCitizenship ec                            
                            WHERE   EntrantID = (SELECT EntrantID 
                                                FROM Application app 
                                                WHERE app.ApplicationID = @AppID) ";

            var citzenships = new List<int?>();
            DataSet ds = null;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("AppID", appID));
                ds = new DataSet();
                try
                {                 
                    con.Open();
                    SqlDataAdapter sqlAdapter = new SqlDataAdapter(com);
                    sqlAdapter.Fill(ds);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        citzenships.Add(ds.Tables[0].Rows[i]["ID"] is DBNull ? null : ds.Tables[0].Rows[i]["ID"]  as int?);
                    }
                                       
                    con.Close();
                }
                catch (SqlException e)
                {
                    throw e;
                }

                finally
                {
                    if (con.State != ConnectionState.Closed) { con.Close(); }
                }
            }
                return citzenships;
        }

        public static int UpdApplicationWz1(ApplicationWz1Model app, log4net.ILog logger = null)
        {
            int c = 0;
            #region SQL
                string sql = @"
                            DECLARE @EntrantID	int
                            DECLARE @EntrantDocumentID	int
                            DECLARE @AttachmentID	int
                            DECLARE @newAttachmentID	int

                            SELECT	    @EntrantID=e.EntrantID, @EntrantDocumentID=ed.EntrantDocumentID, @AttachmentID=ed.AttachmentID
                            FROM        [dbo].[Application] app (NOLOCK)  
                            INNER JOIN  Entrant e (NOLOCK) on app.EntrantID=e.EntrantID  
                            INNER JOIN  EntrantDocument ed (NOLOCK) on ed.EntrantDocumentID=e.IdentityDocumentID
                            WHERE       app.ApplicationID=@ApplicationID
                          
                            UPDATE  [dbo].[Application] SET [WizardStepID]=@WizardStepID WHERE ApplicationId=@ApplicationID;

                            UPDATE  Entrant 
                            SET     IsFromKrymEntrantDocumentID=@IsFromKrymEntrantDocumentID,Email=@Email,RegionID=@RegionID,TownTypeID=@TownTypeID
                                    ,Address=@Address,IsFromKrym=@IsFromKrym, FirstName=@FirstName, LastName=@LastName, MiddleName=@MiddleName, GenderID=@GenderID
                                    ,CustomInformation=@CustomInformation , SNILS=@sSNILS --, PersonId=NULL
                            WHERE   EntrantID=@EntrantID

                            UPDATE  EntrantDocument
                            SET     DocumentSeries=@DocumentSeries, DocumentNumber=@DocumentNumber, DocumentOrganization=@DocumentOrganization,
                                    DocumentDate=@DocumentDate,ReleasePlace=@ReleasePlace,ReleaseCountryID=@ReleaseCountryID
                                    WHERE EntrantDocumentID=@EntrantDocumentID
								
                            UPDATE  EntrantDocumentIdentity
                            SET     IdentityDocumentTypeID=@DocumentTypeID, GenderTypeID=@GenderID, BirthDate=@BirthDate, BirthPlace=@BirthPlace,
                                    SubdivisionCode=@SubdivisionCode,NationalityTypeID=@NationalityTypeID, FirstName=@FirstName, LastName=@LastName, MiddleName=@MiddleName
                            WHERE   EntrantDocumentID=@EntrantDocumentID
                            


                            IF @AttachmentFileID IS NOT NULL BEGIN
                            SELECT @newAttachmentID=AttachmentID FROM Attachment (NOLOCK) WHERE FileID=@AttachmentFileID
                            IF @newAttachmentID IS NOT NULL BEGIN
                            UPDATE EntrantDocument  SET AttachmentID=@newAttachmentID WHERE EntrantDocumentID=@EntrantDocumentID                           
                          
                            END
                            END
                                    ";

            string sqlCitizenship = @"--DECLARE @EntrantCitizenships AS dbo.EntrantCitizenships 

                                    MERGE EntrantCitizenship WITH(UPDLOCK) AS TARGET
                                    USING (SELECT EntrantID, CountryID FROM @EntrantCitizenships) AS SOURCE
                                    ON  TARGET.EntrantID = SOURCE.EntrantID AND 
                                        TARGET.CountryID = SOURCE.CountryID
                                     WHEN NOT MATCHED THEN
                                         INSERT
                                         ([EntrantID] ,[CountryID])
                                         VALUES (entrantID, countryID)
                                    WHEN NOT MATCHED BY SOURCE AND 
                                    TARGET.EntrantID = (SELECT TOP 1 EntrantID FROM @EntrantCitizenships)
                                         THEN 
                                    DELETE;;";
#endregion
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);

                #region Параметры

                //var entrantCitizenshipsTable = new DataTable();
                //entrantCitizenshipsTable.Columns.Add("EntrantID", typeof(int));
                //entrantCitizenshipsTable.Columns.Add("CountryID", typeof(int));

                //foreach (var citizenship in app.SelectedCitizenships)
                //{
                //    var row = entrantCitizenshipsTable.NewRow();
                //    //entrantCitizenshipsTable.Rows.Add(new { app.EntrantID, citizenship });
                //    row["EntrantID"] = app.EntrantID;
                //    row["CountryID"] = citizenship;
                //    entrantCitizenshipsTable.Rows.Add(row);
                //}
                //logger.DebugFormat("Принадлежность к гражданству -> {0}", entrantCitizenshipsTable.Rows.Count);

                
                //var command = con.CreateCommand();
                //command.CommandText = sqlCitizenship;
                //command.CommandType = CommandType.Text;

                //var parameter = command.CreateParameter();
                //parameter.SqlDbType = SqlDbType.Structured;
                //parameter.ParameterName = "@EntrantCitizenships";
                ////parameter.Value = entrantCitizenshipsTable;
                //parameter.TypeName = "dbo.EntrantCitizenships";

                //command.Parameters.Add(parameter);
                

                com.Parameters.Add(new SqlParameter("ApplicationID", SqlDbType.Int) { Value = app.ApplicationID });
                com.Parameters.Add(new SqlParameter("WizardStepID", SqlDbType.Int) { Value = app.WizardStepID });

                com.Parameters.Add(new SqlParameter("FirstName", app.FirstName));
                com.Parameters.Add(new SqlParameter("LastName", app.LastName));
                com.Parameters.Add(new SqlParameter("ReleasePlace", app.ReleasePlace));
                com.Parameters.Add(new SqlParameter("ReleaseCountryID", app.ReleaseCountryID));
                if (app.MiddleName != null)
                {
                    com.Parameters.Add(new SqlParameter("MiddleName", app.MiddleName));
                }
                else
                {
                    com.Parameters.Add(new SqlParameter("MiddleName", DBNull.Value));
                }
                com.Parameters.Add(new SqlParameter("DocumentTypeID", app.DocumentTypeID));

                if (app.DocumentSeries != null)
                {
                    com.Parameters.Add(new SqlParameter("DocumentSeries", app.DocumentSeries));
                }
                else
                {
                    com.Parameters.Add(new SqlParameter("DocumentSeries", DBNull.Value));
                }

                com.Parameters.Add(new SqlParameter("DocumentNumber", app.DocumentNumber));
                if (app.DocumentOrganization != null)
                {
                    com.Parameters.Add(new SqlParameter("DocumentOrganization", app.DocumentOrganization));
                }
                else
                {
                    com.Parameters.Add(new SqlParameter("DocumentOrganization", DBNull.Value));
                }
                com.Parameters.Add(new SqlParameter("DocumentDate", SqlDbType.DateTime) { Value = app.DocumentDate });
                if (app.CustomInformation != null)
                {
                    com.Parameters.Add(new SqlParameter("CustomInformation", app.CustomInformation));
                }
                else
                {
                    com.Parameters.Add(new SqlParameter("CustomInformation", DBNull.Value));
                }

                if (app.SubdivisionCode == null)
                {
                    app.SubdivisionCode = "";
                }

                com.Parameters.Add(new SqlParameter("GenderID", SqlDbType.Int) { Value = app.GenderID });
                com.Parameters.Add(new SqlParameter("SubdivisionCode", app.SubdivisionCode));
                com.Parameters.Add(new SqlParameter("BirthDate", SqlDbType.DateTime) { Value = app.BirthDate });
                com.Parameters.Add(new SqlParameter("NationalityTypeID", SqlDbType.Int) { Value = app.NationalityID });
                com.Parameters.Add(new SqlParameter("BirthPlace", SqlDbType.VarChar) { Value = (app.BirthPlace != null) ? app.BirthPlace : (object)DBNull.Value });

                //if(app.AttachmentFileID!=null) {
                //   com.Parameters.Add(new SqlParameter("DocumentAttachmentID",SqlDbType.Int) { Value=app.DocumentAttachmentID.Value });
                //} else {
                //   com.Parameters.Add(new SqlParameter("DocumentAttachmentID",DBNull.Value));
                //}
                if (app.AttachmentFileID != null && app.AttachmentFileID != Guid.Empty)
                {
                    com.Parameters.Add(new SqlParameter("AttachmentFileID", SqlDbType.UniqueIdentifier) { Value = app.AttachmentFileID });				//ed.AttachmentID
                }
                else
                {
                    com.Parameters.Add(new SqlParameter("AttachmentFileID", DBNull.Value));
                }

                com.Parameters.Add(new SqlParameter("Email", SqlDbType.VarChar) { Value = (app.Email != null) ? app.Email : (object)DBNull.Value });
                com.Parameters.Add(new SqlParameter("RegionID", SqlDbType.Int) { Value = app.RegionID });
                com.Parameters.Add(new SqlParameter("TownTypeID", SqlDbType.Int) { Value = app.TownTypeID == 0 ? (object)DBNull.Value : app.TownTypeID });
                com.Parameters.Add(new SqlParameter("Address", SqlDbType.VarChar) { Value = (app.Address != null) ? app.Address : (object)DBNull.Value });
                com.Parameters.Add(new SqlParameter("IsFromKrym", SqlDbType.Bit) { Value = app.IsFromKrym });

                com.Parameters.Add(new SqlParameter("IsFromKrymEntrantDocumentID", SqlDbType.Int) { Value = app.IsFromKrymEntrantDocumentID == null ? (object)DBNull.Value : app.IsFromKrymEntrantDocumentID });
                //com.Parameters.AddWithValue("@sSNILS", app.SNILS);
                com.Parameters.Add(new SqlParameter("@sSNILS", SqlDbType.VarChar) { Value = (app.SNILS != null) ? app.SNILS : (object)DBNull.Value });

                #endregion
                try
                {
                    con.Open();
                    //com.CommandTimeout = SQL.TIMEOUT;
                    // НЕ синхронизируем, если таблица пустая!
                    //if (entrantCitizenshipsTable.Rows.Count > 0)
                    //{
                    //    logger.Debug("Сохранение гражданства...");
                    //    //command.ExecuteNonQuery();
                    //}
                    //else
                    //{
                    //    logger.Debug("Множественное гражданство не указано...");
                    //}
                    //logger.Debug("Сохранение основного заявления...");
                    com.ExecuteNonQuery();
                    con.Close();
                    logger.DebugFormat("Синхронизация абитуриента {0}...", app.EntrantID.HasValue ? app.EntrantID.Value : 0);
                    SyncEntrant(app.EntrantID.HasValue ? app.EntrantID.Value : 0);
                    logger.Debug("Успешно!");
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
            return c;
        }

        #endregion

        public static bool SyncEntrant(int entrantId)
        {

            bool result = false;
            if (entrantId == 0)
            {
                return result;
            }

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand("SyncEntrant", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@entrantId", SqlDbType.VarChar) { Value = entrantId });

                    con.Open();
                    command.ExecuteNonQuery();

                    result = true;
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
            }
            return result;
        }

        #region Wz2



        #endregion

        #region Wz4


        public static List<string> GetUidList(int institutionId)
        {
            #region SQL

            string sql = @" SELECT ia.IAUID FROM IndividualAchivement AS ia (NOLOCK)
	                            INNER JOIN [Application] app (NOLOCK) ON app.ApplicationID = ia.ApplicationID
                            WHERE app.InstitutionID = @InstitutionID AND ia.IAUID IS NOT NULL";
            #endregion

            List<string> UidList = new List<string>();
            DataSet ds = null;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("InstitutionID", institutionId));
                ds = new DataSet();
                try
                {
                    con.Open();
                    SqlDataAdapter sqlAdapter = new SqlDataAdapter(com);
                    sqlAdapter.Fill(ds);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        UidList.Add(ds.Tables[0].Rows[i]["IAUID"] as string);
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
            return UidList;
        }

        #region SaveIndividualAchievement

        public static ApplicationWz4ViewModel.IndividualAchivementViewModel SaveIndividualAchievement(ApplicationWz4ViewModel.IndividualAchivementViewModel model)
        {

            #region SQL
            string sql = @"
DECLARE @IAID int

  INSERT INTO [dbo].[IndividualAchivement]
           ([ApplicationID]
           ,[IAUID]
           ,[IAName]
           ,[IAMark]
           ,[isAdvantageRight]
           ,[EntrantDocumentID]
           ,[CreatedDate]
           ,[ModifiedDate]
           ,[IdAchievement])
     VALUES(
            @ApplicationID,
            @UID,
            @IAName,
            @IAMarkString,
            @isAdvantageRight,            
            @EntrantDocumentID,
            @CreatedDate,
            NULL,
            @IdAchievement)
SELECT @IAID=cast(scope_identity() as int)

SELECT DISTINCT ia.ApplicationID AS ApplicationID
    ,ia.IAID AS IAID
    ,ia.IAUID AS IAUID
	,ia.IAName AS IAName
	,ia.IAMark AS IAMark
	,ia.EntrantDocumentID AS EntrantDocumentID
    ,ia.isAdvantageRight
	
    ,app.EntrantID AS EntrantID
	,ed.DocumentDate AS DocumentDate
	,ed.DocumentNumber AS DocumentNumber
	,ed.DocumentSeries AS DocumentSeries
	,ed.DocumentOrganization AS DocumentOrganization
	,ed.DocumentName AS DocumentTypeNameText
FROM Application app (NOLOCK)
INNER JOIN IndividualAchivement ia (NOLOCK) ON ia.ApplicationID = app.ApplicationID
--INNER JOIN Entrant e (NOLOCK) ON e.EntrantID = app.EntrantID
LEFT OUTER JOIN EntrantDocument ed (NOLOCK) ON ed.EntrantDocumentID = ia.EntrantDocumentID
LEFT OUTER JOIN EntrantDocumentCustom EDC (NOLOCK) ON edc.EntrantDocumentID = ed.EntrantDocumentID
WHERE ia.IAID=@IAID 
";

            #endregion


            ApplicationWz4ViewModel.IndividualAchivementViewModel ach = null;

            decimal IAMarkDecimal;
            decimal.TryParse(model.IAMarkString, out IAMarkDecimal);

            using (SqlConnection con = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                try
                {
                    cmd.Parameters.Add("@ApplicationID", SqlDbType.Int).Value = model.ApplicationID;
                    cmd.Parameters.Add("@UID", SqlDbType.VarChar, 50).Value = (model.UID == null ? DBNull.Value : (object)model.UID);
                    cmd.Parameters.Add("@IAName", SqlDbType.VarChar, 500).Value = model.IAName;
                    cmd.Parameters.Add("@IAMarkString", SqlDbType.Decimal).Value = string.IsNullOrEmpty(model.IAMarkString) ? 0 : model.IAMarkString.ToDecimal() as decimal?;
                    cmd.Parameters.Add("@IdAchievement", SqlDbType.Int).Value = model.IdAchievement ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@isAdvantageRight", SqlDbType.Bit).Value = model.isAdvantageRight ?? (object)DBNull.Value;

                    if (model.IADocumentID > 0)
                    {
                        cmd.Parameters.Add("@EntrantDocumentID", SqlDbType.Int).Value = model.IADocumentID;
                    }
                    else
                    {
                        cmd.Parameters.Add("@EntrantDocumentID", SqlDbType.Int).Value = DBNull.Value;
                    }
                    cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = DateTime.Now;
                    //cmd.Parameters.Add("@ModifiedDate", SqlDbType.DateTime).Value = null;
                    //cmd.Parameters.Add("@IdAchievement", SqlDbType.Int).Value = ApplicationID;

                    con.Open();
                    var r = cmd.ExecuteReader();
                    if (r.Read())
                    {
                        ach = new ApplicationWz4ViewModel.IndividualAchivementViewModel()
                        {
                            IAID = r["IAID"] as Int32?,
                            UID = r["IAUID"] as string,
                            IAName = r["IAName"] as string,
                            IAMark = r["IAMark"] as decimal?,
                            isAdvantageRight = r["isAdvantageRight"] as bool?,
                            IADocument = new ApplicationWz4ViewModel.IndividualAchivementViewModel.IADocumentViewModel()
                            {
                                ApplicationID = r["ApplicationID"] as Int32?,
                                EntrantDocumentID = r["EntrantDocumentID"] as Int32?,
                                DocumentTypeNameText = r["DocumentTypeNameText"] as string,
                                DocumentSeries = r["DocumentSeries"] as string,
                                DocumentNumber = r["DocumentNumber"] as string,
                                DocumentOrganization = r["DocumentOrganization"] as string,
                                DocumentDate = r["DocumentDate"] as DateTime?
                            }
                        };
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

            return ach;
        }

        //public static AjaxResultModel SaveIndividualAchievement(this EntrantsEntities dbContext, IndividualAchivementsViewModel.IndividualAchivementViewModel model, int applicationID)
        //{
        //    if (!model.IADocumentID.HasValue)
        //        return new AjaxResultModel("Не выбран подтверждающий документ");

        //    BaseDocumentViewModel baseDoc = dbContext.LoadEntrantDocument(model.IADocumentID.Value);
        //    if (baseDoc == null || !(baseDoc is CustomDocumentViewModel))
        //        return new AjaxResultModel("Не найден подтверждающий документ");

        //    var newIA = dbContext.IndividualAchivement.CreateObject();
        //    newIA.ApplicationID = applicationID;
        //    newIA.EntrantDocumentID = model.IADocumentID.Value;
        //    newIA.IAMark = string.IsNullOrEmpty(model.IAMarkString) ? (decimal?)null : model.IAMarkString.ToDecimal();
        //    newIA.IAName = model.IAName;
        //    newIA.IAUID = model.UID;

        //    dbContext.IndividualAchivement.AddObject(newIA);
        //    dbContext.SaveChanges();
        //    return new AjaxResultModel();
        //}
        #endregion

        #region DeleteIndividualAchievement
        public static AjaxResultModel DeleteIndividualAchievement(int achievementID, int entrantDocumentID)
        {
            #region SQL
            string sql = @"DELETE FROM [dbo].[IndividualAchivement] WHERE IAID = @achievementID
                           DELETE FROM [dbo].[ApplicationEntrantDocument] WHERE EntrantDocumentID = @entrantDocumentID";
            #endregion
            if (entrantDocumentID == 0)
            {
                sql = @"DELETE FROM [dbo].[IndividualAchivement] WHERE IAID = @achievementID";
            }

            int c = 0;

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);

                #region Параметры
                com.Parameters.Add(new SqlParameter("achievementID", achievementID));
                if (entrantDocumentID != 0) { com.Parameters.Add(new SqlParameter("entrantDocumentID", entrantDocumentID)); }

                #endregion
                try
                {
                    con.Open();
                    // Вставить и вернуть новый ID
                    c = com.ExecuteNonQuery();
                    if (c != 1)
                    {
                        // Не обновилась запись
                    }
                    con.Close();
                }
                catch (SqlException e)
                {
                    // Пробросить дальше
                    throw e;
                }
                catch (Exception e)
                {
                    // Пробросить дальше
                    throw e;
                }
                finally
                {
                    if (con.State != ConnectionState.Closed) { con.Close(); }
                }
            }
            return new AjaxResultModel();
        }
        #endregion

        public static void UpdateIndividualAchivementsMark(int applicationId)
        {
            string sql = @"
DECLARE @individualAchivementMarksSum decimal(10,4);
DECLARE @checkSpecialConditions bit = 0;
DECLARE @specialConditionValue decimal(7,4) = null;
DECLARE @instID int; 
SELECT @individualAchivementMarksSum = ISNULL(SUM (IAMark), 0) FROM IndividualAchivement (NOLOCK) WHERE ApplicationID = @applicationId
SELECT @instID = InstitutionID FROM Application (NOLOCK) WHERE ApplicationID = @applicationId
SELECT @checkSpecialConditions = 1 WHERE EXISTS (SELECT 1 FROM IASpecialConditions WHERE InstitutionID = @instID)
SELECT TOP 1 @specialConditionValue = Value FROM IASpecialConditions WHERE InstitutionID = @instID ORDER BY Value DESC
{0}
UPDATE Application
SET IndividualAchivementsMark = @individualAchivementMarksSum
WHERE ApplicationID = @applicationId
";
            if (GetCampaingTypeId(applicationId) == GVUZ.Data.Model.CampaignTypes.BachelorOrSpecialist)
            {
                sql = String.Format(sql, @"
IF (@individualAchivementMarksSum > 10 AND @checkSpecialConditions = 0)
BEGIN 
    SET @individualAchivementMarksSum = 10 
END
ELSE IF (@checkSpecialConditions = 1 AND (@individualAchivementMarksSum > @specialConditionValue))
BEGIN
    SET @individualAchivementMarksSum = @specialConditionValue
END
");
            }
            else
            {
                sql =String.Format(sql,"");
            } 

            using (SqlConnection con = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.Add("@applicationId", SqlDbType.Int).Value = applicationId;

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public static short GetCampaingTypeId(int applicationId)
        {
            string sql = @"
SELECT TOP 1 
    c.CampaignTypeID
FROM 
    Application app (NOLOCK) 
    INNER JOIN ApplicationCompetitiveGroupItem acgi (NOLOCK) ON acgi.ApplicationId = app.ApplicationId
	INNER JOIN CompetitiveGroup cg (NOLOCK) on acgi.CompetitiveGroupId = cg.CompetitiveGroupID
	INNER JOIN Campaign c (NOLOCK) on cg.CampaignID = c.CampaignID 
WHERE 
    app.ApplicationID = @applicationId
";

            using (SqlConnection con = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.Add("@applicationId", SqlDbType.Int).Value = applicationId;

                con.Open();
                return (cmd.ExecuteScalar() as short?).GetValueOrDefault();
            }
        } 

        public static int GetApplicationIdByAchivementId(int achievementId)
        {
            string sql = @"
SELECT ApplicationID FROM IndividualAchivement (NOLOCK) WHERE IAID = @achievementId
";
            using (SqlConnection con = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.Add("@achievementId", SqlDbType.Int).Value = achievementId;

                con.Open();
                return  (cmd.ExecuteScalar() as int?).GetValueOrDefault();
            }
        }

        public static string GetApplicationNumber(int applicationId)
        {
            string sql = @"
SELECT ApplicationNumber FROM Application (NOLOCK) WHERE ApplicationId = @applicationId
";
            using (SqlConnection con = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.Add("@applicationId", SqlDbType.Int).Value = applicationId;

                con.Open();
                return (cmd.ExecuteScalar() as string);
            }
        }

        public static ApplicationWz4ViewModel GetApplicationWz4(int ApplicationID)
        {

            #region SQL
            string sql = @"
SELECT DISTINCT 
    ia.ApplicationID AS ApplicationID
    ,ia.IAID AS IAID
    ,ia.IAUID AS IAUID
	,ISNULL(instAch.Name,ia.IAName) AS IAName
	,ia.IAMark AS IAMark
	,ia.EntrantDocumentID AS EntrantDocumentID
    ,ia.isAdvantageRight AS isAdvantageRight
	
    ,app.EntrantID AS EntrantID
	,ed.DocumentDate AS DocumentDate
	,ed.DocumentNumber AS DocumentNumber
	,ed.DocumentSeries AS DocumentSeries
	,ed.DocumentOrganization AS DocumentOrganization
	,ed.DocumentName AS DocumentTypeNameText
FROM 
    Application app (NOLOCK)
    INNER JOIN IndividualAchivement ia (NOLOCK) ON ia.ApplicationID = app.ApplicationID
    LEFT JOIN InstitutionAchievements instAch (NOLOCK) ON ia.IdAchievement = instAch.IdAchievement
    LEFT OUTER JOIN EntrantDocument ed (NOLOCK) ON ed.EntrantDocumentID = ia.EntrantDocumentID
    LEFT OUTER JOIN EntrantDocumentCustom EDC (NOLOCK) ON edc.EntrantDocumentID = ed.EntrantDocumentID
WHERE 
    ia.ApplicationID = @ApplicationID
                
SELECT DISTINCT 
    app.ApplicationID AS ApplicationID,
    e.EntrantID AS EntrantID, 
    app.InstitutionID,
    app.StatusID,
    c.CampaignTypeID,
    app.IndividualAchivementsMark
FROM 
    Application app (NOLOCK) 
    INNER JOIN ApplicationCompetitiveGroupItem acgi (NOLOCK) ON acgi.ApplicationId = app.ApplicationId
	INNER JOIN CompetitiveGroup cg (NOLOCK) on acgi.CompetitiveGroupId = cg.CompetitiveGroupID
	INNER JOIN Campaign c (NOLOCK) on cg.CampaignID = c.CampaignID
    INNER JOIN Entrant e (NOLOCK) ON e.EntrantID = app.EntrantID
WHERE 
    app.ApplicationID = @ApplicationID

SELECT DISTINCT
	ia.Name AS IAName
	,ia.IdAchievement AS IAID
	,ia.MaxValue AS IAMaxValue
	,ia.IdCategory AS IAIdCategory
	,ia.UID AS IAUID
	,ia.CampaignID AS IACampaignID
FROM InstitutionAchievements AS ia (NOLOCK) 
	INNER JOIN Campaign AS c (NOLOCK) ON ia.CampaignID = c.CampaignID
	INNER JOIN CompetitiveGroup AS CG (NOLOCK) ON c.CampaignID = CG.CampaignID
	INNER JOIN ApplicationCompetitiveGroupItem AS acgi (NOLOCK) ON CG.CompetitiveGroupID = acgi.CompetitiveGroupId
WHERE     
    acgi.ApplicationId = @ApplicationID
ORDER BY 
    IACampaignID

IF EXISTS(SELECT 1 FROM [IASpecialConditions] (NOLOCK) WHERE InstitutionID = (SELECT InstitutionID FROM Application (NOLOCK) WHERE ApplicationID = @ApplicationID))
BEGIN
    SELECT TOP(1) Value as Value
      FROM [IASpecialConditions] (NOLOCK)
     WHERE InstitutionID = (SELECT InstitutionID FROM Application (NOLOCK) WHERE ApplicationID = @ApplicationID)
     ORDER BY Value DESC
END
ELSE
BEGIN 
    SELECT CAST(10 as decimal(7,4)) as Value
END

SELECT 
    ia2.ApplicationID
FROM 
    InstitutionAchievements AS ia (NOLOCK)                
    INNER JOIN IndividualAchievementsCategory AS iac (NOLOCK) ON iac.IdCategory = ia.IdCategory
    INNER JOIN IndividualAchivement AS ia2 (NOLOCK) ON ia2.IdAchievement = ia.IdAchievement
WHERE 
    iac.IdCategory=12 
    AND ia2.ApplicationID=@ApplicationID

SELECT 
    cg.EducationLevelID 
FROM 
    CompetitiveGroup AS CG (NOLOCK) 
    INNER JOIN ApplicationCompetitiveGroupItem AS acgi (NOLOCK) ON CG.CompetitiveGroupID = acgi.CompetitiveGroupId
WHERE 
    acgi.ApplicationId = @ApplicationID
";


            #endregion

            ApplicationWz4ViewModel appw4 = new ApplicationWz4ViewModel();
            appw4.ApplicationID = ApplicationID;
            appw4.EntrantID = ApplicationID;

            DataSet ds = null;

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("ApplicationID", ApplicationID));
                ds = new DataSet();

                try
                {
                    con.Open();
                    SqlDataAdapter sqlAdapter = new SqlDataAdapter(com);
                    sqlAdapter.Fill(ds);
                    appw4 = new ApplicationWz4ViewModel();

                    appw4.ApplicationID = ApplicationID;

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        appw4.Items.Add(new ApplicationWz4ViewModel.IndividualAchivementViewModel()
                        {
                            IAID = ds.Tables[0].Rows[i]["IAID"] as Int32?,
                            UID = ds.Tables[0].Rows[i]["IAUID"] as string,
                            IAName = ds.Tables[0].Rows[i]["IAName"] as string,
                            IAMark = ds.Tables[0].Rows[i]["IAMark"] as decimal?,
                            isAdvantageRight = ds.Tables[0].Rows[i]["isAdvantageRight"] as bool?,

                            IADocument = new ApplicationWz4ViewModel.IndividualAchivementViewModel.IADocumentViewModel()
                            {
                                ApplicationID = ds.Tables[0].Rows[i]["ApplicationID"] as Int32?,
                                EntrantDocumentID = ds.Tables[0].Rows[i]["EntrantDocumentID"] as Int32?,
                                DocumentDate = ds.Tables[0].Rows[i]["DocumentDate"] as DateTime?,
                                DocumentTypeNameText = ds.Tables[0].Rows[i]["DocumentTypeNameText"] as string,
                                DocumentSeries = ds.Tables[0].Rows[i]["DocumentSeries"] as string,
                                DocumentNumber = ds.Tables[0].Rows[i]["DocumentNumber"] as string,
                                DocumentOrganization = ds.Tables[0].Rows[i]["DocumentOrganization"] as string
                            }
                        });
                    }
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        appw4.EntrantID = ds.Tables[1].Rows[i]["EntrantID"] as Int32?;
                        appw4.ApplicationID = ds.Tables[1].Rows[i]["ApplicationID"] as Int32?;
                        appw4.InstitutionID = ds.Tables[1].Rows[i]["InstitutionID"] as Int32?;
                        appw4.StatusID = ds.Tables[1].Rows[i]["StatusID"] as Int32?;
                        appw4.CheckAchievementsSum =( (ds.Tables[1].Rows[i]["CampaignTypeID"] as short?) == GVUZ.Data.Model.CampaignTypes.BachelorOrSpecialist);
                        appw4.IndividualAchivementsMark = (ds.Tables[1].Rows[i]["IndividualAchivementsMark"] as decimal?).GetValueOrDefault();
                    }
                    for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                    {
                        appw4.IAID = ds.Tables[2].Rows[i]["IAID"] as Int32?;
                        appw4.IAchievements.Add(new ApplicationWz4ViewModel.InstitutionAchievements()
                        {
                            IdAchievement = ds.Tables[2].Rows[i]["IAID"] as Int32?,
                            CampaignID = ds.Tables[2].Rows[i]["IACampaignID"] as Int32?,
                            IdCategory = ds.Tables[2].Rows[i]["IAIdCategory"] as Int32?,
                            MaxValue = ds.Tables[2].Rows[i]["IAMaxValue"] as decimal?,
                            Name = ds.Tables[2].Rows[i]["IAName"] as string,
                            UID = ds.Tables[2].Rows[i]["IAUID"] as string,
                            //EducationLevelID = ds.Tables[2].Rows[i]["EducationLevelID"] as Int32?,
                        });
                    }                                       
                    appw4.MaxIAValues = Convert.ToDecimal(ds.Tables[3].Rows[0][0]);


                    for (int i = 0; i < ds.Tables[4].Rows.Count; i++)
                    {
                        appw4.CheckWorks = 1;
                    }

                    appw4.EducationLevelID = (ds.Tables.Count >= 6 && ds.Tables[5].Rows.Count > 0) ? (short)ds.Tables[5].Rows[0][0] : (short)0;
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
            return appw4;
        }
        #endregion

        public static int DelApplication(int applicationid)
        {
            int c = 0;
            #region SQL
            string sql = @"DELETE FROM [dbo].[Application] WHERE ApplicationId=@ApplicationID";
            #endregion
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);

                #region Параметры
                com.Parameters.Add(new SqlParameter("ApplicationID", applicationid));

                #endregion
                try
                {
                    con.Open();
                    // Вставить и вернуть новый ID
                    c = com.ExecuteNonQuery();
                    if (c != 1)
                    {
                        // Не обновилась запись
                    }
                    con.Close();
                }
                catch (SqlException e)
                {
                    // Пробросить дальше
                    throw e;
                }
                catch (Exception e)
                {
                    // Пробросить дальше
                    throw e;
                }
                finally
                {
                    if (con.State != ConnectionState.Closed) { con.Close(); }
                }
            }
            return c;
        }

        //  public static bool IsValidNewApplicationModel(ApplicationWz0Model model, int InstitutionID, ref Dictionary<string, string> validationMessages, out bool isGroupAbsent) {
        //      bool isValid = true;
        //      isGroupAbsent = false;
        //      //ситуация уже ошибочная, так что без разницы, что ошибки по одной будем выдавать
        //      model.SelectedCompetitiveGroupIDs = model.SelectedCompetitiveGroupIDs ?? new int[0];

        //      using(SqlConnection con = new SqlConnection(ConnectionString)) {
        //          try {
        //              #region Проверка на наличие групп и не нулевые группы
        //              SqlCommand comCGEx = new SqlCommand("SELECT Count(cg.name) FROM CompetitiveGroup cg WHERE cg.CompetitiveGroupID = @CompetitiveGroupID AND cg.InstitutionID = @InstitutionID", con);
        //              comCGEx.Parameters.Add(new SqlParameter("InstitutionID", SqlDbType.Int) { Value = InstitutionID });
        //              comCGEx.Parameters.Add(new SqlParameter("CompetitiveGroupID", SqlDbType.Int));

        //              SqlCommand comCGLoad = new SqlCommand(
        //                          @" SELECT Count(cg.name)  
        //	FROM CompetitiveGroupItem cgi (NOLOCK) INNER JOIN CompetitiveGroup cg (NOLOCK) on cgi.CompetitiveGroupID=cg.CompetitiveGroupID
        //	WHERE cg.InstitutionID = @InstitutionID AND (cg.CompetitiveGroupID = @CompetitiveGroupID OR @CompetitiveGroupID IS NULL)  
        //		AND
        //		( (cgi.NumberBudgetO > 0 OR cgi.NumberBudgetOZ > 0 OR cgi.NumberBudgetZ > 0 OR  cgi.NumberPaidO > 0 OR cgi.NumberPaidOZ > 0 OR cgi.NumberPaidZ > 0 )
        //			OR
        //			(0<(SELECT SUM(cgti.NumberTargetO + cgti.NumberTargetOZ + cgti.NumberTargetZ) 
        //				FROM CompetitiveGroupTargetItem cgti (NOLOCK) WHERE cgti.CompetitiveGroupItemID=cgi.CompetitiveGroupItemID
        //		)
        //	)
        //)"
        //                          , con);
        //              comCGLoad.Parameters.Add(new SqlParameter("InstitutionID", SqlDbType.Int) { Value = InstitutionID });
        //              comCGLoad.Parameters.Add(new SqlParameter("CompetitiveGroupID", SqlDbType.Int));
        //              con.Open();
        //              foreach(var selectedCompetitiveGroupID in model.SelectedCompetitiveGroupIDs) {
        //                  comCGEx.Parameters["CompetitiveGroupID"].Value = selectedCompetitiveGroupID;
        //                  if((int)comCGEx.ExecuteScalar() == 0) {
        //                      validationMessages.Add("CompetitionGroup", "Указанная конкурсная группа отсутствует");
        //                      isGroupAbsent = true;
        //                      return false;
        //                  }
        //                  //проверяем на пустые группы
        //                  comCGLoad.Parameters["CompetitiveGroupID"].Value = selectedCompetitiveGroupID;
        //                  if((int)comCGEx.ExecuteScalar() == 0) {
        //                      validationMessages["CompetitionGroup"] = "Некорректная конкурсная группа";
        //                      return false;
        //                  }
        //              }
        //              con.Close();
        //              #endregion
        //          } catch(Exception e) {
        //              con.Close();
        //              return false;
        //          } finally {
        //              con.Close();
        //          }
        //      }

        //      #region Проверка на то, что для каждого направления выбрана хотя бы одна комбинация источника финансирования и формы обучения
        //      var directionIds = model.Priorities.ApplicationPriorities
        //           .Select(x => new { x.CompetitiveGroupItemId })
        //           .Distinct()
        //           .ToArray();

        //      bool error = false;

        //      foreach(var directionId in directionIds) {
        //          var selectedItems = model.Priorities.ApplicationPriorities
        //               .Where(x => x.CompetitiveGroupItemId == directionId.CompetitiveGroupItemId && x.Priority.HasValue)
        //               .Count();

        //          if(selectedItems == 0) {
        //              validationMessages[directionId.CompetitiveGroupItemId.ToString()] = "Для специальности на выбрано ни одной комбинации формы обучения и источника финансирования";
        //              error = true;
        //          }
        //      }

        //      if(error) {
        //          validationMessages["Fake"] = "Fake";
        //          return false;
        //      }
        //      #endregion

        //      #region Проверка на то, что все приоритеты различны
        //      //if(model.CheckUniqueBeforeCreate) {
        //      //   int totalValues=model.Priorities.ApplicationPriorities.Count(x => x.Priority.HasValue&&x.Priority.Value!=0);
        //      //   int distinctValues=model.Priorities.ApplicationPriorities
        //      //       .Select(x => x.Priority)
        //      //       .Distinct()
        //      //       .Count(x => x.HasValue&&x.Value!=0);

        //      //   if(totalValues>distinctValues) {
        //      //      validationMessages["NonUniquePriorities"]="Для нескольких условий приема указаны одинаковые приоритеты. Вы уверены, что хотите продолжить?";
        //      //      return false;
        //      //   }
        //      //}
        //      #endregion

        //      #region Проверка на то, что все приоритеты - нули
        //      if(model.CheckZerozBeforeCreate) {
        //          var zeroCount = model.Priorities.ApplicationPriorities.Count(x => x.Priority.HasValue && x.Priority.Value == 0);

        //          if(zeroCount > 0 && zeroCount != model.Priorities.ApplicationPriorities.Count) {
        //              validationMessages["zeroMessage"] = "Нули могут быть указаны только в случае приема без учета приоритетов. Вы уверены, что хотите продолжить?";
        //              return false;
        //          }
        //      }
        //      #endregion

        //      if(IdentityDocumentViewModel.IsSeriesRequired(model.IdentityDocumentTypeID) && String.IsNullOrEmpty(model.DocumentSeries)) {
        //          validationMessages["DocumentSeries"] = "";
        //          validationMessages["DocumentNumber"] = "Неверная серия у документа";
        //          return false;
        //      }
        //      return isValid;
        //  }

        //public static void AddApplicationAccessToLog(AppData oldData, AppData newData, string accessMethod,int institutionID,int? applicationID=null,bool autoSave=true) { 
        //}

        public static DocumentListViewModel getDocumentForAppDocList(int ApplicationID, int EntrantDocumentID)
        {
            DocumentListViewModel d = null;
            #region SQL
            string sql = @"
SELECT ed.EntrantDocumentID 
	  ,ed.DocumentTypeID
	  ,dt.Name as DocumentTypeName
      , (COALESCE(DocumentSeries,'')+' '+COALESCE(DocumentNumber,'')) as DocumentSeriesNumber
      , DocumentDate
      , DocumentOrganization
	  , ed.AttachmentID as AttachmentID, att.Name as AttachmentName,	att.FileID as AttachmentFileID
	  , aed.ID as ApplicationEntrantDocumentID
	  , aed.OriginalReceivedDate as OriginalReceivedDate
	  , (SELECT COUNT(aed.id) FROM ApplicationEntrantDocument aed (NOLOCK) WHERE aed.EntrantDocumentID=ed.EntrantDocumentID AND aed.ApplicationID=@ApplicationID) as inThisApp
	  , (SELECT COUNT(aed.id) FROM ApplicationEntrantDocument aed (NOLOCK) WHERE aed.EntrantDocumentID=ed.EntrantDocumentID ) as inAppCount
	  , (SELECT COUNT(aetd.id) FROM ApplicationEntranceTestDocument aetd (NOLOCK) WHERE aetd.EntrantDocumentID=ed.EntrantDocumentID ) as inTestCount
	  , (SELECT COUNT(e.IdentityDocumentID) FROM Entrant e (NOLOCK) WHERE e.EntrantID=ed.EntrantID AND e.IdentityDocumentID=ed.EntrantDocumentID) as  isIdentity
      ,ed.OlympApproved
FROM EntrantDocument ed (NOLOCK)
	INNER JOIN DocumentType dt (NOLOCK) on ed.DocumentTypeID=dt.DocumentID
	LEFT OUTER JOIN Attachment att (NOLOCK) on att.AttachmentID=ed.AttachmentID
	LEFT OUTER JOIN ApplicationEntrantDocument aed (NOLOCK) on aed.EntrantDocumentID=ed.EntrantDocumentID and aed.ApplicationID=@ApplicationID
WHERE ed.EntrantDocumentID=@EntrantDocumentID
";
            #endregion
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("ApplicationID", ApplicationID));
                com.Parameters.Add(new SqlParameter("EntrantDocumentID", EntrantDocumentID));

                try
                {
                    con.Open();
                    SqlDataReader r = com.ExecuteReader();
                    object o;
                    int inAppCount;
                    int inTestCount;
                    int isIdentity;
                    bool inThisApp;
                    #region Read
                    if (r.Read())
                    {
                        d = new DocumentListViewModel();
                        d.EntrantDocumentID = (int)r["EntrantDocumentID"];
                        d.DocumentTypeID = (int)r["DocumentTypeID"];
                        d.DocumentTypeName = r["DocumentTypeName"] as string;
                        d.DocumentSeriesNumber = r["DocumentSeriesNumber"] as string; 

                        if (!r.IsDBNull(r.GetOrdinal("DocumentDate")))
                        {
                            if ((int)r["DocumentTypeID"] as int? == 2)
                            {
                                d.DocumentDate = ((DateTime)r["DocumentDate"]).ToString("yyyy");
                            }
                            else
                            {
                                d.DocumentDate = ((DateTime)r["DocumentDate"]).ToString("dd.MM.yyyy");
                            }
                        }

                        d.DocumentOrganization = r["DocumentOrganization"] as string;
                        d.AttachmentID = r["AttachmentID"] as int?;
                        d.AttachmentName = r["AttachmentName"] as string;
                        d.AttachmentFileID = r["AttachmentFileID"] as Guid?;
                        o = r["OriginalReceivedDate"];
                        if (o != null && o != DBNull.Value)
                        {
                            d.OriginalReceivedDate = ((DateTime)r["OriginalReceivedDate"]).ToString("dd.MM.yyyy");
                            d.OriginalReceived = true;
                        }
                        else
                        {
                            d.OriginalReceivedDate = "";
                            d.OriginalReceived = false;
                        }
                        inThisApp = (1 == (int)r["inThisApp"]);
                        if (inThisApp) { d.ApplicationID = ApplicationID; }
                        inAppCount = (int)r["inAppCount"];
                        inTestCount = (int)r["inTestCount"];
                        isIdentity = (int)r["isIdentity"];
                        //можно редактировать, если не используется в ВИ данного или другого заявления
                        d.CanBeModified = (inTestCount == 0);
                        //можно открепить, если не используется в данном заявлении где-нибудь еще
                        //	CanBeDetached=!(x.Entrant_IdentityDocument.Any(u => u.EntrantID==app.EntrantID)||x.ApplicationEntranceTestDocument.Any(y => y.ApplicationID==app.ApplicationID)),
                        d.CanBeDetached = (isIdentity == 0);
                        d.CanBeDeleted = (isIdentity == 0) && !(inAppCount > 0 || inTestCount > 0);
                        d.ShowWarnBeforeModifying = ((inThisApp && inAppCount > 1) || !inThisApp && inAppCount > 0);


                        bool olympApproved = (r["OlympApproved"] as bool?).GetValueOrDefault();
                        if (d.DocumentTypeID == 9 || d.DocumentTypeID == 10)
                        {
                            d.DocumentTypeName += olympApproved ? " (результаты подтверждены)" : " (результаты не подтверждены)";
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
            return d;
        }


        public static DocumentListViewModel AttachDocument(int ApplicationID, int EntrantDocumentID)
        {
            DocumentListViewModel d = null;
            #region SQL
            string sql = @"
SET NOCOUNT ON
--BEGIN TRANSACTION Tran1
  IF NOT EXISTS( SELECT ID FROM  [ApplicationEntrantDocument]  (NOLOCK) WHERE ApplicationID=@ApplicationID AND EntrantDocumentID=@EntrantDocumentID  ) BEGIN
	INSERT INTO ApplicationEntrantDocument(ApplicationID,EntrantDocumentID)VALUES(@ApplicationID,@EntrantDocumentID)
  END
--COMMIT TRANSACTION Tran1
SET NOCOUNT OFF

SELECT ed.EntrantDocumentID 
	  ,ed.DocumentTypeID
	  ,dt.Name as DocumentTypeName
      , (COALESCE(DocumentSeries,'')+' '+COALESCE(DocumentNumber,'')) as DocumentSeriesNumber
      , DocumentDate
      , DocumentOrganization
	  , ed.AttachmentID as AttachmentID, att.Name as AttachmentName,	att.FileID as AttachmentFileID
	  , aed.ID as ApplicationEntrantDocumentID
	  , aed.OriginalReceivedDate as OriginalReceivedDate
	  , (SELECT COUNT(aed.id) FROM ApplicationEntrantDocument aed WHERE aed.EntrantDocumentID=ed.EntrantDocumentID AND aed.ApplicationID=@ApplicationID) as inThisApp
	  , (SELECT COUNT(aed.id) FROM ApplicationEntrantDocument aed WHERE aed.EntrantDocumentID=ed.EntrantDocumentID ) as inAppCount
	  , (SELECT COUNT(aetd.id) FROM ApplicationEntranceTestDocument aetd WHERE  aetd.ApplicationID=@ApplicationID AND aetd.EntrantDocumentID=ed.EntrantDocumentID ) as inTestCount
     , (SELECT COUNT(aetd.id) FROM ApplicationEntranceTestDocument aetd (NOLOCK) WHERE aetd.EntrantDocumentID=ed.EntrantDocumentID AND aed.ApplicationID=@ApplicationID) as inThisAppTestCount
	  , (SELECT COUNT(e.IdentityDocumentID) FROM Entrant e (NOLOCK) WHERE e.EntrantID=ed.EntrantID AND e.IdentityDocumentID=ed.EntrantDocumentID) as  isIdentity
      ,ed.OlympApproved
FROM EntrantDocument ed (NOLOCK)
	INNER JOIN DocumentType dt (NOLOCK) on ed.DocumentTypeID=dt.DocumentID
	LEFT OUTER JOIN Attachment att (NOLOCK) on att.AttachmentID=ed.AttachmentID
	LEFT OUTER JOIN ApplicationEntrantDocument aed (NOLOCK) on aed.EntrantDocumentID=ed.EntrantDocumentID and aed.ApplicationID=@ApplicationID
WHERE ed.EntrantDocumentID=@EntrantDocumentID
";
            #endregion
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("ApplicationID", ApplicationID));
                com.Parameters.Add(new SqlParameter("EntrantDocumentID", EntrantDocumentID));

                try
                {
                    con.Open();
                    SqlDataReader r = com.ExecuteReader();
                    object o;
                    int inAppCount;
                    int inTestCount;
                    int isIdentity;
                    bool inThisApp;
                    int inThisAppTestCount;
                    #region Read
                    if (r.Read())
                    {
                        d = new DocumentListViewModel();
                        d.EntrantDocumentID = (int)r["EntrantDocumentID"];
                        d.DocumentTypeID = (int)r["DocumentTypeID"];
                        d.DocumentTypeName = r["DocumentTypeName"] as string;
                        d.DocumentSeriesNumber = r["DocumentSeriesNumber"] as string;
                        if (!r.IsDBNull(r.GetOrdinal("DocumentDate")))
                        {
                            if (d.DocumentTypeID == 2)
                            {
                                d.DocumentDate = ((DateTime)r["DocumentDate"]).ToString("yyyy");
                            }
                            else
                            {
                                d.DocumentDate = ((DateTime)r["DocumentDate"]).ToString("dd.MM.yyyy");
                            }
                        }
                        d.DocumentOrganization = r["DocumentOrganization"] as string;
                        d.AttachmentID = r["AttachmentID"] as int?;
                        d.AttachmentName = r["AttachmentName"] as string;
                        d.AttachmentFileID = r["AttachmentFileID"] as Guid?;
                        o = r["OriginalReceivedDate"];
                        if (o != null && o != DBNull.Value)
                        {
                            d.OriginalReceivedDate = ((DateTime)r["OriginalReceivedDate"]).ToString("dd.MM.yyyy");
                            d.OriginalReceived = true;
                        }
                        else
                        {
                            d.OriginalReceivedDate = "";
                            d.OriginalReceived = false;
                        }
                        inThisApp = (1 == (int)r["inThisApp"]);
                        inAppCount = (int)r["inAppCount"];
                        inTestCount = (int)r["inTestCount"];
                        isIdentity = (int)r["isIdentity"];
                        inThisAppTestCount = (int)r["inThisAppTestCount"];
                        //можно редактировать, если не используется в ВИ данного заявления
                        //d.CanBeModified=(inTestCount==0);
                        d.CanBeModified = (inThisAppTestCount == 0);
                        //можно открепить, если не используется в данном заявлении где-нибудь еще
                        //	CanBeDetached=!(x.Entrant_IdentityDocument.Any(u => u.EntrantID==app.EntrantID)||x.ApplicationEntranceTestDocument.Any(y => y.ApplicationID==app.ApplicationID)),
                        d.CanBeDetached = (isIdentity == 0);
                        d.CanBeDeleted = false;  //прикрепленные доки нельзя удалять
                        d.ShowWarnBeforeModifying = ((inThisApp && inAppCount > 1) || !inThisApp && inAppCount > 0 || inTestCount > 0);

                        bool olympApproved = (r["OlympApproved"] as bool?).GetValueOrDefault();
                        if (d.DocumentTypeID == 9 || d.DocumentTypeID == 10)
                        {
                            d.DocumentTypeName += olympApproved ? " (результаты подтверждены)" : " (результаты не подтверждены)";
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
            return d;
        }

        public static DocumentListViewModel DetachDocument(int ApplicationID, int EntrantDocumentID)
        {
            DocumentListViewModel d = null;
            #region SQL
            string sql = @"
SET NOCOUNT ON
	DELETE FROM [ApplicationEntrantDocument]		WHERE ApplicationID=@ApplicationID AND EntrantDocumentID=@EntrantDocumentID
	DELETE FROM ApplicationEntranceTestDocument	WHERE ApplicationID=@ApplicationID AND EntrantDocumentID=@EntrantDocumentID
SET NOCOUNT OFF
SELECT ed.EntrantDocumentID 
	  ,ed.DocumentTypeID
	  ,dt.Name as DocumentTypeName
      , (COALESCE(DocumentSeries,'')+' '+COALESCE(DocumentNumber,'')) as DocumentSeriesNumber
      , DocumentDate
      , DocumentOrganization
	  , ed.AttachmentID as AttachmentID, att.Name as AttachmentName,	att.FileID as AttachmentFileID
	  , aed.ID as ApplicationEntrantDocumentID
	  , aed.OriginalReceivedDate as OriginalReceivedDate
	  , (SELECT COUNT(aed.id) FROM ApplicationEntrantDocument aed (NOLOCK) WHERE aed.EntrantDocumentID=ed.EntrantDocumentID AND aed.ApplicationID=@ApplicationID) as inThisApp
	  , (SELECT COUNT(aed.id) FROM ApplicationEntrantDocument aed (NOLOCK) WHERE aed.EntrantDocumentID=ed.EntrantDocumentID ) as inAppCount
	  , (SELECT COUNT(aetd.id) FROM ApplicationEntranceTestDocument aetd (NOLOCK) WHERE  aetd.ApplicationID=@ApplicationID AND aetd.EntrantDocumentID=ed.EntrantDocumentID ) as inTestCount
     , (SELECT COUNT(aetd.id) FROM ApplicationEntranceTestDocument aetd (NOLOCK) WHERE aetd.EntrantDocumentID=ed.EntrantDocumentID AND aed.ApplicationID=@ApplicationID) as inThisAppTestCount
	  , (SELECT COUNT(e.IdentityDocumentID) FROM Entrant e (NOLOCK) WHERE e.EntrantID=ed.EntrantID AND e.IdentityDocumentID=ed.EntrantDocumentID) as  isIdentity
      ,ed.OlympApproved
FROM EntrantDocument ed (NOLOCK)
	INNER JOIN DocumentType dt (NOLOCK) on ed.DocumentTypeID=dt.DocumentID
	LEFT OUTER JOIN Attachment att (NOLOCK) on att.AttachmentID=ed.AttachmentID
	LEFT OUTER JOIN ApplicationEntrantDocument aed (NOLOCK) on aed.EntrantDocumentID=ed.EntrantDocumentID and aed.ApplicationID=@ApplicationID
WHERE ed.EntrantDocumentID=@EntrantDocumentID
";
            #endregion
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("ApplicationID", ApplicationID));
                com.Parameters.Add(new SqlParameter("EntrantDocumentID", EntrantDocumentID));

                try
                {
                    con.Open();
                    SqlDataReader r = com.ExecuteReader();
                    object o;
                    int inAppCount;
                    int inTestCount;
                    int isIdentity;
                    bool inThisApp;
                    int inThisAppTestCount;
                    #region Read
                    if (r.Read())
                    {
                        d = new DocumentListViewModel();
                        d.EntrantDocumentID = (int)r["EntrantDocumentID"];
                        d.DocumentTypeID = (int)r["DocumentTypeID"];
                        d.DocumentTypeName = r["DocumentTypeName"] as string;
                        d.DocumentSeriesNumber = r["DocumentSeriesNumber"] as string;
                        if (!r.IsDBNull(r.GetOrdinal("DocumentDate")))
                        {
                            if (d.DocumentTypeID == 2)
                            {
                                d.DocumentDate = ((DateTime)r["DocumentDate"]).ToString("yyyy");
                            }
                            else
                            {
                                d.DocumentDate = ((DateTime)r["DocumentDate"]).ToString("dd.MM.yyyy");
                            }
                        }
                        d.DocumentOrganization = r["DocumentOrganization"] as string;
                        d.AttachmentID = r["AttachmentID"] as int?;
                        d.AttachmentName = r["AttachmentName"] as string;
                        d.AttachmentFileID = r["AttachmentFileID"] as Guid?;
                        o = r["OriginalReceivedDate"];
                        if (o != null && o != DBNull.Value)
                        {
                            d.OriginalReceivedDate = ((DateTime)r["OriginalReceivedDate"]).ToString("dd.MM.yyyy");
                            d.OriginalReceived = true;
                        }
                        else
                        {
                            d.OriginalReceivedDate = "";
                            d.OriginalReceived = false;
                        }
                        inThisApp = (1 == (int)r["inThisApp"]);
                        inAppCount = (int)r["inAppCount"];
                        inTestCount = (int)r["inTestCount"];
                        isIdentity = (int)r["isIdentity"];
                        inThisAppTestCount = (int)r["inThisAppTestCount"];

                        //можно редактировать, если не используется в ВИ данного заявления
                        //d.CanBeModified=(inTestCount==0);
                        d.CanBeModified = (inThisAppTestCount == 0);
                        //можно открепить, если не используется в данном заявлении где-нибудь еще
                        //	CanBeDetached=!(x.Entrant_IdentityDocument.Any(u => u.EntrantID==app.EntrantID)||x.ApplicationEntranceTestDocument.Any(y => y.ApplicationID==app.ApplicationID)),
                        d.CanBeDetached = (isIdentity == 0);
                        d.CanBeDeleted = (isIdentity == 0) && !(inAppCount > 0 || inTestCount > 0);
                        d.ShowWarnBeforeModifying = ((inThisApp && inAppCount > 1) || !inThisApp && inAppCount > 0 || inTestCount > 0);

                        bool olympApproved = (r["OlympApproved"] as bool?).GetValueOrDefault();
                        if (d.DocumentTypeID == 9 || d.DocumentTypeID == 10)
                        {
                            d.DocumentTypeName += olympApproved ? " (результаты подтверждены)" : " (результаты не подтверждены)";
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
                SpSetDocumentReceived(ApplicationID);
            }
            return d;
        }

        public static DocumentListViewModel DeleteDocument(int ApplicationID, int EntrantDocumentID)
        {
            DocumentListViewModel d = null;
            #region SQL
            string sql = @"
SET NOCOUNT ON
	DELETE FROM [ApplicationEntrantDocument]	WHERE EntrantDocumentID=@EntrantDocumentID
	DELETE FROM ApplicationEntranceTestDocument	WHERE EntrantDocumentID=@EntrantDocumentID
    DELETE FROM EntrantDocument	WITH (ROWLOCK)  WHERE EntrantDocumentID=@EntrantDocumentID
SET NOCOUNT OFF
SELECT ed.EntrantDocumentID 
	  ,ed.DocumentTypeID
	  ,dt.Name as DocumentTypeName
      , (COALESCE(DocumentSeries,'')+' '+COALESCE(DocumentNumber,'')) as DocumentSeriesNumber
      , DocumentDate
      , DocumentOrganization
	  , ed.AttachmentID as AttachmentID, att.Name as AttachmentName,	att.FileID as AttachmentFileID
	  , aed.ID as ApplicationEntrantDocumentID
	  , aed.OriginalReceivedDate as OriginalReceivedDate
	  , (SELECT COUNT(aed.id) FROM ApplicationEntrantDocument aed (NOLOCK) WHERE aed.EntrantDocumentID=ed.EntrantDocumentID AND aed.ApplicationID=@ApplicationID) as inThisApp
	  , (SELECT COUNT(aed.id) FROM ApplicationEntrantDocument aed (NOLOCK) WHERE aed.EntrantDocumentID=ed.EntrantDocumentID ) as inAppCount
	  , (SELECT COUNT(aetd.id) FROM ApplicationEntranceTestDocument aetd (NOLOCK) WHERE  aetd.ApplicationID=@ApplicationID AND aetd.EntrantDocumentID=ed.EntrantDocumentID ) as inTestCount
     , (SELECT COUNT(aetd.id) FROM ApplicationEntranceTestDocument aetd (NOLOCK) WHERE aetd.EntrantDocumentID=ed.EntrantDocumentID AND aed.ApplicationID=@ApplicationID) as inThisAppTestCount
	  , (SELECT COUNT(e.IdentityDocumentID) FROM Entrant e (NOLOCK) WHERE e.EntrantID=ed.EntrantID AND e.IdentityDocumentID=ed.EntrantDocumentID) as  isIdentity
      ,ed.OlympApproved
FROM EntrantDocument ed (NOLOCK)
	INNER JOIN DocumentType dt (NOLOCK) on ed.DocumentTypeID=dt.DocumentID
	LEFT OUTER JOIN Attachment att (NOLOCK) on att.AttachmentID=ed.AttachmentID
	LEFT OUTER JOIN ApplicationEntrantDocument aed (NOLOCK) on aed.EntrantDocumentID=ed.EntrantDocumentID and aed.ApplicationID=@ApplicationID
WHERE ed.EntrantDocumentID=@EntrantDocumentID
";
            #endregion
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("ApplicationID", ApplicationID));
                com.Parameters.Add(new SqlParameter("EntrantDocumentID", EntrantDocumentID));

                try
                {
                    con.Open();
                    SqlDataReader r = com.ExecuteReader();
                    object o;
                    int inAppCount;
                    int inTestCount;
                    int isIdentity;
                    bool inThisApp;
                    int inThisAppTestCount;
                    #region Read
                    if (r.Read())
                    {
                        d = new DocumentListViewModel();
                        d.EntrantDocumentID = (int)r["EntrantDocumentID"];
                        d.DocumentTypeID = (int)r["DocumentTypeID"];
                        d.DocumentTypeName = r["DocumentTypeName"] as string;
                        d.DocumentSeriesNumber = r["DocumentSeriesNumber"] as string;
                        if (!r.IsDBNull(r.GetOrdinal("DocumentDate")))
                        {
                            if (d.DocumentTypeID == 2)
                            {
                                d.DocumentDate = ((DateTime)r["DocumentDate"]).ToString("yyyy");
                            }
                            else
                            {
                                d.DocumentDate = ((DateTime)r["DocumentDate"]).ToString("dd.MM.yyyy");
                            }
                        }
                        d.DocumentOrganization = r["DocumentOrganization"] as string;
                        d.AttachmentID = r["AttachmentID"] as int?;
                        d.AttachmentName = r["AttachmentName"] as string;
                        d.AttachmentFileID = r["AttachmentFileID"] as Guid?;
                        o = r["OriginalReceivedDate"];
                        if (o != null && o != DBNull.Value)
                        {
                            d.OriginalReceivedDate = ((DateTime)r["OriginalReceivedDate"]).ToString("dd.MM.yyyy");
                            d.OriginalReceived = true;
                        }
                        else
                        {
                            d.OriginalReceivedDate = "";
                            d.OriginalReceived = false;
                        }
                        inThisApp = (1 == (int)r["inThisApp"]);
                        inAppCount = (int)r["inAppCount"];
                        inTestCount = (int)r["inTestCount"];
                        isIdentity = (int)r["isIdentity"];
                        inThisAppTestCount = (int)r["inThisAppTestCount"];

                        //можно редактировать, если не используется в ВИ данного заявления
                        //d.CanBeModified=(inTestCount==0);
                        d.CanBeModified = (inThisAppTestCount == 0);
                        //можно открепить, если не используется в данном заявлении где-нибудь еще
                        //	CanBeDetached=!(x.Entrant_IdentityDocument.Any(u => u.EntrantID==app.EntrantID)||x.ApplicationEntranceTestDocument.Any(y => y.ApplicationID==app.ApplicationID)),
                        d.CanBeDetached = (isIdentity == 0);
                        d.CanBeDeleted = (isIdentity == 0) && !(inAppCount > 0 || inTestCount > 0);
                        d.ShowWarnBeforeModifying = ((inThisApp && inAppCount > 1) || !inThisApp && inAppCount > 0 || inTestCount > 0);

                        bool olympApproved = (r["OlympApproved"] as bool?).GetValueOrDefault();
                        if (d.DocumentTypeID == 9 || d.DocumentTypeID == 10)
                        {
                            d.DocumentTypeName += olympApproved ? " (результаты подтверждены)" : " (результаты не подтверждены)";
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
                SpSetDocumentReceived(ApplicationID);
            }
            return d;
        }

        public static DocumentListViewModel SetDocumentReceived(int ApplicationID, int EntrantDocumentID, bool DocumentsReceived, DateTime? ReceivedDate)
        {
            DocumentListViewModel d = null;
            #region SQL
            string sql = @"
SET NOCOUNT ON
IF @DocumentsReceived=1  BEGIN
    UPDATE [ApplicationEntrantDocument] SET OriginalReceivedDate=@ReceivedDate WHERE ApplicationID=@ApplicationID AND EntrantDocumentID=@EntrantDocumentID
END
ELSE BEGIN
    UPDATE [ApplicationEntrantDocument] SET OriginalReceivedDate=NULL WHERE ApplicationID=@ApplicationID AND EntrantDocumentID=@EntrantDocumentID
END
    

		

	--IF @DocumentsReceived= 0  BEGIN 
		--SELECT TOP 1 @ReceivedDate=aed.OriginalReceivedDate, @DocumentsReceived=1 
			--FROM [ApplicationEntrantDocument] aed 
			--INNER JOIN  EntrantDocument ed on ed.EntrantDocumentID=aed.EntrantDocumentID
			--WHERE aed.ApplicationID=@ApplicationID AND aed.OriginalReceivedDate IS NOT NULL 
			--and ed.DocumentTypeID in (3,4,5,6,7,8,16,19,25,26)
	--END
	--UPDATE [Application] SET OriginalDocumentsReceived=@DocumentsReceived, OriginalDocumentsReceivedDate=@ReceivedDate WHERE ApplicationId=@ApplicationID
SET NOCOUNT OFF
SELECT ed.EntrantDocumentID 
	  ,ed.DocumentTypeID
	  ,dt.Name as DocumentTypeName
      , (COALESCE(DocumentSeries,'')+' '+COALESCE(DocumentNumber,'')) as DocumentSeriesNumber
      , DocumentDate
      , DocumentOrganization
	  , ed.AttachmentID as AttachmentID, att.Name as AttachmentName,	att.FileID as AttachmentFileID
	  , aed.ID as ApplicationEntrantDocumentID
	  , aed.OriginalReceivedDate as OriginalReceivedDate
	  , (SELECT COUNT(aed.id) FROM ApplicationEntrantDocument aed (NOLOCK) WHERE aed.EntrantDocumentID=ed.EntrantDocumentID AND aed.ApplicationID=@ApplicationID) as inThisApp
	  , (SELECT COUNT(aed.id) FROM ApplicationEntrantDocument aed (NOLOCK) WHERE aed.EntrantDocumentID=ed.EntrantDocumentID ) as inAppCount
	  , (SELECT COUNT(aetd.id) FROM ApplicationEntranceTestDocument aetd (NOLOCK) WHERE aetd.EntrantDocumentID=ed.EntrantDocumentID ) as inTestCount
	  , (SELECT COUNT(e.IdentityDocumentID) FROM Entrant e (NOLOCK) WHERE e.EntrantID=ed.EntrantID AND e.IdentityDocumentID=ed.EntrantDocumentID) as  isIdentity
FROM EntrantDocument ed (NOLOCK)
	INNER JOIN DocumentType dt (NOLOCK) on ed.DocumentTypeID=dt.DocumentID
	LEFT OUTER JOIN Attachment att (NOLOCK) on att.AttachmentID=ed.AttachmentID
	LEFT OUTER JOIN ApplicationEntrantDocument aed (NOLOCK) on aed.EntrantDocumentID=ed.EntrantDocumentID and aed.ApplicationID=@ApplicationID
WHERE ed.EntrantDocumentID=@EntrantDocumentID
";
            #endregion
            if (!DocumentsReceived)
            {
                ReceivedDate = null;
            }
            else
            {
                if (ReceivedDate == null)
                {
                    ReceivedDate = DateTime.Today;
                }
            }
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("ApplicationID", ApplicationID));
                com.Parameters.Add(new SqlParameter("EntrantDocumentID", EntrantDocumentID));
                com.Parameters.Add(new SqlParameter("DocumentsReceived", DbType.Boolean) { Value = DocumentsReceived });
                if (ReceivedDate.HasValue)
                {
                    com.Parameters.Add(new SqlParameter("ReceivedDate", SqlDbType.DateTime) { Value = ReceivedDate });
                }
                else
                {
                    com.Parameters.Add(new SqlParameter("ReceivedDate", SqlDbType.DateTime) { Value = DBNull.Value });
                }

                try
                {
                    con.Open();
                    SqlDataReader r = com.ExecuteReader();
                    object o;
                    int inAppCount, inTestCount, isIdentity;
                    bool inThisApp;
                    #region Read
                    if (r.Read())
                    {
                        d = new DocumentListViewModel();
                        d.EntrantDocumentID = (int)r["EntrantDocumentID"];
                        d.DocumentTypeID = (int)r["DocumentTypeID"];
                        d.DocumentTypeName = r["DocumentTypeName"] as string;
                        d.DocumentSeriesNumber = r["DocumentSeriesNumber"] as string;
                        if (!r.IsDBNull(r.GetOrdinal("DocumentDate")))
                        {
                            d.DocumentDate = ((DateTime)r["DocumentDate"]).ToString("dd.MM.yyyy");
                        }
                        d.DocumentOrganization = r["DocumentOrganization"] as string;
                        d.AttachmentID = r["AttachmentID"] as int?;
                        d.AttachmentName = r["AttachmentName"] as string;
                        d.AttachmentFileID = r["AttachmentFileID"] as Guid?;
                        o = r["OriginalReceivedDate"];
                        if (o != null && o != DBNull.Value)
                        {
                            d.OriginalReceivedDate = ((DateTime)r["OriginalReceivedDate"]).ToString("dd.MM.yyyy");
                            d.OriginalReceived = true;
                        }
                        else
                        {
                            d.OriginalReceivedDate = "";
                            d.OriginalReceived = false;
                        }
                        inThisApp = (1 == (int)r["inThisApp"]);
                        inAppCount = (int)r["inAppCount"];
                        inTestCount = (int)r["inTestCount"];
                        isIdentity = (int)r["isIdentity"];
                        //можно редактировать, если не используется в ВИ данного или другого заявления
                        d.CanBeModified = (inTestCount == 0);
                        //можно открепить, если не используется в данном заявлении где-нибудь еще
                        //	CanBeDetached=!(x.Entrant_IdentityDocument.Any(u => u.EntrantID==app.EntrantID)||x.ApplicationEntranceTestDocument.Any(y => y.ApplicationID==app.ApplicationID)),
                        d.CanBeDetached = (isIdentity == 0);
                        d.CanBeDeleted = (isIdentity == 0) && !(inAppCount > 0 || inTestCount > 0);
                        d.ShowWarnBeforeModifying = ((inThisApp && inAppCount > 1) || !inThisApp && inAppCount > 0);
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
            SpSetDocumentReceived(ApplicationID);

            return d;
        }

        public static SPResult SpSetDocumentReceived(int applicationId)
        {
            SPResult result = null;

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand("SPSetDocumentReceived", con);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@returnValue", SqlDbType.Int) { Direction = ParameterDirection.ReturnValue });
                    command.Parameters.Add(new SqlParameter("@ApplicationID", SqlDbType.Int) { Value = applicationId });

                    con.Open();
                    command.ExecuteNonQuery();

                    result = new SPResult
                    {
                        returnValue = Convert.ToBoolean(command.Parameters["@returnValue"].Value as int?)
                    };
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

        //#DocumentsCheck - проверка наличия документов
        public static bool CheckApplicationDocuments(int applicationId, out IEnumerable<string> requiredDocuments)
        {
            string sql = @"
IF (NOT(EXISTS (
	SELECT DISTINCT ed.DocumentTypeID
	FROM Application app (NOLOCK)
		INNER JOIN ApplicationEntrantDocument aed (NOLOCK) on aed.ApplicationID=app.ApplicationID
		INNER JOIN EntrantDocument ed (NOLOCK) ON aed.EntrantDocumentID = ed.EntrantDocumentID
	WHERE aed.ApplicationID = @ApplicationID AND ed.DocumentTypeID<>1
		AND ed.DocumentTypeID in (
				SELECT DISTINCT eldy.DocumentTypeId
				FROM ApplicationCompetitiveGroupItem acgi (NOLOCK)
	INNER JOIN CompetitiveGroup cg (NOLOCK) on acgi.CompetitiveGroupID=cg.CompetitiveGroupID
	INNER JOIN EduLevels el on el.AdmissionItemTypeID=cg.EducationLevelID
	INNER JOIN EduLevelDocumentType eldy on eldy.LevelID=el.LevelID
				WHERE acgi.ApplicationID = @ApplicationID AND eldy.DocumentTypeId<>1
			)
			AND ed.DocumentTypeId<>1
    ))
AND ((EXISTS(
        SELECT DISTINCT ed.DocumentTypeID
	        FROM Application app (NOLOCK)
		        INNER JOIN ApplicationEntrantDocument aed (NOLOCK) on aed.ApplicationID=app.ApplicationID
		        INNER JOIN EntrantDocument ed (NOLOCK) ON aed.EntrantDocumentID = ed.EntrantDocumentID
	        WHERE aed.ApplicationID = @ApplicationID AND ed.DocumentTypeID<>1
		        AND ed.DocumentTypeID in (SELECT dt.DocumentID FROM DocumentType dt
                    WHERE dt.DocumentID in (4,5,6) 
                        AND (EXISTS(SELECT * FROM ApplicationCompetitiveGroupItem acgi (NOLOCK)
                                    INNER JOIN CompetitiveGroup cg (NOLOCK) on acgi.CompetitiveGroupID=cg.CompetitiveGroupID
                                    INNER JOIN Campaign c (NOLOCK) on cg.CampaignID = c.CampaignID
                                    WHERE acgi.ApplicationID = @ApplicationID AND acgi.IsForSPOandVO = 1 AND c.CampaignTypeID = 1))
             )
        )
    )
    OR
    (NOT EXISTS(SELECT * FROM ApplicationCompetitiveGroupItem acgi (NOLOCK)
                                    INNER JOIN CompetitiveGroup cg (NOLOCK) on acgi.CompetitiveGroupID=cg.CompetitiveGroupID
                                    INNER JOIN Campaign c (NOLOCK) on cg.CampaignID = c.CampaignID
                                    WHERE acgi.ApplicationID = @ApplicationID AND acgi.IsForSPOandVO = 1 AND c.CampaignTypeID = 1)
    )
    )
)
BEGIN
	SELECT DocumentTypeId as ID, Name FROM 
	(
    -- Требуемые документы
	    SELECT DISTINCT eldy.DocumentTypeId
	    FROM ApplicationCompetitiveGroupItem acgi (NOLOCK)
	    INNER JOIN CompetitiveGroup cg (NOLOCK) on acgi.CompetitiveGroupID=cg.CompetitiveGroupID
	    INNER JOIN EduLevels el on el.AdmissionItemTypeID=cg.EducationLevelID
	    INNER JOIN EduLevelDocumentType eldy on eldy.LevelID=el.LevelID
	    WHERE acgi.ApplicationID = @ApplicationID

    UNION
        SELECT dt.DocumentID FROM DocumentType dt
        WHERE dt.DocumentID in (4,5,6) 
            AND (EXISTS(SELECT * FROM ApplicationCompetitiveGroupItem acgi (NOLOCK)
                        INNER JOIN CompetitiveGroup cg (NOLOCK) on acgi.CompetitiveGroupID=cg.CompetitiveGroupID
                        INNER JOIN Campaign c (NOLOCK) on cg.CampaignID = c.CampaignID
                        WHERE acgi.ApplicationID = @ApplicationID AND acgi.IsForSPOandVO = 1 AND c.CampaignTypeID = 1))

	) as exDT
	INNER JOIN DocumentType dt on exDT.DocumentTypeId=dt.DocumentID
	WHERE exDT.DocumentTypeId<>1
END
";

            List<string> requiredDocumentsList = new List<string>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.Add("@ApplicationID", SqlDbType.Int).Value = applicationId;

                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        requiredDocumentsList.Add(reader["Name"] as string);
                    }
                }
            }
            requiredDocuments = requiredDocumentsList;
            return !requiredDocumentsList.Any();
        }

        public static bool GetChekcEGE(int ApplicationId)
        {
            bool result = false;

            string sql = @"
            --SELECT * FROM ApplicationEntranceTestDocument AS aetd 
            --WHERE aetd.ApplicationID=@ApplicationID
            --AND (aetd.EntrantDocumentID IN
            --(
			--	SELECT aed.EntrantDocumentID
			--	FROM ApplicationEntrantDocument aed
			--		INNER JOIN EntrantDocument AS ed
			--			ON ed.EntrantDocumentID = aed.EntrantDocumentID
			--	WHERE ApplicationID=@ApplicationID AND ed.DocumentTypeID IN (2,9,10)
            --) OR aetd.EntrantDocumentID IS NULL)

            SELECT * FROM ApplicationCompetitiveGroupItem AS acgi (NOLOCK)
	            INNER JOIN CompetitiveGroup AS cg (NOLOCK)
					ON cg.CompetitiveGroupID = acgi.CompetitiveGroupId
            WHERE acgi.ApplicationId=@ApplicationID
                AND cg.EducationLevelID IN (2,3,5,19)
            ";

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                try
                {
                    SqlCommand com = new SqlCommand(sql, con);
                    com.Parameters.Add(new SqlParameter("ApplicationID", ApplicationId));

                    con.Open();
                    DataSet ds = new DataSet();
                    SqlDataAdapter sqlAdapter = new SqlDataAdapter(com);
                    sqlAdapter.Fill(ds);
                    //if (ds.Tables[0].Rows.Count > 0)
                    //{
                    //    result = true;
                    //}
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        result = true; ////false
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

        #region CheckEGEResults --- Получить/проверить результаты ЕГЭ

        /// <summary>
        /// 2.2.5.1.6	Указание баллов по кнопке «Получить/проверить результаты ЕГЭ»
        /// </summary>
        /// <returns></returns>
        public static SPResult GetCheckEGEResults(AppResultsModel model, log4net.ILog c_logger = null)
        {
            SPResult result = null;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.InfoMessage += (sndr, evt) =>
                {
                    List<int> error_codes = new List<int>();
                    int error_count = 0;
                    foreach (System.Data.SqlClient.SqlError msg in evt.Errors)
                    {
                        error_count++;
                        string error = string.Format("{0} {1} {2}: {3}", msg.Source, msg.State, msg.Number, msg.Message);
                        Console.WriteLine(error);
                        //messages.Add(error);
                        if (c_logger != null)
                        {
                            if (msg.State >= 10)
                                c_logger.ErrorFormat("Заявление №{0} (пользователь {1}) -> {2} [{3} {4}]: {5}",
                                                      model.ApplicationID, model.userLogin, msg.Source, msg.State, msg.Number, msg.Message);
                            else
                                c_logger.DebugFormat("Заявление №{0} (пользователь {1}) -> {2} [{3} {4}]: {5}",
                                                      model.ApplicationID, model.userLogin, msg.Source, msg.State, msg.Number, msg.Message);

                        }
                    }
                    return;
                };

                try
                {
                    SqlCommand command = new SqlCommand("FindEntrantEGEMarks", con);

                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 600; // попробовать 600 секунд!
                    command.Parameters.Add(new SqlParameter("@userLogin", SqlDbType.VarChar) { Value = model.userLogin });
                    ///	метод поиска: “по ФИО и серии, номеру паспорта“
                    command.Parameters.Add(new SqlParameter("@method", SqlDbType.VarChar) { Value = model.method });
                    ///	идентификатор заявления (Application.Application¬ID) 
                    command.Parameters.Add(new SqlParameter("@app", SqlDbType.Int) { Value = model.ApplicationID });
                    ///	идентификатор документа типа “Свидетельство ЕГЭ” (передается NULL)
                    command.Parameters.Add(new SqlParameter("@doc", SqlDbType.Int) { Value = model.doc });
                    ///	Регистрационный номер документа (передается NULL). 
                    command.Parameters.Add(new SqlParameter("@regNum", SqlDbType.NVarChar, 100) { Value = model.regNum });
                    ///	признак обновления таблицы ApplicationEntranceTestDocument (передается 1).
                    command.Parameters.Add(new SqlParameter("@refr", SqlDbType.Int) { Value = model.refr });
                    ///	признак проверки на наличие результата ЕГЭ текущего года (передается 0).
                    command.Parameters.Add(new SqlParameter("@currentYear", SqlDbType.Int) { Value = model.currentYear });

                    command.Parameters.Add(new SqlParameter("@returnValue", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.ReturnValue
                    });
                    command.Parameters.Add(new SqlParameter("@errorMessage", SqlDbType.NVarChar, 4000)
                    {
                        Direction = ParameterDirection.Output
                    });
                    command.Parameters.Add(new SqlParameter("@violationMessage", SqlDbType.NVarChar, 4000)
                    {
                        Direction = ParameterDirection.Output
                    });
                    command.Parameters.Add(new SqlParameter("@violationId", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    });

                    con.Open();
                    command.ExecuteNonQuery();

                    result = new SPResult
                    {
                        returnValue = Convert.ToBoolean(command.Parameters["@returnValue"].Value as int?),
                        errorMessage = command.Parameters["@errorMessage"].Value as string,
                        violationMessage = command.Parameters["@violationMessage"].Value as string,
                        violationId = Convert.ToInt32(command.Parameters["@violationId"].Value as int?)
                    };
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
            }
            return result;
        }

        #endregion

        #region CheckApplicationStatus

        public static int GetApplicationStatusId(int applicationId)
        {
            int result = 0;
            string sql = @"SELECT a.StatusID FROM [Application] (NOLOCK) AS a WHERE a.ApplicationID=@ApplicationID";

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                try
                {
                    SqlCommand com = new SqlCommand(sql, con);
                    com.Parameters.Add(new SqlParameter("ApplicationID", applicationId));

                    con.Open();
                    SqlDataReader sdr = com.ExecuteReader();
                    if (sdr.Read())
                    {
                        result = Convert.ToInt32(sdr["StatusID"] as Int32?);
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


        #endregion

        public static int GetResultValueSubject(int ApplicationId, int etiID)
        {
            string sql = @"SELECT * FROM ApplicationEntranceTestDocument (NOLOCK) AS aetd WHERE aetd.ApplicationID=@ApplicationID AND aetd.EntranceTestItemID=@etiID";

            int rvs = 0;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("ApplicationID", ApplicationId));
                com.Parameters.Add(new SqlParameter("etiID", etiID));
                try
                {
                    con.Open();
                    DataSet ds = new DataSet();
                    SqlDataAdapter sqlAdapter = new SqlDataAdapter(com);
                    sqlAdapter.Fill(ds);
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        rvs = 1;
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
            return rvs;
        }

        #region ViolationErrors

        public static string GetViolationErrors(int ApplicationID)
        {
            #region SQL
            string sql = @"SELECT app.ViolationErrors AS ViolationErrors FROM Application app (NOLOCK) WHERE app.ApplicationID = @ApplicationID";
            #endregion
            string result = "";
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("ApplicationID", ApplicationID));
                try
                {
                    con.Open();
                    SqlDataReader r = com.ExecuteReader();

                    if (r.Read())
                    {
                        result = r["ViolationErrors"] as string;
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
            return result;
        }
        #endregion

        public static List<string> GetUidListWz5(int institutionId)
        {
            #region SQL

            string sql = @"SELECT a.UID, a.ApplicationID FROM [Application] AS a (NOLOCK) WHERE a.InstitutionID=@InstitutionID AND a.UID IS NOT NULL";
            #endregion

            List<string> UidList = new List<string>();
            DataSet ds = null;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);
                com.Parameters.Add(new SqlParameter("InstitutionID", institutionId));
                ds = new DataSet();
                try
                {
                    con.Open();
                    SqlDataAdapter sqlAdapter = new SqlDataAdapter(com);
                    sqlAdapter.Fill(ds);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        UidList.Add(ds.Tables[0].Rows[i]["UID"] as string);
                        UidList.Add(Convert.ToString(ds.Tables[0].Rows[i]["ApplicationID"] as Int32?));
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
            return UidList;
        }


        //------------------------------------------------------------------------------------------------------------
        // movaxcs FindEntrantOlympic вместо старой FindEntrantOlympicMarks
        public static SPResult GetCheckOlympicResults(AppResultsModel model)
        {
            SPResult result = null;

            if (model.Step >= 5)
            {
                string sql = @"
                select ed.EntrantDocumentID as docId, edo.OlympicID as olympicId, edo.OlympicTypeProfileID as olympicTypeProfileId 
                from ApplicationEntranceTestDocument aetd (NOLOCK) 
                inner join EntrantDocument ed (NOLOCK) on ed.EntrantDocumentID = aetd.EntrantDocumentID 
                inner join EntrantDocumentOlympic edo (NOLOCK) on edo.EntrantDocumentID = ed.EntrantDocumentID 
                where aetd.ApplicationID = @ApplicationID and (ed.DocumentTypeID = 9 or ed.DocumentTypeID = 10) and 
                aetd.BenefitID is not null and aetd.EntranceTestItemID is not null";

                DataSet ds = new DataSet();
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    SqlCommand com = new SqlCommand(sql, con);
                    com.Parameters.Add(new SqlParameter("ApplicationID", model.ApplicationID));


                    con.Open();
                    SqlDataAdapter sqlAdapter = new SqlDataAdapter(com);
                    sqlAdapter.Fill(ds);
                }
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    result = FindEntrantOlympic(ds.Tables[0].Rows[i]["docId"] as int?, ds.Tables[0].Rows[i]["olympicId"] as int?, ds.Tables[0].Rows[i]["olympicTypeProfileId"] as int?);
                    if (result.returnValue)
                        break;
                }
            }
            else
            {
                result = FindEntrantOlympic(model.DocId, model.OlympicID, model.OlympicTypeProfileID);
            }
            
            if (result == null)
            {
                result = new SPResult { returnValue = false };
            }

            return result;
        }

        //------------------------------------------------------------------------------------------------------------

        public static SPResult FindEntrantOlympic(int? docId, int? olympicID, int? olympicTypeProfileID)
        {
            var result = new SPResult { returnValue = false };

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand("FindEntrantOlympic", con);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@docId", SqlDbType.Int) { Value = docId });
                command.Parameters.Add(new SqlParameter("@olympicId", SqlDbType.Int) { Value = olympicID });
                command.Parameters.Add(new SqlParameter("@olympicTypeProfileId", SqlDbType.Int) { Value = olympicTypeProfileID });

                command.Parameters.Add(new SqlParameter("@returnValue", SqlDbType.Int) { Direction = ParameterDirection.ReturnValue });
                command.Parameters.Add(new SqlParameter("@errorMessage", SqlDbType.NVarChar, 4000) { Direction = ParameterDirection.Output });
                command.Parameters.Add(new SqlParameter("@violationMessage", SqlDbType.NVarChar, 4000) { Direction = ParameterDirection.Output });
                command.Parameters.Add(new SqlParameter("@violationId", SqlDbType.Int) { Direction = ParameterDirection.Output });

                con.Open();
                command.ExecuteNonQuery();

                result.returnValue = Convert.ToBoolean(command.Parameters["@returnValue"].Value as int?);
                result.errorMessage = command.Parameters["@errorMessage"].Value as string;
                result.violationMessage = command.Parameters["@violationMessage"].Value as string;
                result.violationId = Convert.ToInt32(command.Parameters["@violationId"].Value as int?);
            }

            return result;
        }
    }
}
