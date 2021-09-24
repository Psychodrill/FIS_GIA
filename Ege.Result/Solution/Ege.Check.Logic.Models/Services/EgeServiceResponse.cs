namespace Ege.Check.Logic.Models.Services
{
    using System;

    public class EgeServiceResponse
    {
        public EgeServiceResponse(Guid responseId)
        {
            ResponseId = responseId;
        }

        public EgeServiceResponse() : this(Guid.NewGuid())
        {
        }

        public Guid ResponseId { get; set; }
    }
}