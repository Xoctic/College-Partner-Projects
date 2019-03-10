using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SS
{
    public interface Interface
    {
        event Action<string> SaveFileEvent;

        event Action<string> OpenFileEvent;

        event Action HelpButtonEvent;

        event Action CloseWindowEvent;

        event Action UpdateCellEvent;

        event Action NewCellSelectedEvent;

        string ContentsOfCell { set; }

        void ChangeValueOfCell();

        void DoClose();

        void DoOpen();

        void changeValueOfCell(string name, string ContentsOfCell);
        
    }
}
