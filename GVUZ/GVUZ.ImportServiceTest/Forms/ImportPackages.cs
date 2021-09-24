using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace GVUZ.ImportServiceTest.Forms
{
    public struct PackageItem
    {
        public string Path { get; set; }
        public long SizeInKb { get; set; }
        public bool IsSuccess { get; set; }
        public string ResultMessage { get; set; }
    }

    public partial class ImportPackages : Form
    {
        public ImportPackages()
        {
            InitializeComponent();
        }

        ConcurrentStack<PackageItem> packages = new ConcurrentStack<PackageItem>();

        private void btn_Export_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tb_Path.Text))
            {
                WriteToLog("Необходимо указать папку с пакетами!");
                return;
            }
            
            var di = new DirectoryInfo(tb_Path.Text);
            if (!di.Exists)
            {
                WriteToLog("Указанная папка с пакетами не найдена!");
                return;
            }

            var files = di.GetFiles("*.xml");
            if (files.Length == 0)
            {
                WriteToLog("Указанная папка с пакетами не содержит файлов для отправки!");
                return;
            }

            foreach (var fileInfo in files)
                packages.Push(new PackageItem {Path = fileInfo.FullName, SizeInKb = fileInfo.Length/1024});

            progress.Step = 1;
            progress.Value = 0;
            progress.Maximum = files.Length;

            for (int i = 1; i <= (int) tb_ThreadsCount.Value; i++)
            {
                ThreadStart handler = () => RunThread(i);
                new Thread(handler).Start();
            }

            while (packages.Count > 0) { }
            WriteToLog("== ВСЕ ПАКЕТЫ ОТПРАВЛЕНЫ ==");
        }

        public void RunThread(int index)
        {
            try
            {
                PackageItem pi;
                while (packages.TryPop(out pi))
                {
                    progress.PerformStep();
                    Application.DoEvents();

                    try
                    {
                        using (var cl = new WebClient { Encoding = Encoding.UTF8 })
                        {
                            cl.Headers["Content-Type"] = "text/xml";

                            var doc = new XmlDocument();
                            doc.Load(pi.Path);

                            pi.ResultMessage = cl.UploadString(this.textBox2.Text, doc.OuterXml);

                            WriteToLog(string.Format("Поток №{3} >>> Отправил файл: {1} ({0}), Статус: {2}",
                                DateTime.Now.ToLongTimeString(), pi.Path, 200, index));
                        }
                    }
                    catch (WebException ex)
                    {
                        pi.ResultMessage = ex.Message;
                        WriteToLog(string.Format("Поток №{4} !!! Отправил файл: {1} ({0}), Статус: {2}, Сообщение: {3}",
                            DateTime.Now.ToLongTimeString(), pi.Path, ex.Status, pi.ResultMessage, index));
                    }
                }
            }
            catch (Exception ex)
            {
                WriteToLog(string.Format("Поток №{0} !!! Ошибка: {1}", index, ex.Message));
            }

            WriteToLog(string.Format("Поток №{0} <<< Завершил работу", index));
        }

        public delegate void OnWriteToLog (string message);
        private void WriteToLog(string message)
        {
            if (InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate { WriteToLogMethod(message); }));
                return;
            }

            WriteToLogMethod(message);
        }

        public void WriteToLogMethod(string message)
        {
            lb_Results.Items.Add(message);
            lb_Results.SelectedIndex = lb_Results.Items.Count - 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog(this) == DialogResult.OK)
                tb_Path.Text = folderBrowserDialog1.SelectedPath;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string res = this.textBox1.Text.Trim();
            string res2 = this.comboBox1.SelectedItem as string;
            if (!String.IsNullOrEmpty(res2))
                res2 = res2.Substring(0, res2.IndexOf(" "));
            this.textBox2.Text = res + res2;
        }
    }
}
