using BoggleService;
using BoggleService.Controllers;
using BoggleService.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace BoggleServiceLocalTests
{
    [TestClass]
    public class BoggleServiceLocalTests
    {
        [TestMethod]
        public void PostRegisterTest()
        {
            BoggleController controller = new BoggleController();

            string result = controller.PostRegister("Billy");

            Assert.IsNotNull(result);
            Assert.AreEqual(36, result.Length);

            try
            {
                controller.PostRegister("");
                Assert.Fail("Exception not found");
            }
            catch(HttpResponseException e)
            {
                Assert.AreEqual(HttpStatusCode.Forbidden, e.Response.StatusCode);
            }

            try
            {
                controller.PostRegister(null);
                Assert.Fail("Exception not found");

            }
            catch(HttpResponseException e)
            {
                Assert.AreEqual(HttpStatusCode.Forbidden, e.Response.StatusCode);

            }
            
        }


        [TestMethod]
        public void PostJoinGameTest()
        {
            BoggleController controller = new BoggleController();
            string result = controller.PostRegister("Billy");
            JoinGameInput joinGame = new JoinGameInput(80, "");

            //testing when userToken is invalid
            try
            {
                controller.PostJoinGame(joinGame);
            }
            catch(HttpResponseException e)
            {
                Assert.AreEqual(HttpStatusCode.Forbidden, e.Response.StatusCode);
            }

            //testing when timeLimit < 5
            joinGame.userToken = Guid.NewGuid().ToString();
            joinGame.timeLimit = 0;
            try
            {
                controller.PostJoinGame(joinGame);
            }
            catch(HttpResponseException e)
            {
                Assert.AreEqual(HttpStatusCode.Forbidden, e.Response.StatusCode);
            }


            //testing when timeLimis > 120
            joinGame.userToken = Guid.NewGuid().ToString();
            joinGame.timeLimit = 121;

            try
            {
                controller.PostJoinGame(joinGame);
            }
            catch(HttpResponseException e)
            {
                Assert.AreEqual(HttpStatusCode.Forbidden, e.Response.StatusCode);
            }

        }

        /// <summary>
        /// Testing joining a game with the same name
        /// </summary>
        [TestMethod]
        public void PostJoinGameTest2()
        {
            BoggleController controller = new BoggleController();
            JoinGameInput joinGame = new JoinGameInput(10, controller.PostRegister("Billy"));
            
            controller.PostJoinGame(joinGame);
            
            

            try
            {
                controller.PostJoinGame(joinGame);
            }
            catch(HttpResponseException e)
            {
                Assert.AreEqual(HttpStatusCode.Conflict, e.Response.StatusCode);
            }
        }

        /// <summary>
        /// Tests when usertoken passed in is invalid (null or not 36 characters long)
        /// </summary>
        [TestMethod]
        public void CancelJoinRequestTest()
        {
            BoggleController controller = new BoggleController();
            JoinGameInput joinGame = new JoinGameInput(10, controller.PostRegister("Billy"));

            

            controller.PostJoinGame(joinGame);

            try
            {
                controller.PutCancelJoin(null);
            }
            catch(HttpResponseException e)
            {
                Assert.AreEqual(HttpStatusCode.Forbidden, e.Response.StatusCode);
            }

            try
            {
                controller.PutCancelJoin("");
            }
            catch(HttpResponseException e)
            {
                Assert.AreEqual(HttpStatusCode.Forbidden, e.Response.StatusCode);
            }

        }

        [TestMethod]
        public void PlayWordTest1()
        {
            BoggleController controller = new BoggleController();
            controller.testFlag = true;
            controller.testScore = 11;
            JoinGameInput joinGame;
            string player1 = controller.PostRegister("Billy");
            string player2 = controller.PostRegister("Mr. Bean");


            joinGame = new JoinGameInput(10, player1);
            controller.PostJoinGame(joinGame);
            joinGame.userToken = player2;
            controller.PostJoinGame(joinGame);

            PlayWordInput input = new PlayWordInput(player1, "ABANDONMENTS");

            int score = controller.PutPlayWord("G1", input);

            Assert.IsTrue(score == 11);


            



           // string gId = controller.getGameId();

            



        }


        [TestMethod]
        public void PlayWordTest2()
        {
            BoggleController controller = new BoggleController();
            controller.testFlag = true;
            controller.testScore = 5;
            JoinGameInput joinGame;
            string player1 = controller.PostRegister("Billy");
            string player2 = controller.PostRegister("Mr. Bean");


            joinGame = new JoinGameInput(10, player1);
            controller.PostJoinGame(joinGame);
            joinGame.userToken = player2;
            controller.PostJoinGame(joinGame);

            PlayWordInput input = new PlayWordInput(player1, "PENNAME");

            int score = controller.PutPlayWord("G1", input);

            Assert.IsTrue(score == 5);
        }



        [TestMethod]
        public void PlayWordTest3()
        {
            BoggleController controller = new BoggleController();
            controller.testFlag = true;
            controller.testScore = 3;
            JoinGameInput joinGame;
            string player1 = controller.PostRegister("Billy");
            string player2 = controller.PostRegister("Mr. Bean");


            joinGame = new JoinGameInput(10, player1);
            controller.PostJoinGame(joinGame);
            joinGame.userToken = player2;
            controller.PostJoinGame(joinGame);

            PlayWordInput input = new PlayWordInput(player1, "PENNAE");

            int score = controller.PutPlayWord("G1", input);

            Assert.IsTrue(score == 3);
        }


        [TestMethod]
        public void PlayWordTest4()
        {
            BoggleController controller = new BoggleController();
            controller.testFlag = true;
            controller.testScore = 2;
            JoinGameInput joinGame;
            string player1 = controller.PostRegister("Billy");
            string player2 = controller.PostRegister("Mr. Bean");


            joinGame = new JoinGameInput(10, player1);
            controller.PostJoinGame(joinGame);
            joinGame.userToken = player2;
            controller.PostJoinGame(joinGame);

            PlayWordInput input = new PlayWordInput(player1, "PENNA");

            int score = controller.PutPlayWord("G1", input);

            Assert.IsTrue(score == 2);
        }


        [TestMethod]
        public void  PlayWordTest5()
        {
            BoggleController controller = new BoggleController();
            controller.testFlag = true;
            controller.testScore = 1;
            JoinGameInput joinGame;
            string player1 = controller.PostRegister("Billy");
            string player2 = controller.PostRegister("Mr. Bean");


            joinGame = new JoinGameInput(10, player1);
            controller.PostJoinGame(joinGame);
            joinGame.userToken = player2;
            controller.PostJoinGame(joinGame);

            PlayWordInput input = new PlayWordInput(player1, "PENS");

            int score = controller.PutPlayWord("G1", input);

            Assert.IsTrue(score == 1);

            score = controller.PutPlayWord("G1", input);

            Assert.IsTrue(score == 0);

        }


        [TestMethod]
        public void PlayWordTest6()
        {
            BoggleController controller = new BoggleController();
            controller.testFlag = true;
            controller.testScore = 0;
            JoinGameInput joinGame;
            string player1 = controller.PostRegister("Billy");
            string player2 = controller.PostRegister("Mr. Bean");


            joinGame = new JoinGameInput(10, player1);
            controller.PostJoinGame(joinGame);
            joinGame.userToken = player2;
            controller.PostJoinGame(joinGame);

            PlayWordInput input = new PlayWordInput(player1, "A");

            int score = controller.PutPlayWord("G1", input);

            Assert.IsTrue(score == 0);
        }


        [TestMethod]
        public void gameStatusTest()
        {
            BoggleController controller = new BoggleController();
            JoinGameInput joinGame;
            GameInfo info;
            string player1 = controller.PostRegister("Billy");
            string player2 = controller.PostRegister("Mr. Bean");

            joinGame = new JoinGameInput(10, player1);
            info = controller.GetGameStatus("G1", true);

            Assert.IsTrue(info.GameState == "pending");
        }

        



        


    }
}
