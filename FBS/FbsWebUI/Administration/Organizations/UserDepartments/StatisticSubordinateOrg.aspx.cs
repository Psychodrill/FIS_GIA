using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using Fbs.Core;
using Fbs.Core.CatalogElements;
using Fbs.Core.Reports;
using Fbs.Core.Reports.Email;
using Fbs.Web.Administration.SqlConstructor.Organizations;
using Fbs.Web.Helpers;

namespace Fbs.Web.Administration.Organizations.UserDepartments
{
    public partial class StatisticSubordinateOrg : System.Web.UI.Page
    {
        protected SqlConstructor_GetSubordOrgs m_SqlConstructorGetOrganizations;

        protected override void OnInit(EventArgs e)
        {
            m_SqlConstructorGetOrganizations = new SqlConstructor_GetSubordinateOrganizations(Request.QueryString, GetOrzanizationUser());

            RepeaterRegions.DataSource = Regions;
            RepeaterRegions.DataBind();

            tablePager_top.ItemCount = GetOrgsCount();
            tablePager_bottom.ItemCount = tablePager_top.ItemCount;

            base.OnInit(e);

            dgStatSubordinateOrg.DataSource = GetOrgs();
            if (dgStatSubordinateOrg.DataSource != null)
                dgStatSubordinateOrg.DataBind();

            UserAccount CurrentAccount = UserAccount.GetUserAccount(Account.ClientLogin);
            LblOrgName.Text = "Название организации: " + CurrentAccount.OrganizationName;
        }

        public int GetOrzanizationUser()
        {
            string orgId = "";
            string connectionString = ConfigurationManager.ConnectionStrings["Fbs.Core.Properties.Settings.FbsConnectionString"].ToString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT
                                      O.Id
                                    FROM
                                      Account A
                                      INNER JOIN Organization2010 O ON O.Id = A.OrganizationId
                                    WHERE
                                        A.Login = '" + User.Identity.Name + "'";
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

        public string IsElementSelected(int id, string name)
        {
            switch (name.ToLower())
            {
                case "regioid":
                case "kindid":
                case "typeid":
                case "CountUser":
                    int QueryId = m_SqlConstructorGetOrganizations.GetVal_int(name);
                    bool Res = (id == QueryId);
                    return Res ? " selected " : "";
                default: return "";
            }
        }

        private DataTable GetOrgs()
        {
            DataTable table = null;

            string connectionString = ConfigurationManager.ConnectionStrings["Fbs.Core.Properties.Settings.FbsConnectionString"].ToString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = m_SqlConstructorGetOrganizations.GetSQL();
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

            string connectionString = ConfigurationManager.ConnectionStrings["Fbs.Core.Properties.Settings.FbsConnectionString"].ToString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = m_SqlConstructorGetOrganizations.GetCountOrgsSQL();
                cmd.Connection = connection;
                connection.Open();

                cmd.ExecuteNonQuery();

                orgCount = (int)cmd.Parameters["rowCount"].Value;
            }
            return orgCount;
        }

        #region регионы
        private DataTable m_Regions;
        private DataTable Regions
        {
            get
            {
                if (m_Regions == null)
                    m_Regions = RegionDataAcessor.GetAllInEtalon(null);
                return m_Regions;

            }
        }
        #endregion

        #region типы организаций
        private DataTable m_OrgTypes;
        private DataTable OrgTypes
        {
            get
            {
                if (m_OrgTypes == null)
                    m_OrgTypes = CatalogDataAcessor.GetAll(CatalogDataAcessor.Catalogs.OrganizationTypes);
                DataRowCollection itemsCollection = m_OrgTypes.Rows;
                //Не отображаем тип организации "Учредитель"
                itemsCollection[4].Delete();
                return m_OrgTypes;
            }
        }

        #endregion



        public string FilterStatusString()
        {
            List<string> filterConditions = new List<string>();
            {
                string orgName = m_SqlConstructorGetOrganizations.GetVal_Str("orgName");
                if (orgName.Length > 0)
                {
                    filterConditions.Add(string.Format("{0} содержит “{1}”;", "Название", orgName));
                }
            }

            {

                string TypeId = Request.QueryString["TypeId"];
                if (!String.IsNullOrEmpty(TypeId))
                {
                    DataRow[] Rows = OrgTypes.Select("id=" + TypeId);
                    if (Rows.Length > 0)
                    {
                        filterConditions.Add(string.Format("{0}=“{1}”;", "Тип", Rows[0]["Name"].ToString()));
                    }
                }
            }

            {
                string RegionId = Request.QueryString["RegionId"];
                if (!String.IsNullOrEmpty(RegionId))
                {
                    DataRow[] Rows = Regions.Select("id=" + RegionId);
                    if (Rows.Length > 0)
                    {
                        filterConditions.Add(string.Format("{0}=“{1}”;", "Регион", Rows[0]["Name"].ToString()));
                    }
                }
            }

            {
                int userCount = m_SqlConstructorGetOrganizations.GetVal_int("CountUser");
                if (userCount > 0)
                {
                    if (userCount == 1)
                        filterConditions.Add("Количество пользователей=0;");
                    else
                        filterConditions.Add("Количество пользователей>0;");
                }
            }
            return string.Join(" и ", filterConditions.ToArray());
        }

        protected void Report_Click(object sender, EventArgs e)
        {
            MemoryLogger Logger = new MemoryLogger();
            try
            {
                DateTime PeriodEnd = DateTime.Now;
                int DaysToSubtract = 24;//int.Parse(RBPeriods.SelectedValue);
                DateTime PeriodBegin = PeriodEnd.AddDays(-DaysToSubtract);

                List<ReportInfo> ReportInfos = new List<ReportInfo>();
                ReportInfos.Add(new ReportInfo(((LinkButton)sender).CommandArgument,
                    @"Статистика по подведомственным учреждениям. " + LblOrgName.Text,
                    PeriodBegin, PeriodEnd, GetOrzanizationUser().ToString()));

                string XSLTFilePath = "~\\Administration\\Reports\\Resources\\CreateExcel.xsl";
                XSLTFilePath = Server.MapPath(XSLTFilePath);

                string MailTemplatePath = "~\\Administration\\Reports\\Resources\\EmailTemplate_ReportViews.xml";
                MailTemplatePath = Server.MapPath(MailTemplatePath);

                var message = new EMailMessageViewReports("Отчеты из Подсистемы ФИС Результаты ЕГЭ", ReportInfos, MailTemplatePath, XSLTFilePath, Fbs.Core.DBSettings.ConnectionString, Logger, 600);

                Fbs.Utility.ResponseWriter.WriteStream("Report.xml", "text/xml", message.Attachments[0].ContentStream);
            }
            catch
            {
            }
        }

    }
}
