using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using CheckWebService.XmlToCsv;
using CheckWebService.WSChecksClient;

namespace CheckWebService
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Получаем web-сервис. Проходим аутентификацию
        /// </summary>
        /// <returns></returns>
        private WSChecks GetClient()
        {
            WSChecksClient.WSChecks webService = new WSChecksClient.WSChecks();

            // в SOAP-заголовок добавляются поля: логин, пароль, клиент
            WSChecksClient.UserCredentials credentials = new WSChecksClient.UserCredentials();
            credentials.Login = tb_Login.Text;
            credentials.Password = tb_Password.Text;
            credentials.Client = tb_Client.Text;
            webService.UserCredentialsValue = credentials;

            return webService;
        }

        private void btTryConnect_Click(object sender, EventArgs e)
        {
            if (!ValidateUrl())
                return;

            string rawSingleQuerySample = String.Empty;
            string rawBatchQuerySample = String.Empty;

            try
            {
                WSChecksClient.WSChecks wsChecks = GetClient();
                rawSingleQuerySample = wsChecks.GetSingleCheckQuerySample();
                rawBatchQuerySample = wsChecks.GetBatchCheckQuerySample();
                ShowSuccessResult();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            DisplayXml(tb_SingleQuerySample, rawSingleQuerySample);
            DisplayXml(tb_BatchQuerySample, rawBatchQuerySample);

            tbResult.Text = "См. примеры запросов";
        }

        private void ShowSuccessResult()
        {
            MessageBox.Show("Операция завершена");
        }

        private void ValidateXmlInput(string xmlString)
        {
            if (String.IsNullOrEmpty(xmlString))
            {
                MessageBox.Show("Введите запрос");
                return;
            }
            try
            {
                new XmlDocument().LoadXml(xmlString);
            }
            catch
            {
                MessageBox.Show("Запрос имеет неверный формат (ожидается xml)");
                return;
            }
        }

        private string GetCleanString(string dirtyString)
        {
            string cleanString = dirtyString.Replace(Environment.NewLine, "");
            while ((cleanString.Contains(" <")) || (cleanString.Contains("> ")))
            {
                cleanString = cleanString.Replace(" <", "<").Replace("> ", ">");
            }
            return cleanString;
        }

        private void DisplayXml(TextBox textBox, string xml)
        {
            textBox.Text = new XmlFormatter().FormatXml(xml);
        }

        private bool ValidateUrl()
        {
            if (String.IsNullOrEmpty(tb_Url.Text))
            {
                MessageBox.Show("Не задан адрес веб-сервиса");
                return false;
            }
            return true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tb_Url.Text = new WSChecksClient.WSChecks().Url;
            tb_Login.Text = Settings.CheckWSUserName;
            tb_Password.Text = Settings.CheckWSPassword;
            tb_Client.Text = Settings.CheckWSClient;
        }

        private void AssignInnerTextIfNotNull(XmlNode node, string innerText)
        {
            if (node == null)
                return;

            node.InnerText = innerText;
        }

        /// <summary>
        /// Загрузка примера для единичной проверки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_LoadSingleExample_Click(object sender, EventArgs e)
        {
            if (!ValidateUrl())
                return;

            try
            {
                WSChecksClient.WSChecks wsChecks = GetClient();
                DisplayXml(tb_SingleQuerySample, wsChecks.GetSingleCheckQuerySample());
                ShowSuccessResult();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Единичная проверка
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SingleCheck_Click(object sender, EventArgs e)
        {
            if (!ValidateUrl())
                return;

            string result = String.Empty;
            try
            {
                result = GetClient().SingleCheck(GetCleanString(tb_SingleQuerySample.Text));
                ShowSuccessResult();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            DisplayXml(tbResult, result);
        }

        /// <summary>
        /// Загрузка примера для пакетной проверки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_LoadBatchExample_Click(object sender, EventArgs e)
        {
            if (!ValidateUrl())
                return;

            try
            {
                WSChecksClient.WSChecks wsChecks = GetClient();
                DisplayXml(tb_BatchQuerySample, wsChecks.GetBatchCheckQuerySample());
                ShowSuccessResult();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Запуск пакетной проверки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_StartBatchCheck_Click(object sender, EventArgs e)
        {
            if (!ValidateUrl())
                return;

            string result = String.Empty;
            try
            {
                result = GetClient().BatchCheck(GetCleanString(tb_BatchQuerySample.Text));
                ShowSuccessResult();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            DisplayXml(tbResult, result);
        }

        /// <summary>
        /// Получение результатов пакетной проверки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_GetBatchResult_Click(object sender, EventArgs e)
        {
            if (!ValidateUrl())
                return;

            string result = String.Empty;
            try
            {
                result = GetClient().GetBatchCheckResult(tbBatchId.Text.Trim());
                ShowSuccessResult();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            DisplayXml(tbResult, result);
        }
    }
}
