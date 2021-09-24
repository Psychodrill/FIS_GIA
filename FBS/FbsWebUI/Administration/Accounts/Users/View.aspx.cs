using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using Fbs.Core;
using Fbs.Web.Administration.Organizations;
using Region=Fbs.Web.Administration.Organizations.Region;

using Fbs.Core.Users;
using Fbs.Core.Organizations;

namespace Fbs.Web.Administration.Accounts.Users
{
    public partial class View : BasePage
    {
        public bool isUserAdminOrSupport()
        {
            Type accountType = Account.GetType(Account.ClientLogin);

            // Определить страницу перехода
            if (accountType == typeof(AdministratorAccount))
                return true;
            if (accountType == typeof(SupportAccount))
                return true;
            return false;
        }

        //UserAccount mCurrentUser;

        //public UserAccount CurrentUser
        //{
        //    get { return mCurrentUser; }
        //    set { mCurrentUser = value; }
        //}

        OrgUser  mCurrentUser;
        public OrgUser CurrentUser
        {
            get { return mCurrentUser; }
            set { mCurrentUser = value; }
        }

        public string Login
        {
            get
            {
                if (string.IsNullOrEmpty(Request.QueryString["login"]))
                    return String.Empty;

                return Request.QueryString["login"];
            }
        }

        public bool IsRegistrationDocumentExists
        {
            get { return CurrentUser.registrationDocument  != null; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
           // CurrentUser = UserAccount.GetUserAccount(Login);
            CurrentUser = OrgUserDataAccessor.Get(Login);

            if (CurrentUser == null)
                throw new NullReferenceException(String.Format("Пользователь \"{0}\" не найден",
                    Login));

            // Установлю заголовок страницы
            this.PageTitle = string.Format("Регистрационные данные “{0}”", CurrentUser.login);

            CompareAll_WithEtalonOrgInfo();

        }

//        private DataTable GetUsers(int OrgID)
//        {
//            DataTable table = null;
//            string connectionString = ConfigurationManager.ConnectionStrings["Fbs.Core.Properties.Settings.FbsConnectionString"].ToString();

//            using (SqlConnection connection = new SqlConnection(connectionString))
//            {
//                SqlCommand cmd = new SqlCommand(@"SELECT A.[Login], 
//                        A.LastName+' '+A.FirstName+' '+A.PatronymicName FIO, A.[Email], A.[Status]
//                        FROM dbo.Account A
//                        INNER JOIN dbo.Organization O ON A.OrganizationId=O.Id
//                        WHERE O.EtalonOrgID=@EtalonOrgID");
//                cmd.Parameters.Add(new SqlParameter("EtalonOrgID", SqlDbType.Int));
//                cmd.Parameters["EtalonOrgID"].Value = OrgID;
//                cmd.Connection = connection;
//                connection.Open();

//                using (SqlDataReader reader = cmd.ExecuteReader())
//                {
//                    table = MainFunctions.CreateTable_FromSQLDataReader(reader);
//                }
//            }
//            return table;
//        }


        public static string GetUserStatus(string sysStatus)
        {
            return UserAccountExtentions.GetNewStatusName(sysStatus);
        }

        private void CompareAll_WithEtalonOrgInfo()
        {
            int etalonOrgId = 0;
            if (CurrentUser.RequestedOrganization.OrganizationId !=null)
                etalonOrgId = (int)CurrentUser.RequestedOrganization.OrganizationId ;
           // Org etalonOrg = Org.Get(etalonOrgId);
            Organization Org = OrganizationDataAccessor.Get(etalonOrgId);
            
            //if (etalonOrg.OrgID>0)
            if (Org!=null )
            {
                CompareWithEtalonOrgInfo(CurrentUser.RequestedOrganization.FullName, Org.FullName, lblEtalonOrgName);
                CompareWithEtalonOrgInfo(CurrentUser.RequestedOrganization.LawAddress, Org.LawAddress, lblEtalonAddress);
                CompareWithEtalonOrgInfo(CurrentUser.RequestedOrganization.DirectorFullName, Org.DirectorFullName, lblEtalonChief);
                CompareWithEtalonOrgInfo(CurrentUser.RequestedOrganization.OwnerDepartment, Org.OwnerDepartment, lblEtalonFounder);
                CompareWithEtalonOrgInfo(CurrentUser.RequestedOrganization.Phone, Org.Phone, lblEtalonOrgPhone);
                CompareWithEtalonOrgInfo(CurrentUser.RequestedOrganization.OrgType.Name, Org.OrgType.Name, lblEtalonOrgType);
                CompareWithEtalonOrgInfo(CurrentUser.RequestedOrganization.Region.Name, Org.Region.Name, lblEtalonRegion);
            }
        }
        private void CompareWithEtalonOrgInfo(string text, string etalonText, System.Web.UI.WebControls.Label label)
        {
            label.Text = string.IsNullOrEmpty(etalonText) ? "[нет данных]" : etalonText;
            label.ToolTip = "Данные эталонной организации";
            if (text==etalonText)
            {
                label.ForeColor = Color.Gray;
            }
            else label.ForeColor = Color.Red;
        }
    }
}
