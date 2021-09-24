using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace FBS.Replicator.Helpers
{
    public class NetworkIO
    {
        private string _user;
        private string _password;
        public NetworkIO(string user, string password)
        {
            _user = user;
            _password = password;
        }

        public IEnumerable<string> EnumerateDirectories(string path)
        {
            if (String.IsNullOrEmpty(_user))
                return Directory.EnumerateDirectories(path);
            else
            {
                using (new NetworkConnection(path, new NetworkCredential(_user, _password)))
                {
                    return Directory.EnumerateDirectories(path);
                }
            }
        }

        public IEnumerable<string> EnumerateFiles(string path)
        {
            if (String.IsNullOrEmpty(_user))
                return Directory.EnumerateFiles(path);
            else
            {
                using (new NetworkConnection(path, new NetworkCredential(_user, _password)))
                {
                    return Directory.EnumerateFiles(path);
                }
            }
        }

        public bool FileExists(string path)
        {
            if (String.IsNullOrEmpty(_user))
                return File.Exists(path);
            else
            {
                string directory = Path.GetDirectoryName(path);
                using (new NetworkConnection(directory, new NetworkCredential(_user, _password)))
                {
                    return File.Exists(path);
                }
            }
        }

        public string[] ReadTestLinesFile(string path)
        {
            if (String.IsNullOrEmpty(_user))
                return File.ReadAllLines(path);
            else
            {
                string directory = Path.GetDirectoryName(path);
                using (new NetworkConnection(directory, new NetworkCredential(_user, _password)))
                {
                    return File.ReadAllLines(path);
                }
            }
        }

        public string ReadTextFile(string path)
        {
            if (String.IsNullOrEmpty(_user))
                return File.ReadAllText (path);
            else
            {
                string directory = Path.GetDirectoryName(path);
                using (new NetworkConnection(directory, new NetworkCredential(_user, _password)))
                {
                    return File.ReadAllText(path);
                }
            }
        }
    }
}
