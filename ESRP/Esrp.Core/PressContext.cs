namespace Esrp.Core
{
    partial class PressContext
    {
        static private ThreadInstanceManager<PressContext> mInstanceManager =
                new ThreadInstanceManager<PressContext>(CreateInstance);

        static private PressContext CreateInstance()
        {
            PressContext instance = new PressContext();
            instance.ObjectTrackingEnabled = false;
            instance.CommandTimeout = 0;
            return instance;
        }

        static internal PressContext Instance()
        {
            return mInstanceManager.Instance();
        }

        static internal void BeginLock()
        {
            mInstanceManager.BeginLock();
        }

        static internal void EndLock()
        {
            mInstanceManager.EndLock();
        }
    }
}