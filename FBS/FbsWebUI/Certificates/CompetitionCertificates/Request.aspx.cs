using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Fbs.Utility;
using WebControls;

namespace Fbs.Web.Certificates.CompetitionCertificates
{
    public partial class Request : System.Web.UI.Page
    {
        private const string SearchUrl =
           "/Certificates/CompetitionCertificates/RequestResult.aspx?" +
           "CompetitionType={0}&LastName={1}&FirstName={2}&PatronymicName={3}&Region={4}";

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

        protected void btnCheck_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            // Сохраню состояния контролов.
            StateManager.AddEntry(ddlCompetitionType.ID, ddlCompetitionType.SelectedValue);
            StateManager.AddEntry(txtLastName.ID, txtLastName.Text);
            StateManager.AddEntry(txtFirstName.ID, txtFirstName.Text);
            StateManager.AddEntry(txtPatronymicName.ID, txtPatronymicName.Text);
            StateManager.SaveState();

            // Перейду на страницу результатов (поиска).
            Response.Redirect(String.Format(SearchUrl,
                ddlCompetitionType.SelectedValue,
                HttpUtility.UrlEncode(txtLastName.Text.Trim()),
                HttpUtility.UrlEncode(txtFirstName.Text.Trim()),
                HttpUtility.UrlEncode(txtPatronymicName.Text.Trim()),
                ddlRegion.SelectedValue));
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.RawUrl);
        }

    }
}
