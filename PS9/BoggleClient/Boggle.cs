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
    public partial class Boggle : Form, IBogleView
    {
        public bool IsUserRegistered { get; set;  }

        public Boggle()
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
            //Commented out code from Professors Example, possibly needed.
            //cancelButton.Enabled = !state;
        }

        public event Action<string, string> RegisterPressed;

        public event Action<string, string> JoinGamePressed;

        public event Action<string, string> QuitGamePressed;

        public event Action CancelPressed;

        private void RegisterUserButton_Click(object sender, EventArgs e)
        {

        }

        private void JoinGameButton_Click(object sender, EventArgs e)
        {

        }

        private void QuitGameButton_Click(object sender, EventArgs e)
        {

        }

        private void CancelButton_Click(object sender, EventArgs e)
        {

        }

        public void Clear()
        {
            throw new NotImplementedException();
        }
    }
}
