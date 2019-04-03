using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BoggleService.Models
{
    /// <summary>
    /// Class to store all of the game information needed for each individual game.
    /// </summary>
    [DataContract]
    public class GameInfo
    {
        /// <summary>
        /// Stores the state of the game.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string gameState { get; set; }

        /// <summary>
        /// Stores the board of this current game.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public BoggleBoard board;

        /// <summary>
        /// Stores the time limit of this game, which is the average time of both players.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public int timeLimit { get; set; }

        /// <summary>
        /// Stores the time remaining for this current game.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public int timeLeft { get; set; }

        /// <summary>
        /// Stores the player information for player 1.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public PlayerInfo player1;

        /// <summary>
        /// Stores the player information for player 2.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public PlayerInfo player2;
    }
}