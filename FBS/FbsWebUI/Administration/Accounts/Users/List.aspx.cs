namespace Fbs.Web.Administration.Accounts.Users
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Web.UI;

    using Fbs.Core.CatalogElements;
    using Fbs.Web.Administration.Organizations;
    using Fbs.Web.Administration.SqlConstructor.UserAccounts;

    /// <summary>
    /// Список пользователей организации
    /// </summary>
    public partial class List : Page
    {
        #region Constants and Fields

        private SqlConstructor_GetUsers m_SqlConstructor_GetUsers;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Формирование строки статуса фильтра
        /// </summary>
        /// <returns>
        /// строка для фильтра
        /// </returns>
        public string FilterStatusString()
        {
            var filter = new ArrayList();

            if (!string.IsNullOrEmpty(this.Request.QueryString["login"]))
            {
                filter.Add(string.Format("Логин/E-mail “{0}”", this.Request.QueryString["login"]));
            }

            if (!string.IsNullOrEmpty(this.Request.QueryString["organizationName"]))
            {
                filter.Add(string.Format("Организация “{0}”", this.Request.QueryString["organizationName"]));
            }

            if (!string.IsNullOrEmpty(this.Request.QueryString["byip"]))
            {
                filter.Add(string.Format("Доступ “{0}”", this.GetAccessName(this.Request.QueryString["byip"])));
            }

            if (!string.IsNullOrEmpty(this.Request.QueryString["status"]))
            {
                string status = this.GetShortStatus(this.Request.QueryString["status"]);
                filter.Add(string.Format("Статус “{0}”", status));
            }

            if (!string.IsNullOrEmpty(this.Request.QueryString["eitype"]))
            {
                var types = this.GetTypes(this.Request.QueryString["eitype"]);
                filter.Add(types);
            }

            if (!string.IsNullOrEmpty(this.Request.QueryString["operatorName"]))
            {
                filter.Add(string.Format("Оператор “{0}”", this.Request.QueryString["operatorName"]));
            }

            if (!string.IsNullOrEmpty(this.Request.QueryString["hasComments"]))
            {
                if (this.Request.QueryString["hasComments"] == "1")
                {
                    filter.Add("Нет комментариев оператора");
                }

                if (this.Request.QueryString["hasComments"] == "2")
                {
                    filter.Add("Есть комментарии оператора");
                }
            }

            if (filter.Count == 0)
            {
                filter.Add("Не задан");
            }

            return string.Join("; ", (string[])filter.ToArray(typeof(string)));
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

        /// <summary>
        /// The get short status.
        /// </summary>
        /// <param name="sysStatus">
        /// The sys status.
        /// </param>
        /// <returns>
        /// The get short status.
        /// </returns>
        public string GetShortStatus(string sysStatus)
        {
            string[] arrayStatus = sysStatus.Split(Convert.ToChar(","));
            var arrayStatusResult = new List<string>();
            foreach (string status in arrayStatus)
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

            string result = string.Join(", ", arrayStatusResult.Cast<object>().Select(c => c.ToString()).ToArray());

            char[] charsToTrim = { ',' };

            result = result.Trim(charsToTrim);
            return string.IsNullOrEmpty(result) ? "[Не определён]" : result;
        }

        /// <summary>
        /// The select status.
        /// </summary>
        /// <param name="status">
        /// The status.
        /// </param>
        /// <returns>
        /// The select status.
        /// </returns>
        public string SelectStatus(string status)
        {
            if (this.Request.QueryString["status"] == status)
            {
                return "selected=\"selected\"";
            }

            return string.Empty;
        }

        /// <summary>
        /// Выбирать элемент или нет
        /// </summary>
        /// <param name="paramName">
        /// Имя парметра 
        /// </param>
        /// <param name="value">
        /// Значение параметра 
        /// </param>
        /// <returns>
        /// строка selected=\"selected\" или пусто, в зависимости от этого элемент либо выбирается, либо нет 
        /// </returns>
        public string SelectValue(string paramName, string value)
        {
            if (this.Request.QueryString[paramName] != null)
            {
                string[] selectedParam = this.Request.QueryString[paramName].Split(Convert.ToChar(","));

                foreach (string param in selectedParam)
                {
                    if (string.Compare(param, value, true) == 0)
                    {
                        return "selected=\"selected\"";
                    }
                }
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

        #endregion

        #region Methods

        /// <summary>
        /// Обработчик события "Загрузка страницы"
        /// </summary>
        /// <param name="sender">
        /// Источник события 
        /// </param>
        /// <param name="e">
        /// Источник события 
        /// </param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Request.QueryString["status"] == null)
            {
                string url = string.Format(
                    "{0}{1}", 
                    "/Administration/Accounts/Users/List.aspx?", 
                    "status=registration&status=revision&status=consideration&status=activated");
                url = this.AddQueryInUrl(url);
                this.Page.Response.Redirect(url);
                return;
            }

            if (this.Request.QueryString["eitype"] == null)
            {
                string url = string.Format("{0}{1}", "/Administration/Accounts/Users/List.aspx?", "eitype=");
                url = this.AddQueryInUrl(url);
                this.Page.Response.Redirect(url);
                return;
            }

            this.m_SqlConstructor_GetUsers = new SqlConstructor_GetUsers(this.Request.QueryString);

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
                ConfigurationManager.ConnectionStrings["Fbs.Core.Properties.Settings.FbsConnectionString"].ToString();

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

            string connectionString =
                ConfigurationManager.ConnectionStrings["Fbs.Core.Properties.Settings.FbsConnectionString"].ToString();

            using (var connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = this.m_SqlConstructor_GetUsers.GetSQL();
                cmd.Connection = connection;
                connection.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    table = MainFunctions.CreateTable_FromSQLDataReader(reader);
                }
            }

            return table;
        }

        #endregion
    }
}