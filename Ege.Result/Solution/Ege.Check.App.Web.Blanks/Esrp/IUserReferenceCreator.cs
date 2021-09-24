namespace Ege.Check.App.Web.Blanks.Esrp
{
    using System.Security.Principal;
    using Ege.Hsc.Logic.Models;

    public interface IUserReferenceCreator
    {
        UserReference Create(IPrincipal principal, bool allowEsrp);
    }
}
