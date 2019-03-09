// Written by Joseph Zachary for CS 3500
// Copyright Joseph Zachary, March 2019

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReaderWriterLockClasses
{
    public static class RWLockBuilder
    {
        /// <summary>
        /// Creates a new RWLock by creating and wrapping an underlying lock object, such as a ReaderWriterLockSlim.
        /// The full qualified class name of the underlying lock class is obtained from the project setting LockType.
        /// To change this setting, open the project properties and go to the Settings tab. Throws an ArgumentException
        /// if there is no LockType setting or if it does not name a valid class.
        /// </summary>
        public static RWLock NewLock()
        {
            // Create an object to wrap by using the zero-parameter constructor via reflection
            String lockType = Properties.Settings.Default.LockType;
            RWLock rwLock = (RWLock)GetInstance(lockType);
            if (rwLock == null)
            {
                throw new ArgumentException("Lock class " + lockType + " not found");
            }
            else
            {
                return rwLock;
            }
        }

        /// <summary>
        /// Creates an instance of the specified class by using reflection to call its 
        /// zero-parameter constructor.  Copied from https://stackoverflow.com/questions/223952
        /// </summary>
        private static object GetInstance(string strFullyQualifiedName)
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

    }
}
