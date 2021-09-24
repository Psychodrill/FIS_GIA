namespace Ege.Check.Captcha
{
    using System;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using Ege.Check.Logic.Models;
    using JetBrains.Annotations;
    using global::Common.Logging;

    internal class DiskCaptchaService : ICaptchaService
    {
        private const int CachedCaptchaCount = 1000000;
        private const int CaptchaBlockLength = 16384;

        [NotNull] private static readonly ILog Logger = LogManager.GetLogger<DiskCaptchaService>();

        [ThreadStatic] private static FileStream _fileStream;
        [NotNull] private readonly ICaptchaGenerator _generator;
        private readonly int _imageOffset;
        private readonly int _numberOffset;
        private readonly int _numberSize;
        private readonly string _path;

        public DiskCaptchaService(
            [NotNull] ICaptchaCacheSettingsProvider settings,
            [NotNull] ICaptchaGenerator generator)
        {
            _generator = generator;

            _path = settings.GetCaptchasFile();

            _numberOffset = sizeof (int);
            _numberSize = generator.TextLength;
            _imageOffset = _numberOffset + _numberSize;
        }

        public void TryGenerateBatch()
        {
            throw new NotSupportedException();
        }

        public int CurrentCachedCount()
        {
            return CachedCaptchaCount;
        }

        public async Task<CachedCaptcha> Retrieve(int id)
        {
            try
            {
                var buffer = new byte[CaptchaBlockLength];
                if (_fileStream == null)
                {
                    _fileStream = File.Open(_path, FileMode.Open, FileAccess.Read, FileShare.Read);
                }
                _fileStream.Seek((long) id*CaptchaBlockLength, SeekOrigin.Begin);
                await _fileStream.ReadAsync(buffer, 0, CaptchaBlockLength);
                var captchaSize = BitConverter.ToInt32(buffer, 0);
                var result = new CachedCaptcha
                    {
                        Number = Encoding.ASCII.GetString(buffer, _numberOffset, _numberSize),
                        Image = Encoding.ASCII.GetString(buffer, _imageOffset, captchaSize)
                    };
                return result;
            }
            catch (Exception ex)
            {
                _fileStream = null;
                Logger.Error(ex);
                return _generator.GenerateOne();
            }
        }

        public bool Generatable
        {
            get { return false; }
        }

        public async Task GenerateCaptchaFile(string path)
        {
            using (var file = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
            {
                var generated = 0;
                var buffer = new byte[CaptchaBlockLength];
                while (generated < CachedCaptchaCount)
                {
                    var captcha = _generator.GenerateOne();
                    if (captcha.Image.Length > CaptchaBlockLength)
                    {
                        continue;
                    }
                    ++generated;
                    Array.Copy(BitConverter.GetBytes(captcha.Image.Length), buffer, _numberOffset);
                    Encoding.ASCII.GetBytes(captcha.Number, 0, _numberSize, buffer, _numberOffset);
                    Encoding.ASCII.GetBytes(captcha.Image, 0, captcha.Image.Length, buffer, _imageOffset);
                    await file.WriteAsync(buffer, 0, CaptchaBlockLength);
                }
            }
        }
    }
}