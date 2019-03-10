using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SS;

namespace SS
{
    public class Controller
    {
        private Interface window;
        private AbstractSpreadsheet model;

        public Controller(Interface _window)
        {
            this.window = window;
            this.model = new Spreadsheet();
            window.CloseWindowEvent += HandleClose;
            window.HelpButtonEvent += HandleHelp;
            window.NewCellSelectedEvent += HnadleCellSelected;
            window.OpenFileEvent += HandleOpenFile;
            window.SaveFileEvent += HandleSave;
            window.UpdateCellEvent += HandleUpdateCell;
        }

        private void HandleUpdateCell()
        {
            throw new NotImplementedException();
        }

        private void HandleSave(string obj)
        {
            throw new NotImplementedException();
        }

        private void HandleOpenFile(string obj)
        {
            window.DoOpen();
        }

        private void HnadleCellSelected()
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
