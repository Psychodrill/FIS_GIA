using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Fbs.Core;
using Fbs.Web.Administration.Organizations;

namespace Fbs.Web.Controls
{
    public partial class OperatorPanel : System.Web.UI.UserControl
    {
        
        private string GetConnString()
        {
            return ConfigurationManager.ConnectionStrings["Fbs.Core.Properties.Settings.FbsConnectionString"].ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                GetUserInfo(Request.QueryString["login"], Account.ClientLogin);
        }

        private void GetUserInfo(string userLogin, string operatorLogin)
        {

            SqlCommand cmd = new SqlCommand("[dbo].[Operator_GetUserInfo]");
                
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("OperatorLogin", SqlDbType.NVarChar, 255));
            cmd.Parameters["OperatorLogin"].Value = operatorLogin;
            cmd.Parameters.Add(new SqlParameter("UserLogin", SqlDbType.NVarChar, 255));
            cmd.Parameters["UserLogin"].Value = userLogin;
            cmd.Parameters.Add(new SqlParameter("IsMainOperator", SqlDbType.Bit));
            cmd.Parameters["IsMainOperator"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add(new SqlParameter("MainOperatorName", SqlDbType.VarChar, 255));
            cmd.Parameters["MainOperatorName"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add(new SqlParameter("Comments", SqlDbType.VarChar, 1024));
            cmd.Parameters["Comments"].Direction = ParameterDirection.Output;

            DataTable dt1=null, dt2=null;

            using (SqlConnection connection = new SqlConnection(GetConnString()))
            {
                cmd.Connection = connection;
                connection.Open();

                using(SqlDataReader reader=cmd.ExecuteReader())
                {
                    dt1 = MainFunctions.CreateTable_FromSQLDataReader(reader);
                    reader.NextResult();
                    dt2 = MainFunctions.CreateTable_FromSQLDataReader(reader);
                }

            }

            imgEditStatus.Visible=cmd.Parameters["IsMainOperator"].Value == DBNull.Value
                                     ? true
                                     : !((bool)cmd.Parameters["IsMainOperator"].Value);

            lblOperatorName.Text = cmd.Parameters["MainOperatorName"].Value == DBNull.Value
                                       ? ""
                                       : cmd.Parameters["MainOperatorName"].Value.ToString();


            txtComments.Text = cmd.Parameters["Comments"].Value == DBNull.Value
                                       ? ""
                                       : cmd.Parameters["Comments"].Value.ToString();

            rptMyUsers.DataSource = dt1;
            rptMyUsers.DataBind();
            rptMyUsersWithComment.DataSource = dt2;
            rptMyUsersWithComment.DataBind();
        }

        protected void btnEditComment_Click(object sender, EventArgs e)
        {
            SetCommentsUser(Request.QueryString["login"], txtComments.Text);
            Page.Response.Redirect(Page.Request.Url.ToString(), true);
        }

        private void SetCommentsUser(string userLogin, string comments)
        {
            SqlCommand cmd = new SqlCommand("dbo.Operator_AddUserComment");

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("UserLogin", SqlDbType.NVarChar, 255));
            cmd.Parameters["UserLogin"].Value = userLogin;
            cmd.Parameters.Add(new SqlParameter("Comment", SqlDbType.VarChar,1024));
            cmd.Parameters["Comment"].Value = comments;
            using (SqlConnection connection = new SqlConnection(GetConnString()))
            {
                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void GetNewUser(string operatorLogin)
        {
            SqlCommand cmd = new SqlCommand("[dbo].[Operator_GetNewUser]");

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("OperatorLogin", SqlDbType.NVarChar, 255));
            cmd.Parameters["OperatorLogin"].Value = operatorLogin;
            cmd.Parameters.Add(new SqlParameter("UserID", SqlDbType.Int));
            cmd.Parameters["UserID"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add(new SqlParameter("UserLogin", SqlDbType.VarChar, 255));
            cmd.Parameters["UserLogin"].Direction = ParameterDirection.Output;

            using (SqlConnection connection = new SqlConnection(GetConnString()))
            {
                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteNonQuery();
            }

            if (cmd.Parameters["UserID"].Value != DBNull.Value)
            {
                // переход на нового пользователя
                string userLogin = cmd.Parameters["UserLogin"].Value.ToString();
                Response.Redirect("./View.aspx?login=" + userLogin);
            }
            else
            {
                // пользователей нет - переход на список
                Response.Redirect("./List.aspx");
            }
        }

        protected void btnGetNewUser_Click(object sender, EventArgs e)
        {

            GetNewUser(Account.ClientLogin);
        }
    }
}