using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using GVUZ.Web.Helpers;
using GVUZ.Web.Infrastructure;
using GVUZ.Web.SQLDB;
using GVUZ.Web.ViewModels.OrderOfAdmission;
using GVUZ.Web.ViewModels.Shared;
using GVUZ.DAL.Dapper.ViewModel.Common;

namespace GVUZ.Web.ContextExtensionsSQL
{
    public static partial class OrderOfAdmissionForAppSQL
    {
        #region Фильтр выбора списка приказов о зачислении

        public static ApplicationSelectOrderFilterViewModel GetOrderOfAdmissionFilter(int institutionId,
            int applicationId)
        {
            var model = FilterStateManager.Current.GetOrCreate<ApplicationSelectOrderFilterViewModel>();
            model.EducationForms.Items.AddRange(CacheManager.Current.GetOrCreateItem(CacheKeys.EducationFormsForOrders,
                LoadEducationForms));
            model.EducationSources.Items.AddRange(
                CacheManager.Current.GetOrCreateItem(CacheKeys.EducationSourcesForOrders, LoadEducationSources));
            model.EducationLevels.Items.AddRange(LoadInstitutionCampaignEducationLevels(institutionId));
            return model;
        }

        /// <summary>
        /// Список форм обучения
        /// </summary>
        public static IEnumerable<SelectListItemViewModel<short>> LoadEducationForms()
        {
            const string query =
                @"select ait.ItemTypeId, ait.Name from AdmissionItemType ait where ait.ItemLevel = 7 order by ait.ItemTypeID";
            return SqlQueryHelper.GetRecords(query, null, SqlQueryHelper.MapSelectListItem<short>);
        }

        /// <summary>
        /// Список источников финансирования
        /// </summary>
        private static IEnumerable<SelectListItemViewModel<short>> LoadEducationSources()
        {
            const string query =
                @"select ait.ItemTypeId, ait.Name from AdmissionItemType ait where ait.ItemLevel = 8 order by ait.ItemTypeID";
            return SqlQueryHelper.GetRecords(query, null, SqlQueryHelper.MapSelectListItem<short>);
        }

        /// <summary>
        /// Список уровней образования для ПК ОО
        /// </summary>
        /// <param name="institutionId">Id ОО</param>
        private static IEnumerable<SelectListItemViewModel<short>> LoadInstitutionCampaignEducationLevels(
            int institutionId)
        {
            const string query = @"
                select distinct
                    ait.ItemTypeId,
                    ait.Name
                from
                    Campaign cmp
                    inner join CampaignEducationLevel cel on cmp.CampaignId = cel.CampaignId
                    inner join AdmissionItemType ait on cel.EducationLevelId = ait.ItemTypeId
                where 
                    cmp.InstitutionID = @pInstitutionId
                    and ait.ItemLevel = 2
                order by 
                    ait.ItemTypeID";

            return SqlQueryHelper.GetRecords(query, new[] { new SqlParameter("@pInstitutionId", institutionId) },
                SqlQueryHelper.MapSelectListItem<short>);
        }

        /// <summary>
        /// Список курсов для ПК ОО
        /// </summary>
        /// <param name="institutionId">Id ОО</param>
        private static IEnumerable<SelectListItemViewModel<int>> LoadInstitutionOrderCourses(int institutionId)
        {
            const string query = @"
                select distinct
                    cel.Course,
                    convert(varchar(50), cel.Course) Name
                from
                    Campaign cmp
                    inner join CampaignEducationLevel cel on cmp.CampaignId = cel.CampaignId
                where 
                    cmp.InstitutionID = @pInstitutionId
                order 
                    by cel.Course";

            return SqlQueryHelper.GetRecords(query, new[] { new SqlParameter("@pInstitutionId", institutionId) },
                SqlQueryHelper.MapSelectListItem<int>);
        }

        #endregion

        public static ApplicationSelectOrderViewModel GetOrderOfAdmissionRecords(int institutionId, int[] appsId, int orderTypeId,
            ApplicationSelectOrderQueryViewModel queryModel, ConditionForIncudeToOrder cfiToOrder)
        {
            ApplicationSelectOrderFilterViewModel filterData = queryModel.Filter ??
                                                               new ApplicationSelectOrderFilterViewModel();
            PagerViewModel pager = queryModel.Pager ?? new PagerViewModel();
            SortViewModel sortOptions = queryModel.Sort;

            FilterStateManager.Current.Update(filterData);

            int totalUnfiltered = 0;

            if (ConfigHelper.ShowFilterStatistics())
            {
                totalUnfiltered = GetTotalOrdersCountByInstitution(institutionId);
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
            StringBuilder insertTab = new StringBuilder();
            if (cfiToOrder != null)
            {
                if (cfiToOrder.Conditions != null)
                {
                    foreach (var cc in cfiToOrder.Conditions)
                    {
                        insertTab.AppendFormat(@"
INSERT INTO #tab (CompetitiveGroupID, EducationFormID, EducationSourceID, CampaignID, EducationLevelID)
VALUES ({0},{1},{2},{3},{4})",
                            cc.CompetitiveGroupID.HasValue ? cc.CompetitiveGroupID.Value.ToString() : "NULL",
                            cc.EducationFormID.HasValue ? cc.EducationFormID.Value.ToString() : "NULL",
                            cc.EducationSourceID.HasValue ? cc.EducationSourceID.Value.ToString() : "NULL",
                            cc.CampaignID.HasValue ? cc.CampaignID.Value.ToString() : "NULL",
                            cc.EducationLevelID.HasValue ? cc.EducationLevelID.Value.ToString() : "NULL");
                    }
                }
            }
            string rowCountQuery = string.Format(@"

CREATE TABLE #tab
(
	CompetitiveGroupID INT, 
	EducationFormID INT,
	EducationSourceID INT,
	CampaignID INT,
	EducationLevelID INT
)
{0}

SELECT COUNT(*) app_orders
	FROM
	(
		SELECT ooa.OrderId 
		FROM OrderOfAdmission AS ooa (NOLOCK)
			LEFT JOIN AdmissionItemType AS edu_level (NOLOCK) ON ooa.EducationLevelID = edu_level.ItemTypeId
			LEFT JOIN AdmissionItemType AS edu_form (NOLOCK) ON ooa.EducationFormID = edu_form.ItemTypeId
			LEFT JOIN AdmissionItemType AS edu_source (NOLOCK) ON ooa.EducationSourceID = edu_source.ItemTypeId
			INNER JOIN
			(
				SELECT
					CompetitiveGroupID, 
					EducationFormID,
					EducationSourceID,
					CampaignID,
					EducationLevelID
				FROM #tab
			) AS CCG
			ON (CCG.EducationLevelID = ooa.EducationLevelID OR ooa.EducationLevelID IS NULL)
			AND (CCG.EducationFormID = ooa.EducationFormID OR ooa.EducationFormID IS NULL)
			AND (CCG.EducationSourceID = ooa.EducationSourceID OR ooa.EducationSourceID IS NULL)
			AND CCG.CampaignID = ooa.CampaignID	
		WHERE 
            ooa.InstitutionID=@pInstitutionId
			AND ooa.OrderOfAdmissionStatusID NOT IN (3,4) --Не опубликован и не удален
            AND ooa.OrderOfAdmissionTypeID = @orderTypeId
		GROUP BY 
            ooa.OrderId,
			ooa.OrderName,
            ooa.OrderNumber,
			ooa.Stage,
			edu_level.Name, 
			edu_form.Name,
			edu_source.Name 
	) AS CountOrderId

DROP TABLE #tab

", insertTab.ToString());

            string paged = @"
paged AS (
    SELECT t.*, ROW_NUMBER() OVER(ORDER BY {0} {1}) rn
    FROM main t
)
SELECT paged.* from paged where paged.rn between {2} and {3}

DROP TABLE #tab
";

            string pagedQuery = string.Format(@" 
CREATE TABLE #tab
(
	CompetitiveGroupID INT, 
	EducationFormID INT,
	EducationSourceID INT,
	CampaignID INT, 
	EducationLevelID INT
)
{0};

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
			INNER JOIN
			(
				SELECT
					CompetitiveGroupID, 
					EducationFormID,
					EducationSourceID,
					CampaignID, 
					EducationLevelID
				FROM #tab
			) AS CCG
			ON (CCG.EducationLevelID = ooa.EducationLevelID OR ooa.EducationLevelID IS NULL)
			AND (CCG.EducationFormID = ooa.EducationFormID OR ooa.EducationFormID IS NULL)
			AND (CCG.EducationSourceID = ooa.EducationSourceID OR ooa.EducationSourceID IS NULL)
			AND CCG.CampaignID = ooa.CampaignID	
		WHERE 
            ooa.InstitutionID=@pInstitutionId
			AND ooa.OrderOfAdmissionStatusID NOT IN (3,4) --Не опубликован и не удален
            AND ooa.OrderOfAdmissionTypeID = @orderTypeId
		GROUP BY 
            ooa.OrderId 
	) AS CountOrderId
),
main AS
(
	SELECT
		ooa.OrderId,
		ooa.OrderName,
        ooa.OrderNumber,
		ooa.Stage,
		edu_level.Name EducationLevel, 
		edu_form.Name EducationForm,
		edu_source.Name EducationSource,
        CAST (CASE
            WHEN ooa.EducationSourceID = 20 --Квота приема лиц, имеющих особое право
            THEN 1 
            ELSE 0 
        END AS BIT) AS IsForBeneficiary,
		CAST (CASE 
            WHEN c.CampaignTypeID = 5 --Прием по направлениям Минобрнауки
            THEN 1 
            ELSE 0 
        END AS BIT) AS IsForeigner
	FROM OrderOfAdmission AS ooa (NOLOCK)
		LEFT JOIN AdmissionItemType AS edu_level (NOLOCK) ON ooa.EducationLevelID = edu_level.ItemTypeId
		LEFT JOIN AdmissionItemType AS edu_form (NOLOCK) ON ooa.EducationFormID = edu_form.ItemTypeId
		LEFT JOIN AdmissionItemType AS edu_source (NOLOCK) ON ooa.EducationSourceID = edu_source.ItemTypeId
        LEFT JOIN Campaign AS c ON c.CampaignID = ooa.CampaignID        
		INNER JOIN
		(
			SELECT
				CompetitiveGroupID, 
				EducationFormID,
				EducationSourceID,
				CampaignID,
				EducationLevelID
			FROM #tab
		) AS CCG
		ON (CCG.EducationLevelID = ooa.EducationLevelID OR ooa.EducationLevelID IS NULL)		 
		AND (CCG.EducationFormID = ooa.EducationFormID OR ooa.EducationFormID IS NULL)
		AND (CCG.EducationSourceID = ooa.EducationSourceID OR ooa.EducationSourceID IS NULL)
		AND CCG.CampaignID = ooa.CampaignID	
	WHERE 
        ooa.InstitutionID=@pInstitutionId
		AND ooa.OrderOfAdmissionStatusID NOT IN (3,4) --Не опубликован и не удален
        AND ooa.OrderOfAdmissionTypeID = @orderTypeId
	GROUP BY 
        ooa.OrderId,
		ooa.OrderName,
        ooa.OrderNumber,
		ooa.Stage,
		edu_level.Name, 
		edu_form.Name,
		edu_source.Name,
		ooa.EducationSourceID,
		c.CampaignTypeID
),
{1}
", insertTab.ToString(), paged);

            #region Создаем параметры с учетом значений фильтра

            object dbnull = DBNull.Value;
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@pInstitutionId", institutionId));
            parameters.Add(new SqlParameter("@orderTypeId", orderTypeId));
            if (appsId != null)
            {
                if (appsId.Length == 1)
                {
                    parameters.Add(new SqlParameter("@pApplicationId", appsId[0]));
                }
                parameters.Add(new SqlParameter("@CountApp", appsId[0]));
            }
            parameters.Add(new SqlParameter("@pStage", SqlDbType.SmallInt)
            {
                Value = filterData.SelectedStage.HasValue ? Convert.ToInt16(filterData.SelectedStage.Value) : dbnull
            });
            parameters.Add(new SqlParameter("@pOrderName", SqlDbType.VarChar)
            {
                Value = SqlQueryHelper.LikeParamValueOrDBNull(filterData.OrderName)
            });
            parameters.Add(new SqlParameter("@pEducationLevelId", SqlDbType.SmallInt)
            {
                Value =
                    filterData.SelectedEducationLevel.HasValue
                        ? Convert.ToInt16(filterData.SelectedEducationLevel.Value)
                        : dbnull
            });
            parameters.Add(new SqlParameter("@pEducationFormId", SqlDbType.SmallInt)
            {
                Value =
                    filterData.SelectedEducationForm.HasValue
                        ? Convert.ToInt16(filterData.SelectedEducationForm.Value)
                        : dbnull
            });
            parameters.Add(new SqlParameter("@pEducationSourceId", SqlDbType.SmallInt)
            {
                Value =
                    filterData.SelectedEducationSource.HasValue
                        ? Convert.ToInt16(filterData.SelectedEducationSource.Value)
                        : dbnull
            });
              
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
                records = SqlQueryHelper.GetRecords(selectQuery, parameters.CopyToArray(), MapOrderOfAdmissionRecord);
            }

            return new ApplicationSelectOrderViewModel
            {
                ApplicationsId = appsId,
                Filter = filterData,
                Pager = pager,
                SortKey = sortOptions.SortKey,
                SortDescending = sortOptions.SortDescending,
                Records = records,
                TotalOrdersCount = totalUnfiltered
            };
        }

        private static int GetTotalOrdersCountByInstitution(int institutionId)
        {
            const string query =
                @"select count(ord.OrderId) from OrderOfAdmission ord (NOLOCK) where ord.InstitutionId = @pInstitutionId and ord.OrderOfAdmissionStatusID <> 3 and ord.OrderOfAdmissionStatusID <> 4";
            return
                SqlQueryHelper.GetScalarInt(query, new[] { new SqlParameter("@pInstitutionId", institutionId) })
                    .GetValueOrDefault();
        }

        public static ApplicationSelectOrderRecordViewModel MapOrderOfAdmissionRecord(SqlDataReader reader)
        {

            int index = -1;

            ApplicationSelectOrderRecordViewModel asorvModel = new ApplicationSelectOrderRecordViewModel();
            asorvModel.OrderId = reader.GetInt32(++index);
            asorvModel.OrderName = reader.SafeGetString(++index);
            asorvModel.OrderNumber = reader.SafeGetString(++index);
            asorvModel.Stage = reader.SafeGetShortAsString(++index);
            asorvModel.EducationLevel = reader.SafeGetString(++index);
            asorvModel.EducationForm = reader.SafeGetString(++index);
            asorvModel.EducationSource = reader.SafeGetString(++index);
            asorvModel.IsForBeneficiary = reader.GetBoolean(++index);
            asorvModel.IsForeigner = reader.GetBoolean(++index);

            return asorvModel;
        }

        public static void SaveApplicationDisagreeData(int applicationItemId, bool isDisagreed, DateTime? disagreedDate)
        {
            string sql = @"
UPDATE 
    ApplicationCompetitiveGroupItem
SET 
    IsDisagreed = @isDisagreed,
    IsDisagreedDate = @isDisagreedDate
FROM ApplicationCompetitiveGroupItem acgi (NOLOCK)
    INNER JOIN Application a (NOLOCK) ON acgi.ApplicationId = a.ApplicationId
        AND acgi.CompetitiveGroupId = a.OrderCompetitiveGroupID
WHERE
    acgi.id = @applicationItemId";
            SqlQueryHelper.Execute(sql, new SqlParameter[]
            {new SqlParameter("applicationItemId", applicationItemId),
            new SqlParameter("isDisagreed", isDisagreed),
            new SqlParameter("isDisagreedDate", disagreedDate.HasValue?disagreedDate.Value:(object)DBNull.Value)});
        }
    }
}