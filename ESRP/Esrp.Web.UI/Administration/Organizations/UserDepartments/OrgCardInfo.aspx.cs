namespace Esrp.Web.Administration.Organizations.UserDepartments
{
    using System;
    using System.Configuration;

    using Esrp.Core.Organizations;

    /// <summary>
    /// The org card info.
    /// </summary>
    public partial class OrgCardInfo : BasePage
    {
        #region Methods

        /// <summary>
        /// инициализировать состояние контролов
        /// </summary>
        /// <param name="e">
        /// у
        /// </param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if ((!string.IsNullOrEmpty(this.Request["IsNew"])) && (this.Request["IsNew"].ToLower() == "true"))
            {
                this.OrganizationView.Message = "Новая организация успешно создана!";
            }

            var orgId = this.GetParamInt("OrgId");
            if (orgId == 0)
            {
                throw new ArgumentException("OrgId cannot be empty");
            }

            this.OrganizationView.OrganizationId = orgId;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (Request.UrlReferrer != null && Request.UrlReferrer.LocalPath != null)
            {
                if (Request.UrlReferrer.LocalPath.Contains("OrgList.aspx"))
                {
                    Session["BackLink.HRef"] = Request.UrlReferrer.ToString();
                }
                BackLink.HRef = (string)Session["BackLink.HRef"];
            }

            this.ForConfirmation.Text = ConfigurationManager.AppSettings["OrgCardInstructionForConfirmation"];
        }

        /// <summary>
        /// Обработчик нажатия кнопки сохранить
        /// </summary>
        /// <param name="sender">
        /// Источник события
        /// </param>
        /// <param name="e">
        /// Параметры события
        /// </param>
        protected void BtnUpdateClick(object sender, EventArgs e)
        {
            this.CBXConfirm.Checked = false;

            if (this.Page.IsValid)
            {
                var orgID = this.GetParamInt("OrgID");

                try
                {
                    OrganizationView.SaveRCModel(orgID);

                    this.lblError.Text = string.Empty;
                    this.lblError.Visible = false;
                }
                catch (Exception exp)
                {
                    if (orgID > 0)
                    {
                        this.lblError.Text = string.Format("Во время обновления организации произошла ошибка: {0}", exp.Message);
                    }
                    else
                    {
                        this.lblError.Text = string.Format("Во время создания организации произошла ошибка: {0}", exp.Message);
                    }

                    this.lblError.Visible = true;
                }
            }
        }

        #endregion
    }
}