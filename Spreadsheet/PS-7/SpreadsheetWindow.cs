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

namespace PS_7
{
    public partial class SpreadsheetWindow : Form, ISpreadsheetView
    {
       
        public SpreadsheetWindow()
        {
            InitializeComponent();
            spreadsheetPanel1.SelectionChanged += getCellInfo;
     
            //updateCell += displayContentsOfCell;
        }

        private void getCellInfo(SpreadsheetPanel sender)
        {
            spreadsheetPanel1.GetSelection(out int col, out int row);
            char letter = (char)('A' + col);
            row++;
            string cellName = letter.ToString() + row;
            NewCellSelectedEvent(cellName);
        }

      

        public string ContentsOfCell
        {
            set
            {
                cellContentText.Text = value;
            }
        }

        public string Title {
            set { Text = value; }
        }

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
        public event Action CloseWindowEvent;

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
        /// 
        /// </summary>
        public void ChangeValueOfCell(string _cellName, string _cellValue)
        {
            spreadsheetPanel1.SetValue(getCol(_cellName), getRow(_cellName), _cellValue);
            
        }

        private void cellContentTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Return)
            {

                string _text = cellContentText.Text;
                spreadsheetPanel1.GetSelection(out int col, out int row);

                char letter = (char)('A' + col);
                row++;
                string cellName = letter.ToString() + row.ToString();
                UpdateCellEvent(cellName, _text);
                getCellInfo(spreadsheetPanel1);

                spreadsheetPanel1.SetValue(col, row-1, _text);
                //ALL YOU NEEDED WAS THIS LINE TO FIX THE ANNOYING NOISE WHEN ENTER IS PRESSED LMAO
                e.Handled = true; 
            }
        }


        public int getCol(string _cellName)
        {
            string letter = _cellName.Substring(0, 0);

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
                    break;
            }
            return -1;
        }


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

        public void updateCell(string _cellContents)
        {
            cellContentText.Text = _cellContents;
        }

        private void fileMenuOpen_Click(object sender, EventArgs e)
        {
            DialogResult result = fileDialog.ShowDialog();
            if (result == DialogResult.Yes || result == DialogResult.OK)
            {
                if (OpenFileEvent != null)
                {
                    OpenFileEvent(fileDialog.FileName);
                }
            }
        }

        private void newMenuItem_Click(object sender, EventArgs e)
        {
            if (OpenNewEvent != null)
            {
                OpenNewEvent();
            }
        }

        private void saveMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = ".ss File|*.ss";
            DialogResult result = saveFileDialog.ShowDialog();
            if (result == DialogResult.Yes || result == DialogResult.OK)
            {
                if (SaveFileEvent != null)
                {
                    SaveFileEvent(saveFileDialog.FileName);
                }
            }
        }

        private void helpMenuItem_Click(object sender, EventArgs e)
        {
            //Not implemented
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CloseWindowEvent != null)
            {
                CloseWindowEvent();
            }
        }

        public void DoClose()
        {
            Close();
        }

        public void OpenNew()
        {
            SpreadsheetApplicationContext.GetContext().RunNew();
        }
    }
}
