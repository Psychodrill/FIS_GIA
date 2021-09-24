using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using GVUZ.Web.Helpers;
using GVUZ.Web.Infrastructure;
using GVUZ.Web.ViewModels.CompositionResults;
using GVUZ.Web.ViewModels.Shared;
using GVUZ.DAL.Dapper.ViewModel.Common;
using System;
using System.Configuration;
using GVUZ.Web.SQLDB;

namespace GVUZ.Web.ContextExtensionsSQL
{
    public static class CompositionResultSQL
    {
        public static CompositionResultsListViewModel GetCompositionResultRecords(int institutionId, 
            CompositionResultsQueryViewModel queryModel, bool fetchAll = false)
        {
            CompositionResultsFilterViewModel filterData = queryModel.Filter ?? CompositionResultsFilterViewModel.Default;
            PagerViewModel pager = queryModel.Pager ?? new PagerViewModel();
            SortViewModel sortOptions = queryModel.Sort;

            FilterStateManager.Current.Update(filterData);

            const string query = @"
                select 
                    app.ApplicationId,
                    ent.LastName,
                    ent.FirstName,
                    ent.MiddleName,
                    ent_doc.DocumentNumber IdentityDocumentNumber,
                    ct.ThemeId CompositionCode,
                    ct.Name CompositionTitle,
                    rs.Result CompositionResult,
                    app.RegistrationDate,
                    app.ApplicationNumber,
                    ent_doc.DocumentSeries IdentityDocumentSeries
                from
                    Application app (NOLOCK)
                    inner join ApplicationStatusType st (NOLOCK) on app.StatusId = st.StatusID and st.IsActiveApp = 1
                    inner join Entrant ent (NOLOCK) on app.EntrantID = ent.EntrantID
                    inner join EntrantDocument ent_doc (NOLOCK) on ent.IdentityDocumentID = ent_doc.EntrantDocumentID
                    left join ApplicationCompositionResults rs (NOLOCK) on rs.ApplicationID = app.ApplicationID
                    left join CompositionThemes ct (NOLOCK) on ct.ThemeID = rs.ThemeID and ct.[Year] = YEAR(rs.[Year])";

            #region Собираем фильтр WHERE
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<string> filter = new List<string>();
            parameters.Add(filter.FieldEqualsOrNullParamInt("app.InstitutionId", "pInstitutionId", institutionId));
            parameters.AddRange(filter.FieldInDateRangeOrNullParams("app.RegistrationDate", "pDateFrom", filterData.RegistrationDateFrom, "pDateTo", filterData.RegistrationDateTo));


            parameters.Add(filter.FieldLikeOrNullParam("app.ApplicationNumber", "pNumber", filterData.ApplicationNumber));
            parameters.Add(filter.FieldLikeOrNullParam("ent.LastName", "pLastName", filterData.LastName));


            if (filterData.SelectedCompetitiveGroup.HasValue)
            {
                SqlParameter pCompetitiveGroupId = new SqlParameter("@pCompetitiveGroupId", SqlDbType.Int);
                parameters.Add(pCompetitiveGroupId);

                // учитываем заявления входящие в конкретную конкурсную группу CompetitiveGroupID = filterData.SelectedCompetitiveGroup
                filter.Add(@"exists(select top 1 1 from CompetitiveGroup cg (NOLOCK) inner join ApplicationCompetitiveGroupItem acgi (NOLOCK) on acgi.CompetitiveGroupId = cg.CompetitiveGroupId and acgi.ApplicationId = app.ApplicationId and cg.InstitutionId = @pInstitutionId and cg.CompetitiveGroupId = @pCompetitiveGroupId)");
                pCompetitiveGroupId.Value = filterData.SelectedCompetitiveGroup.Value;
            }


            // поиск по выбранной ПК
            if (filterData.SelectedCampaign.HasValue)
            {
                filter.Add(@"exists(select top 1 1 from ApplicationCompetitiveGroupItem acgi (NOLOCK) inner join CompetitiveGroup cg (NOLOCK) on acgi.CompetitiveGroupId = cg.CompetitiveGroupId and acgi.ApplicationId = app.ApplicationID inner join Campaign cmp on cg.CampaignID = cmp.CampaignID and cmp.CampaignId = @pCampaignId inner join InstitutionAchievements ia on cmp.CampaignId = ia.CampaignID and ia.IdCategory = 12)");
                SqlParameter pCampaignId = new SqlParameter("@pCampaignId", filterData.SelectedCampaign.Value);
                parameters.Add(pCampaignId);
            }
            else
            {
                // поиск по всем ПК
                filter.Add(@"exists(select top 1 1 from ApplicationCompetitiveGroupItem acgi (NOLOCK) inner join CompetitiveGroup cg (NOLOCK) on acgi.CompetitiveGroupId = cg.CompetitiveGroupId and acgi.ApplicationId = app.ApplicationID inner join Campaign cmp on cg.CampaignID = cmp.CampaignID inner join InstitutionAchievements ia on cmp.CampaignId = ia.CampaignId and ia.IdCategory = 12)");
            }            

            // поиск по наличию результатов сочинений
            if (filterData.HasResults.GetValueOrDefault())
            {
                filter.Add(@"rs.CompositionID is not null");
            }

            // поиск не выгруженных ранее сочинений
            if (filterData.NotDownloaded.GetValueOrDefault())
            {
                filter.Add(@"rs.DownloadDate is null");
            }

            #endregion
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(query);
            sql.AppendLine("WHERE");
            sql.AppendLine(filter.JoinAnd());

            var records = fetchAll ? SqlQueryHelper.GetRecords(sql.ToString(),parameters.ToArray(), 
                MapCompositionResultRecordViewModel) :SqlQueryHelper.GetPagedRecords(sql.ToString(), 
                parameters.ToArray(), MapCompositionResultRecordViewModel, pager, sortOptions);

            return new CompositionResultsListViewModel
                {
                    Filter = filterData,
                    Pager = pager,
                    SortKey = sortOptions.SortKey,
                    SortDescending = sortOptions.SortDescending,
                    Records = records
                };
        }

        public static CompositionResultsFilterViewModel GetCompositionResultsFilter(int institutionId)
        {
            var filter = FilterStateManager.Current.GetOrCreate<CompositionResultsFilterViewModel>();
            filter.Campaigns.Items.AddRange(LoadCompositionCampaigns(institutionId));
            filter.CompetitiveGroups.Items.AddRange(EntrantApplicationSQL.LoadCompetitiveGroups(institutionId));
            return filter;
        }

        private static CompositionResultRecordViewModel MapCompositionResultRecordViewModel(SqlDataReader reader)
        {
            int index = -1;
            return new CompositionResultRecordViewModel
            {
                ApplicationId = reader.GetInt32(++index),
                LastName = reader.SafeGetString(++index),
                FirstName = reader.SafeGetString(++index),
                MiddleName = reader.SafeGetString(++index),
                IdentityDocumentNumber = reader.SafeGetString(++index),
                CompositionCode = reader.SafeGetIntAsString(++index),
                CompositionTitle = reader.SafeGetString(++index),
                CompositionResult = reader.SafeGetBool(++index),
                RegistrationDate = reader.SafeGetDateTimeAsString(++index),
                ApplicationNumber = reader.SafeGetString(++index),
                IdentityDocumentSeries = reader.SafeGetString(++index),
            };
        }

        /// <summary>
        /// Загрузка списка приемных кампаний, по которым можно фильтровать результаты сочинений
        /// </summary>
        /// <param name="institutionId">Id ОО</param>
        /// <returns>Список приемных кампаний</returns>
        private static IEnumerable<SelectListItemViewModel<int>> LoadCompositionCampaigns(int institutionId)
        {
            const string query = @"
                select
                    cmp.CampaignId,
                    cmp.Name
                from
                    Campaign cmp
                    inner join InstitutionAchievements ia on ia.CampaignID = cmp.CampaignID 
                where 
                    ia.IdCategory = 12 
                    and cmp.InstitutionID = @pInstitutionId
                group by cmp.CampaignId, cmp.Name
                order by cmp.Name";

            return SqlQueryHelper.GetRecords(query, new[] { new SqlParameter("@pInstitutionId", institutionId) }, SqlQueryHelper.MapSelectListItem<int>);
        }


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


        /// <summary>
        /// Обновить результаты сочинений
        /// </summary>
        public static void UpdateCompositionResults(int institutionId)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("[dbo].[FindEntrantCompositionMarksByInstitution]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@inst", institutionId));
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Сохраняет список в формате CSV
        /// </summary>
        public static void WriteCsv(StreamWriter writer, IEnumerable<CompositionResultRecordViewModel> records)
        {
            const char separator = '%';

            writer.Write("Surname%");
            writer.Write("Name%");
            writer.Write("SecondName%");
            writer.Write("DocumentNumber%");
            writer.Write("VariantCode%");
            writer.Write("VariantName%");
            writer.Write("Result%");
            writer.Write("URL%");
            writer.WriteLine("AppDateReg");

            foreach (var record in records)
            {
                writer.Write(record.LastName ?? string.Empty);
                writer.Write(separator);
                writer.Write(record.FirstName ?? string.Empty);
                writer.Write(separator);
                writer.Write(record.MiddleName ?? string.Empty);
                writer.Write(separator);
                writer.Write(record.IdentityDocumentNumber ?? string.Empty);
                writer.Write(separator);
                writer.Write(record.CompositionCode ?? string.Empty);
                writer.Write(separator);
                writer.Write(record.CompositionTitle.EscapeCsvField());
                writer.Write(separator);
                writer.Write(record.CompositionResultText ?? string.Empty);
                writer.Write(separator);
                writer.Write(record.BlankUrlText.EscapeCsvField());
                writer.Write(separator);
                writer.WriteLine(record.RegistrationDate ?? string.Empty);
            }    
        }

        /// <summary>
        /// Сохраняет список в формате HTML
        /// </summary>
        public static void WriteHtml(HttpServerUtilityBase server, string htmlTitle, StreamWriter writer, IEnumerable<CompositionResultRecordViewModel> records)
        {
            int[] colWidth = new[] { 10, 10, 10, 5, 5, 25, 5, 25, 5 };

            using (var html = new Html32TextWriter(writer, new string(' ', 2)))
            {
                html.WriteFullBeginTag("html");
                html.WriteFullBeginTag("head");
                html.WriteFullBeginTag("title");
                if (htmlTitle != null)
                {
                    html.Write(htmlTitle);
                }
                html.WriteEndTag("title");
                html.WriteEndTag("head");
                html.WriteFullBeginTag("body");

                html.WriteBeginTag("table");
                html.WriteAttribute("style", "border-collapse: collapse;width: 100%;");
                html.WriteAttribute("border", "1");
                html.WriteAttribute("cellpadding", "3");
                html.Write(HtmlTextWriter.TagRightChar);

                html.WriteFullBeginTag("colgroup");
                foreach (var width in colWidth)
                {
                    html.WriteBeginTag("col");
                    html.WriteAttribute("style", string.Format("width: {0}%", width));
                    html.Write(HtmlTextWriter.SelfClosingTagEnd);
                }
                html.WriteEndTag("colgroup");

                html.WriteFullBeginTag("thead");
                html.WriteFullBeginTag("tr");

                // LastName
                html.WriteFullBeginTag("th");
                html.Write("Фамилия");
                html.WriteEndTag("th");

                // FirstName
                html.WriteFullBeginTag("th");
                html.Write("Имя");
                html.WriteEndTag("th");

                // MiddleName
                html.WriteFullBeginTag("th");
                html.Write("Отчество");
                html.WriteEndTag("th");

                // IdentityDocumentNumber
                html.WriteFullBeginTag("th");
                html.Write("Номер документа, удостоверяющего личность");
                html.WriteEndTag("th");

                // CompositionCode
                html.WriteFullBeginTag("th");
                html.Write("Код темы сочинения");
                html.WriteEndTag("th");

                // CompositionTitle
                html.WriteFullBeginTag("th");
                html.Write("Тема сочинения");
                html.WriteEndTag("th");

                // CompositionResultText
                html.WriteFullBeginTag("th");
                html.Write("Результат");
                html.WriteEndTag("th");

                // BlankUrl
                html.WriteFullBeginTag("th");
                html.Write("Бланки сочинений");
                html.WriteEndTag("th");

                // RegistrationDate
                html.WriteFullBeginTag("th");
                html.Write("Дата регистрации");
                html.WriteEndTag("th");

                html.WriteEndTag("tr");
                html.WriteEndTag("thead");
                html.WriteFullBeginTag("tbody");

                foreach (var record in records)
                {
                    html.WriteFullBeginTag("tr");

                    html.WriteFullBeginTag("td");
                    html.Write(server.HtmlEncode(record.LastName ?? string.Empty));
                    html.WriteEndTag("td");

                    html.WriteFullBeginTag("td");
                    html.Write(server.HtmlEncode(record.FirstName ?? string.Empty));
                    html.WriteEndTag("td");

                    html.WriteFullBeginTag("td");
                    html.Write(server.HtmlEncode(record.MiddleName ?? string.Empty));
                    html.WriteEndTag("td");

                    html.WriteFullBeginTag("td");
                    html.Write(server.HtmlEncode(record.IdentityDocumentNumber ?? string.Empty));
                    html.WriteEndTag("td");

                    html.WriteFullBeginTag("td");
                    html.Write(server.HtmlEncode(record.CompositionCode ?? string.Empty));
                    html.WriteEndTag("td");

                    html.WriteFullBeginTag("td");
                    html.Write(server.HtmlEncode(record.CompositionTitle ?? string.Empty));
                    html.WriteEndTag("td");

                    html.WriteFullBeginTag("td");
                    html.Write(server.HtmlEncode(record.CompositionResultText ?? string.Empty));
                    html.WriteEndTag("td");

                    html.WriteFullBeginTag("td");
                    if (record.BlankUrl != null)
                    {
                        html.WriteBeginTag("a");
                        html.WriteAttribute("href", record.BlankUrl);
                        html.Write(HtmlTextWriter.TagRightChar);
                        html.Write(record.BlankUrlText);
                        html.WriteEndTag("a");    
                    }
                    html.WriteEndTag("td");

                    html.WriteFullBeginTag("td");
                    html.Write(server.HtmlEncode(record.RegistrationDate ?? string.Empty));
                    html.WriteEndTag("td");

                    html.WriteEndTag("tr");
                }

                html.WriteEndTag("tbody");
                html.WriteEndTag("table");
                html.WriteEndTag("body");
                html.WriteEndTag("html");
            }
        }
    }
}