namespace Ege.Dal.Common.Helpers
{
    using JetBrains.Annotations;

    /// <summary>
    ///     Получает имя процедуры для мерджа
    /// </summary>
    public interface IProcedureNameGetter
    {
        /// <summary>
        ///     Получить имя процедуры для мерджа
        /// </summary>
        /// <typeparam name="TDto">Тип DTO-модели</typeparam>
        /// <returns>Имя процедуры</returns>
        [NotNull]
        string GetName<TDto>();
    }
}