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

namespace GVUZ.Web.ContextExtensionsSQL
{

    public static partial class ApplicationSQL
    {

        #region Wz0

        public static ApplicationWz0ViewModel GetApplicationWz0(int ApplicationId)
        {
            string sql = @"SELECT app.ApplicationID, app.StatusID, 
								COALESCE(app.ApplicationNumber,'не задан') as ApplicationNumber, 
								app.RegistrationDate,
								app.WizardStepID
							FROM  [dbo].[Application] app (NOLOCK)  
								INNER JOIN Entrant ent (NOLOCK) on app.EntrantID=ent.EntrantID  
								INNER JOIN EntrantDocumentIdentity ed (NOLOCK) on ent.IdentityDocumentID=ed.EntrantDocumentID  
								INNER JOIN IdentityDocumentType dt (NOLOCK)  on dt.IdentityDocumentTypeID=ed.IdentityDocumentTypeID  
							WHERE app.ApplicationID=@ApplicationID";

            ApplicationWz0ViewModel appwz0 = null;
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
                        appwz0 = new ApplicationWz0ViewModel();
                        appwz0.ApplicationID = r["ApplicationID"] as Int32?;
                        appwz0.ApplicationNumber = r["ApplicationNumber"] as string;
                        appwz0.RegistrationDate = (DateTime)r["RegistrationDate"];
                        //appwz0.RegistrationDateTime=(DateTime)r["RegistrationDate"];
                        //appwz0.WizardStepID			=(int)r["WizardStepID"];
                        appwz0.StatusID = r["StatusID"] as Int32?;
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
            return appwz0;
        }

        public static int NewApplicationWz0(ApplicationWz0Model app)
        {
            int id = 0;

            //string compGroupIDs = String.Join(",", app.SelectedCompetitiveGroupIDs);

            #region SQL
            string sql = @"
DECLARE @EntrantID	int
DECLARE @EntrantIsNew	bit
DECLARE @EntrantDocumentID	int
DECLARE @ApplicationID int 
DECLARE @DocumentTypeID int 

SET @DocumentTypeID=1;
SET @EntrantIsNew=0;

SET NOCOUNT ON;
--BEGIN TRANSACTION Tran1;

IF NOT EXISTS (
				SELECT TOP 1 a.ApplicationId 
                FROM [Application] a (NOLOCK) 
                INNER JOIN ENTRANT e (NOLOCK) ON a.EntrantID = e.EntrantID
                WHERE a.InstitutionID=@InstitutionID AND a.ApplicationNumber=@ApplicationNumber
                AND e.IdentityDocumentID IS NOT NULL
			)
BEGIN 
-- Если номеров таких же не найдено, то ....
SELECT @EntrantID=ed.EntrantID, @EntrantDocumentID=ed.EntrantDocumentID
FROM EntrantDocument ed (NOLOCK) 
INNER JOIN EntrantDocumentIdentity edi (NOLOCK) on ed.EntrantDocumentID=edi.EntrantDocumentID
INNER JOIN Entrant AS e (NOLOCK) ON e.EntrantID = ed.EntrantID
WHERE ed.DocumentSeries=@DocumentSeries 
AND ed.DocumentNumber=@DocumentNumber 
AND edi.IdentityDocumentTypeID=@IdentityDocumentTypeID
AND e.InstitutionID=@InstitutionID

IF @EntrantID IS NULL BEGIN
	
	INSERT INTO [dbo].[Entrant] with (ROWLOCK) ([LastName],[FirstName],[GenderID],[InstitutionID]) 
	VALUES('', '', 1,@InstitutionID);
	
	SELECT @EntrantID=cast(scope_identity() as int);
	SET @EntrantIsNew=1;
	
	INSERT INTO [dbo].[EntrantDocument] with (ROWLOCK) (EntrantID, DocumentTypeID, DocumentSpecificData,DocumentSeries,DocumentNumber) 
	VALUES (@EntrantID,@DocumentTypeID,'',@DocumentSeries, @DocumentNumber);

	SELECT @EntrantDocumentID=cast(scope_identity() as int);
	
	UPDATE [dbo].[Entrant] with (ROWLOCK)
	SET IdentityDocumentID=@EntrantDocumentID WHERE EntrantID=@EntrantID;

	INSERT INTO [EntrantDocumentIdentity] with (ROWLOCK) ([EntrantDocumentID],[IdentityDocumentTypeID])
    VALUES (@EntrantDocumentID,	@IdentityDocumentTypeID);
END  

INSERT INTO [dbo].[Application] with (ROWLOCK)
			( 
			[EntrantID], [ApplicationNumber], [RegistrationDate]
			,[InstitutionID], [StatusID],[WizardStepID],[ViolationID],[SourceID]
		    ,[CreatedDate]
            ,[OriginalDocumentsReceived]           
            ,[IsRequiresBudgetO],[IsRequiresBudgetOZ],[IsRequiresBudgetZ]
			,[IsRequiresPaidO],[IsRequiresPaidOZ],[IsRequiresPaidZ]
			,[IsRequiresTargetO],[IsRequiresTargetOZ],[IsRequiresTargetZ]
			,ApproveInstitutionCount
			,FirstHigherEducation
			,ApprovePersonalData
			,FamiliarWithLicenseAndRules
			,FamiliarWithAdmissionType
			,FamiliarWithOriginalDocumentDeliveryDate
			,OrderCalculatedRating

)
VALUES(@EntrantID, @ApplicationNumber, CONVERT(date, @RegistrationDate),
      @InstitutionID,  @StatusID,  @WizardStepID, @ViolationID, @SourceID,
  		@CreatedDate,
      @OriginalDocumentsReceived
		,0,0,0  ,0,0,0  ,0,0,0,  1,1,1,1,1,1, 0 
);
SELECT @ApplicationID=cast(scope_identity() as int);

INSERT INTO ApplicationEntrantDocument with (ROWLOCK) (ApplicationID, EntrantDocumentID) 
VALUES (@ApplicationID, @EntrantDocumentID);

END 
ELSE BEGIN
	-- Если номер заявления уже есть то 
	SELECT @ApplicationID=-1, @EntrantID=-1, @EntrantDocumentID=-1;
END
--COMMIT TRANSACTION Tran1;
SET NOCOUNT OFF;

SELECT @ApplicationID as ApplicationID, @EntrantID as EntrantID, @EntrantDocumentID as EntrantDocumentID, @EntrantIsNew as EntrantIsNew;
";
  
            #endregion

            //string sqlPriorities = " INSERT INTO ApplicationCompetitiveGroupItem  " + 
            //    "(ApplicationId,CompetitiveGroupId,CompetitiveGroupItemId,CompetitiveGroupTargetId," + 
            //    "EducationFormId, EducationSourceId, [Priority]) " + 
            //    "VALUES  (@ApplicationID,@CompetitiveGroupId,@CompetitiveGroupItemId,@CompetitiveGroupTargetId," + 
            //    "@EducationFormId, @EducationSourceId, @Priority)";

            string sqlPriorities = " INSERT INTO ApplicationCompetitiveGroupItem with (rowlock) " +
                "(ApplicationId,CompetitiveGroupId,EducationFormId,EducationSourceId,IsForSPOandVO,CompetitiveGroupTargetId) " +
                "VALUES  " +
                "(@ApplicationID,@CompetitiveGroupId,@EducationFormId,@EducationSourceId,@IsForSPOandVO,@CompetitiveGroupTargetId)";

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);
                // TODO: вынести это всё нормально вот если кароче
                com.CommandTimeout = 600;
                #region Парамтры
                com.Parameters.Add(new SqlParameter("DocumentSeries", SqlDbType.VarChar) { Value = (app.DocumentSeries ?? (object)DBNull.Value) });
                com.Parameters.Add(new SqlParameter("DocumentNumber", app.DocumentNumber) { Value = (app.DocumentNumber ?? (object)DBNull.Value) });

                com.Parameters.Add(new SqlParameter("IdentityDocumentTypeID", SqlDbType.Int) { Value = app.IdentityDocumentTypeID });

                com.Parameters.Add(new SqlParameter("CampaignID", SqlDbType.Int) { Value = app.CampaignID });

                com.Parameters.Add(new SqlParameter("ApplicationNumber", SqlDbType.NVarChar, 50) { Value = app.ApplicationNumber });
                com.Parameters.Add(new SqlParameter("RegistrationDate", SqlDbType.DateTime) { Value = app.RegistrationDate });
                com.Parameters.Add(new SqlParameter("InstitutionID", SqlDbType.Int) { Value = app.InstitutionID });
                com.Parameters.Add(new SqlParameter("StatusID", SqlDbType.Int) { Value = 1 }); // ApplicationStatusType.Draft
                com.Parameters.Add(new SqlParameter("WizardStepID", SqlDbType.Int) { Value = 1 });	// Переводим в режим 1 или 2?
                com.Parameters.Add(new SqlParameter("ViolationID", SqlDbType.Int) { Value = 0 });
                com.Parameters.Add(new SqlParameter("SourceID", SqlDbType.Int) { Value = 2 });
                com.Parameters.Add(new SqlParameter("CreatedDate", SqlDbType.DateTime) { Value = DateTime.Now });
                com.Parameters.Add(new SqlParameter("BirthDate", SqlDbType.DateTime) { Value = DateTime.Now });
                com.Parameters.Add(new SqlParameter("OriginalDocumentsReceived", SqlDbType.Bit) { Value = 0 });
                #endregion

                SqlCommand comPriorities = new SqlCommand(sqlPriorities, con);
                comPriorities.Parameters.Add(new SqlParameter("ApplicationId", SqlDbType.Int));
                comPriorities.Parameters.Add(new SqlParameter("CompetitiveGroupId", SqlDbType.Int));
                comPriorities.Parameters.Add(new SqlParameter("EducationFormId", SqlDbType.Int));
                comPriorities.Parameters.Add(new SqlParameter("EducationSourceId", SqlDbType.Int));
                comPriorities.Parameters.Add(new SqlParameter("IsForSPOandVO", SqlDbType.Bit));
                comPriorities.Parameters.Add(new SqlParameter("CompetitiveGroupTargetId", SqlDbType.Int));

                //comPriorities.Parameters.Add(new SqlParameter("Priority", SqlDbType.Int));
                //comPriorities.Parameters.Add(new SqlParameter("CompetitiveGroupItemId", SqlDbType.Int));

                try
                {
                    con.Open();
                    object o = null;// Вставить и вернуть новый ID
                    SqlDataReader r = com.ExecuteReader();
                    if (r.Read())
                    {
                        o = r["ApplicationID"];
                        if (o != null && o != DBNull.Value) { app.ApplicationID = Convert.ToInt32(o); id = app.ApplicationID.Value; }
                        o = r["EntrantID"];
                        if (o != null && o != DBNull.Value) { app.EntrantId = Convert.ToInt32(o); }
                        o = r["EntrantDocumentID"];
                        if (o != null && o != DBNull.Value) { app.EntrantDocumentID = Convert.ToInt32(o); }
                        o = (bool)r["EntrantIsNew"];
                        if (o != null && o != DBNull.Value) { app.EntrantIsNew = Convert.ToBoolean(o); }
                    }
                    r.Close();
                    if (app.ApplicationID > 0)
                    {
                        comPriorities.Parameters["ApplicationId"].Value = app.ApplicationID;
                        foreach (var p in app.Priorities.ApplicationPriorities)
                        {
                            comPriorities.Parameters["CompetitiveGroupId"].Value = p.CompetitiveGroupId;
                            //comPriorities.Parameters["CompetitiveGroupItemId"].Value = p.CompetitiveGroupItemId;
                            //comPriorities.Parameters["CompetitiveGroupItemId"].Value = 54; // нужен CompetitiveGroupItemId
                            comPriorities.Parameters["EducationFormId"].Value = p.EducationFormId;
                            comPriorities.Parameters["EducationSourceId"].Value = p.EducationSourceId;
                            comPriorities.Parameters["IsForSPOandVO"].Value = p.IsForSPOandVO;

                            if (p.CompetitiveGroupTargetId.HasValue)
                                comPriorities.Parameters["CompetitiveGroupTargetId"].Value = p.CompetitiveGroupTargetId;
                            else
                                comPriorities.Parameters["CompetitiveGroupTargetId"].Value = DBNull.Value;


                            //if (p.Priority == null)
                            //{
                            //    comPriorities.Parameters["CompetitiveGroupTargetId"].Value = DBNull.Value;
                            //}
                            //else
                            //{
                            //    if (p.CompetitiveGroupTargetId.HasValue)
                            //    {
                            //        comPriorities.Parameters["CompetitiveGroupTargetId"].Value =
                            //            p.CompetitiveGroupTargetId;
                            //    }
                            //    else
                            //    {
                            //        comPriorities.Parameters["CompetitiveGroupTargetId"].Value = DBNull.Value;
                            //    }
                            //}
                            //if (p.Priority == null)
                            //{
                            //    comPriorities.Parameters["Priority"].Value = DBNull.Value;
                            //}
                            //else
                            //{
                            //    comPriorities.Parameters["Priority"].Value = p.Priority.Value;
                            //}

                            comPriorities.ExecuteNonQuery();
                        }
                    }
                    id = app.ApplicationID.Value;
                    con.Close();
                }
                catch (SqlException)
                {
                    throw;	// Пробросить дальше
                }
                catch (Exception)
                {
                    throw; // Пробросить дальше
                }
                finally
                {
                    if (con.State != ConnectionState.Closed) { con.Close(); }
                }
            }
            //dbContext.AddApplicationAccessToLog(null,new PersonalDataAccessLogger.AppData(app),"CreateApplication",institutionID,app.ApplicationID);
            //EntrantCacheManager.Add(app.ApplicationID,app);
            return id;
        }

        public static int UpdApplicationWz0(ApplicationWz0Model app)
        {
            int c = 0;
            #region SQL
            string sql = @"UPDATE [dbo].[Application]
SET
	[ApplicationNumber]=@ApplicationNumber,
	[RegistrationDate]=CONVERT(date, @RegistrationDate),
	[WizardStepID]=@WizardStepID
WHERE ApplicationId=@ApplicationID";
            #endregion
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);
                /*
                    SELECT @EntrantID=9975651, @ApplicationNumber='2015-01', @RegistrationDate=GETDATE(), @InstitutionID=587, @StatusID=3, @WizardStepID=2, @ViolationID=2, @SourceID=2,
                    @CreatedDate=GETDATE(),
                    @OriginalDocumentsReceived=0, @IsRequiresBudgetO=0, @IsRequiresBudgetOZ=0, @IsRequiresBudgetZ=0, @IsRequiresPaidO=0, @IsRequiresPaidOZ=0, @IsRequiresPaidZ=0, @IsRequiresTargetO=0, @IsRequiresTargetOZ=0, @IsRequiresTargetZ=0
                */
                #region Параметры
                com.Parameters.Add(new SqlParameter("ApplicationID", SqlDbType.Int) { Value = app.ApplicationID });
                com.Parameters.Add(new SqlParameter("ApplicationNumber", app.ApplicationNumber));
                com.Parameters.Add(new SqlParameter("RegistrationDate", SqlDbType.DateTime) { Value = DateTime.Now });
                com.Parameters.Add(new SqlParameter("WizardStepID", SqlDbType.Int) { Value = 0 });
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

        #endregion

    }
}