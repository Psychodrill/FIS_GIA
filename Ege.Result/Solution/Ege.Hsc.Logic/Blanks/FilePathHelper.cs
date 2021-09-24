namespace Ege.Hsc.Logic.Blanks
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Text.RegularExpressions;
    using Common.Logging;
    using Ege.Hsc.Logic.Models.Blanks;
    using JetBrains.Annotations;

    internal class FilePathHelper : IFilePathHelper
    {
        [NotNull] private readonly IBlankApplicationSettings _blankApplicationSettings;
        [NotNull] private readonly Regex _fileNameRegex = new Regex(
            @"([0-9a-fA-F]*)\\([0-9]*)\\([0-9a-fA-F-]*)\\(\d*)$", RegexOptions.Compiled | RegexOptions.RightToLeft);

        [NotNull] private static readonly ILog Logger = LogManager.GetLogger<FilePathHelper>();

        public FilePathHelper([NotNull] IBlankApplicationSettings blankApplicationSettings)
        {
            _blankApplicationSettings = blankApplicationSettings;
        }

        public string GetOrCreatePath(string hash, string documentNumber, Guid rbdId)
        {
            if (hash.Length < 4)
            {
                throw new ArgumentException("Invalid length of hash");
            }
            var root = _blankApplicationSettings.BlanksRootPath();
            var folder = Path.Combine(root, hash.Substring(0, 2), hash.Substring(0, 4), hash, documentNumber, rbdId.ToString());
            if (folder == null)
            {
                throw new InvalidOperationException("Path.Combine returned null");
            }
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            return folder;
        }

        public string GetOrCreatePath(string hash, string documentNumber, Guid rbdId, int order)
        {
            var folder = GetOrCreatePath(hash, documentNumber, rbdId);
            var res = Path.Combine(folder, order.ToString(CultureInfo.InvariantCulture));
            return res;
        }

        public string GetZipPath(Guid requestId)
        {
            var directory = _blankApplicationSettings.ZipRootPath();
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            var result = Path.Combine(directory, requestId.ToString());
            return result;
        }

        public DownloadedBlank TryParsePath(string path)
        {
            const int hashGroup = 1;
            const int numberGroup = 2;
            const int rbdGroup = 3;
            const int orderGroup = 4;

            var match = _fileNameRegex.Match(path);
            if (!match.Success)
            {
                Logger.InfoFormat("{0} does not match the blank file name pattern", path);
                return null;
            }
            Guid rbdId;
            int order;
            if (!Guid.TryParse(match.Groups[rbdGroup].Value, out rbdId))
            {
                Logger.InfoFormat("{0} has invalid rbdId {1}", path, match.Groups[rbdGroup].Value);
                return null;
            }
            if (!int.TryParse(match.Groups[orderGroup].Value, out order))
            {
                Logger.InfoFormat("{0} has invalid order {1}", path, match.Groups[orderGroup].Value);
                return null;
            }
            return new DownloadedBlank
            {
                DocumentNumber = match.Groups[numberGroup].Value,
                RbdId = rbdId,
                Order = order,
                Hash = match.Groups[hashGroup].Value,
            };
        }
    }
}
