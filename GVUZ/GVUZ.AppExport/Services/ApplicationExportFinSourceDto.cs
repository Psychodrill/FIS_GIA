using System.Collections.Generic;

namespace GVUZ.AppExport.Services
{
    public class ApplicationExportFinSourceDto
    {
        public long FinanceSourceId { get; set; }
        public long EducationFormId { get; set; }
        public long EducationLevelId { get; set; }
        public long DirectionId { get; set; }
        public string Number { get; set; }
        public long? OrderTypeId { get; set; }
        public long? IsForBeneficiary { get; set; }
        public long? UseBeneficiarySubject { get; set; }
        public long? CommonBeneficiaryDocTypeId { get; set; }
        private ICollection<ApplicationExportEntranceTestDto> _entranceTestResults;

        public ICollection<ApplicationExportEntranceTestDto> EntranceTestResults
        {
            get { return _entranceTestResults ?? (_entranceTestResults = new List<ApplicationExportEntranceTestDto>()); }
            set { _entranceTestResults = value; }
        }
    }
}