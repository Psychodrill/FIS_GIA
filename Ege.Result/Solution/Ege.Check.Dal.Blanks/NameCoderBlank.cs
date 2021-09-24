namespace Ege.Check.Dal.Blanks
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;
    using JetBrains.Annotations;

    /// <summary>
    ///     Осуществляет кодирование имён файлов бланков
    /// </summary>
    public class NameCoderBlank : IDisposable
    {
        [NotNull] readonly ICryptoTransform _transform;

// ReSharper disable InconsistentNaming
        private const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
// ReSharper restore InconsistentNaming

        /// <summary>
        /// Реализация Base32 для C#.
        /// </summary>
        /// <remarks>
        /// В C# отсутствует реализация Base32, приходится писать самим
        /// Данная реализация отличается от RFC тем, что не добавляет символы '='
        /// </remarks>
        /// <param name="data">Конвертируемые данные</param>
        /// <returns>Строка в base32</returns>
        [NotNull]
        private static string ToBase32([NotNull]byte[] data)
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

        [NotNull]
        private static byte[] FromBase32([NotNull]string data)
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

        /// <summary>
        /// Инициализирует ICryptoTransform с указанным ключом
        /// </summary>
        public NameCoderBlank()
        {
            using (Rijndael aes = Rijndael.Create())
            {
                if (aes == null)
                {
                    throw new NullReferenceException("Rijndael.Create returned null");
                }
                aes.IV = new byte[16];
                aes.Key = new byte[]
                {
                    0xc5, 0x4d, 0x95, 0xc0, 0x6e, 0xba, 0x39, 0xbc, 0x28, 0x4c, 0x07, 0x0e, 0x76, 0x79, 0x50, 0x33, 0x8b,
                    0xcd, 0xdb, 0xf3, 0xaa, 0x97, 0x72, 0x87, 0x58, 0x83, 0xaa, 0x76, 0x83, 0xb3, 0x6d, 0xee
                };
                aes.Padding = PaddingMode.None;
                _transform = aes.CreateEncryptor();
            }
        }

        public static string Decode([NotNull]string code)
        {
            byte[] data = FromBase32(code);
            using (Rijndael aes = Rijndael.Create())
            {
                if (aes == null)
                {
                    throw new NullReferenceException("Rijndael.Create returned null");
                }
                aes.IV = new byte[16];
                aes.Key = new byte[] { 0xc5, 0x4d, 0x95, 0xc0, 0x6e, 0xba, 0x39, 0xbc, 0x28, 0x4c, 0x07, 0x0e, 0x76, 0x79, 0x50, 0x33, 0x8b, 0xcd, 0xdb, 0xf3, 0xaa, 0x97, 0x72, 0x87, 0x58, 0x83, 0xaa, 0x76, 0x83, 0xb3, 0x6d, 0xee };
                aes.Padding = PaddingMode.None;
                using (var decryptor = aes.CreateDecryptor())
                {
                    data = decryptor.TransformFinalBlock(data, 0, data.Length);
                    if (data == null)
                    {
                        throw new NullReferenceException("ICryptoTransform.TransformFinalBlock returned null");
                    }
                }
                return Encoding.ASCII.GetString(data);
            }
        }

        /// <summary>
        /// Реализация кодирования имени для бланков AB - они одностраничные
        /// </summary>
        /// <param name="barcode">Штрих-код участника</param>
        /// <returns>Код файла</returns>
        [NotNull]
        public string CreateCodeAb(string barcode)
        {
            return CreateCode(barcode, 1, 0);
        }

        /// <summary>
        /// Реализация кодирования имени для бланков C
        /// </summary>
        /// <param name="barcode">Штрих-код участника</param>
        /// <param name="number">Номер страницы бланка</param>
        /// <returns>Код файла</returns>
        [NotNull]
        public string CreateCodeC(string barcode, int number)
        {
            return CreateCode(barcode, 2, number);
        }

        /// <summary>
        /// Реализация кодирования имени для бланков записи
        /// </summary>
        /// <param name="barcode">Штрих-код бланка (работы)</param>
        /// <param name="number">Номер страницы бланка записи</param>
        /// <returns>Код файла</returns>
        [NotNull]
        public string CreateCodeZ(string barcode, int number)
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
        [NotNull]
        public string CreateCode(string barcode, int blankType, int number)
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

        [NotNull]
        private string CreateCodeBySplitCode([NotNull]string code)
        {
            string resCode = "";
            while(code.Length > 0)
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

        [NotNull]
        private string CreateCode([NotNull]string code)
        {
            byte[] str = Encoding.ASCII.GetBytes(code);
            byte[] data = _transform.TransformFinalBlock(str, 0, str.Length);
            if (data == null)
            {
                throw new NullReferenceException("ICryptoTransform.TransformFinalBlock returned null");
            }
            return ToBase32(data);
        }

        /// <summary>
        /// Создаёт имя файла из кода файла.
        /// </summary>
        /// <param name="server">Сервер, на котором расположен файл</param>
        /// <param name="code">Код файла</param>
        /// <returns>Готовый URL файла</returns>
        public static string FileUrl(string server, string code)
        {
            if (string.IsNullOrEmpty(server)) throw new ArgumentNullException("server");
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
    
        public void Dispose()
        {
            _transform.Dispose();
        }
    }
}
