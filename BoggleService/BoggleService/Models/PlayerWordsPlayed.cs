using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BoggleService.Models
{
    /// <summary>
    /// Class to store the words played by the current player.
    /// </summary>
    [DataContract]
    public class PlayerWordsPlayed
    {
        /// <summary>
        /// Stores the word played by the current player.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public Dictionary<string, int> playerWordsPlayed = new Dictionary<string, int>();
    }
}