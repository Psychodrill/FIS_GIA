using System;
using GVUZ.ServiceModel.Import.Bulk.Attributes;
using GVUZ.ServiceModel.Import.Bulk.Model.Base;

namespace GVUZ.ServiceModel.Import.Bulk.Model
{
    [DestinationTableName("blk_ConsideredApplication")]
    public class ConsideredApplicationBulkEntity : BulkEntityBase, IEquatable<ConsideredApplicationBulkEntity>
    {
        public DateTime RegistrationDate { get; set; }
        public string ApplicationNumber { get; set; }
        public int DirectionID { get; set; }
        public int EducationLevelID { get; set; }
        public int EducationFormID { get; set; }
        public int FinanceSourceID { get; set; }
        public bool IsRequiresBudgetO { get; set; }
        public bool IsRequiresBudgetOZ { get; set; }
        public bool IsRequiresBudgetZ { get; set; }
        public bool IsRequiresPaidO { get; set; }
        public bool IsRequiresPaidOZ { get; set; }
        public bool IsRequiresPaidZ { get; set; }
        public bool IsRequiresTargetO { get; set; }
        public bool IsRequiresTargetOZ { get; set; }
        public bool IsRequiresTargetZ { get; set; }

        #region IEquatable<ConsideredApplicationBulkEntity> Members
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as ConsideredApplicationBulkEntity);
        }

        public bool Equals(ConsideredApplicationBulkEntity other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            bool result = true;
            result &= other.RegistrationDate == RegistrationDate;
            result &= other.ApplicationNumber.Equals(ApplicationNumber);
            result &= other.DirectionID == DirectionID;
            result &= other.EducationLevelID == EducationLevelID;
            result &= other.EducationFormID == EducationFormID;
            result &= other.FinanceSourceID == FinanceSourceID;
            return result;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = 17;
                result = result * 37 + ApplicationNumber.GetHashCode();
                result = result * 37 + RegistrationDate.GetHashCode();
                result = result * 37 + DirectionID.GetHashCode();
                result = result * 37 + EducationLevelID.GetHashCode();
                result = result * 37 + EducationFormID.GetHashCode();
                result = result * 37 + FinanceSourceID.GetHashCode();
                return result;
            }
        }
        #endregion
    }
}
