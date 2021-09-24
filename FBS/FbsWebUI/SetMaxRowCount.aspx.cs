using System;
using System.Collections.Specialized;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using WebControls;

namespace Fbs.Web
{
    /// <summary>
    /// Разрешенные вариант количества страниц списка
    /// </summary>
    public enum MaxRowCountEnum
    {
        None = 0,
        Ten = 10,
        Twenty = 20,
        Fifty = 50,
        Hundred = 100
    }

    public partial class SetMaxRowCount : System.Web.UI.Page
    {
        private const string QueryStartKey = "start";
        private const string QueryCountKey = "count";
        private const string CookieCountKey = "count";
        // Дата окончания куки
        private DateTime CookieExpireDate = new DateTime(2020, 1, 1);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Request.QueryString[QueryCountKey]))
                return;

            int value;
            if (!int.TryParse(Request.QueryString[QueryCountKey], out value))
                throw new ArgumentException(QueryCountKey);

            // Проверю допустимость передаваемого параметра
            MaxRowCountEnum count;
            switch (value)
            {
                case (int)MaxRowCountEnum.Ten:
                    count = MaxRowCountEnum.Ten;
                    break;

                case (int)MaxRowCountEnum.Twenty:
                    count = MaxRowCountEnum.Twenty;
                    break;
                case (int)MaxRowCountEnum.Fifty:
                    count = MaxRowCountEnum.Fifty;
                    break;
                case (int)MaxRowCountEnum.Hundred:
                    count = MaxRowCountEnum.Hundred;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(QueryCountKey);
            }

            // Установлю куку
            HttpCookie cookie = new HttpCookie(CookieCountKey, Convert.ToString((int)count));
            cookie.Expires = CookieExpireDate;
            Request.Cookies.Set(cookie);
            Response.Cookies.Set(cookie);

            // Получу предыдущую страницу
            string referrer = "/";
            if (Request.UrlReferrer != null)
                referrer = Request.UrlReferrer.ToString();

            // Удалю из адреса исходной стрианицы параметры QueryStartKey и QueryCountKey, 
            // для того, чтобы после возвращения список отображатся с первой страницы
            NameValueCollection queryParams = UrlUtility.GetParams(referrer);
            queryParams.Remove(QueryStartKey);
            queryParams.Remove(QueryCountKey);

            // Соберу url обратно
            referrer = UrlUtility.GetUrl(referrer, queryParams);

            // Перейду на предыдущую страницу
            Response.Redirect(referrer, true);
        }
    }
}
