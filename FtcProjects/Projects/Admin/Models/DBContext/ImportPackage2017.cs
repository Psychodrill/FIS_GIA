using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class ImportPackage2017
    {
        public int PackageId { get; set; }
        public int InstitutionId { get; set; }
        public int TypeId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastDateChanged { get; set; }
        public int StatusId { get; set; }
        public string Comment { get; set; }
        public string PackageData { get; set; }
        public string ProcessResultInfo { get; set; }
        public int? CheckStatusId { get; set; }
        public string CheckResultInfo { get; set; }
        public string ImportedAppIds { get; set; }
        public string UserLogin { get; set; }
        public string Content { get; set; }
        public DateTime? InProgressDate { get; set; }
        public DateTime? CompleteDate { get; set; }
        public DateTime? CheckInProgressDate { get; set; }
        public DateTime? CheckCompleteDate { get; set; }
    }
}
