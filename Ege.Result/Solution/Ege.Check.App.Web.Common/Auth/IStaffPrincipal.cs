namespace Ege.Check.App.Web.Common.Auth
{
    using System.Security.Principal;
    using Ege.Check.Logic.Models.Staff;
    using JetBrains.Annotations;

    public interface IStaffPrincipal : IPrincipal
    {
        [NotNull]
        UserModel User { get; set; }
    }
}