using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Express
{
    /// <summary>
    /// Class to store the Pending Game Info when a Join Game request executes.
    /// </summary>
    [DataContract]
    public class PendingGameInfo
    {
        /// <summary>
        /// Stores the time limit being passed into the server.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public int TimeLimit { get; set; }

        /// <summary>
        /// Stores the game ID created when Join Game request is called.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string GameID { get; set; }

        /// <summary>
        /// Stores the user ID that is awaiting a game.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string UserToken { get; set; }

        /// <summary>
        /// Stores whether or not a game is pending.
        /// </summary>
        [DataMember(EmitDefaultValue = true)]
        public bool IsPending { get; set; }
    }
}