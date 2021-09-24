using System;
using System.Collections.Specialized;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using WebControls;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using Esrp.Web.Extentions;
namespace Esrp.Web
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
            if (Request.UrlReferrer != null && Request.UrlReferrer.LocalPath!=null)
                referrer = Request.UrlReferrer.ToString();

            // Удалю из адреса исходной стрианицы параметры QueryStartKey и QueryCountKey, 
            // для того, чтобы после возвращения список отображатся с первой страницы
            string url = referrer.IndexOf("?")>0?referrer.Substring(0, referrer.IndexOf("?")):referrer;
            string oldqs =referrer.IndexOf("?")>0?referrer.Substring(referrer.IndexOf("?") + 1):"";
            NameValueCollection queryParams = new NameValueCollection(new HttpRequest("",url,oldqs ).QueryString);

            queryParams.Remove(QueryStartKey);
            queryParams.Remove(QueryCountKey);
            StringBuilder builder = new StringBuilder();
            if (queryParams.Count > 0)
            {
                foreach (string str in queryParams.Keys)
                {
                    if (builder.Length > 0)
                    {
                        builder.Append("&");
                    }
                    if (String.IsNullOrEmpty(str))
                        builder.Append(HttpUtility.UrlEncode(queryParams[str]));
                    else
                        builder.Append(HttpUtility.UrlEncode(str)).Append("=").Append(HttpUtility.UrlEncode(queryParams[str]));
                }
            }
            string qs= builder.ToString();

            // Соберу url обратно
            // забудьте об этом!!! этот метод не эскейпит параметры
           //referrer = UrlUtility.GetUrl(referrer, queryParams);
            referrer = String.Format("{0}?{1}", url, qs);
            // Перейду на предыдущую страницу

            Response.Redirect(referrer, true);
        }
    }
}
