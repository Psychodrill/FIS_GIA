using System;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Web;
using Microsoft.Practices.ServiceLocation;

namespace FogSoft.Web.Captcha
{
    /// <summary>
    ///     Экземляр состояния для вывода Captcha.
    /// </summary>
    /// <seealso cref="ICaptchaService" />
    [DebuggerDisplay("{_text} / {_uid}")]
    public class CaptchaItem
    {
        private readonly string _familyName;
        private readonly int _height;

        private readonly string _uid;
        private readonly int _width;
        private string _text;

        protected CaptchaItem(int charactersCount, int width, int height, string familyName)
        {
            if (charactersCount <= 0) throw new ArgumentOutOfRangeException("charactersCount");
            if (width <= 0) throw new ArgumentOutOfRangeException("width");
            if (height <= 0) throw new ArgumentOutOfRangeException("height");

            _text = Generate(charactersCount);
            _width = width;
            _height = height;

            _familyName = ProcessFamilyName(familyName);

            _uid = Guid.NewGuid().ToString();
        }

        public string Text
        {
            get { return _text; }
        }

        public int Height
        {
            get { return _height; }
        }

        public int Width
        {
            get { return _width; }
        }

        public string FamilyName
        {
            get { return _familyName; }
        }

        public string Uid
        {
            get { return _uid; }
        }

        /// <summary>
        ///     Создаёт <see cref="CaptchaItem" />, используя зарегистрированный в <see cref="ServiceLocator" />
        ///     <see cref="ICaptchaService" />. Если параметры не заданы, используется <see cref="ICaptchaOptionsService" />.
        /// </summary>
        public static CaptchaItem CreateCaptchaItem
            (HttpContextBase context, int charactersCount = 0, int width = 0, int height = 0, string familyName = null)
        {
            if (context == null) throw new ArgumentNullException("context");

            var captchaService = ServiceLocator.Current.GetInstance<ICaptchaService>();
            var optionsService = ServiceLocator.Current.GetInstance<ICaptchaOptionsService>();

            if (charactersCount == 0)
                charactersCount = optionsService.CharactersCount;
            if (width == 0)
                width = optionsService.Width;
            if (height == 0)
                height = optionsService.Height;
            if (string.IsNullOrEmpty(familyName))
                familyName = optionsService.FamilyName;

            var captchaItem = new CaptchaItem(charactersCount, width, height, familyName);
            captchaService.CacheCaptcha(context, captchaItem);
            return captchaItem;
        }

        private string Generate(int charactersCount)
        {
            var random = new Random();

            var builder = new StringBuilder(charactersCount);
            for (int i = 0; i < charactersCount; i++)
                builder.Append(random.Next(0, 10));
            return builder.ToString();
        }

        private string ProcessFamilyName(string familyName)
        {
            if (!String.IsNullOrEmpty(familyName))
            {
                try
                {
                    var font = new Font(_familyName, 12F);
                    font.Dispose();

                    return familyName;
                }
// ReSharper disable EmptyGeneralCatchClause
                catch
// ReSharper restore EmptyGeneralCatchClause
                {
                }
            }
            return FontFamily.GenericSerif.Name;
        }

        /// <summary>
        ///     Используйте этот метод только для тестирования CAPTCHA.
        /// </summary>
        public void SetTextForTests(string text)
        {
            if (text == null) throw new ArgumentNullException("text");
            _text = text;
        }
    }
}