using System;
using System.Text.RegularExpressions;

namespace FbsService
{
    public static class Helper
    {
        /// <summary>
        /// Удаление из строки пробелов по краям; конвертация многих пробелов к одному, недопущение обрамляющих пробелов у дефисов
        /// Например: "    Абдуллина - Билялетдинов    " -> "Абдуллина-Билялетдинов"
        /// Например: "    12  -12345678 -34    " -> "12-12345678-34"
        /// См. также: StringUnitTest.cs
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string FullTrim(this string input)
        {
            return Regex.Replace(Regex.Replace(input.Trim(), @"\s+", " "), @" ?- ?", "-");
        }

        /// <summary>
        /// Преобразует строку в DBNull.Value, если она пустая
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static object ToDbFriendlyString(this string input)
        {
            return string.IsNullOrEmpty(input) ? (object)DBNull.Value : input;
        }
    }
}
