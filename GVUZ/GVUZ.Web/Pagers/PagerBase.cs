using System;
using System.Data.Objects.DataClasses;
using System.Linq;
using FogSoft.Helpers;
using GVUZ.Web.Helpers;

namespace GVUZ.Web.Pagers
{
    public abstract class PagerBase<T, TModel> where T : EntityObject where TModel : class, IOrderable, IPageable
    {
        public const int NameSortOrder = 2;
        protected static readonly int PageSize = AppSettings.Get("Search.PageSize", 25);

        protected abstract IQueryable<T> GetOrderedBy(IQueryable<T> queryable, int? sortId);
        protected abstract void Load(IQueryable<T> queryable, TModel model);

        public TModel Fill(IQueryable<T> queryable, TModel model)
        {
            // set total page count before skipping
            model.TotalPageCount = GetCount(queryable);

            queryable = GetOrderedBy(queryable, model.SortID);

            if (UseSkipping())
                queryable = GetSkippedBy(queryable, model.PageNumber);

            Load(queryable, model);

            return model;
        }

        private static IQueryable<T> GetSkippedBy(IQueryable<T> queryable, int? pageNumber)
        {
            if (!pageNumber.HasValue || pageNumber < 0)
                pageNumber = 0;

            return queryable.Skip(pageNumber.Value*PageSize).Take(PageSize);
        }

        protected virtual bool UseSkipping()
        {
            return true;
        }

        protected virtual int GetCount(IQueryable<T> queryable)
        {
            return (Math.Max(queryable.Count(), 1) - 1)/PageSize + 1;
        }
    }
}