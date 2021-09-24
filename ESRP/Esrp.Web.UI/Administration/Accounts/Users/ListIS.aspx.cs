using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Esrp.Core.Systems;
using Esrp.Web.Administration.Organizations;
using Esrp.Web.Administration.SqlConstructor.UserAccounts;
using Esrp.Core.Loggers;

namespace Esrp.Web.Administration.Accounts.Users
{
    public partial class ListIS : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        	bool isAdmin = GeneralSystemManager.HasAccessToGroup(User.Identity.Name, EsrpManager.AdministratorGroupCode);
			if(!isAdmin)
			{
				if (!GeneralSystemManager.HasAccessToGroup(User.Identity.Name, EsrpManager.SupportGroupCode))
					Page.Response.Redirect("/default.aspx");
			}
        	dsAdministratorListCount.SelectParameters["isAdmin"].DefaultValue =
        		isAdmin ? "true" : "false";
			dsAdministratorList.SelectParameters["isAdmin"].DefaultValue =
				isAdmin ? "true" : "false";
            dgAdministratorList.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(dgAdministratorList_ItemDataBound);
        }

        void dgAdministratorList_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
             
            if(Convert.ToBoolean(ConfigurationManager.AppSettings["LogUserView"]) && e.Item.ItemType==System.Web.UI.WebControls.ListItemType.Item)
                AccountEventLogger.LogAccountViewEvent((e.Item.DataItem as DataRowView).Row["Login"].ToString());
        }
    }
}
