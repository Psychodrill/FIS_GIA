using FBS.CompositionsPathGenerator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FBS.ComposisionFix
{
    public partial class MainFrm : Form
    {
        public MainFrm()
        {
            InitializeComponent();
        }

        private void tbOOid_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnGetCompPaths_Click(object sender, EventArgs e)
        {
            //if (tbOOid.Text.Length == 0)
            //{
            //    MessageBox.Show("Введите индентификатор ОО");
            //}
            //else
            //{
            //    int inst_id;
            //    if (int.TryParse(tbOOid.Text, out inst_id))
            //    {
            //        //GetMissingData(inst_id);
            //        //GetErbdCompInfo();
            //        if ((compdataFromFiles == null) || (compdataFromFiles.Count == 0))
            //        {
            //            Dictionary<string, ERBDCompositionInfo> result = new Dictionary<string, ERBDCompositionInfo>();
            //            List<PagesCountFile> pagesCountFiles = new List<PagesCountFile>();
            //            if (!string.IsNullOrEmpty(Connections.CompositionsPagesCountPath2016Plus))
            //            {
            //                foreach (string pagesCountFilePath in Directory.EnumerateFiles(Connections.CompositionsPagesCountPath2016Plus, "*.txt", SearchOption.AllDirectories))
            //                {
            //                    long len = new FileInfo(pagesCountFilePath).Length;
            //                    if (len > 2097152)
            //                        continue;
            //                    pagesCountFiles.Add(new PagesCountFile(File.ReadAllLines(pagesCountFilePath)));
            //                }
            //            }
            //            else
            //            {
            //                NetworkIO networkIO = new NetworkIO(Connections.CompositionsDirectoryUser2016Plus, Connections.CompositionsDirectoryPassword2016Plus);
            //                foreach (string dateDirectory in networkIO.EnumerateDirectories(Connections.CompositionsStaticPath2016Plus))
            //                {
            //                    string pagesCountFilePath = dateDirectory + "\\pagescount.txt";
            //                    if (networkIO.FileExists(pagesCountFilePath))
            //                    {
            //                        pagesCountFiles.Add(new PagesCountFile(networkIO.ReadTestLinesFile(pagesCountFilePath)));
            //                    }
            //                }
            //            }
            //            foreach (PagesCountFile pagesCountFile in pagesCountFiles)
            //            {
            //                foreach (string line in pagesCountFile.Content)
            //                {
            //                    ERBDCompositionInfo compositionInfo = new ERBDCompositionInfo(line);
            //                    if (!compositionInfo.Parsed)
            //                        continue;

            //                    string barcode = compositionInfo.BarcodeStr;
            //                    if (!result.ContainsKey(barcode))
            //                    {
            //                        result.Add(barcode, compositionInfo);
            //                    }

            //                }
            //            }

            //Dictionary<string, ERBDCompositionInfo> result = new Dictionary<string, ERBDCompositionInfo>();
            ////compdataFromFiles = CompositionsHelper.GetAllCompositionInfos(false, true);
            ////rtbCompFilesData.AppendText("count : " + compdataFromFiles.Count.ToString());
            //List<PagesCountFile> pagesCountFiles = new List<PagesCountFile>();
            //if (!string.IsNullOrEmpty(/*@"\\10.0.3.5\Forms\20"*/ Connections.CompositionsPagesCountPath2016Plus))
            //{
            //    foreach (string pagesCountFilePath in Directory.EnumerateFiles(Connections.CompositionsPagesCountPath2016Plus, "*.txt", SearchOption.AllDirectories))
            //    {
            //        double len = new FileInfo(pagesCountFilePath).Length;
            //        if (len > 2097152)
            //            continue;
            //        pagesCountFiles.Add(new PagesCountFile(File.ReadAllLines(pagesCountFilePath)));
            //        rtbCompFilesData.AppendText( " : " + pagesCountFilePath + "\r\n");
            //    }

            //    //foreach (PagesCountFile pagesCountFile in pagesCountFiles)
            //    //{
            //    //    foreach (string line in pagesCountFile.Content)
            //    //    {
            //    //        ERBDCompositionInfo compositionInfo = new ERBDCompositionInfo(line);
            //    //        if (!compositionInfo.Parsed)
            //    //            continue;
            //    //        string barcode = compositionInfo.BarcodeStr;
            //    //        if (!result.ContainsKey(barcode))
            //    //        {
            //    //            result.Add(barcode, compositionInfo);
            //    //            //Logger.WriteLine(String.Format("Сочинение : Barcode {0} , List cout {}", barcode, compositionInfo.PagesCount));
            //    //        }
            //    //    }
            //    //}
            //}
            //}
            //try
            //{
            //foreach (DataRow dtRow in compMissingData.Rows)
            //{
            //    var personId = dtRow.Field<string>("ParticipantCode");
            //    foreach (DataRow erbdDatarow in erbdData.Rows)
            //    {
            //        var participantCode = erbdDatarow.Field<string>("ParticipantCode");
            //        if (personId == participantCode)
            //        {
            //            var barcode = dtRow.Field<string>("CompositionBarcode");
            //            int listCount = compdataFromFiles[barcode].PagesCount;
            //            var projName = erbdDatarow.Field<string>("ProjectName");
            //            var projBatchId = erbdDatarow.Field<int>("ProjectBatchID");
            //            var examDate = erbdDatarow.Field<DateTime>("ExamDate");
            //            //check for validity
            //            string paths = FBS.Common.CompositionPathsHelper.GetCompositionPaths2016Plus(@"\\10.0.3.5\Forms\20\", barcode, projBatchId,
            //                projName, examDate, listCount);
            //            rtbEnryptedPaths.AppendText(paths);

            //        }
            //    }
            //}
            //    }
            //    catch (Exception except)
            //    {
            //        MessageBox.Show("Ошибка : " + except.Message);
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("Введите числовой индентификатор ОО");
            //}
            //}
            rtbEnryptedPaths.Clear();
            string projName = tbProjName.Text;
            int projBatchId = Convert.ToInt32(tbProjBatchId.Text);
            string barcode = tbBarcode.Text;
            DateTime examDate = Convert.ToDateTime(tbExamDate.Text);
            int pageCount = Convert.ToInt32(tbPageCount.Text);
            string url = "\\\\10.0.3.5\\Forms\\20";
            string paths = FBS.Common.CompositionPathsHelper.GetCompositionPaths2016Plus(url, barcode, projBatchId,
              projName, examDate, pageCount);
            rtbEnryptedPaths.AppendText(paths);
        }

        private void btnfixCompPaths_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var rnd = new Random();
            long r = 2989060405486;
            for (int i = 1; i < 100000; i++ )
            {
                r = r - rnd.Next(10, 1000000);
                File.AppendAllText(@"D:\FIS_PRIEM\src\fisgia\FBS\binaries\Debug\x64\FBS.Replicator\test\t3\pagecount.txt", r.ToString() + "_3875_00_15_April_Soch_Izl:4\r\n");
            }
        }
    }
}
