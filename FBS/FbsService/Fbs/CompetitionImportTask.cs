using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace FbsService.Fbs
{
    public class CompetitionCertificateImportTask : Task
    {
        class Search : TaskStatus
        {
            static string LoadingDirectory = ConfigurationManager.AppSettings["CompetitionCertificateLoadingDirectory"];
            static string LogFileName = ConfigurationManager.AppSettings["CompetitionCertificateLoadingLog"];
            static long EditorAccountId = long.Parse(ConfigurationManager.AppSettings["LoaderAccountId"]);
            static string EditorIp = ConfigurationManager.AppSettings["LoaderIp"];

            private const string CreateTempImportStatement =
                    "create table #CompetitionCertificate \r\n" +
                    "    ( \r\n" +
		            "    CompetitionTypeId int \r\n" +
		            "    , LastName nvarchar(255) \r\n" +
		            "    , FirstName nvarchar(255) \r\n" +
		            "    , PatronymicName nvarchar(255) \r\n" +
		            "    , Degree nvarchar(255) \r\n" +
		            "    , RegionId int \r\n" +
		            "    , City nvarchar(255) \r\n" +
		            "    , School nvarchar(255) \r\n" +
                    "    , Class nvarchar(255) \r\n" +
                    "    , EditorAccountId bigint \r\n" +
                    "    , EditorIp nvarchar(255) \r\n" +
                    "    ) ";

            private const string DropTempImportStatement =
                    "drop table #CompetitionCertificate ";

            private const string InsertTempImportStatement =
                    "insert into #CompetitionCertificate \r\n" +
                    "values (@competitionTypeId, @lastName, @firstName, @patronymicName \r\n" +
                    "   , @degree, @regionId, @city, @school, @class, @editorAccountId, @editorIp)";

            private const string CommitSPName =
                    "dbo.ImportCompetitionCertificate";

            private static Hashtable Regions = new Hashtable()
                {
                    {"Республика Адыгея", 1},
                    {"Республика Башкортостан", 2},
                    {"Республика Бурятия", 3},
                    {"Республика Алтай", 4},
                    {"Республика Дагестан", 5},
                    {"Республика Ингушетия", 6},
                    {"Кабардино-Балкарская Республика", 7},
                    {"Республика Калмыкия", 8},
                    {"Карачаево-Черкесская Республика", 9},
                    {"Республика Карелия", 10},
                    {"Республика Коми", 11},
                    {"Республика Марий Эл", 12},
                    {"Республика Мордовия", 13},
                    {"Республика Саха (Якутия)", 14},
                    {"Республика Северная Осетия (Алания)", 15},
                    {"Республика Татарстан", 16},
                    {"Республика Тыва", 17},
                    {"Удмуртская Республика", 18},
                    {"Республика Хакасия", 19},
                    {"Чеченская Республика", 20},
                    {"Чувашская Республика", 21},
                    {"Алтайский край", 22},
                    {"Краснодарский край", 23},
                    {"Красноярский край", 24},
                    {"Приморский край", 25},
                    {"Ставропольский край", 26},
                    {"Хабаровский край", 27},
                    {"Амурская область", 28},
                    {"Архангельская область", 29},
                    {"Астраханская область", 30},
                    {"Белгородская область", 31},
                    {"Брянская область", 32},
                    {"Владимирская область", 33},
                    {"Волгоградская область", 34},
                    {"Вологодская область", 35},
                    {"Воронежская область", 36},
                    {"Ивановская область", 37},
                    {"Иркутская область", 38},
                    {"Калининградская область", 39},
                    {"Калужская область", 40},
                    {"Камчатский край", 41},
                    {"Камчатская область", 41},
                    {"Кемеровская область", 42},
                    {"Кировская область", 43},
                    {"Костромская область", 44},
                    {"Курганская область", 45},
                    {"Курская область", 46},
                    {"Ленинградская область", 47},
                    {"Липецкая область", 48},
                    {"Магаданская область", 49},
                    {"Московская область", 50},
                    {"Мурманская область", 51},
                    {"Нижегородская область", 52},
                    {"Новгородская область", 53},
                    {"Новосибирская область", 54},
                    {"Омская область", 55},
                    {"Оренбургская область", 56},
                    {"Орловская область", 57},
                    {"Пензенская область", 58},
                    {"Пермский край", 59},
                    {"Псковская область", 60},
                    {"Ростовская область", 61},
                    {"Рязанская область", 62},
                    {"Самарская область", 63},
                    {"Саратовская область", 64},
                    {"Сахалинская область", 65},
                    {"Свердловская область", 66},
                    {"Смоленская область", 67},
                    {"Тамбовская область", 68},
                    {"Тверская область", 69},
                    {"Томская область", 70},
                    {"Тульская область", 71},
                    {"Тюменская область", 72},
                    {"Ульяновская область", 73},
                    {"Челябинская область", 74},
                    {"Забайкальский край", 75},
                    {"Читинская область", 75},
                    {"Ярославская область", 76},
                    {"Москва", 77},
                    {"Санкт-Петербург", 78},
                    {"Еврейская автономная область", 79},
                    {"Ненецкий автономный округ", 83},
                    {"Ханты-Мансийский автономный округ-Югра", 86},
                    {"Чукотский автономный округ", 87},
                    {"Ямало-Ненецкий автономный округ", 89},
                };

            private static Hashtable CompetitionTypes = new Hashtable()
                {
                    {"Английский язык", 1},
                    {"Астрономия", 2},
                    {"Биология", 3},
                    {"География", 4},
                    {"Информатика", 5},
                    {"История", 6},
                    {"Литература", 7},
                    {"Математика", 8},
                    {"Немецкий язык", 9},
                    {"Обществознание", 10},
                    {"Право", 11},
                    {"Русский язык", 12},
                    {"Технология", 13},
                    {"Физика", 14},
                    {"Физическая культура", 15},
                    {"Французский язык", 16},
                    {"Химия", 17},
                    {"Экология", 18},
                    {"Экономика", 19},
                    {"Основы предпринимательской деятельности и потребительских знаний", 20},
                    {"Политехническая олимпиада", 21},                
                };

            protected internal override string GetStatusCode()
            {
                return "search";
            }

            private void WriteLog(string errorMessage, int? lineIndex)
            {
                using (FileStream stream = File.Open(LogFileName, FileMode.Append, FileAccess.Write,
                        FileShare.ReadWrite))
                using (StreamWriter writer = new StreamWriter(stream, Encoding.GetEncoding(1251)))
                    if (lineIndex != null)
                        writer.WriteLine(string.Format("{0:u}. Cтрока {2}: {1}", DateTime.Now, errorMessage, lineIndex));
                    else
                        writer.WriteLine(string.Format("{0:u}. {1}", DateTime.Now, errorMessage));
            }

            private void AddError(string errorMessage, int lineIndex)
            {
                WriteLog(errorMessage, lineIndex);
            }

            private int? GetRegionId(string regionName)
            {
                return (int?)Regions[regionName.Replace("г.", string.Empty).Trim()];
            }

            private int? GetCompetitionTypeId(string competitionTypeName)
            {
                return (int?)CompetitionTypes[competitionTypeName];
            }

            private string GetCityName(string cityName)
            {
                return cityName.Replace("\"", string.Empty).Trim();
            }

            private void AddCompetitionCertificate(SqlConnection connection, int? competitionTypeId, 
                    string lastName, string firstName, string patronymicName, string degree, 
                    int? regionId, string city, string school, string @class)
            {
                using (SqlCommand cmdInsert = connection.CreateCommand())
                {
                    SqlParameter[] addParameters = new SqlParameter[] 
                    {
                    new SqlParameter("@competitionTypeId", System.Data.SqlDbType.Int),
                    new SqlParameter("@lastName", System.Data.SqlDbType.NVarChar, 255),
                    new SqlParameter("@firstName", System.Data.SqlDbType.NVarChar, 255),
                    new SqlParameter("@patronymicName", System.Data.SqlDbType.NVarChar, 255),
                    new SqlParameter("@degree", System.Data.SqlDbType.NVarChar, 255),
                    new SqlParameter("@regionId", System.Data.SqlDbType.Int),
                    new SqlParameter("@city", System.Data.SqlDbType.NVarChar, 255),
                    new SqlParameter("@school", System.Data.SqlDbType.NVarChar, 255),
                    new SqlParameter("@class", System.Data.SqlDbType.NVarChar, 255),
                    new SqlParameter("@editorAccountId", System.Data.SqlDbType.BigInt),
                    new SqlParameter("@editorIp", System.Data.SqlDbType.NVarChar, 255),
                    };

                    cmdInsert.CommandText = InsertTempImportStatement;
                    cmdInsert.Parameters.AddRange(addParameters);

                    cmdInsert.Parameters["@competitionTypeId"].Value = competitionTypeId;
                    cmdInsert.Parameters["@lastName"].Value = lastName;
                    cmdInsert.Parameters["@firstName"].Value = firstName;
                    cmdInsert.Parameters["@patronymicName"].Value = patronymicName;
                    cmdInsert.Parameters["@degree"].Value = degree;
                    cmdInsert.Parameters["@regionId"].Value = regionId;
                    cmdInsert.Parameters["@city"].Value = city;
                    cmdInsert.Parameters["@school"].Value = school;
                    cmdInsert.Parameters["@class"].Value = @class;
                    cmdInsert.Parameters["@editorAccountId"].Value = EditorAccountId;
                    cmdInsert.Parameters["@editorIp"].Value = EditorIp;
                    cmdInsert.ExecuteNonQuery();
                }
            }

            private void BeginParsing(SqlConnection connection)
            {
                using (SqlCommand cmdCreate = connection.CreateCommand())
                {
                    cmdCreate.CommandText = CreateTempImportStatement;
                    cmdCreate.ExecuteNonQuery();
                }
            }

            private void EndParsing(SqlConnection connection)
            {
                if (connection.State != ConnectionState.Open)
                    return;
                using (SqlCommand cmdDrop = connection.CreateCommand())
                {
                    cmdDrop.CommandText = DropTempImportStatement;
                    cmdDrop.ExecuteNonQuery();
                }
            }

            private void CommitParsing(SqlConnection connection)
            {
                using (SqlCommand cmdCommit = connection.CreateCommand())
                {
                    cmdCommit.CommandType = CommandType.StoredProcedure;
                    cmdCommit.CommandText = CommitSPName;
                    cmdCommit.CommandTimeout = 0;
                    cmdCommit.ExecuteNonQuery();
                }
            }

            private void Parse(string fileName)
            {
                if (File.Exists(LogFileName))
                    File.Delete(LogFileName);

                using (FileStream sourceFileStream = File.Open(fileName, FileMode.Open, FileAccess.Read, 
                        FileShare.None))
                    using (StreamReader reader = new StreamReader(sourceFileStream, Encoding.GetEncoding(1251)))
                        using (SqlConnection connection = new SqlConnection(
                                global::FbsService.Properties.Settings.Default.FbsWebConnectionString))
                        {
                            connection.Open();
                            BeginParsing(connection);
                            try
                            {
                                string line;
                                int lineIndex = 0;
                                string errorMessage;

                                int? competitionTypeId = null;
                                string lastName = null;
                                string firstName = null;
                                string patronymicName = null;
                                string degree = null;
                                int? regionId = null;
                                string city = null;
                                string school = null;
                                string @class = null;

                                WriteLog("Начата обработка данных...", null);
                                while ((line = reader.ReadLine()) != null)
                                {
                                    line = line.Trim();
                                    lineIndex++;

                                    if (string.IsNullOrEmpty(line))
                                        continue;
                                    string[] parts = line.Split('#');
                                    errorMessage = null;

                                    if (parts.Length != 9)
                                        errorMessage = "Некорректное количество параметров";

                                    if (errorMessage == null)
                                    {
                                        degree = parts[0].Trim();
                                        if (string.IsNullOrEmpty(degree))
                                            errorMessage = "Не указана степень";
                                    }
                                    if (errorMessage == null)
                                        lastName = parts[1].Trim();
                                    if (errorMessage == null)
                                        firstName = parts[2].Trim();
                                    if (errorMessage == null)
                                        patronymicName = parts[3].Trim();
                                    if (errorMessage == null)
                                    {
                                        regionId = GetRegionId(parts[4].Trim());
                                        if (regionId == null)
                                            errorMessage = string.Format(
                                                    "Некорректное наименование региона \"{0}\"", parts[4]);
                                    }
                                    if (errorMessage == null)
                                    {
                                        city = GetCityName(parts[5].Trim());
                                        if (string.IsNullOrEmpty(city))
                                            errorMessage = "Не указан город";
                                    }
                                    if (errorMessage == null)
                                    {
                                        school = parts[6].Trim();
                                        if (string.IsNullOrEmpty(school))
                                            errorMessage = "Не указана школа";
                                    }
                                    if (errorMessage == null)
                                    {
                                        @class = parts[7].Trim();
                                        if (string.IsNullOrEmpty(@class))
                                            errorMessage = "Не указан класс";
                                    }
                                    if (errorMessage == null)
                                    {
                                        competitionTypeId = GetCompetitionTypeId(parts[8].Trim());
                                        if (competitionTypeId == null)
                                            errorMessage = string.Format(
                                                    "Некорректное наименование олимпиады \"{0}\"", parts[8]);
                                    }

                                    if (errorMessage != null)
                                    {
                                        AddError(errorMessage, lineIndex);
                                        continue;
                                    }

                                    AddCompetitionCertificate(connection, competitionTypeId, lastName, 
                                            firstName, patronymicName, degree, regionId, city, school, @class);
                                }
                                WriteLog("Данные обработаны успешно.", null);

                                CommitParsing(connection);
                            }
                            finally
                            {
                                EndParsing(connection);
                            }
                        }
            }

            protected internal override void Execute()
            {
                if (Directory.GetFiles(LoadingDirectory).Length == 0)
                    return;
                string fileName = Directory.GetFiles(LoadingDirectory)[0];
                Parse(fileName);
                File.Delete(fileName);
            }
        }

        protected override string GetTaskCode()
        {
            return "CompetitionCertificateImport";
        }

        protected internal override TaskStatus GetStatus(string code)
        {
            return new Search();
        }
    }
}
