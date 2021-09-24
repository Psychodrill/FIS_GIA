namespace Ege.Check.Dal.Cache.LoadServices.Helpers
{
    using System;
    using System.Collections.Generic;
    using Ege.Check.Dal.Cache.Interfaces;
    using JetBrains.Annotations;

    interface IBulkProcessor
    {
        /// <summary>
        /// Обработать коллекцию элементов кэша
        /// </summary>
        /// <typeparam name="TKey">Ключ кэша</typeparam>
        /// <typeparam name="TCached">Кэшированный тип</typeparam>
        /// <typeparam name="TDtoPack">Тип множества dto, меняющего один элемент кэша</typeparam>
        /// <param name="lockedElements">Обрабатываемые элементы кэша</param>
        /// <param name="dtos">Dto, сгруппированные по элементам кэша, порядок групп обязан совпадать с порядком в lockedElements</param>
        /// <param name="processAction">Метод обработки</param>
        void Process<TKey, TCached, TDtoPack>(
            [NotNull]CacheBulkLock<TKey, TCached> lockedElements,
            [NotNull]IEnumerable<TDtoPack> dtos,
            [NotNull]Action<TCached, TDtoPack> processAction) 
            where TCached : class;
    }
}
