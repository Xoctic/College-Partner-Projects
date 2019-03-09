// Written by Joseph Zachary for CS 3500
// Copyright Joseph Zachary, March 2019

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ReaderWriterLockClasses
{
    /// <summary>
    /// This class provides some of the constructors, properties, and methods provided by the
    /// System.Threading.ReaderWriterLockSlim class.  For the specification of ReaderWriterLockSlim,
    /// see https://docs.microsoft.com/en-us/dotnet/api/system.threading.readerwriterlockslim?view=netframework-4.6.1.
    /// 
    /// All locks that are created by this class are non-recursive.  A single Task cannot acquire the same lock for 
    /// a second time without having first released it.  This means that you can ignore the portions of the
    /// specification that refer to recursive locks or recursive mode.
    /// 
    /// None of the locks that are created by this class can be upgraded from reader to writer.  This means that you
    /// can ignore the portions of the specification that refer to upgrading.
    /// 
    /// To create RWLocks, use the factory method RWLockBuilder.NewLock().
    /// </summary>
    public abstract class RWLock : IDisposable
    {
        /// <summary>
        /// See ReaderWriterLockSlim for specification.
        /// </summary>
        public RWLock ()
        {
        }

        /// <summary>
        /// See ReaderWriterLockSlim for specification.
        /// </summary>
        public abstract int CurrentReadCount { get; }

        /// <summary>
        /// See ReaderWriterLockSlim for specification.
        /// </summary>
        public abstract bool IsReadLockHeld { get; }

        /// <summary>
        /// See ReaderWriterLockSlim for specification.
        /// </summary>
        public abstract int WaitingReadCount { get; }

        /// <summary>
        /// See ReaderWriterLockSlim for specification.
        /// </summary>
        public abstract bool IsWriteLockHeld { get; }

        /// <summary>
        /// See ReaderWriterLockSlim for specification.
        /// </summary>
        public abstract int WaitingWriteCount { get; }

        /// <summary>
        /// See ReaderWriterLockSlim for specification.
        /// </summary>
        public abstract void EnterReadLock();

        /// <summary>
        /// See ReaderWriterLockSlim for specification.
        /// </summary>
        public abstract void EnterWriteLock();

        /// <summary>
        /// See ReaderWriterLockSlim for specification.
        /// </summary>
        public abstract void ExitReadLock();

        /// <summary>
        /// See ReaderWriterLockSlim for specification.
        /// </summary>
        public abstract void ExitWriteLock();

        /// <summary>
        /// See ReaderWriterLockSlim for specification.
        /// </summary>
        public abstract bool TryEnterReadLock(int millisecondsTimeout);

        /// <summary>
        /// See ReaderWriterLockSlim for specification.
        /// </summary>
        public abstract bool TryEnterWriteLock(int millisecondsTimeout);

        /// <summary>
        /// See ReaderWriterLockSlim for specification.
        /// </summary>
        public abstract void Dispose();
    }
}
