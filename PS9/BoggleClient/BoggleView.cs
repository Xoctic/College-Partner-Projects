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
        public bool RegistrationComplete { get; set; }

        public event Action<string, string> RegisterPressed;

        public event Action<int> JoinGamePressed;

        public event Action QuitGamePressed;

        public event Action CancelJoinGamePressed;

        public event Action<string> EnterPressedInWordTextBox;

        public event Action CancelRegisterPressed;

        public event Action HelpMenuPressed;

        /// <summary>
        /// Creates a new window 
        /// </summary>
        public BoggleView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// allows all buttons to be pressed
        /// </summary>
        /// <param name="state"></param>
        public void EnableControls(bool state)
        {
            RegisterUserButton.Enabled = state && NameTextBox.Text.Length > 0 && ServerTextBox.Text.Length > 0;
            JoinGameButton.Enabled = state && IsUserRegistered && TimeTextBox.Text.Length > 0;
            QuitGameButton.Enabled = state && IsUserRegistered;

            ServerTextBox.Enabled = state;
            NameTextBox.Enabled = state;
            TimeTextBox.Enabled = state;
            WordTextBox.Enabled = state;

            foreach (Control control in BogleGrid.Controls)
            {
                if (control is Button)
                {
                    control.Enabled = state && IsUserRegistered;
                }
            }
            if(RegistrationComplete == false)
            {
                CancelRegisterUser.Enabled = !state;
            }
            if(RegistrationComplete == true)
            {
                CancelJoinGameButton.Enabled = !state;
            }
        }

        /// <summary>
        /// restricts the ability to press buttons
        /// </summary>
        /// <param name="state"></param>
        public void DisableControls(bool state)
        {
            RegisterUserButton.Enabled = state && NameTextBox.Text.Length > 0 && ServerTextBox.Text.Length > 0;
            JoinGameButton.Enabled = state && IsUserRegistered && TimeTextBox.Text.Length > 0;

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
        }

        /// <summary>
        /// sets the cells of the boggle board to the letters of the board passed in from the server
        /// </summary>
        /// <param name="board"></param>
        public void SetBoard(string board)
        {
            Console.WriteLine(board);
            //int counter = 0;
            char[] array = board.ToCharArray();


            for(int i = 1; i < 17; i++)
            {
                switch (i)
                {
                    case 1:
                        cell1.Text = array[i - 1].ToString();
                        break;

                    case 2:
                        cell2.Text = array[i - 1].ToString();
                        break;

                    case 3:
                        cell3.Text = array[i - 1].ToString();
                        break;

                    case 4:
                        cell4.Text = array[i - 1].ToString();
                        break;

                    case 5:
                        cell5.Text = array[i - 1].ToString();
                        break;

                    case 6:
                        cell6.Text = array[i - 1].ToString();
                        break;

                    case 7:
                        cell7.Text = array[i - 1].ToString();
                        break;

                    case 8:
                        cell8.Text = array[i - 1].ToString();
                        break;

                    case 9:
                        cell9.Text = array[i - 1].ToString();
                        break;

                    case 10:
                        cell10.Text = array[i - 1].ToString();
                        break;

                    case 11:
                        cell11.Text = array[i - 1].ToString();
                        break;

                    case 12:
                        cell12.Text = array[i - 1].ToString();
                        break;

                    case 13:
                        cell13.Text = array[i - 1].ToString();
                        break;

                    case 14:
                        cell14.Text = array[i - 1].ToString();
                        break;

                    case 15:
                        cell15.Text = array[i - 1].ToString();
                        break;

                    case 16:
                        cell16.Text = array[i - 1].ToString();
                        break;

                }
                
            }

        }

        /// <summary>
        /// handles when the register button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RegisterUserButton_Click(object sender, EventArgs e)
        {
            CancelRegisterUser.Visible = true;
            CancelRegisterUser.Enabled = true;
            RegisterPressed?.Invoke(NameTextBox.Text.Trim(), ServerTextBox.Text.Trim());
        }

        /// <summary>
        /// handles when the join game button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void JoinGameButton_Click(object sender, EventArgs e)
        {
            CancelJoinGameButton.Visible = true;
            CancelJoinGameButton.Enabled = true;
            JoinGamePressed?.Invoke(Convert.ToInt32(TimeTextBox.Text.Trim()));
        }

        /// <summary>
        /// Handles when the quitGame bUTTONE is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QuitGameButton_Click(object sender, EventArgs e)
        {
            QuitGamePressed?.Invoke();
        }

        /// <summary>
        /// Handles when the cancel button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton_Click(object sender, EventArgs e)
        {
            CancelJoinGamePressed?.Invoke();
            CancelJoinGameButton.Visible = false;
        }

        /// <summary>
        /// Handles when the wordTextBox is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WordTextBox_EnterPressed(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                EnterPressedInWordTextBox?.Invoke(WordTextBox.Text.Trim());
                WordTextBox.Text = "";
                e.Handled = true;
            }
        }

        /// <summary>
        /// Handles when the registration textBox is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Registration_TextChanged(object sender, EventArgs e)
        {
            EnableControls(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimeLimit_TextChanged(object sender, EventArgs e)
        {
            EnableControls(true);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            BogleGrid.Controls.Clear();
        }

        /// <summary>
        /// sets the name of player1
        /// </summary>
        /// <param name="name"></param>
        public void SetPlayer1NameLabel(string name)
        {
            Player1NameLabel.Text = name;
        }

        /// <summary>
        /// sets the name of player 2
        /// </summary>
        /// <param name="name"></param>
        public void SetPlayer2NameLabel(string name)
        {
            Player2NameLabel.Text = name;
        }

        /// <summary>
        /// displays the remaining seconds
        /// </summary>
        /// <param name="seconds"></param>
        public void SetSecondsLabel(string seconds)
        {
            SecondsLabel.Text = seconds;
        }

        /// <summary>
        /// displays player 1's current score
        /// </summary>
        /// <param name="score"></param>
        public void SetPlayer1Score(string score)
        {
            Player1ScoreLabel.Text = score;
        }

        /// <summary>
        /// displays player 2's current score
        /// </summary>
        /// <param name="score"></param>
        public void SetPlayer2Score(string score)
        {
            Player2ScoreLabel.Text = score;
        }

        /// <summary>
        /// sets the name of the player 
        /// </summary>
        /// <param name="name"></param>
        public void SetNameTextBox(string name)
        {
            NameTextBox.Text = name;
        }

        /// <summary>
        /// sets the name of the server
        /// </summary>
        /// <param name="server"></param>
        public void SetServerTextBox(string server)
        {
            ServerTextBox.Text = server;
        }


        /// <summary>
        /// sets the time limit
        /// </summary>
        /// <param name="time"></param>
        public void SetTimeTextBox(string time)
        {
            TimeTextBox.Text = time;
        }

        /// <summary>
        /// displays the words played by both players to the window
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        public void SetWordsPlayed(List<string> p1, List<string> p2)
        {
            string p1Words = "";
            string p2Words = "";

            foreach (string el in p1)
            {
                p1Words += "\n" + el;
            }

            foreach (string el in p2)
            {
                p2Words += "\n" + el;
            }

            Player1WordsPlayed.Text = p1Words;
            Player2WordsPlayed.Text = p2Words;
        }
    
        /// <summary>
        /// handles when the cancel register button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelRegisterUser_Click(object sender, EventArgs e)
        {
            CancelRegisterPressed?.Invoke();
            CancelRegisterUser.Visible = false;
        }

        /// <summary>
        /// handles when the help menu is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HelpMenuPressed?.Invoke();
        }
    }
}
