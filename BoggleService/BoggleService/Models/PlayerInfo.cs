﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BoggleService.Models
{
    /// <summary>
    /// Class to store the information of each player.
    /// </summary>
    [DataContract]
    public class PlayerInfo
    {
        /// <summary>
        /// Stores the token of this player.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string PlayerToken { get; set; }

        /// <summary>
        /// Stores the nickName of the player.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string NickName { get; set; }

        /// <summary>
        /// Stores the current score of the player.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public int Score { get; set; }

        /// <summary>
        /// Stores the words played by the player.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public PlayerWordsPlayed WordsPlayed;
    }
}