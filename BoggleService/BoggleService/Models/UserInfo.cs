using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoggleService.Models
{
    public class UserInfo
    {
        /// <summary>
        /// constructor for information about a user if user has not joined a game yet
        /// </summary>
        /// <param name="_name"></param>
        public UserInfo(string _name)
        {
            name = _name;
        }

        /// <summary>
        /// constructor for information about a user if user has joined a game
        /// _currentID is required for the following statement located in PlayWord in the API (finding a gameID with a userToken)
        /// "Otherwise, records the trimmed Word as being played by UserToken in the game identified by gameID"
        /// </summary>
        /// <param name="_name"></param>
        /// <param name="_currentID"></param>
        public UserInfo(string _name, int _currentID)
        {
            name = _name;
            currentGameID = _currentID;
        }

        public string name;
        public int currentGameID;
    }
}