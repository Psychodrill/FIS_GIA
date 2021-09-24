namespace Ege.Check.Captcha
{
    using System.Threading.Tasks;
    using Ege.Check.Logic.Models;
    using JetBrains.Annotations;

    public interface ICaptchaService
    {
        bool Generatable { get; }

        void TryGenerateBatch();

        int CurrentCachedCount();

        [NotNull]
        Task<CachedCaptcha> Retrieve(int id);
    }
}