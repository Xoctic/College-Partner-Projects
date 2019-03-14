using System;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PS_7;

namespace PS_7UnitTestss
{
    class SpreadsheetwindowStub : ISpreadsheetView
    {
        // These four properties record whether a method has been called
        public bool CalledDoClose
        {
            get; private set;
        }

        public bool CalledOpenNew
        {
            get; private set;
        }

        public bool CalledChangeValueOfCell
        {
            get; private set;
        }

        public bool CalledOpenNewOpenedFile
        {
            get; private set;
        }

        public bool CalledUpdateCell
        {
            get; private set;
        }

        // These eight methods cause events to be fired
        public void FireCloseEvent()
        {
            if (CloseButtonClickedEvent != null)
            {
                CloseButtonClickedEvent();
            }
        }

        public void FireHelpButtonEvent()
        {
            if (HelpButtonEvent != null)
            {
                HelpButtonEvent();
            }
        }

        public void FireFileChosenEvent(string filename)
        {
            if (OpenFileEvent != null)
            {
                OpenFileEvent(filename);
            }
        }

        public void FireNewEvent()
        {
            if (OpenNewEvent != null)
            {
                OpenNewEvent();
            }
        }
        public void FireSaveFileEvent(string filename)
        {
            if (SaveFileEvent != null)
            {
                SaveFileEvent(filename);
            }
        }
        public void FireCloseWindowEvent(FormClosingEventArgs e)
        {
            if (CloseWindowEvent != null)
            {
                CloseWindowEvent(e);
            }
        }
        public void FireUpdateCellEvent(string cellName, string text)
        {
            if (UpdateCellEvent != null)
            {
                UpdateCellEvent(cellName, text);
            }
        }
        public void FireNewCellSelectedEvent(string cellName)
        {
            if (NewCellSelectedEvent != null)
            {
                NewCellSelectedEvent(cellName);
            }
        }
        public string ContentsOfCurrentCell { get; set; }
        public string ValueOfCurrentCell { get; set; }
        public string currentCellName { get; set; }
        public string Title { set; get; }
        public string Message { set; get; }

        public event Action<string> SaveFileEvent;
        public event Action<string> OpenFileEvent;
        public event Action OpenNewEvent;
        public event Action HelpButtonEvent;
        public event Action CloseButtonClickedEvent;
        public event Action<FormClosingEventArgs> CloseWindowEvent;
        public event Action<string, string> UpdateCellEvent;
        public event Action<string> NewCellSelectedEvent;

        public void ChangeValueOfCell(string cellName, string cellContents)
        {
            CalledChangeValueOfCell = true;
        }

        public void DoClose()
        {
            CalledDoClose = true;
        }

        public void OpenNew()
        {
            CalledOpenNew = true;
        }

        public void OpenNewOpenedFile(AbstractSpreadsheet model)
        {
            CalledOpenNewOpenedFile = true;
        }

        public void updateCell(string cellContents)
        {
            CalledUpdateCell = true;
        }
    }
}