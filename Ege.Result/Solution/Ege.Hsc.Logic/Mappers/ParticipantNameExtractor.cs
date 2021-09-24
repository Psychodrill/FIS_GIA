namespace Ege.Hsc.Logic.Mappers
{
    using System;
    using Ege.Check.Common;
    using Ege.Check.Logic.Models.Requests;

    class ParticipantNameExtractor : IMapper<ParticipantBlankRequest, string>
    {
        public string Map(ParticipantBlankRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            return string.Format("{0} {1} {2}", request.Surname, request.FirstName, request.Patronymic);
        }
    }
}
