namespace Ege.Hsc.Dal.Store.Mappers.Blanks
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Threading.Tasks;
    using Ege.Dal.Common;
    using Ege.Dal.Common.Mappers;
    using Ege.Hsc.Dal.Entities;

    class BlankRequestMapper : DataReaderCollectionMapper<BlankRequest>
    {
        private const string Id = "Id";
        private const string Name = "ParticipantName";
        private const string IsCollision = "IsCollision";
        private const string Hash = "ParticipantHash";
        private const string RbdId = "ParticipantRbdId";
        private const string DocumentNumber = "DocumentNumber";
        private const string RegionId = "RegionId";
        private const string HasErrors = "HasErrors";
        private const string RegionName = "RegionName";
        private const string HasNoServerUrl = "HasNoServerUrl";

        public override async Task<ICollection<BlankRequest>> Map(DbDataReader @from)
        {
            var idOrdinal = GetOrdinal(from, Id);
            var nameOrdinal = GetOrdinal(from, Name);
            var collisionOrdinal = GetOrdinal(from, IsCollision);
            var hashOrdinal = GetOrdinal(from, Hash);
            var rbdOrdinal = GetOrdinal(from, RbdId);
            var docOrdinal = GetOrdinal(from, DocumentNumber);
            var regionOrdinal = GetOrdinal(from, RegionId);
            var hasErrorsOrdinal = GetOrdinal(from, HasErrors);
            var regionNameOrdinal = GetOrdinal(from, RegionName);
            var noServerUrlOrdinal = GetOrdinal(from, HasNoServerUrl);
            var hasNoBlanksOrdinal = GetOrdinal(from, "HasNoBlanks");

            var result = new List<BlankRequest>();

            Guid? lastId = null;
            BlankRequest current = null;
            while (await from.ReadAsync())
            {
                var currentId = from.GetGuid(idOrdinal);
                if (currentId != lastId)
                {
                    current = new BlankRequest {Id = currentId, Participants = new List<RequestedParticipant>()};
                    result.Add(current);
                }
                current.Participants.Add(new RequestedParticipant
                    {
                        IsCollision = from.GetBoolean(collisionOrdinal),
                        Name = from.GetString(nameOrdinal),

                        HasErrors = from.GetBoolean(hasErrorsOrdinal),

                        // здесь приходят нуллы для отсутствующих в БД пользователей
                        RbdId = await from.GetNullableGuidAsync(rbdOrdinal),
                        RegionId = await from.GetNullableInt32Async(regionOrdinal),
                        Region = await from.GetNullableStringAsync(regionNameOrdinal),
                        DocumentNumber = await from.GetNullableStringAsync(docOrdinal),
                        Hash = await from.GetNullableStringAsync(hashOrdinal),

                        HasNoServerUrl = from.GetBoolean(noServerUrlOrdinal),
                        HasNoBlanks = from.GetBoolean(hasNoBlanksOrdinal),
                    });
                lastId = currentId;
            }
            return result;
        }
    }
}
