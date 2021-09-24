namespace Ege.Check.App.Web.Auth
{
    using System.Security.Principal;
    using Ege.Check.Logic.Models.Cache;
    using JetBrains.Annotations;

    internal class ParticipantPrincipal : IParticipantPrincipal
    {
        public ParticipantPrincipal([NotNull] ParticipantCacheModel participant)
        {
            Identity = new GenericIdentity("name");
            Participant = participant;
        }

        public ParticipantCacheModel Participant { get; set; }

        public IIdentity Identity { get; private set; }

        public bool IsInRole(string role)
        {
            return false;
        }
    }
}