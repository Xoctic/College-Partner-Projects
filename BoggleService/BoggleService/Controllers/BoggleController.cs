using BoggleService.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Threading;
using System.Web.Http;

namespace BoggleService.Controllers
{
    /// <summary>
    /// Server controller that deals with a multitude of commands.
    /// </summary>
    public class BoggleController : ApiController
    {
        //Connection string to the database
        private static string DB;


        
        static BoggleController()
        {
            // Saves the connection string for the database.  A connection string contains the
            // information necessary to connect with the database server.  When you create a
            // DB, there is generally a way to obtain the connection string.  From the Server
            // Explorer pane, obtain the properties of DB to see the connection string.

            // The connection string of my BoggleDB.mdf shows as
            //
            //    Data Source = Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename="C:\Users\Aric Campbell\Source\Repos\u1188031\BoggleService\BoggleDB\App_Data\BoggleDB.mdf";Integrated Security=True
            //
            // Unfortunately, this is absolute pathname on my computer, which means that it
            // won't work if the solution is moved.  Fortunately, it can be shorted to
            //
            //    Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\ BoggleDB.mdf;Integrated Security=True
            //
            // You should shorten yours this way as well.
            //
            // Rather than build the connection string into the program, I store it in the Web.config
            // file where it can be easily found and changed.  You should do that too.
            DB = ConfigurationManager.ConnectionStrings["BoggleDB"].ConnectionString;
        }


        /// <summary>
        /// Registers a new user with a provided string Nickname.
        /// If user is null or is empty after trimming, responds with status code Forbidden.
        /// Otherwise, creates a user, returns the user's token, and responds with status code Ok. 
        /// </summary>
        /// <param name="user">String sser nickname to be added to users</param>
        /// <returns>A unique string user token of the newly added user</returns>
        [Route("BoggleService/users")]
        public string PostRegister([FromBody]string user)
        {
            if (user == "stall")
            {
                
                Thread.Sleep(5000);
            }
            lock (sync)
            {
                if (user == null || user.Trim().Length == 0 || user.Trim().Length > 50)
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
        /// Attempts to join a game, depending on if a game is pending or not.
        /// If UserToken is null, not of length 36, or not a token in users, responds with status code Forbidden.
        /// If Timelimit is less than 5 or greater than 120, responds with status code Forbidden.
        /// If UserToken is already in a pending game,  responds with status code Conflict.
        /// Otherwise, attempts to join a pending game, returns the gameID and game pending, and responds with status code Ok. 
        /// </summary>
        /// <param name="joinGameInput">Input object from the client which contains the user token and the time limit</param>
        /// <returns>A pending game info object which contains the game ID and a pending game status</returns>
        [Route("BoggleService/games")]
        public PendingGameInfo PostJoinGame(JoinGameInput joinGameInput)
        {
            lock (sync)
            {
                
                if (!validToken(joinGameInput.userToken))
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
                        string letterG = "G";
                        gameId = letterG + gameIDnum;
                        pendingInfo = new PendingGameInfo();
                        pendingInfo.GameID = gameId;
                        pendingInfo.TimeLimit = joinGameInput.timeLimit;
                        pendingInfo.UserToken = joinGameInput.userToken;
                        pendingInfo.IsPending = true;

                        GameInfo gameInfo = new GameInfo();
                        gameInfo.GameState = "pending";
                        gameInfo.TimeLimit = joinGameInput.timeLimit;
                        gameInfo.Player1 = new PlayerInfo();
                        gameInfo.Player1.PlayerToken = joinGameInput.userToken;
                        gameInfo.Player1.Nickname = users[joinGameInput.userToken];
                        games.Add(gameId, gameInfo);
                        
                        output.IsPending = true;
                        output.GameID = gameId;
                        return output;
                    }
                    else
                    {
                        GameInfo temp = games[gameId];
                        temp.MisterBoggle = new BoggleBoard();
                        temp.Board = temp.MisterBoggle.ToString();
                        temp.Player2 = new PlayerInfo();
                        temp.Player2.PlayerToken = joinGameInput.userToken;
                        temp.Player2.Nickname = users[joinGameInput.userToken];
                        temp.Player1.WordsPlayed = new List<PlayedWord>();
                        temp.Player2.WordsPlayed = new List<PlayedWord>();
                        temp.GameState = "active";
                        temp.startTime = (DateTime.Now.Minute * 60) + DateTime.Now.Second;

                        //Averages both players time limits
                        temp.TimeLimit = (games[gameId].TimeLimit + joinGameInput.timeLimit) / 2;

                        //pendingInfo.IsPending = false;
                        output.IsPending = false;
                        output.GameID = pendingInfo.GameID;
                        pendingInfo = new PendingGameInfo();
                        return output;
                    }
                }
            }
        }

        /// <summary>
        /// Attempts to cancel a pending game.
        /// If UserToken is null, not of length 36, or not a token in users, responds with status code Forbidden.
        /// Otherwise, remoes user token from the pending game and responds witht status code 204(NoContent).
        /// </summary>
        /// <param name="token">String user token</param>
        [Route("BoggleService/games")]
        public void PutCancelJoin([FromBody]string token)
        {
            lock (sync)
            {
                if (!(validToken(token)))
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
        /// Plays a word and returns its score depending if it is valid or not.
        /// If UserToken is null, not of length 36, or not a token in users, responds with status code Forbidden.
        /// If GameID is null, or not found in games, responds with status code Forbidden.
        /// If the string word input is null or empty, responds with status code Forbidden.
        /// If game stats is anything other than "active", responds with status code Conflict.
        /// Otherwise, attempts to join a pending game, returns the , and responds with status code Ok.
        /// </summary>
        /// <param name="gameID">Input string of a game ID</param>
        /// <param name="play">Input object which contains the user token and the word played</param>
        /// /// <returns>Score of string word input</returns>
        [Route("BoggleService/games/{gameID}")]
        public int PutPlayWord(string gameID, PlayWordInput play)
        {
            lock (sync)
            {
                int score = 0;

                if (play.word == null || play.word == "" || play.word.Trim().Length > 30 || !validToken(play.userToken) || !validID(gameID))
                {
                    throw new HttpResponseException(HttpStatusCode.Forbidden);
                }
                games[gameID].calculateTimeLeft();
                GameInfo temp = games[gameID];

                if (temp.Player1.PlayerToken != play.userToken && temp.Player2.PlayerToken != play.userToken)
                {
                    throw new HttpResponseException(HttpStatusCode.Forbidden);
                }
                if (temp.GameState == "completed" || temp.GameState == "pending" || temp.TimeLeft <= 0)
                {
                    throw new HttpResponseException(HttpStatusCode.Conflict);
                }
                play.word = play.word.Trim();
                PlayedWord playedWord = new PlayedWord(play.word);
                if (temp.Player1.PlayerToken == play.userToken)
                {
                    int index = temp.Player1.WordsPlayed.FindIndex(f => f.Word == play.word);

                    if (index >= 0)
                    {
                        playedWord.Score = 0;
                        score = 0;
                        temp.Player1.WordsPlayed.Add(playedWord);
                    }
                    else
                    {
                        score = temp.MisterBoggle.score(play.word);
                        playedWord.Score = score;
                        temp.Player1.WordsPlayed.Add(playedWord);
                    }
                    temp.Player1.Score += score;
                    games[gameID] = temp;
                }
                else
                {
                    int index = temp.Player2.WordsPlayed.FindIndex(f => f.Word == play.word);
                    if (index >= 0)
                    {
                        playedWord.Score = 0;
                        score = 0;
                        temp.Player2.WordsPlayed.Add(playedWord);
                    }
                    else
                    {
                        score = temp.MisterBoggle.score(play.word);
                        playedWord.Score = score;
                        temp.Player2.WordsPlayed.Add(playedWord);
                    }
                    temp.Player2.Score += score;
                    games[gameID] = temp;
                }
                return score;
            } 
        }

        /// <summary>
        /// Attempts to join a game, depending on if a game is pending or not.
        /// Otherwise, returns information about the game name by gameID.
        /// Note that the information returned depends on whether brief is true or false. Responds with status code Ok. 
        /// </summary>
        /// <param name="gameID">Input string of a game ID</param>
        /// <param name="brief">Input boolean ,true for minimal information, false for maximum information</param>
        /// <returns>A GameInfo object which contains information depending on brief</returns>
        [Route("BoggleService/games/{gameID}/{brief}")]
        public GameInfo GetGameStatus(string gameID, bool brief)
        {
            lock (sync)
            {
                if (!validID(gameID))
                {
                    throw new HttpResponseException(HttpStatusCode.Forbidden);
                }
                if(games[gameID].GameState == "active")
                {
                    games[gameID].calculateTimeLeft();
                    if(games[gameID].TimeLeft <= 0)
                    {
                        games[gameID].GameState = "completed";
                    }
                }
                GameInfo output = new GameInfo();
                GameInfo temp = games[gameID];

                if (brief == true)
                {
                    if(temp.GameState == "pending")
                    {
                        output.GameState = "pending";
                        return output;
                    }
                    if(temp.GameState == "active")
                    {
                        output.GameState = "active";
                        output.TimeLeft = temp.TimeLeft;
                        output.Player1 = new PlayerInfo();
                        output.Player1.Score = temp.Player1.Score;
                        output.Player2 = new PlayerInfo();
                        output.Player2.Score = temp.Player2.Score;
                        return output;
                    }
                    if(temp.GameState == "completed")
                    {
                        output.GameState = "completed";
                        output.Player1 = new PlayerInfo();
                        output.Player1.Score = temp.Player1.Score;
                        output.Player2 = new PlayerInfo();
                        output.Player2.Score = temp.Player2.Score;
                        return output;
                    }
                }
                else
                {
                    if (temp.GameState == "pending")
                    {
                        output.GameState = "pending";
                        return output;
                    }
                    if(temp.GameState == "active")
                    {
                        output.GameState = "active";
                        output.Board = temp.Board;
                        output.TimeLimit = temp.TimeLimit;
                        output.TimeLeft = temp.TimeLeft;
                        output.Player1 = new PlayerInfo();
                        output.Player1.Nickname = temp.Player1.Nickname;
                        output.Player1.Score = temp.Player1.Score;
                        output.Player2 = new PlayerInfo();
                        output.Player2.Nickname = temp.Player2.Nickname;
                        output.Player2.Score = temp.Player2.Score;
                        return output;
                    }
                    if(temp.GameState == "completed")
                    {
                        output.GameState = "completed";
                        output.Board = temp.Board;
                        output.TimeLimit = temp.TimeLimit;
                        output.Player1 = new PlayerInfo();
                        output.Player1.Nickname = temp.Player1.Nickname;
                        output.Player1.Score = temp.Player1.Score;
                        output.Player1.WordsPlayed = temp.Player1.WordsPlayed;
                        output.Player2 = new PlayerInfo();
                        output.Player2.Nickname = temp.Player2.Nickname;
                        output.Player2.Score = temp.Player2.Score;
                        output.Player2.WordsPlayed = temp.Player2.WordsPlayed;

                        return output;
                    }
                }
                //We should never to this line of code.
                return output;
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
            if(gID == null || gID.Trim().Length == 0 || !games.ContainsKey(gID))
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
        /// The initial ID of the first game.
        /// </summary>
        private static string gameId = "G";

        /// <summary>
        /// Game ID counter, which is incremented each time a new game is created.
        /// </summary>
        private static int gameIDnum = 0;

        /// <summary>
        /// A sync lock used to make sure that at any time only 1 method can be running at a time during multi threading.
        /// </summary>
        private static readonly object sync = new object();       
    }
}