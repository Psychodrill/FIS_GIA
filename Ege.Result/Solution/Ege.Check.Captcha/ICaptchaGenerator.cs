namespace Ege.Check.Captcha
{
    using Ege.Check.Logic.Models;
    using JetBrains.Annotations;

    public interface ICaptchaGenerator
    {
        int TextLength { get; }

        [NotNull]
        CachedCaptcha GenerateOne();
    }
}