using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Esrp.Web.Profile
{
    public partial class ConfirmedDocumentView : System.Web.UI.Page
    {
        public string GetLogin()
        {
            string login = Request.QueryString["login"];
            if (string.IsNullOrEmpty(login))
                login = User.Identity.Name;
            return login;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string login = GetLogin();
            string fileType;
            int dataLen;
            
            if (GetFileInfo(login, out dataLen, out fileType))
            {
                if ((dataLen > 0) && fileType.ToLower().Contains("image") && !fileType.ToLower().Contains("tiff"))
                {
                    ShowImage(dataLen);
                }
                else
                {
                    if (dataLen > 0)
                        ShowNotImage(dataLen,fileType);
                    else
                        ShowError(string.Format("Регистрационный документ для логина '{0}' не загружен.", login));
                }

            }
            else
                ShowError("Не верный запрос");
        }

        public void ddlChangeSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            img.Width = new Unit(ddlChangeSize.SelectedItem.Text);
        }

        private void ShowError(string error)
        {
            pnlImage.Visible = false;
            pnlNotImage.Visible = false;
            pnlError.Visible = true;
            lblErrorMsg.Text = error;
        }

        private void ShowImage(int dataLen)
        {
            pnlError.Visible = false;
            pnlNotImage.Visible = false;
            pnlImage.Visible = true;

            lblDocSize_img.Text = string.Format("{0:0.0}", dataLen / 1024.0);

            if (!Page.IsPostBack)
                ddlChangeSize.SelectedValue = "100%";
            img.Width = new Unit(ddlChangeSize.SelectedItem.Text);
            img.ImageUrl = "ConfirmedDocument.aspx?login=" + GetLogin();
        }

        private void ShowNotImage(int dataLen, string doctype)
        {
            pnlError.Visible = false;
            pnlImage.Visible = false;
            pnlNotImage.Visible = true;

            lblDocType.Text = doctype;
            lblDocSize_notImg.Text = string.Format("{0:0.0}", dataLen/ 1024.0);
        }

        private bool GetFileInfo(string login, out int dataLen, out string  fileType)
        {
            dataLen = 0;
            fileType = "";
            bool hasLogin = false;
            //Создаем подключение к БД и получаем картинку по login-у пользователя
            string connectionString = ConfigurationManager.ConnectionStrings["Esrp.Core.Properties.Settings.EsrpConnectionString"].ToString();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("select RegistrationDocumentContentType ContentType, datalength(RegistrationDocument) as DLen from dbo.Account WHERE [Login]=@Login", connection);
                cmd.Parameters.Add(new SqlParameter("Login", SqlDbType.VarChar, 255));
                cmd.Parameters["Login"].Value = login;
                connection.Open();

                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SingleRow))
                {
                    if (reader.Read())
                    {
                        hasLogin = true;
                        if (reader["ContentType"] != DBNull.Value)
                            fileType = reader["ContentType"].ToString();
                        if (reader["DLen"] != DBNull.Value)
                            dataLen = (int)reader["DLen"];
                    }
                }
            }
            return hasLogin;
        }

    }
}
