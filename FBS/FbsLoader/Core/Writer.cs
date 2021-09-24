using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data.SqlClient;

namespace Core
{
    public class BulkWriter
    {
        private string TableName { get; set; } // Имя таблицы
        private string Columns { get; set; } // Список столбцов таблицы
        private string ValueDelimeter { get; set; } // Разделитель значение в csv-файлах
        private string TempDirPath { get; set; } // Папка в которой хранятся файлы для bulk insert
        private int FlushRowsCount { get; set; } // Количество строк, при которых инициируется bulk insert
        private List<string> Values { get; set; } // Коллекция значений для вставки в таблицу
        private string ConnectionString { get; set; } // Строка соединения

        public BulkWriter(string tableName, List<string> columns)
        {
            TableName = tableName;
            Columns = string.Join(", ", columns.ToArray());
            ValueDelimeter = "|"; // TODO: естественно, блять, брать это надо из настроек. Но сейчас времени нет
            TempDirPath = "C:\\Shared\\temp\\"; // TODO: тоже из настроект надо брать
            FlushRowsCount = 1000; // TODO: тоже надо брать из настроек

            Values = new List<string>();
        }

        public void Add(string dataRow)
        {
            Values.Add(dataRow);

            if (Values.Count >= FlushRowsCount)
            {
                string fileName = CreateBulkFile(Values);
                FlushBulkFile(fileName);
                Values.Clear();
            }
        }

        #region Private Meshods

        private string CreateBulkFile(List<string> values)
        {
            string filePath = TempDirPath + Guid.NewGuid().ToString() + ".dat";

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            TextWriter tw = new StreamWriter(filePath, false, Encoding.UTF8);
            foreach (string val in values)
            {
                tw.WriteLine(val);
            }
            tw.Close();

            return filePath;
        }

        private void FlushBulkFile(string fileName)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText =
                    string.Format(@"
                    bulk insert {0}
                    from '{1}'
                    with (
                        FIELDTERMINATOR = '{2}'
                    )
                    ", 
                    TableName,
                    fileName,
                    ValueDelimeter
                    );
            }
        }

        #endregion
    }
}
