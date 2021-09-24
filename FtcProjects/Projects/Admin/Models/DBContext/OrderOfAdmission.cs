using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class OrderOfAdmission
    {
        public OrderOfAdmission()
        {
            Application = new HashSet<Application>();
            ApplicationCompetitiveGroupItemOrderOfAdmission = new HashSet<ApplicationCompetitiveGroupItem>();
            ApplicationCompetitiveGroupItemOrderOfException = new HashSet<ApplicationCompetitiveGroupItem>();
            OrderOfAdmissionHistory = new HashSet<OrderOfAdmissionHistory>();
        }

        public int OrderId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateEdited { get; set; }
        public DateTime? DatePublished { get; set; }
        public int InstitutionId { get; set; }
        public string Uid { get; set; }
        public int CampaignId { get; set; }
        public short? EducationLevelId { get; set; }
        public short? EducationFormId { get; set; }
        public short? EducationSourceId { get; set; }
        public short? Stage { get; set; }
        public bool IsForBeneficiary { get; set; }
        public bool IsForeigner { get; set; }
        public string OrderName { get; set; }
        public string OrderNumber { get; set; }
        public DateTime? OrderDate { get; set; }
        public int OrderOfAdmissionStatusId { get; set; }
        public int OrderOfAdmissionTypeId { get; set; }

        public virtual Campaign Campaign { get; set; }
        public virtual AdmissionItemType EducationForm { get; set; }
        public virtual AdmissionItemType EducationLevel { get; set; }
        public virtual AdmissionItemType EducationSource { get; set; }
        public virtual Institution Institution { get; set; }
        public virtual OrderOfAdmissionStatus OrderOfAdmissionStatus { get; set; }
        public virtual OrderOfAdmissionType OrderOfAdmissionType { get; set; }
        public virtual ICollection<Application> Application { get; set; }
        public virtual ICollection<ApplicationCompetitiveGroupItem> ApplicationCompetitiveGroupItemOrderOfAdmission { get; set; }
        public virtual ICollection<ApplicationCompetitiveGroupItem> ApplicationCompetitiveGroupItemOrderOfException { get; set; }
        public virtual ICollection<OrderOfAdmissionHistory> OrderOfAdmissionHistory { get; set; }
    }
}
