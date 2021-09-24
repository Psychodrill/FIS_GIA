using System.Collections.Generic;
using System.Text;

namespace GVUZ.Web.Portlets
{
    public class PagerBuilder
    {
        private class PagerLink
        {
            public string Title { get; set; }
            public int PageNo { get; set; }
            public string Class { get; set; }
            public string OnClick { get; set; }
        }

        private readonly string _urlTemplate;
        private readonly List<PagerLink> _pagerLinks =
                new List<PagerLink>();

        public PagerBuilder(string urlTemplate)
        {
            _urlTemplate = urlTemplate;
        }

        public string PagerClass { get; set; }

        public string PagerAction { get; set; }

        public int PagerPage { get; set; }

        public void AddPage(string title, int pageNo)
        {
            AddPage(title, pageNo, string.Empty, string.Empty);
        }

        public void AddPage(string title, int pageNo,
                            string itemClass)
        {
            AddPage(title, pageNo, itemClass, string.Empty);
        }

        public void AddPage(string title, int pageNo,
                            string itemClass, string itemClick)
        {
            var link = new PagerLink
            {
                PageNo = pageNo,
                Title = title,
                Class = itemClass,
                OnClick = itemClick
            };
            _pagerLinks.Add(link);
        }
        
        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("<form method=\"post\" id=\"Pager\" name=\"Pager\"");

            if (!string.IsNullOrEmpty(PagerClass))
            {
                builder.Append(" class=\"");
                builder.Append(PagerClass);
                builder.Append("\"");
            }
            if (!string.IsNullOrEmpty(PagerAction))
            {
                builder.Append(" action=\"");
                builder.Append(PagerAction);
                builder.Append("\"");
            }
            builder.Append(">");
            builder.Append("<input type=\"hidden\" id=\"PageNumber\" name=\"PageNumber\" value=\"");
            builder.Append(PagerPage);
            builder.Append("\"/>");
            
            foreach (var link in _pagerLinks)
            {
                builder.Append("<input type=\"submit\" value=\"");
                builder.Append(link.Title);
                builder.Append("\"");

                builder.Append(" id=\"btn");
                builder.Append(link.PageNo);
                builder.Append("\"");

                builder.Append(" name=\"btn");
                builder.Append(link.PageNo);
                builder.Append("\"");

                if (!string.IsNullOrEmpty(link.Class))
                {
                    builder.Append(" class=\"");
                    builder.Append(link.Class);
                    builder.Append("\"");
                }
                if (!string.IsNullOrEmpty(link.OnClick))
                {
                    builder.Append(" onclick=\"");
                    builder.Append(link.OnClick);
                    builder.Append("\"");
                }
                builder.Append("/>");
               /* builder.Append("<a href=\"");
                builder.AppendFormat(_urlTemplate, link.PageNo);
                builder.Append("\"");

                if (!string.IsNullOrEmpty(link.Class))
                {
                    builder.Append(" class=\"");
                    builder.Append(link.Class);
                    builder.Append("\"");
                }

                if (!string.IsNullOrEmpty(link.OnClick))
                {
                    builder.Append(" onclick=\"");
                    builder.Append(link.OnClick);
                    builder.Append("\"");
                }
                
                builder.Append(">");
                builder.Append(link.Title);
                builder.Append("</a>");*/
            }
            builder.Append("</form>");
            return builder.ToString();
        }
    }
}