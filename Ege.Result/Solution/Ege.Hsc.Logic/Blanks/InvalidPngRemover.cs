namespace Ege.Hsc.Logic.Blanks
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Logging;
    using Ege.Dal.Common.Factory;
    using Ege.Hsc.Dal.Store.Repositories;
    using Ege.Hsc.Logic.Models.Blanks;
    using JetBrains.Annotations;

    class InvalidPngRemover : IInvalidPngRemover
    {
        [NotNull]private readonly IBlankApplicationSettings _blankApplicationSettings;
        [NotNull] private readonly IFilePathHelper _filePathHelper;
        [NotNull] private readonly IDbConnectionFactory _connectionFactory;
        [NotNull] private readonly IBlankDownloadRepository _repository;

        [NotNull] private readonly static ILog Logger = LogManager.GetLogger<InvalidPngRemover>();

        public InvalidPngRemover(
            [NotNull]IBlankApplicationSettings blankApplicationSettings, 
            [NotNull]IFilePathHelper filePathHelper, 
            [NotNull]IDbConnectionFactory connectionFactory, 
            [NotNull]IBlankDownloadRepository repository)
        {
            _blankApplicationSettings = blankApplicationSettings;
            _filePathHelper = filePathHelper;
            _connectionFactory = connectionFactory;
            _repository = repository;
        }

        public async Task RemoveInvalidPngsFromStorage(bool remove = true)
        {
            var root = _blankApplicationSettings.BlanksRootPath();
            var files = Directory.EnumerateFiles(root, "*", SearchOption.AllDirectories);
            var invalidBlanks = new ConcurrentBag<DownloadedBlank>();
            Parallel.ForEach(
                files, new ParallelOptions {MaxDegreeOfParallelism = 16}, file =>
                {
                    if (file == null || IsPng(file))
                    {
                        return;
                    }
                    if (remove)
                    {
                        File.Delete(file);
                    }
                    var blank = _filePathHelper.TryParsePath(file);
                    if (blank != null)
                    {
                        invalidBlanks.Add(blank);
                        Logger.InfoFormat("File {0} is not a png: rbdid {1}, hash {2}, document {3}, order {4}",
                            file, blank.RbdId, blank.Hash, blank.DocumentNumber, blank.Order);
                    }
                    else
                    {
                        Logger.InfoFormat("File {0} is not a png and is not linked to any download queue item", file);
                    }
                });
            Logger.InfoFormat("Found {0} invalid downloaded files", invalidBlanks.Count);
            var ids = invalidBlanks.Select(b => b.RbdId).Distinct().ToList();
            Logger.InfoFormat("Found {0} participants with invalid downloaded files", ids.Count);
            Logger.InfoFormat("Participants having invalid files: {0}", 
                string.Join("\n", ids.Select(id => string.Format("'{0}'", id))));
            if (!remove)
            {
                return;
            }
            Logger.InfoFormat("Resetting status in download queue");
            using (var connection = await _connectionFactory.CreateHscAsync())
            {
                if (connection == null)
                {
                    throw new InvalidOperationException("IDbConnectionFactory::CreateHscAsync returned null");
                }
                var reset = await _repository.SetErrorStatus(connection, invalidBlanks);
                Logger.InfoFormat("Reset status for {0} download queue items", reset);
            }
        }

        private bool IsPng([NotNull]string file)
        {
            var buffer = new byte[MagicNumbers.Png.Length];
            using (var stream = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                stream.Read(buffer, 0, MagicNumbers.Png.Length);
            }
            return !buffer.Where((b, i) => b != MagicNumbers.Png[i]).Any();
        }
    }
}

