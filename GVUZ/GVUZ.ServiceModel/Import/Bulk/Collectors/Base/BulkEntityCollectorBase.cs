using System.Collections.Generic;
using System.Data;
using System.Linq;
using GVUZ.ServiceModel.Import.Bulk.Extensions;
using GVUZ.ServiceModel.Import.Bulk.Infrastructure;
using GVUZ.ServiceModel.Import.Bulk.Model.Base;

namespace GVUZ.ServiceModel.Import.Bulk.Collectors.Base
{
    /// <summary>
    ///     Базовый класс обработчиков подготавливающих объекты к BULK загрузке в БД
    /// </summary>
    /// <typeparam name="TSource">Тип коллекции источника данных</typeparam>
    /// <typeparam name="TResult">Результирующий тип BULK данных</typeparam>
    public abstract class BulkEntityCollectorBase<TSource, TResult> : IBulkEntityCollector
        where TSource : class
        where TResult : IBulkEntity
    {
        protected readonly IEnumerable<TSource> _collection;
        protected readonly int _institutionId;
        protected readonly int _packageId;

        protected BulkEntityCollectorBase(IEnumerable<TSource> collection, int packageId, int institutionId)
        {
            _collection = collection;
            _packageId = packageId;
            _institutionId = institutionId;
        }

        public virtual string DestinationTableName
        {
            get { return typeof (TResult).GetDestinationTableName(); }
        }

        public abstract IEnumerable<IBulkEntity> Collect();

        public virtual IDataReader CollectToReader()
        {
            IEnumerable<IBulkEntity> collectedItems = Collect();
            return collectedItems.Any() ? collectedItems.OfType<TResult>().AsDataReader() : null;
        }
    }
}