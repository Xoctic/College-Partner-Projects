using SS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Formulas;

namespace DevelopmentTests
{
    /// <summary>
    /// These are grading tests for PS5
    ///</summary>
    [TestClass()]
    public class SpreadsheetTest
    {
        // EMPTY SPREADSHEETS
        [TestMethod()]
        public void Test3()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            Assert.AreEqual("", s.GetCellContents("A2"));
        }

        // SETTING CELL TO A DOUBLE
        [TestMethod()]
        public void Test6()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetCellContents("Z7", 1.5);
            Assert.AreEqual(1.5, (double)s.GetCellContents("Z7"), 1e-9);
        }

        // SETTING CELL TO A STRING
        [TestMethod()]
        public void Test10()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetCellContents("Z7", "hello");
            Assert.AreEqual("hello", s.GetCellContents("Z7"));
        }

        // SETTING CELL TO A FORMULA
        [TestMethod()]
        public void Test13()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetCellContents("Z7", new Formula("3"));
            Formula f = (Formula)s.GetCellContents("Z7");
            Assert.AreEqual(3, f.Evaluate(x => 0), 1e-6);
        }

        // CIRCULAR FORMULA DETECTION
        [TestMethod()]
        [ExpectedException(typeof(CircularException))]
        public void Test14()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", new Formula("A2"));
            s.SetCellContents("A2", new Formula("A1"));
        }

        [TestMethod()]
        public void getCellsToRecalculateSingleStringTest()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetCellContents("A1", new Formula("3"));
            s.SetCellContents("A2", new Formula("A11"));
            s.SetCellContents("A3", new Formula("3"));
            s.SetCellContents("A4", new Formula("A10"));

            s.SetCellContents("A4", new Formula("3"));

            Assert.IsTrue(s.GetCellContents("A4").ToString() == "3");
        }




        [TestMethod()]
        public void regexNameTest()
        {
            Spreadsheet s = new Spreadsheet();
            Assert.IsFalse(s.validName("A1A"));
            Assert.IsTrue(s.validName("A1"));
            Assert.IsTrue(s.validName("AAAAAAA1111111"));
            Assert.IsFalse(s.validName("AAAAAAAA111111A1111"));
            Assert.IsFalse(s.validName("AAAaAAA1111a111"));
        }



    }
}
