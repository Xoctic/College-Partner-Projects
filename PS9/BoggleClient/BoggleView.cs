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
        public bool IsUserRegistered { get; set; }

        public event Action<string, string> RegisterPressed;

        public event Action<int> JoinGamePressed;

        public event Action<string, string> QuitGamePressed;

        public event Action CancelPressed;

        public event Action<string> EnterPressedInWordTextBox;

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

        public void SetBoard(string board)
        {
            int counter = 0;
            char[] array = board.ToCharArray();

            foreach (Control control in BogleGrid.Controls)
            {
                Label label = control as Label;
                if (label != null)
                {
                    label.Text = array[counter].ToString();
                }

                counter++;
            }
        }

        private void RegisterUserButton_Click(object sender, EventArgs e)
        {
            RegisterPressed?.Invoke(NameTextBox.Text.Trim(), ServerTextBox.Text.Trim());
        }

        private void JoinGameButton_Click(object sender, EventArgs e)
        {
            CancelButton.Visible = true;
            CancelButton.Enabled = true;
            JoinGamePressed?.Invoke(Convert.ToInt32(TimeTextBox.Text.Trim()));
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
            if (e.KeyChar == (char)Keys.Return)
            {
                EnterPressedInWordTextBox?.Invoke(WordTextBox.Text.Trim());
                WordTextBox.Text = "";
                e.Handled = true;
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

        public void SetPlayer1NameLabel(string name)
        {
            Player1NameLabel.Text = name;
        }

        public void SetPlayer2NameLabel(string name)
        {
            Player2NameLabel.Text = name;
        }

        public void SetSecondsLabel(string seconds)
        {
            SecondsLabel.Text = seconds;
        }

        public void SetPlayer1Score(string score)
        {
            Player1ScoreLabel.Text = score;
        }

        public void SetPlayer2Score(string score)
        {
            Player2ScoreLabel.Text = score;
        }

       
    }
}
