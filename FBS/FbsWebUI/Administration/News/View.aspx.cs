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
using Fbs.Core;

namespace Fbs.Web.Administration.News
{
    public partial class View : BasePage
    {
        private const string NewsQueryKey = "id";
        private const string ErrorNewsNotFound = "Новость не найдена";

        private long NewsId
        {
            get
            {
                if (string.IsNullOrEmpty(Request.QueryString[NewsQueryKey]))
                    throw new NullReferenceException(ErrorNewsNotFound);

                long result;
                if (!long.TryParse(Request.QueryString[NewsQueryKey], out result))
                    throw new NullReferenceException(ErrorNewsNotFound);

                return result;
            }
        }

        private Fbs.Core.News mCurrentNews;
        public Fbs.Core.News CurrentNews
        {
            get
            {
                if (mCurrentNews == null)
                {
                    if ((mCurrentNews = Fbs.Core.News.GetNews(NewsId)) == null)
                        throw new NullReferenceException(ErrorNewsNotFound);
                }
                return mCurrentNews;
            }
        }

        protected override void OnPreLoad(EventArgs e)
        {
            // Установлю заголовок страницы
            this.PageTitle = string.Format("Просмотр новости “{0}”", CurrentNews.Name);
        }
    }
}
