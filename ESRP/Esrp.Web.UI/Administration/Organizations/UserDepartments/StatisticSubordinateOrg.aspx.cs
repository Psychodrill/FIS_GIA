namespace Esrp.Web.Administration.Organizations.UserDepartments
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using Esrp.Core;
    using Esrp.Core.CatalogElements;
    using Esrp.Core.Reports;
    using Esrp.Core.Reports.Email;
    using Esrp.Core.Systems;
    using Esrp.Utility;
    using Esrp.Web.Administration.Reports;
    using Esrp.Web.Administration.SqlConstructor.Organizations;

    /// <summary>
    /// The statistic subordinate org.
    /// </summary>
    public partial class StatisticSubordinateOrg : Page
    {
        #region Constants and Fields

        protected SqlConstructor_GetSubordOrgs m_SqlConstructorGetOrganizations;

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

                DataRowCollection itemsCollection = this.m_OrgTypes.Rows;

                // Не отображаем тип организации "Учредитель"
                itemsCollection[4].Delete();
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

        /// <summary>
        /// The filter status string.
        /// </summary>
        /// <returns>
        /// The filter status string.
        /// </returns>
        public string FilterStatusString()
        {
            var filterConditions = new List<string>();
            char[] charsToTrim = { ',' };
            var orgName = this.m_SqlConstructorGetOrganizations.GetVal_Str("orgName");
            if (orgName.Length > 0)
            {
                filterConditions.Add(string.Format("{0} содержит “{1}”;", "Название", orgName));
            }

            var type = string.Empty;
            var typeIds = this.Request.QueryString["TypeId"];
            if (!string.IsNullOrEmpty(typeIds))
            {
                var typeString = typeIds.Split(Convert.ToChar(","));

                foreach (var typeId in typeString)
                {
                    if (typeId == string.Empty)
                    {
                        continue;
                    }

                    var rows = this.OrgTypes.Select("id=" + typeId);
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

            var region = string.Empty;
            var regionIds = this.Request.QueryString["RegID"];
            if (!string.IsNullOrEmpty(regionIds))
            {
                var regionString = regionIds.Split(Convert.ToChar(","));

                foreach (var regionId in regionString)
                {
                    if (regionId == string.Empty)
                    {
                        continue;
                    }

                    var rows = this.Regions.Select("id=" + regionId);
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

            if (filterConditions.Count == 0)
                filterConditions.Add("Не задан");

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

        /// <summary>
        /// The get orzanization user.
        /// </summary>
        /// <returns>
        /// The get orzanization user.
        /// </returns>
        public int GetOrzanizationUser()
        {
            string orgId = string.Empty;
            string connectionString =
                ConfigurationManager.ConnectionStrings["Esrp.Core.Properties.Settings.EsrpConnectionString"].ToString();

            using (var connection = new SqlConnection(connectionString))
            {
                var cmd = new SqlCommand();
                cmd.CommandText =
                    @"SELECT
	                                    OrR.OrganizationId
                                    FROM
	                                    Account A
	                                    INNER JOIN OrganizationRequest2010 OrR ON OrR.Id = A.OrganizationId
                                    WHERE
                                        A.Login = '"
                    + this.User.Identity.Name + "'";
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

            return orgId == string.Empty ? 0 : Convert.ToInt32(orgId);
        }

        /// <summary>
        /// The is element selected.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The is element selected.
        /// </returns>
        public string IsElementSelected(int id, string name)
        {
            switch (name.ToLower())
            {
                case "regioid":
                case "kindid":
                case "typeid":
                case "CountUser":
                    int QueryId = this.m_SqlConstructorGetOrganizations.GetVal_int(name);
                    bool Res = id == QueryId;
                    return Res ? " selected " : string.Empty;
                default:
                    return string.Empty;
            }
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
            this.m_SqlConstructorGetOrganizations =
                new SqlConstructor_GetSubordinateOrganizations(this.Request.QueryString, this.GetOrzanizationUser());

            

            this.tablePager_top.ItemCount = this.GetOrgsCount();
            this.tablePager_bottom.ItemCount = this.tablePager_top.ItemCount;

            base.OnInit(e);

            this.dgStatSubordinateOrg.DataSource = this.GetOrgs();
            if (this.dgStatSubordinateOrg.DataSource != null)
            {
                this.dgStatSubordinateOrg.DataBind();
            }

            UserAccount CurrentAccount = UserAccount.GetUserAccount(Account.ClientLogin);
            this.LblOrgName.Text = "Название организации: " + CurrentAccount.OrganizationName;
        }

        /// <summary>
        /// The report_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void Report_Click(object sender, EventArgs e)
        {
            var logger = new MemoryLogger();
            try
            {
                DateTime PeriodEnd = DateTime.Now;
                int DaysToSubtract = 24; 
                DateTime PeriodBegin = PeriodEnd.AddDays(-DaysToSubtract);

                var ReportInfos = new List<ReportInfo>();
                ReportInfos.Add(
                    new ReportInfo(
                        ((LinkButton)sender).CommandArgument, 
                        @"Статистика по подведомственный учреждениям. " + this.LblOrgName.Text, 
                        PeriodBegin, 
                        PeriodEnd, 
                        this.GetOrzanizationUser().ToString()));

                string XSLTFilePath = "~\\Administration\\Reports\\Resources\\CreateExcel.xsl";
                XSLTFilePath = this.Server.MapPath(XSLTFilePath);

                string MailTemplatePath = "~\\Administration\\Reports\\Resources\\EmailTemplate_ReportViews.xml";
                MailTemplatePath = this.Server.MapPath(MailTemplatePath);

                var message = new EMailMessageViewReports(
                    "Отчеты из " + GeneralSystemManager.GetSystemName(2), 
                    ReportInfos, 
                    MailTemplatePath, 
                    XSLTFilePath, 
                    DBSettings.ConnectionString, 
                    logger, 
                    600);

                ResponseWriter.WriteStream("Report.xml", "text/xml", message.Attachments[0].ContentStream);
            }
            catch 
            {

            }
        }

        private DataTable GetOrgs()
        {
            DataTable table = null;

            string connectionString =
                ConfigurationManager.ConnectionStrings["Esrp.Core.Properties.Settings.EsrpConnectionString"].ToString();

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
                ConfigurationManager.ConnectionStrings["Esrp.Core.Properties.Settings.EsrpConnectionString"].ToString();

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