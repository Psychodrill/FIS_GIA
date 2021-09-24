namespace Ege.Check.Captcha
{
    using JetBrains.Annotations;

    public interface ICaptchaCacheSettingsProvider
    {
        int GetBatchSize();

        int GetMaxCacheSize();

        int GetCaptchaTtlSeconds();

        bool GetCaptchaTtlEnabled();

        [NotNull]
        byte[] GetKey();

        [NotNull]
        byte[] GetInitializationVector();

        string GetCaptchasFile();

        bool IsCaptchaEnabled();
    }
}
