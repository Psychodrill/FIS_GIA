namespace Ege.Hsc.Dal.Store.Mappers.Requests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Ege.Dal.Common.Mappers;
    using Ege.Hsc.Dal.Entities;

    class RequestedParticipantTableMapper : IDataTableMapper<IEnumerable<RequestedParticipant>>
    {
        private const string HashColumn = "Hash";
        private const string DocumentColumn = "DocumentNumber";
        private const string NameColumn = "Name";

        public DataTable Map(IEnumerable<RequestedParticipant> @from)
        {
            var result = new DataTable();
            if (result.Rows == null)
            {
                throw new InvalidOperationException("DataTable.Rows is null");
            }
            result.Columns.Add(HashColumn, typeof(string));
            result.Columns.Add(DocumentColumn, typeof(string));
            result.Columns.Add(NameColumn, typeof(string));

            foreach (var participant in from ?? Enumerable.Empty<RequestedParticipant>())
            {
                if (participant == null)
                {
                    continue;
                }
                result.Rows.Add(participant.Hash, participant.DocumentNumber, participant.Name);
            }
            return result;
        }
    }
}
