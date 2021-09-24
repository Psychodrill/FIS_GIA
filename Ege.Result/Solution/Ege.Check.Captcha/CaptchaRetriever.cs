namespace Ege.Check.Captcha
{
    using System;
    using System.Threading.Tasks;
    using Ege.Check.Common.Random;
    using Ege.Check.Logic.Models;
    using JetBrains.Annotations;
    using global::Common.Logging;

    internal class CaptchaRetriever : ICaptchaRetriever
    {
        [NotNull] private static readonly ILog Logger = LogManager.GetLogger<CaptchaRetriever>();

        [ThreadStatic] private static Random Random;
        [NotNull] private readonly ICaptchaGenerator _generator;
        [NotNull] private readonly IRandomCreator _randomCreator;
        [NotNull] private readonly ICaptchaService _service;
        [NotNull] private readonly ICaptchaTokenHelper _tokenHelper;

        public CaptchaRetriever(
            [NotNull] ICaptchaGenerator generator,
            [NotNull] IRandomCreator randomCreator,
            [NotNull] ICaptchaTokenHelper tokenHelper,
            [NotNull] ICaptchaService service)
        {
            _generator = generator;
            _randomCreator = randomCreator;
            _tokenHelper = tokenHelper;
            _service = service;
        }

        public async Task<Captcha> Retrieve()
        {
            var currentCount = _service.CurrentCachedCount();
            if (currentCount <= 0)
            {
                Logger.Trace("Cache is down or contains no captchas, generating a new one");
                return GetByImage(_generator.GenerateOne());
            }
            Random = Random ?? _randomCreator.Create();
            var captchaId = Random.Next(currentCount);
            Logger.TraceFormat("Retrieving captcha #{0} from cache", captchaId);
            var result = _service.Retrieve(captchaId);
            return GetByImage(await result);
        }

        private Captcha GetByImage([NotNull] CachedCaptcha image)
        {
            return new Captcha(image.Image, _tokenHelper.Generate(image.Number));
        }
    }
}