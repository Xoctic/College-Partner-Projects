using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoggleService.Models
{
    /// <summary>
    /// Class specifically designed to be set only for return value of PostJoinGame 
    /// </summary>
    public class JoinGameOutput
    {
        /// <summary>
        /// Stores the game ID created when Join Game request is called.
        /// </summary>
        public string GameID { get; set; }

        /// <summary>
        /// Stores whether or not a game is pending.
        /// </summary>
        public bool IsPending { get; set; }
    }
}