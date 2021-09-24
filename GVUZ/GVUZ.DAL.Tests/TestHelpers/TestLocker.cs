using System.Threading;

namespace GVUZ.DAL.Tests.TestHelpers
{
    internal static class TestLocker
    {
        private static readonly object _lock = new object();

        public static void Lock()
        {
            Monitor.Enter(_lock);
        }

        public static void Unlock()
        {
            Monitor.Exit(_lock);
        }
    }
}
