using System;

namespace GVUZ.ServiceModel.Import.Bulk.Extensions
{
    public static class MonadicExtensions
    {
        public static TResult With<TInput, TResult>(this TInput o, Func<TInput, TResult> evaluator)
            where TResult : class
            where TInput : class
        {
            return o == null ? null : evaluator(o);
        }

        public static TResult Return<TInput, TResult>(this TInput o, Func<TInput, TResult> evaluator,
                                                      TResult failureValue)
            where TInput : class
        {
            return o == null ? failureValue : evaluator(o);
        }
    }
}