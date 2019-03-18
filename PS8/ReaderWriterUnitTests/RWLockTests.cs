// Initial version written by Joseph Zachary for CS 3500
// Copyright Joseph Zachary, March 2019

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Threading.Tasks;
using ReaderWriterLockClasses;

namespace ReaderWriterUnitTests
{
    [TestClass]
    public class RWLockTests
    {
        //This method runs a read lock
        public void GetReadLock(RWLock _rwLock, int _count, ManualResetEvent _mre)
        {
            // Acquire a read lock
            _rwLock.EnterReadLock();

            // Atomically decrement the shared count variable.  Note that merely doing count-- won't always work.
            Interlocked.Decrement(ref _count);

            // Block until the main task sets the mre to true.
            _mre.WaitOne();

            // Exit the read lock
            _rwLock.ExitReadLock();
        }

        // This method is runs a write lock
        private void GetWriteLock(RWLock _rwLock, int _count, ManualResetEvent _mre)
        {
            // Acquire a read lock
            _rwLock.EnterWriteLock();

            // Atomically decrement the shared count variable.  Note that merely doing count-- won't always work.
            Interlocked.Decrement(ref _count);

            // Block until the main task sets the mre to true.
            _mre.WaitOne();

            // Exit the read lock
            _rwLock.ExitWriteLock();
        }


        /// <summary>
        /// Verifies that an attempt to exit a write lock without having previously acquired a write
        /// lock results in a SynchronizationLockException.
        /// </summary>
        [TestMethod, Timeout(500)]
        [ExpectedException(typeof(SynchronizationLockException))]
        public void TestMethod1()
        {
            // This is how you should create a lock in all of your test cases.
            // Do not create locks any other way or your code will be ungradeable.
            RWLock rwLock = RWLockBuilder.NewLock();

            // This should result in an exception because a write lock was never acquired.
            rwLock.ExitWriteLock();
        }

        /// <summary>
        /// Verifies that two tasks can simultaneously acquire the same read lock.
        /// </summary>
        [TestMethod, Timeout(1500)]
        public void TestMethod2()
        {
            // These local variables are used by the GetReadLock method below.  They are accessible
            // to that method because it is nested within TestMethod2.
            int count = 2;
            ManualResetEvent mre = new ManualResetEvent(false);
            RWLock rwLock = RWLockBuilder.NewLock();

            // Run GetReadLock() on two tasks.  Wait up to one second for count to be decremented to zero.
            Task t1 = Task.Run(() => GetReadLock());
            Task t2 = Task.Run(() => GetReadLock());
            Assert.IsTrue(SpinWait.SpinUntil(() => count == 0, 1000), "Unable to have two simultaneous readers");

            // Allow the blocked tasks to resume, which will result in their termination
            mre.Set();

            // This method is run simultaneously in two tasks
            void GetReadLock ()
            {
                // Acquire a read lock
                rwLock.EnterReadLock();

                // Atomically decrement the shared count variable.  Note that merely doing count-- won't always work.
                Interlocked.Decrement(ref count);

                // Block until the main task sets the mre to true.
                mre.WaitOne();

                // Exit the read lock
                rwLock.ExitReadLock();
            }
        }

        //Tests to ensure that two writers can not enter the lock at the same time
        [TestMethod]
        public void TestMethod3()
        {
            int count = 2;
            ManualResetEvent mre = new ManualResetEvent(false);
            RWLock rwLock = RWLockBuilder.NewLock();

            Task t1 = Task.Run(() => GetWriteLock());
            Task t2 = Task.Run(() => GetWriteLock());

            Assert.IsFalse(SpinWait.SpinUntil(() => count == 0, 1000), "Unable to have two simultaneous writers");
            Console.WriteLine("bitch");
            mre.Set();

            // This method is run simultaneously in two tasks
            void GetWriteLock()
            {
                // Acquire a read lock
                rwLock.EnterWriteLock();

                // Atomically decrement the shared count variable.  Note that merely doing count-- won't always work.
                Interlocked.Decrement(ref count);

                // Block until the main task sets the mre to true.
                mre.WaitOne();

                // Exit the read lock
                rwLock.ExitWriteLock();
            }

        }


        [TestMethod]
        public void TestMethod4()
        {
            int count = 2;
            ManualResetEvent mre = new ManualResetEvent(false);
            RWLock rWLock = RWLockBuilder.NewLock();

            Task t1 = Task.Run(() => GetReadLock());
            Task t2 = Task.Run(() => GetReadLock());

            Assert.IsTrue(rWLock.CurrentReadCount == 2);


            // This method is run simultaneously in two tasks
            void GetReadLock()
            {
                // Acquire a read lock
                rWLock.EnterReadLock();

                // Atomically decrement the shared count variable.  Note that merely doing count-- won't always work.
                Interlocked.Decrement(ref count);

                // Block until the main task sets the mre to true.
                mre.WaitOne();

                // Exit the read lock
                rWLock.ExitReadLock();
            }
        }

        [TestMethod]
        public void TestMethod5()
        {
            System.Diagnostics.Debug.WriteLine("hi");
        }
    }
}
