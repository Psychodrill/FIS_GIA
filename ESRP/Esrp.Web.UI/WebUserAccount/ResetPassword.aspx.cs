using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Text;
using System.Configuration;
using Esrp.Core;
using Esrp.Web.Extentions;
using System.Web.Security;
using Esrp.Utility;

namespace Esrp.Web.WebUserAccount
{
    public partial class ResetPassword : System.Web.UI.Page
    {
        private RequestInfo _info;
        protected override void OnInit(EventArgs e)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.SignOut();
                this.Response.Cookies.Remove(FormsAuthentication.FormsCookieName);
                this.Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
                HttpCookie cookie = this.Request.Cookies[FormsAuthentication.FormsCookieName];
                if (cookie != null)
                {
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(cookie);
                }
                this.Session.Abandon();
                this.Response.Redirect(this.Request.RawUrl, true);
            }
            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            this.validPwdValidator.ServerValidate += new ServerValidateEventHandler(validPwdValidator_ServerValidate);
            
            try
            {
                RSACryptoServiceProvider Rsa = Helper.GetCurrentRsaProvider(false);
                string _accountToChange = Encoding.Default.GetString(Rsa.Decrypt(Convert.FromBase64String(this.Request.QueryString["userId"]), false));
                this._info = new RequestInfo(_accountToChange);
                if (!_info.IsValid)
                    throw new HttpException(403, "Срок действия ссылки истек либо пароль был уже изменен");
                this.userName.Text = this._info.Login;
                this.oldPwdRow.Visible = this._info.UseConfirmation;

            }
            catch (HttpException ex)
            {
                throw ex;
            }
            catch (Exception other)
            {
                throw new HttpException(404, "Нет такой страницы",other);
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            if (this.IsValid)
            {
                Account.ChangePassword(this._info.Login, this.newPassword.Text);
                if (!String.IsNullOrEmpty(this.Request.QueryString["isNew"]) && this.Request.QueryString["isNew"] == "1")
                {
                    var user = UserAccount.GetUserAccount(this._info.Login);
                    var template = new EmailTemplate(EmailTemplateTypeEnum.Registration);
                    EmailMessage message = template.ToEmailMessage();
                    message.To = user.Email;
                    message.Params = Utility.CollectEmailMetaVariables(
                            user, user.PasswordHash, Utility.GetSeverPath(this.Request));
                    TaskManager.SendEmail(message);
                    //this.registrationSuccessPanel.Visible = true;
                    this.Response.Redirect("~/Profile/RegistrationSuccess.aspx", true);
                }
                else
                    this.resetSuccessPanel.Visible = true;
                this.resetPasswordPanel.Visible = false;
            }

        }
        void validPwdValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = this._info.UseConfirmation || (this._info.UseConfirmation && Account.Verify(this._info.Login, this.password.Text) == Account.VerifyStateEnum.Valid);
        }
    }

    public class RequestInfo
    {
        public string Login
        {
            get;
            set;
        }

        public DateTime DateRequested
        {
            get;
            set;
        }

        public string OldPasswordHash
        {
            get;
            set;
        }

        public bool UseConfirmation
        {
            get;
            set;
        }

        public RequestInfo(string csvInfo)
        {
            string[] splitted = csvInfo.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            this.Login = splitted[0];
            this.DateRequested = DateTime.Parse(splitted[1]);
            this.OldPasswordHash = splitted[2];
            this.UseConfirmation = splitted.Length > 3;
        }

        public bool IsValid {

            get
            {
                if ((DateTime.Now - this.DateRequested).Hours > 24)
                    return false;
                if(UserAccount.GetUserAccount(this.Login).PasswordHash!=this.OldPasswordHash)
                    return false;
                return true;
            }
        }
    }
}