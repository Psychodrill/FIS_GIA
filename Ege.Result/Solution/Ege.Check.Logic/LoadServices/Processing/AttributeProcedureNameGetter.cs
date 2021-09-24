namespace Ege.Check.Logic.LoadServices.Processing
{
    using System;
    using System.Collections.Concurrent;
    using System.Reflection;
    using Ege.Check.Logic.Services.Dtos.Metadata;
    using Ege.Dal.Common.Helpers;
    using JetBrains.Annotations;

    internal class AttributeProcedureNameGetter : IProcedureNameGetter
    {
        [NotNull] private readonly ConcurrentDictionary<Type, string> _procedureNameCache =
            new ConcurrentDictionary<Type, string>();

        /// <exception cref="InvalidOperationException">Выбрасывается, если отсутствует атрибут</exception>
        public string GetName<TDto>()
        {
            var procedureName = _procedureNameCache.GetOrAdd(typeof (TDto), type =>
                {
                    var attr = type.GetCustomAttribute<BulkMergeProcedureAttribute>();
                    if (attr == null)
                    {
                        throw new InvalidOperationException(string.Format("Can not found attribute {0} at {1}", type,
                                                                          typeof (BulkMergeProcedureAttribute)));
                    }
                    return attr.ProcedureName;
                });
            return procedureName;
        }
    }
}