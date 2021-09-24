using System;
using Esrp.Core;
using Esrp.Core.Users;

namespace Esrp.Web.Controls
{
	/// <summary>
	/// Контрол для отображение и редактирования комментариев операторами системы.
	/// Для инициализации контрола необходимо установить идентификатор заявки OrganizationRequestID на OnInit.
	/// </summary>
	public partial class OperatorPanelForRequest : System.Web.UI.UserControl
	{
		public string UserKey { get; set; }

		public int OrganizationRequestID { get; set; }

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			LoadAndBindData();
		}

		private void LoadAndBindData()
		{
			if (!Page.IsPostBack)
			{
				OperatorComment operatorComment = OrgRequestManager.GetComment(OrganizationRequestID);
				if (operatorComment != null)
				{
					txtComments.Text = operatorComment.Comment;
				}
			}
		}

		protected void btnEditComment_Click(object sender, EventArgs e)
		{
			OrgRequestManager.SaveComment(OrganizationRequestID, Account.ClientLogin, txtComments.Text);			
			Page.Response.Redirect(Page.Request.Url.ToString(), true);
		}
	}
}