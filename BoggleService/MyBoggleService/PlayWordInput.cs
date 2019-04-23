using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Controller
{
    /// <summary>
    /// Class that stores the input Info when a play word request executes.
    /// </summary>
    public class PlayWordInput
    {
        /// <summary>
        /// Constructor to set the userToken and word from the input.
        /// </summary>
        /// <param name="u"></param>
        /// <param name="w"></param>
        public PlayWordInput(string u, string w)
        {
            userToken = u;
            word = w;
        }

        /// <summary>
        /// Stores the token of the user.
        /// </summary>
        public string userToken { get; set; }

        /// <summary>
        /// Stores the word of the user.
        /// </summary>
        public string word { get; set; }
    }
}