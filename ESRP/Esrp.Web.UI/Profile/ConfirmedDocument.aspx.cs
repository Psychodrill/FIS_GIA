using System;
using System.Collections;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Drawing;
using System.Drawing.Imaging;
using Esrp.Core;

namespace Esrp.Web.Personal.Profile
{
    public partial class ConfirmedDocument : BasePage
    {
        private string Login
        {
            get
            {
                if (string.IsNullOrEmpty(Request.QueryString["login"]))
                    return string.Empty;

                return Request.QueryString["login"];
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // получу текущего пользователя
            UserAccount user;
            if (Login == string.Empty)
                user = UserAccount.GetUserAccount(CurrentUserName);
            else
                user = UserAccount.GetUserAccount(Login);

            // проверю, существует ли пользователь
            if (user == null)
                throw new NullReferenceException(String.Format("Пользователь \"{0}\" не найден.", 
                    (Login == string.Empty ? CurrentUserName : Login)));

            // проверю наличие файла
            if (user.RegistrationDocument == null)
                throw new NullReferenceException("Файл не загружен");
            
            // Отдам файл в response
            user.RegistrationDocument.WriteToResponse(Response);
        }
    }
}
