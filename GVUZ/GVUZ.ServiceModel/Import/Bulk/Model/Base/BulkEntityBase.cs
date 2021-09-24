using System;

namespace GVUZ.ServiceModel.Import.Bulk.Model.Base
{
    public abstract class BulkEntityBase : IBulkEntity
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public string UID { get; set; }
        public int ImportPackageId { get; set; }
        public int InstitutionId { get; set; }
    }
}