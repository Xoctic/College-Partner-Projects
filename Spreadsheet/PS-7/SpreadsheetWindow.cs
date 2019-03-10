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
        public event Action<string, string> UpdateCellEvent;

        /// <summary>
        /// Fired when a new cell is selected
        /// Changes the text in the cellContentsTextBox
        /// to the current cells' contents
        /// </summary>
        public event Action<string> NewCellSelectedEvent;

        /// <summary>
        /// 
        /// </summary>
        public void ChangeValueOfCell(string _cellName, string _cellContents)
        {




            throw new NotImplementedException();
        }

        public void UpdateCell(string _cellName, string _cellContents)
        {

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
    }
}
