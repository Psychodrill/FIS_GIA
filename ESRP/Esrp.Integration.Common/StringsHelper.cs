using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Esrp.Integration.Common
{
    public static class StringsHelper
    {
        private static Dictionary<string, string> transliterationDictionary;
        private static void EnsureTransliterationDictionary()
        {
            if (transliterationDictionary == null)
            {
                transliterationDictionary = new Dictionary<string, string>();
                transliterationDictionary.Add("Є", "YE");
                transliterationDictionary.Add("І", "I");
                transliterationDictionary.Add("Ѓ", "G");
                transliterationDictionary.Add("і", "i");
                transliterationDictionary.Add("№", "#");
                transliterationDictionary.Add("є", "ye");
                transliterationDictionary.Add("ѓ", "g");
                transliterationDictionary.Add("А", "A");
                transliterationDictionary.Add("Б", "B");
                transliterationDictionary.Add("В", "V");
                transliterationDictionary.Add("Г", "G");
                transliterationDictionary.Add("Д", "D");
                transliterationDictionary.Add("Е", "E");
                transliterationDictionary.Add("Ё", "YO");
                transliterationDictionary.Add("Ж", "ZH");
                transliterationDictionary.Add("З", "Z");
                transliterationDictionary.Add("И", "I");
                transliterationDictionary.Add("Й", "J");
                transliterationDictionary.Add("К", "K");
                transliterationDictionary.Add("Л", "L");
                transliterationDictionary.Add("М", "M");
                transliterationDictionary.Add("Н", "N");
                transliterationDictionary.Add("О", "O");
                transliterationDictionary.Add("П", "P");
                transliterationDictionary.Add("Р", "R");
                transliterationDictionary.Add("С", "S");
                transliterationDictionary.Add("Т", "T");
                transliterationDictionary.Add("У", "U");
                transliterationDictionary.Add("Ф", "F");
                transliterationDictionary.Add("Х", "X");
                transliterationDictionary.Add("Ц", "C");
                transliterationDictionary.Add("Ч", "CH");
                transliterationDictionary.Add("Ш", "SH");
                transliterationDictionary.Add("Щ", "SHH");
                transliterationDictionary.Add("Ъ", "'");
                transliterationDictionary.Add("Ы", "Y");
                transliterationDictionary.Add("Ь", "");
                transliterationDictionary.Add("Э", "E");
                transliterationDictionary.Add("Ю", "YU");
                transliterationDictionary.Add("Я", "YA");
                transliterationDictionary.Add("а", "a");
                transliterationDictionary.Add("б", "b");
                transliterationDictionary.Add("в", "v");
                transliterationDictionary.Add("г", "g");
                transliterationDictionary.Add("д", "d");
                transliterationDictionary.Add("е", "e");
                transliterationDictionary.Add("ё", "yo");
                transliterationDictionary.Add("ж", "zh");
                transliterationDictionary.Add("з", "z");
                transliterationDictionary.Add("и", "i");
                transliterationDictionary.Add("й", "j");
                transliterationDictionary.Add("к", "k");
                transliterationDictionary.Add("л", "l");
                transliterationDictionary.Add("м", "m");
                transliterationDictionary.Add("н", "n");
                transliterationDictionary.Add("о", "o");
                transliterationDictionary.Add("п", "p");
                transliterationDictionary.Add("р", "r");
                transliterationDictionary.Add("с", "s");
                transliterationDictionary.Add("т", "t");
                transliterationDictionary.Add("у", "u");
                transliterationDictionary.Add("ф", "f");
                transliterationDictionary.Add("х", "x");
                transliterationDictionary.Add("ц", "c");
                transliterationDictionary.Add("ч", "ch");
                transliterationDictionary.Add("ш", "sh");
                transliterationDictionary.Add("щ", "shh");
                transliterationDictionary.Add("ъ", "");
                transliterationDictionary.Add("ы", "y");
                transliterationDictionary.Add("ь", "");
                transliterationDictionary.Add("э", "e");
                transliterationDictionary.Add("ю", "yu");
                transliterationDictionary.Add("я", "ya");
                transliterationDictionary.Add("«", "");
                transliterationDictionary.Add("»", "");
                transliterationDictionary.Add("—", "-");
            }
        }

        public static string TransliterateToEng(string str)
        {
            if (String.IsNullOrEmpty(str))
                return str;
            EnsureTransliterationDictionary();
            string result = null;
            foreach (char ch in str)
            {
                if (Char.IsDigit(ch))
                {
                    result += ch;
                    continue;
                }
                if (ch == '_')
                {
                    result += "_";
                    continue;
                }
                if (transliterationDictionary.ContainsKey(ch.ToString()))
                {
                    result += transliterationDictionary[ch.ToString()];
                }
            }
            return result;
        }

        public static string Normalize(string str, bool removeCasing)
        {
            string[] temp;
            return Normalize(str, removeCasing, false, null, out temp);
        }

        public static string Normalize(string str, bool removeCasing, bool removeSingleLetters)
        {
            string[] temp;
            return Normalize(str, removeCasing, removeSingleLetters, null, out temp);
        }

        public static string Normalize(string str, bool removeCasing, bool removeSingleLetters, string[] ignoreWords)
        {
            string[] temp;
            return Normalize(str, removeCasing, removeSingleLetters, ignoreWords, out temp);
        }

        public static string Normalize(string str, bool removeCasing, out string[] extractedWords)
        {
            return Normalize(str, removeCasing, false, null, out extractedWords);
        }

        public static string Normalize(string str, bool removeCasing, bool removeSingleLetters, string[] ignoreWords, out string[] extractedWords)
        {
            extractedWords = new string[0];
            if (String.IsNullOrEmpty(str))
                return null;
            if (str.All(obj => Char.IsDigit(obj)))
                return str;

            string[] ignoreWordsCasing = null;
            if (removeCasing)
            {
                str = str.ToLower();
                if (ignoreWords != null)
                {
                    ignoreWordsCasing = ignoreWords.Select(x => x.ToLower()).ToArray();
                }
            }
            else
            {
                ignoreWordsCasing = ignoreWords;
            }

            char[] separators = new char[] { ' ', '-', '"', '\'', '»', '«', '(', ')', '.', ',', ';', ':', '№' };

            string[] words = str.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            List<string> newWords = new List<string>();
            foreach (string word in words)
            {
                if ((ignoreWordsCasing != null) && (ignoreWordsCasing.Contains(word)))
                    continue;

                if ((removeSingleLetters) && (word.Length == 1))
                    continue;

                string newWord = word;
                if ((removeCasing) && (newWord.Length >= 5))
                {
                    if (newWord.EndsWith("а") || newWord.EndsWith("у") || newWord.EndsWith("ы")
                        || (newWord.EndsWith("е") && (!newWord.EndsWith("ие")) && (!newWord.EndsWith("ое")) && (!newWord.EndsWith("ые")) && (!newWord.EndsWith("ее"))))
                    {
                        newWord = newWord.Substring(0, newWord.Length - 1);
                    }
                    else if (newWord.EndsWith("ой")
                        || newWord.EndsWith("ый")
                        || newWord.EndsWith("ая")
                        || newWord.EndsWith("ая")
                        || newWord.EndsWith("ии")
                        || newWord.EndsWith("ий")
                        || newWord.EndsWith("ов")
                        || newWord.EndsWith("ом")
                        || newWord.EndsWith("ия")
                        || newWord.EndsWith("ию")
                        || newWord.EndsWith("ею")
                        || newWord.EndsWith("ие")
                        || newWord.EndsWith("ое")
                        || newWord.EndsWith("ее")
                        || newWord.EndsWith("ем")
                        || newWord.EndsWith("ым")
                        || newWord.EndsWith("ых")
                        || newWord.EndsWith("их")
                        || newWord.EndsWith("ые"))
                    {
                        newWord = newWord.Substring(0, newWord.Length - 2);
                    }
                    else if (newWord.EndsWith("ого")
                        || newWord.EndsWith("его")
                        || newWord.EndsWith("ами")
                        || newWord.EndsWith("ями")
                        || newWord.EndsWith("ему")
                        || newWord.EndsWith("ому")
                        || newWord.EndsWith("ыми"))
                    {
                        newWord = newWord.Substring(0, newWord.Length - 3);
                    }
                }

                newWords.Add(newWord);
            }

            extractedWords = newWords.ToArray();
            str = String.Join("", extractedWords);

            return str;
        }

        public static bool ExpressionsAreSimilar(string[] expression1Words, string[] expression2Words, out int notMatchedCount)
        {
            string[] baseWords;
            string[] comparableWords;
            if (expression1Words.Length > expression2Words.Length)
            {
                baseWords = expression1Words;
                comparableWords = expression2Words;
            }
            else
            {
                baseWords = expression2Words;
                comparableWords = expression1Words;
            }

            int wordsMatched = 0;
            int wordsNotMatched = 0;
            int totalWords = baseWords.Length;
            foreach (string nameWord in baseWords)
            {
                if (comparableWords.Any(obj => obj == nameWord))
                {
                    wordsMatched++;
                }
                else
                {
                    wordsNotMatched++;
                }
            }
            notMatchedCount = wordsNotMatched;
            if ((totalWords > 8) && (wordsNotMatched < 3))
                return true;
            if ((totalWords > 5) && (wordsNotMatched < 2))
                return true;
            if (wordsNotMatched == 0)
                return true;
            return false;
        }

        public static string JoinWithoutEmptyElements(string separator, params string[] values)
        {
            if (values == null)
                return null;
            if (values.Where(obj => !String.IsNullOrEmpty(obj)).Count() == 0)
                return null;
            return String.Join(separator, values.Where(x => !String.IsNullOrEmpty(x)).ToArray());
        }

        public static string EmptyToNull(string str)
        {
            if (String.IsNullOrEmpty(str))
                return null;
            return str;
        }

        public static bool IsTrueString(string str)
        {
            return (str == "1" || str == "true" || str == "TRUE" || str == "True");
        }
    }
}
