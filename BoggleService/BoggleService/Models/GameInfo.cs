using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoggleService.Models
{
    public class GameInfo
    {
        private int startTime;
        private int timeLimit;
        public string player1Token;
        public string player2Token;
        public string gameState;

        //Constructor for a new game
        public GameInfo(int _timeLimit, string _player1Token, string _player2Token)
        {
            startTime = DateTime.Now.Second;
            timeLimit = _timeLimit;
            player1Token = _player1Token;
            player2Token = _player2Token;
            gameState = "active";
        }



        public Dictionary<string, int> player1Words = new Dictionary<string, int>();
        public Dictionary<string, int> player2Words = new Dictionary<string, int>();

    }
}