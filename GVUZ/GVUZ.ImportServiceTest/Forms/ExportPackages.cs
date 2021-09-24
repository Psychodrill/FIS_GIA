using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace GVUZ.ImportServiceTest.Forms
{
    public partial class ExportPackages : Form
    {
        public ExportPackages()
        {
            InitializeComponent();
        }

        private void btn_Export_Click(object sender, EventArgs e)
        {
            int packageId;

            if (checkBox1.Checked && MessageBox.Show("Вы действительно хотите изменить права доступа в ЕСРП?", 
                "", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
            {
                return;
            }

            finish.Visible = false;
            sad.Visible = false;

            lb_Results.Items.Clear();
            progress.Value = 0;
            progress.Step = 1;
            progress.Maximum = lb_PackagesIds.Lines.Length;

            if (string.IsNullOrEmpty(tb_EsrpDbName.Text))
            {
                WriteToLog("Необходимо указать наименование базы данных ЕСРП!");
                return;
            }
            if (string.IsNullOrEmpty(tb_Path.Text))
            {
                WriteToLog("Необходимо указать папку для пакетов!");
                return;
            }
            if (!Directory.Exists(tb_Path.Text))
            {
                WriteToLog("Указанная директория не найдена!");
                return;
            }

            foreach (string id in lb_PackagesIds.Lines)
            {
                if (!Int32.TryParse(id, out packageId))
                    WriteToLog(string.Format("Не распознан идентификатор пакета ({0})", id));

                ExportPackageItem(packageId, checkBox1.Checked);
            }

            finish.Visible = !sad.Visible;
            WriteToLog(string.Format("== ИЗВЛЕЧЕНИЕ ЗАВЕРШЕНО =="));
        }

        private void ExportPackageItem(int packageId, bool withGroupAccessInsert)
        {
            progress.PerformStep();
            Application.DoEvents();

            var groups = string.Format(@"
            INSERT INTO {1}..GroupAccount
            SELECT 15, a.Id 
            FROM
	            ImportPackage ip
	            JOIN {1}..Account a ON ip.UserLogin = a.Login
	            LEFT JOIN {1}..GroupAccount ga 
		            ON a.Id = ga.AccountId AND ga.GroupId = 15
            WHERE ip.PackageID = {0} AND ga.Id IS NULL", packageId, tb_EsrpDbName.Text.Trim());

            var query = string.Format(@"
                select UserLogin, uap.Password, PackageData
                from ImportPackage ip
	                join {1}..Account pa ON pa.Login = ip.UserLogin
	                join {1}..UserAccountPassword uap on uap.AccountId = pa.Id
                WHERE PackageID = {0}", packageId, tb_EsrpDbName.Text.Trim());

            try
            {
                XElement rootElement = null;
                XElement packageDataElement = null;

                bool packageIsExists = false;
                using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Main"].ConnectionString))
                {
                    connection.Open();

                    SqlCommand command;
                    /* Если у логина нет прав - пробить из в ЕСРП */
                    if (withGroupAccessInsert)
                    {
                        command = new SqlCommand(groups, connection);
                        var result = command.ExecuteNonQuery();
                        if (result != 0)
                        {
                            WriteToLog(string.Format(">>> Предоставлен доступ к ЕСРП для пакета ({0})", packageId));
                        }
                    }
                    
                    command = new SqlCommand(query, connection) {CommandTimeout = 600};
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader.HasRows)
                            {
                                packageIsExists = true;
                                rootElement = new XElement("Root",
                                                            new XElement("AuthData",
                                                                        new XElement("Login", reader[0]),
                                                                        new XElement("Pass", reader[1])));
                                var data = reader[2].ToString();
                                if (!string.IsNullOrEmpty(data))
                                {
                                    packageDataElement = XElement.Parse(data);
                                    rootElement.Add(packageDataElement);
                                }
                            }
                        }
                    }
                }

                if (!packageIsExists)
                {
                    WriteToLog(string.Format(">>> Не найден пакет ({0}) (возможно нет логина в ЕСРП)", packageId));
                    sad.Visible = true;
                }
                else
                {
                    var di = new DirectoryInfo(tb_Path.Text);
                    if (di.Exists)
                    {
                        var settings = new XmlWriterSettings { OmitXmlDeclaration = true };
                        using (var writer = XmlWriter.Create(Path.Combine(di.FullName, string.Format("{0}.xml", packageId)), settings))
                        {
                            rootElement.Save(writer);
                        }

                        int appsCount = 0;
                        if (packageDataElement != null)
                        {
                            var apps = packageDataElement.Descendants("Application");
                            appsCount = apps.Count();
                        }

                        Application.DoEvents();
                        WriteToLog(string.Format("Выгружен пакет ({0}). Кол-во заявлений в пакете ({1})", packageId, appsCount));
                    }
                    else
                    {
                        WriteToLog(string.Format("Не найдена директория ({0})", tb_Path.Text));
                        sad.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                WriteToLog(string.Format("Ошибка в пакете ({0}): {1}", packageId, ex.Message));
                sad.Visible = true;
            }

            Application.DoEvents();
        }

        private void WriteToLog(string message)
        {
            lb_Results.Items.Add(message);
            lb_Results.SelectedIndex = lb_Results.Items.Count - 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog(this) == DialogResult.OK)
                tb_Path.Text = folderBrowserDialog1.SelectedPath;
        }
    }
}
