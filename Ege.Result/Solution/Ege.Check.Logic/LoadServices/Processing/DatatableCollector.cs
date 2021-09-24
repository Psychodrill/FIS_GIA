namespace Ege.Check.Logic.LoadServices.Processing
{
    using System;
    using System.Collections.Concurrent;
    using System.Data;
    using System.Linq;
    using System.Reflection;
    using Ege.Check.Common.Extensions;
    using Ege.Check.Logic.Helpers;
    using JetBrains.Annotations;
    using global::Common.Logging;

    internal class DatatableCollector<TDto> : IDatatableCollector<TDto>
        where TDto : class
    {
        [NotNull] private readonly ConcurrentDictionary<PropertyInfo, Func<TDto, object>> _cache;
        [NotNull] private readonly ILog _log;
        [NotNull] private readonly PropertyInfo[] _props;
        [NotNull] private readonly Type _type;

        public DatatableCollector([NotNull] IExpressionHelper expressionHelper)
        {
            _type = typeof (TDto);
            _props = _type.GetProperties();
            _cache = new ConcurrentDictionary<PropertyInfo, Func<TDto, object>>();
            foreach (var propertyInfo in _props)
            {
                var getter = expressionHelper.Getter<TDto>(propertyInfo);
                _cache.AddOrUpdate(propertyInfo, getter, (info, func) => getter);
            }
            _log = LogManager.GetLogger(GetType());
        }

        public DataTable Create()
        {
            var dt = new DataTable(_type.Name);
            _log.TraceFormat("Created data table for {0}", _type.Name);
            foreach (var propertyInfo in _props)
            {
                Type realType;
                if (propertyInfo.PropertyType.TryGetNullableParameter(out realType))
                {
                    dt.Columns.Add(propertyInfo.Name, realType);
                    dt.Columns[propertyInfo.Name].AllowDBNull = true;
                }
                else
                {
                    dt.Columns.Add(propertyInfo.Name, propertyInfo.PropertyType);
                }
                _log.TraceFormat("Appended column to data table {0}. Name: {1}, type: {2}, allow null: {3}", _type.Name,
                                 propertyInfo.Name, dt.Columns[propertyInfo.Name].DataType,
                                 dt.Columns[propertyInfo.Name].AllowDBNull);
            }
            return dt;
        }

        public void AddRow(TDto dto, DataTable dt)
        {
            if (dto == null)
            {
                return;
            }
            dt.Rows.Add(GetPropertyValues(dto));
        }

        [NotNull]
        private object[] GetPropertyValues(TDto dto)
        {
            return _props.Select(pi => _cache[pi](dto)).ToArray();
        }
    }
}