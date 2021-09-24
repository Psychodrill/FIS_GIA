using GVUZ.CompositionExportModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace CompositionExportServiceClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void TestCompositions_btnclk(object sender, EventArgs e)
        {
            var compositionItems = new List<CompositionRequestItem>();

            var item = new CompositionRequestItem
            {
                CompositionPaths = txtPath.Text,
                LastName = txtLastName.Text
            };
            compositionItems.Add(item);

            using (ICompositionExportService service = new GVUZ.CompositionExportModel.CompositionExportServiceClient())
            {
                var res = service.GetCompositions(compositionItems);
                MessageBox.Show(string.Format("результат: {0}, {1}", res.HasData, res.File.Length));
            }

        }

        private void LoadCompositions_btnclk(object sender, EventArgs e)
        {
            txtResult.Text = "";
            using (ICompositionExportService service = new GVUZ.CompositionExportModel.CompositionExportServiceClient())
            {
                var res = service.GetAllCompositionInfos(chk2015.Checked, chk2016.Checked);
                foreach (var v in res.Items)
                {
                    var partId = v.ParticipantId.HasValue ? v.ParticipantId.Value.ToString() : string.Empty;
                    var barcodeLen = v.Barcode != null ? v.Barcode.Length : 0;
                    txtResult.Text += string.Format("{0} {1} {2} {3} {4}\n", v.Key, partId, v.PagesCount, barcodeLen, v.Parsed);
                }
            }
        }

        private void SaveCompositions_btnclk(object sender, EventArgs e)
        {
            using (ICompositionExportService service = new GVUZ.CompositionExportModel.CompositionExportServiceClient())
            {
                var res = service.PrepareCompositionInfos(chk2015.Checked, chk2016.Checked);
                txtResult.Text = res.ToString();
            }
        }

        private void CreateComposition_btnclk(object sender, EventArgs e)
        {
            string CompositionNewDrive = ConfigurationManager.AppSettings["CompositionNewDrive"];
            string fileName2015 = Path.Combine(CompositionNewDrive, "compositionInfo2015.txt");
            string fileName2016 = Path.Combine(CompositionNewDrive, "compositionInfo2016_w.txt");

            ERBDCompositionInfoList result = new ERBDCompositionInfoList();
            int counter = 0;

            List<PagesCountFile> pagesCountFiles = new List<PagesCountFile>();
            var basePath2016 = Path.Combine(CompositionNewDrive, "20");
            foreach (string dateDirectory in Directory.EnumerateDirectories(basePath2016))
            {
                string pagesCountFilePath = Path.Combine(dateDirectory, "pagescount.txt");
                if (File.Exists(pagesCountFilePath))
                {
                    pagesCountFiles.Add(new PagesCountFile(File.ReadAllLines(pagesCountFilePath)));
                }
            }
            txtResult.Text += "pagesCountFiles готовы" + Environment.NewLine;
            txtResult.Refresh();

            foreach (PagesCountFile pagesCountFile in pagesCountFiles)
            {
                foreach (string line in pagesCountFile.Content)
                {
                    ERBDCompositionInfo compositionInfo = new ERBDCompositionInfo(line);
                    if (compositionInfo.Parsed)
                    {

                        string barcode = Encoding.UTF8.GetString(compositionInfo.Barcode);
                        if (!result.Items.Any(t => t.Key == barcode))
                        {
                            compositionInfo.Key = barcode;
                            result.Items.Add(compositionInfo);
                        }
                        counter++;
                        if (counter % 100000 == 0)
                        {
                            txtResult.Text += string.Format("Обработано {0} бланков сочинений за 2016+ год", counter) + Environment.NewLine;
                            txtResult.Refresh();
                        }
                    }
                }
            }
            txtResult.Text += string.Format("Обработано {0} бланков сочинений за 2016+ год", counter) + Environment.NewLine;
            txtResult.Refresh();
            SaveCompositionInfoFile(fileName2016, result);
            txtResult.Text += string.Format("Данные бланков сочинений за 2016+ год сохранены {0}", fileName2016) + Environment.NewLine;
            txtResult.Refresh();
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
    }
}
