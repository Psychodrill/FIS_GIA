using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OlympicImport.Services
{
    public static class OlympicsImportSchema
    {
        /// <summary>
        /// уникальный ID участника в базе РСОШ
        /// </summary>
        public const string Id = "id";

        /// <summary>
        /// ParticipantId - может быть более одной записи - для ФБС
        /// </summary>
        public const string EgeId = "egeid";

        /// <summary>
        /// закодирована информация об олимпиаде и предмете 
        /// </summary>
        public const string CodeName = "code_name";

        /// <summary>
        /// фамилия 
        /// </summary>
        public const string LastName = "last_name";

        /// <summary>
        /// имя 
        /// </summary>
        public const string FirstName = "first_name";

        /// <summary>
        /// отчество 
        /// </summary>
        public const string MiddleName = "middle_name";

        /// <summary>
        /// дата рождения 
        /// </summary>
        public const string BirthDate = "birth_date";

        /// <summary>
        /// регион ОУ
        /// </summary>
        public const string SchoolRegion = "school_region";

        /// <summary>
        /// код школы в системе ЕГЭ
        /// </summary>
        public const string SchoolEgeCode = "school_ege_code";

        /// <summary>
        /// название школы
        /// </summary>
        public const string SchoolEgeName = "school_ege_name";

        /// <summary>
        /// класс (может быть только 11)
        /// </summary>
        public const string FormNumber = "form_number";

        /// <summary>
        /// название олимпиады 
        /// </summary>
        public const string OlympiadName = "olympiad_name";

        /// <summary>
        /// номер олимпиады в перечне (в диапазоне 1…100) 
        /// </summary>
        public const string OlympiadNumber = "olympiad_number";

        /// <summary>
        /// уровень олимпиады (в диапазоне 1…3) 
        /// </summary>
        public const string OlympiadLevel = "olympiad_level";

        /// <summary>
        /// комплекс предметов (строка) 
        /// </summary>
        public const string OlympiadSubjectName = "olympiad_subject_name";

        /// <summary>
        /// профильные общеобразовательные предметы 
        /// </summary>
        public const string OlympiadSubjectProfileName = "olympiad_subject_profile_name";

        /// <summary>
        /// Номер электронной копии диплома (код подтверждения (включает в себя в том числе и ID)
        /// </summary>
        public const string RegCode = "reg_code";

        /// <summary>
        /// степень диплома (в интервале 1…3) 
        /// </summary>
        public const string ResultLevel = "result_level";

        public static DateTime? AsDate(this string dateString)
        {
            if (!string.IsNullOrEmpty(dateString))
            {
                DateTime date;
                if (DateTime.TryParse(dateString, out date))
                {
                    return date;    
                }
            }

            return null;
        }

        public static readonly char[] CommaSeparator = new char[]{','};
        public static readonly char[] SpaceSeparator = new char[]{' '};

        public static ICollection<Guid> AsGuidCollection(this string guidString)
        {
            HashSet<Guid> results = new HashSet<Guid>();

            if (!string.IsNullOrEmpty(guidString))
            {
                string[] guids = guidString.Split(CommaSeparator, StringSplitOptions.RemoveEmptyEntries);

                foreach (string sGuid in guids)
                {
                    Guid guid = new Guid(sGuid.Trim());
                    results.Add(guid);
                }
            }

            return results;
        }

        public static int AsInt(this string strValue)
        {
            int result;

            if (Int32.TryParse(strValue, out result))
            {
                return result;
            }

            return 0;
        }

        public static int? AsNullableInt(this string strValue)
        {
            if (!string.IsNullOrEmpty(strValue))
            {
                int result;
                if (Int32.TryParse(strValue, out result))
                {
                    return result;
                }
            }

            return null;
        }

        public static long AsLong(this string strValue)
        {
            long result;

            if (Int64.TryParse(strValue, out result))
            {
                return result;
            }

            return 0;
        }

        public static long? AsNullableLong(this string strValue)
        {
            if (!string.IsNullOrEmpty(strValue))
            {
                long result;
                if (Int64.TryParse(strValue, out result))
                {
                    return result;
                }
            }

            return null;
        }

        public static IEnumerable<string> AsDistinctSubjectNames(this string allRecords)
        {
            if (!string.IsNullOrEmpty(allRecords))
            {
                if (allRecords.Contains(")") || allRecords.Contains(":"))
                {
                    return new[]{allRecords.Trim().CapitalizeFirst()};
                }
                return allRecords.Split(CommaSeparator, StringSplitOptions.RemoveEmptyEntries)
                                 .Select(s => CapitalizeFirst(s.Trim().Replace("  ", " ").ToLower()))
                                 .Where(s => !string.IsNullOrEmpty(s)).Distinct();
            }

            return Enumerable.Empty<string>();
        }

        public static string CapitalizeFirst(this string str)
        {
            if (string.IsNullOrEmpty(str) || str.Length == 1)
            {
                return str;
            }

            string first = new string(str[0], 1).ToUpper();
            StringBuilder sb = new StringBuilder(first);
            sb.Append(str.ToCharArray().Skip(1).ToArray());
            return sb.ToString();
        }
    }
}