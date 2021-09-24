namespace Ege.Check.Logic.Helpers
{
    using System;
    using System.Reflection;

    public interface IExpressionHelper
    {
        Func<T, object> Getter<T>(PropertyInfo pi);
    }
}