using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
//Authors:  Andrew Hare(u1033940), Aric Campbell(u1188031)

namespace PS_7
{
    class Controller
    {
        //only two instance variable used for the controller
        //one is the window which is an interface
        //other is the model which is a spreadsheet object
        private ISpreadsheetView window;
        private AbstractSpreadsheet model;

        /// <summary>
        /// constuctor for the controller takes in an interface, and initializes the winodw and model
        /// also hooks all the events in the window and handles them in the various methods below
        /// </summary>
        /// <param name="_window"></param>
        public Controller(ISpreadsheetView _window)
        {
            this.window = _window;
            this.model = new Spreadsheet(new Regex("^([a-zA-Z][1-9]([0-9]?))$"));
 
            window.CloseWindowEvent += HandleExitWindow;
            window.CloseButtonClickedEvent += HandleCloseButtonClick;
            window.HelpButtonEvent += HandleHelp;
            window.NewCellSelectedEvent += HandleCellSelected;
            window.OpenFileEvent += HandleFileChosen;
            window.SaveFileEvent += HandleSave;
            window.UpdateCellEvent += HandleUpdateCell;
            window.OpenNewEvent += OpenNewWindow;
            
        }

        public Controller(ISpreadsheetView _window, AbstractSpreadsheet _model)
        {
            this.window = _window;
            this.model = _model;

            window.CloseWindowEvent += HandleExitWindow;
            window.CloseButtonClickedEvent += HandleCloseButtonClick;
            window.HelpButtonEvent += HandleHelp;
            window.NewCellSelectedEvent += HandleCellSelected;
            window.OpenFileEvent += HandleFileChosen;
            window.SaveFileEvent += HandleSave;
            window.UpdateCellEvent += HandleUpdateCell;
            window.OpenNewEvent += OpenNewWindow;
        }


        /// <summary>
        /// uses the model to set the contents of the cell
        /// uses the methods returnCellContents
        /// </summary>
        /// <param name="_cellName"></param>
        /// <param name="_contents"></param>
        private void HandleUpdateCell(string _cellName, string _contents)
        {
            model.SetContentsOfCell(_cellName, _contents);

            ReturnCellContents(_cellName);
            ReturnCellValue(_cellName);
            //updates all cells in the spreadsheet after a cell value is changed
            foreach(string cell in model.GetNamesOfAllNonemptyCells())
            {
                ReturnCellContents(cell);
                ReturnCellValue(cell);
            }
        }

        private void ReturnCellContents(string _cellName)
        {
            string cellContents = model.GetCellContents(_cellName).ToString();
            window.ContentsOfCurrentCell = cellContents;

            window.updateCell(cellContents);
        }

        private void ReturnCellValue(string _cellName)
        {
            string cellValue = model.GetCellValue(_cellName).ToString();
            window.ValueOfCurrentCell = cellValue;
            window.ChangeValueOfCell(_cellName, window.ValueOfCurrentCell);
        }

        private void HandleSave(string filename)
        {
            try
            {
                StringWriter stringWriter = new StringWriter();
                model.Save(stringWriter);
                File.WriteAllText(filename, stringWriter.ToString());
            }
            catch (Exception ex)
            {
                window.Message = "Unable to save file\n" + ex.Message;
            }
        }

        private void HandleFileChosen(string filename)
        {
            try
            {
                string contents = File.ReadAllText(filename);
                StringReader reader = new StringReader(contents);
                AbstractSpreadsheet modelToOpen = new Spreadsheet(reader, new Regex("^([a-zA-Z][1-9]([0-9]?))$"));
                window.OpenNewOpenedFile(modelToOpen);
                //window.Title = filename;
                foreach (string cell in modelToOpen.GetNamesOfAllNonemptyCells())
                {
                    ReturnCellContents(cell);
                    ReturnCellValue(cell);
                }
            }
            catch (Exception ex)
            {
                window.Message = "Unable to open file\n" + ex.Message;
            }
        }

        private void OpenNewWindowSelectedFile()
        {

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
            HelpMenu helpMenu = new HelpMenu();
            helpMenu.Show();
        }

        private void HandleExitWindow(FormClosingEventArgs e)
        {
            if(model.Changed == true)
            {
                DialogResult dialog = MessageBox.Show("Do you really want to close the Spread Sheet without saving?", "Exit", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }
        private void HandleCloseButtonClick()
        {
            window.DoClose();
        }

    }
}
