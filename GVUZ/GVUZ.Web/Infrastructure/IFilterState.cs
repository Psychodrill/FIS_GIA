namespace GVUZ.Web.Infrastructure
{
    public interface IFilterState<TFilter> where TFilter : class 
    {
        TFilter CloneInputFields(TFilter source);
        TFilter CloneInputFields();
    }
}