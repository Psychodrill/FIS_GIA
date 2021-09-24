using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Esrp.Core;
using Esrp.Utility;

namespace Esrp.Web.Administration.Accounts.Users
{
    public partial class SendToRevision : System.Web.UI.Page
    {
		protected String GetUserKeyCode()
		{
			if (String.IsNullOrEmpty(Request["UserKey"]))
				return "";
			return Request["UserKey"];
		}

        private const string SuccessUri = "/Administration/Accounts/Users/SendToRevisionSuccess.aspx?login={0}&UserKey={1}";

        UserAccount mCurrentUser;

        public UserAccount CurrentUser
        {
            get
            {
                if (mCurrentUser == null)
                    mCurrentUser = UserAccount.GetUserAccount(Login);

                if (mCurrentUser == null)
                    throw new NullReferenceException(String.Format("Пользователь \"{0}\" не найден",
                        Login));

                return mCurrentUser;
            }
        }

        private string Login
        {
            get
            {
                if (string.IsNullOrEmpty(Request.QueryString["login"]))
                    return string.Empty;

                return Request.QueryString["login"];
            }
        }

        protected void btnSendToRevision_Click(object sender, EventArgs e)
        {
            // Проверю свободен ли емаил и валидность контролов страницы.
            if (!Page.IsValid)
                return;

            // Отправлю пользователя на доработку
            CurrentUser.SendToRevision(txtCause.Text.Trim());

            // Подготовлю email сообщение 
            EmailTemplate template = new EmailTemplate(EmailTemplateTypeEnum.SendToRevision);
            EmailMessage message = template.ToEmailMessage();
            message.To = CurrentUser.Email;
            message.Params = Utility.CollectEmailMetaVariables(CurrentUser);
            // Отправлю уведомление
            TaskManager.SendEmail(message);

            // Переход на страницу успеха
            Response.Redirect(String.Format(SuccessUri, Login, GetUserKeyCode()), true);
        }
    }
}
