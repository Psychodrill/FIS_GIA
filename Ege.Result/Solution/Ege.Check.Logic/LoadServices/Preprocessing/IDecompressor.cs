namespace Ege.Check.Logic.LoadServices.Preprocessing
{
    using System.IO;
    using System.Threading.Tasks;
    using JetBrains.Annotations;

    /// <summary>
    ///     Отвечает за разархивацию данных
    /// </summary>
    public interface IDecompressor
    {
        /// <summary>
        ///     Разархивирует данные
        /// </summary>
        /// <param name="source">Сжатые данные в виде массива байт</param>
        /// <returns>Задача, возвращающая поток разархивированных данных</returns>
        [NotNull]
        Task<Stream> DecompressAsync([NotNull] byte[] source);
    }
}