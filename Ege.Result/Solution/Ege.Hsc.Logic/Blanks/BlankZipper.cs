namespace Ege.Hsc.Logic.Blanks
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Ege.Hsc.Dal.Entities;
    using JetBrains.Annotations;

    internal class BlankZipper : IBlankZipper
    {
        [NotNull] private readonly IFilePathHelper _filePathHelper;
        [NotNull] private readonly IErrorFileCreator _errorFileCreator;

        private const string ErrorFileName = "errors.xlsx";

        public BlankZipper(
            [NotNull] IFilePathHelper filePathHelper,
            [NotNull] IErrorFileCreator errorFileCreator)
        {
            _filePathHelper = filePathHelper;
            _errorFileCreator = errorFileCreator;
        }

        public async Task Zip(BlankRequest request)
        {
            var zipPath = _filePathHelper.GetZipPath(request.Id);
            using (var zipStream = File.Open(zipPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                await Zip(request, zipStream);
            }
        }

        public async Task Zip([NotNull]BlankRequest request, [NotNull]Stream outputStream)
        {
            using (var zipArchive = new ZipArchive(outputStream, ZipArchiveMode.Create, false, Encoding.GetEncoding(866)))
            {
                var entryNames = new HashSet<string>();
                foreach (var participant in request.Participants)
                {
                    if (participant == null)
                    {
                        continue;
                    }
                    await ZipParticipantBlanks(zipArchive, participant, entryNames);
                }

                using (var errorStream = _errorFileCreator.CreateErrorFile(request))
                {
                    if (errorStream != null)
                    {
                        using (var entryStream = zipArchive.CreateEntry(ErrorFileName).Open())
                        {
                            await errorStream.CopyToAsync(entryStream);
                        }
                    }
                }
            }
        }

        private async Task ZipParticipantBlanks([NotNull] ZipArchive archive, [NotNull] RequestedParticipant participant,
            [NotNull] ISet<string> entryNames)
        {
            if (participant.RbdId == null || participant.Hash == null || participant.DocumentNumber == null)
            {
                return; // Участник не найден
            }
            var participantFolder = _filePathHelper.GetOrCreatePath(
                participant.Hash, participant.DocumentNumber, participant.RbdId.Value);
            foreach (var file in Directory.EnumerateFiles(participantFolder))
            {
                var fileName = Path.GetFileName(file);
                var entry = archive.CreateEntry(GetEntryName(participant, fileName, entryNames));
                using (var entryStream = entry.Open())
                using (var fileStream = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    await fileStream.CopyToAsync(entryStream);
                }
            }
        }

        [NotNull]
        private string GetEntryName([NotNull] RequestedParticipant participant, string fileName,
            [NotNull] ISet<string> entryNames)
        {
            var result = !participant.IsCollision
                ? string.Format("{0} {1} ({2}).png", participant.Name, participant.DocumentNumber, fileName)
                : string.Format(
                    "{0} {1} {2} ({3}).png", participant.Name, participant.DocumentNumber, participant.Region, fileName);
            if (!entryNames.Add(result))
            {
                int i = 0;
                while (!entryNames.Add(result))
                {
                    result = string.Format("{2} {0} ({1})", participant.RbdId, ++i, result);
                }
            }
            return result;
        }

        [NotNull]
        private readonly Regex _entryNameRegex = new Regex(@"^((?:\w+\s?)*)\s\w+\s\(");

        private string GetFioFilename([NotNull] string entryName)
        {
            var match = _entryNameRegex.Match(entryName);
            if (!match.Success)
            {
                return null;
            }
            var nameParts = match.Groups[1].Value.Split(' ');
            if (nameParts.Length == 0)
            {
                return null;
            }
            return nameParts.Length > 1
                ? string.Format(
                    "{0}_{1}.zip", nameParts[0],
                    new string(nameParts.Skip(1).Select(np => np != null && np.Length >= 1 ? np[0] : '_').ToArray()))
                : string.Format("{0}.zip", nameParts[0]);
        }

        public string GetFioFilename(Stream zipStream)
        {
            const string defaultResult = "none.zip";
            string result;
            using (var zipArchive = new ZipArchive(zipStream, ZipArchiveMode.Read, true, Encoding.GetEncoding(866)))
            {
                if (zipArchive.Entries == null)
                {
                    throw new InvalidOperationException("zipArchive.Entries is null");
                }
                var fioEntry = zipArchive.Entries.FirstOrDefault(e => e != null && e.Name != null && !e.Name.Equals(ErrorFileName, StringComparison.OrdinalIgnoreCase));
                if (fioEntry == null || fioEntry.Name == null)
                {
                    return defaultResult;
                }
                result = GetFioFilename(fioEntry.Name) ?? defaultResult;
            }
            zipStream.Position = 0;
            return result;
        }
    }
}
