using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace Core
{
    public class BulkWriter
    {
        private string TableName { get; set; } // Имя таблицы
        private string ValueDelimeter { get; set; } // Разделитель значение в csv-файлах
        private string TempDirPath { get; set; } // Папка в которой хранятся файлы для bulk insert
        private int FlushRowsCount { get; set; } // Количество строк, при которых инициируется bulk insert
        private List<string> Values { get; set; } // Коллекция значений для вставки в таблицу
        private SqlConnection DestConnection { get; set; } // Соединение

        public BulkWriter(string tableName)
        {
            TableName = tableName;
            ValueDelimeter = Config.ValueDelimeter(); // TODO: естественно, блять, брать это надо из настроек. Но сейчас времени нет
            TempDirPath = Config.TempDirPath(); // TODO: тоже из настроект надо брать
            FlushRowsCount = Config.InsertFlushRowsCount();

            Values = new List<string>();
        }

        public void Init(SqlConnection destConnection)
        {
            DestConnection = destConnection;
        }

        public void Add(string dataRow)
        {
            Values.Add(dataRow);

            if (Values.Count >= FlushRowsCount)
            {
                Flush();
            }
        }

        public void Flush()
        {
            string fileName = CreateBulkFile(Values);
            FlushBulkFile(fileName);
            Values.Clear();
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            Logger.Instance.WriteLog("Загрузка из файла выполнена", 2);
        }

        #region Private Meshods

        private string CreateBulkFile(List<string> values)
        {
            string filePath = TempDirPath + Guid.NewGuid().ToString() + ".dat";

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            TextWriter tw = new StreamWriter(filePath, false, Encoding.Unicode);
            foreach (string val in values)
            {
                tw.WriteLine(val);
            }
            tw.Close();

            return filePath;
        }

        private void FlushBulkFile(string fileName)
        {
            SqlCommand cmd = DestConnection.CreateCommand();
            cmd.CommandTimeout = Config.CommandTimeout();
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
            cmd.ExecuteNonQuery();
        }

        #endregion
    }
}
