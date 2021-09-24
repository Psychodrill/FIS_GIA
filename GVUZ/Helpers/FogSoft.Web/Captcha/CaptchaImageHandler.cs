using System;
using System.Drawing;
using System.Reflection;
using System.Web;
using System.Web.SessionState;
using Microsoft.Practices.ServiceLocation;
using log4net;

namespace FogSoft.Web.Captcha
{
    /// <summary>
    ///     Обработчик запросов для вывода Captcha.
    /// </summary>
    /// <seealso cref="ICaptchaService" />
    /// <remarks>
    ///     Для подключения необходимо добавить в web.config (секция httpHandlers) код вида: &lt;add verb="GET" path="captcha.ashx" validate="false" type="FogSoft.Web.Captcha.CaptchaImageHandler, FogSoft.Web, Version=1.0.0.0, Culture=neutral" /&gt;
    /// </remarks>
    public class CaptchaImageHandler : IHttpHandler, IRequiresSessionState
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void ProcessRequest(HttpContext httpContext)
        {
            if (httpContext == null)
                return;

            var context = new HttpContextWrapper(httpContext);

            SaveCaptcha(context);
        }

        public bool IsReusable
        {
            get { return true; }
        }

        public bool SaveCaptcha(HttpContextBase context)
        {
            try
            {
                var service = ServiceLocator.Current.GetInstance<ICaptchaService>();
                var optionsService = ServiceLocator.Current.GetInstance<ICaptchaOptionsService>();

                string uid = context.Request.QueryString["uid"];
                CaptchaItem item = service.GetCachedCaptcha(context, uid);

                if (item != null)
                {
                    using (Bitmap captcha = service.CreateBitmap(item))
                    {
                        context.Response.ContentType = optionsService.ContentType;
                        captcha.Save(context.Response.OutputStream, optionsService.ImageFormat);

                        context.Response.StatusCode = 200;
                        context.Response.StatusDescription = "OK";
                    }
                    return true;
                }
            }
            catch (Exception e)
            {
                Log.Error(e.Message, e);
            }

            context.Response.StatusCode = 404;
            context.Response.StatusDescription = "Not Found";
            return false;
        }

        public static string GetCaptchaUrl(string uid)
        {
            return VirtualPathUtility.ToAbsolute("~/captcha.ashx?uid=" + uid);
        }
    }
}