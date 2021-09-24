using System.Reflection;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Collections;
using System.Threading;

namespace FbsService
{
    internal partial class TaskContext
    {
        static private ThreadInstanceManager<TaskContext> mInstanceManager =
                new ThreadInstanceManager<TaskContext>(CreateInstance);

        static private TaskContext CreateInstance()
        {
            TaskContext instance = new TaskContext();
            instance.ObjectTrackingEnabled = false;
            return instance;
        }

        static internal TaskContext Instance()
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
