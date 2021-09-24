namespace Ege.Check.Logic.LoadServices.Preprocessing
{
    using System.IO;
    using System.Threading.Tasks;
    using JetBrains.Annotations;

    /// <summary>
    ///     Отвечает за сжатие данных
    /// </summary>
    public interface ICompressor
    {
        /// <summary>
        ///     Сжимает поток
        /// </summary>
        /// <param name="source">Исходный поток не сжатых данных</param>
        /// <returns>Задача, возвращающая сжатый массив байт</returns>
        [NotNull]
        Task<byte[]> CompressAsync([NotNull] Stream source);
    }
}