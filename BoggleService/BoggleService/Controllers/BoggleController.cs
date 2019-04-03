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
                    users.Add(userID, user);
                    return userID;
                }
            }
        }

        /// <summary>
        /// Joins a game.
        /// If user is null or is empty after trimming, responds with status code Forbidden.
        /// Otherwise, creates a user, returns the user's token, and responds with status code Ok. 
        /// </summary>
        /// <param name="joinGameInfo">User to be added to users list</param>
        /// <returns>ID number of newly added user</returns>
        [Route("BoggleService/JoinGame")]
        public PendingOutput PostJoinGame(JoinGameInfo joinGameInfo)
        {
            lock (sync)
            {
                if (joinGameInfo.userToken == null || joinGameInfo.userToken.Trim().Length != 36 || !(users.ContainsKey(joinGameInfo.userToken)))
                {
                    throw new HttpResponseException(HttpStatusCode.Forbidden);
                }
                else if(joinGameInfo.timeLimit < 5 || joinGameInfo.timeLimit > 120)
                {
                    throw new HttpResponseException(HttpStatusCode.Forbidden);
                }
                else if(pendingInfo.userToken == joinGameInfo.userToken)
                {
                    throw new HttpResponseException(HttpStatusCode.Conflict);
                }
                else
                {
                    PendingOutput output = new PendingOutput();
                    if(pendingInfo.isPending == false)
                    {
                        gameID++;
                        pendingInfo.gameID = gameID;
                        pendingInfo.timeLimit = joinGameInfo.timeLimit;
                        pendingInfo.userToken = joinGameInfo.userToken;
                        //games.Add(gameID, );
                        pendingInfo.isPending = true;

                        output.isPending = true;
                        output.gameID = gameID;
                        return output;
                    }
                    else
                    {
                        output.isPending = false;
                        output.gameID = pendingInfo.gameID;
                        pendingInfo = new PendingGameInfo();
                        return output;
                    }
                }
            }
            return "hi";
        }

        /// <summary>
        /// cancels a pending game
        /// </summary>
        /// <param name="token"></param>
        [Route("BoggleService/CancelJoinGame")]
        public void PutCancelJoin(string token)
        {
            if(token == null || token.Length != 36)
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }
            if(pendingInfo.userToken == null || pendingInfo.userToken != token)
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }

            pendingInfo = new PendingGameInfo();


        }

        /// <summary>
        /// attempts to play a word
        /// </summary>
        /// <param name="gameID"></param>
        /// <param name="token"></param>
        /// <param name="word"></param>
        [Route("BoggleService/PlayWord/{gameID}")]
        public void PutPlayWord(int gameID, string token, string word)
        {
            int score = 0;

            if(word == null || word == "" || word.Trim().Length > 30 || !validToken(token) || !validID(gameID))
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }
            GameInfo temp = games[gameID];
            if(temp.player1Token != token && temp.player2Token != token)
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }
            if(temp.gameState != "active")
            {
                throw new HttpResponseException(HttpStatusCode.Conflict);
            }
            word = word.Trim();

            if(temp.player1Token == token)
            {
                
            }
            else
            {

            }
            

        }

        /// <summary>
        /// ensures that the userToken passed in is a valid token
        /// </summary>
        /// <param name="userToken"></param>
        /// <returns></returns>
        public bool validToken(string userToken)
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
        public bool validID(int gID)
        {
            if(gID == null || !games.ContainsKey(gID))
            {
                return false;
            }
            return true;
        }

        



        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }


        private static Dictionary<String, String> users = new Dictionary<String, String>();
        private static Dictionary<int, GameInfo> games = new Dictionary<int, GameInfo>();
        private static PendingGameInfo pendingInfo = new PendingGameInfo();
        private static int gameID = 0;
        private static readonly object sync = new object();
    }
}