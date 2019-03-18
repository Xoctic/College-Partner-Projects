// Written by Joseph Zachary for CS 3500
// Copyright Joseph Zachary, March 2019

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ReaderWriterLockClasses
{
    public class ReaderWriterLockSlimWrapper: RWLock
    {
        ReaderWriterLockSlim rwLock;

        /// <summary>
        /// Initializes a new instance of the ReaderWriterLockSlim class with default property values.
        /// </summary>
        public ReaderWriterLockSlimWrapper ()
        {
            rwLock = new ReaderWriterLockSlim();
        }

        /// <summary>	
        /// Gets the total number of unique threads that have entered the lock in read mode.
        /// </summary>
        public override int CurrentReadCount
        {
            get
            {
                return rwLock.CurrentReadCount;
            }
        }

        /// <summary>
        /// Gets a value that indicates whether the current thread has entered the lock in read mode.
        /// </summary>
        public override bool IsReadLockHeld
        {
            get
            {
                return rwLock.IsReadLockHeld;
            }
        }

        /// <summary>
        /// Gets the total number of threads that are waiting to enter the lock in read mode.
        /// </summary>
        public override int WaitingReadCount
        {
            get
            {
                return rwLock.WaitingReadCount;
            }
        }

        /// <summary>
        /// Gets a value that indicates whether the current thread has entered the lock in write mode.
        /// </summary>
        public override bool IsWriteLockHeld
        {
            get
            {
                return rwLock.IsWriteLockHeld;
            }
        }

        /// <summary>
        /// Gets the total number of threads that are waiting to enter the lock in write mode.
        /// </summary>
        public override int WaitingWriteCount
        {
            get
            {
                return rwLock.WaitingWriteCount;
            }
        }

        /// <summary>
        /// Tries to enter the lock in read mode.
        /// </summary>
        public override void EnterReadLock()
        {
            rwLock.EnterReadLock();
        }

        /// <summary>
        /// Tries to enter the lock in write mode.
        /// </summary>
        public override void EnterWriteLock()
        {
            rwLock.EnterWriteLock();
        }

        /// <summary>
        /// Reduces the recursion count for read mode, and exits read mode if the resulting count is 0 (zero)
        /// </summary>
        public override void ExitReadLock()
        {
            rwLock.ExitReadLock();
        }

        /// <summary>
        /// Reduces the recursion count for write mode, and exits write mode if the resulting count is 0 (zero).
        /// </summary>
        public override void ExitWriteLock()
        {
            rwLock.ExitWriteLock();
        }

        /// <summary>	
        /// Tries to enter the lock in read mode, with an optional integer time-out.
        /// </summary>
        public override bool TryEnterReadLock(int millisecondsTimeout)
        {
            return rwLock.TryEnterReadLock(millisecondsTimeout);
        }

        /// <summary>
        /// Tries to enter the lock in write mode, with an optional time-out.
        /// </summary>
        public override bool TryEnterWriteLock(int millisecondsTimeout)
        {
            return rwLock.TryEnterWriteLock(millisecondsTimeout);
        }

        /// <summary>
        /// Releases all resources used by the current instance of the ReaderWriterLockSlim class.
        /// </summary>
        public override void Dispose()
        {
            rwLock.Dispose();
        }
    }
}

