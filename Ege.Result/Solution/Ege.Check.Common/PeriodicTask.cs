namespace Ege.Check.Common
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using JetBrains.Annotations;

    public class PeriodicTask
    {
        public static async Task Run([NotNull] Action action, TimeSpan period, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                action();
                await Task.Delay(period, cancellationToken);
            }
        }

        public static Task Run([NotNull] Action action, TimeSpan period)
        {
            return Run(action, period, CancellationToken.None);
        }
    }
}