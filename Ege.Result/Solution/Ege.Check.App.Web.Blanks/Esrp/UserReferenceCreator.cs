namespace Ege.Check.App.Web.Blanks.Esrp
{
    using System.Security.Principal;
    using Ege.Check.App.Web.Common.Auth;
    using Ege.Hsc.Logic.Models;

    class UserReferenceCreator : IUserReferenceCreator
    {
        public UserReference Create(IPrincipal principal, bool allowEsrp)
        {
            if (principal == null)
            {
                return null;
            }
            var fctOperator = principal as IStaffPrincipal;
            if (!allowEsrp && fctOperator == null)
            {
                return null;
            }
            return new UserReference
            {
                UserId = fctOperator != null ? fctOperator.User.Id : (int?)null,
                EsrpLogin = fctOperator != null ? null : principal.Identity.Name,
            };
        }
    }
}