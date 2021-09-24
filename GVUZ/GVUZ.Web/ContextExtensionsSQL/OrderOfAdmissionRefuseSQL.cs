using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using GVUZ.Web.Helpers;
using GVUZ.Web.Infrastructure;
using GVUZ.Web.SQLDB;
using GVUZ.Web.ViewModels.OrderOfAdmission;
using GVUZ.Web.ViewModels.Shared;
using GVUZ.DAL.Dapper.ViewModel.Common;
using GVUZ.Data.Model;

namespace GVUZ.Web.ContextExtensionsSQL
{
    public static partial class OrderOfAdmissionSQL
    {
        public static void IncludeApplicationsToOrderOfAdmissionRefuse(IEnumerable<int> applicationItemIds, int orderId)
        {
            if (applicationItemIds == null)
                throw new ArgumentNullException("applicationItemIds");
            if (!applicationItemIds.Any())
                throw new ArgumentException("applicationItemIds");
            if (orderId == 0)
                throw new ArgumentException("orderId");

            StringBuilder ids=new StringBuilder();
            foreach (int applicationItemId in applicationItemIds)
            {
                ids.AppendFormat("INSERT INTO @applicationItemIds(Id) VALUES ({0})", applicationItemId).AppendLine();
            }

            string sql = String.Format(@"
DECLARE @now DATETIME = GETDATE()

DECLARE @applicationItemIds TABLE (Id int)
{0}


--Обновим старые приказы о зачислении (из которых исключаем заявления), статус - не меняем
UPDATE 
    ordersOfAdmissionToUpdate
SET
    ordersOfAdmissionToUpdate.DateEdited = @now
FROM
    OrderOfAdmission ordersOfAdmissionToUpdate (NOLOCK)
    INNER JOIN ApplicationCompetitiveGroupItem acgi (NOLOCK) ON acgi.OrderOfAdmissionID = ordersOfAdmissionToUpdate.OrderID    
    INNER JOIN @applicationItemIds applicationItemIds ON applicationItemIds.Id = acgi.Id


--Обновим приказ об отказе от зачисления (в который включаем заявления)
UPDATE 
    OrderOfAdmission
SET
    OrderOfAdmission.DateCreated = ISNULL(DateCreated,@now)
    ,OrderOfAdmission.DateEdited = @now
    ,OrderOfAdmission.OrderOfAdmissionStatusID
        = CASE WHEN OrderOfAdmission.OrderOfAdmissionStatusID = @noApplicationStatus 
        THEN @notPublishedStatus
        ELSE OrderOfAdmission.OrderOfAdmissionStatusID END
WHERE  
    OrderOfAdmission.OrderID = @orderId


--Обновим заявления
UPDATE  
    applicationsToUpdate
SET 
    applicationsToUpdate.StatusID = @applicationStatusId
    ,applicationsToUpdate.OrderOfAdmissionID = @orderId
FROM 
    [Application] applicationsToUpdate (NOLOCK)
    INNER JOIN ApplicationCompetitiveGroupItem acgi (NOLOCK) on acgi.ApplicationId = applicationsToUpdate.ApplicationId
    INNER JOIN @applicationItemIds applicationItemIds ON applicationItemIds.Id = acgi.Id


--Обновим условия приема
UPDATE
    acgiToUpdate
SET
    acgiToUpdate.OrderOfExceptionID = @orderId, --Поставить ссылку на приказ об отказе от зачисления
    acgiToUpdate.ExceptionDate = @now
FROM 
    ApplicationCompetitiveGroupItem acgiToUpdate (NOLOCK)
    INNER JOIN @applicationItemIds applicationItemIds ON applicationItemIds.Id = acgiToUpdate.Id


--Запишем все в историю
INSERT INTO
    OrderOfAdmissionHistory
    (
        OrderID
        ,ApplicationID
        ,CreatedDate
    )
SELECT
    @orderId
    ,app.ApplicationId
    ,@now
FROM
    [Application] app (NOLOCK)
    INNER JOIN ApplicationCompetitiveGroupItem acgi (NOLOCK) on acgi.ApplicationId = app.ApplicationId
    INNER JOIN @applicationItemIds applicationItemIds ON applicationItemIds.Id = acgi.Id
", ids.ToString());

            SqlQueryHelper.Execute(sql,new SqlParameter[]{
            new SqlParameter("applicationStatusId",ApplicationStatusType.Accepted)
            ,new SqlParameter("orderId",orderId)
            ,new SqlParameter("noApplicationStatus",OrderOfAdmissionStatus.NoApplications)
            ,new SqlParameter("notPublishedStatus",OrderOfAdmissionStatus.NotPublished)
            }); 
        }

        public static ApplicationSelectOrderViewModel GetOrderOfAdmissionRefuseRecords(int institutionId      ,       ApplicationSelectOrderQueryViewModel queryModel )
        {
            ApplicationSelectOrderFilterViewModel filterData = queryModel.Filter ??
                                                               new ApplicationSelectOrderFilterViewModel();
            PagerViewModel pager = queryModel.Pager ?? new PagerViewModel();
            SortViewModel sortOptions = queryModel.Sort;

            FilterStateManager.Current.Update(filterData);

            int totalUnfiltered = 0;

            if (ConfigHelper.ShowFilterStatistics())
            {
                totalUnfiltered = GetTotalOrdersCountByInstitution(institutionId,OrderOfAdmissionType.OrderOfAdmissionRefuse,null);
                if (totalUnfiltered == 0)
                {
                    return new ApplicationSelectOrderViewModel
                    {
                        Pager = new PagerViewModel(),
                        SortDescending = sortOptions.SortDescending,
                        SortKey = sortOptions.SortKey,
                        TotalOrdersCount = totalUnfiltered
                    };
                }
            }
             
            string rowCountQuery = @"
SELECT COUNT(*) app_orders
	FROM
	(
		SELECT ooa.OrderId 
		FROM OrderOfAdmission AS ooa (NOLOCK)
			LEFT JOIN AdmissionItemType AS edu_level (NOLOCK) ON ooa.EducationLevelID = edu_level.ItemTypeId
			LEFT JOIN AdmissionItemType AS edu_form (NOLOCK) ON ooa.EducationFormID = edu_form.ItemTypeId
			LEFT JOIN AdmissionItemType AS edu_source (NOLOCK) ON ooa.EducationSourceID = edu_source.ItemTypeId			
		WHERE 
            ooa.InstitutionID=@pInstitutionId
			AND ooa.OrderOfAdmissionStatusID NOT IN (3,4) --Не опубликован и не удален
            AND ooa.OrderOfAdmissionTypeID = @orderTypeId
		GROUP BY 
            ooa.OrderId,
			ooa.OrderName,
			ooa.Stage,
			edu_level.Name, 
			edu_form.Name,
			edu_source.Name,
			ooa.IsForBeneficiary,
			ooa.IsForeigner
	) AS CountOrderId 
";

            string paged = @"
paged AS (
    SELECT t.*, ROW_NUMBER() OVER(ORDER BY {0} {1}) rn
    FROM main t
)
SELECT paged.* from paged where paged.rn between {2} and {3}
";

            string pagedQuery = string.Format(@" 
WITH app_orders AS
(
	SELECT 
        COUNT(*) app_orders
	FROM
	(
		SELECT ooa.OrderId 
		FROM OrderOfAdmission AS ooa (NOLOCK)
			LEFT JOIN AdmissionItemType AS edu_level (NOLOCK) ON ooa.EducationLevelID = edu_level.ItemTypeId
			LEFT JOIN AdmissionItemType AS edu_form (NOLOCK) ON ooa.EducationFormID = edu_form.ItemTypeId
			LEFT JOIN AdmissionItemType AS edu_source (NOLOCK) ON ooa.EducationSourceID = edu_source.ItemTypeId
		WHERE 
            ooa.InstitutionID=@pInstitutionId
			AND ooa.OrderOfAdmissionStatusID NOT IN (3,4) --Не опубликован и не удален
            AND ooa.OrderOfAdmissionTypeID =@orderTypeId
		GROUP BY 
            ooa.OrderId,
			ooa.OrderName,
			ooa.Stage,
			edu_level.Name, 
			edu_form.Name,
			edu_source.Name,
			ooa.IsForBeneficiary,
			ooa.IsForeigner
	) AS CountOrderId
),
main AS
(
	SELECT
		ooa.OrderId,
		ooa.OrderName,
        ooa.orderNumber,
		ooa.Stage,
		edu_level.Name EducationLevel, 
		edu_form.Name EducationForm,
		edu_source.Name EducationSource,
		ooa.IsForBeneficiary,
		ooa.IsForeigner
	FROM OrderOfAdmission AS ooa (NOLOCK)
		LEFT JOIN AdmissionItemType AS edu_level (NOLOCK) ON ooa.EducationLevelID = edu_level.ItemTypeId
		LEFT JOIN AdmissionItemType AS edu_form (NOLOCK) ON ooa.EducationFormID = edu_form.ItemTypeId
		LEFT JOIN AdmissionItemType AS edu_source (NOLOCK) ON ooa.EducationSourceID = edu_source.ItemTypeId
		WHERE 
            ooa.InstitutionID=@pInstitutionId
			AND ooa.OrderOfAdmissionStatusID NOT IN (3,4) --Не опубликован и не удален
            AND ooa.OrderOfAdmissionTypeID = @orderTypeId
		GROUP BY 
            ooa.OrderId,
			ooa.OrderName,
            ooa.orderNumber,
			ooa.Stage,
			edu_level.Name, 
			edu_form.Name,
			edu_source.Name,
			ooa.IsForBeneficiary,
			ooa.IsForeigner
),
{0}
", paged);

            #region Создаем параметры с учетом значений фильтра

            object dbnull = DBNull.Value;
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@pInstitutionId", institutionId));
            parameters.Add(new SqlParameter("@orderTypeId", OrderOfAdmissionType.OrderOfAdmissionRefuse));

            #endregion

            List<ApplicationSelectOrderRecordViewModel> records = null;

            pager.TotalRecords = SqlQueryHelper.GetScalarInt(rowCountQuery, parameters.ToArray()).GetValueOrDefault();

            if (pager.TotalRecords > 0)
            {
                string selectQuery = string.Format(pagedQuery, sortOptions.SortKey,
                    sortOptions.SortDescending.GetValueOrDefault() ? "DESC" : "ASC",
                    SqlQueryHelper.PageFirstRecordParameterName, SqlQueryHelper.PageLastRecordParameterName);
                parameters.AddRange(SqlQueryHelper.CreatePageRangeParameters(pager.FirstRecordOffset,
                    pager.LastRecordOffset));
                records = SqlQueryHelper.GetRecords(selectQuery, parameters.CopyToArray(),OrderOfAdmissionForAppSQL.MapOrderOfAdmissionRecord);
            }

            return new ApplicationSelectOrderViewModel
            { 
                Filter = filterData,
                Pager = pager,
                SortKey = sortOptions.SortKey,
                SortDescending = sortOptions.SortDescending,
                Records = records,
                TotalOrdersCount = totalUnfiltered
            };
        }
    }
}