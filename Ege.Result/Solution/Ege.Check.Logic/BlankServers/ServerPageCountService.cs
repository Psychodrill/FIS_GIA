namespace Ege.Check.Logic.BlankServers
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;
    using Ege.Check.Common.Config;
    using Ege.Check.Common.Extensions;
    using Ege.Check.Dal.Cache.CacheFactory;
    using Ege.Check.Dal.Cache.LoadServices;
    using Ege.Check.Dal.MemoryCache.Regions;
    using Ege.Check.Dal.MemoryCache.Subjects;
    using Ege.Check.Dal.Store.Factory;
    using Ege.Check.Dal.Store.Repositories.Blanks;
    using Ege.Check.Dal.Store.Repositories.PagesCount;
    using Ege.Check.Logic.Models.Cache;
    using Ege.Check.Logic.Models.Servers;
    using global::Common.Logging;
    using JetBrains.Annotations;

    class ServerPageCountService : IServerPageCountService
    {
        [NotNull]private readonly IRegionSettingsMemoryCache _regionSettingsMemoryCache;
        [NotNull] private readonly ISubjectExamMemoryCache _examMemoryCache;
        [NotNull] private readonly IBlankInfoRepository _repository;
        [NotNull] private readonly IPageCountRetriever _retriever;
        [NotNull]private readonly IConnectionFactory<SqlConnection> _connectionFactory;
        [NotNull] private readonly ICacheFactory _cacheFactory;
        [NotNull] private readonly IBlankInfoCacheUpdater _cacheUpdater;
        [NotNull] private readonly IConfigReaderHelper _configReaderHelper;
        [NotNull]private readonly IPagesCountRepository _pagesCountRepository;

        [NotNull] private static readonly ILog Logger = LogManager.GetLogger<ServerPageCountService>();

        public ServerPageCountService(
            [NotNull]IRegionSettingsMemoryCache regionSettingsMemoryCache,
            [NotNull]ISubjectExamMemoryCache examMemoryCache, 
            [NotNull]IBlankInfoRepository repository, 
            [NotNull]IPageCountRetriever retriever, 
            [NotNull]IConnectionFactory<SqlConnection> connectionFactory, 
            [NotNull]ICacheFactory cacheFactory, 
            [NotNull]IBlankInfoCacheUpdater cacheUpdater, 
            [NotNull]IConfigReaderHelper configReaderHelper, 
            [NotNull]IPagesCountRepository pagesCountRepository)
        {
            _regionSettingsMemoryCache = regionSettingsMemoryCache;
            _examMemoryCache = examMemoryCache;
            _repository = repository;
            _retriever = retriever;
            _connectionFactory = connectionFactory;
            _cacheFactory = cacheFactory;
            _cacheUpdater = cacheUpdater;
            _configReaderHelper = configReaderHelper;
            _pagesCountRepository = pagesCountRepository;
        }

        private async Task ProcessSingleRegionExam(int regionId, [NotNull]string url, DateTime date, int subjectCode)
        {
            Logger.InfoFormat("Retrieving page count data for region {0} (url {1}), exam {2}", regionId, url, date.Date);
            var data = await _retriever.GetPageCountData(url, date, subjectCode);
            if (data == null || !data.Any())
            {
                Logger.InfoFormat("No page count data found");
                return;
            }
            Logger.InfoFormat("Retrieved page count data for region {0} (url {1}), exam {2}", regionId, url, date.Date);
            ICollection<UpdatedBlankInfo> updatedBlanks;
            using (var connection = await _connectionFactory.CreateAsync())
            {
                if (connection == null)
                {
                    throw new InvalidOperationException("IDbConnectionFactory::CreateAsync returned null");
                }
                Logger.InfoFormat("Updating stored in database page count data for region {0} (url {1}), exam {2}", regionId, url, date.Date);
                updatedBlanks = await _repository.UpdatePageCount(connection, regionId, data);
                Logger.InfoFormat("Updated stored in database page count data for region {0} (url {1}), exam {2}", regionId, url, date.Date);
            }
            var cache = _cacheFactory.GetCache();
            if (cache != null)
            {
                Logger.InfoFormat("Updating cached page count data for region {0} (url {1}), exam {2}", regionId, url, date.Date);
                foreach (var batch in updatedBlanks.ArrayBatch(_configReaderHelper.GetInt("CacheBatchSize", string.Empty, 100000)))
                {
                    await _cacheUpdater.UpdatePageCount(cache, batch);
                }
                Logger.InfoFormat("Updated cached page count data for region {0} (url {1}), exam {2}", regionId, url, date.Date);
            }
        }

        public async Task LoadPageCount()
        {
            var regions = _regionSettingsMemoryCache.GetAll();//.Where(x=>x.Key == 8);
            var exams = _examMemoryCache.GetAllExams().Where(e => e != null && e.Subject.IsComposition);//.Where(x=>x.Id == 331 || x.Id == 332).ToList();
            foreach (var region in regions)
            {
                if (region.Value == null || string.IsNullOrWhiteSpace(region.Value.Servers.Composition))
                {
                    Logger.InfoFormat("Region {0} does not have composition server", region.Key);
                    continue;
                }
                foreach (var exam in exams)
                {
                    if (exam == null)
                    {
                        continue;
                    }
                    try
                    {
                        await ProcessSingleRegionExam(region.Key, region.Value.Servers.Composition, exam.Date, exam.SubjectCode);
                    }
                    catch (Exception ex)
                    {
                        Logger.ErrorFormat("Exception while updating region {0}, exam {1}, subject {2}. Message: {3}", new object[] {region.Key, exam.Date, exam.SubjectCode, ex});
                    }
                }
            }
        }
        
        public async Task LoadPageCountIntoSpecialTable()
        {
            Logger.InfoFormat("Loading pages count data into special table");
            var regions = _regionSettingsMemoryCache.GetAll();
            var exams = _examMemoryCache.GetAllExams().Where(e => e != null && e.Subject.SubjectCode == 20).ToList();
            var tasks = new List<KeyValuePair<KeyValuePair<int, ExamMemoryCacheModel>, Task<ICollection<PageCountData>>>>();
            foreach (var region in regions)
            {
                if (region.Value == null || string.IsNullOrWhiteSpace(region.Value.Servers.Composition))
                {
                    Logger.InfoFormat("Region {0} does not have composition server", region.Key);
                    continue;
                }
                foreach (var exam in exams)
                {
                    if (exam == null)
                    {
                        continue;
                    }
                    try
                    {
                        var task = _retriever.GetPageCountData(region.Value.Servers.Composition, exam.Date, exam.SubjectCode);
                        tasks.Add(new KeyValuePair<KeyValuePair<int, ExamMemoryCacheModel>, Task<ICollection<PageCountData>>>(
                            new KeyValuePair<int, ExamMemoryCacheModel>(region.Key, exam), 
                            task));
                    }
                    catch (Exception ex)
                    {
                        Logger.ErrorFormat("Exception while loading data from region {0}, exam {1}, subject {2}. Message: {3}", 
                            new object[] { region.Key, exam.Date, exam.SubjectCode, ex });
                    }
                }
            }
            await Task.WhenAll(tasks.Select(t => t.Value));
            using (var connection = await _connectionFactory.CreateHscAsync())
            {
                await _pagesCountRepository.Merge(connection, tasks.Select(t => 
                    new KeyValuePair<KeyValuePair<int, ExamMemoryCacheModel>, ICollection<PageCountData>>(
                        t.Key, t.Value.Result)));
            }
            Logger.InfoFormat("Loaded pages count data into special table");
        }
    }
}