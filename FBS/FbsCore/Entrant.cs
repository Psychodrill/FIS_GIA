using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Fbs.Core
{
    /// <summary>
    /// Абитуриент.
    /// </summary>
    public class Entrant
    {
        #region public fields

        /// <summary>
        /// Фамилия.
        /// </summary>
        public string LastName;

        /// <summary>
        /// Имя.
        /// </summary>
        public string FirstName;

        /// <summary>
        /// Отчество.
        /// </summary>
        public string PatronymicName;

        /// <summary>
        /// Номер документа.
        /// </summary>
        public string PassportNumber;

        /// <summary>
        /// Серия документа.
        /// </summary>
        public string PassportSeria;

        /// <summary>
        /// Номер свидетельства.
        /// </summary>
        public string CertificateNumber;

        /// <summary>
        /// ГИФО
        /// </summary>
        public string GIFOCategoryName;

        /// <summary>
        /// Код направления.
        /// </summary>
        public string DirectionCode;

        /// <summary>
        /// Код специальности.
        /// </summary>
        public string SpecialtyCode;

        /// <summary>
        /// Ошибки в формате входной строки.
        /// </summary>
        public enum BatchLineFormatError : int
        {
            None = -1,              // нет ошибок
            LastName = 0,           // ошибка в фамилии
            FirstName = 1,          // ошибка в имени
            PatronymicName = 2,     // ошибка в отчестве
            CertificateNumber = 3,  // ошибка в номере свидетельств
            PassportNumber = 4,     // ошибка в номере паспорта
            Other = 5
        }

        #endregion

        #region private fields

        private static Regex BatchLineRegex = new Regex(
            @"^(?<LastName>[А-Яа-яЁё\s-]+)%(?<FirstName>[А-Яа-яЁё\s-]+)%(?<PatronymicName>[А-Яа-яЁё\s-]+)*%" +
            @"(?<CertificateNumber>\d{2}-\d{9}-\d{2})*%(?<PassportNumber>[^%]+)%(?<PassportSeria>[^%]*)%" +
            @"(?<DirectionCode>[^%]*)%(?<SpecialtyCode>[^%]*)$", RegexOptions.Compiled);

        // Количество частей в строке разделенных "%".
        private static int BatchLinePartsCount = 8;

        #endregion

        #region public methods

        /// <summary>
        /// Проверка пакета.
        /// </summary>
        /// <param name="BatchLines">Строки пакета.</param>
        /// <param name="LineNumber">Номер строки с неверным форматом.</param>
        /// <returns>Тип ошибки.</returns>
        public static BatchLineFormatError CheckBatch(string[] BatchLines, out int LineNumber)
        {
            for (int lineNum = 0; lineNum <= BatchLines.Length - 1; lineNum++)
            {
                LineNumber = lineNum + 1;
                string[] parts = BatchLines[lineNum].Split(new char[] { '%' });

                if (parts.Length != BatchLinePartsCount)
                    return BatchLineFormatError.Other;

                else if (!Regex.IsMatch(parts[(int)BatchLineFormatError.LastName],
                        @"^[А-Яа-яЁё\s-]+$"))
                    return BatchLineFormatError.LastName;

                else if (!Regex.IsMatch(parts[(int)BatchLineFormatError.FirstName],
                        @"^[А-Яа-яЁё\s-]+$"))
                    return BatchLineFormatError.FirstName;

                else if (!Regex.IsMatch(parts[(int)BatchLineFormatError.PatronymicName],
                        @"^[А-Яа-яЁё\s-]*$"))
                    return BatchLineFormatError.PatronymicName;

                else if (!Regex.IsMatch(parts[(int)BatchLineFormatError.CertificateNumber],
                        @"^(\d{2}-\d{9}-\d{2})*$"))
                    return BatchLineFormatError.CertificateNumber;

                else if (String.IsNullOrEmpty(parts[(int)BatchLineFormatError.PassportNumber]))
                    return BatchLineFormatError.PassportNumber;

            }
            LineNumber = -1;
            return BatchLineFormatError.None;
        }

        /// <summary>
        /// Получение сообщения о неверном формате для пакета.
        /// </summary>
        /// <param name="Error">Тип ошибки.</param>
        /// <returns>Текст ошибки.</returns>
        public static string GetFormatErrorMsg(BatchLineFormatError Error)
        {
            switch (Error)
            {
                case BatchLineFormatError.Other:
                    return "Неверный формат";
                case BatchLineFormatError.LastName:
                    return "Неверно указана фамилия";
                case BatchLineFormatError.FirstName:
                    return "Неверно указано имя";
                case BatchLineFormatError.PatronymicName:
                    return "Неверно указано отчество";
                case BatchLineFormatError.CertificateNumber:
                    return "Неверно указан номер свидетельства";
                case BatchLineFormatError.PassportNumber:
                    return "Не указан номер документа";
                default:
                    return String.Empty;
            }
        }

        /// <summary>
        /// Загрузка пакета.
        /// </summary>
        /// <param name="BatchLines">Строки пакета.</param>
        public static void LoadBatch(string[] BatchLines)
        {
            CommonNationalCertificateContext.UpdateEntrant(ParseBatch(BatchLines));
        }

        /// <summary>
        /// Формирование массива абитуриентов на основе пакета.
        /// </summary>
        /// <param name="BatchLines">
        /// Строка из пакетного файла вида:
        /// "Фамилия%Имя%Отчество%Номер свидетельства (если есть) %Номер документа%Серия документа%Код направления%Код специальности".
        /// </param>
        /// <returns>Массив абитуриентов.</returns>
        private static Entrant[] ParseBatch(string[] BatchLines)
        {
            Entrant[] entrants = new Entrant[BatchLines.Length];
            for (int lineNum = 0; lineNum <= BatchLines.Length - 1; lineNum++)
            {
                Match match = BatchLineRegex.Match(BatchLines[lineNum]);
                if (match.Success)
                {
                    entrants[lineNum] = new Entrant();
                    entrants[lineNum].LastName = match.Groups["LastName"].Value;
                    entrants[lineNum].FirstName = match.Groups["FirstName"].Value;
                    entrants[lineNum].PatronymicName = match.Groups["PatronymicName"].Value;
                    entrants[lineNum].PassportNumber = match.Groups["PassportNumber"].Value;
                    entrants[lineNum].PassportSeria = match.Groups["PassportSeria"].Value;
                    entrants[lineNum].CertificateNumber = match.Groups["CertificateNumber"].Value;
                    entrants[lineNum].DirectionCode = match.Groups["DirectionCode"].Value;
                    entrants[lineNum].SpecialtyCode = match.Groups["SpecialtyCode"].Value;
                }
                else
                    throw new FormatException("Неверный формат строки");
            }
            return entrants;
        }

        #endregion
    }
}
