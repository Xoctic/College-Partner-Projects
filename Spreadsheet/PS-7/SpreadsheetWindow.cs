using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PS_7
{
    public partial class SpreadsheetWindow : Form, ISpreadsheetView
    {
        public SpreadsheetWindow()
        {
            InitializeComponent();
        }

        public string ContentsOfCell
        {
            set
            {
                cellContentTextBox.Text = value;
            }
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
        public event Action<string> UpdateCellEvent;

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

        public void updateCell(string _cellContents)
        {
            cellContentTextBox.Text = _cellContents;
            
        }

        

        /// <summary>
        /// Closes the current window
        /// </summary>
        public void DoClose()
        {
            Close();
        }

        /// <summary>
        /// Opens a new window
        /// </summary>
        public void OpenNew()
        {
            SpreadsheetApplicationContext.GetContext().RunNew();
        }


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
                    break;
            }
            return -1;
        }


        public int getRow(string _cellName)
        {
            string numString;
            if(_cellName.Length == 2)
            {
                numString = _cellName.Substring(1, 2);
            }
            else
            {
                numString = _cellName.Substring(1, 3);
            }
            int result;

            int.TryParse(numString, out result);

            return result-1;
        }

        private void OpenClicked(object sender, EventArgs e)
        {

        }
    }
}
