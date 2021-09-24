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

namespace Fbs.Web.Certificates.Entrants
{
    public partial class Check : System.Web.UI.Page
    {
        private const string SearchUrl ="/Certificates/Entrants/CheckResult.aspx?Number={0}";

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            // Выйду если постбэк или нет объекта в сессии
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

            // Сохранить состояния контролов.
            StateManager.AddEntry(txtNumber.ID, txtNumber.Text);
            StateManager.SaveState();

            Response.Redirect(String.Format(SearchUrl, txtNumber.Text.Trim()), true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.RawUrl);
        }
    }
}
