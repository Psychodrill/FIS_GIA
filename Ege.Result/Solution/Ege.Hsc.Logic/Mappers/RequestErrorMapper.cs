namespace Ege.Hsc.Logic.Mappers
{
    using System;
    using System.Collections.Generic;
    using Ege.Check.Common;
    using Ege.Hsc.Dal.Entities;
    using Ege.Hsc.Logic.Models.Requests;
    using JetBrains.Annotations;

    class RequestErrorMapper : IMapper<BlankRequest, ParticipantErrorCollectionExcelModel>
    {
        public ParticipantErrorCollectionExcelModel Map(BlankRequest @from)
        {
            if (from == null)
            {
                return null;
            }
            var result = new ParticipantErrorCollectionExcelModel
            {
                Errors = new List<ParticipantErrorExcelModel>(),
            };
            foreach (var participant in from.Participants)
            {
                if (participant == null)
                {
                    throw new ArgumentException("BlankRequest.Participants contains null");
                }
                if (participant.RbdId.HasValue)
                {
                    if (participant.IsCollision)
                    {
                        AddError(result.Errors, participant, SingleParticipantRequestStatus.Collision);
                    }
                    if (participant.HasErrors)
                    {
                        AddError(result.Errors, participant, SingleParticipantRequestStatus.HasErrors);
                    }
                    if (participant.HasNoServerUrl)
                    {
                        AddError(result.Errors, participant, SingleParticipantRequestStatus.NoServerUrl);
                    }
                    if (participant.HasNoBlanks)
                    {
                        AddError(result.Errors, participant, SingleParticipantRequestStatus.NotYetDownloaded);
                    }
                }
                else
                {
                    AddError(result.Errors, participant, SingleParticipantRequestStatus.NotFound);
                }
            }
            return result;
        }

        private void AddError(
            [NotNull] ICollection<ParticipantErrorExcelModel> errors,
            [NotNull] RequestedParticipant participant,
            SingleParticipantRequestStatus error)
        {
            errors.Add(new ParticipantErrorExcelModel
            {
                ErrorStatus = error,
                Name = participant.Name,
                Document = participant.DocumentNumber,
                Region = participant.Region,
            });
        }
    }
}
