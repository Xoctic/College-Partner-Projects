using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SSGui;
using Formulas;

namespace PS_7
{
    public partial class SpreadsheetWindow : Form, ISpreadsheetView
    {
        /// <summary>
        /// Private instance variables to hold the conents, and value of the current cell selected in spreadsheetPanel1
        /// </summary>
        private string currentContents = "";
        private string currentValue = "";
        private string currentName = "";
       
        /// <summary>
        /// Constructor which initializes a spreadSheet GUI
        /// Hooks a selectionChanged event that occurs inside spreadsheetPanel1 to a method getCellInfo
        /// </summary>
        public SpreadsheetWindow()
        {
            InitializeComponent();
            spreadsheetPanel1.SelectionChanged += getCellInfo;
        }

        /// <summary>
        /// Retrieves the row and column number of the current selected cell
        /// Converts the row and column number into a cellName which is passed into a newCellSectedEvent where it will be used by the spreadsheet model
        /// </summary>
        /// <param name="sender"></param>
        private void getCellInfo(SpreadsheetPanel sender)
        {
            spreadsheetPanel1.GetSelection(out int col, out int row);
            char letter = (char)('A' + col);
            row++;
            string cellName = letter.ToString() + row;
            NewCellSelectedEvent(cellName);
        }

      
        /// <summary>
        /// setter and getter for the contents of the current selected cell in spreadsheetPanel1
        /// </summary>
        public string ContentsOfCurrentCell
        {
            set
            {
                currentContents = value.ToString();
                //cellContentText.Text = value;
            }

            get
            {
                return currentContents;
            }

        }

        /// <summary>
        /// setter and getter for the value of the current selected cell in spreadSheetPanel1
        /// </summary>
        public string ValueOfCurrentCell
        {
            set
            {
                currentValue = value.ToString();
                //spreadsheetPanel1.SetValue(col, row - 1, _text);
            }
            get
            {
                return currentValue;
            }

        }

        /// <summary>
        /// setter and getter for the name of the current selected cell in spreadsheetPanel1
        /// </summary>
        public string currentCellName
        {
            set
            {
                currentName = value;
            }
            get
            {
                return currentName;
            }
        }

        /// <summary>
        /// Sets the title of the spreadSheetWindow
        /// </summary>
        public string Title {
            set { Text = value; }
        }

        /// <summary>
        /// ???
        /// </summary>
        public string Message
        {
            set { MessageBox.Show(value); }
        }

        /// <summary>
        /// Fired when the save button is clicked
        /// </summary>
        public event Action<string> SaveFileEvent;


        /// <summary>
        /// Fired when a file is chosen with a file dialog.
        /// The parameter is the chosen filename.
        /// </summary>
        public event Action<string> OpenFileEvent;


        /// <summary>
        /// Fired when a new window is created
        /// </summary>
        public event Action OpenNewEvent;

        /// <summary>
        /// Fired when help button is clicked.
        /// 
        /// Opens a help window dialogue which explains
        /// how to use the program.
        /// </summary>
        public event Action HelpButtonEvent;

        /// <summary>
        /// Fired when a close action is requested.
        /// </summary>
        public event Action<FormClosingEventArgs> CloseWindowEvent;

        /// <summary>
        /// Fired when the close button is clocked.
        /// </summary>
        public event Action CloseButtonClickedEvent;

        /// <summary>
        /// Fired when the contents of the cellContentsTextBox is changed
        /// </summary>
        public event Action<string,string> UpdateCellEvent;

        /// <summary>
        /// Fired when a new cell is selected
        /// Changes the text in the cellContentsTextBox
        /// to the current cells' contents
        /// </summary>
        public event Action<string> NewCellSelectedEvent;

      

        /// <summary>
        /// sets the new value of the cell in spreadsheetPanel1 that corresponds with the cellName passed in
        /// Converts a cell name into corresponding row and column numbers in spreadSheetPanel1
        /// </summary>
        public void ChangeValueOfCell(string _cellName, string _cellValue)
        {
            spreadsheetPanel1.SetValue(getCol(_cellName), getRow(_cellName), currentValue);
            
        }

        /// <summary>
        /// Handles the event when the enter key is pressed within the cellContentsTextBox
        /// Catches any circularException or invalidNameException using a try/catch and an error winodw without modifying the spreadSheet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cellContentTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
           
                try
                {
                    if (e.KeyChar == (char)Keys.Return)
                    {
                        //appends current string in cellContentsTextBox
                        //retrieves the row and column number of the current selected cell in spreadSheetPanel1
                        string _text = cellContentText.Text;
                        spreadsheetPanel1.GetSelection(out int col, out int row);

                        //converts the row and column number into a readable cell name for the model
                        char letter = (char)('A' + col);
                        row++;
                        string cellName = letter.ToString() + row.ToString();

                        //calls an updateCellEvent using the newly created cellName and the text that was appended in the cellContentsTextBox
                        UpdateCellEvent(cellName, _text);
                        getCellInfo(spreadsheetPanel1);

                        //Sets new value of the cell after being calculated by the model as well as ensuring theevent is handled
                        //FIXED! Was passing in _text instead of currentValue
                        spreadsheetPanel1.SetValue(col, row - 1, currentValue);
                        //ALL YOU NEEDED WAS THIS LINE TO FIX THE ANNOYING NOISE WHEN ENTER IS PRESSED LMAO
                        e.Handled = true;
                    }
                }
                catch (Exception eX)
                {
                    if(eX.GetType() == typeof(CircularException))
                    {
                        //create a new form
                        MessageBox.Show("Can't perform operation because it will result in a circular dependency", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                    if(eX.GetType() == typeof(InvalidNameException) || eX.GetType() == typeof(FormulaFormatException))
                    {
                        MessageBox.Show("Cell Name Entered is Invalid", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        throw eX;
                    }


                }
           
        }

        /// <summary>
        /// Retrieves the column number for spreadsheetPanel1 that is associated with the cellName passed in
        /// </summary>
        /// <param name="_cellName"></param>
        /// <returns></returns>
        public int getCol(string _cellName)
        {
            string letter = _cellName.Substring(0, 1);

            switch (letter.ToUpper())
            {
                case "A":
                    return 0;
                case "B":
                    return 1;
                case "C":
                    return 2;
                case "D":
                    return 3;
                case "E":
                    return 4;
                case "F":
                    return 5;
                case "G":
                    return 6;
                case "H":
                    return 7;
                case "I":
                    return 8;
                case "J":
                    return 9;
                case "K":
                    return 10;
                case "L":
                    return 11;
                case "M":
                    return 12;
                case "N":
                    return 13;
                case "O":
                    return 14;
                case "P":
                    return 15;
                case "Q":
                    return 16;
                case "R":
                    return 17;
                case "S":
                    return 18;
                case "T":
                    return 19;
                case "U":
                    return 20;
                case "V":
                    return 21;
                case "W":
                    return 22;
                case "X":
                    return 23;
                case "Y":
                    return 24;
                case "Z":
                    return 25;
                default:
                    return -1;
                   
            }
           
        }

        /// <summary>
        /// Retrieves the row number for spreadsheetPanel1 that is associated with the cellName passed in
        /// </summary>
        /// <param name="_cellName"></param>
        /// <returns></returns>
        public int getRow(string _cellName)
        {
            string numString;
            if (_cellName.Length == 2)
            {
                numString = _cellName.Substring(1, 1);
            }
            else
            {
                numString = _cellName.Substring(1, 2);
            }
            int result;

            int.TryParse(numString, out result);

            return result - 1;
        }

        /// <summary>
        /// assigns the contents of cellContentTextBox to whatever string is passed in
        /// </summary>
        /// <param name="_cellContents"></param>
        public void updateCell(string _cellContents)
        {
            cellContentText.Text = _cellContents;
        }

        /// <summary>
        /// Handles when the open fileMenuItem is clicked
        /// ???
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fileMenuOpen_Click(object sender, EventArgs e)
        {
            fileDialog.Filter = ".ss SpreadSheet File|*.ss|All files|*.*";
            fileDialog.FilterIndex = 1;
            DialogResult result = fileDialog.ShowDialog();
            if (result == DialogResult.Yes || result == DialogResult.OK)
            {
                if (OpenFileEvent != null)
                {
                    OpenFileEvent(fileDialog.FileName);
                }
            }
        }

        /// <summary>
        /// Handles when the new fileMenuItem is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newMenuItem_Click(object sender, EventArgs e)
        {
            if (OpenNewEvent != null)
            {
                OpenNewEvent();
            }
        }

        /// <summary>
        /// Handles when the save fileMenuItem is clicked
        /// ???
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = ".ss SpreadSheet File|*.ss|All files|*.*";
            saveFileDialog.FilterIndex = 1;
            DialogResult result = saveFileDialog.ShowDialog();
            if (result == DialogResult.Yes || result == DialogResult.OK)
            {
                if (SaveFileEvent != null)
                {
                    SaveFileEvent(saveFileDialog.FileName);
                }
            }
        }
        
        /// <summary>
        /// Handles when the help fileMenuItem is clicked
        /// ???
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void helpMenuItem_Click(object sender, EventArgs e)
        {
            if (HelpButtonEvent != null)
            {
                HelpButtonEvent();
            }
        }

        /// <summary>
        /// Handles when the close menuItem is clicked
        /// ???
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CloseButtonClickedEvent != null)
            {
                CloseButtonClickedEvent();
            }
        }

        /// <summary>
        ///  Closes the current window
        /// </summary>
        public void DoClose()
        {
            Close();
        }

        /// <summary>
        /// Runs a new spreadSheetWindow
        /// </summary>
        public void OpenNew()
        {
            SpreadsheetApplicationContext.GetContext().RunNew();
        }

        /// <summary>
        /// Handles the event of when arrow keys are pressed to navigate in the spreadsheet
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            spreadsheetPanel1.GetSelection(out int x, out int y);
            if(keyData == Keys.Up)
            {
                spreadsheetPanel1.SetSelection(x, y - 1);

                return true;
            }
            else if(keyData == Keys.Down)
            {
                spreadsheetPanel1.SetSelection(x, y + 1);
                return true;
            }
            else if (keyData == Keys.Right)
            {
                spreadsheetPanel1.SetSelection(x + 1, y);
                return true;
            }
            else if (keyData == Keys.Left)
            {
                spreadsheetPanel1.SetSelection(x - 1, y);
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>
        /// ???
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpreadsheetWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseWindowEvent(e);
        }
    }
}
