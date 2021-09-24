using System;

namespace GVUZ.Model.FileStorage
{
    public class FileDescription
    {
        public string FileId { get; set; }
        public DateTime UploadDate { get; set; }
        public long FileSize { get; set; }
        public string FileName { get; set; }
        public string Comments { get; set; }
    }
}