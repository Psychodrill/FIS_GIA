namespace Ege.Hsc.Logic.Blanks
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Logging;
    using Ege.Check.Common;
    using Ege.Check.Common.Extensions;
    using Ege.Check.Dal.Blanks;
    using Ege.Check.Dal.Store.Factory;
    using Ege.Hsc.Dal.Entities;
    using Ege.Hsc.Dal.Store.Repositories;
    using Ege.Hsc.Logic.Models.Blanks;
    using JetBrains.Annotations;

    internal class BlankService : IBlankService
    {
        [NotNull] private readonly IBlankApplicationSettings _blankApplicationSettings;
        [NotNull] private readonly IBlankDownloadRepository _blankDownloadRepository;
        [NotNull] private readonly IMapper<BlankToDownload, Blank> _blankMapper;
        [NotNull] private readonly IConnectionFactory<SqlConnection> _connectionFactory;
        [NotNull] private readonly IBlankModelCreator _blankModelCreator;
        [NotNull] private readonly IParticipantRepository _participantRepository;

        [NotNull] private static readonly ILog Logger = LogManager.GetLogger<BlankService>();

        public BlankService(
            [NotNull] IBlankDownloadRepository blankDownloadRepository,
            [NotNull] IBlankApplicationSettings blankApplicationSettings,
            [NotNull] IConnectionFactory<SqlConnection> connectionFactory,
            [NotNull] IMapper<BlankToDownload, Blank> blankMapper, 
            [NotNull] IBlankModelCreator blankModelCreator, 
            [NotNull] IParticipantRepository participantRepository)
        {
            _blankDownloadRepository = blankDownloadRepository;
            _blankApplicationSettings = blankApplicationSettings;
            _connectionFactory = connectionFactory;
            _blankMapper = blankMapper;
            _blankModelCreator = blankModelCreator;
            _participantRepository = participantRepository;
        }

        public async Task<ICollection<Blank>> BlanksToDownload()
        {
            IList<BlankToDownload> blanks;
            using (var connection = await _connectionFactory.CreateHscAsync())
            {
                if (connection == null)
                {
                    throw new InvalidOperationException("IDbConnectionFactory::CreateHscAsync returned null");
                }
                blanks = await _blankDownloadRepository.TopNotDownloadFromServerAsync(connection, _blankApplicationSettings.BatchBlankDownload());
            }
            if (blanks == null)
            {
                throw new InvalidOperationException("IBlankDownloadRepository::TopNotDownloadFromServerAsync returned null");
            }
            var result = new Blank[blanks.Count];
            for (var i = 0; i < blanks.Count; i++)
            {
                result[i] = _blankMapper.Map(blanks[i]);
            }
            return result;
        }

        private async Task LoadParticipantsFromCheckEgeDb([NotNull]DbConnection connection, DbTransaction transaction)
        {
            var participantCount = await _participantRepository.LoadFromCheckEgeDb(connection, transaction);
            Logger.TraceFormat("Loaded {0} participants", participantCount);
        }

        [NotNull]
        private IEnumerable<BlankDownload> GetBlankDownloadModels([NotNull]NameCoderBlank blankcoder, [NotNull]IEnumerable<LoadedBlank> blanks)
        {
            return blanks.Where(b => b != null).SelectMany(b =>
                        _blankModelCreator.Create(b.BlankData, blankcoder).Where(bm => bm != null).Select((bm, order) => new BlankDownload
                        {
                            ParticipantId = b.ParticipantId,
                            Order = order + 1,
                            RelativePath = bm.Url,
                            State = BlankDownloadState.Queued,
                            RegionId = b.RegionId,
                            ExamDate = b.BlankData.ExamDate,
                            SubjectCode = b.BlankData.SubjectCode,
                            Code = bm.Code,
                        }));
        }

        public async Task LoadBlanksFromCheckEgeDb()
        {
            using (var connection = await _connectionFactory.CreateHscAsync())
            using (var transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
            {
                await LoadParticipantsFromCheckEgeDb(connection, transaction);

                var loaded = await _blankDownloadRepository.LoadFromCheckEgeDb(connection, transaction);
                if (loaded == null)
                {
                    throw new InvalidOperationException("IBlankDownloadRepository::LoadFromCheckEgeDb returned null");
                }
                using (var blankcoder = new NameCoderBlank())
                {
                    var blanksToQueue = GetBlankDownloadModels(blankcoder, loaded.Blanks);
                    await _blankDownloadRepository.AddAsync(connection, transaction, blanksToQueue);
                }
                transaction.Commit();
            }
        }

        public async Task FixInconsistenciesWithCheckEgeDb()
        {
            using (var connection = await _connectionFactory.CreateHscAsync())
            {
                if (connection == null)
                {
                    throw new InvalidOperationException("IDbConnectionFactory::CreateHscAsync returned null");
                }
                Logger.InfoFormat("Fixing inconsistencies in page count");
                var loaded = await _blankDownloadRepository.GetInconsistenciesWithCheckEgeDb(connection);
                if (loaded == null)
                {
                    throw new InvalidOperationException("IBlankDownloadRepository::GetInconsistenciesWithCheckEgeDb returned null");
                }
                Logger.InfoFormat("Got {0} blanks with inconsistent data", loaded.Blanks.Count);
                int fixedOverall = 0;
                using (var blankcoder = new NameCoderBlank())
                {
                    int batchId = 0;
                    foreach (var batch in loaded.Blanks.ArrayBatch(10000))
                    {
                        if (batch == null || batch.Length == 0)
                        {
                            continue;
                        }
                        Logger.InfoFormat("Fixing batch #{0}", batchId++);
                        var blanksToQueue = GetBlankDownloadModels(blankcoder, batch);
                        var fixedQueueItems = await _blankDownloadRepository.FixInconsistenciesWithCheckEgeDb(connection, blanksToQueue);
                        Logger.InfoFormat("Fixed {0} inconsistent blanks in download queue", fixedQueueItems);
                        fixedOverall += fixedQueueItems;
                    }
                }
                Logger.InfoFormat("Finished fixing inconsistent blanks: fixed {0} items in download queue", fixedOverall);
            }
        }
    }
}
