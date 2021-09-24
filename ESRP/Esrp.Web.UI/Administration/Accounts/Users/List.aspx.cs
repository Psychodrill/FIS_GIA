namespace Esrp.Web.Administration.Accounts.Users
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Web.UI;

    using Esrp.Core.CatalogElements;
    using Esrp.Core.Systems;
    using Esrp.Web.Administration.Organizations;
    using Esrp.Web.Administration.SqlConstructor.UserAccounts;
    using System.Web;
    using Esrp.Core.Loggers;
    using Esrp.Core;
    using Esrp.Core.Users;

    /// <summary>
    /// Список пользователей организации
    /// </summary>
    public partial class List : Page
    {
        #region Constants and Fields

        private SqlConstructor_GetUsers m_SqlConstructor_GetUsers;

        #endregion

        #region Methods

        /// <summary>
        /// Обработчик события "Загрузка страницы"
        /// </summary>
        /// <param name="sender"> Источник события </param>
        /// <param name="e"> Источник события </param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!GeneralSystemManager.IsUserActivated(this.User.Identity.Name))
            {
                this.Page.Response.Redirect("/");
                return;
            }

            if (!this.User.IsInRole("ViewUserAccount"))
            {
                if (this.User.IsInRole("ViewUserOUAccount"))
                {
                    this.Page.Response.Redirect("/Administration/Accounts/Users/ListOU.aspx");
                    return;
                }

                if (this.User.IsInRole("ViewUserISAccount"))
                {
                    this.Page.Response.Redirect("/Administration/Accounts/Users/ListIS.aspx");
                    return;
                }

                this.Page.Response.Redirect("/");
                return;
            }

            if (this.Request.QueryString["status"] == null)
            {
                var url = string.Format(
                    "{0}{1}",
                    "/Administration/Accounts/Users/List.aspx?",
                    "status=registration&status=revision&status=consideration&status=activated");
                url = this.AddQueryInUrl(url);
                this.Page.Response.Redirect(url);
                return;
            }

            if (this.Request.QueryString["eitype"] == null)
            {
                var url = string.Format(
                    "{0}{1}",
                    "/Administration/Accounts/Users/List.aspx?",
                    "eitype=");
                url = this.AddQueryInUrl(url);
                this.Page.Response.Redirect(url);
                return;
            }

            this.m_SqlConstructor_GetUsers = new SqlConstructor_GetUsers(
                this.Request.QueryString, this.User.Identity.Name);

            this.rptEducationTypes.DataSource = CatalogDataAcessor.GetAll(CatalogDataAcessor.Catalogs.OrganizationTypes);
            this.rptEducationTypes.DataBind();

            this.tablePager_top.ItemCount = this.GetUserCount();
            this.tablePager_bottom.ItemCount = this.tablePager_top.ItemCount;

            this.OnInit(e);

            this.dgUserList.DataSource = this.GetUsers();
            
            if (this.dgUserList.DataSource != null)
            {
                this.dgUserList.DataBind();
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

        private int GetUserCount()
        {
            int orgCount = 0;

            string connectionString =
                ConfigurationManager.ConnectionStrings["Esrp.Core.Properties.Settings.EsrpConnectionString"].ToString();

            using (var connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = this.m_SqlConstructor_GetUsers.GetCountOrgsSQL();
                cmd.Connection = connection;
                connection.Open();

                cmd.ExecuteNonQuery();

                orgCount = (int)cmd.Parameters["rowCount"].Value;
            }

            return orgCount;
        }

        private DataTable GetUsers()
        {
            DataTable table = null;

            var connectionString =
                ConfigurationManager.ConnectionStrings["Esrp.Core.Properties.Settings.EsrpConnectionString"].ToString();

            using (var connection = new SqlConnection(connectionString))
            {
                var cmd = this.m_SqlConstructor_GetUsers.GetSQL();
                cmd.Connection = connection;
                connection.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    table = MainFunctions.CreateTable_FromSQLDataReader(reader);
                }
            }
            foreach (DataRow row in table.Rows)
            {
                if (Convert.ToBoolean(ConfigurationManager.AppSettings["LogUserView"]))
                {
                    AccountEventLogger.LogAccountViewEvent(row["Login"].ToString());
                }
            }
            return table;
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

        public string GetTypes(string types)
        {
            var arrayTypes = types.Split(Convert.ToChar(","));
            var arrayTypesResult = new List<string>();
            foreach (var type in arrayTypes)
            {
                var eitype = 0;
                if (int.TryParse(type, out eitype))
                {
                    arrayTypesResult.Add(string.Format("Тип ОУ “{0}”", OrgTypeDataAccessor.GetName(eitype)));
                }
            }

            var result = string.Join(", ", arrayTypesResult.Cast<object>().Select(c => c.ToString()).ToArray());

            char[] charsToTrim = { ',' };

            result = result.Trim(charsToTrim);

            return string.IsNullOrEmpty(result) ? string.Empty : result;
        }

        public string GetShortStatus(string sysStatus)
        {
            var arrayStatus = sysStatus.Split(Convert.ToChar(","));
            var arrayStatusResult = new List<string>();
            foreach (var status in arrayStatus)
            {
                switch (status)
                {
                    case "registration":
                        arrayStatusResult.Add("На регистрации");
                        break;
                    case "consideration":
                        arrayStatusResult.Add("На согласовании");
                        break;
                    case "revision":
                        arrayStatusResult.Add("На доработке");
                        break;
                    case "activated":
                        arrayStatusResult.Add("Действующий");
                        break;
                    case "deactivated":
                        arrayStatusResult.Add("Отключенный");
                        break;
                }
            }

            var result = string.Join(", ", arrayStatusResult.Cast<object>().Select(c => c.ToString()).ToArray());

            char[] charsToTrim = { ',' };

            result = result.Trim(charsToTrim);
            return string.IsNullOrEmpty(result) ? "[Не определён]" : result;
        }

        /// <summary>
        /// Формирование строки статуса фильтра
        /// </summary>
        /// <returns>строка для фильтра</returns>
        public string FilterStatusString()
        {
            var filter = new ArrayList();

            if (!string.IsNullOrEmpty(Request.QueryString["login"]))
            {
                filter.Add(string.Format("Логин/E-mail “{0}”", Request.QueryString["login"]));
            }

            if (!string.IsNullOrEmpty(Request.QueryString["organizationName"]))
            {
                filter.Add(string.Format("Организация “{0}”", Request.QueryString["organizationName"]));
            }

            if (!string.IsNullOrEmpty(Request.QueryString["byip"]))
            {
                filter.Add(string.Format("Доступ “{0}”", this.GetAccessName(this.Request.QueryString["byip"])));
            }

            if (!string.IsNullOrEmpty(Request.QueryString["status"]))
            {
                var status = this.GetShortStatus(this.Request.QueryString["status"]);
                filter.Add(string.Format("Статус “{0}”", status));
            }

            if (!string.IsNullOrEmpty(this.Request.QueryString["eitype"]))
            {
                var types = this.GetTypes(this.Request.QueryString["eitype"]);
                filter.Add(types);
            }

            if (!string.IsNullOrEmpty(Request.QueryString["operatorName"]))
            {
                filter.Add(string.Format("Оператор “{0}”", Request.QueryString["operatorName"]));
            }

            if (!string.IsNullOrEmpty(Request.QueryString["hasComments"]))
            {
                if (Request.QueryString["hasComments"] == "1")
                {
                    filter.Add("Нет комментариев оператора");
                }

                if (Request.QueryString["hasComments"] == "2")
                {
                    filter.Add("Есть комментарии оператора");
                }
            }

            if (filter.Count == 0)
            {
                filter.Add("Не задан");
            }

            return string.Join("; ", ((string[])filter.ToArray(typeof(string))));
        }

        public string SelectStatus(string status)
        {
            if (Request.QueryString["status"] == status)
            {
                return "selected=\"selected\"";
            }

            return string.Empty;
        }

        public string GetAccessName(object isFixedIp)
        {
            if (!Convert.ToBoolean(Convert.ToInt16(isFixedIp)))
            {
                return "VPN";
            }
            else
            {
                return "IP";
            }
        }
    }
}