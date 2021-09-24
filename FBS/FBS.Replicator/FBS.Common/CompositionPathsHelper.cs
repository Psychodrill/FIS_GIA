using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions; 

namespace FBS.Common
{
    public static class CompositionPathsHelper
    {
        public static string GetCompositionPaths2015(string staticPath, Guid participantId, string documentNumber, string name, string surname, string secondName, int pagesCount)
        {
            Logger.DetailedLogger.WriteLine(string.Format("GetCompositionPaths2015 {0} {1} {2} {3} {4} {5} {6}", staticPath, participantId, documentNumber, name, surname, secondName, pagesCount));
            while (documentNumber.Length < 10)
            {
                documentNumber = "0" + documentNumber;
            }
            string hash = Helper2015.getMd5Hash(string.Format("{0}{1}{2}", Helper2015.NormalizeString(surname), Helper2015.NormalizeString(name), Helper2015.NormalizeString(secondName)));
            string basePath = String.Format("{0}\\{1}\\{2}\\{3}\\{4}\\{5}\\",
                staticPath,
                hash.Substring(0, 2),
                hash.Substring(0, 4),
                hash,
                documentNumber,
                participantId.ToString());

            string result = String.Empty;
            for (int page = 1; page <= pagesCount; page++)
            {
                result += basePath + page.ToString() + ";";
            }
            Logger.DetailedLogger.WriteLine(string.Format("GetCompositionPaths2015 result={7}", staticPath, participantId, documentNumber, name, surname, secondName, pagesCount, result));
            return result;
        }

        public static string GetCompositionPaths2016Plus(string staticPath, string barcode, int projectBatchID, string projectName, DateTime examDate, int pagesCount)
        {
            //Logger.DetailedLogger.WriteLine(string.Format("GetCompositionPaths2016Plus {0} {1} {2} {3} {4} {5}", staticPath, barcode, projectBatchID, projectName, examDate.ToShortDateString(), pagesCount));
            string generatedBarcode = String.Format("{0}_{1}_{2}", barcode, projectBatchID, projectName);
            string dateStr = examDate.ToString("yyyy.MM.dd");

            string result = String.Empty;
            for (int page = 0; page < pagesCount; page++)
            {
                string hash = Helper2016Plus.CreateCodeZ(generatedBarcode, page);
                result += String.Format("{0}\\{1}\\{2}\\{3}\\{4}.png;",
                    staticPath,
                    dateStr,
                    hash[0],
                    hash[1],
                    hash);
            }

            result += $"\n\n{pagesCount}";
            //Logger.DetailedLogger.WriteLine(string.Format("GetCompositionPaths2016Plus result={6}", staticPath, barcode, projectBatchID, projectName, examDate.ToShortDateString(), pagesCount, result));

            return result;
        }

        private static class Helper2015
        {
            public static string NormalizeString(string input)
            {
                if (input == null) return "";
                var r = new Regex("\\w*");
                var res = r.Matches(input);
                string result = "";
                foreach (var re in res)
                {
                    if (re.ToString().Length > 0) result += re.ToString();
                }
                return result.ToLower().Replace('ё', 'е').Replace('й', 'и');
            }

            public static string getMd5Hash(string input)
            {
                using (var x = new System.Security.Cryptography.MD5CryptoServiceProvider())
                {
                    byte[] bs = System.Text.Encoding.UTF8.GetBytes(input);
                    bs = x.ComputeHash(bs);
                    System.Text.StringBuilder s = new System.Text.StringBuilder();
                    foreach (byte b in bs)
                    {
                        s.Append(b.ToString("x2").ToLower());
                    }
                    return s.ToString();
                }
            }
        }

        private static class Helper2016Plus
        {
            static Helper2016Plus()
            {
                Rijndael aes = Rijndael.Create();
                aes.IV = new byte[16];
                aes.Key = new byte[] { 0xc5, 0x4d, 0x95, 0xc0, 0x6e, 0xba, 0x39, 0xbc, 0x28, 0x4c, 0x07, 0x0e, 0x76, 0x79, 0x50, 0x33, 0x8b, 0xcd, 0xdb, 0xf3, 0xaa, 0x97, 0x72, 0x87, 0x58, 0x83, 0xaa, 0x76, 0x83, 0xb3, 0x6d, 0xee };
                aes.Padding = PaddingMode.None;
                transform = aes.CreateEncryptor();
            }

            private static ICryptoTransform transform;

            private const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";

            /// <summary>
            /// Реализация Base32 для C#.
            /// </summary>
            /// <remarks>
            /// В C# отсутствует реализация Base32, приходится писать самим
            /// Данная реализация отличается от RFC тем, что не добавляет символы '='
            /// </remarks>
            /// <param name="data">Конвертируемые данные</param>
            /// <returns>Строка в base32</returns>
            private static string ToBase32(byte[] data)
            {
                int val = data[0] << 8;
                int bytes = 8;
                int shift = 1;
                string result = "";
                while (bytes > 0)
                {
                    result += alphabet[(val & 0xf800) >> 11];
                    val <<= 5;
                    bytes -= 5;
                    if (bytes < 5 && shift < data.Length)
                    {
                        val |= data[shift++] << (8 - bytes);
                        bytes += 8;
                    }
                }
                return result;
            }

            private static byte[] FromBase32(string data)
            {
                List<byte> result = new List<byte>();
                int shift = 0;
                int val = 0;

                for (int i = 0; i < data.Length; i++)
                {
                    val = val << 5 | alphabet.IndexOf(data[i]);
                    shift += 5;
                    if (shift >= 8)
                    {
                        shift -= 8;
                        result.Add((byte)((val >> shift) & 0xff));
                    }
                }
                if (shift >= 8)
                {
                    shift -= 8;
                    result.Add((byte)((val >> shift) & 0xff));
                }

                return result.ToArray();
            }

            public static string Decode(string code)
            {
                byte[] data = FromBase32(code);
                Rijndael aes = Rijndael.Create();
                aes.IV = new byte[16];
                aes.Key = new byte[] { 0xc5, 0x4d, 0x95, 0xc0, 0x6e, 0xba, 0x39, 0xbc, 0x28, 0x4c, 0x07, 0x0e, 0x76, 0x79, 0x50, 0x33, 0x8b, 0xcd, 0xdb, 0xf3, 0xaa, 0x97, 0x72, 0x87, 0x58, 0x83, 0xaa, 0x76, 0x83, 0xb3, 0x6d, 0xee };
                aes.Padding = PaddingMode.None;
                data = aes.CreateDecryptor().TransformFinalBlock(data, 0, data.Length);
                return Encoding.ASCII.GetString(data);
            }

            /// <summary>
            /// Реализация кодирования имени для бланков AB - они одностраничные
            /// </summary>
            /// <param name="barcode">Штрих-код участника</param>
            /// <returns>Код файла</returns>
            public static string CreateCodeAB(string barcode)
            {
                return CreateCode(barcode, 1, 0);
            }

            /// <summary>
            /// Реализация кодирования имени для бланков C
            /// </summary>
            /// <param name="barcode">Штрих-код участника</param>
            /// <param name="number">Номер страницы бланка</param>
            /// <returns>Код файла</returns>
            public static string CreateCodeC(string barcode, int number)
            {
                return CreateCode(barcode, 2, number);
            }

            /// <summary>
            /// Реализация кодирования имени для бланков записи
            /// </summary>
            /// <param name="barcode">Штрих-код бланка (работы)</param>
            /// <param name="number">Номер страницы бланка записи</param>
            /// <returns>Код файла</returns>
            public static string CreateCodeZ(string barcode, int number)
            {
                return CreateCode(barcode, 0, number);
            }


            /// <summary>
            /// Универсальная реализация кодирования имени файла
            /// </summary>
            /// <param name="barcode">Штрих-код участника</param>
            /// <param name="blankType">Тип бланка</param>
            /// <param name="number">Номер страницы бланка</param>
            /// <returns>Код файла</returns>
            public static string CreateCode(string barcode, int blankType, int number)
            {
                if (string.IsNullOrEmpty(barcode)) throw new ArgumentNullException("barcode");
                //if (barcode.Length > 13) throw new ArgumentException("barcode is too long");
                if (blankType < 0 || blankType > 9) throw new ArgumentOutOfRangeException("blankType");
                if (number < 0 || number > 99) throw new ArgumentOutOfRangeException("number");

                string code = barcode + blankType.ToString() + number.ToString("00");
                if (blankType == 0)
                {
                    return CreateCodeBySplitCode(code);
                }
                else
                {
                    while (code.Length < 16) code += "=";
                    return CreateCode(code);
                }
            }

            private static string CreateCodeBySplitCode(string code)
            {
                string resCode = "";
                while (code.Length > 0)
                {
                    string tmpCode = "";
                    if (code.Length > 16)
                    {
                        tmpCode = code.Substring(0, 16);
                        code = code.Substring(16, code.Length - 16);
                    }
                    else
                    {
                        tmpCode = code;
                        code = "";
                    }
                    while (tmpCode.Length < 16) tmpCode += "=";
                    resCode = resCode + CreateCode(tmpCode);
                }

                return resCode;
            }

            private static string CreateCode(string code)
            {
                byte[] str = Encoding.ASCII.GetBytes(code);
                byte[] data = transform.TransformFinalBlock(str, 0, str.Length);
                return ToBase32(data);
            }

            /// <summary>
            /// Создаёт имя файла из кода файла.
            /// </summary>
            /// <param name="server">Сервер, на котором расположен файл</param>
            /// <param name="code">Код файла</param>
            /// <returns>Гтовый URL файла</returns>
            public static string FileURL(string server, string code)
            {
                if (string.IsNullOrEmpty(server)) throw new ArgumentNullException("sever");
                if (code == null || (code.Length != 26 && code.Length != 52 && code.Length != 78 && code.Length != 104)) throw new ArgumentException("invalid file code");
                if (!server.StartsWith("http://")) server = "http://" + server;
                return string.Format("{0}/{1}/{2}/{3}.png", server, code[0], code[1], code);
            }

            /// <summary>
            /// Создаёт выходное имя файла
            /// </summary>
            /// <param name="folder">Папка хранилища</param>
            /// <param name="subjectCode">Код предмета</param>
            /// <param name="examDate">Дата экзамена</param>
            /// <param name="code">Код</param>
            /// <returns>Путь к файлу</returns>
            public static string FilePath(string folder, int subjectCode, string examDate, string code)
            {
                if (string.IsNullOrEmpty(folder)) throw new ArgumentNullException("folder");
                if (subjectCode < 0) throw new ArgumentOutOfRangeException("subjectCode");
                if (examDate == null) throw new ArgumentNullException("examDate");
                if (examDate.Length != 10) throw new ArgumentException("Invalid examDate");
                if (code == null) throw new ArgumentNullException("code");
                if (code.Length != 26 && code.Length != 52 && code.Length != 78 && code.Length != 104) throw new ArgumentException("invalid file code");
                return Path.Combine(folder, string.Format("{0:00}\\{1}\\{2}\\{3}\\{4}.png", subjectCode, examDate, code[0], code[1], code));
            }
        }
    }
}
