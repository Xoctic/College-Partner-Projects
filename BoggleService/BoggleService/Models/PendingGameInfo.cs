using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoggleService.Models
{
    /// <summary>
    /// Class to store the Pending Game Info when a Join Game request executes.
    /// </summary>
    public class PendingGameInfo
    {
        /// <summary>
        /// Stores the time limit being passed into the server.
        /// </summary>
        public int timeLimit { get; set; }

        /// <summary>
        /// Stores the game ID created when Join Game request is called.
        /// </summary>
        public string gameID { get; set; }

        /// <summary>
        /// Stores whether or not a game is pending.
        /// </summary>
        public bool isPending { get; set; }
    }
}