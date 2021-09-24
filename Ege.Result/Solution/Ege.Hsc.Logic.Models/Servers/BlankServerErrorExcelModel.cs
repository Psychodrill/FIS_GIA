namespace Ege.Hsc.Logic.Models.Servers
{
    using System;
    using TsSoft.Excel.Attributes;

    /// <summary>
    /// Расхождение в данных БД и сервера бланков
    /// </summary>
    [ExcelModel]
    public class BlankServerErrorExcelModel
    {
        /// <summary>
        /// GUID участника / null, если информации нет в БД
        /// </summary>
        [ExcelCell(Description = "GUID участника")]
        [AutoFit(AutoFitType.StretchColumns)]
        [HeaderAutoFit(AutoFitType.StretchColumns)]
        public Guid? RbdId { get; set; }

        /// <summary>
        /// Хэш бланка
        /// </summary>
        [ExcelCell(Description = "Код бланка")]
        [AutoFit(AutoFitType.StretchColumns)]
        [HeaderAutoFit(AutoFitType.StretchColumns)]
        public string Code { get; set; }

        /// <summary>
        /// Дата экзамена
        /// </summary>
        [ExcelCell(Description = "Дата экзамена")]
        [HeaderAutoFit(AutoFitType.StretchColumns)]
        public DateTime ExamDate { get; set; }

        /// <summary>
        /// Причина: «Результат отсутствует в БД», «Бланки отсутствуют на сервере РЦОИ»
        /// </summary>
        [ExcelCell(Description = "Ошибка")]
        [AutoFit(AutoFitType.StretchColumns)]
        [HeaderAutoFit(AutoFitType.StretchColumns)]
        public string Error { get; set; }
    }
}