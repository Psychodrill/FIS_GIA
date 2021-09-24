namespace Ege.Check.Captcha
{
    using Ege.Check.Common.Config;
    using Ege.Check.Common.Path;
    using JetBrains.Annotations;

    internal class CaptchaCacheSettingsProvider : ICaptchaCacheSettingsProvider
    {
        [NotNull] private readonly IConfigReaderHelper _reader;
        [NotNull] private readonly IPathResolver _resolver;

        public CaptchaCacheSettingsProvider([NotNull] IPathResolver resolver, [NotNull] IConfigReaderHelper reader)
        {
            _resolver = resolver;
            _reader = reader;
        }

        public int GetCaptchaTtlSeconds()
        {
            return _reader.GetInt("CaptchaTtl", "время жизни капчи");
        }

        public bool GetCaptchaTtlEnabled()
        {
            return _reader.GetInt("CaptchaTtlEnabled", "включено ли устаревание капчи") > 0;
        }

        public bool IsCaptchaEnabled()
        {
            return _reader.GetInt("CaptchaEnabled", "включена ли капча") > 0;
        }

        public byte[] GetKey()
        {
            return _reader.GetByteArrayFromBase64("CaptchaKey", "ключ для токена капчи", 32);
        }

        public byte[] GetInitializationVector()
        {
            return _reader.GetByteArrayFromBase64("CaptchaIv", "вектор инициализации для токена капчи", 16);
        }

        public int GetBatchSize()
        {
            return _reader.GetInt("CaptchaCacheBatchSize", "размер генерируемого одним сервером набора капч в кэше");
        }

        public int GetMaxCacheSize()
        {
            return _reader.GetInt("CaptchaCacheMaxSize", "максимальное количество капч в кэше");
        }

        public string GetCaptchasFile()
        {
            return _resolver.Resolve(_reader.GetString("CaptchasFile", "путь к файлу капч"));
        }
    }
}
