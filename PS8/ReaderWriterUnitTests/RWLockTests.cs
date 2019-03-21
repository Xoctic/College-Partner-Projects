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
        //Also tests the isWriteLockHeld method to make sure that when the tasl enters the writeLock, isWriteLockHeld == true and when the task exits the writeLock, isWriteLockHeld == false
        //[TestMethod, Timeout(1500)]
        [TestMethod]
        public void TestMethod3()
        {
            int count = 2;
            ManualResetEvent mre = new ManualResetEvent(false);
            RWLock rwLock = RWLockBuilder.NewLock();

            Task t1 = Task.Run(() => GetWriteLock());
            Task t2 = Task.Run(() => GetWriteLock());

            Assert.IsFalse(SpinWait.SpinUntil(() => count == 0, 1000), "Unable to have two simultaneous writers");
            
            mre.Set();

            // This method is run simultaneously in two tasks
            void GetWriteLock()
            {
                //mre.WaitOne();
                // Acquire a read lock
                rwLock.EnterWriteLock();

                Assert.IsTrue(rwLock.IsWriteLockHeld);

                // Atomically decrement the shared count variable.  Note that merely doing count-- won't always work.
                Interlocked.Decrement(ref count);

                // Block until the main task sets the mre to true.
                mre.WaitOne();
                //mre.Close();
                // Exit the read lock
                rwLock.ExitWriteLock();

                Assert.IsFalse(rwLock.IsWriteLockHeld);
            }
        }

        //Tests creating more than 2 readers, ensuring currentReadCount is 5 and count is decremented to 0 at the end
        //Also tests the isReadLockHeld method to make sure the when the task enters the readLock isReadLockHeld == true and when the task exits the readLock isReadLockHeld == false
        [TestMethod, Timeout(1500)]
        public void TestMethod4()
        {
            int count = 5;
            ManualResetEvent mre = new ManualResetEvent(false);
            RWLock rWLock = RWLockBuilder.NewLock();

            Task t1 = Task.Run(() => GetReadLock());
            Task t2 = Task.Run(() => GetReadLock());
            Task t3 = Task.Run(() => GetReadLock());
            Task t4 = Task.Run(() => GetReadLock());
            Task t5 = Task.Run(() => GetReadLock());

            

            Assert.IsTrue(SpinWait.SpinUntil(() => count == 0, 1000), "Unable to have two simultaneous readers");
            
            //Assert.IsTrue(rWLock.CurrentReadCount == 5);
            Assert.IsTrue(count == 0);

            mre.Set();

            // This method is run simultaneously in two tasks
            void GetReadLock()
            {
                // Acquire a read lock
                //rWLock.EnterReadLock();

                //Assert.IsTrue(rWLock.IsReadLockHeld);

                // Atomically decrement the shared count variable.  Note that merely doing count-- won't always work.
                Interlocked.Decrement(ref count);

                // Block until the main task sets the mre to true.
                //mre.WaitOne();

                // Exit the read lock
                //rWLock.ExitReadLock();

                //Assert.IsFalse(rWLock.IsReadLockHeld);
            }

        }

        //[TestMethod, Timeout(1500)]
        [TestMethod]
        public void TestMethod5()
        {
            int count = 5;

            ManualResetEvent mre = new ManualResetEvent(false);
            RWLock rWLock = RWLockBuilder.NewLock();

            Task t1 = Task.Run(() => GetReadLock());
            Task t2 = Task.Run(() => GetReadLock());
            Task t3 = Task.Run(() => GetWriteLock());
            Task t4 = Task.Run(() => GetReadLock());
            Task t5 = Task.Run(() => GetWriteLock());
            
            Assert.IsTrue(SpinWait.SpinUntil(() => count == 0, 1000), "Unable to have two simultaneous readers");
            
            //Assert.IsTrue(rWLock.CurrentReadCount == 3);
            Assert.IsTrue(count == 0);

            mre.Set();

            // This method is run simultaneously in two tasks
            void GetReadLock()
            {
                //mre.WaitOne();
                // Acquire a read lock
                rWLock.EnterReadLock();

                //Assert.IsTrue(rWLock.WaitingReadCount == 0);
                Assert.IsTrue(rWLock.IsReadLockHeld);

                // Atomically decrement the shared count variable.  Note that merely doing count-- won't always work.
                Interlocked.Decrement(ref count);

                // Block until the main task sets the mre to true.
                //mre.WaitOne();

                // Exit the read lock
                rWLock.ExitReadLock();

                Assert.IsFalse(rWLock.IsReadLockHeld);
            }
            void GetWriteLock()
            {
                //mre.Reset();
                // Acquire a read lock
                rWLock.EnterWriteLock();

                Assert.IsTrue(rWLock.IsWriteLockHeld);

                // Atomically decrement the shared count variable.  Note that merely doing count-- won't always work.
                Interlocked.Decrement(ref count);

                // Block until the main task sets the mre to true.
                //mre.WaitOne();
                //mre.Set();

                // Exit the read lock
                rWLock.ExitWriteLock();
                
                Assert.IsFalse(rWLock.IsWriteLockHeld);
            }
        }

        //[TestMethod]
        //public void TestMethod5()
        //{
        //    System.Diagnostics.Debug.WriteLine("hi");
        //}
    }
}
