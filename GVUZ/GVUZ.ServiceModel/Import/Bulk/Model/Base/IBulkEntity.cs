using System;

namespace GVUZ.ServiceModel.Import.Bulk.Model.Base
{
    /*
     * Сущности для Bulk загрузки в БД
     */

    public interface IBulkEntity
    {
        Guid Id { get; set; }
        Guid? ParentId { get; set; }
        int ImportPackageId { get; set; }
        string UID { get; set; }
        int InstitutionId { get; set; }
    }
}