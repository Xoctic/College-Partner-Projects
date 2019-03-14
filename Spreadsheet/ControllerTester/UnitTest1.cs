using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PS_7;
using PS_7UnitTestss;
using SS;

namespace ControllerTester
{
    [TestClass]
    public class UnitTest1
    {
        [TestClass]
        public class UnitTests
        {
            [TestMethod]
            public void TestMethod1()
            {
                SpreadsheetwindowStub stub = new SpreadsheetwindowStub();
                Controller controller = new Controller(stub);
                stub.FireCloseEvent();
                Assert.IsTrue(stub.CalledDoClose);
            }

            //[TestMethod]
            //public void TestMethod2()
            //{
            //    AnalysisViewStub stub = new AnalysisViewStub();
            //    Controller controller = new Controller(stub);

            //    stub.FireFileChosenEvent("testfile1.txt");
            //    Assert.AreEqual("testfile1.txt", stub.Title);
            //    Assert.AreEqual(1, stub.LineCount);
            //    Assert.AreEqual(4, stub.WordCount);
            //    Assert.AreEqual(16, stub.CharCount);

            //    stub.FireCountEvent("is");
            //    Assert.AreEqual(2, stub.SubstringCount);
            //}
        }
    }
}
