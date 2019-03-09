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

        public ReaderWriterLockSlimWrapper ()
        {
            rwLock = new ReaderWriterLockSlim();
        }

        /// <summary>
        /// See ReaderWriterLockSlim for specification.
        /// </summary>
        public override int CurrentReadCount
        {
            get
            {
                return rwLock.CurrentReadCount;
            }
        }

        /// <summary>
        /// See ReaderWriterLockSlim for specification.
        /// </summary>
        public override bool IsReadLockHeld
        {
            get
            {
                return rwLock.IsReadLockHeld;
            }
        }

        /// <summary>
        /// See ReaderWriterLockSlim for specification.
        /// </summary>
        public override int WaitingReadCount
        {
            get
            {
                return rwLock.WaitingReadCount;
            }
        }

        /// <summary>
        /// See ReaderWriterLockSlim for specification.
        /// </summary>
        public override bool IsWriteLockHeld
        {
            get
            {
                return rwLock.IsWriteLockHeld;
            }
        }

        /// <summary>
        /// See ReaderWriterLockSlim for specification.
        /// </summary>
        public override int WaitingWriteCount
        {
            get
            {
                return rwLock.WaitingWriteCount;
            }
        }

        /// <summary>
        /// See ReaderWriterLockSlim for specification.
        /// </summary>
        public override void EnterReadLock()
        {
            rwLock.EnterReadLock();
        }

        /// <summary>
        /// See ReaderWriterLockSlim for specification.
        /// </summary>
        public override void EnterWriteLock()
        {
            rwLock.EnterWriteLock();
        }

        /// <summary>
        /// See ReaderWriterLockSlim for specification.
        /// </summary>
        public override void ExitReadLock()
        {
            rwLock.ExitReadLock();
        }

        /// <summary>
        /// See ReaderWriterLockSlim for specification.
        /// </summary>
        public override void ExitWriteLock()
        {
            rwLock.ExitWriteLock();
        }

        /// <summary>
        /// See ReaderWriterLockSlim for specification.
        /// </summary>
        public override bool TryEnterReadLock(int millisecondsTimeout)
        {
            return rwLock.TryEnterReadLock(millisecondsTimeout);
        }

        /// <summary>
        /// See ReaderWriterLockSlim for specification.
        /// </summary>
        public override bool TryEnterWriteLock(int millisecondsTimeout)
        {
            return rwLock.TryEnterWriteLock(millisecondsTimeout);
        }

        /// <summary>
        /// See ReaderWriterLockSlim for specification.
        /// </summary>
        public override void Dispose()
        {
            rwLock.Dispose();
        }
    }
}

