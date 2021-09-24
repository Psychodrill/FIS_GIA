using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Esrp.Services;

namespace Esrp.Web.Controls
{
    public partial class RssNewsList : System.Web.UI.UserControl
    {
        public readonly int MAX_NEWS_TITLE_LENGTH = 200;
        public readonly int NEWS_COUNT = 3;
        private readonly NewsService newsService = new NewsService();


        protected void Page_Load(object sender, EventArgs e)
        {
            var news = this.newsService.SelectNews().OrderByDescending(p=>p.CreateDate).ToList();
            if (news.Count > NEWS_COUNT)
            {
                // удалим лишние 
                // todo проверить что нужно показывать первые а не последние 3
                news.RemoveRange(NEWS_COUNT, news.Count - NEWS_COUNT);
            }
            foreach(var item in news)
            {
                if (item.Name.Length > MAX_NEWS_TITLE_LENGTH)
                {
                    item.Name = item.Name.Remove(MAX_NEWS_TITLE_LENGTH - 3) + "...";
                }
            }
            lvNews.DataSource = news;
            lvNews.DataBind();

        }
    }
}