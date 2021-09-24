namespace Ege.Check.Captcha
{
    using System.Threading.Tasks;
    using Ege.Check.Logic.Models;
    using JetBrains.Annotations;

    /// <summary>
    ///     Получает капчу из кэша или генерирует её, если кэш недоступен
    /// </summary>
    public interface ICaptchaRetriever
    {
        [NotNull]
        Task<Captcha> Retrieve();
    }
}