using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoggleService.Models
{
    /// <summary>
    /// Class to store the Game Info when a Join Game request executes.
    /// </summary>
    public class JoinGameInput
    {
        /// <summary>
        /// Constructor to set the time limit and the user token.
        /// </summary>
        /// <param name="_timeLimit"></param>
        /// <param name="_userToken"></param>
        public JoinGameInput(int _timeLimit, string _userToken)
        {
            timeLimit = _timeLimit;
            userToken = _userToken;
        }

        /// <summary>
        /// Stores the user token being passed into the server.
        /// </summary>
        public string userToken { get; set; }

        /// <summary>
        /// Stores the time limit being passed into the server.
        /// </summary>
        public int timeLimit { get; set; }
    }
}