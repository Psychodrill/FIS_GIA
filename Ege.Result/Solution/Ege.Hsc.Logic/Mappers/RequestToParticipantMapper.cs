namespace Ege.Hsc.Logic.Mappers
{
    using System;
    using Ege.Check.Common;
    using Ege.Check.Common.Hash;
    using Ege.Check.Logic.Models.Requests;
    using Ege.Hsc.Dal.Entities;
    using JetBrains.Annotations;

    class RequestToParticipantMapper : IMapper<ParticipantBlankRequest, RequestedParticipant>
    {
        [NotNull]private readonly IFioHasher _fioHasher;
        [NotNull]private readonly IMapper<ParticipantBlankRequest, string> _nameExtractor; 

        public RequestToParticipantMapper([NotNull]IFioHasher fioHasher, [NotNull]IMapper<ParticipantBlankRequest, string> nameExtractor)
        {
            _fioHasher = fioHasher;
            _nameExtractor = nameExtractor;
        }

        public RequestedParticipant Map(ParticipantBlankRequest @from)
        {
            if (from == null)
            {
                throw new ArgumentNullException("from");
            }
            return new RequestedParticipant
            {
                Hash = _fioHasher.GetHash(from.Surname, from.FirstName, from.Patronymic),
                DocumentNumber = from.Document,
                Name = _nameExtractor.Map(from),
            };
        }
    }
}
