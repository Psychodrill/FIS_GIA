using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using GVUZ.ImportServiceTest.Forms;
using GVUZ.ImportServiceTest.Properties;
using System.Xml;

namespace GVUZ.ImportServiceTest
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
            menuStripOps.Visible = false; // true;
		}

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			string res = this.tbBaseAddress.Text.Trim();
			string res2 = this.cmbxMethodsList.SelectedItem as string;
			if (!String.IsNullOrEmpty(res2))
				res2 = res2.Substring(0, res2.IndexOf(" "));
			this.tbResultPath.Text = res + res2;
			this.tbXmlQry.Enabled = res2 == null || !res2.Contains("/test/");
            this.btnCallService.Enabled = !String.IsNullOrEmpty(res2);
		}

        private void button1_Click(object sender, EventArgs e)
        {
            tbServiceResponse.Text = "Запрос отправлен";
            Application.DoEvents();

            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(this.tbResultPath.Text);
                request.KeepAlive = false;
                request.Method = "Post";
                request.ContentType = "text/xml";
                request.Timeout = Int32.MaxValue;
                byte[] data = Encoding.UTF8.GetBytes(this.tbXmlQry.Text);

                request.ContentLength = data.Length;
                Stream reqStream = request.GetRequestStream();
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();

                WebResponse response = request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    this.tbServiceResponse.Text = reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                tbServiceResponse.Text = ex.Message;
            }

            //WebClient cl = new WebClient();
            //cl.Encoding = Encoding.UTF8;
            //cl.Headers["Content-Type"] = "text/xml";
            //string url = this.textBox2.Text;
            //try
            //{

            //    if (url.Contains("/test/"))
            //    {
            //        this.textBox4.Text = cl.DownloadString(url);
            //    }
            //    else
            //    {
            //        this.textBox4.Text = cl.UploadString(url, this.textBox3.Text);
            //    }
            //}
            //catch (WebException ex)
            //{
            //    if (ex.Response != null)
            //    {
            //        using (var sr = new StreamReader(ex.Response.GetResponseStream()))
            //            this.textBox4.Text = sr.ReadToEnd();
            //    }
            //    else
            //        this.textBox4.Text = ex.Message;
            //}
            FormatOutput();
        }

		private void FormatOutput()
		{
			try
			{
				var xElement = XElement.Load(new StringReader(this.tbServiceResponse.Text));
				this.tbServiceResponse.Text = xElement.ToString(SaveOptions.None);
			}
			catch (Exception)
			{
			}
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			Settings.Default.Save();
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			comboBox1_SelectedIndexChanged(sender, e);
		}

		private void textBox3_KeyDown(object sender, KeyEventArgs e)
		{
            if (e.Control && (e.KeyCode == System.Windows.Forms.Keys.A))
            {
                ((RichTextBox)sender).SelectAll();
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
            else
                base.OnKeyDown(e);
		}

        private void button2_Click(object sender, EventArgs e)
        {
            string s = this.tbXmlQry.Text;
            if (String.IsNullOrWhiteSpace(s))
                s = "<Root></Root>";
            XElement xElement = null;
            try
            {
                xElement = XElement.Load(new StringReader(s));
            }
            catch (Exception)
            {
                MessageBox.Show("Неверный XML");
                return;
            }
            if (!xElement.Name.LocalName.Equals("Root", StringComparison.CurrentCultureIgnoreCase))
            {
                xElement = new XElement("Root", xElement);
            }

            if (xElement.Elements("AuthData").Count() > 0)
                xElement.Elements("AuthData").ToList().ForEach(x => x.Remove());

            XElement auth = new XElement("AuthData", new XElement("Login", this.tbLogin.Text.Trim()), new XElement("Pass", this.tbPassword.Text.Trim()));
            if (checkBox1.Checked)
                auth.Add(XElement.Parse("<InstitutionID>" + tbOOId.Text.Trim() +"</InstitutionID>"));
            //XElement auth = XElement.Load(String.Format("<AuthData><Login>{0}</Login><Pass>{1}</Pass></AuthData>", this.textBox5.Text.Trim(), this.textBox6.Text.Trim()));
            //xElement.Add(new XElement("test"));
            xElement.AddFirst(auth);
            this.tbXmlQry.Text = xElement.ToString();
        }

        private int increment = 0;
        private void button3_Click(object sender, EventArgs e)
        {
            int count;
            Int32.TryParse(tb_Count.Text, out count);

            var xml = tbXmlQry.Text;
            if (string.IsNullOrEmpty(xml))
            {
                MessageBox.Show("Необходимо вставить шаблон с тильдами _{0} для клонирования!");
                return;
            }

            tbXmlQry.Text = string.Empty;
            XElement xElement = null;
            try
            {
                xElement = XElement.Load(new StringReader(xml));
            }
            catch (Exception)
            {
                MessageBox.Show("Неверный XML");
                return;
            }

            var root = new XElement("Root");
            for (int i = 0; i < count; i++)
            {
                increment++;
                root.Add(XElement.Parse(string.Format(xml, increment)));
            }

            tbXmlQry.Text = root.ToString();
        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
                 && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
              tbOOId.Enabled = true;
            else
              tbOOId.Enabled = false;
        }

        private void извлечениеПакетовИзБДToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ExportPackages().ShowDialog(this);
        }

        private void загрузкаПакетовВБДToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ImportPackages().ShowDialog(this);
        }

        private void bLoadXml_Click(object sender, EventArgs e)
        {
            if (openPackageDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string fileName = openPackageDialog.FileName;

                using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, (int)fs.Length);

                    string text = Encoding.UTF8.GetString(buffer);
                    tbXmlQry.Text = text;
                }
            }
        }

        //private void импортПакетовToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    new ImportServiceTest.Forms.ImportServiceTest().ShowDialog(this);
        //}
    }
}
