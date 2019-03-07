using System;
using Formulas;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SS;

namespace SpeadSheetTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void lookerTest()
        {
            AbstractSpreadsheet s = new Spreadsheet();

            s.SetContentsOfCell("A1", "100");
            s.SetContentsOfCell("A2", "=A1+50");
            Assert.AreEqual(s.GetCellValue("A2"), (double)150);

            
        }
    }
}
