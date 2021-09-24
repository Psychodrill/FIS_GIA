using System;
using System.Web;
using System.Web.UI.WebControls;
using Fbs.Core.Shared;
using Fbs.Utility;
using WebControls;

namespace Fbs.Web.Certificates.CommonNationalCertificates
{
    public partial class WildcardRequest : BasePage
    {
        private const string SearchUrl =
            "/Certificates/CommonNationalCertificates/WildcardRequestResultCommon.aspx?" +
            "LastName={0}&FirstName={1}&PatronymicName={2}&DocSeries={3}&DocNumber={4}&TypographicNumber={5}&Number={6}";

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
                StateManager.RestoreState(this.Page);
            else
                StateManager.ClearState();
            
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            // Сохраню состояния контролов
            StateManager.AddEntry(txtLastName.ID, txtLastName.Text.FullTrim());
            StateManager.AddEntry(txtFirstName.ID, txtFirstName.Text.FullTrim());
            StateManager.AddEntry(txtPatronymicName.ID, txtPatronymicName.Text.FullTrim());
            StateManager.AddEntry(txtDocSeries.ID, txtDocSeries.Text.FullTrim());
            StateManager.AddEntry(txtDocNumber.ID, txtDocNumber.Text.FullTrim());
            StateManager.AddEntry(txtTypographicNumber.ID, txtTypographicNumber.Text.FullTrim());
            StateManager.AddEntry(txtNumber.ID, txtNumber.Text.FullTrim());
            StateManager.SaveState();

            // Перейду на страницу результатов (поиска).
            Response.Redirect(String.Format(SearchUrl,
                HttpUtility.UrlEncode(txtLastName.Text.FullTrim()),
                HttpUtility.UrlEncode(txtFirstName.Text.FullTrim()),
                HttpUtility.UrlEncode(txtPatronymicName.Text.FullTrim()),
                HttpUtility.UrlEncode(txtDocSeries.Text.FullTrim()),
                HttpUtility.UrlEncode(txtDocNumber.Text.FullTrim()),
                HttpUtility.UrlEncode(txtTypographicNumber.Text.FullTrim()),
                HttpUtility.UrlEncode(txtNumber.Text.FullTrim())
            ));
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.RawUrl);
        }

    }
}
