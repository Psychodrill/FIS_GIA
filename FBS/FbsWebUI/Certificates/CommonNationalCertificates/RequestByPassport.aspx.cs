namespace Fbs.Web.Certificates.CommonNationalCertificates
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web;

    using Fbs.Core.Organizations;
    using Fbs.Core.Shared;
    using Fbs.Core.UICheckLog;
    using Fbs.Utility;

    using WebControls;

    public partial class RequestByPassport : BasePage, IHistoryNavigator
    {
        private const string SearchUrl =
            "/Certificates/CommonNationalCertificates/RequestByPassportResultCommon.aspx?" + 
            "&surname={0}&name={1}&secondName={2}&documentSeries={3}&documentNumber={4}&Ev={5}";

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            // Выйду если постбэк или нет объекта в сесии
            if (Page.IsPostBack || Session[UINavigation.SessionId] == null)
            {
                return;
            }

            // Получу состояние контрола навигации
            var state = (UserNavigatorState)Session[UINavigation.SessionId];

            // Если происходит переход назад по экшену Back, то восстановлю сохраненные состояния 
            // контролов, иначе удалю сохраненные состояния 
            if (state.OkBack)
            {
                try
                {
                    StateManager.RestoreState(this.Page);
                }
                catch
                {
                    StateManager.ClearState();
                }
            }
            else StateManager.ClearState();
        }

        public override void Validate()
        {
            base.Validate();

            List<string> nErrors = DocumentCheck.DocNumberCheck(txtNumber.Text);
            if (nErrors.Count > 0)
            {
                vlEnchancedPassportNumber.IsValid = false;
                vlEnchancedPassportNumber.ErrorMessage = string.Join("<br />", nErrors.ToArray());
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            // Сохраню состояния контролов
            StateManager.AddEntry(txtLastName.ID, txtLastName.Text.FullTrim());
            StateManager.AddEntry(txtFirstName.ID, txtFirstName.Text.FullTrim());
            StateManager.AddEntry(txtPatronymicName.ID, txtPatronymicName.Text.FullTrim());
            StateManager.AddEntry(txtSeries.ID, txtSeries.Text.FullTrim());
            StateManager.AddEntry(txtNumber.ID, txtNumber.Text.FullTrim());
            StateManager.SaveState();

            // Зарегистритую событие
            var login = HttpContext.Current.User.Identity.Name;
            var org = OrganizationDataAccessor.GetByLogin(login);
            if ((org != null && org.DisableLog == false) || org == null)
            {
                var eventId = CheckLogDataAccessor.AddPassportCheckEvent(
                    User.Identity.Name,
                    txtLastName.Text.FullTrim(),
                    txtFirstName.Text.FullTrim(),
                    txtPatronymicName.Text.FullTrim(),
                    txtSeries.Text.FullTrim(),
                    txtNumber.Text.FullTrim());

                // Перейду на страницу результатов (поиска).
                Response.Redirect(
                    string.Format(
                        SearchUrl,
                        HttpUtility.UrlEncode(txtLastName.Text.FullTrim()),
                        HttpUtility.UrlEncode(txtFirstName.Text.FullTrim()),
                        HttpUtility.UrlEncode(txtPatronymicName.Text.FullTrim()),
                        HttpUtility.UrlEncode(txtSeries.Text.FullTrim()),
                        HttpUtility.UrlEncode(txtNumber.Text.FullTrim()),
                        HttpUtility.UrlEncode(eventId.ToString(CultureInfo.InvariantCulture))));
            }
            else
            {
                // Перейду на страницу результатов (поиска).
                Response.Redirect(
                    string.Format(
                        SearchUrl,
                        HttpUtility.UrlEncode(txtLastName.Text.FullTrim()),
                        HttpUtility.UrlEncode(txtFirstName.Text.FullTrim()),
                        HttpUtility.UrlEncode(txtPatronymicName.Text.FullTrim()),
                        HttpUtility.UrlEncode(txtSeries.Text.FullTrim()),
                        HttpUtility.UrlEncode(txtNumber.Text.FullTrim()),
                        string.Empty));
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.RawUrl);
        }

        public string GetPageName()
        {
            return "RequestByPassportHistoryResultCommon.aspx";
        }
    }
}
