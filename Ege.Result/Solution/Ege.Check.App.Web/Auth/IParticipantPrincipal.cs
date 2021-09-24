namespace Ege.Check.App.Web.Auth
{
    using System.Security.Principal;
    using Ege.Check.Logic.Models.Cache;
    using JetBrains.Annotations;

    public interface IParticipantPrincipal : IPrincipal
    {
        [NotNull]
        ParticipantCacheModel Participant { get; set; }
    }
}