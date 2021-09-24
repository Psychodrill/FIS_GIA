using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using GVUZ.Web.ViewModels;
using System.Configuration;
using GVUZ.Helper;
using System.Collections;
using System.Text;

namespace GVUZ.Web.ContextExtensionsSQL
{
    public static partial class ApplicationSQL
    {
        #region Wz5

        public static ApplicationWz5ViewModel.Wz5SendingViewModel GetApplicationWz5(int ApplicationID, int InstitutionID)
        {
            #region SQL

            string sql = @"
-- DECLARE @ApplicationID int = 160;
--------------------
SELECT app.ApplicationID
	,app.RegistrationDate AS RegistrationDate
	,app.[Priority] AS [Priority]
	,app.[UID] AS [UID]
	,app.NeedHostel AS NeedHostel
	,ISNULL(ent.LastName,'') + ' ' + ISNULL(ent.FirstName,'') + ' ' + ISNULL(ent.MiddleName,'')  AS FIO
	,edi.BirthDate AS DOB
	,dt.Name + ': ' + ed.DocumentSeries + ' ' + ed.DocumentNumber + ' ' + CONVERT(varchar(10), ed.DocumentDate, 104) AS IdentityDocument
	,ent.GenderID AS GenderID
    ,(SELECT gt.Name FROM [dbo].[GenderType] gt WHERE gt.GenderID = ent.GenderID) AS Gender
	,(SELECT ct.Name FROM [dbo].[CountryType] ct WHERE ct.CountryID = edi.NationalityTypeID) AS Citizen
	,edi.BirthPlace AS POB
	,ent.CustomInformation AS CustomInformation
	,app.StatusID
	,ent.Email
    ,ent.Address
    ,(SELECT rt.Name FROM [dbo].[RegionType] rt WHERE rt.RegionID = ent.RegionID) AS Region
    ,(SELECT tt.Name FROM [dbo].[TownType] tt WHERE tt.TownTypeID = ent.TownTypeID) AS TownType
    ,app.ApplicationNumber
	
FROM  [dbo].[Application] app   (NOLOCK)
	INNER JOIN Entrant ent (NOLOCK)
	ON ent.EntrantID =app.EntrantID
	INNER JOIN EntrantDocument ed (NOLOCK)
	ON ed.EntrantDocumentID= ent.IdentityDocumentID 
	INNER JOIN EntrantDocumentIdentity edi (NOLOCK)
	ON edi.EntrantDocumentID=ed.EntrantDocumentID
	INNER JOIN IdentityDocumentType dt (NOLOCK)
	ON dt.IdentityDocumentTypeID=edi.IdentityDocumentTypeID
WHERE app.ApplicationID=@ApplicationID
--------------------
SELECT DISTINCT 
	i.InstitutionID
	,i.FullName AS Institution
	,ast.Name AS [Status]

FROM [dbo].[Application] app (NOLOCK)
	INNER JOIN [dbo].[Institution] i (NOLOCK)
	ON app.InstitutionID = i.InstitutionID
	INNER JOIN [dbo].[ApplicationStatusType] ast (NOLOCK)
	ON app.StatusID = ast.StatusID
WHERE app.ApplicationID = @ApplicationID
-------------------
SELECT DISTINCT c.Name AS CampaignName, c.CampaignID AS CampaignID, c.CampaignTypeID AS CampaignTypeID
FROM
    Campaign AS c (NOLOCK)
    INNER JOIN CompetitiveGroup AS CG (NOLOCK) ON c.CampaignID = CG.CampaignID
    INNER JOIN ApplicationCompetitiveGroupItem AS acgi (NOLOCK) ON CG.CompetitiveGroupID = acgi.CompetitiveGroupId
WHERE     (acgi.ApplicationId = @ApplicationID)
-------------------
select count(ID) as cnt from ApplicationEntrantDocument  (NOLOCK)
where ApplicationId = @ApplicationID 
and OriginalReceivedDate is not null


            ";
            #endregion

            //            SELECT

            //        acgi.CompetitiveGroupId
            //		,CG.Name AS CompetitiveGroupName
            //        ,d.DirectionID
            //		,cg.EducationLevelID
            //		,isnull(acgi.CompetitiveGroupItemID, 0) as CompetitiveGroupItemId


            //FROM[dbo].[ApplicationCompetitiveGroupItem] acgi
            //   INNER JOIN[dbo].[CompetitiveGroup] cg ON cg.CompetitiveGroupID = acgi.CompetitiveGroupId
            //    INNER JOIN[dbo].[Direction]
            //        d ON d.DirectionID = cg.DirectionID
            //INNER JOIN[dbo].[AdmissionItemType]
            //        Sour ON Sour.ItemTypeID = acgi.EducationSourceId
            //INNER JOIN[dbo].[AdmissionItemType]
            //        F ON F.ItemTypeID = acgi.EducationFormId
            //LEFT JOIN[dbo].[CompetitiveGroupTarget]
            //        cgt ON acgi.CompetitiveGroupTargetId = cgt.CompetitiveGroupTargetID
            //WHERE ApplicationID=@ApplicationID AND acgi.Priority IS NOT NULL
            //GROUP BY acgi.CompetitiveGroupId, CG.Name,d.DirectionID,cg.EducationLevelID , acgi.CompetitiveGroupItemID

            //            SELECT
            //        acgi.id
            //		,acgi.ApplicationId
            //		,acgi.CompetitiveGroupId
            //		,CG.Name AS CompetitiveGroupName
            //		,acgi.CompetitiveGroupItemId
            //		,D.Name AS CompetitiveGroupItemName
            //		,acgi.EducationFormId
            //		,F.Name AS EducationFormName
            //		,acgi.EducationSourceId
            //		,Sour.Name AS EducationSourceName
            //        ,acgi.CompetitiveGroupTargetId
            //		,cgt.Name AS CompetitiveGroupTargetName
            //		,acgi.[Priority]
            //		,d.DirectionID
            //        ,cg.EducationLevelID
            //        ,Level.Name as LevelName
            //        ,acgi.IsAgreed
            //        ,acgi.IsDisagreed
            //        ,acgi.IsAgreedDate
            //        ,acgi.IsForSPOandVO
            //        ,acgi.IsDisagreedDate
            //        ,acgi.CalculatedRating

            //FROM[dbo].[ApplicationCompetitiveGroupItem]
            //        acgi
            //   INNER JOIN[dbo].[CompetitiveGroup]
            //        cg ON cg.CompetitiveGroupID = acgi.CompetitiveGroupId
            //INNER JOIN[dbo].[Direction]
            //        d ON d.DirectionID = cg.DirectionID
            //INNER JOIN[dbo].[AdmissionItemType]
            //        Sour ON Sour.ItemTypeID = acgi.EducationSourceId
            //INNER JOIN[dbo].[AdmissionItemType]
            //        F ON F.ItemTypeID = acgi.EducationFormId
            //LEFT JOIN[dbo].[AdmissionItemType]
            //        Level ON Level.ItemTypeID = cg.EducationLevelID
            //LEFT JOIN[dbo].[CompetitiveGroupTarget]
            //        cgt ON acgi.CompetitiveGroupTargetId = cgt.CompetitiveGroupTargetID
            //WHERE ApplicationID=@ApplicationID
            //ORDER BY acgi.id


            //SELECT cgti.CompetitiveGroupTargetID, cgt.Name AS CompetitiveGroupTargetName, cgti.CompetitiveGroupID, cgti.CompetitiveGroupTargetItemID
            //FROM CompetitiveGroupTarget AS cgt
            //INNER JOIN CompetitiveGroupTargetItem AS cgti ON cgti.CompetitiveGroupTargetID = cgt.CompetitiveGroupTargetID

            //INNER JOIN CompetitiveGroup AS cg ON cg.CompetitiveGroupID = cgti.CompetitiveGroupID
            //WHERE cg.CompetitiveGroupID IN
            //(

            //SELECT acgi.CompetitiveGroupId
            //FROM ApplicationCompetitiveGroupItem AS acgi
            //WHERE acgi.ApplicationId= @ApplicationID
            //)


            ApplicationWz5ViewModel.Wz5SendingViewModel appw5 = new ApplicationWz5ViewModel.Wz5SendingViewModel();
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
                    appw5 = new ApplicationWz5ViewModel.Wz5SendingViewModel();
                    appw5.ApplicationID = ApplicationID;

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        appw5.FIO = ds.Tables[0].Rows[i]["FIO"] as string;
                        appw5.DOB = (ds.Tables[0].Rows[i]["DOB"] as DateTime?).Value.ToString("dd.MM.yyyy");
                        appw5.IdentityDocument = ds.Tables[0].Rows[i]["IdentityDocument"] as string;
                        appw5.GenderID = Convert.ToInt32(ds.Tables[0].Rows[i]["GenderID"]);
                        appw5.Gender = ds.Tables[0].Rows[i]["Gender"] as string;
                        appw5.Citizen = ds.Tables[0].Rows[i]["Citizen"] as string;
                        appw5.POB = ds.Tables[0].Rows[i]["POB"] as string;
                        appw5.CustomInformation = ds.Tables[0].Rows[i]["CustomInformation"] as string;
                        appw5.RegistrationDate = ds.Tables[0].Rows[i]["RegistrationDate"] as DateTime?;
                        appw5.Priority = ds.Tables[0].Rows[i]["Priority"] as int?;
                        appw5.Uid = ds.Tables[0].Rows[i]["UID"] as string;
                        appw5.NeedHostel = Convert.ToBoolean(ds.Tables[0].Rows[i]["NeedHostel"] as bool?);
                        appw5.StatusID = Convert.ToInt32(ds.Tables[0].Rows[i]["StatusID"]);
                        appw5.Email = ds.Tables[0].Rows[i]["Email"] as string;
                        appw5.Address = ds.Tables[0].Rows[i]["Address"] as string;
                        appw5.Region = ds.Tables[0].Rows[i]["Region"] as string;
                        appw5.TownType = ds.Tables[0].Rows[i]["TownType"] as string;
                        appw5.ApplicationNumber = ds.Tables[0].Rows[i]["ApplicationNumber"] as string;
                    }

                    appw5.Priorities = new ApplicationPrioritiesViewModel();
                    appw5.Priorities.ApplicationPriorities = SQL.GetApplicationPriorities(ApplicationID, InstitutionID);

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        appw5.InstitutionID = Convert.ToInt32(ds.Tables[1].Rows[0]["InstitutionID"]);
                        appw5.Institution = ds.Tables[1].Rows[0]["Institution"] as string;
                    }

                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        appw5.CampaignID = Convert.ToInt32(ds.Tables[2].Rows[0]["CampaignID"]);
                        appw5.CampaignTypeID = Convert.ToInt32(ds.Tables[2].Rows[0]["CampaignTypeID"]);
                    }

                    appw5.OriginalsProvided = Convert.ToInt32(ds.Tables[3].Rows[0]["cnt"]) > 0;

                    appw5.ForcedAdmissionReasons.AddRange(SQL.GetForcedAdmissionReasons());

                    //appw5.ListAppPrioritiesG = new List<ApplicationPriorityViewModel>();

                    //for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                    //{
                    //    appw5.ListAppPrioritiesG.Add(new ApplicationPriorityViewModel
                    //    {
                    //        CompetitiveGroupId = Convert.ToInt32(ds.Tables[3].Rows[i]["CompetitiveGroupId"]),
                    //        CompetitiveGroupName = ds.Tables[3].Rows[i]["CompetitiveGroupName"] as string,
                    //        DirectionID = Convert.ToInt32(ds.Tables[3].Rows[i]["DirectionID"] as int?),
                    //        CompetitiveGroupItemId = Convert.ToInt32(ds.Tables[3].Rows[i]["CompetitiveGroupItemId"]),
                    //        EducationLevelID = Convert.ToInt16(ds.Tables[3].Rows[i]["EducationLevelID"] as short?)
                    //    });
                    //}

                    //appw5.ListAppPrioritiesGI = new List<ApplicationPriorityViewModel>();
                    //for (int i = 0; i < ds.Tables[4].Rows.Count; i++)
                    //{
                    //    appw5.ListAppPrioritiesGI.Add(new ApplicationPriorityViewModel()
                    //    {
                    //        Id = Convert.ToInt32(ds.Tables[4].Rows[i]["Id"]),
                    //        ApplicationId = Convert.ToInt32(ds.Tables[4].Rows[i]["ApplicationId"]),
                    //        CompetitiveGroupId = Convert.ToInt32(ds.Tables[4].Rows[i]["CompetitiveGroupId"]),
                    //        CompetitiveGroupName = ds.Tables[4].Rows[i]["CompetitiveGroupName"] as string,
                    //        CompetitiveGroupItemId = 0, // Convert.ToInt32(ds.Tables[4].Rows[i]["CompetitiveGroupItemId"]),
                    //        CompetitiveGroupItemName = ds.Tables[4].Rows[i]["CompetitiveGroupItemName"] as string,
                    //        EducationFormId = Convert.ToInt32(ds.Tables[4].Rows[i]["EducationFormId"]),
                    //        EducationFormName = ds.Tables[4].Rows[i]["EducationFormName"] as string,
                    //        EducationSourceId = Convert.ToInt32(ds.Tables[4].Rows[i]["EducationSourceId"]),
                    //        EducationSourceName = ds.Tables[4].Rows[i]["EducationSourceName"] as string,
                    //        CompetitiveGroupTargetId = Convert.ToInt32(ds.Tables[4].Rows[i]["CompetitiveGroupTargetId"] as int?),
                    //        TargetOrganizationName = ds.Tables[4].Rows[i]["CompetitiveGroupTargetName"] as string,
                    //        TargetOrganizations = Convert.ToInt32(ds.Tables[4].Rows[i]["EducationSourceId"]) == 16 ? ConvertToTargetOrganization(ds.Tables[5]) : null,
                    //        Priority = ds.Tables[4].Rows[i]["Priority"] as int?,
                    //        DirectionID = Convert.ToInt32(ds.Tables[4].Rows[i]["DirectionID"] as int?),
                    //        EducationLevelID = Convert.ToInt16(ds.Tables[4].Rows[i]["EducationLevelID"] as short?),
                    //        LevelName = ds.Tables[4].Rows[i]["LevelName"] as string,
                    //        IsAgreed = ds.Tables[4].Rows[i]["IsAgreed"] as bool?,
                    //        IsDisagreed = ds.Tables[4].Rows[i]["IsDisagreed"] as bool?,
                    //        IsAgreedDate = ds.Tables[4].Rows[i]["IsAgreedDate"] as DateTime?,
                    //        IsDisagreedDate = ds.Tables[4].Rows[i]["IsDisagreedDate"] as DateTime?,
                    //        CalculatedRating = ds.Tables[4].Rows[i]["CalculatedRating"] as decimal?,
                    //        IsForSPOandVO = ds.Tables[4].Rows[i]["IsForSPOandVO"] as bool?,
                    //        //AllowEdit = !(ds.Tables[4].Rows[i]["HasOrderOfAdmissionRefuse"] as bool?).GetValueOrDefault(false),
                    //    });
                    //}

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
            return appw5;
        }

        private static IEnumerable<TargetOrganization> ConvertToTargetOrganization(DataTable dataTable)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                yield return new TargetOrganization
                {
                    CompetitiveGroupTargetID = Convert.ToInt32(row["CompetitiveGroupTargetID"]),
                    CompetitiveGroupTargetName = row["CompetitiveGroupTargetName"].ToString(),
                    CompetitiveGroupItemID = Convert.ToInt32(row["CompetitiveGroupID"]),
                    CompetitiveGroupTargetItemID = Convert.ToInt32(row["CompetitiveGroupTargetItemID"])
                };
            }
        }

        #endregion

        #region Delete ApplicationCompetitiveGroupItem - Wz5

        public static AjaxResultModel DeleteApplicationCompetitiveGroupItem(int ApplicationId, bool changeCg)
        {
            int c = 0;
            #region SQL

            string sql = "";

            // RomanNB: непонятно, зачем применяется changeCg = false, если всегда нужно удалить и заново добавить документы
            changeCg = true;
            //

            if (changeCg)
            {
                sql = @"
                    DELETE FROM [dbo].[ApplicationCompetitiveGroupItem] WHERE ApplicationId = @ApplicationId
                    DELETE FROM [dbo].[ApplicationEntranceTestDocument] WHERE ApplicationId = @ApplicationId";
            }
            else
            {
                sql = @"
                    DELETE FROM [dbo].[ApplicationCompetitiveGroupItem] WHERE ApplicationId = @ApplicationId";
            }
            #endregion

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);

                #region Параметры
                com.Parameters.Add(new SqlParameter("ApplicationId", ApplicationId));
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
                    throw e;// Пробросить дальше
                }
                catch (Exception e)
                {
                    throw e;// Пробросить дальше
                }
                finally
                {
                    if (con.State != ConnectionState.Closed) { con.Close(); }
                }
            }
            return new AjaxResultModel();
        }

        #endregion

        #region Delete CompetitiveGroup - Wz5

        public static AjaxResultModel DeleteCompetitiveGroup(int CompetitiveGroupId)
        {
            int c = 0;
            #region SQL

            string sql = @"DELETE FROM [dbo].[ApplicationCompetitiveGroupItem] WHERE CompetitiveGroupId = @CompetitiveGroupId";

            #endregion

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);

                #region Параметры
                com.Parameters.Add(new SqlParameter("CompetitiveGroupId", CompetitiveGroupId));
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
                    throw e;// Пробросить дальше
                }
                catch (Exception e)
                {
                    throw e;// Пробросить дальше
                }
                finally
                {
                    if (con.State != ConnectionState.Closed) { con.Close(); }
                }
            }
            return new AjaxResultModel();
        }

        #endregion

        #region Delete CompetitiveGroupItem - Wz5

        public static AjaxResultModel DeleteCompetitiveGroupItem(int CompetitiveGroupItemId)
        {
            int c = 0;
            #region SQL

            string sql = @"DELETE FROM [dbo].[ApplicationCompetitiveGroupItem] WHERE CompetitiveGroupItemId = @CompetitiveGroupItemId";

            #endregion

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);

                #region Параметры
                com.Parameters.Add(new SqlParameter("CompetitiveGroupItemId", CompetitiveGroupItemId));
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
                    throw e;// Пробросить дальше
                }
                catch (Exception e)
                {
                    throw e;// Пробросить дальше
                }
                finally
                {
                    if (con.State != ConnectionState.Closed) { con.Close(); }
                }
            }
            return new AjaxResultModel();
        }

        #endregion

        #region Insert ApplicationCompetitiveGroupItem - Wz5

        public static AjaxResultModel InsertApplicationCompetitiveGroupItem(ApplicationPriorityViewModel model)
        {

            string sql = " INSERT INTO ApplicationCompetitiveGroupItem  " +
                "(ApplicationId,CompetitiveGroupId,EducationFormId,EducationSourceId,IsForSPOandVO,CompetitiveGroupTargetId,IsAgreed,IsDisagreed,IsAgreedDate,IsDisagreedDate,CalculatedRating) " +
                "VALUES  " +
                "(@ApplicationID,@CompetitiveGroupId,@EducationFormId,@EducationSourceId,@IsForSPOandVO,@CompetitiveGroupTargetId,@IsAgreed,@IsDisagreed,@IsAgreedDate,@IsDisagreedDate,@CalculatedRating)";


            using (SqlConnection con = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {

                try
                {
                    cmd.Parameters.Add(new SqlParameter("ApplicationId", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("CompetitiveGroupId", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("EducationFormId", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("EducationSourceId", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("IsForSPOandVO", SqlDbType.Bit));
                    cmd.Parameters.Add(new SqlParameter("CompetitiveGroupTargetId", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("IsAgreed", SqlDbType.Bit));
                    cmd.Parameters.Add(new SqlParameter("IsDisagreed", SqlDbType.Bit));
                    cmd.Parameters.Add(new SqlParameter("IsAgreedDate", SqlDbType.DateTime));
                    cmd.Parameters.Add(new SqlParameter("IsDisagreedDate", SqlDbType.DateTime));
                    cmd.Parameters.Add(new SqlParameter("CalculatedRating", SqlDbType.Decimal));

                    cmd.Parameters["ApplicationId"].Value = model.ApplicationId;
                    cmd.Parameters["CompetitiveGroupId"].Value = model.CompetitiveGroupId;
                    cmd.Parameters["EducationFormId"].Value = model.EducationFormId;
                    cmd.Parameters["EducationSourceId"].Value = model.EducationSourceId;
                    cmd.Parameters["IsForSPOandVO"].Value = model.IsForSPOandVO;
                    cmd.Parameters["IsAgreed"].Value = model.IsAgreed;
                    cmd.Parameters["IsDisagreed"].Value = model.IsDisagreed;
                    cmd.Parameters["CalculatedRating"].Value = model.CalculatedRating;

                    if (model.CompetitiveGroupTargetId.HasValue)
                        cmd.Parameters["CompetitiveGroupTargetId"].Value = model.CompetitiveGroupTargetId;
                    else
                        cmd.Parameters["CompetitiveGroupTargetId"].Value = DBNull.Value;

                    if (model.IsAgreedDate.HasValue)
                        cmd.Parameters["IsAgreedDate"].Value = model.IsAgreedDate;
                    else
                        cmd.Parameters["IsAgreedDate"].Value = DBNull.Value;

                    if (model.IsDisagreedDate.HasValue)
                        cmd.Parameters["IsDisagreedDate"].Value = model.IsDisagreedDate;
                    else
                        cmd.Parameters["IsDisagreedDate"].Value = DBNull.Value;

                    if (model.CalculatedRating.HasValue)
                        cmd.Parameters["CalculatedRating"].Value = model.CalculatedRating;
                    else
                        cmd.Parameters["CalculatedRating"].Value = DBNull.Value;

                    con.Open();
                    cmd.ExecuteNonQuery();
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
            return new AjaxResultModel();
        }
        #endregion

        #region Update ApplicationCompetitiveGroupItems - Wz5

        /// <summary>
        /// Сохранение выбранных в заявлении конкурсов.
        /// </summary>
        /// <param name="applicationId">ID заявления</param>
        /// <param name="items">выбранные конкурсы</param>
        /// <param name="deleteAETD">Удалять ли результаты ВИ при смене конкурсов (нужно, когда идет изменение по кнопке, а не общее сохранение с шага визарда)</param>
        public static void UpdateApplicationCompetitiveGroupItems(int applicationId, IEnumerable<ApplicationPriorityViewModel> items, bool deleteAETD = false)
        {
            StringBuilder insertAcgis = new StringBuilder();
            if (items != null)
            {
                foreach (ApplicationPriorityViewModel acgiModel in items)
                {
                    insertAcgis.AppendFormat(@"
INSERT INTO @acgis
    (
        ApplicationId 
        ,CompetitiveGroupId 
        ,EducationFormId 
        ,EducationSourceId 
        ,IsForSPOandVO 
        ,CompetitiveGroupTargetId 
        ,IsAgreed 
        ,IsDisagreed 
        ,IsAgreedDate 
        ,IsDisagreedDate 
        ,CalculatedRating 
    )
VALUES
    (
        {0}
        ,{1}
        ,{2}
        ,{3}
        ,{4}
        ,{5}
        ,{6}
        ,{7}
        ,{8}
        ,{9}
        ,{10}
    )
",
     acgiModel.ApplicationId,
     acgiModel.CompetitiveGroupId,
     acgiModel.EducationFormId,
     acgiModel.EducationSourceId,
     acgiModel.IsForSPOandVO.HasValue ? (acgiModel.IsForSPOandVO.Value ? "1" : "0") : "NULL",
     acgiModel.CompetitiveGroupTargetId.HasValue ? acgiModel.CompetitiveGroupTargetId.Value.ToString() : "NULL",
     acgiModel.IsAgreed.HasValue ? (acgiModel.IsAgreed.Value ? "1" : "0") : "NULL",
     acgiModel.IsDisagreed.HasValue ? (acgiModel.IsDisagreed.Value ? "1" : "0") : "NULL",
     acgiModel.IsAgreedDate.HasValue ? String.Format("CONVERT(DATETIME,'{0}',104)", acgiModel.IsAgreedDate.Value.ToString("dd.MM.yyyy")) : "NULL",
     acgiModel.IsDisagreedDate.HasValue ? String.Format("CONVERT(DATETIME,'{0}',104)", acgiModel.IsDisagreedDate.Value.ToString("dd.MM.yyyy")) : "NULL",
     acgiModel.CalculatedRating.HasValue ? acgiModel.CalculatedRating.Value.ToString().Replace(",",".") : "NULL"
     );
                }
            }

            string sql = String.Format(@"
--declare @applicationId int = 4818577;
--declare @deleteAETD bit = 1; -- Удалять ли AETD

DECLARE @acgis TABLE 
    (
        ApplicationId int
        ,CompetitiveGroupId int
        ,EducationFormId int
        ,EducationSourceId int
        ,IsForSPOandVO bit
        ,CompetitiveGroupTargetId int
        ,IsAgreed bit
        ,IsDisagreed bit
        ,IsAgreedDate datetime
        ,IsDisagreedDate datetime
        ,CalculatedRating decimal(10,4)
    )

{0}


declare @acgiDeleteOrChange table (
	id int not null
	,cgid int null
	,isSpoBefore bit null
	,isSpoAfter bit null
);


--Удалим записи (есть в ApplicationCompetitiveGroupItem но нет в @acgis)
DELETE 
    acgiToDelete
output deleted.id, deleted.CompetitiveGroupId, null, null into @acgiDeleteOrChange
FROM 
    ApplicationCompetitiveGroupItem acgiToDelete
    LEFT JOIN @acgis acgis ON acgis.ApplicationId = acgiToDelete.ApplicationId
        AND acgis.CompetitiveGroupId = acgiToDelete.CompetitiveGroupId
WHERE
    acgis.ApplicationId IS NULL 
    AND acgiToDelete.ApplicationId = @applicationId;

--Обновим то, что есть среди @acgis
UPDATE
    acgiToUpdate
SET
    acgiToUpdate.EducationFormId = acgis.EducationFormId
    ,acgiToUpdate.EducationSourceId = acgis.EducationSourceId
    ,acgiToUpdate.IsForSPOandVO = acgis.IsForSPOandVO
    ,acgiToUpdate.CompetitiveGroupTargetId = acgis.CompetitiveGroupTargetId
    ,acgiToUpdate.IsAgreed = acgis.IsAgreed
    ,acgiToUpdate.IsDisagreed = acgis.IsDisagreed
    ,acgiToUpdate.IsAgreedDate = acgis.IsAgreedDate
    ,acgiToUpdate.IsDisagreedDate = acgis.IsDisagreedDate
    ,acgiToUpdate.CalculatedRating = acgis.CalculatedRating
output inserted.id, inserted.CompetitiveGroupId, deleted.IsForSPOandVO, inserted.IsForSPOandVO into @acgiDeleteOrChange
FROM
    ApplicationCompetitiveGroupItem acgiToUpdate
    INNER JOIN @acgis acgis ON acgis.ApplicationId = acgiToUpdate.ApplicationId
        AND acgis.CompetitiveGroupId = acgiToUpdate.CompetitiveGroupId

--Добавим новые записи (есть в @acgis, но нет в ApplicationCompetitiveGroupItem)
INSERT INTO ApplicationCompetitiveGroupItem  
    (
        ApplicationId
        ,CompetitiveGroupId
        ,EducationFormId
        ,EducationSourceId
        ,IsForSPOandVO
        ,CompetitiveGroupTargetId
        ,IsAgreed
        ,IsDisagreed
        ,IsAgreedDate
        ,IsDisagreedDate
        ,CalculatedRating
    ) 
SELECT
    acgis.ApplicationId
    ,acgis.CompetitiveGroupId
    ,acgis.EducationFormId
    ,acgis.EducationSourceId
    ,acgis.IsForSPOandVO
    ,acgis.CompetitiveGroupTargetId
    ,acgis.IsAgreed
    ,acgis.IsDisagreed
    ,acgis.IsAgreedDate
    ,acgis.IsDisagreedDate
    ,acgis.CalculatedRating
FROM
    @acgis acgis
    LEFT JOIN ApplicationCompetitiveGroupItem acgi ON acgis.ApplicationId = acgi.ApplicationId
        AND acgis.CompetitiveGroupId = acgi.CompetitiveGroupId  
WHERE
    acgi.id IS NULL;


Delete From ApplicationEntranceTestDocument 
Where ApplicationID = @applicationId 
and @deleteAETD = 1
and CompetitiveGroupID in (
	select distinct cgid
	from @acgiDeleteOrChange 
	Where isSpoBefore is null OR -- удалили конкурс
	(isSpoBefore is not null AND isSpoAfter is not null AND isSpoBefore != isSpoAfter) -- изменили галку 
);
"
                , insertAcgis.ToString()
                // Если не сохранение на выходе из шага визарда, а именно изменение списка конкурсов по кнопке - то надо очистить всю внесенную информацию по ВИ.
                //, acgiChange ? "DELETE FROM [dbo].[ApplicationEntranceTestDocument] WHERE ApplicationId = @ApplicationId;" : ""
            );

            SqlConnection db = new SqlConnection(ConnectionString);
            SqlTransaction transaction;

            db.Open();
            transaction = db.BeginTransaction();
            try
            {
                SqlCommand cmd = new SqlCommand(sql, db, transaction);
                // TODO: таймауты нужно хранить где-то в конфигурации
                cmd.CommandTimeout = 600;
                cmd.Parameters.AddWithValue("applicationId", applicationId);
                cmd.Parameters.AddWithValue("deleteAETD", deleteAETD);
                cmd.ExecuteNonQuery();
                transaction.Commit();
            }
            catch (SqlException sqlError)
            {
                Console.WriteLine("Commit Exception Type: {0}", sqlError.GetType());
                Console.WriteLine("  Message: {0}", sqlError.Message);
                transaction.Rollback();
                throw;
            }
            finally
            {
                if (db != null && db.State != ConnectionState.Closed)
                {
                    db.Close();
                    db = null;
                }
            }
        }

        #endregion
        #region Update CompetitiveGroup - Wz5

        public static AjaxResultModel UpdateCompetitiveGroup(ApplicationPriorityViewModel model)
        {
            #region SQL

            string sql = @"

UPDATE [dbo].[ApplicationCompetitiveGroupItem]
   SET [ApplicationId] = @ApplicationId
      ,[CompetitiveGroupId] = @CompetitiveGroupId
      ,[CompetitiveGroupItemId] = @CompetitiveGroupItemId
      ,[EducationFormId] = @EducationFormId
      ,[EducationSourceId] = @EducationSourceId
      ,[Priority] = @Priority
      ,[CompetitiveGroupTargetId] = @CompetitiveGroupTargetId
 WHERE [id] = @id

";
            #endregion

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, con))

                    try
                    {
                        cmd.Parameters.Add("@ApplicationID", SqlDbType.Int).Value = model.ApplicationId;
                        cmd.Parameters.Add("@CompetitiveGroupId", SqlDbType.Int).Value = model.CompetitiveGroupId;
                        cmd.Parameters.Add("@CompetitiveGroupItemId", SqlDbType.Int).Value = model.CompetitiveGroupItemId;
                        cmd.Parameters.Add("@EducationFormId", SqlDbType.Int).Value = model.EducationFormId;
                        cmd.Parameters.Add("@EducationSourceId", SqlDbType.Int).Value = model.EducationSourceId;
                        cmd.Parameters.Add("@Priority", SqlDbType.Int).Value = model.Priority;
                        cmd.Parameters.Add("@CompetitiveGroupTargetId", SqlDbType.Int).Value = model.CompetitiveGroupTargetId;
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = model.Id;

                        con.Open();
                        cmd.ExecuteNonQuery();
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
            return new AjaxResultModel();
        }
        #endregion

        #region SaveWz5 - Application.StatusID = 4
        public static int Wz5Save(AppResultsModel model)
        {
            int c = 0;
            #region SQL
            string sql = @"

UPDATE [dbo].[Application] 
    SET [WizardStepID]=@WizardStepID,
        [StatusID]=@StatusID,
        [RegistrationDate]=@RegistrationDate,
        [Priority]=@Priority,
        [Uid]=@Uid,
        [NeedHostel]=@NeedHostel,
        [LastEgeDocumentsCheckDate]=@LastEgeDocumentsCheckDate,
        [LastCheckDate]=@LastCheckDate,  
        [ApplicationNumber]=@ApplicationNumber
WHERE ApplicationId=@ApplicationID

";
            #endregion
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);

                #region Параметры
                com.Parameters.Add(new SqlParameter("ApplicationID", SqlDbType.Int) { Value = model.ApplicationID });
                com.Parameters.Add(new SqlParameter("WizardStepID", SqlDbType.Int) { Value = model.Step });
                com.Parameters.Add(new SqlParameter("StatusID", SqlDbType.Int) { Value = 4 });

                com.Parameters.Add(new SqlParameter("RegistrationDate", SqlDbType.DateTime) { Value = model.RegistrationDate });
                com.Parameters.Add(new SqlParameter("LastEgeDocumentsCheckDate", SqlDbType.DateTime) { Value = DateTime.Now });
                com.Parameters.Add(new SqlParameter("LastCheckDate", SqlDbType.DateTime) { Value = DateTime.Now });
                if (model.Priority.HasValue)
                    com.Parameters.Add(new SqlParameter("Priority", SqlDbType.Int) { Value = Convert.ToInt32(model.Priority.Value) });
                else
                    com.Parameters.Add(new SqlParameter("Priority", SqlDbType.Int) { Value = DBNull.Value });
                if (model.Uid == null)
                    com.Parameters.Add(new SqlParameter("Uid", SqlDbType.NVarChar, 200) { Value = DBNull.Value });
                else
                    com.Parameters.Add(new SqlParameter("Uid", SqlDbType.NVarChar, 200) { Value = model.Uid });
                com.Parameters.Add(new SqlParameter("NeedHostel", SqlDbType.Bit) { Value = model.NeedHostel });
                if (model.ApplicationNumber == null)
                    com.Parameters.Add(new SqlParameter("ApplicationNumber", SqlDbType.NVarChar, 50) { Value = DBNull.Value });
                else
                    com.Parameters.Add(new SqlParameter("ApplicationNumber", SqlDbType.NVarChar, 50) { Value = model.ApplicationNumber });

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

        #region SaveWz5 - Application.StatusID = 2

        /// <summary>
        /// При сохранении заявления по кнопке «Сохранить без проверки в новых» Система не должна осуществлять проверку результатов ЕГЭ и наличие ошибок.
        /// Заявление должно сохраняться со статусом Application.StatusID = 2.
        /// </summary>
        /// <param name="ApplicationID"></param>
        /// <param name="step"></param>
        /// <returns></returns>
        public static int SaveWz5(AppResultsModel model)
        {
            int c = 0;
            #region SQL
            string sql = @"
UPDATE [dbo].[Application]
SET [WizardStepID]=@WizardStepID,
    [StatusID]=@StatusID,
    [RegistrationDate]=@RegistrationDate,
    [Priority]=@Priority, 
    [Uid]=@Uid,
    [NeedHostel]=@NeedHostel,
    [LastEgeDocumentsCheckDate]=@LastEgeDocumentsCheckDate,
    [LastCheckDate]=@LastCheckDate,
    [ApplicationNumber]=@ApplicationNumber
WHERE ApplicationId=@ApplicationID";
            #endregion
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);

                #region Параметры
                com.Parameters.Add(new SqlParameter("ApplicationID", SqlDbType.Int) { Value = model.ApplicationID });
                com.Parameters.Add(new SqlParameter("WizardStepID", SqlDbType.Int) { Value = model.Step });
                com.Parameters.Add(new SqlParameter("StatusID", SqlDbType.Int) { Value = 2 });
                com.Parameters.Add(new SqlParameter("RegistrationDate", SqlDbType.DateTime) { Value = model.RegistrationDate });
                com.Parameters.Add(new SqlParameter("LastEgeDocumentsCheckDate", SqlDbType.DateTime) { Value = DateTime.Now });
                com.Parameters.Add(new SqlParameter("LastCheckDate", SqlDbType.DateTime) { Value = DateTime.Now });
                if (model.Priority.HasValue)
                    com.Parameters.Add(new SqlParameter("Priority", SqlDbType.Int) { Value = Convert.ToInt32(model.Priority.Value) });
                else
                    com.Parameters.Add(new SqlParameter("Priority", SqlDbType.Int) { Value = DBNull.Value });
                if (model.Uid == null)
                    com.Parameters.Add(new SqlParameter("Uid", SqlDbType.NVarChar, 200) { Value = DBNull.Value });
                else
                    com.Parameters.Add(new SqlParameter("Uid", SqlDbType.NVarChar, 200) { Value = model.Uid });
                com.Parameters.Add(new SqlParameter("NeedHostel", SqlDbType.Bit) { Value = model.NeedHostel });
                if (model.ApplicationNumber == null)
                    com.Parameters.Add(new SqlParameter("ApplicationNumber", SqlDbType.NVarChar, 50) { Value = DBNull.Value });
                else
                    com.Parameters.Add(new SqlParameter("ApplicationNumber", SqlDbType.NVarChar, 50) { Value = model.ApplicationNumber });

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

        #region Wz5SaveUnauthenticated Application.StatusID = 3
        /// <summary>
        /// При нажатии кнопки «Оставить непрошедшим проверку» процесс работы Мастера завершается, заявлению присваивается статус «Не прошедшее проверку» (полю Application. StatusID присваивается «3»).
        /// </summary>
        /// <param name="ApplicationID"></param>
        /// <param name="step"></param>
        /// <returns></returns>
        public static int Wz5SaveUnauthenticated(AppResultsModel model)
        {
            int c = 0;
            #region SQL
            string sql = @"UPDATE [dbo].[Application]
SET [WizardStepID]=@WizardStepID,
    [StatusID]=@StatusID,
    [RegistrationDate]=@RegistrationDate,
    [Priority]=@Priority,
    [Uid]=@Uid,
    [NeedHostel]=@NeedHostel,
    [LastEgeDocumentsCheckDate]=@LastEgeDocumentsCheckDate,
    [LastCheckDate]=@LastCheckDate
WHERE ApplicationId=@ApplicationID";
            #endregion
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);

                #region Параметры
                com.Parameters.Add(new SqlParameter("ApplicationID", SqlDbType.Int) { Value = model.ApplicationID });
                com.Parameters.Add(new SqlParameter("WizardStepID", SqlDbType.Int) { Value = model.Step });
                com.Parameters.Add(new SqlParameter("StatusID", SqlDbType.Int) { Value = 3 });
                com.Parameters.Add(new SqlParameter("RegistrationDate", SqlDbType.DateTime) { Value = model.RegistrationDate });
                com.Parameters.Add(new SqlParameter("LastEgeDocumentsCheckDate", SqlDbType.DateTime) { Value = DateTime.Now });
                com.Parameters.Add(new SqlParameter("LastCheckDate", SqlDbType.DateTime) { Value = DateTime.Now });
                if (model.Priority.HasValue)
                {
                    com.Parameters.Add(new SqlParameter("Priority", SqlDbType.Int) { Value = Convert.ToInt32(model.Priority.Value) });
                }
                else
                {
                    com.Parameters.Add(new SqlParameter("Priority", SqlDbType.Int) { Value = DBNull.Value });
                }
                if (model.Uid == null)
                {
                    com.Parameters.Add(new SqlParameter("Uid", SqlDbType.NVarChar, 200) { Value = DBNull.Value });
                }
                else
                {
                    com.Parameters.Add(new SqlParameter("Uid", SqlDbType.NVarChar, 200) { Value = model.Uid });
                }
                com.Parameters.Add(new SqlParameter("NeedHostel", SqlDbType.Bit) { Value = model.NeedHostel });

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

        #region Wz5Approve - Application.StatusID = 4
        public static int ForceAdmission(ForceAdmissionModel model)
        {
            int c = 0;
            #region SQL
            string sql = @"
UPDATE [dbo].[Application]
    SET [WizardStepID]=@WizardStepID,
    [StatusID]=@StatusID,
    [StatusDecision]=@StatusDecision,
    [RegistrationDate]=@RegistrationDate,
    [Priority]=@Priority,
    [Uid]=@Uid,
    [NeedHostel]=@NeedHostel,
    [LastEgeDocumentsCheckDate]=@LastEgeDocumentsCheckDate,
    [LastCheckDate]=@LastCheckDate,
    [ApplicationForcedAdmissionReasonsID]=@AdmissionReasonId
WHERE ApplicationId=@ApplicationID 

DECLARE @attachmentsToDelete TABLE (ID INT)
INSERT INTO @attachmentsToDelete(ID) SELECT AttachmentID FROM Attachment WHERE AttachmentID IN (SELECT AttachmentID FROM ApplicationForcedAdmissionDocument WHERE ApplicationID=@ApplicationID)

DELETE FROM ApplicationForcedAdmissionDocument WHERE ApplicationID=@ApplicationID
DELETE FROM Attachment WHERE AttachmentID IN (SELECT ID FROM @attachmentsToDelete)
";
            if (model.Attachments != null)
            {
                foreach (ForceAdmissionAttachmentModel attachment in model.Attachments)
                {
                    sql += String.Format(@"

INSERT INTO ApplicationForcedAdmissionDocument(ApplicationID,AttachmentID,DocumentType)
SELECT @ApplicationID, (SELECT TOP 1 AttachmentID FROM Attachment WHERE FileID='{0}'),{1}", attachment.AttachmentFileID, attachment.AttachmentType);
                }
            }

            #endregion
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);

                #region Параметры
                com.Parameters.Add(new SqlParameter("ApplicationID", SqlDbType.Int) { Value = model.ApplicationID });
                com.Parameters.Add(new SqlParameter("WizardStepID", SqlDbType.Int) { Value = 1 });
                com.Parameters.Add(new SqlParameter("StatusID", SqlDbType.Int) { Value = 4 });
                com.Parameters.Add(new SqlParameter("StatusDecision", SqlDbType.VarChar) { Value = model.Comment });
                com.Parameters.Add(new SqlParameter("RegistrationDate", SqlDbType.DateTime) { Value = model.ApplicationRegistrationDate });
                com.Parameters.Add(new SqlParameter("LastEgeDocumentsCheckDate", SqlDbType.DateTime) { Value = DateTime.Now });
                com.Parameters.Add(new SqlParameter("LastCheckDate", SqlDbType.DateTime) { Value = DateTime.Now });

                if (model.ApplicationPriority.HasValue)
                {
                    com.Parameters.Add(new SqlParameter("Priority", SqlDbType.Int) { Value = Convert.ToInt32(model.ApplicationPriority.Value) });
                }
                else
                {
                    com.Parameters.Add(new SqlParameter("Priority", SqlDbType.Int) { Value = DBNull.Value });
                }
                if (model.ApplicationUid == null)
                {
                    com.Parameters.Add(new SqlParameter("Uid", SqlDbType.NVarChar, 200) { Value = DBNull.Value });
                }
                else
                {
                    com.Parameters.Add(new SqlParameter("Uid", SqlDbType.NVarChar, 200) { Value = model.ApplicationUid });
                }
                com.Parameters.Add(new SqlParameter("NeedHostel", SqlDbType.Bit) { Value = model.ApplicationNeedHostel });

                if (model.ReasonID != 0)
                {
                    com.Parameters.Add(new SqlParameter("AdmissionReasonId", SqlDbType.Int) { Value = model.ReasonID });
                }
                else
                {
                    com.Parameters.Add(new SqlParameter("AdmissionReasonId", SqlDbType.Int) { Value = DBNull.Value });
                }

                #endregion
                try
                {
                    con.Open();
                    // Вставить и вернуть новый ID
                    com.ExecuteNonQuery(); 
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

        #region UpdateViolation
        public static int UpdateViolation(int ApplicationID, string ViolationErrors)
        {
            int c = 0;
            #region SQL
            string sql = @"UPDATE [dbo].[Application] SET [ViolationID]=@ViolationID, [ViolationErrors]=@ViolationErrors WHERE ApplicationId=@ApplicationID";
            #endregion
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand com = new SqlCommand(sql, con);

                #region Параметры
                com.Parameters.Add(new SqlParameter("ApplicationID", SqlDbType.Int) { Value = ApplicationID });
                com.Parameters.Add(new SqlParameter("ViolationID", SqlDbType.Int) { Value = 1 });
                com.Parameters.Add(new SqlParameter("ViolationErrors", SqlDbType.VarChar) { Value = ViolationErrors });

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

        #region UpdateViolation
        public static ApplicationWz5ViewModel.getViolationMoreInfoViewModel getViolationMoreInfo(int ApplicationID)
        {
            #region SQL

            string sql = @"
-- DECLARE @ApplicationID int = 160;
--------------------
SELECT app.StatusDecision
FROM Application app (NOLOCK)
where ApplicationId = @ApplicationID 
            ";
            #endregion

            ApplicationWz5ViewModel.getViolationMoreInfoViewModel vmf = new ApplicationWz5ViewModel.getViolationMoreInfoViewModel();
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
                    vmf = new ApplicationWz5ViewModel.getViolationMoreInfoViewModel();
                    vmf.ApplicationID = ApplicationID;

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        vmf.StatusDecision = ds.Tables[0].Rows[i]["StatusDecision"] as string;                       
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
            return vmf;
        }
        #endregion

        #region checkApplication
        /// <summary>
        /// FIS-206 Запускает процедуру по проверке кол-ва Вузов, в которые абитуриент подал заявления -(Подключить к окну ввода заявления проверку факта подачи абитуриентом заявления в более чем 5 вузов)
        /// </summary>
        /// <param name="ApplicationID"></param>
        /// <returns></returns>

        public static SPResult GetCheckApplication(int ApplicationID)
        {
            SPResult result = null;

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand("gvuz_ValidateOtherApplicationsCount", con);

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@applicationId", SqlDbType.Int) { Value = ApplicationID });

                    command.Parameters.Add(new SqlParameter("@returnValue", SqlDbType.Int) { Direction = ParameterDirection.ReturnValue });
                    command.Parameters.Add(new SqlParameter("@errorMessage", SqlDbType.NVarChar, 4000) { Direction = ParameterDirection.Output });
                    command.Parameters.Add(new SqlParameter("@violationMessage", SqlDbType.NVarChar, 4000) { Direction = ParameterDirection.Output });
                    command.Parameters.Add(new SqlParameter("@violationId", SqlDbType.Int) { Direction = ParameterDirection.Output });

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