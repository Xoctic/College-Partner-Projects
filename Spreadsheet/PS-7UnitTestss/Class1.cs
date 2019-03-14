using Microsoft.VisualStudio.TestTools.UnitTesting;
using SS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS_7UnitTestss
{
    [TestClass]
    class UnitTestss
    {
        [TestMethod]
        public void TestMethod1()
        {
            Stub stub = new Stub();
            Controller controller = new Controller(stub);
            stub.FireCloseEvent();
            Assert.IsTrue(stub.CalledDoClose);
        }

        [TestMethod]
        public void TestMethod2()
        {
            Stub stub = new Stub();
            Controller controller = new Controller(stub);

            stub.FireFileChosenEvent("testfile1.txt");
            Assert.AreEqual("testfile1.txt", stub.Title);
            Assert.AreEqual(1, stub.LineCount);
            Assert.AreEqual(4, stub.WordCount);
            Assert.AreEqual(16, stub.CharCount);

            stub.FireCountEvent("is");
            Assert.AreEqual(2, stub.SubstringCount);
        }
    }
}
