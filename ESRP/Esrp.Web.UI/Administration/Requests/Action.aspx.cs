// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Action.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The action.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Esrp.Web.Administration.Requests
{
    using System;
    using System.Collections.Generic;
    using System.Web.UI;

    using Esrp.Core;
    using Esrp.Core.Users;
    using Esrp.Utility;

    using FogSoft.Helpers;

    /// <summary>
    /// The action.
    /// </summary>
    public partial class Action : Page
    {
        #region Constants and Fields

        /// <summary>
        /// The error message.
        /// </summary>
        protected string ErrorMessage;

        /// <summary>
        /// The _send mail user list.
        /// </summary>
        private readonly List<string> _sendMailUserList = new List<string>();

        #endregion

        #region Properties

        /// <summary>
        /// Gets ActionType.
        /// </summary>
        protected UserAccount.UserAccountStatusEnum ActionType
        {
            get
            {
                if (string.IsNullOrEmpty(this.Request.QueryString["action"]))
                {
                    return UserAccount.UserAccountStatusEnum.None;
                }

                return (UserAccount.UserAccountStatusEnum)this.Request.QueryString["action"].To(0);
            }
        }

        /// <summary>
        /// Gets RequestID.
        /// </summary>
        protected int RequestID
        {
            get
            {
                if (string.IsNullOrEmpty(this.Request.QueryString["requestID"]))
                {
                    return 0;
                }

                return this.Request.QueryString["requestID"].To(0);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The on pre render.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            this.ManageUI();
        }

        /// <summary>
        /// The btn action_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void btnAction_Click(object sender, EventArgs e)
        {
            this.ErrorMessage = OrgRequestManager.UpdateOrganizationRequestStatus(this.RequestID, this.ActionType);
            OrgRequest orgRequest = OrgRequestManager.GetRequest(this.RequestID);
            if (string.IsNullOrEmpty(this.ErrorMessage))
            {
                if (this.ActionType == UserAccount.UserAccountStatusEnum.Activated)
                {
                    // отправить сообщение пользователям указанным в заявке.
                    
                    foreach (OrgUserBrief user in orgRequest.Organization.ActivatedUsers)
                    {
                        if (this._sendMailUserList.Contains(user.Login))
                        {
                            continue;
                        }

                        UserAccountExtentions.SendMail(user, EmailTemplateTypeEnum.Activation);
                        this._sendMailUserList.Add(user.Login);
                    }
                }

                this.phSuccess.Visible = true;
            }
            else if (this.ErrorMessage.Contains("нет активированных пользователей"))
            {
                foreach (OrgUserBrief user in orgRequest.LinkedUsers)
                {
                    if (this._sendMailUserList.Contains(user.Login))
                    {
                        continue;
                    }
                    var template = new EmailTemplate(EmailTemplateTypeEnum.FilialActivationFailed);
                    EmailMessage message = template.ToEmailMessage();
                    message.To =user.Email;
                    message.Params = Utility.CollectEmailMetaVariables(user);
                    TaskManager.SendEmail(message);
                    this._sendMailUserList.Add(user.Login);

                }
            }
        }

        /// <summary>
        /// The btn send to revision_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void btnSendToRevision_Click(object sender, EventArgs e)
        {
            // Проверю свободен ли емаил и валидность контролов страницы.
            if (!this.Page.IsValid)
            {
                return;
            }

            // заявка отправляется на доработку.
            this.ErrorMessage = OrgRequestManager.UpdateOrganizationRequestStatus(
                this.RequestID, this.ActionType, this.txtCause.Text.Trim());
            if (!string.IsNullOrEmpty(this.ErrorMessage))
            {
                return;
            }

            OrgRequest orgRequest = OrgRequestManager.GetRequest(this.RequestID);
            foreach (OrgUserBrief user in orgRequest.LinkedUsers)
            {
                if (this._sendMailUserList.Contains(user.Login))
                {
                    continue;
                }

                // Подготовлю email сообщение 
                var template = new EmailTemplate(EmailTemplateTypeEnum.SendToRevision);
                EmailMessage message = template.ToEmailMessage();
                message.To = user.Email;
                message.Params = Utility.CollectEmailMetaVariables(user);

                // Отправлю уведомление
                TaskManager.SendEmail(message);
                this._sendMailUserList.Add(user.Login);
            }

            this.phSuccess.Visible = true;
        }

        /// <summary>
        /// The manage ui.
        /// </summary>
        private void ManageUI()
        {
            this.phActivate.Visible = true;
            this.pErrorMessage.Visible = false;
            this.phRevision.Visible = false;

            switch (this.ActionType)
            {
                case UserAccount.UserAccountStatusEnum.Activated:
                    this.spanConfirmMessage.InnerText =
                        string.Format("Пожалуйста, подтвердите активацию заявки под номером \"{0}\"", this.RequestID);
                    this.btnAction.Text = "Активировать";
                    break;
                case UserAccount.UserAccountStatusEnum.Deactivated:
                    this.spanConfirmMessage.InnerText =
                        string.Format("Пожалуйста, подтвердите деактивацию заявки под номером \"{0}\"", this.RequestID);
                    this.btnAction.Text = "Деактивировать";
                    break;
                case UserAccount.UserAccountStatusEnum.Revision:
                    this.spanConfirmMessage.InnerText =
                        string.Format(
                            "Пожалуйста, подтвердите отправку на доработку заявки под номером \"{0}\"", this.RequestID);
                    this.btnAction.Text = "Отправить на доработку";
                    this.phRevision.Visible = true;
                    this.phActivate.Visible = false;
                    break;
                default:
                    this.spanConfirmMessage.InnerText = "Указано некорректное действие.";
                    this.btnAction.Visible = false;
                    break;
            }

            if (this.phSuccess.Visible)
            {
                this.phActivate.Visible = false;
                this.phRevision.Visible = false;
            }

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

        #endregion
    }
}