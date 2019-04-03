using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoggleService.Models
{
    /// <summary>
    /// Class to store the Game Info when a Join Game request executes.
    /// </summary>
    public class JoinGameInfo
    {
        public JoinGameInfo(int _timeLimit, string _userToken)
        {
            timeLimit = _timeLimit;
            userToken = _userToken;
        }

        /// <summary>
        /// Stores the time limit being passed into the server.
        /// </summary>
        public int timeLimit { get; set; }

        /// <summary>
        /// Stores the user token being passed into the server.
        /// </summary>
        public string userToken { get; set; }
    }
}