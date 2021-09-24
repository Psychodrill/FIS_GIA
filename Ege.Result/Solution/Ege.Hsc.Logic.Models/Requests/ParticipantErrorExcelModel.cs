namespace Ege.Hsc.Logic.Models.Requests
{
    using System.Collections.Generic;
    using JetBrains.Annotations;
    using OfficeOpenXml.Style;
    using TsSoft.Commons.Collections;
    using TsSoft.Excel.Attributes;
    using TsSoft.Excel.Stylizers;

    public class ParticipantErrorExcelModel
    {
        [ExcelCell(Description = "Имя участника")]
        public string Name { get; set; }

        [ExcelCell(Description = "Номер документа")]
        public string Document { get; set; }

        [ExcelCell(Description = "Регион")]
        public string Region { get; set; }

        [ExcelCell(Description = "Ошибка")]
        public string ErrorString { get { return Enums.GetEnumDescription(ErrorStatus); } }

        public SingleParticipantRequestStatus ErrorStatus { get; set; }
    }

    [ExcelModel(WorkbookName = "Ошибки")]
    public class ParticipantErrorCollectionExcelModel
    {
        [ExcelTable(typeof(ErrorRowStylizer))]
        [AutoFit(AutoFitType.StretchColumns)]
        [HeaderAutoFit(AutoFitType.StretchColumns)]
        [NotNull]
        public ICollection<ParticipantErrorExcelModel> Errors { get; set; }
    }

    public class ErrorRowStylizer : ITableRowStylizer<ParticipantErrorExcelModel>
    {
        public void Apply(ExcelStyle style)
        {
            style.Font.Bold = true;
        }

        public bool ShouldApply(ParticipantErrorExcelModel model)
        {
            return model.ErrorStatus == SingleParticipantRequestStatus.Collision;
        }
    }
}
