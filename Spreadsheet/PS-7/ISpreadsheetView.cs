using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS_7
{
    public interface ISpreadsheetView
    {
        event Action<string> SaveFileEvent;

        event Action<string> OpenFileEvent;

        event Action OpenNewEvent;

        event Action HelpButtonEvent;

        event Action CloseWindowEvent;

        event Action<string> UpdateCellEvent;

        event Action<string> NewCellSelectedEvent;

        string ContentsOfCell { set; }

        string Title { set; }

        string Message { set; }

        void ChangeValueOfCell(string cellName, string cellContents);

        void DoClose();

        void OpenNew();

        void updateCell(string cellContents);
       
    }
}
