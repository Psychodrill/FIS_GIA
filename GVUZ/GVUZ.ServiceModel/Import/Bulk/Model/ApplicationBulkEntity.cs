using System;
using GVUZ.ServiceModel.Import.Bulk.Attributes;
using GVUZ.ServiceModel.Import.Bulk.Model.Base;

namespace GVUZ.ServiceModel.Import.Bulk.Model
{
    [DestinationTableName("blk_Application")]
    public class ApplicationBulkEntity : BulkEntityBase, IEquatable<ApplicationBulkEntity>
    {
        public string EntrantUID { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string ApplicationNumber { get; set; }
        public bool? NeedHostel { get; set; }
        public int StatusId { get; set; }
        public DateTime? LastDenyDate { get; set; }
        public string StatusDecision { get; set; }
        public bool IsRequiresBudgetO { get; set; }
        public bool IsRequiresBudgetOZ { get; set; }
        public bool IsRequiresBudgetZ { get; set; }
        public bool IsRequiresPaidO { get; set; }
        public bool IsRequiresPaidOZ { get; set; }
        public bool IsRequiresPaidZ { get; set; }
        public bool IsRequiresTargetO { get; set; }
        public bool IsRequiresTargetOZ { get; set; }
        public bool IsRequiresTargetZ { get; set; }
        public bool? OriginalDocumentsReceived { get; set; }
        public DateTime? OriginalDocumentsReceivedDate { get; set; }
        public int? OrderOfAdmissionId { get; set; }
        public int? Priority { get; set; }

        #region IEquatable<ApplicationBulkEntity> Members
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as ApplicationBulkEntity);
        }

        public bool Equals(ApplicationBulkEntity other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            bool result = true;
            result &= other.RegistrationDate == RegistrationDate;
            result &= other.ApplicationNumber.Equals(ApplicationNumber);
            return result;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = 17;
                result = result * 37 + ApplicationNumber.GetHashCode();
                result = result * 37 + RegistrationDate.GetHashCode();
                return result;
            }
        }
        #endregion
    }
}
