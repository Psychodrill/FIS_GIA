using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2015.Dto.DataReaders
{
    public class BulkApplicationDto
    {
        public int EntrantID { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int InstitutionID { get; set; }


        public bool ApproveInstitutionCount { get; set; }
        public bool NeedHostel { get; set; }
        public bool FirstHigherEducation { get; set; }
        public bool ApprovePersonalData { get; set; }
        public bool FamiliarWithLicenseAndRules { get; set; }
        public bool FamiliarWithAdmissionType { get; set; }
        public bool FamiliarWithOriginalDocumentDeliveryDate { get; set; }
        public int StatusID { get; set; }
        public int WizardStepID { get; set; }
        public int ViolationID { get; set; }
        public string StatusDecision { get; set; }
        public DateTime LastCheckDate { get; set; }
        public string ViolationErrors { get; set; }
        public DateTime PublishDate { get; set; }
        public int SourceID { get; set; }
        public string ApplicationNumber { get; set; }
        public bool OriginalDocumentsReceived { get; set; }
        public int OrderCompetitiveGroupID { get; set; }
        public int OrderOfAdmissionID { get; set; }
        public int OrderCompetitiveGroupItemID { get; set; }
        public decimal OrderCalculatedRating { get; set; }
        public int OrderCalculatedBenefitID { get; set; }
        public int OrderEducationFormID { get; set; }
        public int OrderEducationSourceID { get; set; }
        public DateTime LastDenyDate { get; set; }
        public string UID { get; set; }
        public bool IsRequiresBudgetO { get; set; }
        public bool IsRequiresBudgetOZ { get; set; }
        public bool IsRequiresBudgetZ { get; set; }
        public bool IsRequiresPaidO { get; set; }
        public bool IsRequiresPaidOZ { get; set; }
        public bool IsRequresPaidZ { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime OriginalDocumentsReceivedDate { get; set; }
        public DateTime LastEgeDocumentsCheckDate { get; set; }
        public int OrderCompetitiveGroupTargetID { get; set; }
        public bool IsRequresTargetO { get; set; }
        public bool IsRequiesTargetOZ { get; set; }
        public bool IsRequiresTargetZ { get; set; }
        public Guid ApplicationGUID { get; set; }
        public int Priority { get; set; }
        public string EntrantUID { get; set; }
        public int ImportPackageID { get; set; }

    }
}
