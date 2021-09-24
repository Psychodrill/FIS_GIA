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

namespace Fbs.Web
{
    public partial class News : BasePage
    {
        private const string NewsQueryKey = "id";
        private const string ErrorDocumentNotFound = "Новость не найден";
        private const string SuccessUri = "/Administration/News/List.aspx";
        private Fbs.Core.News mCurrentNews;

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

        public Fbs.Core.News CurrentNews
        {
            get
            {
                if (mCurrentNews == null)
                {
                    if ((mCurrentNews = Fbs.Core.News.GetNews(NewsId)) == null || !mCurrentNews.IsActive)
                        throw new NullReferenceException(ErrorDocumentNotFound);
                }
                return mCurrentNews;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Установлю заголовок страницы
            this.PageTitle = string.Format("{0} {1}", CurrentNews.Date.ToShortDateString(), 
                CurrentNews.Name);
        }
    }
}
