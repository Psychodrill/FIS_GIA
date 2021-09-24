namespace Ege.Hsc.Logic.Mappers
{
    using System;
    using Ege.Check.Common;
    using Ege.Hsc.Dal.Entities;
    using Ege.Hsc.Logic.Models.Blanks;

    public class BlankToDownloadEntityToBlankMapper : IMapper<BlankToDownload, Blank>
    {
        /// <exception cref="NullReferenceException"></exception>
        public Blank Map(BlankToDownload @from)
        {
            if (@from == null)
            {
                return null;
            }
            if (string.IsNullOrWhiteSpace(from.ServerUrl) || string.IsNullOrWhiteSpace(from.RelativePath))
            {
                throw new NullReferenceException(string.Format("Can not create blank url. Server: {0}, Url: {1}, Blank id: {2}", from.ServerUrl,
                                                               from.RelativePath, from.Id));
            }
            return new Blank
                {
                    Id = @from.Id,
                    Url = new Uri(new Uri(from.ServerUrl), from.RelativePath).ToString(),
                    ParticipantHash = from.ParticipantHash,
                    ParticipantDocumentNumber = from.ParticipantDocumentNumber,
                    ParticipantRbdId = from.ParticipantRbdId,
                    Order = from.Order
                };
        }
    }
}