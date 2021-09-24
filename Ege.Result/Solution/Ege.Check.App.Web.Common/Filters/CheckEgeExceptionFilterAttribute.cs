namespace Ege.Check.App.Web.Common.Filters
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Http.ExceptionHandling;
    using System.Web.Http.Filters;
    using JetBrains.Annotations;
    using global::Common.Logging;

    public class CheckEgeExceptionFilterAttribute : ExceptionFilterAttribute
    {
        [NotNull] private static readonly ILog Logger = LogManager.GetLogger<CheckEgeExceptionFilterAttribute>();

        private void OnException(Exception ex)
        {
            Logger.Error(ex);
        }

        public override void OnException([NotNull] HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception is TaskCanceledException || actionExecutedContext.Exception is OperationCanceledException)
            {
                // TaskCanceledExceptions are thrown on web api if a user agent resets connection - we don't want to log these
                return;
            }
            OnException(actionExecutedContext.Exception);
        }

        public void OnException([NotNull] ExceptionContext filterContext)
        {
            OnException(filterContext.Exception);
        }
    }
}