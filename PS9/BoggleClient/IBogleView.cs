using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BoggleClient
{
    public interface IBogleView
    {
        /// <summary>
        /// Fires when enter is pressed when inside the WordTextBox.
        /// </summary>
        event Action<string> EnterPressedInWordTextBox;

        /// <summary>
        /// If state == true, enables all controls that are normally enabled; disables Cancel.
        /// If state == false, disables all controls; enables Cancel.
        /// </summary>
        void EnableControls(bool state);

        /// <summary>
        /// Resets the game board.
        /// </summary>
        void Clear();

        /// <summary>
        /// Updates the time and the current score for each player.
        /// </summary>
        void Update();

        /// <summary>
        /// Is the user currently registered?
        /// </summary>
        bool IsUserRegistered { get; set; }

        /// <summary>
        /// Fired when user must be registered.
        /// Parameters are name and email.
        /// </summary>
        event Action<string, string> RegisterPressed;

        /// <summary>
        /// Fired when user selects join game button. Initiates search for a second player.
        /// </summary>
        event Action<int> JoinGamePressed;

        /// <summary>
        /// Fired when a user selects the quit game button.  Clears the game board and resets the timer.
        /// </summary>
        event Action QuitGamePressed;

        /// <summary>
        /// Fires when the cancel join game button is pressed.
        /// </summary>
        event Action CancelJoinGamePressed;

        /// <summary>
        /// Fires when the cancel register button is pressed.
        /// </summary>
        event Action CancelRegisterPressed;
    }
}
