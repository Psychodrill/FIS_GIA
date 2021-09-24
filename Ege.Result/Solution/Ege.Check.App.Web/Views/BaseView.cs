namespace Ege.Check.App.Web.Views
{
    using System.IO;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using Ege.Check.App.Web.Common.Auth;
    using Newtonsoft.Json;

    public class BaseView : WebViewPage
    {
        public new IStaffPrincipal User
        {
            get { return base.User as IStaffPrincipal; }
        }

        public IHtmlString RawJson(object obj)
        {
            return Html.Raw(HttpUtility.JavaScriptStringEncode(JsonConvert.SerializeObject(obj)));
        }

        public IHtmlString RawHtml(string path)
        {
            var realPath = Server.MapPath(Url.Content(path));
            return Html.Raw(File.ReadAllText(realPath, Encoding.UTF8));
        }

        public override void Execute()
        {
        }
    }

    public class BaseView<T> : WebViewPage<T>
    {
        public new IStaffPrincipal User
        {
            get { return base.User as IStaffPrincipal; }
        }


        public IHtmlString RawJson(object obj)
        {
            return Html.Raw(HttpUtility.JavaScriptStringEncode(JsonConvert.SerializeObject(obj)));
        }

        public IHtmlString RawHtml(string path)
        {
            var realPath = Server.MapPath(Url.Content(path));
            return Html.Raw(File.ReadAllText(realPath, Encoding.UTF8));
        }

        public override void Execute()
        {
        }
    }
}