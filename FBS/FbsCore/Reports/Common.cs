using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Fbs.Core.Reports
{
    /// <summary>
    /// Таблица с привязкой к информации по хранимому в ней отчету
    /// </summary>
    public class DataTableWithTag : DataTable
    {
        ReportInfo Tag_;
        public ReportInfo Tag
        {
            get { return Tag_; }
            set { Tag_ = value; }
        }
    }

    /// <summary>
    /// Информация по отчету
    /// </summary>
    public class ReportInfo
    {
        string ExtractorMethodName_;
        /// <summary>
        /// Метод (Table-valued function) применяемый для извлечения данных
        /// </summary>
        public string ExtractorMethodName
        {
            get { return ExtractorMethodName_; }
            set { ExtractorMethodName_ = value; }
        }

        string ReportName_;
        /// <summary>
        /// Название отчета, отображаемое пользователю
        /// </summary>
        public string ReportName
        {
            get { return ReportName_; }
            set { ReportName_ = value; }
        }

        DateTime PeriodBegin_;
        /// <summary>
        /// Начало отчетного периода
        /// </summary>
        public DateTime PeriodBegin
        {
            get { return PeriodBegin_; }
            set { PeriodBegin_ = value; }
        }

        DateTime PeriodEnd_;
        /// <summary>
        /// Конец отчетного периода
        /// </summary>
        public DateTime PeriodEnd
        {
            get { return PeriodEnd_; }
            set { PeriodEnd_ = value; }
        }

        string AdditionalArg_;
        /// <summary>
        /// Дополнительный параметр (применим не ко всем SQL-методам)
        /// </summary>
        public string AdditionalArg
        {
            get { return AdditionalArg_; }
            set { AdditionalArg_ = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="extractorMethodName">Метод (Table-valued function) применяемый для извлечения данных</param>
        /// <param name="reportName">Название отчета</param>
        /// <param name="periodBegin">Начало отчетного периода</param>
        /// <param name="periodEnd">Конец отчетного периода</param>
        public ReportInfo(string extractorMethodName, string reportName, DateTime periodBegin, DateTime periodEnd)
            : this(extractorMethodName, reportName, periodBegin, periodEnd, null)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="extractorMethodName">Метод (Table-valued function) применяемый для извлечения данных</param>
        /// <param name="reportName">Название отчета</param>
        /// <param name="periodBegin">Начало отчетного периода</param>
        /// <param name="periodEnd">Конец отчетного периода</param>
        /// <param name="arg">Дополнительный аргумент</param>
        public ReportInfo(string extractorMethodName, string reportName, DateTime periodBegin, DateTime periodEnd, string arg)
        {
            ExtractorMethodName = extractorMethodName;
            ReportName = reportName;
            PeriodBegin = periodBegin;
            PeriodEnd = periodEnd;
            AdditionalArg_ = arg;
        }
    }
}
