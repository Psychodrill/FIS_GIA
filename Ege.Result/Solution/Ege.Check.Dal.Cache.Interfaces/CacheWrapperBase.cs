namespace Ege.Check.Dal.Cache.Interfaces
{
    using System;
    using System.Threading.Tasks;
    using JetBrains.Annotations;

    public abstract class CacheWrapperBase<TException>
        where TException: Exception
    {
        public abstract bool IsLivingCacheException([NotNull]TException exception);

        [NotNull]
        private CacheException Wrap([NotNull]TException ex)
        {
            return IsLivingCacheException(ex)
                ? new LivingCacheException(ex.Message, ex)
                : new CacheException(ex.Message, ex);
        }

        protected void ExecuteAndWrapException([NotNull]Action action)
        {
            try
            {
                action();
            }
            catch (TException ex)
            {
                throw Wrap(ex);
            }
        }

        protected T ExecuteAndWrapException<T>([NotNull] Func<T> func)
        {
            try
            {
                return func();
            }
            catch (TException ex)
            {
                throw Wrap(ex);
            }
        }

        protected async Task ExecuteAndWrapExceptionAsync([NotNull] Func<Task> action)
        {
            try
            {
                await action();
            }
            catch (TException ex)
            {
                throw Wrap(ex);
            }
        }

        protected async Task<T> ExecuteAndWrapExceptionAsync<T>([NotNull] Func<Task<T>> func)
        {
            try
            {
                return await func();
            }
            catch (TException ex)
            {
                throw Wrap(ex);
            }
        }
    }
}
