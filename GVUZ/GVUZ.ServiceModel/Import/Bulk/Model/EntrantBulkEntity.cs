using System;
using GVUZ.ServiceModel.Import.Bulk.Attributes;
using GVUZ.ServiceModel.Import.Bulk.Model.Base;

namespace GVUZ.ServiceModel.Import.Bulk.Model
{
    [DestinationTableName("blk_Entrant")]
    public class EntrantBulkEntity : BulkEntityBase, IEquatable<EntrantBulkEntity>
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public int GenderId { get; set; }
        public string CustomInformation { get; set; }
        public string Snils { get; set; }

        #region IEquatable<ApplicationBulkEntity> Members
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as EntrantBulkEntity);
        }

        public bool Equals(EntrantBulkEntity other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            bool result = true;
            result &= other.UID.Equals(UID);
            result &= other.ImportPackageId == ImportPackageId;
            return result;
        }

        public override int GetHashCode()
        {
            unchecked 
            {
                int result = 17;
                result = result * 37 + UID.GetHashCode();
                result = result * 37 + ImportPackageId.ToString().GetHashCode();
                return result;
            }
        }
        #endregion
    }
}
