namespace Ege.Check.Logic.Helpers
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using JetBrains.Annotations;

    public class ExpressionHelper : IExpressionHelper
    {
        public Func<T, object> Getter<T>([NotNull] PropertyInfo pi)
        {
            var param = Expression.Parameter(typeof (T), "value");
            var prop = Expression.Property(param, pi);
            var resExpr = pi.PropertyType.IsValueType
                              ? (Expression) Expression.Convert(prop, typeof (object))
                              : prop;
            return Expression.Lambda<Func<T, object>>(resExpr, param).Compile();
        }
    }
}