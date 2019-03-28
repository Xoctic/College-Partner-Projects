using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BoggleClient
{
    public partial class BoggleView : Form, IBogleView
    {
        public bool IsUserRegistered { get; set;  }

        public event Action<string, string> RegisterPressed;

        public event Action<int> JoinGamePressed;

        public event Action<string, string> QuitGamePressed;

        public event Action CancelPressed;

        public BoggleView()
        {
            InitializeComponent();
        }

        public void EnableControls(bool state)
        {
            RegisterUserButton.Enabled = state && NameTextBox.Text.Length > 0 && ServerTextBox.Text.Length > 0;
            JoinGameButton.Enabled = state && IsUserRegistered && TimeTextBox.Text.Length > 0;
            QuitGameButton.Enabled = state && IsUserRegistered;
            //Commented out code from Professors Example, possibly needed.
            //showCompletedTasksButton.Enabled = state && IsUserRegistered;

            ServerTextBox.Enabled = state;
            NameTextBox.Enabled = state;
            TimeTextBox.Enabled = state;

            foreach (Control control in BogleGrid.Controls)
            {
                if (control is Button)
                {
                    control.Enabled = state && IsUserRegistered;
                }
            }
            CancelButton.Enabled = !state;
        }

       

        private void RegisterUserButton_Click(object sender, EventArgs e)
        {
            //RegisterPressed(NameTextBox.Text.Trim(), ServerTextBox.Text.Trim());
            RegisterPressed?.Invoke(NameTextBox.Text.Trim(), ServerTextBox.Text.Trim());
        }

        private void JoinGameButton_Click(object sender, EventArgs e)
        {
            //Incomplete method
            CancelButton.Visible = true;
        }

        private void QuitGameButton_Click(object sender, EventArgs e)
        {

        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            CancelPressed?.Invoke();
            CancelButton.Visible = false;
        }

        private void WordTextBox_EnterPressed(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Return)
            {
                //Incomplete method
            }
        }

        private void Registration_TextChanged(object sender, EventArgs e)
        {
            EnableControls(true);
        }

        private void TimeLimit_TextChanged(object sender, EventArgs e)
        {
            EnableControls(true);
        }
        public void Clear()
        {
            
        }
    }
}
