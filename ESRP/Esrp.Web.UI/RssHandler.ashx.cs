namespace Esrp.Web
{
    using System;
    using System.Collections.Generic;
    using System.ServiceModel.Syndication;
    using System.Web;
    using System.Xml;

    using Esrp.Services;

    /// <summary>
    /// Rss канал
    /// </summary>
    public class RssHandler : IHttpHandler
    {
        #region Public Properties

        private readonly NewsService newsService = new NewsService();
        private readonly DocumentsService documentsService = new DocumentsService();

        /// <summary>
        /// Gets a value indicating whether IsReusable.
        /// </summary>
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The process request.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        public void ProcessRequest(HttpContext context)
        {
            var url = string.Format(
                "{0}://{1}:{2}/", context.Request.Url.Scheme, context.Request.Url.Host, context.Request.Url.Port);

            var feed = new SyndicationFeed
                {
                    // Название
                    Title = new TextSyndicationContent("Единая система регистрации пользователей"),
                    Copyright = new TextSyndicationContent("© 2013 Рособрнадзор"),
                    Generator = "RSS Feed Generator"
                };

            // Cсылка на источник
            var link = new SyndicationLink { Title = "Единая система регистрации пользователей", Uri = new Uri(url) };
            feed.Links.Add(link);

            // картинка
            feed.ImageUrl = new Uri(string.Format("{0}{1}", url, "Common/Images/rss.gif"));
            
            if ("news".Equals(context.Request.QueryString["type"]))
            {
                feed.Description = new TextSyndicationContent("Rss канал новостей");
                feed.Items = this.GetNews(url);
            }
            else if ("docs".Equals(context.Request.QueryString["type"]))
            {
                feed.Description = new TextSyndicationContent("Rss канал документов");
                feed.Items = this.GetDocs(url);
            }
            else context.Response.End();

            context.Response.Clear();
            context.Response.ContentEncoding = System.Text.Encoding.UTF8;
            context.Response.ContentType = "text/xml";

            var rssWriter = XmlWriter.Create(context.Response.Output);
            var rssFormatter = new Rss20FeedFormatter(feed);
            rssFormatter.WriteTo(rssWriter);
            rssWriter.Close();

            context.Response.End();
        }

        private IEnumerable<SyndicationItem> GetNews(string url)
        {
            var items = new List<SyndicationItem>();

            var news = this.newsService.SelectNews();

            foreach (var itemNews in news)
            {
                var item = new SyndicationItem
                    {
                        Id = Guid.NewGuid().ToString(),
                        Title = new TextSyndicationContent(itemNews.Name),
                        Summary = new TextSyndicationContent(itemNews.Description)
                    };
                item.Categories.Add(new SyndicationCategory("Новости"));
                item.PublishDate = new DateTimeOffset(itemNews.CreateDate);
                var itemLink = string.Format("{0}News.aspx?id={1}", url, itemNews.Id);
                item.AddPermalink(new Uri(itemLink));
                items.Add(item);
            }

            return items;
        }

        private IEnumerable<SyndicationItem> GetDocs(string url)
        {
            var items = new List<SyndicationItem>();

            var documents = this.documentsService.SelectDocuments();

            foreach (var document in documents)
            {
                var item = new SyndicationItem
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = new TextSyndicationContent(document.Name),
                    Summary = new TextSyndicationContent(document.Description)
                };
                item.Categories.Add(new SyndicationCategory("Документы"));
                item.PublishDate = new DateTimeOffset(document.CreateDate);
                var itemLink = (!string.IsNullOrEmpty(Convert.ToString(document.RelativeUrl)))
                               ? document.RelativeUrl
                               : string.Format("{0}Document.aspx?id={1}", url, document.Id);
                item.AddPermalink(new Uri(itemLink));
                items.Add(item);
            }

            return items;
        }

        #endregion
    }
}