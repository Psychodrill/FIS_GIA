namespace Ege.Check.Dal.Store.Mappers
{
    using System.Data.Common;
    using System.Threading.Tasks;
    using Ege.Check.Logic.Models.Staff;
    using Ege.Dal.Common;
    using Ege.Dal.Common.Mappers;

    internal class RcoiInfoMapper : DataReaderMapper<RcoiInfo>
    {
        private const string Fio = "Fio";
        private const string Phone = "Phone";
        private const string Email = "Email";
        private const string HotLinePhone = "HotLineData";
        private const string BlanksServer = "BlanksServer";
        private const string CompositionBlanksServer = "CompositionBlanksServer";
        private const string Description = "Description";

        public override async Task<RcoiInfo> Map(DbDataReader @from)
        {
            var fio = GetOrdinal(from, Fio);
            var phone = GetOrdinal(from, Phone);
            var email = GetOrdinal(from, Email);
            var hotLinePhone = GetOrdinal(from, HotLinePhone);
            var blanksServer = GetOrdinal(from, BlanksServer);
            var compositionBlankServer = GetOrdinal(from, CompositionBlanksServer);
            var description = GetOrdinal(from, Description);

            RcoiInfo result = null;
            if (await from.ReadAsync())
            {
                result = new RcoiInfo
                    {
                        Fio = await from.GetNullableStringAsync(fio),
                        Phone = await from.GetNullableStringAsync(phone),
                        Email = await from.GetNullableStringAsync(email),
                        HotLinePhone = await from.GetNullableStringAsync(hotLinePhone),
                        BlanksServer = await from.GetNullableStringAsync(blanksServer),
                        CompositionBlanksServer = await from.GetNullableStringAsync(compositionBlankServer),
                        Description = await from.GetNullableStringAsync(description),
                    };
            }
            return result;
        }
    }
}