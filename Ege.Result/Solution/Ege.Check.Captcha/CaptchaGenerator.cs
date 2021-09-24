namespace Ege.Check.Captcha
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.IO;
    using Ege.Check.Common.Random;
    using Ege.Check.Logic.Models;
    using JetBrains.Annotations;
    using XtractPro.Multimedia;

    internal class CaptchaGenerator : ICaptchaGenerator
    {
        /// <summary>
        ///     Количество символов в картинке
        /// </summary>
        public const int SymbolsCount = 6;

        [ThreadStatic] private static Random _random;

        [NotNull] private readonly IRandomCreator _randomCreator;

        public CaptchaGenerator([NotNull] IRandomCreator randomCreator)
        {
            _randomCreator = randomCreator;
        }

        public CachedCaptcha GenerateOne()
        {
            var generator = new CaptchaImage(_random ?? (_random = _randomCreator.Create()))
                {
                    Text = "",
                    Width = 240,
                    Height = 60,
                    LinesFactor = 0,
                    NoiseFactor = 0,
                    WarpFactor = 100,
                    BackgroundBackColor = Color.White,
                    BackgroundForeColor = Color.DarkGray,
                    BackgroundStyle = HatchStyle.ZigZag,
                    TextBackColor = Color.LightSteelBlue,
                    TextForeColor = Color.CornflowerBlue,
                    TextStyle = HatchStyle.Trellis,
                    CharacterSet = "1234567890",
                    Length = SymbolsCount,
                    Unique = true,
                    FontName = "Tahoma",
                    FontSize = 25,
                    FontStyle = FontStyle.Bold,
                };
            var image = generator.GetImage();
            if (image == null)
            {
                throw new InvalidOperationException("CaptchaImage generated null image");
            }
            if (generator.Text == null)
            {
                throw new InvalidOperationException("CaptchaImage generated null image");
            }
            using (var stream = new MemoryStream())
            {
                image.Save(stream, ImageFormat.Jpeg);
                return new CachedCaptcha
                    {
                        Image = Convert.ToBase64String(stream.ToArray()),
                        Number = generator.Text,
                    };
            }
        }


        public int TextLength
        {
            get { return SymbolsCount; }
        }
    }
}