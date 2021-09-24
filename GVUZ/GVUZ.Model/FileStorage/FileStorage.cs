using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace GVUZ.Model.FileStorage
{
    public class FileStorage : IFileStorage
    {
        private readonly DirectoryInfo _baseDir;
        
        public FileStorage()
        {
            const string defaultAppDataFolder = "FIS_FILES";
            _baseDir = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), defaultAppDataFolder));
        }

        public FileStorage(string baseDirectoryPath)
        {
            _baseDir = new DirectoryInfo(baseDirectoryPath);
        }

        private DirectoryInfo GetClientDir(int clientId)
        {
            DirectoryInfo dir = new DirectoryInfo(Path.Combine(_baseDir.FullName, clientId.ToString()));
            if (!dir.Exists)
            {
                dir.Create();
            }

            return dir;
        }

        private string ValidateComments(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
            {
                return null;
            }

            const int maxCommentLength = 1024;

            if (raw.Length > maxCommentLength)
            {
                return raw.Substring(0, maxCommentLength);
            }

            return raw;
        }

        public string Add(int clientId, Stream input, string fileName, string comments)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }

            fileName = Path.GetFileName(fileName);

            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }

            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("invalid file name", "fileName");
            }

            if (fileName.ToLowerInvariant() == ".desc")
            {
                throw new InvalidOperationException("Unacceptable file name");
            }

            string id = DateTime.Now.ToString("yyyyMMdd_HHmmss");

            DirectoryInfo folder = new DirectoryInfo(Path.Combine(GetClientDir(clientId).FullName, id));
            folder.Create();

            const string commentsFileName = ".desc";
            string commentsFilePath = Path.Combine(folder.FullName, commentsFileName);

            comments = ValidateComments(comments);

            if (!string.IsNullOrEmpty(comments))
            {
                File.WriteAllText(commentsFilePath, comments);
            }

            FileInfo contentFile = new FileInfo(Path.Combine(folder.FullName, fileName));

            using (FileStream fs = new FileStream(contentFile.FullName, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            {
                input.CopyTo(fs);
            }

            return id;
        }

        public FileDescription Get(int clientId, string fileId, bool includeComments)
        {
            Regex dirName = new Regex(@"^\d{8}_\d{6}$");

            if (fileId == null)
            {
                throw new ArgumentNullException("fileId");
            }

            if (!dirName.Match(fileId).Success)
            {
                throw new ArgumentException("invalid file id " + fileId);
            }

            DirectoryInfo dir = new DirectoryInfo(Path.Combine(GetClientDir(clientId).FullName, fileId));

            return GetDescriptionFromClientFolder(dir, includeComments);    
        }

        public IEnumerable<FileDescription> GetAll(int clientId, bool includeComments)
        {
            DirectoryInfo dir = GetClientDir(clientId);
            Regex dirName = new Regex(@"^\d{8}_\d{6}$");

            return dir.GetDirectories()
                      .Where(x => dirName.Match(x.Name).Success)
                      .Select(x => GetDescriptionFromClientFolder(x, includeComments));
        }

        public void WriteContentTo(int clientId, string fileId, Stream output)
        {
            Regex dirName = new Regex(@"^\d{8}_\d{6}$");

            if (fileId == null)
            {
                throw new ArgumentNullException("fileId");
            }

            if (!dirName.Match(fileId).Success)
            {
                throw new ArgumentException("invalid file id " + fileId);
            }

            if (output == null)
            {
                throw new ArgumentNullException("output");
            }

            if (!output.CanWrite)
            {
                throw new ArgumentException("stream is not writable", "output");
            }

            DirectoryInfo fileDir = new DirectoryInfo(Path.Combine(GetClientDir(clientId).FullName, fileId));

            FileDescription fd = GetDescriptionFromClientFolder(fileDir, false);

            FileInfo fi = new FileInfo(Path.Combine(fileDir.FullName, fd.FileName));

            if (!fi.Exists)
            {
                throw new InvalidOperationException("File not found");
            }

            using (FileStream fs = fi.OpenRead())
            {
                fs.CopyTo(output);
            }
        }

        private static FileDescription GetDescriptionFromClientFolder(DirectoryInfo fileFolder, bool includeComments)
        {
            FileDescription fd = new FileDescription();
            fd.FileId = fileFolder.Name;
            fd.UploadDate = DateTime.ParseExact(fileFolder.Name, "yyyyMMdd_HHmmss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);

            if (includeComments)
            {
                FileInfo commentsFile = fileFolder.GetFiles(".desc").SingleOrDefault();

                if (commentsFile != null && commentsFile.Length > 0)
                {
                    fd.Comments = File.ReadAllText(commentsFile.FullName);
                }    
            }
            
            FileInfo contentFile = fileFolder.GetFiles("*.*").OrderBy(x => x.CreationTime).First(x => x.Name != "." && x.Name != ".." && x.Name.ToLowerInvariant() != ".desc");

            fd.FileName = contentFile.Name;
            fd.FileSize = contentFile.Length;

            return fd;
        }
    }
}