using GVUZ.Web.Helpers;

namespace GVUZ.Web.Infrastructure
{
    public static class FilterStateManager
    {
        public static readonly IFilterStateManager Current = new CookieFilterStateManager();
    }
}