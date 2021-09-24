using System.ComponentModel;

namespace GVUZ.ServiceModel.Import.WebService.Dto
{
    [Description("Приказ")]
    public class OrderOfAdmissionItemDto : BaseDto
    {
        public ApplicationRef Application;
        // информация о зачислении
        public string DirectionID;
        public string DirectionName;
        public string EducationFormID;
        public string EducationLevelID;
        public string FinanceSourceID;
        public string IsBeneficiary;
        public string Stage;
        public string OrderOfAdmissionUID;
        public bool? IsForeigner { get; set; }
        public string CompetitiveGroupUID { get; set; }
    }
}