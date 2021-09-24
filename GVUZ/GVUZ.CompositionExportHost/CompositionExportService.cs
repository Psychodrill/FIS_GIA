using GVUZ.CompositionExportModel;
using GVUZ.CompositionExportModel.Helper;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GVUZ.CompositionExportHost
{
    public class CompositionExportService : ICompositionExportService
    {
        static string CompositionOldDrive = ConfigurationManager.AppSettings["CompositionOldDrive"];
        static string CompositionNewDrive = ConfigurationManager.AppSettings["CompositionNewDrive"];
        static string fileName2015 = Path.Combine(CompositionNewDrive, "compositionInfo2015.txt");
        static string fileName2016 = Path.Combine(CompositionNewDrive, "compositionInfo2016.txt");

        static readonly int GetFileTryCount = 10;
        static readonly int GetFileTryDelay = 2;
        static readonly int GetFileTotalTryCount = 100;

        private const int MessageSize = 10000;

        static CompositionExportService()
        {

        }

        public CompositionExportResult GetCompositions(List<CompositionRequestItem> compositionItems)
        {
            LogHelper.Log.DebugFormat("GetCompositions entered, count: {0}", compositionItems.Count);
            var res = new CompositionExportResult();

            string fileName = string.Format("Результаты_сочинений_{0}.zip", DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
            //const string contentType = "zip";

            bool hasData = false;
            using (var mem = new MemoryStream())
            {
                using (var zip = new ZipOutputStream(mem))
                {
                    zip.IsStreamOwner = false;
                    zip.SetLevel(3);

                    var totalTryNumber = 1;
                    foreach (var item in compositionItems)
                    {
                        LogHelper.Log.DebugFormat("item: {0} {1}", item.ToString(), item.CompositionPaths);

                        if (!string.IsNullOrEmpty(item.CompositionPaths))
                        {
                            var i = 1;
                            foreach (var path in item.CompositionPaths.Split(';'))
                            {
                                var trimmedPath = path.Trim();
                                if (!string.IsNullOrWhiteSpace(CompositionNewDrive))
                                {
                                    trimmedPath = trimmedPath.Replace(CompositionOldDrive, CompositionNewDrive);
                                }
                                if (trimmedPath != "")
                                {
                                    var name = string.Format("{0}_{1}.png", item.ToString(), i++);

                                    byte[] file = null;
                                    int tryNumber = 1;

                                    while (tryNumber <= GetFileTryCount)
                                    {
                                        try
                                        {
                                            file = File.ReadAllBytes(trimmedPath);
                                            LogHelper.Log.DebugFormat("Успешный факт получения бланка сочинения: {0}, {1}", trimmedPath, item.ToString());
                                            break;
                                        }
                                        catch (IOException ioEx)
                                        {
                                            LogHelper.Log.Error(string.Format("Ошибка IOException получения бланка сочинения: {0}, {1}, попытка: {2}, общая попытка: {3}", trimmedPath, item.ToString(), tryNumber, totalTryNumber), ioEx);
                                            tryNumber++;
                                            totalTryNumber++;
                                            if (!(tryNumber >= GetFileTryCount || totalTryNumber >= GetFileTotalTryCount))
                                                Thread.Sleep(1000 * GetFileTryDelay);
                                        }
                                        catch (Exception e)
                                        {
                                            LogHelper.Log.Error(string.Format("Ошибка Exception получения бланка сочинения: {0}, {1}, попытка: {2}, общая попытка: {3}", trimmedPath, item.ToString(), tryNumber, totalTryNumber), e);
                                            tryNumber++;
                                            totalTryNumber++;
                                            if (!(tryNumber >= GetFileTryCount || totalTryNumber >= GetFileTotalTryCount))
                                                Thread.Sleep(1000 * GetFileTryDelay);
                                        }
                                    }

                                    if ((file != null) && (file.Length > 0))
                                    {
                                        var newEntry = new ZipEntry(name);
                                        newEntry.DateTime = DateTime.Now;

                                        zip.PutNextEntry(newEntry);
                                        zip.Write(file, 0, file.Length);
                                        zip.CloseEntry();

                                        hasData = true;
                                    }
                                }
                            }
                        }
                    }

                    zip.Close();
                    mem.Position = 0;

                    res.File = mem.ToArray();
                    res.HasData = hasData;
                }
            }

            LogHelper.Log.DebugFormat("GetCompositions exited, data: {0}, filesize: {0}", res.HasData, res.File.Length);
            return res;
        }

        public bool PrepareCompositionInfos(bool for2015, bool for2016)
        {
            Task.Factory.StartNew(() => SaveCompositionInfo(for2015, for2016));
            return true;
        }

        private void SaveCompositionInfo(bool for2015, bool for2016)
        {
            if (for2015)
            {
                ERBDCompositionInfoList result = new ERBDCompositionInfoList();
                int counter = 0;
                var basePath2015 = Path.Combine(CompositionNewDrive, "blankstorage");


                foreach (string hash2Directory in Directory.GetDirectories(basePath2015))
                {
                    foreach (string hash4Directory in Directory.GetDirectories(hash2Directory))
                    {
                        foreach (string hashDirectory in Directory.GetDirectories(hash4Directory))
                        {
                            foreach (string documentNumberDirectory in Directory.GetDirectories(hashDirectory))
                            {
                                foreach (string participantIdDirectory in Directory.GetDirectories(documentNumberDirectory))
                                {
                                    string participantIdStr = participantIdDirectory.Split('\\').Last();
                                    Guid participantId;
                                    if (!Guid.TryParse(participantIdStr, out participantId))
                                        continue;

                                    int pagesCount = Directory.GetFiles(participantIdDirectory).Count();
                                    if (!result.Items.Any(t => t.Key == participantIdStr))
                                    {
                                        result.Items.Add(new ERBDCompositionInfo(participantIdStr, participantId, (byte)pagesCount));
                                    }

                                    counter++;
                                    if (counter % MessageSize == 0)
                                    {
                                        LogHelper.Log.DebugFormat("Обработано {0} бланков сочинений за 2015 год", counter);
                                    }
                                }
                            }
                        }
                    }
                }
                LogHelper.Log.DebugFormat("Обработано {0} бланков сочинений за 2015 год", counter);
                SaveCompositionInfoFile(fileName2015, result);
                LogHelper.Log.DebugFormat("Данные бланков сочинений за 2015 год сохранены {0}", fileName2015);
            }
            if (for2016)
            {
                ERBDCompositionInfoList result = new ERBDCompositionInfoList();
                int counter = 0;

                List<PagesCountFile> pagesCountFiles = new List<PagesCountFile>();
                var basePath2016 = Path.Combine(CompositionNewDrive, "20");
                foreach (string dateDirectory in Directory.GetDirectories(basePath2016))
                {
                    string pagesCountFilePath = Path.Combine(dateDirectory, "pagescount.txt");
                    if (File.Exists(pagesCountFilePath))
                    {
                        pagesCountFiles.Add(new PagesCountFile(File.ReadAllLines(pagesCountFilePath)));
                    }
                }

                foreach (PagesCountFile pagesCountFile in pagesCountFiles)
                {
                    foreach (string line in pagesCountFile.Content)
                    {
                        ERBDCompositionInfo compositionInfo = new ERBDCompositionInfo(line);
                        if (!compositionInfo.Parsed)
                            continue;

                        string barcode = DataHelper.BytesToString(compositionInfo.Barcode);
                        if (!result.Items.Any(t => t.Key == barcode))
                        {
                            compositionInfo.Key = barcode;
                            result.Items.Add(compositionInfo);
                        }

                        counter++;
                        if (counter % MessageSize == 0)
                        {
                            LogHelper.Log.DebugFormat("Обработано {0} бланков сочинений за 2016+ год", counter);
                        }
                    }
                }

                LogHelper.Log.DebugFormat("Обработано {0} бланков сочинений за 2016+ год", counter);
                SaveCompositionInfoFile(fileName2016, result);
                LogHelper.Log.DebugFormat("Данные бланков сочинений за 2016+ год сохранены {0}", fileName2016);
            }

        }
        
        private void SaveCompositionInfoFile(string fileName, ERBDCompositionInfoList data)
        {
            using (var writer = new System.IO.StreamWriter(fileName))
            {
                var serializer = new XmlSerializer(data.GetType());
                serializer.Serialize(writer, data);
                writer.Flush();
            }
        }

        private ERBDCompositionInfoList LoadCompositionInfoFile (string fileName)
        {
            using (var stream = System.IO.File.OpenRead(fileName))
            {
                var serializer = new XmlSerializer(typeof(ERBDCompositionInfoList));
                return serializer.Deserialize(stream) as ERBDCompositionInfoList;
            }
        }


        public ERBDCompositionInfoList GetAllCompositionInfos(bool for2015, bool for2016)
        {
            ERBDCompositionInfoList result = new ERBDCompositionInfoList();
            if (for2015)
            {
                var items2015 = LoadCompositionInfoFile(fileName2015);
                result.Items.AddRange(items2015.Items);
                result.ActualDate = items2015.ActualDate;
            }
            if (for2016)
            {
                var items2016 = LoadCompositionInfoFile(fileName2016);
                result.Items.AddRange(items2016.Items);
                if (items2016.ActualDate < result.ActualDate)
                    result.ActualDate = items2016.ActualDate;
            }

            return result;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // dispose managed state (managed objects).
                }

                // free unmanaged resources (unmanaged objects) and override a finalizer below.
                // set large fields to null.

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
