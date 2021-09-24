namespace Fbs.Core.BatchCheck
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// The batch check result.
    /// </summary>
    public class BatchCheckResult
    {
        #region Fields

        private readonly bool outputSubject1;

        private readonly bool outputSubject10;

        private readonly bool outputSubject11;

        private readonly bool outputSubject12;

        private readonly bool outputSubject13;

        private readonly bool outputSubject14;

        private readonly bool outputSubject2;

        private readonly bool outputSubject3;

        private readonly bool outputSubject4;

        private readonly bool outputSubject5;

        private readonly bool outputSubject6;

        private readonly bool outputSubject7;

        private readonly bool outputSubject8;

        private readonly bool outputSubject9;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BatchCheckResult"/> class.
        /// </summary>
        /// <param name="filter">
        /// The filter.
        /// </param>
        public BatchCheckResult(BatchCheckFilter filter)
        {
            int[] subjectIds = (filter != null) ? filter.SubjectIds : BatchCheckFilter.EmptyFilter().SubjectIds;
            this.outputSubject1 = subjectIds.Contains(1);
            this.outputSubject2 = subjectIds.Contains(2);
            this.outputSubject3 = subjectIds.Contains(3);
            this.outputSubject4 = subjectIds.Contains(4);
            this.outputSubject5 = subjectIds.Contains(5);
            this.outputSubject6 = subjectIds.Contains(6);
            this.outputSubject7 = subjectIds.Contains(7);
            this.outputSubject8 = subjectIds.Contains(8);
            this.outputSubject9 = subjectIds.Contains(9);
            this.outputSubject10 = subjectIds.Contains(10);
            this.outputSubject11 = subjectIds.Contains(11);
            this.outputSubject12 = subjectIds.Contains(12);
            this.outputSubject13 = subjectIds.Contains(13);
            this.outputSubject14 = subjectIds.Contains(14);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether OutputCheckedValues.
        /// </summary>
        public bool OutputCheckedValues { get; set; }

        #endregion

        /* 29.06.2011 Виниченко А.В. "TS-Soft"
        * Данный метод используется всеми видами пакетных проверок 
        * для формирования файла *.csv c результатами проверок. Каждый вид
        * пакетных проверок использует свою хранимую процедуру для поиска, 
        * результаты которой передаются в dataSet. И получается, что колонки 
        * в dataSet для каждого вида пакетной проверки свои. Что бы не возникали
        * ошибки выполняется проверка на существование столбцов в наборе
        *      dataRow.Table.Columns.Contains("CheckLastName")
        *      dataRow.Table.Columns.Contains("CheckFirstName")
        *      dataRow.Table.Columns.Contains("CheckPatronymicName")
        */
        #region Public Methods and Operators

        /// <summary>
        /// открытая это ФБС или открытая
        /// </summary>
        protected bool IsSystemOpen
        {
            get
            {
                string appSetting = ConfigurationManager.AppSettings["EnableOpenedFbs"];
                return !string.IsNullOrEmpty(appSetting) && Convert.ToBoolean(appSetting);     
            }
        }

        /// <summary>
        /// The export.
        /// </summary>
        /// <param name="dataSet">
        /// The data set.
        /// </param>
        /// <param name="writer">
        /// The writer.
        /// </param>
        public void Export(IEnumerable<DataRow> dataSet, StreamWriter writer)
        {
           

            this.WriteHeader(writer);
            writer.Flush();
            foreach (DataRow dataRow in dataSet)
            {
                var certificateNumber = dataRow["CertificateNumber"] as string;
                string certificateNumberString = Convert.ToBoolean(dataRow["IsExist"])
                                                     ? !Convert.ToBoolean(dataRow["IsDeny"])
                                                           ? "Нет свидетества".Equals (certificateNumber)
                                                              ? "Нет свидетельства"
                                                              : certificateNumber
                                                           : string.Format(
                                                               "АННУЛИРОВАНО {0}: {1}. Актуальное свидетельство: {2}", 
                                                               certificateNumber, 
                                                               dataRow["DenyComment"], 
                                                               dataRow["DenyNewCertificateNumber"])
                                                     : string.Format("НЕ НАЙДЕНО{0}", "Нет свидетества".Equals (certificateNumber) || "Нет свидетельства".Equals (certificateNumber) ? "" : " " + certificateNumber);
                writer.Write(certificateNumberString);
                writer.Write("%" + dataRow["TypographicNumber"]);

                if (!this.IsSystemOpen)
                {
                    writer.Write(
                        "%"
                        +
                        (string.IsNullOrEmpty(dataRow["LastName"].ToString())
                             ? dataRow.Table.Columns.Contains("CheckLastName")
                                   ? this.NameToLower(dataRow["CheckLastName"].ToString())
                                   : this.NameToLower(dataRow["LastName"].ToString())
                             : this.NameToLower(dataRow["LastName"].ToString())));

                    writer.Write(
                        "%"
                        +
                        (string.IsNullOrEmpty(dataRow["FirstName"].ToString())
                             ? dataRow.Table.Columns.Contains("CheckFirstName")
                                   ? this.NameToLower(dataRow["CheckFirstName"].ToString())
                                   : this.NameToLower(dataRow["FirstName"].ToString())
                             : this.NameToLower(dataRow["FirstName"].ToString())));

                    writer.Write(
                        "%"
                        +
                        (string.IsNullOrEmpty(dataRow["PatronymicName"].ToString())
                             ? dataRow.Table.Columns.Contains("CheckPatronymicName")
                                   ? this.NameToLower(dataRow["CheckPatronymicName"].ToString())
                                   : this.NameToLower(dataRow["PatronymicName"].ToString())
                             : this.NameToLower(dataRow["PatronymicName"].ToString())));
                }

                writer.Write("%" + dataRow["PassportSeria"]);
                writer.Write("%" + dataRow["PassportNumber"]);
                writer.Write("%" + Convert.ToString(dataRow["RegionCode"]));
                writer.Write("%" + dataRow["SourceCertificateYear"]);
                writer.Write("%" + dataRow["Status"]);
                if (this.outputSubject1)
                {
                    this.WriteSubject(
                        writer, dataRow, "RussianMark", "RussianCheckMark", "RussianMarkIsCorrect", "RussianHasAppeal");
                }

                if (this.outputSubject2)
                {
                    this.WriteSubject(
                        writer, 
                        dataRow, 
                        "MathematicsMark", 
                        "MathematicsCheckMark", 
                        "MathematicsMarkIsCorrect", 
                        "MathematicsHasAppeal");
                }

                if (this.outputSubject3)
                {
                    this.WriteSubject(
                        writer, dataRow, "PhysicsMark", "PhysicsCheckMark", "PhysicsMarkIsCorrect", "PhysicsHasAppeal");
                }

                if (this.outputSubject4)
                {
                    this.WriteSubject(
                        writer, 
                        dataRow, 
                        "ChemistryMark", 
                        "ChemistryCheckMark", 
                        "ChemistryMarkIsCorrect", 
                        "ChemistryHasAppeal");
                }

                if (this.outputSubject5)
                {
                    this.WriteSubject(
                        writer, dataRow, "BiologyMark", "BiologyCheckMark", "BiologyMarkIsCorrect", "BiologyHasAppeal");
                }

                if (this.outputSubject6)
                {
                    this.WriteSubject(
                        writer, 
                        dataRow, 
                        "RussiaHistoryMark", 
                        "RussiaHistoryCheckMark", 
                        "RussiaHistoryMarkIsCorrect", 
                        "RussiaHistoryHasAppeal");
                }

                if (this.outputSubject7)
                {
                    this.WriteSubject(
                        writer, 
                        dataRow, 
                        "GeographyMark", 
                        "GeographyCheckMark", 
                        "GeographyMarkIsCorrect", 
                        "GeographyHasAppeal");
                }

                if (this.outputSubject8)
                {
                    this.WriteSubject(
                        writer, dataRow, "EnglishMark", "EnglishCheckMark", "EnglishMarkIsCorrect", "EnglishHasAppeal");
                }

                if (this.outputSubject9)
                {
                    this.WriteSubject(
                        writer, dataRow, "GermanMark", "GermanCheckMark", "GermanMarkIsCorrect", "GermanHasAppeal");
                }

                if (this.outputSubject10)
                {
                    this.WriteSubject(
                        writer, dataRow, "FranchMark", "FranchCheckMark", "FranchMarkIsCorrect", "FranchHasAppeal");
                }

                if (this.outputSubject11)
                {
                    this.WriteSubject(
                        writer, 
                        dataRow, 
                        "SocialScienceMark", 
                        "SocialScienceCheckMark", 
                        "SocialScienceMarkIsCorrect", 
                        "SocialScienceHasAppeal");
                }

                if (this.outputSubject12)
                {
                    this.WriteSubject(
                        writer, 
                        dataRow, 
                        "LiteratureMark", 
                        "LiteratureCheckMark", 
                        "LiteratureMarkIsCorrect", 
                        "LiteratureHasAppeal");
                }

                if (this.outputSubject13)
                {
                    this.WriteSubject(
                        writer, dataRow, "SpanishMark", "SpanishCheckMark", "SpanishMarkIsCorrect", "SpanishHasAppeal");
                }

                if (this.outputSubject14)
                {
                    this.WriteSubject(
                        writer, 
                        dataRow, 
                        "InformationScienceMark", 
                        "InformationScienceCheckMark", 
                        "InformationScienceMarkIsCorrect", 
                        "InformationScienceHasAppeal");
                }

                string uniqueCheck = string.IsNullOrEmpty(dataRow["UniqueIHEaFCheck"].ToString())
                                         ? "0"
                                         : dataRow["UniqueIHEaFCheck"].ToString();
                writer.Write("%" + uniqueCheck);
                writer.WriteLine();
                writer.Flush();
            }
        }

        #endregion

        #region Methods

        private string NameToLower(string text)
        {
            return string.IsNullOrEmpty(text)
                       ? string.Empty
                       : text.Trim().Replace(
                           text.Trim(), 
                           text.Trim().Substring(0, 1).ToUpper()
                           + text.Trim().Substring(1, text.Trim().Length - 1).ToLower());
        }

        private void WriteFooter(StreamWriter writer)
        {
            writer.WriteLine(
                "Комментарий: Специальным знаком «!» перед баллом выделены баллы, которые меньше минимальных значений, установленных Федеральной службой по надзору в сфере образования и науки (Рособрнадзор). С Приказами Рособрнадзора о применении шкалы минимальных баллов за соответствующие годы можно ознакомиться в разделе «Документы» Подсистемы ФИС Результаты ЕГЭ");
            writer.WriteLine(
                "Комментарий: Срок действия свидетельств ЕГЭ определяется в соответствии с действующими нормативными документами. С нормативными документами можно ознакомиться в разделе «Документы» Подсистемы ФИС Результаты ЕГЭ");
        }

        private void WriteHeader(StreamWriter writer)
        {
            writer.Write(
                "Номер свидетельства%Типографский номер%{0}Серия документа%Номер документа%Регион%Год%Статус", this.IsSystemOpen? string.Empty : "Фамилия%Имя%Отчество%");
            if (this.outputSubject1)
            {
                writer.Write("%Русский язык%Апелляция");
            }

            if (this.outputSubject2)
            {
                writer.Write("%Математика%Апелляция");
            }

            if (this.outputSubject3)
            {
                writer.Write("%Физика%Апелляция");
            }

            if (this.outputSubject4)
            {
                writer.Write("%Химия%Апелляция");
            }

            if (this.outputSubject5)
            {
                writer.Write("%Биология%Апелляция");
            }

            if (this.outputSubject6)
            {
                writer.Write("%История России%Апелляция");
            }

            if (this.outputSubject7)
            {
                writer.Write("%География%Апелляция");
            }

            if (this.outputSubject8)
            {
                writer.Write("%Английский язык%Апелляция");
            }

            if (this.outputSubject9)
            {
                writer.Write("%Немецкий язык%Апелляция");
            }

            if (this.outputSubject10)
            {
                writer.Write("%Французский язык%Апелляция");
            }

            if (this.outputSubject11)
            {
                writer.Write("%Обществознание%Апелляция");
            }

            if (this.outputSubject12)
            {
                writer.Write("%Литература%Апелляция");
            }

            if (this.outputSubject13)
            {
                writer.Write("%Испанский язык%Апелляция");
            }

            if (this.outputSubject14)
            {
                writer.Write("%Информатика%Апелляция");
            }

            writer.Write("%Проверок ВУЗами и их филиалами");
            writer.WriteLine();
        }

        private void WriteSubject(
            StreamWriter writer, 
            DataRow dataRow, 
            string valueColumn, 
            string checkValueColumn, 
            string isCorrectColumn, 
            string isAppealedColumn)
        {
            string value = Convert.ToString(dataRow[valueColumn]);
            int isAppealed = Convert.IsDBNull(dataRow[isAppealedColumn])
                             || !Convert.ToBoolean(dataRow[isAppealedColumn])
                                 ? 0
                                 : 1;
            string valueString = string.Empty;
            if (this.OutputCheckedValues)
            {
                string checkValue = Convert.ToString(dataRow[checkValueColumn]);
                bool isCorrect = Convert.ToBoolean(dataRow[isCorrectColumn]);
                if (string.IsNullOrEmpty(value) && string.IsNullOrEmpty(checkValue))
                {
                    valueString = string.Empty;
                }
                else if (isCorrect)
                {
                    valueString = value;
                }
                else
                {
                    valueString = string.Format("Ошибка {0} ({1})", checkValue, value);
                }
            }
            else if (!string.IsNullOrEmpty(value))
            {
                valueString = value;
            }

            writer.Write("%{0}%{1}", valueString, isAppealed);
        }

        #endregion
    }
}