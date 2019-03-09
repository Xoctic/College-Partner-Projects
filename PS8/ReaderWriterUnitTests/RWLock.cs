using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ReaderWriterUnitTests
{
    public class RWLock : IDisposable
    {
        private dynamic rwLock;

        public RWLock()
        {
            String lockType = Properties.Settings.Default.LockType;
            rwLock = GetInstance(lockType);
            if (rwLock == null)
            {
                throw new ArgumentException("Lock class " + lockType + " not found");
            }
        }

        // https://stackoverflow.com/questions/223952
        private object GetInstance(string strFullyQualifiedName)
        {
            Type type = Type.GetType(strFullyQualifiedName);
            if (type != null)
                return Activator.CreateInstance(type);
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = asm.GetType(strFullyQualifiedName);
                if (type != null)
                    return Activator.CreateInstance(type);
            }
            return null;
        }

        public virtual void EnterReadLock()
        {
            rwLock.EnterReadLock();
        }

        public virtual void EnterWriteLock()
        {
            rwLock.EnterWriteLock();
        }

        public virtual void ExitReadLock()
        {
            rwLock.ExitReadLock();
        }

        public virtual void ExitWriteLock()
        {
            rwLock.ExitWriteLock();
        }

        public virtual bool TryEnterReadLock(int millisecondsTimeout)
        {
            return rwLock.TryEnterReadLock(millisecondsTimeout);
        }

        public virtual bool TryEnterWriteLock(int millisecondsTimeout)
        {
            return rwLock.TryEnterWriteLock(millisecondsTimeout);
        }

        public virtual void Dispose()
        {
            rwLock.Dispose();
        }
    }
}
