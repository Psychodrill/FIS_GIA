using System.Diagnostics;
using System.Threading;

namespace FogSoft.Helpers.Threading
{
    /// <summary>
    ///     Simplifies <see cref="ReaderWriterLockSlim" /> management.
    /// </summary>
    /// <remarks>
    ///     Based on http://www.nobletech.co.uk/Articles/ReaderWriterLockMgr.aspx.
    ///     This article additionally describes why we cannot write more simple wrapper like "ReaderLock". Also see usage example below.
    /// </remarks>
    /// <example>
    ///     ReaderWriterLockSlim myLock = new ReaderWriterLockSlim();
    ///     using (ReaderWriterLocker locker = new ReaderWriterLocker(myLock))
    ///     {
    ///     locker.EnterReadLock();
    ///     // ...
    ///     }
    ///     using (ReaderWriterLocker locker1 = new ReaderWriterLocker(myLock))
    ///     {
    ///     locker1.EnterUpgradeableReadLock();
    ///     // ...
    ///     using (ReaderWriterLocker locker2 = new ReaderWriterLocker(myLock))
    ///     {
    ///     locker2.EnterWriteLock();
    ///     // ...
    ///     }
    ///     }
    /// </example>
    [DebuggerStepThrough]
    public class ReaderWriterLocker : Disposable
    {
        private LockTypes _enteredLockType = LockTypes.None;
        private ReaderWriterLockSlim _readerWriterLock;

        public ReaderWriterLocker(ReaderWriterLockSlim readerWriterLock)
        {
            _readerWriterLock = readerWriterLock;
        }

        public void EnterReadLock()
        {
            _readerWriterLock.EnterReadLock();
            _enteredLockType = LockTypes.Read;
        }

        public void EnterWriteLock()
        {
            _readerWriterLock.EnterWriteLock();
            _enteredLockType = LockTypes.Write;
        }

        public void EnterUpgradeableReadLock()
        {
            _readerWriterLock.EnterUpgradeableReadLock();
            _enteredLockType = LockTypes.Upgradeable;
        }

        public void ExitLock()
        {
            if (_readerWriterLock == null)
                return;
            switch (_enteredLockType)
            {
                case LockTypes.Read:
                    _readerWriterLock.ExitReadLock();
                    _enteredLockType = LockTypes.None;
                    return;
                case LockTypes.Write:
                    _readerWriterLock.ExitWriteLock();
                    _enteredLockType = LockTypes.None;
                    return;
                case LockTypes.Upgradeable:
                    _readerWriterLock.ExitUpgradeableReadLock();
                    _enteredLockType = LockTypes.None;
                    return;
                default:
                    return;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (_readerWriterLock != null)
            {
                ExitLock();
                _readerWriterLock = null;
            }
        }

        private enum LockTypes
        {
            None,
            Read,
            Write,
            Upgradeable
        }
    }
}