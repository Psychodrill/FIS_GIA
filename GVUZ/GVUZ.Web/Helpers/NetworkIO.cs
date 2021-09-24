using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Web.Mvc;

namespace GVUZ.Web.Helpers
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
                using (new NetworkConnection(path, new NetworkCredential(_user, _password)))
                    return Directory.EnumerateDirectories(path);
        }

        public IEnumerable<string> EnumerateFiles(string path)
        {
            if (String.IsNullOrEmpty(_user))
                return Directory.EnumerateFiles(path);
            else
                using (new NetworkConnection(path, new NetworkCredential(_user, _password)))
                    return Directory.EnumerateFiles(path);
        }

        public string ReadTextFile(string path)
        {
            if (String.IsNullOrEmpty(_user))
                return File.ReadAllText(path);
            else
            {
                string directory = Path.GetDirectoryName(path);
                using (new NetworkConnection(directory, new NetworkCredential(_user, _password)))
                    return File.ReadAllText(path);
            }
        }

        public byte[] ReadFile(string path)
        {
            if (String.IsNullOrEmpty(_user))
                return File.ReadAllBytes(path);
            else
            {
                string directory = Path.GetDirectoryName(path);
                using (new NetworkConnection(directory, new NetworkCredential(_user, _password)))
                    return File.ReadAllBytes(path);
            }
        }

        public FileContentResult ReadImageFile(string path)
        {
            if (String.IsNullOrEmpty(_user))
                return new FileContentResult(File.ReadAllBytes(path), "image/png");
            else
            {
                string directory = Path.GetDirectoryName(path);
                using (new NetworkConnection(directory, new NetworkCredential(_user, _password)))
                    return new FileContentResult(File.ReadAllBytes(path), "image/png");
            }
        }

        public string ReadImageBytes(string path)
        {
            if (String.IsNullOrEmpty(_user))
                return string.Format("data:image/png;base64,{0}", Convert.ToBase64String(File.ReadAllBytes(path)));
            else
            {
                string directory = Path.GetDirectoryName(path);
                using (new NetworkConnection(directory, new NetworkCredential(_user, _password)))
                    return string.Format("data:image/png;base64,{0}", Convert.ToBase64String(File.ReadAllBytes(path)));
            }
        }
    }



    public class NetworkConnection : IDisposable
    {
        string _networkName;

        public NetworkConnection(string networkName, NetworkCredential credentials)
        {
            _networkName = networkName;

            var netResource = new NetResource()
            {
                Scope = ResourceScope.GlobalNetwork,
                ResourceType = ResourceType.Disk,
                DisplayType = ResourceDisplaytype.Share,
                RemoteName = networkName
            };

            var userName = string.IsNullOrEmpty(credentials.Domain)
                ? credentials.UserName
                : string.Format(@"{0}\{1}", credentials.Domain, credentials.UserName);

            var result = WNetAddConnection2(
                netResource,
                credentials.Password,
                userName,
                0);

            if (result != 0)
                throw new Win32Exception(result, String.Format("Ошибка доступа к сетевому ресурсу {0} (пользователь: {1}; код ошибки: {2})", _networkName, userName, result));
        }

        ~NetworkConnection()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            WNetCancelConnection2(_networkName, 0, true);
        }

        [DllImport("mpr.dll")]
        private static extern int WNetAddConnection2(NetResource netResource,
            string password, string username, int flags);

        [DllImport("mpr.dll")]
        private static extern int WNetCancelConnection2(string name, int flags,
            bool force);
    }

    [StructLayout(LayoutKind.Sequential)]
    public class NetResource
    {
        public ResourceScope Scope;
        public ResourceType ResourceType;
        public ResourceDisplaytype DisplayType;
        public int Usage;
        public string LocalName;
        public string RemoteName;
        public string Comment;
        public string Provider;
    }

    public enum ResourceScope : int
    {
        Connected = 1,
        GlobalNetwork,
        Remembered,
        Recent,
        Context
    };

    public enum ResourceType : int
    {
        Any = 0,
        Disk = 1,
        Print = 2,
        Reserved = 8,
    }

    public enum ResourceDisplaytype : int
    {
        Generic = 0x0,
        Domain = 0x01,
        Server = 0x02,
        Share = 0x03,
        File = 0x04,
        Group = 0x05,
        Network = 0x06,
        Root = 0x07,
        Shareadmin = 0x08,
        Directory = 0x09,
        Tree = 0x0a,
        Ndscontainer = 0x0b
    }

}
