using System.Web.Mvc;
using GVUZ.Web.Infrastructure;

namespace GVUZ.Web.Filters
{
    public class TransactionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.IsChildAction)
            {
                return;
            }

            SqlTransactionManager manager = TransactionManager.Current as SqlTransactionManager;

            if (manager != null && manager.Activated)
            {
                try
                {
                    if (filterContext.Exception == null || filterContext.ExceptionHandled)
                    {
                        manager.Commit();
                    }
                    else
                    {
                        manager.Rollback();
                    }

                }
                finally
                {
                    manager.DisposeConnection();
                }
            }
            
            base.OnActionExecuted(filterContext);
        }
    }
}