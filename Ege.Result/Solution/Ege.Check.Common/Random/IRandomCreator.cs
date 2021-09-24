namespace Ege.Check.Common.Random
{
    using System;
    using JetBrains.Annotations;

    /// <summary>
    ///     Инкапсулирует создание рандома, чтобы при большом количестве паралельных запросов не было коллизий
    /// </summary>
    public interface IRandomCreator
    {
        /// <summary>
        ///     Создать рандом
        /// </summary>
        /// <returns>Рандом</returns>
        [NotNull]
        Random Create();
    }
}