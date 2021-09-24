using System.Collections.Generic;
using System.Data;
using GVUZ.ServiceModel.Import.Bulk.Model.Base;

namespace GVUZ.ServiceModel.Import.Bulk.Collectors.Base
{
    public interface IBulkEntityCollector
    {
        string DestinationTableName { get; }
        IEnumerable<IBulkEntity> Collect();
        IDataReader CollectToReader();
    }
}