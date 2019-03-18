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
        /// Initializes a new instance of the ReaderWriterLockSlim class with default property values.
        /// </summary>
        public RWLock ()
        {
        }

        /// <summary>	
        /// Gets the total number of unique threads that have entered the lock in read mode.
        /// </summary>
        public abstract int CurrentReadCount { get; }

        /// <summary>
        /// Gets a value that indicates whether the current thread has entered the lock in read mode.
        /// </summary>
        public abstract bool IsReadLockHeld { get; }

        /// <summary>
        /// Gets the total number of threads that are waiting to enter the lock in read mode.
        /// </summary>
        public abstract int WaitingReadCount { get; }

        /// <summary>
        /// Gets a value that indicates whether the current thread has entered the lock in write mode.
        /// </summary>
        public abstract bool IsWriteLockHeld { get; }

        /// <summary>
        /// Gets the total number of threads that are waiting to enter the lock in write mode.
        /// </summary>
        public abstract int WaitingWriteCount { get; }

        /// <summary>
        /// Tries to enter the lock in read mode.
        /// </summary>
        public abstract void EnterReadLock();

        /// <summary>
        /// Tries to enter the lock in write mode.
        /// 1 test currently
        /// </summary>
        public abstract void EnterWriteLock();

        /// <summary>
        /// Reduces the recursion count for read mode, and exits read mode if the resulting count is 0 (zero)
        /// </summary>
        public abstract void ExitReadLock();

        /// <summary>
        /// Reduces the recursion count for write mode, and exits write mode if the resulting count is 0 (zero).
        /// 1 test currently
        /// </summary>
        public abstract void ExitWriteLock();

        /// <summary>	
        /// Tries to enter the lock in read mode, with an optional integer time-out.
        /// </summary>
        public abstract bool TryEnterReadLock(int millisecondsTimeout);

        /// <summary>
        /// Tries to enter the lock in write mode, with an optional time-out.
        /// </summary>
        public abstract bool TryEnterWriteLock(int millisecondsTimeout);

        /// <summary>
        /// Releases all resources used by the current instance of the ReaderWriterLockSlim class.
        /// </summary>
        public abstract void Dispose();
    }
}
