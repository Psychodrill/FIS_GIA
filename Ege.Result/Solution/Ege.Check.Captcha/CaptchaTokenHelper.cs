namespace Ege.Check.Captcha
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;
    using JetBrains.Annotations;
    using global::Common.Logging;

    internal class CaptchaTokenHelper : ICaptchaTokenHelper
    {
        [NotNull] public static byte[] Key;
        [NotNull] public static byte[] RgbIv;

        [NotNull] private static readonly ILog Logger = LogManager.GetLogger<CaptchaTokenHelper>();

        private long CaptchaTicksToLive;
        private bool CheckCaptchaTicksToLive;
        public bool CheckCaptcha { get; private set; }
        [NotNull] private static readonly Regex Base64Regex = new Regex(@"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);
        [NotNull] private readonly ICaptchaCacheSettingsProvider _settings;

        public CaptchaTokenHelper([NotNull] ICaptchaCacheSettingsProvider settings)
        {
            _settings = settings;

            TryUpdateKey();
            try
            {
                CaptchaTicksToLive = settings.GetCaptchaTtlSeconds()*10000000L;
                CheckCaptchaTicksToLive = settings.GetCaptchaTtlEnabled();
                CheckCaptcha = settings.IsCaptchaEnabled();
            }
            catch (Exception ex)
            {
                CheckCaptchaTicksToLive = false;
                Logger.Error(ex);
            }
        }

        public string Generate(string text)
        {
            using (var aesManaged = new AesManaged {KeySize = 256, BlockSize = 128, Mode = CipherMode.CBC})
            using (var transform = aesManaged.CreateEncryptor(Key, RgbIv))
            using (var ms = new MemoryStream())
            {
                using (var outStreamEncrypted = new CryptoStream(ms, transform, CryptoStreamMode.Write))
                {
                    var bytes = Encoding.ASCII.GetBytes(text);
                    var now = BitConverter.GetBytes(DateTime.Now.Ticks);
                    outStreamEncrypted.Write(bytes, 0, bytes.Length);
                    outStreamEncrypted.Write(now, 0, now.Length);
                }
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        public bool IsCorrect(string text, string token)
        {
            if (!CheckCaptcha)
            {
                return true;
            }
            return text != null && token != null && IsValidBase64(token) && IsValidToken(token, text);
        }

        private bool IsValidToken([NotNull] string token, [NotNull] string text)
        {
            using (var aesManaged = new AesManaged {KeySize = 256, BlockSize = 128, Mode = CipherMode.CBC})
            using (var transform = aesManaged.CreateDecryptor(Key, RgbIv))
            using (var ms = new MemoryStream())
            {
                using (var outStreamEncrypted = new CryptoStream(ms, transform, CryptoStreamMode.Write))
                {
                    var bytes = Convert.FromBase64String(token);
                    outStreamEncrypted.Write(bytes, 0, bytes.Length);
                }
                var decodedTextArray = ms.ToArray();
                var decoded = Encoding.ASCII.GetString(decodedTextArray);
                if (text.Length + 8 > decodedTextArray.Length)
                {
                    return false;
                }
                var ticks = BitConverter.ToInt64(decodedTextArray, text.Length);
                return decoded.StartsWith(text) &&
                       (!CheckCaptchaTicksToLive || (DateTime.Now.Ticks - ticks) <= CaptchaTicksToLive);
            }
        }

        private static bool IsValidBase64([NotNull] string base64)
        {
            return (base64.Length%4 == 0) && Base64Regex.IsMatch(base64);
        }

        public bool TryUpdateKey()
        {
            try
            {
                Key = _settings.GetKey();
                RgbIv = _settings.GetInitializationVector();
                return true;
            }
            catch (Exception ex)
            {
                Key = new byte[]
                    {
                        0xFA, 0xD2, 0x26, 0x26, 0x41, 0xB3, 0x77, 0x70,
                        0xB0, 0x04, 0xEF, 0xE3, 0xA6, 0x36, 0x6C, 0xA8,
                        0x6E, 0xAF, 0xDC, 0x0E, 0x0A, 0x9C, 0x2D, 0xBC,
                        0x81, 0x31, 0x57, 0xD5, 0x12, 0xD4, 0x05, 0x89
                    };
                RgbIv = new byte[]
                    {
                        0x58, 0x79, 0x7A, 0x5E, 0x9D, 0x84, 0x22, 0xBB,
                        0x95, 0xE0, 0x9D, 0x9E, 0xD8, 0xEC, 0x1F, 0xA4
                    };
                Logger.Error(ex);
                return false;
            }
        }
    }
}
