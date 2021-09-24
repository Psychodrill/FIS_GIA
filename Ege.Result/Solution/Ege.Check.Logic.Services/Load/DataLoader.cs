namespace Ege.Check.Logic.Services.Load
{
    using System;
    using System.Configuration;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Logging;
    using Ege.Check.Dal.Cache.CacheFactory;
    using Ege.Check.Dal.Cache.Interfaces;
    using Ege.Check.Dal.Cache.Interfaces.CacheFactory;
    using Ege.Check.Dal.Cache.LoadServices;
    using Ege.Check.Dal.Cache.LoadServices.DtoCache;
    using Ege.Check.Dal.Store.Bulk;
    using Ege.Check.Dal.Store.Bulk.Load;
    using Ege.Check.Dal.Store.Factory;
    using Ege.Check.Logic.LoadServices.Preprocessing;
    using Ege.Check.Logic.LoadServices.Processing;
    using Ege.Check.Logic.Models.Services;
    using Ege.Check.Logic.Services.Inspectors;
    using Ege.Check.Logic.Services.Participant;
    using Ege.Dal.Common.Bulk;
    using JetBrains.Annotations;
    using LogManager = Common.Logging.LogManager;

    internal class DataLoader<TDto> : IDataLoader<TDto>
    {
        [NotNull] private readonly ICacheWriter<TDto> _cacheWriter;
        [NotNull] private readonly IBulkLoader _bulkLoader;
        [NotNull] private readonly ICacheFactory _cacheFactory;
        [NotNull] private readonly IDataMerger _dataMerger;
        [NotNull] private readonly IDatatableCollector<TDto> _datatableCollector;
        [NotNull] private readonly IDecompressor _decompressor;
        [NotNull] private readonly IDeserializer _deserializer;
        [NotNull] private readonly IEgeTempTableOperator _egeTempTableOperator;
        [NotNull] private readonly ILog _log;
        [NotNull] private readonly IConnectionFactory<SqlConnection> _sqlConnectionFactory;
        [NotNull] private readonly IRequestLogWriter _requestLogWriter;
        [NotNull] private readonly IDtoCache<TDto> _cache;
        [NotNull]private readonly IMemoryCacheService _memoryCacheService;

        public bool FlushCacheAfterFinalize { get; set; }
        public bool IsTruncateDto { get; set; }


        public DataLoader(
            [NotNull] IDecompressor decompressor,
            [NotNull] IDeserializer deserializer,
            [NotNull] IDatatableCollector<TDto> datatableCollector,
            [NotNull] IBulkLoader bulkLoader,
            [NotNull] IEgeTempTableOperator egeTempTableOperator,
            [NotNull] IConnectionFactory<SqlConnection> sqlConnectionFactory,
            [NotNull] IDataMerger dataMerger,
            [NotNull] ICacheWriter<TDto> cacheWriter,
            [NotNull] ICacheFactory cacheFactory, 
            [NotNull] IRequestLogWriter requestLogWriter, 
            [NotNull] IDtoCache<TDto> cache, 
            [NotNull] IMemoryCacheService memoryCacheService)
        {
            _decompressor = decompressor;
            _deserializer = deserializer;
            _datatableCollector = datatableCollector;
            _bulkLoader = bulkLoader;
            _egeTempTableOperator = egeTempTableOperator;
            _sqlConnectionFactory = sqlConnectionFactory;
            _dataMerger = dataMerger;
            _cacheWriter = cacheWriter;
            _cacheFactory = cacheFactory;
            _requestLogWriter = requestLogWriter;
            _cache = cache;
            _memoryCacheService = memoryCacheService;
            _log = LogManager.GetLogger(GetType());

            FlushCacheAfterFinalize = ConfigurationManager.AppSettings["LoadServices.Settings.LastBatch"]
                                      == typeof(TDto).Name;

            IsTruncateDto = ConfigurationManager.AppSettings["IsTruncateDto"] == "1";
        }
        
        private async Task LoadDataWrapped([NotNull]EgeServiceRequest request, [NotNull]EgeServiceResponse response)
        {
            if (request.Data == null)
            {
                _log.TraceFormat("Received null instead of data");
                return;
            }
            _log.TraceFormat("Start load data for response {0}. {1} bytes received", response.ResponseId,
                             request.Data.Length);
            using (var decompressedStream = await _decompressor.DecompressAsync(request.Data))
            using (var dataTable = _datatableCollector.Create())
            {
                await _requestLogWriter.Log(decompressedStream as MemoryStream, typeof(TDto), response.ResponseId);
                _log.TraceFormat("{0} bytes decompressed for response {1}", decompressedStream.Length,
                                 response.ResponseId);
                var dtos = _deserializer.Deserialize<TDto>(decompressedStream) ?? new TDto[0];
                _log.TraceFormat("{0} DTO entities deserialized for response {1}", dtos.Length, response.ResponseId);
                foreach (var dto in dtos)
                {
                    _datatableCollector.AddRow(dto, dataTable);
                }
                _log.TraceFormat("Filled data table for response {0}", response.ResponseId);
                using (var connection = await _sqlConnectionFactory.CreateAsync())
                {
                    using (var transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                    {
                        _log.TraceFormat("Start bulk load for response {0}", response.ResponseId);
                        await _bulkLoader.LoadDataAsync(dataTable, dataTable.TableName, connection, transaction);
                        transaction.Commit();
                        _log.TraceFormat("End bulk load for response {0}", response.ResponseId);
                    }
                    await PutDtoPackageIntoCache(dtos, response);
                    _log.TraceFormat("Response {0} is ready", response.ResponseId);
                }
            }
        }

        [NotNull]
        private async Task PutDtoPackageIntoCache(TDto[] dtos, [NotNull]EgeServiceResponse response)
        {
            _log.TraceFormat("Putting DTOs into cache for response {0}", response.ResponseId);
            var cacheConnection = _cacheFactory.GetCache();
            if (cacheConnection != null)
            {
                _log.TraceFormat("Created cache connection for response {0}", response.ResponseId);
                try
                {
                    var lockedCount = await _cache.LockGetCount(cacheConnection);
                    _log.TraceFormat("Current count of {0} packages in cache: {1} (response {2})", typeof(TDto).Name, lockedCount.Key, response.ResponseId);
                    await _cache.PutAsync(cacheConnection, lockedCount.Key, dtos);
                    var newCount = lockedCount.Key + 1;
                    _log.TraceFormat("Put dto package into cache for response {0}", response.ResponseId);
                    _cache.UnlockPutCount(cacheConnection, newCount, lockedCount.Value);
                    _log.TraceFormat("Put the new count of {0} packages in cache: {1} (response {2})", typeof(TDto).Name, newCount, response.ResponseId);
                }
                catch (CacheException ex)
                {
                    _log.TraceFormat("Cache failed on response {0}: {1}", response.ResponseId, ex);
                }
            }
        }

        public async Task<EgeServiceResponse> LoadData(EgeServiceRequest request)
        {
            var response = new EgeServiceResponse();
            try
            {
                await LoadDataWrapped(request, response);
            }
            catch (Exception e)
            {
                _log.ErrorFormat("Error has occurred in LoadData. ResponseId: {0}. Exception: {1}", response.ResponseId, e);
                throw;
            }
            return response;
        }

        private async Task MergePackageIntoCache([NotNull] EgeServiceResponse response, [NotNull] ICacheWrapper cacheConnection, int index)
        {
            _log.TraceFormat("Retrieving package #{0} of {1} from cache (response {2})", index, typeof(TDto).Name, response.ResponseId);
            var cachedDtos = await _cache.GetAsync(cacheConnection, index);
            _log.TraceFormat("Retrieved package #{0} of {1} from cache (response {2})", index, typeof(TDto).Name, response.ResponseId);
            if (cachedDtos == null)
            {
                _log.TraceFormat("Null cached as package #{0} of {1} (response {2})", index, typeof(TDto).Name, response.ResponseId);
            }
            else
            {
                if (cachedDtos.Length > 0)
                {
                    await _cacheWriter.Write(cacheConnection, cachedDtos);
                }
                _log.TraceFormat("Processed package #{0} of {1} (response {2})", index, typeof(TDto).Name, response.ResponseId);
                await _cache.PutAsync(cacheConnection, index, null);
                _log.TraceFormat("Deleted package #{0} of {1} from cache (response {2})", index, typeof(TDto).Name, response.ResponseId);
            }
        }
        
        private async void WriteToCache([NotNull] EgeServiceResponse response)
        {
            var cacheConnection = _cacheFactory.GetCache();
            if (cacheConnection != null)
            {
                _log.TraceFormat("Created cache connection for response {0}", response.ResponseId);
                _log.TraceFormat("Start cache merging for response {0}", response.ResponseId);
                try
                {
                    var count = _cache.GetCount(cacheConnection);
                    if (!cacheConnection.SupportsBulkOperations)
                    {
                        var tasks = Enumerable.Range(0, count).Select(i => MergePackageIntoCache(response, cacheConnection, i));
                        await Task.WhenAll(tasks);
                    }
                    else
                    {
                        for (int i = 0; i < count; ++i)
                        {
                            await MergePackageIntoCache(response, cacheConnection, i);
                        }
                    }
                    _cache.PutCount(cacheConnection, 0);
                }
                catch (CacheException ex)
                {
                    _log.TraceFormat("Cache failed on response {0}: {1}", response.ResponseId, ex);
                }
                _log.TraceFormat("End cache merging for response {0}", response.ResponseId);
            }
            else
            {
                _log.TraceFormat("Can't merge cache: cache failed for response {0}", response.ResponseId);
            }
        }

        private async Task FlushCache([NotNull] EgeServiceResponse response)
        {
            var cache = _cacheFactory.GetCache();
            if (cache != null)
            {
                _log.TraceFormat("Clearing cache for response {0}", response.ResponseId);
                await cache.Clear();
                _log.TraceFormat("Cleared cache for response {0}", response.ResponseId);
            }
            else
            {
                _log.TraceFormat("Cache connection could not be established for response {0}", response.ResponseId);
            }
        }

        private async Task FinalizeLoadDataWrapped(EgeServiceResponse response)
        {
            using (var connection = await _sqlConnectionFactory.CreateAsync())
            using (var transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                _log.TraceFormat("Start merge data for response {0}", response.ResponseId);
                var dtoTotalCount = await _dataMerger.MergeData<TDto>(connection, transaction);
                if (IsTruncateDto) await TruncateDtoTable(response, connection, transaction);
                transaction.Commit();
                _log.TraceFormat("End merge data for response {0}: merged {1} entities", response.ResponseId, dtoTotalCount);
            }
            if (FlushCacheAfterFinalize)
            {
                await FlushCache(response);
            }
        }

        public async Task<EgeServiceResponse> FinalizeLoadData()
        {
            var response = new EgeServiceResponse();
            try
            {
                await _memoryCacheService.RefreshMemoryCache(
                    refreshAnswerCriteria: false,
                    refreshRegionSettings: false,
                    refreshSubjectsAndExams: true,
                    refreshAvailableRegions: false,
                    refreshCancelledExams: false);
                await FinalizeLoadDataWrapped(response);
            }
            catch (Exception e)
            {
                _log.ErrorFormat("Error has occurred in FinalizeLoadData. ResponseId: {0}. Exception: {1}", response.ResponseId, e);
                // пачки плохие - очищаем кэш
                var cacheConnection = _cacheFactory.GetCache();
                _cache.PutCount(cacheConnection, 0); 
                throw;
            }
            return response;
        }

        private async Task TruncateDtoTable(EgeServiceResponse response, DbConnection connection, DbTransaction transaction)
        {
            var cmd = connection.CreateCommand();
            cmd.CommandText = string.Format("truncate table [{0}]", typeof(TDto).Name);
            cmd.Transaction = transaction;
            cmd.CommandTimeout = 30000;
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<EgeServiceResponse> StartLoadData()
        {
            var response = new EgeServiceResponse();
            try
            {
                using (var connection = await _sqlConnectionFactory.CreateAsync())
                {
                    _log.TraceFormat("Deleting DTOs {0}", response.ResponseId);
                    await TruncateDtoTable(response, connection, null);
                    _log.TraceFormat("Deleted DTOs from database {0}", response.ResponseId);
                    var cacheConnection = _cacheFactory.GetCache();
                    if (cacheConnection != null)
                    {
                        _log.TraceFormat("Deleting DTOs from cache {0}", response.ResponseId);
                        var count = _cache.GetCount(cacheConnection);
                        for (var i = 0; i < count; ++i)
                        {
                            await _cache.PutAsync(cacheConnection, i, null);
                            _log.TraceFormat("Deleted DTO #{0} from cache {1}", i, response.ResponseId);
                        }
                        _cache.PutCount(cacheConnection, 0);
                        _log.TraceFormat("Deleted DTOs from cache {0}", response.ResponseId);
                    }
                }
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Error has occurred in StartLoadData. ResponseId: {0}. Exception: {1}", response.ResponseId, ex);
                throw;
            }
            return response;
        }
    }
}
