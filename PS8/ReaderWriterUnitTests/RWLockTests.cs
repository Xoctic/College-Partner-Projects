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
        [TestMethod, Timeout(1500)]
        //[TestMethod]
        public void TestMethod3()
        {
            int count = 2;
            ManualResetEvent mre = new ManualResetEvent(false);
            RWLock rwLock = RWLockBuilder.NewLock();

            Thread t1 = new Thread(() => GetWriteLock());
            Thread t2 = new Thread(() => GetWriteLock());

            t1.Start();
            t2.Start();

            Assert.IsFalse(SpinWait.SpinUntil(() => count == 0, 5), "Unable to have two simultaneous writers");

            mre.Set();

            // This method is run simultaneously in two tasks
            void GetWriteLock()
            {
                // Acquire a write lock
                rwLock.EnterWriteLock();

                // Atomically decrement the shared count variable.  Note that merely doing count-- won't always work.
                Interlocked.Decrement(ref count);

                // Block until the main task sets the mre to true.
                mre.WaitOne();

                rwLock.ExitWriteLock();
            }
        }

        //Tests creating more than 2 readers, ensuring currentReadCount is 5 and count is decremented to 0 at the end
        [TestMethod, Timeout(1500)]
        public void TestMethod4()
        {
            int count = 5;
            ManualResetEvent mre = new ManualResetEvent(false);
            ManualResetEvent mre2 = new ManualResetEvent(false);
            RWLock rWLock = RWLockBuilder.NewLock();

            Thread t1 = new Thread(() => GetReadLock());
            Thread t2 = new Thread(() => GetReadLock());
            Thread t3 = new Thread(() => GetReadLock());
            Thread t4 = new Thread(() => GetReadLock());
            Thread t5 = new Thread(() => GetReadLock());

            t1.Start();
            t2.Start();
            t3.Start();
            t4.Start();
            t5.Start();

            Assert.IsTrue(SpinWait.SpinUntil(() => count == 0, 1000), "Unable to have two simultaneous readers");
            Assert.IsTrue(rWLock.CurrentReadCount == 5);
            Assert.IsTrue(rWLock.WaitingReadCount == 0);
            Assert.IsTrue(rWLock.WaitingWriteCount == 0);
            mre.Set();

            mre2.WaitOne();
            Assert.IsTrue(rWLock.CurrentReadCount == 0);

            void GetReadLock()
            {
                //Acquire a read lock
                rWLock.EnterReadLock();

                //Atomically decrement the shared count variable.  Note that merely doing count-- won't always work.
                Interlocked.Decrement(ref count);

                //Block until the main task sets the mre to true.
                mre.WaitOne();

                //Exit the read lock
                rWLock.ExitReadLock();
                if (rWLock.CurrentReadCount == 0)
                {
                    mre2.Set();
                }
            }

        }
        //Tests 20 read locks and if there currentReadCount == 20 before any locks have exited.  Also checks if all 20 read locks exit by checking currentReadCount == 0.
        [TestMethod, Timeout(1500)]
        public void TestMethod5()
        {
            int count = 20;
            ManualResetEvent mre1 = new ManualResetEvent(false);
            ManualResetEvent mre2 = new ManualResetEvent(false);
            RWLock rwLock = RWLockBuilder.NewLock();

            Thread t1 = new Thread(() => GetReadLock());
            Thread t2 = new Thread(() => GetReadLock());
            Thread t3 = new Thread(() => GetReadLock());
            Thread t4 = new Thread(() => GetReadLock());
            Thread t5 = new Thread(() => GetReadLock());
            Thread t6 = new Thread(() => GetReadLock());
            Thread t7 = new Thread(() => GetReadLock());
            Thread t8 = new Thread(() => GetReadLock());
            Thread t9 = new Thread(() => GetReadLock());
            Thread t10 = new Thread(() => GetReadLock());
            Thread t11 = new Thread(() => GetReadLock());
            Thread t12 = new Thread(() => GetReadLock());
            Thread t13 = new Thread(() => GetReadLock());
            Thread t14 = new Thread(() => GetReadLock());
            Thread t15 = new Thread(() => GetReadLock());
            Thread t16 = new Thread(() => GetReadLock());
            Thread t17 = new Thread(() => GetReadLock());
            Thread t18 = new Thread(() => GetReadLock());
            Thread t19 = new Thread(() => GetReadLock());
            Thread t20 = new Thread(() => GetReadLock());

            t1.Start();
            t2.Start();
            t3.Start();
            t4.Start();
            t5.Start();
            t6.Start();
            t7.Start();
            t8.Start();
            t9.Start();
            t10.Start();
            t11.Start();
            t12.Start();
            t13.Start();
            t14.Start();
            t15.Start();
            t16.Start();
            t17.Start();
            t18.Start();
            t19.Start();
            t20.Start();

            Assert.IsTrue(SpinWait.SpinUntil(() => count == 0, 1000), "Unable to have two simultaneous readers");
            Assert.IsTrue(rwLock.CurrentReadCount == 20);
            Assert.IsTrue(rwLock.WaitingReadCount == 0);
            Assert.IsTrue(rwLock.WaitingWriteCount == 0);
            mre1.Set();
            mre2.WaitOne();
            Assert.IsTrue(rwLock.CurrentReadCount == 0);

            void GetReadLock()
            {
                rwLock.EnterReadLock();

                // Atomically decrement the shared count variable.  Note that merely doing count-- won't always work.
                Interlocked.Decrement(ref count);

                mre1.WaitOne();
                // Exit the read lock
                rwLock.ExitReadLock();
                if (rwLock.CurrentReadCount == 0)
                {
                    mre2.Set();
                }
            }
        }
        //Test using 10 write locks.  Tests if lock is held when a write lock is entered.  Checks if lock is not held after all write locks have finished.
        [TestMethod, Timeout(1500)]
        public void TestMethod6()
        {
            int count = 10;
            bool writeLockIsHeld = false;
            ManualResetEvent mre1 = new ManualResetEvent(false);
            RWLock rwLock = RWLockBuilder.NewLock();

            Thread t1 = new Thread(() => GetWriteLock());
            Thread t2 = new Thread(() => GetWriteLock());
            Thread t3 = new Thread(() => GetWriteLock());
            Thread t4 = new Thread(() => GetWriteLock());
            Thread t5 = new Thread(() => GetWriteLock());
            Thread t6 = new Thread(() => GetWriteLock());
            Thread t7 = new Thread(() => GetWriteLock());
            Thread t8 = new Thread(() => GetWriteLock());
            Thread t9 = new Thread(() => GetWriteLock());
            Thread t10 = new Thread(() => GetWriteLock());

            t1.Start();
            t2.Start();
            t3.Start();
            t4.Start();
            t5.Start();
            t6.Start();
            t7.Start();
            t8.Start();
            t9.Start();
            t10.Start();
            Assert.IsTrue(SpinWait.SpinUntil(() => count == 9, 1000), "Unable to have two simultaneous readers");
            Assert.IsTrue(writeLockIsHeld);
            Assert.IsTrue(rwLock.WaitingReadCount == 0);
            mre1.Set();

            Assert.IsTrue(SpinWait.SpinUntil(() => count == 0, 1000), "Unable to have two simultaneous readers");
            Assert.IsTrue(rwLock.WaitingWriteCount == 0);
            Assert.IsFalse(writeLockIsHeld);
            void GetWriteLock()
            {
                // Acquire a write lock
                rwLock.EnterWriteLock();
                if (rwLock.IsWriteLockHeld)
                {
                    writeLockIsHeld = true;
                }
                // Atomically decrement the shared count variable.  Note that merely doing count-- won't always work.
                Interlocked.Decrement(ref count);

                mre1.WaitOne();
                // Exit the write lock
                rwLock.ExitWriteLock();
                if (rwLock.IsWriteLockHeld == false)
                {
                    writeLockIsHeld = false;
                }
            }
        }

        //Test using 20 write locks.  Tests if lock is held when a write lock is entered.  Checks other properties at certain points.  Checks if lock is not held after all write locks have finished.
        [TestMethod, Timeout(1500)]
        public void TestMethod7()
        {
            int count = 20;
            bool writeLockIsHeld = false;

            ManualResetEvent mre1 = new ManualResetEvent(false);
            ManualResetEvent mre2 = new ManualResetEvent(false);
            RWLock rwLock = RWLockBuilder.NewLock();

            Thread t1 = new Thread(() => GetWriteLock());
            Thread t2 = new Thread(() => GetWriteLock());
            Thread t3 = new Thread(() => GetWriteLock());
            Thread t4 = new Thread(() => GetWriteLock());
            Thread t5 = new Thread(() => GetWriteLock());
            Thread t6 = new Thread(() => GetWriteLock());
            Thread t7 = new Thread(() => GetWriteLock());
            Thread t8 = new Thread(() => GetWriteLock());
            Thread t9 = new Thread(() => GetWriteLock());
            Thread t10 = new Thread(() => GetWriteLock());
            Thread t11 = new Thread(() => GetWriteLock());
            Thread t12 = new Thread(() => GetWriteLock());
            Thread t13 = new Thread(() => GetWriteLock());
            Thread t14 = new Thread(() => GetWriteLock());
            Thread t15 = new Thread(() => GetWriteLock());
            Thread t16 = new Thread(() => GetWriteLock());
            Thread t17 = new Thread(() => GetWriteLock());
            Thread t18 = new Thread(() => GetWriteLock());
            Thread t19 = new Thread(() => GetWriteLock());
            Thread t20 = new Thread(() => GetWriteLock());

            t1.Start();
            t2.Start();
            t3.Start();
            t4.Start();
            t5.Start();
            t6.Start();
            t7.Start();
            t8.Start();
            t9.Start();
            t10.Start();
            t11.Start();
            t12.Start();
            t13.Start();
            t14.Start();
            t15.Start();
            t16.Start();
            t17.Start();
            t18.Start();
            t19.Start();
            t20.Start();

            Assert.IsTrue(SpinWait.SpinUntil(() => rwLock.WaitingWriteCount == 19, 1000), "Unable to have two simultaneous readers");
            Assert.IsTrue(writeLockIsHeld);
            Assert.IsTrue(rwLock.WaitingReadCount == 0);
            Assert.IsTrue(count == 19);
            mre1.Set();

            Assert.IsTrue(SpinWait.SpinUntil(() => count == 15, 1000), "Unable to have two simultaneous readers");
            Assert.IsTrue(rwLock.WaitingReadCount == 0);
//            Assert.IsTrue(writeLockIsHeld);
            Assert.IsTrue(rwLock.WaitingWriteCount == 15);
            mre2.Set();

            Assert.IsTrue(SpinWait.SpinUntil(() => count == 0, 1000), "Unable to have two simultaneous readers");
            Assert.IsTrue(rwLock.WaitingWriteCount == 0);
//            Assert.IsFalse(writeLockIsHeld);
            void GetWriteLock()
            {
                // Acquire a write lock
                rwLock.EnterWriteLock();
                if (rwLock.IsWriteLockHeld)
                {
                    writeLockIsHeld = true;
                }
                // Atomically decrement the shared count variable.  Note that merely doing count-- won't always work.
                Interlocked.Decrement(ref count);
                if (count == 15)
                {
                    mre2.WaitOne();
                }
                mre1.WaitOne();
                // Exit the write lock
                rwLock.ExitWriteLock();
                if (rwLock.IsWriteLockHeld == false)
                {
                    writeLockIsHeld = false;
                }
            }
        }
        //When one reader and one writer try to enter the lock at the same time.
        [TestMethod, Timeout(1500)]
        public void TestMethod8()
        {
            // These local variables are used by the GetReadLock method below.  They are accessible
            // to that method because it is nested within TestMethod2.
            int count = 2;
            bool writeLockIsHeld = false;
            bool readLockIsHeld = false;
            bool readerIsFinished = false;
            ManualResetEvent mre1 = new ManualResetEvent(false);
            ManualResetEvent mre2 = new ManualResetEvent(false);
            RWLock rwLock = RWLockBuilder.NewLock();

            // Run GetReadLock() on two tasks.  Wait up to one second for count to be decremented to zero.
            Thread t1 = new Thread(() => GetReadLock());
            Thread t2 = new Thread(() => GetWriteLock());
            t1.Start();
            t2.Start();

            Assert.IsTrue(SpinWait.SpinUntil(() => rwLock.WaitingReadCount == 1, 1000), "Unable to have two simultaneous readers");
            Assert.IsTrue(writeLockIsHeld);
            Assert.IsTrue(rwLock.WaitingWriteCount == 0);
            Assert.IsTrue(rwLock.CurrentReadCount == 0);
            mre2.Set();

            Assert.IsTrue(SpinWait.SpinUntil(() => readerIsFinished == true, 1000), "Unable to have two simultaneous readers");
            Assert.IsTrue(rwLock.CurrentReadCount == 0);
            Assert.IsTrue(rwLock.WaitingReadCount == 0);
            Assert.IsTrue(rwLock.WaitingWriteCount == 0);
            Assert.IsFalse(writeLockIsHeld);
            Assert.IsFalse(readLockIsHeld);
            Assert.IsTrue(count == 0);
            // This method is run simultaneously in two tasks
            void GetReadLock()
            {
                mre1.WaitOne();
                // Acquire a read lock
                rwLock.EnterReadLock();
                if (rwLock.IsReadLockHeld)
                {
                    readLockIsHeld = true;
                }
                // Atomically decrement the shared count variable.  Note that merely doing count-- won't always work.
                Interlocked.Decrement(ref count);

                // Exit the read lock
                rwLock.ExitReadLock();
                if (rwLock.IsReadLockHeld == false)
                {
                    readLockIsHeld = false;
                }
                readerIsFinished = true;
            }
            void GetWriteLock()
            {
                // Acquire a write lock
                rwLock.EnterWriteLock();
                mre1.Set();
                if (rwLock.IsWriteLockHeld)
                {
                    writeLockIsHeld = true;
                }
                // Atomically decrement the shared count variable.  Note that merely doing count-- won't always work.
                Interlocked.Decrement(ref count);

                mre2.WaitOne();
                // Exit the write lock
                rwLock.ExitWriteLock();
                if (rwLock.IsWriteLockHeld == false)
                {
                    writeLockIsHeld = false;
                }
            }
        }

        [TestMethod, Timeout(1500)]
        //[TestMethod]
        public void TestMethod9()
        {
            int count = 5;
            bool isReadLockHeld = false;
            bool writerIsFinished = false;
            //for write locks
            ManualResetEvent mre1 = new ManualResetEvent(false);
            //for read locks
            ManualResetEvent mre2 = new ManualResetEvent(false);
            RWLock rwLock = RWLockBuilder.NewLock();

            Thread t1 = new Thread(() => GetReadLock());
            Thread t2 = new Thread(() => GetReadLock());
            Thread t3 = new Thread(() => GetWriteLock());
            Thread t4 = new Thread(() => GetReadLock());
            Thread t5 = new Thread(() => GetReadLock());
            t1.Start();
            t2.Start();
            t3.Start();
            t4.Start();
            t5.Start();

            Assert.IsTrue(SpinWait.SpinUntil(() => rwLock.CurrentReadCount == 4, 1000), "Unable to have two simultaneous readers");
            Assert.IsTrue(rwLock.WaitingReadCount == 0);
            Assert.IsTrue(isReadLockHeld);
            mre2.Set();

            Assert.IsTrue(SpinWait.SpinUntil(() => writerIsFinished == true, 1000), "Unable to have two simultaneous readers");
            Assert.IsTrue(rwLock.CurrentReadCount == 0);
            Assert.IsTrue(rwLock.WaitingReadCount == 0);
            Assert.IsTrue(rwLock.WaitingWriteCount == 0);
            Assert.IsFalse(isReadLockHeld);
            Assert.IsTrue(count == 0);

            void GetReadLock()
            {
                rwLock.EnterReadLock();
                if (rwLock.IsReadLockHeld)
                {
                    isReadLockHeld = true;
                }

                Interlocked.Decrement(ref count);

                mre2.WaitOne();
                rwLock.ExitReadLock();
                if (rwLock.CurrentReadCount == 0)
                {
                    isReadLockHeld = false;
                    mre1.Set();
                }
            }
            void GetWriteLock()
            {
                mre1.WaitOne();
                rwLock.EnterWriteLock();

                Interlocked.Decrement(ref count);

                rwLock.ExitWriteLock();
                writerIsFinished = true;
            }
        }
        //Mostly ReaderLocks, total of 10 lock
        //A check to see that the readerWriterLockSlim is performing as it should be
        [TestMethod, Timeout(1500)]
        //[TestMethod]
        public void TestMethod10()
        {
            int count = 10;
            bool readerWasAllowed = false;
            bool writerWasAllowed = false;
            bool checkPoint1 = false;
            bool checkPoint2 = false;
            //for write locks
            ManualResetEvent mre1 = new ManualResetEvent(false);
            //for read locks
            ManualResetEvent mre2 = new ManualResetEvent(false);
            RWLock rwLock = RWLockBuilder.NewLock();

            Thread t1 = new Thread(() => GetWriteLock());
            Thread t2 = new Thread(() => GetReadLock());
            Thread t3 = new Thread(() => GetReadLock());
            Thread t4 = new Thread(() => GetWriteLock());
            Thread t5 = new Thread(() => GetReadLock());
            Thread t6 = new Thread(() => GetReadLock());
            Thread t7 = new Thread(() => GetReadLock());
            Thread t8 = new Thread(() => GetWriteLock());
            Thread t9 = new Thread(() => GetReadLock());
            Thread t10 = new Thread(() => GetReadLock());
            t1.Start();
            t2.Start();
            t3.Start();
            t4.Start();
            t5.Start();
            t6.Start();
            t7.Start();
            t8.Start();
            t9.Start();
            t10.Start();

            Assert.IsTrue(SpinWait.SpinUntil(() => checkPoint1 == true, 1000), "Unable to have two simultaneous readers");
            if (rwLock.TryEnterReadLock(1))
            {
                readerWasAllowed = true;
            }
            Assert.IsFalse(readerWasAllowed);
            mre1.Set();
            Assert.IsTrue(SpinWait.SpinUntil(() => checkPoint2 == true, 1000), "Unable to have two simultaneous readers");
            if (rwLock.TryEnterWriteLock(1))
            {
                writerWasAllowed = true;
            }
            Assert.IsFalse(writerWasAllowed);
            mre2.Set();
            // This method is run simultaneously in two tasks
            void GetReadLock()
            {
                rwLock.EnterReadLock();
                if (checkPoint2 == false)
                {
                    checkPoint2 = true;
                }
                mre2.WaitOne();
                // Atomically decrement the shared count variable.  Note that merely doing count-- won't always work.
                Interlocked.Decrement(ref count);
                // Exit the read lock
                rwLock.ExitReadLock();
            }
            void GetWriteLock()
            {
                // Acquire a write lock
                rwLock.EnterWriteLock();
                if (checkPoint1 == false)
                {
                    checkPoint1 = true;
                }
                mre1.WaitOne();
                // Atomically decrement the shared count variable.  Note that merely doing count-- won't always work.
                Interlocked.Decrement(ref count);
                // Exit the writelock
                rwLock.ExitWriteLock();

            }
        }



        [TestMethod, Timeout(1500)]
        //[TestMethod]
        public void TestMethod11()
        {
            int count = 15;
            bool curentlyInReadMode = false;
            bool curentlyInWriteMode = false;
            //for write locks
            ManualResetEvent mre1 = new ManualResetEvent(false);
            //for read locks
            ManualResetEvent mre2 = new ManualResetEvent(false);
            RWLock rwLock = RWLockBuilder.NewLock();

            Thread t1 = new Thread(() => GetWriteLock());
            Thread t2 = new Thread(() => GetReadLock());
            Thread t3 = new Thread(() => GetReadLock());
            Thread t4 = new Thread(() => GetWriteLock());
            Thread t5 = new Thread(() => GetReadLock());
            Thread t6 = new Thread(() => GetReadLock());
            Thread t7 = new Thread(() => GetWriteLock());
            Thread t8 = new Thread(() => GetWriteLock());
            Thread t9 = new Thread(() => GetReadLock());
            Thread t10 = new Thread(() => GetReadLock());
            Thread t11 = new Thread(() => GetWriteLock());
            Thread t12 = new Thread(() => GetReadLock());
            Thread t13 = new Thread(() => GetReadLock());
            Thread t14 = new Thread(() => GetWriteLock());
            Thread t15 = new Thread(() => GetReadLock());

            t1.Start();
            t2.Start();
            t3.Start();
            t4.Start();
            t5.Start();
            t6.Start();
            t7.Start();
            t8.Start();
            t9.Start();
            t10.Start();
            t11.Start();
            t12.Start();
            t13.Start();
            t14.Start();
            t15.Start();
            Assert.IsTrue(SpinWait.SpinUntil(() => curentlyInWriteMode == true, 1000), "Unable to have two simultaneous readers");
            Assert.IsTrue(rwLock.CurrentReadCount == 0);
            mre1.Set();

            Assert.IsTrue(SpinWait.SpinUntil(() => curentlyInReadMode == true, 1000), "Unable to have two simultaneous readers");
            //Assert.IsTrue(rwLock.WaitingReadCount == 0);


            Assert.IsTrue(SpinWait.SpinUntil(() => count == 0, 1000), "Unable to have two simultaneous readers");
            // This method is run simultaneously in two tasks
            void GetReadLock()
            {
                rwLock.EnterReadLock();
                if (curentlyInReadMode == false && curentlyInWriteMode == true)
                {
                    curentlyInReadMode = true;
                }
                // Atomically decrement the shared count variable.  Note that merely doing count-- won't always work.
                Interlocked.Decrement(ref count);
                // Exit the read lock
                rwLock.ExitReadLock();

            }
            void GetWriteLock()
            {
                // Acquire a write lock
                rwLock.EnterWriteLock();
                if (curentlyInWriteMode == false)
                {
                    curentlyInWriteMode = true;
                }
                mre1.WaitOne();
                // Atomically decrement the shared count variable.  Note that merely doing count-- won't always work.
                Interlocked.Decrement(ref count);
                // Exit the write lock
                rwLock.ExitWriteLock();
            }
        }




        //Simple writer test
        [TestMethod, Timeout(1500)]
        public void TestMethodA()
        {
            int count = 2;
            ManualResetEvent mre1 = new ManualResetEvent(false);
            ManualResetEvent mre2 = new ManualResetEvent(false);
            RWLock lock1 = RWLockBuilder.NewLock();

            Task t1 = Task.Run(() => GetWriteLock(mre1));
            Task t2 = Task.Run(() => GetWriteLock(mre2));

            mre1.Set();

            t1.Wait();

            mre2.Set();

            t2.Wait();


            Assert.IsTrue(count == 0);


            void GetWriteLock(ManualResetEvent _mre)
            {
                _mre.WaitOne();
                // Acquire a read lock
                lock1.EnterWriteLock();

                //Assert.IsTrue(rwLock.IsWriteLockHeld);

                // Atomically decrement the shared count variable.  Note that merely doing count-- won't always work.
                Interlocked.Decrement(ref count);

               
                // Exit the read lock
                lock1.ExitWriteLock();

                //mre.Reset();

                //Assert.IsFalse(rwLock.IsWriteLockHeld);
            }

        }


        //Tests if the write lock is held in the lock
        //Tests if the read lock is held in the lock
        [TestMethod, Timeout(1500)]
        public void TestMethodB()
        {

            ManualResetEvent mre = new ManualResetEvent(false);
            
            
            RWLock lock1 = RWLockBuilder.NewLock();
            bool writeLockHeld = false;
            bool readLockHeld = false;
            Task t1 = Task.Run(() => GetWriteLock(mre));
            
            mre.Set();
            t1.Wait();
            Assert.IsTrue(writeLockHeld);



           
            mre = new ManualResetEvent(false);
            t1 = Task.Run(() => GetReadLock(mre));

            mre.Set();
            t1.Wait();
            Assert.IsTrue(readLockHeld);


            void GetWriteLock(ManualResetEvent _mre)
            {

                _mre.WaitOne();
                // Acquire a read lock
                lock1.EnterWriteLock();

                writeLockHeld = lock1.IsWriteLockHeld;

                // Exit the read lock
                lock1.ExitWriteLock();

            }


            void GetReadLock(ManualResetEvent _mre)
            {
                // Acquire a read lock
                lock1.EnterReadLock();

                readLockHeld = lock1.IsReadLockHeld;

                // Exit the read lock
                lock1.ExitReadLock();

                mre.Set();
            }

        }

        //Tests if the write lock is not held outside of the lock
        //Tests if the read lock is not held outside of the lock
        [TestMethod, Timeout(1500)]
        public void TestMethodC()
        {
            ManualResetEvent mre = new ManualResetEvent(false);
            RWLock lock1 = RWLockBuilder.NewLock();
            bool writeLockHeld = false;
            bool readLockHeld = false;


            Task t1 = Task.Run(() => GetWriteLock(mre));

            mre.Set();
            t1.Wait();
            Assert.IsFalse(writeLockHeld);

            mre = new ManualResetEvent(false);
            t1 = Task.Run(() => GetReadLock(mre));

            mre.Set();
            t1.Wait();
            Assert.IsFalse(readLockHeld);

            void GetWriteLock(ManualResetEvent _mre)
            {

                _mre.WaitOne();
                // Acquire a read lock
                lock1.EnterWriteLock();

                writeLockHeld = lock1.IsWriteLockHeld;

                // Exit the read lock
                lock1.ExitWriteLock();

                writeLockHeld = lock1.IsWriteLockHeld;

            }


            void GetReadLock(ManualResetEvent _mre)
            {
                // Acquire a read lock
                lock1.EnterReadLock();

                readLockHeld = lock1.IsReadLockHeld;

                // Exit the read lock
                lock1.ExitReadLock();

                readLockHeld = lock1.IsReadLockHeld;
            }

        }

        //tries to enter a write lock when the write lock is already held
        [TestMethod, Timeout(1500)]
        public void TestMethodD()
        {
            ManualResetEvent mre1 = new ManualResetEvent(false);
            ManualResetEvent mre2 = new ManualResetEvent(false);
            RWLock lock1 = RWLockBuilder.NewLock();
            bool enteredWriteLock = false;
           

            Task t1 = Task.Run(() => GetWriteLock());


            mre1.Set();


            Assert.IsTrue(SpinWait.SpinUntil(() => enteredWriteLock == true, 1000));

            Assert.IsFalse(lock1.TryEnterWriteLock(100));

            mre2.Set();
         
            

            void GetWriteLock()
            {

                mre1.WaitOne();

                // Acquire a read lock
                lock1.EnterWriteLock();

                enteredWriteLock = lock1.IsWriteLockHeld;
                
                mre2.WaitOne();

                // Exit the read lock
                lock1.ExitWriteLock();

            }

        }

        //Tests to see if more than one task can access a readLock at the same time
        [TestMethod, Timeout(1500)]
        public void TestMethodE()
        {

            ManualResetEvent mre1 = new ManualResetEvent(false);
            RWLock locker = RWLockBuilder.NewLock();
            bool enteredReadLock = false;

            //Task t1 = Task.Run(() => GetReadLock());
            Thread t1 = new Thread(() => GetReadLock());
            t1.Start();
            //mre1.Set();

            Assert.IsTrue(SpinWait.SpinUntil(() => enteredReadLock == true, 1000));

            Assert.IsTrue(locker.TryEnterReadLock(100));

            mre1.Set();
            

            void GetReadLock()
            {

                //mre1.WaitOne();

                // Acquire a read lock
                locker.EnterReadLock();

                enteredReadLock = locker.IsReadLockHeld;

                mre1 = new ManualResetEvent(false);
                mre1.WaitOne();

                // Exit the read lock
                locker.ExitReadLock();

            }


        }

        //Tests if waitingReadCount == 1 after 1 reader tries to enter the lock when the writer is still in the lock
        [TestMethod, Timeout(1500)]
        public void TestMethodF()
        {

            ManualResetEvent mre1 = new ManualResetEvent(false);
            ManualResetEvent mre2 = new ManualResetEvent(false);
            ManualResetEvent mre3 = new ManualResetEvent(false);

            RWLock lock1 = RWLockBuilder.NewLock();
            bool enteredWriteLock = false;
            bool enteredGetReadLock = false;
            bool enteredReadLock = false;


            Task t1 = Task.Run(() => GetWriteLock());
            Task t2 = Task.Run(() => GetReadLock());


            mre1.Set();


            Assert.IsTrue(SpinWait.SpinUntil(() => enteredWriteLock == true, 1000));

            Assert.IsFalse(lock1.TryEnterWriteLock(100));

            mre3.Set();

            Assert.IsTrue(SpinWait.SpinUntil(() => enteredGetReadLock == true, 1000));

            Assert.IsFalse(SpinWait.SpinUntil(() => enteredReadLock == true, 1000));

            Assert.IsTrue(lock1.WaitingReadCount == 1); 

            mre2.Set();



            void GetWriteLock()
            {

                mre1.WaitOne();
                // Acquire a read lock

                lock1.EnterWriteLock();

                enteredWriteLock = lock1.IsWriteLockHeld;

                mre2.WaitOne();

                // Exit the read lock
                lock1.ExitWriteLock();
            }


            void GetReadLock()
            {
                mre3.WaitOne();

                enteredGetReadLock = true;

                // Acquire a read lock
                lock1.EnterReadLock();

                enteredReadLock = lock1.IsReadLockHeld;

                // Exit the read lock
                lock1.ExitReadLock();

            }

        }

        //Tests if waitingWriteCount == 1 after 1 writer tries to enter the lock when a reader is still in the lock
        [TestMethod, Timeout(1500)]
        public void TestMethodG()
        {

            ManualResetEvent mre1 = new ManualResetEvent(false);
            ManualResetEvent mre2 = new ManualResetEvent(false);
            ManualResetEvent mre3 = new ManualResetEvent(false);

            RWLock lock1 = RWLockBuilder.NewLock();
            bool enteredGetWriteLock = false;
            bool enteredWriteLock = false;
            bool enteredReadLock = false;


            Task t1 = Task.Run(() => GetReadLock());
            Task t2 = Task.Run(() => GetWriteLock());

            mre1.Set();

            Assert.IsTrue(SpinWait.SpinUntil(() => enteredReadLock == true, 1000));
            Assert.IsTrue(lock1.TryEnterReadLock(50));    

            mre3.Set();
                
            Assert.IsTrue(SpinWait.SpinUntil(() => enteredGetWriteLock == true, 1000));
            Assert.IsFalse(SpinWait.SpinUntil(() => enteredWriteLock == true, 1000));

            Assert.IsTrue(lock1.WaitingWriteCount == 1);

            mre2.Set();


            void GetReadLock()
            {
                
                mre1.WaitOne();
                // Acquire a read lock
                lock1.EnterReadLock();

                enteredReadLock = lock1.IsReadLockHeld;

                mre2.WaitOne();

                // Exit the read lock
                lock1.ExitReadLock();

            }


            void GetWriteLock()
            {

                mre3.WaitOne();

                enteredGetWriteLock = true;

                // Acquire a read lock
                lock1.EnterWriteLock();

                enteredWriteLock = lock1.IsWriteLockHeld;

                // Exit the read lock
                lock1.ExitWriteLock();

            }

        }





        /// <summary>
        /// When the current thread has attempted to acquire the read lock when it already holds  the read lock
        /// </summary>
        [TestMethod, Timeout(1500)]
        [ExpectedException(typeof(LockRecursionException))]
        public void TestMethodH()
        {

            RWLock lock1 = RWLockBuilder.NewLock();
            ManualResetEvent MRE = new ManualResetEvent(false);
            bool ready = false;
         

            Thread  t1 = new Thread(() => GetReadLock());
            //Task t1 = Task.Run(() => GetReadLock());
            Exception e = null;

            MRE.Set();

         
                //t1.Wait();
                t1.Start();
                Assert.IsTrue(SpinWait.SpinUntil(() => ready == true, 500));
                if(e != null)
                {
                    throw e;
                }
 

            void GetReadLock()
            {
                MRE.WaitOne();
                // Acquire a read lock
                lock1.EnterReadLock();

                if(lock1.IsReadLockHeld)
                {
                    try
                    {
                        lock1.EnterReadLock();
                    }
                    catch(LockRecursionException f)
                    {
                        e = f;
                        ready = true;
                        // Exit the read lock
                        lock1.ExitReadLock();
                    }
                    
                }

               
            }
        }


        /// <summary>
        /// When the current thread has attempted to acquire the read lock when it already holds a write lock
        /// </summary>
        [TestMethod, Timeout(1500)]
        [ExpectedException(typeof(LockRecursionException))]
        public void TestMethodI()
        {
            RWLock lock1 = RWLockBuilder.NewLock();
            ManualResetEvent MRE = new ManualResetEvent(false);
            bool ready = false;


            Thread t1 = new Thread(() => GetWriteLock());
            //Task t1 = Task.Run(() => GetReadLock());
            Exception e = null;

            MRE.Set();


            //t1.Wait();
            t1.Start();
            Assert.IsTrue(SpinWait.SpinUntil(() => ready == true, 500));
            if (e != null)
            {
                throw e;
            }



            void GetWriteLock()
            {
                MRE.WaitOne();
                // Acquire a read lock
                lock1.EnterWriteLock();

                if (lock1.IsWriteLockHeld)
                {
                    try
                    {
                        lock1.EnterReadLock();
                    }
                    catch (LockRecursionException f)
                    {
                        e = f;
                        ready = true;
                        // Exit the read lock
                        lock1.ExitWriteLock();
                    }

                }

            }
        }



        /// <summary>
        /// When the current thread has attempted to acquire the write lock when it already holds the write lock
        /// </summary>
        [TestMethod, Timeout(1500)]
        [ExpectedException(typeof(LockRecursionException))]
        public void TestMethodJ()
        {
            RWLock lock1 = RWLockBuilder.NewLock();
            ManualResetEvent MRE = new ManualResetEvent(false);
            bool ready = false;


            Thread t1 = new Thread(() => GetWriteLock());
            //Task t1 = Task.Run(() => GetReadLock());
            Exception e = null;

            MRE.Set();


            //t1.Wait();
            t1.Start();
            Assert.IsTrue(SpinWait.SpinUntil(() => ready == true, 500));
            if (e != null)
            {
                throw e;
            }



            void GetWriteLock()
            {
                MRE.WaitOne();
                // Acquire a read lock
                lock1.EnterWriteLock();

                if (lock1.IsWriteLockHeld)
                {
                    try
                    {
                        lock1.EnterWriteLock();
                    }
                    catch (LockRecursionException f)
                    {
                        e = f;
                        ready = true;
                        // Exit the read lock
                        lock1.ExitWriteLock();
                    }

                }

            }
        }


        /// <summary>
        /// When the current thread has attempted to acquire the write lock when it already holds the read lock
        /// </summary>
        [TestMethod, Timeout(1500)]
        [ExpectedException(typeof(LockRecursionException))]
        public void TestMethodK()
        {
            RWLock lock1 = RWLockBuilder.NewLock();
            ManualResetEvent MRE = new ManualResetEvent(false);
            bool ready = false;


            Thread t1 = new Thread(() => GetReadLock());
            //Task t1 = Task.Run(() => GetReadLock());
            Exception e = null;

            MRE.Set();


            //t1.Wait();
            t1.Start();
            Assert.IsTrue(SpinWait.SpinUntil(() => ready == true, 500));
            if (e != null)
            {
                throw e;
            }




            void GetReadLock()
            {
                MRE.WaitOne();
                // Acquire a read lock
                lock1.EnterReadLock();

                if (lock1.IsReadLockHeld)
                {
                    try
                    {
                        lock1.EnterWriteLock();
                    }
                    catch (LockRecursionException f)
                    {
                        e = f;
                        ready = true;
                        // Exit the read lock
                        lock1.ExitReadLock();
                    }

                }


            }
        }

        /// <summary>
        /// The current thread has not entered the lock in read mode.
        /// </summary>
        [TestMethod, Timeout(1500)]
        [ExpectedException(typeof(SynchronizationLockException))]
        public void TestMethodL()
        {
            RWLock lock1 = RWLockBuilder.NewLock();

            lock1.ExitReadLock();

        }

        /// <summary>
        /// The current thread has not entered the lock in write mode.
        /// </summary>
        [TestMethod, Timeout(1500)]
        [ExpectedException(typeof(SynchronizationLockException))]
        public void TestMethodM()
        {
            RWLock lock1 = RWLockBuilder.NewLock();

            lock1.ExitWriteLock();
        }


        /// <summary>
        /// The RecursionPolicy property is NoRecursion and the current thread has already entered the lock.
        /// tryEnterReadLock in a readLock
        /// </summary>
        [TestMethod, Timeout(1500)]
        [ExpectedException(typeof(LockRecursionException))]
        public void TestMethodN()
        {
            RWLock lock1 = RWLockBuilder.NewLock();

            
            lock1.EnterReadLock();

            lock1.TryEnterReadLock(20);

            lock1.ExitWriteLock();

        }

        /// <summary>
        /// The RecursionPolicy property is NoRecursion and the current thread has already entered the lock.
        /// tryEnterReadLock in a writeLock
        /// </summary>
        [TestMethod, Timeout(1500)]
        [ExpectedException(typeof(LockRecursionException))]
        public void TestMethodO()
        {
            RWLock lock1 = RWLockBuilder.NewLock();

            lock1.EnterWriteLock();

            lock1.TryEnterReadLock(20);

            lock1.ExitWriteLock();
        }

        /// <summary>
        /// The value of millisecondsTimeout is negative, but it is not equal to Infinite(-1), which is the only negative value allowed.
        /// </summary>
        [TestMethod, Timeout(1500)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestMethodP()
        {
            RWLock lock1 = RWLockBuilder.NewLock();

            lock1.TryEnterReadLock(-10);
        }







        /// <summary>
        /// The RecursionPolicy property is NoRecursion and the current thread has already entered the lock.
        /// tryEnterWriteLock in a readLock
        /// </summary>
        [TestMethod, Timeout(1500)]
        [ExpectedException(typeof(LockRecursionException))]
        public void TestMethodQ()
        {
            RWLock lock1 = RWLockBuilder.NewLock();

            lock1.EnterReadLock();

            lock1.TryEnterWriteLock(20);

            lock1.ExitReadLock();
        }


        /// <summary>
        /// The RecursionPolicy property is NoRecursion and the current thread has already entered the lock.
        /// tryEnterWriteLock in a writeLock
        /// </summary>
        [TestMethod, Timeout(1500)]
        [ExpectedException(typeof(LockRecursionException))]
        public void TestMethodR()
        {
            RWLock lock1 = RWLockBuilder.NewLock();

            lock1.EnterWriteLock();

            lock1.TryEnterWriteLock(20);

            lock1.ExitWriteLock();   

        }

        /// <summary>
        /// -The value of millisecondsTimeout is negative, but it is not equal to Infinite(-1), which is the only negative value allowed.
        /// </summary>
        [TestMethod, Timeout(1500)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestMethodS()
        {
            RWLock lock1 = RWLockBuilder.NewLock();

            lock1.TryEnterWriteLock(-10);

        }






































    }
}
