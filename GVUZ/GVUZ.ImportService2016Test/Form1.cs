using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GVUZ.ImportService2016.Core.Main;
using GVUZ.ImportService2016.Core.Main.Log;
using System.Data.SqlClient;
using GVUZ.ImportService2016.Core.Main.Import;

namespace ImportService2016Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            txtMessages.Text = "Hellow!";
        }

        private void ImportPackBtnClk(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(txtPackageID.Text) && GetPackageID == 0)
                {
                    if (MessageBox.Show("Номер пакета указан, но не является числом!\nВы уверены, что хотите продолжить?\n(Будет выбран первый пакет из очереди)", "Очистка bulk-таблиц", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                        return;
                }
                
                AddMessage("Обработка пакета " + (GetPackageID != 0 ? GetPackageID.ToString() : "") + " стартовала");
                LogHelper.InitLoging();
                ProcessingManager.StartWinForms(GetPackageID, !chkBulksNotDeleted.Checked);
                LogHelper.Log.Info("Сервис импорта в тестовом режиме запущен: " + DateTime.Now);
                AddMessage("Обработка пакета " + (GetPackageID != 0 ? GetPackageID.ToString() : "") + " завершена");
            }
            catch (Exception ex)
            {
                LogHelper.Log.Error(ex.Message, ex);
                AddMessage("Во время работы сервиса возникла ошибка: " + ex.Message + "\n" + ex.StackTrace);
                if (ex.InnerException != null)
                    AddMessage("Inner Exception: " + ex.InnerException.Message + "\n" + ex.InnerException.StackTrace);
            }
        }

        private void CheckBtnClick(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(txtPackageID.Text) && GetPackageID == 0)
                {
                    if (MessageBox.Show("Номер пакета указан, но не является числом!\nВы уверены, что хотите продолжить?\n(Будет выбран первый пакет из очереди)", "Очистка bulk-таблиц", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                        return;
                }

                AddMessage("Проверка пакета " + (GetPackageID != 0 ? GetPackageID.ToString() : "") + " стартовала");
                LogHelper.InitLoging();
                ProcessingManager.CheckWinForms(GetPackageID);
                LogHelper.Log.Info("Проверка в тестовом режиме запущен: " + DateTime.Now);
                AddMessage("Проверка пакета " + (GetPackageID != 0 ? GetPackageID.ToString() : "") + " завершена");
            }
            catch (Exception ex)
            {
                LogHelper.Log.Error(ex.Message, ex);
                AddMessage("Во время проверки возникла ошибка: " + ex.Message + "\n" + ex.StackTrace);
                if (ex.InnerException != null)
                    AddMessage("Inner Exception: " + ex.InnerException.Message + "\n" + ex.InnerException.StackTrace);
            }
        }

        //bool check = false;
        private void OnImportPackFixedIDbtnclk(object sender, EventArgs e)
        {
            var _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Main"].ConnectionString;
            var res = new GVUZ.ImportService2016.Core.Dto.Partial.ImportPackage();
            var ds = new DataSet();

            using (var connection = new SqlConnection(_connectionString))
            {


                string sql = @"Select * from ImportPackage where PackageID = 584881;";

                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 600000;
                    var adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(ds);
                }

                if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    return;

                // Get ImportPackage
                var ipRow = ds.Tables[0].Rows[0];
                res.PackageData = ipRow["PackageData"].ToString();
                res.InstitutionID = (int)ipRow["InstitutionID"];
                res.PackageID = (int)ipRow["PackageID"];
                res.StatusID = ipRow["StatusID"] != null ? (int)ipRow["StatusID"] : 1;
                res.CheckStatusID = ipRow["CheckStatusID"] != DBNull.Value ? (int)ipRow["CheckStatusID"] : 0;
                res.ImportedAppIDs = ipRow["ImportedAppIDs"] != null ? ipRow["ImportedAppIDs"].ToString() : "";
                res.TypeID = (int)ipRow["TypeID"];


                res.UserLogin = ipRow["UserLogin"].ToString();

                //VocabularyStorage vocabularyStorage = new VocabularyStorage(ds);
                //res.VocabularyStorage = vocabularyStorage;
            }


            var importManager = new ImportManager(res);
            List<int> applicationIDs = new List<int>() { 20514746, 20514780, 20514781, 20514782 };


            //check = checkBox1.Checked;
            //while (check)
            //{
            //    new GVUZ.ImportService2016.Core.Main.Check.CheckManager(importManager.PackageData, applicationIDs, "usman@timacad.ru").DoWork();
            //}
        }

        private int GetPackageID
        {
            get
            {
                int packageID;
                if (!int.TryParse(txtPackageID.Text, out packageID))
                    packageID = 0;
                return packageID;
            }
        }

        private void cmdClearBulk_Click(object sender, EventArgs e)
        {
            if (GetPackageID ==0)
            {
                if (MessageBox.Show("Номер пакета не указан!\nВы уверены, что хотите очистить все bulk-таблицы?", "Очистка bulk-таблиц", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                    return;
            }

            AddMessage("Очистка балк-таблиц запущена");
            try
            {
                GVUZ.ImportService2016.Core.Main.Repositories.ADOPackageRepository.ResetImportPackages(false, false, true, GetPackageID);
                AddMessage("Очистка балк-таблиц завершена");
            }
            catch (Exception ex)
            {
                AddMessage("Очистка балк-таблиц завершена c ошибкой: " + ex.Message + "\n" + ex.StackTrace);
            }
        }

        private void AddMessage(string text)
        {
            txtMessages.Text += string.Format("\n{0} {1}", DateTime.Now.ToUniversalTime(), text);
            txtMessages.Refresh();
        }

        
    }
}
