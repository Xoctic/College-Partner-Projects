using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            window.NewCellSelectedEvent += HandleUpdateCell;
            window.OpenFileEvent += HandleFileChosen;
            window.SaveFileEvent += HandleSave;
            window.UpdateCellEvent += HandleUpdateCell;
            window.OpenNewEvent += OpenNewWindow;
        }

        private void HandleUpdateCell(string _cellName, string _contents)
        {
            model.SetContentsOfCell(_cellName, _contents);
            ReturnCellContents(_cellName);
            ReturnCellValue(_cellName);
        }

        private void ReturnCellContents(string _cellName)
        {
            string cellContents = model.GetCellContents(_cellName).ToString();
            window.updateCell(cellContents);
        }

        private void ReturnCellValue(string _cellName)
        {
            string cellValue = model.GetCellValue(_cellName).ToString();
            window.ChangeValueOfCell(_cellName, cellValue);
        }

        private void HandleSave(string filename)
        {
            //try
            //{
            //    string contents = File.ReadAllText(filename);
            //    StringWriter toBeSaved;
            //    File.WriteAllText(to)
            //    model.Save();
            //    window.SubstringCount = 0;
            //    window.SearchString = "";
            //    window.Title = filename;
            //}
            //catch (Exception ex)
            //{
            //    window.Message = "Unable to open file\n" + ex.Message;
            //}
        }

        private void HandleFileChosen(string filename)
        {
            try
            {
                string contents = File.ReadAllText(filename);
                StringReader reader = new StringReader(contents);
                model = new Spreadsheet(reader, new Regex("[a-zA-Z][1-9][0-9]"));
                window.Title = filename;
                SpreadsheetApplicationContext.GetContext().RunNew(window);
            }
            catch (Exception ex)
            {
                window.Message = "Unable to open file\n" + ex.Message;
            }
        }

        private void OpenNewWindow()
        {
            window.OpenNew();
        }

        private void HandleCellSelected(string _cellName)
        {
            ReturnCellContents(_cellName);
            ReturnCellValue(_cellName);
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
