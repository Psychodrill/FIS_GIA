using System;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using FogSoft.Helpers;
using Microsoft.Web.Mvc;

namespace GVUZ.Helper
{
    public static class UrlHelperExtensions
    {
        /// <summary>
        ///     Урл для портлетов
        /// </summary>
        private const string PortletUrlTemplate =
            "wsrp_rewrite?wsrp-urlType=resource&wsrp-url={0}&wsrp-requiresRewrite=false&wsrp-secureURL=false/wsrp_rewrite";

        private static readonly string AppVirtualPath;

        /// <summary>
        ///     Инициализация, проставляем правильный путь приложения
        /// </summary>
        static UrlHelperExtensions()
        {
            AppVirtualPath = HttpRuntime.AppDomainAppVirtualPath;

            if (AppVirtualPath != null && AppVirtualPath.EndsWith("/") == false) AppVirtualPath += "/";
        }

        /// <summary>
        ///     Проверяем, в портлете ли мы.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool IsInsidePortlet(this UrlHelper url)
        {
            return url.RequestContext.HttpContext.InsidePortlet();
        }

        /// <summary>
        ///     Если в портлете и есть атрибут, указывающий что делать, вручную вызываем метод контроллера, чтобы он думал что его напрямую вызвали
        /// </summary>
        private static string CheckPortletAttribute<TController>(Expression<Action<TController>> action)
        {
            var me = action.Body as MethodCallExpression;
            if (me != null)
            {
                object[] attr = me.Method.GetCustomAttributes(typeof (GeneratorPortletLinkAttribute), false);
                if (attr.Length > 0)
                {
                    var pla = attr[0] as GeneratorPortletLinkAttribute;
                    var args = new object[me.Arguments.Count];
                    for (int i = 0; i < args.Length; i++)
                    {
                        var expr = me.Arguments[i] as ConstantExpression;
                        if (expr != null)
                            args[i] = expr.Value;
                        else
                            args[i] = CachedExpressionCompiler.Evaluate(me.Arguments[i]);
                    }
                    return pla.ExecuteMethod(args);
                }
            }
            return null;
        }

        /// <summary>
        ///     Создаём правильный url до метода контроллера. Если в портлете, то делаем хитрый, иначе стандартный
        /// </summary>
        public static string Generate<TController>(this UrlHelper urlHelper,
                                                   Expression<Action<TController>> action)
            where TController : Controller
        {
            bool insidePortlet = urlHelper.IsInsidePortlet();
            if (insidePortlet)
            {
                string portletUrl = CheckPortletAttribute(action);
                if (portletUrl != null)
                    return portletUrl;
            }
            string path = insidePortlet
                              ? PreProcessUrl(
                                  LinkBuilder.BuildUrlFromExpression(urlHelper.RequestContext, urlHelper.RouteCollection,
                                                                     action), true)
                              : LinkBuilder.BuildUrlFromExpression(urlHelper.RequestContext, urlHelper.RouteCollection,
                                                                   action);

            //внутри портлета, берём абсолютный префикс пути
            if (insidePortlet)
                path = GetAbsolutePathPrefix(urlHelper.RequestContext.HttpContext.Request.Url) + path;

            return PostProcessUrl(urlHelper, path, urlHelper.IsInsidePortlet());
        }

        /// <summary>
        ///     Генеририуем ссылку
        /// </summary>
        public static string GenerateLink<TController>(this UrlHelper urlHelper,
                                                       Expression<Action<TController>> action, string linkText)
            where TController : Controller
        {
            string url = Generate(urlHelper, action);
            return String.Format("<a href=\"{0}\">{1}</a>", url, HttpUtility.HtmlEncode(linkText));
        }

        /// <summary>
        ///     Генеририуем ссылку, если условие true (метод для удобства, когда нужно прятать ссылки при определённых правилах)
        /// </summary>
        public static string GenerateLinkIf<TController>(this UrlHelper urlHelper,
                                                         Expression<Action<TController>> action, string linkText,
                                                         bool condition, string noDocText = null)
            where TController : Controller
        {
            if (!condition) return noDocText ?? "";
            string url = Generate(urlHelper, action);
            return String.Format("<a href=\"{0}\">{1}</a>", url, HttpUtility.HtmlEncode(linkText));
        }

        /// <summary>
        ///     Генерация пути к ресурсам
        /// </summary>
        public static string Resource(this UrlHelper urlHelper, string path)
        {
            bool insidePortlet = urlHelper.IsInsidePortlet();
            path = PreProcessUrl(path, insidePortlet);

            if (insidePortlet)
                path = GetAbsolutePathPrefix(urlHelper.RequestContext.HttpContext.Request.Url) + path;
            else
                path = AppVirtualPath + path;

            return PostProcessUrl(urlHelper, path, insidePortlet);
        }

        /// <summary>
        ///     Генерация пути к картинкам
        /// </summary>
        public static string Images(this UrlHelper url, string path)
        {
            return Resource(url, "Resources/Images/" + RemoveFirstSlash(path));
        }

        /// <summary>
        ///     Генерация пути к скриптам
        /// </summary>
        public static string Scripts(this UrlHelper url, string path)
        {
            return Resource(url, "Resources/Scripts/" + RemoveFirstSlash(path));
        }

        /// <summary>
        ///     Генерация ссылки, не для портлетов
        /// </summary>
        public static MvcHtmlString Link<TController>(this UrlHelper url,
                                                      Expression<Action<TController>> action, string linkText)
            where TController : Controller
        {
            if (IsInsidePortlet(url))
                throw new InvalidOperationException
                    ("Попытка сгенерировать ссылку '{0}' внутри портлета".FormatWith(linkText));
            return MvcHtmlString.Create("<a href={0}>{1}</a>".FormatWith(url.Generate(action), linkText));
        }

// ReSharper disable UnusedParameter.Local
        private static string PreProcessUrl(string path, bool insidePortlet)
// ReSharper restore UnusedParameter.Local
        {
            // return !insidePortlet ? path : "portlets/" + (path.StartsWith("/") ? path.Substring(1) : path);
            return RemoveFirstSlash(path);
        }

        private static string PostProcessUrl(UrlHelper urlHelper, string url, bool insidePortlet)
        {
            return !insidePortlet ? url : PortletUrlTemplate.FormatWith(urlHelper.Encode(url));
        }

        private static string RemoveFirstSlash(string path)
        {
            return (path != null && path.StartsWith("/") ? path.Substring(1) : path);
        }

        /// <summary>
        ///     Генерация абсолютного пути. Используется для портлетов
        /// </summary>
        private static string GetAbsolutePathPrefix(Uri uri)
        {
            if (uri == null) throw new ArgumentNullException("uri");
            string authority = uri.Authority;
            string path = "{0}://{1}/{2}".FormatWith
                (uri.Scheme, authority, "localhost".Equals(authority) ? uri.Segments[1] : "");
            return path;
        }
    }
}