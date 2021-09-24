using System.Drawing.Imaging;
using System.Web;

namespace FogSoft.Web.Captcha
{
    /// <summary>
    ///     Настройки по для <see cref="ICaptchaService" /> .
    /// </summary>
    public interface ICaptchaOptionsService
    {
        /// <summary>
        ///     <see cref="HttpResponse.ContentType" /> .
        ///     Должен поддерживаться <see cref="ICaptchaService" /> и соответствовать <see cref="ImageFormat" />.
        /// </summary>
        string ContentType { get; }

        /// <summary>
        ///     Название скрытого поля с уникальным идентификатором.
        /// </summary>
        string UidHiddenField { get; }

        /// <summary>
        ///     Ширина картинки.
        /// </summary>
        int Width { get; }

        /// <summary>
        ///     Высота картинки.
        /// </summary>
        int Height { get; }

        /// <summary>
        ///     Количество символов для вывода в картинку.
        /// </summary>
        int CharactersCount { get; }

        /// <summary>
        ///     Формат картинки.
        ///     Должен поддерживаться <see cref="ICaptchaService" /> и соответствовать <see cref="ContentType" />.
        /// </summary>
        ImageFormat ImageFormat { get; }

        /// <summary>
        ///     Font-family для вывода текста на картинку.
        /// </summary>
        string FamilyName { get; }
    }
}