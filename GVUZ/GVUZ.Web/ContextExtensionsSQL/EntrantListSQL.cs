using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using GVUZ.DAL.Dapper.ViewModel.Common;
using GVUZ.Web.Helpers;
using GVUZ.Web.Infrastructure;
using GVUZ.Web.SQLDB;
using GVUZ.Web.ViewModels.Entrants;
using GVUZ.Web.ViewModels.Shared;

namespace GVUZ.Web.ContextExtensionsSQL
{
    public static class EntrantListSQL
    {
        public static EntrantRecordListViewModel GetEntrantListRecords(int institutionId, EntrantRecordListQueryViewModel queryModel)
        {
            EntrantRecordListFilterViewModel filterData = queryModel.Filter ?? new EntrantRecordListFilterViewModel();
            PagerViewModel pager = queryModel.Pager ?? new PagerViewModel();
            SortViewModel sortOptions = queryModel.Sort;

            FilterStateManager.Current.Update(filterData);

            int totalUnfiltered = 0;

            if (ConfigHelper.ShowFilterStatistics())
            {
                totalUnfiltered = GetTotalEntrantsCount(institutionId);

                if (totalUnfiltered == 0)
                {
                    return new EntrantRecordListViewModel
                    {
                        Pager = new PagerViewModel(),
                        SortDescending = sortOptions.SortDescending,
                        SortKey = sortOptions.SortKey,
                        TotalRecordsCount = totalUnfiltered
                    };
                }
            }
            //pager.TotalRecords = totalUnfiltered;

            List<string> filter = new List<string>(); // sql-выражения, по одному для каждой позиции в списке
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(filter.FieldEqualsOrNullParamInt("app.InstitutionId", "institution_id", institutionId));
            parameters.Add(filter.FieldEqualsOrNullParamInt("cmp.CampaignID", "campaign_id", filterData.SelectedCampaign));
            parameters.Add(filter.FieldEqualsOrNullParamInt("registrationYear", "year", filterData.CampaignYear));
            parameters.Add(filter.FieldEqualsOrNullParamInt("cg.CompetitiveGroupID", "cmp_grp_id", filterData.SelectedCompetitiveGroup));
            parameters.Add(filter.FieldLikeOrNullParam("app.ApplicationNumber", "app_number", filterData.ApplicationNumber));
            parameters.Add(filter.FieldLikeOrNullParam("ent.LastName", "last_name", filterData.LastName));
            parameters.Add(filter.FieldLikeOrNullParam("ent.FirstName", "first_name", filterData.FirstName));
            parameters.Add(filter.FieldLikeOrNullParam("ent.MiddleName", "middle_name", filterData.MiddleName));
            parameters.AddRange(filter.FieldInDateRangeOrNullParams("app.RegistrationDate", "reg_date_from", filterData.RegistrationDateFrom, "reg_date_to", filterData.RegistrationDateTo));
            parameters.Add(filter.FieldLikeOrNullParam("ent_doc.DocumentSeries", "doc_series", filterData.DocumentSeries));
            parameters.Add(filter.FieldLikeOrNullParam("ent_doc.DocumentNumber", "doc_number", filterData.DocumentNumber));

            parameters.Add(filter.FieldEqualsOrNullParamInt("st.StatusID", "status_id", filterData.SelectedStatus));
            parameters.Add(filter.FieldEqualsOrNullParamInt("start_page", "start_page", pager.FirstRecordOffset));
            parameters.Add(filter.FieldEqualsOrNullParamInt("finish_page", "finish_page", pager.LastRecordOffset));
            //parameters.Add(filter.FieldEqualsOrNullParamInt("count_only", "count_only", 0));

            List<EntrantRecordViewModel> records = new List<EntrantRecordViewModel>();

            //if (pager.TotalRecords > 0)
            //{

            Dictionary<int, string> orderBy = new Dictionary<int, string>();
            orderBy.Add(1, "ApplicationNumber");
            orderBy.Add(2, "StatusName");
            orderBy.Add(3, "LastCheckDate");
            orderBy.Add(4, "EntrantName");
            orderBy.Add(5, "IdentityDocument");
            orderBy.Add(6, "RegistrationDate");
            orderBy.Add(7, "Rating");


            int orderKey = orderBy.Where(x => x.Value == sortOptions.SortKey).Select(x => x.Key).FirstOrDefault();

            using (SqlCommand cmd = TransactionManager.Current.CreateCommand(CommandType.StoredProcedure, "ftc_EntrantSearch"))
                {

                    int totalCount = 0;
                    cmd.Parameters.AddRange(parameters.CopyToArray());
                    cmd.Parameters.AddWithValue("count_only", 1);
                    cmd.CommandTimeout = 120;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            totalCount = reader.GetInt32(0);


                        }

                    }

                    cmd.Parameters.RemoveAt(cmd.Parameters.IndexOf("count_only"));
                    cmd.Parameters.AddWithValue("count_only", 0);
                    cmd.Parameters.AddWithValue("order_by", orderKey);
                    cmd.Parameters.AddWithValue("is_desc", sortOptions.SortDescending);


                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess))
                    {
                        cmd.CommandTimeout = 120;
                        while (reader.Read())
                        {
                            records.Add(MapRecordFromReader(reader));
                        }
                    }
                pager.TotalRecords = totalCount;
                }
            //}

            return new EntrantRecordListViewModel
            {
                Pager = pager,
                SortDescending = sortOptions.SortDescending,
                SortKey = sortOptions.SortKey,
                Records = records,
                TotalRecordsCount = totalUnfiltered
            };
        }

        private static int GetTotalEntrantsCount(int institutionId)
        {
            const string query = @"
                select count(*) from
                (
                    select distinct
	                    ent.EntrantID,
	                    app.ApplicationId,
	                    cmp.CampaignID 
                    from
	                    Entrant ent (NOLOCK)
	                    inner join Application app (NOLOCK) on app.EntrantId = ent.EntrantID
	                    left join ApplicationCompetitiveGroupItem acgi (NOLOCK) on acgi.ApplicationID = app.ApplicationID
	                    left join CompetitiveGroup cg (NOLOCK) on acgi.CompetitiveGroupID = cg.CompetitiveGroupID
	                    left join Campaign cmp (NOLOCK) on cg.CampaignID = cmp.CampaignID and cmp.InstitutionID = app.InstitutionID
	                    inner join ApplicationStatusType st (NOLOCK) on app.StatusID = st.StatusID
	                where app.InstitutionId = @pInstitutionId
                ) t";

            using (SqlCommand cmd = TransactionManager.Current.CreateCommand(CommandType.Text, query))
            {
                cmd.Parameters.Add(new SqlParameter("@pInstitutionId", institutionId));
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public static EntrantRecordListFilterViewModel GetEntrantListFilter(int institutionId)
        {
            EntrantRecordListFilterViewModel model = FilterStateManager.Current.GetOrCreate<EntrantRecordListFilterViewModel>();
            model.CompetitiveGroups.Items.AddRange(EntrantApplicationSQL.LoadCompetitiveGroups(institutionId));
            model.Campaigns.Items.AddRange(EntrantApplicationSQL.LoadInstitutionCampaigns(institutionId));
            model.Statuses.Items.AddRange(CacheManager.Current.GetOrCreateItem(CacheKeys.ApplicationStatuses, EntrantApplicationSQL.LoadApplicationStatuses));
            model.CampaignYears.Items.AddRange(LoadRegistrationYears(institutionId));
            return model;
        }

        private static EntrantRecordViewModel MapRecordFromReader(SqlDataReader reader)
        {
            int index = -1;
            return new EntrantRecordViewModel
                {
                    EntrantId = reader.GetInt32(++index),
                    EntrantName = reader.SafeGetString(++index),
                    IdentityDocument = reader.SafeGetString(++index),
                    ApplicationId = reader.GetInt32(++index),
                    ApplicationNumber = reader.SafeGetString(++index),
                    RegistrationDate = reader.SafeGetDateTimeAsString(++index),
                    CampaignName = reader.SafeGetString(++index),
                    CompetitiveGroupNames = reader.SafeGetString(++index),
                    StatusName = reader.SafeGetString(++index),
                    AllowEdit = reader.SafeGetBool(++index).GetValueOrDefault(),
                    IsCampaignFinished = reader.SafeGetBool(++index).GetValueOrDefault()
                };
        }

        public static EntrantDetailsViewModel GetEntrantDetails(int institutionId, int entrantId)
        {
            if (entrantId <= 0)
            {
                return null;
            }

            #region Текст sql-запросов
            // сведения о заявителе
            const string entrantQuery = @"
                select top 1
                    ent.EntrantID,
                    rtrim(ISNULL(ent.LastName, '') + ' ' + ISNULL(ent.FirstName, '') + ' ' + ISNULL(ent.MiddleName, '')) EntrantName,
                    lower(gnd.Name) Gender,
                    ent_doc_typ.Name IdentityDocumentType,
                    ltrim(ISNULL(ent_doc.DocumentSeries, '') + ' ' + ISNULL(ent_doc.DocumentNumber, '')) IdentityDocumentNumber,
                    ent_doc_id.BirthDate,
                    ent_doc_id.BirthPlace,
                    ent.UID

                from Entrant ent (NOLOCK)
                    inner join Application app (NOLOCK) on app.EntrantId = ent.EntrantID
                    left join EntrantDocument ent_doc (NOLOCK) on ent.IdentityDocumentId = ent_doc.EntrantDocumentId
                    left join IdentityDocumentType ent_doc_typ (NOLOCK) on ent_doc.DocumentTypeId = ent_doc_typ.IdentityDocumentTypeID
                    left join EntrantDocumentIdentity ent_doc_id (NOLOCK) on ent_doc_id.EntrantDocumentId = ent_doc.EntrantDocumentId
                    left join GenderType gnd (NOLOCK) on ent.GenderId = gnd.GenderId
                where 
                    ent.EntrantId = @pEntrantId 
                    and app.InstitutionID = @pInstitutionID";

            // список заявлений
            const string applicationListQuery = @"
                select distinct
                    app.ApplicationId,
                    app.ApplicationNumber,
                    st.Name StatusName,
                    cmp.Name CampaignName,
                    substring((
                            select t.CompetitiveGroupName as [text()]
                            from (
                                select ', ' + cg.Name CompetitiveGroupName,
                                       ROW_NUMBER() over(PARTITION BY cg.CompetitiveGroupId order by acgi.ApplicationId) is_distinct
                                from
                                   ApplicationCompetitiveGroupItem acgi (NOLOCK) 
                                   inner join CompetitiveGroup cg (NOLOCK) on acgi.CompetitiveGroupId = cg.CompetitiveGroupId
                                where
                                   acgi.ApplicationId = app.ApplicationId
                            ) t 
                            where t.is_distinct = 1 order by t.CompetitiveGroupName
                           for xml path('')
                    ), 3, 8000) CompetitiveGroupName,
                    app.RegistrationDate,
                    bnf.Name BenefitName,
                    cast(case when (cmp.StatusID = 2 or st.StatusId = 8) then 0 else 1 end as bit) AllowEdit,
                    cast(case cmp.StatusId when 2 then 1 else 0 end as bit) IsCampaignFinished
                from
                    Application app (NOLOCK)
                    inner join Entrant ent (NOLOCK) on app.EntrantId = ent.EntrantId
                    inner join ApplicationStatusType st (NOLOCK) on app.StatusId = st.StatusId
                    left join ApplicationCompetitiveGroupItem acgi (NOLOCK) on acgi.ApplicationId = app.ApplicationId
                    left join CompetitiveGroup cg (NOLOCK) on acgi.CompetitiveGroupId = cg.CompetitiveGroupId
                    left join Campaign cmp (NOLOCK) on cg.CampaignId = cmp.CampaignId and cmp.InstitutionId = app.InstitutionId
                    left join ApplicationEntranceTestDocument aetd (NOLOCK) on app.ApplicationId = aetd.Applicationid
                    and aetd.BenefitId is not null
                    left join Benefit bnf (NOLOCK) on aetd.BenefitId = bnf.BenefitId
                where 
                    app.InstitutionId= @pInstitutionId and app.EntrantID = @pEntrantId
                    order by app.RegistrationDate, app.ApplicationNumber"; 
            #endregion

            SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@pInstitutionId", institutionId) {Value = institutionId},
                    new SqlParameter("@pEntrantId", entrantId) {Value = entrantId}
                };

            EntrantDetailsViewModel model = new EntrantDetailsViewModel();

            if (SqlQueryHelper.SelectOne(entrantQuery, parameters, reader => MapEntrantDetailsViewModel(reader, model)))
            {
                model.ApplicationList = SqlQueryHelper.GetRecords(applicationListQuery, parameters.CopyToArray(), MapEntrantDetailsApplicationViewModel);
                return model;
            }

            return model;
        }

        private static void MapEntrantDetailsViewModel(SqlDataReader reader, EntrantDetailsViewModel model)
        {
            int index = -1;
            model.EntrantId = reader.GetInt32(++index);
            model.EntrantName = reader.SafeGetString(++index);
            model.Gender = reader.SafeGetString(++index);
            model.IdentityDocumentType = reader.SafeGetString(++index);
            model.IdentityDocumentNumber = reader.SafeGetString(++index);
            model.DateOfBirth = reader.SafeGetDateTimeAsString(++index);
            model.PlaceOfBirth = reader.SafeGetString(++index);
            model.UID = reader.SafeGetString(++index);
        }

        private static EntrantDetailsApplicationViewModel MapEntrantDetailsApplicationViewModel(SqlDataReader reader)
        {
            int index = -1;
            return new EntrantDetailsApplicationViewModel
                {
                    ApplicationId = reader.GetInt32(++index),
                    ApplicationNumber = reader.SafeGetString(++index),
                    StatusName = reader.SafeGetString(++index),
                    Campaign = reader.SafeGetString(++index),
                    CompetitiveGroup = reader.SafeGetString(++index),
                    RegistrationDate = reader.SafeGetDateTimeAsString(++index),
                    Benefit = reader.SafeGetString(++index),
                    AllowEdit = reader.SafeGetBool(++index).GetValueOrDefault(),
                    IsCampaignFinished = reader.SafeGetBool(++index).GetValueOrDefault()
                };
        }

        public static bool UpdateEntrantUid(int institutionId, int entrantId, string uid, out string validationError)
        {
            const string validateQuery = @"
                select TOP 1 1 
                from 
                    Entrant ent  (NOLOCK)
                    inner join Application app (NOLOCK) on app.EntrantID = ent.EntrantId 
                where 
                    ent.EntrantId <> @pEntrantId 
                    and app.InstitutionId = @pInstitutionId
                    and ent.UID is not null
                    and lower(ent.UID) = lower(@pUID)";

            SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@pEntrantId", entrantId) {Value = entrantId},
                    new SqlParameter("@pInstitutionId", institutionId) {Value = institutionId},
                    new SqlParameter("@pUID", uid) {Value = string.IsNullOrEmpty(uid) ? (object) DBNull.Value : uid},
                };

            if (!string.IsNullOrEmpty(uid))
            {
                using (SqlCommand cmd = TransactionManager.Current.CreateCommand(CommandType.Text, validateQuery))
                {
                    cmd.Parameters.AddRange(parameters);

                    object res = cmd.ExecuteScalar();

                    if (res != null && res != DBNull.Value)
                    {
                        validationError = "Данный идентификатор уже присвоен для другого абитуриента";
                        return false;
                    }
                }
            }

            validationError = null;

            const string updateQuery = @"
                update ent 
                set 
	                ent.UID = @pUid 
                from Entrant ent  (NOLOCK)
	                 inner join Application app (NOLOCK) on app.EntrantId = ent.EntrantId 
                where 
                     app.InstitutionId = @pInstitutionId 
                     and ent.EntrantId = @pEntrantId";

            using (SqlCommand cmd = TransactionManager.Current.CreateCommand(CommandType.Text, updateQuery))
            {
                cmd.Parameters.AddRange(parameters.CopyToArray());
                return cmd.ExecuteNonQuery() == 1;
            }
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
    }
}