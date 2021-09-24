namespace Ege.Check.App.Web.Common.Auth
{
    using System;
    using System.Security.Principal;
    using Ege.Check.Logic.Models.Staff;
    using JetBrains.Annotations;

    public class StaffPrincipal : IStaffPrincipal
    {
        public StaffPrincipal([NotNull] UserModel user)
        {
            Identity = new GenericIdentity(user.Login ?? "(null)");
            User = user;
        }

        public bool IsInRole(string role)
        {
            return User.IsEnabled && User.Role.ToString().Equals(role, StringComparison.OrdinalIgnoreCase);
        }

        public IIdentity Identity { get; private set; }
        public UserModel User { get; set; }
    }
}
