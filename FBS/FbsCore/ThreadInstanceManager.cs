using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Fbs.Core
{
    public class ThreadInstanceManager<T>
    {
        public delegate T InstanceCreateCallback();

        private object mLock = new object();
        private Hashtable mActiveInstances = new Hashtable();
        private Hashtable mLockCounts = new Hashtable();
        private Stack<T> mFreeInstances = new Stack<T>();
        private InstanceCreateCallback mCallback;

        private int LockCount
        {
            get
            {
                int lockCount = 0;
                if (!mLockCounts.Contains(Thread.CurrentThread))
                    mLockCounts.Add(Thread.CurrentThread, lockCount);
                else
                    lockCount = (int)mLockCounts[Thread.CurrentThread];
                return lockCount;
            }
            set
            {
                mLockCounts[Thread.CurrentThread] = value;
            }
        }

        private void ActivateInstance()
        {
            T mInstance;
            if (mFreeInstances.Count == 0)
            {
                mInstance = mCallback();
            }
            else
                mInstance = mFreeInstances.Pop();
            mActiveInstances.Add(Thread.CurrentThread, mInstance);
        }

        private void DeactivateInstance()
        {
            mFreeInstances.Push((T)mActiveInstances[Thread.CurrentThread]);
            mActiveInstances.Remove(Thread.CurrentThread);
        }

        public T Instance()
        {
            lock (mLock)
            {
                if (LockCount == 0)
                    throw new InvalidOperationException("Cannot use instance without locking.");
                return (T)mActiveInstances[Thread.CurrentThread];
            }
        }

        public void BeginLock()
        {
            lock (mLock)
            {
                if (LockCount == 0)
                    ActivateInstance();
                LockCount += 1;
            }
        }

        public void EndLock()
        {
            lock (mLock)
            {
                LockCount -= 1;
                if (LockCount == 0)
                    DeactivateInstance();
            }
        }

        public ThreadInstanceManager(InstanceCreateCallback callback)
        {
            mCallback = callback;
        }
    }
}
