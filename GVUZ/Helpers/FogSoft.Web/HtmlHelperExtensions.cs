using System;
using System.Text;
using System.Web.Mvc;
using FogSoft.Web.Captcha;
using Microsoft.Practices.ServiceLocation;

namespace FogSoft.Web
{
	public static class HtmlHelperExtensions
	{
		public const string DefaultCaptchaImgId = "captcha-img";
		private const string DefaultCaptchaRefreshId = "captcha-refresh";

		/// <summary>
		/// 	Рисует input для CAPTCHA с заданными параметрами. Если параметры не указаны, используется <see
		///  	cref="ICaptchaOptionsService" /> .
		/// </summary>
		public static MvcHtmlString CaptchaInput(this HtmlHelper html, string name = "captcha", int charactersCount = 0)
		{
			if (html == null) throw new ArgumentNullException("html");
			if (name == null) throw new ArgumentNullException("name");

			ICaptchaOptionsService optionsService = ServiceLocator.Current.GetInstance<ICaptchaOptionsService>();
			charactersCount = charactersCount > 0 ? charactersCount : optionsService.CharactersCount;
			const string inputPattern
				= @"<input type=""text"" id=""{0}"" name=""{0}"" value="""" maxlength=""{1}"" autocomplete=""off"" />";

			return new MvcHtmlString(string.Format(inputPattern, name, charactersCount));
		}

		/// <summary>
		/// 	Рисует CAPTCHA с заданными параметрами. Если параметры не указаны, используется <see cref="ICaptchaOptionsService" /> .
		/// </summary>
		public static MvcHtmlString Captcha(this HtmlHelper html, string imageId = null,
		                                    int charactersCount = 0,
		                                    int height = 0, int width = 0, string familyName = null)
		{
			if (html == null) throw new ArgumentNullException("html");
			ICaptchaOptionsService optionsService = ServiceLocator.Current.GetInstance<ICaptchaOptionsService>();
			width = width > 0 ? width : optionsService.Width;
			height = height > 0 ? height : optionsService.Height;
			charactersCount = charactersCount > 0 ? charactersCount : optionsService.CharactersCount;
			familyName = familyName ?? optionsService.FamilyName;
			var captchaItem = CaptchaItem.CreateCaptchaItem
				(html.ViewContext.HttpContext, charactersCount, width, height, familyName);

			StringBuilder builder = new StringBuilder(256);
			string uidHiddenField = optionsService.UidHiddenField;

			builder.Append("<input type=\"hidden\" name=\"").Append(uidHiddenField)
				.Append("\" id=\"").Append(uidHiddenField)
				.Append("\" value=\"")
				.Append(captchaItem.Uid)
				.Append("\" />")
				.AppendLine()
				.Append("<img id=\"")
				.Append(imageId ?? DefaultCaptchaImgId)
				.Append("\" src=\"")
				.Append(CaptchaImageHandler.GetCaptchaUrl(captchaItem.Uid))
				.Append("\" alt=\"CAPTCHA\" width=\"")
				.Append(width)
				.Append("\" height=\"")
				.Append(height)
				.Append("\" />");

			return new MvcHtmlString(builder.ToString());
		}

		/// <summary>
		/// 	Выводит скрипт для обновления CAPTCHA с заданными параметрами (для использования внутри $(function () {...}).
		/// </summary>
		/// <example>
		/// @Html.Raw(Html.CaptchaRefreshScript
		///		(Url.Generate&lt;AccountController&gt;(x => x.RefreshCaptcha()), "captcha-div").ToString())
		/// </example>
		public static MvcHtmlString CaptchaRefreshScript(this HtmlHelper html, string refreshUrl, string containerId,
		                                                 string imageId = null, string refreshId = null)
		{
			if (html == null) throw new ArgumentNullException("html");
			ICaptchaOptionsService optionsService = ServiceLocator.Current.GetInstance<ICaptchaOptionsService>();

			StringBuilder builder = new StringBuilder(256);
			builder.Append("refreshCaptcha($('#").Append(containerId).AppendLine("'));")
				.AppendLine(@"function refreshCaptcha($container) {")
				.Append("var $captchaImg = $('#").Append(imageId ?? DefaultCaptchaImgId).AppendLine("', $container);")
				.Append("var $captchaGuid = $('#").Append(optionsService.UidHiddenField).AppendLine("', $container);")
				.Append("$('#").Append(refreshId ?? DefaultCaptchaRefreshId)
				.AppendLine("', $container).live('click', function () {")
				.Append("$.post('").Append(refreshUrl).AppendLine("', '', function (data) {")
				.AppendLine("$captchaImg.attr('src', data.src);")
				.AppendLine("$captchaGuid.val(data.uid);});")
				.AppendLine("});}");

			return new MvcHtmlString(builder.ToString());
		}
	}
}
