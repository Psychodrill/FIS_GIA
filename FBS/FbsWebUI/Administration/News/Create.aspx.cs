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
using Fbs.Core;

namespace Fbs.Web.Administration.News
{
    public partial class Create : BasePage
    {
        private const string SuccessUri = "/Administration/News/List.aspx";

        private Fbs.Core.News mCurrentNews;
        private Fbs.Core.News CurrentNews
        {
            get
            {
                if (mCurrentNews == null)
                {
                    mCurrentNews = new Fbs.Core.News();
                    DataBindCurrenNews();
                }
                return mCurrentNews;
            }
        }

        private void DataBindCurrenNews()
        {
            CurrentNews.IsActive = Convert.ToBoolean(ddlIsActive.SelectedValue);
            CurrentNews.Date = Convert.ToDateTime(txtDate.Text);
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
    }
}
