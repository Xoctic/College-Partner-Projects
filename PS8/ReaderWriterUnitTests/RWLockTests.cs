using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace ReaderWriterUnitTests
{
    [TestClass]
    public class RWLockTests
    {
        [TestMethod]
        [ExpectedException(typeof(SynchronizationLockException))]
        public void TestMethod1()
        {
            RWLock rwLock = new RWLock();
            rwLock.EnterReadLock();
            rwLock.ExitWriteLock();
        }
    }
}
