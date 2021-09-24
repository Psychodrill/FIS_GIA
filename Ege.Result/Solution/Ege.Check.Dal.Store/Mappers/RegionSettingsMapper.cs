namespace Ege.Check.Dal.Store.Mappers
{
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Threading.Tasks;
    using Ege.Check.Logic.Models.Cache;
    using Ege.Dal.Common;
    using Ege.Dal.Common.Mappers;

    internal class RegionSettingsMapper : DataReaderMapper<IDictionary<int, RegionSettingsCacheModel>>
    {
        private const string RegionId = "RegionId";
        private const string ExamId = "ExamGlobalId";
        private const string ShowBlanks = "ShowBlanks";
        private const string ShowResult = "ShowResult";
        private const string GekNumber = "Number";
        private const string GekDate = "CreateDate";
        private const string HotlinePhone = "HotLineData";
        private const string HotLineData = "Description";
        private const string CommonServer = "BlanksServer";
        private const string CompositionServer = "CompositionBlanksServer";
        private const string RowType = "RowType";
        private const string GekUrl = "Url";

        public override async Task<IDictionary<int, RegionSettingsCacheModel>> Map(DbDataReader @from)
        {
            var result = new Dictionary<int, RegionSettingsCacheModel>();

            var regionIdOrdinal = GetOrdinal(from, RegionId);
            var examId = GetOrdinal(from, ExamId);
            var showBlanks = GetOrdinal(from, ShowBlanks);
            var showResult = GetOrdinal(from, ShowResult);
            var gekNumber = GetOrdinal(from, GekNumber);
            var gekDate = GetOrdinal(from, GekDate);
            var hotlineData = GetOrdinal(from, HotLineData);
            var hotlinePhone = GetOrdinal(from, HotlinePhone);
            var commonServer = GetOrdinal(from, CommonServer);
            var compositionServer = GetOrdinal(from, CompositionServer);
            var gekUrl = GetOrdinal(from, GekUrl);
            var rowType = GetOrdinal(from, RowType);

            int? lastRegion = null;
            RegionSettingsCacheModel current = null;
            while (await from.ReadAsync())
            {
                var currentRegion = from.GetInt32(regionIdOrdinal);
                if (currentRegion != lastRegion)
                {
                    current = new RegionSettingsCacheModel
                        {
                            Info = new RegionInfoCacheModel(),
                            Settings = new Dictionary<int, RegionExamSettingCacheModel>(),
                            Servers = new BlanksServerCacheModel(),
                        };
                    result.Add(currentRegion, current);
                    lastRegion = currentRegion;
                }
                if (from.GetBoolean(rowType))
                {
                    current.Info.Info = await @from.GetNullableStringAsync(hotlineData);
                    current.Info.HotlinePhone = await @from.GetNullableStringAsync(hotlinePhone);

                    current.Servers.Common = await @from.GetNullableStringAsync(commonServer);
                    current.Servers.Composition = await @from.GetNullableStringAsync(compositionServer);
                }
                else
                {
                    current.Settings.Add(@from.GetInt32(examId), new RegionExamSettingCacheModel
                        {
                            ShowBlanks = @from.GetBoolean(showBlanks),
                            ShowResult = @from.GetBoolean(showResult),
                            GekDocument = new GekDocumentCacheModel
                                {
                                    GekNumber = await @from.GetNullableStringAsync(gekNumber),
                                    GekDate = await @from.GetNullableDateTimeAsync(gekDate),
                                    Url = await @from.GetNullableStringAsync(gekUrl),
                                },
                        });
                }
            }

            return result;
        }
    }
}