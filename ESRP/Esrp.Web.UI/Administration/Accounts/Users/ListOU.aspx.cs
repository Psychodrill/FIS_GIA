using System;
using Esrp.Core.Systems;
using System.Configuration;
using Esrp.Core.Loggers;
using System.Data;

namespace Esrp.Web.Administration.Accounts.Users
{
    public partial class ListOU : System.Web.UI.Page
    {
		protected override void OnLoad(EventArgs e)
		{
			var orgID = "-1";
			orgID = GeneralSystemManager.GetUserOrganizationRequest(User.Identity.Name).ToString();
			dsAdministratorListCount.SelectParameters["orgID"].DefaultValue = orgID;
			dsAdministratorList.SelectParameters["orgID"].DefaultValue = orgID;
            dgAdministratorList.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(dgAdministratorList_ItemDataBound);
			base.OnLoad(e);
		}

        void dgAdministratorList_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["LogUserView"]) && e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item)
                AccountEventLogger.LogAccountViewEvent((e.Item.DataItem as DataRowView).Row["Login"].ToString());
        }
    }
}
