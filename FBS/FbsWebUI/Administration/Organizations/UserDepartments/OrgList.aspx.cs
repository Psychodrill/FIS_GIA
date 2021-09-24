namespace Fbs.Web.Administration.Organizations.UserDepartments
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Web.UI;

    using Fbs.Core.CatalogElements;
    using Fbs.Web.Administration.SqlConstructor.Organizations;

    /// <summary>
    /// The org list.
    /// </summary>
    public partial class OrgList : Page
    {
        #region Constants and Fields

        protected SqlConstructor_GetOrganizations m_SqlConstructorGetOrganizations;

        private DataTable m_OrgTypes;

        private DataTable m_Regions;

        #endregion

        #region Properties

        private DataTable OrgTypes
        {
            get
            {
                if (this.m_OrgTypes == null)
                {
                    this.m_OrgTypes = CatalogDataAcessor.GetAll(CatalogDataAcessor.Catalogs.OrganizationTypes);
                }

                return this.m_OrgTypes;
            }
        }

        private DataTable Regions
        {
            get
            {
                if (this.m_Regions == null)
                {
                    this.m_Regions = RegionDataAcessor.GetAllInEtalon(null);
                }

                return this.m_Regions;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The get orzanization user.
        /// </summary>
        /// <returns>
        /// The get orzanization user.
        /// </returns>
        public string GetOrzanizationUser()
        {
            string orgId = string.Empty;
            string connectionString =
                ConfigurationManager.ConnectionStrings["Fbs.Core.Properties.Settings.FbsConnectionString"].ToString();

            using (var connection = new SqlConnection(connectionString))
            {
                var cmd = new SqlCommand();
                cmd.CommandText = @"select OrganizationId from Account where Login = '" + this.User.Identity.Name + "'";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                connection.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    orgId = dr[0].ToString();
                }

                dr.Close();
            }

            return orgId;
        }

        /// <summary>
        /// Отображение полей по которым фильтруем
        /// </summary>
        /// <returns>
        /// Строка с полями, по которым фильтруем
        /// </returns>
        public string FilterStatusString()
        {
            var filterConditions = new List<string>();
            char[] charsToTrim = { ',' };
            string orgName = this.m_SqlConstructorGetOrganizations.GetVal_Str("orgName");
            if (orgName.Length > 0)
            {
                filterConditions.Add(string.Format("{0} содержит “{1}”;", "Название", orgName));
            }

            string type = string.Empty;
            string typeIds = this.Request.QueryString["TypeId"];
            if (!string.IsNullOrEmpty(typeIds))
            {
                string[] typeString = typeIds.Split(Convert.ToChar(","));

                foreach (string typeId in typeString)
                {
                    if (typeId == string.Empty)
                    {
                        continue;
                    }

                    DataRow[] rows = this.OrgTypes.Select("id=" + typeId);
                    if (rows.Length > 0)
                    {
                        type += string.Format("“{0}”,", rows[0]["Name"]);
                    }
                }

                type = type.Trim(charsToTrim);
            }

            if (!string.IsNullOrEmpty(type))
            {
                type = string.Format("{0}{1}; ", "Тип :", type);
                filterConditions.Add(type);
            }

            string region = string.Empty;
            string regionIds = this.Request.QueryString["RegID"];
            if (!string.IsNullOrEmpty(regionIds))
            {
                string[] regionString = regionIds.Split(Convert.ToChar(","));

                foreach (string regionId in regionString)
                {
                    if (regionId == string.Empty)
                    {
                        continue;
                    }

                    DataRow[] rows = this.Regions.Select("id=" + regionId);
                    if (rows.Length > 0)
                    {
                        region += string.Format("“{0}”,", rows[0]["Name"]);
                    }
                }

                region = region.Trim(charsToTrim);
            }

            if (!string.IsNullOrEmpty(region))
            {
                region = string.Format("{0}{1}; ", "Регион :", region);
                filterConditions.Add(region);
            }

            var userCount = this.m_SqlConstructorGetOrganizations.GetVal_int("UserCount");
            if (userCount > 0)
            {
                filterConditions.Add(userCount == 1 ? "Количество пользователей=0;" : "Количество пользователей>0;");
            }

            var activatedUsers = this.m_SqlConstructorGetOrganizations.GetVal_int("ActivatedUsers");
            if (activatedUsers > 0)
            {
                filterConditions.Add(activatedUsers == 1 ? "Количество активированных пользователей=0;" : "Количество активированных пользователей>0;");
            }

            return string.Join(" и ", filterConditions.ToArray());
        }

        /// <summary>
        /// Выбирать элемент или нет
        /// </summary>
        /// <param name="paramName"> Имя парметра  </param>
        /// <param name="value"> Значение параметра  </param>
        /// <returns> строка selected=\"selected\" или пусто, в зависимости от этого элемент либо выбирается, либо нет  </returns>
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
            if (!string.IsNullOrEmpty(this.GetOrzanizationUser()))
            {
                this.m_SqlConstructorGetOrganizations =
                    new SqlConstructor_GetOrganizations_Department(
                        this.Request.QueryString, Convert.ToInt32(this.GetOrzanizationUser()));

                this.RepeaterOrgTypes.DataSource = this.OrgTypes;
                this.RepeaterOrgTypes.DataBind();

                this.RepeaterRegions.DataSource = this.Regions;
                this.RepeaterRegions.DataBind();

                this.tablePager_top.ItemCount = this.GetOrgsCount();
                this.tablePager_bottom.ItemCount = this.tablePager_top.ItemCount;

                base.OnInit(e);

                this.dgOrgList.DataSource = this.GetOrgs();
                if (this.dgOrgList.DataSource != null)
                {
                    this.dgOrgList.DataBind();
                }
            }
        }

        /// <summary>
        /// Обработчик события "Загрузка страницы"
        /// </summary>
        /// <param name="sender"> Источник события </param>
        /// <param name="e"> Источник события </param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Request.QueryString["RegID"] == null)
            {
                var url = string.Format(
                    "{0}{1}",
                    "/Administration/Organizations/Administrators/OrgList.aspx?",
                    "RegID=");
                url = this.AddQueryInUrl(url);
                this.Page.Response.Redirect(url);
                return;
            }

            if (this.Request.QueryString["TypeId"] == null)
            {
                var url = string.Format(
                    "{0}{1}",
                    "/Administration/Organizations/Administrators/OrgList.aspx?",
                    "TypeId=");
                url = this.AddQueryInUrl(url);
                this.Page.Response.Redirect(url);
                return;
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

        private DataTable GetOrgs()
        {
            DataTable table = null;

            string connectionString =
                ConfigurationManager.ConnectionStrings["Fbs.Core.Properties.Settings.FbsConnectionString"].ToString();

            using (var connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = this.m_SqlConstructorGetOrganizations.GetSQL();
                cmd.Connection = connection;
                connection.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    table = MainFunctions.CreateTable_FromSQLDataReader(reader);
                }
            }

            return table;
        }

        private int GetOrgsCount()
        {
            int orgCount = 0;

            string connectionString =
                ConfigurationManager.ConnectionStrings["Fbs.Core.Properties.Settings.FbsConnectionString"].ToString();

            using (var connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = this.m_SqlConstructorGetOrganizations.GetCountOrgsSQL();
                cmd.Connection = connection;
                connection.Open();

                cmd.ExecuteNonQuery();

                orgCount = (int)cmd.Parameters["rowCount"].Value;
            }

            return orgCount;
        }

        #endregion
    }
}