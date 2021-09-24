namespace Ege.Check.Logic.Services.Load
{
    using System.Threading.Tasks;
    using Ege.Check.Logic.Models.Services;
    using JetBrains.Annotations;

    public interface IDataLoader<TDto>
    {
        [NotNull]
        Task<EgeServiceResponse> LoadData([NotNull] EgeServiceRequest request);

        [NotNull]
        Task<EgeServiceResponse> FinalizeLoadData();

        [NotNull]
        Task<EgeServiceResponse> StartLoadData();
    }
}
