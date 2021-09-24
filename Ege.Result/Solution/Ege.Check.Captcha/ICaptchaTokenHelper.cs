namespace Ege.Check.Captcha
{
    using JetBrains.Annotations;

    public interface ICaptchaTokenHelper
    {
        [NotNull]
        string Generate([NotNull] string text);

        bool IsCorrect(string text, string token);

        bool CheckCaptcha { get; }
    }
}