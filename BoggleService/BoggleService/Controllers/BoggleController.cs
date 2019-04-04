using BoggleService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

namespace BoggleService.Controllers
{
    /// <summary>
    /// Server controller that deals with all commands.
    /// </summary>
    public class BoggleController : ApiController
    {

        /// <summary>
        /// Registers a new user.
        /// If user is null or is empty after trimming, responds with status code Forbidden.
        /// Otherwise, creates a user, returns the user's token, and responds with status code Ok. 
        /// </summary>
        /// <param name="user">User to be added to users list</param>
        /// <returns>ID number of newly added user</returns>
        [Route("BoggleService/RegisterUser")]
        public string PostRegister([FromBody]string user)
        {
            if (user == "stall")
            {
                
                Thread.Sleep(5000);
            }
            lock (sync)
            {
                if (user == null || user.Trim().Length == 0)
                {
                    throw new HttpResponseException(HttpStatusCode.Forbidden);
                }
                else
                {
                    string userID = Guid.NewGuid().ToString();
                    users.Add(userID, user.Trim());
                    return userID;
                }
            }
        }

        /// <summary>
        /// Joins a game.
        /// If UserToken is null or is not of length 36, responds with status code Forbidden.
        /// If Timelimit is less than 5 or greateer than 120, responds with status code Forbidden.
        /// If UserToken is already in a pending game, responds with status code Conflict.
        /// Otherwise, attempts to join a pending game, returns the , and responds with status code Ok. 
        /// </summary>
        /// <param name="joinGameInput">Input from the client which contains the user token and the time limit.</param>
        /// <returns>A pending game info object which contains the game ID and the status of isPending</returns>
        [Route("BoggleService/JoinGame")]
        public PendingGameInfo PostJoinGame(JoinGameInput joinGameInput)
        {
            lock (sync)
            {
                //|| !(users.ContainsKey(joinGameInfo.userToken))
                if (joinGameInput.userToken == null || joinGameInput.userToken.Trim().Length != 36)
                {
                    throw new HttpResponseException(HttpStatusCode.Forbidden);
                }
                else if(joinGameInput.timeLimit < 5 || joinGameInput.timeLimit > 120)
                {
                    throw new HttpResponseException(HttpStatusCode.Forbidden);
                }
                else if(pendingInfo.UserToken == joinGameInput.userToken)
                {
                    throw new HttpResponseException(HttpStatusCode.Conflict);
                }
                else
                {
                    PendingGameInfo output = new PendingGameInfo();
                    if(pendingInfo.IsPending == false)
                    {
                        gameIDnum++;
                        gameID += gameIDnum;
                        pendingInfo.GameID = gameID;
                        pendingInfo.TimeLimit = joinGameInput.timeLimit;
                        pendingInfo.UserToken = joinGameInput.userToken;
                        pendingInfo.IsPending = true;

                        GameInfo gameInfo = new GameInfo(true);
                        gameInfo.GameState = "pending";
                        gameInfo.TimeLimit = joinGameInput.timeLimit;
                        gameInfo.Player1.PlayerToken = joinGameInput.userToken;
                        gameInfo.Player1.NickName = users[joinGameInput.userToken];
                        games.Add(gameID, gameInfo);
                        //users[pendingInfo.userToken].currentGameID = pendingInfo.gameID;
                        output.IsPending = true;
                        output.GameID = gameID;
                        return output;
                    }
                    else
                    {
                        games[gameID].Player2.PlayerToken = joinGameInput.userToken;
                        games[gameID].Player2.NickName = users[joinGameInput.userToken];
                        games[gameID].GameState = "active";
                        //Averages both players time limits
                        games[gameID].TimeLimit = (games[gameID].TimeLimit + joinGameInput.timeLimit) / 2;

                        output.IsPending = false;
                        output.GameID = pendingInfo.GameID;
                        pendingInfo = new PendingGameInfo();
                        return output;
                    }
                }
            }
        }

        /// <summary>
        /// cancels a pending game
        /// </summary>
        /// <param name="token"></param>
        [Route("BoggleService/CancelJoinGame")]
        public void PutCancelJoin(string token)
        {
            lock (sync)
            {
                if (token == null || token.Length != 36)
                {
                    throw new HttpResponseException(HttpStatusCode.Forbidden);
                }
                if (pendingInfo.UserToken == null || pendingInfo.UserToken != token)
                {
                    throw new HttpResponseException(HttpStatusCode.Forbidden);
                }
                //Removes the pending game from games, and then creates a new pending Game Info.
                games.Remove(pendingInfo.GameID);
                pendingInfo = new PendingGameInfo();
            }
        }

        /// <summary>
        /// attempts to play a word
        /// </summary>
        /// <param name="_gameID"></param>
        /// <param name="token"></param>
        /// <param name="word"></param>
        [Route("BoggleService/PlayWord/{_gameID}")]
        public int PutPlayWord(string _gameID, PlayWordInput play)
        {
            lock (sync)
            {
                int score = 0;

                if (play.word == null || play.word == "" || play.word.Trim().Length > 30 || !validToken(play.userToken) || !validID(_gameID))
                {
                    throw new HttpResponseException(HttpStatusCode.Forbidden);
                }

                GameInfo temp = games[_gameID];


                if (temp.Player1.PlayerToken != play.userToken && temp.Player2.PlayerToken != play.userToken)
                {
                    throw new HttpResponseException(HttpStatusCode.Forbidden);
                }
                if (temp.GameState != "active")
                {
                    throw new HttpResponseException(HttpStatusCode.Conflict);
                }

                play.word = play.word.Trim();


                if (temp.Player1.PlayerToken == play.userToken)
                {
                    if (temp.Player1.WordsPlayed.playerWordsPlayed.ContainsKey(play.word))
                    {
                        score = 0;
                    }
                    else
                    {
                        score = temp.MisterBoggle.score(play.word);
                        temp.Player1.WordsPlayed.playerWordsPlayed.Add(play.word, score);
                        games[_gameID] = temp;
                    }
                }
                else
                {
                    if (temp.Player2.WordsPlayed.playerWordsPlayed.ContainsKey(play.word))
                    {
                        score = 0;
                    }
                    else
                    {
                        score = temp.MisterBoggle.score(play.word);
                        temp.Player2.WordsPlayed.playerWordsPlayed.Add(play.word, score);
                        games[_gameID] = temp;
                    }
                }

                return score;
            }
        }

        /// <summary>
        /// Joins a game.
        /// If UserToken is null or is not of length 36, responds with status code Forbidden.
        /// If Timelimit is less than 5 or greateer than 120, responds with status code Forbidden.
        /// If UserToken is already in a pending game, responds with status code Conflict.
        /// Otherwise, attempts to join a pending game, returns the , and responds with status code Ok. 
        /// </summary>
        /// <param name="joinGameInput">User to be added to users list</param>
        /// <returns>ID number of newly added user</returns>
        [Route("BoggleService/JoinGame")]
        public PendingGameInfo GetGameStatus(JoinGameInput joinGameInput)
        {
            lock (sync)
            {
                if ()
                {
                    throw new HttpResponseException(HttpStatusCode.Forbidden);
                }
            }
        }

        /// <summary>
        /// ensures that the userToken passed in is a valid token
        /// </summary>
        /// <param name="userToken"></param>
        /// <returns></returns>
        private bool validToken(string userToken)
        {
            if(userToken == null || userToken.Trim().Length != 36 || !users.ContainsKey(userToken))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// esures the gameID passed in is a valid gameId
        /// </summary>
        /// <param name="gID"></param>
        /// <returns></returns>
        private bool validID(string gID)
        {
            if(gID == null || !games.ContainsKey(gID))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// A dictionary to store the key User Token which links to its value Nickname.
        /// </summary>
        private static Dictionary<String, String> users = new Dictionary<String, String>();

        /// <summary>
        /// A dictionary to store the string key gameID which links to its value GameInfo.
        /// </summary>
        private static Dictionary<String, GameInfo> games = new Dictionary<String, GameInfo>();

        /// <summary>
        /// The pending game info for the current pending game.
        /// </summary>
        private static PendingGameInfo pendingInfo = new PendingGameInfo();

        /// <summary>
        /// The initial ID of the first game.  Is incremented by 1 each time a new game is created in the PostJoinGame method.
        /// </summary>
        private static string gameID = "G";

        private static int gameIDnum = 0;

        /// <summary>
        /// A sync lock used to make sure that at any time only 1 method can be running at a time during multi threading.
        /// </summary>
        private static readonly object sync = new object();       
    }
}