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
            s.SetContentsOfCell("Z7", "1.5");
            Assert.AreEqual(1.5, (double)s.GetCellContents("Z7"), 1e-9);
        }

        // SETTING CELL TO A STRING
        [TestMethod()]
        public void Test10()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("Z7", "hello");
            Assert.AreEqual("hello", s.GetCellContents("Z7"));
        }

        // SETTING CELL TO A FORMULA
        [TestMethod()]
        public void Test13()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("Z7", "=3");
            Formula f = (Formula)s.GetCellContents("Z7");
            Assert.AreEqual(3, f.Evaluate(x => 0), 1e-6);
        }

        // CIRCULAR FORMULA DETECTION
        [TestMethod()]
        [ExpectedException(typeof(CircularException))]
        public void Test14()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "=A2");
            s.SetContentsOfCell("A2", "=A1");
        }

        [TestMethod()]
        public void getCellsToRecalculateSingleStringTest()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "=3");
            s.SetContentsOfCell("A2", "=A11");
            s.SetContentsOfCell("A3", "=3");
            s.SetContentsOfCell("A4", "=A10");

            s.SetContentsOfCell("A4", "=3");

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


        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
         public void getCellContentsNullName()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "3");
            s.GetCellContents(null);
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void getCellContentsInvalidName()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "3");
            s.GetCellContents("A1A");
        }

        [TestMethod()]
        public void getNamesOfAllNonEmptyCells()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "1");
            s.SetContentsOfCell("A2", "2");
            s.SetContentsOfCell("A3", "3");
            s.SetContentsOfCell("A4", "4");
            IEnumerable<string> cellies = s.GetNamesOfAllNonemptyCells();

            int counter = 0;

            foreach(string el in cellies)
            {
                switch(counter)
                {
                    case 0:
                        Assert.IsTrue(el == "A1");
                        Assert.IsFalse(el == "A2");
                    break;

                    case 1:
                        Assert.IsTrue(el == "A2");
                        Assert.IsFalse(el == "A1");
                    break;

                    case 2:
                        Assert.IsTrue(el == "A3");
                        Assert.IsFalse(el == "A4");
                    break;

                    case 3:
                        Assert.IsTrue(el == "A4");
                        Assert.IsFalse(el == "A3");
                    break;
                }
                counter++;
            }
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void setCellContentsUsingDoubleAndNullName()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell(null, "2.5");
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void setCellContentsUsingDoubleAndInvalidName()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1A", "2.5");
        }

        //setCellContents with double
        [TestMethod()]
        public void replacingAnExistingCell()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "1.5");
            s.SetContentsOfCell("A2", "2.5");
            s.SetContentsOfCell("A3", "3.5");
            s.SetContentsOfCell("A3", "4.5");

            Assert.IsTrue(s.GetCellContents("A3").ToString() == "4.5");
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void setCellContentsUsingTextAndNullName()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell(null, "hi");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void setCellContentsUsingTextAndInvalidName()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1A", "hi");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void setCellContentsUsingNullText()
        {
            Spreadsheet s = new Spreadsheet();
            throw new ArgumentNullException();
            //wont let me pass in null text
        }

        //setCellContents with text
        [TestMethod]
        public void replacingAnExistingCell2()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "hi");
            s.SetContentsOfCell("A2", "My name is");
            s.SetContentsOfCell("A3", "what?");
            s.SetContentsOfCell("A3", "slim shady");

            Assert.IsTrue(s.GetCellContents("A3").ToString() == "slim shady");
        }

        //
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void setCellContentsUsingFormula1()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell(null, "=A1");
        }

        //passing in invalid cell name
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void setCellContentsUsingFormula2()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1A", "=A2");
        }

        //Formula contains name of cell
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void setCellContentsUsingFormula3()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "=A2 + A1");
        }

        //Formula contains invalid cell name
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void setCellContentsUsingFormula4()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "=A1A + A2");
        }

        //changing cell whose name is contained in other cells formulas
        //throws circular exception using text, formula, or double passed in for A1
        [TestMethod]
        public void replacingExistingCell3()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "=5");
            s.SetContentsOfCell("A2", "=A1");
            s.SetContentsOfCell("A3", "=A2 + A1");
            s.SetContentsOfCell("A1", "=A5");
        }

        [TestMethod]
        public void replacingExistingCell4()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "5");
            s.SetContentsOfCell("A2", "A1");
            s.SetContentsOfCell("A3", "A1 + A2");
            s.SetContentsOfCell("A3", "A4");
        }

        //pass in null name
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void getDirectDependents1()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "1.5");

            throw new InvalidNameException();
        }

        [TestMethod]
        public void makeCell()
        {
            cell c = new cell("a1", 3, typeof(double));
            Assert.IsTrue(c.content.ToString() == "a1");
            c.value = 1;
            Assert.IsTrue(c.value.ToString() == "1");
        }

    }
}
