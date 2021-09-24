// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Activate.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The activate.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Esrp.Web.Administration.Accounts.Users
{
    using System;
    using System.Web.UI;

    using Esrp.Core;
    using Esrp.Core.Users;
    using Esrp.Web.Administration.SqlConstructor.Organizations;
    using Esrp.Utility;

    /// <summary>
    /// The activate.
    /// </summary>
    public partial class Activate : Page
    {
        #region Constants and Fields

        /// <summary>
        /// The error message.
        /// </summary>
        protected string ErrorMessage;
        SqlConstructor_GetRequests m_SqlConstructorGetRequests;
        /// <summary>
        /// The success uri.
        /// </summary>
        private const string SuccessUri = "/Administration/Accounts/Users/ActivateSuccess.aspx?Login={0}&UserKey={1}";

        /// <summary>
        /// The m current user.
        /// </summary>
        private OrgUser mCurrentUser;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets CurrentUser.
        /// </summary>
        /// <exception cref="NullReferenceException">
        /// </exception>
        public OrgUser CurrentUser
        {
            get
            {
                if (this.mCurrentUser == null)
                {
                    this.mCurrentUser = OrgUserDataAccessor.Get(this.Login);
                }

                if (this.mCurrentUser == null)
                {
                    throw new NullReferenceException(string.Format("Пользователь \"{0}\" не найден", this.Login));
                }

                return this.mCurrentUser;
            }
        }

        /// <summary>
        /// Gets Login.
        /// </summary>
        public string Login
        {
            get
            {
                if (string.IsNullOrEmpty(this.Request.QueryString["login"]))
                {
                    return string.Empty;
                }

                return this.Request.QueryString["login"];
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The check new user account email.
        /// </summary>
        /// <param name="eMail">
        /// The e mail.
        /// </param>
        /// <returns>
        /// The check new user account email.
        /// </returns>
        public bool CheckNewUserAccountEmail(string eMail)
        {
            return Account.CheckNewUserAccountEmail(eMail);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The get user key code.
        /// </summary>
        /// <returns>
        /// The get user key code.
        /// </returns>
        protected string GetUserKeyCode()
        {
            if (string.IsNullOrEmpty(this.Request["UserKey"]))
            {
                return string.Empty;
            }

            return this.Request["UserKey"];
        }

        /// <summary>
        /// The on pre render.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            ValidateOrganizationRequest();
            this.ManageUI();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.m_SqlConstructorGetRequests=new SqlConstructor_GetRequests(this.Request.QueryString);
            if (!Page.IsPostBack)
            {
                if (Request.UrlReferrer != null && Request.UrlReferrer.LocalPath!=null)
                {
                    if (Request.UrlReferrer.LocalPath.Contains("List.aspx"))
                    {
                        Session["BackLink.HRef"] = Request.UrlReferrer.ToString();
                    }
                    BackLink.HRef = (string)Session["BackLink.HRef"];
                }
            }
            
        }

        /// <summary>
        /// The btn activate_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void btnActivate_Click(object sender, EventArgs e)
        {
            // Проверю свободен ли емаил и валидность контролов страницы.
            if (!this.Page.IsValid)
            {
                return;
            }

            this.ErrorMessage = UserAccountExtentions.ActivateUser(this.CurrentUser);
            if (string.IsNullOrEmpty(this.ErrorMessage))
            {
                // Переход на страницу успеха
                this.Response.Redirect(string.Format(SuccessUri, this.Login, this.GetUserKeyCode()), true);
            }
        }

        /// <summary>
        /// The manage ui.
        /// </summary>
        private void ManageUI()
        {
            this.phActivate.Visible = true;
            this.pErrorMessage.Visible = false;

            if (!string.IsNullOrEmpty(this.ErrorMessage))
            {
                this.ShowErrorMessage(this.ErrorMessage);
            }
        }

        /// <summary>
        /// The show error message.
        /// </summary>
        /// <param name="errorMessage">
        /// The error message.
        /// </param>
        private void ShowErrorMessage(string errorMessage)
        {
            this.phActivate.Visible = false;
            this.pErrorMessage.Visible = true;
            this.pErrorMessage.InnerText = errorMessage;
        }

        /// <summary>
        /// The validate organization request.
        /// </summary>
        private void ValidateOrganizationRequest()
        {
            if (this.CurrentUser.RequestedOrganization.OrgType.Id == null)
            {
                this.ErrorMessage = "Активация невозможна, так как для данного пользователя не указан тип ОУ.";
            }

            // GVUZ-595. Исключить подтверждение заявки без приложенных документов.
            if (this.CurrentUser.registrationDocument == null || this.CurrentUser.registrationDocument.Length == 0)
            {
                this.ErrorMessage = "Активация невозможна. Не приложен скан заявки.";
            }
            
        }

        #endregion

        // UserAccount mCurrentUser;

        // public UserAccount CurrentUser
        // {
        // get
        // {
        // if (mCurrentUser == null)
        // mCurrentUser = UserAccount.GetUserAccount(Login);

        // if (mCurrentUser == null)
        // throw new NullReferenceException(String.Format("Пользователь \"{0}\" не найден",
        // Login));

        // return mCurrentUser;
        // }
        // }
    }
}