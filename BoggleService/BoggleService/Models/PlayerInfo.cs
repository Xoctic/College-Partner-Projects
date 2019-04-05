using System;
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
        public string Nickname { get; set; }

        /// <summary>
        /// Stores the current score of the player.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public int Score { get; set; }

        /// <summary>
        /// List that stores each word that is played by the player.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public List<PlayedWord> WordsPlayed;

        [DataMember(EmitDefaultValue = false)]
        public Dictionary<string, int> wordsPlayedDictionary;
    }
}