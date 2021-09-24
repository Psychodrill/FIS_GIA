namespace Ege.Check.App.Web.Blanks.Esrp
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http.Controllers;
    using System.Web.Mvc;
    using System.Web.Security;
    using Ege.Check.App.Web.Blanks.CheckAuth;
    using Ege.Check.App.Web.Blanks.Ninject;
    using Ege.Check.App.Web.Common.Auth;
    using Ege.Dal.Common.Factory;
    using Ege.Hsc.Dal.Entities;
    using Ege.Hsc.Dal.Store.Repositories;
    using global::Ninject;
    using JetBrains.Annotations;

    public class EsrpAuthorizeAttribute : System.Web.Mvc.AuthorizeAttribute, System.Web.Http.Filters.IAuthorizationFilter
    {
        [NotNull]private readonly IDbConnectionFactory _connectionFactory;
        [NotNull]private readonly IUserRepository _repository;
        
        public bool AllowEsrp { get; set; }

        public EsrpAuthorizeAttribute()
        {
            _connectionFactory = BlanksNinjectDependencyResolver.Kernel.Get<IDbConnectionFactory>();
            _repository = BlanksNinjectDependencyResolver.Kernel.Get<IUserRepository>();

            AllowEsrp = true;
        }
        
        // mvc
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }
            if (filterContext.RequestContext == null || filterContext.RequestContext.HttpContext == null || filterContext.RequestContext.HttpContext.User == null
                || !filterContext.RequestContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new HttpUnauthorizedResult();
                return;
            }
            var principal = filterContext.RequestContext.HttpContext.User;
            if (!(principal is IStaffPrincipal))
            {
                if (!AllowEsrp)
                {
                    filterContext.Result = new HttpUnauthorizedResult();
                    return;
                }
                var login = principal.Identity.Name;
                User user;
                using (var connection = _connectionFactory.CreateHscSync())
                {
                    user = _repository.GetByLoginSync(connection, login);
                }
                if (user == null || user.Ticket == null)
                {
                    FormsAuthentication.SignOut();
                    filterContext.Result = new HttpUnauthorizedResult();
                    return;
                }
                using (var client = new CheckAuthSoapClient("CheckAuthSoap"))
                {
                    var checkResult = client.CheckUserTicket(user.Login, user.Ticket.Value, BlanksSystemId.IntSystemId);
                    if (checkResult == null || checkResult.StatusID <= 0)
                    {
                        FormsAuthentication.SignOut();
                        filterContext.Result = new HttpUnauthorizedResult();
                    }
                }
            }
        }

        // web api
        public async Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException("actionContext");
            }
            if (continuation == null)
            {
                throw new ArgumentNullException("continuation");
            }
            if (actionContext.RequestContext == null || actionContext.RequestContext.Principal == null || !actionContext.RequestContext.Principal.Identity.IsAuthenticated)
            {
                return actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            var principal = actionContext.RequestContext.Principal;
            if (!(principal is IStaffPrincipal))
            {
                if (!AllowEsrp)
                {
                    return actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                }
                var login = principal.Identity.Name;
                User user;
                using (var connection = await _connectionFactory.CreateHscAsync())
                {
                    if (connection == null)
                    {
                        throw new InvalidOperationException("IDbConnectionFactory::CreateHscAsync returned null");
                    }
                    user = await _repository.GetByLoginAsync(connection, login);
                }
                if (user == null || user.Ticket == null)
                {
                    FormsAuthentication.SignOut();
                    return actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                }
                using (var client = new CheckAuthSoapClient("CheckAuthSoap"))
                {
                    var checkResult = await client.CheckUserTicketAsync(user.Login, user.Ticket.Value, BlanksSystemId.IntSystemId);
                    if (checkResult == null || checkResult.StatusID <= 0)
                    {
                        FormsAuthentication.SignOut();
                        return actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                    }
                }
            }
            return await continuation();
        }

        public new bool AllowMultiple
        {
            get { return false; }
        }
    }
}
