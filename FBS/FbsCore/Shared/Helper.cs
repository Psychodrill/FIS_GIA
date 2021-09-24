using System.Text.RegularExpressions;

namespace Fbs.Core.Shared
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
            return Regex.Replace(Regex.Replace(input.Trim(), "\\s+", " "), " ?- ?", "-");
        }
    }
}
