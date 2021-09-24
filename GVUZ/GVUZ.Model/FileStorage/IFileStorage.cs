using System.Collections.Generic;
using System.IO;

namespace GVUZ.Model.FileStorage
{
    public interface IFileStorage
    {
        string Add(int clientId, Stream input, string fileName, string comments);
        FileDescription Get(int clientId, string fileId, bool includeComments);
        IEnumerable<FileDescription> GetAll(int clientId, bool includeComments);
        void WriteContentTo(int clientId, string fileId, Stream output);
    }
}