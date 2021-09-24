using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Configuration;

namespace FbsService
{
    internal class TaskManager
    {
        private class TaskThreadState
        {
            private Task mTask;
            private Thread mThread;

            public TaskThreadState(Thread thread)
            {
                mThread = thread;
            }

            private log4net.ILog Logger
            {
                get
                {
                    return TaskManager.Instance().mLogger;
                }
            }

            public void ExecuteTask()
            {
                mTask = TaskManager.Instance().GetTask();
                try
                {
                    if (mTask != null)
                    {
                        string statusCode = mTask.InternalStatus;
                        if (!TaskManager.Instance().IsCycledTask(mTask))
                            Logger.InfoFormat("Executing Task {0}: {1}...", mTask.Id, mTask.InternalStatus);
                        mTask.ExecuteStep();
                        if (statusCode != mTask.InternalStatus)
                            TaskManager.Instance().RemoveCycledTask(mTask);
                        else
                            TaskManager.Instance().AddCycledTask(mTask);
                    }
                }
                finally
                {
                    TaskManager.Instance().ReleaseUsingTask(mTask);
                    mTask = null;
                }
            }

            public bool IsIdle()
            {
                return mTask == null;
            }

            public bool IsExecutingTask(Task task)
            {
                return mTask == task;
            }
        }

        private const string LoggerName = "Task";
        private const string TaskThreadNameFormat = "TaskThread{0}";
        private static TimeSpan FirstTimeTimeout = TimeSpan.Parse("00:00:15");

        static private TaskManager mInstance;
        static private object mTaskLock = new object();
        
        private List<Task> mTasks = new List<Task> { };
        private List<Task> mUsingTasks = new List<Task> { };
        private List<Thread> mThreads = new List<Thread>();
        private List<ThreadStart> mThreadStarts = new List<ThreadStart>();
        private List<TaskThreadState> mThreadStates = new List<TaskThreadState>();
        private List<long> mCycledTaskIds = new List<long>();
        private DateTime mLastUpdateTaskDate = DateTime.MinValue;
        protected log4net.ILog mLogger = null;

        private TaskManager()
        {
            mLogger = LogManager.GetLogger(LoggerName);
        }

        public static TaskManager Instance()
        {
            if (mInstance == null)
                mInstance = new TaskManager();
            return mInstance;
        }

        public log4net.ILog Logger
        {
            get
            {
                return mLogger;
            }
        }

        private TimeSpan GetThreadIdleTimeout()
        {
            TimeSpan result = new TimeSpan(mLastUpdateTaskDate.Add(
                    TaskService.ConfigThreadIdleTimeout).Ticks - DateTime.Now.Ticks);
            if (result.Ticks > 0)
                return result;
            return TimeSpan.Zero;
        }
        
        private void RefreshTasks()
        {
            if (GetThreadIdleTimeout() != TimeSpan.Zero)
                return;
            mTasks = Task.GetActiveTasks(TaskService.ConfigThreadCount).ToList();
            foreach (Task task in mUsingTasks)
                if (mTasks.Contains(task))
                    mTasks.Remove(task);
            mLastUpdateTaskDate = DateTime.Now;
        }

        private Task GetTask()
        {
            lock (mTaskLock)
            {
                if (TaskService.ServiceStopping)
                    return null;
                RefreshTasks();
                if (TaskService.ServiceStopping)
                    return null;

                if (mTasks.Count == 0)
                    return null;
                Task result = mTasks[0];
                mTasks.RemoveAt(0);
                mUsingTasks.Add(result);
                return result;
            }
        }

        private void ReleaseUsingTask(Task task)
        {
            lock (mTaskLock)
            {
                mUsingTasks.Remove(task);
            }
        }

        private void AddCycledTask(Task task)
        {
            lock (mTaskLock)
            {
                if (!mCycledTaskIds.Contains(task.Id))
                {
                    mLogger.InfoFormat("Cycle executing for Task {0} started...", task.Id);
                    mCycledTaskIds.Add(task.Id);
                }
            }
        }

        private void RemoveCycledTask(Task task)
        {
            lock (mTaskLock)
            {
                if (mCycledTaskIds.Contains(task.Id))
                {
                    mCycledTaskIds.Remove(task.Id);
                    mLogger.InfoFormat("Cycle executing for Task {0} finished.", task.Id);
                }
            }
        }

        private bool IsCycledTask(Task task)
        {
            lock (mTaskLock)
            {
                return mCycledTaskIds.Contains(task.Id);
            }
        }

        public void BeginExecuteTasks()
        {
            for (int ind = 0; ind < TaskService.ConfigThreadCount; ind++)
            {
                ThreadStart startThread = new ThreadStart(this.RunThread);
                mThreadStarts.Add(startThread);
                Thread thread = new Thread(startThread);
                thread.Name = string.Format(TaskThreadNameFormat, ind + 1);
                thread.Start();
                mThreads.Add(thread);
            }
        }

        public void EndExecuteTasks()
        {
            // Жесткий останов
            foreach (Thread thread in mThreads)
                if (thread != null && thread.IsAlive)
                    thread.Abort();
        }

        public bool HasExecutingTask()
        {
            foreach (TaskThreadState state in mThreadStates)
                if (!state.IsIdle())
                    return false;
            return true;
        }

        private void RunThread()
        {
            TaskThreadState state = new TaskThreadState(Thread.CurrentThread);
            mThreadStates.Add(state);
            try
            {
                mLogger.WarnFormat("Thread {0} started...", Thread.CurrentThread.Name);
                Thread.Sleep(FirstTimeTimeout);
                while (!TaskService.ServiceStopping)
                {
                    try
                    {
                        state.ExecuteTask();
                    }
                    catch (ThreadAbortException)
                    {
                        throw;
                    }
                    catch (Exception ex)
                    {
                        mLogger.Error("Error during executing Task.", ex);
                        TaskService.Logger.Warn("Exception occured in Task threads, see that log for details");
                    }

                    if (!TaskService.ServiceStopping)
                        Thread.Sleep(GetThreadIdleTimeout());
                }
            }
            catch (ThreadAbortException)
            {
                mLogger.WarnFormat("Thread {0} aborting", Thread.CurrentThread.Name);
            }
            catch (Exception ex)
            {
                mLogger.Error(ex);
                TaskService.Logger.Warn("Exception occured in Task threads, see that log for details");
            }
            finally
            {
                mThreadStates.Remove(state);
                mLogger.WarnFormat("Task thread {0} finished.", Thread.CurrentThread.Name);
            }
        }

    }
}
