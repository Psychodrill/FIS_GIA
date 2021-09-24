using GVUZ.ImportService2016.Core.Main.Dictionaries;
using GVUZ.ImportService2016.Core.Main.Dictionaries.Application;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace GVUZ.ImportService2016.Core.Main.Repositories
{
	public class ADOOrderOfAdmissionRepository
	{
		public static ApplicationCompetitiveGroupItemVoc GetApplicationCompetitiveGroupItemVoc(int ApplicationID)
		{
			string sql = @"
SELECT
	acgi.[id] as ID
	,[ApplicationId]
	,acgi.[CompetitiveGroupId]
	,acgi.[CompetitiveGroupItemId]
	,[EducationFormId]
	,[EducationSourceId]
	,isnull([Priority], -1) as 'Priority'
	,[CompetitiveGroupTargetId]
	,cg.Course
	,cg.CampaignID
	,cgi.DirectionID
	,cgi.EducationLevelID
    ,cg.UID as CompetitiveGroupUID
    ,c.StatusId as CampaignStatusID
FROM [ApplicationCompetitiveGroupItem] acgi
inner join [CompetitiveGroup] cg on cg.CompetitiveGroupID = acgi.CompetitiveGroupId
inner join [CompetitiveGroupItem] cgi on cgi.CompetitiveGroupItemID = acgi.CompetitiveGroupItemId
inner join [Campaign] c on c.CampaignID = cg.CampaignID
Where ApplicationId = @ApplicationId
Order by Priority, ID;
";

			var parameters = new Dictionary<string, object>();
			parameters.Add("@ApplicationId", ApplicationID);

			var ds = ADOBaseRepository.ExecuteQuery(sql, parameters);

			if (ds != null && ds.Tables.Count > 0)
			{
				ApplicationCompetitiveGroupItemVoc res = new ApplicationCompetitiveGroupItemVoc(ds.Tables[0]);
				return res;
			}
			return null;
		}

        public static bool GetResultCheck8(int ApplicationID, int IdLevelBudget, int AppCGItemId)
        {
            //            string sql = @"
            //DECLARE @Errors TABLE (ID INT, Msg varchar(500));
            //DECLARE @CGCount int; 
            //DECLARE @CGTotal int;
            //SELECT 
            //	@CGCount=(SELECT COUNT(DISTINCT a.ApplicationID) 
            //	 FROM Application a (NOLOCK) 
            //     INNER JOIN OrderOfAdmission a_ooa (NOLOCK) on a_ooa.OrderID=a.OrderOfAdmissionID AND a_ooa.IsForeigner=0
            //	 WHERE a.StatusID=8 
            //		and a.OrderCompetitiveGroupID=acgi.CompetitiveGroupID
            //		and a.OrderCompetitiveGroupItemID=acgi.CompetitiveGroupItemID
            //		AND a.OrderEducationFormID=acgi.EducationFormID
            //		AND a.OrderEducationSourceId=acgi.EducationSourceId
            //		AND a.InstitutionID=app.InstitutionID		
            //		AND (a.OrderCompetitiveGroupTargetID=acgi.CompetitiveGroupTargetID OR (a.OrderCompetitiveGroupTargetID IS NULL AND acgi.CompetitiveGroupTargetID IS NULL))
            //	),
            //	@CGTotal=ISNULL(KCP.CGTotal,0)
            //FROM Application app (NOLOCK) 
            //	INNER JOIN ApplicationCompetitiveGroupItem acgi (NOLOCK) on acgi.ApplicationID = app.ApplicationID
            //	INNER JOIN CompetitiveGroup cg (NOLOCK) on acgi.CompetitiveGroupID = cg.CompetitiveGroupId
            //    INNER JOIN CompetitiveGroupItem cgi (NOLOCK) on acgi.CompetitiveGroupItemId = cgi.CompetitiveGroupItemId    
            //	INNER JOIN dbo.FGetKCP(@AppCGItemId) KCP on KCP.ApplicationCompetitiveGroupItemID=acgi.id
            //WHERE acgi.Priority is not null AND app.ApplicationID=@ApplicationID AND acgi.id=@AppCGItemId;
            //
            // IF @CGCount>=@CGTotal BEGIN
            //	INSERT INTO @Errors(ID,Msg)VALUES(100,'Включение в приказ о зачислении  данного заявления превышает доступный объем приема в рамках выбранной конкурсной группы');
            // END;
            // select * from @Errors;
            //";

            string sql = @"
SELECT * FROM [dbo].[FuncAdmissionVolumeExceeding] (
   @ApplicationID
  ,@OrderID
  ,@IdLevelBudget
  ,@AppCGItemId
  ,@FullCheck)            
";

            var parameters = new Dictionary<string, object>();
            parameters.Add("@ApplicationID", ApplicationID);
            parameters.Add("@OrderID", 0); // Не важно значение, когда FullCheck = 0, т.е. только проверка №8
            parameters.Add("@IdLevelBudget", IdLevelBudget);
            parameters.Add("@AppCGItemId", AppCGItemId);
            parameters.Add("@FullCheck", 0);

            var ds = ADOBaseRepository.ExecuteQuery(sql, parameters);

            return !(ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0);
        }


		public static ApplicationVocDto GetApplicationByNumberAndRegDate(string ApplicationNumber, DateTime RegistrationDate,
			int InstitutionID)
		{
			string sql = @"
SELECT ApplicationID, UID, ApplicationNumber, RegistrationDate, EntrantID, StatusID, 
	OrderOfAdmissionID, OrderCompetitiveGroupItemID,  OrderCompetitiveGroupID, OrderCompetitiveGroupTargetID,  ApplicationGUID
FROM [Application] where InstitutionID = @InstitutionID AND ApplicationNumber = @ApplicationNumber AND convert(varchar, RegistrationDate, 112) = @RegistrationDate
";

			var parameters = new Dictionary<string, object>();
			parameters.Add("@InstitutionID", InstitutionID);
			parameters.Add("@ApplicationNumber", ApplicationNumber);
			parameters.Add("@RegistrationDate", RegistrationDate.ToString("yyyyMMdd"));

			var ds = ADOBaseRepository.ExecuteQuery(sql, parameters);

			if (ds != null && ds.Tables.Count > 0)
			{
				ApplicationVoc appVoc = new ApplicationVoc(ds.Tables[0]);
				if (appVoc.Items.Any())
					return appVoc.Items.FirstOrDefault();
			}
			return null;
		}

		public static List<int> GetApplicationOrderOfAdmissionsByHistory(int ApplicationID)
		{
			string sql = @"
select OrderID
from OrderOfAdmissionHistory ooah
where ooah.ApplicationID = @ApplicationID AND ooah.ModifiedDate is null;
";

			var parameters = new Dictionary<string, object>();
			parameters.Add("@ApplicationId", ApplicationID);

			var ds = ADOBaseRepository.ExecuteQuery(sql, parameters);

			var res = new List<int>();
			if (ds != null && ds.Tables.Count > 0)
			{
				foreach (DataRow row in ds.Tables[0].Rows)
					res.Add((int) row[0]);
			}
			return res;
		}

		public static Tuple<int, bool> GetApplicationNationalityType(int ApplicationID)
		{
			string sql = @"
select isnull(edi.NationalityTypeID, 0), isnull(idt.IsRussianNationality, 0)
from Application a (NOLOCK)
inner join Entrant e (NOLOCK) on a.EntrantID = e.EntrantID
inner join [EntrantDocumentIdentity] edi (NOLOCK) on edi.[EntrantDocumentID]= e.IdentityDocumentID
inner join IdentityDocumentType idt (NOLOCK) on idt.[IdentityDocumentTypeID] = edi.[IdentityDocumentTypeID]
where a.ApplicationID = @ApplicationID;
";

			var parameters = new Dictionary<string, object>();
			parameters.Add("@ApplicationId", ApplicationID);

			var ds = ADOBaseRepository.ExecuteQuery(sql, parameters);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0].ItemArray != null && ds.Tables[0].Rows[0].ItemArray.Count() >= 2)
            {
				var res = new Tuple<int, bool>((int) ds.Tables[0].Rows[0][0], (bool) ds.Tables[0].Rows[0][1]);
				return res;
			}
			return null;
		}

		public static List<int> GetApplicationsViolationCheck(List<int> applicationsToCheckViolation)
		{
			string sql = @"
select a.applicationID
from application a (NOLOCK)
inner join @applicationIDs aIDs on a.ApplicationID = aIDs.Id
Where a.ViolationID != 0 AND a.StatusDecision is null;
";

			DataTable applicationIDs = new DataTable("applicationIDs");
			applicationIDs.Columns.Add("Id", typeof (int));
			applicationsToCheckViolation.Distinct().ToList().ForEach(x => applicationIDs.Rows.Add(x));

			//var parameters = new Dictionary<string, object>();
			//parameters.Add("@applicationIDs",  applicationIDs);

			//var ds = ADODependencyRepository.ExecuteQuery(sql, parameters);

			var ds = new DataSet();
			var connectionString = ConfigurationManager.ConnectionStrings["Main"].ConnectionString;
			using (var connection = new SqlConnection(connectionString))
			{
				connection.Open();
				//using (SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted)) 
				using (SqlCommand cmd = new SqlCommand(sql, connection))
				{
					cmd.CommandTimeout = ADOBaseRepository.TIMEOUT;

					//foreach (var p in parameters)
					SqlParameter param = cmd.Parameters.AddWithValue("@applicationIDs", applicationIDs);
					param.SqlDbType = SqlDbType.Structured;
					param.TypeName = "dbo.Identifiers";

					var adapter = new SqlDataAdapter(cmd);
					adapter.Fill(ds);
				}
			}


			var res = new List<int>();
			if (ds != null && ds.Tables.Count > 0)
			{
				foreach (DataRow row in ds.Tables[0].Rows)
					res.Add((int) row[0]);
			}
			return res;
		}

		public static OrderOfAdmissionVocDto GetOrderByUID(string uid, int InstitutionID)
		{
			if (String.IsNullOrEmpty(uid))
			{
				return null;
			}
			string sql = @"
SELECT [OrderID]
      ,[OrderStatus]
      ,[DatePublished]
      ,[UID]
      ,[CampaignID]
      ,[Course]
      ,[EducationLevelID]
      ,[EducationFormID]
      ,[EducationSourceID]
      ,IsNull([Stage], -1) as Stage
      ,[IsForBeneficiary]
      ,[IsForeigner]
      ,[OrderName]
      ,[OrderNumber]
      ,[OrderDate]
FROM [OrderOfAdmission]
WHERE InstitutionID = @InstitutionID AND  UID = @UID AND OrderStatus <= 3;
";

			var parameters = new Dictionary<string, object>();
			parameters.Add("@InstitutionID", InstitutionID);
			parameters.Add("@UID", uid);

			var ds = ADOBaseRepository.ExecuteQuery(sql, parameters);

			if (ds != null && ds.Tables.Count > 0)
			{
				OrderOfAdmissionVoc voc = new OrderOfAdmissionVoc(ds.Tables[0]);
				if (voc.Items.Any())
					return voc.Items.FirstOrDefault();
			}
			return null;
		}

		public static bool HasApplicationsInOrder(int orderID)
		{
			string sql = @"
SELECT [ApplicationID]
FROM [Application]
WHERE OrderOfAdmissionID = @OrderID AND StatusID = 8
";

			var parameters = new Dictionary<string, object>();
			parameters.Add("@OrderID", orderID);

			var ds = ADOBaseRepository.ExecuteQuery(sql, parameters);

			if (ds != null && ds.Tables.Count > 0)
			{
				return ds.Tables[0].Rows.Count > 0;
			}
			return false;
		}

        public static string HasOtherOrderWithSameNumber(string orderNumber, string uid, int institutionID)
        {
            string sql = @"
SELECT [OrderID], UID
FROM [OrderOfAdmission]
WHERE InstitutionID = @InstitutionID 
AND (UID is null OR UID != @UID) 
AND OrderNumber = @OrderNumber AND OrderStatus <= 3;
";

            var parameters = new Dictionary<string, object>();
            parameters.Add("@InstitutionID", institutionID);
            parameters.Add("@UID", uid);
            parameters.Add("@OrderNumber", orderNumber);

            var ds = ADOBaseRepository.ExecuteQuery(sql, parameters);
            var res = string.Empty;
            if (ds != null && ds.Tables.Count > 0)
            {
                List<string> uids = new List<string>();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    var rUid = row[1].ToString();
                    if (rUid == string.Empty)
                        rUid = "(не указан), ID=" + row[0].ToString();
                    uids.Add(rUid);
                }

                res = string.Join(",", uids);
            }
            return res;
        }



	}
}


