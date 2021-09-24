using GVUZ.DAL.Dapper.Repository.Model.OlympicDiplomant;
using System;
using GVUZ.Data.Model;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web;
using System.Diagnostics;

namespace GVUZ.Web.ViewModels.OlympicDiplomant
{
    public class OlympicDiplomantImportViewModel
    {

        DataTable table = new DataTable();
        DataRow row = null;
        OlympicDiplomantRepository repository = new OlympicDiplomantRepository();
        int institutionID;

        public OlympicDiplomantImportViewModel()
        {
            Columns = new List<Column>();

            _debugInfo = new StringBuilder();
            _swTotal = new Stopwatch();
            _swFindOlympicTypeProfile = new Stopwatch();
            _swFindExistingDiplomant = new Stopwatch();
            _swGetDiplomantById = new Stopwatch();
            _swInsertDiplomant = new Stopwatch();
            _swUpdateDiplomant = new Stopwatch();
        }

        public OlympicDiplomantImportViewModel(int institutionID)
            : this()
        {
            this.institutionID = institutionID;
        }

        //-----------------------------------------------------------------------------------------------------------

        public class Column
        {
            public int Index { get; set; }
            public string Info { get; set; }
        }

        //-----------------------------------------------------------------------------------------------------------

        public void Add(string info)
        {
            Columns.Add(new Column { Index = Count, Info = info });
            ErrorCount++;
        }

        //-----------------------------------------------------------------------------------------------------------

        public string ErrorMessage { get; set; }
        public int Count { get; set; }
        public int ErrorCount { get; set; }
        public int InsertCount { get; set; }
        public int UpdateCount { get; set; }
        public List<Column> Columns { get; set; }

        public string DebugInfo { get { return _debugInfo.ToString(); } }

        private StringBuilder _debugInfo;

        private Stopwatch _swTotal;
        private Stopwatch _swFindOlympicTypeProfile;
        private Stopwatch _swFindExistingDiplomant;
        private Stopwatch _swGetDiplomantById;
        private Stopwatch _swInsertDiplomant;
        private Stopwatch _swUpdateDiplomant;

        //-----------------------------------------------------------------------------------------------------------

        public bool ReadFile(HttpPostedFileBase file)
        {
            using (var reader = new StreamReader(file.InputStream, Encoding.GetEncoding("windows-1251")))
            {
                bool first = true;
                string line = "";

                while ((line = reader.ReadLine()) != null)
                {
                    string[] vars = line.Split(';');

                    // колонки
                    if (first)
                    {
                        first = false;

                        try
                        {
                            foreach (var item in vars)
                                table.Columns.Add((new DataColumn(item) { }));
                        }
                        catch (Exception)
                        {
                            ErrorMessage = "Имена колонок в файле повторяются!";
                            return false;
                        }

                        if (!table.Columns.Contains("form_number") || !table.Columns.Contains("ending_date") ||
                            !table.Columns.Contains("result_level") || !table.Columns.Contains("last_name") ||
                            !table.Columns.Contains("first_name") || !table.Columns.Contains("birth_date") ||
                            !table.Columns.Contains("identity_document_type_id") || !table.Columns.Contains("Document_Number") ||
                            !table.Columns.Contains("olympiad_subject_name") || !table.Columns.Contains("olympiad_name") ||
                            !table.Columns.Contains("olympiad_year"))
                        {
                            ErrorMessage = "Не обнаружена строка с именами колонок!";
                            return false;
                        }
                    }
                    // значения полей
                    else
                    {
                        Count++;

                        try
                        {
                            if (vars.Count() != table.Columns.Count)
                            {
                                Add("Число столбцов не соответствует заголовку таблицы");
                                continue;
                            }
                            else
                            {
                                row = table.Rows.Add(vars);
                            }
                        }
                        catch (Exception x)
                        {
                            ErrorMessage = "Ошибка с чтением строк файла: " + x.Message;
                            return false;
                        }

                        _swTotal.Start();
                        PrepareRowForTable();
                        _swTotal.Stop();
                    }
                }
                reader.Close();
            }

            WriteStopwatchInfo(_swFindOlympicTypeProfile, "Поиск профиля, олимпиады");
            WriteStopwatchInfo(_swFindExistingDiplomant, "Поиск существующей записи");
            WriteStopwatchInfo(_swGetDiplomantById, "Получение существующей записи");
            WriteStopwatchInfo(_swInsertDiplomant, "Добавление записи");
            WriteStopwatchInfo(_swUpdateDiplomant, "Обновление записи");
            WriteStopwatchInfo(repository.SWSPFindOlympicDiplomantRVIPersons, "Поиск PersonId");

            WriteStopwatchInfo(_swTotal, "Всего");
            return true;
        }

        //-----------------------------------------------------------------------------------------------------------

        bool ParseInt64<T>(T target, Expression<Func<T, long?>> outExp, string column, bool empty = false)
        {
            if (table.Columns.Contains(column))
            {
                try
                {
                    string s = row[column].ToString().Trim();
                    if (empty && s == "")
                        return true;


                    var expr = (MemberExpression)outExp.Body;
                    var prop = (PropertyInfo)expr.Member;
                    prop.SetValue(target, Convert.ToInt64(row[column]), null);
                }
                catch (Exception)
                {
                    Add("Ошибка обработки колонки: " + column);
                    return false;
                }
            }
            return true;
        }

        //-----------------------------------------------------------------------------------------------------------

        bool ParseInt32<T>(T target, Expression<Func<T, int?>> outExp, string column, bool empty = false)
        {
            if (table.Columns.Contains(column))
            {
                try
                {
                    string s = row[column].ToString().Trim();
                    if (empty && s == "")
                        return true;


                    var expr = (MemberExpression)outExp.Body;
                    var prop = (PropertyInfo)expr.Member;
                    prop.SetValue(target, Convert.ToInt32(row[column]), null);
                }
                catch (Exception)
                {
                    Add("Ошибка обработки колонки: " + column);
                    return false;
                }
            }
            return true;
        }

        //-----------------------------------------------------------------------------------------------------------

        bool ParseInt32Key<T>(T target, Expression<Func<T, int>> outExp, string column, bool empty = false)
        {
            if (table.Columns.Contains(column))
            {
                try
                {
                    string s = row[column].ToString().Trim();
                    if (empty && s == "")
                        return true;

                    var expr = (MemberExpression)outExp.Body;
                    var prop = (PropertyInfo)expr.Member;
                    prop.SetValue(target, Convert.ToInt32(row[column]), null);
                }
                catch (Exception)
                {
                    Add("Ошибка обработки колонки: " + column);
                    return false;
                }
            }
            return true;
        }

        //-----------------------------------------------------------------------------------------------------------

        bool ParseShortKey<T>(T target, Expression<Func<T, short>> outExp, string column, bool empty = false)
        {
            if (table.Columns.Contains(column))
            {
                try
                {
                    string s = row[column].ToString().Trim();
                    if (empty && s == "")
                        return true;

                    var expr = (MemberExpression)outExp.Body;
                    var prop = (PropertyInfo)expr.Member;
                    prop.SetValue(target, Convert.ToInt16(row[column]), null);
                }
                catch (Exception)
                {
                    Add("Ошибка обработки колонки: " + column);
                    return false;
                }
            }
            return true;
        }

        //-----------------------------------------------------------------------------------------------------------

        bool ParseDateTime<T>(T target, Expression<Func<T, DateTime?>> outExp, string column, bool empty = false)
        {
            if (table.Columns.Contains(column))
            {
                try
                {
                    string s = row[column].ToString().Trim();
                    if (empty && s == "")
                        return true;

                    var expr = (MemberExpression)outExp.Body;
                    var prop = (PropertyInfo)expr.Member;
                    prop.SetValue(target, Convert.ToDateTime(row[column]), null);
                }
                catch (Exception)
                {
                    Add("Ошибка обработки колонки: " + column);
                    return false;
                }
            }
            return true;
        }

        //-----------------------------------------------------------------------------------------------------------

        bool ParseString<T>(T target, Expression<Func<T, string>> outExp, string column, bool empty = false)
        {
            if (table.Columns.Contains(column))
            {
                try
                {
                    string s = row[column].ToString().Trim();
                    if (empty && s == "")
                        return true;
                    if (!empty && s == "")
                    {
                        Add("Ошибка обработки колонки: " + column);
                        return false;
                    }

                    var expr = (MemberExpression)outExp.Body;
                    var prop = (PropertyInfo)expr.Member;
                    prop.SetValue(target, s, null);
                }
                catch (Exception)
                {
                    Add("Ошибка обработки колонки: " + column);
                    return false;
                }
            }
            return true;
        }

        //-----------------------------------------------------------------------------------------------------------

        bool ParseStringHard<T>(T target, Expression<Func<T, string>> outExp, string column, bool empty = false)
        {
            if (table.Columns.Contains(column))
            {
                try
                {
                    string s = row[column].ToString().Trim();
                    if (empty && s == "")
                    {
                        Add("Ошибка обработки колонки: " + column);
                        return false;
                    }

                    var expr = (MemberExpression)outExp.Body;
                    var prop = (PropertyInfo)expr.Member;
                    prop.SetValue(target, s, null);
                }
                catch (Exception)
                {
                    Add("Ошибка обработки колонки: " + column);
                    return false;
                }
            }
            return true;
        }

        bool CheckDocumentData(int typeId, string series, string number)
        {
            if (typeId != 1)
                return true;

            if ((String.IsNullOrEmpty(series)) || (series.Trim().Length != 4) || (!series.All(x => Char.IsDigit(x))))
            {
                Add("Ошибка обработки колонки: document_series");
                return false;
            }
            if ((String.IsNullOrEmpty(number)) || (number.Trim().Length != 6) || (!number.All(x => Char.IsDigit(x))))
            {
                Add("Ошибка обработки колонки: document_number");
                return false;
            }
            return true;
        }

        //-----------------------------------------------------------------------------------------------------------

        bool PrepareRowForTable()
        {
            var diplomant = new GVUZ.Data.Model.OlympicDiplomant();
            var diplomantDocument = new OlympicDiplomantDocument();
            var typeProfile = new OlympicTypeProfile();
            var profile = new OlympicProfile();
            var olympicType = new OlympicType();

            diplomant.OlympicDiplomantDocument = diplomantDocument;
            diplomant.OlympicTypeProfile = typeProfile;
            diplomant.OlympicTypeProfile.OlympicProfile = profile;
            diplomant.OlympicTypeProfile.OlympicType = olympicType;

            //-----------------------------------------------------------------------------------
            // читаем все обязательные колонки
            //-----------------------------------------------------------------------------------

            if (!ParseInt32(diplomant, x => x.FormNumber, "form_number")) return false;
            if (!ParseInt32(diplomant, x => x.EndingDate, "ending_date")) return false;
            if (!ParseShortKey(diplomant, x => x.ResultLevelID, "result_level")) return false;
            if (diplomant.ResultLevelID == 3)//Если передали "Победитель или призер" - заменяем на "Призер"
            {
                diplomant.ResultLevelID = 2;
            }
            if (!ParseString(diplomantDocument, x => x.LastName, "last_name")) return false;
            if (!ParseString(diplomantDocument, x => x.FirstName, "first_name")) return false;
            if (!ParseDateTime(diplomantDocument, x => x.BirthDate, "birth_date")) return false;
            if (!ParseInt32Key(diplomantDocument, x => x.IdentityDocumentTypeID, "identity_document_type_id")) return false;
            if (!ParseString(diplomantDocument, x => x.DocumentNumber, "document_number")) return false;
            if (!ParseString(profile, x => x.ProfileName, "olympiad_subject_name")) return false;
            if (!ParseString(olympicType, x => x.Name, "olympiad_name")) return false;
            if (!ParseInt32Key(olympicType, x => x.OlympicYear, "olympiad_year")) return false;

            // Номер диплома - необязательный, но нужен для поиска, поэтому читаем его тут 
            // (до FindOlympicDiplomant)
            if (table.Columns.Contains("diploma_number"))
                diplomant.DiplomaNumber = row["diploma_number"].ToString();

            //-----------------------------------------------------------------------------------
            // обязательно нужен OlympicTypeProfileID, но он в csv не приходит
            // нужно искать профиль олимпиады по наименованиям и году
            //-----------------------------------------------------------------------------------

            _swFindOlympicTypeProfile.Start();
            // ищем OlympicTypeID по наименованию и году
            try
            {
                diplomant.OlympicTypeProfile.OlympicTypeID =
                    repository.GetOlympicTypeByNameAndYear(
                        diplomant.OlympicTypeProfile.OlympicType.Name,
                        diplomant.OlympicTypeProfile.OlympicType.OlympicYear).OlympicID;
            }
            catch (Exception)
            {
                _swFindOlympicTypeProfile.Stop();
                Add("Не найдена олимпиада по наименованию и году");
                return false;
            }

            // ищем OlympicProfileID по наименованию
            try
            {
                diplomant.OlympicTypeProfile.OlympicProfileID =
                    repository.GetOlympicProfileByName(
                        diplomant.OlympicTypeProfile.OlympicProfile.ProfileName).OlympicProfileID;
            }
            catch (Exception)
            {
                _swFindOlympicTypeProfile.Stop();
                Add("Не найдено наименование профиля олимпиады");
                return false;
            }

            // ищем OlympicTypeProfileID по ранее найденным OlympicTypeID и OlympicProfileID
            try
            {
                diplomant.OlympicTypeProfileID =
                    repository.GetOlympicTypeProfileByTypeAndProfile(
                        diplomant.OlympicTypeProfile.OlympicTypeID,
                        diplomant.OlympicTypeProfile.OlympicProfileID,
                        institutionID).OlympicTypeProfileID;
            }
            catch (Exception)
            {
                _swFindOlympicTypeProfile.Stop();
                Add("Не найден профиль олимпиады");
                return false;
            }

            _swFindOlympicTypeProfile.Stop();

            //-----------------------------------------------------------------------------------
            // пытаемся найти призера в базе
            //-----------------------------------------------------------------------------------

            _swFindExistingDiplomant.Start();

            var id = repository.FindOlympicDiplomant(
                diplomant.OlympicDiplomantDocument.LastName,
                diplomant.OlympicDiplomantDocument.FirstName,
                diplomant.OlympicDiplomantDocument.IdentityDocumentTypeID,
                diplomant.OlympicDiplomantDocument.DocumentNumber,
                diplomant.OlympicTypeProfileID,
                diplomant.DiplomaNumber);

            _swFindExistingDiplomant.Stop();

            //-----------------------------------------------------------------------------------
            // призер не найден, создаем новую запись
            //-----------------------------------------------------------------------------------

            if (id == null)
            {
                //--------------------------------------
                // ищем и читаем необязательные колонки
                //--------------------------------------
                if (table.Columns.Contains("Middle_Name"))
                    diplomant.OlympicDiplomantDocument.MiddleName = row["Middle_Name"].ToString();
                if (table.Columns.Contains("Document_Series"))
                    diplomant.OlympicDiplomantDocument.DocumentSeries = row["Document_Series"].ToString();

                if (!CheckDocumentData(diplomant.OlympicDiplomantDocument.IdentityDocumentTypeID, diplomant.OlympicDiplomantDocument.DocumentSeries, diplomant.OlympicDiplomantDocument.DocumentNumber))
                    return false;

                if (table.Columns.Contains("Organization_Issue"))
                    diplomant.OlympicDiplomantDocument.OrganizationIssue = row["Organization_Issue"].ToString();
                if (table.Columns.Contains("school_name"))
                    diplomant.SchoolEgeName = row["school_name"].ToString();
                if (table.Columns.Contains("diploma_series"))
                    diplomant.DiplomaSeries = row["diploma_series"].ToString();

                //--------------------------------------------------
                // ищем и читаем необязательные колонки с конвертами
                //--------------------------------------------------

                if (!ParseDateTime(diplomant.OlympicDiplomantDocument, x => x.DateIssue, "date_issue", true)) return false;
                if (!ParseInt32(diplomant, x => x.SchoolRegionID, "school_region", true)) return false;
                if (!ParseInt64(diplomant, x => x.SchoolEgeCode, "school_code", true)) return false;
                if (!ParseDateTime(diplomant, x => x.DiplomaDateIssue, "diploma_date_issue", true)) return false;

                //-----------------------------------------------------------
                // нужно искать дополнительные документы с суффиксами _2..._N
                //-----------------------------------------------------------
                int n = 2;
                while (CheckColumns(n))
                {
                    var d = new OlympicDiplomantDocument();

                    if (!ParseInt32Key(d, x => x.IdentityDocumentTypeID, "identity_document_type_id_" + n))
                        return false;

                    if (!ParseStringHard(d, x => x.DocumentNumber, "document_number_" + n, true))
                        return false;

                    if (!ParseDateTime(d, x => x.DateIssue, "date_issue_" + n, true))
                        return false;

                    d.BirthDate = diplomant.OlympicDiplomantDocument.BirthDate; // т.к. не приходит в файле

                    if (table.Columns.Contains("last_name_" + n) && row["last_name_" + n].ToString().Trim() != "")
                        d.LastName = row["last_name_" + n].ToString().Trim();
                    else
                        d.LastName = diplomant.OlympicDiplomantDocument.LastName;

                    if (table.Columns.Contains("first_name_" + n) && row["first_name_" + n].ToString().Trim() != "")
                        d.FirstName = row["first_name_" + n].ToString().Trim();
                    else
                        d.FirstName = diplomant.OlympicDiplomantDocument.FirstName;

                    if (table.Columns.Contains("middle_name_" + n) && row["middle_name_" + n].ToString().Trim() != "")
                        d.MiddleName = row["middle_name_" + n].ToString().Trim();
                    else
                        d.MiddleName = diplomant.OlympicDiplomantDocument.MiddleName;

                    if (table.Columns.Contains("document_series_" + n))
                        d.DocumentSeries = row["document_series_" + n].ToString();

                    if (!CheckDocumentData(d.IdentityDocumentTypeID, d.DocumentSeries, d.DocumentNumber))
                        return false;

                    if (table.Columns.Contains("organization_issue_" + n))
                        d.OrganizationIssue = row["organization_issue_" + n].ToString();

                    if (diplomant.OlympicDiplomantDocumentCanceled == null)
                        diplomant.OlympicDiplomantDocumentCanceled = new List<OlympicDiplomantDocument>();

                    d.OlympicDiplomantID = diplomant.OlympicDiplomantID;

                    diplomant.OlympicDiplomantDocumentCanceled.Add(d);

                    n++;
                }

                //--------------------------
                // добавляем призера в базу
                //--------------------------
                _swInsertDiplomant.Start();
                try
                {
                    repository.InsertOlympicDiplomant(diplomant);
                    InsertCount++;
                }
                catch (Exception)
                {
                    Add("Ошибка добавления призера в базу");
                    return false;
                }
                finally
                {
                    _swInsertDiplomant.Stop();
                }
            }

            //-----------------------------------------------------------------------------------
            // призер найден, 
            // считываем и модифицируем уже имеющуюся по этому призеру информацию
            //-----------------------------------------------------------------------------------

            else
            {
                _swGetDiplomantById.Start();

                var old = repository.GetOlympicDiplomantById((long)id);

                _swGetDiplomantById.Stop();

                // обязательные
                old.FormNumber = diplomant.FormNumber;
                old.EndingDate = diplomant.EndingDate;
                old.ResultLevelID = diplomant.ResultLevelID;

                // необязательные
                if (table.Columns.Contains("Middle_Name"))
                    old.OlympicDiplomantDocument.MiddleName = row["Middle_Name"].ToString();
                if (table.Columns.Contains("Document_Series"))
                    old.OlympicDiplomantDocument.DocumentSeries = row["Document_Series"].ToString();

                if (!CheckDocumentData(old.OlympicDiplomantDocument.IdentityDocumentTypeID, old.OlympicDiplomantDocument.DocumentSeries, old.OlympicDiplomantDocument.DocumentNumber))
                    return false;

                if (table.Columns.Contains("Organization_Issue"))
                    old.OlympicDiplomantDocument.OrganizationIssue = row["Organization_Issue"].ToString();
                if (table.Columns.Contains("school_name"))
                    old.SchoolEgeName = row["school_name"].ToString();
                if (table.Columns.Contains("diploma_series"))
                    old.DiplomaSeries = row["diploma_series"].ToString();
                if (table.Columns.Contains("diploma_number"))
                    old.DiplomaNumber = row["diploma_number"].ToString();

                if (!ParseDateTime(old.OlympicDiplomantDocument, x => x.DateIssue, "date_issue", true)) return false;
                if (!ParseInt32(old, x => x.SchoolRegionID, "school_region", true)) return false;
                if (!ParseInt64(old, x => x.SchoolEgeCode, "school_code", true)) return false;
                if (!ParseDateTime(old, x => x.DiplomaDateIssue, "diploma_date_issue", true)) return false;

                //-----------------------------------------------------------
                // нужно искать дополнительные документы с суффиксами _2..._N
                //-----------------------------------------------------------

                int n = 2;
                while (CheckColumns(n))
                {
                    var d = new OlympicDiplomantDocument();

                    if (!ParseInt32Key(d, x => x.IdentityDocumentTypeID, "identity_document_type_id_" + n))
                        return false;

                    if (!ParseStringHard(d, x => x.DocumentNumber, "document_number_" + n, true))
                        return false;

                    if (!ParseDateTime(d, x => x.DateIssue, "date_issue_" + n, true))
                        return false;

                    d.BirthDate = old.OlympicDiplomantDocument.BirthDate; // т.к. не приходит в файле


                    if (table.Columns.Contains("last_name_" + n) && row["last_name_" + n].ToString().Trim() != "")
                        d.LastName = row["last_name_" + n].ToString().Trim();
                    else
                        d.LastName = old.OlympicDiplomantDocument.LastName;

                    if (table.Columns.Contains("first_name_" + n) && row["first_name_" + n].ToString().Trim() != "")
                        d.FirstName = row["first_name_" + n].ToString().Trim();
                    else
                        d.FirstName = old.OlympicDiplomantDocument.FirstName;

                    if (table.Columns.Contains("middle_name_" + n) && row["middle_name_" + n].ToString().Trim() != "")
                        d.MiddleName = row["middle_name_" + n].ToString().Trim();
                    else
                        d.MiddleName = old.OlympicDiplomantDocument.MiddleName;

                    if (table.Columns.Contains("document_series_" + n))
                        d.DocumentSeries = row["document_series_" + n].ToString();

                    if (!CheckDocumentData(d.IdentityDocumentTypeID, d.DocumentSeries, d.DocumentNumber))
                        return false;

                    if (table.Columns.Contains("organization_issue_" + n))
                        d.OrganizationIssue = row["organization_issue_" + n].ToString();

                    if (old.OlympicDiplomantDocumentCanceled == null)
                        old.OlympicDiplomantDocumentCanceled = new List<OlympicDiplomantDocument>();

                    d.OlympicDiplomantID = old.OlympicDiplomantID;

                    // поиск уже имеющихся недействующих документов с такими реквизитами
                    var findDoc = old.OlympicDiplomantDocumentCanceled.
                        Where(x => x.DocumentNumber == d.DocumentNumber &&
                                   x.IdentityDocumentTypeID == d.IdentityDocumentTypeID &&
                                   x.LastName == d.LastName &&
                                   x.FirstName == d.FirstName &&
                                   x.MiddleName == d.MiddleName).
                        FirstOrDefault();

                    // найден документ введенный ранее, заменяем его на новый, тем самым обновляя инфу
                    // метод корректировки обработает это корректно
                    if (findDoc != null)
                        old.OlympicDiplomantDocumentCanceled.Remove(findDoc);

                    old.OlympicDiplomantDocumentCanceled.Add(d);

                    n++;
                }

                //------------------------------
                // модифицируем призера в базе
                //------------------------------                
                _swUpdateDiplomant.Start();
                try
                {
                    repository.UpdateOlympicDiplomant(old);
                    UpdateCount++;
                }
                catch (Exception)
                {
                    Add("Ошибка модификации призера в базе");
                    return false;
                }
                finally
                {
                    _swUpdateDiplomant.Stop();
                }
            }

            return true;
        }

        //-----------------------------------------------------------------------------------------------------------

        bool CheckColumns(int n)
        {
            if (
                ((table.Columns.Contains("identity_document_type_id_" + n) && row["identity_document_type_id_" + n].ToString().Trim() != "")) ||
                ((table.Columns.Contains("document_number_" + n) && row["document_number_" + n].ToString().Trim() != "")) ||
                ((table.Columns.Contains("last_name_" + n) && row["last_name_" + n].ToString().Trim() != "")) ||
                ((table.Columns.Contains("first_name_" + n) && row["first_name_" + n].ToString().Trim() != "")) ||
                ((table.Columns.Contains("middle_name_" + n) && row["middle_name_" + n].ToString().Trim() != "")) ||
                ((table.Columns.Contains("document_series_" + n) && row["document_series_" + n].ToString().Trim() != "")) ||
                ((table.Columns.Contains("organization_issue_" + n) && row["organization_issue_" + n].ToString().Trim() != "")) ||
                ((table.Columns.Contains("date_issue_" + n) && row["date_issue_" + n].ToString().Trim() != "")))
                return true;
            else
                return false;
        }

        //-----------------------------------------------------------------------------------------------------------


        private void WriteStopwatchInfo(Stopwatch sw, string name)
        {
            _debugInfo.AppendFormat("{0} = {1}мс", name, sw.ElapsedMilliseconds).AppendLine();
        }
    }
}