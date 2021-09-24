namespace Ege.Check.Logic.BlankServers
{
    using System.Threading.Tasks;
    using JetBrains.Annotations;

    public interface IServerPageCountService
    {
        [NotNull]
        Task LoadPageCount();

        [NotNull]
        Task LoadPageCountIntoSpecialTable();
    }
}
