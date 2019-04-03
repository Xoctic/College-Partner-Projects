﻿using BoggleService.Models;
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
        /// If UserToken is null or is not of length 36, responds with status code Forbidden.
        /// If Timelimit is less than 5 or greateer than 120, responds with status code Forbidden.
        /// If UserToken is already in a pending game, responds with status code Conflict.
        /// Otherwise, attempts to join a pending game, returns the , and responds with status code Ok. 
        /// </summary>
        /// <param name="joinGameInfo">User to be added to users list</param>
        /// <returns>ID number of newly added user</returns>
        [Route("BoggleService/JoinGame")]
        public PendingOutput PostJoinGame(JoinGameInfo joinGameInfo)
        {
            lock (sync)
            {
                //|| !(users.ContainsKey(joinGameInfo.userToken))
                if (joinGameInfo.userToken == null || joinGameInfo.userToken.Trim().Length != 36)
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
        }

        [Route("BoggleService/CancelJoinGame")]
        public void PutCancelJoin(string token)
        {
            if(token == null || token.Length != 36)
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }


        }


        [Route("BoggleService/PlayWord/{gameID}")]
        public void PutPlayWord(string gameID, string token, string word)
        {
            if(word == null || word == "" || word.Trim().Length > 30)
            {

            }
        }


        public bool validToken(string userToken)
        {
            return true;
        }

        public bool validID(string gID)
        {

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