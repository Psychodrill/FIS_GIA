using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fbs.Web.Helpers
{
    public static class SubjectsHelper
    {
        public const string BoolMarkPassedText = "Зачет";
        public const string BoolMarkNotPassedText = "Незачет";

        /// <summary>
        /// Результат по предмету принимает только значения "зачет" и "незачет"
        /// </summary>
        /// <param name="subjectCode"></param>
        /// <returns></returns>
        public static bool SubjectHasBoolMark(int subjectCode)
        {
            return SubjectHasBoolMark(subjectCode.ToString());
        }

        /// <summary>
        /// Результат по предмету принимает только значения "зачет" и "незачет"
        /// </summary>
        /// <param name="subjectCode"></param>
        /// <returns></returns>
        public static bool SubjectHasBoolMark(string subjectCode)
        {
            if (String.IsNullOrEmpty(subjectCode))
                return false;
            return (subjectCode == "20" || subjectCode == "21");//Сочинение или изложение
        }

        /// <summary>
        /// Конвертация результата по предмету в значение "зачет" или "незачет"
        /// </summary>
        /// <param name="mark"></param>
        /// <returns></returns>
        public static string BoolMarkToText(int mark)
        {
            return (mark == 0) ? BoolMarkNotPassedText : BoolMarkPassedText;
        }

        /// <summary>
        /// Конвертация результата по предмету из значений "зачет" или "незачет" в числовое значение (1 или 0 соответственно)
        /// </summary>
        /// <param name="mark"></param>
        /// <returns></returns>
        public static int BoolMarkFromText(string text)
        {
            return (IsBoolMarkPassedText(text)) ? 1 : 0;
        }

        /// <summary>
        /// Проверка, что текстовое значение может быть преобразовано в числовое значение (1 или 0)
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsBoolMarkValue(string text)
        {
            return (text == "1" || text == "зачет" || text == "зачёт" || text == "0" || text == "незачет" || text == "незачёт");
        }

        private static bool IsBoolMarkPassedText(string text)
        {
            if (String.IsNullOrEmpty(text))
                throw new ArgumentNullException("text");
            text = text.Trim().ToLower();
            if (text == "1" || text == "зачет" || text == "зачёт")
                return true;
            else if (text == "0" || text == "незачет" || text == "незачёт")
                return false;
            throw new ArgumentException("Недопустимое значение text: " + text);
        }
    }
}