namespace Esrp.Web.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.UI;

    /// <summary>
    /// Пейджер
    /// </summary>
    public class Pager_Simple : Control
    {
        #region Constants and Fields

        /// <summary>
        /// Размеры страниц по умолчанию
        /// </summary>
        public const string defaultPageSizes = "20, 50, 100";

        /// <summary>
        /// Количество записей
        /// </summary>
        public int ItemCount;

        /// <summary>
        /// Для чего пейджинг
        /// Например если список организации, то ItemName = "Организации"
        /// </summary>
        public string ItemName = "Организации";

        /// <summary>
        /// Номер страницы
        /// </summary>
        public int PageNumber;

        /// <summary>
        /// Размер страницы по умолчанию
        /// </summary>
        public int PageSize = 20;

        /// <summary>
        /// Размеры страниц по умолчанию
        /// </summary>
        public string PageSizes = defaultPageSizes;

        #endregion

        #region Properties

        /// <summary>
        /// Номер максимальной страницы
        /// </summary>
        private int MaxPageNum
        {
            get
            {
                return (int)Math.Ceiling(((decimal)this.ItemCount) / this.PageSize);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The get current url.
        /// </summary>
        /// <param name="excludeParamName">
        /// The exclude param name.
        /// </param>
        /// <returns>
        /// The get current url.
        /// </returns>
        public string GetCurrentUrl(string excludeParamName)
        {
            string url = string.Empty;
            for (int i = 0; i < this.Page.Request.QueryString.Count; i++)
            {
                string key = this.Page.Request.QueryString.GetKey(i);
                if (!string.IsNullOrEmpty(key))
                {
                    string val = HttpUtility.UrlEncode(this.Page.Request.QueryString.Get(key));

                    if (key.ToLower() != excludeParamName.ToLower())
                    {
                        url += string.Format("{2}{0}={1}", key, val, url.Length == 0 ? "?" : "&");
                    }
                }
            }

            url = this.Page.Request.Path + url + string.Format("{1}{0}=", excludeParamName, url.Length == 0 ? "?" : "&");

            return url;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The on load.
        /// </summary>
        /// <param name="e">
        /// Параметры события
        /// </param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.InitState();

            var startRowIndex = (this.PageNumber * this.PageSize) + 1;
            if (startRowIndex > this.ItemCount)
            {
                startRowIndex = this.ItemCount;
            }

            var lastRowIndex = (this.PageNumber + 1) * this.PageSize;
            if (lastRowIndex > this.ItemCount)
            {
                lastRowIndex = this.ItemCount;
            }

            //this.Controls.Add(new LiteralControl("<table class=\"pager\"><tr><td>"));
            //this.Controls.Add(
            //    new LiteralControl(
            //        string.Format("{0} {1}-{2} из {3}. ", this.ItemName, startRowIndex, lastRowIndex, this.ItemCount)));
            //this.Controls.Add(new LiteralControl("Записей на странице:"));
            //this.AddPageCtrls();
            //this.Controls.Add(new LiteralControl("</td><td  align=\"right\">"));
            //this.AddPageNumbers();
            //this.Controls.Add(new LiteralControl("</td></tr></table>"));

            this.Controls.Add(new LiteralControl("<div class=\"sort_page\">\n"));
            this.AddPageCtrls();
            this.Controls.Add(new LiteralControl("</div>\n"));
            this.Controls.Add(
                new LiteralControl(
                    string.Format("<p class=\"rec\">Записей на странице:&nbsp;</p>\n" +
						"<p class=\"views\">Показано <span>{0}-{1}</span> из {2}.</p>\n", 
                            startRowIndex, lastRowIndex, this.ItemCount)));
            this.Controls.Add(new LiteralControl("<p class=\"page_nav\">Страницы&nbsp;\n"));
            this.AddPageNumbers();
            this.Controls.Add(new LiteralControl("</p><div class=\"clear\"></div>\n"));
        }

        private void AddCtrl(string url, int pageSize, bool isEnd)
        {
            this.Controls.Add(
                pageSize == this.PageSize
                    ? new LiteralControl(pageSize.ToString())
                    : new LiteralControl(string.Format("<a href='{0}{1}'>{1}</a>", url, pageSize)));

            if (!isEnd)
            {
                this.Controls.Add(new LiteralControl(", "));
            }
        }

        /// <summary>
        /// Добавляются размеры страниц в верстку
        /// </summary>
        private void AddPageCtrls()
        {
            string url = this.GetCurrentUrl("pageSize");
            SortedList<int, int> sList = this.parsePageSizes(this.PageSizes);
            if (sList.Count == 0)
            {
                sList = this.parsePageSizes(defaultPageSizes);
            }

            string jsOnChangeFunctionName = "changePageCount";
            if (this.ClientID.ToLower().Contains("top"))
            {
                jsOnChangeFunctionName = "changePageCountTop";
            }
            else if (this.ClientID.ToLower().Contains("bottom"))
            {
                jsOnChangeFunctionName = "changePageCountBottom";
            }

            this.Controls.Add(new LiteralControl("<select id=\"" + this.ClientID + "\" class=\"_change_select_page_size\" onchange=\"" + jsOnChangeFunctionName + "()\">"));

            for (int i = 0; i < sList.Count; i++)
            {
                //this.AddCtrl(url, sList.Values[i], i == sList.Count - 1);
                string selected = sList.Values[i] == this.PageSize? "selected=\"selected\"" : "";
                this.Controls.Add(new LiteralControl("<option " + selected + " value=\"" + sList.Values[i] + "\"><span>" + sList.Values[i] + "</span></option>"));
            }
            this.Controls.Add(new LiteralControl("</select>"));
        }

        /// <summary>
        /// Добавляются "Первая", "Предыдущая", Цифры между, "Следущая", "Последняя"
        /// </summary>
        private void AddPageNumbers()
        {
            var url = this.GetCurrentUrl("pageNum");

            if (this.MaxPageNum == 1)
            {
                return;
            }

            // Добавляем "Первая"
            // Добавляем "Предыдущая"
            if (this.PageNumber > 0)
            {
                this.Controls.Add(new LiteralControl(string.Format("<a href='{0}{1}'><<&nbsp</a>", url, 0)));
                this.Controls.Add(
                    new LiteralControl(string.Format("<a href='{0}{1}'><</a>&nbsp;", url, this.PageNumber - 1)));
            }

            // Добавляем цифры
            int pageNumStart = this.PageNumber > 2 ? this.PageNumber - 2 : 0;
            int pageNumFinish = this.PageNumber + 2 < this.MaxPageNum ? this.PageNumber + 2 : this.MaxPageNum;

            for (int pageNum = pageNumStart; pageNum <= pageNumFinish; pageNum++)
            {
                if (pageNum < this.MaxPageNum)
                {
                    this.Controls.Add(
                        pageNum == this.PageNumber
                            ? new LiteralControl(string.Format("<span>{0}</span>", (pageNum + 1).ToString()))
                            : new LiteralControl(string.Format("<a href='{0}{1}'>{2}</a>", url, pageNum, pageNum + 1)));

                    this.Controls.Add(new LiteralControl("&nbsp;"));
                }
            }

            // Добавляем "Следущая"
            // Добавляем "Последняя"
            if (this.PageNumber < this.MaxPageNum - 1)
            {
                this.Controls.Add(new LiteralControl(string.Format("<a href='{0}{1}'>></a>", url, this.PageNumber + 1)));
                this.Controls.Add(
                    new LiteralControl(string.Format("<a href='{0}{1}'>&nbsp;>></a>&nbsp;", url, this.MaxPageNum - 1)));
            }
        }

        private void InitState()
        {
            int pageSize;
            if (int.TryParse(this.Page.Request.QueryString.Get("pageSize"), out pageSize))
            {
                if (pageSize > 0)
                {
                    this.PageSize = pageSize;
                }
            }

            int pageNum = 0;
            if (int.TryParse(this.Page.Request.QueryString.Get("pageNum"), out pageNum))
            {
                if (pageNum >= 0)
                {
                    this.PageNumber = pageNum;
                }
            }
        }

        private SortedList<int, int> parsePageSizes(string pageSizesString)
        {
            var sList = new SortedList<int, int>();
            string[] pageSizes = this.PageSizes.Split(new[] { ',', ' ', ';' });
            foreach (string pageSize in pageSizes)
            {
                int val;
                if (int.TryParse(pageSize, out val))
                {
                    if (val > 0)
                    {
                        if (!sList.ContainsKey(val))
                        {
                            sList.Add(val, val);
                        }
                    }
                }
            }

            return sList;
        }

        #endregion
    }
}