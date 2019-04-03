using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoggleService.Models
{
    /// <summary>
    /// Response that is sent to the client containing the gameID and the bool isPending.
    /// </summary>
    public class PendingOutput
    {
        /// <summary>
        /// Stores the game ID created when Join Game request is called.
        /// </summary>
        public int gameID { get; set; }

        /// <summary>
        /// Stores whether or not a game is pending.
        /// </summary>
        public bool isPending { get; set; }
    }
}