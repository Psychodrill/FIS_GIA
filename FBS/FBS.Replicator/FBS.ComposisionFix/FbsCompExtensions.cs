using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Net;
using FBS.Common;
using System.Runtime.ConstrainedExecution;


namespace FBS.CompositionsPathGenerator
{
    public class NetworkConnection : IDisposable
    {
        string _networkName;

        public NetworkConnection(string networkName,
            NetworkCredential credentials)
        {
            _networkName = networkName;

            var netResource = new NetResource()
            {
                Scope = ResourceScope.GlobalNetwork,
                ResourceType = ResourceType.Disk,
                DisplayType = ResourceDisplaytype.Share,
                RemoteName = networkName
            };

            var userName = string.IsNullOrEmpty(credentials.Domain)
                ? credentials.UserName
                : string.Format(@"{0}\{1}", credentials.Domain, credentials.UserName);

            var result = WNetAddConnection2(
                netResource,
                credentials.Password,
                userName,
                0);

            if (result != 0)
                throw new Win32Exception(result, String.Format("Ошибка доступа к сетевому ресурсу {0} (пользователь: {1}; код ошибки: {2})", _networkName, userName, result));
        }

        ~NetworkConnection()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            WNetCancelConnection2(_networkName, 0, true);
        }

        [DllImport("mpr.dll")]
        private static extern int WNetAddConnection2(NetResource netResource,
            string password, string username, int flags);

        [DllImport("mpr.dll")]
        private static extern int WNetCancelConnection2(string name, int flags,
            bool force);
    }

    [StructLayout(LayoutKind.Sequential)]
    public class NetResource
    {
        public ResourceScope Scope;
        public ResourceType ResourceType;
        public ResourceDisplaytype DisplayType;
        public int Usage;
        public string LocalName;
        public string RemoteName;
        public string Comment;
        public string Provider;
    }

    public enum ResourceScope : int
    {
        Connected = 1,
        GlobalNetwork,
        Remembered,
        Recent,
        Context
    };

    public enum ResourceType : int
    {
        Any = 0,
        Disk = 1,
        Print = 2,
        Reserved = 8,
    }

    public enum ResourceDisplaytype : int
    {
        Generic = 0x0,
        Domain = 0x01,
        Server = 0x02,
        Share = 0x03,
        File = 0x04,
        Group = 0x05,
        Network = 0x06,
        Root = 0x07,
        Shareadmin = 0x08,
        Directory = 0x09,
        Tree = 0x0a,
        Ndscontainer = 0x0b
    }

    public class NetworkIO
    {
        private string _user;
        private string _password;
        public NetworkIO(string user, string password)
        {
            _user = user;
            _password = password;
        }

        public IEnumerable<string> EnumerateDirectories(string path)
        {
            if (String.IsNullOrEmpty(_user))
                return Directory.EnumerateDirectories(path);
            else
            {
                using (new NetworkConnection(path, new NetworkCredential(_user, _password)))
                {
                    return Directory.EnumerateDirectories(path);
                }
            }
        }

        public IEnumerable<string> EnumerateFiles(string path)
        {
            if (String.IsNullOrEmpty(_user))
                return Directory.EnumerateFiles(path);
            else
            {
                using (new NetworkConnection(path, new NetworkCredential(_user, _password)))
                {
                    return Directory.EnumerateFiles(path);
                }
            }
        }

        public bool FileExists(string path)
        {
            if (String.IsNullOrEmpty(_user))
                return File.Exists(path);
            else
            {
                string directory = Path.GetDirectoryName(path);
                using (new NetworkConnection(directory, new NetworkCredential(_user, _password)))
                {
                    return File.Exists(path);
                }
            }
        }

        public string[] ReadTestLinesFile(string path)
        {
            if (String.IsNullOrEmpty(_user))
                return File.ReadAllLines(path);
            else
            {
                string directory = Path.GetDirectoryName(path);
                using (new NetworkConnection(directory, new NetworkCredential(_user, _password)))
                {
                    return File.ReadAllLines(path);
                }
            }
        }

        public string ReadTextFile(string path)
        {
            if (String.IsNullOrEmpty(_user))
                return File.ReadAllText(path);
            else
            {
                string directory = Path.GetDirectoryName(path);
                using (new NetworkConnection(directory, new NetworkCredential(_user, _password)))
                {
                    return File.ReadAllText(path);
                }
            }
        }
    }


    public class FastDataReader
    {
        public FastDataReader(IDataReader reader)
        {
            _reader = reader;
            _ordinals = new Dictionary<string, int>();

            for (int i = 0; i < _reader.FieldCount; i++)
            {
                _ordinals.Add(reader.GetName(i), i);
            }
        }

        private readonly IDataReader _reader;
        private readonly Dictionary<string, int> _ordinals;

        public object GetObject(string columnName)
        {
            return _reader.GetValue(GetOrdinal(columnName));
        }

        public bool IsNull(string columnName)
        {
            return _reader.IsDBNull(GetOrdinal(columnName));
        }

        private int GetOrdinal(string columnName)
        {
            if (!_ordinals.ContainsKey(columnName))
                return _reader.GetOrdinal(columnName);
            else
                return _ordinals[columnName];
        }
    }

    public static class DataHelper
    {


        public static bool? GetBool(FastDataReader reader, string columnName)
        {
            bool? result;
            if (IsNull(reader, columnName))
            {
                result = null;
            }
            else
            {
                result = Convert.ToBoolean(reader.GetObject(columnName));
            }
            return result;
        }

        public static int? GetInt(FastDataReader reader, string columnName)
        {
            int? result;
            if (IsNull(reader, columnName))
            {
                result = null;
            }
            else
            {
                result = Convert.ToInt32(reader.GetObject(columnName));
            }
            return result;
        }

        public static short? GetShort(FastDataReader reader, string columnName)
        {
            short? result;
            if (IsNull(reader, columnName))
            {
                result = null;
            }
            else
            {
                result = Convert.ToInt16(reader.GetObject(columnName));
            }
            return result;
        }

        public static byte? GetByte(FastDataReader reader, string columnName)
        {
            byte? result;
            if (IsNull(reader, columnName))
            {
                result = null;
            }
            else
            {
                object obj = reader.GetObject(columnName);
                if (obj is byte)
                {
                    result = (byte)obj;
                }
                else if (obj is short)
                {
                    result = (byte)(short)obj;
                }
                else if (obj is int)
                {
                    result = (byte)(int)obj;
                }
                else if (obj is long)
                {
                    result = (byte)(long)obj;
                }
                else
                {
                    result = Convert.ToByte(reader.GetObject(columnName));
                }
            }
            return result;
        }

        public static Guid? GetGuid(FastDataReader reader, string columnName)
        {
            Guid? result;
            if (IsNull(reader, columnName))
            {
                result = null;
            }
            else
            {
                object obj = reader.GetObject(columnName);
                if (obj is Guid)
                {
                    result = (Guid)obj;
                }
                else
                {
                    result = new Guid(obj.ToString());
                }
            }
            return result;
        }

        public static DateTime? GetDateTime(FastDataReader reader, string columnName)
        {
            DateTime? result;
            if (IsNull(reader, columnName))
            {
                result = null;
            }
            else
            {
                object obj = reader.GetObject(columnName);
                if (obj is DateTime)
                {
                    result = (DateTime)obj;
                }
                else
                {
                    string str = obj.ToString();
                    DateTime temp;
                    if (DateTime.TryParse(str, out temp))
                    {
                        result = temp;
                    }
                    else if (DateTime.TryParseExact(str, "yyyy.MM.dd", System.Globalization.CultureInfo.InvariantCulture,
                        System.Globalization.DateTimeStyles.None, out temp))
                    {
                        result = temp;
                    }
                    else
                    {
                        result = null;
                    }
                }
            }
            return result;
        }

        public static string GetString(FastDataReader reader, string columnName)
        {
            string result;
            if (IsNull(reader, columnName))
            {
                result = null;
            }
            else
            {
                result = reader.GetObject(columnName).ToString();
            }
            return result;
        }

        private static bool IsNull(FastDataReader reader, string columnName)
        {
            bool result = reader.IsNull(columnName);
            return result;
        }

        public static byte[] StringToBytes(string stringValue)
        {
            if (stringValue == null)
                return null;
            return Encoding.UTF8.GetBytes(stringValue);
        }

        public static string BytesToString(byte[] bytes)
        {
            if (bytes == null)
                return null;
            return Encoding.UTF8.GetString(bytes);
        }

        public static object ReplaceNullToDBNull(object value)
        {
            if (value == null)
                return DBNull.Value;
            return value;
        }

        public static string NormalizeString(string value, bool removeWhitespaces)
        {
            if (value == null)
            {
                value = String.Empty;
            }
            if (removeWhitespaces)
            {
                value = value.Replace(" ", "");
            }
            return value.ToUpper().Replace("Ё", "Е").Replace("Й", "И");
        }
    }

    public class ERBDCompositionInfo
    {
        public ERBDCompositionInfo(Guid participantId, byte pagesCount)
        {
            ParticipantId = participantId;
            PagesCount = pagesCount;
        }

        public ERBDCompositionInfo(string infoRow)
        {
            if ((!infoRow.Contains("_")) && (!infoRow.Contains(":")))
            {
                Parsed = false;
            }
            else
            {
                string barcode = infoRow.Split('_')[0];
                Barcode = DataHelper.StringToBytes(barcode);
                byte pagesCount;
                if (Byte.TryParse(infoRow.Split(':').Last(), out pagesCount))
                {
                    PagesCount = pagesCount;
                    Parsed = true;
                }
                else
                {
                    Parsed = false;
                }
            }
        }

        public readonly Guid? ParticipantId;

        public readonly bool Parsed;
        public readonly byte[] Barcode;
        public string BarcodeStr { get { return DataHelper.BytesToString(Barcode); } }

        public readonly byte PagesCount;
    }

    public static class Connections
    {
        private static string FBSConnectionString
        {
            get
            {
                if (ConfigurationManager.ConnectionStrings["FBS"] != null)
                    return ConfigurationManager.ConnectionStrings["FBS"].ConnectionString;
                return null;
            }
        }

        private static string ERBDConnectionString
        {
            get
            {
                if (ConfigurationManager.ConnectionStrings["ERBD"] != null)
                    return ConfigurationManager.ConnectionStrings["ERBD"].ConnectionString;
                return null;
            }
        }

        private static string GVUZConnectionString
        {
            get
            {
                if (ConfigurationManager.ConnectionStrings["GVUZ"] != null)
                    return ConfigurationManager.ConnectionStrings["GVUZ"].ConnectionString;
                return null;
            }
        }

        public static string CompositionsStaticPath2015
        {
            get
            {
                string result = ConfigurationManager.AppSettings["CompositionsStaticPath2015"];
                if (!String.IsNullOrEmpty(result))
                {
                    result = result.TrimEnd('\\', '/');
                }
                return result;
            }
        }

        public static string CompositionsDirectoryUser2015
        {
            get
            {
                return ConfigurationManager.AppSettings["CompositionsDirectoryUser2015"];
            }
        }

        public static string CompositionsDirectoryPassword2015
        {
            get
            {
                return ConfigurationManager.AppSettings["CompositionsDirectoryPassword2015"];
            }
        }

        public static string CompositionsStaticPath2016Plus
        {
            get
            {
                string result = ConfigurationManager.AppSettings["CompositionsStaticPath2016Plus"];
                if (!String.IsNullOrEmpty(result))
                {
                    result = result.TrimEnd('\\', '/');
                }
                return result;
            }
        }

        public static string CompositionsDirectoryUser2016Plus
        {
            get
            {
                return ConfigurationManager.AppSettings["CompositionsDirectoryUser2016Plus"];
            }
        }

        public static string CompositionsDirectoryPassword2016Plus
        {
            get
            {
                return ConfigurationManager.AppSettings["CompositionsDirectoryPassword2016Plus"];
            }
        }

        public static string CompositionsPagesCountPath2016Plus
        {
            get
            {
                return ConfigurationManager.AppSettings["CompositionsPagesCountPath2016Plus"];
            }
        }
        public static string CompositionsPagesCountPath2015Plus
        {
            get
            {
                return ConfigurationManager.AppSettings["CompositionsPagesCountPath2015Plus"];
            }
        }

        public static SqlConnection CreateFBSConnection()
        {
            if (String.IsNullOrEmpty(FBSConnectionString))
                return null;
            SqlConnection result = new SqlConnection(FBSConnectionString);
            return result;
        }

        public static SqlConnection CreateERBDConnection()
        {
            if (String.IsNullOrEmpty(ERBDConnectionString))
                return null;
            SqlConnection result = new SqlConnection(ERBDConnectionString);
            return result;
        }

        public static SqlConnection CreateGVUZConnection()
        {
            if (String.IsNullOrEmpty(GVUZConnectionString))
                return null;
            SqlConnection result = new SqlConnection(GVUZConnectionString);
            return result;
        }

        public static bool TryConnectToERBD(out string errorMessage)
        {
            return TryConnect(ERBDConnectionString, out errorMessage);
        }

        public static bool TryConnectToFBS(out string errorMessage)
        {
            return TryConnect(FBSConnectionString, out errorMessage);
        }

        public static bool TryConnectToGVUZ(out string errorMessage)
        {
            return TryConnect(GVUZConnectionString, out errorMessage);
        }

        private static bool TryConnect(string connectionString, out string errorMessage)
        {
            if (String.IsNullOrEmpty(connectionString))
            {
                errorMessage = "Не определена строка соединения";
                return false;
            }
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                errorMessage = null;
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
            finally
            {
                if (connection != null)
                {
                    connection.Dispose();
                }
            }
        }

        public static bool TryOpen2015Directory(out string errorMessage)
        {
            return TryOpenDirectory(CompositionsStaticPath2015, CompositionsDirectoryUser2015, CompositionsDirectoryPassword2015, out errorMessage);
        }

        public static bool TryOpen2016PlusDirectory(out string errorMessage)
        {
            return TryOpenDirectory(CompositionsStaticPath2016Plus, CompositionsDirectoryUser2016Plus, CompositionsDirectoryPassword2016Plus, out errorMessage);
        }

        public static bool CheckPagesCountExists(out string errorMessage)
        {
            if (String.IsNullOrEmpty(CompositionsPagesCountPath2016Plus))
            {
                errorMessage = "Не определен путь к каталогу";
                return false;
            }
            if (!Directory.Exists(CompositionsPagesCountPath2016Plus))
            {
                errorMessage = "Каталог не существует";
                return false;
            }
            errorMessage = null;
            return true;
        }

        private static bool TryOpenDirectory(string path, string user, string password, out string errorMessage)
        {
            if (String.IsNullOrEmpty(path))
            {
                errorMessage = "Не определен путь к каталогу";
                return false;
            }
            try
            {
                NetworkIO networkIO = new NetworkIO(user, password);
                IEnumerable<string> directories = networkIO.EnumerateDirectories(path);
                if (!directories.Any())
                    throw new Exception("Вложенные каталоги не найдены");

                errorMessage = null;
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += " - " + ex.InnerException.Message;
                }
                return false;
            }
        }
    }

    public class CompositionsHelper
    {
        public const byte OKStatus = 2;
        private const int MessageSize = 10000;

        public static Dictionary<string, ERBDCompositionInfo> GetAllCompositionInfos(bool for2015, bool for2016)
        {
            Dictionary<string, ERBDCompositionInfo> result = new Dictionary<string, ERBDCompositionInfo>();
           
            if (for2016)
            {
                List<PagesCountFile> pagesCountFiles = new List<PagesCountFile>();
                if (!string.IsNullOrEmpty(/*@"\\10.0.3.5\Forms\20"*/ Connections.CompositionsPagesCountPath2016Plus))
                {
                    foreach (string pagesCountFilePath in Directory.EnumerateFiles(Connections.CompositionsPagesCountPath2016Plus, "*.txt", SearchOption.AllDirectories))
                    {
                        double len = new FileInfo(pagesCountFilePath).Length;
                        if (len > 2097152)
                            continue;
                        pagesCountFiles.Add(new PagesCountFile(File.ReadAllLines(pagesCountFilePath)));
                    }

                    foreach (PagesCountFile pagesCountFile in pagesCountFiles)
                    {
                        foreach (string line in pagesCountFile.Content)
                        {
                            ERBDCompositionInfo compositionInfo = new ERBDCompositionInfo(line);
                            if (!compositionInfo.Parsed)
                                continue;
                            string barcode = compositionInfo.BarcodeStr;
                            if (!result.ContainsKey(barcode))
                            {
                                result.Add(barcode, compositionInfo);
                                //Logger.WriteLine(String.Format("Сочинение : Barcode {0} , List cout {}", barcode, compositionInfo.PagesCount));
                            }
                        }
                    }

                }
            }
            return result;
        }


    }
    public class PagesCountFile
    {
        public PagesCountFile(string[] content)
        {
            Content = content;
        }
        public string[] Content { get; private set; }
    }
    public static class FbsCompExtensions
    {

        public static string GetMissingPaths()
        {
            string sql = @"
select  app.ApplicationID, app.ApplicationNumber, ent.EntrantID ,
				ed.DocumentSeries, ed.DocumentNumber ,res1.UseYear,res1.REGION,
				res1.Mark,res1.SubjectCode,res1.SubjectName,res1.[Surname], res1.[Name],res1.SecondName,
				res1.ParticipantCode,res1.PersonId as PID, res1.ExamDate, res1.CompositionBarcode, res1.CompositionPagesCount, res1.CompositionPaths
		from dbo.[Application] as app WITH(NOLOCK)
		inner join dbo.Entrant as ent 
			on ent.EntrantID = app.EntrantID
		inner join dbo.EntrantDocument ed WITH(NOLOCK)
			on ed.EntrantID = ent.EntrantID and 
				ed.DocumentNumber is not null and 
				ed.DocumentSeries is not null 
				--LEN (ed.DocumentSeries) = 4 and ( ed.DocumentSeries like '%[^0-9]%')
				cross apply (
					SELECT marks.CertificateMarkID, marks.UseYear,marks.REGION,marks.CertificateFK,
					certificates.TypographicNumber,certificates.Cancelled,marks.Mark,marks.AppealStatusID,subjects.SubjectCode,subjects.SubjectName,
					resultStatuses.GlobalStatusID,participants.ParticipantID,participants.Surname,participants.Name,participants.SecondName,
					participants.DocumentSeries,participants.DocumentNumber, crtts.LicenseNumber, crtts.CertificateID,
					participants.ParticipantCode, participants.CreateDate ,participants.ImportUpdateDate, participants.UpdateDate, 
					marks.CompositionPaths, participants.PersonId, marks.ExamDate, marks.CompositionBarcode,marks.CompositionPagesCount
					FROM fbs.prn.CertificatesMarks marks WITH(NOLOCK)
					INNER JOIN fbs.rbd.Participants participants WITH(NOLOCK)
						ON	(marks.ParticipantFK = participants.ParticipantID
							AND marks.UseYear = participants.UseYear
							AND marks.REGION = participants.REGION)                     
					INNER JOIN fbs.dat.Subjects subjects  WITH(NOLOCK)
							ON marks.SubjectCode = subjects.SubjectCode
					INNER JOIN fbs.rbdc.ResultStatuses resultStatuses WITH(NOLOCK)
							ON marks.ProcessCondition = resultStatuses.StatusID
					LEFT JOIN fbs.prn.Certificates certificates WITH(NOLOCK)
						ON	certificates.CertificateID = marks.CertificateFK
							AND certificates.UseYear = marks.UseYear
							AND certificates.REGION = marks.REGION
					left join fbs.prn.CertificatesB crtts WITH (NOLOCK)
						on crtts.ParticipantFK = participants.ParticipantID
					where participants.PersonId = ent.PersonId and  marks.UseYear > 2016 	  
					) as res1 
		where	
				app.InstitutionID = @inst_id and 
				YEAR(app.[CreatedDate]) = 2021 and 
				res1.SubjectCode = 20 and 
				res1.CompositionPaths is  null";
            return sql;
        }
       
        public static string GetErbdCompositionsInfo()
        {
            string sql = @"select	p.DocumentNumber, p.DocumentSeries, p.DocumentNumber, p.Surname, p.[Name], p.SecondName,
		ht.UseYear, ht.SubjectCode, subj.SubjectName,  ht.Mark , ht.ExamDate, ht.TestTypeID,
		p.ParticipantCode, p.ParticipantID, p.REGION, packages.ProjectName, sheets.ProjectBatchID, packages.ExamDate
from  [ERBD_2015].rbd.Participants as p with (nolock)
left join [ERBD_2015].res.HumanTests ht on  ht.ParticipantFK = p.ParticipantID
left join [ERBD_2015].dat.Subjects subj on subj.SubjectCode = ht.SubjectCode
inner join [ERBD_2015].sht.Sheets_R sheets on ht.ParticipantFK = sheets.ParticipantID
inner join [ERBD_2015].sht.Packages packages on sheets.PackageFK = packages.PackageID 
where p.ParticipantCode  in ( '938373933330' ) and ht.SubjectCode = 20";
            return sql;
        }


    }
}
