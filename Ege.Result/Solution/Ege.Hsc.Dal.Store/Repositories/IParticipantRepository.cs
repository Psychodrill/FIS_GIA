namespace Ege.Hsc.Dal.Store.Repositories
{
    using System.Data.Common;
    using System.Threading.Tasks;
    using JetBrains.Annotations;

    public interface IParticipantRepository
    {
        /// <summary>
        /// Загрузить участников сочинения и изложения из основной базы
        /// </summary>
        /// <returns>Количество загруженных участников</returns>
        [NotNull]
        Task<int> LoadFromCheckEgeDb([NotNull]DbConnection connection, DbTransaction transaction = null);
    }
}
