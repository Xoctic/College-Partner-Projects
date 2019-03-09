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
    public class BadReaderWriterLock : RWLock
    {
        public override int CurrentReadCount => 0;

        public override bool IsReadLockHeld => false;

        public override int WaitingReadCount => 0;

        public override bool IsWriteLockHeld => false;

        public override int WaitingWriteCount => 0;

        public override void Dispose()
        {
        }

        public override void EnterReadLock()
        {
        }

        public override void EnterWriteLock()
        {
        }

        public override void ExitReadLock()
        {
        }

        public override void ExitWriteLock()
        {
        }

        public override bool TryEnterReadLock(int millisecondsTimeout)
        {
            return false;
        }

        public override bool TryEnterWriteLock(int millisecondsTimeout)
        {
            return false;
        }
    }
}
