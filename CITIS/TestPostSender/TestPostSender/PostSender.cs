using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestPostSender
{
    public partial class PostSender : Form
    {
        public PostSender()
        {
            InitializeComponent();
        }

        private void Send_Click(object sender, EventArgs e)
        {
            
            PostRequestAsync(URL.Text,InputName.Text, OutText, PathToFile.Text);
        }

        private void OpenFile_Click(object sender, EventArgs e)
        {
            if (FileDialog.ShowDialog() == DialogResult.OK)
            {
                PathToFile.Text = FileDialog.FileName;
            }
        }


        private static async Task PostRequestAsync(string url,string inpname, RichTextBox outtext, string path)
        {
            WebRequest request = WebRequest.Create(url);
            request.Method = "POST"; // для отправки используется метод Post
                                     // данные для отправки

            string data = inpname + "=";// "sName=Hello world!";
            using (StreamReader file = new StreamReader(path, Encoding.Default))
            {
                data+=file.ReadToEnd();
                file.Close();
            }


            // преобразуем данные в массив байтов
            byte[] byteArray = Encoding.UTF8.GetBytes(data);
            // устанавливаем тип содержимого - параметр ContentType
            request.ContentType = "application/x-www-form-urlencoded";
            // Устанавливаем заголовок Content-Length запроса - свойство ContentLength
            request.ContentLength = byteArray.Length;

            //записываем данные в поток запроса
            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }

            WebResponse response = await request.GetResponseAsync();
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    //Console.WriteLine(reader.ReadToEnd());
                    //MessageBox.Show(reader.ReadToEnd());
                    outtext.Text = reader.ReadToEnd();
                }
            }
            response.Close();

        }

    }
}
