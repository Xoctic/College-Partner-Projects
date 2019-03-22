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
        [TestMethod, Timeout(15000000)]
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
            Assert.IsTrue(SpinWait.SpinUntil(() => count == 0, 10000000), "Unable to have two simultaneous readers");

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
            bool mreWasSetWhenReadCountIsZero = false;

            ManualResetEvent mre = new ManualResetEvent(false);
            RWLock rWLock = RWLockBuilder.NewLock();

            Task t1 = Task.Run(() => GetReadLock());
            Task t2 = Task.Run(() => GetReadLock());
            Task t3 = Task.Run(() => GetWriteLock());
            Task t4 = Task.Run(() => GetReadLock());
            Task t5 = Task.Run(() => GetWriteLock());
            
            Assert.IsTrue(SpinWait.SpinUntil(() => count == 0, 10000000), "Unable to have two simultaneous readers");
            
            //Assert.IsTrue(rWLock.CurrentReadCount == 3);
            Assert.IsTrue(count == 0);

            mre.Set();

            // This method is run simultaneously in two tasks
            void GetReadLock()
            {
                if(mreWasSetWhenReadCountIsZero == true)
                {
                    mre.WaitOne();
                }
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
                //if(rWLock.CurrentReadCount == 0)
                if(rWLock.IsReadLockHeld == false)
                {
                    mre.Set();
                    mreWasSetWhenReadCountIsZero = true;
                }
            }
            void GetWriteLock()
            {
                mre.WaitOne();
                if(rWLock.IsWriteLockHeld == true)
                {
                    mre.WaitOne();
                }
                //mre.Reset();
                // Acquire a read lock
                rWLock.EnterWriteLock();

                Assert.IsTrue(rWLock.IsWriteLockHeld);

                // Atomically decrement the shared count variable.  Note that merely doing count-- won't always work.
                Interlocked.Decrement(ref count);

                // Block until the main task sets the mre to true.
                //mre.WaitOne();

                // Exit the read lock
                rWLock.ExitWriteLock();
                mre.Set();
                mreWasSetWhenReadCountIsZero = false;
                Assert.IsFalse(rWLock.IsWriteLockHeld);
            }
        }

        [TestMethod, Timeout(1500)]
        public void TestMethod6()
        {
            // These local variables are used by the GetReadLock method below.  They are accessible
            // to that method because it is nested within TestMethod2.
            int count = 2;
            ManualResetEvent mre1 = new ManualResetEvent(false);
            RWLock rwLock = RWLockBuilder.NewLock();

            // Run GetReadLock() on two tasks.  Wait up to one second for count to be decremented to zero.
            Task t1 = Task.Run(() => GetReadLock());
            Task t2 = Task.Run(() => GetWriteLock());
            Assert.IsTrue(SpinWait.SpinUntil(() => count == 0, 1000), "Unable to have two simultaneous readers");

            Assert.IsTrue(rwLock.CurrentReadCount == 0);
            Assert.IsFalse(rwLock.IsReadLockHeld);
            Assert.IsFalse(rwLock.IsWriteLockHeld);
            Assert.IsTrue(rwLock.WaitingReadCount == 0);
            Assert.IsTrue(rwLock.WaitingWriteCount == 0);
            // This method is run simultaneously in two tasks
            void GetReadLock()
            {
                // Acquire a read lock
                rwLock.EnterReadLock();

                // Atomically decrement the shared count variable.  Note that merely doing count-- won't always work.
                Interlocked.Decrement(ref count);

                // Exit the read lock

                rwLock.ExitReadLock();
                mre1.Set();
            }
            void GetWriteLock()
            {
                mre1.WaitOne();
                // Acquire a read lock
                rwLock.EnterWriteLock();

                //Assert.IsTrue(rwLock.IsWriteLockHeld);

                // Atomically decrement the shared count variable.  Note that merely doing count-- won't always work.
                Interlocked.Decrement(ref count);

                // Exit the read lock
                rwLock.ExitWriteLock();

                //Assert.IsFalse(rwLock.IsWriteLockHeld);
            }
        }


        [TestMethod, Timeout(1500000000)]
        public void TestMethod7()
        {
            // These local variables are used by the GetReadLock method below.  They are accessible
            // to that method because it is nested within TestMethod2.
            int count = 3;
            ManualResetEvent mre1 = new ManualResetEvent(false);
            ManualResetEvent mre2 = new ManualResetEvent(false);
            RWLock rwLock = RWLockBuilder.NewLock();
            // Run GetReadLock() on two tasks.  Wait up to one second for count to be decremented to zero.
            Task t1 = Task.Run(() => GetReadLock());
            Task t2 = Task.Run(() => GetWriteLock());
            Task t3 = Task.Run(() => GetReadLock());
            Assert.IsTrue(SpinWait.SpinUntil(() => count == 1, 1000), "Unable to have two simultaneous readers");
            Assert.IsFalse(rwLock.TryEnterWriteLock(1));
            Assert.IsTrue(rwLock.CurrentReadCount == 2);
            //Assert.IsTrue(rwLock.IsWriteLockHeld);
            mre2.Set();
            t1.Wait();
            t3.Wait();
            t2.Wait();
            Assert.IsTrue(rwLock.CurrentReadCount == 0);
            Assert.IsFalse(rwLock.IsReadLockHeld);
            Assert.IsFalse(rwLock.IsWriteLockHeld);
            Assert.IsTrue(rwLock.WaitingReadCount == 0);
            Assert.IsTrue(rwLock.WaitingWriteCount == 0);
            // This method is run simultaneously in two tasks
            void GetReadLock()
            {
                // Acquire a read lock
                rwLock.EnterReadLock();
                // Atomically decrement the shared count variable.  Note that merely doing count-- won't always work.
                Interlocked.Decrement(ref count);
                mre2.WaitOne();
                // Exit the read lock
                rwLock.ExitReadLock();
                if (rwLock.CurrentReadCount == 0)
                {
                    mre1.Set();
                }
            }
            void GetWriteLock()
            {
                mre1.WaitOne();
                // Acquire a read lock
                rwLock.EnterWriteLock();
                //Assert.IsTrue(rwLock.IsWriteLockHeld);
                // Atomically decrement the shared count variable.  Note that merely doing count-- won't always work.
                Interlocked.Decrement(ref count);
                //mre2.WaitOne();
                // Exit the read lock
                rwLock.ExitWriteLock();
                //Assert.IsFalse(rwLock.IsWriteLockHeld);
            }
        }


        [TestMethod, Timeout(1500)]
        //[TestMethod]
        public void TestMethod8()
        {
            int count = 6;
            bool currentlyNoActiveReaders = false;
            //for write locks
            ManualResetEvent mre1 = new ManualResetEvent(false);
            //for read locks
            ManualResetEvent mre2 = new ManualResetEvent(false);
            RWLock rwLock = RWLockBuilder.NewLock();

            Task t1 = Task.Run(() => GetReadLock());
            Task t2 = Task.Run(() => GetReadLock());
            Task t3 = Task.Run(() => GetWriteLock());
            Task t4 = Task.Run(() => GetReadLock());
            Task t5 = Task.Run(() => GetReadLock());
            Task t6 = Task.Run(() => GetReadLock());

            Assert.IsTrue(SpinWait.SpinUntil(() => count == 0, 1000), "Unable to have two simultaneous readers");
            Assert.IsTrue(count == 0);
            //mre2.Set();
            // This method is run simultaneously in two tasks
            void GetReadLock()
            {
                if (currentlyNoActiveReaders == true)
                {
                    mre2.WaitOne();
                }
                rwLock.EnterReadLock();
                // Atomically decrement the shared count variable.  Note that merely doing count-- won't always work.
                Interlocked.Decrement(ref count);
                // Exit the read lock
                rwLock.ExitReadLock();
                if (rwLock.CurrentReadCount == 0)
                {
                    currentlyNoActiveReaders = true;
                    mre1.Set();
                }
            }
            void GetWriteLock()
            {
                mre1.WaitOne();
                // Acquire a read lock
                rwLock.EnterWriteLock();
                // Atomically decrement the shared count variable.  Note that merely doing count-- won't always work.
                Interlocked.Decrement(ref count);
                // Exit the read lock
                rwLock.ExitWriteLock();
                mre2.Set();
            }
        }




        //Simple writer test
        [TestMethod]
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
        [TestMethod]
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
        [TestMethod]
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
        [TestMethod]
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
        [TestMethod]
        public void TestMethodE()
        {
            ManualResetEvent mre1 = new ManualResetEvent(false);
            RWLock locker = RWLockBuilder.NewLock();
            bool enteredReadLock = false;

            Task t1 = Task.Run(() => GetReadLock());

            mre1.Set();

            Assert.IsTrue(SpinWait.SpinUntil(() => enteredReadLock == true, 10000000));

            Assert.IsTrue(locker.TryEnterReadLock(100));

            //mre1.Set();
            

            void GetReadLock()
            {
                mre1.WaitOne();
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
        [TestMethod]
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
        [TestMethod]
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



















       
    }
}
