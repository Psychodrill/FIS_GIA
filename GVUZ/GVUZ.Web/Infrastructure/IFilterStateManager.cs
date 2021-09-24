using GVUZ.Web.ViewModels.ApplicationsList;

namespace GVUZ.Web.Infrastructure
{
    public interface IFilterStateManager
    {
        void Update<TFilter>(TFilter instance) where TFilter : class, IFilterState<TFilter>, new();
        void Remove<TFilter>();
        TFilter GetOrCreate<TFilter>() where TFilter : class, IFilterState<TFilter>, new();
        void RemoveAll();
    }
}