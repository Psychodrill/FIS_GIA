using System;
using System.Web;
using System.Web.Mvc;
using FogSoft.Web.Captcha;
using Microsoft.Practices.ServiceLocation;

namespace FogSoft.Web.Mvc
{
	/// <summary>
	/// Используется для валидации <see cref="HtmlHelperExtensions.CaptchaInput"/>.</summary>
	/// <remarks>Если используется кнопка обновления CAPTCHA, не делайте валидацию на обновляющий Action контроллера,
	/// потому что он сбросит значение.</remarks>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	public class CaptchaValidAttribute : ActionFilterAttribute
	{
		public const string CaptchaIsValidParameter = "captchaIsValid";
		private const string DefaultFieldName = "captcha";

		public override void OnActionExecuting(ActionExecutingContext context)
		{
			if (!Validate(context.HttpContext))
				context.Controller.ViewData.ModelState.AddModelError(Field, Errors.CaptchaValidAttribute_InvalidText);
		}

		public bool Validate(HttpContextBase context)
		{
			bool valid = false;
			ICaptchaService captchaService = ServiceLocator.Current.GetInstance<ICaptchaService>();
			CaptchaItem item = captchaService.GetCachedCaptcha(context);
			if (item != null)
			{
				string actualValue = context.Request.Form[Field];
				string expectedValue = item.Text;

				// удаляем из кэша, чтобы нельзя было использовать значение еще раз
				captchaService.RemoveCachedCaptcha(context);

				valid = !string.IsNullOrEmpty(actualValue) &&
				        expectedValue.Equals(actualValue, StringComparison.OrdinalIgnoreCase);
			}
			return valid;
		}

		private string _field;

		/// <summary>
		/// Возвращает или устанавливает название поля на форме, отвечающего за ввод CAPTCHA.
		/// Если не указано, используется 'captcha'.</summary>
		public string Field
		{
			get { return _field ?? DefaultFieldName; }
			set { _field = value; }
		}
	}
}
