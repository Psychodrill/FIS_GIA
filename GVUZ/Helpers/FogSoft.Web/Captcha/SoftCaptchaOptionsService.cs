using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using FogSoft.Helpers;

namespace FogSoft.Web.Captcha
{
    /// <summary>
    ///     Настройки по умолчанию для <see cref="SoftCaptchaService" /> .
    /// </summary>
    public class SoftCaptchaOptionsService : ICaptchaOptionsService
    {
        private int? _charactersCount;
        private string _familyName;
        private int? _height;
        private int? _width;

        public string ContentType
        {
            get { return "image/jpeg"; }
        }

        public string UidHiddenField
        {
            get { return AppSettings.Get("SoftCaptchaOptions.UidHiddenField", "captcha-guid"); }
        }

        public int Width
        {
            get { return (_width = _width ?? AppSettings.Get("SoftCaptchaOptions.Width", 150)).Value; }
        }

        public int Height
        {
            get { return (_height = _height ?? AppSettings.Get("SoftCaptchaOptions.Height", 50)).Value; }
        }

        public int CharactersCount
        {
            get
            {
                if (!_charactersCount.HasValue)
                {
                    _charactersCount = AppSettings.Get("SoftCaptchaOptions.TextLength", 6);
                    if (_charactersCount < 1)
                        throw new ConfigurationErrorsException(Errors.SoftCaptchaOptions_InvalidCharactersCount);
                }
                return _charactersCount.Value;
            }
        }

        public string FamilyName
        {
            get { return _familyName = _familyName ?? FontFamily.GenericSerif.Name; }
        }

        public ImageFormat ImageFormat
        {
            get { return ImageFormat.Jpeg; }
        }
    }
}