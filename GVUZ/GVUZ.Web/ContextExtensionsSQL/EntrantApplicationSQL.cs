using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web;
using GVUZ.Model.Helpers;
using GVUZ.Web.ContextExtensionsSQL;
using GVUZ.Web.Helpers;
using GVUZ.Web.Infrastructure;
using GVUZ.Web.ViewModels;
using GVUZ.Web.ViewModels.ApplicationsList;
using GVUZ.Web.ViewModels.Shared;
using Newtonsoft.Json;
using GVUZ.DAL.Dapper.ViewModel.Common;
using GVUZ.DAL.Dapper.Repository.Model.Olympics;

namespace GVUZ.Web.SQLDB
{

    public static class EntrantApplicationSQL
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

        #region Поиск записей по закладкам на странице заявлений (выборка)
        /// <summary>
        /// Поиск новых заявлений
        /// </summary>
        /// <param name="institutionId">Id ОО</param>
        /// <param name="queryModel">Модель запроса на выборку данных <see cref="NewApplicationsQueryViewModel"/></param>
        /// <returns>Список новых заявлений <see cref="NewApplicationsListViewModel"/></returns>
        public static NewApplicationsListViewModel GetNewApplicationsRecords(int institutionId, NewApplicationsQueryViewModel queryModel)
        {
            NewApplicationsFilterViewModel filterData = queryModel.Filter ?? new NewApplicationsFilterViewModel();
            PagerViewModel pager = queryModel.Pager ?? new PagerViewModel();
            SortViewModel sortOptions = queryModel.Sort;

            FilterStateManager.Current.Update(filterData);

            int totalUnfiltered = 0;

            if (ConfigHelper.ShowFilterStatistics())
            {
                totalUnfiltered = GetTotalApplicationCountByStatusIdSet(institutionId, 1, 2);

                if (totalUnfiltered == 0)
                {
                    return new NewApplicationsListViewModel
                    {
                        Pager = new PagerViewModel(),
                        SortDescending = sortOptions.SortDescending,
                        SortKey = sortOptions.SortKey,
                        InstitutionId = institutionId,
                        TotalApplicationsCount = totalUnfiltered
                    };
                }
            }


            #region Текст основного запроса для выбора новых заявлений
        //    const string mainQuery = @"
        //            select
	       //             app.ApplicationId,
	       //             app.ApplicationNumber,
	       //             st.Name StatusName,
        //                app.StatusID,
	       //             app.LastCheckDate,
	       //             -- объединяем уникальные названия конкурсных групп для заявления в строку --
  						//(STUFF(
  						//	  (SELECT
  						//			', ' + cg.name from
								//	 ApplicationCompetitiveGroupItem acgi (NOLOCK) 
								//	 inner join CompetitiveGroup cg (NOLOCK) on acgi.CompetitiveGroupId = cg.CompetitiveGroupId
								//				where
								//	  app.ApplicationID = acgi.ApplicationId
							 // FOR XML PATH(''),TYPE).value('.','varchar(8000)'),
  						//	  1, 2, ''
  						//	 )
  						// ) as  CompetitiveGroupNames,
        //                -------------
        //                rtrim(ISNULL(ent.LastName, '') + ' ' + ISNULL(ent.FirstName, '') + ' ' + ISNULL(ent.MiddleName, '')) EntrantName,
        //                ltrim(ISNULL(ent_doc.DocumentSeries, '') + ' ' + ISNULL(ent_doc.DocumentNumber, '')) IdentityDocument,
        //                app.RegistrationDate,
        //                cast(case when exists(select top 1 1 from RecomendedLists (NOLOCK) where applicationid = app.applicationID and institutionId = @pInstitutionId) then 1 else 0 end as bit) IsRecommended,
        //                cast(case when (exists(
	       //                 select top 1 1 from ApplicationCompetitiveGroupItem acgi (NOLOCK) 
	       //                 inner join CompetitiveGroup cg (NOLOCK) on acgi.CompetitiveGroupId = cg.CompetitiveGroupId and acgi.ApplicationId = app.applicationId 
	       //                 inner join Campaign cmp (NOLOCK) on cg.CampaignId = cmp.CampaignId and cmp.InstitutionID = @pInstitutionId
	       //                 where cmp.StatusID = 2
        //                )) then 1 else 0 end as bit) as CampaignIsFinished,
        //                cast(case when st.StatusID = 1 then 1 else 0 end as bit) IsEditable
        //              from 
	       //             Application app (NOLOCK)
	       //             inner join ApplicationStatusType st (NOLOCK) on app.StatusID = st.StatusID
	       //             inner join Entrant ent (NOLOCK) on app.EntrantID = ent.EntrantId
	       //             inner join EntrantDocument ent_doc (NOLOCK) on ent_doc.EntrantDocumentID = ent.IdentityDocumentID
        //            ";
            #endregion

            //#region Собираем фильтр WHERE...
            List<string> filter = new List<string>(); // sql-выражения, по одному для каждой позиции в списке
            List<SqlParameter> parameters = new List<SqlParameter>();

            //filter.Add("st.StatusID IN(1, 2)");

            parameters.Add(filter.FieldEqualsOrNullParamInt("model_id", "model_id", 1));
            parameters.Add(filter.FieldEqualsOrNullParamInt("app.InstitutionId", "institution_id", institutionId));
            parameters.Add(filter.FieldEqualsOrNullParamInt("registrationYear", "year", filterData.CampaignYear));
            parameters.Add(filter.FieldLikeOrNullParam("app.ApplicationNumber", "@app_number", filterData.ApplicationNumber));
            parameters.Add(filter.FieldLikeOrNullParam("ent.LastName", "last_name", filterData.LastName));
            parameters.Add(filter.FieldLikeOrNullParam("ent.FirstName", "first_name", filterData.FirstName));
            parameters.Add(filter.FieldLikeOrNullParam("ent.MiddleName", "middle_name", filterData.MiddleName));
            parameters.AddRange(filter.FieldInDateRangeOrNullParams("app.RegistrationDate", "reg_date_from", filterData.RegistrationDateFrom, "reg_date_to", filterData.RegistrationDateTo));
            parameters.Add(filter.FieldLikeOrNullParam("ent_doc.DocumentSeries", "doc_series", filterData.DocumentSeries));
            parameters.Add(filter.FieldLikeOrNullParam("ent_doc.DocumentNumber", "doc_number", filterData.DocumentNumber));
            parameters.Add(filter.FieldEqualsOrNullParamInt("cg.CompetitiveGroupID", "cmp_grp_id", filterData.SelectedCompetitiveGroup));
            parameters.Add(filter.FieldEqualsOrNullParamInt("st.StatusID", "status_id", 1));
            parameters.Add(filter.FieldEqualsOrNullParamInt("cmp.CampaignID", "campaign_id", filterData.SelectedCampaignId));
            parameters.Add(filter.FieldEqualsOrNullParamInt("order_id", "order_id", null));
            parameters.Add(filter.FieldEqualsOrNullParamInt("edu_form_id", "edu_form_id", filterData.SelectedEducationFormType?.FormId));
            parameters.Add(filter.FieldEqualsOrNullParamInt("src_id", "src_id", filterData.SelectedEducationSourceType?.SourceId));
            parameters.Add(filter.FieldEqualsOrNullParamInt("benefit_id", "benefit_id", filterData.SelectedBenefitId));
            parameters.Add(filter.FieldEqualsOrNullParamBool("orig_received", "orig_received", null));
            parameters.Add(filter.FieldEqualsOrNullParamInt("start_page", "start_page", pager.FirstRecordOffset));
            parameters.Add(filter.FieldEqualsOrNullParamInt("finish_page", "finish_page", pager.LastRecordOffset));
            //parameters.Add(filter.FieldEqualsOrNullParamInt("count_only", "count_only", 0));

            //parameters.Add(filter.FieldEqualsOrNullParamInt("app.InstitutionId", "pInstitutionId", institutionId));
            ////LikeParamValueOrNull для ускорения поиска
            //parameters.Add(filter.LikeParamValueOrNull("app.ApplicationNumber", "pNumber", filterData.ApplicationNumber));
            //parameters.Add(filter.FieldEqualsOrNullParamBool("app.OriginalDocumentsReceived", "pOriginalsReceived", filterData.OriginalDocumentsReceived));
            //parameters.Add(filter.FieldLikeOrNullParam("ent.LastName", "pLastName", filterData.LastName));
            //parameters.Add(filter.FieldLikeOrNullParam("ent.FirstName", "pFirstName", filterData.FirstName));
            //parameters.Add(filter.FieldLikeOrNullParam("ent.MiddleName", "pMiddleName", filterData.MiddleName));
            //parameters.AddRange(filter.FieldInDateRangeOrNullParams("app.RegistrationDate", "pRegDateFrom", filterData.RegistrationDateFrom, "pRegDateTo", filterData.RegistrationDateTo));
            //parameters.Add(filter.FieldLikeOrNullParam("ent_doc.DocumentSeries", "pDocSeries", filterData.DocumentSeries));
            //parameters.Add(filter.FieldLikeOrNullParam("ent_doc.DocumentNumber", "pDocNumber", filterData.DocumentNumber));

            //            // поиск по конкурсным группам
            //            SqlParameter pCompetitiveGroupId = new SqlParameter("@pCompetitiveGroupId", SqlDbType.Int);
            //            parameters.Add(pCompetitiveGroupId);

            //            if (filterData.SelectedCompetitiveGroup.HasValue)
            //            {
            //                // учитываем заявления входящие в конкретную конкурсную группу CompetitiveGroupID = filterData.SelectedCompetitiveGroup
            //                filter.Add(@"exists(select top 1 1 from CompetitiveGroup cg (NOLOCK) inner join ApplicationCompetitiveGroupItem acgi (NOLOCK) on acgi.CompetitiveGroupId = cg.CompetitiveGroupId and acgi.ApplicationId = app.ApplicationId and cg.InstitutionId = @pInstitutionId and cg.CompetitiveGroupId = @pCompetitiveGroupId)");
            //                pCompetitiveGroupId.Value = filterData.SelectedCompetitiveGroup.Value;
            //            }
            //            else
            //            {
            //                // поиск без учета конкурсной группы
            //                filter.Add("@pCompetitiveGroupId is null");
            //                pCompetitiveGroupId.Value = DBNull.Value;
            //            }

            //            // поиск по наличию льгот
            //            SqlParameter pBenefitId = new SqlParameter("@pBenefitId", SqlDbType.Int);
            //            parameters.Add(pBenefitId);

            //            if (filterData.SelectedBenefitId < 0)
            //            {
            //                // нет льгот
            //                filter.Add(@"not exists (select top 1 1
            //from Application a (NOLOCK)
            //left join ApplicationEntranceTestDocument aetd (NOLOCK) on a.ApplicationId = aetd.ApplicationId
            //left join Benefit ben (NOLOCK) on aetd.BenefitID = ben.BenefitID
            //where a.ApplicationId = app.ApplicationId and app.InstitutionId = @pInstitutionId and ben.BenefitId is not null)");
            //                pBenefitId.Value = filterData.SelectedBenefitId.Value;
            //            }
            //            else if (filterData.SelectedBenefitId > 0)
            //            {
            //                // проверяем, что заявление связано с конкретной льготой BenefitId = filterData.SelectedBenefitId
            //                filter.Add(@"(@pBenefitId is not null and exists(select top 1 1
            //from ApplicationEntranceTestDocument aetd (NOLOCK)
            //inner join Benefit ben (NOLOCK) on aetd.BenefitID = ben.BenefitID
            //where aetd.ApplicationId = app.ApplicationId and app.InstitutionId = @pInstitutionId and @pBenefitId = ben.BenefitID))");
            //                pBenefitId.Value = filterData.SelectedBenefitId.Value;
            //            }
            //            else
            //            {
            //                // не важно есть ли льготы
            //                filter.Add("@pBenefitId is null");
            //                pBenefitId.Value = DBNull.Value;
            //            }

            //            // фильтр по приемной кампании
            //            SqlParameter pCampaignId = new SqlParameter("@pCampaignId", SqlDbType.Int);
            //            parameters.Add(pCampaignId);

            //            if (filterData.SelectedCampaignId.HasValue)
            //            {
            //                filter.Add(@"(@pCampaignId is not null and exists(
            //	                        select top 1 1 
            //from ApplicationCompetitiveGroupItem acgi  (NOLOCK)
            //inner join CompetitiveGroup cg (NOLOCK) on acgi.CompetitiveGroupId = cg.CompetitiveGroupId and acgi.ApplicationId = app.applicationId
            //where cg.CampaignID = @pCampaignId
            //                        ))");
            //                pCampaignId.Value = filterData.SelectedCampaignId.Value;
            //            }
            //            else
            //            {
            //                //filter.Add("@pCampaignId is null");
            //                pCampaignId.Value = DBNull.Value;
            //            }

            //            // Фильтр по форме обучения
            //            SqlParameter pFormId = new SqlParameter("@pFormId", SqlDbType.Int);
            //            parameters.Add(pFormId);
            //            if (filterData.SelectedEducationFormType != null)
            //            {
            //                filter.Add("exists(select top 1 1 from ApplicationCompetitiveGroupItem acgi (NOLOCK) where acgi.ApplicationId = app.ApplicationId and acgi.EducationFormId = @pFormId)");
            //                pFormId.Value = filterData.SelectedEducationFormType.FormId;
            //            }
            //            else
            //            {
            //                pFormId.Value = DBNull.Value;
            //            }


            //            // Фильтр по источнику
            //            SqlParameter pSourceId = new SqlParameter("@pSourceId", SqlDbType.Int);
            //            parameters.Add(pSourceId);
            //            if (filterData.SelectedEducationSourceType != null)
            //            {
            //                filter.Add("exists(select top 1 1 from ApplicationCompetitiveGroupItem acgi (NOLOCK) where acgi.ApplicationId = app.ApplicationId and acgi.EducationSourceId = @pSourceId)");
            //                pSourceId.Value = filterData.SelectedEducationSourceType.SourceId;
            //            }
            //            else
            //            {
            //                pSourceId.Value = DBNull.Value;
            //            } 

            //            #endregion

            //            StringBuilder mainQueryBuilder = new StringBuilder();
            //            mainQueryBuilder.AppendLine(mainQuery);
            //            mainQueryBuilder.AppendLine("WHERE");
            //            mainQueryBuilder.AppendLine(filter.JoinAnd());
            //pager.TotalRecords = totalUnfiltered;
            List<NewApplicationsRecordViewModel> records = SqlQueryHelper.GetPagedRecordsNew( parameters.ToArray(), MapNewApplicationListRecord, pager, sortOptions);

            return new NewApplicationsListViewModel
            {
                Pager = pager,
                SortDescending = sortOptions.SortDescending,
                SortKey = sortOptions.SortKey,
                Records = records,
                InstitutionId = institutionId,
                TotalApplicationsCount = totalUnfiltered
            };
        }

        /// <summary>
        /// Поиск заявлений, не прошедших проверку
        /// </summary>
        /// <param name="institutionId">Id ОО</param>
        /// <param name="queryModel">Модель запроса на выборку данных <see cref="UncheckedApplicationsQueryViewModel"/></param>
        /// <returns>Список заявлений, не прошедших проверку <see cref="UncheckedApplicationsListViewModel"/></returns>
        public static UncheckedApplicationsListViewModel GetUncheckedApplicationsRecords(int institutionId, UncheckedApplicationsQueryViewModel queryModel)
        {
            UncheckedApplicationsFilterViewModel filterData = queryModel.Filter ?? new UncheckedApplicationsFilterViewModel();
            PagerViewModel pager = queryModel.Pager ?? new PagerViewModel();
            SortViewModel sortOptions = queryModel.Sort;

            FilterStateManager.Current.Update(filterData);

            int totalUnfiltered = 0;

            if (ConfigHelper.ShowFilterStatistics())
            {
                totalUnfiltered = GetTotalApplicationCountByStatusIdSet(institutionId, 3, 5);

                if (totalUnfiltered == 0)
                {
                    return new UncheckedApplicationsListViewModel
                    {
                        Pager = new PagerViewModel(),
                        SortDescending = sortOptions.SortDescending,
                        SortKey = sortOptions.SortKey,
                        TotalApplicationsCount = totalUnfiltered
                    };
                }
            }


        //    #region Текст основного запроса для выбора заявлений, не прошедших проверку
        //    const string mainQuery = @"
        //            select
	       //             app.ApplicationId,
	       //             app.ApplicationNumber,
	       //             app.ViolationErrors,
	       //             st.Name StatusName,
        //                app.StatusID,
	       //             app.LastCheckDate,
	       //             rtrim(ISNULL(ent.LastName, '') + ' ' + ISNULL(ent.FirstName, '') + ' ' + ISNULL(ent.MiddleName, '')) EntrantName,
	       //             ltrim(ISNULL(ent_doc.DocumentSeries, '') + ' ' + ISNULL(ent_doc.DocumentNumber, '')) IdentityDocument,
	       //             app.RegistrationDate,
	       //             cast(0 as bit) as IsRecommended,
	                    
        //                -- объединяем уникальные названия конкурсных групп для заявления в строку --
  						//(STUFF(
  						//	  (SELECT
  						//			', ' + cg.name from
								//	 ApplicationCompetitiveGroupItem acgi (NOLOCK) 
								//	 inner join CompetitiveGroup cg (NOLOCK) on acgi.CompetitiveGroupId = cg.CompetitiveGroupId
								//				where
								//	  app.ApplicationID = acgi.ApplicationId
							 // FOR XML PATH(''),TYPE).value('.','varchar(8000)'),
  						//	  1, 2, ''
  						//	 )
  						// ) as  CompetitiveGroupNames,
        //                -------------
 
        //                cast(case when (exists(
	       //                 select top 1 1 from ApplicationCompetitiveGroupItem acgi  (NOLOCK)
	       //                 inner join CompetitiveGroup cg (NOLOCK) on acgi.CompetitiveGroupId = cg.CompetitiveGroupId and acgi.ApplicationId = app.applicationId 
	       //                 inner join Campaign cmp (NOLOCK) on cg.CampaignId = cmp.CampaignId and cmp.InstitutionID = @pInstitutionId
	       //                 where cmp.StatusID = 2
        //                )) then 1 else 0 end as bit) as CampaignIsFinished
        //              from 
	       //             Application app (NOLOCK)
	       //             inner join ApplicationStatusType st (NOLOCK) on app.StatusID = st.StatusID
	       //             inner join Entrant ent (NOLOCK) on app.EntrantID = ent.EntrantId
	       //             inner join EntrantDocument ent_doc (NOLOCK) on ent_doc.EntrantDocumentID = ent.IdentityDocumentID
	       //             left join ViolationType vt (NOLOCK) on app.Violationid = vt.ViolationID
        //            ";
        //    #endregion

        //    #region Собираем фильтр WHERE...
            List<string> filter = new List<string>(); // sql-выражения, по одному для каждой позиции в списке
            List<SqlParameter> parameters = new List<SqlParameter>();


            parameters.Add(filter.FieldEqualsOrNullParamInt("model_id", "model_id", 2));
            parameters.Add(filter.FieldEqualsOrNullParamInt("app.InstitutionId", "institution_id", institutionId));
            parameters.Add(filter.FieldEqualsOrNullParamInt("registrationYear", "year", filterData.CampaignYear));
            parameters.Add(filter.FieldLikeOrNullParam("app.ApplicationNumber", "app_number", filterData.ApplicationNumber));
            parameters.Add(filter.FieldLikeOrNullParam("ent.LastName", "last_name", filterData.LastName));
            parameters.Add(filter.FieldLikeOrNullParam("ent.FirstName", "first_name", filterData.FirstName));
            parameters.Add(filter.FieldLikeOrNullParam("ent.MiddleName", "middle_name", filterData.MiddleName));
            parameters.AddRange(filter.FieldInDateRangeOrNullParams("app.RegistrationDate", "reg_date_from", filterData.RegistrationDateFrom, "reg_date_to", filterData.RegistrationDateTo));
            parameters.Add(filter.FieldLikeOrNullParam("ent_doc.DocumentSeries", "doc_series", filterData.DocumentSeries));
            parameters.Add(filter.FieldLikeOrNullParam("ent_doc.DocumentNumber", "doc_number", filterData.DocumentNumber));
            parameters.Add(filter.FieldEqualsOrNullParamInt("cg.CompetitiveGroupID", "cmp_grp_id", filterData.SelectedCompetitiveGroup));
            parameters.Add(filter.FieldEqualsOrNullParamInt("st.StatusID", "status_id", 3));
            parameters.Add(filter.FieldEqualsOrNullParamInt("cmp.CampaignID", "campaign_id", filterData.SelectedCampaignId));
            parameters.Add(filter.FieldEqualsOrNullParamInt("order_id", "order_id", null));
            parameters.Add(filter.FieldEqualsOrNullParamInt("edu_form_id", "edu_form_id", filterData.SelectedEducationFormType?.FormId));
            parameters.Add(filter.FieldEqualsOrNullParamInt("src_id", "src_id", filterData.SelectedEducationSourceType?.SourceId));
            parameters.Add(filter.FieldEqualsOrNullParamInt("benefit_id", "benefit_id", filterData.SelectedBenefitId));
            parameters.Add(filter.FieldEqualsOrNullParamBool("orig_received", "orig_received", null));
            parameters.Add(filter.FieldEqualsOrNullParamInt("start_page", "start_page", pager.FirstRecordOffset));
            parameters.Add(filter.FieldEqualsOrNullParamInt("finish_page", "finish_page", pager.LastRecordOffset));
            //            filter.Add("st.StatusID=3");

            //            parameters.Add(filter.FieldEqualsOrNullParamInt("app.InstitutionId", "pInstitutionId", institutionId));
            //            //LikeParamValueOrNull для ускорения поиска
            //            parameters.Add(filter.LikeParamValueOrNull("app.ApplicationNumber", "pNumber", filterData.ApplicationNumber));
            //            parameters.Add(filter.FieldEqualsOrNullParamInt("vt.ViolationId", "pViolationTypeId", filterData.SelectedViolationType));
            //            parameters.Add(filter.FieldEqualsOrNullParamBool("app.OriginalDocumentsReceived", "pOriginalsReceived", filterData.OriginalDocumentsReceived));
            //            parameters.Add(filter.FieldLikeOrNullParam("ent.LastName", "pLastName", filterData.LastName));
            //            parameters.Add(filter.FieldLikeOrNullParam("ent.FirstName", "pFirstName", filterData.FirstName));
            //            parameters.Add(filter.FieldLikeOrNullParam("ent.MiddleName", "pMiddleName", filterData.MiddleName));
            //            parameters.AddRange(filter.FieldInDateRangeOrNullParams("app.RegistrationDate", "pRegDateFrom", filterData.RegistrationDateFrom, "pRegDateTo", filterData.RegistrationDateTo));
            //            parameters.Add(filter.FieldLikeOrNullParam("ent_doc.DocumentSeries", "pDocSeries", filterData.DocumentSeries));
            //            parameters.Add(filter.FieldLikeOrNullParam("ent_doc.DocumentNumber", "pDocNumber", filterData.DocumentNumber));

            //            // поиск по конкурсным группам
            //            SqlParameter pCompetitiveGroupId = new SqlParameter("@pCompetitiveGroupId", SqlDbType.Int);
            //            parameters.Add(pCompetitiveGroupId);

            //            if (filterData.SelectedCompetitiveGroup.HasValue)
            //            {
            //                // учитываем заявления входящие в конкретную конкурсную группу CompetitiveGroupID = filterData.SelectedCompetitiveGroup
            //                filter.Add(@"exists(select top 1 1 from CompetitiveGroup cg (NOLOCK) inner join ApplicationCompetitiveGroupItem acgi (NOLOCK) on acgi.CompetitiveGroupId = cg.CompetitiveGroupId and acgi.ApplicationId = app.ApplicationId and cg.InstitutionId = @pInstitutionId and cg.CompetitiveGroupId = @pCompetitiveGroupId)");
            //                pCompetitiveGroupId.Value = filterData.SelectedCompetitiveGroup.Value;
            //            }
            //            else
            //            {
            //                // поиск без учета конкурсной группы
            //                filter.Add("@pCompetitiveGroupId is null");
            //                pCompetitiveGroupId.Value = DBNull.Value;
            //            }

            //            // поиск по наличию льгот
            //            SqlParameter pBenefitId = new SqlParameter("@pBenefitId", SqlDbType.Int);
            //            parameters.Add(pBenefitId);

            //            if (filterData.SelectedBenefitId < 0)
            //            {
            //                // нет льгот
            //                filter.Add(@"not exists (select top 1 1
            //from Application a (NOLOCK)
            //left join ApplicationEntranceTestDocument aetd (NOLOCK) on a.ApplicationId = aetd.ApplicationId
            //left join Benefit ben (NOLOCK) on aetd.BenefitID = ben.BenefitID
            //where a.ApplicationId = app.ApplicationId and app.InstitutionId = @pInstitutionId and ben.BenefitId is not null)");
            //                pBenefitId.Value = filterData.SelectedBenefitId.Value;
            //            }
            //            else if (filterData.SelectedBenefitId > 0)
            //            {
            //                // проверяем, что заявление связано с конкретной льготой BenefitId = filterData.SelectedBenefitId
            //                filter.Add(@"(@pBenefitId is not null and exists(select top 1 1
            //from ApplicationEntranceTestDocument aetd (NOLOCK)
            //inner join Benefit ben (NOLOCK) on aetd.BenefitID = ben.BenefitID
            //where aetd.ApplicationId = app.ApplicationId and app.InstitutionId = @pInstitutionId and @pBenefitId = ben.BenefitID))");
            //                pBenefitId.Value = filterData.SelectedBenefitId.Value;
            //            }
            //            else
            //            {
            //                // не важно есть ли льготы
            //                filter.Add("@pBenefitId is null");
            //                pBenefitId.Value = DBNull.Value;
            //            }


            //            // фильтр по приемной кампании
            //            SqlParameter pCampaignId = new SqlParameter("@pCampaignId", SqlDbType.Int);
            //            parameters.Add(pCampaignId);

            //            if (filterData.SelectedCampaignId.HasValue)
            //            {
            //                filter.Add(@"(@pCampaignId is not null and exists(
            //	                        select top 1 1 from ApplicationCompetitiveGroupItem acgi  (NOLOCK)
            //	                        inner join CompetitiveGroup cg (NOLOCK) on acgi.CompetitiveGroupId = cg.CompetitiveGroupId and acgi.ApplicationId = app.applicationId
            //	                        where cg.CampaignID = @pCampaignId
            //                        ))");
            //                pCampaignId.Value = filterData.SelectedCampaignId.Value;
            //            }
            //            else
            //            {
            //                //filter.Add("@pCampaignId is null");
            //                pCampaignId.Value = DBNull.Value;
            //            }

            //            // Фильтр по форме обучения
            //            SqlParameter pFormId = new SqlParameter("@pFormId", SqlDbType.Int);
            //            parameters.Add(pFormId);
            //            if (filterData.SelectedEducationFormType != null)
            //            {
            //                filter.Add("exists(select top 1 1 from ApplicationCompetitiveGroupItem acgi (NOLOCK) where acgi.ApplicationId = app.ApplicationId and acgi.EducationFormId = @pFormId)");
            //                pFormId.Value = filterData.SelectedEducationFormType.FormId;
            //            }
            //            else
            //            {
            //                pFormId.Value = DBNull.Value;
            //            }

            //            // Фильтр по источнику финансирования
            //            SqlParameter pSourceId = new SqlParameter("@pSourceId", SqlDbType.Int);
            //            parameters.Add(pSourceId);
            //            if (filterData.SelectedEducationSourceType != null)
            //            {
            //                filter.Add("exists(select top 1 1 from ApplicationCompetitiveGroupItem acgi (NOLOCK) where acgi.ApplicationId = app.ApplicationId and acgi.EducationSourceId = @pSourceId)");
            //                pSourceId.Value = filterData.SelectedEducationSourceType.SourceId;
            //            }
            //            else
            //            {
            //                pSourceId.Value = DBNull.Value;
            //            } 

            //            #endregion

            //            StringBuilder mainQueryBuilder = new StringBuilder();
            //            mainQueryBuilder.AppendLine(mainQuery);
            //            mainQueryBuilder.AppendLine("WHERE");
            //            mainQueryBuilder.AppendLine(filter.JoinAnd());

            List<UncheckedApplicationsRecordViewModel> records = SqlQueryHelper.GetPagedRecordsNew( parameters.ToArray(), MapUncheckedApplicationListRecord, pager, sortOptions);

            return new UncheckedApplicationsListViewModel
            {
                Pager = pager,
                SortDescending = sortOptions.SortDescending,
                SortKey = sortOptions.SortKey,
                Records = records,
                TotalApplicationsCount = totalUnfiltered
            };
        }

        /// <summary>
        /// Поиск отозванных заявлений
        /// </summary>
        /// <param name="institutionId">Id ОО</param>
        /// <param name="queryModel">Модель запроса на выборку данных <see cref="RevokedApplicationsQueryViewModel"/></param>
        /// <returns>Список отозванных заявлений <see cref="RevokedApplicationsListViewModel"/></returns>
        public static RevokedApplicationsListViewModel GetRevokedApplicationsRecords(int institutionId, RevokedApplicationsQueryViewModel queryModel)
        {
            RevokedApplicationsFilterViewModel filterData = queryModel.Filter ?? new RevokedApplicationsFilterViewModel();
            PagerViewModel pager = queryModel.Pager ?? new PagerViewModel();
            SortViewModel sortOptions = queryModel.Sort;

            FilterStateManager.Current.Update(filterData);

            int totalUnfiltered = 0;

            if (ConfigHelper.ShowFilterStatistics())
            {
                totalUnfiltered = GetTotalApplicationCountByStatusIdSet(institutionId, 6);

                if (totalUnfiltered == 0)
                {
                    return new RevokedApplicationsListViewModel
                    {
                        Pager = new PagerViewModel(),
                        SortDescending = sortOptions.SortDescending,
                        SortKey = sortOptions.SortKey,
                        TotalApplicationsCount = totalUnfiltered
                    };
                }
            }

            #region Текст основного запроса для выбора отозванных заявлений
            const string mainQuery = @"
                    select
	                    app.ApplicationId,
	                    app.ApplicationNumber,
	                    app.LastDenyDate,
                        app.StatusID,
	                    rtrim(ISNULL(ent.LastName, '') + ' ' + ISNULL(ent.FirstName, '') + ' ' + ISNULL(ent.MiddleName, '')) EntrantName,
	                    ltrim(ISNULL(ent_doc.DocumentSeries, '') + ' ' + ISNULL(ent_doc.DocumentNumber, '')) IdentityDocument,
	                    app.RegistrationDate,
	                    cast(case when exists(select top 1 1 from RecomendedLists (NOLOCK) where applicationid = app.applicationID and institutionId = @pInstitutionId) then 1 else 0 end as bit) IsRecommended,
                        cast(case when (exists(
	                        select top 1 1 from ApplicationCompetitiveGroupItem acgi (NOLOCK)
	                        inner join CompetitiveGroup cg (NOLOCK) on acgi.CompetitiveGroupId = cg.CompetitiveGroupId and acgi.ApplicationId = app.applicationId 
	                        inner join Campaign cmp (NOLOCK) on cg.CampaignId = cmp.CampaignId and cmp.InstitutionID = @pInstitutionId
	                        where cmp.StatusID = 2
                        )) then 1 else 0 end as bit) as CampaignIsFinished
                        from 
	                    Application app (NOLOCK)
	                    inner join Entrant ent (NOLOCK) on app.EntrantID = ent.EntrantId
	                    inner join EntrantDocument ent_doc (NOLOCK) on ent_doc.EntrantDocumentID = ent.IdentityDocumentID
                    ";
            #endregion

            #region Собираем фильтр WHERE...
            List<string> filter = new List<string>(); // sql-выражения, по одному для каждой позиции в списке
            List<SqlParameter> parameters = new List<SqlParameter>();

            filter.Add("app.StatusID = 6");

            parameters.Add(filter.FieldEqualsOrNullParamInt("app.InstitutionId", "pInstitutionId", institutionId));
            //LikeParamValueOrNull для ускорения поиска
            parameters.Add(filter.LikeParamValueOrNull("app.ApplicationNumber", "pNumber", filterData.ApplicationNumber));
            parameters.Add(filter.FieldEqualsOrNullParamBool("app.OriginalDocumentsReceived", "pOriginalsReceived", filterData.OriginalDocumentsReceived));
            parameters.Add(filter.FieldLikeOrNullParam("ent.LastName", "pLastName", filterData.LastName));
            parameters.Add(filter.FieldLikeOrNullParam("ent.FirstName", "pFirstName", filterData.FirstName));
            parameters.Add(filter.FieldLikeOrNullParam("ent.MiddleName", "pMiddleName", filterData.MiddleName));
            parameters.AddRange(filter.FieldInDateRangeOrNullParams("app.RegistrationDate", "pRegDateFrom", filterData.RegistrationDateFrom, "pRegDateTo", filterData.RegistrationDateTo));
            parameters.Add(filter.FieldLikeOrNullParam("ent_doc.DocumentSeries", "pDocSeries", filterData.DocumentSeries));
            parameters.Add(filter.FieldLikeOrNullParam("ent_doc.DocumentNumber", "pDocNumber", filterData.DocumentNumber));

            // поиск по конкурсным группам
            SqlParameter pCompetitiveGroupId = new SqlParameter("@pCompetitiveGroupId", SqlDbType.Int);
            parameters.Add(pCompetitiveGroupId);

            if (filterData.SelectedCompetitiveGroup.HasValue)
            {
                // учитываем заявления входящие в конкретную конкурсную группу CompetitiveGroupID = filterData.SelectedCompetitiveGroup
                filter.Add(@"exists(select top 1 1 from CompetitiveGroup cg (NOLOCK) inner join ApplicationCompetitiveGroupItem acgi (NOLOCK) on acgi.CompetitiveGroupId = cg.CompetitiveGroupId and acgi.ApplicationId = app.ApplicationId and cg.InstitutionId = @pInstitutionId and cg.CompetitiveGroupId = @pCompetitiveGroupId)");
                pCompetitiveGroupId.Value = filterData.SelectedCompetitiveGroup.Value;
            }
            else
            {
                // поиск без учета конкурсной группы
                filter.Add("@pCompetitiveGroupId is null");
                pCompetitiveGroupId.Value = DBNull.Value;
            }

            // поиск по наличию льгот
            SqlParameter pBenefitId = new SqlParameter("@pBenefitId", SqlDbType.Int);
            parameters.Add(pBenefitId);

            if (filterData.SelectedBenefitId < 0)
            {
                // нет льгот
                filter.Add(@"not exists (select top 1 1
from Application a (NOLOCK)
left join ApplicationEntranceTestDocument aetd (NOLOCK) on a.ApplicationId = aetd.ApplicationId
left join Benefit ben (NOLOCK) on aetd.BenefitID = ben.BenefitID
where a.ApplicationId = app.ApplicationId and app.InstitutionId = @pInstitutionId and ben.BenefitId is not null)");
                pBenefitId.Value = filterData.SelectedBenefitId.Value;
            }
            else if (filterData.SelectedBenefitId > 0)
            {
                // проверяем, что заявление связано с конкретной льготой BenefitId = filterData.SelectedBenefitId
                filter.Add(@"(@pBenefitId is not null and exists(select top 1 1
from ApplicationEntranceTestDocument aetd (NOLOCK)
inner join Benefit ben (NOLOCK) on aetd.BenefitID = ben.BenefitID
where aetd.ApplicationId = app.ApplicationId and app.InstitutionId = @pInstitutionId and @pBenefitId = ben.BenefitID))");
                pBenefitId.Value = filterData.SelectedBenefitId.Value;
            }
            else
            {
                // не важно есть ли льготы
                filter.Add("@pBenefitId is null");
                pBenefitId.Value = DBNull.Value;
            }


            // фильтр по приемной кампании
            SqlParameter pCampaignId = new SqlParameter("@pCampaignId", SqlDbType.Int);
            parameters.Add(pCampaignId);

            if (filterData.SelectedCampaignId.HasValue)
            {
                filter.Add(@"(@pCampaignId is not null and exists(
	                        select top 1 1 from ApplicationCompetitiveGroupItem acgi  (NOLOCK)
	                        inner join CompetitiveGroup cg (NOLOCK) on acgi.CompetitiveGroupId = cg.CompetitiveGroupId and acgi.ApplicationId = app.applicationId
	                        where cg.CampaignID = @pCampaignId
                        ))");
                pCampaignId.Value = filterData.SelectedCampaignId.Value;
            }
            else
            {
                //filter.Add("@pCampaignId is null");
                pCampaignId.Value = DBNull.Value;
            }

            // Фильтр по форме обучения
            SqlParameter pFormId = new SqlParameter("@pFormId", SqlDbType.Int);
            parameters.Add(pFormId);
            if (filterData.SelectedEducationFormType != null)
            {
                filter.Add("exists(select top 1 1 from ApplicationCompetitiveGroupItem acgi (NOLOCK) where acgi.ApplicationId = app.ApplicationId and acgi.EducationFormId = @pFormId)");
                pFormId.Value = filterData.SelectedEducationFormType.FormId;
            }
            else
            {
                pFormId.Value = DBNull.Value;
            }

            // Фильтр по источнику финансирования
            SqlParameter pSourceId = new SqlParameter("@pSourceId", SqlDbType.Int);
            parameters.Add(pSourceId);
            if (filterData.SelectedEducationSourceType != null)
            {
                filter.Add("exists(select top 1 1 from ApplicationCompetitiveGroupItem acgi (NOLOCK) where acgi.ApplicationId = app.ApplicationId and acgi.EducationSourceId = @pSourceId)");
                pSourceId.Value = filterData.SelectedEducationSourceType.SourceId;
            }
            else
            {
                pSourceId.Value = DBNull.Value;
            } 

            #endregion

            StringBuilder mainQueryBuilder = new StringBuilder();
            mainQueryBuilder.AppendLine(mainQuery);
            mainQueryBuilder.AppendLine("WHERE");
            mainQueryBuilder.AppendLine(filter.JoinAnd());

            List<RevokedApplicationsRecordViewModel> records = SqlQueryHelper.GetPagedRecords(mainQueryBuilder.ToString(), parameters.ToArray(), MapRevokedApplicationListRecord, pager, sortOptions);

            return new RevokedApplicationsListViewModel
            {
                Pager = pager,
                SortDescending = sortOptions.SortDescending,
                SortKey = sortOptions.SortKey,
                Records = records,
                TotalApplicationsCount = totalUnfiltered
            };
        }

        /// <summary>
        /// Поиск принятых заявлений
        /// </summary>
        /// <param name="institutionId">Id ОО</param>
        /// <param name="queryModel">Модель запроса на выборку данных <see cref="AcceptedApplicationsQueryViewModel"/></param>
        /// <returns>Список принятых заявлений<see cref="AcceptedApplicationsListViewModel"/></returns>
        public static AcceptedApplicationsListViewModel GetAcceptedApplicationsRecords(int institutionId, 
            AcceptedApplicationsQueryViewModel queryModel)
        {
            AcceptedApplicationsFilterViewModel filterData = queryModel.Filter ?? new AcceptedApplicationsFilterViewModel();
            PagerViewModel pager = queryModel.Pager ?? new PagerViewModel();
            SortViewModel sortOptions = queryModel.Sort;

            FilterStateManager.Current.Update(filterData);

            int totalUnfiltered = 0;

            if (ConfigHelper.ShowFilterStatistics())
            {
                totalUnfiltered = GetTotalApplicationCountByStatusIdSet(institutionId, 4);

                if (totalUnfiltered == 0)
                {
                    return new AcceptedApplicationsListViewModel
                    {
                        Pager = new PagerViewModel(),
                        SortDescending = sortOptions.SortDescending,
                        SortKey = sortOptions.SortKey,
                        TotalApplicationsCount = totalUnfiltered
                    };
                }
            }

  //          #region Текст основного запроса для выбора принятых заявлений
  //          const string mainQuery = @"
  //      select
  //          app.ApplicationId
		//	, app.ApplicationNumber
		//	, ast.Name AS StatusName
		//	, app.LastCheckDate
		//	, app.StatusID
		//	, RTRIM(ISNULL(ent.LastName, '') + ' ' + ISNULL(ent.FirstName, '') + ' ' + ISNULL(ent.MiddleName, '')) AS EntrantName
		//	, LTRIM(ISNULL(ent_doc.DocumentSeries, '') + ' ' + ISNULL(ent_doc.DocumentNumber, '')) AS IdentityDocument
		//	, app.RegistrationDate
		//	, app.OriginalDocumentsReceived
		//	, app.OrderCalculatedRating AS Rating
		//	, CAST(CASE WHEN rs.ApplicationID  IS NOT NULL THEN 1 ELSE 0 END AS BIT) AS IsRecommended
  //			  			, (STUFF(
  //					(SELECT
  //						', ' + cg.name from
		//					ApplicationCompetitiveGroupItem acgi (NOLOCK) 
		//					inner join CompetitiveGroup cg (NOLOCK) on acgi.CompetitiveGroupId = cg.CompetitiveGroupId
		//							where
		//					app.ApplicationID = acgi.ApplicationId
		//			FOR XML PATH(''),TYPE).value('.','varchar(8000)'),
  //					1, 2, ''
  //					)
  //				) as  CompetitiveGroupNames
		//	, CAST(CASE WHEN t2.ApplicationId IS NOT NULL THEN 1 ELSE 0 END AS BIT) AS CampaignIsFinished
		//	, CAST(CASE WHEN acgi.ApplicationId IS NOT NULL THEN 1 ELSE 0 END AS BIT) AS CanIncludeInRecommended

		//FROM dbo.[Application] AS app WITH (NOLOCK)
		//	JOIN dbo.ApplicationStatusType AS ast WITH (NOLOCK) ON app.StatusID = ast.StatusID
		//	JOIN dbo.Entrant AS ent WITH (NOLOCK) ON app.EntrantID = ent.EntrantID
		//	JOIN dbo.EntrantDocument AS ent_doc WITH (NOLOCK) ON ent.IdentityDocumentID = ent_doc.EntrantDocumentID AND ent.EntrantID = ent_doc.EntrantID
		//	LEFT JOIN (
		//				SELECT ApplicationID, InstitutionID
		//				FROM RecomendedLists WITH (NOLOCK)
		//				GROUP BY ApplicationID, InstitutionID
		//	) rs ON rs.ApplicationID = app.applicationID and rs.InstitutionID = app.InstitutionID
		//	LEFT JOIN (
		//				SELECT acgi.ApplicationId, cmp.InstitutionID
		//				FROM ApplicationCompetitiveGroupItem acgi WITH (NOLOCK)
		//					JOIN CompetitiveGroup cg WITH (NOLOCK) ON acgi.CompetitiveGroupId = cg.CompetitiveGroupId
		//					JOIN Campaign cmp WITH (NOLOCK) ON cg.CampaignId = cmp.CampaignId
		//				WHERE cmp.StatusID = 2
		//				GROUP BY acgi.ApplicationId, cmp.InstitutionID
		//	) t2 ON t2.ApplicationId = app.ApplicationId AND t2.InstitutionID = app.InstitutionID
		//	LEFT JOIN (
		//				SELECT acgi.ApplicationId
		//				FROM ApplicationCompetitiveGroupItem acgi WITH (NOLOCK)
		//					JOIN CompetitiveGroup cg WITH (NOLOCK) ON acgi.CompetitiveGroupId = cg.CompetitiveGroupID 
		//				WHERE cg.EducationLevelID IN (2, 3, 5, 19) 
		//					AND acgi.EducationFormId in (11, 12)
		//					AND acgi.EducationSourceId = 14
		//				GROUP BY acgi.ApplicationId
		//	) acgi ON acgi.ApplicationId = app.ApplicationId
  //                  ";
  //          #endregion

            //#region Собираем фильтр WHERE...
            List<string> filter = new List<string>();
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(filter.FieldEqualsOrNullParamInt("model_id", "model_id", 4));
            parameters.Add(filter.FieldEqualsOrNullParamInt("app.InstitutionId", "institution_id", institutionId));
            parameters.Add(filter.FieldEqualsOrNullParamInt("registrationYear", "year", filterData.CampaignYear));
            parameters.Add(filter.FieldLikeOrNullParam("app.ApplicationNumber", "app_number", filterData.ApplicationNumber));
            parameters.Add(filter.FieldLikeOrNullParam("ent.LastName", "last_name", filterData.LastName));
            parameters.Add(filter.FieldLikeOrNullParam("ent.FirstName", "first_name", filterData.FirstName));
            parameters.Add(filter.FieldLikeOrNullParam("ent.MiddleName", "middle_name", filterData.MiddleName));
            parameters.AddRange(filter.FieldInDateRangeOrNullParams("app.RegistrationDate", "reg_date_from", filterData.RegistrationDateFrom, "reg_date_to", filterData.RegistrationDateTo));
            parameters.Add(filter.FieldLikeOrNullParam("ent_doc.DocumentSeries", "doc_series", filterData.DocumentSeries));
            parameters.Add(filter.FieldLikeOrNullParam("ent_doc.DocumentNumber", "doc_number", filterData.DocumentNumber));
            parameters.Add(filter.FieldEqualsOrNullParamInt("cg.CompetitiveGroupID", "cmp_grp_id", filterData.SelectedCompetitiveGroup));
            parameters.Add(filter.FieldEqualsOrNullParamInt("st.StatusID", "status_id", 4));
            parameters.Add(filter.FieldEqualsOrNullParamInt("cmp.CampaignID", "campaign_id", filterData.SelectedCampaignId));
            parameters.Add(filter.FieldEqualsOrNullParamInt("order_id", "order_id", null));
            parameters.Add(filter.FieldEqualsOrNullParamInt("edu_form_id", "edu_form_id", filterData.SelectedEducationFormType?.FormId));
            parameters.Add(filter.FieldEqualsOrNullParamInt("src_id", "src_id", filterData.SelectedEducationSourceType?.SourceId));
            parameters.Add(filter.FieldEqualsOrNullParamInt("benefit_id", "benefit_id", filterData.SelectedBenefitId));
            parameters.Add(filter.FieldEqualsOrNullParamBool("orig_received", "orig_received", null));
            parameters.Add(filter.FieldEqualsOrNullParamInt("start_page", "start_page", pager.FirstRecordOffset));
            parameters.Add(filter.FieldEqualsOrNullParamInt("finish_page", "finish_page", pager.LastRecordOffset));

            //            parameters.Add(filter.FieldEqualsOrNullParamInt("app.StatusID", "pStatusID", 4));
            //            parameters.Add(filter.FieldEqualsOrNullParamInt("app.InstitutionId", "pInstitutionId", institutionId));
            //            if (filterData.ApplicationNumber != null)
            //            {             //LikeParamValueOrNull для ускорения поиска
            //                parameters.Add(filter.LikeParamValueOrNull("app.ApplicationNumber", "pNumber", filterData.ApplicationNumber));
            //            }
            //            if (filterData.OriginalDocumentsReceived != null) { parameters.Add(filter.FieldEqualsOrNullParamBool("app.OriginalDocumentsReceived", "pOriginalsReceived", filterData.OriginalDocumentsReceived)); }
            //            if (filterData.LastName != null) { parameters.Add(filter.FieldLikeOrNullParam("ent.LastName", "pLastName", filterData.LastName)); }
            //            if (filterData.FirstName != null) { parameters.Add(filter.FieldLikeOrNullParam("ent.FirstName", "pFirstName", filterData.FirstName)); }
            //            if (filterData.MiddleName != null) { parameters.Add(filter.FieldLikeOrNullParam("ent.MiddleName", "pMiddleName", filterData.MiddleName)); }
            //            if ((filterData.RegistrationDateFrom != null) || (filterData.RegistrationDateTo != null)) { parameters.AddRange(filter.FieldInDateRangeOrNullParams("app.RegistrationDate", "pRegDateFrom", filterData.RegistrationDateFrom, "pRegDateTo", filterData.RegistrationDateTo)); }
            //            if (filterData.DocumentSeries != null) { parameters.Add(filter.FieldLikeOrNullParam("ent_doc.DocumentSeries", "pDocSeries", filterData.DocumentSeries)); }
            //            if (filterData.DocumentNumber != null) { parameters.Add(filter.FieldLikeOrNullParam("ent_doc.DocumentNumber", "pDocNumber", filterData.DocumentNumber)); }

            //            // поиск по конкурсным группам
            //            SqlParameter pCompetitiveGroupId = new SqlParameter("@pCompetitiveGroupId", SqlDbType.Int);
            //            parameters.Add(pCompetitiveGroupId);

            //            if (filterData.SelectedCompetitiveGroup.HasValue)
            //            {
            //                // учитываем заявления входящие в конкретную конкурсную группу CompetitiveGroupID = filterData.SelectedCompetitiveGroup
            //                filter.Add(@"exists(select top 1 1 from CompetitiveGroup cg (NOLOCK) inner join ApplicationCompetitiveGroupItem acgi (NOLOCK) on acgi.CompetitiveGroupId = cg.CompetitiveGroupId and acgi.ApplicationId = app.ApplicationId and cg.InstitutionId = @pInstitutionId and cg.CompetitiveGroupId = @pCompetitiveGroupId)");
            //                pCompetitiveGroupId.Value = filterData.SelectedCompetitiveGroup.Value;
            //            }
            //            else
            //            {
            //                // поиск без учета конкурсной группы
            //                //filter.Add("@pCompetitiveGroupId is null");
            //                pCompetitiveGroupId.Value = DBNull.Value;
            //            }

            //            // поиск по наличию льгот
            //            SqlParameter pBenefitId = new SqlParameter("@pBenefitId", SqlDbType.Int);
            //            parameters.Add(pBenefitId);

            //            if (filterData.SelectedBenefitId < 0)
            //            {
            //                // нет льгот
            //                filter.Add(@"not exists (select top 1 1
            //from Application a (NOLOCK)
            //left join ApplicationEntranceTestDocument aetd (NOLOCK) on a.ApplicationId = aetd.ApplicationId
            //left join Benefit ben (NOLOCK) on aetd.BenefitID = ben.BenefitID
            //where a.ApplicationId = app.ApplicationId and app.InstitutionId = @pInstitutionId and ben.BenefitId is not null)");
            //                pBenefitId.Value = filterData.SelectedBenefitId.Value;
            //            }
            //            else if (filterData.SelectedBenefitId > 0)
            //            {
            //                // проверяем, что заявление связано с конкретной льготой BenefitId = filterData.SelectedBenefitId
            //                filter.Add(@"(@pBenefitId is not null and exists(select top 1 1
            //from ApplicationEntranceTestDocument aetd (NOLOCK)
            //inner join Benefit ben (NOLOCK) on aetd.BenefitID = ben.BenefitID
            //where aetd.ApplicationId = app.ApplicationId and app.InstitutionId = @pInstitutionId and @pBenefitId = ben.BenefitID))");
            //                pBenefitId.Value = filterData.SelectedBenefitId.Value;
            //            }
            //            else
            //            {
            //                // не важно есть ли льготы
            //                filter.Add("@pBenefitId is null");
            //                pBenefitId.Value = DBNull.Value;
            //            }

            //            // фильтр по приемной кампании
            //            SqlParameter pCampaignId = new SqlParameter("@pCampaignId", SqlDbType.Int);
            //            parameters.Add(pCampaignId);

            //            if (filterData.SelectedCampaignId.HasValue)
            //            {
            //                filter.Add(@"(@pCampaignId is not null and exists(
            //	                        select top 1 1 from ApplicationCompetitiveGroupItem acgi  (NOLOCK)
            //	                        inner join CompetitiveGroup cg (NOLOCK) on acgi.CompetitiveGroupId = cg.CompetitiveGroupId and acgi.ApplicationId = app.applicationId
            //	                        where cg.CampaignID = @pCampaignId
            //                        ))");
            //                pCampaignId.Value = filterData.SelectedCampaignId.Value;
            //            }
            //            else
            //            {
            //                //filter.Add("@pCampaignId is null");
            //                pCampaignId.Value = DBNull.Value;
            //            }

            //            // Фильтр по форме обучения
            //            SqlParameter pFormId = new SqlParameter("@pFormId", SqlDbType.Int);
            //            parameters.Add(pFormId);
            //            if (filterData.SelectedEducationFormType != null)
            //            {
            //                filter.Add("exists(select top 1 1 from ApplicationCompetitiveGroupItem acgi (NOLOCK) where acgi.ApplicationId = app.ApplicationId and acgi.EducationFormId = @pFormId)");
            //                pFormId.Value = filterData.SelectedEducationFormType.FormId;
            //            }
            //            else
            //            {
            //                pFormId.Value = DBNull.Value;
            //            }

            //            // Фильтр по источнику финансирования
            //            SqlParameter pSourceId = new SqlParameter("@pSourceId", SqlDbType.Int);
            //            parameters.Add(pSourceId);
            //            if (filterData.SelectedEducationSourceType != null)
            //            {
            //                filter.Add("exists(select top 1 1 from ApplicationCompetitiveGroupItem acgi (NOLOCK) where acgi.ApplicationId = app.ApplicationId and acgi.EducationSourceId = @pSourceId)");
            //                pSourceId.Value = filterData.SelectedEducationSourceType.SourceId;
            //            }
            //            else
            //            {
            //                pSourceId.Value = DBNull.Value;
            //            } 

            //            #endregion

            //            StringBuilder mainQueryBuilder = new StringBuilder();
            //            mainQueryBuilder.AppendLine(mainQuery);
            //            mainQueryBuilder.AppendLine("WHERE");
            //            mainQueryBuilder.AppendLine(filter.JoinAnd());

            List<AcceptedApplicationsRecordViewModel> records = SqlQueryHelper.GetPagedRecordsNew(parameters.ToArray(), MapAcceptedApplicationListRecord, pager, sortOptions);

            return new AcceptedApplicationsListViewModel
            {
                Pager = pager,
                SortDescending = sortOptions.SortDescending,
                SortKey = sortOptions.SortKey,
                Records = records,
                TotalApplicationsCount = totalUnfiltered
            };
        }

        ///// <summary>
        ///// Поиск заявлений, рекомендованных к зачислению
        ///// </summary>
        ///// <param name="institutionId">Id ОО</param>
        ///// <param name="queryModel">Модель запроса на выборку данных <see cref="RecommendedApplicationsQueryViewModel"/></param>
        ///// <returns>Список заявлений, рекомендованных к зачислению <see cref="RecommendedApplicationsListViewModel"/></returns>
        //public static RecommendedApplicationsListViewModel GetRecommendedApplicationsRecords(int institutionId, RecommendedApplicationsQueryViewModel queryModel)
        //{
        //    RecommendedApplicationsFilterViewModel filterData = queryModel.Filter ?? new RecommendedApplicationsFilterViewModel();
        //    PagerViewModel pager = queryModel.Pager ?? new PagerViewModel();
        //    SortViewModel sort = queryModel.Sort;

        //    FilterStateManager.Current.Update(filterData);

        //    int totalUnfiltered = 0;

        //    if (ConfigHelper.ShowFilterStatistics())
        //    {
        //        totalUnfiltered = GetTotalApplicationCountByRecommendedLists(institutionId);

        //        if (totalUnfiltered == 0)
        //        {
        //            return new RecommendedApplicationsListViewModel
        //            {
        //                Pager = new PagerViewModel(),
        //                SortDescending = sort.SortDescending,
        //                SortKey = sort.SortKey,
        //                TotalApplicationsCount = totalUnfiltered
        //            };
        //        }
        //    }

        //    #region Текст основного запроса для рекомендованных к зачислению
        //    const string mainQuery = @"SELECT 
        //        rec.RecListId RecommendedListId,
        //        app.ApplicationId,
        //        rec.EduLevelID EducationLevelId,
        //        rec.EduFormID EducationFormId,
        //        rec.DirectionID DirectionId,
        //        cmp.Name CampaignName,
        //        case rec.Stage when 1 then '1 этап' when 2 then '2 этап' end StageName,
        //        app.ApplicationNumber,
        //        rtrim(ISNULL(ent.LastName, '') + ' ' + ISNULL(ent.FirstName, '') + ' ' + ISNULL(ent.MiddleName, '')) EntrantName,
        //        edu_levels.Name EducationLevelName,
        //        edu_forms.Name EducationFormName,
        //        cg.Name CompetitiveGroupName,
        //        dir.Name DirectionName,
        //        app.OriginalDocumentsReceived,
        //        rec.Rating,
        //        cast(case cmp.StatusId when 2 then 1 else 0 end as bit) CampaignIsFinished,
        //        app.StatusId ApplicationStatusId
        //        FROM 
	       //         RecomendedLists rec (NOLOCK)
	       //         inner join RecomendedListsHistory rlh (NOLOCK) on rlh.RecListID = rec.RecListID
	       //         inner join Application app (NOLOCK) on rec.ApplicationId = app.ApplicationId and app.InstitutionId = rec.InstitutionID
	       //         inner join Campaign cmp (NOLOCK) on rec.CampaignId = cmp.CampaignID and cmp.InstitutionId = rec.InstitutionId
	       //         inner join CompetitiveGroup cg (NOLOCK) on rec.CompetitiveGroupID = cg.CompetitiveGroupID and cg.CampaignID = cmp.CampaignID
	       //         inner join Direction dir (NOLOCK) on rec.DirectionID = dir.DirectionId
	       //         inner join Entrant ent (NOLOCK) on app.EntrantId = ent.EntrantID
	       //         left join EntrantDocument ent_doc (NOLOCK) on ent.IdentityDocumentID = ent_doc.EntrantDocumentID
	       //         left join AdmissionItemType edu_forms (NOLOCK) on rec.EduFormID = edu_forms.ItemTypeID
	       //         left join AdmissionItemType edu_levels (NOLOCK) on rec.EduLevelID = edu_levels.ItemTypeID";
        //    #endregion

        //    #region Собираем фильтр WHERE...
        //    List<string> filter = new List<string>();
        //    List<SqlParameter> parameters = new List<SqlParameter>();

        //    filter.Add("rlh.DateDelete IS NULL");
        //    parameters.Add(filter.FieldEqualsOrNullParamInt("rec.InstitutionId", "pInstitutionId", institutionId));
        //    parameters.Add(filter.FieldLikeOrNullParam("app.ApplicationNumber", "pApplicationNumber", filterData.ApplicationNumber));
        //    parameters.Add(filter.FieldEqualsOrNullParamBool("app.OriginalDocumentsReceived", "pOriginalsReceived", filterData.OriginalDocumentsReceived));
        //    parameters.Add(filter.FieldLikeOrNullParam("ent.LastName", "pLastName", filterData.LastName));
        //    parameters.Add(filter.FieldLikeOrNullParam("ent.FirstName", "pFirstName", filterData.FirstName));
        //    parameters.Add(filter.FieldLikeOrNullParam("ent.MiddleName", "pMiddleName", filterData.MiddleName));
        //    parameters.Add(filter.FieldEqualsOrNullParamInt("rec.EduLevelID", "pEduLevelId", filterData.SelectedEducationLevel));
        //    parameters.Add(filter.FieldEqualsOrNullParamInt("cg.CompetitiveGroupId", "pCompetitiveGroupId", filterData.SelectedCompetitiveGroup));
        //    parameters.Add(filter.FieldEqualsOrNullParamInt("rec.EduFormID", "pEduFormId", filterData.SelectedEducationForm));
        //    parameters.Add(filter.FieldEqualsOrNullParamInt("dir.DirectionID", "pDirectionID", filterData.SelectedDirection));
        //    parameters.Add(filter.FieldEqualsOrNullParamInt("cmp.CampaignID", "pCampaignID", filterData.SelectedCampaign));

        //    int? selectedStage = null;
        //    if (filterData.SelectedStage.GetValueOrDefault() > 0)
        //    {
        //        selectedStage = filterData.SelectedStage.GetValueOrDefault();
        //    }

        //    parameters.Add(filter.FieldEqualsOrNullParamInt("cast(rec.Stage as int)", "pStageId", selectedStage));
        //    #endregion

        //    StringBuilder sql = new StringBuilder();
        //    sql.AppendLine(mainQuery);
        //    sql.AppendLine("WHERE");
        //    sql.AppendLine(filter.JoinAnd());

        //    List<RecommendedApplicationsRecordViewModel> records = SqlQueryHelper.GetPagedRecords(sql.ToString(), parameters.ToArray(), MapRecommendedApplicationsListRecord, pager, sort);

        //    return new RecommendedApplicationsListViewModel
        //    {
        //        Pager = pager,
        //        SortDescending = sort.SortDescending,
        //        SortKey = sort.SortKey,
        //        Records = records,
        //        TotalApplicationsCount = totalUnfiltered
        //    };
        //}

        /// <summary>
        /// Расширенный поиск заявлений
        /// </summary>
        /// <param name="institutionId">Id ОО</param>
        /// <param name="queryModel">Модель запроса на выборку данных <see cref="SearchApplicationsQueryViewModel"/></param>
        /// <returns>Список заявлений, удовлетворяющих условиям расширенного поиска <see cref="SearchApplicationsListViewModel"/></returns>
        public static SearchApplicationsListViewModel GetSearchApplicationsRecords(int institutionId, SearchApplicationsQueryViewModel queryModel)
        {
            SearchApplicationsFilterViewModel filterData = queryModel.Filter ?? new SearchApplicationsFilterViewModel();
            PagerViewModel pager = queryModel.Pager ?? new PagerViewModel();
            SortViewModel sortOptions = queryModel.Sort;

            FilterStateManager.Current.Update(filterData);

            int totalUnfiltered = 0;

            if (ConfigHelper.ShowFilterStatistics())
            {
                totalUnfiltered = GetTotalApplicationCountByStatusIdSet(institutionId);

                if (totalUnfiltered == 0)
                {
                        return new SearchApplicationsListViewModel
                        {
                            Pager = new PagerViewModel(),
                            SortDescending = sortOptions.SortDescending,
                            SortKey = sortOptions.SortKey,
                            TotalApplicationsCount = totalUnfiltered
                        };
                }
            }


            #region Текст основного запроса для выбора заявлений при расширенном поиске
            //const string mainQuery = @"
            //                                select
            //                app.ApplicationId,
            //                app.ApplicationNumber,
            //                app.RegistrationDate,
            //                app.StatusID,
            //                -- объединяем уникальные названия конкурсных групп для заявления в строку --
            //                substring((
            //                    select t.CompetitiveGroupName as [text()]
            //                    from (
            //                        select ', ' + cg.Name CompetitiveGroupName,
            //                                ROW_NUMBER() over(PARTITION BY cg.CompetitiveGroupId order by acgi.ApplicationId) is_distinct
            //                        from
            //                            ApplicationCompetitiveGroupItem acgi (NOLOCK) 
            //                            inner join CompetitiveGroup cg (NOLOCK) on acgi.CompetitiveGroupId = cg.CompetitiveGroupId
            //                        where
            //                            acgi.ApplicationId = app.ApplicationId
            //                    ) t 
            //                    where t.is_distinct = 1 order by t.CompetitiveGroupName
            //                    for xml path('')
            //                ), 3, 8000) CompetitiveGroupNames,
            //                -------------
            //                st.Name StatusName,
            //                rtrim(ISNULL(ent.LastName, '') + ' ' + ISNULL(ent.FirstName, '') + ' ' + ISNULL(ent.MiddleName, '')) EntrantName,
            //                ltrim(ISNULL(ent_doc.DocumentSeries, '') + ' ' + ISNULL(ent_doc.DocumentNumber, '')) IdentityDocument,
            //                cast(case when t.ApplicationId IS NULL then 0 else 1 end as bit) as CampaignIsFinished,
            //                cast(case st.StatusID when 8 then 1 else 0 end as bit) IsInOrder,
            //                ent.EntrantId,
            //                app.OrderOfAdmissionId OrderId
            //                from 
            //                Application app (NOLOCK)
            //                inner join ApplicationStatusType st (NOLOCK) on app.StatusID = st.StatusID
            //                inner join Entrant ent (NOLOCK) on app.EntrantID = ent.EntrantId
            //                inner join EntrantDocument ent_doc (NOLOCK) on ent_doc.EntrantDocumentID = ent.IdentityDocumentID
            //                left join (select top (1) acgi.ApplicationId  from ApplicationCompetitiveGroupItem acgi (NOLOCK)
            //                inner join CompetitiveGroup cg (NOLOCK) on acgi.CompetitiveGroupId = cg.CompetitiveGroupId
            //                inner join Campaign cmp (NOLOCK) on cg.CampaignId = cmp.CampaignId and cmp.InstitutionID = @pInstitutionId
            //                where cmp.StatusID = 2) t
            //                on app.ApplicationID = t.ApplicationId  
            //        ";

            //const string execQuery = @"EXEC	[dbo].[ftc_EntrantApplicationSearch]
		          //                                                              @model_id = 1,
		          //                                                              @institution_id = 1,
		          //                                                              @cmp_grp_id = NULL,
		          //                                                              @app_number = NULL,
		          //                                                              @last_name = NULL,
		          //                                                              @first_name = NULL,
		          //                                                              @middle_name = NULL,
		          //                                                              @reg_date_from = NULL,
		          //                                                              @reg_date_to = NULL,
		          //                                                              @doc_series = NULL,
		          //                                                              @doc_number = NULL,
		          //                                                              @order_id = NULL,
		          //                                                              @edu_form_id = NULL,
		          //                                                              @src_id = NULL,
		          //                                                              @benefit_id = NULL,
		          //                                                              @orig_received = NULL,
		          //                                                              @status_id = NULL,
		          //                                                              @start_page = NULL,
		          //                                                              @finish_page = NULL,
		          //                                                              @count_only = 0,
		          //                                                              @campaign_id = NULL";
            #endregion

            #region Собираем фильтр WHERE...
            List<string> filter = new List<string>(); // sql-выражения, по одному для каждой позиции в списке
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(filter.FieldEqualsOrNullParamInt("model_id", "model_id", 3));
            parameters.Add(filter.FieldEqualsOrNullParamInt("app.InstitutionId", "institution_id", institutionId));
            parameters.Add(filter.FieldEqualsOrNullParamInt("registrationYear", "year", null));
            parameters.Add(filter.FieldLikeOrNullParam("app.ApplicationNumber", "app_number", filterData.ApplicationNumber));
            parameters.Add(filter.FieldLikeOrNullParam("ent.LastName", "last_name", filterData.LastName));
            parameters.Add(filter.FieldLikeOrNullParam("ent.FirstName", "first_name", filterData.FirstName));
            parameters.Add(filter.FieldLikeOrNullParam("ent.MiddleName", "middle_name", filterData.MiddleName));
            parameters.AddRange(filter.FieldInDateRangeOrNullParams("app.RegistrationDate", "reg_date_from", filterData.RegistrationDateFrom, "reg_date_to", filterData.RegistrationDateTo));
            parameters.Add(filter.FieldLikeOrNullParam("ent_doc.DocumentSeries", "doc_series", filterData.DocumentSeries));
            parameters.Add(filter.FieldLikeOrNullParam("ent_doc.DocumentNumber", "doc_number", filterData.DocumentNumber));
            parameters.Add(filter.FieldEqualsOrNullParamInt("cg.CompetitiveGroupID", "cmp_grp_id", filterData.SelectedCompetitiveGroup));
            parameters.Add(filter.FieldEqualsOrNullParamInt("st.StatusID", "status_id", filterData.SelectedStatus));
            parameters.Add(filter.FieldEqualsOrNullParamInt("cmp.CampaignID", "campaign_id", null));
            parameters.Add(filter.FieldEqualsOrNullParamInt("order_id", "order_id", null));
            parameters.Add(filter.FieldEqualsOrNullParamInt("edu_form_id", "edu_form_id", null));
            parameters.Add(filter.FieldEqualsOrNullParamInt("src_id", "src_id", null));
            parameters.Add(filter.FieldEqualsOrNullParamInt("benefit_id", "benefit_id", null));
            parameters.Add(filter.FieldEqualsOrNullParamBool("orig_received", "orig_received", null));
            parameters.Add(filter.FieldEqualsOrNullParamInt("start_page", "start_page", pager.FirstRecordOffset));
            parameters.Add(filter.FieldEqualsOrNullParamInt("finish_page", "finish_page", pager.LastRecordOffset));
            //parameters.Add(filter.FieldEqualsOrNullParamInt("count_only", "count_only", 0));


            // поиск по конкурсным группам
            //SqlParameter pCompetitiveGroupId = new SqlParameter("@pCompetitiveGroupId", SqlDbType.Int);
            //parameters.Add(pCompetitiveGroupId);

            //if (filterData.SelectedCompetitiveGroup.HasValue)
            //{
            //    // учитываем заявления входящие в конкретную конкурсную группу CompetitiveGroupID = filterData.SelectedCompetitiveGroup
            //    filter.Add(@"exists(select top 1 1 from CompetitiveGroup cg (NOLOCK) inner join ApplicationCompetitiveGroupItem acgi (NOLOCK) on acgi.CompetitiveGroupId = cg.CompetitiveGroupId and acgi.ApplicationId = app.ApplicationId and cg.InstitutionId = @pInstitutionId and cg.CompetitiveGroupId = @pCompetitiveGroupId)");
            //    pCompetitiveGroupId.Value = filterData.SelectedCompetitiveGroup.Value;
            //}
            //else
            //{
            //    // поиск без учета конкурсной группы
            //    filter.Add("@pCompetitiveGroupId is null");
            //    pCompetitiveGroupId.Value = DBNull.Value;
            //}
            #endregion

            //StringBuilder mainQueryBuilder = new StringBuilder();
            //mainQueryBuilder.AppendLine(mainQuery);
            //mainQueryBuilder.AppendLine("WHERE");
            //mainQueryBuilder.AppendLine(filter.JoinAnd());


            //List<SearchApplicationsRecordViewModel> records = new List<SearchApplicationsRecordViewModel>();

            //if (totalUnfiltered > 0)
            //{

            //    using (SqlCommand cmd = TransactionManager.Current.CreateCommand(CommandType.StoredProcedure, "ftc_EntrantApplicationSearch"))
            //    {
            //        cmd.Parameters.AddRange(parameters.CopyToArray());
            //        cmd.Parameters.Add(filter.FieldEqualsOrNullParamInt("start_page", "start_page", pager.FirstRecordOffset));
            //        cmd.Parameters.Add(filter.FieldEqualsOrNullParamInt("finish_page", "finish_page", pager.LastRecordOffset));
            //        cmd.Parameters.Add(filter.FieldEqualsOrNullParamInt("count_only", "count_only", 0));


            //        using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess))
            //        {
            //            while (reader.Read())
            //            {
            //                records.Add(MapSearchApplicationListRecord(reader));
            //            }
            //        }
            //    }
            //}
            //pager.TotalRecords = totalUnfiltered;

            List<SearchApplicationsRecordViewModel> records = SqlQueryHelper.GetPagedRecordsNew(/*execQuery,*/ parameters.ToArray(), MapSearchApplicationListRecord, pager, sortOptions);

            return new SearchApplicationsListViewModel
            {
                Pager = pager,
                SortDescending = sortOptions.SortDescending,
                SortKey = sortOptions.SortKey,
                Records = records,
                TotalApplicationsCount = totalUnfiltered
            };
        }

        #region Загрузка фильтров для списков на закладках
        public static NewApplicationsFilterViewModel GetNewApplicationsFilter(int institutionId, int? highlight = null)
        {
            NewApplicationsFilterViewModel model;

            if (highlight.HasValue)
            {
                model = NewApplicationsFilterViewModel.Default;
                FilterStateManager.Current.Update(model);
            }
            else
            {
                model = FilterStateManager.Current.GetOrCreate<NewApplicationsFilterViewModel>();
            }

            model.CompetitiveGroups.Items.AddRange(LoadCompetitiveGroups(institutionId));
            model.Benefits.Items.AddRange(CacheManager.Current.GetOrCreateItem(CacheKeys.BenefitTypes, LoadBenefitTypes));
            model.Campaigns.Items.AddRange(LoadInstitutionCampaigns(institutionId));
            model.CampaignYears.Items.AddRange(LoadRegistrationYears(institutionId));
            return model;
        }

        public static UncheckedApplicationsFilterViewModel GetUncheckedApplicationsFilter(int institutionId, int? highlight = null)
        {
            UncheckedApplicationsFilterViewModel model;

            if (highlight.HasValue)
            {
                model = UncheckedApplicationsFilterViewModel.Default;
                FilterStateManager.Current.Update(model);
            }
            else
            {
                model = FilterStateManager.Current.GetOrCreate<UncheckedApplicationsFilterViewModel>();
            }


            model.ViolationTypes.Items.AddRange(CacheManager.Current.GetOrCreateItem(CacheKeys.ViolationTypes, LoadViolationTypes));
            // надо прописать удаление из кэша при редактировании списка CompetitiveGroups, тогда можно будет включить
            // model.CompetitiveGroups.Items.AddRange(CacheManager.Current.GetOrCreateItem(CacheKeys.CompetitiveGroups(institutionId), () => LoadCompetitiveGroups(institutionId)));
            model.CompetitiveGroups.Items.AddRange(LoadCompetitiveGroups(institutionId));
            model.Benefits.Items.AddRange(CacheManager.Current.GetOrCreateItem(CacheKeys.BenefitTypes, LoadBenefitTypes));
            model.Campaigns.Items.AddRange(LoadInstitutionCampaigns(institutionId));
            model.CampaignYears.Items.AddRange(LoadRegistrationYears(institutionId));
            return model;
        }

        public static RevokedApplicationsFilterViewModel GetRevokedApplicationsFilter(int institutionId, int? highlight = null)
        {
            RevokedApplicationsFilterViewModel model;

            if (highlight.HasValue)
            {
                model = RevokedApplicationsFilterViewModel.Default;
                FilterStateManager.Current.Update(model);
            }
            else
            {
                model = FilterStateManager.Current.GetOrCreate<RevokedApplicationsFilterViewModel>();
            }

            // надо прописать удаление из кэша при редактировании списка CompetitiveGroups, тогда можно будет включить
            // model.CompetitiveGroups.Items.AddRange(CacheManager.Current.GetOrCreateItem(CacheKeys.CompetitiveGroups(institutionId), () => LoadCompetitiveGroups(institutionId)));
            model.CompetitiveGroups.Items.AddRange(LoadCompetitiveGroups(institutionId));
            model.Benefits.Items.AddRange(CacheManager.Current.GetOrCreateItem(CacheKeys.BenefitTypes, LoadBenefitTypes));
            model.Campaigns.Items.AddRange(LoadInstitutionCampaigns(institutionId));

            return model;
        }

        public static AcceptedApplicationsFilterViewModel GetAcceptedApplicationsFilter(int institutionId, int? highlight)
        {
            AcceptedApplicationsFilterViewModel model;

            if (highlight.HasValue)
            {
                model = AcceptedApplicationsFilterViewModel.Default;
                FilterStateManager.Current.Update(model);
            }
            else
            {
                model = FilterStateManager.Current.GetOrCreate<AcceptedApplicationsFilterViewModel>();
            }

            model.CompetitiveGroups.Items.AddRange(LoadCompetitiveGroups(institutionId));
            model.Benefits.Items.AddRange(CacheManager.Current.GetOrCreateItem(CacheKeys.BenefitTypes, LoadBenefitTypes));
            model.Campaigns.Items.AddRange(LoadInstitutionCampaigns(institutionId));
            model.CampaignYears.Items.AddRange(LoadRegistrationYears(institutionId));
            return model;
        }

        public static RecommendedApplicationsFilterViewModel GetRecommendedApplicationsFilter(int institutionId)
        {
            var model = FilterStateManager.Current.GetOrCreate<RecommendedApplicationsFilterViewModel>();
            model.Campaigns.Items.AddRange(LoadInstitutionCampaigns(institutionId));
            model.CompetitiveGroups.Items.AddRange(LoadCompetitiveGroups(institutionId));
            model.Directions.Items.AddRange(LoadInstitutionDirections(institutionId));
            model.EducationForms.Items.AddRange(CacheManager.Current.GetOrCreateItem(CacheKeys.EducationForms, LoadEducationForms));
            model.EducationLevels.Items.AddRange(CacheManager.Current.GetOrCreateItem(CacheKeys.EducationLevels, LoadEducationLevels));

            return model;
        }

        public static SearchApplicationsFilterViewModel GetSearchApplicationsFilter(int institutionId, string autoQuery = null)
        {
            var model = FilterStateManager.Current.GetOrCreate<SearchApplicationsFilterViewModel>();

            if (!string.IsNullOrEmpty(autoQuery))
            {
                model = new SearchApplicationsFilterViewModel();
                var r = new Regex(@"\d+", RegexOptions.Compiled | RegexOptions.Singleline);
                if (r.Match(autoQuery).Success)
                {
                    model.ApplicationNumber = autoQuery;
                }
                else
                {
                    model.LastName = autoQuery;
                }
            }

            model.CompetitiveGroups.Items.AddRange(LoadCompetitiveGroups(institutionId));
            model.Statuses.Items.AddRange(CacheManager.Current.GetOrCreateItem(CacheKeys.ApplicationStatuses, LoadApplicationStatuses));
            return model;
        }
        #endregion

        #region Маппинг результатов запроса во view-модели представления записей списков
        private static NewApplicationsRecordViewModel MapNewApplicationListRecord(SqlDataReader reader)
        {
            int index = -1;
            return new NewApplicationsRecordViewModel
            {
                ApplicationId = reader.GetInt32(++index),
                ApplicationNumber = reader.SafeGetString(++index),
                StatusName = reader.SafeGetString(++index),
                StatusID = reader.GetInt32(++index),
                LastCheckDate = reader.SafeGetDateTimeAsString(++index),
                CompetitiveGroupNames = reader.SafeGetString(++index),
                EntrantFullName = reader.SafeGetString(++index),
                IdentityDocument = reader.SafeGetString(++index),
                RegistrationDate = reader.SafeGetDateTimeAsString(++index),
                IsInRecommendedLists = reader.SafeGetBool(++index).GetValueOrDefault(),

                IsCampaignFinished = reader.SafeGetBool(++index).GetValueOrDefault(),

                IsEditable = reader.SafeGetBool(++index).GetValueOrDefault()
            };
        }

        private static UncheckedApplicationsRecordViewModel MapUncheckedApplicationListRecord(SqlDataReader reader)
        {
            int index = -1;
            return new UncheckedApplicationsRecordViewModel
            {
                ApplicationId = reader.GetInt32(++index),
                ApplicationNumber = reader.SafeGetString(++index),
                ViolationErrors = reader.SafeGetString(++index),
                StatusName = reader.SafeGetString(++index),
                StatusID = reader.GetInt32(++index),
                LastCheckDate = reader.SafeGetDateTimeAsString(++index),
                EntrantFullName = reader.SafeGetString(++index),
                IdentityDocument = reader.SafeGetString(++index),
                RegistrationDate = reader.SafeGetDateTimeAsString(++index),
                IsInRecommendedLists = reader.SafeGetBool(++index).GetValueOrDefault(),
                CompetitiveGroupNames = reader.SafeGetString(++index),
                IsCampaignFinished = reader.SafeGetBool(++index).GetValueOrDefault()
            };
        }

        private static RevokedApplicationsRecordViewModel MapRevokedApplicationListRecord(SqlDataReader reader)
        {
            int index = -1;
            return new RevokedApplicationsRecordViewModel
            {
                ApplicationId = reader.GetInt32(++index),
                ApplicationNumber = reader.SafeGetString(++index),
                LastDenyDate = reader.SafeGetDateTimeAsString(++index),
                StatusID = reader.GetInt32(++index),
                EntrantFullName = reader.SafeGetString(++index),
                IdentityDocument = reader.SafeGetString(++index),
                RegistrationDate = reader.SafeGetDateTimeAsString(++index),
                IsInRecommendedLists = reader.SafeGetBool(++index).GetValueOrDefault(),
                IsCampaignFinished = reader.SafeGetBool(++index).GetValueOrDefault()
            };
        }

        private static AcceptedApplicationsRecordViewModel MapAcceptedApplicationListRecord(SqlDataReader reader)
        {
            int index = -1;
            return new AcceptedApplicationsRecordViewModel
            {
                ApplicationId = reader.GetInt32(++index),
                ApplicationNumber = reader.SafeGetString(++index),
                StatusName = reader.SafeGetString(++index),
                LastCheckDate = reader.SafeGetDateTimeAsString(++index),
                StatusID = reader.GetInt32(++index),
                EntrantFullName = reader.SafeGetString(++index),
                IdentityDocument = reader.SafeGetString(++index),
                RegistrationDate = reader.SafeGetDateTimeAsString(++index),
                OriginalDocumentsReceived = reader.SafeGetBool(++index).GetValueOrDefault(),
                Rating = reader.SafeGetDecimal(++index).ToInvariantRating(),
                IsInRecommendedLists = reader.SafeGetBool(++index).GetValueOrDefault(),
                CompetitiveGroupNames = reader.SafeGetString(++index),
                IsCampaignFinished = reader.SafeGetBool(++index).GetValueOrDefault(),
                CanIncludeInRecommended = reader.SafeGetBool(++index).GetValueOrDefault(),
                //CalculatedRating = reader.SafeGetDecimal(++index).GetValueOrDefault(),
            };
        }

        private static RecommendedApplicationsRecordViewModel MapRecommendedApplicationsListRecord(SqlDataReader reader)
        {
            int index = -1;
            return new RecommendedApplicationsRecordViewModel
            {
                RecommendedListId = reader.SafeGetInt(++index).GetValueOrDefault(),
                ApplicationId = reader.SafeGetInt(++index).GetValueOrDefault(),
                EducationLevelId = reader.SafeGetInt(++index),
                EducationFormId = reader.SafeGetInt(++index),
                DirectionId = reader.SafeGetInt(++index),
                CampaignName = reader.SafeGetString(++index),
                StageName = reader.SafeGetString(++index),
                ApplicationNumber = reader.SafeGetString(++index),
                EntrantFullName = reader.SafeGetString(++index),
                EducationLevelName = reader.SafeGetString(++index),
                EducationFormName = reader.SafeGetString(++index),
                CompetitiveGroupName = reader.SafeGetString(++index),
                DirectionName = reader.SafeGetString(++index),
                OriginalDocumentsReceived = reader.SafeGetBool(++index).GetValueOrDefault(),
                Rating = reader.SafeGetDecimal(++index).ToInvariantRating(),
                IsCampaignFinished = reader.SafeGetBool(++index).GetValueOrDefault(),
                ApplicationStatusId = reader.SafeGetInt(++index).GetValueOrDefault()
            };
        }

        private static SearchApplicationsRecordViewModel MapSearchApplicationListRecord(SqlDataReader reader)
        {
            int index = -1;
            return new SearchApplicationsRecordViewModel
            {
                ApplicationId = reader.GetInt32(++index),
                ApplicationNumber = reader.SafeGetString(++index),
                RegistrationDate = reader.SafeGetDateTimeAsString(++index),
                StatusID = reader.GetInt32(++index),
                CompetitiveGroupNames = reader.SafeGetString(++index),
                StatusName = reader.SafeGetString(++index),
                EntrantFullName = reader.SafeGetString(++index),
                IdentityDocument = reader.SafeGetString(++index),
                IsCampaignFinished = reader.SafeGetBool(++index).GetValueOrDefault(),
                IsInOrder = reader.SafeGetBool(++index).GetValueOrDefault(),
                EntrantId = reader.GetInt32(++index),
                OrderId = reader.SafeGetInt(++index)
            };
        }

        #endregion

        #region Загрузка справочников (данных комбобоксов), используемых в фильтрах поиска в списках заявлений
        /// <summary>
        /// Типы нарушений
        /// </summary>
        public static IEnumerable<SelectListItemViewModel<int>> LoadViolationTypes()
        {
            const string violationQuery = "select ViolationId, BriefName from ViolationType order by BriefName";
            return SqlQueryHelper.GetRecords(violationQuery, null, SqlQueryHelper.MapSelectListItem<int>);
        }

        /// <summary>
        /// Список конкурсных групп ОО
        /// </summary>
        /// <param name="institutuionId">Id ОО</param>
        public static IEnumerable<SelectListItemViewModel<int>> LoadCompetitiveGroups(int institutuionId)
        {
            const string competitiveGroupsQuery = @"
                    select cg.competitivegroupid, cg.name
                    from CompetitiveGroup cg (NOLOCK)
                    where cg.InstitutionId = @pInstitutionId
                    order by cg.name";

            return SqlQueryHelper.GetRecords(competitiveGroupsQuery, new[] { new SqlParameter("@pInstitutionId", institutuionId) }, SqlQueryHelper.MapSelectListItem<int>);
        }

        /// <summary>
        /// Список льгот
        /// </summary>
        public static IEnumerable<SelectListItemViewModel<short>> LoadBenefitTypes()
        {
            const string benefitsQuery = "select BenefitID, ShortName from Benefit order by ShortName";
            return SqlQueryHelper.GetRecords(benefitsQuery, null, SqlQueryHelper.MapSelectListItem<short>);
        }

        /// <summary>
        /// Список приемных кампаний ОО
        /// </summary>
        /// <param name="institutionId">Id ОО</param>
        public static IEnumerable<SelectListItemViewModel<int>> LoadInstitutionCampaigns(int institutionId)
        {
            const string institutionCampaignsQuery = @"select CampaignId, Name, StatusID from Campaign (NOLOCK) where InstitutionId = @pInstitutionId order by Name";

            return SqlQueryHelper.GetRecords(institutionCampaignsQuery, new[] { new SqlParameter("@pInstitutionId", institutionId) }, SqlQueryHelper.MapSelectListItemsс<int>);
        }

        /// <summary>
        /// Список годов
        /// </summary>
        /// <param name="institutionId">Id ОО</param>
        public static IEnumerable<SelectListItemViewModel<int>> LoadRegistrationYears(int institutionId)
        {
            const string campaignYearsQuery = @"SELECT DISTINCT YearStart FROM Campaign WITH (NOLOCK) WHERE InstitutionID = @pInstitutionId AND YearStart > DATEPART(YEAR, GETDATE()) - 5";

            return SqlQueryHelper.GetRecords(campaignYearsQuery, new[] { new SqlParameter("@pInstitutionId", institutionId) }, SqlQueryHelper.MapSelectListItemsy<int>);
        }

        /// <summary>
        /// Список направлений подготовки ОО
        /// </summary>
        /// <param name="institutionId">Id ОО</param>
        public static IEnumerable<SelectListItemViewModel<int>> LoadInstitutionDirections(int institutionId)
        {
            const string directionsQuery = @"select dir.DirectionID, dir.Name
                                            from
                                                AllowedDirections ad inner (NOLOCK) join Direction dir on ad.DirectionID = dir.DirectionID
                                            where ad.InstitutionID = @pInstitutionId
                                            order by dir.Name";

            return SqlQueryHelper.GetRecords(directionsQuery, new[] { new SqlParameter("@pInstitutionId", institutionId) }, SqlQueryHelper.MapSelectListItem<int>);
        }

        /// <summary>
        /// Список форм обучения
        /// </summary>
        public static IEnumerable<SelectListItemViewModel<short>> LoadEducationForms()
        {
            const string query = @"select ItemTypeId, Name from AdmissionItemType where ItemTypeId in (10, 11, 12) order by Name";
            return SqlQueryHelper.GetRecords(query, null, SqlQueryHelper.MapSelectListItem<short>);
        }

        /// <summary>
        /// Список уровней образования
        /// </summary>
        public static IEnumerable<SelectListItemViewModel<short>> LoadEducationLevels()
        {
            const string query = @"select ItemTypeId, Name from AdmissionItemType where ItemTypeId in (2,3,4,5,17,18,19) order by Name";
            return SqlQueryHelper.GetRecords(query, null, SqlQueryHelper.MapSelectListItem<short>);
        }

        /// <summary>
        /// Список статусов заявлений
        /// </summary>
        public static IEnumerable<SelectListItemViewModel<int>> LoadApplicationStatuses()
        {
            const string query = @"select StatusID, Name from ApplicationStatusType order by StatusID";
            return SqlQueryHelper.GetRecords(query, null, SqlQueryHelper.MapSelectListItem<int>);
        }

        #endregion
        #endregion

        #region Действия над записями списков заявлений
        #region Принять заявления
        /// <summary>
        /// Возвращает информацию о принимаемых заявлениях
        /// </summary>
        /// <param name="institutionId">Id ОО</param>
        /// <param name="applicationId">Перечень идентификаторов заявлений, для которых следует выгрузить сведения для отображения в диалоге приема заявлений</param>
        /// <returns>Список сведений о принимаемых заявлениях <see cref="AcceptApplicationsListViewModel"/></returns>
        public static AcceptApplicationsListViewModel GetAcceptApplicationsListViewModel(int institutionId, int[] applicationId)
        {
            #region Текст SQL-запросов
            const string queryMultipleText = @"select
                                            app.ApplicationId,
                                            app.ApplicationNumber,
                                            rtrim(ent.LastName + ' ' + ISNULL(ent.FirstName, '') + ' ' + ISNULL(ent.MiddleName, '')) EntrantName,
                                            rtrim(ISNULL(ent_doc.DocumentSeries, '') + ' ' + ISNULL(ent_doc.DocumentNumber, '')) IdentityDocument
                                        from
                                            Application app (NOLOCK)
                                            inner join #tmpAppId tmp on app.ApplicationId = tmp.Id
                                            inner join Entrant ent (NOLOCK) on app.EntrantID = ent.EntrantID
                                            left join EntrantDocument ent_doc (NOLOCK) on ent.IdentityDocumentID = ent_doc.EntrantDocumentID
                                        where 
                                            app.InstitutionID = @pInstitutionId";

            const string querySingleText = @"select
                                            app.ApplicationId,
                                            app.ApplicationNumber,
                                            rtrim(ent.LastName + ' ' + ISNULL(ent.FirstName, '') + ' ' + ISNULL(ent.MiddleName, '')) EntrantName,
                                            rtrim(ISNULL(ent_doc.DocumentSeries, '') + ' ' + ISNULL(ent_doc.DocumentNumber, '')) IdentityDocument
                                        from
                                            Application app (NOLOCK)
                                            inner join Entrant ent (NOLOCK) on app.EntrantID = ent.EntrantID
                                            left join EntrantDocument ent_doc (NOLOCK) on ent.IdentityDocumentID = ent_doc.EntrantDocumentID
                                        where
                                            app.ApplicationID = @pApplicationId                                            
                                            and app.InstitutionID = @pInstitutionId";
            #endregion

            var model = new AcceptApplicationsListViewModel();

            if (applicationId != null && applicationId.Length > 0)
            {
                string queryText;

                var parameters = new List<SqlParameter>(2)
                {
                    new SqlParameter("@pInstitutionId", SqlDbType.Int){ Value = institutionId }
                };

                if (applicationId.Length == 1)
                {
                    queryText = querySingleText;
                    parameters.Add(new SqlParameter("@pApplicationId", SqlDbType.Int) { Value = applicationId[0] });
                }
                else
                {
                    applicationId.WriteToTempTable("#tmpAppId");
                    queryText = queryMultipleText;
                }

                model.ApplicationRecords = SqlQueryHelper.GetRecords(queryText, parameters.ToArray(), MapAcceptApplicationRecordViewModel);
            }

            return model;
        }

        private static AcceptApplicationRecordViewModel MapAcceptApplicationRecordViewModel(SqlDataReader reader)
        {
            int index = -1;

            return new AcceptApplicationRecordViewModel
            {
                ApplicationId = reader.SafeGetInt(++index).GetValueOrDefault(),
                ApplicationNumber = reader.SafeGetString(++index),
                EntrantName = reader.SafeGetString(++index),
                IdentityDocument = reader.SafeGetString(++index)
            };
        }

        public static void AcceptApplications(int institutionId, AcceptApplicationsListViewModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }

            if (model.ApplicationRecords == null || model.ApplicationRecords.Count == 0)
            {
                return;
            }

            #region Текст SQL-запросов
            const string updateSingleText = @"update Application With (Rowlock)
                                                     set StatusDecision = @pReason, 
                                                         StatusID = @pStatusId 
                                              where 
                                                    ApplicationID = @pApplicationId 
                                                    and InstitutionID = @pInstitutionId";

            const string updateMultipleText = @"update app With (Rowlock)
                                                       set app.StatusDecision = tmp.Reason, 
                                                           app.StatusID = @pStatusId 
                                                       from 
                                                           Application app 
                                                           inner join #tmpApp tmp on app.ApplicationID = tmp.Id
                                                       where
                                                           app.InstitutionID = @pInstitutionId";
            #endregion

            List<SqlParameter> parameters = new List<SqlParameter>
                {
                    new SqlParameter("@pInstitutionId", SqlDbType.Int) {Value = institutionId},
                    new SqlParameter("@pStatusId", SqlDbType.Int) { Value = 4 }
                };

            string queryText;

            if (model.ApplicationRecords.Count == 1)
            {
                queryText = updateSingleText;
                parameters.Add(new SqlParameter("@pApplicationId", SqlDbType.Int)
                {
                    Value = model.ApplicationRecords[0].ApplicationId
                });
                parameters.Add(new SqlParameter("@pReason", SqlDbType.VarChar)
                {
                    Value = model.ApplicationRecords[0].Reason ?? (object)DBNull.Value
                });
            }
            else
            {
                using (DataTable tmp = new DataTable("#tmpApp"))
                {
                    tmp.Columns.Add("Id", typeof(int)).AllowDBNull = false;
                    tmp.Columns.Add("Reason", typeof(string)).AllowDBNull = true;

                    tmp.BeginLoadData();

                    foreach (var record in model.ApplicationRecords)
                    {
                        tmp.Rows.Add(record.ApplicationId, record.Reason);
                    }

                    tmp.EndLoadData();

                    tmp.WriteToTempTable();
                }

                queryText = updateMultipleText;
            }

            using (var cmd = TransactionManager.Current.CreateCommand(CommandType.Text, queryText))
            {
                cmd.Parameters.AddRange(parameters.ToArray());
                cmd.ExecuteNonQuery();
            }
        }
        #endregion

        #region Отозвать заявления
        /// <summary>
        /// Возвращает информацию об отзываемых заявлениях
        /// </summary>
        /// <param name="institutionId">Id ОО</param>
        /// <param name="applicationId">Перечень идентификаторов заявлений, для которых следует выгрузить сведения для отображения в диалоге отзыва заявлений</param>
        /// <returns>Список сведений об отзываемых заявлениях <see cref="RevokeApplicationsListViewModel"/></returns>
        public static RevokeApplicationsListViewModel GetRevokeApplicationsListViewModel(int institutionId, int[] applicationId)
        {
            #region Текст SQL-запросов
            const string queryMultipleText = @"select
                                            app.ApplicationId,
                                            app.ApplicationNumber,
                                            app.ReturnDocumentsTypeId,
                                            rtrim(ent.LastName + ' ' + ISNULL(ent.FirstName, '') + ' ' + ISNULL(ent.MiddleName, '')) EntrantName,
                                            rtrim(ISNULL(ent_doc.DocumentSeries, '') + ' ' + ISNULL(ent_doc.DocumentNumber, '')) IdentityDocument
                                        from
                                            Application app  (NOLOCK)
                                            inner join #tmpAppId tmp on app.ApplicationId = tmp.Id
                                            inner join Entrant ent (NOLOCK) on app.EntrantID = ent.EntrantID
                                            left join EntrantDocument ent_doc (NOLOCK) on ent.IdentityDocumentID = ent_doc.EntrantDocumentID
                                        where 
                                             app.InstitutionID = @pInstitutionId";

            const string querySingleText = @"select top 1
                                            app.ApplicationId,
                                            app.ApplicationNumber,
                                            app.ReturnDocumentsTypeId,
                                            rtrim(ent.LastName + ' ' + ISNULL(ent.FirstName, '') + ' ' + ISNULL(ent.MiddleName, '')) EntrantName,
                                            rtrim(ISNULL(ent_doc.DocumentSeries, '') + ' ' + ISNULL(ent_doc.DocumentNumber, '')) IdentityDocument
                                        from
                                            Application app (NOLOCK)
                                            inner join Entrant ent (NOLOCK) on app.EntrantID = ent.EntrantID
                                            left join EntrantDocument ent_doc (NOLOCK) on ent.IdentityDocumentID = ent_doc.EntrantDocumentID
                                        where
                                            app.ApplicationID = @pApplicationId                                            
                                            and app.InstitutionID = @pInstitutionId";
            #endregion
            var model = new RevokeApplicationsListViewModel();

            if (applicationId != null && applicationId.Length > 0)
            {
                string queryText;

                var parameters = new List<SqlParameter>(2)
                {
                    new SqlParameter("@pInstitutionId", SqlDbType.Int){ Value = institutionId }
                };

                if (applicationId.Length == 1)
                {
                    queryText = querySingleText;
                    parameters.Add(new SqlParameter("@pApplicationId", SqlDbType.Int) { Value = applicationId[0] });
                }
                else
                {
                    applicationId.WriteToTempTable("#tmpAppId");
                    queryText = queryMultipleText;
                }

                model.ApplicationRecords = SqlQueryHelper.GetRecords(queryText, parameters.ToArray(), MapRevokeApplicationRecordViewModel);
                // заполнение комбо с типом возвращаемых доков
                model.ReturnDocumentsTypes = SQL.GetApplicationReturnDocumentsType();
            }

            return model;
        }

        public static void RevokeApplications(int institutionId, RevokeApplicationsListViewModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }

            if (model.ApplicationRecords == null || model.ApplicationRecords.Count == 0)
            {
                return;
            }

            #region Текст SQL-запросов
            const string updateSingleText = @"update Application With (Rowlock)
                                                     set StatusDecision = @pReason, 
                                                         StatusID = @pStatusId,
                                                         ViolationId = @pViolationId,
                                                         LastDenyDate = GETDATE(),
                                                         ReturnDocumentsDate = @pReturnDocumentsDate,
                                                         ReturnDocumentsTypeId = @pReturnDocumentsTypeId
                                              where 
                                                    ApplicationID = @pApplicationId 
                                                    and InstitutionID = @pInstitutionId";

            const string updateMultipleText = @"update app With (rowlock)
                                                       set app.StatusDecision = tmp.Reason, 
                                                           app.StatusID = @pStatusId,
                                                           app.ViolationId = @pViolationId,
                                                           app.LastDenyDate = GETDATE(),
                                                           app.ReturnDocumentsDate = tmp.ReturnDocumentsDate,
                                                           app.ReturnDocumentsTypeId = tmp.ReturnDocumentsTypeId   
                                                       from 
                                                           Application app 
                                                           inner join #tmpApp tmp on app.ApplicationID = tmp.Id
                                                       where
                                                           app.InstitutionID = @pInstitutionId";
            #endregion

            List<SqlParameter> parameters = new List<SqlParameter>
                {
                    new SqlParameter("@pInstitutionId", SqlDbType.Int) {Value = institutionId},
                    new SqlParameter("@pStatusId", SqlDbType.Int) { Value = 6 },
                    new SqlParameter("@pViolationId", SqlDbType.Int) {Value = 3}
                };

            string queryText;

            if (model.ApplicationRecords.Count == 1)
            {
                queryText = updateSingleText;
                parameters.Add(new SqlParameter("@pApplicationId", SqlDbType.Int)
                {
                    Value = model.ApplicationRecords[0].ApplicationId
                });
                parameters.Add(new SqlParameter("@pReason", SqlDbType.VarChar)
                {
                    Value = model.ApplicationRecords[0].Reason ?? (object)DBNull.Value
                });
                parameters.Add(new SqlParameter("@pReturnDocumentsTypeId", SqlDbType.Int)
                {
                    Value = model.ApplicationRecords[0].ReturnDocumentsTypeId ?? (object)DBNull.Value
                });
                parameters.Add(new SqlParameter("@pReturnDocumentsDate", SqlDbType.DateTime)
                {
                    Value = model.ApplicationRecords[0].ReturnDocumentsDate ?? (object)DBNull.Value
                });

            }
            else
            {
                using (DataTable tmp = new DataTable("#tmpApp"))
                {
                    tmp.Columns.Add("Id", typeof(int)).AllowDBNull = false;
                    tmp.Columns.Add("Reason", typeof(string)).AllowDBNull = true;
                    tmp.Columns.Add("ReturnDocumentsTypeId", typeof(int)).AllowDBNull = true;
                    tmp.Columns.Add("ReturnDocumentsDate", typeof(DateTime)).AllowDBNull = true;

                    tmp.BeginLoadData();

                    foreach (var record in model.ApplicationRecords)
                    {
                        tmp.Rows.Add(record.ApplicationId, record.Reason, record.ReturnDocumentsTypeId, record.ReturnDocumentsDate);
                    }

                    tmp.EndLoadData();

                    tmp.WriteToTempTable();
                }

                queryText = updateMultipleText;
            }

            using (var cmd = TransactionManager.Current.CreateCommand(CommandType.Text, queryText))
            {
                cmd.Parameters.AddRange(parameters.ToArray());
                cmd.ExecuteNonQuery();
            }
        }

        private static RevokeApplicationRecordViewModel MapRevokeApplicationRecordViewModel(SqlDataReader reader)
        {
            int index = -1;

            return new RevokeApplicationRecordViewModel
            {
                ApplicationId = reader.SafeGetInt(++index).GetValueOrDefault(),
                ApplicationNumber = reader.SafeGetString(++index),
                ReturnDocumentsTypeId = reader.SafeGetInt(++index).GetValueOrDefault(),
                EntrantName = reader.SafeGetString(++index),
                IdentityDocument = reader.SafeGetString(++index)
            };
        }
        #endregion

        #region Отменить отзыв заявлений
        /// <summary>
        /// Отмена отзыва заявлений
        /// </summary>
        /// <param name="institutionId">Id ОО</param>
        /// <param name="applicationId">Перечень идентифкаторов заявлений для которых отменяется отзыв</param>
        public static void CancelRevoke(int institutionId, int[] applicationId)
        {
            const string query = @"update app With (Rowlock)
                                   set app.StatusId = 2, LastDenyDate = NULL
                                   from Application app 
                                        inner join #tmpAppId tmp on app.ApplicationId = tmp.Id 
                                   where app.InstitutionId = @pInstitutionId";

            applicationId.WriteToTempTable("#tmpAppId");

            using (SqlCommand cmd = TransactionManager.Current.CreateCommand(CommandType.Text, query))
            {
                cmd.Parameters.Add(new SqlParameter("@pInstitutionId", SqlDbType.Int) { Value = institutionId });
                cmd.ExecuteNonQuery();
            }
        }
        #endregion 


        #endregion

        #region Включить в список рекомендованных

        /// <summary>
        /// Получение данных о заявлении для отображения в диалоге включения в список рекомендованных
        /// </summary>
        /// <param name="applicationId">ID заявления</param>
        /// <param name="institutionId">Id ОО</param>
        /// <returns>View-модель включения в список рекомендованных</returns>
        public static IncludeRecommendedListViewModel GetIncludeRecommendedListViewModel(int applicationId, int institutionId)
        {
            #region Текст sql-запросов
            // сведения о заявлении
            const string applicationInfoQuery = @"
                select TOP 1 
                    app.ApplicationID,
	                app.ApplicationNumber,
	                rtrim(ISNULL(ent.LastName, '') + ' ' + ISNULL(ent.FirstName, '') + ' ' + ISNULL(ent.MiddleName, '')) EntrantName,
	                ltrim(ISNULL(ent_doc.DocumentSeries, '') + ' ' + ISNULL(ent_doc.DocumentNumber, '')) IdentityDocument
                from
	                Application app (NOLOCK)
	                inner join Entrant ent (NOLOCK) on app.EntrantID = ent.EntrantID
	                left join EntrantDocument ent_doc (NOLOCK) on ent.IdentityDocumentID = ent_doc.EntrantDocumentID
                where
	                app.ApplicationID = @pApplicationID
	                and app.InstitutionID = @pInstitutionID";

            // список условий приема, соответствующих заявлению
            const string conditionsQuery = @"
                select 
	                acgi.id ApplicationCompetitiveGroupItemId,
	                cg.Name CompetitiveGroupName,
	                dir.Name DirectionName,
	                lvl.Name EducationLevelName,
	                frm.Name EducationFormName,
	                src.Name EducationSourceName,
	                acgi.Priority Priority
                from
	                Application app (NOLOCK)
	                inner join ApplicationCompetitiveGroupItem acgi (NOLOCK) on acgi.ApplicationID = app.ApplicationID
	                inner join CompetitiveGroup cg (NOLOCK) on acgi.CompetitiveGroupID = cg.CompetitiveGroupId
	                inner join Direction dir (NOLOCK) on cgi.DirectionId = dir.DirectionID
	                inner join AdmissionItemType lvl (NOLOCK) on cgi.EducationLevelID = lvl.ItemTypeID
	                inner join AdmissionItemType frm (NOLOCK) on acgi.EducationFormId = frm.ItemTypeID
	                inner join AdmissionItemType src (NOLOCK) on acgi.EducationSourceId = src.ItemTypeID
                where 
	                app.InstitutionID = @pInstitutionID
	                and app.ApplicationID = @pApplicationID
	                and app.InstitutionID = cg.InstitutionID
                    and cg.EducationLevelID in (2,3,5,19)
	                and acgi.EducationFormId in (11, 12)
	                and acgi.EducationSourceId = 14";
            #endregion

            var sqlParameters = new[]
                {
                    new SqlParameter("@pApplicationID", applicationId),
                    new SqlParameter("@pInstitutionID", institutionId)
                };

            // получаем модель, созданную по результатам запроса сведений о заявлении
            var model = SqlQueryHelper.GetRecord(applicationInfoQuery, sqlParameters, MapIncludeRecommendedList);

            // заполняем список условий приема (клонируем инстансы в sqlParameters, т.к. они уже привязаны к первому запросу)
            model.ApplicationRecords = SqlQueryHelper.GetRecords(conditionsQuery, sqlParameters.CopyToArray(), MapIncludeRecommendedRecord);

            return model;
        }

        private static IncludeRecommendedListViewModel MapIncludeRecommendedList(SqlDataReader reader)
        {
            int index = -1;
            return new IncludeRecommendedListViewModel
            {
                ApplicationId = reader.GetInt32(++index),
                ApplicationNumber = reader.SafeGetString(++index),
                EntrantName = reader.SafeGetString(++index),
                IdentityDocument = reader.SafeGetString(++index)
            };
        }

        private static IncludeRecommendedRecordViewModel MapIncludeRecommendedRecord(SqlDataReader reader)
        {
            int index = -1;
            return new IncludeRecommendedRecordViewModel
            {
                ApplicationCompetitiveGroupItemId = reader.GetInt32(++index),
                CompetitiveGroupName = reader.SafeGetString(++index),
                DirectionName = reader.SafeGetString(++index),
                EducationLevelName = reader.SafeGetString(++index),
                EducationFormName = reader.SafeGetString(++index),
                EducationSourceName = reader.SafeGetString(++index),
                Priority = reader.SafeGetInt(++index)
            };
        }

        /// <summary>
        /// Включение заявления в список рекомендованных
        /// </summary>
        /// <param name="institutionId">Id ОО</param>
        /// <param name="model">Модель данных запроса на включение в список рекомендованных</param>
        /// <returns>true если заявление было включено хотя бы по одному из направлений, иначе - false
        /// если false, то это означает что все доступные условия приема для данного заявления уже использованы
        /// </returns>
        public static bool IncludeInRecommendedList(int institutionId, IncludeRecommendedListSubmitViewModel model)
        {
            #region Заполнение временной таблицы #tmpStages
            using (DataTable table = new DataTable("#tmpStages"))
            {
                table.Columns.Add("ApplicationCompetitiveGroupItemID", typeof(int));
                table.Columns.Add("StageId", typeof(int));

                table.BeginLoadData();

                foreach (var item in model.SelectedItems)
                {
                    table.Rows.Add(item.Id, item.Stage);
                }

                table.EndLoadData();

                table.WriteToTempTable();
            }
            #endregion

            #region Текст sql-запроса для включения в список рекомендованных
            const string includeQuery = @"
                declare @insertLog TABLE(ID INT);
                declare @logDate datetime;

                with included as (
                    select 
	                    app.InstitutionID,
	                    cmp.CampaignID,
	                    lvl.ItemTypeID EduLevelID,
	                    frm.ItemTypeID EduFormID,
	                    cg.CompetitiveGroupID,
	                    dir.DirectionID,
	                    tmp.StageId as Stage,
	                    app.ApplicationID,
	                    (select sum(t.mark) from (
	                        select sum(ia.iamark) mark from IndividualAchivement ia (NOLOCK) where ia.ApplicationID = app.ApplicationID and ia.IAMark is not null
	                        union all
	                        select sum(aetd.resultvalue) from ApplicationEntranceTestDocument aetd (NOLOCK) where aetd.ApplicationID = app.ApplicationID and aetd.ResultValue is not null
	                        ) t)  as Rating
                    from
	                    Application app  (NOLOCK)
	                    inner join ApplicationCompetitiveGroupItem acgi (NOLOCK) on acgi.ApplicationID = app.ApplicationID
	                    inner join #tmpStages tmp on tmp.ApplicationCompetitiveGroupItemID = acgi.id
	                    inner join CompetitiveGroup cg (NOLOCK) on acgi.CompetitiveGroupID = cg.CompetitiveGroupId
	                    inner join Campaign cmp (NOLOCK) on cg.CampaignID = cmp.CampaignID and cmp.InstitutionID = app.InstitutionID
	                    inner join Direction dir (NOLOCK) on cgi.DirectionId = dir.DirectionID
	                    inner join AdmissionItemType lvl (NOLOCK) on cgi.EducationLevelID = lvl.ItemTypeID
	                    inner join AdmissionItemType frm (NOLOCK) on acgi.EducationFormId = frm.ItemTypeID
                    where 
	                    app.InstitutionID = @pInstitutionID
	                    and app.ApplicationID = @pApplicationID
	                    and app.InstitutionID = cg.InstitutionID
	                    and cg.EducationLevelID in (2,3,5,19)
	                    and acgi.EducationFormId in (11, 12)
	                    and acgi.EducationSourceId = 14
                )
                merge RecomendedLists as rl
                using included as t -- выбираем те записи для которых в RecomendedListsHistory НЕ ПРОСТАВЛЕНА дата удаления
                on 
                    t.InstitutionID = rl.InstitutionID 
                    and t.CampaignID = rl.CampaignID 
                    and t.EduLevelID = rl.EduLevelID 
                    and t.EduFormID = rl.EduFormID
                    and t.CompetitiveGroupID = rl.CompetitiveGroupID
                    and t.DirectionID = rl.DirectionID 
                    and t.Stage = rl.Stage 
                    and t.ApplicationID = rl.ApplicationID
	                AND exists(select top 1 1 from RecomendedListsHistory rlh where rlh.[DateDelete] is null and rlh.RecListID = rl.RecListID)
                WHEN NOT MATCHED -- НЕ СОВПАДУТ записи отсутствующие в RecomendedLists (по критериям merge) и те записи 
                                 -- которые есть в RecomendedLists но в RecomendedListsHistory ПРОСТАВЛЕНА дата удаления
                THEN
                    insert(InstitutionID, CampaignID, EduLevelID, EduFormID, CompetitiveGroupID, DirectionID, Stage, ApplicationID, Rating)
                    values(t.InstitutionID, t.CampaignID, t.EduLevelID, t.EduFormID, t.CompetitiveGroupID, t.DirectionID, t.Stage, t.ApplicationID, t.Rating)
                OUTPUT Inserted.RecListID into @insertLog;

                set @logDate = GETDATE();

                insert into RecomendedListsHistory([RecListID], [DateAdd])
                select id, @logDate from @insertLog;

                select count(*) from @insertLog;";
            #endregion

            using (SqlCommand cmd = TransactionManager.Current.CreateCommand(CommandType.Text, includeQuery))
            {
                cmd.Parameters.Add(new SqlParameter("@pApplicationId", model.ApplicationId));
                cmd.Parameters.Add(new SqlParameter("@pInstitutionId", institutionId));
                object numIns = cmd.ExecuteScalar();
                if (numIns != null && numIns != DBNull.Value)
                {
                    return Convert.ToInt32(numIns) > 0;
                }

                return false;
            }
        }

        /// <summary>
        /// Исключение из списка рекомендованных
        /// </summary>
        /// <param name="recListId">Id условия приема (записи в RecomendedLists)</param>
        /// <param name="applicationId">Id заявления</param>
        /// <param name="institutionId">Id ОО</param>
        public static void ExcludeFromRecommendedList(int? recListId, int? applicationId, int institutionId)
        {
            const string query = @"
                update rlh 
	            set 
		            rlh.[DateDelete] = GETDATE()
	            from 
		            RecomendedListsHistory rlh
		            inner join RecomendedLists rl on rlh.RecListID = rl.RecListID
		            inner join Application app on rl.ApplicationID = app.ApplicationID
	            where 
		            rl.RecListID = @pRecListId 
		            and app.ApplicationID = @pApplicationId
		            and app.InstitutionID = @pInstitutionId";

            if (!recListId.HasValue || !applicationId.HasValue)
            {
                return;
            }

            using (SqlCommand cmd = TransactionManager.Current.CreateCommand(CommandType.Text, query))
            {
                cmd.Parameters.Add(new SqlParameter("@pRecListId", recListId.Value));
                cmd.Parameters.Add(new SqlParameter("@pApplicationId", applicationId.Value));
                cmd.Parameters.Add(new SqlParameter("@pInstitutionId", institutionId));

                cmd.ExecuteNonQuery();
            }
        }
        #endregion


        public static bool ComputeNavigationParameters(int? applicationId, int institutionId)
        {
            var model = new ApplicationNavigationParameters
            {
                ApplicationId = applicationId
            };

            // Определяем к какому списку относится заявление в зависимости от значений его статуса
            const string statusQuery = @"select TOP 1 st.StatusID from Application app (NOLOCK) inner join ApplicationStatusType st on app.StatusID = st.StatusID where app.ApplicationID = @pApplicationId and app.InstitutionID = @pInstitutionID";
            string statusFilter = null; // содержит значения фильтра по статусу заявления при определении страницы в соответствующем списке, к которому относится заявление
            if (model.ApplicationId.HasValue)
            {
                int? statusId = null;

                using (SqlCommand cmd = TransactionManager.Current.CreateCommand(CommandType.Text, statusQuery))
                {
                    cmd.Parameters.Add(new SqlParameter("@pApplicationId", model.ApplicationId.Value));
                    cmd.Parameters.Add(new SqlParameter("@pInstitutionId", institutionId));

                    object obj = cmd.ExecuteScalar();

                    if (obj != null && obj != DBNull.Value)
                    {
                        statusId = Convert.ToInt32(obj);
                    }
                }

                if (statusId.HasValue)
                {
                    switch (statusId.Value)
                    {
                        // новые 
                        case 1:
                        case 2:
                            model.Tab = ApplicationNavigationParameters.TabPage.New;
                            statusFilter = "st.StatusID in(1, 2)";
                            break;
                        // не прошедшие проверку
                        case 3:
                        case 5:
                            model.Tab = ApplicationNavigationParameters.TabPage.Unchecked;
                            statusFilter = "st.StatusID in(3, 5)";
                            break;
                        // отозванные
                        case 6:
                            model.Tab = ApplicationNavigationParameters.TabPage.Revoked;
                            statusFilter = "st.StatusID = 6";
                            break;
                        // принятые
                        case 4:
                            model.Tab = ApplicationNavigationParameters.TabPage.Accepted;
                            statusFilter = "st.StatusID = 4";
                            break;
                        default:
                            model.Tab = null;
                            break;
                    }
                }
            }

            // определяем, на какой странице списка находится интересующее нас заявление
            const string findPageQueryTemplate = @"
                select 
                    app.ApplicationId, 
                    ROW_NUMBER() OVER(order by app.ApplicationNumber) rn 
                from 
                    Application app (NOLOCK) 
                    inner join ApplicationStatusType st (NOLOCK) on app.StatusID = st.StatusID 
                    inner join Entrant ent (NOLOCK) on app.EntrantID = ent.EntrantID
                    inner join EntrantDocument ent_doc (NOLOCK) on ent.IdentityDocumentID = ent_doc.EntrantDocumentID
                where app.InstitutionId = @pInstitutionId";

            if (model.Tab.HasValue && statusFilter != null)
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("with apps as(");
                sql.AppendLine(findPageQueryTemplate);
                sql.AppendFormat("and {0}", statusFilter);
                sql.AppendLine(")");
                sql.AppendLine("select TOP 1 t.rn from apps t (NOLOCK)");
                sql.AppendLine("where t.ApplicationId = @pApplicationId");

                using (SqlCommand cmd = TransactionManager.Current.CreateCommand(CommandType.Text, sql.ToString()))
                {
                    cmd.Parameters.Add(new SqlParameter("@pApplicationId", model.ApplicationId.Value));
                    cmd.Parameters.Add(new SqlParameter("@pInstitutionId", institutionId));

                    object obj = cmd.ExecuteScalar();
                    if (obj != null && obj != DBNull.Value)
                    {
                        const decimal pageSize = 10m; //TRICKY! Поскольку размер страницы не хранится, то используем дефолтный (10 записей на страницу).
                        // Если же состояние размера страницы будет храниться (условно) постоянно, 
                        // то нужно переписать присвоение значения переменной pageSize (и сделать ее не const)
                        int recordNumber = Convert.ToInt32(obj);
                        model.Page = (int)Math.Ceiling(recordNumber / pageSize);
                    }
                }
            }

            if (model.Tab.HasValue && model.Page.HasValue && model.ApplicationId.HasValue)
            {
                const string navCookieKey = "fis_app_nav";
                string json = JsonConvert.SerializeObject(model, NavCookieSerializationSettings);
                HttpContext.Current.Response.SetCookie(new HttpCookie(navCookieKey, ZipCompressionHelper.ZipToBase64String(json)) { Expires = DateTime.UtcNow.AddYears(1) });
                return true;
            }

            return false;
        }

        private static readonly JsonSerializerSettings NavCookieSerializationSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };

        public static NewInstitutionApplicationListViewModel GetInstitutionApplicationListViewModel()
        {
            const string navCookieKey = "fis_app_nav";

            var model = new NewInstitutionApplicationListViewModel();

            HttpCookie navCookie = HttpContext.Current.Request.Cookies[navCookieKey];
            if (navCookie != null)
            {
                HttpContext.Current.Response.SetCookie(new HttpCookie(navCookieKey, null) { Expires = DateTime.UtcNow.AddYears(-1) });

                var obj = JsonConvert.DeserializeObject<ApplicationNavigationParameters>(ZipCompressionHelper.UnzipFromBase64String(navCookie.Value));

                if (obj.Tab.HasValue && obj.ApplicationId.HasValue)
                {
                    model.HighlightApplicationId = obj.ApplicationId.Value;
                    model.InitialTab = obj.Tab.Value;
                    model.InitialPage = obj.Page.GetValueOrDefault(1);
                }
            }

            return model;
        }

        public static int GetTotalApplicationCountByStatusIdSet(int institutionId, params int[] statusValues)
        {
            const string queryTemplate = @"
                select count(*) 
                from 
                    Application app (NOLOCK)
                    inner join ApplicationStatusType st on app.StatusID = st.StatusID 
                    inner join Entrant ent (NOLOCK) on app.EntrantID = ent.EntrantID
                    inner join EntrantDocument ent_doc (NOLOCK) on ent.IdentityDocumentID = ent_doc.EntrantDocumentID
                where 
                    app.InstitutionID = @pInstitutionId";

            string sql;
            List<SqlParameter> parameters = new List<SqlParameter> { new SqlParameter("@pInstitutionId", institutionId) };

            if (statusValues != null && statusValues.Length > 0)
            {
                statusValues = statusValues.Distinct().ToArray();
                SqlParameter[] statusParams = new SqlParameter[statusValues.Length];

                for (int paramIndex = 0; paramIndex < statusValues.Length; paramIndex++)
                {
                    string statusParamName = string.Format("@pStatus_{0}", paramIndex);
                    statusParams[paramIndex] = new SqlParameter(statusParamName, statusValues[paramIndex]);
                }

                StringBuilder sqlBuilder = new StringBuilder();
                sqlBuilder.AppendLine(queryTemplate);
                sqlBuilder.Append("and st.StatusID");

                if (statusParams.Length == 1)
                {
                    sqlBuilder.AppendFormat(" = {0}", statusParams[0].ParameterName);
                }
                else
                {
                    sqlBuilder.AppendFormat(" IN({0})", string.Join(", ", statusParams.Select(p => p.ParameterName)));
                }

                parameters.AddRange(statusParams);

                sql = sqlBuilder.ToString();
            }
            else
            {
                sql = queryTemplate;
            }

            using (SqlCommand cmd = TransactionManager.Current.CreateCommand(CommandType.Text, sql))
            {
                cmd.Parameters.AddRange(parameters.ToArray());
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public static int GetTotalApplicationCountByRecommendedLists(int institutionId)
        {
            const string query = @"
                SELECT 
                    count(*)
                FROM 
	                RecomendedLists rec
	                inner join RecomendedListsHistory rlh on rlh.RecListID = rec.RecListID
	                inner join Application app on rec.ApplicationId = app.ApplicationId and app.InstitutionId = rec.InstitutionID
	                inner join Campaign cmp on rec.CampaignId = cmp.CampaignID and cmp.InstitutionId = rec.InstitutionId
	                inner join CompetitiveGroup cg on rec.CompetitiveGroupID = cg.CompetitiveGroupID and cg.CampaignID = cmp.CampaignID
	                inner join Direction dir on rec.DirectionID = dir.DirectionId
	                inner join Entrant ent on app.EntrantId = ent.EntrantID
                WHERE
                    rlh.DateDelete IS NULL
                    and rec.InstitutionId = @pInstitutionId";

            using (SqlCommand cmd = TransactionManager.Current.CreateCommand(CommandType.Text, query))
            {
                cmd.Parameters.Add(new SqlParameter("@pInstitutionId", institutionId));
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        /// <summary>
        /// Возвращает статус заявления
        /// </summary>
        /// <param name="applicationId">Id заявления</param>
        /// <param name="institutionId">Id ОО</param>
        /// <returns></returns>
        public static int? GetApplicationStatus(int? applicationId, int institutionId)
        {
            if (applicationId.HasValue)
            {
                const string query = @"SELECT TOP 1 app.StatusID from Application app (NOLOCK) where app.InstitutionId = @pInstitutionId and app.StatusID is not null and app.ApplicationId = @pApplicationId";

                using (SqlCommand cmd = TransactionManager.Current.CreateCommand(CommandType.Text, query))
                {
                    cmd.Parameters.Add(new SqlParameter("@pInstitutionId", institutionId));
                    cmd.Parameters.Add(new SqlParameter("@pApplicationId", applicationId.GetValueOrDefault()));

                    object res = cmd.ExecuteScalar();

                    if (res != null && res != DBNull.Value)
                    {
                        return Convert.ToInt32(res);
                    }
                }
            }

            return null;
        }



        //==========================================================================================================

        //static bool hardError = false;

        /// <summary>
        /// Выполняет проверки заявлений
        /// </summary>
        /// <param name="institutionId">Id ОО</param>
        /// <param name="applicationsId">список идентификаторов проверяемых заявлений</param>
        /// <returns>View-модель результатов проверок</returns>
        public static CheckApplicationsListViewModel CheckApplications(int institutionId, int[] applicationsId)
        {
            if (applicationsId == null)
                throw new ArgumentNullException("applicationsId");

            if (applicationsId.Length == 0)
                throw new ArgumentException("Список идентификаторов заявлений не содержит записей", "applicationsId");

            // контейнер для сбора ошибок по каждому заявлению при выполнении проверок
            Dictionary<int, ViolationErrorList> errors = applicationsId.ToDictionary(key => key, v => new ViolationErrorList());
            errors.Keys.WriteToTempTable("#tmpAppId", "ApplicationId");
            // каждая проверка должна обновить ViolationError в errors[id заявления]

            Dictionary<int, ApplicationViolationInfo> otherErrors = new Dictionary<int, ApplicationViolationInfo>();

            //hardError = false;

            // ================================================================

            foreach (var appId in errors.Keys)
            {
                var app = new AppResultsModel
                {
                    ApplicationID = appId,
                    refr = 1,
                    method = "ByIdentityDocument",
                    Step = 5
                };

                //#DocumentsCheck - добавлена проверка (FIS-1711)
                IEnumerable<string> requiredDocuments;
                var checkDocuments = ApplicationSQL.CheckApplicationDocuments(app.ApplicationID, out requiredDocuments);
                if (!checkDocuments)//Если документов нет - никаких доп. проверок не делаем
                {
                    ApplicationViolationInfo applicationViolationInfo = new ApplicationViolationInfo();
                    applicationViolationInfo.ApplicationId = app.ApplicationID;
                    applicationViolationInfo.ApplicationNumber = ApplicationSQL.GetApplicationNumber(app.ApplicationID);
                    applicationViolationInfo.ViolationMessage = String.Format("Заявление должно содержать один из следующих документов об образовании: {0}", String.Join("; ", requiredDocuments));
                    otherErrors.Add(applicationViolationInfo.ApplicationId, applicationViolationInfo);
                }
                else
                {
                    // жесткая проверка Проверка ЕГЭ
                    var check1 = Check_EGE(app);
                    if (check1 != null && check1.violationId != 0)
                    {
                        errors[appId].Errors.Add(new ViolationError
                        {
                            ViolationId = check1.violationId,
                            ViolationMessage = check1.violationMessage
                        });
                        //hardError = true;
                    }

                    // жесткая проверка Проверка Олимпиады
                    var check2 = EntrantApplicationSQL.Check_Olympic(app);
                    if (check2.violationId != 0)
                    {
                        errors[appId].Errors.Add(new ViolationError
                        {
                            ViolationId = check2.violationId,
                            ViolationMessage = check2.violationMessage
                        });
                        //hardError = true;
                    }

                    // проверка льгот
                    var check3 = Check_Benefit(appId);
                    if (check3.violationId != 0)
                    {
                        errors[appId].Errors.Add(new ViolationError
                        {
                            ViolationId = check3.violationId,
                            ViolationMessage = check3.violationMessage
                        });
                        //hardError = true;
                    }

                    // проверка предупредительного характера 5 вузов
                    var check4 = EntrantApplicationSQL.Check_5VUZ(appId);
                    if (check4.violationId != 0)
                    {
                        errors[appId].Errors.Add(new ViolationError
                        {
                            ViolationId = check4.violationId,
                            ViolationMessage = check4.violationMessage
                        });
                    }
                }
            }

            // ================================================================

            // проверка на запуск проверок ЕГЭ
            //ChekcEGE(errors);

            // проверка олимпиад
            //CheckOlympicResults(errors);
            // добавляем сюда другие проверки по мере реализации

            // проверка что заявление подано более чем в 5 вузов
            //CheckInstitutionLimit(errors);

            // проверка результатов ЕГЭ
            //try
            //{
            //    // другая база
            //CheckExaminationMarks(errors);
            //}
            //catch (Exception)
            //{
            //    hardError = true;
            //    //throw;
            //}

            // проверка что заявление подано не более чем в 3 ОО за пределами Крыма
            //CheckOutsideCrimeaLimit(errors);

            // проверка что заявление подано не более чем на 3 специальности
            //CheckDirectionLimit(errors);




            var model = new CheckApplicationsListViewModel();

            // Сохраняем результаты проверок заявлений в БД ФИС 
            // устанавливаем StatusID, ViolationId, ViolationErrors в Application
            // заполняем временную таблицу #tmpAppCheckResult которая будет использоваться 
            // в запросах update и select (для выборки данных о результатах проверок)
            using (DataTable tmp = new DataTable("#tmpAppCheckResult"))
            {
                // id заявления
                tmp.Columns.Add(new DataColumn("ApplicationId", typeof(int)) { AllowDBNull = false });
                // комбинация сообщений об ошибках для всех проверок
                tmp.Columns.Add(new DataColumn("ViolationErrors", typeof(string)) { AllowDBNull = true  });
                // значение, которое следует установить в поле Application.ViolationId (первая ошибка для заявления)
                tmp.Columns.Add(new DataColumn("ViolationId", typeof(int)) { AllowDBNull = false });

                tmp.BeginLoadData();

                foreach (var item in errors.Where(x => !otherErrors.ContainsKey(x.Key)))
                {
                    // добавляем строку временной таблицы ApplicationId, ViolationError, ViolationId
                    tmp.Rows.Add(item.Key, item.Value.ViolationMessages ?? "Ошибок не обнаружено", item.Value.ErrorCode);
                }

                tmp.EndLoadData();

                // пишем временную таблицу в нашу сессию БД
                tmp.WriteToTempTable();
            }

            // Собираем запрос на обновление таблицы Applications по результатам выполнения проверок
            List<SqlParameter> parameters = new List<SqlParameter>
                {
                    new SqlParameter("@pInstitutionId", institutionId)
                };

            StringBuilder sql = new StringBuilder();
            sql.AppendLine("update app");
            // если ошибок не было, то StatusID = 4 иначе - 3
            sql.AppendLine("set app.StatusID = case tmp.ViolationId when 0 then 4 else 3 end,");
            // указанный ViolationId (в tmp будет равен 0 если ошибок не было)
            sql.AppendLine("app.ViolationId = tmp.ViolationId,");
            sql.AppendLine("app.ViolationErrors = tmp.ViolationErrors,");
            sql.AppendLine("app.LastEgeDocumentsCheckDate = GETDATE(),");
            sql.AppendLine("app.LastCheckDate = GETDATE()");
            sql.AppendLine("from Application app (NOLOCK)");
            sql.AppendLine("inner join #tmpAppCheckResult tmp on app.ApplicationId = tmp.ApplicationId");
            // дополнительный фильтр по текущему ОО
            sql.AppendLine("where app.InstitutionId = @pInstitutionId");

            using (SqlCommand cmd = TransactionManager.Current.CreateCommand(CommandType.Text, sql.ToString()))
            {
                cmd.Parameters.AddRange(parameters.ToArray());
                model.ApplicationsRemoved = cmd.ExecuteNonQuery() > 0;
            }

            // загружаем данные с результатами проверки заявлений
            const string selectResultQuery =
                "select app.ApplicationId, app.ApplicationNumber, tmp.ViolationErrors " +
                " from #tmpAppCheckResult tmp inner join Application app (NOLOCK) on tmp.ApplicationId = app.ApplicationId " +
                " where app.InstitutionId = @pInstitutionId order by app.ApplicationNumber";

            model.ApplicationRecords = SqlQueryHelper.GetRecords(selectResultQuery, new[] {
                new SqlParameter("@pInstitutionId", institutionId) },
                reader =>
                new CheckApplicationRecordViewModel
                {
                    ApplicationId = reader.GetInt32(0),
                    ApplicationNumber = reader.SafeGetString(1),
                    ViolationMessage = reader.SafeGetString(2)
                });
            foreach (int applicationId in otherErrors.Keys)
            {
                ApplicationViolationInfo applicationViolationInfo = otherErrors[applicationId];

                CheckApplicationRecordViewModel applicationRecord = model.ApplicationRecords.FirstOrDefault(x => x.ApplicationId == applicationId);
                if (applicationRecord == null)
                {
                    applicationRecord = new CheckApplicationRecordViewModel();
                    model.ApplicationRecords.Add(applicationRecord);
                }

                applicationRecord.ApplicationId = applicationViolationInfo.ApplicationId;
                applicationRecord.ApplicationNumber = applicationViolationInfo.ApplicationNumber;
                applicationRecord.ViolationMessage = applicationViolationInfo.ViolationMessage;
            }

            return model;
        }

        //=======================================================================
        // проверки заявления
        //=======================================================================
         

        // Проверка ЕГЭ
        public static SPResult Check_EGE(AppResultsModel model)
        {
            if (ApplicationSQL.GetChekcEGE(model.ApplicationID))
                return ApplicationSQL.GetCheckEGEResults(model);

            return null;
        }

        // Проверка Олимпиады
        public static SPResult Check_Olympic(AppResultsModel model)
        {
            return ApplicationSQL.GetCheckOlympicResults(model);
        }

        // Проверка 5 вузов
        public static SPResult Check_5VUZ(int appId)
        {
            return ApplicationSQL.GetCheckApplication(appId);
        }

        // Проверка льгот по олимпиадам
        public static GVUZ.DAL.Model.Common.SPResult Check_Benefit(int appId)
        {
            return new OlympicsRepository().CheckBenefitOlympic(appId);
        } 
    }
}