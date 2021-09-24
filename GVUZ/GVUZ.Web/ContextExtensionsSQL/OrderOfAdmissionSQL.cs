using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using GVUZ.Web.Helpers;
using GVUZ.Web.Infrastructure;
using GVUZ.Web.SQLDB;
using GVUZ.Web.ViewModels.OrderOfAdmission;
using GVUZ.Web.ViewModels.Shared;
using GVUZ.DAL.Dapper.ViewModel.Common;
using GVUZ.DAL.Dapper.Repository.Model;
using GVUZ.Data.Model;

namespace GVUZ.Web.ContextExtensionsSQL
{
    public static partial class OrderOfAdmissionSQL
    {
        #region Фильтр списка приказов о зачислении
        /// <summary>
        /// Загрузка данных для фильтра списка приказов о зачислении
        /// </summary>
        /// <param name="institutionId"></param>
        /// <returns></returns>
        public static OrderOfAdmissionFilterViewModel GetOrderOfAdmissionFilter(int institutionId, int orderTypeId)
        {
            var model = FilterStateManager.Current.GetOrCreate<OrderOfAdmissionFilterViewModel>();
            model.OrderTypeId = orderTypeId;
            model.Campaigns.Items.AddRange(EntrantApplicationSQL.LoadInstitutionCampaigns(institutionId));
            model.EducationForms.Items.AddRange(CacheManager.Current.GetOrCreateItem(CacheKeys.EducationFormsForOrders, LoadEducationForms));
            model.EducationSources.Items.AddRange(CacheManager.Current.GetOrCreateItem(CacheKeys.EducationSourcesForOrders, LoadEducationSources));
            model.EducationLevels.Items.AddRange(LoadInstitutionCampaignEducationLevels(institutionId));
            model.OrderStatuses.Items.AddRange(CacheManager.Current.GetOrCreateItem(CacheKeys.OrderOfAdmissionStatuses, LoadOrderOfAdmissionStatuses));
            return model;
        }

        /// <summary>
        /// Список статусов приказов
        /// </summary>
        public static IEnumerable<SelectListItemViewModel<short>> LoadOrderOfAdmissionStatuses()
        {
            return new OrderOfAdmissionRepository().OrderStatuses_GetAllNotDeleted().Select(x => new SelectListItemViewModel<short>((short)x.Id, x.Name));
        }

        /// <summary>
        /// Список форм обучения
        /// </summary>
        public static IEnumerable<SelectListItemViewModel<short>> LoadEducationForms()
        {
            const string query = @"select ait.ItemTypeId, ait.Name from AdmissionItemType ait where ait.ItemLevel = 7 order by ait.ItemTypeID";
            return SqlQueryHelper.GetRecords(query, null, SqlQueryHelper.MapSelectListItem<short>);
        }

        /// <summary>
        /// Список источников финансирования
        /// </summary>
        private static IEnumerable<SelectListItemViewModel<short>> LoadEducationSources()
        {
            const string query = @"select ait.ItemTypeId, ait.Name from AdmissionItemType ait where ait.ItemLevel = 8 order by ait.ItemTypeID";
            return SqlQueryHelper.GetRecords(query, null, SqlQueryHelper.MapSelectListItem<short>);
        }

        /// <summary>
        /// Список уровней образования для ПК ОО
        /// </summary>
        /// <param name="institutionId">Id ОО</param>
        private static IEnumerable<SelectListItemViewModel<short>> LoadInstitutionCampaignEducationLevels(int institutionId)
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

            return SqlQueryHelper.GetRecords(query, new[] { new SqlParameter("@pInstitutionId", institutionId) }, SqlQueryHelper.MapSelectListItem<short>);
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

            return SqlQueryHelper.GetRecords(query, new[] { new SqlParameter("@pInstitutionId", institutionId) }, SqlQueryHelper.MapSelectListItem<int>);
        }
        #endregion

        #region Список приказов
        public static OrderOfAdmissionListViewModel GetOrderOfAdmissionRecords(int institutionId, OrderOfAdmissionQueryViewModel queryModel)
        {
            if ((queryModel.Filter.OrderTypeId != OrderOfAdmissionType.OrderOfAdmission)
                &&(queryModel.Filter.OrderTypeId != OrderOfAdmissionType.OrderOfAdmissionRefuse))
            {
                queryModel.Filter.OrderTypeId = OrderOfAdmissionType.OrderOfAdmission;
            }

            OrderOfAdmissionFilterViewModel filterData = queryModel.Filter ?? new OrderOfAdmissionFilterViewModel();
            PagerViewModel pager = queryModel.Pager ?? new PagerViewModel();
            SortViewModel sortOptions = queryModel.Sort;

            FilterStateManager.Current.Update(filterData);

            int totalUnfiltered = 0;

            if (ConfigHelper.ShowFilterStatistics())
            {
                totalUnfiltered = GetTotalOrdersCountByInstitution(institutionId, filterData.OrderTypeId, filterData.SelectedOrderStatus);
                if (totalUnfiltered == 0)
                {
                    return new OrderOfAdmissionListViewModel
                    {
                        Pager = new PagerViewModel(),
                        SortDescending = sortOptions.SortDescending,
                        SortKey = sortOptions.SortKey,
                        TotalOrdersCount = totalUnfiltered
                    };
                }
            }

            #region Текст запроса для подсчета количества записей с учетом фильтра

            const string rowCountQuery = @"
                select 
                    count(ord.OrderId)
                from 
                    OrderOfAdmission ord
                    left join Campaign cmp on ord.CampaignId = cmp.CampaignId and cmp.InstitutionId = ord.InstitutionId
                    left join AdmissionItemType edu_level on ord.EducationLevelID = edu_level.ItemTypeId
                    left join AdmissionItemType edu_form on ord.EducationFormID = edu_form.ItemTypeId
                    left join AdmissionItemType edu_source on ord.EducationSourceID = edu_source.ItemTypeId
                where 
                    ord.InstitutionId = @pInstitutionId and ord.OrderOfAdmissionTypeID = @pOrderTypeId 
                     and ((@pOrderStatusId IS NULL and ord.OrderOfAdmissionStatusID <> 4)
                        or (@pOrderStatusId IS NOT NULL and ord.OrderOfAdmissionStatusID = @pOrderStatusId))
                     and (cmp.CampaignId = @pCampaignId or @pCampaignId is null)
                     and (ord.Stage = @pStage or @pStage is null)
                     and (ord.OrderName like(@pOrderName) or @pOrderName is null)
                     and (ord.OrderNumber like(@pOrderNumber) or @pOrderNumber is null)
                     and ((@pOrderDateFrom is not null and @pOrderDateTo is not null and ord.OrderDate between @pOrderDateFrom and @pOrderDateTo) OR
	                      (@pOrderDateFrom is not null and @pOrderDateTo is null and ord.OrderDate >= @pOrderDateFrom) OR
	                      (@pOrderDateFrom is null and @pOrderDateTo is not null and ord.OrderDate <= @pOrderDateTo) OR
	                      (@pOrderDateFrom is null and @pOrderDateTo is null))
                     and ((@pOrderPublishDateFrom is not null and @pOrderPublishDateTo is not null and ord.DatePublished between @pOrderPublishDateFrom and @pOrderPublishDateTo) OR
	                      (@pOrderPublishDateFrom is not null and @pOrderPublishDateTo is null and ord.DatePublished >= @pOrderPublishDateFrom) OR
	                      (@pOrderPublishDateFrom is null and @pOrderPublishDateTo is not null and ord.DatePublished <= @pOrderPublishDateTo) OR
	                      (@pOrderPublishDateFrom is null and @pOrderPublishDateTo is null)) 
                    and (ord.EducationLevelId = @pEducationLevelId or @pEducationLevelId is null)                    
                    and (ord.EducationFormId = @pEducationFormId or @pEducationFormId is null)
                    and (ord.EducationSourceId = @pEducationSourceId or @pEducationSourceId is null)
                    and (ord.IsForBeneficiary = @pIsForBeneficiary or @pIsForBeneficiary is null)
                    and (ord.IsForeigner = @pIsForeigner or @pIsForeigner is null)";

            #endregion

            #region Текст запроса для постраничной выборки записей с учетом фильтра

            string applicationCountSql = null;
            if (filterData.OrderTypeId == OrderOfAdmissionType.OrderOfAdmission)
            {
                applicationCountSql= @"
                   select 
                        acgi.OrderOfAdmissionId as OrderId, 
                        count(acgi.ApplicationId) num_apps
                   from 
                        ApplicationCompetitiveGroupItem acgi
                        inner join Application app on acgi.ApplicationId = app.ApplicationId
                   where 
                        app.StatusId IN (4,8) 
                        and acgi.OrderOfAdmissionID is not null and acgi.OrderOfExceptionID is null
                        and app.InstitutionId = @pInstitutionId
                   group by 
                        acgi.OrderOfAdmissionID
";
            }
            else 
            {
                applicationCountSql = @"
                   select 
                        acgi.OrderOfExceptionId as OrderId, 
                        count(acgi.ApplicationId) num_apps
                   from 
                        ApplicationCompetitiveGroupItem acgi
                        inner join Application app on acgi.ApplicationId = app.ApplicationId
                   where 
                        app.StatusId IN (4,8) 
                        and acgi.OrderOfExceptionId is not null
                        and app.InstitutionId = @pInstitutionId
                   group by 
                        acgi.OrderOfExceptionId
";
            }

            string pagedQuery = @"
                with app_orders as
                (
{4}
                ),
                main as
                (
                    select
                        ord.OrderId,
                        ord.OrderName,
                        ord.OrderNumber,
                        ord.OrderDate,
                        ord.OrderOfAdmissionStatusID OrderStatusId,
                        s.Name OrderStatusName,
                        cmp.Name CampaignName,
                        ord.Stage,
                        edu_level.Name EducationLevel, 
                        edu_form.Name EducationForm,
                        edu_source.Name EducationSource,
                        ISNULL(app_orders.num_apps, 0) NumberOfApplicants,
                        ord.IsForBeneficiary,
                        ord.IsForeigner,
                        cast(case cmp.StatusID when 2 then 1 else 0 end as bit) IsCampaignFinished
                    from 
                        OrderOfAdmission ord
                        left join Campaign cmp on ord.CampaignId = cmp.CampaignId and cmp.InstitutionId = ord.InstitutionId
                        left join AdmissionItemType edu_level on ord.EducationLevelID = edu_level.ItemTypeId
                        left join AdmissionItemType edu_form on ord.EducationFormID = edu_form.ItemTypeId
                        left join AdmissionItemType edu_source on ord.EducationSourceID = edu_source.ItemTypeId
                        left join app_orders on app_orders.OrderId = ord.OrderId
                        inner join  OrderOfAdmissionStatus s on s.OrderOfAdmissionStatusID = ord.OrderOfAdmissionStatusID
                    where 
                        ord.InstitutionId = @pInstitutionId and ord.OrderOfAdmissionTypeID = @pOrderTypeId 
                         and ((@pOrderStatusId IS NULL and ord.OrderOfAdmissionStatusID <> 4)
                            or (@pOrderStatusId IS NOT NULL and ord.OrderOfAdmissionStatusID = @pOrderStatusId))
                         and (cmp.CampaignId = @pCampaignId or @pCampaignId is null)
                         and (ord.Stage = @pStage or @pStage is null)
                         and (ord.OrderName like(@pOrderName) or @pOrderName is null)
                         and (ord.OrderNumber like(@pOrderNumber) or @pOrderNumber is null)
                         and ((@pOrderDateFrom is not null and @pOrderDateTo is not null and ord.OrderDate between @pOrderDateFrom and @pOrderDateTo) OR
	                          (@pOrderDateFrom is not null and @pOrderDateTo is null and ord.OrderDate >= @pOrderDateFrom) OR
	                          (@pOrderDateFrom is null and @pOrderDateTo is not null and ord.OrderDate <= @pOrderDateTo) OR
	                          (@pOrderDateFrom is null and @pOrderDateTo is null))
                         and ((@pOrderPublishDateFrom is not null and @pOrderPublishDateTo is not null and ord.DatePublished between @pOrderPublishDateFrom and @pOrderPublishDateTo) OR
	                          (@pOrderPublishDateFrom is not null and @pOrderPublishDateTo is null and ord.DatePublished >= @pOrderPublishDateFrom) OR
	                          (@pOrderPublishDateFrom is null and @pOrderPublishDateTo is not null and ord.DatePublished <= @pOrderPublishDateTo) OR
	                          (@pOrderPublishDateFrom is null and @pOrderPublishDateTo is null)) 
                        and (ord.EducationLevelId = @pEducationLevelId or @pEducationLevelId is null)
                        and (ord.EducationFormId = @pEducationFormId or @pEducationFormId is null)
                        and (ord.EducationSourceId = @pEducationSourceId or @pEducationSourceId is null)
                        and (ord.IsForBeneficiary = @pIsForBeneficiary or @pIsForBeneficiary is null)
                        and (ord.IsForeigner = @pIsForeigner or @pIsForeigner is null)
                ),
                paged as (
                  select t.*, ROW_NUMBER() OVER(ORDER BY {0} {1}) rn
                  from main t
                )
                select paged.* from paged where paged.rn between {2} and {3}";

            #endregion

            #region Создаем параметры с учетом значений фильтра
            object dbnull = DBNull.Value;
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@pInstitutionId", institutionId));
            parameters.Add(new SqlParameter("@pOrderTypeId", filterData.OrderTypeId));
            parameters.Add(new SqlParameter("@pOrderStatusId", SqlDbType.SmallInt) { Value = filterData.SelectedOrderStatus.HasValue ? Convert.ToInt16(filterData.SelectedOrderStatus.Value) : dbnull });

            parameters.Add(new SqlParameter("@pCampaignId", SqlDbType.Int) { Value = filterData.SelectedCampaign.HasValue ? filterData.SelectedCampaign.Value : dbnull });
            parameters.Add(new SqlParameter("@pStage", SqlDbType.SmallInt) { Value = filterData.SelectedStage.HasValue ? Convert.ToInt16(filterData.SelectedStage.Value) : dbnull });
            parameters.Add(new SqlParameter("@pOrderName", SqlDbType.VarChar) { Value = SqlQueryHelper.LikeParamValueOrDBNull(filterData.OrderName) });
            parameters.Add(new SqlParameter("@pOrderNumber", SqlDbType.VarChar) { Value = SqlQueryHelper.LikeParamValueOrDBNull(filterData.OrderNumber) });
            parameters.Add(new SqlParameter("@pOrderDateFrom", SqlDbType.DateTime) { Value = filterData.OrderDateFrom.HasValue ? filterData.OrderDateFrom.Value : dbnull });
            parameters.Add(new SqlParameter("@pOrderDateTo", SqlDbType.DateTime) { Value = filterData.OrderDateTo.HasValue ? filterData.OrderDateTo.Value : dbnull });
            parameters.Add(new SqlParameter("@pOrderPublishDateFrom", SqlDbType.DateTime) { Value = filterData.OrderPublishDateFrom.HasValue ? filterData.OrderPublishDateFrom.Value : dbnull });
            parameters.Add(new SqlParameter("@pOrderPublishDateTo", SqlDbType.DateTime) { Value = filterData.OrderPublishDateTo.HasValue ? filterData.OrderPublishDateTo.Value : dbnull });
            parameters.Add(new SqlParameter("@pEducationLevelId", SqlDbType.SmallInt) { Value = filterData.SelectedEducationLevel.HasValue ? Convert.ToInt16(filterData.SelectedEducationLevel.Value) : dbnull });
            parameters.Add(new SqlParameter("@pEducationFormId", SqlDbType.SmallInt) { Value = filterData.SelectedEducationForm.HasValue ? Convert.ToInt16(filterData.SelectedEducationForm.Value) : dbnull });
            parameters.Add(new SqlParameter("@pEducationSourceId", SqlDbType.SmallInt) { Value = filterData.SelectedEducationSource.HasValue ? Convert.ToInt16(filterData.SelectedEducationSource.Value) : dbnull });
            parameters.Add(new SqlParameter("@pIsForBeneficiary", SqlDbType.Bit) { Value = filterData.IsForBeneficiary.HasValue ? filterData.IsForBeneficiary.Value : dbnull });
            parameters.Add(new SqlParameter("@pIsForeigner", SqlDbType.Bit) { Value = filterData.IsForeigner.HasValue ? filterData.IsForeigner.Value : dbnull });
            #endregion

            List<OrderOfAdmissionRecordViewModel> records = null;

            pager.TotalRecords = SqlQueryHelper.GetScalarInt(rowCountQuery, parameters.ToArray()).GetValueOrDefault();

            if (pager.TotalRecords > 0)
            {
                string selectQuery = String.Format(
                    pagedQuery, 
                    sortOptions.SortKey, 
                    sortOptions.SortDescending.GetValueOrDefault() ? "DESC" : "ASC", 
                    SqlQueryHelper.PageFirstRecordParameterName, 
                    SqlQueryHelper.PageLastRecordParameterName, 
                    applicationCountSql);
                parameters.AddRange(SqlQueryHelper.CreatePageRangeParameters(pager.FirstRecordOffset, pager.LastRecordOffset));
                records = SqlQueryHelper.GetRecords(selectQuery, parameters.CopyToArray(), MapOrderOfAdmissionRecord);
            }

            return new OrderOfAdmissionListViewModel
                {
                    Filter = filterData,
                    Pager = pager,
                    SortKey = sortOptions.SortKey,
                    SortDescending = sortOptions.SortDescending,
                    Records = records,
                    TotalOrdersCount = totalUnfiltered
                };
        }

        //private void CheckOrderForDoubleAgreements(int institutionID, int OrderID)
        //{

        //}

        private static int GetTotalOrdersCountByInstitution(int institutionId, int orderTypeId, int? orderStatusId)
        {
            const string query = @"
select 
    count(ord.OrderId) 
from 
    OrderOfAdmission ord 
where 
    ord.InstitutionId = @pInstitutionId 
    and ord.OrderOfAdmissionTypeID = @pOrderTypeId 
    and ((@pOrderStatusId IS NULL and ord.OrderOfAdmissionStatusID <> 4)
        or (@pOrderStatusId IS NOT NULL and ord.OrderOfAdmissionStatusID = @pOrderStatusId))";

            return SqlQueryHelper.GetScalarInt(query, new[] 
                {
                    new SqlParameter("@pInstitutionId", institutionId), 
                    new SqlParameter("@pOrderTypeId", orderTypeId),
                    new SqlParameter("@pOrderStatusId", orderStatusId.HasValue?(object)orderStatusId:(object)DBNull.Value) 
                }).GetValueOrDefault();
        }

        private static OrderOfAdmissionRecordViewModel MapOrderOfAdmissionRecord(SqlDataReader reader)
        {
            int index = -1;
            return new OrderOfAdmissionRecordViewModel
                {
                    OrderId = reader.GetInt32(++index),
                    OrderName = reader.SafeGetString(++index),
                    OrderNumber = reader.SafeGetString(++index),
                    OrderDate = reader.SafeGetDateTimeAsString(++index),
                    OrderStatusId = reader.GetInt32(++index),
                    OrderStatusName = reader.SafeGetString(++index),
                    CampaignName = reader.SafeGetString(++index),
                    Stage = reader.SafeGetShortAsString(++index),
                    EducationLevel = reader.SafeGetString(++index),
                    EducationForm = reader.SafeGetString(++index),
                    EducationSource = reader.SafeGetString(++index),
                    NumberOfApplicants = reader.GetInt32(++index),
                    IsForBeneficiary = reader.GetBoolean(++index),
                    IsForeigner = reader.GetBoolean(++index),
                    IsCampaignFinished = reader.SafeGetBool(++index).GetValueOrDefault()
                };
        }
        #endregion

        #region Создание приказа
        public static OrderOfAdmissionCreateViewModel GetOrderOfAdmissionCreateModel(int institutionId, int orderTypeId, int[] applicationId = null)
        {
            var model = new OrderOfAdmissionCreateViewModel();
            model.OrderTypeId = orderTypeId;
            if (applicationId != null && applicationId.Length > 0)
            {
                // создание приказа на основе данных заявлений
                model.FromApplication = true;
                model.ApplicationId = applicationId;

                FillOrderOfAdmissionCreateModelDictionaries(institutionId, model, null);
                if (model.EducationLevels.Items.Count == 1)
                {
                    model.EducationLevelId = model.EducationLevels.Items[0].Id;
                    if (model.EducationForms.Items.Count == 1)
                    {
                        model.EducationFormId = model.EducationForms.Items[0].Id;
                    }
                    if (model.EducationSource.Items.Count == 1)
                    {
                        model.EducationSourceId = model.EducationSource.Items[0].Id;
                    }

                    model.Stages = GetStagesListForCampaign(institutionId, model.CampaignId, model.EducationLevelId, model.EducationFormId, model.EducationSourceId);
                }
            }
            else
            {
                model.Campaigns.Items.AddRange(EntrantApplicationSQL.LoadInstitutionCampaigns(institutionId));
            }

            return model;
        }

        private static IEnumerable<SelectListItemViewModel<int>> LoadCampaignsFromApplicationSource(int institutionId,
                                                                                       IEnumerable<int> applicationId)
        {

            const string selectCampaignsQuery = @"
                select top 1
                 cmp.CampaignId,
                 cmp.Name CampaignName,
                 cmp.StatusID AS CampaignStatusID,
                 cg.IsAdditional
                from 
                  Application app inner join #tmpAppId on app.ApplicationID = #tmpAppId.ApplicationId
                  inner join ApplicationCompetitiveGroupItem acgi on acgi.ApplicationId = app.ApplicationId
                  inner join CompetitiveGroup cg on cg.CompetitiveGroupId = acgi.CompetitiveGroupID
                  inner join Campaign cmp on cmp.CampaignID = cg.CampaignID
                where 
                   app.InstitutionId = @pInstitutionId;
            ";

            applicationId.WriteToTempTable("#tmpAppId", "ApplicationId");
            return SqlQueryHelper.GetRecords(selectCampaignsQuery,
                                      new[] { SqlQueryHelper.CreateIntParam("@pInstitutionId", institutionId) },
                                      SqlQueryHelper.MapSelectListItems<int>);

        }

        public static int CreateOrder(int institutionId, OrderOfAdmissionCreateViewModel model, ModelStateDictionary modelState)
        {
            ValidateOrderNumber(institutionId, model.CampaignId, 0, model.OrderNumber, modelState);
            ValidateOrderUID(institutionId, 0, model.UID, modelState);

            if (modelState.IsValid)
            {
                const string insertQuery = @"
                    insert into [dbo].[OrderOfAdmission](
                        [OrderOfAdmissionStatusID],[OrderOfAdmissionTypeID], [DateCreated], [DateEdited], [InstitutionID], [CampaignID], 
                        [EducationLevelID], [EducationFormID], [EducationSourceID], [Stage], 
                        [IsForBeneficiary], [IsForeigner], [OrderName], [OrderNumber], [OrderDate], [UID])
                    values(
                        @pOrderStatus, @pOrderType, @pDateCreated, @pDateEdited, @pInstitutionId, @pCampaignId, 
                        @pEducationLevelId, @pEducationFormId, @pEducationSourceId, @pStage, 
                        @pIsForBeneficiary, @pIsForeigner, @pOrderName, @pOrderNumber, @pOrderDate, @pUID
                    );
                    select @@IDENTITY";

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlQueryHelper.CreateIntParam("pOrderStatus", 1));
                parameters.Add(SqlQueryHelper.CreateIntParam("pOrderType", model.OrderTypeId));
                parameters.Add(SqlQueryHelper.CreateDateParam("pDateCreated", DateTime.Now));
                parameters.Add(SqlQueryHelper.CreateDateParam("pDateEdited", DateTime.Now));
                parameters.Add(SqlQueryHelper.CreateIntParam("pInstitutionId", institutionId));
                parameters.Add(SqlQueryHelper.CreateIntParam("pCampaignId", model.CampaignId));
                parameters.Add(SqlQueryHelper.CreateShortParam("pEducationLevelId", model.EducationLevelId));
                parameters.Add(SqlQueryHelper.CreateShortParam("pEducationFormId", model.EducationFormId));
                parameters.Add(SqlQueryHelper.CreateShortParam("pEducationSourceId", model.EducationSourceId));
                parameters.Add(SqlQueryHelper.CreateShortParam("pStage", model.Stage));
                parameters.Add(SqlQueryHelper.CreateBoolParam("pIsForBeneficiary", model.IsForBeneficiary.GetValueOrDefault()));
                parameters.Add(SqlQueryHelper.CreateBoolParam("pIsForeigner", model.IsForeigner.GetValueOrDefault()));
                parameters.Add(SqlQueryHelper.CreateStringParam("pOrderName", model.OrderName));
                parameters.Add(SqlQueryHelper.CreateStringParam("pOrderNumber", model.OrderNumber));
                parameters.Add(SqlQueryHelper.CreateDateParam("pOrderDate", model.OrderDate));
                parameters.Add(SqlQueryHelper.CreateStringParam("pUID", model.UID));

                return SqlQueryHelper.GetScalarInt(insertQuery, parameters.ToArray()).GetValueOrDefault();
            }

            FillOrderOfAdmissionCreateModelDictionaries(institutionId, model, null);

            return 0;
        }
        #endregion

        #region Редактирование приказа
        public static OrderOfAdmissionEditViewModel GetOrderOfAdmissionEditModel(int institutionId, int? id)
        {
            if (id.GetValueOrDefault() <= 0)
            {
                return null;
            }

            const string selectQuery = @"
                select TOP 1 
                       ord.[OrderId], 
                       ord.[OrderOfAdmissionTypeID],
                       ord.[OrderOfAdmissionStatusID], 
                       s.Name OrderOfAdmissionStatusName,
                       cmp.[CampaignID],
                       cmp.[StatusID] CampaignStatusID,
                       cmp.[Name] CampaignName, 
                       ord.[EducationLevelID], 
                       edu_levels.[Name] EducationLevelName,
                       ord.[EducationFormID], 
                       edu_forms.[Name] EducationFormName,
                       ord.[EducationSourceID], 
                       edu_sources.[Name] EducationSourceName,
                       ord.[Stage], 
                       ord.[IsForBeneficiary], 
                       ord.[IsForeigner], 
                       ord.[OrderName], 
                       ord.[OrderNumber], 
                       ord.[OrderDate],
                       ord.[DateCreated], 
                       ord.[DateEdited], 
                       ord.[DatePublished],
                       ord.[UID],
                       cast(case cmp.StatusID when 2 then 1 else 0 end as bit) IsCampaignFinished
                 from 
                        [dbo].[OrderOfAdmission] ord  (NOLOCK)
                        left join [dbo].[Campaign] cmp (NOLOCK) on ord.[CampaignID] = cmp.[CampaignID]
                        left join [dbo].[AdmissionItemType] edu_levels on ord.[EducationLevelID] = edu_levels.[ItemTypeID]
                        left join [dbo].[AdmissionItemType] edu_forms on ord.[EducationFormID] = edu_forms.[ItemTypeID]
                        left join [dbo].[AdmissionItemType] edu_sources on ord.[EducationSourceID] = edu_sources.[ItemTypeID]
                        inner join [dbo].OrderOfAdmissionStatus s on s.OrderOfAdmissionStatusID = ord.OrderOfAdmissionStatusID
                where 
                        ord.[OrderId] = @pOrderId 
                        and ord.[InstitutionId] = @pInstitutionId
                        and ord.[OrderOfAdmissionStatusID] <> 4";

            SqlParameter[] parameters = new SqlParameter[]
                {
                    SqlQueryHelper.CreateIntParam("@pInstitutionId", institutionId),
                    SqlQueryHelper.CreateIntParam("@pOrderId", id.GetValueOrDefault())
                };

            var model = SqlQueryHelper.GetRecord(selectQuery, parameters, MapOrderOfAdmissionEditModel);

            if (model != null)
            {
                FillOrderOfAdmissionEditModelDictionaries(institutionId, model);
            }

            return model;
        }

        private static int GetCurrentOrderStatus(int institutionId, int orderId)
        {
            return SqlQueryHelper.GetScalarInt("select top 1 OrderOfAdmissionStatusID from OrderOfAdmission (NOLOCK) where OrderId = @pOrderId and InstitutionId = @pInstitutionId",
                    new[]
                    {
                        new SqlParameter("@pOrderId", orderId), 
                        new SqlParameter("@pInstitutionId", institutionId)
                    }).GetValueOrDefault();
        }

        public static void UpdateOrder(int institutionId, OrderOfAdmissionEditViewModel model, ModelStateDictionary modelState)
        {
            int currentOrderStatus = GetCurrentOrderStatus(institutionId, model.OrderId);

            if (currentOrderStatus < 1 || currentOrderStatus > 3)
            {
                modelState.Remove("IsForBeneficiary");
                modelState.Remove("IsForeigner");
                modelState.Remove("OrderNumber");
                modelState.Remove("CampaignId");
                return;
            }

            // для заявлений в статусе неопубликован или опубликован удаляем обязательные поля валидаторов 
            // в данном случае в базе они не будут обновляться
            if (currentOrderStatus == 2 || currentOrderStatus == 3)
            {
                modelState.Remove("IsForBeneficiary");
                modelState.Remove("IsForeigner");
                modelState.Remove("OrderNumber");
                modelState.Remove("CampaignId");
            }

            // Проверяем уникальность номера только если приказ не содержит заявлений
            // (номер приказа обновляется только в этом случае)
            if (currentOrderStatus == 1)
            {
                ValidateOrderNumber(institutionId, model.CampaignId, model.OrderId, model.OrderNumber, modelState);
            }

            ValidateOrderUID(institutionId, model.OrderId, model.UID, modelState);

            if (modelState.IsValid)
            {
                const string updateQueryNoApplications = @"
                    update [dbo].[OrderOfAdmission] With (Rowlock)
                       set [DateEdited] = @pDateEdited,
                           [CampaignId] = @pCampaignId, 
                           [EducationLevelID] = @pEducationLevelId,
                           [EducationFormID] = @pEducationFormId,
                           [EducationSourceID] = @pEducationSourceId,
                           [Stage] = @pStage,
                            [IsForBeneficiary] = @pIsForBeneficiary,
                            [IsForeigner] = @pIsForeigner,
                            [OrderName] = @pOrderName,
                            [OrderNumber] = @pOrderNumber,
                            [OrderDate] = @pOrderDate,
                            [UID] = @pUID
                            where [InstitutionID] = @pInstitutionID
                                  and [OrderID] = @pOrderId";

                const string updateQueryNotPublished = @"
                    update [dbo].[OrderOfAdmission]
                       set [DateEdited] = @pDateEdited,
                            [OrderName] = @pOrderName,
                            [OrderNumber] = @pOrderNumber,
                            [OrderDate] = @pOrderDate,
                            [DatePublished] = null,
                            [UID] = @pUID
                            where [InstitutionID] = @pInstitutionID
                                  and [OrderID] = @pOrderId";

                const string updateQueryPublished = @"
                    update [dbo].[OrderOfAdmission] with (Rowlock)
                       set [DateEdited] = @pDateEdited,
                            [OrderOfAdmissionStatusID] = @pOrderStatus,
                            [DatePublished] = case @pOrderStatus when 3 then [DatePublished] end,
                            [UID] = @pUID
                            where [InstitutionID] = @pInstitutionID
                                  and [OrderID] = @pOrderId";

                string updateQuery = null;

                List<SqlParameter> parameters = new List<SqlParameter>();

                if (currentOrderStatus == 1)
                {
                    updateQuery = updateQueryNoApplications;
                    parameters.Add(SqlQueryHelper.CreateDateParam("pDateEdited", DateTime.Now));
                    parameters.Add(SqlQueryHelper.CreateIntParam("pCampaignId", model.CampaignId));
                    parameters.Add(SqlQueryHelper.CreateShortParam("pEducationLevelId", model.EducationLevelId));
                    parameters.Add(SqlQueryHelper.CreateShortParam("pEducationFormId", model.EducationFormId));
                    parameters.Add(SqlQueryHelper.CreateShortParam("pEducationSourceId", model.EducationSourceId));
                    parameters.Add(SqlQueryHelper.CreateShortParam("pStage", model.Stage));
                    parameters.Add(SqlQueryHelper.CreateBoolParam("pIsForBeneficiary",
                                                                  model.IsForBeneficiary.GetValueOrDefault()));
                    parameters.Add(SqlQueryHelper.CreateBoolParam("pIsForeigner", model.IsForeigner.GetValueOrDefault()));
                    parameters.Add(SqlQueryHelper.CreateStringParam("pOrderName", model.OrderName));
                    parameters.Add(SqlQueryHelper.CreateStringParam("pOrderNumber", model.OrderNumber));
                    parameters.Add(SqlQueryHelper.CreateDateParam("pOrderDate", model.OrderDate));
                    parameters.Add(SqlQueryHelper.CreateIntParam("pInstitutionId", institutionId));
                    parameters.Add(SqlQueryHelper.CreateIntParam("pOrderId", model.OrderId));
                    parameters.Add(SqlQueryHelper.CreateStringParam("pUID", model.UID));
                }
                if (currentOrderStatus == 2)
                {
                    updateQuery = updateQueryNotPublished;
                    parameters.Add(SqlQueryHelper.CreateDateParam("pDateEdited", DateTime.Now));
                    parameters.Add(SqlQueryHelper.CreateStringParam("pOrderName", model.OrderName));
                    parameters.Add(SqlQueryHelper.CreateStringParam("pOrderNumber", model.OrderNumber));
                    parameters.Add(SqlQueryHelper.CreateDateParam("pOrderDate", model.OrderDate));
                    parameters.Add(SqlQueryHelper.CreateIntParam("pInstitutionId", institutionId));
                    parameters.Add(SqlQueryHelper.CreateIntParam("pOrderId", model.OrderId));
                    parameters.Add(SqlQueryHelper.CreateStringParam("pUID", model.UID));
                }
                if (currentOrderStatus == 3)
                {
                    updateQuery = updateQueryPublished;
                    parameters.Add(SqlQueryHelper.CreateDateParam("pDateEdited", DateTime.Now));
                    parameters.Add(SqlQueryHelper.CreateIntParam("pOrderStatus", model.OrderStatus));
                    parameters.Add(SqlQueryHelper.CreateIntParam("pInstitutionId", institutionId));
                    parameters.Add(SqlQueryHelper.CreateIntParam("pOrderId", model.OrderId));
                    parameters.Add(SqlQueryHelper.CreateStringParam("pUID", model.UID));
                }

                if (updateQuery != null)
                {
                    SqlQueryHelper.Execute(updateQuery, parameters.ToArray());
                }
            }
            else
            {
                FillOrderOfAdmissionCreateModelDictionaries(institutionId, model, null);
                // обновляем состояние статуса и даты публикации независимо от результатов сохранения
                var updatedModel = GetOrderOfAdmissionEditModel(institutionId, model.OrderId);
                model.OrderStatus = updatedModel.OrderStatus;
                model.DateCreated = updatedModel.DateCreated;
                model.DateEdited = updatedModel.DateEdited;
                model.DatePublished = updatedModel.DatePublished;
            }
        }

        private static OrderOfAdmissionEditViewModel MapOrderOfAdmissionEditModel(SqlDataReader reader)
        {
            int index = -1;

            OrderOfAdmissionEditViewModel result = new OrderOfAdmissionEditViewModel
            {
                 OrderId = reader.GetInt32(++index),
                 OrderTypeId = reader.GetInt32(++index),
                 OrderStatus = reader.GetInt32(++index),
                 OrderStatusName = reader.SafeGetString(++index),
                 CampaignId = reader.SafeGetInt(++index),
                 CampaignStatusID = reader.SafeGetInt(++index),
                 CampaignName = reader.SafeGetString(++index),
                 EducationLevelId = reader.SafeGetShort(++index),
                 EducationLevelName = reader.SafeGetString(++index),
                 EducationFormId = reader.SafeGetShort(++index),
                 EducationFormName = reader.SafeGetString(++index),
                 EducationSourceId = reader.SafeGetShort(++index),
                 EducationSourceName = reader.SafeGetString(++index),
                 Stage = reader.SafeGetShort(++index),
                 IsForBeneficiary = reader.SafeGetBool(++index),
                 IsForeigner = reader.SafeGetBool(++index),
                 OrderName = reader.SafeGetString(++index),
                 OrderNumber = reader.SafeGetString(++index),
                 OrderDate = reader.SafeGetDateTime(++index),
                 DateCreated = reader.SafeGetDateTimeAsString(++index),
                 DateEdited = reader.SafeGetDateTimeAsString(++index),
                 DatePublished = reader.SafeGetDateTimeAsString(++index),
                 UID = reader.SafeGetString(++index),
                 IsCampaignFinished = reader.SafeGetBool(++index).GetValueOrDefault()
            };

            return result;
        }
        #endregion

        #region Загрузка данных связанных списков
        public static SelectListViewModel<short> GetEducationLevelListForCampaign(int institutionId, int? campaignId)
        {
            const string query = @"select distinct
                                    ait.ItemTypeID,
                                    ait.Name
                                    from
                                     Campaign cmp (NOLOCK)
                                     inner join CampaignEducationLevel cel on cel.CampaignID = cmp.CampaignID
                                     inner join  AdmissionItemType ait on cel.EducationLevelID = ait.ItemTypeID
                                    where
                                     cmp.InstitutionID = @pInstitutionId 
                                     and cmp.CampaignID = @pCampaignId
                                     and ait.ItemLevel = 2
                                    order by ait.ItemTypeID";

            var model = new SelectListViewModel<short> { ShowUnselectedText = true, UnselectedText = "Не выбрано" };

            if (campaignId.GetValueOrDefault() > 0)
            {
                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@pInstitutionId", institutionId),
                    new SqlParameter("@pCampaignId", campaignId.GetValueOrDefault())
                };


                model.Items.AddRange(SqlQueryHelper.GetRecords(query, parameters, SqlQueryHelper.MapSelectListItem<short>));
            }

            return model;
        }

        public static bool CampaignIsForeigner(int campaignId)
        {
            int? campaignTypeId = SqlQueryHelper.GetScalarInt("select CampaignTypeID from Campaign (NOLOCK) where CampaignID = @id", new SqlParameter[] { new SqlParameter("id", campaignId) });
            return campaignTypeId == CampaignTypes.ForeignersDirection;
        }

        public static SelectListViewModel<short> GetStagesListForCampaign(int institutionId, int? campaignId, short? educationLevelId, short? educationFormId, short? educationSourceId)
        {
            var model = new SelectListViewModel<short> { ShowUnselectedText = true, UnselectedText = "Не выбрано" };

            List<short> educationLevels = new List<short> { 2, 3, 5, 19 };
            List<short> educationForms = new List<short> { 11, 12 };

            if (campaignId.GetValueOrDefault() > 0 && educationLevelId.GetValueOrDefault() > 0 &&
                educationFormId.GetValueOrDefault() > 0 && educationSourceId.GetValueOrDefault() > 0)
            {
                if (educationLevels.Contains(educationLevelId.GetValueOrDefault()) &&
                    educationForms.Contains(educationFormId.GetValueOrDefault()))
                {
                    switch (educationSourceId.GetValueOrDefault())
                    {
                        case 14:
                            model.Add(0, "0");
                            model.Add(1, "1");
                            //model.Add(2, "2");
                            return model;
                        case 16:
                        case 20:
                            model.Add(0, "0");
                            return model;
                        case 15:
                            return model;

                    }
                }
            }

            model.Add(0, "0");
            model.Add(1, "1");
            //model.Add(2, "2");

            return model;
        }

        private static void FillOrderOfAdmissionEditModelDictionaries(int institutionId, OrderOfAdmissionEditViewModel model)
        {
            var options = GetOrderOfAdmissionParameters(institutionId, new OrderOfAdmissionParametersViewModel
            {
                SelectedCampaignId = model.CampaignId,
                SelectedAdditional = model.Additional,
                SelectedCampaignStatusID = model.CampaignStatusID,
                SelectedEducationLevel = model.EducationLevelId.AsIntValue(),
                SelectedEducationForm = model.EducationFormId.AsIntValue(),
                SelectedEducationSource = model.EducationSourceId.AsIntValue(),
                SelectedStage = model.Stage.AsIntValue(),
                FromApplication = model.FromApplication,
                ApplicationId = model.ApplicationId
            });

            FillOrderOfAdmissionCreateModelDictionaries(institutionId, model, options);

            model.OrderStatusList.Items.Clear();
            model.OrderStatusList.Items.AddRange(options.PublicationStatuses.Items); 
        }

        private static void FillOrderOfAdmissionCreateModelDictionaries(int institutionId, OrderOfAdmissionCreateViewModel model, OrderOfAdmissionParametersViewModel options)
        {
            if (options == null)
            {
                options = GetOrderOfAdmissionParameters(institutionId, new OrderOfAdmissionParametersViewModel
                  {
                      SelectedCampaignId = model.CampaignId,
                      SelectedAdditional = model.Additional,
                      SelectedCampaignStatusID = model.CampaignStatusID,
                      SelectedEducationLevel = model.EducationLevelId.AsIntValue(),
                      SelectedEducationForm = model.EducationFormId.AsIntValue(),
                      SelectedEducationSource = model.EducationSourceId.AsIntValue(),
                      SelectedStage = model.Stage.AsIntValue(),
                      FromApplication = model.FromApplication,
                      ApplicationId = model.ApplicationId
                  });
            }

            model.Campaigns.Items.Clear();
            model.Campaigns.Items.AddRange(options.Campaigns.Items);
            model.CampaignId = options.SelectedCampaignId;
            model.Additional = options.SelectedAdditional;
            model.CampaignStatusID = options.SelectedCampaignStatusID;

            model.EducationLevels.Items.Clear();
            model.EducationLevels.Items.AddRange(options.EducationLevels.Items);
            model.EducationLevelId = options.SelectedEducationLevel.AsShortValue();

            model.EducationForms.Items.Clear();
            model.EducationForms.Items.AddRange(options.EducationForms.Items);
            model.EducationFormId = options.SelectedEducationForm.AsShortValue();

            model.EducationSource.Items.Clear();
            model.EducationSource.Items.AddRange(options.EducationSources.Items);
            model.EducationSourceId = options.SelectedEducationSource.AsShortValue();

            model.Stages.Items.Clear();
            model.Stages.Items.AddRange(options.Stages.Items);
            model.Stage = options.SelectedStage.AsShortValue();
        }
        #endregion

        #region Валидация данных при создании/редактировании приказа
        private static void ValidateOrderNumber(int institutionId, int? campaignId, int orderId, string orderNumber, ModelStateDictionary modelState)
        {
            // validate ordernumber
            const string validateOrderNumberQuery = @"
                    select count(*) from [dbo].[OrderOfAdmission] (NOLOCK)
                    where [OrderId] <> @pOrderId and [InstitutionId] = @pInstitutionId and [CampaignId] = @pCampaignId
                    and lower(rtrim(ltrim([OrderNumber]))) = lower(rtrim(ltrim( @pOrderNumber ))) and @pOrderNumber is not null and OrderOfAdmissionStatusID <> 4
                ";

            const string orderNumberViolationMessage = "Регистрационный номер приказа о зачислении должен быть уникальным в рамках приемной кампании";

            if (orderNumber != null)
            {
                SqlParameter[] validationParams = new SqlParameter[]
                        {
                            SqlQueryHelper.CreateIntParam("pOrderId", orderId),
                            SqlQueryHelper.CreateIntParam("pInstitutionId", institutionId),
                            SqlQueryHelper.CreateIntParam("pCampaignId", campaignId),
                            SqlQueryHelper.CreateStringParam("pOrderNumber", orderNumber)
                        };

                if (
                    SqlQueryHelper.GetScalarInt(validateOrderNumberQuery, validationParams.ToArray())
                                  .GetValueOrDefault() != 0)
                {
                    modelState.AddModelError("OrderNumber", orderNumberViolationMessage);
                    modelState.AddModelError(string.Empty, orderNumberViolationMessage);
                }
            }
        }

        private static void ValidateOrderUID(int institutionId, int orderId, string orderUid, ModelStateDictionary modelState)
        {
            const string validateOrderUidQuery = @"
                    select count(*) from [dbo].[OrderOfAdmission]
                    where [OrderId] <> @pOrderId and [InstitutionId] = @pInstitutionId
                    and lower(rtrim(ltrim([UID]))) = lower(rtrim(ltrim( @pOrderUID ))) and @pOrderUID is not null and OrderOfAdmissionStatusID <> 4
                ";

            const string orderUidViolationMessage = "UID приказа о зачислении должен быть уникальным в рамках ОУ";

            if (orderUid != null)
            {
                SqlParameter[] validationParams = new SqlParameter[]
                        {
                            SqlQueryHelper.CreateIntParam("pOrderId", orderId),
                            SqlQueryHelper.CreateIntParam("pInstitutionId", institutionId),
                            SqlQueryHelper.CreateStringParam("pOrderUID", orderUid)
                        };

                if (
                    SqlQueryHelper.GetScalarInt(validateOrderUidQuery, validationParams.ToArray()).GetValueOrDefault() != 0)
                {
                    modelState.AddModelError("UID", orderUidViolationMessage);
                    modelState.AddModelError(string.Empty, orderUidViolationMessage);
                }
            }
        }

        #endregion

        #region Удаление приказа
        public static bool RemoveOrder(int institutionId, int orderId, out string errorMessage)
        {
            int status = GetCurrentOrderStatus(institutionId, orderId);

            if (status == 3)
            {
                errorMessage = "Невозможно удалить опубликованный приказ";
                return false;
            }

            const string checkExistingAppQuery = @"select cast(case when exists (select TOP 1 ApplicationId from Application (NOLOCK) where OrderOfAdmissionId = @pOrderId and InstitutionId = @pInstitutionId) then 1 else 0 end as int)";

            if (1 == SqlQueryHelper.GetScalarInt(checkExistingAppQuery,
                                            new[]
                                                {
                                                    new SqlParameter("@pOrderId", orderId),
                                                    new SqlParameter("@pInstitutionId", institutionId)
                                                })
                              .GetValueOrDefault())
            {
                errorMessage = "Невозможно удалить приказ, в который включены заявления";
                return false;
            }

            SqlQueryHelper.Execute(
                "update OrderOfAdmission set OrderOfAdmissionStatusID = 4 where OrderId = @pOrderId and InstitutionId = @pInstitutionId",
                new[]
                    {
                        new SqlParameter("@pOrderId", orderId),
                        new SqlParameter("@pInstitutionId", institutionId)
                    });

            errorMessage = null;
            return true;
        }
        #endregion

        #region Список заявлений, включенных в приказ

        public static ApplicationOrderListViewModel GetApplicationOrderRecords(int institutionId, int orderId)
        {
            //ApplicationOrderFilterViewModel filterData = queryModel.Filter ?? new ApplicationOrderFilterViewModel();
            //SortViewModel sortOptions = queryModel.Sort;
            //PagerViewModel pager = queryModel.Pager;

            //FilterStateManager.Current.Update(filterData);

            //int totalUnfiltered = 0;

            //if (!forExport)
            //{
            //    if (ConfigHelper.ShowFilterStatistics())
            //    {
            //        //totalUnfiltered = GetTotalApplicationCountByOrder(institutionId, orderId);
            //        //if (totalUnfiltered == 0)
            //        //{
            //            return new ApplicationOrderListViewModel
            //            {
            //                //Pager = new PagerViewModel(),
            //                //SortDescending = sortOptions.SortDescending,
            //                //SortKey = sortOptions.SortKey,
            //                //TotalApplicationOrderCount = totalUnfiltered,
            //                OrderId = orderId
            //            };
            //        //}
            //    }
            //}

            #region Текст запроса для формирования списка заявлений, включенных в приказ
            const string selectQuery = "ftc_GetOrderOfAdmissionApplications";
/*            const string selectQuery = @"
                select  
                    acgi.id,
                    acgi.ApplicationId,
                    ord.OrderId,
                    ord.OrderOfAdmissionStatusID,
                    ord.OrderOfAdmissionTypeID,
                    app.ApplicationNumber,
                    ltrim(ISNULL(ent.LastName, '') + ' ' + ISNULL(ent.FirstName, '') + ' ' + ISNULL(ent.MiddleName, '')) EntrantName,
                    rtrim(ISNULL(doc.DocumentSeries, '') + ' ' + ISNULL(doc.DocumentNumber, '')) IdentityDocument,
                    edu_levels.Name EducationLevelName,
                    edu_forms.Name EducationFormName,
                    edu_sources.Name EducationSourceName,
                    cg.Name CompetitiveGroupName,
                    dir.Name DirectionName,
                    bnf.ShortName Benefit,
                    ISNULL(cgt.Name ,cgt.ContractOrganizationName)+cgt.ContractNumber CompetitiveGroupTargetName,
                    acgi.CalculatedRating Rating,
                    cast(case cmp.StatusID when 2 then 1 else 0 end as bit) IsCampaignFinished,
                    (SELECT 
                        lb.BudgetName 
                    FROM 
                        LevelBudget lb 
                    WHERE 
                        lb.IdLevelBudget = acgi.OrderIdLevelBudget) AS LevelBudgetName,
                    acgi.IsAgreed,
                    acgi.IsAgreedDate,
                    acgi.IsDisagreed,
                    acgi.IsDisagreedDate,

                    (select TOP 1 
                        oa.OrderNumber 
                    from 
                        OrderOfAdmission oa (NOLOCK)                         
                    where oa.OrderOfAdmissionTypeID = 1--Приказ о зачислении
                        and oa.OrderId = acgi.OrderOfAdmissionId
                    ) OrderOfAdmissionInfo 
                from
                    ApplicationCompetitiveGroupItem acgi
                    inner join Application app on app.ApplicationId = acgi.ApplicationId
                    inner join OrderOfAdmission ord 
                        on (acgi.OrderOfAdmissionId = ord.OrderId or acgi.OrderOfExceptionId = ord.OrderId)
                    inner join Campaign cmp on ord.CampaignID = cmp.CampaignID
                    inner join Entrant ent on app.EntrantId = ent.EntrantId
                    inner join EntrantDocument doc on ent.IdentityDocumentId = doc.EntrantDocumentId
                    inner join CompetitiveGroup cg on acgi.CompetitiveGroupID = cg.CompetitiveGroupID
	                inner join AdmissionItemType edu_levels on cg.EducationLevelID = edu_levels.ItemTypeID and edu_levels.ItemLevel = 2
                    inner join AdmissionItemType edu_forms on acgi.EducationFormId = edu_forms.ItemTypeId and edu_forms.ItemLevel = 7
                    inner join AdmissionItemType edu_sources on acgi.EducationSourceId = edu_sources.ItemTypeId and edu_sources.ItemLevel = 8
                    inner join Direction dir on cg.DirectionId = dir.DirectionId
                    left join Benefit bnf on acgi.OrderBenefitID = bnf.BenefitID
                    left join CompetitiveGroupTarget cgt on acgi.CompetitiveGroupTargetId = cgt.CompetitiveGroupTargetId    
                where 
	                app.StatusId IN (4,8)
                    and (acgi.OrderOfAdmissionId = @pOrderId or acgi.OrderOfExceptionId = @pOrderId)
	                and ord.OrderId = @pOrderId 
	                --and (app.ApplicationNumber like(@pApplicationNumber) or @pApplicationNumber is null)
	                --and (ent.LastName like(@pLastName) or @pLastName is null)
	                --and (ent.FirstName like(@pFirstName) or @pFirstName is null)
	                --and (ent.MiddleName like(@pMiddleName) or @pMiddleName is null)
	                --and (doc.DocumentSeries like(@pDocSeries) or @pDocSeries is null)
	                --and (doc.DocumentNumber like(@pDocNumber) or @pDocNumber is null)
	                --and (edu_levels.ItemTypeID = @pEducationLevelId or @pEducationLevelId is null)
	                --and (edu_forms.ItemTypeID = @pEducationFormId or @pEducationFormId is null)
	                --and (edu_sources.ItemTypeID = @pEducationSourceId or @pEducationSourceId is null)
                    --and (cg.CompetitiveGroupId = @pCompetitiveGroupId or @pCompetitiveGroupId is null)
	                --and (dir.Name like(@pDirectionName) or @pDirectionName is null)
	                --and ((@pBenefit < 0 and acgi.OrderBenefitID is null)
		            --     or bnf.BenefitID = @pBenefit
		            --     or @pBenefit is null)
                    and ((ord.OrderOfAdmissionTypeID = 1 and acgi.OrderOfExceptionId is null) or (ord.OrderOfAdmissionTypeID = 2))




";*/
            #endregion

            #region Формирование набора параметров запроса для ApplicationOrderSelectQuery на основе значений пользовательского фильтра
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlQueryHelper.CreateIntParam("institution_id", institutionId));
            parameters.Add(SqlQueryHelper.CreateIntParam("@order_id", orderId));
            //parameters.Add(SqlQueryHelper.CreateIntParam("pInstitutionId", institutionId));
            //parameters.Add(SqlQueryHelper.CreateIntParam("pOrderId", orderId));
            //parameters.Add(SqlQueryHelper.CreateStringLikeParam("pApplicationNumber", filterData.ApplicationNumber));
            //parameters.Add(SqlQueryHelper.CreateStringLikeParam("pLastName", filterData.LastName));
            //parameters.Add(SqlQueryHelper.CreateStringLikeParam("pFirstName", filterData.FirstName));
            //parameters.Add(SqlQueryHelper.CreateStringLikeParam("pMiddleName", filterData.MiddleName));
            //parameters.Add(SqlQueryHelper.CreateStringLikeParam("pDocSeries", filterData.DocumentSeries));
            //parameters.Add(SqlQueryHelper.CreateStringLikeParam("pDocNumber", filterData.DocumentNumber));
            //parameters.Add(SqlQueryHelper.CreateShortParam("pEducationLevelId", filterData.SelectedEducationLevel.AsShortValue()));
            //parameters.Add(SqlQueryHelper.CreateShortParam("pEducationFormId", filterData.SelectedEducationForm.AsShortValue()));
            //parameters.Add(SqlQueryHelper.CreateShortParam("pEducationSourceId", filterData.SelectedEducationSource.AsShortValue()));
            //parameters.Add(SqlQueryHelper.CreateIntParam("pCompetitiveGroupId", filterData.SelectedCompetitiveGroup));
            //parameters.Add(SqlQueryHelper.CreateStringLikeParam("pDirectionName", filterData.DirectionName));
            //parameters.Add(SqlQueryHelper.CreateShortParam("pBenefit", filterData.SelectedBenefit.AsShortValue()));
            #endregion

            //        ? SqlQueryHelper.GetPagedRecords(selectQuery, parameters.ToArray(), MapApplicationOrderRecordViewModel, pager, sortOptions)
            //        : SqlQueryHelper.GetRecords(selectQuery, parameters.ToArray(), MapApplicationOrderRecordViewModel);
            List <ApplicationOrderRecordViewModel> records = SqlQueryHelper.GetRecordsProc(selectQuery, parameters.ToArray(), MapApplicationOrderRecordViewModel);

            return new ApplicationOrderListViewModel
                {
                    //Filter = filterData,
                    //Pager = pager,
                    //SortKey = sortOptions.SortKey,
                    //SortDescending = sortOptions.SortDescending,
                    Records = records,
                    //TotalApplicationOrderCount = totalUnfiltered,
                    OrderId = orderId
                };
        }

        private static ApplicationOrderRecordViewModel MapApplicationOrderRecordViewModel(SqlDataReader reader)
        {
            int index = -1;
            ApplicationOrderRecordViewModel result= new ApplicationOrderRecordViewModel
            {
                    ApplicationItemId = reader.GetInt32(++index),
                    ApplicationId = reader.GetInt32(++index),
                    OrderId = reader.GetInt32(++index),
                    OrderStatus = reader.GetInt32(++index),
                    OrderTypeId = reader.GetInt32(++index),
                    ApplicationNumber = reader.SafeGetString(++index),
                    EntrantName = reader.SafeGetString(++index),
                    IdentityDocument = reader.SafeGetString(++index),
                    EducationLevelName = reader.SafeGetString(++index),
                    EducationFormName = reader.SafeGetString(++index),
                    EducationSourceName = reader.SafeGetString(++index),
                    CompetitiveGroupName = reader.SafeGetString(++index),
                    DirectionName = reader.SafeGetString(++index),
                    Benefit = reader.SafeGetString(++index),
                    CompetitiveGroupTargetName = reader.SafeGetString(++index),
                    Rating = reader.SafeGetDecimal(++index),
                    IsCampaignFinished = reader.SafeGetBool(++index).GetValueOrDefault(),
                    LevelBudgetName = reader.SafeGetString(++index),
                    IsAgreed = reader.SafeGetBool(++index).GetValueOrDefault(),
                    IsAgreedDate = reader.SafeGetDateTime(++index),
                    IsDisagreed = reader.SafeGetBool(++index).GetValueOrDefault(),
                    IsDisagreedDate = reader.SafeGetDateTime(++index),
                    OrderOfAdmissionInfo = reader.SafeGetString(++index),
                };
            result.ApplicationOrderId =String.Format("{0}0{2}{3}0{1}", 
                result.ApplicationItemId,
                result.OrderId,
                result.ApplicationItemId.ToString()[0],
                result.OrderId.ToString()[0]);
            return result ;
        }
        #endregion

        #region Фильтр списка заявлений, включенных в приказ
        public static ApplicationOrderFilterViewModel GetApplicationOrderFilter(int institutionId)
        {
            var model = FilterStateManager.Current.GetOrCreate<ApplicationOrderFilterViewModel>();
            model.EducationLevels.Items.AddRange(LoadInstitutionCampaignEducationLevels(institutionId));
            model.EducationForms.Items.AddRange(CacheManager.Current.GetOrCreateItem(CacheKeys.EducationFormsForOrders, LoadEducationForms));
            model.EducationSources.Items.AddRange(CacheManager.Current.GetOrCreateItem(CacheKeys.EducationSourcesForOrders, LoadEducationSources));
            model.Benefits.Items.AddRange(CacheManager.Current.GetOrCreateItem(CacheKeys.BenefitTypes, EntrantApplicationSQL.LoadBenefitTypes));
            model.CompetitiveGroups.Items.AddRange(LoadCompetitiveGroupsByInstitution(institutionId));

            return model;
        }

        private static IEnumerable<SelectListItemViewModel<int>> LoadCompetitiveGroupsByInstitution(int institutionId)
        {
            const string query = "select cg.CompetitiveGroupID, cg.Name from CompetitiveGroup cg (NOLOCK) where cg.InstitutionID = @pInstitutionId order by cg.Name";

            return SqlQueryHelper.GetRecords(query, new[] { SqlQueryHelper.CreateIntParam("pInstitutionId", institutionId) }, SqlQueryHelper.MapSelectListItem<int>);
        }
        #endregion


        // Экспорт заявлений, включенных в приказ, в формат CSV
        public static void WriteApplicationsCsv(StreamWriter writer, IEnumerable<ApplicationOrderRecordViewModel> records)
        {
            const char separator = ';';
            //0xEF,0xBB,0xBF
            //writer.Write(0xEF);
            //writer.Write(0xBB);
            //writer.Write(0xBF);
            writer.Write("\"Идентификатор\"");
            writer.Write(separator);
            writer.Write("\"Номер заявления\"");
            writer.Write(separator);
            writer.Write("\"ФИО\"");
            writer.Write(separator);
            writer.Write("\"Документ, удостоверяющий личность\"");
            writer.Write(separator);
            writer.Write("\"Уровень образования\"");
            writer.Write(separator);
            writer.Write("\"Форма обучения\"");
            writer.Write(separator);
            writer.Write("\"Источник финансирования\"");
            writer.Write(separator);
            writer.Write("\"Конкурс\"");
            writer.Write(separator);
            writer.Write("\"Направление подготовки / специальность\"");
            writer.Write(separator);
            writer.Write("\"Льгота\"");
            writer.Write(separator);
            writer.Write("\"Организация целевого приема\"");
            writer.Write(separator);
            writer.Write("\"Количество баллов\"");
            writer.Write(separator);
            writer.Write("\"Уровень бюджета\"");
            writer.Write(separator);
            writer.Write("\"Согласие\"");
            writer.Write(separator);
            writer.Write("\"Дата согласия\"");
            writer.Write(separator);
            writer.Write("\"Отказ от согласия\"");
            writer.Write(separator);
            writer.WriteLine("\"Дата отказа\"");

            foreach (var record in records)
            {
                writer.Write(record.ApplicationOrderId);
                writer.Write(separator);
                writer.Write(record.ApplicationNumber.EscapeCsvField());
                writer.Write(separator);
                writer.Write(record.EntrantName.EscapeCsvField());
                writer.Write(separator);
                writer.Write(record.IdentityDocument.EscapeCsvField());
                writer.Write(separator);
                writer.Write(record.EducationLevelName.EscapeCsvField());
                writer.Write(separator);
                writer.Write(record.EducationFormName.EscapeCsvField());
                writer.Write(separator);
                writer.Write(record.EducationSourceName.EscapeCsvField());
                writer.Write(separator);
                writer.Write(record.CompetitiveGroupName.EscapeCsvField());
                writer.Write(separator);
                writer.Write(record.DirectionName.EscapeCsvField());
                writer.Write(separator);
                writer.Write(record.Benefit.EscapeCsvField());
                writer.Write(separator);
                writer.Write(record.CompetitiveGroupTargetName.EscapeCsvField());
                writer.Write(separator);
                writer.Write(record.RatingText.EscapeCsvField());
                writer.Write(separator);
                writer.Write(record.LevelBudgetName.EscapeCsvField());
                writer.Write(separator);
                writer.Write(record.IsAgreed ? "Да" : "Нет");
                writer.Write(separator);
                writer.Write(record.IsAgreedDate.HasValue ? record.IsAgreedDate.Value.ToString("dd.MM.yyyy") : "");
                writer.Write(separator);
                writer.Write(record.IsDisagreed ? "Да" : "Нет");
                writer.Write(separator);
                writer.WriteLine(record.IsDisagreedDate.HasValue ? record.IsDisagreedDate.Value.ToString("dd.MM.yyyy") : "");
            }

        }

        private static int GetTotalApplicationCountByOrder(int institutionId, int orderId)
        {
            const string query = @"
select 
    count(*)
from 
    applicationCompetitiveGroupItem acgi
    inner join application app 
        on acgi.ApplicationId = app.ApplicationId
    inner join OrderOfAdmission ord 
        on (ord.OrderId = acgi.OrderOfAdmissionId 
            or ord.OrderId = acgi.OrderOfExceptionId)
where 
	app.StatusId IN (4,8)
	and ord.OrderId = @pOrderId
	and ord.InstitutionId = @pInstitutionId 
    and ((ord.OrderOfAdmissionTypeID = 1 and acgi.OrderOfExceptionId is null)
        or (ord.OrderOfAdmissionTypeID = 2))
                ";

            SqlParameter[] parameters = new[]
                {
                    SqlQueryHelper.CreateIntParam("pInstitutionId", institutionId),
                    SqlQueryHelper.CreateIntParam("pOrderId", orderId)
                };

            return SqlQueryHelper.GetScalarInt(query, parameters).GetValueOrDefault();
        }

        public static ExcludeApplicationsFromOrderResult ExcludeApplicationsFromOrder(int institutionId, int orderId, int[] applicationItemIds)
        {
            #region Запрос на удаление заявления из приказа о зачислении
            const string excludeFromOrderOfAdmissionSql = @"
update 
    app With (Rowlock)
set 
    app.StatusID = 4,
    app.OrderOfAdmissionID = null, 
    app.OrderCompetitiveGroupID = null, 
    app.OrderCalculatedRating = null, 
    app.OrderCalculatedBenefitID = null, 
    app.OrderIdLevelBudget = null, 
    app.OrderCompetitiveGroupTargetID = null,
    app.OrderEducationFormID = null,
    app.OrderEducationSourceID = null
from 
    Application app
    inner join ApplicationCompetitiveGroupItem acgi (NOLOCK) on acgi.ApplicationId = app.ApplicationId
    inner join #excludableACGIs acgi_ex on acgi.Id = acgi_ex.id

--Обновим историю
update his
set 
    his.ModifiedDate = getdate()
from 
    OrderOfAdmissionHistory his (NOLOCK)
    inner join ApplicationCompetitiveGroupItem acgi (NOLOCK) on acgi.OrderOfAdmissionID = his.OrderID and his.ApplicationId = acgi.ApplicationId
    inner join #excludableACGIs acgi_ex on acgi.id = acgi_ex.Id    
where 
    his.OrderId = @pOrderID 

--Обновим условия приема
update
    acgiToUpdate
set
    acgiToUpdate.OrderOfAdmissionID = NULL, --Стереть ссылку на приказ о зачислении
    acgiToUpdate.AdmissionDate = NULL,
    acgiToUpdate.OrderBenefitID = NULL,
    acgiToUpdate.OrderIdLevelBudget = NULL
from 
    ApplicationCompetitiveGroupItem acgiToUpdate (NOLOCK)
    inner join #excludableACGIs acgi_ex 
        on acgi_ex.Id = acgiToUpdate.id  
where
    acgiToUpdate.OrderOfAdmissionID = @pOrderId
       
update OrderOfAdmission With (Rowlock)
set 
    DateEdited = @now,
    OrderOfAdmissionStatusID 
        = case when 
            (exists 
                (select top 1 
                    ApplicationId 
                from 
                    Application app (NOLOCK) 
                where 
                    app.OrderOfAdmissionID = @pOrderID 
                    and app.StatusID IN (4,8)
                ) 
            )
        then 2 
        else 1 end
where 
    OrderId = @pOrderId and OrderOfAdmissionStatusID in (1, 2)";
            #endregion

            #region Запрос на удаление заявления из приказа об отказе от зачисления
            const string excludeFromOrderOfAdmissionRefuseSql = @" 
declare @orderOfAdmissionIds table(ApplicationItemId int, OrderId int)

--Подготовим связи заявления - старый приказ о зачислении (из которого его перенесли в приказ об отказе)
insert into 
    @orderOfAdmissionIds
    (
        ApplicationItemId
        ,OrderId
    )
select 
    acgi_ex.Id
    ,acgi.OrderOfAdmissionID
from 
    #excludableACGIs acgi_ex
    inner join ApplicationCompetitiveGroupItem acgi (NOLOCK)
        on acgi.id = acgi_ex.id
    where 
        acgi.OrderOfExceptionId = @pOrderId
     
--Обновим заявление (вернем в приказ о зачислении)
update 
    app With (Rowlock)
set 
    app.StatusID = 8,
    app.OrderOfAdmissionID = orderOfAdmissionIds.OrderId
from 
    Application app
    inner join ApplicationCompetitiveGroupItem acgi (NOLOCK) on acgi.ApplicationId = app.ApplicationId
    inner join @orderOfAdmissionIds orderOfAdmissionIds 
        on orderOfAdmissionIds.ApplicationItemId = acgi.id

--Обновим историю
update his With (Rowlock)
set 
    his.ModifiedDate = getdate()
from 
    OrderOfAdmissionHistory his 
    inner join ApplicationCompetitiveGroupItem acgi (NOLOCK) on acgi.OrderOfExceptionID = his.OrderID and his.ApplicationId = acgi.ApplicationId
    inner join @orderOfAdmissionIds orderOfAdmissionIds  on orderOfAdmissionIds.ApplicationItemId = acgi.id
where 
    his.OrderId = @pOrderID 

--Обновим условия приема
update
    acgiToUpdate
set
    acgiToUpdate.OrderOfExceptionID = NULL, --Стереть ссылку на приказ об отказе от зачисления
    acgiToUpdate.ExceptionDate = NULL
from 
    ApplicationCompetitiveGroupItem acgiToUpdate (NOLOCK)
    inner join @orderOfAdmissionIds orderOfAdmissionIds  
        on orderOfAdmissionIds.ApplicationItemId = acgiToUpdate.id 

--Обновим данные текущего приказа об отказе от зачисления
update OrderOfAdmission With (Rowlock)
set 
    DateEdited = @now,
    OrderOfAdmissionStatusID 
        = case when 
            (exists 
                (select top 1 
                    ApplicationId 
                from 
                    Application app 
                where 
                    app.OrderOfAdmissionID = @pOrderID 
                    and app.StatusID IN (4,8)
                ) 
            )
        then 2 
        else 1 end
where 
    OrderId = @pOrderId and OrderOfAdmissionStatusID in (1, 2)";
            #endregion

            //В зависимости от типа приказа процедура удаления из приказа отличается
            string sql = String.Format(@"
declare @now DATETIME = GETDATE()

declare @orderTypeId int

select @orderTypeId = OrderOfAdmissionTypeID from OrderOfAdmission (NOLOCK) where OrderId = @pOrderID
if(@orderTypeId = 1)
begin
    {0}
end
else if (@orderTypeId = 2)
begin
    {1}
end
", excludeFromOrderOfAdmissionSql,excludeFromOrderOfAdmissionRefuseSql);

            int oldStatus = GetCurrentOrderStatus(institutionId, orderId);

            applicationItemIds.WriteToTempTable("#excludableACGIs");

            SqlParameter[] parameters = new[]
                { 
                    SqlQueryHelper.CreateIntParam("pOrderId", orderId)
                };

            int affected = SqlQueryHelper.Execute(sql, parameters);

            int newStatus = GetCurrentOrderStatus(institutionId, orderId);

            return new ExcludeApplicationsFromOrderResult
                {
                    OrderId = orderId,
                    StatusChanged = oldStatus != newStatus,
                    AffectedRows = affected
                };
        }

        public class ExcludeApplicationsFromOrderResult
        {
            public int OrderId { get; set; }
            public bool StatusChanged { get; set; }
            public int AffectedRows { get; set; }
        }

        public static List<string> ValidatePublishAvailable(int institutionId, int orderId, string customRegNumber, DateTime? customRegDate, string orderName, string orderUID)
        {
            const string query = @"select top 1 OrderNumber, OrderDate from OrderOfAdmission (NOLOCK) where OrderId = @pOrderId and InstitutionId = @pInstitutionId";

            List<string> errors = new List<string>();

            SqlParameter[] parameters = new[]
                {
                    SqlQueryHelper.CreateIntParam("pInstitutionId", institutionId),
                    SqlQueryHelper.CreateIntParam("pOrderId", orderId)
                };

            bool emptyNumber = false;
            bool emptyDate = false;

            if (!SqlQueryHelper.SelectOne(query, parameters, reader =>
                {
                    emptyNumber = string.IsNullOrEmpty(reader.SafeGetString(0));
                    emptyDate = reader.IsDBNull(1);
                }))
            {
                errors.Add("Данный приказ не может быть опубликован");
                return errors;
            }

            if (emptyNumber && emptyDate && string.IsNullOrEmpty(customRegNumber) && !customRegDate.HasValue)
            {
                errors.Add("Не указаны регистрационный номер и дата регистрации приказа о зачислении");
            }
            else if (emptyNumber && string.IsNullOrEmpty(customRegNumber))
            {
                errors.Add("Не указан регистрационный номер приказа о зачислении");
            }
            else if (emptyDate && !customRegDate.HasValue)
            {
                errors.Add("Не указана дата регистрации приказа о зачислении");
            }

            return errors;
        }

        public static List<string> PublishOrder(int institutionId, int orderId, string customRegNumber, DateTime? customRegDate, string orderName, string orderUID)
        {
            List<string> errors = ValidatePublishAvailable(institutionId, orderId, customRegNumber, customRegDate, orderName, orderUID);

            if (errors.Count > 0)
            {
                return errors;
            }

            const string query = @"update 
                                     OrderOfAdmission 
                                   set 
                                     OrderOfAdmissionStatusID = 3, 
                                     DatePublished = getdate(), 
                                     OrderNumber = ISNULL(@pOrderNumber, OrderNumber), 
                                     OrderDate = ISNULL(@pOrderDate, OrderDate), 
                                     OrderName = ISNULL(@pOrderName, OrderName),
                                     UID = IsNull(@pUID, UID)
                                   where 
                                         OrderId = @pOrderId 
                                     and InstitutionId = @pInstitutionId";

            SqlParameter[] parameters = new[]
                {
                    SqlQueryHelper.CreateIntParam("pInstitutionId", institutionId),
                    SqlQueryHelper.CreateIntParam("pOrderId", orderId),
                    SqlQueryHelper.CreateStringParam("pOrderNumber", customRegNumber, 128),
                    SqlQueryHelper.CreateDateParam("pOrderDate", customRegDate),
                    SqlQueryHelper.CreateStringParam("pOrderName", orderName, 128),
                    SqlQueryHelper.CreateStringParam("pUID", orderUID)
                };

            if (SqlQueryHelper.Execute(query, parameters) != 1 || GetCurrentOrderStatus(institutionId, orderId) != 3)
            {
                errors.Add("Не удалось опубликовать приказ");
            }

            return errors;
        }

        /// <summary>
        /// Получение параметров приказа
        /// </summary>
        /// <param name="institutionId">ID ОО</param>
        /// <param name="inputModel">Модель данных со значениями параметров, выбранных пользователем</param>
        /// <returns>Обновленная модель с отфильтрованными условиями приема в зависимости от выбора пользователя</returns>
        public static OrderOfAdmissionParametersViewModel GetOrderOfAdmissionParameters(int institutionId, OrderOfAdmissionParametersViewModel inputModel)
        {
            var model = new OrderOfAdmissionParametersViewModel
                {
                    FromApplication = inputModel.FromApplication,
                    ApplicationId = inputModel.ApplicationId
                };

            if (model.FromApplication)
            {
                model.Campaigns.Items.AddRange(LoadCampaignsFromApplicationSource(institutionId, inputModel.ApplicationId));
                if (!inputModel.SelectedCampaignId.HasValue)
                {
                    model.SelectedCampaignId = model.Campaigns.Items.Single().Id;
                    model.SelectedAdditional = false; // model.Campaigns.Items.Single().Additional;
                    model.SelectedCampaignStatusID = inputModel.SelectedCampaignStatusID;
                }
                else
                {
                    model.SelectedCampaignId = inputModel.SelectedCampaignId;
                    model.SelectedAdditional = inputModel.SelectedAdditional;
                    model.SelectedCampaignStatusID = inputModel.SelectedCampaignStatusID;
                }
            }
            else
            {
                model.Campaigns.Items.AddRange(EntrantApplicationSQL.LoadInstitutionCampaigns(institutionId));
                model.SelectedCampaignId = inputModel.SelectedCampaignId;
                model.SelectedAdditional = inputModel.SelectedAdditional;
                model.SelectedCampaignStatusID = inputModel.SelectedCampaignStatusID;
            }

            // если не выбрана ПК, то возвращаем пустую модель с незаполненными комбобоксами условий приема
            if (!model.SelectedCampaignId.HasValue)
            {
                return model;
            }

            if (!model.FromApplication)
            {
                #region Получение параметров приказа при создании на отдельной странице

                // ItemLevel уровень образования = 2, форма обучения = 7, источник финансирования = 8, курс = -1

                #region Текст запроса для получения доступных наборов условий приема

                const string query = @"
                    with tmp as
                    (
	                    select distinct 
                            cmp.CampaignID, 
                            CASE WHEN cmp.EducationFormFlag & 1 > 0 THEN 1 ELSE 0 END AS HasFullTime, 
                            CASE WHEN cmp.EducationFormFlag & 2 > 0 THEN 1 ELSE 0 END AS HasPartTime, 
                            CASE WHEN cmp.EducationFormFlag & 4 > 0 THEN 1 ELSE 0 END AS HasExtramural 
	                    from 
                            Campaign cmp   
	                    where 
                            cmp.InstitutionID = @pInstitutionId
	                        and cmp.CampaignID = @pCampaignId
                    ),
                    conditions AS (
                        select distinct
                            ait.ItemLevel, ait.ItemTypeId, ait.Name
                        from AdmissionItemType ait
                            inner join CampaignEducationLevel cel on cel.EducationLevelID = ait.ItemTypeID
                            inner join tmp on cel.CampaignID = tmp.CampaignID
                        where ait.ItemLevel = 2
                        union all
                        select distinct
                            ait.ItemLevel, ait.ItemTypeId, ait.Name
                        from AdmissionItemType ait
                            inner join tmp 
                                on (ait.ItemTypeID = 11 and tmp.HasFullTime = 1)
                                or (ait.ItemTypeID = 12 and tmp.HasPartTime = 1)
                                or (ait.ItemTypeID = 10 and tmp.HasExtramural = 1)
                        where ait.ItemLevel = 7
                        union all
                        select distinct
                            ait.ItemLevel, ait.ItemTypeId, ait.Name
                        from AdmissionItemType ait, tmp
                        where 
                            ait.ItemLevel = 8 
                        )
                    select t.* from conditions t order by t.ItemLevel, t.ItemTypeId";

                #endregion

                SqlParameter[] parameters = new[]
                    {
                        SqlQueryHelper.CreateIntParam("@pInstitutionId", institutionId),
                        SqlQueryHelper.CreateIntParam("@pCampaignId", inputModel.SelectedCampaignId),
                    };

                SqlQueryHelper.SelectAny(query, parameters,
                                         reader => UpdateOrderOfAdmissionParametersViewModel(reader, model));

                #endregion
            }
            else
            {
                #region Получение параметров при создании приказа по данным одного и более заявлений
                model.ApplicationId.WriteToTempTable("#tmpAppId", "ApplicationId");
                const string selectEducationLevelsByApplication = @"
                    select distinct
                      edu_levels.ItemTypeId EducationLevelId,
                      edu_levels.Name EducationLevelName
                    from
                        Application app (NOLOCK)
                        inner join #tmpAppId on app.ApplicationID = #tmpAppId.ApplicationId
                        inner join ApplicationCompetitiveGroupItem acgi (NOLOCK) on acgi.ApplicationId = app.ApplicationId
                        inner join CompetitiveGroup cg (NOLOCK) on acgi.CompetitiveGroupID = cg.CompetitiveGroupID
					    inner join AdmissionItemType edu_levels on edu_levels.ItemTypeId = cg.EducationLevelId and edu_levels.ItemLevel = 2

                    where app.InstitutionID = @pInstitutionId
		                and ((@pCampaignId is not null and cg.CampaignID = @pCampaignId) or @pCampaignId is null)
                    order by EducationLevelId
                ";

                const string selectEducationFormsByApplication = @"
                    select distinct
                      edu_forms.ItemTypeId EducationFormId,
                      edu_forms.Name EducationFormName
                    from
                        Application app (NOLOCK) 
                        inner join #tmpAppId on app.ApplicationID = #tmpAppId.ApplicationId
                        inner join ApplicationCompetitiveGroupItem acgi (NOLOCK) on acgi.ApplicationId = app.ApplicationId
                        inner join AdmissionItemType edu_forms on edu_forms.ItemTypeId = acgi.EducationFormId and edu_forms.ItemLevel = 7
                        inner join CompetitiveGroup cg (NOLOCK) on acgi.CompetitiveGroupID = cg.CompetitiveGroupID
                    where app.InstitutionID = @pInstitutionId
                        and ((@pSelectedEducationLevel is not null and cg.EducationLevelID = @pSelectedEducationLevel) or @pSelectedEducationLevel is null)
                        and ((@pCampaignId is not null and cg.CampaignID = @pCampaignId) or @pCampaignId is null)
                    order by EducationFormId
                ";

                const string selectEducationSourcesByApplication = @"
                    select distinct
                      edu_sources.ItemTypeId EducationSourceId,
                      edu_sources.Name EducationSourceName
                    from
                        Application app (NOLOCK) 
                        inner join #tmpAppId on app.ApplicationID = #tmpAppId.ApplicationId
                        inner join ApplicationCompetitiveGroupItem acgi (NOLOCK) on acgi.ApplicationId = app.ApplicationId
                        inner join AdmissionItemType edu_sources on edu_sources.ItemTypeId = acgi.EducationSourceId and edu_sources.ItemLevel = 8
                        inner join CompetitiveGroup cg (NOLOCK) on acgi.CompetitiveGroupID = cg.CompetitiveGroupID
                    where app.InstitutionID = @pInstitutionId
                        and ((@pSelectedEducationLevel is not null and cg.EducationLevelID = @pSelectedEducationLevel) or @pSelectedEducationLevel is null)
                        and ((@pCampaignId is not null and cg.CampaignID = @pCampaignId) or @pCampaignId is null)
                    order by EducationSourceId
                ";

                SqlParameter[] parameters = new SqlParameter[]
                    {
                        SqlQueryHelper.CreateIntParam("@pInstitutionId", institutionId),
                        SqlQueryHelper.CreateIntParam("@pCampaignId", model.SelectedCampaignId),
                        SqlQueryHelper.CreateShortParam("@pSelectedEducationLevel", model.SelectedEducationLevel.AsShortValue())
                    };

                model.EducationLevels.Items.AddRange(SqlQueryHelper.GetRecords(selectEducationLevelsByApplication, parameters.CopyToArray(), SqlQueryHelper.MapSelectListItem<short>));
                model.EducationForms.Items.AddRange(SqlQueryHelper.GetRecords(selectEducationFormsByApplication, parameters.CopyToArray(), SqlQueryHelper.MapSelectListItem<short>));
                model.EducationSources.Items.AddRange(SqlQueryHelper.GetRecords(selectEducationSourcesByApplication, parameters.CopyToArray(), SqlQueryHelper.MapSelectListItem<short>));

                #endregion
            }


            if (model.EducationLevels.Items.Any(x => x.Id == inputModel.SelectedEducationLevel.AsShortValue().GetValueOrDefault(-1)))
            {
                model.SelectedEducationLevel = inputModel.SelectedEducationLevel;
            }

            if (model.EducationForms.Items.Any(x => x.Id == inputModel.SelectedEducationForm.AsShortValue().GetValueOrDefault(-1)))
            {
                model.SelectedEducationForm = inputModel.SelectedEducationForm;
            }

            if (model.EducationSources.Items.Any(x => x.Id == inputModel.SelectedEducationSource.AsShortValue().GetValueOrDefault(-1)))
            {
                model.SelectedEducationSource = inputModel.SelectedEducationSource;
            }

            model.Stages = GetStagesListForCampaign(institutionId, model.SelectedCampaignId,
                                                    model.SelectedEducationLevel.AsShortValue(),
                                                    model.SelectedEducationForm.AsShortValue(),
                                                    model.SelectedEducationSource.AsShortValue());

            if (model.Stages.Items.Any(x => x.Id == inputModel.SelectedStage.AsShortValue().GetValueOrDefault(-1)))
            {
                model.SelectedStage = inputModel.SelectedStage;
            }

            string publicationStatusesSql = @"
select 
    OrderOfAdmissionStatusID, Name 
from 
    OrderOfAdmissionStatus 
where 
    OrderOfAdmissionStatusID in (2,3)";
            model.PublicationStatuses.Items.AddRange(SqlQueryHelper.GetRecords(publicationStatusesSql, null, SqlQueryHelper.MapSelectListItem<int>));

            if (model.PublicationStatuses.Items.Any(x => x.Id == inputModel.SelectedStatus.GetValueOrDefault(-1)))
            {
                model.SelectedStatus = inputModel.SelectedStage;
            }

            model.IsForeigner = inputModel.IsForeigner;
            model.IsForBeneficiary = inputModel.IsForBeneficiary;

            if (!model.IsForeigner.HasValue)
            {
                if (model.SelectedCampaignId.HasValue)
                {
                    model.IsForeigner = CampaignIsForeigner(model.SelectedCampaignId.Value);                    
                }
            }
            if (!model.IsForBeneficiary.HasValue)
            {
                if (model.SelectedEducationSource.HasValue)
                {
                    if (model.SelectedEducationSource.Value == 20)
                    {
                        model.IsForBeneficiary = true;
                    }
                    else
                    {
                        model.IsForBeneficiary = false;
                    }
                }
            }

            return model;
        }

        private static void UpdateOrderOfAdmissionParametersViewModel(SqlDataReader reader, OrderOfAdmissionParametersViewModel model)
        {
            short typeId = reader.GetInt16(0);
            short itemId = reader.GetInt16(1);
            string itemName = reader.GetString(2);

            switch (typeId)
            {
                // Уровень образования
                case 2: model.EducationLevels.Add(itemId, itemName);
                    break;
                // Форма обучения
                case 7: model.EducationForms.Add(itemId, itemName);
                    break;
                // Источник финансирования
                case 8: model.EducationSources.Add(itemId, itemName);
                    break;
            }
        }

        /// <summary>
        /// Данные дла просмотра приказа
        /// </summary>
        public static OrderOfAdmissionViewModel GetOrderOfAdmissionViewModel( int  orderId)
        {
            if (orderId== 0)
                return null;

            const string selectQuery = @"
			 declare @maxdate DateTime;
		     declare @checkUID varchar(200); 
	         with maxDateCte as(SELECT 
							     maxDate = Max(CreateDate)

					       FROM [dbo].[DoubleEntrantsOrderHistory]
				    )
	         ,lastCheckCTE as(SELECT 
							    CheckUID
							    ,OrderId
							    ,CreateDate
							    FROM [dbo].[DoubleEntrantsOrderHistory] as DEOH
							    inner join maxDateCte as mdc
							    on DEOH.CreateDate = mdc.maxDate)

				SELECT * INTO #LastCheck FROM lastCheckCTE
				SELECT DISTINCT @checkUID = CheckUID
								,@maxDate = CreateDate

				FROM #LastCheck

               SELECT TOP 1 
                       ord.[OrderId],
                       ord.[OrderOfAdmissionTypeID], 
                       ord.[OrderOfAdmissionStatusID], 
                       s.Name [OrderOfAdmissionStatusName], 
                       cmp.[Name] CampaignName, 
                       edu_levels.[Name] EducationLevelName,
                       edu_forms.[Name] EducationFormName,
                       edu_sources.[Name] EducationSourceName,
                       ord.[Stage], 
                       ord.[IsForBeneficiary], 
                       ord.[IsForeigner], 
                       ord.[OrderName], 
                       ord.[OrderNumber], 
                       ord.[OrderDate],
                       ord.[DateCreated], 
                       ord.[DateEdited], 
                       ord.[DatePublished],
                       ord.[UID],
                       cast(case cmp.StatusID when 2 then 1 else 0 end as bit) IsCampaignFinished
					   ,LastCheckUID =@checkUID
					   ,DEOH.Status
					   ,LastCheckDate=@maxDate
                 from 
                        [dbo].[OrderOfAdmission] ord (NOLOCK)
                        inner join [dbo].[Campaign] cmp (NOLOCK) on ord.[CampaignID] = cmp.[CampaignID]
                        left join [dbo].[AdmissionItemType] edu_levels on ord.[EducationLevelID] = edu_levels.[ItemTypeID]
                        left join [dbo].[AdmissionItemType] edu_forms on ord.[EducationFormID] = edu_forms.[ItemTypeID]
                        left join [dbo].[AdmissionItemType] edu_sources on ord.[EducationSourceID] = edu_sources.[ItemTypeID]
                        inner join [dbo].[OrderOfAdmissionStatus] s on s.OrderOfAdmissionStatusID = ord.OrderOfAdmissionStatusID
						Left JOIN #LastCheck LC ON ord.OrderId =LC.OrderId
						LEFT JOIN DoubleEntrantsOrderHistory as DEOH ON LC.OrderId=DEOH.OrderID AND LC.CreateDate=DEOH.CreateDate
                where 
                        ord.[OrderId] = @pOrderId  
                        and ord.[OrderOfAdmissionStatusID] <> 4
						drop table #LastCheck";

            SqlParameter[] parameters = new SqlParameter[]
                { 
                    SqlQueryHelper.CreateIntParam("@pOrderId", orderId)
                };

            var model = SqlQueryHelper.GetRecord(selectQuery, parameters, MapOrderOfAdmissionViewModel);

            return model;
        }

        private static OrderOfAdmissionViewModel MapOrderOfAdmissionViewModel(SqlDataReader reader)
        {            
            int index = -1; 

            OrderOfAdmissionViewModel result= new OrderOfAdmissionViewModel
            {
                OrderId = reader.GetInt32(++index),
                OrderTypeId = reader.GetInt32(++index),
                OrderStatus = reader.GetInt32(++index),
                OrderStatusName = reader.SafeGetString(++index),
                CampaignName = reader.SafeGetString(++index),
                EducationLevelName = reader.SafeGetString(++index),
                EducationFormName = reader.SafeGetString(++index),
                EducationSourceName = reader.SafeGetString(++index),
                Stage = reader.SafeGetShort(++index),
                IsForBeneficiary = reader.SafeGetBool(++index),
                IsForeigner = reader.SafeGetBool(++index),
                OrderName = reader.SafeGetString(++index),
                OrderNumber = reader.SafeGetString(++index),
                OrderDate = reader.SafeGetDateTime(++index),
                DateCreated = reader.SafeGetDateTimeAsString(++index),
                DateEdited = reader.SafeGetFullDateTimeAsString(++index),
                DatePublished = reader.SafeGetDateTimeAsString(++index),
                UID = reader.SafeGetString(++index),
                IsCampaignFinished = reader.SafeGetBool(++index).GetValueOrDefault(),
                CheckOrderUID = reader.SafeGetString(++index),
                CheckOrderStatus = reader.SafeGetBoolAsString(++index),
                CheckOrderDate = reader.SafeGetFullDateTimeAsString(++index)
            };

            return result;
        }

        //private static OrderOfAdmissionViewModel MapCheckOrderOfAdmissionViewModel(SqlDataReader reader, OrderOfAdmissionViewModel model)
        //{
        //    int index = -1;
        //    model.OrderId = reader.GetInt32(++index);
        //    model.CheckOrderUID = reader.SafeGetString(++index);
        //    model.OrderStatus = reader.GetInt32(++index);
        //    model.DateCreated = reader.SafeGetDateTimeAsString(++index);
                       

        //    return model;
        //}
    }


}