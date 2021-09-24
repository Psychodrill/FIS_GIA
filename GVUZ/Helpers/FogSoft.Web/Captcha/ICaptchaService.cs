using System.Drawing;
using System.Web;

namespace FogSoft.Web.Captcha
{
    /// <summary>
    ///     Интерфейс сервиса для генерации Captcha.
    /// </summary>
    /// <remarks>
    ///     Для подключения Captcha необходимо добавить в web.config:
    /// </remarks>
    public interface ICaptchaService
    {
        /// <summary>
        ///     Создаёт и возвращает картинку с CAPTCHA. Не забудьте использовать using.
        /// </summary>
        Bitmap CreateBitmap(CaptchaItem item);

        /// <summary>
        ///     Кэширует <see cref="CaptchaItem" /> . Выбор кэша/сессии зависит от реализации.
        /// </summary>
        void CacheCaptcha(HttpContextBase context, CaptchaItem captchaItem);

        /// <summary>
        ///     Возвращает кэшированный <see cref="CaptchaItem" /> для текущих параметров контекста.
        ///     Выбор кэша/сессии зависит от реализации.
        /// </summary>
        /// <remarks>
        ///     Если <see cref="uid" /> не указан, то он будет получен из параметра формы.
        ///     <seealso
        ///         cref="ICaptchaOptionsService.UidHiddenField" />
        /// </remarks>
        CaptchaItem GetCachedCaptcha(HttpContextBase context, string uid = null);

        /// <summary>
        ///     Удаляет кэшированный <see cref="CaptchaItem" /> для текущих параметров контекста.
        ///     Выбор кэша/сессии зависит от реализации.
        /// </summary>
        void RemoveCachedCaptcha(HttpContextBase context);
    }
}