using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Controller
{
    /// <summary>
    /// Class to store each word played by a player
    /// </summary>
    public class PlayedWord
    {
        /// <summary>
        /// Stores the word played.
        /// </summary>
        public string Word;

        /// <summary>
        /// Stores the score for the word played.
        /// </summary>
        public int Score;

        /// <summary>
        /// Constructor to set the Word.
        /// </summary>
        /// <param name="_Word"></param>
        //public PlayedWord(string _Word)
        //{
        //    Word = _Word;
        //}
    }
}