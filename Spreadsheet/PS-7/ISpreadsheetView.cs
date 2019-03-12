using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PS_7
{
    public interface ISpreadsheetView
    {
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

        string currentCellName { set; }

        string Title { set; }

        string Message { set; }

        void ChangeValueOfCell(string cellName, string cellContents);

        void DoClose();

        void OpenNew();

        void updateCell(string cellContents);
       
    }
}
