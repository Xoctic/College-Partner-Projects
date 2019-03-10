using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS_7
{
    class Controller
    {
        private ISpreadsheetView window;
        private AbstractSpreadsheet model;

        public Controller(ISpreadsheetView _window)
        {
            this.window = _window;
            this.model = new Spreadsheet();
            window.CloseWindowEvent += HandleClose;
            window.HelpButtonEvent += HandleHelp;
            window.NewCellSelectedEvent += HandleCellSelected;
            window.OpenFileEvent += HandleOpenFile;
            window.SaveFileEvent += HandleSave;
            window.UpdateCellEvent += HandleUpdateCell;
            window.OpenNewEvent += OpenNewWindow;
        }

        private void HandleUpdateCell(string _cellName, string _cellContents)
        {
            foreach(string el in model.SetContentsOfCell(_cellName, _cellContents))
            {

            }
           
        }

        private void HandleSave(string obj)
        {
            throw new NotImplementedException();
        }

        private void HandleOpenFile(string obj)
        {
            
        }

        private void OpenNewWindow()
        {
            window.OpenNew();
        }

        private void HandleCellSelected(string _cellName)
        {
            
            

            throw new NotImplementedException();
        }

        private void HandleHelp()
        {
            throw new NotImplementedException();
        }

        private void HandleClose()
        {
            window.DoClose();
        }


    }
}
