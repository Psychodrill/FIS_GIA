using System;
using System.Text;

namespace Esrp.Utility
{
    /// <summary>
    /// Транслитерация русских слов в латиницу, например: <Превед!> в <Preved!>
    /// </summary>
    public sealed class RussianToLatin
    {
        private static char[] StrFrom = {'А', 'Б', 'В', 'Г', 'Д', 'Е', 'Ё', 'Ж', 'З', 'И', 
            'Й', 'К', 'Л', 'М', 'Н', 'О', 'П', 'Р', 'С', 'Т', 
            'У', 'Ф', 'Х', 'Ц', 'Ч', 'Ш', 'Щ', 'Ъ', 'Ы', 'Ь', 
            'Э', 'Ю', 'Я', 'а', 'б', 'в', 'г', 'д', 'е', 'ё', 
            'ж', 'з', 'и', 'й', 'к', 'л', 'м', 'н', 'о', 'п', 
            'р', 'с', 'т', 'у', 'ф', 'х', 'ц', 'ч', 'ш', 'щ', 
            'ъ', 'ы', 'ь', 'э', 'ю', 'я'};

        private static string[] StrTo = {"A", "B", "V", "G", "D", "E", "E", "ZH", "Z", "I", 
            "J", "K", "L", "M", "N", "O", "P", "R", "S", "T", 
            "U", "F", "KH", "TS", "CH", "SH", "SHCH", "", "", "", 
            "E", "YU", "YA", "a", "b", "v", "g", "d", "e", "e", 
            "zh", "z", "i", "j", "k", "l", "m", "n", "o", "p", 
            "r", "s", "t", "u", "f", "kh", "ts", "ch", "sh", "shch", 
            "", "", "", "e", "yu", "ya"};

        private static string[] StrTo2 = {"A", "B", "V", "G", "D", "E", "E", "ZH", "Z", "I", 
            "J", "K", "L", "M", "N", "O", "P", "R", "S", "T", 
            "U", "F", "KH", "TS", "CH", "SH", "SHCH", "", "", "", 
            "E", "YU", "YA", "a", "b", "v", "g", "d", "e", "e", 
            "zh", "z", "i", "j", "k", "l", "m", "n", "o", "p", 
            "r", "s", "t", "u", "f", "kh", "ts", "ch", "sh", "shch", 
            "", "", "", "e", "yu", "ya"};

        private static string[] StrEn = {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", 
            "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", 
            "U", "V", "W", "X", "Y", "Z", "a", "b", "c", "d", 
            "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", 
            "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", 
            "y", "z", "1", "2", "3", "4", "5", "6", "7", "8", 
            "9", "0", "_"};

        private RussianToLatin() { }

        /// <summary> Транслитерация русских слов в латиницу. </summary>
        /// <param name="value">Строка с русскими символами.</param>
        /// <param name="preserveCase"> Ecли true, то буквы сохраняют регистр, иначе все буквы переводятся в строчные. </param>
        /// <returns> Результат операции транслитерации. </returns>
        public static string Encode(string value, bool preserveCase)
        {
            StringBuilder result = new StringBuilder();
            int i;

            if (!preserveCase)
                value = value.ToLower();

            foreach (char c in value)
            {
                i = Array.IndexOf(StrFrom, c);

                if (i >= 0)
                    result.Append(StrTo[i]);
                else
                    result.Append(c);
            }

            return result.ToString();
        }

        /// <summary> Транслитерация русских слов в латиницу. Символ <_> переводится в пробел. </summary>
        /// <param name="value">Строка с русскими символами.</param>
        /// <param name="preserveCase"> Ecли true, то буквы сохраняют регистр, иначе все буквы переводятся в строчные. </param>
        /// <returns> Результат операции транслитерации. </returns>
        public static string EncodePhrase(string value, bool preserveCase)
        {
            StringBuilder result = new StringBuilder();
            int i;

            if (!preserveCase)
                value = value.ToLower();

            foreach (char c in value)
            {
                i = Array.IndexOf(StrFrom, c);

                if (i >= 0)
                    result.Append(StrTo2[i]);
                else
                    result.Append(c);
            }

            return result.ToString();
        }

        /// <summary>
        /// Транслитерация русских слов в латиницу.
        /// Если не находится соотвествие переводимому символу, то символ удаляется.
        /// </summary>
        /// <param name="value">Строка с русскими символами.</param>
        /// <param name="preserveCase"> Ecли true, то буквы сохраняют регистр, иначе все буквы переводятся в строчные. </param>
        /// <returns> Результат операции транслитерации. </returns>
        public static string ReplaceEncode(string value, bool preserveCase)
        {
            StringBuilder result = new StringBuilder();
            string s;
            int i;

            if (!preserveCase)
                value = value.ToLower();

            foreach (char c in value)
            {
                i = Array.IndexOf(StrFrom, c);

                if (i >= 0)
                    result.Append(StrTo[i]);
                else
                {
                    s = Convert.ToString(c);
                    i = Array.IndexOf(StrEn, s);
                    if (i >= 0)
                        result.Append(s);
                    else
                        result.Append(string.Empty);
                }
            }

            return result.ToString();
        }
    }
}