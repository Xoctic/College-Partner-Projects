// Initial version written by Joseph Zachary for CS 3500
// Copyright Joseph Zachary, March 2019

// Don't test RWLockBuilder class
// Don't test Dispose
// Don't use directly the lock constructors

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using ReaderWriterLockClasses;

namespace ReaderWriterUnitTests
{
    [TestClass]
    public class RWLockTests
    {
        /// <summary>
        /// Verifies that an attempt to exit a write lock without having previously acquired a write
        /// lock results in a SynchronizationLockException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SynchronizationLockException))]
        public void TestMethod1()
        {
            // This is how you should create a lock in all of your test cases.
            RWLock rwLock = RWLockBuilder.NewLock();

            // This should result in an exception because a write lock was acquired first
            rwLock.ExitWriteLock();
        }
    }
}
