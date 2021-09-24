using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.SqlClient;
using FBS.Replicator.DB.FBS;
using FBS.Replicator.Entities;
using FBS.Replicator.Entities.ERBD;
using FBS.Replicator.Entities.FBS;
using System.Text.RegularExpressions;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using FBS.Replicator.Helpers;
using FBS.Common;
using System.Xml.Serialization;
using CEM = GVUZ.CompositionExportModel;

namespace FBS.Replicator.DB.ERBD
{
    public class CompositionsHelper
    {
        public const byte OKStatus = 2;
        private const int MessageSize = 10000;

        public static Dictionary<string, ERBDCompositionInfo> GetAllCompositionInfos(bool for2015, bool for2016)
        {
            Dictionary<string, ERBDCompositionInfo> result = new Dictionary<string, ERBDCompositionInfo>();
            if (for2015)
            {
                int counter = 0;
                if (!String.IsNullOrEmpty(Connections.CompositionsPagesCountPath2015Plus))
                {
                    Logger.WriteLine("PagesCount2015 from local path");
                    using (var stream = System.IO.File.OpenRead(Connections.CompositionsPagesCountPath2015Plus))
                    {
                        var serializer = new XmlSerializer(typeof(CEM.ERBDCompositionInfoList));
                        var ciList = serializer.Deserialize(stream) as CEM.ERBDCompositionInfoList;
                        Logger.WriteLine("PagesCount2015 local - " + ciList.Items.Count);
                        foreach (var i in ciList.Items)
                        {
                            if (!i.ParticipantId.HasValue)
                                continue;

                            if (!result.ContainsKey(i.ParticipantId.ToString()))
                            {
                                result.Add(i.ParticipantId.ToString(), new ERBDCompositionInfo(i.ParticipantId.Value, (byte)i.PagesCount));
                            }

                            counter++;
                            if (counter % MessageSize == 0)
                            {
                                Logger.WriteLine(String.Format("Обработано {0} бланков сочинений за 2015 год", counter));
                            }
                        }
                    }
                }
                else
                {
                    NetworkIO networkIO = new NetworkIO(Connections.CompositionsDirectoryUser2015, Connections.CompositionsDirectoryPassword2015);
                    foreach (string hash2Directory in networkIO.EnumerateDirectories(Connections.CompositionsStaticPath2015))
                    {
                        foreach (string hash4Directory in networkIO.EnumerateDirectories(hash2Directory))
                        {
                            foreach (string hashDirectory in networkIO.EnumerateDirectories(hash4Directory))
                            {
                                foreach (string documentNumberDirectory in networkIO.EnumerateDirectories(hashDirectory))
                                {
                                    foreach (string participantIdDirectory in networkIO.EnumerateDirectories(documentNumberDirectory))
                                    {
                                        string participantIdStr = participantIdDirectory.Split('\\').Last();
                                        Guid participantId;
                                        if (!Guid.TryParse(participantIdStr, out participantId))
                                            continue;

                                        int pagesCount = networkIO.EnumerateFiles(participantIdDirectory).Count();
                                        if (!result.ContainsKey(participantIdStr))
                                        {
                                            result.Add(participantIdStr, new ERBDCompositionInfo(participantId, (byte)pagesCount));
                                        }

                                        counter++;
                                        if (counter % MessageSize == 0)
                                        {
                                            Logger.WriteLine(String.Format("Обработано {0} бланков сочинений за 2015 год", counter));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                Logger.WriteLine(String.Format("Обработано {0} бланков сочинений за 2015 год", counter));
            }
            if (for2016)
            {
                int counter = 0;

                List<PagesCountFile> pagesCountFiles = new List<PagesCountFile>();
                if (!string.IsNullOrEmpty(Connections.CompositionsPagesCountPath2016Plus))
                {
                    foreach (string pagesCountFilePath in Directory.EnumerateFiles(Connections.CompositionsPagesCountPath2016Plus, "*.txt", SearchOption.AllDirectories))
                    {
                        pagesCountFiles.Add(new PagesCountFile(File.ReadAllLines(pagesCountFilePath)));
                    }
                }
                else
                {
                    NetworkIO networkIO = new NetworkIO(Connections.CompositionsDirectoryUser2016Plus, Connections.CompositionsDirectoryPassword2016Plus);
                    foreach (string dateDirectory in networkIO.EnumerateDirectories(Connections.CompositionsStaticPath2016Plus))
                    {
                        string pagesCountFilePath = dateDirectory + "\\pagescount.txt";
                        if (networkIO.FileExists(pagesCountFilePath))
                        {
                            pagesCountFiles.Add(new PagesCountFile(networkIO.ReadTestLinesFile(pagesCountFilePath)));
                        }
                    }
                }

                foreach (PagesCountFile pagesCountFile in pagesCountFiles)
                {
                    foreach (string line in pagesCountFile.Content)
                    {
                        ERBDCompositionInfo compositionInfo = new ERBDCompositionInfo(line);
                        if (!compositionInfo.Parsed)
                            continue;

                        string barcode = compositionInfo.BarcodeStr;
                        if (!result.ContainsKey(barcode))
                        {
                            result.Add(barcode, compositionInfo);
                        }

                        counter++;
                        if (counter % MessageSize == 0)
                        {
                            Logger.WriteLine(String.Format("Обработано {0} бланков сочинений за 2016+ год", counter));
                        }
                    }
                }

                Logger.WriteLine(String.Format("Обработано {0} бланков сочинений за 2016+ год", counter));
            }
            return result;
        }

        public class PagesCountFile
        {
            public PagesCountFile(string[] content)
            {
                Content = content;
            }
            public string[] Content { get; private set; }
        }
    }
}
