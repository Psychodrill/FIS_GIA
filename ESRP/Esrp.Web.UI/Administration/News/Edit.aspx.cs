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
    public partial class Edit : BasePage
    {
        private const string NewsQueryKey = "id";
        private const string ErrorDocumentNotFound = "Новость не найдена";
        private const string SuccessUri = "/Administration/News/List.aspx";
        private Esrp.Core.News mCurrentNews;

        private long NewsId
        {
            get
            {
                long result;
                if (!long.TryParse(Request.QueryString[NewsQueryKey], out result))
                    throw new NullReferenceException(ErrorDocumentNotFound);
                return result;
            }
        }

        public Esrp.Core.News CurrentNews
        {
            get
            {
                if (mCurrentNews == null)
                {
                    if ((mCurrentNews = Esrp.Core.News.GetNews(NewsId)) == null)
                        throw new NullReferenceException(ErrorDocumentNotFound);
                    if (Page.IsPostBack)
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

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            // Обновлю новость
            CurrentNews.Update();

            // Выполню действия после успешного редактирование новости
            ProcessSuccess();
        }


        private void ProcessSuccess()
        {
            Response.Redirect(SuccessUri, true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack) return;
            txtDate.Value = CurrentNews.Date;
            txtName.Text = CurrentNews.Name;
            txtDescription.Text = CurrentNews.Description;
            txtNews.Text = CurrentNews.Text;
            ddlIsActive.SelectedValue = CurrentNews.IsActive.ToString();

            // Установлю заголовок страницы
            this.PageTitle = string.Format("Редактирование новости “{0}”", CurrentNews.Name);
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
