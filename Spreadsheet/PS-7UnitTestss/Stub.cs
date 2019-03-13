using System;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PS_7;
using SSGui;

namespace PS_7UnitTestss
{
    [TestClass]
    public class Stub : ISpreadsheetView
    {
        private string currentContents = "";
        private string currentValue = "";
        private string currentName = "";

        SpreadsheetPanel.SelectionChanged += getCellInfo;

        event Action<string> SaveFileEvent;

        event Action<string> OpenFileEvent;

        event Action OpenNewEvent;

        event Action HelpButtonEvent;

        event Action CloseButtonClickedEvent;

        event Action<FormClosingEventArgs> CloseWindowEvent;

        event Action<string, string> UpdateCellEvent;

        event Action<string> NewCellSelectedEvent;



        //event Action<string> updateCell;

        string ContentsOfCurrentCell { get; set; }

        string ValueOfCurrentCell { get; set; }

        string currentCellName { get; set; }

        string Title { set; get; }

        string Message { set; get; }

        void ChangeValueOfCell(string cellName, string cellContents)
        {

        }

        void DoClose()
        {

        }

        void OpenNew()
        {

        }

        void updateCell(string cellContents)
        {

        }





        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}
