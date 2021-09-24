using System;
using System.Web;
using Fbs.Core.Organizations;
using Fbs.Core.Shared;
using Fbs.Core.UICheckLog;
using Fbs.Utility;
using WebControls;

namespace Fbs.Web.Certificates.CommonNationalCertificates
{
    public partial class RequestByTypographicNumber : BasePage, IHistoryNavigator
    {
        private const string SearchUrl =
            "/Certificates/CommonNationalCertificates/RequestByTypographicNumberResultCommon.aspx?" +
            "LastName={0}&FirstName={1}&PatronymicName={2}&TypographicNumber={3}&Ev={4}";

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            // Выйду если постбэк или нет объекта в сесии
            if (Page.IsPostBack || Session[UINavigation.SessionId] == null)
                return;

            // Получу состояние контрола навигации
            UserNavigatorState state = (UserNavigatorState)Session[UINavigation.SessionId];

            // Если происходит переход назад по экшену Back, то восстановлю сохраненные состояния 
            // контролов, иначе удалю сохраненные состояния 
            if (state.OkBack)
            {
                try
                {
                    StateManager.RestoreState(this.Page);
                }
                catch { StateManager.ClearState(); }
            }
            else
                StateManager.ClearState();
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
            StateManager.AddEntry(txtTypographicNumber.ID, txtTypographicNumber.Text.FullTrim());
            StateManager.SaveState();

            // Зарегистритую событие
            var login = HttpContext.Current.User.Identity.Name;
            var org = OrganizationDataAccessor.GetByLogin(login);
            if ((org != null && org.DisableLog == false) || org == null)
            {
                var eventId = CheckLogDataAccessor.AddTNCheckEvent(
                    User.Identity.Name,
                    txtLastName.Text.FullTrim(),
                    txtFirstName.Text.FullTrim(),
                    txtPatronymicName.Text.FullTrim(),
                    txtTypographicNumber.Text.FullTrim());

                // Перейду на страницу результатов (поиска).
                Response.Redirect(
                    string.Format(
                        SearchUrl,
                        HttpUtility.UrlEncode(txtLastName.Text.FullTrim()),
                        HttpUtility.UrlEncode(txtFirstName.Text.FullTrim()),
                        HttpUtility.UrlEncode(txtPatronymicName.Text.FullTrim()),
                        HttpUtility.UrlEncode(txtTypographicNumber.Text.FullTrim()),
                        HttpUtility.UrlEncode(eventId.ToString())));
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
                        HttpUtility.UrlEncode(txtTypographicNumber.Text.FullTrim()),
                        string.Empty));
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.RawUrl);
        }


        public string GetPageName()
        {
            return "RequestByTypographicNumberHistoryResultCommon.aspx";
        }
    }
}
