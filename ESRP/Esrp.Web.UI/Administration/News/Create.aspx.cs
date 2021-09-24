using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Esrp.Core;

namespace Esrp.Web.Administration.News
{
    public partial class Create : BasePage
    {
        private const string SuccessUri = "/Administration/News/List.aspx";

        private Esrp.Core.News mCurrentNews;
        private Esrp.Core.News CurrentNews
        {
            get
            {
                if (mCurrentNews == null)
                {
                    mCurrentNews = new Esrp.Core.News();
                    DataBindCurrenNews();
                }
                return mCurrentNews;
            }
        }

        private void DataBindCurrenNews()
        {
            CurrentNews.IsActive = Convert.ToBoolean(ddlIsActive.SelectedValue);
            CurrentNews.Date = txtDate.Value.Value;
            CurrentNews.Name = txtName.Text;
            CurrentNews.Description = txtDescription.Text;
            CurrentNews.Text = txtNews.Text;
        }

        private void ProcessSuccess()
        {
            Response.Redirect(SuccessUri, true);
        }


        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            // Создам новый документ
            CurrentNews.Update();

            // Выполню действия после успешного создания документа
            ProcessSuccess();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack) return;
            if (Request.UrlReferrer != null && Request.UrlReferrer.LocalPath!=null)
            {
                if (Request.UrlReferrer.LocalPath.Contains("List.aspx"))
                {
                    Session["BackLink.HRef"] = Request.UrlReferrer.ToString();
                }
                BackLink.HRef = (string)Session["BackLink.HRef"];
            }
        }

        protected void validateDate(object sender, ServerValidateEventArgs e)
        {
            e.IsValid = this.txtDate.Value.HasValue;
        }
    }
}
