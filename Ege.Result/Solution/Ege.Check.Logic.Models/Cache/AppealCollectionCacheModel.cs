namespace Ege.Check.Logic.Models.Cache
{
    using System.Collections.Generic;

    public class AppealCollectionCacheModel
    {
        /// <summary>
        ///     Кэш-модель апелляций
        /// </summary>
        public ICollection<AppealCacheModel> Appeals { get; set; }
    }
}