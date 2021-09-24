namespace Esrp.Web.Administration.Organizations
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Web.UI;

    using Esrp.Core.CatalogElements;
    using Esrp.Web.Administration.SqlConstructor;
    using Esrp.Web.Administration.SqlConstructor.Organizations;

    using Esrp.Services;
    using System.Web;

    /// <summary>
    /// The request list.
    /// </summary>
    public partial class RequestList : Page
    {
        #region Constants and Fields

        /// <summary>
        /// объект для обращения к сервису заявок
        /// </summary>
        private readonly RequestsService requestsService = new RequestsService();

        protected SqlConstructor_GetRequests m_SqlConstructorGetRequests;

        private DataTable _accountStatuses;

        #endregion

        #region Properties

        private DataTable AccountStatuses
        {
            get
            {
                if (this._accountStatuses == null)
                {
                    this._accountStatuses = CatalogDataAcessor.GetAll(CatalogDataAcessor.Catalogs.AccountStatuses);
                }

                return this._accountStatuses;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Отображение полей по которым фильтруем
        /// </summary>
        /// <returns>
        /// Строка с полями, по которым фильтруем
        /// </returns>
        public string FilterStatusString()
        {
            var status = string.Empty;
            var typeId = this.Request.QueryString["StatusId"];
            if (!string.IsNullOrEmpty(typeId))
            {
                var typeString = typeId.Split(Convert.ToChar(","));

                foreach (var statusId in typeString)
                {
                    if (statusId == string.Empty)
                    {
                        continue;
                    }

                    var rows = this.AccountStatuses.Select("StatusID=" + statusId);
                    if (rows.Length > 0)
                    {
                        status += string.Format("“{0}”,", rows[0]["Name"]);
                    }
                }

                char[] charsToTrim = { ',' };
                status = status.Trim(charsToTrim);
            }

            if (!string.IsNullOrEmpty(status))
            {
                status = string.Format("{0}{1}; ", "Статус :", status);
            }

            var years = this.Request.QueryString["YearInRequests"];
            if (!string.IsNullOrEmpty(years))
            {
                years = string.Format("{0}{1}; ", "Год: ", years);
            }

            return string.Format("{0}{1}", status, years);
        }

        /// <summary>
        /// Выбирать элемент или нет
        /// </summary>
        /// <param name="paramName"> Имя парметра </param>
        /// <param name="value"> Значение параметра </param>
        /// <returns> строка selected=\"selected\" или пусто, в зависимости от этого элемент либо выбирается, либо нет </returns>
        public string SelectValue(string paramName, string value)
        {
            if (this.Request.QueryString[paramName] != null)
            {
                var selectedParam = Request.QueryString[paramName].Split(Convert.ToChar(","));

                foreach (var param in selectedParam)
                {
                    if (string.Compare(param, value, true) == 0)
                    {
                        return "selected=\"selected\"";
                    }
                }
            }

            return string.Empty;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The on init.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        protected override void OnInit(EventArgs e)
        {
            this.rYearInRequests.DataSource = this.requestsService.GetYearsInRequests();
            this.rYearInRequests.DataBind();
            this.m_SqlConstructorGetRequests = new SqlConstructor_GetRequests(this.Request.QueryString);
            this.RepeaterOrgTypes.DataSource = this.AccountStatuses;
            this.RepeaterOrgTypes.DataBind();

            this.tablePager_top.ItemCount = this.tablePager_bottom.ItemCount = this.GetRequestsCount();

            base.OnInit(e);

            // список заявок
            this.dgReqList.DataSource = this.GetRequests();
            if (this.dgReqList.DataSource != null)
            {
                this.dgReqList.DataBind();
            }
        }

        /// <summary>
        /// Обработчик события "Загрузка страницы"
        /// </summary>
        /// <param name="sender"> Источник события </param>
        /// <param name="e"> Источник события </param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Request.QueryString["StatusId"] == null)
            {
                var url = string.Format(
                    "{0}{1}",
                    "/Administration/Requests/RequestList.aspx?",
                    "StatusId=1&StatusId=2&StatusId=3&StatusId=4");
                url = this.AddQueryInUrl(url);
                this.Page.Response.Redirect(url);
                return;
            }


            if (this.Request.QueryString["YearInRequests"] == null)
            {
                var years = this.requestsService.GetYearsInRequests();
                var existing = years.FirstOrDefault(x => x.Year == DateTime.Now.Year.ToString());

                if (existing != null)
                {
                    var url = string.Format(
                        "{0}{1}{2}", "/Administration/Requests/RequestList.aspx?", "YearInRequests=", DateTime.Now.Year);
                    url = this.AddQueryInUrl(url);
                    this.Page.Response.Redirect(url);
                    return;
                }
            }
        }

        private string AddQueryInUrl(string url)
        {
            if (!string.IsNullOrEmpty(this.Request.Url.Query))
            {
                url = string.Format("{0}&{1}", url, this.Request.Url.Query.Trim(Convert.ToChar("?")));
            }

            return url;
        }

        private DataTable GetRequests()
        {
            using (var dbExecutor = new EsrpDbExecutor(this.m_SqlConstructorGetRequests))
            {
                using (SqlDataReader reader = dbExecutor.CreateCommand().ExecuteReader())
                {
                    return MainFunctions.CreateTable_FromSQLDataReader(reader);
                }
            }
        }

        private int GetRequestsCount()
        {
            using (var dbExecutor = new EsrpDbExecutor(this.m_SqlConstructorGetRequests))
            {
                SqlCommand sqlCommand = dbExecutor.CreateCountRowsCommand();
                sqlCommand.ExecuteNonQuery();
                return (int)sqlCommand.Parameters["rowCount"].Value;
            }
        }

        /// <summary>
        /// The get current url.
        /// </summary>
        /// <param name="excludeParamName">
        /// The exclude param name.
        /// </param>
        /// <returns>
        /// The get current url.
        /// </returns>
        public string GetCurrentUrl(string excludeParamName)
        {
            string url = string.Empty;
            for (int i = 0; i < this.Page.Request.QueryString.Count; i++)
            {
                string key = this.Page.Request.QueryString.GetKey(i);
                if (!string.IsNullOrEmpty(key))
                {
                    string val = HttpUtility.UrlEncode(this.Page.Request.QueryString.Get(key));

                    if (key.ToLower() != excludeParamName.ToLower())
                    {
                        url += string.Format("{2}{0}={1}", key, val, url.Length == 0 ? "?" : "&");
                    }
                }
            }

            url = this.Page.Request.Path + url + string.Format("{1}{0}=", excludeParamName, url.Length == 0 ? "?" : "&");

            return url;
        }

        #endregion
    }
}